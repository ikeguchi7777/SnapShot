using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UniRx.Async;
using System.IO;

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using Graphics = System.Drawing.Graphics;
using System.Threading.Tasks;

public delegate void ChangeBatteryUI(bool IO);
public class SmartPhoneCamera : MonoBehaviour
{
    
    public delegate void BatteryEventHandler(float value);
    public event BatteryEventHandler OnBatterChanged;

    const float MAX_BATTERY = 100.0f;
    public Camera _camera { get; private set; }
    GameManager gameManager;
    public RenderTexture _panelTexture { get; private set; }
    [SerializeField]
    Material panelMaterial = null;
    [SerializeField]
    int width = 960, height = 540;

    int photoNum = 0;

    const int border = 10;

    float powerConsumption = 2.0f;
    float photoConsume = 5.0f;
    float energy
    {
        get { return _energy; }
        set
        {
            _energy = value;
            OnBatterChanged(_energy/MAX_BATTERY);
        }
    }
    float _energy = 100.0f;

    bool _useable = true;
    bool _isPowerOn = true;
    public bool Useable
    {
        get { return _useable; }
        set
        {
            if (value != _isPowerOn)
            {
                _useable = value;
                _isPowerOn = value;
                if (value)
                    PowerON();
                else
                    PowerOFF();
            }
        }
    }

    int cullingMask;

    public ChangeBatteryUI changeBatteryUI { set; private get; }

    void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        var textureDescriptor = new RenderTextureDescriptor(width, height);
        _panelTexture = new RenderTexture(textureDescriptor);
        _camera.targetTexture = _panelTexture;
        panelMaterial.mainTexture = _panelTexture;
        panelMaterial.SetTexture("_EmissionMap", _panelTexture);
        gameManager = FindObjectOfType<GameManager>();
        enabled = false;
        cullingMask = _camera.cullingMask;
        
    }


    void PowerON()
    {
        //_camera.cullingMask = cullingMask;
        //_camera.clearFlags = CameraClearFlags.Skybox;
        changeBatteryUI(true);
    }

    void PowerOFF()
    {
        //_camera.cullingMask = 0;
        //_camera.clearFlags = CameraClearFlags.SolidColor;
        changeBatteryUI(false);
    }

    public async void TakePhoto(int playerID)
    {
        if (!gameManager.isTakeablePhoto)
            return;
        _useable = false;
        var score = CaluculateScore(playerID);
        Debug.Log("Score:"+score);
        var _tex = new Texture2D(width, height,TextureFormat.RGBA32,false);
        var request = AsyncGPUReadback.Request(_panelTexture);
        await UniTask.WaitUntil(() => request.done == true);
        var buffer = request.GetData<byte>();
        var path = Application.dataPath +"/Image/" + (playerID+1) + "P/" + (playerID+1) + "P_" + photoNum + ".png";
        GameInstance.Instance.EachPicture[playerID].Add(new PictureScore(path, score));
        if (border <= score)
            (gameManager as MainGameManager).Affect(playerID, 5.0f, Item.Transparency);
        photoNum++;
        Task task = Task.Run(() => {
            test(path, buffer.ToArray());
        });
        Debug.Log("Saved");
        ConsumeBattery(photoConsume);
        await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f), ignoreTimeScale: false);
        if (energy > 0.0f)
            _useable = true;
    }

    int CaluculateScore(int playerID)
    {
        int score = 0;
        if (TakeScoreLog.instance != null)
            TakeScoreLog.instance.AddLog("Take:Player" + (playerID + 1));
        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            if (i == playerID||Distance(gameManager.Players[i].gameObject,_camera.gameObject)>ScoreConfig.maxDistance)
                continue;
            //if (_camera.fieldOfView * 2.12f < Vector3.Angle(_camera.transform.forward, gameManager.Players[i].transform.position - _camera.transform.position))
            if(!gameManager.Players[i].IsInViewport(_camera))
                continue;
            Debug.Log("Player" + (i + 1));
            int t = gameManager.Players[i].CalculateScore(_camera);
            if (t >= border)
                (gameManager as MainGameManager).Affect(i, 5.0f, Item.Transparency);
            score += t;
        }
        if (TakeScoreLog.instance != null)
            TakeScoreLog.instance.AddLog("Score:" + score);
        return score;
    }

    float Distance(GameObject a,GameObject b)
    {
        return (a.transform.position - b.transform.position).magnitude;
    }

    public bool ConsumeBattery(float consumption)
    {
        energy -= consumption;
        if (energy <= 0.0f)
        {
            energy = 0.0f;
            Useable = false;
        }
        return Useable;
    }

    public bool ConsumeStandbyPower(float dtime)
    {
        energy -= dtime * powerConsumption;
        if (energy <= 0.0f)
        {
            energy = 0.0f;
            Useable = false;
        }
        return Useable;
    }

    public void ChargeBattery(float value)
    {
        if (!Useable)
        {
            Useable = true;
        }
        energy = Mathf.Clamp(energy + value, 0.0f, MAX_BATTERY);

    }

    public void test(string path,byte[] raw)
    {
        int length = width * height;
        for (int i = 0; i < length; i++)
        {
            byte tmp = raw[i * 4];
            raw[i * 4] = raw[i * 4 + 2];
            raw[i * 4 + 2] = tmp;
        }

        Bitmap bitmap = new Bitmap(width, height);
        BitmapData _data = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadWrite,
            PixelFormat.Format32bppArgb);

        
        Marshal.Copy(raw, 0, _data.Scan0, raw.Length);

        bitmap.UnlockBits(_data);
        bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
        bitmap.Save(path,ImageFormat.Png);
    }
}