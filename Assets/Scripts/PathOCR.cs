using Baidu.Aip.Ocr;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using System.Linq;
public class PathOCR : MonoBehaviour
{
    const string API_KEY = "uala0E6n2KDNddSghIwbKo5a";
    const string SECRET_KEY = "cG9OrX42wSpauga2ACU5jlwLLsjUINUi";
    public TMP_InputField path;//输入框
    public TMP_Text showinfo;//输出内容
    static Ocr client;//ocr的调用类
    // Start is called before the first frame update
    void Awake()
    {
        client = new Ocr(API_KEY, SECRET_KEY);// new一个文字识别Ocr
        client.Timeout = 60000;  //修改超时时间
    }
    public void buttonClick()//按钮点击开始
    {
        StartCoroutine(Ocr());//调用协程开始ocr
    }
    public IEnumerator Ocr()
    {
        yield return new WaitForEndOfFrame();
        string resultwords = "";//最终结果
        string imgpath = path.text;//文件路径
        var image = File.ReadAllBytes(imgpath);//读取字节
        //异常处理
        try
        {
            var result = client.AccurateBasic(image);// 调取API识别图片文字
            var wordsResult = result["words_result"];//读取结果的array数组
            Debug.Log(result);
            foreach (var word in wordsResult)//遍历所有的内容
            {
                resultwords += word["words"].ToString();//加到输出string中
            }
            Debug.Log(resultwords);
            //接下来再调用下面Datecut方法处理结果
            //保质期：这里只设计算法，需要配置按钮，比如if什么时候调用保质期算法
            string baozhiqi = Datecut(resultwords);
            showinfo.text = InfoCut(wordsResult)+"\n";
            showinfo.text += baozhiqi;
            if (showinfo.text.Equals("")) showinfo.text = resultwords;
            //生产日期：这里只设计算法，需要配置按钮来调用拍生产日期还是保质器，先处理大多数情况，保质期都是单独列出来然后位于第一行
            string shengchangdate = Datecut2(resultwords);
        }
        catch (Exception e)
        {
            //打印异常信息
            Debug.Log("异常：" + e);
        }
    }
    public string Datecut(string result)
    {
        string back = "";
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i] == '保')
            {
                if (result[i + 1] == '质')
                {
                    if (result[i + 2] == '期')
                    {
                        back += "保质期：";
                        //开始做寻找时间的处理
                        i += 3;
                        while (result[i] != '月' && result[i] != '年' && result[i] != '个')
                        {
                            if (result[i] >= '0' && result[i] <= '9')
                            {
                                back += result[i];
                            }
                            i++;
                        }
                        //年月加上
                        while (result[i] == '月' || result[i] == '年' || result[i] != '个')
                            back += result[i];
                        //如果检测到了月或年就跳出，此时result里面存储的就是数字
                        break;
                    }
                }
            }
        }
        return back;
    }
    public string Datecut2(string result)
    {
        string back = "";
        int count = 0;//生产日期最多存储8个数字
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i] >= '0' && result[i] <= '9' && count <= 8)
            {
                count++;
                back += result[i];
            }
        }
        return back;
    }
    public string InfoCut(JToken wordsResult)
    {
        foreach (var word in wordsResult)//遍历所有的内容
        {
            if (word.ToString().Contains("名称"))
            {
                return word["words"].ToString();
            };
        }
        return "";
    }
}
