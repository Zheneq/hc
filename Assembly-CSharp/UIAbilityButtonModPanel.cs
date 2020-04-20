using System;
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
		if (this.m_disabled != null)
		{
			UIManager.SetGameObjectActive(this.m_disabled, false, null);
		}
		this.m_selectedModIndex = -1;
		this.m_selectedVfxSwapIndex = 0;
		this.m_buttonHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, new TooltipPopulateCall(this.ShowAbilityTooltip), null);
	}

	public bool ShowAbilityTooltip(UITooltipBase tooltip)
	{
		if (this.m_theAbility != null)
		{
			if (!(this.m_theAbility.ability == null))
			{
				UIAbilityTooltip uiabilityTooltip = (UIAbilityTooltip)tooltip;
				string movieAssetName = "Video/AbilityPreviews/" + this.m_theAbility.ability.m_previewVideo;
				uiabilityTooltip.Setup(this.m_theAbility.ability, this.m_selectedMod, movieAssetName);
				return true;
			}
		}
		return false;
	}

	public AbilityData.AbilityEntry GetAbilityEntry()
	{
		return this.m_theAbility;
	}

	public AbilityMod GetSelectedMod()
	{
		return this.m_selectedMod;
	}

	public void SetSelectedMod(AbilityMod theMod)
	{
		this.m_selectedMod = theMod;
		if (this.m_selectedMod != null)
		{
			this.m_selectedModIcon.sprite = this.m_selectedMod.m_iconSprite;
			UIManager.SetGameObjectActive(this.m_selectedModIcon, true, null);
		}
		else
		{
			this.m_selectedModIcon.sprite = null;
			UIManager.SetGameObjectActive(this.m_selectedModIcon, false, null);
		}
	}

	public int GetSelectedModIndex()
	{
		return this.m_selectedModIndex;
	}

	public void SetSelectedModIndex(int modIndex)
	{
		this.m_selectedModIndex = modIndex;
	}

	public CharacterAbilityVfxSwap GetSelectedVfxSwap()
	{
		return this.m_selectedVfxSwap;
	}

	public void SetSelectedVfxSwap(CharacterAbilityVfxSwap theSwap)
	{
		this.m_selectedVfxSwap = theSwap;
	}

	public int GetSelectedVfxSwapIndex()
	{
		return this.m_selectedVfxSwapIndex;
	}

	public void SetSelectedVfxSwapIndex(int vfxSwapIndex)
	{
		this.m_selectedVfxSwapIndex = vfxSwapIndex;
	}

	private void SetHotKeyText(string text)
	{
		for (int i = 0; i < this.m_hotkey.Length; i++)
		{
			this.m_hotkey[i].text = text;
		}
	}

	public void RefreshHotkey()
	{
		if (this.m_theAbility != null)
		{
			this.m_theAbility.InitHotkey();
			this.SetHotKeyText(this.m_theAbility.hotkey);
		}
	}

	private void SetAbilityIconSprite(Sprite sprite)
	{
		for (int i = 0; i < this.m_abilityIcon.Length; i++)
		{
			this.m_abilityIcon[i].sprite = sprite;
		}
	}

	public void Setup(AbilityData.AbilityEntry theAbilityEntry)
	{
		this.m_theAbility = theAbilityEntry;
		if (this.m_theAbility != null)
		{
			if (this.m_theAbility.ability != null)
			{
				UIManager.SetGameObjectActive(base.gameObject, true, null);
				this.SetHotKeyText(theAbilityEntry.hotkey.ToString());
				this.SetAbilityIconSprite(theAbilityEntry.ability.sprite);
				UIManager.SetGameObjectActive(this.m_freeActionIcon, theAbilityEntry.ability.IsFreeAction(), null);
				int num = UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(theAbilityEntry.ability.RunPriority));
				for (int i = 0; i < this.m_phaseIndicators.Length; i++)
				{
					UIManager.SetGameObjectActive(this.m_phaseIndicators[i], i == num, null);
				}
				Color color;
				if (theAbilityEntry.ability.GetPhaseString() == StringUtil.TR("Blast", "Global"))
				{
					color = this.m_blastColor;
				}
				else if (theAbilityEntry.ability.GetPhaseString() == StringUtil.TR("Dash", "Global"))
				{
					color = this.m_dashColor;
				}
				else if (theAbilityEntry.ability.GetPhaseString() == StringUtil.TR("Prep", "Global"))
				{
					color = this.m_prepColor;
				}
				else
				{
					color = this.m_blastColor;
				}
				this.m_defaultBG.color = new Color(color.r, color.g, color.b, this.m_bgAlpha);
				this.m_selectedBG.color = new Color(color.r, color.g, color.b, this.m_selBgAlpha);
				return;
			}
		}
		UIManager.SetGameObjectActive(base.gameObject, false, null);
	}

	public void SetCallback(_ButtonSwapSprite.ButtonClickCallback callbackFunc)
	{
		this.m_buttonHitBox.callback = callbackFunc;
	}

	public void SetSelected(bool selected, bool forceAnimation = false)
	{
		if (this.m_selectBtn != null)
		{
			this.m_selectBtn.SetSelected(selected, forceAnimation, string.Empty, string.Empty);
		}
	}
}
