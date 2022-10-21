// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

// server-only
#if SERVER
public static class GameplayMetricHelper
{
	private const string c_moveDeniedHeader = "<color=magenta>movement_denied: </color>";

	public static bool GameplayMetricDebugTraceOn => false;

	public static void DebugLog(string log)
	{
		Log.Warning(Log.Category.GameplayMetrics, DebugHeader() + log);
	}

	public static void DebugLogMoveDenied(string log)
	{
		Log.Warning(Log.Category.GameplayMetrics, DebugHeader() + "<color=magenta>movement_denied: </color>" + log);
	}

	public static void InitializePlayerGameSummary(List<PlayerDetails> humanPlayerInfoList)
	{
		GameManager.Get().GameSummary.PlayerGameSummaryList.Clear();
		foreach (PlayerDetails playerDetails in humanPlayerInfoList)
		{
			foreach (GameObject gameObject in playerDetails.m_gameObjects)
			{
				ActorData component = gameObject.GetComponent<ActorData>();
				if (component != null)
				{
					PlayerGameSummary playerGameSummary = new PlayerGameSummary();
					CharacterResourceLink characterResourceLink = component.GetCharacterResourceLink();
					playerGameSummary.AccountId = playerDetails.m_accountId;
					playerGameSummary.PlayerId = playerDetails.m_serverPlayerInfo.PlayerId;
					playerGameSummary.InGameName = playerDetails.m_handle;
					playerGameSummary.CharacterPlayed = component.m_characterType;
					playerGameSummary.CharacterName = (characterResourceLink ? characterResourceLink.m_displayName : "?");
					if (playerDetails.m_serverPlayerInfo != null && playerDetails.m_serverPlayerInfo.SelectedCharacter != null)
					{
						playerGameSummary.CharacterSkinIndex = playerDetails.m_serverPlayerInfo.SelectedCharacter.selectedSkin.skinIndex;
					}
					playerGameSummary.Team = (int)playerDetails.m_team;
					playerGameSummary.ActorIndex = component.ActorIndex;
					playerGameSummary.PrepCatalystName = component.m_selectedCards.PrepCard.ToString();
					playerGameSummary.DashCatalystName = component.m_selectedCards.DashCard.ToString();
					playerGameSummary.CombatCatalystName = component.m_selectedCards.CombatCard.ToString();
					InitializeAbilitySummaryList(playerGameSummary, component, playerDetails);
					GameManager.Get().GameSummary.PlayerGameSummaryList.Add(playerGameSummary);
					if (GameplayMetricDebugTraceOn)
					{
						Debug.LogWarning(DebugHeader() + "Initialize Player Metric " + playerGameSummary.ToPlayerInfoString());
					}
				}
			}
		}
	}

	private static void InitializeAbilitySummaryList(PlayerGameSummary playerSummary, ActorData actor, PlayerDetails playerInfo)
	{
		if (playerSummary != null && actor != null)
		{
			playerSummary.AbilityGameSummaryList.Clear();
			AbilityData.AbilityEntry[] abilityEntries = actor.GetAbilityData().abilityEntries;
			for (int i = 0; i <= 4; i++)
			{
				Ability ability = abilityEntries[i].ability;
				if (ability != null)
				{
					AbilityGameSummary abilityGameSummary = new AbilityGameSummary();
					// rogues
					//List<Gear> availableGearForAbilityAndAccount = GearHelper.GetAvailableGearForAbilityAndAccount(playerSummary.AccountId, ability, actor.m_characterType, i);
					//abilityGameSummary.GearName = "NoGear";
					//foreach (Gear gear in availableGearForAbilityAndAccount)
					//{
					//	if (actor.m_selectedGear.GetGearIDForAbility(i) == gear.ID)
					//	{
					//		abilityGameSummary.GearName = gear.GetName();
					//	}
					//}
					abilityGameSummary.ActionType = i;
					abilityGameSummary.AbilityName = ability.m_abilityName;
					abilityGameSummary.AbilityClassName = ability.GetType().ToString();
					playerSummary.AbilityGameSummaryList.Add(abilityGameSummary);
				}
			}
		}
	}

