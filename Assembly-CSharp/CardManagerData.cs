using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManagerData : MonoBehaviour
{
	private const int c_defaultDeckSize = 0xF;

	[Header("-- List cards used in game here --")]
	public GameObject[] m_cardIndex = new GameObject[0xF];

	[Header("-- cooldown on card when added to hand --")]
	public int m_cooldownOnAddToHand;

	[Space(10f)]
	public CardManagerData.CardConsumeMode m_cardConsumeMode;

	[Header("-- Default Cards --")]
	public CardType m_defaultPrepCardType = CardType.RadiationInjection;

	public CardType m_defaultDashCardType = CardType.Flash;

	public CardType m_defaultCombatCardType = CardType.SecondWind;

	private static CardManagerData s_instance;

	private Dictionary<AbilityRunPhase, HashSet<CardType>> m_phaseToCards = new Dictionary<AbilityRunPhase, HashSet<CardType>>();

	internal static CardManagerData Get()
	{
		return CardManagerData.s_instance;
	}

	private void Awake()
	{
		CardManagerData.s_instance = this;
		GameObject cardPrefab = this.GetCardPrefab(this.m_defaultPrepCardType);
		if (!(cardPrefab == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CardManagerData.Awake()).MethodHandle;
			}
			if (cardPrefab.GetComponent<Card>().GetAbilityRunPhase() == AbilityRunPhase.Prep)
			{
				goto IL_57;
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
		Debug.LogError("Did not find default Prep card or is in the wrong phase. Please check CardManagerData prefab");
		IL_57:
		GameObject cardPrefab2 = this.GetCardPrefab(this.m_defaultDashCardType);
		if (!(cardPrefab2 == null))
		{
			if (cardPrefab2.GetComponent<Card>().GetAbilityRunPhase() == AbilityRunPhase.Dash)
			{
				goto IL_97;
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
		Debug.LogError("Did not find default Dash card or is in the wrong phase. Please check CardManagerData prefab");
		IL_97:
		GameObject cardPrefab3 = this.GetCardPrefab(this.m_defaultCombatCardType);
		if (!(cardPrefab3 == null))
		{
			if (cardPrefab3.GetComponent<Card>().GetAbilityRunPhase() == AbilityRunPhase.Combat)
			{
				goto IL_D3;
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
		Debug.LogError("Did not find default Combat card or is in the wrong phase. Please check CardManagerData prefab");
		IL_D3:
		this.m_phaseToCards.Add(AbilityRunPhase.Prep, new HashSet<CardType>());
		this.m_phaseToCards.Add(AbilityRunPhase.Dash, new HashSet<CardType>());
		this.m_phaseToCards.Add(AbilityRunPhase.Combat, new HashSet<CardType>());
		for (int i = 0; i < this.m_cardIndex.Length; i++)
		{
			GameObject gameObject = this.m_cardIndex[i];
			if (gameObject != null)
			{
				Card component = gameObject.GetComponent<Card>();
				if (component != null)
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
					if (!component.m_isHidden)
					{
						AbilityRunPhase abilityRunPhase = component.GetAbilityRunPhase();
						if (this.m_phaseToCards.ContainsKey(abilityRunPhase))
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
							this.m_phaseToCards[abilityRunPhase].Add(component.m_cardType);
						}
					}
				}
			}
		}
	}

	private void OnDestroy()
	{
		CardManagerData.s_instance = null;
	}

	internal GameObject[] GetCardIndex()
	{
		return this.m_cardIndex;
	}

	internal List<Card> GetUsableCardsByPhase(AbilityRunPhase cardAbilityPhase, bool ignoreHidden = true)
	{
		List<Card> list = new List<Card>();
		CardType defaultCardType = this.GetDefaultCardType(cardAbilityPhase);
		list.Add(this.GetCardByType(defaultCardType));
		GameObject[] cardIndex = CardManagerData.Get().GetCardIndex();
		foreach (GameObject gameObject in cardIndex)
		{
			if (gameObject != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CardManagerData.GetUsableCardsByPhase(AbilityRunPhase, bool)).MethodHandle;
				}
				Card component = gameObject.GetComponent<Card>();
				if (component != null && component.GetAbilityRunPhase() == cardAbilityPhase)
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
					if (ignoreHidden)
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
						if (component.m_isHidden)
						{
							goto IL_BA;
						}
					}
					if (component.m_cardType != defaultCardType)
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
						list.Add(component);
					}
				}
			}
			IL_BA:;
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
		return list;
	}

	public Card GetCardByType(CardType cardType)
	{
		return this.GetCardByTypeInt((int)cardType);
	}

	public Card GetCardByTypeInt(int cardTypeInt)
	{
		foreach (GameObject gameObject in this.m_cardIndex)
		{
			if (gameObject != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CardManagerData.GetCardByTypeInt(int)).MethodHandle;
				}
				Card component = gameObject.GetComponent<Card>();
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
					if (component.m_cardType == (CardType)cardTypeInt)
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
						return component;
					}
				}
			}
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
		return null;
	}

	public GameObject GetCardPrefab(CardType cardType)
	{
		foreach (GameObject gameObject in this.m_cardIndex)
		{
			if (gameObject != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CardManagerData.GetCardPrefab(CardType)).MethodHandle;
				}
				Card component = gameObject.GetComponent<Card>();
				if (component.m_cardType == cardType)
				{
					return gameObject;
				}
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
		return null;
	}

	public List<GameObject> GetCardPrefabs(CharacterCardInfo cardInfo)
	{
		List<GameObject> list = new List<GameObject>();
		if (cardInfo.PrepCard != CardType.None)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CardManagerData.GetCardPrefabs(CharacterCardInfo)).MethodHandle;
			}
			list.Add(this.GetCardPrefab(cardInfo.PrepCard));
		}
		if (cardInfo.CombatCard != CardType.None)
		{
			list.Add(this.GetCardPrefab(cardInfo.CombatCard));
		}
		if (cardInfo.DashCard != CardType.None)
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
			list.Add(this.GetCardPrefab(cardInfo.DashCard));
		}
		return list;
	}

	public CardType GetDefaultPrepCardType()
	{
		return this.m_defaultPrepCardType;
	}

	public CardType GetDefaultDashCardType()
	{
		return this.m_defaultDashCardType;
	}

	public CardType GetDefaultCombatCardType()
	{
		return this.m_defaultCombatCardType;
	}

	public CardType GetDefaultCardType(AbilityRunPhase phase)
	{
		if (phase == AbilityRunPhase.Prep)
		{
			return this.GetDefaultPrepCardType();
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(CardManagerData.GetDefaultCardType(AbilityRunPhase)).MethodHandle;
		}
		if (phase == AbilityRunPhase.Dash)
		{
			return this.GetDefaultDashCardType();
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
		if (phase != AbilityRunPhase.Combat)
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
			return CardType.None;
		}
		return this.GetDefaultCombatCardType();
	}

	public bool IsCardTypePossibleInGame(CardType cardType, AbilityRunPhase cardPhase)
	{
		bool result = false;
		if (this.m_phaseToCards.ContainsKey(cardPhase))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CardManagerData.IsCardTypePossibleInGame(CardType, AbilityRunPhase)).MethodHandle;
			}
			result = this.m_phaseToCards[cardPhase].Contains(cardType);
		}
		return result;
	}

	public enum CardConsumeMode
	{
		OneUse,
		ByCooldown
	}
}
