using PixelCrushers.QuestMachine;
using UnityEngine;

public class CollectBag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("itemForTransfer"))
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("questGiver"))
        {
            Destroy(gameObject);
        }

    }
}
