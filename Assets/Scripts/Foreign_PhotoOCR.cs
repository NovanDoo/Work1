using Baidu.Aip.Ocr;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Foreign_PhotoOCR : MonoBehaviour
{
    public Text TxtCurrentTime;
    public static string resultwords = "";//结果字符串
    public TMP_Text showinfo;
    public string deviceName;//摄像机设备的名称
    public RawImage display;//显示的摄像机内容
    //接收返回的图片数据  
    WebCamTexture tex;
    //参数设置，设置APPID/AK/SK
    const string API_KEY = "uala0E6n2KDNddSghIwbKo5a";
    const string SECRET_KEY = "cG9OrX42wSpauga2ACU5jlwLLsjUINUi";
    static Ocr client;//Ocr类
    void Awake()
    {
        client = new Ocr(API_KEY, SECRET_KEY);// new一个文字识别Ocr
        client.Timeout = 60000;  //修改超时时间
    }
    public void CameraAccurateBasic(byte[] image)//相机的文字识别
    {
        //异常处理
        try
        {
            // 调取API识别图片文字
            var result = client.AccurateBasic(image);// 调取API识别图片文字
            var wordsResult = result["words_result"];//读取结果的array数组
            Debug.Log(result);
            foreach (var word in wordsResult)//遍历所有的内容
            {
                resultwords += word["words"].ToString();//加到输出string中
            }
            resultwords = Translate.translate(resultwords);
            showinfo.text = resultwords;
            //接下来再调用下面Datecut方法处理结果
            //保质期：这里只设计算法，需要配置按钮，比如if什么时候调用保质期算法
            string baozhiqi = Datecut(resultwords);
            showinfo.text = InfoCut(wordsResult) + "\n";
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
    public void getPhoto()
    {
        tex.Pause();//暂停摄像机
        StartCoroutine(getTexture());//获取当前的照片
        //tex.Stop();
    }
    public IEnumerator getTexture()//获取当前照片
    {
        yield return new WaitForEndOfFrame();
        Texture2D img = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, true);//新建一个照片类
        img.SetPixels(tex.GetPixels());
        img.Apply();
        byte[] imageTytes = img.EncodeToJPG();//转化为byte
        //File.WriteAllBytes(Application.dataPath + "/Pictures/" + "photo.jpg", imageTytes);
        CameraAccurateBasic(imageTytes);//调用OCR识别
    }
    //注意：：
    //以下部分是截取保质期和生产日期和计算时间差的代码传入参数都为字符串类型，需要设置交互界面，根据需要获取的内容设置相应的交互按钮
    public string Datecut(string result)//传入读取的字符串
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
    //获取生产日期
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