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
		m_hoverObj.Setup(TooltipType.Titled, SetupTooltip);
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (m_tooltipTitle.IsNullOrEmpty())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(m_tooltipTitle, m_tooltipDescription, string.Empty);
		return true;
	}

	public void SetupTooltip(Ability ability)
	{
		m_tooltipTitle = ability.GetNameString();
		m_tooltipDescription = ability.GetToolTipString();
	}
}
