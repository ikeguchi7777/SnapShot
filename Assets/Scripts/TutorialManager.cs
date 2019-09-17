using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialManager : GameManager
{


    [SerializeField]
    GameObject checkarea;
    [SerializeField]
    GameObject barrier;
    [SerializeField]
    Message message;


    //KeyNameList[] keynamelist = new KeyNameList[GameInstance.Instance.PlayerNum] ;
    KeyNameList[] keynamelist = new KeyNameList[1];//デバッグ用

    int phaseNum = 1;

    bool[] checks = new bool[GameInstance.Instance.PlayerNum];
    


    public enum Phase
    {
        start = 1,
        move,
        view,
        jump,
        aim,
        snap,
    }

    void Start()
    {
        for (int i = 0; i < keynamelist.Length; i++)
        {
            keynamelist[i] = new KeyNameList(i);

        }


        message.SetMessagePanel("始めにチュートリアルを行います。<>" +
            "まずは左アナログパッドを動かして光っているエリアまで移動してください。<>" +
            "右アナログパッドで視点を操作することができます。<>" +
            "2ボタンを押すことでジャンプすることができます。<>" +
            "次はR1ボタンを押して、カメラを構えましょう。<>" +
            "そのまま2ボタンを押すことで写真を撮ることができます。<>" +
            "他のプレイヤーを大きく正面から撮って、高得点を目指そう!");

    }

    void Update()
    {
  
        switch (phaseNum)
        {
            case (int)Phase.move:



                /* float rangeX = checkarea.GetComponent<RectTransform>().sizeDelta.x/2;
                 float rangeZ = checkarea.GetComponent<RectTransform>().sizeDelta.y/2;
                Debug.Log(rangeX + " " + rangeZ);

                for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                {

                     float dx = Mathf.Abs((Players[i].transform.position - checkarea.transform.position).x);
                     float dz = Mathf.Abs((Players[i].transform.position - checkarea.transform.position).z);
                    Debug.Log(dx + " " + dz);

                    if (dx <= rangeX && dz <= rangeZ)
                    {
                        checks[i] = true;
                    }
                    else
                    {
                        checks[i] = false;
                    }
                    Debug.Log(checks[i]);
                }*/

                for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                {

                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        Debug.Log(Players[i].collider.transform.tag);
                    }

                    if (Players[i].collider && Players[i].collider.transform.tag == "CheckArea")
                    {
                        checks[i] = true;
                    }
                    else
                    {
                        checks[i] = false;
                    }
                    // Debug.Log(checks[i]);
                }


                bool tmp = true;
                foreach (var item in checks)
                {
                    tmp &= item;
                }

                if (tmp)
                {
                    if (message.isOneMessage)
                    {
                        message.Next();
                        phaseNum++;
                    }
                }
                break;

            case (int)Phase.aim:
                if (Input.GetButtonDown(keynamelist[0].AimCamera))
                {
                    if (message.isOneMessage)
                    {
                        message.Next();
                        phaseNum++;
                    }
                }
                break;

            case (int)Phase.snap:
                if (Input.GetButton(keynamelist[0].AimCamera)&&Input.GetButtonDown(keynamelist[0].Snap))
                {
                    if (message.isOneMessage)
                    {
                        message.Next();
                        phaseNum++;
                    }
                }
                break;

            default:
                if (Input.GetButtonDown(keynamelist[0].Snap))
                {
                    if (message.isOneMessage)
                    {
                        message.Next();
                        phaseNum++;

                        Sequence seq = DOTween.Sequence();
                        seq.AppendInterval(3f);
                        seq.OnComplete(() =>
                        {
                            if (barrier.activeSelf == true)
                            {
                                barrier.SetActive(false);
                            }

                            Instantiate(checkarea);
                        });
                    }
                }
                break;
        }



    }



}
