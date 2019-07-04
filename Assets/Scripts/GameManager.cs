using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    SnapShotPlayerController[] players;
    [SerializeField]
    TPCamera tpCamera;
    [SerializeField]
    Vector3[] position;
    public TPCamera[] tpCameras{get;private set;}
    public SnapShotPlayerController[] Players{get;private set;}
    // Start is called before the first frame update
    void Awake()
    {
        if(GameInstance.Instance.PlayerNum == 0){
            Debug.LogError("人数が正しくない");
            #if UNITY_EDITOR
            GameInstance.Instance.PlayerNum = 1;
            #else
            SceneManager.LoadScene("Title");
            #endif
        }
        tpCameras = new TPCamera[GameInstance.Instance.PlayerNum];
        Players = new SnapShotPlayerController[GameInstance.Instance.PlayerNum];
        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            tpCameras[i] = Instantiate(tpCamera);
            Players[i] = Instantiate(players[i],position[i],Quaternion.identity);
            Players[i].PlayerID = i;
            tpCameras[i].SetId(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
