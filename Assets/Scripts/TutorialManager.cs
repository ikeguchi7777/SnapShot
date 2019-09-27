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


    KeyNameList[] keynamelist = new KeyNameList[GameInstance.Instance.PlayerNum];
    //KeyNameList[] keynamelist = new KeyNameList[1];//デバッグ用

    int phaseNum = 1;
    bool tmp;

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
        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            keynamelist[i] = new KeyNameList(i);
            checks[i] = false;
        }


        message.SetMessagePanel("始めにチュートリアルを行います。<>" +
            "まずは左アナログパッドを動かして光っているエリアまで移動してください。<>" +
            "右アナログパッドで視点を操作することができます。<>" +
            "1ボタンを押すことでジャンプすることができます。<>" +
            "次はR1ボタンを押して、カメラを構えましょう。<>" +
            "そのまま2ボタンを押すことで写真を撮ることができます。<>" +
            "他のプレイヤーを大きく正面から撮って、高得点を目指そう!");

    }

    void Update()
    {
        //Debug.Log(phaseNum);
        if (Input.GetKeyDown(KeyCode.D))
        {

            for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
            {

                Debug.Log(Players[i].collision.transform.tag);
            }
        }
        switch (phaseNum)
        {
            case (int)Phase.move:


                for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                {



                    if (Players[i].collision.transform.tag == "CheckArea")
                    {
                        checks[i] = true;
                    }
                    else
                    {
                        checks[i] = false;
                    }
                    // Debug.Log(checks[i]);
                }


                tmp = true;
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

                        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                        {
                            checks[i] = false;
                        }

                        Sequence viewseq = DOTween.Sequence();
                        viewseq.AppendInterval(7f);
                        viewseq.OnComplete(() =>
                        {
                            message.Next();
                            phaseNum++;
                        });
                    }
                }
                break;

            case (int)Phase.view:


                break;

            case (int)Phase.jump:

                for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                {
                    //if (Input.GetButtonDown(keynamelist[i].Jump))
                    if (Input.GetButtonDown(keynamelist[0].Jump))//デバッグ
                    {
                        checks[i] = true;
                    }

                }


                tmp = true;
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

                        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                        {
                            checks[i] = false;
                        }

                    }
                }
                break;

            case (int)Phase.aim:

                for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                {
                    //if (Input.GetButtonDown(keynamelist[i].AimCamera))
                    if (Input.GetButtonDown(keynamelist[0].AimCamera))//デバッグ
                    {
                        checks[i] = true;
                    }

                }


                tmp = true;
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

                        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                        {
                            checks[i] = false;
                        }

                    }
                }
                break;

            case (int)Phase.snap:

                for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                {
                    //if (Input.GetButton(keynamelist[i].AimCamera) && Input.GetButtonDown(keynamelist[i].Snap)))
                    if (Input.GetButton(keynamelist[0].AimCamera) && Input.GetButtonDown(keynamelist[0].Snap))//デバッグ
                    {
                        checks[i] = true;
                    }

                }


                tmp = true;
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

                        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                        {
                            checks[i] = false;
                        }

                    }
                }
                break;

            default:

                for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                {

                    if (Input.GetButtonDown(keynamelist[i].Snap))
                    {
                        checks[i] = true;
                    }

                }


                tmp = false;
                foreach (var item in checks)
                {
                    tmp |= item;
                }

                if (tmp)
                {
                    if (message.isOneMessage)
                    {
                        if (phaseNum == (int)Phase.start)
                        {
                            Sequence barrierseq = DOTween.Sequence();
                            barrierseq.AppendInterval(3f);
                            barrierseq.OnComplete(() =>
                            {
                                if (barrier.activeSelf == true)
                                {
                                    barrier.SetActive(false);
                                }

                                Instantiate(checkarea, new Vector3(0, -2.95f, 15), Quaternion.identity);
                            });
                        }


                        message.Next();
                        phaseNum++;

                        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                        {
                            checks[i] = false;
                        }
                    }
                }


                break;
        }



    }



}
