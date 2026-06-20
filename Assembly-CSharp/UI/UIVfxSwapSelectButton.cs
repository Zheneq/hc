using TMPro;
using UnityEngine;

public class UIVfxSwapSelectButton : MonoBehaviour
{
	public RectTransform m_SelectedContainer;

	public RectTransform[] m_lockIcon;

	public TextMeshProUGUI[] m_labels;

	public _SelectableBtn m_selectBtn;

	public _ButtonSwapSprite m_buttonHitBox;

	private CharacterAbilityVfxSwap m_swapReference;

	private bool m_lockVisible;

	public CharacterAbilityVfxSwap GetSwap()
	{
		return m_swapReference;
	}

	public void SetVfxSwap(CharacterAbilityVfxSwap theSwap, int swapNumber)
	{
		m_swapReference = theSwap;
		if (theSwap != null)
		{
			UIManager.SetGameObjectActive(base.gameObject, true);
		}
		for (int i = 0; i < m_labels.Length; i++)
		{
			m_labels[i].text = swapNumber.ToString();
		}
	}

	public void SetAsUnselected(UIVfxSwapSelectButton referenceButton)
	{
		m_swapReference = null;
		UIManager.SetGameObjectActive(base.gameObject, true);
	}

	public void SetCallback(_ButtonSwapSprite.ButtonClickCallback callbackFunc)
	{
		m_buttonHitBox.callback = callbackFunc;
	}

	public void SetSelected(bool selected, bool forceAnimation = false)
	{
		if (m_SelectedContainer != null)
		{
			UIManager.SetGameObjectActive(m_SelectedContainer, selected);
		}
		if (!(m_selectBtn != null))
		{
			return;
		}
		while (true)
		{
			m_selectBtn.SetSelected(selected, forceAnimation, string.Empty, string.Empty);
			return;
		}
	}

	public void SetLocked(bool isLocked)
	{
		m_lockVisible = isLocked;
		UpdateLocked();
	}

	public void UpdateLocked()
	{
		RectTransform[] lockIcon = m_lockIcon;
		foreach (RectTransform component in lockIcon)
		{
			UIManager.SetGameObjectActive(component, m_lockVisible);
		}
	}
}
