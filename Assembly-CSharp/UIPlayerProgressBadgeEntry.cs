using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerProgressBadgeEntry : MonoBehaviour
{
	[Serializable]
	public class BadgeDisplayInfo
	{
		public RectTransform m_Container;

		public Image m_Icon;

		public TextMeshProUGUI m_count;

		public UITooltipHoverObject m_tooltipObject;

		public _ButtonSwapSprite m_badgeHitbox;
	}

	public class BadgeGroupDisplayInfo
	{
		public GameBalanceVars.GameResultBadge.BadgeQuality Quality;

		public string Requirements;

		public string GetRequirementDescription()
		{
			string text = string.Empty;
			string text2 = null;
			if (Quality == GameBalanceVars.GameResultBadge.BadgeQuality.Gold)
			{
				text2 = HUD_UIResources.ColorToHex(GameResultBadgeData.Get().BadgeGroupGoldRequirementColorHex);
			}
			else if (Quality == GameBalanceVars.GameResultBadge.BadgeQuality.Silver)
			{
				text2 = HUD_UIResources.ColorToHex(GameResultBadgeData.Get().BadgeGroupSilverRequirementColorHex);
			}
			else if (Quality == GameBalanceVars.GameResultBadge.BadgeQuality.Bronze)
			{
				text2 = HUD_UIResources.ColorToHex(GameResultBadgeData.Get().BadgeGroupBronzeRequirementColorHex);
			}
			if (!text2.IsNullOrEmpty())
			{
				text += string.Format("<color={0}>{2}</color>: {1}", text2, Requirements, StringUtil.TR(Quality.ToString(), "BadgeQuality"));
			}
			if (Requirements.IsNullOrEmpty())
			{
				text += "Need to fill out group requirement description in SPECIFIC badges";
			}
			return text;
		}
	}

	public BadgeDisplayInfo[] displayInfos;

	public TextMeshProUGUI m_name;

	public _ButtonSwapSprite m_hitbox;

	private UITooltipHoverObject m_tooltipObj;

	private GameResultBadgeData.ConsolidatedBadgeGroup m_groupReference;

	private int m_groupIndex;

	private GameBalanceVars.GameResultBadge m_badgeReference;

	public bool DoBadgeTooltip(UITooltipBase baseTooltip)
	{
		UITitledTooltip uITitledTooltip = baseTooltip as UITitledTooltip;
		if (uITitledTooltip == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (m_groupReference != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					string text = StringUtil.TR_BadgeGroupDescription(m_groupIndex);
					if (text.IsNullOrEmpty())
					{
						text = "No badge GROUP description, needs to be authored";
					}
					List<BadgeGroupDisplayInfo> list = new List<BadgeGroupDisplayInfo>();
					for (int i = 0; i < m_groupReference.BadgeIDs.Length; i++)
					{
						GameBalanceVars.GameResultBadge badgeInfo = GameResultBadgeData.Get().GetBadgeInfo(m_groupReference.BadgeIDs[i]);
						if (badgeInfo != null)
						{
							list.Add(new BadgeGroupDisplayInfo
							{
								Quality = badgeInfo.Quality,
								Requirements = GameResultBadgeData.GetBadgeGroupRequirementDescription(badgeInfo, UIPlayerProgressPanel.Get().m_badgesPanel.CurrentCharacterTypeFilter)
							});
						}
					}
					
					list.Sort(delegate(BadgeGroupDisplayInfo x, BadgeGroupDisplayInfo y)
						{
							if (x.Quality != y.Quality)
							{
								int result;
								if (x.Quality > y.Quality)
								{
									result = -1;
								}
								else
								{
									result = 1;
								}
								return result;
							}
							return 0;
						});
					for (int j = 0; j < list.Count; j++)
					{
						text = new StringBuilder().Append(text).Append("\n").Append(list[j].GetRequirementDescription()).ToString();
					}
					uITitledTooltip.Setup(StringUtil.TR_BadgeGroupName(m_groupIndex), text, string.Empty);
					return true;
				}
				}
			}
		}
		if (m_badgeReference != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					string badgeDescription = GameResultBadgeData.GetBadgeDescription(m_badgeReference, UIPlayerProgressPanel.Get().m_badgesPanel.CurrentCharacterTypeFilter);
					uITitledTooltip.Setup(StringUtil.TR_BadgeName(m_badgeReference.UniqueBadgeID), badgeDescription, string.Empty);
					return true;
				}
				}
			}
		}
		return false;
	}

	private void Awake()
	{
		m_tooltipObj = m_hitbox.GetComponent<UITooltipHoverObject>();
		if (!(m_tooltipObj != null))
		{
			return;
		}
		while (true)
		{
			m_tooltipObj.Setup(TooltipType.Titled, DoBadgeTooltip);
			return;
		}
	}

	public void Setup(GameResultBadgeData.ConsolidatedBadgeGroup badgeGroup, int Index, Dictionary<int, int> earnedBadges)
	{
		m_groupReference = badgeGroup;
		m_groupIndex = Index;
		m_badgeReference = null;
		for (int i = 0; i < displayInfos.Length; i++)
		{
			if (i < badgeGroup.BadgeIDs.Length)
			{
				int value;
				earnedBadges.TryGetValue(badgeGroup.BadgeIDs[i], out value);
				UIManager.SetGameObjectActive(displayInfos[i].m_Container, true);
				displayInfos[i].m_count.text = new StringBuilder().Append("x").Append(UIStorePanel.FormatIntToString(value, true)).ToString();
				displayInfos[i].m_count.raycastTarget = false;
				GameBalanceVars.GameResultBadge badgeInfo = GameResultBadgeData.Get().GetBadgeInfo(badgeGroup.BadgeIDs[i]);
				displayInfos[i].m_Icon.sprite = Resources.Load<Sprite>(badgeInfo.BadgeIconString);
				UIManager.SetGameObjectActive(displayInfos[i].m_badgeHitbox, false);
			}
			else
			{
				UIManager.SetGameObjectActive(displayInfos[i].m_Container, false);
			}
		}
		while (true)
		{
			m_name.text = StringUtil.TR_BadgeGroupName(Index);
			return;
		}
	}

	public void Setup(GameBalanceVars.GameResultBadge badge, int count)
	{
		m_name.text = StringUtil.TR_BadgeName(badge.UniqueBadgeID);
		UIManager.SetGameObjectActive(displayInfos[0].m_Container, true);
		displayInfos[0].m_Icon.sprite = Resources.Load<Sprite>(badge.BadgeIconString);
		displayInfos[0].m_count.text = new StringBuilder().Append("x").Append(UIStorePanel.FormatIntToString(count, true)).ToString();
		displayInfos[0].m_count.raycastTarget = false;
		UIManager.SetGameObjectActive(displayInfos[0].m_badgeHitbox, false);
		for (int i = 1; i < displayInfos.Length; i++)
		{
			UIManager.SetGameObjectActive(displayInfos[i].m_Container, false);
		}
		while (true)
		{
			m_groupReference = null;
			m_groupIndex = 0;
			m_badgeReference = badge;
			return;
		}
	}
}