	public static void CollectGameplayStats(List<ActorData> allActors)
	{
		int totalTeamDamageReceived = GameFlowData.Get().GetTotalTeamDamageReceived(Team.TeamA);
		int totalTeamDamageReceived2 = GameFlowData.Get().GetTotalTeamDamageReceived(Team.TeamB);
		foreach (ActorData actorData in allActors)
		{
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(actorData);
			if (playerSummaryByActor != null)
			{
				playerSummaryByActor.TotalGameTurns = actorData.GetActorBehavior().totalPlayerTurns;
				playerSummaryByActor.NumDeaths = actorData.GetActorBehavior().totalDeaths;
				playerSummaryByActor.NumKills = actorData.GetActorBehavior().totalPlayerKills;
				playerSummaryByActor.NumAssists = actorData.GetActorBehavior().totalPlayerAssists;
				playerSummaryByActor.TotalPlayerDamage = actorData.GetActorBehavior().totalPlayerDamage;
				playerSummaryByActor.TotalPlayerHealing = actorData.GetActorBehavior().totalPlayerHealing;
				playerSummaryByActor.TotalPlayerAbsorb = actorData.GetActorBehavior().totalPlayerAbsorb;
				playerSummaryByActor.TotalPlayerDamageReceived = actorData.GetActorBehavior().totalPlayerDamageReceived;
				if (actorData.GetTeam() == Team.TeamA)
				{
					playerSummaryByActor.TotalTeamDamageReceived = totalTeamDamageReceived;
				}
				else if (actorData.GetTeam() == Team.TeamB)
				{
					playerSummaryByActor.TotalTeamDamageReceived = totalTeamDamageReceived2;
				}
				playerSummaryByActor.NetDamageAvoidedByEvades = actorData.GetActorBehavior().netDamageAvoidedByEvades;
				playerSummaryByActor.DamageAvoidedByEvades = actorData.GetActorBehavior().damageDodgedByEvades;
				playerSummaryByActor.DamageInterceptedByEvades = actorData.GetActorBehavior().damageInterceptedByEvades;
				playerSummaryByActor.MyIncomingDamageReducedByCover = actorData.GetActorBehavior().myIncomingDamageReducedByCover;
				playerSummaryByActor.MyOutgoingDamageReducedByCover = actorData.GetActorBehavior().myOutgoingDamageReducedByCover;
				playerSummaryByActor.MyOutgoingExtraDamageFromEmpowered = actorData.GetActorBehavior().myOutgoingExtraDamageFromEmpowered;
				playerSummaryByActor.MyOutgoingReducedDamageFromWeakened = actorData.GetActorBehavior().myOutgoingReducedDamageFromWeakened;
				playerSummaryByActor.TeamOutgoingDamageIncreasedByEmpoweredFromMe = actorData.GetActorBehavior().teamOutgoingDamageIncreasedByEmpoweredFromMe;
				playerSummaryByActor.TeamIncomingDamageReducedByWeakenedFromMe = actorData.GetActorBehavior().teamIncomingDamageReducedByWeakenedFromMe;
				playerSummaryByActor.MovementDeniedByMe = actorData.GetActorBehavior().movementDeniedByMe;
				playerSummaryByActor.EnergyGainPerTurn = actorData.GetActorBehavior().EnergyGainPerTurn;
				playerSummaryByActor.DamageEfficiency = actorData.GetActorBehavior().DamageEfficiency;
				playerSummaryByActor.KillParticipation = actorData.GetActorBehavior().KillParticipation;
				playerSummaryByActor.EffectiveHealing = actorData.GetActorBehavior().EffectiveHealing;
				playerSummaryByActor.EffectiveHealingFromAbility = actorData.GetActorBehavior().EffectiveHealingFromAbility;
				playerSummaryByActor.TeamExtraEnergyByEnergizedFromMe = actorData.GetActorBehavior().teamExtraEnergyGainFromMe;
				playerSummaryByActor.EnemiesSightedPerTurn = actorData.GetActorBehavior().NumEnemiesSightedPerTurn;
				if (Application.isEditor && GameplayMetricDebugTraceOn)
				{
					string text = "Gameplay Stats for " + actorData.DebugNameString() + "\n";
					foreach (object obj in Enum.GetValues(typeof(StatDisplaySettings.StatType)))
					{
						StatDisplaySettings.StatType typeOfStat = (StatDisplaySettings.StatType)obj;
						text = string.Concat(text, "Stat[ ", typeOfStat.ToString(), " ] = ", actorData.GetActorBehavior().GetStat(typeOfStat), "\n");
					}
					DebugLog(text);
				}
				if (actorData.m_characterType == CharacterType.TeleportingNinja && actorData.GetComponent<Ninja_SyncComponent>() != null)
				{
					int totalDeathmarkDamage = actorData.GetComponent<Ninja_SyncComponent>().GetTotalDeathmarkDamage();
					playerSummaryByActor.CharacterSpecificStat = totalDeathmarkDamage;
				}
			}
		}
	}

