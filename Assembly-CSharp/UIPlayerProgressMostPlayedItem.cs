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
		UIManager.SetGameObjectActive(base.gameObject, true);
		if (charData != null)
		{
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charData.CharacterType);
			CharacterVisualInfo lastSkin = charData.CharacterComponent.LastSkin;
			int num = lastSkin.skinIndex;
			if (num >= characterResourceLink.m_skins.Count)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = 0;
			}
			string skinSelectionIconPath = characterResourceLink.m_skins[num].m_skinSelectionIconPath;
			m_heroImage.sprite = (Sprite)Resources.Load(skinSelectionIconPath, typeof(Sprite));
			m_name.text = characterResourceLink.GetDisplayName();
			m_matchesPlayed.text = string.Format(StringUtil.TR("MatchesPlayed", "Global"), charData.ExperienceComponent.Matches.ToString());
			UIManager.SetGameObjectActive(m_heroImage, true);
			UIManager.SetGameObjectActive(m_matchesPlayed, true);
			UIManager.SetGameObjectActive(m_name, true);
		}
		else
		{
			UIManager.SetGameObjectActive(m_heroImage, false);
			UIManager.SetGameObjectActive(m_matchesPlayed, false);
			UIManager.SetGameObjectActive(m_name, false);
		}
	}
}
