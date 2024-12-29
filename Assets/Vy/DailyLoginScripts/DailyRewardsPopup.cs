using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DailyRewards;
using Doozy.Engine.UI;
using UnityEngine;

namespace DailyRewards
{
    public class DailyRewardsPopup : MonoBehaviour
    {
        [SerializeField] private bool initOnAwake = false;
        [SerializeField] private UIPopup popup;

        [SerializeField, Header("View/UI")] private DailyRewardsContent content;
        [SerializeField] AbstractButtonsUIController buttonsUIController;
        [SerializeField] AbstractTimerUIController timerUIController;

        private DailyRewardsSystem dailyRewardsSystem;
        public DailyRewardsSystem DailyRewardsSystem => dailyRewardsSystem;
        
        private DailyRewardsModel dataModel;
        private LockUI lockUI;

        private void Start()
        {
            if (initOnAwake)
            {
                Initialize(DailyRewardsSystem.Instance);
            }
        }

        public void Initialize(DailyRewardsSystem dailyRewardsSystem)
        {
            if (dailyRewardsSystem == null)
            {
                PrintLogError("dailyRewardsSystem is null");
                return;
            }

            this.dailyRewardsSystem = dailyRewardsSystem;
            dataModel = this.dailyRewardsSystem.Model;
            lockUI = new LockUI();

            InitializeContent();
            InitializeTimerUIController();
            InitializeButtonsUIController();
        }

        private void InitializeTimerUIController()
        {
            if (timerUIController == null)
            {
                PrintLog("timerUIController is null");
                return;
            }

            timerUIController?.Initialize(dailyRewardsSystem);
            timerUIController?.UpdateTimer();
        }

        private void InitializeButtonsUIController()
        {
            if (buttonsUIController == null)
            {
                PrintLog("ButtonsUIController is null");
                return;
            }

            buttonsUIController?.Initialize(dailyRewardsSystem.Model);
            buttonsUIController?.UpdateButtons();
        }

        private void InitializeContent()
        {
            content.Initialize(this);
        }

        public void OnButtonClosedClicked()
        {
            if (lockUI.IsLocked())
                return;

            // StaticLock.TryExecute(() =>
            // {
            //     if (popup != null) popup.Hide();
            // });
        }

        public void OnFreeButtonClicked()
        {
            if (lockUI.IsLocked())
                return;

            // StaticLock.TryExecute(async () =>
            // {
            //     lockUI.Lock();
            //
            //     if (!dailyRewardsSystem.CanClaimFreeRewards())
            //     {
            //         buttonsUIController.SetAllClaimButtonsActive(false);
            //         buttonsUIController.SetCloseButton(true);
            //         lockUI.UnLock();
            //         return;
            //     }
            //
            //     var rewardsToday = dailyRewardsSystem.GetRewardsToday();
            //     var success = dailyRewardsSystem.ClaimFreeRewards();
            //     if (success)
            //     {
            //         buttonsUIController?.UpdateButtons();
            //         
            //         ShowReceivedRewards(rewardsToday, false);
            //         await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            //         lockUI.ForceUnlock();
            //     }
            // });
        }

        private void ShowReceivedRewards(List<RewardItemData> rewardList, bool watchAd)
        {
            if (rewardList == null || rewardList.Count == 0)
                return;

            if (content.ReceivableAbstractRewardSlot != null)
            {
                var doesTodayHaveRewards = dataModel.DoesTodayHaveRewards;
                var stillClaimable = doesTodayHaveRewards &&
                                     (/*!dataModel.HasClaimedAdRewards || */!dataModel.HasClaimed);
                content.ReceivableAbstractRewardSlot.ClaimRewards(rewardList, stillClaimable, watchAd);
            }
        }

        public void OnX2ButtonClicked()
        {
            if (lockUI.IsLocked())
                return;

            // StaticLock.TryExecute(() =>
            // {
            //     lockUI .Lock();
            //
            //     if (!dailyRewardsSystem.CanClaimX2Rewards())
            //     {
            //         buttonsUIController.SetAllClaimButtonsActive(false);
            //         buttonsUIController.SetCloseButton(true);
            //         lockUI.UnLock();
            //         return;
            //     }
            //
            //     GameMaster.Instance.ShowRewardedVideo(OnDoubleClaimSuccess, OnWatchAdFailed, "DailyRewardsX2");
            // });
        }

        public void OnOneMoreButtonClicked()
        {
            if (lockUI.IsLocked())
                return;

            // StaticLock.TryExecute(() =>
            // {
            //     lockUI.Lock();
            //
            //     if (!dailyRewardsSystem.CanClaimOneMoreRewards())
            //     {
            //         buttonsUIController.SetAllClaimButtonsActive(false);
            //         buttonsUIController.SetCloseButton(true);
            //         lockUI.UnLock();
            //         return;
            //     }
            //
            //     GameMaster.Instance.ShowRewardedVideo(OnOneMoreClaimSuccess, OnWatchAdFailed, "DailyRewardsOneMore");
            // });
        }

        private async void OnDoubleClaimSuccess()
        {
            var rewardsToday = dailyRewardsSystem.GetDoubledRewardsToday();
            var success = dailyRewardsSystem.ClaimX2Rewards();
            if (success)
            {
                buttonsUIController.UpdateButtons();
                ShowReceivedRewards(rewardsToday, true);
                await UniTask.Delay(TimeSpan.FromSeconds(0.5));
                lockUI.ForceUnlock();
            }
        }

        private void OnWatchAdFailed(string error)
        {
            PrintLogError($"OnWatchAdFailed: {error}");
            lockUI.UnLock();

            //Debug.LogError($"[{nameof(DailyRewardsPopup)}] OnWatchAdFailed: {error}");
            // var uiPopup = UIPopupManager.ShowPopup(UINameConstants.WatchAdFailedPopup, false, false);
            // if (uiPopup != null && uiPopup.TryGetComponent<WatchAdFailedPopup>(out var watchAdFailedPopup))
            // {
            //     watchAdFailedPopup.OnCloseButtonClickedEvent += () =>
            //     {
            //         if (this == null)
            //             return;
            //
            //         lockUI = false;
            //     };
            // }
            // else
            // {
            //     lockUI = false;
            // }
        }

        private async void OnOneMoreClaimSuccess()
        {
            var rewardsToday = dailyRewardsSystem.GetRewardsToday();
            var success = dailyRewardsSystem.ClaimOneMoreRewards();
            if (success)
            {
                buttonsUIController.UpdateButtons();
                ShowReceivedRewards(rewardsToday, true);
                await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            }

            lockUI.UnLock();
        }

        private static void PrintLog(string message)
        {
            var logPrefix = $"[{nameof(DailyRewardsPopup)}] ";
            Debug.Log(logPrefix + message);
        }

        private static void PrintLogError(string message)
        {
            var logPrefix = $"[{nameof(DailyRewardsPopup)}] ";
            Debug.LogError(logPrefix + message);
        }
    }
}