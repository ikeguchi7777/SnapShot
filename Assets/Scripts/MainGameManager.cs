using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum Item
{
    Transparency,
    SpeedUp,
    FindPlayer,
    Warp,
    MaxNum
}
public class MainGameManager : GameManager
{
    [SerializeField]
    bool isTakeScoreLog;
    [SerializeField]
    ItemUI ItemUI;
    [SerializeField]
    int time = 0;
    float[,] duration = new float[(int)Item.MaxNum,4];
    bool[,] isenable = new bool[(int)Item.MaxNum, 4];
    [SerializeField]
    float Timer = 300;

    protected override void Awake()
    {
        base.Awake();
        if (isTakeScoreLog)
            TakeScoreLog.Make();
        isTakeablePhoto = true;
    }

    protected override void SpawnPlayers()
    {
        tpCameras = new TPCamera[GameInstance.Instance.PlayerNum];
        Players = new SnapShotPlayerController[GameInstance.Instance.PlayerNum];
        var ids = GetComb(players.Length, GameInstance.Instance.PlayerNum);
        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            tpCameras[i] = Instantiate(tpCamera, transform);
            Players[i] = Instantiate(players[ids[i]], position[i], Quaternion.identity, transform);
            Players[i].PlayerID = i;
            tpCameras[i].SetId(i);
            Players[i].SetTPCamera(tpCameras[i]);
            
        }
    }

    int[] GetComb(int num,int length)
    {
        if (length > num)
            return null;
        int[] ans = new int[length];
        for (int i = 0; i < length; i++)
        {
            ans[i] = Random.Range(0, num - i);
            for (int j = 0; j < i; j++)
            {
                if (ans[i] >= ans[j])
                    ans[i]++;
            }
        }
        return ans;
    }

    public void Affect(int playerID, float effectDuration,Item itemID)
    {
        duration[(int)itemID, playerID] += effectDuration;
        if (!isenable[(int)itemID, playerID])
        {
            isenable[(int)itemID, playerID] = true;
            switch (itemID)
            {
                case Item.Transparency:
                    StartCoroutine(TransparencyCoroutine(playerID));
                    break;
                case Item.SpeedUp:
                    StartCoroutine(SpeedUpCoroutine(playerID));
                    break;
                case Item.FindPlayer:
                    StartCoroutine(FindPlayerCoroutine(playerID));
                    break;
                case Item.Warp:
                    Warp(playerID);
                    isenable[(int)itemID, playerID] = false;
                    break;
                default:
                    Debug.Log("効果が設定されていません");
                    break;
            }
        }
    }

    void Warp(int playerID)
    {
        int count = 0;
        tpCameras[playerID].SetItemIcon(Item.Warp);
        while (count != 10)
        {
            int id;
            do
            {
                id = Random.Range(0, GameInstance.Instance.PlayerNum);
            } while (id == playerID);
            Vector3 result;
            if (NavMeshSamplePosition.RandomPoint(Players[id].transform.position - Players[id].transform.forward * 10.0f, 5.0f, out result))
            {
                Players[playerID].transform.position = result;
                return;
            }
            count++;
        }
        Players[playerID].transform.position = position[Random.Range(0, position.Length)];
    }

    IEnumerator TransparencyCoroutine(int playerID)
    {
        for (int i = 0; i < tpCameras.Length; i++)
        {
            if (i == playerID)
                continue;
            tpCameras[i].SetLayer(LayerMask.GetMask((playerID + 1) + "P"), false);
        }
        var itemUI = tpCameras[playerID].SetItemIcon(Item.Transparency);
        yield return null;
        do
        {
            var t = duration[(int)Item.Transparency, playerID];
            duration[(int)Item.Transparency, playerID] = 0.0f;
            while (t>0.0f)
            {
                if (!pause.isPaused)
                    t -= Time.deltaTime;
                yield return null;
            }
        } while (duration[(int)Item.Transparency, playerID]>0);
        isenable[(int)Item.Transparency, playerID] = false;
        tpCameras[playerID].RemoveIcon(itemUI);
        for (int i = 0; i < tpCameras.Length; i++)
        {
            if (i == playerID)
                continue;
            tpCameras[i].SetLayer(LayerMask.GetMask((playerID + 1) + "P"), true);
        }
    }

    IEnumerator SpeedUpCoroutine(int playerID)
    {
        Players[playerID].ChangeSpeed(2.0f);
        var itemUI=tpCameras[playerID].SetItemIcon(Item.SpeedUp);
        yield return null;
        do
        {
            var t = duration[(int)Item.SpeedUp, playerID];
            duration[(int)Item.SpeedUp, playerID] = 0.0f;
            while (t > 0.0f)
            {
                if (!pause.isPaused)
                    t -= Time.deltaTime;
                yield return null;
            }
        } while (duration[(int)Item.SpeedUp, playerID] > 0);
        Debug.Log("加速終了");
        tpCameras[playerID].RemoveIcon(itemUI);
        isenable[(int)Item.SpeedUp, playerID] = false;
        Players[playerID].ChangeSpeed(0.5f);
    }

    IEnumerator FindPlayerCoroutine(int playerID)
    {
        var itemUI = tpCameras[playerID].SetItemIcon(Item.FindPlayer);
        yield return null;
        do
        {
            var t = duration[(int)Item.FindPlayer, playerID];
            var ids = new int[GameInstance.Instance.PlayerNum - 1];
            int num = 0;
            Vector3 view_pos;
            for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
            {
                if (i == playerID)
                    continue;
                ids[num++] = i;
            }
            duration[(int)Item.FindPlayer, playerID] = 0.0f;
            while (t > 0.0f)
            {
                if (pause.isPaused)
                    yield return null;
                t -= Time.deltaTime;
                var cam = Players[playerID].currentCamera;
                tpCameras[playerID].ClearPlayerIcon();
                foreach (var id in ids)
                {
                    if (Players[id].IsInViewport(cam,out view_pos))
                    {
                        tpCameras[playerID].SetPlayerIcon(view_pos, id);
                    }
                }
                yield return null;
            }
        } while (duration[(int)Item.FindPlayer, playerID] > 0);
        Debug.Log("探索終了");
        tpCameras[playerID].RemoveIcon(itemUI);
        tpCameras[playerID].ClearPlayerIcon();
        isenable[(int)Item.FindPlayer, playerID] = false;
    }
    protected override void Update()
    {
        base.Update();
        if (!pause.isPaused)
            Timer -= Time.deltaTime;
        if (Timer < 0.0f)
            SceneManager.LoadScene("Result");


        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Result");
        }
    }
}
