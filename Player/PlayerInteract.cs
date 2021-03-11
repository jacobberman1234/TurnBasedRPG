using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public bool InRange;
    public IInteractable Interactable;

    void Update()
    {
        Interact();
    }

    void Interact()
    {
        if (InRange)
            if (Input.GetKeyDown(KeyCode.E))
                Interactable.Interact();
    }
}
