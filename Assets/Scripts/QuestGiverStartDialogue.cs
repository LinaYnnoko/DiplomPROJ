using PixelCrushers.QuestMachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestGiverStartDialogue : MonoBehaviour
{
    public string questGiverName;
    public GameObject startDialogueTextObject;
    QuestGiver questGiver;
    GameObject player;
    TMP_Text startDialogueText;

    private void Start()
    {
        startDialogueText = startDialogueTextObject.GetComponent<TMP_Text>();
        questGiver = GetComponent<QuestGiver>();
        startDialogueText.text = questGiverName + "\nначать диалог 'E'";
    }

    private void Update()
    {
        if (player != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                questGiver.StartDialogueWithPlayer();
            }
            if(Time.timeScale == 1)
            {
                startDialogueTextObject.SetActive(true);
            }
            else
            {
                startDialogueTextObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            startDialogueTextObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player = null;
        startDialogueTextObject.SetActive(false);

    }
}
