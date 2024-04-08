using UnityEngine;
using PixelCrushers;
using PixelCrushers.QuestMachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject escapePanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject wonPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject dialogueTextObject;
    bool isOpenedPause = false;
    bool isOpenedJournal = false;
    bool endPanelIsOpened = false;

    private void Awake()
    {
        StartCutSceneManager.startGameEvent.AddListener(GameStart);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            isOpenedJournal = true;
            QuestMachine.GetQuestJournal().ShowJournalUI();
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isOpenedJournal && !settingsPanel.activeSelf)
        {
            isOpenedPause = !isOpenedPause;
            if (!isOpenedPause)
            {
                Cursor.lockState = CursorLockMode.Locked;
                escapePanel.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                escapePanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        if(wonPanel.activeSelf || losePanel.activeSelf && !endPanelIsOpened)
        {
            SetTime(0);
            endPanelIsOpened = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void GameStart()
    {
        gameObject.SetActive(true);
    }

    public void WonGame()
    {
        wonPanel.SetActive(true);
    }

    public void LoseGame()
    {
        losePanel.SetActive(true);
    }

    public void SetTime(int time)
    {
        Time.timeScale = time;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void JournalClosed()
    {
        isOpenedJournal = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void PauseMenuClosed()
    {
        isOpenedPause = false;
        Cursor.lockState = CursorLockMode.Locked;
        escapePanel.SetActive(false);
        Time.timeScale = 1f;
    }

}
