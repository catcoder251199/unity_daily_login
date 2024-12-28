using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDailyRewardsStorage : MonoBehaviour
{
    [SerializeField] protected DailyRewardsUserData dailyRewardsUserData;
    
    public DailyRewardsUserData DailyRewardsUserData => dailyRewardsUserData;
    public abstract int CurrentLevel { get; }
    public abstract void Initialize();
    public abstract void MarkDirty();
}
