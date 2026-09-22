using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestCardNPC : QuestCard
{
    [SerializeField] private TextMeshProUGUI questRewardTMP;

    public override void ConfigQuestUI(Quest quest)
    {
        base.ConfigQuestUI(quest);

        string rewardText = string.Empty;

        // Check if gold reward is greater than 0 before displaying it
        if (quest.GoldReward > 0)
            rewardText += $"- {quest.GoldReward} Gold\n";

        // Check if experience reward is greater than 0 before displaying it
        if (quest.ExpReward > 0)
            rewardText += $"- {quest.ExpReward} Exp\n";

        // Always display the item reward if it exists
        if (quest.ItemReward != null && quest.ItemReward.Item != null)
            rewardText += $"- x{quest.ItemReward.Quantity} {quest.ItemReward.Item.Name}";

        questRewardTMP.text = rewardText;
    }

    public void AcceptQuest()
    {
        if(QuestToComplete == null) return;
        QuestToComplete.QuestAccepted = true;
        QuestManager.Instance.AcceptQuest(QuestToComplete);
        gameObject.SetActive(false);
    }
}
