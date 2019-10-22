using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UniRx.Async;
using System.IO;


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

    float powerConsumption = 1.0f;
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
    Coroutine consumingRoutine;

    bool _useable = true;
    public bool Useable
    {
        get { return _useable; }
        set
        {
            if (value != _useable)
            {
                _useable = value;
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
        var score = CaluculateScore(playerID);
        Debug.Log("Score:"+score);
        var _tex = new Texture2D(width, height,TextureFormat.RGBA32,false);
        var request = AsyncGPUReadback.Request(_panelTexture);
        await UniTask.WaitUntil(() => request.done == true);
        var buffer = request.GetData<Color32>();
        _tex.LoadRawTextureData(buffer);
        _tex.Apply();
        var buf_png = _tex.EncodeToPNG();
        var path = Application.dataPath +"/Image/" + (playerID+1) + "P/" + (playerID+1) + "P_" + photoNum + ".png";
        GameInstance.Instance.EachPicture[playerID].Add(new PictureScore(path, score));
        photoNum++;
        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            await fs.WriteAsync(buf_png, 0, buf_png.Length);
        }
        Debug.Log("Saved");
    }

    int CaluculateScore(int playerID)
    {
        int score = 0;
        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            if (i == playerID||Distance(gameManager.Players[i].gameObject,_camera.gameObject)>ScoreConfig.maxDistance)
                continue;
            //if (_camera.fieldOfView * 2.12f < Vector3.Angle(_camera.transform.forward, gameManager.Players[i].transform.position - _camera.transform.position))
            if(!gameManager.Players[i].IsInViewport(_camera))
                continue;
            Debug.Log("Player" + (i + 1));
            score += gameManager.Players[i].CalculateScore(_camera);
        }
        return score;
    }

    float Distance(GameObject a,GameObject b)
    {
        return (a.transform.position - b.transform.position).magnitude;
    }

    public bool ConsumeBattery(float consumption)
    {
        if (energy - consumption < 0.0f)
        {
            return false;
        }
        energy -= consumption;
        if (energy == 0.0f)
        {
            Useable = false;
        }
        return true;
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
}
