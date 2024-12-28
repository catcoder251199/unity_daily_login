using System;

[Serializable]
public class DailyRewardsUserData
{
    public int currentDay = 0;
    public bool claimedFreeRewards;
    public bool claimedAdRewards;
    public long rewardDateTime;
}
