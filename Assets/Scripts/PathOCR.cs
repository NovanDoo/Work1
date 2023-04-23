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
    public TMP_InputField path;//�����
    public TMP_Text showinfo;//�������
    static Ocr client;//ocr�ĵ�����
    // Start is called before the first frame update
    void Awake()
    {
        client = new Ocr(API_KEY, SECRET_KEY);// newһ������ʶ��Ocr
        client.Timeout = 60000;  //�޸ĳ�ʱʱ��
    }
    public void buttonClick()//��ť�����ʼ
    {
        StartCoroutine(Ocr());//����Э�̿�ʼocr
    }
    public IEnumerator Ocr()
    {
        yield return new WaitForEndOfFrame();
        string resultwords = "";//���ս��
        string imgpath = path.text;//�ļ�·��
        var image = File.ReadAllBytes(imgpath);//��ȡ�ֽ�
        //�쳣����
        try
        {
            var result = client.AccurateBasic(image);// ��ȡAPIʶ��ͼƬ����
            var wordsResult = result["words_result"];//��ȡ�����array����
            Debug.Log(result);
            foreach (var word in wordsResult)//�������е�����
            {
                resultwords += word["words"].ToString();//�ӵ����string��
            }
            Debug.Log(resultwords);
            //�������ٵ�������Datecut����������
            //�����ڣ�����ֻ����㷨����Ҫ���ð�ť������ifʲôʱ����ñ������㷨
            string baozhiqi = Datecut(resultwords);
            showinfo.text = InfoCut(wordsResult)+"\n";
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
    public string Datecut(string result)
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
