using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private NPCDialogue dialogueToShow;
    [SerializeField] private GameObject interactionBox;

    public NPCDialogue DialogueToShow => dialogueToShow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            DialogueManager.Instance.NPCSelected = this;
            interactionBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager dm = DialogueManager.Instance;
            // Check if the manager or the panel is null
            if (dm != null)
            {
                dm.NPCSelected = null;
                dm.CloseDialoguePanel(); // inside that method, also check if panel != null
            }
            interactionBox.SetActive(false);
        }
    }
}
