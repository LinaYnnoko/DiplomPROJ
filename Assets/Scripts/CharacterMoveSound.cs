using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip walkingSound;

    public void FootStep()
    {
        audioSource.PlayOneShot(walkingSound);
    }
}
