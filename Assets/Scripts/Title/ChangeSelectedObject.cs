using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeSelectedObject : MonoBehaviour
{
    [SerializeField]EventSystem eventSystem = default;
    [SerializeField]GameObject[] FirstSelectObj;

    private void Awake()
    {
        enabled = false;
    }
    public void ChangeNowSelect(int panelNum)
    {
        eventSystem.SetSelectedGameObject(FirstSelectObj[panelNum - 1]);
    }
}
