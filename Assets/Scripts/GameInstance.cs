using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance
{
    public int PlayerNum;                   //プレイヤーの人数1～4
    GameInstance(){
        init();
    }
    private static GameInstance _instance;

    public static GameInstance Instance{
        get{
            if(_instance == null)
                _instance = new GameInstance();
            return _instance;
        }
    }

    public void init(){
        PlayerNum = 4;
    }
}
