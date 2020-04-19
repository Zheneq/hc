using System;
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

	public CharacterResourceLink m_charLink { get; private set; }

	public int m_tauntIndex { get; private set; }

	public void Start()
	{
		this.m_parent = base.GetComponentInParent<UICharacterTauntsPanel>();
		this.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnClick);
		this.m_hitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.TauntPreview, new TooltipPopulateCall(this.SetupTooltip), null);
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (this.m_tauntVideoPath.Length > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITauntButton.SetupTooltip(UITooltipBase)).MethodHandle;
			}
			UIFrontendTauntMouseoverVideo uifrontendTauntMouseoverVideo = tooltip as UIFrontendTauntMouseoverVideo;
			uifrontendTauntMouseoverVideo.Setup("Video/taunts/" + this.m_tauntVideoPath);
			return true;
		}
		return false;
	}

	private void OnClick(BaseEventData data)
	{
		this.m_parent.Select(this);
	}

	public void Setup(CharacterResourceLink charLink, int tauntIndex, AbilityData abilityData, bool isUnlocked)
	{
		this.m_tauntUnlockData = GameBalanceVars.Get().GetCharacterUnlockData(charLink.m_characterType).tauntUnlockData[tauntIndex];
		this.m_charLink = charLink;
		this.m_tauntIndex = tauntIndex;
		this.m_isUnlocked = isUnlocked;
		CharacterTaunt characterTaunt = charLink.m_taunts[tauntIndex];
		this.SetImageArraySprite(this.m_icons, this.GetTauntSprite(characterTaunt, abilityData));
		UIManager.SetGameObjectActive(this.m_locked, !isUnlocked, null);
		string colorHexString = this.m_tauntUnlockData.Rarity.GetColorHexString();
		string text = string.Concat(new string[]
		{
			"<color=",
			colorHexString,
			">",
			charLink.GetTauntName(tauntIndex),
			"</color>"
		});
		this.SetTextArrayText(this.m_nameTexts, text);
		string text2 = string.Format(StringUtil.TR("TauntFor", "Global"), this.GetTauntAbilityName(characterTaunt, abilityData));
		if (!this.m_tauntUnlockData.ObtainedDescription.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITauntButton.Setup(CharacterResourceLink, int, AbilityData, bool)).MethodHandle;
			}
			text2 = text2 + Environment.NewLine + this.m_tauntUnlockData.GetObtainedDescription();
		}
		if (!characterTaunt.m_flavorText.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			string text3 = text2;
			text2 = string.Concat(new string[]
			{
				text3,
				Environment.NewLine,
				"<i>",
				characterTaunt.m_flavorText,
				"</i>"
			});
		}
		this.SetTextArrayText(this.m_descriptionTexts, text2);
		if (!isUnlocked)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			string text4 = "<sprite name=iso>" + this.GetIsoCost();
			if (characterTaunt.m_obtainedText.Trim().IsNullOrEmpty())
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				int unlockCharacterLevel = this.m_tauntUnlockData.GetUnlockCharacterLevel(charLink.m_characterType, false);
				if (unlockCharacterLevel > 0)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					text4 = string.Format(StringUtil.TR("UnlocksAtLevel", "Global"), unlockCharacterLevel);
				}
			}
			this.SetTextArrayText(this.m_costTexts, text4);
		}
		else
		{
			this.SetTextArrayText(this.m_costTexts, string.Empty);
		}
		this.m_hitbox.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
		this.m_tauntVideoPath = characterTaunt.m_tauntVideoPath;
	}

	private Sprite GetTauntSprite(CharacterTaunt characterTaunt, AbilityData abilityData)
	{
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITauntButton.GetTauntSprite(CharacterTaunt, AbilityData)).MethodHandle;
			}
			return abilityData.m_sprite0;
		}
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_1)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			return abilityData.m_sprite1;
		}
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_2)
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
			return abilityData.m_sprite2;
		}
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_3)
		{
			return abilityData.m_sprite3;
		}
		if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_4)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			return abilityData.m_sprite4;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITauntButton.GetTauntAbilityName(CharacterTaunt, AbilityData)).MethodHandle;
			}
			ability = abilityData.m_ability0;
		}
		else if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_1)
		{
			ability = abilityData.m_ability1;
		}
		else if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_2)
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
			ability = abilityData.m_ability2;
		}
		else if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_3)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			ability = abilityData.m_ability3;
		}
		else if (characterTaunt.m_actionForTaunt == AbilityData.ActionType.ABILITY_4)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			ability = abilityData.m_ability4;
		}
		else if (Application.isEditor)
		{
			Log.Warning("Trying to get taunt index for " + characterTaunt.m_actionForTaunt.ToString(), new object[0]);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UITauntButton.SetTextArrayText(TextMeshProUGUI[], string)).MethodHandle;
		}
	}

	private void SetImageArraySprite(Image[] imgArray, Sprite sprite)
	{
		for (int i = 0; i < imgArray.Length; i++)
		{
			imgArray[i].sprite = sprite;
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UITauntButton.SetImageArraySprite(Image[], Sprite)).MethodHandle;
		}
	}

	public bool IsUnlocked()
	{
		return this.m_isUnlocked;
	}

	public int GetIsoCost()
	{
		return this.m_tauntUnlockData.GetUnlockISOPrice();
	}

	public void SetUnlocked()
	{
		this.m_isUnlocked = true;
		UIManager.SetGameObjectActive(this.m_locked, false, null);
		this.SetTextArrayText(this.m_costTexts, string.Empty);
	}
}
