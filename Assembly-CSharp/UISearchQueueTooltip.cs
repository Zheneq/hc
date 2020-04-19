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
		if (queueInfo != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISearchQueueTooltip.Setup()).MethodHandle;
			}
			TimeSpan timeSpan;
			if (ClientGameManager.Get().QueueEntryTime == DateTime.MinValue)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				timeSpan = TimeSpan.FromMinutes(0.0);
			}
			else
			{
				timeSpan = DateTime.UtcNow - ClientGameManager.Get().QueueEntryTime;
			}
			TimeSpan matchDuration = timeSpan;
			this.m_searchQueueTimeInQueueTooltipText.text = string.Format(StringUtil.TR("SecondsTimerShort", "Global"), (int)matchDuration.TotalSeconds);
			if (queueInfo.ShowQueueSize && queueInfo.QueuedPlayers > 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(this.m_searchQueueSizeRankTooltipText, true, null);
				UIManager.SetGameObjectActive(this.m_searchQueueSizeRankTooltipTextLabel, true, null);
				this.m_searchQueueSizeRankTooltipText.text = string.Format("{0}", queueInfo.QueuedPlayers);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_searchQueueSizeRankTooltipText, false, null);
				UIManager.SetGameObjectActive(this.m_searchQueueSizeRankTooltipTextLabel, false, null);
			}
			this.m_searchQueueAverageWaitTimeTooltipText.text = string.Format(StringUtil.TR("SecondsTimerShort", "Global"), (int)queueInfo.AverageWaitTime.TotalSeconds);
			this.m_searchQueueQueueStatusTooltipText.text = string.Format(StringUtil.TR("WaitingForStatus", "Global"), queueInfo.WhatQueueIsWaitingForToMakeNextGame);
			ushort soloSubGameMask = ClientGameManager.Get().GetSoloSubGameMask(queueInfo.GameConfig.GameType);
			int num = -1;
			for (ushort num2 = 1; num2 != 0; num2 = (ushort)(num2 << 1))
			{
				if ((num2 & soloSubGameMask) != 0)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					GameBalanceVars.GameRewardBucketType rewardBucket = queueInfo.GameConfig.GetSubType(num2).RewardBucket;
					int xpbonusForQueueTime = GameBalanceVars.Get().GetXPBonusForQueueTime(rewardBucket, matchDuration);
					if (num < xpbonusForQueueTime)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						num = xpbonusForQueueTime;
					}
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_searchQueueXpGainedText.text = string.Format(StringUtil.TR("XpGained", "Global"), num);
		}
	}
}
