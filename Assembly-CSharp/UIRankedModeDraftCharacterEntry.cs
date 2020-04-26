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
		if (m_linkRef != null)
		{
			return m_linkRef.m_characterType;
		}
		return CharacterType.None;
	}

	public void SetBrowseCharacterImageVisible(bool visible)
	{
		if (visible)
		{
			UIManager.SetGameObjectActive(m_noCharacterImage, false);
		}
		UIManager.SetGameObjectActive(m_browseCharacterImage, visible);
	}

	public void SetSelectedCharacterImageVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_selectCharacterImage, visible);
	}

	public void SetCharacter(CharacterResourceLink link)
	{
		m_linkRef = link;
		m_selectCharacterImage.sprite = link.GetCharacterSelectIcon();
	}

	public void SetHoverCharacter(CharacterResourceLink link)
	{
		m_browseCharacterImage.sprite = link.GetCharacterSelectIcon();
	}

	public void Init()
	{
		m_linkRef = null;
		UIManager.SetGameObjectActive(m_noCharacterImage, true);
		UIManager.SetGameObjectActive(m_browseCharacterImage, false);
		UIManager.SetGameObjectActive(m_selectCharacterImage, false);
		DoSelecting(false);
	}

	private void DoSelecting(bool selecting)
	{
		m_selectingStatus = selecting;
		for (int i = 0; i < m_currentlySelectingObjects.Length; i++)
		{
			UIManager.SetGameObjectActive(m_currentlySelectingObjects[i], selecting);
		}
	}

	public void SetAsSelecting(bool selecting)
	{
		if (m_selectingStatus == selecting)
		{
			return;
		}
		while (true)
		{
			DoSelecting(selecting);
			return;
		}
	}
}
