using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UniRx.Async;
using System.IO;

public class SmartPhoneCamera : MonoBehaviour
{
    Camera _camera;
    GameManager gameManager;
    public RenderTexture _panelTexture { get; private set; }
    [SerializeField]
    Material panelMaterial = null;
    [SerializeField]
    int width = 960, height = 540;

    int photoNum = 0;
    
    void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        var textureDescriptor = new RenderTextureDescriptor(width, height);
        _panelTexture = new RenderTexture(textureDescriptor);
        _camera.targetTexture = _panelTexture;
        panelMaterial.mainTexture = _panelTexture;
        panelMaterial.SetTexture("_EmissionMap", _panelTexture);
        gameManager = FindObjectOfType<GameManager>();
    }

    public async void TakePhoto(int playerID)
    {
        var score = CaluculateScore(playerID);
        Debug.Log("Score:"+score);
        GameInstance.Instance.scores[playerID].Enqueue(score);
        var _tex = new Texture2D(width, height,TextureFormat.RGBA32,false);
        var request = AsyncGPUReadback.Request(_panelTexture);
        await UniTask.WaitUntil(() => request.done == true);
        //while (!request.done)
        //    yield return null;
        var buffer = request.GetData<Color32>();
        _tex.LoadRawTextureData(buffer);
        _tex.Apply();
        var buf_png = _tex.EncodeToPNG();
        var path = Application.dataPath +"/Image/" + (playerID+1) + "P/" + (playerID+1) + "P_" + photoNum + ".png";
        photoNum++;
        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            await fs.WriteAsync(buf_png, 0, buf_png.Length);
        }
        GameInstance.Instance.photos[playerID].Enqueue(Sprite.Create(_tex, new Rect(0, 0, width, height), Vector2.zero));
        Debug.Log("Saved");
    }

    int CaluculateScore(int playerID)
    {
        int score = 0;
        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            if (i == playerID||Distance(gameManager.Players[i].gameObject,_camera.gameObject)>ScoreConfig.maxDistance)
                continue;
            if (_camera.fieldOfView * 2.12f < Vector3.Angle(_camera.transform.forward, gameManager.Players[i].transform.position - _camera.transform.position))
                continue;
            score += gameManager.Players[i].CalculateScore(_camera);
        }
        return score;
    }

    float Distance(GameObject a,GameObject b)
    {
        return (a.transform.position - b.transform.position).magnitude;
    }
}
