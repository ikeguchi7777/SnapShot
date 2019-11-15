using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Message : MonoBehaviour
{
    KeyNameList keyname;

    //　メッセージUI
    private Text messageText;
    //　表示するメッセージ
    [SerializeField]
    [TextArea(1, 20)]
    private string allMessage = "";
    //　使用する分割文字列
    [SerializeField]
    private string splitString = "<>";
    //　分割したメッセージ
    private string[] splitMessage;
    //　分割したメッセージの何番目か
    private int messageNum;
    //　テキストスピード
    [SerializeField]
    private float textSpeed = 0.1f;
    //　経過時間
    private float elapsedTime = 0f;
    //　今見ている文字番号
    private int nowTextNum = 0;
    //　マウスクリックを促すアイコン
    private Image clickIcon1, clickIcon2;
    //　クリックアイコンの点滅秒数
    [SerializeField]
    private float clickFlashTime = 0.2f;
    public bool IconFlip { get; set; } = true;
    //　1回分のメッセージを表示したかどうか
    public bool isOneMessage { get; private set; } = false;
    //　メッセージをすべて表示したかどうか
    private bool isEndMessage = false;

    private bool next = false;

    CanvasGroup canvasGroup;

    void Start()
    {
        clickIcon1 = transform.Find("Panel/Icon1").GetComponent<Image>();
        clickIcon1.enabled = true;
        clickIcon2 = transform.Find("Panel/Icon2").GetComponent<Image>();
        clickIcon2.enabled = false;
        messageText = GetComponentInChildren<Text>();
        messageText.text = "";
        SetMessage(allMessage);
        keyname = new KeyNameList(0);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        //　メッセージが終わっているか、メッセージがない場合はこれ以降何もしない
        if (isEndMessage || allMessage == null)
        {
            return;
        }

        //　1回に表示するメッセージを表示していない	
        if (!isOneMessage)
        {
            //　テキスト表示時間を経過したらメッセージを追加
            if (elapsedTime >= textSpeed)
            {
                messageText.text += splitMessage[messageNum][nowTextNum];

                nowTextNum++;
                elapsedTime = 0f;

                //　メッセージを全部表示、または行数が最大数表示された
                if (nowTextNum >= splitMessage[messageNum].Length)
                {
                    isOneMessage = true;
                }
            }
            elapsedTime += Time.deltaTime;

            //　メッセージ表示中にSnap押したら一括表示
            /*if ()
            {
                //　ここまでに表示しているテキストに残りのメッセージを足す
                messageText.text += splitMessage[messageNum].Substring(nowTextNum);
                isOneMessage = true;
                next = false;
            }*/
            //　1回に表示するメッセージを表示した
        }
        else
        {

            elapsedTime += Time.deltaTime;

            //　クリックアイコンを点滅する時間を超えた時、反転させる
            if (elapsedTime >= clickFlashTime && IconFlip)
            {
                clickIcon1.enabled = !clickIcon1.enabled;
                clickIcon2.enabled = !clickIcon2.enabled;
                elapsedTime = 0f;
            }

            if (canvasGroup.alpha>0.7f)
            {
                canvasGroup.alpha -= 0.3f * Time.deltaTime /2;
            }
            

            if (next)
            {
                nowTextNum = 0;
                messageNum++;
                messageText.text = "";
                clickIcon1.enabled = true;
                clickIcon2.enabled = false;
                IconFlip = true;                
                elapsedTime = 0f;
                isOneMessage = false;
                next = false;
                canvasGroup.alpha = 1;
                //　メッセージが全部表示されていたらゲームオブジェクト自体の削除
                if (messageNum >= splitMessage.Length)
                {
                    isEndMessage = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }
    //　新しいメッセージを設定
    void SetMessage(string message)
    {
        this.allMessage = message;
        //　分割文字列で一回に表示するメッセージを分割する
        splitMessage = Regex.Split(allMessage, @"\s*" + splitString + @"\s*", RegexOptions.IgnorePatternWhitespace);
        nowTextNum = 0;
        messageNum = 0;
        messageText.text = "";
        isOneMessage = false;
        isEndMessage = false;
    }
    //　他のスクリプトから新しいメッセージを設定しUIをアクティブにする
    public void SetMessagePanel(string message)
    {
        SetMessage(message);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Next() {
        next = true;
        

    }
}