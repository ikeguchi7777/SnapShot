using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultController : MonoBehaviour
{


    ImageManager imageManager = new ImageManager();

    List<GameObject>[] images = new List<GameObject>[4];

    int imageNumber = 0;
    const int IMAGE_X = 5, IMAGE_Y = 5;
    const float REDUCTION_RATE = 1 / 4f;

    float waitTime = 2.5f;

    [SerializeField] Font font;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i] = new List<GameObject>();
        }

        StartCoroutine(Loop());

    }

    // Update is called once per frame
    void Update()
    {

        

    }

    IEnumerator Loop() {

        for (int i = 0; i < 7; i++)
        {
            imageNumber++;
            Disp();
            yield return new WaitForSeconds(waitTime);

        }
    }

    void Disp() {

        

        for (int i = 0; i < images.Length; i++)
        {
            if (imageManager.GetEachImageNum()[i] >= imageNumber)
            {
                images[i].Add(imageManager.LoadImage(i + 1, imageNumber));

                GameObject txt = new GameObject((i + 1) + "P_" + imageNumber + "point");

                txt.transform.parent = GameObject.Find("Canvas/" + (i + 1) + "PPanel/" + (i + 1) + "P_" + imageNumber).transform;
                txt.AddComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                txt.GetComponent<RectTransform>().sizeDelta = new Vector3(1000, 600, 1);
                txt.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                txt.AddComponent<Text>().font = font;
                txt.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                txt.GetComponent<Text>().fontSize = 250;
                txt.GetComponent<Text>().text = "1234点";
                txt.GetComponent<Text>().color = new Color(0, 0, 0, 0);
                //txt.GetComponent<Text>().resizeTextForBestFit = true;


                Move(images[i][imageNumber - 1], txt, ((imageNumber - 1) % IMAGE_X) * 380 + (-760), (int)((imageNumber - 1) / IMAGE_X) * (-200) + 400, 1);

            }
        }

    }

    void Move(GameObject obj,GameObject txt, float destX, float destY, float time)
    {



        Sequence seq = DOTween.Sequence();
        seq.Append(obj.transform.DOScale(new Vector2(1.5f, 1.5f), 1f));
        seq.Append(txt.GetComponent<Text>().DOFade(1,0.5f));
        seq.AppendInterval(0.5f);
        seq.Append(obj.transform.DOScale(new Vector2(REDUCTION_RATE, REDUCTION_RATE), time));
        seq.Join(obj.transform.DOLocalMove(new Vector2(destX, destY), time));

       // seq.OnComplete(() => {  });

    }

}
