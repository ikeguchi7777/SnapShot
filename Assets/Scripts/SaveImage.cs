using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//File.~を使うため
using System.IO;

public class SaveImage : MonoBehaviour
{
    public Camera eyeCamera;
    private Texture2D texture;

    private int photoNumber = 1;

    //[SerializeField] SoundController soundController;

    public SaveImage() { }

    // Use this for initialization
    void Start()
    {
        texture = new Texture2D(eyeCamera.targetTexture.width, eyeCamera.targetTexture.height,
                                TextureFormat.RGB24, false);
    }
    // Update is called once per frame
    void Update()
    {
        //キーボードの「s」を押したら画像を保存
        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveCameraImage();
            IsRendered.Renderedcheck();
            soundController.PlaySE(SoundController.Sound.camera);
            
        }*/
    }

    public void SaveCameraImage()
    {
        // Remember currently active render textureture
        RenderTexture currentRT = RenderTexture.active;
        // Set the supplied RenderTexture as the active one
        RenderTexture.active = eyeCamera.targetTexture;
        eyeCamera.Render();
        // Create a new Texture2D and read the RenderTexture texture into it
        texture.ReadPixels(new Rect(0, 0, eyeCamera.targetTexture.width, eyeCamera.targetTexture.height), 0, 0);
        //転送処理の適用
        texture.Apply();
        // Restorie previously active render texture to avoid errors
        RenderTexture.active = currentRT;
        //PNGに変換
        byte[] bytes = texture.EncodeToPNG();
        //保存
        File.WriteAllBytes("C:/Users/Owner/Downloads/Unity Projects/ゲーム/SnapShot/Assets/Image/" + photoNumber + ".png", bytes);
        photoNumber++;
    }
}