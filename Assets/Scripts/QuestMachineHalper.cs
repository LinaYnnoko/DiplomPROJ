using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestMachineHalper : MonoBehaviour
{
    public void TimeSet(int time)
    {
        Time.timeScale = time;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void DestroyObject(GameObject obj)
    {
        if(obj != null)
        {
            Destroy(obj);
        }
    }



}
