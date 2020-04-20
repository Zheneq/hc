using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerProgressMostPlayedItem : MonoBehaviour
{
	public Image m_heroImage;

	public TextMeshProUGUI m_matchesPlayed;

	public TextMeshProUGUI m_name;

	public void Setup(PersistedCharacterData charData)
	{
		UIManager.SetGameObjectActive(base.gameObject, true, null);
		if (charData != null)
		{
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charData.CharacterType);
			int num = charData.CharacterComponent.LastSkin.skinIndex;
			if (num >= characterResourceLink.m_skins.Count)
			{
				num = 0;
			}
			string skinSelectionIconPath = characterResourceLink.m_skins[num].m_skinSelectionIconPath;
			this.m_heroImage.sprite = (Sprite)Resources.Load(skinSelectionIconPath, typeof(Sprite));
			this.m_name.text = characterResourceLink.GetDisplayName();
			this.m_matchesPlayed.text = string.Format(StringUtil.TR("MatchesPlayed", "Global"), charData.ExperienceComponent.Matches.ToString());
			UIManager.SetGameObjectActive(this.m_heroImage, true, null);
			UIManager.SetGameObjectActive(this.m_matchesPlayed, true, null);
			UIManager.SetGameObjectActive(this.m_name, true, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_heroImage, false, null);
			UIManager.SetGameObjectActive(this.m_matchesPlayed, false, null);
			UIManager.SetGameObjectActive(this.m_name, false, null);
		}
	}
}
