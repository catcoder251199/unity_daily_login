using System;
using Cysharp.Threading.Tasks;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

public class DailyRewardsButton : MonoBehaviour
{
    [SerializeField] private DailyRewardsSystem dailyRewardsSystem;
    //[SerializeField] private GameObject redDot;
    //[SerializeField] private GameObject timerPanel;
    //[SerializeField] private TextMeshProUGUI timerText;
    private UIPopup _uiPopup;
    private void Start()
    {
        // if (dailyRewardsSystem == null)
        //     dailyRewardsSystem = DailyRewardsSystem.Instance;
        //
        // if (dailyRewardsSystem == null)
        //     return;
        //
        // if (!dailyRewardsSystem.EnoughLevelToReceiveRewards)
        // {
        //     gameObject.SetActive(false);
        //     timerPanel.SetActive(false);
        //     return;
        // }
        //
        // CheckAvailableRewards();
        //
        // dailyRewardsSystem.OnClaimRewardsEvent += CheckAvailableRewards;
        // if (!dailyRewardsSystem.Model.HasClaimedFreeRewards)
        //     OpenDailyRewards();
    }

    public async UniTask OnHomeEnter()
    {
        if (dailyRewardsSystem == null)
            dailyRewardsSystem = DailyRewardsSystem.Instance;

        if (dailyRewardsSystem == null)
            return;

        if (!dailyRewardsSystem.EnoughLevelToReceiveRewards)
        {
            gameObject.SetActive(false);
            //timerPanel.SetActive(false);
            return;
        }

        CheckAvailableRewards(dailyRewardsSystem.TimeLeftUntilRewardDay.TotalSeconds);

        if (dailyRewardsSystem != null)
        {
            dailyRewardsSystem.OnClaimRewardsEvent += OnRewardsClaimed;
            dailyRewardsSystem.OnTimeUpdatedEvent += OnTimerTicked;
        }

        var currentLocalDateTime = DailyRewardsHelper.GetCurrentLocalDateTime().Date;
        var rewardDateTime = dailyRewardsSystem.Model.RewardDateTime.Date;
        var isTodayHaveRewards = currentLocalDateTime.Date == rewardDateTime.Date;
        if (isTodayHaveRewards && !dailyRewardsSystem.Model.HasClaimedFreeRewards)
        {
            OpenDailyRewards();
            await UniTask.WaitUntil(() => _uiPopup != null && _uiPopup.IsVisible);
            await UniTask.WaitUntil(() => _uiPopup != null && _uiPopup.IsHidden);
        }
    }

    private void OnTimerTicked(TimeSpan timeLeftForNextDay)
    {
        CheckAvailableRewards(timeLeftForNextDay.TotalSeconds);
    }
    
    private void OnRewardsClaimed()
    {
        CheckAvailableRewards(dailyRewardsSystem.TimeLeftUntilRewardDay.TotalSeconds);
    }

    private void CheckAvailableRewards(double secondsUntilTomorrow)
    {
        var doesTodayHaveRewards = dailyRewardsSystem.Model.DoesTodayHaveRewards;
        var canClaimSomething = dailyRewardsSystem.HasAnyClaimableRewards;
        // if (redDot != null)
        //     redDot.SetActive(canClaimSomething && doesTodayHaveRewards);

        var secondsUntilRewardTime = Mathf.FloorToInt((float)dailyRewardsSystem.TimeLeftUntilRewardDay.TotalSeconds);
        //timerPanel.gameObject.SetActive(secondsUntilRewardTime >= 0);
        
        // if (timerPanel.gameObject.activeSelf)
        // {
        //     timerText.text = $"{CommonUtils.GetTimerString(secondsUntilRewardTime)}";
        // }
    }

    [ContextMenu("Open Daily Rewards")]
    public void OnButtonPressed()
    {
        OpenDailyRewardsByButton();
    }

    private const string DailyRewardsPopupName = UINameConstants.PopupDailyRewards;
    
    private void OpenDailyRewardsByButton()
    {
        // StaticLock.TryExecute(() =>
        // {
        _uiPopup = UIPopupManager.ShowPopup(DailyRewardsPopupName, true, false);
        if (_uiPopup != null && _uiPopup.TryGetComponent(out DailyRewardsPopup dailyRewardsUI))
        {
            dailyRewardsUI.Initialize(dailyRewardsSystem);
        }
        // });
    }

    private void OpenDailyRewards()
    {
        _uiPopup = UIPopupManager.ShowPopup(DailyRewardsPopupName, true, false);
        if (_uiPopup != null && _uiPopup.TryGetComponent(out DailyRewardsPopup dailyRewardsUI))
        {
            dailyRewardsUI.Initialize(dailyRewardsSystem);
        }
    }

    private void OnDestroy()
    {
        if (dailyRewardsSystem != null)
        {
            dailyRewardsSystem.OnClaimRewardsEvent -= OnRewardsClaimed;
            dailyRewardsSystem.OnTimeUpdatedEvent -= OnTimerTicked;
        }
    }
}