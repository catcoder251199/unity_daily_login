using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardsStorage : AbstractDailyRewardsStorage
{
    [SerializeField] private UserProfile userProfile;

    public override void Initialize()
    {
        if (userProfile == null)
            userProfile = TryToFindUserProfile();
        if (userProfile != null)
            dailyRewardsUserData = userProfile.DailyRewardsUserData;
    }

    private UserProfile TryToFindUserProfile()
    {
        if (GameMaster.Instance == null)
            return null;
            
        return GameMaster.Instance.GetUserProfile();
    }
    
    public override int CurrentLevel => userProfile.CurrentLevel;
    public override void MarkDirty() => userProfile.MarkDirty();
}
