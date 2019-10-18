using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleButtonManager : MonoBehaviour
{
    Animator animator;
    bool isButtonEnable = true;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enabled = false;
    }

    public void ChangeButtonEnable()
    {
        isButtonEnable = !isButtonEnable;
    }
    public void ChangePanel()
    {
        if (isButtonEnable)
        {
            animator.SetTrigger("Change");
        }
    }
    public void ExitGame()
    {
        if (isButtonEnable)
        {
            Application.Quit();
        }
    }

    public void StartGame(int playerNum)
    {
        if (isButtonEnable)
        {
            GameInstance.Instance.PlayerNum = playerNum;
            SceneManager.LoadScene("Tutorial");
        }
    }
}
