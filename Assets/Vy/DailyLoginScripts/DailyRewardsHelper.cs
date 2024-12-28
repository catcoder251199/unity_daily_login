using System;
//using TrackingHelper;
using UnityEngine;

public static class DailyRewardsHelper
{
    public static DateTime GetCurrentLocalDateTime() => DateTime.Now;

    public static DateTime ConvertLongToDateTime(long value) =>
        DateTimeOffset.FromUnixTimeSeconds(value).ToLocalTime().DateTime;

    public static long ConvertDateTimeToLong(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return (long)Math.Floor(diff.TotalSeconds);
    }
    
    public static long GetStartOfTodayAsLong()
    {
        return new DateTimeOffset(DateTime.Today).ToUnixTimeSeconds();
    }
    
    public static long GetStartTomorrowAsLong()
    {
        return new DateTimeOffset(DateTime.Today.AddDays(1)).ToUnixTimeSeconds();
    }
    
    public static long GetSecondsUntil(long targetUnixTimestamp)
    {
        DateTime now = DateTime.UtcNow;
        long currentUnixTimestamp = new DateTimeOffset(now).ToUnixTimeSeconds();
        long secondsRemaining = targetUnixTimestamp - currentUnixTimestamp;
        return secondsRemaining < 0 ? 0 : secondsRemaining;
    }

    public static long GetStartOfNextDayAsLong()
    {
        DateTime today = DateTime.Today;
        DateTime nextDay = today.AddDays(1);
        return ((DateTimeOffset)nextDay).ToUnixTimeSeconds();
    }
    
    public static bool IsAfterDay(DateTime afterDay, DateTime day) => day.Date > afterDay.Date;
    

    public static void SpawnItem(string rewardId, int rewardAmount, Vector3 spawnPosition, Vector3 destinationPosition, bool watchAd)
    {
        // var flyerManager = InGameFlyersManager.Instance;
        // if (flyerManager == null)
        //     return;
        //
        // var resourceSourceType = watchAd ? ResourceSourceType.Watch_Ads : ResourceSourceType.Free_Reward;
        // var resourceBuilder = new TrackingResourceBuilder()
        //     .SetLocation(DailyRewardsConstants.TrackingLocation)
        //     .SetResourceSourceType(resourceSourceType);
        //
        // switch (rewardId)
        // {
        //     case DailyRewardsConstants.Gold:
        //         resourceBuilder.SetSourceID(SourceID.GOLD);
        //         flyerManager.SpawnCoinWithLog(rewardAmount, spawnPosition, destinationPosition, resourceBuilder);
        //         break;
        //     case DailyRewardsConstants.Magnet:
        //         resourceBuilder.SetSourceID(SourceID.MAGNET);
        //         flyerManager.SpawnBoostersUpWithLog(spawnPosition, FlyerType.MAGNET_UP, rewardAmount, resourceBuilder, 1, null, true);
        //         break;
        //     case DailyRewardsConstants.Undo:
        //         resourceBuilder.SetSourceID(SourceID.UNDO);
        //         flyerManager.SpawnBoostersUpWithLog(spawnPosition, FlyerType.UNDO_UP, rewardAmount, resourceBuilder, 1, null, true);
        //         break;
        //     case DailyRewardsConstants.Shuffle:
        //         resourceBuilder.SetSourceID(SourceID.SHUFFLE);
        //         flyerManager.SpawnBoostersUpWithLog(spawnPosition, FlyerType.SHUFFLE_UP, rewardAmount, resourceBuilder, 1, null, true);
        //         break;
        // }
    }
}
