using System;
using Cysharp.Threading.Tasks.Triggers;
using DailyRewards;
using UnityEngine;

public class DailyRewardsModel : MonoBehaviour
{
    [SerializeField] private AbstractDailyRewardsStorage userStorage;
    [SerializeField] private DailyRewardsConfig config;
    public DailyRewardsConfig Config => config;

    public void Initialize()
    {
        InitializeUserStorage();
        InitializeConfig();
    }

    private void InitializeConfig()
    {
        config.LoadConfigFromRemote();
    }

    private void InitializeUserStorage()
    {
        userStorage.Initialize();
    }

    public int CurrentDay => (userStorage.DailyRewardsUserData.currentDay % config.TotalDays) + 1;

    public DateTime RewardDateTime =>
        DailyRewardsHelper.ConvertLongToDateTime(userStorage.DailyRewardsUserData.rewardDateTime);

    public bool HasClaimedFreeRewards => userStorage.DailyRewardsUserData.claimedFreeRewards;
    public bool HasClaimedAdRewards => userStorage.DailyRewardsUserData.claimedAdRewards;
    public bool HasClaimedAnyRewards => HasClaimedFreeRewards || HasClaimedAdRewards;
    public bool HasClaimedAllRewards => HasClaimedFreeRewards && HasClaimedAdRewards;
    public bool DoesTodayHaveRewards => DailyRewardsHelper.GetCurrentLocalDateTime().Date == RewardDateTime.Date;
    public bool EnoughLevelToReceiveRewards => userStorage.CurrentLevel > config.MinLevelToReceiveRewards;

    public void IncreaseCurrentDayIndex()
    {
        userStorage.DailyRewardsUserData.currentDay++;
        userStorage.MarkDirty();
    }

    public void SetRewardDateTime(long rewardDateTime)
    {
        userStorage.DailyRewardsUserData.rewardDateTime = rewardDateTime;
        userStorage.MarkDirty();
    }


    public void ResetClaimedRewardsFlags()
    {
        userStorage.DailyRewardsUserData.claimedAdRewards = false;
        userStorage.DailyRewardsUserData.claimedFreeRewards = false;
        userStorage.MarkDirty();
    }

    public void SetClaimedFreeRewards(bool claimed)
    {
        userStorage.DailyRewardsUserData.claimedFreeRewards = claimed;
        if (HasClaimedAllRewards)
        {
            userStorage.DailyRewardsUserData.currentDay++;
            userStorage.DailyRewardsUserData.claimedAdRewards = false;
            userStorage.DailyRewardsUserData.claimedFreeRewards = false;
            userStorage.DailyRewardsUserData.rewardDateTime = DailyRewardsHelper.GetStartTomorrowAsLong();
        }

        userStorage.MarkDirty();
    }

    public void SetClaimAdRewards(bool claimed)
    {
        userStorage.DailyRewardsUserData.claimedAdRewards = claimed;
        if (HasClaimedAllRewards)
        {
            userStorage.DailyRewardsUserData.currentDay++;
            userStorage.DailyRewardsUserData.claimedAdRewards = false;
            userStorage.DailyRewardsUserData.claimedFreeRewards = false;
            userStorage.DailyRewardsUserData.rewardDateTime = DailyRewardsHelper.GetStartTomorrowAsLong();
        }

        userStorage.MarkDirty();
    }
}