	public static void CollectFreelancerStats(List<ActorData> allActors)
	{
		foreach (ActorData actorData in allActors)
		{
			FreelancerStats freelancerStats = actorData.GetFreelancerStats();
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(actorData);
			if (freelancerStats != null && playerSummaryByActor != null)
			{
				int numStats = freelancerStats.GetNumStats();
				List<int> list = new List<int>(numStats);
				for (int i = 0; i < numStats; i++)
				{
					list.Add(freelancerStats.GetValueOfStat(i));
				}
				playerSummaryByActor.FreelancerStats = list;
			}
		}
	}

	public static void SetPlayerGameResults()
	{
		GameResult gameResult = GameManager.Get().GameSummary.GameResult;
		foreach (PlayerGameSummary playerGameSummary in GameManager.Get().GameSummary.PlayerGameSummaryList)
		{
			if (gameResult == GameResult.NoResult)
			{
				playerGameSummary.PlayerGameResult = PlayerGameResult.NoResult;
			}
			else if (gameResult == GameResult.TieGame)
			{
				playerGameSummary.PlayerGameResult = PlayerGameResult.Tie;
			}
			else if (playerGameSummary.ResultForWin() == gameResult)
			{
				playerGameSummary.PlayerGameResult = PlayerGameResult.Win;
			}
			else
			{
				playerGameSummary.PlayerGameResult = PlayerGameResult.Lose;
			}
		}
	}

	public static void SetPlayerStats(List<ActorData> allActors)
	{
		foreach (ActorData actorData in allActors)
		{
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(actorData);
			if (playerSummaryByActor != null)
			{
				playerSummaryByActor.MatchResults = GameplayUtils.GenerateStatsFromGame(actorData.GetTeam(), actorData.GetPlayerDetails().m_lobbyPlayerInfoId);
			}
		}
	}

	public static void IncrementTargetHitCount(ActorData caster, Ability sourceAbility)
	{
		if (sourceAbility != null)
		{
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(caster);
			if (playerSummaryByActor != null)
			{
				AbilityData.ActionType parentAbilityActionType = caster.GetAbilityData().GetParentAbilityActionType(sourceAbility);
				AbilityGameSummary abilitySummary = GetAbilitySummary(playerSummaryByActor, parentAbilityActionType);
				if (abilitySummary != null)
				{
					abilitySummary.TotalTargetsHit++;
					if (GameplayMetricDebugTraceOn)
					{
						Debug.LogWarning(string.Concat(DebugHeader(), "Increment TotalTargetHit, currently ", abilitySummary.TotalTargetsHit, " ", DebugSourceString(caster, sourceAbility, parentAbilityActionType)));
					}
				}
			}
		}
	}

