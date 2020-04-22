using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilityButtonModPanel : MonoBehaviour
{
	public TextMeshProUGUI[] m_hotkey;

	public Image[] m_abilityIcon;

	public Image m_defaultBG;

	public Image m_selectedModIcon;

	public Image m_disabled;

	public Image m_selectedBG;

	public Image m_selectedFG;

	public RectTransform m_freeActionIcon;

	public Image[] m_phaseIndicators;

	public _SelectableBtn m_selectBtn;

	public _ButtonSwapSprite m_buttonHitBox;

	public Color m_blastColor;

	public Color m_dashColor;

	public Color m_prepColor;

	public float m_bgAlpha;

	public float m_selBgAlpha;

	private AbilityData.AbilityEntry m_theAbility;

	private AbilityMod m_selectedMod;

	private int m_selectedModIndex;

	private CharacterAbilityVfxSwap m_selectedVfxSwap;

	private int m_selectedVfxSwapIndex;

	private void Start()
	{
		if (m_disabled != null)
		{
			while (true)
			{
				switch (7)
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
			UIManager.SetGameObjectActive(m_disabled, false);
		}
		m_selectedModIndex = -1;
		m_selectedVfxSwapIndex = 0;
		m_buttonHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, ShowAbilityTooltip);
	}

	public bool ShowAbilityTooltip(UITooltipBase tooltip)
	{
		if (m_theAbility != null)
		{
			while (true)
			{
				switch (3)
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
			if (!(m_theAbility.ability == null))
			{
				UIAbilityTooltip uIAbilityTooltip = (UIAbilityTooltip)tooltip;
				string movieAssetName = "Video/AbilityPreviews/" + m_theAbility.ability.m_previewVideo;
				uIAbilityTooltip.Setup(m_theAbility.ability, m_selectedMod, movieAssetName);
				return true;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public AbilityData.AbilityEntry GetAbilityEntry()
	{
		return m_theAbility;
	}

	public AbilityMod GetSelectedMod()
	{
		return m_selectedMod;
	}

	public void SetSelectedMod(AbilityMod theMod)
	{
		m_selectedMod = theMod;
		if (m_selectedMod != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_selectedModIcon.sprite = m_selectedMod.m_iconSprite;
					UIManager.SetGameObjectActive(m_selectedModIcon, true);
					return;
				}
			}
		}
		m_selectedModIcon.sprite = null;
		UIManager.SetGameObjectActive(m_selectedModIcon, false);
	}

	public int GetSelectedModIndex()
	{
		return m_selectedModIndex;
	}

	public void SetSelectedModIndex(int modIndex)
	{
		m_selectedModIndex = modIndex;
	}

	public CharacterAbilityVfxSwap GetSelectedVfxSwap()
	{
		return m_selectedVfxSwap;
	}

	public void SetSelectedVfxSwap(CharacterAbilityVfxSwap theSwap)
	{
		m_selectedVfxSwap = theSwap;
	}

	public int GetSelectedVfxSwapIndex()
	{
		return m_selectedVfxSwapIndex;
	}

	public void SetSelectedVfxSwapIndex(int vfxSwapIndex)
	{
		m_selectedVfxSwapIndex = vfxSwapIndex;
	}

	private void SetHotKeyText(string text)
	{
		for (int i = 0; i < m_hotkey.Length; i++)
		{
			m_hotkey[i].text = text;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public void RefreshHotkey()
	{
		if (m_theAbility == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_theAbility.InitHotkey();
			SetHotKeyText(m_theAbility.hotkey);
			return;
		}
	}

	private void SetAbilityIconSprite(Sprite sprite)
	{
		for (int i = 0; i < m_abilityIcon.Length; i++)
		{
			m_abilityIcon[i].sprite = sprite;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public void Setup(AbilityData.AbilityEntry theAbilityEntry)
	{
		m_theAbility = theAbilityEntry;
		if (m_theAbility != null)
		{
			while (true)
			{
				switch (3)
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
			if (m_theAbility.ability != null)
			{
				UIManager.SetGameObjectActive(base.gameObject, true);
				SetHotKeyText(theAbilityEntry.hotkey.ToString());
				SetAbilityIconSprite(theAbilityEntry.ability.sprite);
				UIManager.SetGameObjectActive(m_freeActionIcon, theAbilityEntry.ability.IsFreeAction());
				int num = UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(theAbilityEntry.ability.RunPriority));
				for (int i = 0; i < m_phaseIndicators.Length; i++)
				{
					UIManager.SetGameObjectActive(m_phaseIndicators[i], i == num);
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					Color color;
					if (theAbilityEntry.ability.GetPhaseString() == StringUtil.TR("Blast", "Global"))
					{
						color = m_blastColor;
					}
					else if (theAbilityEntry.ability.GetPhaseString() == StringUtil.TR("Dash", "Global"))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						color = m_dashColor;
					}
					else if (theAbilityEntry.ability.GetPhaseString() == StringUtil.TR("Prep", "Global"))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						color = m_prepColor;
					}
					else
					{
						color = m_blastColor;
					}
					m_defaultBG.color = new Color(color.r, color.g, color.b, m_bgAlpha);
					m_selectedBG.color = new Color(color.r, color.g, color.b, m_selBgAlpha);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(base.gameObject, false);
	}

	public void SetCallback(_ButtonSwapSprite.ButtonClickCallback callbackFunc)
	{
		m_buttonHitBox.callback = callbackFunc;
	}

	public void SetSelected(bool selected, bool forceAnimation = false)
	{
		if (!(m_selectBtn != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_selectBtn.SetSelected(selected, forceAnimation, string.Empty, string.Empty);
			return;
		}
	}
}
