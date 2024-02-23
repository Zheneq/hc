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
		get { return m_IsSelected; }
	}

	protected virtual void Start()
	{
		GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, TooltipSetup);
	}

	public void SetSelected(bool selected)
	{
		m_IsSelected = selected;
		UIManager.SetGameObjectActive(m_selected, selected);
	}

	public void SetMainSelected(bool visible)
	{
		UIManager.SetGameObjectActive(m_selectedMain, visible);
	}

	private bool TooltipSetup(UITooltipBase tooltip)
	{
		if (!string.IsNullOrEmpty(m_unlockTooltipTitle))
		{
			UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
			uITitledTooltip.Setup(m_unlockTooltipTitle, (m_unlockTooltipText == null) ? string.Empty : m_unlockTooltipText, string.Empty);
			return true;
		}
		return false;
	}
}
