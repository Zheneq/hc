using System;
using UnityEngine;
using UnityEngine.UI;

public class UIRankedModeDraftCharacterEntry : MonoBehaviour
{
	public RectTransform[] m_currentlySelectingObjects;

	public RectTransform m_noCharacterImage;

	public Image m_browseCharacterImage;

	public Image m_selectCharacterImage;

	private bool m_selectingStatus;

	protected CharacterResourceLink m_linkRef;

	public CharacterType GetSelectedCharacter()
	{
		if (this.m_linkRef != null)
		{
			return this.m_linkRef.m_characterType;
		}
		return CharacterType.None;
	}

	public void SetBrowseCharacterImageVisible(bool visible)
	{
		if (visible)
		{
			UIManager.SetGameObjectActive(this.m_noCharacterImage, false, null);
		}
		UIManager.SetGameObjectActive(this.m_browseCharacterImage, visible, null);
	}

	public void SetSelectedCharacterImageVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_selectCharacterImage, visible, null);
	}

	public void SetCharacter(CharacterResourceLink link)
	{
		this.m_linkRef = link;
		this.m_selectCharacterImage.sprite = link.GetCharacterSelectIcon();
	}

	public void SetHoverCharacter(CharacterResourceLink link)
	{
		this.m_browseCharacterImage.sprite = link.GetCharacterSelectIcon();
	}

	public void Init()
	{
		this.m_linkRef = null;
		UIManager.SetGameObjectActive(this.m_noCharacterImage, true, null);
		UIManager.SetGameObjectActive(this.m_browseCharacterImage, false, null);
		UIManager.SetGameObjectActive(this.m_selectCharacterImage, false, null);
		this.DoSelecting(false);
	}

	private void DoSelecting(bool selecting)
	{
		this.m_selectingStatus = selecting;
		for (int i = 0; i < this.m_currentlySelectingObjects.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_currentlySelectingObjects[i], selecting, null);
		}
	}

	public void SetAsSelecting(bool selecting)
	{
		if (this.m_selectingStatus != selecting)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftCharacterEntry.SetAsSelecting(bool)).MethodHandle;
			}
			this.DoSelecting(selecting);
		}
	}
}
