using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHolder : MonoBehaviour
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


        foreach(Quest quest in quests)
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

}
