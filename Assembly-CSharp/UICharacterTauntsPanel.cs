using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterTauntsPanel : MonoBehaviour
{
	public GridLayoutGroup m_tauntsGrid;

	public UIPurchasePanel m_purchasePanel;

	private static List<UICharacterTauntsPanel> m_activeTauntsPanels = new List<UICharacterTauntsPanel>();

	private CharacterResourceLink m_charLink;

	private UITauntButton m_lastButtonClicked;

	public void Start()
	{
		this.m_purchasePanel.m_isoButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PurchaseWithIso);
	}

	public CharacterResourceLink GetDisplayedCharacter()
	{
		return this.m_charLink;
	}

	public void Setup(CharacterType characterType)
	{
		this.Setup(GameWideData.Get().GetCharacterResourceLink(characterType), false);
	}

	public void Setup(CharacterResourceLink characterLink, bool sameCharacter = false)
	{
		this.m_lastButtonClicked = null;
		this.m_charLink = characterLink;
		UITauntButton[] componentsInChildren = this.m_tauntsGrid.GetComponentsInChildren<UITauntButton>(true);
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterLink.m_characterType);
		AbilityData component = characterLink.ActorDataPrefab.GetComponent<AbilityData>();
		List<int> list = new List<int>();
		for (int i = 0; i < characterLink.m_taunts.Count; i++)
		{
			list.Add(i);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterTauntsPanel.Setup(CharacterResourceLink, bool)).MethodHandle;
		}
		list.Sort((int first, int second) => characterLink.m_taunts[first].m_actionForTaunt - characterLink.m_taunts[second].m_actionForTaunt);
		int num = 0;
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (num < list.Count)
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
				int num2 = list[num];
				if (!characterLink.m_taunts[num2].m_isHidden)
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
					if (componentsInChildren.Length > j)
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
						if (playerCharacterData == null)
						{
							goto IL_18E;
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
						if (playerCharacterData.CharacterComponent == null || playerCharacterData.CharacterComponent.Taunts == null || playerCharacterData.CharacterComponent.Taunts.Count <= num2)
						{
							goto IL_18E;
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
						bool isUnlocked = playerCharacterData.CharacterComponent.Taunts[num2].Unlocked;
						IL_191:
						componentsInChildren[j].Setup(characterLink, num2, component, isUnlocked);
						UIManager.SetGameObjectActive(componentsInChildren[j], true, null);
						goto IL_1B0;
						IL_18E:
						isUnlocked = false;
						goto IL_191;
					}
					IL_1B0:;
				}
				else
				{
					j--;
				}
				num++;
			}
			else
			{
				UIManager.SetGameObjectActive(componentsInChildren[j], false, null);
			}
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

	public void SetVisible(bool isVisible)
	{
		UIManager.SetGameObjectActive(base.gameObject, isVisible, null);
	}

	private void OnEnable()
	{
		UICharacterTauntsPanel.m_activeTauntsPanels.Add(this);
	}

	private void OnDisable()
	{
		UICharacterTauntsPanel.m_activeTauntsPanels.Remove(this);
		if (this.m_purchasePanel != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterTauntsPanel.OnDisable()).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
		}
	}

	private void OnDestroy()
	{
		UICharacterTauntsPanel.m_activeTauntsPanels.Remove(this);
	}

	private void PurchaseWithIso(BaseEventData data)
	{
		this.Purchase(CurrencyType.ISO);
	}

	private void Purchase(CurrencyType currencyType)
	{
		if (this.m_lastButtonClicked == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterTauntsPanel.Purchase(CurrencyType)).MethodHandle;
			}
			return;
		}
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = PurchaseItemType.Taunt;
		uipurchaseableItem.m_charLink = this.m_lastButtonClicked.m_charLink;
		uipurchaseableItem.m_tauntIndex = this.m_lastButtonClicked.m_tauntIndex;
		uipurchaseableItem.m_currencyType = currencyType;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, null);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
	}

	public static void RefreshActivePanels(CharacterResourceLink characterLink, int tauntIndex)
	{
		for (int i = 0; i < UICharacterTauntsPanel.m_activeTauntsPanels.Count; i++)
		{
			if (UICharacterTauntsPanel.m_activeTauntsPanels[i].m_charLink.m_characterType == characterLink.m_characterType)
			{
				UITauntButton lastButtonClicked = UICharacterTauntsPanel.m_activeTauntsPanels[i].m_lastButtonClicked;
				if (lastButtonClicked != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterTauntsPanel.RefreshActivePanels(CharacterResourceLink, int)).MethodHandle;
					}
					if (lastButtonClicked.m_tauntIndex == tauntIndex)
					{
						lastButtonClicked.SetUnlocked();
						UIManager.SetGameObjectActive(UICharacterTauntsPanel.m_activeTauntsPanels[i].m_purchasePanel, false, null);
						goto IL_C9;
					}
				}
				UITauntButton[] componentsInChildren = UICharacterTauntsPanel.m_activeTauntsPanels[i].m_tauntsGrid.GetComponentsInChildren<UITauntButton>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					if (componentsInChildren[j].m_tauntIndex == tauntIndex)
					{
						componentsInChildren[j].SetUnlocked();
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
			IL_C9:;
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
	}

	public void Select(UITauntButton newButton)
	{
		if (this.m_lastButtonClicked != null)
		{
			this.m_lastButtonClicked.m_hitbox.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		newButton.m_hitbox.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
		this.m_lastButtonClicked = newButton;
		if (GameManager.Get().GameStatus != GameStatus.LoadoutSelecting)
		{
			if (newButton.GetIsoCost() > 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterTauntsPanel.Select(UITauntButton)).MethodHandle;
				}
				if (!newButton.IsUnlocked())
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
					this.m_purchasePanel.Setup(newButton.GetIsoCost(), 0, 0f, false);
					UIManager.SetGameObjectActive(this.m_purchasePanel, true, null);
					GameBalanceVars.TauntUnlockData tauntUnlockData = GameBalanceVars.Get().GetCharacterUnlockData(newButton.m_charLink.m_characterType).tauntUnlockData[newButton.m_tauntIndex];
					bool flag = GameBalanceVarsExtensions.MeetsPurchaseabilityConditions(tauntUnlockData);
					this.m_purchasePanel.SetDisabled(!flag);
					if (!flag)
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
						if (!tauntUnlockData.PurchaseDescription.IsNullOrEmpty())
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
							this.m_purchasePanel.SetupTooltip(tauntUnlockData.GetPurchaseDescription());
							goto IL_150;
						}
					}
					this.m_purchasePanel.SetupTooltip(string.Empty);
					IL_150:
					return;
				}
			}
			UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
		}
	}
}
