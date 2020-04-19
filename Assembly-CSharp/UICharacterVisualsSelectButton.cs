using System;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterVisualsSelectButton : MonoBehaviour
{
	public RectTransform m_selected;

	public RectTransform m_selectedMain;

	public Image m_lockedIcon;

	protected string m_unlockTooltipTitle;

	protected string m_unlockTooltipText;

	protected bool m_IsSelected;

	public bool isSelected
	{
		get
		{
			return this.m_IsSelected;
		}
	}

	protected virtual void Start()
	{
		base.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.TooltipSetup), null);
	}

	public void SetSelected(bool selected)
	{
		this.m_IsSelected = selected;
		UIManager.SetGameObjectActive(this.m_selected, selected, null);
	}

	public void SetMainSelected(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_selectedMain, visible, null);
	}

	private bool TooltipSetup(UITooltipBase tooltip)
	{
		if (!string.IsNullOrEmpty(this.m_unlockTooltipTitle))
		{
			UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
			uititledTooltip.Setup(this.m_unlockTooltipTitle, (this.m_unlockTooltipText == null) ? string.Empty : this.m_unlockTooltipText, string.Empty);
			return true;
		}
		return false;
	}
}
