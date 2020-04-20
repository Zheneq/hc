using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverBadgeWidget : MonoBehaviour
{
	public Image m_BadgeIcon;

	private GameBalanceVars.GameResultBadge m_BadgeInfo;

	public GameBalanceVars.GameResultBadge BadgeInfo
	{
		get
		{
			return this.m_BadgeInfo;
		}
	}

	public void Setup(BadgeInfo badgeInfo, CharacterType characterType, Dictionary<StatDisplaySettings.StatType, PercentileInfo> percentiles)
	{
		this.m_BadgeInfo = GameBalanceVars.Get().GetGameBadge(badgeInfo.BadgeId);
		if (this.m_BadgeInfo != null)
		{
			this.m_BadgeIcon.sprite = (Sprite)Resources.Load(this.m_BadgeInfo.BadgeIconString, typeof(Sprite));
			if (base.gameObject.GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<UITooltipHoverObject>() == null)
			{
				base.gameObject.GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<UITooltipHoverObject>();
			}
			UITooltipHoverObject component = base.gameObject.GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<UITooltipHoverObject>();
			if (component != null)
			{
				component.Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
				{
					UITitledTooltip uititledTooltip = (UITitledTooltip)tooltip;
					string text = GameResultBadgeData.GetBadgeDescription(this.m_BadgeInfo, characterType);
					if (this.m_BadgeInfo.ComparisonGroup == GameBalanceVars.GameResultBadge.ComparisonType.Global)
					{
						int? num = null;
						int? num2 = null;
						int? num3 = null;
						int? num4 = null;
						PercentileInfo percentileInfo;
						if (percentiles.TryGetValue(this.m_BadgeInfo.BadgePointCalcType, out percentileInfo))
						{
							num = percentileInfo.AgainstAll;
							num2 = percentileInfo.AgainstSameFreelancer;
							num3 = percentileInfo.AgainstRole;
							num4 = percentileInfo.AgainstPeers;
						}
						string text2 = text;
						string oldValue = "[GlobalPercentile]";
						string newValue;
						if (num != null)
						{
							newValue = num.Value.ToString();
						}
						else
						{
							newValue = "???";
						}
						text = text2.Replace(oldValue, newValue);
						string text3 = text;
						string oldValue2 = "[FreelancerPercentile]";
						string newValue2;
						if (num2 != null)
						{
							newValue2 = num2.Value.ToString();
						}
						else
						{
							newValue2 = "???";
						}
						text = text3.Replace(oldValue2, newValue2);
						string text4 = text;
						string oldValue3 = "[RolePercentile]";
						string newValue3;
						if (num3 != null)
						{
							newValue3 = num3.Value.ToString();
						}
						else
						{
							newValue3 = "???";
						}
						text = text4.Replace(oldValue3, newValue3);
						text = text.Replace("[PeerPercentile]", (num4 == null) ? "???" : num4.Value.ToString());
					}
					uititledTooltip.Setup(StringUtil.TR_BadgeName(this.m_BadgeInfo.UniqueBadgeID), text, string.Empty);
					if (UIGameOverScreen.Get() != null)
					{
						UIGameOverScreen.Get().NotifyWidgetMouseOver(this, true);
					}
					return true;
				}, delegate
				{
					if (UIGameOverScreen.Get() != null)
					{
						UIGameOverScreen.Get().NotifyWidgetMouseOver(this, false);
					}
				});
			}
		}
	}
}
