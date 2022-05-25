using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestGiver : MonoBehaviour
{
    
    [SerializeField] string questName;


    public void GiveQuest()
    {
        QuestHolder questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestHolder>();
        questList.AddQuest(questName);
    }
}
