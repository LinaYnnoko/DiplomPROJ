using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyAnimIK : MonoBehaviour
{
    public Transform player;
    public Transform grannyHead;

    Animator animator;
    bool playerInSightRange;
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerInSightRange)
        {
            grannyHead.LookAt(player);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            animator.SetLayerWeight(0, 0f);
            playerInSightRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetLayerWeight(0, 1f);
        playerInSightRange = true;
    }
}
