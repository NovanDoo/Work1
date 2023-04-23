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
    public static string resultwords = "";//����ַ���
    public TMP_Text showinfo;
    public string deviceName;//������豸������
    public RawImage display;//��ʾ�����������
    //���շ��ص�ͼƬ����  
    WebCamTexture tex;
    //�������ã�����APPID/AK/SK
    const string API_KEY = "uala0E6n2KDNddSghIwbKo5a";
    const string SECRET_KEY = "cG9OrX42wSpauga2ACU5jlwLLsjUINUi";
    static Ocr client;//Ocr��
    void Awake()
    {
        client = new Ocr(API_KEY, SECRET_KEY);// newһ������ʶ��Ocr
        client.Timeout = 60000;  //�޸ĳ�ʱʱ��
    }
    public void CameraAccurateBasic(byte[] image)//���������ʶ��
    {
        //�쳣����
        try
        {
            // ��ȡAPIʶ��ͼƬ����
            var result = client.AccurateBasic(image);// ��ȡAPIʶ��ͼƬ����
            var wordsResult = result["words_result"];//��ȡ�����array����
            Debug.Log(result);
            foreach (var word in wordsResult)//�������е�����
            {
                resultwords += word["words"].ToString();//�ӵ����string��
            }
            resultwords = Translate.translate(resultwords);
            showinfo.text = resultwords;
            //�������ٵ�������Datecut����������
            //�����ڣ�����ֻ����㷨����Ҫ���ð�ť������ifʲôʱ����ñ������㷨
            string baozhiqi = Datecut(resultwords);
            showinfo.text = InfoCut(wordsResult) + "\n";
            showinfo.text += baozhiqi;
            if (showinfo.text.Equals("")) showinfo.text = resultwords;
            //�������ڣ�����ֻ����㷨����Ҫ���ð�ť���������������ڻ��Ǳ��������ȴ�����������������ڶ��ǵ����г���Ȼ��λ�ڵ�һ��
            string shengchangdate = Datecut2(resultwords);
        }
        catch (Exception e)
        {
            //��ӡ�쳣��Ϣ
            Debug.Log("�쳣��" + e);
        }
    }
    public void getPhoto()
    {
        tex.Pause();//��ͣ�����
        StartCoroutine(getTexture());//��ȡ��ǰ����Ƭ
        //tex.Stop();
    }
    public IEnumerator getTexture()//��ȡ��ǰ��Ƭ
    {
        yield return new WaitForEndOfFrame();
        Texture2D img = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, true);//�½�һ����Ƭ��
        img.SetPixels(tex.GetPixels());
        img.Apply();
        byte[] imageTytes = img.EncodeToJPG();//ת��Ϊbyte
        //File.WriteAllBytes(Application.dataPath + "/Pictures/" + "photo.jpg", imageTytes);
        CameraAccurateBasic(imageTytes);//����OCRʶ��
    }
    //ע�⣺��
    //���²����ǽ�ȡ�����ں��������ںͼ���ʱ���Ĵ��봫�������Ϊ�ַ������ͣ���Ҫ���ý������棬������Ҫ��ȡ������������Ӧ�Ľ�����ť
    public string Datecut(string result)//�����ȡ���ַ���
    {
        string back = "";
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i] == '��')
            {
                if (result[i + 1] == '��')
                {
                    if (result[i + 2] == '��')
                    {
                        back += "�����ڣ�";
                        //��ʼ��Ѱ��ʱ��Ĵ���
                        i += 3;
                        while (result[i] != '��' && result[i] != '��' && result[i] != '��')
                        {
                            if (result[i] >= '0' && result[i] <= '9')
                            {
                                back += result[i];
                            }
                            i++;
                        }
                        //���¼���
                        while (result[i] == '��' || result[i] == '��' || result[i] != '��')
                            back += result[i];
                        //�����⵽���»������������ʱresult����洢�ľ�������
                        break;
                    }
                }
            }
        }
        return back;
    }
    //��ȡ��������
    public string Datecut2(string result)
    {
        string back = "";
        int count = 0;//�����������洢8������
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
        foreach (var word in wordsResult)//�������е�����
        {
            if (word.ToString().Contains("����"))
            {
                return word["words"].ToString();
            };
        }
        return "";
    }
}