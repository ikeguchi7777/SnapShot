using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Caution : MonoBehaviour
{
    [SerializeField] GameObject CautionPanel;

    // Start is called before the first frame update
    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(2f);
        seq.Append(CautionPanel.GetComponent<CanvasGroup>().DOFade(1, 1.0f));
        seq.AppendInterval(5f);
        seq.Append(CautionPanel.GetComponent<CanvasGroup>().DOFade(0, 1.0f));
        seq.AppendInterval(0.5f);
        seq.OnComplete(() => SceneManager.LoadScene("Title"));


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
