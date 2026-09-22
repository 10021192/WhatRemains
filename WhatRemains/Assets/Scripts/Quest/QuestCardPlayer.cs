using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestCardPlayer : QuestCard
{
    [Header("Config")]
    [SerializeField] private TextMeshProUGUI statusTMP;
    [SerializeField] private TextMeshProUGUI goldRewardTMP;
    [SerializeField] private TextMeshProUGUI expRewardTMP;

    [Header("Item")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemQuantityTMP;

    [Header("Quest Completed")]
    [SerializeField] private GameObject claimButton;
    [SerializeField] private GameObject rewardsPanel;

    private void Update()
    {
        statusTMP.text = $"Status\n{QuestToComplete.CurrentStatus}/{QuestToComplete.QuestGoal}";
    }

    public override void ConfigQuestUI(Quest quest)
    {
        base.ConfigQuestUI(quest);

        statusTMP.text = $"Status\n{quest.CurrentStatus}/{quest.QuestGoal}";
        if (quest.GoldReward > 0)
        {
            goldRewardTMP.gameObject.SetActive(true);
            goldRewardTMP.text = quest.GoldReward.ToString();
        }
        else
        {
            goldRewardTMP.gameObject.SetActive(false);
            goldRewardTMP.transform.parent.gameObject.SetActive(false);
        }

        if (quest.ExpReward > 0)
        {
            expRewardTMP.gameObject.SetActive(true);
            expRewardTMP.text = quest.ExpReward.ToString();
        }
        else
        {
            expRewardTMP.gameObject.SetActive(false);
            expRewardTMP.transform.parent.gameObject.SetActive(false);
        }

        if (quest.ItemReward != null && quest.ItemReward.Item != null)
        {
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = quest.ItemReward.Item.Icon;

            itemQuantityTMP.gameObject.SetActive(true);
            itemQuantityTMP.text = quest.ItemReward.Quantity.ToString();
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
            itemQuantityTMP.gameObject.SetActive(false);
        }
    }

    public void ClaimQuest()
    {
        GameManager.Instance.AddPlayerExp(QuestToComplete.ExpReward);
        Inventory.Instance.AddItem(QuestToComplete.ItemReward.Item, QuestToComplete.ItemReward.Quantity);
        CoinManager.Instance.AddCoins(QuestToComplete.GoldReward);

        if (QuestToComplete.ID == "GreatToadSage") GameManager.Instance.gameOverMenu.ShowGameOverMenu();
        gameObject.SetActive(false);
    }

    private void QuestCompletedCheck()
    {
        if(QuestToComplete.QuestCompleted)
        {
            claimButton.SetActive(true);
            rewardsPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        QuestCompletedCheck();
    }
}
