// ROGUES
// SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
	public float m_combatRange = 7f;
	public float m_idealRange = 5f;
	public float m_retreatFromRange = 15f;
	
#if SERVER // rogues?
	// added in rogues
	public int m_alertedTurn = -1;
#endif

	[HideInInspector]
	// reactor
	public Stack<NPCBrain> previousBrainStack;
	// rogues
	// public Stack<NPCBrain> m_brainStack = new Stack<NPCBrain>();
	
#if SERVER
	public Stack<NPCBrain> m_brainStack => previousBrainStack;  // custom for rogues
#endif

	private int m_aiStartedForTurn;
	
#if SERVER
	// added in rogues
	private ActorData m_actor;
#endif

	private void Start()
	{
		ActorData component = GetComponent<ActorData>();
#if SERVER
		// added in rogues
		m_actor = component;
#endif
		
		// removed in rogues
		BotDifficulty botDifficulty = BotDifficulty.Expert;
		bool botCanTaunt = false;
		foreach (LobbyPlayerInfo lobbyPlayerInfo in GameManager.Get().TeamInfo.TeamPlayerInfo)
		{
			if (component.PlayerData != null
			    && component.PlayerData.LookupDetails() != null
			    && lobbyPlayerInfo.PlayerId == component.PlayerData.LookupDetails().m_lobbyPlayerInfoId)
			{
				botDifficulty = lobbyPlayerInfo.Difficulty;
				botCanTaunt = lobbyPlayerInfo.BotCanTaunt;
				break;
			}
		}
		// end removed in rogues
		
		previousBrainStack = new Stack<NPCBrain>(); // moved to initializer in rogues
		if (GetComponent<NPCBrain>() == null)
		{
			if (component.GetClassName() != "Sniper"
			    && component.GetClassName() != "RageBeast"
			    && component.GetClassName() != "Scoundrel"
			    && component.GetClassName() != "RobotAnimal"
			    && component.GetClassName() != "NanoSmith"
			    && component.GetClassName() != "Thief"
			    && component.GetClassName() != "BattleMonk"
			    && component.GetClassName() != "BazookaGirl"
			    && component.GetClassName() != "SpaceMarine"
			    && component.GetClassName() != "Gremlins"
			    && component.GetClassName() != "Tracker"
			    && component.GetClassName() != "DigitalSorceress"
			    && component.GetClassName() != "Spark"
			    && component.GetClassName() != "Claymore"
			    && component.GetClassName() != "Rampart"
			    && component.GetClassName() != "Trickster"
			    && component.GetClassName() != "Blaster"
			    && component.GetClassName() != "FishMan"
			    && component.GetClassName() != "Thief"
			    && component.GetClassName() != "Soldier"
			    && component.GetClassName() != "Exo"
			    && component.GetClassName() != "Martyr"
			    && component.GetClassName() != "Sensei"
			    && component.GetClassName() != "TeleportingNinja"
			    && component.GetClassName() != "Manta"
			    && component.GetClassName() != "Valkyrie"
			    && component.GetClassName() != "Archer"
			    && component.GetClassName() != "Samurai"
			    && component.GetClassName() != "Cleric"
			    // removed in rogues
			    && component.GetClassName() != "Neko"
			    && component.GetClassName() != "Scamp"
			    && component.GetClassName() != "Dino"
			    && component.GetClassName() != "Iceborg"
			    && component.GetClassName() != "Fireborg")
				// end removed in rogues
			{
				Log.Info("Using Generic AI for {0}", component.GetClassName());
				return;
			}
			
			// reactor
			NPCBrain brain = NPCBrain_Adaptive.Create(this, component.transform, botDifficulty, botCanTaunt);
			// rogues
			// NPCBrain brain = NPCBrain_Adaptive.CreateDefault(this, component.transform);
#if SERVER
			// added in rogues
			PushBrain(brain); // TODO rogues?
#endif
			
			Log.Info("Making Adaptive AI for {0} at difficulty {1}, can taunt: {2}",
				component.GetClassName(), botDifficulty.ToString(), botCanTaunt); // no difficulty or taunts in rogues
			if (IAmTheOnlyBotOnATwoPlayerTeam(component))
			{
				component.GetComponent<NPCBrain_Adaptive>().SendDecisionToTeamChat(true);
			}
		}
	}

	// removed in rogues
	public BoardSquare GetClosestEnemyPlayerSquare(bool includeInvisibles, out int numEnemiesInRange)
	{
		numEnemiesInRange = 0;
		ActorData actorData = GetComponent<ActorData>();
		List<ActorData> enemies = GameFlowData.Get().GetAllTeamMembers(actorData.GetEnemyTeam());
		BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
		BoardSquare closestEnemySquare = null;
		foreach (ActorData enemyActor in enemies)
		{
			BoardSquare enemySquare = enemyActor.GetCurrentBoardSquare();
			if (!enemyActor.IsDead() && enemySquare != null)
			{
				float distToEnemy = currentBoardSquare.HorizontalDistanceOnBoardTo(enemySquare);
				if (distToEnemy <= m_combatRange)
				{
					numEnemiesInRange++;
				}
				if (!includeInvisibles && !actorData.GetFogOfWar().IsVisible(enemySquare))
				{
					continue;
				}
				if (closestEnemySquare == null)
				{
					closestEnemySquare = enemySquare;
				}
				else
				{
					float dist = currentBoardSquare.HorizontalDistanceOnBoardTo(closestEnemySquare);
					if (distToEnemy < dist)
					{
						closestEnemySquare = enemySquare;
					}
				}
			}
		}
		return closestEnemySquare;
	}

	// removed in rogues
	public BoardSquare GetRetreatSquare()
	{
		ActorData actorData = GetComponent<ActorData>();
		List<ActorData> enemies = GameFlowData.Get().GetAllTeamMembers(actorData.GetEnemyTeam());
		BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
		Vector3 dirToEnemies = new Vector3(0f, 0f, 0f);
		foreach (ActorData enemyActor in enemies)
		{
			BoardSquare enemySquare = enemyActor.GetCurrentBoardSquare();
			if (!enemyActor.IsDead() && enemySquare != null)
			{
				float distToEnemy = currentBoardSquare.HorizontalDistanceOnBoardTo(enemySquare);
				if (distToEnemy <= m_retreatFromRange)
				{
					Vector3 dirToEnemy = enemySquare.ToVector3() - currentBoardSquare.ToVector3();
					dirToEnemy.Normalize();
					dirToEnemies += dirToEnemy;
				}
			}
		}
		Vector3 dirFromEnemies = -dirToEnemies;
		dirFromEnemies.Normalize();
		Vector3 retreatPos = currentBoardSquare.ToVector3() + dirFromEnemies * m_retreatFromRange;
		return Board.Get().GetClosestValidForGameplaySquareTo(retreatPos.x, retreatPos.z);
	}

	// removed in rogues
	public BoardSquare GetAdvanceSquare()
	{
		BoardSquare closestEnemyPlayerSquare = GetClosestEnemyPlayerSquare(true, out int numEnemiesInRange);
		if (closestEnemyPlayerSquare == null)
		{
			return null;
		}
		Vector3 closestEnemyPos = closestEnemyPlayerSquare.ToVector3();
		ActorData actorData = GetComponent<ActorData>();
		BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
		Vector3 currentPos = currentBoardSquare.ToVector3();
		Vector3 dirToClosestEnemy = closestEnemyPos - currentPos;
		if (numEnemiesInRange > 1)
		{
			float distToClosestEnemy = dirToClosestEnemy.magnitude;
			if (distToClosestEnemy > m_idealRange)
			{
				dirToClosestEnemy.Normalize();
				dirToClosestEnemy *= m_idealRange;
			}
		}
		Vector3 dirFromAllies = Vector3.zero;
		List<ActorData> allies = GameFlowData.Get().GetAllTeamMembers(actorData.GetTeam());
		foreach (ActorData allyActor in allies)
		{
			if (allyActor.IsDead() && allyActor != actorData)
			{
				continue;
			}
			BoardSquare allySquare = allyActor.GetCurrentBoardSquare();
			if (allySquare != null && currentBoardSquare.HorizontalDistanceOnBoardTo(allySquare) < m_idealRange)
			{
				Vector3 dirToAlly = allySquare.ToVector3() - currentPos;
				dirToAlly.Normalize();
				dirFromAllies -= dirToAlly * 1.5f;
			}
		}
		Vector3 advancePos = currentPos + dirToClosestEnemy + dirFromAllies;
		BoardSquare advanceSquare = Board.Get().GetSquareClosestToPos(advancePos.x, advancePos.z);
		return Board.Get().GetClosestValidForGameplaySquareTo(advanceSquare);
	}

	// removed in rogues
	public void SelectBotAbilityMods()
	{
		NPCBrain component = GetComponent<NPCBrain>();
		if (component != null)
		{
			component.SelectBotAbilityMods();
		}
		else
		{
			SelectBotAbilityMods_Brainless();
		}
	}

	// removed in rogues
	public void SelectBotCards()
	{
		NPCBrain component = GetComponent<NPCBrain>();
		if (component != null)
		{
			component.SelectBotCards();
		}
		else
		{
			SelectBotCards_Brainless();
		}
	}

	// removed in rogues
	public void SelectBotAbilityMods_Brainless()
	{
		ActorData actorData = GetComponent<ActorData>();
		CharacterModInfo selectedMods = default(CharacterModInfo);
		int i = 0;
		foreach (Ability ability in actorData.GetAbilityData().GetAbilitiesAsList())
		{
			AbilityMod defaultModForAbility = AbilityModManager.Get().GetDefaultModForAbility(ability);
			int mod = defaultModForAbility != null ? defaultModForAbility.m_abilityScopeId : -1;
			selectedMods.SetModForAbility(i, mod);
			i++;
		}
		actorData.m_selectedMods = selectedMods;
	}

	// removed in rogues
	public void SelectBotCards_Brainless()
	{
		ActorData component = GetComponent<ActorData>();
		CharacterCardInfo cardInfo = default(CharacterCardInfo);
		cardInfo.PrepCard = CardManagerData.Get().GetDefaultPrepCardType();
		cardInfo.CombatCard = CardManagerData.Get().GetDefaultCombatCardType();
		cardInfo.DashCard = CardManagerData.Get().GetDefaultDashCardType();
		CardManager.Get().SetDeckAndGiveCards(component, cardInfo);
	}

	public bool IAmTheOnlyBotOnATwoPlayerTeam(ActorData actorData)
	{
		PlayerDetails playerDetails = actorData.PlayerData.LookupDetails();
		if (playerDetails == null)
		{
			return false;
		}
		bool haveOneTeammate = false;
		foreach (LobbyPlayerInfo lobbyPlayerInfo in GameManager.Get().TeamInfo.TeamPlayerInfo)
		{
			if (lobbyPlayerInfo.TeamId == actorData.GetTeam()
			    && lobbyPlayerInfo.PlayerId != playerDetails.m_lobbyPlayerInfoId)
			{
				if (haveOneTeammate)
				{
					return false;
				}

				if (lobbyPlayerInfo.IsAIControlled)
				{
					return false;
				}

				haveOneTeammate = true;
			}
		}
		return true;
	}
	
