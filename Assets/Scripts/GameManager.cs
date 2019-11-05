using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    SnapShotPlayerController[] players = null;
    [SerializeField]
    TPCamera tpCamera = null;
    [SerializeField]
    protected Vector3[] position = null;
    public TPCamera[] tpCameras{get;private set;}
    public SnapShotPlayerController[] Players{get;private set;}

    protected PauseSystem pause;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        if(GameInstance.Instance.PlayerNum == 0){
            Debug.LogError("人数が正しくない");
            #if UNITY_EDITOR
            GameInstance.Instance.PlayerNum = 4;
            #else
            SceneManager.LoadScene("Title");
            #endif
        }
        tpCameras = new TPCamera[GameInstance.Instance.PlayerNum];
        Players = new SnapShotPlayerController[GameInstance.Instance.PlayerNum];
        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            tpCameras[i] = Instantiate(tpCamera,transform);
            Players[i] = Instantiate(players[i],position[i],Quaternion.identity,transform);
            Players[i].PlayerID = i;
            tpCameras[i].SetId(i);
            Players[i].SetTPCamera(tpCameras[i]);
        }
        pause = GetComponent<PauseSystem>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            for (int i= 1; i <= 4; i++)
            {
                if (Input.GetButton("Pause" + i))
                {
                    pause.Pause(i);
                    return;
                }
            }
            Debug.LogError("PauseFail");
        }

    }
    public void Respawn(int playerID)
    {
        Players[playerID].transform.position = position[Random.Range(0, position.Length)];
    }
}
