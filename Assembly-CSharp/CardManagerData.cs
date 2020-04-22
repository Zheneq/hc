using System.Collections.Generic;
using UnityEngine;

public class CardManagerData : MonoBehaviour
{
	public enum CardConsumeMode
	{
		OneUse,
		ByCooldown
	}

	private const int c_defaultDeckSize = 15;

	[Header("-- List cards used in game here --")]
	public GameObject[] m_cardIndex = new GameObject[15];

	[Header("-- cooldown on card when added to hand --")]
	public int m_cooldownOnAddToHand;

	[Space(10f)]
	public CardConsumeMode m_cardConsumeMode;

	[Header("-- Default Cards --")]
	public CardType m_defaultPrepCardType = CardType.RadiationInjection;

	public CardType m_defaultDashCardType = CardType.Flash;

	public CardType m_defaultCombatCardType = CardType.SecondWind;

	private static CardManagerData s_instance;

	private Dictionary<AbilityRunPhase, HashSet<CardType>> m_phaseToCards = new Dictionary<AbilityRunPhase, HashSet<CardType>>();

	internal static CardManagerData Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		GameObject cardPrefab = GetCardPrefab(m_defaultPrepCardType);
		if (!(cardPrefab == null))
		{
			if (cardPrefab.GetComponent<Card>().GetAbilityRunPhase() == AbilityRunPhase.Prep)
			{
				goto IL_0057;
			}
		}
		Debug.LogError("Did not find default Prep card or is in the wrong phase. Please check CardManagerData prefab");
		goto IL_0057;
		IL_00d3:
		m_phaseToCards.Add(AbilityRunPhase.Prep, new HashSet<CardType>());
		m_phaseToCards.Add(AbilityRunPhase.Dash, new HashSet<CardType>());
		m_phaseToCards.Add(AbilityRunPhase.Combat, new HashSet<CardType>());
		for (int i = 0; i < m_cardIndex.Length; i++)
		{
			GameObject gameObject = m_cardIndex[i];
			if (!(gameObject != null))
			{
				continue;
			}
			Card component = gameObject.GetComponent<Card>();
			if (!(component != null))
			{
				continue;
			}
			if (component.m_isHidden)
			{
				continue;
			}
			AbilityRunPhase abilityRunPhase = component.GetAbilityRunPhase();
			if (m_phaseToCards.ContainsKey(abilityRunPhase))
			{
				m_phaseToCards[abilityRunPhase].Add(component.m_cardType);
			}
		}
		return;
		IL_0057:
		GameObject cardPrefab2 = GetCardPrefab(m_defaultDashCardType);
		if (!(cardPrefab2 == null))
		{
			if (cardPrefab2.GetComponent<Card>().GetAbilityRunPhase() == AbilityRunPhase.Dash)
			{
				goto IL_0097;
			}
		}
		Debug.LogError("Did not find default Dash card or is in the wrong phase. Please check CardManagerData prefab");
		goto IL_0097;
		IL_0097:
		GameObject cardPrefab3 = GetCardPrefab(m_defaultCombatCardType);
		if (!(cardPrefab3 == null))
		{
			if (cardPrefab3.GetComponent<Card>().GetAbilityRunPhase() == AbilityRunPhase.Combat)
			{
				goto IL_00d3;
			}
		}
		Debug.LogError("Did not find default Combat card or is in the wrong phase. Please check CardManagerData prefab");
		goto IL_00d3;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	internal GameObject[] GetCardIndex()
	{
		return m_cardIndex;
	}

	internal List<Card> GetUsableCardsByPhase(AbilityRunPhase cardAbilityPhase, bool ignoreHidden = true)
	{
		List<Card> list = new List<Card>();
		CardType defaultCardType = GetDefaultCardType(cardAbilityPhase);
		list.Add(GetCardByType(defaultCardType));
		GameObject[] cardIndex = Get().GetCardIndex();
		GameObject[] array = cardIndex;
		foreach (GameObject gameObject in array)
		{
			if (!(gameObject != null))
			{
				continue;
			}
			Card component = gameObject.GetComponent<Card>();
			if (!(component != null) || component.GetAbilityRunPhase() != cardAbilityPhase)
			{
				continue;
			}
			if (ignoreHidden)
			{
				if (component.m_isHidden)
				{
					continue;
				}
			}
			if (component.m_cardType != defaultCardType)
			{
				list.Add(component);
			}
		}
		while (true)
		{
			return list;
		}
	}

	public Card GetCardByType(CardType cardType)
	{
		return GetCardByTypeInt((int)cardType);
	}

	public Card GetCardByTypeInt(int cardTypeInt)
	{
		GameObject[] cardIndex = m_cardIndex;
		foreach (GameObject gameObject in cardIndex)
		{
			if (!(gameObject != null))
			{
				continue;
			}
			Card component = gameObject.GetComponent<Card>();
			if (!(component != null))
			{
				continue;
			}
			if (component.m_cardType != (CardType)cardTypeInt)
			{
				continue;
			}
			while (true)
			{
				return component;
			}
		}
		while (true)
		{
			return null;
		}
	}

	public GameObject GetCardPrefab(CardType cardType)
	{
		GameObject[] cardIndex = m_cardIndex;
		foreach (GameObject gameObject in cardIndex)
		{
			if (gameObject != null)
			{
				Card component = gameObject.GetComponent<Card>();
				if (component.m_cardType == cardType)
				{
					return gameObject;
				}
			}
		}
		while (true)
		{
			return null;
		}
	}

	public List<GameObject> GetCardPrefabs(CharacterCardInfo cardInfo)
	{
		List<GameObject> list = new List<GameObject>();
		if (cardInfo.PrepCard != CardType.None)
		{
			list.Add(GetCardPrefab(cardInfo.PrepCard));
		}
		if (cardInfo.CombatCard != CardType.None)
		{
			list.Add(GetCardPrefab(cardInfo.CombatCard));
		}
		if (cardInfo.DashCard != CardType.None)
		{
			list.Add(GetCardPrefab(cardInfo.DashCard));
		}
		return list;
	}

	public CardType GetDefaultPrepCardType()
	{
		return m_defaultPrepCardType;
	}

	public CardType GetDefaultDashCardType()
	{
		return m_defaultDashCardType;
	}

	public CardType GetDefaultCombatCardType()
	{
		return m_defaultCombatCardType;
	}

	public CardType GetDefaultCardType(AbilityRunPhase phase)
	{
		if (phase != AbilityRunPhase.Prep)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (phase != AbilityRunPhase.Dash)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								if (phase != AbilityRunPhase.Combat)
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											break;
										default:
											return CardType.None;
										}
									}
								}
								return GetDefaultCombatCardType();
							}
						}
					}
					return GetDefaultDashCardType();
				}
			}
		}
		return GetDefaultPrepCardType();
	}

	public bool IsCardTypePossibleInGame(CardType cardType, AbilityRunPhase cardPhase)
	{
		bool result = false;
		if (m_phaseToCards.ContainsKey(cardPhase))
		{
			result = m_phaseToCards[cardPhase].Contains(cardType);
		}
		return result;
	}
}
