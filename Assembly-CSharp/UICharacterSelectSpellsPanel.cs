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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectSpellsPanel.Init()).MethodHandle;
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectSpellsPanel.HighlightSelectedCards()).MethodHandle;
				}
				this.m_prepButtons[i].SetSelectState(true);
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
		for (int j = 0; j < this.m_dashButtons.Length; j++)
		{
			if (this.m_dashButtons[j].IsSelected())
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
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectSpellsPanel.Setup(CharacterType, CharacterCardInfo, bool, bool)).MethodHandle;
			}
			if (this.m_lastCharType == type)
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
				return;
			}
		}
		if (this.m_phaseToSelectedCard.Count >= 3)
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
			if (!this.m_displayedCardInfo.HasEmptySelection() && this.m_displayedCardInfo.Equals(cards))
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			flag = true;
			cards.CombatCard = this.GetDefaultCard(AbilityRunPhase.Combat);
		}
		this.SetupCardButtons(AbilityRunPhase.Prep, cards.PrepCard);
		this.SetupCardButtons(AbilityRunPhase.Dash, cards.DashCard);
		this.SetupCardButtons(AbilityRunPhase.Combat, cards.CombatCard);
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
			this.SaveCardSelection();
		}
	}

	private bool ShouldAssignDefault(CardType cardType, AbilityRunPhase cardPhase)
	{
		bool result;
		if (cardType > CardType.NoOverride)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectSpellsPanel.ShouldAssignDefault(CardType, AbilityRunPhase)).MethodHandle;
			}
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectSpellsPanel.IsCardAllowed(CardType, AbilityRunPhase)).MethodHandle;
			}
			if (!CardManagerData.Get().IsCardTypePossibleInGame(cardType, cardPhase))
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
				flag = false;
			}
		}
		if (flag)
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
			if (GameManager.Get() != null && GameManager.Get().GameplayOverrides != null && GameManager.Get().GameplayOverrides.EnableCards)
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
				bool flag2;
				if (GameManager.Get().GameplayOverrides.IsCardAllowed(cardType))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectSpellsPanel.GetDefaultCard(AbilityRunPhase)).MethodHandle;
			}
			if (this.IsCardAllowed(defaultCardType, phase))
			{
				return defaultCardType;
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
		}
		List<Card> usableCardsByPhase = CardManagerData.Get().GetUsableCardsByPhase(phase, true);
		using (List<Card>.Enumerator enumerator = usableCardsByPhase.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Card card = enumerator.Current;
				if (this.IsCardAllowed(card.m_cardType, phase))
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
					return card.m_cardType;
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return defaultCardType;
	}

	public void SpellClicked(UISpellsSelectButton spellButton, bool clickedFromUI = true)
	{
		AbilityRunPhase phase = spellButton.GetPhase();
		if (this.m_phaseToButtons.ContainsKey(phase))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectSpellsPanel.SpellClicked(UISpellsSelectButton, bool)).MethodHandle;
			}
			foreach (UISpellsSelectButton uispellsSelectButton in this.m_phaseToButtons[phase])
			{
				if (uispellsSelectButton == spellButton)
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
					if (!uispellsSelectButton.IsSelected())
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
						if (clickedFromUI)
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
							GameEventManager.Get().FireEvent(GameEventManager.EventType.FrontEndEquipCatalyst, null);
						}
						this.m_phaseToSelectedCard[phase] = spellButton.GetCard();
					}
				}
				uispellsSelectButton.SetSelected(uispellsSelectButton == spellButton);
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
		}
		UICharacterScreen.Get().UpdateCatalystIcons(this.m_phaseToSelectedCard);
		if (clickedFromUI)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectSpellsPanel.Update()).MethodHandle;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectSpellsPanel.SetupCardButtons(AbilityRunPhase, CardType)).MethodHandle;
				}
				if (i >= array.Length)
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
				}
				else
				{
					bool flag2 = true;
					if (GameManager.Get() != null)
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
						if (GameManager.Get().GameplayOverrides != null)
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
							flag2 = GameManager.Get().GameplayOverrides.IsCardAllowed(card.m_cardType);
						}
					}
					if (flag2)
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
						array[i].Setup(card);
						if (!this.m_phaseToSelectedCard.ContainsKey(phase))
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
							if (card.m_cardType == selectedCardType)
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (-1 < phase - AbilityRunPhase.Prep && phase - AbilityRunPhase.Prep < this.m_bottomsButtonIcons.Length)
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
				UIManager.SetGameObjectActive(this.m_bottomsButtonIcons[phase - AbilityRunPhase.Prep], false, null);
			}
		}
		while (i < array.Length)
		{
			UIManager.SetGameObjectActive(array[i], false, null);
			i++;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterSelectSpellsPanel.SaveCardSelection()).MethodHandle;
			}
			cards.CombatCard = card2.m_cardType;
		}
		else
		{
			cards.CombatCard = CardType.None;
		}
		Card card3;
		if (this.m_phaseToSelectedCard.TryGetValue(AbilityRunPhase.Dash, out card3))
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
			cards.DashCard = card3.m_cardType;
		}
		else
		{
			cards.DashCard = CardType.None;
		}
		AppState_CharacterSelect.Get().UpdateSelectedCards(cards);
	}
}
