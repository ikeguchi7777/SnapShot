using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Item
{
    Transparency,
    SpeedUp,
    MaxNum
}
public class MainGameManager : GameManager
{
    [SerializeField]
    bool isTakeScoreLog;
    float[,] duration = new float[(int)Item.MaxNum,4];
    bool[,] isenable = new bool[(int)Item.MaxNum, 4];

    protected override void Awake()
    {
        base.Awake();
        if (isTakeScoreLog)
            TakeScoreLog.Make();
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
                default:
                    Debug.Log("効果が設定されていません");
                    break;
            }
        }
    }

    IEnumerator TransparencyCoroutine(int playerID)
    {
        for (int i = 0; i < tpCameras.Length; i++)
        {
            if (i == playerID)
                continue;
            tpCameras[i].SetLayer(LayerMask.GetMask((playerID + 1) + "P"), false);
        }
        yield return null;
        do
        {
            var t = duration[(int)Item.Transparency, playerID];
            duration[(int)Item.Transparency, playerID] = 0.0f;
            yield return new WaitForSeconds(t);
        } while (duration[(int)Item.Transparency, playerID]>0);
        isenable[(int)Item.Transparency, playerID] = false;
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
        yield return null;
        do
        {
            var t = duration[(int)Item.SpeedUp, playerID];
            duration[(int)Item.SpeedUp, playerID] = 0.0f;
            yield return new WaitForSeconds(t);
        } while (duration[(int)Item.SpeedUp, playerID] > 0);
        Debug.Log("加速終了");
        isenable[(int)Item.SpeedUp, playerID] = false;
        Players[playerID].ChangeSpeed(0.5f);
    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.T)){
            Affect(0,10,Item.Transparency);
        }
    }
}
