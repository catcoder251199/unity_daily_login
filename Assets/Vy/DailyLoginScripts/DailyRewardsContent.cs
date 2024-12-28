using System.Collections;
using System.Collections.Generic;
using DailyRewards;
using UnityEngine;
using UnityEngine.Serialization;

public class DailyRewardsContent : MonoBehaviour
{
    [SerializeField] private List<AbstractRewardSlot> rewardSlotList;
    private DailyRewardsSystem dailyRewardsSystem;

    public AbstractRewardSlot ReceivableAbstractRewardSlot { get; private set; }

    public void Initialize(DailyRewardsSystem dailyRewardsSystem)
    {
        this.dailyRewardsSystem = dailyRewardsSystem;
        // dailyRewardsSystem.OnClaimRewardsEvent += UpdateUI;
        InitializeUI();
    }

    private void InitializeUI()
    {
        var dataModel = dailyRewardsSystem.Model;
        var config = dailyRewardsSystem.Config;
        
        var daysInOnePage = DailyRewardsConstants.TotalDaysInAWeek;
        var currentDay = dataModel.CurrentDay;
        var currentActivePage = (dataModel.CurrentDay - 1) / daysInOnePage;
        var rewardList = config.RewardList;

        InitializeUI(
            currentDay,
            rewardList.GetRange(currentActivePage * daysInOnePage, daysInOnePage),
            !dataModel.HasClaimedFreeRewards || !dataModel.HasClaimedAdRewards);
    }
    
    private void InitializeUI(int currentDay, List<DateData> dateDataList, bool claimable)
    {
        for (var i = 0; i < rewardSlotList.Count; i++)
        {
            var rewardSlot = rewardSlotList[i];
            if (rewardSlot == null)
                continue;
            
            var dateData = dateDataList[i];

            if (dateData.Day != currentDay)
            {
                rewardSlot.Initialize(dateData, dateData.Day < currentDay ? rewardSlot.GetReceivedState() : rewardSlot.GetUnavailableState());
                continue;
            }
            
            if (claimable)
            {
                ReceivableAbstractRewardSlot = rewardSlot;
                rewardSlot.Initialize(dateData, rewardSlot.GetReceivableState());
            }
            else
                rewardSlot.Initialize(dateData, rewardSlot.GetReceivedState());
        }
    }
}