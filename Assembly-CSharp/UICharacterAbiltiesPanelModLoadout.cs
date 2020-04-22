using LobbyGameClientMessages;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterAbiltiesPanelModLoadout : MonoBehaviour
{
	public RectTransform m_dropdownMenuContainer;

	public RectTransform m_inputfieldContainer;

	public InputField m_inputfield;

	public _SelectableBtn m_loadoutListToggleBtn;

	public _SelectableBtn m_renameButton;

	public RectTransform m_acceptDeclineBtnContainer;

	public _SelectableBtn m_acceptButton;

	public _SelectableBtn m_declineButton;

	public _SelectableBtn m_saveButton;

	public ScrollRect m_listScrollRect;

	public VerticalLayoutGroup m_loadoutList;

	public UICharacterAbilitiesPanelModItem m_modItemPrfab;

	public _SelectableBtn m_buyNewLoadoutBtn;

	public TextMeshProUGUI[] m_buyLoadoutSlotTexts;

	private CharacterType m_selectedCharacter;

	private List<CharacterLoadout> m_loadouts;

	private bool m_initialized;

	private bool m_listIsOpen;

	private bool m_isRenamingLoadout;

	private bool m_pendingLoadingNewLoadout;

	private List<UICharacterAbilitiesPanelModItem> m_modListItems = new List<UICharacterAbilitiesPanelModItem>();

	public void Awake()
	{
		Init();
	}

	private void Init()
	{
		if (m_initialized)
		{
			return;
		}
		if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
		m_initialized = true;
		SetListVisible(false);
		UIManager.SetGameObjectActive(m_acceptDeclineBtnContainer, false);
		UIManager.SetGameObjectActive(m_renameButton, false);
		if (m_saveButton != null)
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
			m_saveButton.SetDisabled(true);
			m_saveButton.spriteController.callback = SaveBtnClicked;
		}
		UIManager.SetGameObjectActive(m_inputfieldContainer, false);
		m_loadoutListToggleBtn.SetSelected(false, true, string.Empty, string.Empty);
		m_pendingLoadingNewLoadout = false;
		m_loadoutListToggleBtn.spriteController.callback = LoadoutToggleBtnClicked;
		m_renameButton.spriteController.callback = RenameButtonClicked;
		m_acceptButton.spriteController.callback = AcceptButtonClicked;
		m_declineButton.spriteController.callback = DeclineButtonClicked;
		m_buyNewLoadoutBtn.spriteController.callback = BuyLoadoutSlotClicked;
		m_buyNewLoadoutBtn.spriteController.RegisterScrollListener(OnListScroll);
		for (int i = 0; i < m_buyLoadoutSlotTexts.Length; i++)
		{
			m_buyLoadoutSlotTexts[i].text = string.Format(StringUtil.TR("BuyLoadoutSlotFor", "SceneGlobal"), "<sprite name=credit>" + GameBalanceVars.Get().FreelancerLoadoutSlotFluxCost);
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			m_inputfield.onValueChanged.AddListener(OnTypeInput);
			return;
		}
	}

	private void Start()
	{
		ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
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
			ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterDataUpdated;
			return;
		}
	}

	private void OnListScroll(BaseEventData data)
	{
		m_listScrollRect.OnScroll((PointerEventData)data);
	}

	public void OnTypeInput(string textString)
	{
		if (m_inputfield.text.Length > 16)
		{
			m_inputfield.text = m_inputfield.text.Substring(0, 16);
		}
	}

	public void LoadoutToggleBtnClicked(BaseEventData data)
	{
		SetListVisible(!m_listIsOpen);
	}

	public void RenameButtonClicked(BaseEventData data)
	{
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter);
		int num = playerCharacterData.CharacterComponent.LastSelectedLoadout;
		if (UseRankedLoadouts())
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
			num = playerCharacterData.CharacterComponent.LastSelectedRankedLoadout;
		}
		if (-1 < num && num < m_loadouts.Count)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					CharacterLoadout characterLoadout = m_loadouts[num];
					m_isRenamingLoadout = true;
					UIManager.SetGameObjectActive(m_inputfieldContainer, true);
					UIManager.SetGameObjectActive(m_renameButton, false);
					UIManager.SetGameObjectActive(m_acceptDeclineBtnContainer, true);
					UIManager.SetGameObjectActive(m_dropdownMenuContainer, false);
					m_inputfield.text = StringUtil.TR_GetLoadoutName(characterLoadout.LoadoutName);
					m_inputfield.Select();
					return;
				}
				}
			}
		}
		Log.Error("Last selected loadout index is out of range of loadouts");
	}

	private void CloseRename()
	{
		m_isRenamingLoadout = false;
		UIManager.SetGameObjectActive(m_inputfieldContainer, false);
		UIManager.SetGameObjectActive(m_renameButton, true);
		UIManager.SetGameObjectActive(m_acceptDeclineBtnContainer, false);
	}

	private bool UseRankedLoadouts()
	{
		ModStrictness requiredModStrictnessForGameSubType = AbilityMod.GetRequiredModStrictnessForGameSubType();
		return requiredModStrictnessForGameSubType == ModStrictness.Ranked;
	}

	public void SaveBtnClicked(BaseEventData data)
	{
		List<CharacterLoadout> list = new List<CharacterLoadout>();
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter);
		int num = playerCharacterData.CharacterComponent.LastSelectedLoadout;
		List<CharacterLoadout> list2 = null;
		if (UseRankedLoadouts())
		{
			num = playerCharacterData.CharacterComponent.LastSelectedRankedLoadout;
			list2 = playerCharacterData.CharacterComponent.CharacterLoadoutsRanked;
		}
		else
		{
			list2 = playerCharacterData.CharacterComponent.CharacterLoadouts;
		}
		for (int i = 0; i < list2.Count; i++)
		{
			if (i == num)
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
				CharacterModInfo modInfo = (!UseRankedLoadouts()) ? playerCharacterData.CharacterComponent.LastMods : playerCharacterData.CharacterComponent.LastRankedMods;
				CharacterAbilityVfxSwapInfo lastAbilityVfxSwaps = playerCharacterData.CharacterComponent.LastAbilityVfxSwaps;
				string loadoutName = list2[i].LoadoutName;
				int strictness;
				if (UseRankedLoadouts())
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
					strictness = 1;
				}
				else
				{
					strictness = 0;
				}
				CharacterLoadout item = new CharacterLoadout(modInfo, lastAbilityVfxSwaps, loadoutName, (ModStrictness)strictness);
				list.Add(item);
			}
			else
			{
				CharacterLoadout item2 = new CharacterLoadout(list2[i].ModSet, list2[i].VFXSet, list2[i].LoadoutName, list2[i].Strictness);
				list.Add(item2);
			}
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			ClientGameManager.Get().UpdateLoadouts(list);
			return;
		}
	}

	public void AcceptButtonClicked(BaseEventData data)
	{
		if (!m_isRenamingLoadout)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			CharacterLoadout characterLoadout = null;
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter);
			int num = playerCharacterData.CharacterComponent.LastSelectedLoadout;
			if (UseRankedLoadouts())
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
				num = playerCharacterData.CharacterComponent.LastSelectedRankedLoadout;
			}
			if (-1 < num && num < m_loadouts.Count)
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
				characterLoadout = m_loadouts[num];
			}
			if (characterLoadout == null)
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
				List<CharacterLoadout> list = new List<CharacterLoadout>();
				for (int i = 0; i < m_loadouts.Count; i++)
				{
					CharacterLoadout characterLoadout2 = new CharacterLoadout(m_loadouts[i].ModSet, m_loadouts[i].VFXSet, m_loadouts[i].LoadoutName, m_loadouts[i].Strictness);
					if (m_loadouts[i] == characterLoadout)
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
						characterLoadout2.LoadoutName = m_inputfield.text;
					}
					list.Add(characterLoadout2);
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					ClientGameManager.Get().UpdateLoadouts(list);
					return;
				}
			}
		}
	}

	public void NotifyLoadoutUpdate(PlayerInfoUpdateResponse response)
	{
		if (response == null)
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
			Log.Error("Response is null somehow...");
		}
		if (response.CharacterInfo == null)
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
			Log.Error("Character info in response is null");
		}
		if (!response.Success)
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
			Log.Error("Failed to update loadouts: " + response.ErrorMessage);
		}
		if (ClientGameManager.Get() == null)
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
			Log.Error("Client Game Manager is somehow null...");
		}
		if (ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter) == null)
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
			Log.Error("Failed to get character data");
		}
		if (ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter).CharacterComponent == null)
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
			Log.Error("Character Componenent is null");
		}
		int numCharacterLoadouts = ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter).CharacterComponent.NumCharacterLoadouts;
		UpdateLoadouts(response.CharacterInfo.CharacterLoadouts, numCharacterLoadouts);
		CloseRename();
	}

	public void DeclineButtonClicked(BaseEventData data)
	{
		if (m_isRenamingLoadout)
		{
			CloseRename();
		}
	}

	public void BuyLoadoutSlotClicked(BaseEventData data)
	{
		SetListVisible(false);
		if (m_selectedCharacter != 0)
		{
			m_pendingLoadingNewLoadout = true;
			ClientGameManager.Get().PurchaseLoadoutSlot(m_selectedCharacter);
		}
	}

	private void OnCharacterDataUpdated(PersistedCharacterData charData)
	{
		if (charData.CharacterType != m_selectedCharacter)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (UseRankedLoadouts())
			{
				UpdateLoadouts(charData.CharacterComponent.CharacterLoadoutsRanked, charData.CharacterComponent.NumCharacterLoadouts);
			}
			else
			{
				UpdateLoadouts(charData.CharacterComponent.CharacterLoadouts, charData.CharacterComponent.NumCharacterLoadouts);
			}
			if (m_pendingLoadingNewLoadout)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					m_pendingLoadingNewLoadout = false;
					NotifyModLoadoutClicked(m_modListItems[m_modListItems.Count - 1]);
					return;
				}
			}
			return;
		}
	}

	public void NotifyModLoadoutClicked(UICharacterAbilitiesPanelModItem item)
	{
		int loadoutIndex = -1;
		int num = 0;
		while (true)
		{
			if (num < m_modListItems.Count)
			{
				if (m_modListItems[num] == item)
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
					loadoutIndex = num;
					break;
				}
				num++;
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		ClientGameManager.Get().SendUIActionNotification("CLICK (UILoadout): Load Request");
		ClientGameManager.Get().RequestToSelectLoadout(item.LoadoutRef, loadoutIndex);
		SetListVisible(false);
	}

	public void NotifySaveModLoadoutClicked(UICharacterAbilitiesPanelModItem item)
	{
		List<CharacterLoadout> list = new List<CharacterLoadout>();
		for (int i = 0; i < m_modListItems.Count; i++)
		{
			if (m_modListItems[i] == item)
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
				PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter);
				CharacterModInfo modInfo;
				if (UseRankedLoadouts())
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
					modInfo = playerCharacterData.CharacterComponent.LastRankedMods;
				}
				else
				{
					modInfo = playerCharacterData.CharacterComponent.LastMods;
				}
				CharacterAbilityVfxSwapInfo lastAbilityVfxSwaps = playerCharacterData.CharacterComponent.LastAbilityVfxSwaps;
				string loadoutName = m_loadouts[i].LoadoutName;
				int strictness;
				if (UseRankedLoadouts())
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
					strictness = 1;
				}
				else
				{
					strictness = 0;
				}
				CharacterLoadout item2 = new CharacterLoadout(modInfo, lastAbilityVfxSwaps, loadoutName, (ModStrictness)strictness);
				list.Add(item2);
			}
			else
			{
				CharacterLoadout item3 = new CharacterLoadout(m_loadouts[i].ModSet, m_loadouts[i].VFXSet, m_loadouts[i].LoadoutName, m_loadouts[i].Strictness);
				list.Add(item3);
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			ClientGameManager.Get().SendUIActionNotification("CLICK (UILoadout): Save Request");
			ClientGameManager.Get().UpdateLoadouts(list);
			SetListVisible(false);
			return;
		}
	}

	private void SetToggleButtonLabels(string text)
	{
		TextMeshProUGUI[] componentsInChildren = m_loadoutListToggleBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = text;
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
			return;
		}
	}

	private void SetListVisible(bool visible)
	{
		m_listIsOpen = visible;
		UIManager.SetGameObjectActive(m_dropdownMenuContainer, visible);
		if (visible)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < m_modListItems.Count; i++)
			{
				m_modListItems[i].m_btn.spriteController.ResetMouseState();
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

	public void Setup(CharacterResourceLink charLink)
	{
		Init();
		m_selectedCharacter = charLink.m_characterType;
		if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charLink.m_characterType);
			if (UseRankedLoadouts())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						UpdateLoadouts(playerCharacterData.CharacterComponent.CharacterLoadoutsRanked, playerCharacterData.CharacterComponent.NumCharacterLoadouts);
						return;
					}
				}
			}
			UpdateLoadouts(playerCharacterData.CharacterComponent.CharacterLoadouts, playerCharacterData.CharacterComponent.NumCharacterLoadouts);
			return;
		}
	}

	public void UpdateLoadouts(List<CharacterLoadout> loadouts, int NumCharacterLoadouts)
	{
		Init();
		if (!m_loadoutList.Equals(loadouts))
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
			m_loadouts = loadouts;
			for (int i = 0; i < m_loadouts.Count; i++)
			{
				if (i >= m_modListItems.Count)
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
					if (m_modItemPrfab != null)
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
						UICharacterAbilitiesPanelModItem uICharacterAbilitiesPanelModItem = Object.Instantiate(m_modItemPrfab);
						uICharacterAbilitiesPanelModItem.transform.SetParent(m_loadoutList.transform);
						uICharacterAbilitiesPanelModItem.transform.localEulerAngles = Vector3.zero;
						uICharacterAbilitiesPanelModItem.transform.localPosition = Vector3.zero;
						uICharacterAbilitiesPanelModItem.transform.localScale = Vector3.one;
						uICharacterAbilitiesPanelModItem.m_btn.spriteController.RegisterScrollListener(OnListScroll);
						m_modListItems.Add(uICharacterAbilitiesPanelModItem);
					}
					else
					{
						Debug.LogError(base.name + " field Mod Item Prefab reference is null, please set it in scene");
					}
				}
				if (i < m_modListItems.Count)
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
					m_modListItems[i].Setup(m_loadouts[i]);
					UIManager.SetGameObjectActive(m_modListItems[i], true);
				}
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
			for (int num = m_modListItems.Count - 1; num >= m_loadouts.Count; num--)
			{
				UIManager.SetGameObjectActive(m_modListItems[num], false);
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
			CheckCurrentModSelection();
		}
		m_buyNewLoadoutBtn.transform.SetAsLastSibling();
		UIManager.SetGameObjectActive(m_buyNewLoadoutBtn, NumCharacterLoadouts < GameBalanceVars.Get().MaxNumberOfFreelancerLoadoutSlots);
	}

	private void CheckCurrentModSelection()
	{
		if (m_selectedCharacter == CharacterType.None)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			string text = StringUtil.TR("SelectLoadout", "Global");
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_selectedCharacter);
			int num = playerCharacterData.CharacterComponent.LastSelectedLoadout;
			CharacterModInfo characterModInfo = playerCharacterData.CharacterComponent.LastMods;
			if (UseRankedLoadouts())
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
				num = playerCharacterData.CharacterComponent.LastSelectedRankedLoadout;
				characterModInfo = playerCharacterData.CharacterComponent.LastRankedMods;
			}
			if (-1 < num)
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
				if (num < m_loadouts.Count)
				{
					CharacterLoadout characterLoadout = m_loadouts[num];
					text = StringUtil.TR_GetLoadoutName(characterLoadout.LoadoutName);
					if (characterLoadout.ModSet.Equals(characterModInfo))
					{
						if (m_saveButton != null)
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
							m_saveButton.SetDisabled(true);
						}
					}
					else if (m_saveButton != null)
					{
						m_saveButton.SetDisabled(false);
					}
					UIManager.SetGameObjectActive(m_renameButton, true);
					m_loadoutListToggleBtn.SetSelected(true, true, string.Empty, string.Empty);
					SetToggleButtonLabels(text);
					return;
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
			Log.Error("Last selected loadout index is out of range of loadouts");
			return;
		}
	}

	private void Update()
	{
		if (!Input.GetMouseButtonDown(0))
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
			bool flag = true;
			if (EventSystem.current != null)
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
				if (EventSystem.current.IsPointerOverGameObject(-1))
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
					StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
					if (component != null)
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
						if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
						{
							RectTransform rectTransform = component.GetLastPointerEventDataPublic(-1).pointerEnter.transform as RectTransform;
							if (rectTransform != null)
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
								RectTransform[] componentsInChildren = m_loadoutListToggleBtn.GetComponentsInChildren<RectTransform>(true);
								for (int i = 0; i < componentsInChildren.Length; i++)
								{
									if (componentsInChildren[i].gameObject.transform == rectTransform)
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
										flag = false;
										break;
									}
								}
								if (flag)
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
									componentsInChildren = m_dropdownMenuContainer.GetComponentsInChildren<RectTransform>(true);
									int num = 0;
									while (true)
									{
										if (num < componentsInChildren.Length)
										{
											if (componentsInChildren[num].gameObject.transform == rectTransform)
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
												flag = false;
												break;
											}
											num++;
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
										break;
									}
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					SetListVisible(false);
					return;
				}
			}
			return;
		}
	}
}
