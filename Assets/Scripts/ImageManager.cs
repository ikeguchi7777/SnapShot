using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//File.~を使うため
using System.IO;

public class ImageManager : MonoBehaviour
{
    [SerializeField] Camera eyeCamera;
    private Texture2D texture;
    int X;
    int Y;

    int id;
    int photoNumber = 1;

    //[SerializeField] SoundController soundController;

    public ImageManager() { }

    // Use this for initialization
    void Start()
    {
        if (GetComponent<PlayerController>())
        {
            id = GetComponent<PlayerController>().GetId();

            switch (id)
            {
                case 1: X = 0; Y = 0; break;
                case 2: X = 1; Y = 0; break;
                case 3: X = 0; Y = 1; break;
                case 4: X = 1; Y = 1; break;

            }




            texture = new Texture2D(eyeCamera.targetTexture.width / 2, eyeCamera.targetTexture.height / 2,
                                TextureFormat.RGB24, false);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = new GameObject();
            obj.name = "picture";
            obj.AddComponent<SpriteRenderer>();
            obj.GetComponent<SpriteRenderer>().sprite = SpriteFromTexture2D(1, 1);
            obj.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            Debug.Log("ok");
        }
    }

    public void SaveCameraImage()
    {
        // Remember currently active render textureture
        RenderTexture currentRT = RenderTexture.active;
        // Set the supplied RenderTexture as the active one
        RenderTexture.active = eyeCamera.targetTexture;
        eyeCamera.Render();
        // Create a new Texture2D and read the RenderTexture texture into it
        texture.ReadPixels(new Rect(eyeCamera.targetTexture.width / 2 * X, eyeCamera.targetTexture.height / 2 * Y, eyeCamera.targetTexture.width / 2, eyeCamera.targetTexture.height / 2), 0, 0);
        //転送処理の適用
        texture.Apply();
        // Restorie previously active render texture to avoid errors
        RenderTexture.active = currentRT;
        //PNGに変換
        byte[] bytes = texture.EncodeToPNG();
        //保存
        File.WriteAllBytes(Application.dataPath + "/Image/" + id + "P/" + photoNumber + ".png", bytes);
        photoNumber++;
    }

    public void DeleteImage()
    {
        DirectoryInfo target = new DirectoryInfo(Application.dataPath + "/Image/" + id + "P");
        foreach (var file in target.GetFiles())
        {
            file.Delete();
        }
    }


    byte[] ReadPngFile(int playerID ,int photoNumber)
    {
        FileStream fileStream = new FileStream(Application.dataPath + "/Image/" + playerID + "P/" + photoNumber + ".png", FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }


    Texture2D ReadPng(int playerID ,int photoNumber)
    {
        byte[] readBinary = ReadPngFile(playerID, photoNumber);

        int pos = 16; // 16バイトから開始

        int width = 0;
        for (int i = 0; i < 4; i++)
        {
            width = width * 256 + readBinary[pos++];
        }

        int height = 0;
        for (int i = 0; i < 4; i++)
        {
            height = height * 256 + readBinary[pos++];
        }

        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(readBinary);

        return texture;
    }

    public Sprite SpriteFromTexture2D(int playerID ,int photoNumber)
    {
        Sprite sprite = null;
        if (ReadPng(playerID ,photoNumber))
        {
            Texture2D png = ReadPng(playerID, photoNumber);
            //Texture2DからSprite作成
            sprite = Sprite.Create(png, new Rect(0, 0, png.width, png.height), new Vector2(0.5f,0.5f));
        }
        return sprite;
    }

}