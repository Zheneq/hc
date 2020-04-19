using System;
using UnityEngine;
using UnityEngine.UI;

public class UIRatingButton : MonoBehaviour
{
	public Image m_background;

	public Text m_numberLabel;

	public _ButtonSwapSprite m_hitBox;

	public Text m_ratingDescription;

	public Image m_selectedImage;

	private void Awake()
	{
		UIManager.SetGameObjectActive(this.m_selectedImage, false, null);
		this.m_hitBox.m_ignoreDialogboxes = true;
	}

	public void SetSelected(bool selected)
	{
		UIManager.SetGameObjectActive(this.m_selectedImage, selected, null);
	}
}