	public static void CollectDamageDealt(ActorData caster, ActorData target, int amount, Ability sourceAbility)
	{
		if (amount > 0 && caster != null)
		{
			CheckCollectionTime();
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(caster);
			if (playerSummaryByActor != null)
			{
				AbilityData.ActionType parentAbilityActionType = caster.GetAbilityData().GetParentAbilityActionType(sourceAbility);
				AbilityGameSummary abilitySummary = GetAbilitySummary(playerSummaryByActor, parentAbilityActionType);
				if (abilitySummary != null)
				{
					abilitySummary.TotalDamage += amount;
				}
				if (GameplayMetricDebugTraceOn)
				{
					Debug.LogWarning(string.Concat(DebugHeader(), "Collecting ", amount.ToString(), " Damage for ", DebugSourceString(caster, sourceAbility, parentAbilityActionType)));
				}
			}
			caster.GetActorBehavior().OnDamageDealt(amount, target);
		}
	}

	public static void CollectHealingDealt(ActorData caster, ActorData target, int amount, Ability sourceAbility)
	{
		if (amount > 0 && caster != null)
		{
			CheckCollectionTime();
			AbilityData.ActionType parentAbilityActionType = caster.GetAbilityData().GetParentAbilityActionType(sourceAbility);
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(caster);
			if (playerSummaryByActor != null)
			{
				AbilityGameSummary abilitySummary = GetAbilitySummary(playerSummaryByActor, parentAbilityActionType);
				if (abilitySummary != null)
				{
					abilitySummary.TotalHealing += amount;
				}
				if (GameplayMetricDebugTraceOn)
				{
					Debug.LogWarning(string.Concat(DebugHeader(), "Collecting ", amount.ToString(), " Healing for ", DebugSourceString(caster, sourceAbility, parentAbilityActionType)));
				}
			}
			caster.GetActorBehavior().OnHealingDealt(amount, parentAbilityActionType >= AbilityData.ActionType.ABILITY_0 && parentAbilityActionType <= AbilityData.ActionType.ABILITY_4, target);
		}
	}

	public static void CollectAbsorbDealt(ActorData actor, int amount, Ability sourceAbility)
	{
		if (amount > 0 && actor != null)
		{
			CheckCollectionTime();
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(actor);
			if (playerSummaryByActor != null)
			{
				AbilityData.ActionType parentAbilityActionType = actor.GetAbilityData().GetParentAbilityActionType(sourceAbility);
				AbilityGameSummary abilitySummary = GetAbilitySummary(playerSummaryByActor, parentAbilityActionType);
				if (abilitySummary != null)
				{
					abilitySummary.TotalAbsorb += amount;
				}
				if (GameplayMetricDebugTraceOn)
				{
					Debug.LogWarning(string.Concat(DebugHeader(), "Collecting ", amount.ToString(), " Absorb for ", DebugSourceString(actor, sourceAbility, parentAbilityActionType)));
				}
			}
			actor.GetActorBehavior().OnAbsorbDealt(amount);
		}
	}

	public static void CollectPotentialAbsorbDealt(ActorData actor, int amount, Ability sourceAbility)
	{
		if (amount > 0 && actor != null)
		{
			CheckCollectionTime();
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(actor);
			if (playerSummaryByActor != null)
			{
				AbilityData.ActionType parentAbilityActionType = actor.GetAbilityData().GetParentAbilityActionType(sourceAbility);
				playerSummaryByActor.TotalPotentialAbsorb += amount;
				AbilityGameSummary abilitySummary = GetAbilitySummary(playerSummaryByActor, parentAbilityActionType);
				if (abilitySummary != null)
				{
					abilitySummary.TotalPotentialAbsorb += amount;
				}
				if (GameplayMetricDebugTraceOn)
				{
					Debug.LogWarning(string.Concat(DebugHeader(), "Collecting ", amount.ToString(), " PotentialAbsorb for ", DebugSourceString(actor, sourceAbility, parentAbilityActionType)));
				}
			}
			actor.GetActorBehavior().OnPotentialAbsorbDealt(amount);
		}
	}

