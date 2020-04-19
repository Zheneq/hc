using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
		this.Init();
	}

	private void Init()
	{
		if (!this.m_initialized)
		{
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				this.m_initialized = true;
				this.SetListVisible(false);
				UIManager.SetGameObjectActive(this.m_acceptDeclineBtnContainer, false, null);
				UIManager.SetGameObjectActive(this.m_renameButton, false, null);
				if (this.m_saveButton != null)
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
					this.m_saveButton.SetDisabled(true);
					this.m_saveButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SaveBtnClicked);
				}
				UIManager.SetGameObjectActive(this.m_inputfieldContainer, false, null);
				this.m_loadoutListToggleBtn.SetSelected(false, true, string.Empty, string.Empty);
				this.m_pendingLoadingNewLoadout = false;
				this.m_loadoutListToggleBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.LoadoutToggleBtnClicked);
				this.m_renameButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.RenameButtonClicked);
				this.m_acceptButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.AcceptButtonClicked);
				this.m_declineButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.DeclineButtonClicked);
				this.m_buyNewLoadoutBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BuyLoadoutSlotClicked);
				this.m_buyNewLoadoutBtn.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnListScroll));
				for (int i = 0; i < this.m_buyLoadoutSlotTexts.Length; i++)
				{
					this.m_buyLoadoutSlotTexts[i].text = string.Format(StringUtil.TR("BuyLoadoutSlotFor", "SceneGlobal"), "<sprite name=credit>" + GameBalanceVars.Get().FreelancerLoadoutSlotFluxCost);
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_inputfield.onValueChanged.AddListener(new UnityAction<string>(this.OnTypeInput));
				return;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.Init()).MethodHandle;
			}
		}
	}

	private void Start()
	{
		ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterDataUpdated;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.OnDestroy()).MethodHandle;
			}
			ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterDataUpdated;
		}
	}

	private void OnListScroll(BaseEventData data)
	{
		this.m_listScrollRect.OnScroll((PointerEventData)data);
	}

	public void OnTypeInput(string textString)
	{
		if (this.m_inputfield.text.Length > 0x10)
		{
			this.m_inputfield.text = this.m_inputfield.text.Substring(0, 0x10);
		}
	}

	public void LoadoutToggleBtnClicked(BaseEventData data)
	{
		this.SetListVisible(!this.m_listIsOpen);
	}

	public void RenameButtonClicked(BaseEventData data)
	{
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_selectedCharacter);
		int num = playerCharacterData.CharacterComponent.LastSelectedLoadout;
		if (this.UseRankedLoadouts())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.RenameButtonClicked(BaseEventData)).MethodHandle;
			}
			num = playerCharacterData.CharacterComponent.LastSelectedRankedLoadout;
		}
		if (-1 < num && num < this.m_loadouts.Count)
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
			CharacterLoadout characterLoadout = this.m_loadouts[num];
			this.m_isRenamingLoadout = true;
			UIManager.SetGameObjectActive(this.m_inputfieldContainer, true, null);
			UIManager.SetGameObjectActive(this.m_renameButton, false, null);
			UIManager.SetGameObjectActive(this.m_acceptDeclineBtnContainer, true, null);
			UIManager.SetGameObjectActive(this.m_dropdownMenuContainer, false, null);
			this.m_inputfield.text = StringUtil.TR_GetLoadoutName(characterLoadout.LoadoutName);
			this.m_inputfield.Select();
		}
		else
		{
			Log.Error("Last selected loadout index is out of range of loadouts", new object[0]);
		}
	}

	private void CloseRename()
	{
		this.m_isRenamingLoadout = false;
		UIManager.SetGameObjectActive(this.m_inputfieldContainer, false, null);
		UIManager.SetGameObjectActive(this.m_renameButton, true, null);
		UIManager.SetGameObjectActive(this.m_acceptDeclineBtnContainer, false, null);
	}

	private bool UseRankedLoadouts()
	{
		ModStrictness requiredModStrictnessForGameSubType = AbilityMod.GetRequiredModStrictnessForGameSubType();
		return requiredModStrictnessForGameSubType == ModStrictness.Ranked;
	}

	public void SaveBtnClicked(BaseEventData data)
	{
		List<CharacterLoadout> list = new List<CharacterLoadout>();
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_selectedCharacter);
		int num = playerCharacterData.CharacterComponent.LastSelectedLoadout;
		List<CharacterLoadout> list2;
		if (this.UseRankedLoadouts())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.SaveBtnClicked(BaseEventData)).MethodHandle;
				}
				CharacterModInfo modInfo = (!this.UseRankedLoadouts()) ? playerCharacterData.CharacterComponent.LastMods : playerCharacterData.CharacterComponent.LastRankedMods;
				CharacterAbilityVfxSwapInfo lastAbilityVfxSwaps = playerCharacterData.CharacterComponent.LastAbilityVfxSwaps;
				string loadoutName = list2[i].LoadoutName;
				ModStrictness strictness;
				if (this.UseRankedLoadouts())
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
					strictness = ModStrictness.Ranked;
				}
				else
				{
					strictness = ModStrictness.AllModes;
				}
				CharacterLoadout item = new CharacterLoadout(modInfo, lastAbilityVfxSwaps, loadoutName, strictness);
				list.Add(item);
			}
			else
			{
				CharacterLoadout item2 = new CharacterLoadout(list2[i].ModSet, list2[i].VFXSet, list2[i].LoadoutName, list2[i].Strictness);
				list.Add(item2);
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		ClientGameManager.Get().UpdateLoadouts(list, 0);
	}

	public void AcceptButtonClicked(BaseEventData data)
	{
		if (this.m_isRenamingLoadout)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.AcceptButtonClicked(BaseEventData)).MethodHandle;
			}
			CharacterLoadout characterLoadout = null;
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_selectedCharacter);
			int num = playerCharacterData.CharacterComponent.LastSelectedLoadout;
			if (this.UseRankedLoadouts())
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
				num = playerCharacterData.CharacterComponent.LastSelectedRankedLoadout;
			}
			if (-1 < num && num < this.m_loadouts.Count)
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
				characterLoadout = this.m_loadouts[num];
			}
			if (characterLoadout != null)
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
				List<CharacterLoadout> list = new List<CharacterLoadout>();
				for (int i = 0; i < this.m_loadouts.Count; i++)
				{
					CharacterLoadout characterLoadout2 = new CharacterLoadout(this.m_loadouts[i].ModSet, this.m_loadouts[i].VFXSet, this.m_loadouts[i].LoadoutName, this.m_loadouts[i].Strictness);
					if (this.m_loadouts[i] == characterLoadout)
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
						characterLoadout2.LoadoutName = this.m_inputfield.text;
					}
					list.Add(characterLoadout2);
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				ClientGameManager.Get().UpdateLoadouts(list, 0);
			}
		}
	}

	public void NotifyLoadoutUpdate(PlayerInfoUpdateResponse response)
	{
		if (response == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.NotifyLoadoutUpdate(PlayerInfoUpdateResponse)).MethodHandle;
			}
			Log.Error("Response is null somehow...", new object[0]);
		}
		if (response.CharacterInfo == null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Error("Character info in response is null", new object[0]);
		}
		if (!response.Success)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Error("Failed to update loadouts: " + response.ErrorMessage, new object[0]);
		}
		if (ClientGameManager.Get() == null)
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
			Log.Error("Client Game Manager is somehow null...", new object[0]);
		}
		if (ClientGameManager.Get().GetPlayerCharacterData(this.m_selectedCharacter) == null)
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
			Log.Error("Failed to get character data", new object[0]);
		}
		if (ClientGameManager.Get().GetPlayerCharacterData(this.m_selectedCharacter).CharacterComponent == null)
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
			Log.Error("Character Componenent is null", new object[0]);
		}
		int numCharacterLoadouts = ClientGameManager.Get().GetPlayerCharacterData(this.m_selectedCharacter).CharacterComponent.NumCharacterLoadouts;
		this.UpdateLoadouts(response.CharacterInfo.CharacterLoadouts, numCharacterLoadouts);
		this.CloseRename();
	}

	public void DeclineButtonClicked(BaseEventData data)
	{
		if (this.m_isRenamingLoadout)
		{
			this.CloseRename();
		}
	}

	public void BuyLoadoutSlotClicked(BaseEventData data)
	{
		this.SetListVisible(false);
		if (this.m_selectedCharacter == CharacterType.None)
		{
			return;
		}
		this.m_pendingLoadingNewLoadout = true;
		ClientGameManager.Get().PurchaseLoadoutSlot(this.m_selectedCharacter, null);
	}

	private void OnCharacterDataUpdated(PersistedCharacterData charData)
	{
		if (charData.CharacterType == this.m_selectedCharacter)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.OnCharacterDataUpdated(PersistedCharacterData)).MethodHandle;
			}
			if (this.UseRankedLoadouts())
			{
				this.UpdateLoadouts(charData.CharacterComponent.CharacterLoadoutsRanked, charData.CharacterComponent.NumCharacterLoadouts);
			}
			else
			{
				this.UpdateLoadouts(charData.CharacterComponent.CharacterLoadouts, charData.CharacterComponent.NumCharacterLoadouts);
			}
			if (this.m_pendingLoadingNewLoadout)
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
				this.m_pendingLoadingNewLoadout = false;
				this.NotifyModLoadoutClicked(this.m_modListItems[this.m_modListItems.Count - 1]);
			}
		}
	}

	public void NotifyModLoadoutClicked(UICharacterAbilitiesPanelModItem item)
	{
		int loadoutIndex = -1;
		for (int i = 0; i < this.m_modListItems.Count; i++)
		{
			if (this.m_modListItems[i] == item)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.NotifyModLoadoutClicked(UICharacterAbilitiesPanelModItem)).MethodHandle;
				}
				loadoutIndex = i;
				IL_51:
				ClientGameManager.Get().SendUIActionNotification("CLICK (UILoadout): Load Request");
				ClientGameManager.Get().RequestToSelectLoadout(item.LoadoutRef, loadoutIndex);
				this.SetListVisible(false);
				return;
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			goto IL_51;
		}
	}

	public void NotifySaveModLoadoutClicked(UICharacterAbilitiesPanelModItem item)
	{
		List<CharacterLoadout> list = new List<CharacterLoadout>();
		for (int i = 0; i < this.m_modListItems.Count; i++)
		{
			if (this.m_modListItems[i] == item)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.NotifySaveModLoadoutClicked(UICharacterAbilitiesPanelModItem)).MethodHandle;
				}
				PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_selectedCharacter);
				CharacterModInfo modInfo;
				if (this.UseRankedLoadouts())
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
					modInfo = playerCharacterData.CharacterComponent.LastRankedMods;
				}
				else
				{
					modInfo = playerCharacterData.CharacterComponent.LastMods;
				}
				CharacterAbilityVfxSwapInfo lastAbilityVfxSwaps = playerCharacterData.CharacterComponent.LastAbilityVfxSwaps;
				string loadoutName = this.m_loadouts[i].LoadoutName;
				ModStrictness strictness;
				if (this.UseRankedLoadouts())
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
					strictness = ModStrictness.Ranked;
				}
				else
				{
					strictness = ModStrictness.AllModes;
				}
				CharacterLoadout item2 = new CharacterLoadout(modInfo, lastAbilityVfxSwaps, loadoutName, strictness);
				list.Add(item2);
			}
			else
			{
				CharacterLoadout item3 = new CharacterLoadout(this.m_loadouts[i].ModSet, this.m_loadouts[i].VFXSet, this.m_loadouts[i].LoadoutName, this.m_loadouts[i].Strictness);
				list.Add(item3);
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		ClientGameManager.Get().SendUIActionNotification("CLICK (UILoadout): Save Request");
		ClientGameManager.Get().UpdateLoadouts(list, 0);
		this.SetListVisible(false);
	}

	private void SetToggleButtonLabels(string text)
	{
		TextMeshProUGUI[] componentsInChildren = this.m_loadoutListToggleBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = text;
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.SetToggleButtonLabels(string)).MethodHandle;
		}
	}

	private void SetListVisible(bool visible)
	{
		this.m_listIsOpen = visible;
		UIManager.SetGameObjectActive(this.m_dropdownMenuContainer, visible, null);
		if (!visible)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.SetListVisible(bool)).MethodHandle;
			}
			for (int i = 0; i < this.m_modListItems.Count; i++)
			{
				this.m_modListItems[i].m_btn.spriteController.ResetMouseState();
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public void Setup(CharacterResourceLink charLink)
	{
		this.Init();
		this.m_selectedCharacter = charLink.m_characterType;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.Setup(CharacterResourceLink)).MethodHandle;
			}
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charLink.m_characterType);
			if (this.UseRankedLoadouts())
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
				this.UpdateLoadouts(playerCharacterData.CharacterComponent.CharacterLoadoutsRanked, playerCharacterData.CharacterComponent.NumCharacterLoadouts);
			}
			else
			{
				this.UpdateLoadouts(playerCharacterData.CharacterComponent.CharacterLoadouts, playerCharacterData.CharacterComponent.NumCharacterLoadouts);
			}
		}
	}

	public void UpdateLoadouts(List<CharacterLoadout> loadouts, int NumCharacterLoadouts)
	{
		this.Init();
		if (!this.m_loadoutList.Equals(loadouts))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.UpdateLoadouts(List<CharacterLoadout>, int)).MethodHandle;
			}
			this.m_loadouts = loadouts;
			for (int i = 0; i < this.m_loadouts.Count; i++)
			{
				if (i >= this.m_modListItems.Count)
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
					if (this.m_modItemPrfab != null)
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
						UICharacterAbilitiesPanelModItem uicharacterAbilitiesPanelModItem = UnityEngine.Object.Instantiate<UICharacterAbilitiesPanelModItem>(this.m_modItemPrfab);
						uicharacterAbilitiesPanelModItem.transform.SetParent(this.m_loadoutList.transform);
						uicharacterAbilitiesPanelModItem.transform.localEulerAngles = Vector3.zero;
						uicharacterAbilitiesPanelModItem.transform.localPosition = Vector3.zero;
						uicharacterAbilitiesPanelModItem.transform.localScale = Vector3.one;
						uicharacterAbilitiesPanelModItem.m_btn.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnListScroll));
						this.m_modListItems.Add(uicharacterAbilitiesPanelModItem);
					}
					else
					{
						Debug.LogError(base.name + " field Mod Item Prefab reference is null, please set it in scene");
					}
				}
				if (i < this.m_modListItems.Count)
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
					this.m_modListItems[i].Setup(this.m_loadouts[i]);
					UIManager.SetGameObjectActive(this.m_modListItems[i], true, null);
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int j = this.m_modListItems.Count - 1; j >= this.m_loadouts.Count; j--)
			{
				UIManager.SetGameObjectActive(this.m_modListItems[j], false, null);
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.CheckCurrentModSelection();
		}
		this.m_buyNewLoadoutBtn.transform.SetAsLastSibling();
		UIManager.SetGameObjectActive(this.m_buyNewLoadoutBtn, NumCharacterLoadouts < GameBalanceVars.Get().MaxNumberOfFreelancerLoadoutSlots, null);
	}

	private void CheckCurrentModSelection()
	{
		if (this.m_selectedCharacter != CharacterType.None)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.CheckCurrentModSelection()).MethodHandle;
			}
			string toggleButtonLabels = StringUtil.TR("SelectLoadout", "Global");
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_selectedCharacter);
			int num = playerCharacterData.CharacterComponent.LastSelectedLoadout;
			CharacterModInfo characterModInfo = playerCharacterData.CharacterComponent.LastMods;
			if (this.UseRankedLoadouts())
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
				num = playerCharacterData.CharacterComponent.LastSelectedRankedLoadout;
				characterModInfo = playerCharacterData.CharacterComponent.LastRankedMods;
			}
			if (-1 < num)
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
				if (num < this.m_loadouts.Count)
				{
					CharacterLoadout characterLoadout = this.m_loadouts[num];
					toggleButtonLabels = StringUtil.TR_GetLoadoutName(characterLoadout.LoadoutName);
					if (characterLoadout.ModSet.Equals(characterModInfo))
					{
						if (this.m_saveButton != null)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_saveButton.SetDisabled(true);
						}
					}
					else if (this.m_saveButton != null)
					{
						this.m_saveButton.SetDisabled(false);
					}
					UIManager.SetGameObjectActive(this.m_renameButton, true, null);
					this.m_loadoutListToggleBtn.SetSelected(true, true, string.Empty, string.Empty);
					this.SetToggleButtonLabels(toggleButtonLabels);
					return;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			Log.Error("Last selected loadout index is out of range of loadouts", new object[0]);
			return;
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterAbiltiesPanelModLoadout.Update()).MethodHandle;
			}
			bool flag = true;
			if (EventSystem.current != null)
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
				if (EventSystem.current.IsPointerOverGameObject(-1))
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
					StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
					if (component != null)
					{
						for (;;)
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
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								RectTransform[] componentsInChildren = this.m_loadoutListToggleBtn.GetComponentsInChildren<RectTransform>(true);
								for (int i = 0; i < componentsInChildren.Length; i++)
								{
									if (componentsInChildren[i].gameObject.transform == rectTransform)
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
										flag = false;
										break;
									}
								}
								if (flag)
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
									componentsInChildren = this.m_dropdownMenuContainer.GetComponentsInChildren<RectTransform>(true);
									for (int j = 0; j < componentsInChildren.Length; j++)
									{
										if (componentsInChildren[j].gameObject.transform == rectTransform)
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
											flag = false;
											goto IL_17F;
										}
									}
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
								}
							}
						}
					}
				}
			}
			IL_17F:
			if (flag)
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
				this.SetListVisible(false);
			}
		}
	}
}
