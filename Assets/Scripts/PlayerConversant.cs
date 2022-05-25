using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class PlayerConversant : MonoBehaviour
{
    AIConversant currentConversant;
    Dialogue currentDialogue;
    DialogueNode currentNode;
    private bool isChoosing;

    public event Action onConversationUpdated;
    public void StartDialogue()
    {
        if(currentConversant == null) { return; }
        
        currentNode = currentDialogue.GetRootNode();
        TriggerEnterAction();
        onConversationUpdated();
    }

    public void SetConversant( Dialogue dialogue, AIConversant dialogueRangeTrigger)
    {
        currentConversant = dialogueRangeTrigger;
        currentDialogue = dialogue;
    }

    public Dialogue GetDialogue()
    {
        return currentDialogue;
    }
    public AIConversant GetConversant()
    {
        return currentConversant;
    }


    public void Next()
    {
        int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
        if (numPlayerResponses > 0)
        {
            isChoosing = true;
            TriggerExitAction();
            onConversationUpdated();
            return;
        }
        DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
        int randomIndex = UnityEngine.Random.Range(0, children.Count());
        TriggerExitAction();

        //If any weird behavior with quests triggers happens, look here first. Mega SUS.
        if (children.Length > 0)
        {
            currentNode = children[randomIndex];
            TriggerEnterAction();
            onConversationUpdated();
        }
    }

    void TriggerEnterAction()
    {
        if (currentNode != null)
        {
            TriggerAction(currentNode.GetOnEnterAction());
        }
    }



    void TriggerExitAction()
    {
        if (currentNode != null)
        {
            TriggerAction(currentNode.GetOnExitAction());
        }
    }


    private void TriggerAction(string action)
    {
        if (action == "") { return; }
        Debug.Log("looking for dialogueTrigger");
        foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
        {
            Debug.Log("found dialogueTrigger");
            trigger.Trigger(action);
        }
    }


    public void Quit()
    {
        currentDialogue = null;
        TriggerExitAction();
        currentNode = null;
        isChoosing = false;
        currentConversant = null;
        onConversationUpdated();
    }

    public bool IsActive()
    {
        return currentDialogue != null;
    }

    public bool IsChoosing()
    {
        return isChoosing;
    }
    public string GetText()
    {
        if (currentNode == null)
        {
            return "";
        }
        return currentNode.GetNodeText();
    }

    internal string GetCurrentConversantName()
    {
        if (isChoosing)
        {
            return "Angel";
        }
        else
        {
            return currentConversant.GetConversantName();
        }
    }

    public IEnumerable<DialogueNode> GetChoices()
    {
        return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
        //here?
    }
    public void SelectChoice(DialogueNode chosenNode)
    {
        currentNode = chosenNode;
        TriggerEnterAction();
        isChoosing = false;
        Next();
    }

    private IEnumerable<IPredicateEvaluator> GetEvaluators()
    {
        return GetComponents<IPredicateEvaluator>();
    }

    public bool HasNext()
    {
        //returns false if there are no children
        return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0;
    }

    private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
    {
        foreach (var node in inputNode)
        {
            if (node.CheckCondition(GetEvaluators()))
            {
                yield return node;
            }
        }
    }
}
