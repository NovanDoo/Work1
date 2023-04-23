using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCamera : MonoBehaviour
{
    // Start is called before the first frame update
    static public WebCamTexture tex;
    public string deviceName;//������豸������
    static public RawImage display;//��ʾ�����������
    void Start()
    {
        display = GameObject.Find("Photo").GetComponent<RawImage>();
    }
    public void openCamera()//���������
    {
        if(tex != null)
        {
            tex.Play();
            
        }
        else 
            StartCoroutine(start());
    }
    public IEnumerator start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);//���������Ȩ��
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;//��ȡ���е�������豸
            deviceName = devices[0].name;
            tex = new WebCamTexture(deviceName);//����tex
            display.texture = tex;//�������������ʾ
            tex.Play();//��ʼ����
            display.rectTransform.localEulerAngles = new Vector3(0, 0, -tex.videoRotationAngle);
        }
    }
}
