using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRewardListTooltip : UITooltipBase
{
	public enum RewardsType
	{
		Seasons,
		Character,
		Tutorial
	}

	public UIPlayerProgressRewardListEntry m_entryPrefab;

	public TextMeshProUGUI m_levelLabel;

	public void Setup(List<RewardUtils.RewardData> rewards, int currentLevel, RewardsType rewardType, bool flipped = false)
	{
		int num;
		if (flipped)
		{
			num = -1;
		}
		else
		{
			num = 1;
		}
		Vector3 localScale = new Vector3(1f, num, 1f);
		base.transform.localScale = localScale;
		List<UIPlayerProgressRewardListEntry> list = new List<UIPlayerProgressRewardListEntry>();
		list.AddRange(base.gameObject.GetComponentsInChildren<UIPlayerProgressRewardListEntry>(true));
		for (int i = 0; i < rewards.Count; i++)
		{
			UIPlayerProgressRewardListEntry uIPlayerProgressRewardListEntry;
			if (i < list.Count)
			{
				list[i].Setup(rewards[i], currentLevel);
				UIManager.SetGameObjectActive(list[i], true);
				uIPlayerProgressRewardListEntry = list[i];
			}
			else
			{
				uIPlayerProgressRewardListEntry = Object.Instantiate(m_entryPrefab);
				uIPlayerProgressRewardListEntry.transform.SetParent(base.transform);
				uIPlayerProgressRewardListEntry.transform.localPosition = Vector3.zero;
				uIPlayerProgressRewardListEntry.Setup(rewards[i], currentLevel);
				list.Add(uIPlayerProgressRewardListEntry);
			}
			for (int j = 0; j < uIPlayerProgressRewardListEntry.m_levelTexts.Length; j++)
			{
				UIManager.SetGameObjectActive(uIPlayerProgressRewardListEntry.m_levelTexts[j], rewardType != RewardsType.Seasons);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					goto end_IL_011a;
				}
				continue;
				end_IL_011a:
				break;
			}
			uIPlayerProgressRewardListEntry.transform.localScale = localScale;
		}
		while (true)
		{
			for (int k = rewards.Count; k < list.Count; k++)
			{
				UIManager.SetGameObjectActive(list[k], false);
			}
			while (true)
			{
				UIManager.SetGameObjectActive(m_levelLabel, rewardType != RewardsType.Seasons);
				if (rewardType == RewardsType.Tutorial)
				{
					m_levelLabel.alignment = TextAlignmentOptions.Midline;
					m_levelLabel.text = StringUtil.TR("MatchesPlayed", "OverlayScreensScene");
				}
				else if (rewardType == RewardsType.Character)
				{
					m_levelLabel.alignment = TextAlignmentOptions.Left;
					m_levelLabel.text = StringUtil.TR("LevelLabel", "PersistentScene");
				}
				RectTransform rectTransform = m_levelLabel.transform as RectTransform;
				rectTransform.localScale = localScale;
				if (flipped)
				{
					for (int l = 0; l < list.Count; l++)
					{
						list[list.Count - l - 1].transform.SetSiblingIndex(l);
					}
					while (true)
					{
						rectTransform.SetAsLastSibling();
						return;
					}
				}
				rectTransform.SetAsFirstSibling();
				return;
			}
		}
	}
}
