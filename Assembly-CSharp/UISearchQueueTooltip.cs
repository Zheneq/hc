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
		while (true)
		{
			TimeSpan timeSpan;
			if (ClientGameManager.Get().QueueEntryTime == DateTime.MinValue)
			{
				timeSpan = TimeSpan.FromMinutes(0.0);
			}
			else
			{
				timeSpan = DateTime.UtcNow - ClientGameManager.Get().QueueEntryTime;
			}
			TimeSpan matchDuration = timeSpan;
			m_searchQueueTimeInQueueTooltipText.text = string.Format(StringUtil.TR("SecondsTimerShort", "Global"), (int)matchDuration.TotalSeconds);
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
			m_searchQueueAverageWaitTimeTooltipText.text = string.Format(StringUtil.TR("SecondsTimerShort", "Global"), (int)queueInfo.AverageWaitTime.TotalSeconds);
			m_searchQueueQueueStatusTooltipText.text = string.Format(StringUtil.TR("WaitingForStatus", "Global"), queueInfo.WhatQueueIsWaitingForToMakeNextGame);
			ushort soloSubGameMask = ClientGameManager.Get().GetSoloSubGameMask(queueInfo.GameConfig.GameType);
			int num = -1;
			for (ushort num2 = 1; num2 != 0; num2 = (ushort)(num2 << 1))
			{
				if ((num2 & soloSubGameMask) != 0)
				{
					GameBalanceVars.GameRewardBucketType rewardBucket = queueInfo.GameConfig.GetSubType(num2).RewardBucket;
					int xPBonusForQueueTime = GameBalanceVars.Get().GetXPBonusForQueueTime(rewardBucket, matchDuration);
					if (num < xPBonusForQueueTime)
					{
						num = xPBonusForQueueTime;
					}
				}
			}
			while (true)
			{
				m_searchQueueXpGainedText.text = string.Format(StringUtil.TR("XpGained", "Global"), num);
				return;
			}
		}
	}
}
