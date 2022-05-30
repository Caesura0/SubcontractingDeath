using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHolder : MonoBehaviour, IPredicateEvaluator
{
    List<Quest> quests = new List<Quest>();

    public class Quest
    {
        public Quest(string questName, bool hasObjective, bool completed)
        {
            this.questName = questName;
            this.hasObjective = hasObjective;
            this.completed = completed;
        }
        public string questName;
        public bool completed;
        public bool hasObjective;
    }

    public Quest GetQuest(string questName)
    {

        if (quests == null) return null;
        foreach (Quest quest in quests)
        {
            if(quest.questName == questName)
            {
                return quest;
            }
        }
        return null;
    }

    public void AddQuest(string questName)
    {
        Quest newQuest = new Quest(questName, false, false) ;
        if (quests.Contains(newQuest)) { return; }
        quests.Add(newQuest);
        Debug.Log(newQuest.questName);
    }

    public void CollectedObjective(string questName)
    {
        Debug.Log("tried to collectedObjective quest");
        foreach (Quest quest in quests)
        {
            if (quest.questName == questName)
            {
                quest.hasObjective = true;
                Debug.Log(quest.questName + " " + quest.hasObjective);
            }
        }
    }

    public void CompleteQuest(string questName)
    {
        Debug.Log("tried to complete quest in quest holder");
        foreach (Quest quest in quests)
            {
                if (quest.questName == questName)
                {
                    quest.completed = true;
                    Debug.Log(quest.questName + " " + quest.completed);
                }
            }
    }

    public IEnumerable<Quest> GetQuests()
    {
        return quests;
    }

    public bool HasQuest(string questName)
    {
        return GetQuest(questName) != null;
    }



    public bool? Evaluate(EPredicate predicate, string[] parameters)
    {
        Debug.Log("evaluate in questholder");
        Debug.Log(parameters[0]);
        Quest quest = GetQuest(parameters[0]);
        
        switch (predicate)
        {

            case EPredicate.HasQuest:
                Debug.Log("HasQuest checked = " + HasQuest(parameters[0]));
                return HasQuest(parameters[0]);

            case EPredicate.CompletedQuest:
                if (quest != null) Debug.Log("completedQuest checked = " + (GetQuest(parameters[0]).completed));
                if (quest != null) { return (GetQuest(parameters[0]).completed == true); }
                return null;


            case EPredicate.HasObjective:
                Debug.Log("hasObjective checked");
                if (quest != null) { return (GetQuest(parameters[0]).hasObjective); }
                return null;
        }

        return null;
    }


}