	public static void CollectTechPointGainForAbility(ActorData caster, ActorData target, int amount, Ability sourceAbility)
	{
		if (caster == null || target == null || sourceAbility == null || amount <= 0)
		{
			return;
		}
		PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(caster);
		if (playerSummaryByActor != null)
		{
			AbilityData.ActionType parentAbilityActionType = caster.GetAbilityData().GetParentAbilityActionType(sourceAbility);
			AbilityGameSummary abilitySummary = GetAbilitySummary(playerSummaryByActor, parentAbilityActionType);
			if (abilitySummary != null)
			{
				if (caster == target)
				{
					abilitySummary.TotalEnergyGainOnSelf += amount;
					return;
				}
				if (caster.GetTeam() == target.GetTeam())
				{
					abilitySummary.TotalEnergyGainToOthers += amount;
				}
			}
		}
	}

	public static void CollectTechPointLossForAbility(ActorData caster, ActorData target, int amount, Ability sourceAbility)
	{
		if (caster == null || target == null || sourceAbility == null || amount <= 0)
		{
			return;
		}
		PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(caster);
		if (playerSummaryByActor != null)
		{
			AbilityData.ActionType parentAbilityActionType = caster.GetAbilityData().GetParentAbilityActionType(sourceAbility);
			AbilityGameSummary abilitySummary = GetAbilitySummary(playerSummaryByActor, parentAbilityActionType);
			if (abilitySummary != null && caster.GetTeam() != target.GetTeam())
			{
				abilitySummary.TotalEnergyLossToOthers += amount;
			}
		}
	}

	public static void CollectPowerup(ActorData collector)
	{
		if (collector == null)
		{
			return;
		}
		PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(collector);
		if (playerSummaryByActor != null)
		{
			playerSummaryByActor.PowerupsCollected++;
		}
	}

	public static void CollectTechPointsRecieved(ActorData actor, int amount)
	{
		if (amount > 0 && actor != null)
		{
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(actor);
			if (playerSummaryByActor != null)
			{
				playerSummaryByActor.TotalTechPointsGained += amount;
				if (GameplayMetricDebugTraceOn)
				{
					Debug.LogWarning(string.Concat(DebugHeader(), " actor '", actor.DisplayName, "' is collecting ", amount.ToString(), " tech (energy) points for a total of ", playerSummaryByActor.TotalTechPointsGained));
				}
			}
		}
	}

	public static void CollectDamageReceived(ActorData receiver, int amount)
	{
		if (amount > 0)
		{
			CheckCollectionTime();
			receiver.GetActorBehavior().OnDamageReceived(amount);
		}
	}

	public static void CollectHealingReceived(ActorData receiver, int amount)
	{
		if (amount > 0)
		{
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(receiver);
			if (playerSummaryByActor != null)
			{
				playerSummaryByActor.TotalHealingReceived += amount;
				if (GameplayMetricDebugTraceOn)
				{
					Debug.LogWarning(string.Concat(DebugHeader(), "Collecting ", amount.ToString(), " Healing Received for ", receiver.DebugNameString()));
				}
			}
			receiver.GetActorBehavior().OnHealingReceived(amount);
		}
	}

	public static void CollectAbsorbReceived(ActorData receiver, int amount)
	{
		if (amount > 0)
		{
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(receiver);
			if (playerSummaryByActor != null)
			{
				playerSummaryByActor.TotalAbsorbReceived += amount;
				if (GameplayMetricDebugTraceOn)
				{
					Debug.LogWarning(string.Concat(DebugHeader(), "Collecting ", amount.ToString(), " Absorb Received for ", receiver.DebugNameString()));
				}
			}
			receiver.GetActorBehavior().OnAbsorbReceived(amount);
		}
	}

