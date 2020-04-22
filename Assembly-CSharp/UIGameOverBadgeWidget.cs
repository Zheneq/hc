using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverBadgeWidget : MonoBehaviour
{
	public Image m_BadgeIcon;

	private GameBalanceVars.GameResultBadge m_BadgeInfo;

	public GameBalanceVars.GameResultBadge BadgeInfo => m_BadgeInfo;

	public void Setup(BadgeInfo badgeInfo, CharacterType characterType, Dictionary<StatDisplaySettings.StatType, PercentileInfo> percentiles)
	{
		m_BadgeInfo = GameBalanceVars.Get().GetGameBadge(badgeInfo.BadgeId);
		if (m_BadgeInfo == null)
		{
			return;
		}
		while (true)
		{
			m_BadgeIcon.sprite = (Sprite)Resources.Load(m_BadgeInfo.BadgeIconString, typeof(Sprite));
			if (base.gameObject.GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<UITooltipHoverObject>() == null)
			{
				base.gameObject.GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<UITooltipHoverObject>();
			}
			UITooltipHoverObject component = base.gameObject.GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<UITooltipHoverObject>();
			if (component != null)
			{
				while (true)
				{
					component.Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
					{
						UITitledTooltip uITitledTooltip = (UITitledTooltip)tooltip;
						string text = GameResultBadgeData.GetBadgeDescription(m_BadgeInfo, characterType);
						if (m_BadgeInfo.ComparisonGroup == GameBalanceVars.GameResultBadge.ComparisonType.Global)
						{
							int? num = null;
							int? num2 = null;
							int? num3 = null;
							int? num4 = null;
							if (percentiles.TryGetValue(m_BadgeInfo.BadgePointCalcType, out PercentileInfo value))
							{
								num = value.AgainstAll;
								num2 = value.AgainstSameFreelancer;
								num3 = value.AgainstRole;
								num4 = value.AgainstPeers;
							}
							string text2 = text;
							object newValue;
							if (num.HasValue)
							{
								newValue = num.Value.ToString();
							}
							else
							{
								newValue = "???";
							}
							text = text2.Replace("[GlobalPercentile]", (string)newValue);
							string text3 = text;
							object newValue2;
							if (num2.HasValue)
							{
								newValue2 = num2.Value.ToString();
							}
							else
							{
								newValue2 = "???";
							}
							text = text3.Replace("[FreelancerPercentile]", (string)newValue2);
							string text4 = text;
							object newValue3;
							if (num3.HasValue)
							{
								newValue3 = num3.Value.ToString();
							}
							else
							{
								newValue3 = "???";
							}
							text = text4.Replace("[RolePercentile]", (string)newValue3);
							text = text.Replace("[PeerPercentile]", (!num4.HasValue) ? "???" : num4.Value.ToString());
						}
						uITitledTooltip.Setup(StringUtil.TR_BadgeName(m_BadgeInfo.UniqueBadgeID), text, string.Empty);
						if (UIGameOverScreen.Get() != null)
						{
							UIGameOverScreen.Get().NotifyWidgetMouseOver(this, true);
						}
						return true;
					}, delegate
					{
						if (UIGameOverScreen.Get() != null)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									UIGameOverScreen.Get().NotifyWidgetMouseOver(this, false);
									return;
								}
							}
						}
					});
					return;
				}
			}
			return;
		}
	}
}
