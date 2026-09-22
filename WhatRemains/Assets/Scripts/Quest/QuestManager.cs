using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [Header("Quests")]
    [SerializeField] private Quest[] quests;

    [Header("NPC Quest Panel")]
    [SerializeField] private QuestCardNPC questCardNPCPrefab;
    [SerializeField] private Transform npcPanelContainer;

    [Header("Player Quest Panel")]
    [SerializeField] private QuestCardPlayer questCardPlayerPrefab;
    [SerializeField] private Transform playerQuestContainer;

    private void Start()
    {
        LoadQuestsIntoNPCPanel();
    }

    public void AcceptQuest(Quest quest)
    {
        QuestCardPlayer cardPlayer = Instantiate(questCardPlayerPrefab, playerQuestContainer);
        cardPlayer.ConfigQuestUI(quest);

        if (quest.ID == "Blacksmith")
        {
            CheckQuestProgressForItem("Blacksmith", "EnchantedSword");
        }

        if (quest.ID == "GreatToadSage")
        {
            if (GameManager.IsBossDead)
            {
                AddProgress("GreatToadSage", 1);
            }
        }
    }

    public void AddProgress(string questID, int amount)
    {
        Quest questToUpdate = QuestExists(questID);
        if(questToUpdate == null) return;
        if(questToUpdate.QuestAccepted)
        {
            questToUpdate.AddProgress(amount);
        }
    }

    public void CheckQuestProgressForItem(string questID, string itemID)
    {
        int itemCount = Inventory.Instance.GetItemCurrentStock(itemID);
        if (itemCount >= 1)
        {
            AddProgress(questID, 1);
        }
    }

    private Quest QuestExists(string questID)
    {
        foreach(Quest quest in quests)
        {
            if(quest.ID == questID)
                return quest;
        }
        return null;
    }

    private void LoadQuestsIntoNPCPanel()
    {
        for(int i = 0; i < quests.Length; i++)
        {
            QuestCard npcCard = Instantiate(questCardNPCPrefab, npcPanelContainer);
            npcCard.ConfigQuestUI(quests[i]);
        }
    }

    private void OnEnable()
    {
        for(int i = 0; i < quests.Length; i++)
            quests[i].ResetQuest();
    }
}