#if SERVER
	// added in rogues
	public NPCBrain ActiveBrain()
	{
		return m_brainStack.Peek();
	}

	// added in rogues
	public void PushBrain(NPCBrain brain)
	{
		m_brainStack.Push(brain);
		brain.m_botController = this;
	}

	// added in rogues
	public void PopBrain()
	{
		m_brainStack.Pop().m_botController = null;
	}

	// added in rogues
	public void UpdateBrainStack()
	{
		NPCBrain npcbrain = m_brainStack.Peek();
		while (npcbrain == null || npcbrain.ShouldStopBrain())
		{
			m_brainStack.Pop();
			npcbrain = m_brainStack.Peek();
		}
	}

	// added in rogues
	public void ChooseBrainParameters()
	{
		NPCBrainSelector component = GetComponent<NPCBrainSelector>();
		if (component != null)
		{
			component.ChooseBrainParameters();
		}
	}

	// added in rogues
	public void StartDecideMovement_FCFS()
	{
		Log.Info("BotController::StartDecideMovement_FCFS"); // custom log
		StartCoroutine(DecideMovement());
	}

	// added in rogues
	public bool ShouldDoAbilityBeforeMovement()
	{
		bool result = false;
		NPCBrain npcbrain = m_brainStack.Peek();
		if (npcbrain != null)
		{
			result = npcbrain.ShouldDoAbilityBeforeMovement();
		}
		return result;
	}

	// added in rogues
	private IEnumerator DecideMovement()
	{
		if (m_actor.IsDead())
		{
			yield return null;
		}
		else
		{
			NPCBrain npcbrain = m_brainStack.Peek();
			if (npcbrain != null)
			{
				yield return npcbrain.DecideMovement();
			}
			else
			{
				yield return null;
			}
		}
		BotManager.Get().OnMovementDecided(m_actor);
	}

	// added in rogues
	public void StartDecideAbilities_FCFS()
	{
		Log.Info($"BotController::StartDecideAbilities_FCFS {m_actor?.DisplayName} {m_actor?.GetTeam()}"); // custom log
		StartCoroutine(DecideAbilities());
	}

	// added in rogues
	private IEnumerator DecideAbilities()
	{
		if (m_actor.IsDead())
		{
			yield return null;
		}
		else
		{
			NPCBrain npcbrain = m_brainStack.Peek();
			if (npcbrain != null)
			{
				yield return npcbrain.DecideAbilities();
			}
			else
			{
				yield return null;
			}
		}
		BotManager.Get().OnAbilitiesDecided(m_actor);
	}

	// added in rogues
	public void RequestAbility(List<AbilityTarget> targets, AbilityData.ActionType actionType)
	{
		if (targets != null && targets.Count > 0)
		{
			AbilityData component = GetComponent<AbilityData>();
			if (component)
			{
				component.SelectAbilityFromActionType(actionType);
			}
			GetComponent<ServerActorController>().ProcessCastAbilityRequest(targets, actionType, true);
			return;
		}
		GetComponent<ServerActorController>().ProcessCastSimpleActionRequest(actionType, true);
	}
#endif
}
