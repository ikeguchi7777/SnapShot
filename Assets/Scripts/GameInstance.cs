using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance
{

    public List<PictureScore>[] EachPicture = new List<PictureScore>[4];

    public int PlayerNum;                   //プレイヤーの人数1～4

    public Sprite camtest;

    public Queue<Sprite>[] photos;

    public Queue<int>[] scores;
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
        photos = new Queue<Sprite>[4];
        scores = new Queue<int>[4];
        for (int i = 0; i < photos.Length; i++)
        {
            photos[i] = new Queue<Sprite>();
            scores[i] = new Queue<int>();
        }
        PlayerNum = 4;
        /*foreach (var item in EachPicture)
        {
            item.Clear();
        }*/
            
    }
}
