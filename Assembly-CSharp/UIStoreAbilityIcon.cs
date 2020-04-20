using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStoreAbilityIcon : MonoBehaviour
{
	public UITooltipHoverObject m_hoverObj;

	public TextMeshProUGUI m_hotkeyLabel;

	public Image m_background;

	public Image m_icon;

	private string m_tooltipTitle;

	private string m_tooltipDescription;

	private void Start()
	{
		this.m_hoverObj.Setup(TooltipType.Titled, new TooltipPopulateCall(this.SetupTooltip), null);
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (this.m_tooltipTitle.IsNullOrEmpty())
		{
			return false;
		}
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(this.m_tooltipTitle, this.m_tooltipDescription, string.Empty);
		return true;
	}

	public void SetupTooltip(Ability ability)
	{
		this.m_tooltipTitle = ability.GetNameString();
		this.m_tooltipDescription = ability.GetToolTipString(false);
	}
}
