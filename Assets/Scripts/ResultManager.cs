using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{

    [SerializeField] GameObject[] ScoreBoards;
    [SerializeField] GameObject AnnouncePanel;
    [SerializeField] Sprite BackGroundPanel;

    ImageManager imageManager = new ImageManager();

    List<GameObject>[] images = new List<GameObject>[GameInstance.Instance.PlayerNum];

    int imageNumber = 0;
    int imageMAX = 0;
    const int IMAGE_X = 5, IMAGE_Y = 5;
    const float REDUCTION_RATE = 1 / 3f;

    float waitTime = 2.0f;
    float speed = 1.0f;
    bool end = false;

    [SerializeField] Font font;

    KeyNameList[] keynamelist = new KeyNameList[GameInstance.Instance.PlayerNum];

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            keynamelist[i] = new KeyNameList(i);
            images[i] = new List<GameObject>();
            GameInstance.Instance.TotalScore[i] = GetTotalScore(i);


            GameObject score = ScoreBoards[i].transform.Find("Score").gameObject;
            score.GetComponent<Text>().text = GameInstance.Instance.TotalScore[i] + "点";

            imageMAX = Mathf.Max(imageMAX, GameInstance.Instance.EachPicture[i].Count);

        }

        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            GameInstance.Instance.Ranking[i] = GetRanking(i);

            GameObject rank = ScoreBoards[i].transform.Find("Rank").gameObject;
            rank.GetComponent<Text>().text = GameInstance.Instance.Ranking[i] + "位";

        }

        for (int i = 0; i < imageMAX; i++)
        {
            for (int j = 0; j < GameInstance.Instance.PlayerNum; j++)
            {

                if (GameInstance.Instance.EachPicture[j].Count > i )
                {
                    images[j].Add(imageManager.LoadImage(j, i));
                }
            }
        }


        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(AnnouncePanel.GetComponent<CanvasGroup>().DOFade(1, 1.0f));
        seq.Join(AnnouncePanel.GetComponent<Transform>().DOScale(new Vector2(3f, 3f), 1.0f));
        seq.AppendInterval(1.5f);
        seq.Append(AnnouncePanel.GetComponent<CanvasGroup>().DOFade(0, 1.0f));
        seq.Join(AnnouncePanel.GetComponent<Transform>().DOScale(new Vector2(2f, 2f), 1.0f));
        seq.OnComplete(() => StartCoroutine(Loop()));

        //StartCoroutine(Loop());

    }

    // Update is called once per frame
    void Update()
    {
        if (imageMAX  == imageNumber)
        {
            TotalDisp();
            imageNumber = 0;
        }

        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            if (Input.GetButtonDown(keynamelist[i].Snap) && end)
            {
                SceneManager.LoadScene("Title");
            }

            if (Input.GetButtonDown("Pause"+(i+1)))
            {
                speed = 20;
            }

        }

        
    }

    IEnumerator Loop()
    {

        for (int i = 0; i < imageMAX + 1; i++)
        {

            
            PictureDisp();
            yield return new WaitForSeconds(waitTime/speed);
            imageNumber++;
        }
    }

    void PictureDisp()
    {



        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            if (GameInstance.Instance.EachPicture[i].Count > imageNumber)
            {
                //images[i].Add(imageManager.LoadImage(i + 1, imageNumber));
                //images[i][imageNumber - 1].SetActive(true);

                GameObject background = new GameObject("background");

                background.transform.parent = GameObject.Find("Canvas/" + (i + 1) + "PPanel/" + (i + 1) + "P_" + imageNumber).transform;
                background.AddComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                background.GetComponent<RectTransform>().sizeDelta = new Vector3(900, 400, 1);
                background.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                background.AddComponent<Image>().sprite = BackGroundPanel;
                background.GetComponent<Image>().color = new Color(1,1,1,0);


                GameObject txt = new GameObject((i + 1) + "P_" + imageNumber  + "point");

                txt.transform.parent = GameObject.Find("Canvas/" + (i + 1) + "PPanel/" + (i + 1) + "P_" + imageNumber).transform;
                txt.AddComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                txt.GetComponent<RectTransform>().sizeDelta = new Vector3(1000, 600, 1);
                txt.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1);
                txt.AddComponent<Text>().font = font;
                txt.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                txt.GetComponent<Text>().fontSize = 250;

                txt.GetComponent<Text>().text = GameInstance.Instance.EachPicture[i][imageNumber].point + "点";

                txt.GetComponent<Text>().color = new Color(0, 0, 0, 0);

                //txt.AddComponent<Outline>().effectColor = new Color(1,1,1,1);

                //txt.GetComponent<Text>().resizeTextForBestFit = true;

                
                Move(images[i][imageNumber], txt,background, ((imageNumber) % IMAGE_X) * 380 + (-760), ((imageNumber) / IMAGE_X % IMAGE_Y) * (-200) + 400, 0.5f);

            }
        }

    }

    void Move(GameObject obj, GameObject txt, GameObject background, float destX, float destY, float movetime)
    {



        Sequence seq = DOTween.Sequence();
        seq.Append(obj.transform.DOScale(new Vector2(1.5f, 1.5f), 0.5f/speed));
        seq.Join(obj.GetComponent<Image>().DOFade(1, 0.5f/speed));
        seq.Append(txt.GetComponent<Text>().DOFade(1, 0.5f/speed));
        seq.Join(background.GetComponent<Image>().DOFade(0.5f, 0.5f / speed));
        //
        //seq.Join(txt.GetComponent<Outline>().DOColor(new Color(1, 1, 1, 1), 0.5f / speed));
        seq.AppendInterval(0.5f/speed);
        seq.Append(obj.transform.DOScale(new Vector2(REDUCTION_RATE, REDUCTION_RATE), movetime/speed));
        seq.Join(obj.transform.DOLocalMove(new Vector2(destX, destY), movetime/speed));

        // seq.OnComplete(() => {  });

    }

    int GetTotalScore(int playerNum)
    {

        int score = 0;

        foreach (var item in GameInstance.Instance.EachPicture[playerNum])
        {
            score += item.point;
        }
        return score;
    }

    int GetRanking(int playeyNum)
    {
        //Debug.Log(GameInstance.Instance.TotalScore[playeyNum]);
        int score = GameInstance.Instance.TotalScore[playeyNum];

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
            seq.Append(ScoreBoards[i].transform.Find("Total").gameObject.GetComponent<Text>().DOFade(0, 1.0f));
            seq.Join(ScoreBoards[i].transform.Find("Score").gameObject.GetComponent<Text>().DOFade(0, 1.0f));
            seq.AppendInterval(0.5f);
            seq.Append(ScoreBoards[i].transform.Find("Rank").gameObject.GetComponent<Text>().DOFade(1, 1.0f));
            seq.AppendInterval(2.0f);
            seq.Append(ScoreBoards[i].GetComponent<CanvasGroup>().DOFade(0, 1.0f));
            seq.OnComplete(() => end = true);
            
        }
    }
}
