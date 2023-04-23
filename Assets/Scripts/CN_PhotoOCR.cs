using UnityEngine;
using System.Collections;
using Baidu.Aip.Ocr;
using System;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;

public class CN_PhotoOCR : MonoBehaviour
{
    public Text TxtCurrentTime;
    public static string resultwords = "";//����ַ���
    public TMP_Text showinfo;
    //���շ��ص�ͼƬ����  

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
            showinfo.text=resultwords;
            //�������ٵ�������Datecut����������
            //�����ڣ�����ֻ����㷨����Ҫ���ð�ť������ifʲôʱ����ñ������㷨
            string baozhiqi = Datecut(resultwords);
            showinfo.text = InfoCut(wordsResult) + "\n";
            showinfo.text += "������"+baozhiqi;
            OpenCamera.tex.Stop();
        }
        catch (Exception e)
        {
            //��ӡ�쳣��Ϣ
            Debug.Log("�쳣��" + e);
        }
    }
    public void getPhoto()
    {
        //OpenCamera.tex.Pause();//��ͣ�����
        StartCoroutine(getTexture());//��ȡ��ǰ����Ƭ
        //OpenCamera.tex.Stop();
    }
   
    public IEnumerator getTexture()//��ȡ��ǰ��Ƭ
    {
        yield return new WaitForEndOfFrame();
        Texture2D img = new Texture2D(1000, 1500);
        //Texture2D img = new Texture2D(OpenCamera.tex.width, OpenCamera.tex.height, TextureFormat.ARGB32, true);//�½�һ����Ƭ��
        //img = OpenCamera.display.texture as Texture2D;
        img.ReadPixels(new Rect(Screen.width / 2 - 500, Screen.height / 2+50, 1000, 1500), 0, 0, false);
        //img.SetPixels(OpenCamera.tesx.GetPixels());
        img.Apply();
        byte[] imageTytes = img.EncodeToJPG();//ת��Ϊbyte
        //File.WriteAllBytes(Application.dataPath + "/Pictures/" + "photo.jpg", imageTytes);
        CameraAccurateBasic(imageTytes);//����OCRʶ��
    }
    //ע�⣺
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
                        DateGet.QualityGuaranteePeriod = back;
                        //���¼���
                        while (result[i]== '��'|| result[i] == '��' || result[i] != '��')
                            back +=result[i];
                        //�����⵽���»������������ʱresult����洢�ľ�������
                        break;
                    }
                }
            }
        }
        return back;
    }
    //��ȡ��������
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