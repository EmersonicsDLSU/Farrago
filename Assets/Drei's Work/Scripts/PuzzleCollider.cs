using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCollider : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
            this.gameObject.GetComponentInParent<PuzzleInteraction>().canInteract = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
            this.gameObject.GetComponentInParent<PuzzleInteraction>().canInteract = false;
    }
}
