using System;
using TMPro;

public class UISearchQueueTooltip : UITooltipBase
{
	public TextMeshProUGUI m_searchQueueSizeRankTooltipTextLabel;
	public TextMeshProUGUI m_searchQueueTimeInQueueTooltipTextLabel;
	public TextMeshProUGUI m_searchQueueAverageWaitTimeTooltipTextLabel;
	public TextMeshProUGUI m_searchQueueXpGainedTextLabel;
	public TextMeshProUGUI m_searchQueueSizeRankTooltipText;
	public TextMeshProUGUI m_searchQueueTimeInQueueTooltipText;
	public TextMeshProUGUI m_searchQueueAverageWaitTimeTooltipText;
	public TextMeshProUGUI m_searchQueueXpGainedText;
	public TextMeshProUGUI m_searchQueueQueueStatusTooltipText;

	public void Setup()
	{
		LobbyMatchmakingQueueInfo queueInfo = GameManager.Get().QueueInfo;
		if (queueInfo == null)
		{
			return;
		}

		TimeSpan matchDuration = ClientGameManager.Get().QueueEntryTime == DateTime.MinValue
			? TimeSpan.FromMinutes(0.0)
			: DateTime.UtcNow - ClientGameManager.Get().QueueEntryTime;
		m_searchQueueTimeInQueueTooltipText.text = string.Format(
			StringUtil.TR("SecondsTimerShort", "Global"),
			(int)matchDuration.TotalSeconds);
		if (queueInfo.ShowQueueSize && queueInfo.QueuedPlayers > 0)
		{
			UIManager.SetGameObjectActive(m_searchQueueSizeRankTooltipText, true);
			UIManager.SetGameObjectActive(m_searchQueueSizeRankTooltipTextLabel, true);
			m_searchQueueSizeRankTooltipText.text = $"{queueInfo.QueuedPlayers}";
		}
		else
		{
			UIManager.SetGameObjectActive(m_searchQueueSizeRankTooltipText, false);
			UIManager.SetGameObjectActive(m_searchQueueSizeRankTooltipTextLabel, false);
		}
		m_searchQueueAverageWaitTimeTooltipText.text = string.Format(
			StringUtil.TR("SecondsTimerShort", "Global"),
			(int)queueInfo.AverageWaitTime.TotalSeconds);
		m_searchQueueQueueStatusTooltipText.text = string.Format(
			StringUtil.TR("WaitingForStatus", "Global"),
			queueInfo.WhatQueueIsWaitingForToMakeNextGame);
		ushort soloSubGameMask = ClientGameManager.Get().GetSoloSubGameMask(queueInfo.GameConfig.GameType);
		int queueXpGained = -1;
		for (ushort subTypeMask = 1; subTypeMask != 0; subTypeMask = (ushort)(subTypeMask << 1))
		{
			if ((subTypeMask & soloSubGameMask) != 0)
			{
				GameBalanceVars.GameRewardBucketType rewardBucket = queueInfo.GameConfig.GetSubType(subTypeMask).RewardBucket;
				int xPBonusForQueueTime = GameBalanceVars.Get().GetXPBonusForQueueTime(rewardBucket, matchDuration);
				if (queueXpGained < xPBonusForQueueTime)
				{
					queueXpGained = xPBonusForQueueTime;
				}
			}
		}
		m_searchQueueXpGainedText.text = string.Format(StringUtil.TR("XpGained", "Global"), queueXpGained);
	}
}
