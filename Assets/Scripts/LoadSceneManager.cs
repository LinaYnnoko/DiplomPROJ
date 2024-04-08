using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] GameObject loadScreen;
    [SerializeField] Slider loadBar;

    public void LoadScene(int index)
    {
        loadScreen.SetActive(true);
        Time.timeScale = 1f;
        StartCoroutine(AsyncLoad(index));
    }

    public void ExitApplication()
    {
        Application.Quit();
    }


    IEnumerator AsyncLoad(int sceneToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!asyncLoad.isDone)
        {
            loadBar.value = asyncLoad.progress;
            yield return null;
        }
    }
}
