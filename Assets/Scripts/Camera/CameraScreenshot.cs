using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class CameraScreenshot : MonoBehaviour
{
    public int width = 1920; // 截图的宽度
    public int height = 1080; // 截图的高度

    public Camera mainCamera;
    public List<Texture2D> ScreenShots = new();
    private void Start()
    {
        //mainCamera = Camera.main;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot(1);
        }
    }
    public void TakeScreenshot(int index)
    {
        mainCamera.GetComponent<Camera>().enabled = true;
        // 创建一个RenderTexture来保存相机画面
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        mainCamera.targetTexture = renderTexture;
        mainCamera.Render();

        // 将RenderTexture转换为Texture2D
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RG16, false);
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        ScreenShots.Add(screenshot);
    }
    
}
