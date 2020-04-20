using System;
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
		return this.m_swapReference;
	}

	public void SetVfxSwap(CharacterAbilityVfxSwap theSwap, int swapNumber)
	{
		this.m_swapReference = theSwap;
		if (theSwap != null)
		{
			UIManager.SetGameObjectActive(base.gameObject, true, null);
		}
		for (int i = 0; i < this.m_labels.Length; i++)
		{
			this.m_labels[i].text = swapNumber.ToString();
		}
	}

	public void SetAsUnselected(UIVfxSwapSelectButton referenceButton)
	{
		this.m_swapReference = null;
		UIManager.SetGameObjectActive(base.gameObject, true, null);
	}

	public void SetCallback(_ButtonSwapSprite.ButtonClickCallback callbackFunc)
	{
		this.m_buttonHitBox.callback = callbackFunc;
	}

	public void SetSelected(bool selected, bool forceAnimation = false)
	{
		if (this.m_SelectedContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_SelectedContainer, selected, null);
		}
		if (this.m_selectBtn != null)
		{
			this.m_selectBtn.SetSelected(selected, forceAnimation, string.Empty, string.Empty);
		}
	}

	public void SetLocked(bool isLocked)
	{
		this.m_lockVisible = isLocked;
		this.UpdateLocked();
	}

	public void UpdateLocked()
	{
		foreach (RectTransform component in this.m_lockIcon)
		{
			UIManager.SetGameObjectActive(component, this.m_lockVisible, null);
		}
	}
}
