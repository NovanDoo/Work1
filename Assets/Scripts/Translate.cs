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
    public static string from,to;//源语言的目标语言
    public static string translate(string text)
    {
        var resultStr = "";//返回的中文文本
        if (text.Length > 1 * 10000)
        {
            //当字符串过长时，需要将字符串分段
            int s = text.Length / 10000;//段数
            int y = text.Length % 10000;//余下的字符数
            if (y > 1) s = s + 1;
            int i = 0;
            while (i < s)
            {
                var q = "";//存放分段的字符串
                if (i == 0)//第一段
                {
                    q = text.Substring(0, 10000);
                }
                else if (i == s - 1)//最后一段
                {
                    q = text.Substring((i * 10000) + 1);
                }
                else
                {
                    q = text.Substring((i * 10000) + 1, i * 10000);
                }
                resultStr = resultStr + GetTranslationFromBaiduFanyi(q);//将当前分段的翻译加到结果上
                i = i + 1;
            }
        }
        else
        {
            resultStr=GetTranslationFromBaiduFanyi(text);//只有一段，直接相等
        }
        return resultStr;//显示
    }

    private static string GetTranslationFromBaiduFanyi(string q)//获取翻译的字符串
    {
        string resultStr = "";
        string resultStr2 = "";
        try
        {
            q = q.Replace("\"", "&*&*");//把双引符号替换成指定字符，要不然，翻译的过程中容易丢失符号

            string jsonResult = String.Empty;//返回的json字符串
            
            string languageFrom = from.ToLower();//源语言
            
            string languageTo = to.ToLower();//目标语言
            
            string randomNum = System.DateTime.Now.Millisecond.ToString();//随机数
            
            string md5Sign = GetMD5WithString(appId + q + randomNum + password);//md5加密

            //将几个拼接在一起，发送http请求
            string url = String.Format("http://api.fanyi.baidu.com/api/trans/vip/translate?q={0}&from={1}&to={2}&appid={3}&salt={4}&sign={5}",
                UnityWebRequest.EscapeURL(q, Encoding.UTF8),
                languageFrom,
                languageTo,
                appId,
                randomNum,
                md5Sign
                );
            WebClient wc = new WebClient();
            jsonResult = wc.DownloadString(url);//获取下载的json字符串
            //解析json                
            TranslationResult result = JsonConvert.DeserializeObject<TranslationResult>(jsonResult);
            //判断是否出错
            if (result.Error_code == null)//若没有出错
            {
                //传过去的文本因为一些奇怪的原因会被分成很多段，
                //返回也会很多段，需要对多段的内容进行处理
                for(int i = 0; i < result.Trans_result.Length; i++)
                {
                    resultStr = result.Trans_result[i].Dst;
                    //文本在翻译的过程中，特殊符号前后有时候会多空格；
                    //如&nbsp;可能会翻译为 & nbsp；会导致样式丢失，所以，这个地方处理下
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
                    resultStr2+=resultStr;//将当前的一段文本放进最终结果字符串中
                }
            }
            else
            {
                //检查appid和密钥是否正确
                resultStr = "翻译出错，错误码：" + result.Error_code + "，错误信息：" + result.Error_msg;
            }
        }
        catch (Exception ex)
        {
            resultStr = "翻译出错：" + ex.Message;
        }
        return resultStr2;
    }
    //对字符串做md5加密
    private static string GetMD5WithString(string input)
    {
        if (input == null)//如果输入的字符串为空
        {
            return null;
        }
        MD5 md5Hash = MD5.Create();
        //将输入字符串转换为字节数组并计算哈希数据  
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        //创建一个 Stringbuilder 来收集字节并创建字符串  
        StringBuilder sBuilder = new StringBuilder();
        //循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        //返回十六进制字符串  
        return sBuilder.ToString();
    }
    public class Translation
    {
        public string Src { get; set; }
        public string Dst { get; set; }
    }
    public class TranslationResult
    {
        //错误码，翻译结果无法正常返回
        public string Error_code { get; set; }
        public string Error_msg { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Query { get; set; }
        //翻译正确，返回的结果
        //这里是数组的原因是百度翻译支持多个单词或多段文本的翻译，在发送的字段q中用换行符（\n）分隔
        public Translation[] Trans_result { get; set; }
    }
}
