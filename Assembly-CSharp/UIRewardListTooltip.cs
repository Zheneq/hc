using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRewardListTooltip : UITooltipBase
{
	public UIPlayerProgressRewardListEntry m_entryPrefab;

	public TextMeshProUGUI m_levelLabel;

	public void Setup(List<RewardUtils.RewardData> rewards, int currentLevel, UIRewardListTooltip.RewardsType rewardType, bool flipped = false)
	{
		float x = 1f;
		float num;
		if (flipped)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRewardListTooltip.Setup(List<RewardUtils.RewardData>, int, UIRewardListTooltip.RewardsType, bool)).MethodHandle;
			}
			num = (float)-1;
		}
		else
		{
			num = (float)1;
		}
		Vector3 localScale = new Vector3(x, num, 1f);
		base.transform.localScale = localScale;
		List<UIPlayerProgressRewardListEntry> list = new List<UIPlayerProgressRewardListEntry>();
		list.AddRange(base.gameObject.GetComponentsInChildren<UIPlayerProgressRewardListEntry>(true));
		for (int i = 0; i < rewards.Count; i++)
		{
			UIPlayerProgressRewardListEntry uiplayerProgressRewardListEntry;
			if (i < list.Count)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				list[i].Setup(rewards[i], currentLevel);
				UIManager.SetGameObjectActive(list[i], true, null);
				uiplayerProgressRewardListEntry = list[i];
			}
			else
			{
				uiplayerProgressRewardListEntry = UnityEngine.Object.Instantiate<UIPlayerProgressRewardListEntry>(this.m_entryPrefab);
				uiplayerProgressRewardListEntry.transform.SetParent(base.transform);
				uiplayerProgressRewardListEntry.transform.localPosition = Vector3.zero;
				uiplayerProgressRewardListEntry.Setup(rewards[i], currentLevel);
				list.Add(uiplayerProgressRewardListEntry);
			}
			for (int j = 0; j < uiplayerProgressRewardListEntry.m_levelTexts.Length; j++)
			{
				UIManager.SetGameObjectActive(uiplayerProgressRewardListEntry.m_levelTexts[j], rewardType != UIRewardListTooltip.RewardsType.Seasons, null);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			uiplayerProgressRewardListEntry.transform.localScale = localScale;
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
		for (int k = rewards.Count; k < list.Count; k++)
		{
			UIManager.SetGameObjectActive(list[k], false, null);
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		UIManager.SetGameObjectActive(this.m_levelLabel, rewardType != UIRewardListTooltip.RewardsType.Seasons, null);
		if (rewardType == UIRewardListTooltip.RewardsType.Tutorial)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_levelLabel.alignment = TextAlignmentOptions.Midline;
			this.m_levelLabel.text = StringUtil.TR("MatchesPlayed", "OverlayScreensScene");
		}
		else if (rewardType == UIRewardListTooltip.RewardsType.Character)
		{
			this.m_levelLabel.alignment = TextAlignmentOptions.Left;
			this.m_levelLabel.text = StringUtil.TR("LevelLabel", "PersistentScene");
		}
		RectTransform rectTransform = this.m_levelLabel.transform as RectTransform;
		rectTransform.localScale = localScale;
		if (flipped)
		{
			for (int l = 0; l < list.Count; l++)
			{
				list[list.Count - l - 1].transform.SetSiblingIndex(l);
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			rectTransform.SetAsLastSibling();
		}
		else
		{
			rectTransform.SetAsFirstSibling();
		}
	}

	public enum RewardsType
	{
		Seasons,
		Character,
		Tutorial
	}
}
