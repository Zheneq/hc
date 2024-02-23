using System.Collections.Generic;
using System.Text;
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
		InitModButtonListIfNeeded();
	}

	private void Start()
	{
		foreach (UIAbilityButtonModPanel abilityButton in m_abilityButtons)
		{
			abilityButton.SetCallback(AbilityButtonSelected);
		}
		m_clearModButton.SetCallback(ModButtonSelected);
		AddMouseEventPasser(m_clearModButton);
		if (m_resetAllMods != null)
		{
			m_resetAllMods.spriteController.callback = delegate
			{
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable()
				    && ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(AccountComponent.UIStateIdentifier.HasResetMods) > 0)
				{
					ResetAllMods(null);
				}
				else
				{
					ClientGameManager.Get().RequestUpdateUIState(AccountComponent.UIStateIdentifier.HasResetMods, 1, null);
					UIDialogPopupManager.OpenTwoButtonDialog(
						StringUtil.TR("ResetMods", "Global"),
						StringUtil.TR("ResetModsConfirm", "Global"), 
						StringUtil.TR("Yes", "Global"),
						StringUtil.TR("No", "Global"),
						ResetAllMods);
				}
				m_resetAllMods.spriteController.ResetMouseState();
			};
		}
		UIManager.SetGameObjectActive(m_purchaseModTokenBtn, false);
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnModUnlocked += UICharacterSelectAbilitiesPanel_OnModUnlocked;
			ClientGameManager.Get().OnBankBalanceChange += UICharacterSelectAbilitiesPanel_OnBankBalanceChange;
			ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange += OnLobbyGameplayOverridesUpdated;
		}
		ResetLockIcons();
		bool modsEnabled = false;
		if (GameManager.Get() != null && GameManager.Get().GameplayOverrides != null)
		{
			modsEnabled = GameManager.Get().GameplayOverrides.EnableMods;
		}
		UIManager.SetGameObjectActive(m_modTokenCountText, false);
		if (m_modTokenLabel != null)
		{
			UIManager.SetGameObjectActive(m_modTokenLabel, false);
		}
		UIManager.SetGameObjectActive(m_purchasePanel, false);
		m_purchasePanel.m_freelancerCurrencyButton.spriteController.callback = PurchaseMod;
		if (m_disabledLabel != null)
		{
			UIManager.SetGameObjectActive(m_disabledLabel, !modsEnabled);
		}
	}

	private void InitModButtonListIfNeeded()
	{
		if (m_cachedModButtonList.Count != 0)
		{
			return;
		}
		foreach (Transform modSelectButtonTransform in m_modLayoutGroup.transform)
		{
			UIModSelectButton component = modSelectButtonTransform.GetComponent<UIModSelectButton>();
			if (component != null && component.gameObject != m_clearModButton.gameObject)
			{
				component.SetCallback(ModButtonSelected);
				AddMouseEventPasser(component);
				m_cachedModButtonList.Add(component);
			}
		}
	}

	public void NotifyLoadoutUpdate(PlayerInfoUpdateResponse response)
	{
		if (m_modloadouts != null)
		{
			m_modloadouts.NotifyLoadoutUpdate(response);
		}

		if (response.CharacterInfo != null)
		{
			Setup(GameWideData.Get().GetCharacterResourceLink(response.CharacterInfo.CharacterType), true);
		}
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnModUnlocked -= UICharacterSelectAbilitiesPanel_OnModUnlocked;
			ClientGameManager.Get().OnBankBalanceChange -= UICharacterSelectAbilitiesPanel_OnBankBalanceChange;
			ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterDataUpdated;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= OnLobbyGameplayOverridesUpdated;
		}
	}

	private void OnDisable()
	{
		if (m_purchasePanel != null)
		{
			UIManager.SetGameObjectActive(m_purchasePanel, false);
		}
	}

	public void RemoveHandleMessage()
	{
		ClientGameManager.Get().OnModUnlocked -= UICharacterSelectAbilitiesPanel_OnModUnlocked;
		ClientGameManager.Get().OnBankBalanceChange -= UICharacterSelectAbilitiesPanel_OnBankBalanceChange;
	}

	private void UICharacterSelectAbilitiesPanel_OnBankBalanceChange(CurrencyData newBalance)
	{
		ResetLockIcons();
		if (newBalance.Type == CurrencyType.FreelancerCurrency)
		{
			UpdatePurchaseModTokenButtonState();
		}
	}

	private void UICharacterSelectAbilitiesPanel_OnModUnlocked(CharacterType character, PlayerModData unlockData)
	{
		if (character != m_selectedCharacter)
		{
			return;
		}
		foreach (UIModSelectButton cachedModButton in m_cachedModButtonList)
		{
			cachedModButton.SetLockIcons();
			if (ClientGameManager.Get().PurchasingMod
			    && cachedModButton.GetMod() != null
			    && cachedModButton.GetMod().m_abilityScopeId == ClientGameManager.Get().ModAttemptingToPurchase)
			{
				DoModButtonSelected(cachedModButton.m_buttonHitBox.gameObject);
			}
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModUnlocked);
	}

	public void OnCharacterDataUpdated(PersistedCharacterData data)
	{
		ResetLockIcons();
	}

	public void ResetLockIcons()
	{
		foreach (UIModSelectButton cachedModButton in m_cachedModButtonList)
		{
			cachedModButton.SetLockIcons();
		}
	}

	private void ResetAllMods(UIDialogBox boxReference)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModClear);
		for (int i = 0; i < m_abilityButtons.Length; i++)
		{
			m_abilityButtons[i].SetSelectedMod(null);
			m_abilityButtons[i].SetSelectedModIndex(-1);
			m_modInfo.SetModForAbility(i, -1);
		}
		foreach (UIModSelectButton modButton in m_cachedModButtonList)
		{
			modButton.SetSelected(false);
		}
		m_clearModButton.SetSelected(true);
		AppState_CharacterSelect.Get().UpdateSelectedMods(m_modInfo);
		UpdateModCounter();
		UpdateModEquipPointsLeft();
	}

	internal void ShowOutOfModEquipPointsDialog()
	{
		UIManager.SetGameObjectActive(m_NotEnoughPointsNotificationAC, true);
	}

	public void DebugSelectMod(BaseEventData data)
	{
		AbilityData.AbilityEntry abilityEntry = m_abilityButtons[m_selectedAbilityIndex].GetAbilityEntry();
		UIDialogPopupManager.OpenDebugModSelectionDialog(abilityEntry, m_selectedAbilityIndex, this);
	}

	public void PurchaseModToken(BaseEventData data)
	{
		if (GameBalanceVars.Get() == null || GameBalanceVars.Get().FreelancerCurrencyPerModToken <= 0)
		{
			return;
		}

		int freelancerCurrencyPerModToken = GameBalanceVars.Get().FreelancerCurrencyPerModToken;
		int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.FreelancerCurrency);
		if (currentAmount >= freelancerCurrencyPerModToken)
		{
			Log.Info(Log.Category.UI, "Sending mod token purchase request");
			ClientGameManager.Get().PurchaseModToken(1);
		}
		else
		{
			UIDialogPopupManager.OpenOneButtonDialog(
				StringUtil.TR("InsufficientCredits", "Global"),
				string.Format(
					StringUtil.TR("InsufficientCurrencyModToken", "Global"),
					StringUtil.TR("FreelancerCurrency", "Global"),
					UIStorePanel.FormatIntToString(freelancerCurrencyPerModToken, true),
					UIStorePanel.FormatIntToString(currentAmount, true)), 
				StringUtil.TR("Ok", "Global"));
		}
	}

	private void UpdatePurchaseModTokenButtonState()
	{
		UIManager.SetGameObjectActive(m_purchaseModTokenBtn, false);
	}

	public void RefreshSelectedMods()
	{
		if (ClientGameManager.Get() == null
		    || ClientGameManager.Get().GroupInfo == null
		    || m_selectedCharacter == CharacterType.None
		    || m_selectedCharacter.IsWillFill()
		    || m_selectedCharacter == CharacterType.PunchingDummy
		    || m_selectedCharacter == CharacterType.Last)
		{
			return;
		}
		CharacterModInfo characterModInfo = AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked
			? ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter)
				.CharacterComponent.LastRankedMods
			: ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter)
				.CharacterComponent.LastMods;

		for (int actionType = 0; actionType < m_abilityButtons.Length; actionType++)
		{
			if (actionType < m_abilities.Length)
			{
				AbilityData.AbilityEntry abilityEntry = m_abilities[actionType];
				Ability ability = abilityEntry.ability;
				int abilityScopeId = characterModInfo.GetModForAbility(actionType);
				if (!AbilityModHelper.IsModAllowed(m_characterLink.m_characterType, actionType, abilityScopeId))
				{
					abilityScopeId = -1;
				}

				AbilityMod abilityMod = AbilityModHelper.GetModForAbility(ability, abilityScopeId);
				if (abilityMod != null && !abilityMod.EquippableForGameType())
				{
					abilityMod = null;
					abilityScopeId = -1;
					characterModInfo.SetModForAbility(actionType, -1);
				}

				m_abilityButtons[actionType].SetSelectedMod(abilityMod);
				m_abilityButtons[actionType].SetSelectedModIndex(abilityScopeId);
			}
			else
			{
				m_abilityButtons[actionType].SetSelectedMod(null);
				m_abilityButtons[actionType].SetSelectedModIndex(-1);
			}

			m_abilityButtons[actionType].SetSelected(m_selectedAbilityButton == m_abilityButtons[actionType]);
		}

		UpdateModCounter();
		UpdateModEquipPointsLeft();
	}

	internal void UpdateModCounter()
	{
		if (!m_canSelectMods)
		{
			return;
		}
		UICharacterScreen.Get().UpdateModIcons(m_abilityButtons, m_prepColor, m_dashColor, m_combatColor);
	}

	public void DoModButtonSelected(GameObject button)
	{
		UIManager.SetGameObjectActive(m_purchasePanel, false);
		if (GameManager.Get() != null
		    && GameManager.Get().GameplayOverrides != null
		    && !GameManager.Get().GameplayOverrides.EnableMods)
		{
			return;
		}
		if (button == null)
		{
			return;
		}
		UIModSelectButton component = button.transform.parent.gameObject.GetComponent<UIModSelectButton>();
		if (component.AvailableForPurchase())
		{
			component.m_buttonHitBox.ForceSetPointerEntered(false);
			m_currentlyPurchasingModButton = component;
			GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
			int freelancerCurrencyToUnlockMod = gameBalanceVars.FreelancerCurrencyToUnlockMod;
			int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.FreelancerCurrency);
			m_purchasePanel.Setup(0, freelancerCurrencyToUnlockMod, 0f, true);
			UIManager.SetGameObjectActive(m_purchasePanel, true);
			m_purchasePanel.SetDisabled(freelancerCurrencyToUnlockMod > currentAmount);
			return;
		}
		if (!m_canSelectMods)
		{
			return;
		}
		if (!UnderTotalModEquipCost(component.GetMod()))
		{
			ShowOutOfModEquipPointsDialog();
			return;
		}
		UIModSelectButton uIModSelectButton = null;
		int selectedModIndex = -1;
		for (int i = 0; i < m_cachedModButtonList.Count; i++)
		{
			UIModSelectButton uIModSelectButton2 = m_cachedModButtonList[i];
			if (uIModSelectButton2.m_buttonHitBox.gameObject == button)
			{
				uIModSelectButton2.SetSelected(true);
				uIModSelectButton = uIModSelectButton2;
				selectedModIndex = i;
			}
			else
			{
				uIModSelectButton2.SetSelected(false);
			}
		}
		if (m_clearModButton.m_buttonHitBox.gameObject == button)
		{
			m_clearModButton.SetSelected(true);
			uIModSelectButton = null;
		}
		else
		{
			m_clearModButton.SetSelected(false);
		}
		if (m_selectedAbilityButton != null)
		{
			if (uIModSelectButton != null && uIModSelectButton.GetMod() != null)
			{
				m_selectedAbilityButton.SetSelectedMod(uIModSelectButton.GetMod());
				m_selectedAbilityButton.SetSelectedModIndex(selectedModIndex);
				m_modInfo.SetModForAbility(m_selectedAbilityIndex, uIModSelectButton.GetMod().m_abilityScopeId);
				UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
				GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndEquipMod, null);
			}
			else
			{
				m_selectedAbilityButton.SetSelectedMod(null);
				m_selectedAbilityButton.SetSelectedModIndex(-1);
				m_modInfo.SetModForAbility(m_selectedAbilityIndex, -1);
				UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModClear);
			}
			ClientGameManager.Get().UpdateSelectedMods(m_modInfo);
		}
		UpdateModCounter();
		UpdateModEquipPointsLeft();
	}

	public void ModButtonSelected(BaseEventData data)
	{
		DoModButtonSelected(data.selectedObject);
	}

	public void RefreshSelectedVfxSwaps()
	{
		if (ClientGameManager.Get() == null
		    || ClientGameManager.Get().GroupInfo == null
		    || m_selectedCharacter == CharacterType.None
		    || m_selectedCharacter.IsWillFill()
		    || m_selectedCharacter == CharacterType.PunchingDummy
		    || m_selectedCharacter == CharacterType.Last)
		{
			return;
		}
		CharacterResourceLink displayedCharacter = GetDisplayedCharacter();
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter);
		CharacterAbilityVfxSwapInfo lastAbilityVfxSwaps = playerCharacterData.CharacterComponent.LastAbilityVfxSwaps;
		for (int abilityId = 0; abilityId < m_abilityButtons.Length; abilityId++)
		{
			if (abilityId < m_abilities.Length)
			{
				int abilityVfxSwapIdForAbility = lastAbilityVfxSwaps.GetAbilityVfxSwapIdForAbility(abilityId);
				int selectedVfxSwapIndex = 0;
				CharacterAbilityVfxSwap selectedVfxSwap = null;
				if (abilityVfxSwapIdForAbility != 0
				    && playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(abilityId, abilityVfxSwapIdForAbility))
				{
					List<CharacterAbilityVfxSwap> availableVfxSwapsForAbilityIndex = displayedCharacter.GetAvailableVfxSwapsForAbilityIndex(abilityId);
					for (int i = 0; i < availableVfxSwapsForAbilityIndex.Count; i++)
					{
						CharacterAbilityVfxSwap characterAbilityVfxSwap = availableVfxSwapsForAbilityIndex[i];
						if (characterAbilityVfxSwap.m_uniqueID == abilityVfxSwapIdForAbility)
						{
							selectedVfxSwapIndex = i + 1;
							selectedVfxSwap = characterAbilityVfxSwap;
						}
					}
				}

				m_abilityButtons[abilityId].SetSelectedVfxSwap(selectedVfxSwap);
				m_abilityButtons[abilityId].SetSelectedVfxSwapIndex(selectedVfxSwapIndex);
			}
			else
			{
				m_abilityButtons[abilityId].SetSelectedVfxSwap(null);
				m_abilityButtons[abilityId].SetSelectedVfxSwapIndex(0);
			}
		}
	}

	private void CreateVfxButtonsIfNeeded(int totalButtons)
	{
		for (int i = m_vfxSwapButtons.Count; i < totalButtons; i++)
		{
			UIVfxSwapSelectButton newButton = Instantiate(m_vfxButtonPrefab);
			newButton.SetCallback(VfxSwapButtonSelected);
			newButton.m_buttonHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.TauntPreview, delegate(UITooltipBase tooltip)
			{
				CharacterAbilityVfxSwap swap = newButton.GetSwap();
				if (swap == null || swap.m_swapVideoPath.IsNullOrEmpty())
				{
					return false;
				}
				(tooltip as UIFrontendTauntMouseoverVideo).Setup(new StringBuilder().Append(c_videoDir).Append(swap.m_swapVideoPath).ToString());
				return true;
			});
			m_vfxSwapButtons.Add(newButton);
			newButton.transform.SetParent(m_vfxButtonContainer.transform);
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
		CharacterResourceLink displayedCharacter = GetDisplayedCharacter();
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(displayedCharacter.m_characterType);
		UIVfxSwapSelectButton uIVfxSwapSelectButton = null;
		int num = -1;
		
		for (int i = 0; i < m_vfxSwapButtons.Count; i++)
		{
			if (m_vfxSwapButtons[i].m_buttonHitBox.gameObject == data.selectedObject)
			{
				if (i == 0 || playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(m_selectedAbilityIndex, m_vfxSwapButtons[i].GetSwap().m_uniqueID))
				{
					uIVfxSwapSelectButton = m_vfxSwapButtons[i];
					num = i;
				}
				break;
			}
		}
		if (num == -1)
		{
			return;
		}
		for (int i = 0; i < m_vfxSwapButtons.Count; i++)
		{
			m_vfxSwapButtons[i].SetSelected(i == num);
		}
		if (m_selectedAbilityButton != null)
		{
			if (uIVfxSwapSelectButton != null && uIVfxSwapSelectButton.GetSwap() != null)
			{
				m_selectedAbilityButton.SetSelectedVfxSwap(uIVfxSwapSelectButton.GetSwap());
				m_selectedAbilityButton.SetSelectedVfxSwapIndex(num);
				m_abilityVfxSwapInfo.SetAbilityVfxSwapIdForAbility(m_selectedAbilityIndex,
					uIVfxSwapSelectButton.GetSwap().m_uniqueID);
				UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
			}
			else
			{
				m_selectedAbilityButton.SetSelectedVfxSwap(null);
				m_selectedAbilityButton.SetSelectedVfxSwapIndex(-1);
				m_abilityVfxSwapInfo.SetAbilityVfxSwapIdForAbility(m_selectedAbilityIndex, 0);
				UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModClear);
			}

			ClientGameManager.Get().UpdateSelectedAbilityVfxSwaps(m_abilityVfxSwapInfo);
		}
	}

	internal void UpdateModEquipPointsLeft(Text optionalAdditionaUpdate = null)
	{
		if (!m_canSelectMods)
		{
			return;
		}

		int usedPoints = CalculateTotalModEquipCost();
		int maxPoints = 10;
		if (m_modEquipPointsLeftNotches.Length == maxPoints)
		{
			for (int i = 0; i < m_modEquipPointsLeftNotches.Length; i++)
			{
				UIManager.SetGameObjectActive(m_modEquipPointsLeftNotches[i], i < usedPoints);
			}

			if (optionalAdditionaUpdate != null)
			{
				optionalAdditionaUpdate.text = new StringBuilder().Append(usedPoints).Append("/").Append(maxPoints).ToString();
			}
		}
	}

	internal bool UnderTotalModEquipCost(AbilityMod testAbilityMod)
	{
		int points = 0;
		for (int i = 0; i < m_abilityButtons.Length; i++)
		{
			if (m_selectedAbilityButton != m_abilityButtons[i])
			{
				AbilityMod modForAbility = AbilityModHelper.GetModForAbility(m_abilityButtons[i].GetAbilityEntry().ability, m_modInfo.GetModForAbility(i));
				if (modForAbility != null)
				{
					points += modForAbility.m_equipCost;
				}
			}
		}
		if (testAbilityMod != null)
		{
			points += testAbilityMod.m_equipCost;
		}
		return points <= 10;
	}

	public int CalculateTotalModEquipCost()
	{
		int usedPoints = 0;
		for (int i = 0; i < m_abilityButtons.Length; i++)
		{
			AbilityData.AbilityEntry abilityEntry = m_abilityButtons[i].GetAbilityEntry();
			if (abilityEntry == null)
			{
				continue;
			}
			Ability ability = abilityEntry.ability;
			int selectedModId = m_modInfo.GetModForAbility(i);
			AbilityMod selectedMod = AbilityModHelper.GetModForAbility(ability, selectedModId);
			if (selectedMod != null)
			{
				usedPoints += selectedMod.m_equipCost;
			}
		}
		return usedPoints;
	}

	public void OnLobbyGameplayOverridesUpdated(LobbyGameplayOverrides overrides)
	{
		AbilityButtonSelected(m_selectedAbilityIndex, false);
		UpdateModEquipPointsLeft();
	}

	private void AbilityButtonSelectedHelper(GameObject selectedButton, bool forceAnimation)
	{
		InitModButtonListIfNeeded();
		bool flag = GameManager.Get() != null && GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
		UIManager.SetGameObjectActive(m_purchasePanel, false);
		for (int i = 0; i < m_abilityButtons.Length; i++)
		{
			if (m_abilityButtons[i].m_buttonHitBox.gameObject != selectedButton)
			{
				m_abilityButtons[i].SetSelected(false, forceAnimation);
				continue;
			}
			m_abilityButtons[i].SetSelected(true, forceAnimation);
			m_selectedAbilityButton = m_abilityButtons[i];
			m_selectedAbilityIndex = i;
			if (m_vfxButtonContainer != null)
			{
				CharacterResourceLink displayedCharacter = GetDisplayedCharacter();
				if (displayedCharacter != null)
				{
					List<CharacterAbilityVfxSwap> availableVfxSwapsForAbilityIndex =
						displayedCharacter.GetAvailableVfxSwapsForAbilityIndex(m_selectedAbilityIndex);
					PersistedCharacterData playerCharacterData = ClientGameManager.Get()
						.GetPlayerCharacterData(displayedCharacter.m_characterType);
					GameBalanceVars.CharacterUnlockData characterUnlockData = GameBalanceVars.Get()
						.GetCharacterUnlockData(displayedCharacter.m_characterType);
					CreateVfxButtonsIfNeeded(availableVfxSwapsForAbilityIndex.Count + 1);
					int num = 0;
					m_vfxSwapButtons[0].SetVfxSwap(null, 1);
					UIManager.SetGameObjectActive(m_vfxSwapButtons[0], true);
					m_vfxSwapButtons[0].SetLocked(false);
					m_vfxSwapButtons[num].SetSelected(m_abilityButtons[i].GetSelectedVfxSwap() == m_vfxSwapButtons[0].GetSwap(), forceAnimation);

					num++;
					foreach (CharacterAbilityVfxSwap item in availableVfxSwapsForAbilityIndex)
					{
						if (num >= m_vfxSwapButtons.Count)
						{
							CreateVfxButtonsIfNeeded(num + 1);
						}

						if (item.m_isHidden && !flag)
						{
							continue;
						}

						GameBalanceVars.AbilityVfxUnlockData abilityVfxUnlockData = null;
						int num2 = 0;
						while (true)
						{
							if (num2 >= characterUnlockData.abilityVfxUnlockData.Length)
							{
								break;
							}

							if (characterUnlockData.abilityVfxUnlockData[num2].ID == item.m_uniqueID)
							{
								abilityVfxUnlockData = characterUnlockData.abilityVfxUnlockData[num2];
								break;
							}

							num2++;
						}

						if (abilityVfxUnlockData != null &&
						    GameBalanceVarsExtensions.MeetsVisibilityConditions(abilityVfxUnlockData))
						{
							m_vfxSwapButtons[num].SetVfxSwap(item, num + 1);
							UIManager.SetGameObjectActive(m_vfxSwapButtons[num], true);
							bool flag2 = playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(m_selectedAbilityIndex, item.m_uniqueID);
							m_vfxSwapButtons[num].SetLocked(!flag2);
							if (m_abilityButtons[i].GetSelectedVfxSwap() == m_vfxSwapButtons[num].GetSwap())
							{
								m_vfxSwapButtons[num].SetSelected(true, forceAnimation);
							}
							else
							{
								m_vfxSwapButtons[num].SetSelected(false, forceAnimation);
							}

							num++;
						}
					}

					for (; num < m_vfxSwapButtons.Count; num++)
					{
						m_vfxSwapButtons[num].SetSelected(false, forceAnimation);
						UIManager.SetGameObjectActive(m_vfxSwapButtons[num], false);
					}
				}
			}

			if (m_abilityButtons[i].GetAbilityEntry() == null)
			{
				continue;
			}

			if (m_modScrollView != null)
			{
				m_modScrollView.verticalScrollbar.value = 1f;
			}

			List<AbilityMod> availableModsForAbility =
				AbilityModHelper.GetAvailableModsForAbility(m_abilityButtons[i].GetAbilityEntry().ability);
			int j = 0;
			bool flag3 = false;
			AbilityData.AbilityEntry abilityEntry = m_abilityButtons[i].GetAbilityEntry();

			foreach (AbilityMod current2 in availableModsForAbility)
			{
				if (j >= m_cachedModButtonList.Count)
				{
					AllocateNewModButton();
				}

				UIModSelectButton uIModSelectButton = m_cachedModButtonList[j];
				bool flag4 = AbilityModHelper.IsModAllowed(m_selectedCharacter, i, current2.m_abilityScopeId);
				if (flag4)
				{
					flag4 = current2.EquippableForGameType();
					if (!flag4)
					{
						UIManager.SetGameObjectActive(uIModSelectButton, false);
					}
				}

				if (!flag4)
				{
					if (m_abilityButtons[i].GetSelectedMod() == current2)
					{
						DoModButtonSelected(m_clearModButton.m_buttonHitBox.gameObject);
					}

					continue;
				}

				uIModSelectButton.SetMod(current2, abilityEntry.ability, i, m_selectedCharacter);
				UIManager.SetGameObjectActive(uIModSelectButton, true);
				if (m_canSelectMods && m_abilityButtons[i].GetSelectedMod() == uIModSelectButton.GetMod())
				{
					uIModSelectButton.SetSelected(true, forceAnimation);
					flag3 = true;
				}
				else
				{
					uIModSelectButton.SetSelected(false, forceAnimation);
				}

				j++;
			}

			for (; j < m_cachedModButtonList.Count; j++)
			{
				m_cachedModButtonList[j].SetSelected(false, forceAnimation);
				UIManager.SetGameObjectActive(m_cachedModButtonList[j], false);
			}

			UIManager.SetGameObjectActive(m_clearModButton, true);
			m_clearModButton.SetSelected(m_canSelectMods && !flag3, forceAnimation);
			if (!m_abilityPreviewPanel)
			{
				continue;
			}

			if (abilityEntry != null && abilityEntry.ability != null && abilityEntry.ability.m_previewVideo != null)
			{
				m_abilityPreviewPanel.Play(new StringBuilder().Append(c_videoDir).Append(abilityEntry.ability.m_previewVideo).ToString());
				continue;
			}

			m_abilityPreviewPanel.Stop();
		}
	}

	private void AllocateNewModButton()
	{
		UIModSelectButton uIModSelectButton = Instantiate(m_modButtonPrefab);
		if (uIModSelectButton == null)
		{
			Debug.LogError("Failed to allocate new mod button");
		}
		uIModSelectButton.transform.SetParent(m_modLayoutGroup.transform, false);
		uIModSelectButton.transform.localPosition = Vector3.zero;
		uIModSelectButton.transform.localScale = Vector3.one;
		uIModSelectButton.SetCallback(ModButtonSelected);
		AddMouseEventPasser(uIModSelectButton);
		m_cachedModButtonList.Add(uIModSelectButton);
	}

	private void AddMouseEventPasser(UIModSelectButton modComp)
	{
		if (m_modScrollView == null)
		{
			return;
		}
		_MouseEventPasser mouseEventPasser = modComp.m_buttonHitBox.gameObject.GetComponent<_MouseEventPasser>();
		if (mouseEventPasser == null)
		{
			mouseEventPasser = modComp.m_buttonHitBox.gameObject.AddComponent<_MouseEventPasser>();
		}

		mouseEventPasser.AddNewHandler(m_modScrollView);
	}

	public void AbilityButtonSelected(int i, bool playsound = true)
	{
		if (i < 0)
		{
			return;
		}
		if (m_abilityButtons.Length > i)
		{
			AbilityButtonSelectedHelper(m_abilityButtons[i].m_buttonHitBox.gameObject, false);
			if (playsound)
			{
				UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
			}
		}
	}

	public void AbilityButtonSelected(UIAbilityButtonModPanel abilityBtn)
	{
		if (abilityBtn == null)
		{
			return;
		}
		AbilityButtonSelectedHelper(abilityBtn.m_buttonHitBox.gameObject, false);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
	}

	public void AbilityButtonSelected(BaseEventData data)
	{
		if (data.selectedObject == null)
		{
			return;
		}
		AbilityButtonSelectedHelper(data.selectedObject, false);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
	}

	public void Setup(CharacterType characterType)
	{
		Setup(GameWideData.Get().GetCharacterResourceLink(characterType));
	}

	public CharacterResourceLink GetDisplayedCharacter()
	{
		return m_characterLink;
	}

	public void Setup(CharacterResourceLink characterLink, bool sameCharacter = false)
	{
		m_characterLink = characterLink;
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_characterLink.m_characterType);
		if (playerCharacterData != null && m_canSelectMods)
		{
			bool isRanked = AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked;
			CharacterModInfo characterModInfo = isRanked ? playerCharacterData.CharacterComponent.LastRankedMods : playerCharacterData.CharacterComponent.LastMods;
			CharacterAbilityVfxSwapInfo lastAbilityVfxSwaps = playerCharacterData.CharacterComponent.LastAbilityVfxSwaps;
			if (m_modloadouts != null)
			{
				m_modloadouts.Setup(characterLink);
			}
			if (sameCharacter && m_modInfo.Equals(characterModInfo) && m_showingRankedMods == isRanked)
			{
				return;
			}
			if (ClientGameManager.Get().WaitingForModSelectResponse != -1)
			{
				return;
			}
			m_modInfo = characterModInfo;
			m_abilityVfxSwapInfo = lastAbilityVfxSwaps;
			m_showingRankedMods = isRanked;
		}
		AbilityData component = m_characterLink.ActorDataPrefab.GetComponent<AbilityData>();
		SetupAbilityArray(component);
		for (int i = 0; i < m_abilityButtons.Length; i++)
		{
			if (i < m_abilities.Length)
			{
				AbilityData.AbilityEntry abilityEntry = m_abilities[i];
				m_abilityButtons[i].Setup(abilityEntry);
				if (m_canSelectMods)
				{
					Ability ability = abilityEntry.ability;
					int num = m_modInfo.GetModForAbility(i);
					if (!AbilityModHelper.IsModAllowed(m_characterLink.m_characterType, i, num))
					{
						num = -1;
					}
					AbilityMod abilityMod = AbilityModHelper.GetModForAbility(ability, num);
					if (abilityMod != null && !abilityMod.EquippableForGameType())
					{
						abilityMod = null;
						num = -1;
						m_modInfo.SetModForAbility(i, -1);
					}
					m_abilityButtons[i].SetSelectedMod(abilityMod);
					m_abilityButtons[i].SetSelectedModIndex(num);
				}
				else
				{
					m_abilityButtons[i].SetSelectedMod(null);
					m_abilityButtons[i].SetSelectedModIndex(-1);
				}
			}
			else
			{
				m_abilityButtons[i].SetSelectedMod(null);
				m_abilityButtons[i].SetSelectedModIndex(-1);
			}
			m_abilityButtons[i].SetSelected(false);
		}
		foreach (UIModSelectButton modButton in m_cachedModButtonList)
		{
			modButton.SetMod(null, null, 0, m_characterLink.m_characterType);
		}
		foreach (UIVfxSwapSelectButton vfxSwapButton in m_vfxSwapButtons)
		{
			vfxSwapButton.SetVfxSwap(null, 0);
		}
		if (m_selectedCharacter != m_characterLink.m_characterType)
		{
			m_selectedAbilityButton = null;
			m_selectedAbilityIndex = -1;
		}
		UIAbilityButtonModPanel uIAbilityButtonModPanel = m_abilityButtons[0];
		if (m_selectedAbilityButton != null)
		{
			uIAbilityButtonModPanel = m_selectedAbilityButton;
		}
		m_selectedCharacter = m_characterLink.m_characterType;
		m_clearModButton.SetMod(null, null, 0, CharacterType.None);
		UpdateModCounter();
		if (CalculateTotalModEquipCost() > 10)
		{
			ResetAllMods(null);
		}
		UpdateModEquipPointsLeft();
		RefreshSelectedVfxSwaps();
		AbilityButtonSelectedHelper(uIAbilityButtonModPanel.m_buttonHitBox.gameObject, true);
	}

	public void RefreshKeyBindings()
	{
		for (int i = 0; i < m_abilityButtons.Length; i++)
		{
			if (i < m_abilities.Length)
			{
				m_abilityButtons[i].RefreshHotkey();
			}
		}
	}

	private void SetupAbilityArray(AbilityData theAbility)
	{
		m_abilities = new AbilityData.AbilityEntry[14];
		for (int i = 0; i < m_abilities.Length; i++)
		{
			m_abilities[i] = new AbilityData.AbilityEntry();
		}
		m_abilities[0].Setup(theAbility.m_ability0, KeyPreference.Ability1);
		if (theAbility.m_ability0 != null)
		{
			theAbility.m_ability0.sprite = theAbility.m_sprite0;
		}
		m_abilities[1].Setup(theAbility.m_ability1, KeyPreference.Ability2);
		if (theAbility.m_ability1 != null)
		{
			theAbility.m_ability1.sprite = theAbility.m_sprite1;
		}
		m_abilities[2].Setup(theAbility.m_ability2, KeyPreference.Ability3);
		if (theAbility.m_ability2 != null)
		{
			theAbility.m_ability2.sprite = theAbility.m_sprite2;
		}
		m_abilities[3].Setup(theAbility.m_ability3, KeyPreference.Ability4);
		if (theAbility.m_ability3 != null)
		{
			theAbility.m_ability3.sprite = theAbility.m_sprite3;
		}
		m_abilities[4].Setup(theAbility.m_ability4, KeyPreference.Ability5);
		if (theAbility.m_ability4 != null)
		{
			theAbility.m_ability4.sprite = theAbility.m_sprite4;
		}
		m_abilities[5].Setup(theAbility.m_ability5, KeyPreference.Ability6);
		if (theAbility.m_ability5 != null)
		{
			theAbility.m_ability5.sprite = theAbility.m_sprite5;
		}
		m_abilities[6].Setup(theAbility.m_ability6, KeyPreference.Ability7);
		if (theAbility.m_ability6 != null)
		{
			theAbility.m_ability6.sprite = theAbility.m_sprite6;
		}
	}

	private void Update()
	{
		if (!m_setZtoZero)
		{
			return;
		}

		foreach (RectTransform childTransform in GetComponentsInChildren<RectTransform>())
		{
			if (childTransform != transform)
			{
				RectTransform obj = childTransform;
				Vector3 localPosition = childTransform.localPosition;
				float x = localPosition.x;
				Vector3 localPosition2 = childTransform.localPosition;
				obj.localPosition = new Vector3(x, localPosition2.y, 0f);
			}
		}
	}

	private void PurchaseMod(BaseEventData data)
	{
		if (m_currentlyPurchasingModButton != null)
		{
			m_currentlyPurchasingModButton.RequestPurchaseMod();
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModUnlocked);
		}
		m_purchasePanel.m_freelancerCurrencyButton.spriteController.ResetMouseState();
		UIManager.SetGameObjectActive(m_purchasePanel, false);
	}
}
