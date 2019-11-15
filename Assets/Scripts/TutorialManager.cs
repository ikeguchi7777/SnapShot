using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TutorialManager : GameManager
{


    [SerializeField]
    GameObject checkarea;
    [SerializeField]
    GameObject chargespot;
    [SerializeField]
    GameObject speedup;
    [SerializeField]
    GameObject transparency;
    [SerializeField]
    GameObject findplayer;
    [SerializeField]
    GameObject warp;
    [SerializeField]
    GameObject barrier;
    [SerializeField]
    Message message;


    KeyNameList[] keynamelist = new KeyNameList[GameInstance.Instance.PlayerNum];
    //KeyNameList[] keynamelist = new KeyNameList[1];//デバッグ用

    int phaseNum = 1;
    bool tmp;

    bool[] checks = new bool[GameInstance.Instance.PlayerNum];

    float time = 0;

    public enum Phase
    {
        intro = 1,
        move,
        water,
        view,
        jump,
        aim,
        snap,
        charge,
        speedup,
        transparency,
        finplayer,
        warp,
        end,
        start,

    }

    void Start()
    {
        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            keynamelist[i] = new KeyNameList(i);
            checks[i] = false;
        }


        message.SetMessagePanel("始めにチュートリアルを行います。<>" +
            "まずは左アナログパッドを動かして光っているエリアまで移動してください。" +
            "LBボタンでダッシュすることもできます。<>" +
            "また水の中では走ることができません。<>" +
            "右アナログパッドで視点を操作することができます。<>" +
            "Aボタンを押すことでジャンプすることができます。<>" +
            "次はRBボタンを押して、カメラを構えましょう。" +
            "緑のゲージがカメラの電池残量で、構えている間は減り続けます。<>" +
            "そのままBボタンを押すことで写真を撮ることができます。<>" +
            "チャージスポットに入っている間はカメラを充電することができます。" +
            "電池がないと写真が撮れないので気を付けよう。<>" +
            "ダッシュが速くなるアイテム「カソク」、<>" +
            "透明になれるアイテム「トウメイ」、<>" +
            "他のプレイヤーの場所がわかるアイテム「サーチ」、<>" +
            "ワープすることができるアイテム「ワープ」があります。<>" +
            "アイテムを利用して他のプレイヤーを大きく正面から撮って高得点を目指そう!<>" +
            "ボタンを押してゲームを開始します。<>");

    }

    protected override void Update()
    {

        base.Update();
        if (Input.GetKeyDown(KeyCode.D))//デバッグ用
        {
            Debug.Log(phaseNum);
            for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
            {

                // Debug.Log(Players[i].collision.transform.tag);
            }
        }

        if (Input.GetButtonDown("Skip"))
        {
            SceneManager.LoadScene("SampleScene");
        }

        switch (phaseNum)
        {
            case (int)Phase.intro:
                {
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
                            NextPhase();
                        }
                    }
                }
                break;

            case (int)Phase.move:
                {
                    if (message.IconFlip != false)
                    {
                        message.IconFlip = false;
                    }

                    if (time >= 0 && time < 3)
                    {
                        time += Time.deltaTime;
                    }
                    else if (time >= 3)
                    {
                        if (barrier.activeSelf == true)
                        {
                            barrier.SetActive(false);
                        }

                        Instantiate(checkarea, new Vector3(0, -2.95f, 15), Quaternion.identity);
                        time = -1;
                    }
                }
                break;

            case (int)Phase.water:
                {
                    if (message.IconFlip != false)
                    {
                        message.IconFlip = false;
                    }

                    time += Time.deltaTime;

                    if (time > 4)
                    {
                        if (message.isOneMessage)
                        {
                            NextPhase();
                        }
                    }
                }
                break;


            case (int)Phase.view:
                {
                    if (message.IconFlip != false)
                    {
                        message.IconFlip = false;
                    }

                    time += Time.deltaTime;

                    if (time > 7)
                    {
                        if (message.isOneMessage)
                        {
                            NextPhase();
                        }
                    }
                }
                break;

            case (int)Phase.jump:
                {
                    for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                    {
                        if (Input.GetButtonDown(keynamelist[i].Jump))
                        //if (Input.GetButtonDown(keynamelist[0].Jump))//デバッグ
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
                            NextPhase();
                        }
                    }
                }
                break;

            case (int)Phase.aim:
                {
                    for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                    {
                        if (Input.GetButtonDown(keynamelist[i].AimCamera))
                        //if (Input.GetButtonDown(keynamelist[0].AimCamera))//デバッグ
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
                            NextPhase();
                        }
                    }
                }
                break;

            case (int)Phase.snap:
                {
                    for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
                    {
                        if (Input.GetButton(keynamelist[i].AimCamera) && Input.GetButtonDown(keynamelist[i].Snap))
                        //if (Input.GetButton(keynamelist[0].AimCamera) && Input.GetButtonDown(keynamelist[0].Snap))//デバッグ
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
                            Instantiate(chargespot, new Vector3(-9, -2, 9), Quaternion.identity);
                            Instantiate(chargespot, new Vector3(-9, -2, -9), Quaternion.identity);
                            Instantiate(chargespot, new Vector3(9, -2, 9), Quaternion.identity);
                            Instantiate(chargespot, new Vector3(9, -2, -9), Quaternion.identity);

                            NextPhase();
                        }
                    }
                }
                break;

            case (int)Phase.charge:
                {
                    if (message.IconFlip != false)
                    {
                        message.IconFlip = false;
                    }

                    time += Time.deltaTime;

                    if (time > 15)
                    {
                        if (message.isOneMessage)
                        {
                            Instantiate(speedup, new Vector3(-3, -2, 9), Quaternion.identity);                            
                            Instantiate(speedup, new Vector3(3, -2, -9), Quaternion.identity);

                            Instantiate(findplayer, new Vector3(-9, -2, -3), Quaternion.identity);
                            Instantiate(findplayer, new Vector3(9, -2, 3), Quaternion.identity);

                            Instantiate(transparency, new Vector3(3, -2, 9), Quaternion.identity);                            
                            Instantiate(transparency, new Vector3(-3, -2, -9), Quaternion.identity);

                            Instantiate(warp,new Vector3(9, -2, -3), Quaternion.identity);
                            Instantiate(warp, new Vector3(-9, -2, 3), Quaternion.identity);

                            NextPhase();
                        }
                    }
                }
                break;

            case (int)Phase.speedup:
                {
                    if (message.IconFlip != false)
                    {
                        message.IconFlip = false;
                    }

                    time += Time.deltaTime;

                    if (time > 5)
                    {
                        if (message.isOneMessage)
                        {
                            NextPhase();
                        }
                    }
                }
                break;

            case (int)Phase.transparency:
                {
                    if (message.IconFlip != false)
                    {
                        message.IconFlip = false;
                    }

                    time += Time.deltaTime;

                    if (time > 10)
                    {
                        if (message.isOneMessage)
                        {
                            NextPhase();
                        }
                    }
                }
                break;

            case (int)Phase.finplayer:
                {
                    if (message.IconFlip != false)
                    {
                        message.IconFlip = false;
                    }

                    time += Time.deltaTime;

                    if (time > 5)
                    {
                        if (message.isOneMessage)
                        {
                            NextPhase();
                        }
                    }
                }
                break;

            case (int)Phase.warp:
                {
                    if (message.IconFlip != false)
                    {
                        message.IconFlip = false;
                    }

                    time += Time.deltaTime;

                    if (time > 5)
                    {
                        if (message.isOneMessage)
                        {
                            NextPhase();
                        }
                    }
                }
                break;

            case (int)Phase.end:
                {
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
                            NextPhase();
                        }
                    }
                }
                break;

            case (int)Phase.start:
                {
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
                            SceneManager.LoadScene("SampleScene");
                        }
                    }
                }
                break;

            default:
         
                break;
        }



    }

    public void NextPhase()
    {

        message.Next();
        phaseNum++;
        time = 0;

        for (int i = 0; i < GameInstance.Instance.PlayerNum; i++)
        {
            checks[i] = false;
        }
        
    }

}
