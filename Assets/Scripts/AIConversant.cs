using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AIConversant : MonoBehaviour
{
    [SerializeField] GameObject dialogueIcon;
    [SerializeField] string conversantName;
    //[SerializeField] TextMeshProUGUI textUI;

    //[SerializeField] string text;
    [SerializeField] Dialogue dialogue;

    bool playerInRange = false;
    private bool isConversable = true;

    private void Awake()
    {
        dialogueIcon.SetActive(false);
    }

    public string GetConversantName()
    {
        return conversantName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isConversable)
        {
            dialogueIcon.SetActive(true);
            PlayerConversant player = collision.GetComponent<PlayerConversant>();
            player.SetConversant(dialogue, this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerConversant player = collision.GetComponent<PlayerConversant>();
            dialogueIcon.SetActive(false);
            if (player.GetConversant() == this && !player.GetIsInDialogue())
            {
                player.SetConversant(null, null);
            }
        }
    }

    public void SetIsConversable()
    {
        isConversable = false;
    }

    internal void SetDialogueIcon(bool v)
    {
        dialogueIcon.SetActive(v);
    }
}
