﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultController : MonoBehaviour
{

    [SerializeField] GameObject[] ScoreBoards;

    ImageManager imageManager = new ImageManager();

    List<GameObject>[] images = new List<GameObject>[4];

    int imageNumber = 0;
    int imageMAX = 0;
    const int IMAGE_X = 5, IMAGE_Y = 5;
    const float REDUCTION_RATE = 1 / 3f;

    float waitTime = 2.5f;

    [SerializeField] Font font;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            images[i] = new List<GameObject>();
            GameInstance.Instance.TotalScore[i] = GetTotalScore(i + 1);
                 
           
            GameObject point = ScoreBoards[i].transform.Find("Score").gameObject;
            point.GetComponent<Text>().text = GameInstance.Instance.TotalScore[i] + "点";

            imageMAX = Mathf.Max(imageMAX, GameInstance.Instance.EachPicture[i].Count);

        }

        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            GameInstance.Instance.Ranking[i] = GetRanking(i + 1);

            GameObject rank = ScoreBoards[i].transform.Find("Rank").gameObject;
            rank.GetComponent<Text>().text = GameInstance.Instance.Ranking[i] + "位";

        }

        StartCoroutine(Loop());

    }

    // Update is called once per frame
    void Update()
    {
        if (imageMAX + 1 == imageNumber)
        {
            TotalDisp();
            imageNumber = 0;
        }


    }

    IEnumerator Loop()
    {

        for (int i = 0; i < imageMAX + 1; i++)
        {

            imageNumber++;
            PictureDisp();
            yield return new WaitForSeconds(waitTime);

        }
    }

    void PictureDisp()
    {



        for (int i = 0; i < images.Length; i++)
        {
            if (GameInstance.Instance.EachPicture[i].Count >= imageNumber)
            {
                images[i].Add(imageManager.LoadImage(i + 1, imageNumber));

                GameObject txt = new GameObject((i + 1) + "P_" + imageNumber + "point");

                txt.transform.parent = GameObject.Find("Canvas/" + (i + 1) + "PPanel/" + (i + 1) + "P_" + imageNumber).transform;
                txt.AddComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                txt.GetComponent<RectTransform>().sizeDelta = new Vector3(1000, 600, 1);
                txt.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1);
                txt.AddComponent<Text>().font = font;
                txt.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                txt.GetComponent<Text>().fontSize = 250;

                txt.GetComponent<Text>().text = GameInstance.Instance.EachPicture[i][imageNumber - 1].point + "点";

                txt.GetComponent<Text>().color = new Color(0, 0, 0, 0);
                //txt.GetComponent<Text>().resizeTextForBestFit = true;


                Move(images[i][imageNumber - 1], txt, ((imageNumber - 1) % IMAGE_X) * 380 + (-760), (int)((imageNumber - 1) / IMAGE_X) * (-200) + 400, 1);

            }
        }

    }

    void Move(GameObject obj, GameObject txt, float destX, float destY, float time)
    {



        Sequence seq = DOTween.Sequence();
        seq.Append(obj.transform.DOScale(new Vector2(1.5f, 1.5f), 1f));
        seq.Append(txt.GetComponent<Text>().DOFade(1, 0.5f));
        seq.AppendInterval(0.5f);
        seq.Append(obj.transform.DOScale(new Vector2(REDUCTION_RATE, REDUCTION_RATE), time));
        seq.Join(obj.transform.DOLocalMove(new Vector2(destX, destY), time));

        // seq.OnComplete(() => {  });

    }

    int GetTotalScore(int playerNum)
    {

        int score = 0;

        foreach (var item in GameInstance.Instance.EachPicture[playerNum - 1])
        {
            score += item.point;
        }
        return score;
    }

    int GetRanking(int playeyNum)
    {
        //Debug.Log(GameInstance.Instance.TotalScore[playeyNum - 1]);
        int score = GameInstance.Instance.TotalScore[playeyNum - 1];

        int rank = 1;

        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            //Debug.Log(score +" "+ GameInstance.Instance.TotalScore[i]);
            if (score < GameInstance.Instance.TotalScore[i])
            {
                rank++;
                
            }
        }
        return rank;
    }

    void TotalDisp()
    {
        

        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            ScoreBoards[i].transform.SetAsLastSibling();

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(0.5f);
            seq.Append(ScoreBoards[i].GetComponent<CanvasGroup>().DOFade(1, 1.0f));
            seq.AppendInterval(2.0f);
            seq.Append(ScoreBoards[i].transform.Find("Total").gameObject.GetComponent<Text>().DOFade(0,1.0f));
            seq.Join(ScoreBoards[i].transform.Find("Score").gameObject.GetComponent<Text>().DOFade(0, 1.0f));
            seq.AppendInterval(0.5f);
            seq.Append(ScoreBoards[i].transform.Find("Rank").gameObject.GetComponent<Text>().DOFade(1, 1.0f));
            seq.AppendInterval(2.0f);
            seq.Append(ScoreBoards[i].GetComponent<CanvasGroup>().DOFade(0, 1.0f));

        }
    }
}
