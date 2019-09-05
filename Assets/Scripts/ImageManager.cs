using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//File.~を使うため
using System.IO;

public class ImageManager : MonoBehaviour
{
    [SerializeField] Camera eyeCamera = default;
    private Texture2D texture;
    int X;
    int Y;

    int id;
    int imageNumber = 0;

   
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


    }

    public void SaveCameraImage()
    {
        imageNumber++;

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
        File.WriteAllBytes(Application.dataPath + "/Image/" + id + "P/" + id + "P_" + imageNumber + ".png", bytes);

    }

    public void DeleteImage()
    {
        DirectoryInfo target = new DirectoryInfo(Application.dataPath + "/Image/" + id + "P");
        foreach (var file in target.GetFiles())
        {
            file.Delete();
        }
    }


    byte[] ReadPngFile(int playerID, int imageNumber)
    {
        FileStream fileStream = new FileStream(Application.dataPath + "/Image/" + playerID + "P/" + playerID + "P_" + +imageNumber + ".png", FileMode.Open, FileAccess.Read);

        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }

    Texture2D ReadPng(int playerID, int imageNumber)
    {
        byte[] readBinary = ReadPngFile(playerID, imageNumber);

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

    public Sprite SpriteFromTexture2D(int playerID, int imageNumber)
    {
        Sprite sprite = null;
        if (ReadPng(playerID, imageNumber))
        {
            Texture2D png = ReadPng(playerID, imageNumber);
            //Texture2DからSprite作成
            sprite = Sprite.Create(png, new Rect(0, 0, png.width, png.height), new Vector2(0.5f, 0.5f));
        }
        return sprite;
    }

    public GameObject LoadImage(int playerID, int imageNumber)
    {

        GameObject obj = new GameObject(playerID + "P_" + imageNumber);

        obj.transform.parent = GameObject.Find("Canvas/" + playerID + "PPanel").transform;
        obj.AddComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        obj.AddComponent<Image>().sprite = SpriteFromTexture2D(playerID, imageNumber);
        obj.GetComponent<Image>().preserveAspect = true;
        obj.GetComponent<Image>().SetNativeSize();

        //obj.AddComponent<SpriteRenderer>();
        //obj.GetComponent<SpriteRenderer>().sprite = SpriteFromTexture2D(playerID, photoNumber);
        //obj.transform.position = new Vector2(X, Y);
        //obj.transform.localScale = new Vector3(0.5f, 0.5f, 1);


        return obj;
    }
}