	public static void CollectTurnStart(ActorData receiver)
	{
		if (GetPlayerSummaryByActor(receiver) != null && receiver.HasBotController && GameplayMetricDebugTraceOn)
		{
			Debug.LogWarning(DebugHeader() + "Collecting a new turn for player '" + receiver.name + string.Format(" IsBot: {0}", receiver.HasBotController));
		}
	}

	public static void IncrementAbilityUseCount(ActorData actor, Ability ability, bool taunted)
	{
		AbilityGameSummary abilitySummary = GetAbilitySummary(actor, ability);
		if (abilitySummary != null)
		{
			abilitySummary.CastCount++;
			if (taunted)
			{
				abilitySummary.TauntCount++;
			}
		}
	}

	public static void RecordCatalystUsed(ActorData actor, AbilityData.ActionType actionType)
	{
		PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(actor);
		if (playerSummaryByActor != null)
		{
			if (actionType == AbilityData.ActionType.CARD_0)
			{
				playerSummaryByActor.PrepCatalystUsed = true;
				return;
			}
			if (actionType == AbilityData.ActionType.CARD_1)
			{
				playerSummaryByActor.DashCatalystUsed = true;
				return;
			}
			if (actionType == AbilityData.ActionType.CARD_2)
			{
				playerSummaryByActor.CombatCatalystUsed = true;
			}
		}
	}

	public static void RecordTimebankUsed(ActorData actor)
	{
		GetPlayerSummaryByActor(actor)?.TimebankUsage.Add(GameFlowData.Get().CurrentTurn);
	}

	private static PlayerGameSummary GetPlayerSummaryByActor(ActorData actor)
	{
		if (actor == null)
		{
			return null;
		}
		foreach (PlayerGameSummary playerGameSummary in GameManager.Get().GameSummary.PlayerGameSummaryList)
		{
			if (playerGameSummary.ActorIndex == actor.ActorIndex)
			{
				return playerGameSummary;
			}
		}
		return null;
	}

	private static AbilityGameSummary GetAbilitySummary(ActorData actor, Ability ability, bool useParentActionType = false)
	{
		if (actor != null && ability != null)
		{
			PlayerGameSummary playerSummaryByActor = GetPlayerSummaryByActor(actor);
			if (playerSummaryByActor != null)
			{
				AbilityData.ActionType actionType = useParentActionType ? actor.GetAbilityData().GetParentAbilityActionType(ability) : actor.GetAbilityData().GetActionTypeOfAbility(ability);
				foreach (AbilityGameSummary abilityGameSummary in playerSummaryByActor.AbilityGameSummaryList)
				{
					if (abilityGameSummary.ActionType == (int)actionType)
					{
						return abilityGameSummary;
					}
				}
			}
		}
		return null;
	}

	private static AbilityGameSummary GetAbilitySummary(PlayerGameSummary playerSummary, AbilityData.ActionType actionType)
	{
		if (playerSummary != null)
		{
			foreach (AbilityGameSummary abilityGameSummary in playerSummary.AbilityGameSummaryList)
			{
				if (abilityGameSummary.ActionType == (int)actionType)
				{
					return abilityGameSummary;
				}
			}
		}
		return null;
	}

	private static string DebugSourceString(ActorData caster, Ability sourceAbility, AbilityData.ActionType actionType)
	{
		string text = "";
		if (caster != null)
		{
			string text2 = (sourceAbility == null) ? "NULL" : sourceAbility.m_abilityName;
			text = string.Concat(text, caster.ToString(), "\n<color=yellow>Ability [ ", text2, " ] of ActionType [ ", actionType.ToString(), " ]</color>");
		}
		return text;
	}

	private static string DebugHeader()
	{
		return "<color=green>[Gameplay_Metrics]: </color>";
	}

	private static void CheckCollectionTime()
	{
	}
}
#endif
