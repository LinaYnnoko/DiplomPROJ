using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] int timeToClose = 5;

    public NotificationScriptableObject startNotify;
    public GameObject notificationPanel;
    public TMP_Text notifyText;

    GameObject obj;

    private void Awake()
    {
        StartCutSceneManager.startGameEvent.AddListener(GameStart);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        notificationPanel.SetActive(true);
        notifyText.text = startNotify.agreeNotify;
        StartCoroutine(ClosePanel());

    }

    public void GameStart()
    {
        gameObject.SetActive(true);
    }

    public void Notify(GameObject collideObj, bool agree)
    {
        obj = collideObj;
        if(obj.GetComponent<Item>() == true)
        {
            if(agree)
            {
                Item item = obj.GetComponent<Item>();
                notificationPanel.SetActive(true);
                notifyText.text = item.notifyScriptableObject.agreeNotify;
                StartCoroutine(ClosePanel());
            }
            else
            {
                Item item = obj.GetComponent<Item>();
                notificationPanel.SetActive(true);
                notifyText.text = item.notifyScriptableObject.discardNotify;
                StartCoroutine(ClosePanel());
            }


        }
    }

    IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(timeToClose);
        notificationPanel.SetActive(false);
    }

}
