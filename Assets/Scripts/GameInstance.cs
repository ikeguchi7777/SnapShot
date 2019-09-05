using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance
{

    public List<PictureScore>[] EachPicture = new List<PictureScore>[4];
    public int[] TotalScore { get; set;} = new int[4];
    public int[] Ranking { get; set; } = new int[4];

    public int PlayerNum;                   //プレイヤーの人数1～4

    public Sprite camtest;

    public Queue<Sprite>[] photos;

    public Queue<int>[] scores;
    GameInstance(){
        for (int i = 0; i < EachPicture.Length; i++)
        {
            EachPicture[i] = new List<PictureScore>();
        }
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

        for (int i = 0; i < EachPicture.Length; i++)
        {
            EachPicture[i].Clear();
        }
        //デバック用
        for (int i = 0; i < EachPicture.Length; i++)
        {
            for (int j = 0; j < 1 + i; j++)
            {
                EachPicture[i].Add(new PictureScore(j.ToString(), 100 * j));

            }

        }

    }
}
