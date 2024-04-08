using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class StartCutSceneManager : MonoBehaviour
{
    VideoPlayer videoPlayer;
    public static UnityEvent startGameEvent = new UnityEvent();

    private void Start()
    {
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        videoPlayer.Play();
    }

    private void Update()
    {
        if (videoPlayer.isPaused)
        {
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        startGameEvent.Invoke();
    }


}
