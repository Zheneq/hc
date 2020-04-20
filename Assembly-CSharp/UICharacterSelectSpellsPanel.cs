using System;
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
		this.Init();
	}

	private void Init()
	{
		if (!this.m_initialized)
		{
			this.m_initialized = true;
			if (Application.isPlaying)
			{
				this.m_phaseToButtons[AbilityRunPhase.Prep] = this.m_prepButtons;
				this.m_phaseToButtons[AbilityRunPhase.Dash] = this.m_dashButtons;
				this.m_phaseToButtons[AbilityRunPhase.Combat] = this.m_combatButtons;
			}
		}
	}

	public void HighlightSelectedCards()
	{
		for (int i = 0; i < this.m_prepButtons.Length; i++)
		{
			if (this.m_prepButtons[i].IsSelected())
			{
				this.m_prepButtons[i].SetSelectState(true);
			}
		}
		for (int j = 0; j < this.m_dashButtons.Length; j++)
		{
			if (this.m_dashButtons[j].IsSelected())
			{
				this.m_dashButtons[j].SetSelectState(true);
			}
		}
		for (int k = 0; k < this.m_combatButtons.Length; k++)
		{
			if (this.m_combatButtons[k].IsSelected())
			{
				this.m_combatButtons[k].SetSelectState(true);
			}
		}
	}

	public CharacterCardInfo GetDisplayedCardInfo()
	{
		return this.m_displayedCardInfo;
	}

	public void Setup(CharacterType type, CharacterCardInfo cards, bool loadedForSelf = false, bool sameCharacter = false)
	{
		this.Init();
		if (ClientGameManager.Get().WaitingForCardSelectResponse != -1)
		{
			if (this.m_lastCharType == type)
			{
				return;
			}
		}
		if (this.m_phaseToSelectedCard.Count >= 3)
		{
			if (!this.m_displayedCardInfo.HasEmptySelection() && this.m_displayedCardInfo.Equals(cards))
			{
				return;
			}
		}
		if (this.m_lastCharType != type)
		{
			ClientGameManager.Get().ClearWaitingForCardResponse();
		}
		this.m_lastCharType = type;
		this.m_displayedCardInfo = cards;
		this.m_phaseToSelectedCard.Clear();
		bool flag = false;
		if (this.ShouldAssignDefault(cards.PrepCard, AbilityRunPhase.Prep))
		{
			flag = true;
			cards.PrepCard = this.GetDefaultCard(AbilityRunPhase.Prep);
		}
		if (this.ShouldAssignDefault(cards.DashCard, AbilityRunPhase.Dash))
		{
			flag = true;
			cards.DashCard = this.GetDefaultCard(AbilityRunPhase.Dash);
		}
		if (this.ShouldAssignDefault(cards.CombatCard, AbilityRunPhase.Combat))
		{
			flag = true;
			cards.CombatCard = this.GetDefaultCard(AbilityRunPhase.Combat);
		}
		this.SetupCardButtons(AbilityRunPhase.Prep, cards.PrepCard);
		this.SetupCardButtons(AbilityRunPhase.Dash, cards.DashCard);
		this.SetupCardButtons(AbilityRunPhase.Combat, cards.CombatCard);
		if (flag)
		{
			this.SaveCardSelection();
		}
	}

	private bool ShouldAssignDefault(CardType cardType, AbilityRunPhase cardPhase)
	{
		bool result;
		if (cardType > CardType.NoOverride)
		{
			result = !this.IsCardAllowed(cardType, cardPhase);
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool IsCardAllowed(CardType cardType, AbilityRunPhase cardPhase)
	{
		bool flag = true;
		if (CardManagerData.Get() != null)
		{
			if (!CardManagerData.Get().IsCardTypePossibleInGame(cardType, cardPhase))
			{
				flag = false;
			}
		}
		if (flag)
		{
			if (GameManager.Get() != null && GameManager.Get().GameplayOverrides != null && GameManager.Get().GameplayOverrides.EnableCards)
			{
				bool flag2;
				if (GameManager.Get().GameplayOverrides.IsCardAllowed(cardType))
				{
					flag2 = (CardManagerData.Get().GetCardPrefab(cardType) != null);
				}
				else
				{
					flag2 = false;
				}
				flag = flag2;
			}
		}
		return flag;
	}

	private CardType GetDefaultCard(AbilityRunPhase phase)
	{
		CardType defaultCardType = CardManagerData.Get().GetDefaultCardType(phase);
		if (defaultCardType != CardType.None)
		{
			if (this.IsCardAllowed(defaultCardType, phase))
			{
				return defaultCardType;
			}
		}
		List<Card> usableCardsByPhase = CardManagerData.Get().GetUsableCardsByPhase(phase, true);
		using (List<Card>.Enumerator enumerator = usableCardsByPhase.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Card card = enumerator.Current;
				if (this.IsCardAllowed(card.m_cardType, phase))
				{
					return card.m_cardType;
				}
			}
		}
		return defaultCardType;
	}

	public void SpellClicked(UISpellsSelectButton spellButton, bool clickedFromUI = true)
	{
		AbilityRunPhase phase = spellButton.GetPhase();
		if (this.m_phaseToButtons.ContainsKey(phase))
		{
			foreach (UISpellsSelectButton uispellsSelectButton in this.m_phaseToButtons[phase])
			{
				if (uispellsSelectButton == spellButton)
				{
					if (!uispellsSelectButton.IsSelected())
					{
						if (clickedFromUI)
						{
							GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndEquipCatalyst, null);
						}
						this.m_phaseToSelectedCard[phase] = spellButton.GetCard();
					}
				}
				uispellsSelectButton.SetSelected(uispellsSelectButton == spellButton);
			}
		}
		UICharacterScreen.Get().UpdateCatalystIcons(this.m_phaseToSelectedCard);
		if (clickedFromUI)
		{
			this.SaveCardSelection();
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

	private void SetupCardButtons(AbilityRunPhase phase, CardType selectedCardType)
	{
		if (!this.m_phaseToButtons.ContainsKey(phase))
		{
			return;
		}
		UISpellsSelectButton[] array = this.m_phaseToButtons[phase];
		List<Card> usableCardsByPhase = CardManagerData.Get().GetUsableCardsByPhase(phase, true);
		int i = 0;
		bool flag = false;
		foreach (Card card in usableCardsByPhase)
		{
			if (!(card == null))
			{
				if (i >= array.Length)
				{
				}
				else
				{
					bool flag2 = true;
					if (GameManager.Get() != null)
					{
						if (GameManager.Get().GameplayOverrides != null)
						{
							flag2 = GameManager.Get().GameplayOverrides.IsCardAllowed(card.m_cardType);
						}
					}
					if (flag2)
					{
						array[i].Setup(card);
						if (!this.m_phaseToSelectedCard.ContainsKey(phase))
						{
							if (card.m_cardType == selectedCardType)
							{
								flag = true;
								this.m_phaseToSelectedCard[phase] = card;
								this.SpellClicked(array[i], false);
							}
						}
						i++;
					}
				}
			}
		}
		if (!flag)
		{
			if (-1 < phase - AbilityRunPhase.Prep && phase - AbilityRunPhase.Prep < this.m_bottomsButtonIcons.Length)
			{
				UIManager.SetGameObjectActive(this.m_bottomsButtonIcons[phase - AbilityRunPhase.Prep], false, null);
			}
		}
		while (i < array.Length)
		{
			UIManager.SetGameObjectActive(array[i], false, null);
			i++;
		}
	}

	private void SaveCardSelection()
	{
		if (this.m_phaseToSelectedCard.Count == 0)
		{
			return;
		}
		CharacterCardInfo cards = default(CharacterCardInfo);
		Card card;
		if (this.m_phaseToSelectedCard.TryGetValue(AbilityRunPhase.Prep, out card))
		{
			cards.PrepCard = card.m_cardType;
		}
		else
		{
			cards.PrepCard = CardType.None;
		}
		Card card2;
		if (this.m_phaseToSelectedCard.TryGetValue(AbilityRunPhase.Combat, out card2))
		{
			cards.CombatCard = card2.m_cardType;
		}
		else
		{
			cards.CombatCard = CardType.None;
		}
		Card card3;
		if (this.m_phaseToSelectedCard.TryGetValue(AbilityRunPhase.Dash, out card3))
		{
			cards.DashCard = card3.m_cardType;
		}
		else
		{
			cards.DashCard = CardType.None;
		}
		AppState_CharacterSelect.Get().UpdateSelectedCards(cards);
	}
}
