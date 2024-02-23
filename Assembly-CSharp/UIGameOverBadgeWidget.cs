using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverBadgeWidget : MonoBehaviour
{
	public Image m_BadgeIcon;

	private GameBalanceVars.GameResultBadge m_BadgeInfo;

	public GameBalanceVars.GameResultBadge BadgeInfo
	{
		get { return m_BadgeInfo; }
	}

	public void Setup(BadgeInfo badgeInfo, CharacterType characterType, Dictionary<StatDisplaySettings.StatType, PercentileInfo> percentiles)
	{
		m_BadgeInfo = GameBalanceVars.Get().GetGameBadge(badgeInfo.BadgeId);
		if (m_BadgeInfo == null)
		{
			return;
		}
		m_BadgeIcon.sprite = (Sprite)Resources.Load(m_BadgeInfo.BadgeIconString, typeof(Sprite));
		if (gameObject.GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<UITooltipHoverObject>() == null)
		{
			gameObject.GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<UITooltipHoverObject>();
		}
		UITooltipHoverObject component = gameObject.GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<UITooltipHoverObject>();
		if (component == null)
		{
			return;
		}
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
				PercentileInfo value;
				if (percentiles.TryGetValue(m_BadgeInfo.BadgePointCalcType, out value))
				{
					num = value.AgainstAll;
					num2 = value.AgainstSameFreelancer;
					num3 = value.AgainstRole;
					num4 = value.AgainstPeers;
				}

				text = text.Replace("[GlobalPercentile]", num.HasValue ? num.Value.ToString() : "???");
				text = text.Replace("[FreelancerPercentile]", num2.HasValue ? num2.Value.ToString() : "???");
				text = text.Replace("[RolePercentile]", num3.HasValue ? num3.Value.ToString() : "???");
				text = text.Replace("[PeerPercentile]", (num4.HasValue) ? num4.Value.ToString() : "???");
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
				UIGameOverScreen.Get().NotifyWidgetMouseOver(this, false);
			}
		});
	}
}
