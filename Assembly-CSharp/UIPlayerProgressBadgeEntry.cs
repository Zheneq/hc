using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerProgressBadgeEntry : MonoBehaviour
{
	public UIPlayerProgressBadgeEntry.BadgeDisplayInfo[] displayInfos;

	public TextMeshProUGUI m_name;

	public _ButtonSwapSprite m_hitbox;

	private UITooltipHoverObject m_tooltipObj;

	private GameResultBadgeData.ConsolidatedBadgeGroup m_groupReference;

	private int m_groupIndex;

	private GameBalanceVars.GameResultBadge m_badgeReference;

	public bool DoBadgeTooltip(UITooltipBase baseTooltip)
	{
		UITitledTooltip uititledTooltip = baseTooltip as UITitledTooltip;
		if (uititledTooltip == null)
		{
			return false;
		}
		if (this.m_groupReference != null)
		{
			string text = StringUtil.TR_BadgeGroupDescription(this.m_groupIndex);
			if (text.IsNullOrEmpty())
			{
				text = "No badge GROUP description, needs to be authored";
			}
			List<UIPlayerProgressBadgeEntry.BadgeGroupDisplayInfo> list = new List<UIPlayerProgressBadgeEntry.BadgeGroupDisplayInfo>();
			for (int i = 0; i < this.m_groupReference.BadgeIDs.Length; i++)
			{
				GameBalanceVars.GameResultBadge badgeInfo = GameResultBadgeData.Get().GetBadgeInfo(this.m_groupReference.BadgeIDs[i]);
				if (badgeInfo != null)
				{
					list.Add(new UIPlayerProgressBadgeEntry.BadgeGroupDisplayInfo
					{
						Quality = badgeInfo.Quality,
						Requirements = GameResultBadgeData.GetBadgeGroupRequirementDescription(badgeInfo, UIPlayerProgressPanel.Get().m_badgesPanel.CurrentCharacterTypeFilter)
					});
				}
			}
			List<UIPlayerProgressBadgeEntry.BadgeGroupDisplayInfo> list2 = list;
			
			list2.Sort(delegate(UIPlayerProgressBadgeEntry.BadgeGroupDisplayInfo x, UIPlayerProgressBadgeEntry.BadgeGroupDisplayInfo y)
				{
					if (x.Quality == y.Quality)
					{
						return 0;
					}
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
				});
			for (int j = 0; j < list.Count; j++)
			{
				text = text + "\n" + list[j].GetRequirementDescription();
			}
			uititledTooltip.Setup(StringUtil.TR_BadgeGroupName(this.m_groupIndex), text, string.Empty);
			return true;
		}
		if (this.m_badgeReference != null)
		{
			string badgeDescription = GameResultBadgeData.GetBadgeDescription(this.m_badgeReference, UIPlayerProgressPanel.Get().m_badgesPanel.CurrentCharacterTypeFilter);
			uititledTooltip.Setup(StringUtil.TR_BadgeName(this.m_badgeReference.UniqueBadgeID), badgeDescription, string.Empty);
			return true;
		}
		return false;
	}

	private void Awake()
	{
		this.m_tooltipObj = this.m_hitbox.GetComponent<UITooltipHoverObject>();
		if (this.m_tooltipObj != null)
		{
			this.m_tooltipObj.Setup(TooltipType.Titled, new TooltipPopulateCall(this.DoBadgeTooltip), null);
		}
	}

	public void Setup(GameResultBadgeData.ConsolidatedBadgeGroup badgeGroup, int Index, Dictionary<int, int> earnedBadges)
	{
		this.m_groupReference = badgeGroup;
		this.m_groupIndex = Index;
		this.m_badgeReference = null;
		for (int i = 0; i < this.displayInfos.Length; i++)
		{
			if (i < badgeGroup.BadgeIDs.Length)
			{
				int value;
				earnedBadges.TryGetValue(badgeGroup.BadgeIDs[i], out value);
				UIManager.SetGameObjectActive(this.displayInfos[i].m_Container, true, null);
				this.displayInfos[i].m_count.text = "x" + UIStorePanel.FormatIntToString(value, true);
				this.displayInfos[i].m_count.raycastTarget = false;
				GameBalanceVars.GameResultBadge badgeInfo = GameResultBadgeData.Get().GetBadgeInfo(badgeGroup.BadgeIDs[i]);
				this.displayInfos[i].m_Icon.sprite = Resources.Load<Sprite>(badgeInfo.BadgeIconString);
				UIManager.SetGameObjectActive(this.displayInfos[i].m_badgeHitbox, false, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.displayInfos[i].m_Container, false, null);
			}
		}
		this.m_name.text = StringUtil.TR_BadgeGroupName(Index);
	}

	public void Setup(GameBalanceVars.GameResultBadge badge, int count)
	{
		this.m_name.text = StringUtil.TR_BadgeName(badge.UniqueBadgeID);
		UIManager.SetGameObjectActive(this.displayInfos[0].m_Container, true, null);
		this.displayInfos[0].m_Icon.sprite = Resources.Load<Sprite>(badge.BadgeIconString);
		this.displayInfos[0].m_count.text = "x" + UIStorePanel.FormatIntToString(count, true);
		this.displayInfos[0].m_count.raycastTarget = false;
		UIManager.SetGameObjectActive(this.displayInfos[0].m_badgeHitbox, false, null);
		for (int i = 1; i < this.displayInfos.Length; i++)
		{
			UIManager.SetGameObjectActive(this.displayInfos[i].m_Container, false, null);
		}
		this.m_groupReference = null;
		this.m_groupIndex = 0;
		this.m_badgeReference = badge;
	}

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
			if (this.Quality == GameBalanceVars.GameResultBadge.BadgeQuality.Gold)
			{
				text2 = HUD_UIResources.ColorToHex(GameResultBadgeData.Get().BadgeGroupGoldRequirementColorHex);
			}
			else if (this.Quality == GameBalanceVars.GameResultBadge.BadgeQuality.Silver)
			{
				text2 = HUD_UIResources.ColorToHex(GameResultBadgeData.Get().BadgeGroupSilverRequirementColorHex);
			}
			else if (this.Quality == GameBalanceVars.GameResultBadge.BadgeQuality.Bronze)
			{
				text2 = HUD_UIResources.ColorToHex(GameResultBadgeData.Get().BadgeGroupBronzeRequirementColorHex);
			}
			if (!text2.IsNullOrEmpty())
			{
				text += string.Format("<color={0}>{2}</color>: {1}", text2, this.Requirements, StringUtil.TR(this.Quality.ToString(), "BadgeQuality"));
			}
			if (this.Requirements.IsNullOrEmpty())
			{
				text += "Need to fill out group requirement description in SPECIFIC badges";
			}
			return text;
		}
	}
}
