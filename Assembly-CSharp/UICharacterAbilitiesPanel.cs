using LobbyGameClientMessages;
using System;
using System.Collections;
using System.Collections.Generic;
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
		for (int i = 0; i < m_abilityButtons.Length; i++)
		{
			m_abilityButtons[i].SetCallback(AbilityButtonSelected);
		}
		m_clearModButton.SetCallback(ModButtonSelected);
		AddMouseEventPasser(m_clearModButton);
		if (m_resetAllMods != null)
		{
			while (true)
			{
				switch (2)
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
			m_resetAllMods.spriteController.callback = delegate
			{
				ClientGameManager clientGameManager = ClientGameManager.Get();
				AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.HasResetMods;
				if (clientGameManager.IsPlayerAccountDataAvailable())
				{
					while (true)
					{
						switch (2)
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
					if (clientGameManager.GetPlayerAccountData().AccountComponent.GetUIState(uiState) > 0)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						ResetAllMods(null);
						goto IL_00b6;
					}
				}
				ClientGameManager.Get().RequestUpdateUIState(uiState, 1, null);
				UIDialogPopupManager.OpenTwoButtonDialog(StringUtil.TR("ResetMods", "Global"), StringUtil.TR("ResetModsConfirm", "Global"), StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), ResetAllMods);
				goto IL_00b6;
				IL_00b6:
				m_resetAllMods.spriteController.ResetMouseState();
			};
		}
		UIManager.SetGameObjectActive(m_purchaseModTokenBtn, false);
		if (ClientGameManager.Get() != null)
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
			ClientGameManager.Get().OnModUnlocked += UICharacterSelectAbilitiesPanel_OnModUnlocked;
			ClientGameManager.Get().OnBankBalanceChange += UICharacterSelectAbilitiesPanel_OnBankBalanceChange;
			ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange += OnLobbyGameplayOverridesUpdated;
		}
		ResetLockIcons();
		bool flag = false;
		if (GameManager.Get() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (GameManager.Get().GameplayOverrides != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = GameManager.Get().GameplayOverrides.EnableMods;
			}
		}
		UIManager.SetGameObjectActive(m_modTokenCountText, false);
		if (m_modTokenLabel != null)
		{
			UIManager.SetGameObjectActive(m_modTokenLabel, false);
		}
		UIManager.SetGameObjectActive(m_purchasePanel, false);
		m_purchasePanel.m_freelancerCurrencyButton.spriteController.callback = PurchaseMod;
		if (!(m_disabledLabel != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			UIManager.SetGameObjectActive(m_disabledLabel, !flag);
			return;
		}
	}

	private void InitModButtonListIfNeeded()
	{
		if (m_cachedModButtonList.Count != 0)
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
			IEnumerator enumerator = m_modLayoutGroup.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					UIModSelectButton component = transform.GetComponent<UIModSelectButton>();
					if (component != null && component.gameObject != m_clearModButton.gameObject)
					{
						component.SetCallback(ModButtonSelected);
						AddMouseEventPasser(component);
						m_cachedModButtonList.Add(component);
					}
				}
				while (true)
				{
					switch (6)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							disposable.Dispose();
							goto end_IL_00b5;
						}
					}
				}
				end_IL_00b5:;
			}
		}
	}

	public void NotifyLoadoutUpdate(PlayerInfoUpdateResponse response)
	{
		if (m_modloadouts != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_modloadouts.NotifyLoadoutUpdate(response);
		}
		if (response.CharacterInfo == null)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			Setup(GameWideData.Get().GetCharacterResourceLink(response.CharacterInfo.CharacterType), true);
			return;
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
		if (newBalance.Type != CurrencyType.FreelancerCurrency)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UpdatePurchaseModTokenButtonState();
			return;
		}
	}

	private void UICharacterSelectAbilitiesPanel_OnModUnlocked(CharacterType character, PlayerModData unlockData)
	{
		if (character != m_selectedCharacter)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			foreach (UIModSelectButton cachedModButton in m_cachedModButtonList)
			{
				cachedModButton.SetLockIcons();
				if (ClientGameManager.Get().PurchasingMod && cachedModButton.GetMod() != null && cachedModButton.GetMod().m_abilityScopeId == ClientGameManager.Get().ModAttemptingToPurchase)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					DoModButtonSelected(cachedModButton.m_buttonHitBox.gameObject);
				}
			}
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModUnlocked);
			return;
		}
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
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			using (List<UIModSelectButton>.Enumerator enumerator = m_cachedModButtonList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIModSelectButton current = enumerator.Current;
					current.SetSelected(false);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			m_clearModButton.SetSelected(true);
			AppState_CharacterSelect.Get().UpdateSelectedMods(m_modInfo);
			UpdateModCounter();
			UpdateModEquipPointsLeft();
			return;
		}
	}

	internal void ShowOutOfModEquipPointsDialog()
	{
		UIManager.SetGameObjectActive(m_NotEnoughPointsNotificationAC, true);
	}

	public void _001D(BaseEventData _001D)
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
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			int numToPurchase = 1;
			int freelancerCurrencyPerModToken = GameBalanceVars.Get().FreelancerCurrencyPerModToken;
			int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.FreelancerCurrency);
			if (currentAmount >= freelancerCurrencyPerModToken)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						Log.Info(Log.Category.UI, "Sending mod token purchase request");
						ClientGameManager.Get().PurchaseModToken(numToPurchase);
						return;
					}
				}
			}
			string description = string.Format(StringUtil.TR("InsufficientCurrencyModToken", "Global"), StringUtil.TR("FreelancerCurrency", "Global"), UIStorePanel.FormatIntToString(freelancerCurrencyPerModToken, true), UIStorePanel.FormatIntToString(currentAmount, true));
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("InsufficientCredits", "Global"), description, StringUtil.TR("Ok", "Global"));
			return;
		}
	}

	private void UpdatePurchaseModTokenButtonState()
	{
		UIManager.SetGameObjectActive(m_purchaseModTokenBtn, false);
	}

	public void RefreshSelectedMods()
	{
		if (ClientGameManager.Get() == null || ClientGameManager.Get().GroupInfo == null)
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
			if (m_selectedCharacter == CharacterType.None)
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
				if (m_selectedCharacter.IsWillFill())
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
					if (m_selectedCharacter == CharacterType.PunchingDummy)
					{
						return;
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						if (m_selectedCharacter == CharacterType.Last)
						{
							while (true)
							{
								switch (6)
								{
								default:
									return;
								case 0:
									break;
								}
							}
						}
						CharacterModInfo characterModInfo;
						if (AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked)
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
							characterModInfo = ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter).CharacterComponent.LastRankedMods;
						}
						else
						{
							characterModInfo = ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter).CharacterComponent.LastMods;
						}
						for (int i = 0; i < m_abilityButtons.Length; i++)
						{
							if (i < m_abilities.Length)
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
								AbilityData.AbilityEntry abilityEntry = m_abilities[i];
								Ability ability = abilityEntry.ability;
								int num = characterModInfo.GetModForAbility(i);
								if (!AbilityModHelper.IsModAllowed(m_characterLink.m_characterType, i, num))
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									num = -1;
								}
								AbilityMod abilityMod = AbilityModHelper.GetModForAbility(ability, num);
								if (abilityMod != null && !abilityMod.EquippableForGameType())
								{
									abilityMod = null;
									num = -1;
									characterModInfo.SetModForAbility(i, -1);
								}
								m_abilityButtons[i].SetSelectedMod(abilityMod);
								m_abilityButtons[i].SetSelectedModIndex(num);
							}
							else
							{
								m_abilityButtons[i].SetSelectedMod(null);
								m_abilityButtons[i].SetSelectedModIndex(-1);
							}
							m_abilityButtons[i].SetSelected(m_selectedAbilityButton == m_abilityButtons[i]);
						}
						UpdateModCounter();
						UpdateModEquipPointsLeft();
						return;
					}
				}
			}
		}
	}

	internal void UpdateModCounter()
	{
		if (!m_canSelectMods)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		UICharacterScreen.Get().UpdateModIcons(m_abilityButtons, m_prepColor, m_dashColor, m_combatColor);
	}

	public void DoModButtonSelected(GameObject gameObject)
	{
		UIManager.SetGameObjectActive(m_purchasePanel, false);
		if (GameManager.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameManager.Get().GameplayOverrides != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
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
				}
			}
		}
		if (!m_canSelectMods)
		{
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		if (!UnderTotalModEquipCost(component.GetMod()))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					ShowOutOfModEquipPointsDialog();
					return;
				}
			}
		}
		UIModSelectButton uIModSelectButton = null;
		int selectedModIndex = -1;
		for (int i = 0; i < m_cachedModButtonList.Count; i++)
		{
			UIModSelectButton uIModSelectButton2 = m_cachedModButtonList[i];
			if (uIModSelectButton2.m_buttonHitBox.gameObject == gameObject)
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
				uIModSelectButton2.SetSelected(true);
				uIModSelectButton = uIModSelectButton2;
				selectedModIndex = i;
			}
			else
			{
				uIModSelectButton2.SetSelected(false);
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (m_clearModButton.m_buttonHitBox.gameObject == gameObject)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				m_clearModButton.SetSelected(true);
				uIModSelectButton = null;
			}
			else
			{
				m_clearModButton.SetSelected(false);
			}
			if (m_selectedAbilityButton != null)
			{
				if (uIModSelectButton != null)
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
					if (uIModSelectButton.GetMod() != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						m_selectedAbilityButton.SetSelectedMod(uIModSelectButton.GetMod());
						m_selectedAbilityButton.SetSelectedModIndex(selectedModIndex);
						m_modInfo.SetModForAbility(m_selectedAbilityIndex, uIModSelectButton.GetMod().m_abilityScopeId);
						UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
						GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndEquipMod, null);
						goto IL_02c3;
					}
				}
				m_selectedAbilityButton.SetSelectedMod(null);
				m_selectedAbilityButton.SetSelectedModIndex(-1);
				m_modInfo.SetModForAbility(m_selectedAbilityIndex, -1);
				UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModClear);
				goto IL_02c3;
			}
			goto IL_02d6;
			IL_02d6:
			UpdateModCounter();
			UpdateModEquipPointsLeft();
			return;
			IL_02c3:
			ClientGameManager.Get().UpdateSelectedMods(m_modInfo);
			goto IL_02d6;
		}
	}

	public void ModButtonSelected(BaseEventData data)
	{
		DoModButtonSelected(data.selectedObject);
	}

	public void RefreshSelectedVfxSwaps()
	{
		if (ClientGameManager.Get() == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ClientGameManager.Get().GroupInfo == null)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (m_selectedCharacter == CharacterType.None)
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (m_selectedCharacter.IsWillFill() || m_selectedCharacter == CharacterType.PunchingDummy)
					{
						return;
					}
					if (m_selectedCharacter == CharacterType.Last)
					{
						while (true)
						{
							switch (6)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					CharacterResourceLink displayedCharacter = GetDisplayedCharacter();
					PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter);
					CharacterAbilityVfxSwapInfo lastAbilityVfxSwaps = playerCharacterData.CharacterComponent.LastAbilityVfxSwaps;
					for (int i = 0; i < m_abilityButtons.Length; i++)
					{
						if (i < m_abilities.Length)
						{
							int abilityVfxSwapIdForAbility = lastAbilityVfxSwaps.GetAbilityVfxSwapIdForAbility(i);
							int selectedVfxSwapIndex = 0;
							CharacterAbilityVfxSwap selectedVfxSwap = null;
							if (abilityVfxSwapIdForAbility != 0)
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
								if (playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(i, abilityVfxSwapIdForAbility))
								{
									List<CharacterAbilityVfxSwap> availableVfxSwapsForAbilityIndex = displayedCharacter.GetAvailableVfxSwapsForAbilityIndex(i);
									for (int j = 0; j < availableVfxSwapsForAbilityIndex.Count; j++)
									{
										CharacterAbilityVfxSwap characterAbilityVfxSwap = availableVfxSwapsForAbilityIndex[j];
										if (characterAbilityVfxSwap.m_uniqueID == abilityVfxSwapIdForAbility)
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
											selectedVfxSwapIndex = j + 1;
											selectedVfxSwap = characterAbilityVfxSwap;
										}
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
							}
							m_abilityButtons[i].SetSelectedVfxSwap(selectedVfxSwap);
							m_abilityButtons[i].SetSelectedVfxSwapIndex(selectedVfxSwapIndex);
						}
						else
						{
							m_abilityButtons[i].SetSelectedVfxSwap(null);
							m_abilityButtons[i].SetSelectedVfxSwapIndex(0);
						}
					}
					while (true)
					{
						switch (6)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
		}
	}

	private void CreateVfxButtonsIfNeeded(int totalButtons)
	{
		for (int i = m_vfxSwapButtons.Count; i < totalButtons; i++)
		{
			UIVfxSwapSelectButton newButton = UnityEngine.Object.Instantiate(m_vfxButtonPrefab);
			newButton.SetCallback(VfxSwapButtonSelected);
			newButton.m_buttonHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.TauntPreview, delegate(UITooltipBase tooltip)
			{
				CharacterAbilityVfxSwap swap = newButton.GetSwap();
				if (swap != null)
				{
					while (true)
					{
						switch (4)
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
					if (!swap.m_swapVideoPath.IsNullOrEmpty())
					{
						(tooltip as UIFrontendTauntMouseoverVideo).Setup("Video/AbilityPreviews/" + swap.m_swapVideoPath);
						return true;
					}
				}
				return false;
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
		int num2 = 0;
		while (true)
		{
			if (num2 < m_vfxSwapButtons.Count)
			{
				if (m_vfxSwapButtons[num2].m_buttonHitBox.gameObject == data.selectedObject)
				{
					while (true)
					{
						switch (2)
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
					if (num2 == 0 || playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(m_selectedAbilityIndex, m_vfxSwapButtons[num2].GetSwap().m_uniqueID))
					{
						uIVfxSwapSelectButton = m_vfxSwapButtons[num2];
						num = num2;
					}
					break;
				}
				num2++;
				continue;
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
			break;
		}
		if (num == -1)
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
			for (int i = 0; i < m_vfxSwapButtons.Count; i++)
			{
				if (i == num)
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
					m_vfxSwapButtons[i].SetSelected(true);
				}
				else
				{
					m_vfxSwapButtons[i].SetSelected(false);
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (!(m_selectedAbilityButton != null))
				{
					return;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					if (uIVfxSwapSelectButton != null)
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
						if (uIVfxSwapSelectButton.GetSwap() != null)
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
							m_selectedAbilityButton.SetSelectedVfxSwap(uIVfxSwapSelectButton.GetSwap());
							m_selectedAbilityButton.SetSelectedVfxSwapIndex(num);
							m_abilityVfxSwapInfo.SetAbilityVfxSwapIdForAbility(m_selectedAbilityIndex, uIVfxSwapSelectButton.GetSwap().m_uniqueID);
							UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
							goto IL_0226;
						}
					}
					m_selectedAbilityButton.SetSelectedVfxSwap(null);
					m_selectedAbilityButton.SetSelectedVfxSwapIndex(-1);
					m_abilityVfxSwapInfo.SetAbilityVfxSwapIdForAbility(m_selectedAbilityIndex, 0);
					UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModClear);
					goto IL_0226;
					IL_0226:
					ClientGameManager.Get().UpdateSelectedAbilityVfxSwaps(m_abilityVfxSwapInfo);
					return;
				}
			}
		}
	}

	internal void UpdateModEquipPointsLeft(Text optionalAdditionaUpdate = null)
	{
		if (!m_canSelectMods)
		{
			return;
		}
		int num = CalculateTotalModEquipCost();
		int num2 = 0;
		int num3 = 0;
		num2 = num;
		num3 = 10;
		if (m_modEquipPointsLeftNotches.Length != num3)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < m_modEquipPointsLeftNotches.Length; i++)
			{
				UIManager.SetGameObjectActive(m_modEquipPointsLeftNotches[i], i < num2);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (optionalAdditionaUpdate != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						optionalAdditionaUpdate.text = $"{num2}/{num3}";
						return;
					}
				}
				return;
			}
		}
	}

	internal bool UnderTotalModEquipCost(AbilityMod testAbilityMod)
	{
		int num = 0;
		for (int i = 0; i < m_abilityButtons.Length; i++)
		{
			if (m_selectedAbilityButton != m_abilityButtons[i])
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
				AbilityMod modForAbility = AbilityModHelper.GetModForAbility(m_abilityButtons[i].GetAbilityEntry().ability, m_modInfo.GetModForAbility(i));
				if ((bool)modForAbility)
				{
					num += modForAbility.m_equipCost;
				}
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (testAbilityMod != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				num += testAbilityMod.m_equipCost;
			}
			return num <= 10;
		}
	}

	public int CalculateTotalModEquipCost()
	{
		int num = 0;
		for (int i = 0; i < m_abilityButtons.Length; i++)
		{
			UIAbilityButtonModPanel uIAbilityButtonModPanel = m_abilityButtons[i];
			AbilityData.AbilityEntry abilityEntry = uIAbilityButtonModPanel.GetAbilityEntry();
			if (abilityEntry == null)
			{
				continue;
			}
			Ability ability = abilityEntry.ability;
			int modForAbility = m_modInfo.GetModForAbility(i);
			AbilityMod modForAbility2 = AbilityModHelper.GetModForAbility(ability, modForAbility);
			if ((bool)modForAbility2)
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
				num += modForAbility2.m_equipCost;
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return num;
		}
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
			if (m_abilityButtons[i].m_buttonHitBox.gameObject == selectedButton)
			{
				while (true)
				{
					switch (5)
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
				m_abilityButtons[i].SetSelected(true, forceAnimation);
				m_selectedAbilityButton = m_abilityButtons[i];
				m_selectedAbilityIndex = i;
				if (m_vfxButtonContainer != null)
				{
					CharacterResourceLink displayedCharacter = GetDisplayedCharacter();
					if (displayedCharacter != null)
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
						List<CharacterAbilityVfxSwap> availableVfxSwapsForAbilityIndex = displayedCharacter.GetAvailableVfxSwapsForAbilityIndex(m_selectedAbilityIndex);
						PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(displayedCharacter.m_characterType);
						GameBalanceVars.CharacterUnlockData characterUnlockData = GameBalanceVars.Get().GetCharacterUnlockData(displayedCharacter.m_characterType);
						CreateVfxButtonsIfNeeded(availableVfxSwapsForAbilityIndex.Count + 1);
						int num = 0;
						m_vfxSwapButtons[0].SetVfxSwap(null, 1);
						UIManager.SetGameObjectActive(m_vfxSwapButtons[0], true);
						m_vfxSwapButtons[0].SetLocked(false);
						if (m_abilityButtons[i].GetSelectedVfxSwap() == m_vfxSwapButtons[0].GetSwap())
						{
							m_vfxSwapButtons[num].SetSelected(true, forceAnimation);
						}
						else
						{
							m_vfxSwapButtons[num].SetSelected(false, forceAnimation);
						}
						num++;
						foreach (CharacterAbilityVfxSwap item in availableVfxSwapsForAbilityIndex)
						{
							if (num >= m_vfxSwapButtons.Count)
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
								CreateVfxButtonsIfNeeded(num + 1);
							}
							if (item.m_isHidden)
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
								if (!flag)
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
									continue;
								}
							}
							GameBalanceVars.AbilityVfxUnlockData abilityVfxUnlockData = null;
							int num2 = 0;
							while (true)
							{
								if (num2 >= characterUnlockData.abilityVfxUnlockData.Length)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									break;
								}
								if (characterUnlockData.abilityVfxUnlockData[num2].ID == item.m_uniqueID)
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									abilityVfxUnlockData = characterUnlockData.abilityVfxUnlockData[num2];
									break;
								}
								num2++;
							}
							if (abilityVfxUnlockData != null && GameBalanceVarsExtensions.MeetsVisibilityConditions(abilityVfxUnlockData))
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
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				if (m_abilityButtons[i].GetAbilityEntry() == null)
				{
					continue;
				}
				if (m_modScrollView != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					m_modScrollView.verticalScrollbar.value = 1f;
				}
				List<AbilityMod> availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(m_abilityButtons[i].GetAbilityEntry().ability);
				int j = 0;
				bool flag3 = false;
				AbilityData.AbilityEntry abilityEntry = m_abilityButtons[i].GetAbilityEntry();
				using (List<AbilityMod>.Enumerator enumerator2 = availableModsForAbility.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						AbilityMod current2 = enumerator2.Current;
						if (j >= m_cachedModButtonList.Count)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							AllocateNewModButton();
						}
						UIModSelectButton uIModSelectButton = m_cachedModButtonList[j];
						bool flag4 = AbilityModHelper.IsModAllowed(m_selectedCharacter, i, current2.m_abilityScopeId);
						if (flag4)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							flag4 = current2.EquippableForGameType();
							if (!flag4)
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
								UIManager.SetGameObjectActive(uIModSelectButton, false);
							}
						}
						if (!flag4)
						{
							if (m_abilityButtons[i].GetSelectedMod() == current2)
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
								DoModButtonSelected(m_clearModButton.m_buttonHitBox.gameObject);
							}
							continue;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						uIModSelectButton.SetMod(current2, abilityEntry.ability, i, m_selectedCharacter);
						UIManager.SetGameObjectActive(uIModSelectButton, true);
						if (m_canSelectMods)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_abilityButtons[i].GetSelectedMod() == uIModSelectButton.GetMod())
							{
								uIModSelectButton.SetSelected(true, forceAnimation);
								flag3 = true;
								goto IL_0525;
							}
						}
						uIModSelectButton.SetSelected(false, forceAnimation);
						goto IL_0525;
						IL_0525:
						j++;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				for (; j < m_cachedModButtonList.Count; j++)
				{
					m_cachedModButtonList[j].SetSelected(false, forceAnimation);
					UIManager.SetGameObjectActive(m_cachedModButtonList[j], false);
				}
				UIManager.SetGameObjectActive(m_clearModButton, true);
				UIModSelectButton clearModButton = m_clearModButton;
				int selected;
				if (m_canSelectMods)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					selected = ((!flag3) ? 1 : 0);
				}
				else
				{
					selected = 0;
				}
				clearModButton.SetSelected((byte)selected != 0, forceAnimation);
				if (!m_abilityPreviewPanel)
				{
					continue;
				}
				if (abilityEntry != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (abilityEntry.ability != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (abilityEntry.ability.m_previewVideo != null)
						{
							m_abilityPreviewPanel.Play("Video/AbilityPreviews/" + abilityEntry.ability.m_previewVideo);
							continue;
						}
					}
				}
				m_abilityPreviewPanel.Stop();
			}
			else
			{
				m_abilityButtons[i].SetSelected(false, forceAnimation);
			}
		}
	}

	private void AllocateNewModButton()
	{
		UIModSelectButton uIModSelectButton = UnityEngine.Object.Instantiate(m_modButtonPrefab);
		if (uIModSelectButton == null)
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
		if (!(m_modScrollView != null))
		{
			return;
		}
		_MouseEventPasser mouseEventPasser = modComp.m_buttonHitBox.gameObject.GetComponent<_MouseEventPasser>();
		if (mouseEventPasser == null)
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
			if (m_abilityButtons.Length > i)
			{
				AbilityButtonSelectedHelper(m_abilityButtons[i].m_buttonHitBox.gameObject, false);
				if (playsound)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
				}
			}
			return;
		}
	}

	public void AbilityButtonSelected(UIAbilityButtonModPanel abilityBtn)
	{
		if (abilityBtn == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		AbilityButtonSelectedHelper(abilityBtn.m_buttonHitBox.gameObject, false);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
	}

	public void AbilityButtonSelected(BaseEventData data)
	{
		if (data.selectedObject == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
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
		if (playerCharacterData != null)
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
			if (m_canSelectMods)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag = AbilityMod.GetRequiredModStrictnessForGameSubType() == ModStrictness.Ranked;
				CharacterModInfo characterModInfo;
				if (flag)
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
					characterModInfo = playerCharacterData.CharacterComponent.LastRankedMods;
				}
				else
				{
					characterModInfo = playerCharacterData.CharacterComponent.LastMods;
				}
				CharacterAbilityVfxSwapInfo lastAbilityVfxSwaps = playerCharacterData.CharacterComponent.LastAbilityVfxSwaps;
				if (m_modloadouts != null)
				{
					m_modloadouts.Setup(characterLink);
				}
				if (sameCharacter)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_modInfo.Equals(characterModInfo))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_showingRankedMods == flag)
						{
							return;
						}
					}
				}
				if (ClientGameManager.Get().WaitingForModSelectResponse != -1)
				{
					return;
				}
				m_modInfo = characterModInfo;
				m_abilityVfxSwapInfo = lastAbilityVfxSwaps;
				m_showingRankedMods = flag;
			}
		}
		AbilityData component = m_characterLink.ActorDataPrefab.GetComponent<AbilityData>();
		SetupAbilityArray(component);
		for (int i = 0; i < m_abilityButtons.Length; i++)
		{
			if (i < m_abilities.Length)
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
				AbilityData.AbilityEntry abilityEntry = m_abilities[i];
				m_abilityButtons[i].Setup(abilityEntry);
				if (m_canSelectMods)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					Ability ability = abilityEntry.ability;
					int num = m_modInfo.GetModForAbility(i);
					if (!AbilityModHelper.IsModAllowed(m_characterLink.m_characterType, i, num))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						num = -1;
					}
					AbilityMod abilityMod = AbilityModHelper.GetModForAbility(ability, num);
					if (abilityMod != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!abilityMod.EquippableForGameType())
						{
							abilityMod = null;
							num = -1;
							m_modInfo.SetModForAbility(i, -1);
						}
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
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			for (int j = 0; j < m_cachedModButtonList.Count; j++)
			{
				m_cachedModButtonList[j].SetMod(null, null, 0, m_characterLink.m_characterType);
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				for (int k = 0; k < m_vfxSwapButtons.Count; k++)
				{
					m_vfxSwapButtons[k].SetVfxSwap(null, 0);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (m_selectedCharacter != m_characterLink.m_characterType)
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
						m_selectedAbilityButton = null;
						m_selectedAbilityIndex = -1;
					}
					UIAbilityButtonModPanel uIAbilityButtonModPanel = m_abilityButtons[0];
					if (m_selectedAbilityButton != null)
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
						uIAbilityButtonModPanel = m_selectedAbilityButton;
					}
					m_selectedCharacter = m_characterLink.m_characterType;
					m_clearModButton.SetMod(null, null, 0, CharacterType.None);
					UpdateModCounter();
					if (CalculateTotalModEquipCost() > 10)
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
						ResetAllMods(null);
					}
					UpdateModEquipPointsLeft();
					RefreshSelectedVfxSwaps();
					AbilityButtonSelectedHelper(uIAbilityButtonModPanel.m_buttonHitBox.gameObject, true);
					return;
				}
			}
		}
	}

	public void RefreshKeyBindings()
	{
		for (int i = 0; i < m_abilityButtons.Length; i++)
		{
			if (i < m_abilities.Length)
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
				m_abilityButtons[i].RefreshHotkey();
			}
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
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
			m_abilities[0].Setup(theAbility.m_ability0, KeyPreference.Ability1);
			if (theAbility.m_ability0 != null)
			{
				theAbility.m_ability0.sprite = theAbility.m_sprite0;
			}
			m_abilities[1].Setup(theAbility.m_ability1, KeyPreference.Ability2);
			if (theAbility.m_ability1 != null)
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
				theAbility.m_ability1.sprite = theAbility.m_sprite1;
			}
			m_abilities[2].Setup(theAbility.m_ability2, KeyPreference.Ability3);
			if (theAbility.m_ability2 != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				theAbility.m_ability2.sprite = theAbility.m_sprite2;
			}
			m_abilities[3].Setup(theAbility.m_ability3, KeyPreference.Ability4);
			if (theAbility.m_ability3 != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				theAbility.m_ability3.sprite = theAbility.m_sprite3;
			}
			m_abilities[4].Setup(theAbility.m_ability4, KeyPreference.Ability5);
			if (theAbility.m_ability4 != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				theAbility.m_ability4.sprite = theAbility.m_sprite4;
			}
			m_abilities[5].Setup(theAbility.m_ability5, KeyPreference.Ability6);
			if (theAbility.m_ability5 != null)
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
				theAbility.m_ability5.sprite = theAbility.m_sprite5;
			}
			m_abilities[6].Setup(theAbility.m_ability6, KeyPreference.Ability7);
			if (theAbility.m_ability6 != null)
			{
				theAbility.m_ability6.sprite = theAbility.m_sprite6;
			}
			return;
		}
	}

	private void Update()
	{
		if (!m_setZtoZero)
		{
			return;
		}
		RectTransform[] componentsInChildren = GetComponentsInChildren<RectTransform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != base.transform)
			{
				while (true)
				{
					switch (4)
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
				RectTransform obj = componentsInChildren[i];
				Vector3 localPosition = componentsInChildren[i].localPosition;
				float x = localPosition.x;
				Vector3 localPosition2 = componentsInChildren[i].localPosition;
				obj.localPosition = new Vector3(x, localPosition2.y, 0f);
			}
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void PurchaseMod(BaseEventData data)
	{
		if (m_currentlyPurchasingModButton != null)
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
			m_currentlyPurchasingModButton.RequestPurchaseMod();
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModUnlocked);
		}
		m_purchasePanel.m_freelancerCurrencyButton.spriteController.ResetMouseState();
		UIManager.SetGameObjectActive(m_purchasePanel, false);
	}
}
