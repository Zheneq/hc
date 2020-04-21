using System;
using System.Collections;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterAbilitiesPanel : MonoBehaviour
{
	public _SelectableBtn m_resetAllMods;

	public UIAbilityButtonModPanel[] m_abilityButtons;

	public UICharacterAbiltiesPanelModLoadout m_modloadouts;

	public Color m_combatColor;

	public Color m_prepColor;

	public Color m_dashColor;

	[Space(10f)]
	public LayoutGroup m_modLayoutGroup;

	public ScrollRect m_modScrollView;

	public UIModSelectButton m_clearModButton;

	public UIModSelectButton m_modButtonPrefab;

	[Space(10f)]
	public UIVfxSwapSelectButton m_vfxButtonPrefab;

	public RectTransform m_vfxButtonContainer;

	public TextMeshProUGUI m_disabledLabel;

	public RectTransform m_modTokenLabel;

	public TextMeshProUGUI m_modTokenCountText;

	public _SelectableBtn m_purchaseModTokenBtn;

	public TextMeshProUGUI[] m_modTokenCostTexts;

	public Image[] m_modEquipPointsLeftNotches;

	public bool m_setZtoZero;

	public AbilityPreview m_abilityPreviewPanel;

	public bool m_canSelectMods = true;

	public UIPurchasePanel m_purchasePanel;

	public Animator m_NotEnoughPointsNotificationAC;

	private const string s_currencyToBuyModTokensSprite = "<sprite name=credit>";

	private const string c_videoDir = "Video/AbilityPreviews/";

	private List<UIVfxSwapSelectButton> m_vfxSwapButtons = new List<UIVfxSwapSelectButton>();

	private AbilityData.AbilityEntry[] m_abilities;

	[HideInInspector]
	public UIAbilityButtonModPanel m_selectedAbilityButton;

	private int m_selectedAbilityIndex;

	internal CharacterType m_selectedCharacter;

	private CharacterResourceLink m_characterLink;

	[HideInInspector]
	public CharacterModInfo m_modInfo;

	[HideInInspector]
	public CharacterAbilityVfxSwapInfo m_abilityVfxSwapInfo;

	private UIModSelectButton m_currentlyPurchasingModButton;

	private bool m_showingRankedMods;

	private List<UIModSelectButton> m_cachedModButtonList = new List<UIModSelectButton>();

	private void Awake()
	{
		this.InitModButtonListIfNeeded();
	}

	private void Start()
	{
		for (int i = 0; i < this.m_abilityButtons.Length; i++)
		{
			this.m_abilityButtons[i].SetCallback(new _ButtonSwapSprite.ButtonClickCallback(this.AbilityButtonSelected));
		}
		this.m_clearModButton.SetCallback(new _ButtonSwapSprite.ButtonClickCallback(this.ModButtonSelected));
		this.AddMouseEventPasser(this.m_clearModButton);
		if (this.m_resetAllMods != null)
		{
			this.m_resetAllMods.spriteController.callback = delegate(BaseEventData x)
			{
				ClientGameManager clientGameManager = ClientGameManager.Get();
				AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.HasResetMods;
				if (clientGameManager.IsPlayerAccountDataAvailable())
				{
					if (clientGameManager.GetPlayerAccountData().AccountComponent.GetUIState(uiState) > 0)
					{
						this.ResetAllMods(null);
						goto IL_B6;
					}
				}
				ClientGameManager.Get().RequestUpdateUIState(uiState, 1, null);
				UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("ResetMods", "Global"), StringUtil.TR("ResetModsConfirm", "Global"), StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), new UIDialogBox.DialogButtonCallback(this.ResetAllMods), null, false, false);
				IL_B6:
				this.m_resetAllMods.spriteController.ResetMouseState();
			};
		}
		UIManager.SetGameObjectActive(this.m_purchaseModTokenBtn, false, null);
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnModUnlocked += this.UICharacterSelectAbilitiesPanel_OnModUnlocked;
			ClientGameManager.Get().OnBankBalanceChange += this.UICharacterSelectAbilitiesPanel_OnBankBalanceChange;
			ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterDataUpdated;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange += this.OnLobbyGameplayOverridesUpdated;
		}
		this.ResetLockIcons();
		bool flag = false;
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameplayOverrides != null)
			{
				flag = GameManager.Get().GameplayOverrides.EnableMods;
			}
		}
		UIManager.SetGameObjectActive(this.m_modTokenCountText, false, null);
		if (this.m_modTokenLabel != null)
		{
			UIManager.SetGameObjectActive(this.m_modTokenLabel, false, null);
		}
		UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
		this.m_purchasePanel.m_freelancerCurrencyButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PurchaseMod);
		if (this.m_disabledLabel != null)
		{
			UIManager.SetGameObjectActive(this.m_disabledLabel, !flag, null);
		}
	}

	private void InitModButtonListIfNeeded()
	{
		if (this.m_cachedModButtonList.Count == 0)
		{
			IEnumerator enumerator = this.m_modLayoutGroup.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					UIModSelectButton component = transform.GetComponent<UIModSelectButton>();
					if (component != null && component.gameObject != this.m_clearModButton.gameObject)
					{
						component.SetCallback(new _ButtonSwapSprite.ButtonClickCallback(this.ModButtonSelected));
						this.AddMouseEventPasser(component);
						this.m_cachedModButtonList.Add(component);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	public void NotifyLoadoutUpdate(PlayerInfoUpdateResponse response)
	{
		if (this.m_modloadouts != null)
		{
			this.m_modloadouts.NotifyLoadoutUpdate(response);
		}
		if (response.CharacterInfo != null)
		{
			this.Setup(GameWideData.Get().GetCharacterResourceLink(response.CharacterInfo.CharacterType), true);
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnModUnlocked -= this.UICharacterSelectAbilitiesPanel_OnModUnlocked;
			ClientGameManager.Get().OnBankBalanceChange -= this.UICharacterSelectAbilitiesPanel_OnBankBalanceChange;
			ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterDataUpdated;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= this.OnLobbyGameplayOverridesUpdated;
		}
	}

	private void OnDisable()
	{
		if (this.m_purchasePanel != null)
		{
			UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
		}
	}

	public void RemoveHandleMessage()
	{
		ClientGameManager.Get().OnModUnlocked -= this.UICharacterSelectAbilitiesPanel_OnModUnlocked;
		ClientGameManager.Get().OnBankBalanceChange -= this.UICharacterSelectAbilitiesPanel_OnBankBalanceChange;
	}

	private void UICharacterSelectAbilitiesPanel_OnBankBalanceChange(CurrencyData newBalance)
	{
		this.ResetLockIcons();
		if (newBalance.Type == CurrencyType.FreelancerCurrency)
		{
			this.UpdatePurchaseModTokenButtonState();
		}
	}

	private void UICharacterSelectAbilitiesPanel_OnModUnlocked(CharacterType character, PlayerModData unlockData)
	{
		if (character == this.m_selectedCharacter)
		{
			foreach (UIModSelectButton uimodSelectButton in this.m_cachedModButtonList)
			{
				uimodSelectButton.SetLockIcons();
				if (ClientGameManager.Get().PurchasingMod && uimodSelectButton.GetMod() != null && uimodSelectButton.GetMod().m_abilityScopeId == ClientGameManager.Get().ModAttemptingToPurchase)
				{
					this.DoModButtonSelected(uimodSelectButton.m_buttonHitBox.gameObject);
				}
			}
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModUnlocked);
		}
	}

	public void OnCharacterDataUpdated(PersistedCharacterData data)
	{
		this.ResetLockIcons();
	}

	public void ResetLockIcons()
	{
		foreach (UIModSelectButton uimodSelectButton in this.m_cachedModButtonList)
		{
			uimodSelectButton.SetLockIcons();
		}
	}

	private void ResetAllMods(UIDialogBox boxReference)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModClear);
		for (int i = 0; i < this.m_abilityButtons.Length; i++)
		{
			this.m_abilityButtons[i].SetSelectedMod(null);
			this.m_abilityButtons[i].SetSelectedModIndex(-1);
			this.m_modInfo.SetModForAbility(i, -1);
		}
		using (List<UIModSelectButton>.Enumerator enumerator = this.m_cachedModButtonList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIModSelectButton uimodSelectButton = enumerator.Current;
				uimodSelectButton.SetSelected(false, false);
			}
		}
		this.m_clearModButton.SetSelected(true, false);
		AppState_CharacterSelect.Get().UpdateSelectedMods(this.m_modInfo);
		this.UpdateModCounter();
		this.UpdateModEquipPointsLeft(null);
	}

	internal void ShowOutOfModEquipPointsDialog()
	{
		UIManager.SetGameObjectActive(this.m_NotEnoughPointsNotificationAC, true, null);
	}

	public void symbol_001D(BaseEventData symbol_001D)
	{
		AbilityData.AbilityEntry abilityEntry = this.m_abilityButtons[this.m_selectedAbilityIndex].GetAbilityEntry();
		UIDialogPopupManager.OpenDebugModSelectionDialog(abilityEntry, this.m_selectedAbilityIndex, this, null);
	}

	public void PurchaseModToken(BaseEventData data)
	{
		if (GameBalanceVars.Get() != null && GameBalanceVars.Get().FreelancerCurrencyPerModToken > 0)
		{
			int numToPurchase = 1;
			int freelancerCurrencyPerModToken = GameBalanceVars.Get().FreelancerCurrencyPerModToken;
			int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.FreelancerCurrency);
			if (currentAmount >= freelancerCurrencyPerModToken)
			{
				Log.Info(Log.Category.UI, "Sending mod token purchase request", new object[0]);
				ClientGameManager.Get().PurchaseModToken(numToPurchase);
			}
			else
			{
				string description = string.Format(StringUtil.TR("InsufficientCurrencyModToken", "Global"), StringUtil.TR("FreelancerCurrency", "Global"), UIStorePanel.FormatIntToString(freelancerCurrencyPerModToken, true), UIStorePanel.FormatIntToString(currentAmount, true));
				UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("InsufficientCredits", "Global"), description, StringUtil.TR("Ok", "Global"), null, -1, false);
			}
		}
	}

	private void UpdatePurchaseModTokenButtonState()
	{
		UIManager.SetGameObjectActive(this.m_purchaseModTokenBtn, false, null);
	}

	public void RefreshSelectedMods()
	{
		if (!(ClientGameManager.Get() == null) && ClientGameManager.Get().GroupInfo != null)
		{
			if (this.m_selectedCharacter != CharacterType.None)
			{
				if (!this.m_selectedCharacter.IsWillFill())
				{
					if (this.m_selectedCharacter != CharacterType.PunchingDummy)
					{
						if (this.m_selectedCharacter != CharacterType.Last)
						{
							bool flag = AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked;
							CharacterModInfo characterModInfo;
							if (flag)
							{
								characterModInfo = ClientGameManager.Get().GetPlayerCharacterData(this.m_selectedCharacter).CharacterComponent.LastRankedMods;
							}
							else
							{
								characterModInfo = ClientGameManager.Get().GetPlayerCharacterData(this.m_selectedCharacter).CharacterComponent.LastMods;
							}
							for (int i = 0; i < this.m_abilityButtons.Length; i++)
							{
								if (i < this.m_abilities.Length)
								{
									AbilityData.AbilityEntry abilityEntry = this.m_abilities[i];
									Ability ability = abilityEntry.ability;
									int num = characterModInfo.GetModForAbility(i);
									if (!AbilityModHelper.IsModAllowed(this.m_characterLink.m_characterType, i, num))
									{
										num = -1;
									}
									AbilityMod abilityMod = AbilityModHelper.GetModForAbility(ability, num);
									if (abilityMod != null && !abilityMod.EquippableForGameType())
									{
										abilityMod = null;
										num = -1;
										characterModInfo.SetModForAbility(i, -1);
									}
									this.m_abilityButtons[i].SetSelectedMod(abilityMod);
									this.m_abilityButtons[i].SetSelectedModIndex(num);
								}
								else
								{
									this.m_abilityButtons[i].SetSelectedMod(null);
									this.m_abilityButtons[i].SetSelectedModIndex(-1);
								}
								this.m_abilityButtons[i].SetSelected(this.m_selectedAbilityButton == this.m_abilityButtons[i], false);
							}
							this.UpdateModCounter();
							this.UpdateModEquipPointsLeft(null);
							return;
						}
					}
				}
			}
		}
	}

	internal void UpdateModCounter()
	{
		if (!this.m_canSelectMods)
		{
			return;
		}
		UICharacterScreen.Get().UpdateModIcons(this.m_abilityButtons, this.m_prepColor, this.m_dashColor, this.m_combatColor);
	}

	public void DoModButtonSelected(GameObject gameObject)
	{
		UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameplayOverrides != null)
			{
				if (!GameManager.Get().GameplayOverrides.EnableMods)
				{
					return;
				}
			}
		}
		if (gameObject == null)
		{
			return;
		}
		UIModSelectButton component = gameObject.transform.parent.gameObject.GetComponent<UIModSelectButton>();
		if (component.AvailableForPurchase())
		{
			component.m_buttonHitBox.ForceSetPointerEntered(false);
			this.m_currentlyPurchasingModButton = component;
			GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
			int freelancerCurrencyToUnlockMod = gameBalanceVars.FreelancerCurrencyToUnlockMod;
			int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.FreelancerCurrency);
			this.m_purchasePanel.Setup(0, freelancerCurrencyToUnlockMod, 0f, true);
			UIManager.SetGameObjectActive(this.m_purchasePanel, true, null);
			this.m_purchasePanel.SetDisabled(freelancerCurrencyToUnlockMod > currentAmount);
			return;
		}
		if (!this.m_canSelectMods)
		{
			return;
		}
		if (!this.UnderTotalModEquipCost(component.GetMod()))
		{
			this.ShowOutOfModEquipPointsDialog();
			return;
		}
		UIModSelectButton uimodSelectButton = null;
		int selectedModIndex = -1;
		for (int i = 0; i < this.m_cachedModButtonList.Count; i++)
		{
			UIModSelectButton uimodSelectButton2 = this.m_cachedModButtonList[i];
			if (uimodSelectButton2.m_buttonHitBox.gameObject == gameObject)
			{
				uimodSelectButton2.SetSelected(true, false);
				uimodSelectButton = uimodSelectButton2;
				selectedModIndex = i;
			}
			else
			{
				uimodSelectButton2.SetSelected(false, false);
			}
		}
		if (this.m_clearModButton.m_buttonHitBox.gameObject == gameObject)
		{
			this.m_clearModButton.SetSelected(true, false);
			uimodSelectButton = null;
		}
		else
		{
			this.m_clearModButton.SetSelected(false, false);
		}
		if (this.m_selectedAbilityButton != null)
		{
			if (uimodSelectButton != null)
			{
				if (uimodSelectButton.GetMod() != null)
				{
					this.m_selectedAbilityButton.SetSelectedMod(uimodSelectButton.GetMod());
					this.m_selectedAbilityButton.SetSelectedModIndex(selectedModIndex);
					this.m_modInfo.SetModForAbility(this.m_selectedAbilityIndex, uimodSelectButton.GetMod().m_abilityScopeId);
					UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndEquipMod, null);
					goto IL_2C3;
				}
			}
			this.m_selectedAbilityButton.SetSelectedMod(null);
			this.m_selectedAbilityButton.SetSelectedModIndex(-1);
			this.m_modInfo.SetModForAbility(this.m_selectedAbilityIndex, -1);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModClear);
			IL_2C3:
			ClientGameManager.Get().UpdateSelectedMods(this.m_modInfo, 0);
		}
		this.UpdateModCounter();
		this.UpdateModEquipPointsLeft(null);
	}

	public void ModButtonSelected(BaseEventData data)
	{
		this.DoModButtonSelected(data.selectedObject);
	}

	public void RefreshSelectedVfxSwaps()
	{
		if (!(ClientGameManager.Get() == null))
		{
			if (ClientGameManager.Get().GroupInfo != null)
			{
				if (this.m_selectedCharacter != CharacterType.None)
				{
					if (!this.m_selectedCharacter.IsWillFill() && this.m_selectedCharacter != CharacterType.PunchingDummy)
					{
						if (this.m_selectedCharacter != CharacterType.Last)
						{
							CharacterResourceLink displayedCharacter = this.GetDisplayedCharacter();
							PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_selectedCharacter);
							CharacterAbilityVfxSwapInfo lastAbilityVfxSwaps = playerCharacterData.CharacterComponent.LastAbilityVfxSwaps;
							for (int i = 0; i < this.m_abilityButtons.Length; i++)
							{
								if (i < this.m_abilities.Length)
								{
									int abilityVfxSwapIdForAbility = lastAbilityVfxSwaps.GetAbilityVfxSwapIdForAbility(i);
									int selectedVfxSwapIndex = 0;
									CharacterAbilityVfxSwap selectedVfxSwap = null;
									if (abilityVfxSwapIdForAbility != 0)
									{
										bool flag = playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(i, abilityVfxSwapIdForAbility);
										if (flag)
										{
											List<CharacterAbilityVfxSwap> availableVfxSwapsForAbilityIndex = displayedCharacter.GetAvailableVfxSwapsForAbilityIndex(i);
											for (int j = 0; j < availableVfxSwapsForAbilityIndex.Count; j++)
											{
												CharacterAbilityVfxSwap characterAbilityVfxSwap = availableVfxSwapsForAbilityIndex[j];
												if (characterAbilityVfxSwap.m_uniqueID == abilityVfxSwapIdForAbility)
												{
													selectedVfxSwapIndex = j + 1;
													selectedVfxSwap = characterAbilityVfxSwap;
												}
											}
										}
									}
									this.m_abilityButtons[i].SetSelectedVfxSwap(selectedVfxSwap);
									this.m_abilityButtons[i].SetSelectedVfxSwapIndex(selectedVfxSwapIndex);
								}
								else
								{
									this.m_abilityButtons[i].SetSelectedVfxSwap(null);
									this.m_abilityButtons[i].SetSelectedVfxSwapIndex(0);
								}
							}
							return;
						}
					}
				}
			}
		}
	}

	private void CreateVfxButtonsIfNeeded(int totalButtons)
	{
		for (int i = this.m_vfxSwapButtons.Count; i < totalButtons; i++)
		{
			UIVfxSwapSelectButton newButton = UnityEngine.Object.Instantiate<UIVfxSwapSelectButton>(this.m_vfxButtonPrefab);
			newButton.SetCallback(new _ButtonSwapSprite.ButtonClickCallback(this.VfxSwapButtonSelected));
			newButton.m_buttonHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.TauntPreview, delegate(UITooltipBase tooltip)
			{
				CharacterAbilityVfxSwap swap = newButton.GetSwap();
				if (swap != null)
				{
					if (!swap.m_swapVideoPath.IsNullOrEmpty())
					{
						(tooltip as UIFrontendTauntMouseoverVideo).Setup("Video/AbilityPreviews/" + swap.m_swapVideoPath);
						return true;
					}
				}
				return false;
			}, null);
			this.m_vfxSwapButtons.Add(newButton);
			newButton.transform.SetParent(this.m_vfxButtonContainer.transform);
			newButton.transform.localScale = Vector3.one;
			newButton.transform.localPosition = Vector3.zero;
		}
	}

	public void VfxSwapButtonSelected(BaseEventData data)
	{
		if (data.selectedObject == null)
		{
			return;
		}
		CharacterResourceLink displayedCharacter = this.GetDisplayedCharacter();
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(displayedCharacter.m_characterType);
		UIVfxSwapSelectButton uivfxSwapSelectButton = null;
		int num = -1;
		for (int i = 0; i < this.m_vfxSwapButtons.Count; i++)
		{
			if (this.m_vfxSwapButtons[i].m_buttonHitBox.gameObject == data.selectedObject)
			{
				bool flag = i == 0 || playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(this.m_selectedAbilityIndex, this.m_vfxSwapButtons[i].GetSwap().m_uniqueID);
				if (flag)
				{
					uivfxSwapSelectButton = this.m_vfxSwapButtons[i];
					num = i;
				}
				break;
			}
		}
		bool flag2 = num != -1;
		if (flag2)
		{
			for (int j = 0; j < this.m_vfxSwapButtons.Count; j++)
			{
				if (j == num)
				{
					this.m_vfxSwapButtons[j].SetSelected(true, false);
				}
				else
				{
					this.m_vfxSwapButtons[j].SetSelected(false, false);
				}
			}
			if (this.m_selectedAbilityButton != null)
			{
				if (uivfxSwapSelectButton != null)
				{
					if (uivfxSwapSelectButton.GetSwap() != null)
					{
						this.m_selectedAbilityButton.SetSelectedVfxSwap(uivfxSwapSelectButton.GetSwap());
						this.m_selectedAbilityButton.SetSelectedVfxSwapIndex(num);
						this.m_abilityVfxSwapInfo.SetAbilityVfxSwapIdForAbility(this.m_selectedAbilityIndex, uivfxSwapSelectButton.GetSwap().m_uniqueID);
						UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
						goto IL_226;
					}
				}
				this.m_selectedAbilityButton.SetSelectedVfxSwap(null);
				this.m_selectedAbilityButton.SetSelectedVfxSwapIndex(-1);
				this.m_abilityVfxSwapInfo.SetAbilityVfxSwapIdForAbility(this.m_selectedAbilityIndex, 0);
				UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModClear);
			IL_226:
				ClientGameManager.Get().UpdateSelectedAbilityVfxSwaps(this.m_abilityVfxSwapInfo, 0);
			}
		}
	}

	internal void UpdateModEquipPointsLeft(Text optionalAdditionaUpdate = null)
	{
		if (!this.m_canSelectMods)
		{
			return;
		}
		int num = this.CalculateTotalModEquipCost();
		int num2 = num;
		int num3 = 0xA;
		if (this.m_modEquipPointsLeftNotches.Length == num3)
		{
			for (int i = 0; i < this.m_modEquipPointsLeftNotches.Length; i++)
			{
				UIManager.SetGameObjectActive(this.m_modEquipPointsLeftNotches[i], i < num2, null);
			}
			if (optionalAdditionaUpdate != null)
			{
				optionalAdditionaUpdate.text = string.Format("{0}/{1}", num2, num3);
			}
		}
	}

	internal bool UnderTotalModEquipCost(AbilityMod testAbilityMod)
	{
		int num = 0;
		for (int i = 0; i < this.m_abilityButtons.Length; i++)
		{
			if (this.m_selectedAbilityButton != this.m_abilityButtons[i])
			{
				AbilityMod modForAbility = AbilityModHelper.GetModForAbility(this.m_abilityButtons[i].GetAbilityEntry().ability, this.m_modInfo.GetModForAbility(i));
				if (modForAbility)
				{
					num += modForAbility.m_equipCost;
				}
			}
		}
		if (testAbilityMod != null)
		{
			num += testAbilityMod.m_equipCost;
		}
		return num <= 0xA;
	}

	public int CalculateTotalModEquipCost()
	{
		int num = 0;
		for (int i = 0; i < this.m_abilityButtons.Length; i++)
		{
			UIAbilityButtonModPanel uiabilityButtonModPanel = this.m_abilityButtons[i];
			AbilityData.AbilityEntry abilityEntry = uiabilityButtonModPanel.GetAbilityEntry();
			if (abilityEntry != null)
			{
				Ability ability = abilityEntry.ability;
				int modForAbility = this.m_modInfo.GetModForAbility(i);
				AbilityMod modForAbility2 = AbilityModHelper.GetModForAbility(ability, modForAbility);
				if (modForAbility2)
				{
					num += modForAbility2.m_equipCost;
				}
			}
		}
		return num;
	}

	public void OnLobbyGameplayOverridesUpdated(LobbyGameplayOverrides overrides)
	{
		this.AbilityButtonSelected(this.m_selectedAbilityIndex, false);
		this.UpdateModEquipPointsLeft(null);
	}

	private void AbilityButtonSelectedHelper(GameObject selectedButton, bool forceAnimation)
	{
		this.InitModButtonListIfNeeded();
		bool flag = GameManager.Get() != null && GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
		UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
		for (int i = 0; i < this.m_abilityButtons.Length; i++)
		{
			if (this.m_abilityButtons[i].m_buttonHitBox.gameObject == selectedButton)
			{
				this.m_abilityButtons[i].SetSelected(true, forceAnimation);
				this.m_selectedAbilityButton = this.m_abilityButtons[i];
				this.m_selectedAbilityIndex = i;
				if (this.m_vfxButtonContainer != null)
				{
					CharacterResourceLink displayedCharacter = this.GetDisplayedCharacter();
					if (displayedCharacter != null)
					{
						List<CharacterAbilityVfxSwap> availableVfxSwapsForAbilityIndex = displayedCharacter.GetAvailableVfxSwapsForAbilityIndex(this.m_selectedAbilityIndex);
						PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(displayedCharacter.m_characterType);
						GameBalanceVars.CharacterUnlockData characterUnlockData = GameBalanceVars.Get().GetCharacterUnlockData(displayedCharacter.m_characterType);
						this.CreateVfxButtonsIfNeeded(availableVfxSwapsForAbilityIndex.Count + 1);
						int j = 0;
						this.m_vfxSwapButtons[0].SetVfxSwap(null, 1);
						UIManager.SetGameObjectActive(this.m_vfxSwapButtons[0], true, null);
						this.m_vfxSwapButtons[0].SetLocked(false);
						if (this.m_abilityButtons[i].GetSelectedVfxSwap() == this.m_vfxSwapButtons[0].GetSwap())
						{
							this.m_vfxSwapButtons[j].SetSelected(true, forceAnimation);
						}
						else
						{
							this.m_vfxSwapButtons[j].SetSelected(false, forceAnimation);
						}
						j++;
						using (List<CharacterAbilityVfxSwap>.Enumerator enumerator = availableVfxSwapsForAbilityIndex.GetEnumerator())
						{
							IL_338:
							while (enumerator.MoveNext())
							{
								CharacterAbilityVfxSwap characterAbilityVfxSwap = enumerator.Current;
								if (j >= this.m_vfxSwapButtons.Count)
								{
									this.CreateVfxButtonsIfNeeded(j + 1);
								}
								if (characterAbilityVfxSwap.m_isHidden)
								{
									if (!flag)
									{
										continue;
									}
								}
								GameBalanceVars.AbilityVfxUnlockData abilityVfxUnlockData = null;
								int k = 0;
								while (k < characterUnlockData.abilityVfxUnlockData.Length)
								{
									if (characterUnlockData.abilityVfxUnlockData[k].ID == characterAbilityVfxSwap.m_uniqueID)
									{
										abilityVfxUnlockData = characterUnlockData.abilityVfxUnlockData[k];
										break;
									}
									else
									{
										k++;
									}
								}
								if (abilityVfxUnlockData == null || !GameBalanceVarsExtensions.MeetsVisibilityConditions(abilityVfxUnlockData))
								{
									goto IL_338;
								}
								this.m_vfxSwapButtons[j].SetVfxSwap(characterAbilityVfxSwap, j + 1);
								UIManager.SetGameObjectActive(this.m_vfxSwapButtons[j], true, null);
								bool flag2 = playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(this.m_selectedAbilityIndex, characterAbilityVfxSwap.m_uniqueID);
								this.m_vfxSwapButtons[j].SetLocked(!flag2);
								if (this.m_abilityButtons[i].GetSelectedVfxSwap() == this.m_vfxSwapButtons[j].GetSwap())
								{
									this.m_vfxSwapButtons[j].SetSelected(true, forceAnimation);
								}
								else
								{
									this.m_vfxSwapButtons[j].SetSelected(false, forceAnimation);
								}
								j++;
								goto IL_338;
							}
						}
						while (j < this.m_vfxSwapButtons.Count)
						{
							this.m_vfxSwapButtons[j].SetSelected(false, forceAnimation);
							UIManager.SetGameObjectActive(this.m_vfxSwapButtons[j], false, null);
							j++;
						}
					}
				}
				if (this.m_abilityButtons[i].GetAbilityEntry() != null)
				{
					if (this.m_modScrollView != null)
					{
						this.m_modScrollView.verticalScrollbar.value = 1f;
					}
					List<AbilityMod> availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(this.m_abilityButtons[i].GetAbilityEntry().ability);
					int l = 0;
					bool flag3 = false;
					AbilityData.AbilityEntry abilityEntry = this.m_abilityButtons[i].GetAbilityEntry();
					using (List<AbilityMod>.Enumerator enumerator2 = availableModsForAbility.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							AbilityMod abilityMod = enumerator2.Current;
							if (l >= this.m_cachedModButtonList.Count)
							{
								this.AllocateNewModButton();
							}
							UIModSelectButton uimodSelectButton = this.m_cachedModButtonList[l];
							bool flag4 = AbilityModHelper.IsModAllowed(this.m_selectedCharacter, i, abilityMod.m_abilityScopeId);
							if (flag4)
							{
								flag4 = abilityMod.EquippableForGameType();
								if (!flag4)
								{
									UIManager.SetGameObjectActive(uimodSelectButton, false, null);
								}
							}
							if (flag4)
							{
								uimodSelectButton.SetMod(abilityMod, abilityEntry.ability, i, this.m_selectedCharacter);
								UIManager.SetGameObjectActive(uimodSelectButton, true, null);
								if (!this.m_canSelectMods)
								{
									goto IL_51C;
								}
								if (!(this.m_abilityButtons[i].GetSelectedMod() == uimodSelectButton.GetMod()))
								{
									goto IL_51C;
								}
								uimodSelectButton.SetSelected(true, forceAnimation);
								flag3 = true;
								IL_525:
								l++;
								continue;
								IL_51C:
								uimodSelectButton.SetSelected(false, forceAnimation);
								goto IL_525;
							}
							if (this.m_abilityButtons[i].GetSelectedMod() == abilityMod)
							{
								this.DoModButtonSelected(this.m_clearModButton.m_buttonHitBox.gameObject);
							}
						}
					}
					while (l < this.m_cachedModButtonList.Count)
					{
						this.m_cachedModButtonList[l].SetSelected(false, forceAnimation);
						UIManager.SetGameObjectActive(this.m_cachedModButtonList[l], false, null);
						l++;
					}
					UIManager.SetGameObjectActive(this.m_clearModButton, true, null);
					UIModSelectButton clearModButton = this.m_clearModButton;
					bool selected;
					if (this.m_canSelectMods)
					{
						selected = !flag3;
					}
					else
					{
						selected = false;
					}
					clearModButton.SetSelected(selected, forceAnimation);
					if (this.m_abilityPreviewPanel)
					{
						if (abilityEntry != null)
						{
							if (abilityEntry.ability != null)
							{
								if (abilityEntry.ability.m_previewVideo != null)
								{
									this.m_abilityPreviewPanel.Play("Video/AbilityPreviews/" + abilityEntry.ability.m_previewVideo);
									goto IL_679;
								}
							}
						}
						this.m_abilityPreviewPanel.Stop();
					}
				}
				IL_679:;
			}
			else
			{
				this.m_abilityButtons[i].SetSelected(false, forceAnimation);
			}
		}
	}

	private void AllocateNewModButton()
	{
		UIModSelectButton uimodSelectButton = UnityEngine.Object.Instantiate<UIModSelectButton>(this.m_modButtonPrefab);
		if (uimodSelectButton == null)
		{
			Debug.LogError("Failed to allocate new mod button");
		}
		uimodSelectButton.transform.SetParent(this.m_modLayoutGroup.transform, false);
		uimodSelectButton.transform.localPosition = Vector3.zero;
		uimodSelectButton.transform.localScale = Vector3.one;
		uimodSelectButton.SetCallback(new _ButtonSwapSprite.ButtonClickCallback(this.ModButtonSelected));
		this.AddMouseEventPasser(uimodSelectButton);
		this.m_cachedModButtonList.Add(uimodSelectButton);
	}

	private void AddMouseEventPasser(UIModSelectButton modComp)
	{
		if (this.m_modScrollView != null)
		{
			_MouseEventPasser mouseEventPasser = modComp.m_buttonHitBox.gameObject.GetComponent<_MouseEventPasser>();
			if (mouseEventPasser == null)
			{
				mouseEventPasser = modComp.m_buttonHitBox.gameObject.AddComponent<_MouseEventPasser>();
			}
			mouseEventPasser.AddNewHandler(this.m_modScrollView);
		}
	}

	public void AbilityButtonSelected(int i, bool playsound = true)
	{
		if (i >= 0)
		{
			if (this.m_abilityButtons.Length > i)
			{
				this.AbilityButtonSelectedHelper(this.m_abilityButtons[i].m_buttonHitBox.gameObject, false);
				if (playsound)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
				}
				return;
			}
		}
	}

	public void AbilityButtonSelected(UIAbilityButtonModPanel abilityBtn)
	{
		if (abilityBtn == null)
		{
			return;
		}
		this.AbilityButtonSelectedHelper(abilityBtn.m_buttonHitBox.gameObject, false);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
	}

	public void AbilityButtonSelected(BaseEventData data)
	{
		if (data.selectedObject == null)
		{
			return;
		}
		this.AbilityButtonSelectedHelper(data.selectedObject, false);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
	}

	public void Setup(CharacterType characterType)
	{
		this.Setup(GameWideData.Get().GetCharacterResourceLink(characterType), false);
	}

	public CharacterResourceLink GetDisplayedCharacter()
	{
		return this.m_characterLink;
	}

	public void Setup(CharacterResourceLink characterLink, bool sameCharacter = false)
	{
		this.m_characterLink = characterLink;
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_characterLink.m_characterType);
		if (playerCharacterData != null)
		{
			if (this.m_canSelectMods)
			{
				bool flag = AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked;
				CharacterModInfo characterModInfo;
				if (flag)
				{
					characterModInfo = playerCharacterData.CharacterComponent.LastRankedMods;
				}
				else
				{
					characterModInfo = playerCharacterData.CharacterComponent.LastMods;
				}
				CharacterAbilityVfxSwapInfo lastAbilityVfxSwaps = playerCharacterData.CharacterComponent.LastAbilityVfxSwaps;
				if (this.m_modloadouts != null)
				{
					this.m_modloadouts.Setup(characterLink);
				}
				if (sameCharacter)
				{
					if (this.m_modInfo.Equals(characterModInfo))
					{
						if (this.m_showingRankedMods == flag)
						{
							return;
						}
					}
				}
				if (ClientGameManager.Get().WaitingForModSelectResponse == -1)
				{
					this.m_modInfo = characterModInfo;
					this.m_abilityVfxSwapInfo = lastAbilityVfxSwaps;
					this.m_showingRankedMods = flag;
					goto IL_10F;
				}
				return;
			}
		}
		IL_10F:
		AbilityData component = this.m_characterLink.ActorDataPrefab.GetComponent<AbilityData>();
		this.SetupAbilityArray(component);
		for (int i = 0; i < this.m_abilityButtons.Length; i++)
		{
			if (i < this.m_abilities.Length)
			{
				AbilityData.AbilityEntry abilityEntry = this.m_abilities[i];
				this.m_abilityButtons[i].Setup(abilityEntry);
				if (this.m_canSelectMods)
				{
					Ability ability = abilityEntry.ability;
					int num = this.m_modInfo.GetModForAbility(i);
					if (!AbilityModHelper.IsModAllowed(this.m_characterLink.m_characterType, i, num))
					{
						num = -1;
					}
					AbilityMod abilityMod = AbilityModHelper.GetModForAbility(ability, num);
					if (abilityMod != null)
					{
						if (!abilityMod.EquippableForGameType())
						{
							abilityMod = null;
							num = -1;
							this.m_modInfo.SetModForAbility(i, -1);
						}
					}
					this.m_abilityButtons[i].SetSelectedMod(abilityMod);
					this.m_abilityButtons[i].SetSelectedModIndex(num);
				}
				else
				{
					this.m_abilityButtons[i].SetSelectedMod(null);
					this.m_abilityButtons[i].SetSelectedModIndex(-1);
				}
			}
			else
			{
				this.m_abilityButtons[i].SetSelectedMod(null);
				this.m_abilityButtons[i].SetSelectedModIndex(-1);
			}
			this.m_abilityButtons[i].SetSelected(false, false);
		}
		for (int j = 0; j < this.m_cachedModButtonList.Count; j++)
		{
			this.m_cachedModButtonList[j].SetMod(null, null, 0, this.m_characterLink.m_characterType);
		}
		for (int k = 0; k < this.m_vfxSwapButtons.Count; k++)
		{
			this.m_vfxSwapButtons[k].SetVfxSwap(null, 0);
		}
		if (this.m_selectedCharacter != this.m_characterLink.m_characterType)
		{
			this.m_selectedAbilityButton = null;
			this.m_selectedAbilityIndex = -1;
		}
		UIAbilityButtonModPanel uiabilityButtonModPanel = this.m_abilityButtons[0];
		if (this.m_selectedAbilityButton != null)
		{
			uiabilityButtonModPanel = this.m_selectedAbilityButton;
		}
		this.m_selectedCharacter = this.m_characterLink.m_characterType;
		this.m_clearModButton.SetMod(null, null, 0, CharacterType.None);
		this.UpdateModCounter();
		if (this.CalculateTotalModEquipCost() > 0xA)
		{
			this.ResetAllMods(null);
		}
		this.UpdateModEquipPointsLeft(null);
		this.RefreshSelectedVfxSwaps();
		this.AbilityButtonSelectedHelper(uiabilityButtonModPanel.m_buttonHitBox.gameObject, true);
	}

	public void RefreshKeyBindings()
	{
		for (int i = 0; i < this.m_abilityButtons.Length; i++)
		{
			if (i < this.m_abilities.Length)
			{
				this.m_abilityButtons[i].RefreshHotkey();
			}
		}
	}

	private void SetupAbilityArray(AbilityData theAbility)
	{
		this.m_abilities = new AbilityData.AbilityEntry[0xE];
		for (int i = 0; i < this.m_abilities.Length; i++)
		{
			this.m_abilities[i] = new AbilityData.AbilityEntry();
		}
		this.m_abilities[0].Setup(theAbility.m_ability0, KeyPreference.Ability1);
		if (theAbility.m_ability0 != null)
		{
			theAbility.m_ability0.sprite = theAbility.m_sprite0;
		}
		this.m_abilities[1].Setup(theAbility.m_ability1, KeyPreference.Ability2);
		if (theAbility.m_ability1 != null)
		{
			theAbility.m_ability1.sprite = theAbility.m_sprite1;
		}
		this.m_abilities[2].Setup(theAbility.m_ability2, KeyPreference.Ability3);
		if (theAbility.m_ability2 != null)
		{
			theAbility.m_ability2.sprite = theAbility.m_sprite2;
		}
		this.m_abilities[3].Setup(theAbility.m_ability3, KeyPreference.Ability4);
		if (theAbility.m_ability3 != null)
		{
			theAbility.m_ability3.sprite = theAbility.m_sprite3;
		}
		this.m_abilities[4].Setup(theAbility.m_ability4, KeyPreference.Ability5);
		if (theAbility.m_ability4 != null)
		{
			theAbility.m_ability4.sprite = theAbility.m_sprite4;
		}
		this.m_abilities[5].Setup(theAbility.m_ability5, KeyPreference.Ability6);
		if (theAbility.m_ability5 != null)
		{
			theAbility.m_ability5.sprite = theAbility.m_sprite5;
		}
		this.m_abilities[6].Setup(theAbility.m_ability6, KeyPreference.Ability7);
		if (theAbility.m_ability6 != null)
		{
			theAbility.m_ability6.sprite = theAbility.m_sprite6;
		}
	}

	private void Update()
	{
		if (this.m_setZtoZero)
		{
			RectTransform[] componentsInChildren = base.GetComponentsInChildren<RectTransform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != base.transform)
				{
					componentsInChildren[i].localPosition = new Vector3(componentsInChildren[i].localPosition.x, componentsInChildren[i].localPosition.y, 0f);
				}
			}
		}
	}

	private void PurchaseMod(BaseEventData data)
	{
		if (this.m_currentlyPurchasingModButton != null)
		{
			this.m_currentlyPurchasingModButton.RequestPurchaseMod();
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModUnlocked);
		}
		this.m_purchasePanel.m_freelancerCurrencyButton.spriteController.ResetMouseState();
		UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
	}
}
