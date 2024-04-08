using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OpenedDoor : MonoBehaviour
{
    [Header("Fader settings")]
    [SerializeField] float fadeTime = .8f; //время необходимое для затемнения экрана
    [SerializeField] float unFadeTime = .8f; //время необходимое для снятия затемнения
    [SerializeField] float faderTimer = 2f; //время задержки между переходами

    [Header("Objects")]
    [SerializeField] Transform point;
    [SerializeField] Image fader;
    [SerializeField] Animator animator;

    AudioSource sound;

    GameObject player;
    Color color;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
        color = fader.color;
    }


    public void Open()
    {
        animator.SetTrigger("DoorOpened");
        sound.Play();
        StartCoroutine(Fade());
        Invoke(nameof(DoorIsOpen), 2f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerAnimation>())
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerAnimation>())
        {
            player = null;
        }
    }

    void DoorIsOpen()
    {
        player.transform.position = point.position;
    }


    IEnumerator Fade()
    {
        while (color.a < 1)
        {
            color.a += fadeTime * Time.deltaTime;
            fader.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(faderTimer);

        while (color.a > 0)
        {
            color.a -= unFadeTime * Time.deltaTime;
            fader.color = color;
            yield return null;

        }
    }
}
