using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSelectSpellsPanel : MonoBehaviour
{
	public bool m_setZtoZero;

	public UISpellsSelectButton[] m_prepButtons;

	public UISpellsSelectButton[] m_dashButtons;

	public UISpellsSelectButton[] m_combatButtons;

	public Image[] m_bottomsButtonIcons;

	public Image[] m_bottomsButtonHoverIcons;

	private CharacterCardInfo m_displayedCardInfo;

	private Dictionary<AbilityRunPhase, UISpellsSelectButton[]> m_phaseToButtons = new Dictionary<AbilityRunPhase, UISpellsSelectButton[]>();

	private Dictionary<AbilityRunPhase, Card> m_phaseToSelectedCard = new Dictionary<AbilityRunPhase, Card>();

	private bool m_initialized;

	private CharacterType m_lastCharType;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		if (m_initialized)
		{
			return;
		}
		m_initialized = true;
		if (!Application.isPlaying)
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
			m_phaseToButtons[AbilityRunPhase.Prep] = m_prepButtons;
			m_phaseToButtons[AbilityRunPhase.Dash] = m_dashButtons;
			m_phaseToButtons[AbilityRunPhase.Combat] = m_combatButtons;
			return;
		}
	}

	public void HighlightSelectedCards()
	{
		for (int i = 0; i < m_prepButtons.Length; i++)
		{
			if (m_prepButtons[i].IsSelected())
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
				m_prepButtons[i].SetSelectState(true);
			}
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			for (int j = 0; j < m_dashButtons.Length; j++)
			{
				if (m_dashButtons[j].IsSelected())
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
					m_dashButtons[j].SetSelectState(true);
				}
			}
			for (int k = 0; k < m_combatButtons.Length; k++)
			{
				if (m_combatButtons[k].IsSelected())
				{
					m_combatButtons[k].SetSelectState(true);
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public CharacterCardInfo GetDisplayedCardInfo()
	{
		return m_displayedCardInfo;
	}

	public void Setup(CharacterType type, CharacterCardInfo cards, bool loadedForSelf = false, bool sameCharacter = false)
	{
		Init();
		if (ClientGameManager.Get().WaitingForCardSelectResponse != -1)
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
			if (m_lastCharType == type)
			{
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		if (m_phaseToSelectedCard.Count >= 3)
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
			if (!m_displayedCardInfo.HasEmptySelection() && m_displayedCardInfo.Equals(cards))
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
		}
		if (m_lastCharType != type)
		{
			ClientGameManager.Get().ClearWaitingForCardResponse();
		}
		m_lastCharType = type;
		m_displayedCardInfo = cards;
		m_phaseToSelectedCard.Clear();
		bool flag = false;
		if (ShouldAssignDefault(cards.PrepCard, AbilityRunPhase.Prep))
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
			flag = true;
			cards.PrepCard = GetDefaultCard(AbilityRunPhase.Prep);
		}
		if (ShouldAssignDefault(cards.DashCard, AbilityRunPhase.Dash))
		{
			flag = true;
			cards.DashCard = GetDefaultCard(AbilityRunPhase.Dash);
		}
		if (ShouldAssignDefault(cards.CombatCard, AbilityRunPhase.Combat))
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
			flag = true;
			cards.CombatCard = GetDefaultCard(AbilityRunPhase.Combat);
		}
		SetupCardButtons(AbilityRunPhase.Prep, cards.PrepCard);
		SetupCardButtons(AbilityRunPhase.Dash, cards.DashCard);
		SetupCardButtons(AbilityRunPhase.Combat, cards.CombatCard);
		if (!flag)
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
			SaveCardSelection();
			return;
		}
	}

	private bool ShouldAssignDefault(CardType cardType, AbilityRunPhase cardPhase)
	{
		int result;
		if (cardType > CardType.NoOverride)
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
			result = ((!IsCardAllowed(cardType, cardPhase)) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private bool IsCardAllowed(CardType cardType, AbilityRunPhase cardPhase)
	{
		bool flag = true;
		if (CardManagerData.Get() != null)
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
			if (!CardManagerData.Get().IsCardTypePossibleInGame(cardType, cardPhase))
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
				flag = false;
			}
		}
		if (flag)
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
			if (GameManager.Get() != null && GameManager.Get().GameplayOverrides != null && GameManager.Get().GameplayOverrides.EnableCards)
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
				int num;
				if (GameManager.Get().GameplayOverrides.IsCardAllowed(cardType))
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
					num = ((CardManagerData.Get().GetCardPrefab(cardType) != null) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				flag = ((byte)num != 0);
			}
		}
		return flag;
	}

	private CardType GetDefaultCard(AbilityRunPhase phase)
	{
		CardType defaultCardType = CardManagerData.Get().GetDefaultCardType(phase);
		if (defaultCardType != CardType.None)
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
			if (IsCardAllowed(defaultCardType, phase))
			{
				return defaultCardType;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		List<Card> usableCardsByPhase = CardManagerData.Get().GetUsableCardsByPhase(phase);
		using (List<Card>.Enumerator enumerator = usableCardsByPhase.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Card current = enumerator.Current;
				if (IsCardAllowed(current.m_cardType, phase))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return current.m_cardType;
						}
					}
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return defaultCardType;
				}
			}
		}
	}

	public void SpellClicked(UISpellsSelectButton spellButton, bool clickedFromUI = true)
	{
		AbilityRunPhase phase = spellButton.GetPhase();
		if (m_phaseToButtons.ContainsKey(phase))
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
			UISpellsSelectButton[] array = m_phaseToButtons[phase];
			foreach (UISpellsSelectButton uISpellsSelectButton in array)
			{
				if (uISpellsSelectButton == spellButton)
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
					if (!uISpellsSelectButton.IsSelected())
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
						if (clickedFromUI)
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
							GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndEquipCatalyst, null);
						}
						m_phaseToSelectedCard[phase] = spellButton.GetCard();
					}
				}
				uISpellsSelectButton.SetSelected(uISpellsSelectButton == spellButton);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		UICharacterScreen.Get().UpdateCatalystIcons(m_phaseToSelectedCard);
		if (!clickedFromUI)
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
			SaveCardSelection();
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
				RectTransform obj = componentsInChildren[i];
				Vector3 localPosition = componentsInChildren[i].localPosition;
				float x = localPosition.x;
				Vector3 localPosition2 = componentsInChildren[i].localPosition;
				obj.localPosition = new Vector3(x, localPosition2.y, 0f);
			}
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
			return;
		}
	}

	private void SetupCardButtons(AbilityRunPhase phase, CardType selectedCardType)
	{
		if (!m_phaseToButtons.ContainsKey(phase))
		{
			return;
		}
		UISpellsSelectButton[] array = m_phaseToButtons[phase];
		List<Card> usableCardsByPhase = CardManagerData.Get().GetUsableCardsByPhase(phase);
		int i = 0;
		bool flag = false;
		foreach (Card item in usableCardsByPhase)
		{
			if (!(item == null))
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
				if (i >= array.Length)
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
				}
				else
				{
					bool flag2 = true;
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
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							flag2 = GameManager.Get().GameplayOverrides.IsCardAllowed(item.m_cardType);
						}
					}
					if (flag2)
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
						array[i].Setup(item);
						if (!m_phaseToSelectedCard.ContainsKey(phase))
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
							if (item.m_cardType == selectedCardType)
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
								flag = true;
								m_phaseToSelectedCard[phase] = item;
								SpellClicked(array[i], false);
							}
						}
						i++;
					}
				}
			}
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
			if ((AbilityRunPhase)(-1) < phase - 1 && (int)(phase - 1) < m_bottomsButtonIcons.Length)
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
				UIManager.SetGameObjectActive(m_bottomsButtonIcons[(int)(phase - 1)], false);
			}
		}
		for (; i < array.Length; i++)
		{
			UIManager.SetGameObjectActive(array[i], false);
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void SaveCardSelection()
	{
		if (m_phaseToSelectedCard.Count == 0)
		{
			return;
		}
		CharacterCardInfo cards = default(CharacterCardInfo);
		if (m_phaseToSelectedCard.TryGetValue(AbilityRunPhase.Prep, out Card value))
		{
			cards.PrepCard = value.m_cardType;
		}
		else
		{
			cards.PrepCard = CardType.None;
		}
		if (m_phaseToSelectedCard.TryGetValue(AbilityRunPhase.Combat, out Card value2))
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
			cards.CombatCard = value2.m_cardType;
		}
		else
		{
			cards.CombatCard = CardType.None;
		}
		if (m_phaseToSelectedCard.TryGetValue(AbilityRunPhase.Dash, out Card value3))
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
			cards.DashCard = value3.m_cardType;
		}
		else
		{
			cards.DashCard = CardType.None;
		}
		AppState_CharacterSelect.Get().UpdateSelectedCards(cards);
	}
}
