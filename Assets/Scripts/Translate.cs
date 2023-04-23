using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Translate : MonoBehaviour
{
    static string appId = "20230405001629111";
    static string password = "IlOvqYCMCB6OVidaVkSp";
    public static string from,to;//Դ���Ե�Ŀ������
    public static string translate(string text)
    {
        var resultStr = "";//���ص������ı�
        if (text.Length > 1 * 10000)
        {
            //���ַ�������ʱ����Ҫ���ַ����ֶ�
            int s = text.Length / 10000;//����
            int y = text.Length % 10000;//���µ��ַ���
            if (y > 1) s = s + 1;
            int i = 0;
            while (i < s)
            {
                var q = "";//��ŷֶε��ַ���
                if (i == 0)//��һ��
                {
                    q = text.Substring(0, 10000);
                }
                else if (i == s - 1)//���һ��
                {
                    q = text.Substring((i * 10000) + 1);
                }
                else
                {
                    q = text.Substring((i * 10000) + 1, i * 10000);
                }
                resultStr = resultStr + GetTranslationFromBaiduFanyi(q);//����ǰ�ֶεķ���ӵ������
                i = i + 1;
            }
        }
        else
        {
            resultStr=GetTranslationFromBaiduFanyi(text);//ֻ��һ�Σ�ֱ�����
        }
        return resultStr;//��ʾ
    }

    private static string GetTranslationFromBaiduFanyi(string q)//��ȡ������ַ���
    {
        string resultStr = "";
        string resultStr2 = "";
        try
        {
            q = q.Replace("\"", "&*&*");//��˫�������滻��ָ���ַ���Ҫ��Ȼ������Ĺ��������׶�ʧ����

            string jsonResult = String.Empty;//���ص�json�ַ���
            
            string languageFrom = from.ToLower();//Դ����
            
            string languageTo = to.ToLower();//Ŀ������
            
            string randomNum = System.DateTime.Now.Millisecond.ToString();//�����
            
            string md5Sign = GetMD5WithString(appId + q + randomNum + password);//md5����

            //������ƴ����һ�𣬷���http����
            string url = String.Format("http://api.fanyi.baidu.com/api/trans/vip/translate?q={0}&from={1}&to={2}&appid={3}&salt={4}&sign={5}",
                UnityWebRequest.EscapeURL(q, Encoding.UTF8),
                languageFrom,
                languageTo,
                appId,
                randomNum,
                md5Sign
                );
            WebClient wc = new WebClient();
            jsonResult = wc.DownloadString(url);//��ȡ���ص�json�ַ���
            //����json                
            TranslationResult result = JsonConvert.DeserializeObject<TranslationResult>(jsonResult);
            //�ж��Ƿ����
            if (result.Error_code == null)//��û�г���
            {
                //����ȥ���ı���ΪһЩ��ֵ�ԭ��ᱻ�ֳɺܶ�Σ�
                //����Ҳ��ܶ�Σ���Ҫ�Զ�ε����ݽ��д���
                for(int i = 0; i < result.Trans_result.Length; i++)
                {
                    resultStr = result.Trans_result[i].Dst;
                    //�ı��ڷ���Ĺ����У��������ǰ����ʱ����ո�
                    //��&nbsp;���ܻᷭ��Ϊ & nbsp���ᵼ����ʽ��ʧ�����ԣ�����ط�������
                    resultStr = resultStr.Replace(" *", "*");
                    resultStr = resultStr.Replace("* ", "*");
                    resultStr = resultStr.Replace("& ", "&");
                    resultStr = resultStr.Replace("/ ", "/");
                    resultStr = resultStr.Replace(" /", "/");
                    resultStr = resultStr.Replace(" <", "<");
                    resultStr = resultStr.Replace("< ", "<");
                    resultStr = resultStr.Replace(" >", ">");
                    resultStr = resultStr.Replace("> ", ">");
                    resultStr = resultStr.Replace("&#39;", "");
                    resultStr = resultStr.Replace(". ", ".");
                    resultStr = resultStr.Replace(" .", ".");
                    resultStr = resultStr.Replace("&quot;", "");
                    resultStr = resultStr.Replace("&*&*", "\"");
                    resultStr2+=resultStr;//����ǰ��һ���ı��Ž����ս���ַ�����
                }
            }
            else
            {
                //���appid����Կ�Ƿ���ȷ
                resultStr = "������������룺" + result.Error_code + "��������Ϣ��" + result.Error_msg;
            }
        }
        catch (Exception ex)
        {
            resultStr = "�������" + ex.Message;
        }
        return resultStr2;
    }
    //���ַ�����md5����
    private static string GetMD5WithString(string input)
    {
        if (input == null)//���������ַ���Ϊ��
        {
            return null;
        }
        MD5 md5Hash = MD5.Create();
        //�������ַ���ת��Ϊ�ֽ����鲢�����ϣ����  
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        //����һ�� Stringbuilder ���ռ��ֽڲ������ַ���  
        StringBuilder sBuilder = new StringBuilder();
        //ѭ��������ϣ���ݵ�ÿһ���ֽڲ���ʽ��Ϊʮ�������ַ���  
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        //����ʮ�������ַ���  
        return sBuilder.ToString();
    }
    public class Translation
    {
        public string Src { get; set; }
        public string Dst { get; set; }
    }
    public class TranslationResult
    {
        //�����룬�������޷���������
        public string Error_code { get; set; }
        public string Error_msg { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Query { get; set; }
        //������ȷ�����صĽ��
        //�����������ԭ���ǰٶȷ���֧�ֶ�����ʻ����ı��ķ��룬�ڷ��͵��ֶ�q���û��з���\n���ָ�
        public Translation[] Trans_result { get; set; }
    }
}
