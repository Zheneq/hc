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
		m_purchasePanel.m_isoButton.spriteController.callback = PurchaseWithIso;
	}

	public CharacterResourceLink GetDisplayedCharacter()
	{
		return m_charLink;
	}

	public void Setup(CharacterType characterType)
	{
		Setup(GameWideData.Get().GetCharacterResourceLink(characterType));
	}

	public void Setup(CharacterResourceLink characterLink, bool sameCharacter = false)
	{
		m_lastButtonClicked = null;
		m_charLink = characterLink;
		UITauntButton[] componentsInChildren = m_tauntsGrid.GetComponentsInChildren<UITauntButton>(true);
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterLink.m_characterType);
		AbilityData component = characterLink.ActorDataPrefab.GetComponent<AbilityData>();
		List<int> list = new List<int>();
		for (int i = 0; i < characterLink.m_taunts.Count; i++)
		{
			list.Add(i);
		}
		while (true)
		{
			list.Sort((int first, int second) => characterLink.m_taunts[first].m_actionForTaunt - characterLink.m_taunts[second].m_actionForTaunt);
			int num = 0;
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				int num2;
				bool isUnlocked;
				if (num < list.Count)
				{
					num2 = list[num];
					if (!characterLink.m_taunts[num2].m_isHidden)
					{
						if (componentsInChildren.Length > j)
						{
							if (playerCharacterData != null)
							{
								if (playerCharacterData.CharacterComponent != null && playerCharacterData.CharacterComponent.Taunts != null && playerCharacterData.CharacterComponent.Taunts.Count > num2)
								{
									isUnlocked = playerCharacterData.CharacterComponent.Taunts[num2].Unlocked;
									goto IL_0191;
								}
							}
							isUnlocked = false;
							goto IL_0191;
						}
					}
					else
					{
						j--;
					}
					goto IL_01b8;
				}
				UIManager.SetGameObjectActive(componentsInChildren[j], false);
				continue;
				IL_0191:
				componentsInChildren[j].Setup(characterLink, num2, component, isUnlocked);
				UIManager.SetGameObjectActive(componentsInChildren[j], true);
				goto IL_01b8;
				IL_01b8:
				num++;
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
	}

	public void SetVisible(bool isVisible)
	{
		UIManager.SetGameObjectActive(base.gameObject, isVisible);
	}

	private void OnEnable()
	{
		m_activeTauntsPanels.Add(this);
	}

	private void OnDisable()
	{
		m_activeTauntsPanels.Remove(this);
		if (!(m_purchasePanel != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_purchasePanel, false);
			return;
		}
	}

	private void OnDestroy()
	{
		m_activeTauntsPanels.Remove(this);
	}

	private void PurchaseWithIso(BaseEventData data)
	{
		Purchase(CurrencyType.ISO);
	}

	private void Purchase(CurrencyType currencyType)
	{
		if (m_lastButtonClicked == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = PurchaseItemType.Taunt;
		uIPurchaseableItem.m_charLink = m_lastButtonClicked.m_charLink;
		uIPurchaseableItem.m_tauntIndex = m_lastButtonClicked.m_tauntIndex;
		uIPurchaseableItem.m_currencyType = currencyType;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
	}

	public static void RefreshActivePanels(CharacterResourceLink characterLink, int tauntIndex)
	{
		for (int i = 0; i < m_activeTauntsPanels.Count; i++)
		{
			if (m_activeTauntsPanels[i].m_charLink.m_characterType != characterLink.m_characterType)
			{
				continue;
			}
			UITauntButton lastButtonClicked = m_activeTauntsPanels[i].m_lastButtonClicked;
			if (lastButtonClicked != null)
			{
				if (lastButtonClicked.m_tauntIndex == tauntIndex)
				{
					lastButtonClicked.SetUnlocked();
					UIManager.SetGameObjectActive(m_activeTauntsPanels[i].m_purchasePanel, false);
					continue;
				}
			}
			UITauntButton[] componentsInChildren = m_activeTauntsPanels[i].m_tauntsGrid.GetComponentsInChildren<UITauntButton>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (componentsInChildren[j].m_tauntIndex == tauntIndex)
				{
					componentsInChildren[j].SetUnlocked();
				}
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void Select(UITauntButton newButton)
	{
		if (m_lastButtonClicked != null)
		{
			m_lastButtonClicked.m_hitbox.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
		}
		newButton.m_hitbox.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
		m_lastButtonClicked = newButton;
		if (GameManager.Get().GameStatus == GameStatus.LoadoutSelecting)
		{
			return;
		}
		if (newButton.GetIsoCost() > 0)
		{
			if (!newButton.IsUnlocked())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						m_purchasePanel.Setup(newButton.GetIsoCost(), 0, 0f);
						UIManager.SetGameObjectActive(m_purchasePanel, true);
						GameBalanceVars.TauntUnlockData tauntUnlockData = GameBalanceVars.Get().GetCharacterUnlockData(newButton.m_charLink.m_characterType).tauntUnlockData[newButton.m_tauntIndex];
						bool flag = GameBalanceVarsExtensions.MeetsPurchaseabilityConditions(tauntUnlockData);
						m_purchasePanel.SetDisabled(!flag);
						if (!flag)
						{
							if (!tauntUnlockData.PurchaseDescription.IsNullOrEmpty())
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										m_purchasePanel.SetupTooltip(tauntUnlockData.GetPurchaseDescription());
										return;
									}
								}
							}
						}
						m_purchasePanel.SetupTooltip(string.Empty);
						return;
					}
					}
				}
			}
		}
		UIManager.SetGameObjectActive(m_purchasePanel, false);
	}
}
