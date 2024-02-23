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
		if (Application.isPlaying)
		{
			m_phaseToButtons[AbilityRunPhase.Prep] = m_prepButtons;
			m_phaseToButtons[AbilityRunPhase.Dash] = m_dashButtons;
			m_phaseToButtons[AbilityRunPhase.Combat] = m_combatButtons;
		}
	}

	public void HighlightSelectedCards()
	{
		foreach (UISpellsSelectButton prepButton in m_prepButtons)
		{
			if (prepButton.IsSelected())
			{
				prepButton.SetSelectState(true);
			}
		}
		foreach (UISpellsSelectButton dashButton in m_dashButtons)
		{
			if (dashButton.IsSelected())
			{
				dashButton.SetSelectState(true);
			}
		}
		foreach (UISpellsSelectButton combatButton in m_combatButtons)
		{
			if (combatButton.IsSelected())
			{
				combatButton.SetSelectState(true);
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
		if (ClientGameManager.Get().WaitingForCardSelectResponse != -1
		    && m_lastCharType == type)
		{
			return;
		}
		if (m_phaseToSelectedCard.Count >= 3
		    && !m_displayedCardInfo.HasEmptySelection()
		    && m_displayedCardInfo.Equals(cards))
		{
			return;
		}
		if (m_lastCharType != type)
		{
			ClientGameManager.Get().ClearWaitingForCardResponse();
		}
		m_lastCharType = type;
		m_displayedCardInfo = cards;
		m_phaseToSelectedCard.Clear();
		
		bool hasChanges = false;
		if (ShouldAssignDefault(cards.PrepCard, AbilityRunPhase.Prep))
		{
			hasChanges = true;
			cards.PrepCard = GetDefaultCard(AbilityRunPhase.Prep);
		}
		if (ShouldAssignDefault(cards.DashCard, AbilityRunPhase.Dash))
		{
			hasChanges = true;
			cards.DashCard = GetDefaultCard(AbilityRunPhase.Dash);
		}
		if (ShouldAssignDefault(cards.CombatCard, AbilityRunPhase.Combat))
		{
			hasChanges = true;
			cards.CombatCard = GetDefaultCard(AbilityRunPhase.Combat);
		}
		SetupCardButtons(AbilityRunPhase.Prep, cards.PrepCard);
		SetupCardButtons(AbilityRunPhase.Dash, cards.DashCard);
		SetupCardButtons(AbilityRunPhase.Combat, cards.CombatCard);
		if (hasChanges)
		{
			SaveCardSelection();
		}
	}

	private bool ShouldAssignDefault(CardType cardType, AbilityRunPhase cardPhase)
	{
		return cardType <= CardType.NoOverride || !IsCardAllowed(cardType, cardPhase);
	}

	private bool IsCardAllowed(CardType cardType, AbilityRunPhase cardPhase)
	{
		if (CardManagerData.Get() != null
		    && !CardManagerData.Get().IsCardTypePossibleInGame(cardType, cardPhase))
		{
			return false;
		}

		if (GameManager.Get() == null
		    || GameManager.Get().GameplayOverrides == null
		    || !GameManager.Get().GameplayOverrides.EnableCards)
		{
			return true;
		}
		
		return GameManager.Get().GameplayOverrides.IsCardAllowed(cardType)
		       && CardManagerData.Get().GetCardPrefab(cardType) != null;
	}

	private CardType GetDefaultCard(AbilityRunPhase phase)
	{
		CardType defaultCardType = CardManagerData.Get().GetDefaultCardType(phase);
		if (defaultCardType != CardType.None && IsCardAllowed(defaultCardType, phase))
		{
			return defaultCardType;
		}

		foreach (Card card in CardManagerData.Get().GetUsableCardsByPhase(phase))
		{
			if (IsCardAllowed(card.m_cardType, phase))
			{
				return card.m_cardType;
			}
		}
		return defaultCardType;
	}

	public void SpellClicked(UISpellsSelectButton spellButton, bool clickedFromUI = true)
	{
		AbilityRunPhase phase = spellButton.GetPhase();
		UISpellsSelectButton[] buttons;
		if (m_phaseToButtons.TryGetValue(phase, out buttons))
		{
			foreach (UISpellsSelectButton uISpellsSelectButton in buttons)
			{
				if (uISpellsSelectButton == spellButton && !uISpellsSelectButton.IsSelected())
				{
					if (clickedFromUI)
					{
						GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndEquipCatalyst, null);
					}
					m_phaseToSelectedCard[phase] = spellButton.GetCard();
				}
				uISpellsSelectButton.SetSelected(uISpellsSelectButton == spellButton);
			}
		}
		UICharacterScreen.Get().UpdateCatalystIcons(m_phaseToSelectedCard);
		if (clickedFromUI)
		{
			SaveCardSelection();
		}
	}

	private void Update()
	{
		if (!m_setZtoZero)
		{
			return;
		}

		foreach (var childTransform in GetComponentsInChildren<RectTransform>())
		{
			if (childTransform != transform)
			{
				Vector3 pos = childTransform.localPosition;
				childTransform.localPosition = new Vector3(pos.x, pos.y, 0f);
			}
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
			if (item == null || i >= array.Length)
			{
				continue;
			}
			bool isCardAllowed = true;
			if (GameManager.Get() != null && GameManager.Get().GameplayOverrides != null)
			{
				isCardAllowed = GameManager.Get().GameplayOverrides.IsCardAllowed(item.m_cardType);
			}

			if (!isCardAllowed)
			{
				continue;
			}
			array[i].Setup(item);
			if (!m_phaseToSelectedCard.ContainsKey(phase) && item.m_cardType == selectedCardType)
			{
				flag = true;
				m_phaseToSelectedCard[phase] = item;
				SpellClicked(array[i], false);
			}

			i++;
		}
		if (!flag && -1 < (int)phase - 1 && (int)(phase - 1) < m_bottomsButtonIcons.Length)
		{
			UIManager.SetGameObjectActive(m_bottomsButtonIcons[(int)(phase - 1)], false);
		}
		for (; i < array.Length; i++)
		{
			UIManager.SetGameObjectActive(array[i], false);
		}
	}

	private void SaveCardSelection()
	{
		if (m_phaseToSelectedCard.Count == 0)
		{
			return;
		}
		CharacterCardInfo cards = default(CharacterCardInfo);
		Card selectedPrepCard;
		cards.PrepCard = m_phaseToSelectedCard.TryGetValue(AbilityRunPhase.Prep, out selectedPrepCard)
			? selectedPrepCard.m_cardType
			: CardType.None;
		Card selectedCombatCard;
		cards.CombatCard = m_phaseToSelectedCard.TryGetValue(AbilityRunPhase.Combat, out selectedCombatCard)
			? selectedCombatCard.m_cardType
			: CardType.None;
		Card selectedDashCard;
		cards.DashCard = m_phaseToSelectedCard.TryGetValue(AbilityRunPhase.Dash, out selectedDashCard)
			? selectedDashCard.m_cardType
			: CardType.None;
		AppState_CharacterSelect.Get().UpdateSelectedCards(cards);
	}
}
