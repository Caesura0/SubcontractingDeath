using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCompletion : MonoBehaviour
{
    [SerializeField] string questName;
    //[SerializeField] string objective;

    public void CompleteObjective()
    {
        Debug.Log("tried to complete Objective");
        QuestHolder questHolder = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestHolder>();

        questHolder.CollectedObjective(questName);
        
    }    
    public void CompleteQuest()
    {
        Debug.Log("tried to complete quest");
        QuestHolder questHolder = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestHolder>();

        questHolder.CompleteQuest(questName);
    }
}

