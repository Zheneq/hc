using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITauntButton : MonoBehaviour
{
	public _ButtonSwapSprite m_hitbox;

	public Image m_locked;

	public Image[] m_icons;

	public TextMeshProUGUI[] m_nameTexts;

	public TextMeshProUGUI[] m_descriptionTexts;

	public TextMeshProUGUI[] m_costTexts;

	private GameBalanceVars.TauntUnlockData m_tauntUnlockData;

	private bool m_isUnlocked;

	private string m_tauntVideoPath = string.Empty;

	private UICharacterTauntsPanel m_parent;

	private const string c_tauntVideoDir = "Video/taunts/";

	public CharacterResourceLink m_charLink
	{
		get;
		private set;
	}

	public int m_tauntIndex
	{
		get;
		private set;
	}

	public void Start()
	{
		m_parent = GetComponentInParent<UICharacterTauntsPanel>();
		m_hitbox.callback = OnClick;
		m_hitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.TauntPreview, SetupTooltip);
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (m_tauntVideoPath.Length > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					UIFrontendTauntMouseoverVideo uIFrontendTauntMouseoverVideo = tooltip as UIFrontendTauntMouseoverVideo;
					uIFrontendTauntMouseoverVideo.Setup(new StringBuilder().Append("Video/taunts/").Append(m_tauntVideoPath).ToString());
					return true;
				}
				}
			}
		}
		return false;
	}

	private void OnClick(BaseEventData data)
	{
		m_parent.Select(this);
	}

	public void Setup(CharacterResourceLink charLink, int tauntIndex, AbilityData abilityData, bool isUnlocked)
	{
		m_tauntUnlockData = GameBalanceVars.Get().GetCharacterUnlockData(charLink.m_characterType).tauntUnlockData[tauntIndex];
		m_charLink = charLink;
		m_tauntIndex = tauntIndex;
		m_isUnlocked = isUnlocked;
		CharacterTaunt characterTaunt = charLink.m_taunts[tauntIndex];
		SetImageArraySprite(m_icons, GetTauntSprite(characterTaunt, abilityData));
		UIManager.SetGameObjectActive(m_locked, !isUnlocked);
		string colorHexString = m_tauntUnlockData.Rarity.GetColorHexString();
		string text = new StringBuilder().Append("<color=").Append(colorHexString).Append(">").Append(charLink.GetTauntName(tauntIndex)).Append("</color>").ToString();
		SetTextArrayText(m_nameTexts, text);
		string text2 = string.Format(StringUtil.TR("TauntFor", "Global"), GetTauntAbilityName(characterTaunt, abilityData));
		if (!m_tauntUnlockData.ObtainedDescription.IsNullOrEmpty())
		{
			text2 = new StringBuilder().AppendLine(text2).Append(m_tauntUnlockData.GetObtainedDescription()).ToString();
		}
		if (!characterTaunt.m_flavorText.IsNullOrEmpty())
		{
			string text3 = text2;
			text2 = new StringBuilder().AppendLine(text3).Append("<i>").Append(characterTaunt.m_flavorText).Append("</i>").ToString();
		}
		SetTextArrayText(m_descriptionTexts, text2);
		if (!isUnlocked)
		{
			string text4 = new StringBuilder().Append("<sprite name=iso>").Append(GetIsoCost()).ToString();
			if (characterTaunt.m_obtainedText.Trim().IsNullOrEmpty())
			{
				int unlockCharacterLevel = m_tauntUnlockData.GetUnlockCharacterLevel(charLink.m_characterType);
				if (unlockCharacterLevel > 0)
				{
					text4 = string.Format(StringUtil.TR("UnlocksAtLevel", "Global"), unlockCharacterLevel);
				}
			}
			SetTextArrayText(m_costTexts, text4);
		}
		else
		{
			SetTextArrayText(m_costTexts, string.Empty);
		}
		m_hitbox.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
		m_tauntVideoPath = characterTaunt.m_tauntVideoPath;
	}

	private Sprite GetTauntSprite(CharacterTaunt characterTaunt, AbilityData abilityData)
	{
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return abilityData.m_sprite0;
				}
			}
		}
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_1)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return abilityData.m_sprite1;
				}
			}
		}
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_2)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return abilityData.m_sprite2;
				}
			}
		}
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_3)
		{
			return abilityData.m_sprite3;
		}
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_4)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return abilityData.m_sprite4;
				}
			}
		}
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_5)
		{
			return abilityData.m_sprite5;
		}
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_6)
		{
			return abilityData.m_sprite6;
		}
		return null;
	}

	private string GetTauntAbilityName(CharacterTaunt characterTaunt, AbilityData abilityData)
	{
		Ability ability = null;
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_0)
		{
			ability = abilityData.m_ability0;
		}
		else if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_1)
		{
			ability = abilityData.m_ability1;
		}
		else if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_2)
		{
			ability = abilityData.m_ability2;
		}
		else if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_3)
		{
			ability = abilityData.m_ability3;
		}
		else if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_4)
		{
			ability = abilityData.m_ability4;
		}
		else if (Application.isEditor)
		{
			Log.Warning(new StringBuilder().Append("Trying to get taunt index for ").Append(characterTaunt.m_actionForTaunt).ToString());
		}
		if (ability != null)
		{
			return ability.GetNameString();
		}
		return string.Empty;
	}

	private void SetTextArrayText(TextMeshProUGUI[] tmpArray, string text)
	{
		for (int i = 0; i < tmpArray.Length; i++)
		{
			tmpArray[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	private void SetImageArraySprite(Image[] imgArray, Sprite sprite)
	{
		for (int i = 0; i < imgArray.Length; i++)
		{
			imgArray[i].sprite = sprite;
		}
		while (true)
		{
			return;
		}
	}

	public bool IsUnlocked()
	{
		return m_isUnlocked;
	}

	public int GetIsoCost()
	{
		return m_tauntUnlockData.GetUnlockISOPrice();
	}

	public void SetUnlocked()
	{
		m_isUnlocked = true;
		UIManager.SetGameObjectActive(m_locked, false);
		SetTextArrayText(m_costTexts, string.Empty);
	}
}
