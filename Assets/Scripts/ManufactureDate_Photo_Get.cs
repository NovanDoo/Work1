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
    public static string resultwords = "";//����ַ���
    public TMP_Text showinfo;
    public string deviceName;//������豸������
    public RawImage display;//��ʾ�����������
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
    public void getPhoto()
    {
        OpenCamera.tex.Pause();//��ͣ�����
        StartCoroutine(getTexture());//��ȡ��ǰ����Ƭ
        //OpenCamera.tex.Stop();
    }
    public IEnumerator getTexture()//��ȡ��ǰ��Ƭ
    {
        yield return new WaitForEndOfFrame();
        Texture2D img = new Texture2D(1000, 1500);
        //Texture2D img = new Texture2D(OpenCamera.tex.width, OpenCamera.tex.height, TextureFormat.ARGB32, true);//�½�һ����Ƭ��
        //img = OpenCamera.display.texture as Texture2D;
        img.ReadPixels(new Rect(Screen.width / 2 - 500, Screen.height / 2 + 50, 1000, 1500), 0, 0, false);
        //img.SetPixels(OpenCamera.tesx.GetPixels());
        img.Apply();
        byte[] imageTytes = img.EncodeToJPG();//ת��Ϊbyte
        //File.WriteAllBytes(Application.dataPath + "/Pictures/" + "photo.jpg", imageTytes);
        CameraAccurateBasic(imageTytes);//����OCRʶ��
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
            //�������ڣ�����ֻ����㷨����Ҫ���ð�ť���������������ڻ��Ǳ��������ȴ�����������������ڶ��ǵ����г���Ȼ��λ�ڵ�һ��
            string shengchangdate = DateGet.Datecut2(resultwords);
            DateGet.ManufactureDate = shengchangdate;
            showinfo.text +="\n�������ڣ�"+shengchangdate;
            showinfo.text += "\n��ʣ��" + DateGet.restdate();
        }
        catch (Exception e)
        {
            //��ӡ�쳣��Ϣ
            Debug.Log("�쳣��" + e);
        }
    }
}
