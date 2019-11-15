using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CountDown : MonoBehaviour
{



    float Timer;

    [SerializeField]
    MainGameManager mainGameManager;

    SoundController soundController;

    [SerializeField]
    GameObject[] count;
    float[] timepoint = {300,240,180,120, 60, 30, 10,  5,  4,  3,  2,  1,  0, -100};
    //float[] timepoint = {300,295,290,285,280,275,270,269,268,267,265,264,263, -100};
  
    [SerializeField]
    int flag = 0;

    // Start is called before the first frame update
    void Start()
    {
        soundController = mainGameManager.GetComponent<SoundController>();
    }

    // Update is called once per frame
    public void Update()
    {

        for (int i = 0; i < timepoint.Length ; i++)
        {
            if (mainGameManager.GetTimer() < timepoint[i] && mainGameManager.GetTimer() > timepoint[i+1])
            {

            
                if (flag == i)
                {
                  
                    if (flag <= 6)
                    {

                        Sequence seq = DOTween.Sequence();
                        seq.Append(count[i].GetComponent<Transform>().DOScale(new Vector2(0.5f, 0.5f), 0f));
                        seq.Append(count[i].GetComponent<CanvasGroup>().DOFade(1, 1.0f));
                        seq.Join(count[i].GetComponent<Transform>().DOScale(new Vector2(1f, 1f), 1.0f));
                        seq.AppendInterval(1f);
                        seq.Append(count[i].GetComponent<CanvasGroup>().DOFade(0, 1.0f));
                        seq.Join(count[i].GetComponent<Transform>().DOScale(new Vector2(0.1f, 0.1f), 1.0f));

                        
                    }
                    else if (flag <= 11)
                    {

                        Sequence seq = DOTween.Sequence();
                        seq.Append(count[i].GetComponent<Transform>().DOScale(new Vector2(0.5f, 0.5f), 0f));
                        seq.Append(count[i].GetComponent<CanvasGroup>().DOFade(1, 0.2f));
                        seq.Join(count[i].GetComponent<Transform>().DOScale(new Vector2(2f, 2f), 0.2f));
                        seq.AppendInterval(0.6f);
                        seq.Append(count[i].GetComponent<CanvasGroup>().DOFade(0, 0.2f));
                        seq.Join(count[i].GetComponent<Transform>().DOScale(new Vector2(0.1f, 0.1f), 0.2f));

                     
                    }
                    else if (flag == 12)
                    {
                        soundController.PlaySE(SoundController.Sound.whistle);

                        Sequence seq = DOTween.Sequence();
                        seq.Append(count[i].GetComponent<Transform>().DOScale(new Vector2(0.5f, 0.5f), 0f));
                        seq.Append(count[i].GetComponent<CanvasGroup>().DOFade(1, 1.0f));
                        seq.Join(count[i].GetComponent<Transform>().DOScale(new Vector2(2f, 2f), 1.0f));
                        seq.AppendInterval(3f);
                        seq.Append(count[i].GetComponent<CanvasGroup>().DOFade(0, 1.0f));
                        seq.Join(count[i].GetComponent<Transform>().DOScale(new Vector2(0.1f, 0.1f), 0.2f));
                        seq.AppendInterval(3f);
                        seq.OnComplete(() => SceneManager.LoadScene("Result"));

                    }

                    flag++;
                }
            }
        }




    }
}
