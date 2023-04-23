using Baidu.Aip.Ocr;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManufactureDate_Photo_Get : MonoBehaviour
{
    // Start is called before the first frame update
    public Text TxtCurrentTime;
    public static string resultwords = "";//结果字符串
    public TMP_Text showinfo;
    public string deviceName;//摄像机设备的名称
    public RawImage display;//显示的摄像机内容
    //接收返回的图片数据  
    //参数设置，设置APPID/AK/SK
    const string API_KEY = "uala0E6n2KDNddSghIwbKo5a";
    const string SECRET_KEY = "cG9OrX42wSpauga2ACU5jlwLLsjUINUi";
    static Ocr client;//Ocr类
    void Awake()
    {
        client = new Ocr(API_KEY, SECRET_KEY);// new一个文字识别Ocr
        client.Timeout = 60000;  //修改超时时间
    }
    public void getPhoto()
    {
        OpenCamera.tex.Pause();//暂停摄像机
        StartCoroutine(getTexture());//获取当前的照片
        //OpenCamera.tex.Stop();
    }
    public IEnumerator getTexture()//获取当前照片
    {
        yield return new WaitForEndOfFrame();
        Texture2D img = new Texture2D(1000, 1500);
        //Texture2D img = new Texture2D(OpenCamera.tex.width, OpenCamera.tex.height, TextureFormat.ARGB32, true);//新建一个照片类
        //img = OpenCamera.display.texture as Texture2D;
        img.ReadPixels(new Rect(Screen.width / 2 - 500, Screen.height / 2 + 50, 1000, 1500), 0, 0, false);
        //img.SetPixels(OpenCamera.tesx.GetPixels());
        img.Apply();
        byte[] imageTytes = img.EncodeToJPG();//转化为byte
        //File.WriteAllBytes(Application.dataPath + "/Pictures/" + "photo.jpg", imageTytes);
        CameraAccurateBasic(imageTytes);//调用OCR识别
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
            //生产日期：这里只设计算法，需要配置按钮来调用拍生产日期还是保质器，先处理大多数情况，保质期都是单独列出来然后位于第一行
            string shengchangdate = DateGet.Datecut2(resultwords);
            DateGet.ManufactureDate = shengchangdate;
            showinfo.text +="\n生产日期："+shengchangdate;
            showinfo.text += "\n还剩：" + DateGet.restdate();
        }
        catch (Exception e)
        {
            //打印异常信息
            Debug.Log("异常：" + e);
        }
    }
}
