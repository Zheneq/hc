using UnityEngine;
using UnityEngine.Networking;

public class CardManager : NetworkBehaviour
{
	public GameObject m_dataPrefab;

	private CardManagerData m_dataPrefabInstanceComponent;
	private static CardManager s_instance;

	public GameObject[] m_cardIndex => m_dataPrefabInstanceComponent == null ? null : m_dataPrefabInstanceComponent.m_cardIndex;

	public bool ShowingInGameCardUI { get; set; }

	internal static CardManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		m_dataPrefabInstanceComponent = CardManagerData.Get();
		s_instance = this;
#if SERVER
		// added in rogues
		DontDestroyOnLoad(gameObject);
#endif
		ShowingInGameCardUI = true;
		Log.Info("Set CardManager reference");
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	// rogues
#if SERVER
	public static bool CardsDisabled()
	{
		// custom
		return false;
		// rogues
		// return true;
	}
#endif

	// empty in reactor
	public void SetDeckAndGiveCards(ActorData actor, CharacterCardInfo cardInfo, bool isDebugRequest = false)  // isDebugRequest removed in rogues
	{
#if SERVER
		// added in rogues
		if (!NetworkServer.active)
		{
			return;
		}
		LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
		if (!gameplayOverrides.EnableCards || CardsDisabled())
		{
			cardInfo.CombatCard = CardType.None;
			cardInfo.PrepCard = CardType.None;
			cardInfo.DashCard = CardType.None;
		}
		else
		{
			if (!gameplayOverrides.IsCardAllowed(cardInfo.CombatCard))
			{
				cardInfo.CombatCard = CardType.None;
			}
			if (!gameplayOverrides.IsCardAllowed(cardInfo.PrepCard))
			{
				cardInfo.PrepCard = CardType.None;
			}
			if (!gameplayOverrides.IsCardAllowed(cardInfo.DashCard))
			{
				cardInfo.DashCard = CardType.None;
			}
		}
		if (actor.GetAbilityData() != null && !CardsDisabled())
		{
			actor.GetAbilityData().SpawnAndSetupCards(cardInfo);
		}
#endif
	}

	// empty in reactor
	internal void RemoveUsedCards()
	{
#if SERVER
		// added in rogues
		if (NetworkServer.active)
		{
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				if (actorData != null && actorData.GetAbilityData() != null)
				{
					actorData.GetAbilityData().RemoveUsedCards();
				}
			}
		}
#endif
	}

	private void UNetVersion()
	{
	}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
