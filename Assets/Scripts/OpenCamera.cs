using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCamera : MonoBehaviour
{
    // Start is called before the first frame update
    static public WebCamTexture tex;
    public string deviceName;//摄像机设备的名称
    static public RawImage display;//显示的摄像机内容
    void Start()
    {
        display = GameObject.Find("Photo").GetComponent<RawImage>();
    }
    public void openCamera()//开启摄像机
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
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);//请求摄像机权限
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;//获取所有的摄像机设备
            deviceName = devices[0].name;
            tex = new WebCamTexture(deviceName);//附给tex
            display.texture = tex;//将拍摄的内容显示
            tex.Play();//开始拍摄
            display.rectTransform.localEulerAngles = new Vector3(0, 0, -tex.videoRotationAngle);
        }
    }
}
