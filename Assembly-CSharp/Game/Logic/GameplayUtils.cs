// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class GameplayUtils
{
	public static GameObject FindInChildren(this GameObject go, string name, int layerBitMask = 0)
	{
		if (layerBitMask == 0)
		{
			layerBitMask = Camera.main == null ? -1 : Camera.main.cullingMask;
		}
		List<GameObject> list = new List<GameObject>();
		GameObject result = go.FindInChildrenRecursive(name, layerBitMask, false, list);
		return result == null && list.Count > 0
			? list[0]
			: result;
	}

	private static GameObject FindInChildrenRecursive(
		this GameObject obj,
		string name,
		int layerBitMask,
		bool ignoreRootJnt,
		List<GameObject> potentialRetVals)
	{
		if (obj.name == name)
		{
			if (((1 << obj.layer) & layerBitMask) != 0)
			{
				return obj;
			}
			potentialRetVals.Add(obj);
		}
		bool isRootJnt = obj.name == "root_JNT";
		if (ignoreRootJnt && isRootJnt)
		{
			return null;
		}
		bool ignoreRootJnt2 = ignoreRootJnt || isRootJnt;
		foreach (Transform transform in obj.transform)
		{
			GameObject result = transform.gameObject.FindInChildrenRecursive(name, layerBitMask, ignoreRootJnt2, potentialRetVals);
			if (result != null)
			{
				return result;
			}
		}
		return null;
	}

	public static void SetLayerRecursively(this GameObject obj, LayerMask layer)
	{
		obj.layer = layer;
		foreach (Transform transform in obj.transform)
		{
			transform.gameObject.SetLayerRecursively(layer);
		}
	}

	public static void SetActiveIfNeeded(this GameObject obj, bool desiredActive)
	{
		if (obj != null // NOTE CHANGE null check added in rogues
		    && obj.activeSelf != desiredActive)
		{
			obj.SetActive(desiredActive);
		}
	}
	
#if SERVER
	// added in rogues
	public static void PlayAnimSafe(this Animator a, string stateName)
	{
		if (a != null && a.isActiveAndEnabled)
		{
			a.Play(stateName);
		}
	}

	// added in rogues
	public static void PlayAnimSafe(this Animator a, int stateNameHash)
	{
		if (a != null && a.isActiveAndEnabled)
		{
			a.Play(stateNameHash);
		}
	}

	// added in rogues
	public static void PlayAnimSafe(this Animator a, string stateName, int layer)
	{
		if (a != null && a.isActiveAndEnabled)
		{
			a.Play(stateName, layer);
		}
	}

	// added in rogues
	public static void PlayAnimSafe(this Animator a, string stateName, int layer, float normalizedTime)
	{
		if (a != null && a.isActiveAndEnabled)
		{
			a.Play(stateName, layer, normalizedTime);
		}
	}
#endif

	// removed in rogues
	public static bool IsMinion(MonoBehaviour entity)
	{
		return entity.GetComponent<MinionData>() != null;
	}

	// removed in rogues
	public static bool IsMinion(GameObject obj)
	{
		return obj.GetComponent<MinionData>() != null;
	}

	public static bool IsBot(ActorData actor)
	{
		bool result = false;
		if (actor != null)
		{
			result = actor.GetComponent<BotController>() != null;
		}
		return result;
	}

	public static bool IsBot(MonoBehaviour entity)
	{
		return IsBot(entity.gameObject);
	}

	public static bool IsBot(GameObject obj)
	{
		return IsBot(obj.GetComponent<ActorData>());
	}

	public static bool IsPlayerControlled(ActorData actor)
	{
		return actor != null
		       && actor.PlayerIndex != PlayerData.s_invalidPlayerIndex;
	}

	public static bool IsPlayerControlled(MonoBehaviour entity)
	{
		ActorData actorData = entity.GetComponent<ActorData>();
		return actorData != null
		       && actorData.PlayerIndex != PlayerData.s_invalidPlayerIndex;
	}

	public static bool IsPlayerControlled(GameObject obj)
	{
		return IsPlayerControlled(obj.GetComponent<ActorData>());
	}

	public static bool IsHumanControlled(ActorData actor)
	{
		return actor != null && actor.IsHumanControlled();
	}

	public static bool IsHumanControlled(MonoBehaviour entity)
	{
		return IsHumanControlled(entity.gameObject);
	}

	public static bool IsHumanControlled(GameObject obj)
	{
		return IsHumanControlled(obj.GetComponent<ActorData>());
	}

	public static bool IsValidPlayer(ActorData actor)
	{
		return actor != null
		       && actor.PlayerIndex != PlayerData.s_invalidPlayerIndex;
	}

#if SERVER
	// added in rogues
	public static void DestroyActor(GameObject obj)
	{
		ActorData actorData = obj.GetComponent<ActorData>();
		if (actorData != null && UIActorDebugPanel.Get() != null)
		{
			UIActorDebugPanel.Get().OnActorDestroyed(actorData);
		}
		if (actorData)
		{
			if (ServerActionBuffer.Get() != null)
			{
				ServerActionBuffer.Get().CancelActionRequests(actorData, false);
			}
			if (GameFlowData.Get() != null)
			{
				GameFlowData.Get().RemoveReferencesToDestroyedActor(actorData);
			}
			actorData.DespawnTeamSensitiveDataNetObjects();
		}
		PlayerData playerData = obj.GetComponent<PlayerData>();
		if (playerData != null && playerData.GetPlayer().m_valid)
		{
			ServerGameManager.DestroyPlayersForConnection(playerData.GetPlayer().m_connectionId);
		}
		NetworkServer.Destroy(obj);
	}
#endif
	
	public static List<Team> GetOtherTeamsThan(Team team)
	{
		List<Team> list = new List<Team>();
		if (team != Team.TeamA)
		{
			list.Add(Team.TeamA);
		}
		if (team != Team.TeamB)
		{
			list.Add(Team.TeamB);
		}
		if (team != Team.Objects)
		{
			list.Add(Team.Objects);
		}
		return list;
	}

	public static int GetTotalTeamDeaths(Team team)
	{
		int num = 0;
		foreach (ActorData actorData in GameFlowData.Get().GetAllTeamMembers(team))
		{
			if (IsPlayerControlled(actorData))
			{
				num += actorData.GetActorBehavior().totalDeaths;
			}
		}
		return num;
	}

	// removed in rogues
	public static int GetTotalTeamCredits(Team team)
	{
		int num = 0;
		foreach (ActorData actorData in GameFlowData.Get().GetAllTeamMembers(team))
		{
			if (IsPlayerControlled(actorData))
			{
				num += actorData.GetComponent<ItemData>().GetNetWorth();
			}
		}
		return num;
	}

	public static int GetScoreForVariableType(ActorData actor, TargeterUtils.VariableType variableType)
	{
		switch (variableType)
		{
			case TargeterUtils.VariableType.Energy:
				return actor.TechPoints;
			case TargeterUtils.VariableType.HitPoints:
				return actor.HitPoints;
			case TargeterUtils.VariableType.MechanicPoints:
				return actor.MechanicPoints;
			default:
				return 0;
		}
	}

	public static int GetActorIndexOfActor(ActorData actor)
	{
		return actor != null
			? actor.ActorIndex
			: ActorData.s_invalidActorIndex;
	}

	public static ActorData GetActorOfActorIndex(int actorIndex)
	{
		return actorIndex != ActorData.s_invalidActorIndex
			? GameFlowData.Get().FindActorByActorIndex(actorIndex)
			: null;
	}

	public static MatchResultsStats GenerateStatsFromGame(Team perspectiveTeam, int perspectivePlayerId)
	{
		Team us = perspectiveTeam == Team.TeamB ? Team.TeamB : Team.TeamA;
		Team them = perspectiveTeam == Team.TeamB ? Team.TeamA : Team.TeamB;
		return new MatchResultsStats
		{
			FriendlyStatlines = (from actorData in GameFlowData.Get().GetPlayerAndBotTeamMembers(us)
				select GenerateStatlineFromGame(actorData, perspectiveTeam, perspectivePlayerId)).ToArray(),
			EnemyStatlines = (from actorData in GameFlowData.Get().GetPlayerAndBotTeamMembers(them)
				select GenerateStatlineFromGame(actorData, perspectiveTeam, perspectivePlayerId)).ToArray(),
			RedScore = ObjectivePoints.Get() != null
				? ObjectivePoints.Get().GetPointsForTeam(Team.TeamA)
				: 0,
			BlueScore = ObjectivePoints.Get() != null
				? ObjectivePoints.Get().GetPointsForTeam(Team.TeamB)
				: 0,
			GameTime = GameFlowData.Get().GetGameTime(),
			VictoryCondition = ObjectivePoints.Get() != null
				? ObjectivePoints.Get().m_victoryCondition
				: "TurnsLeft@GameModes",
			VictoryConditionTurns = ObjectivePoints.Get() != null
				? ObjectivePoints.Get().m_timeLimitTurns
				: 20
		};
	}

	public static MatchResultsStatline GenerateStatlineFromGame(ActorData actorData, Team perspectiveTeam, int perspectivePlayerId)
	{
		PlayerDetails playerDetails = null;
		if (actorData.PlayerData != null)
		{
			playerDetails = GameFlow.Get().playerDetails.TryGetValue(actorData.PlayerData.GetPlayer());
		}
		int lobbyPlayerInfoId = -1;
		if (playerDetails != null)
		{
			lobbyPlayerInfoId = playerDetails.m_lobbyPlayerInfoId;
		}
		LobbyPlayerInfo lobbyPlayerInfo = GameManager.Get().TeamInfo.TeamPlayerInfo.FirstOrDefault(element => element.PlayerId == lobbyPlayerInfoId);
		ActorBehavior actorBehavior = actorData.GetActorBehavior();
		AbilityData abilityData = actorData.GetAbilityData();
		return new MatchResultsStatline
		{
			Actor = actorData,
			PlayerId = lobbyPlayerInfoId,
			DisplayName = actorData.GetDisplayName(),
			Character = actorData.m_characterType,
			IsPerspective = lobbyPlayerInfoId == perspectivePlayerId,
			IsAlly = perspectiveTeam == actorData.GetTeam(),
			IsHumanControlled = playerDetails != null && playerDetails.IsHumanControlled,
			IsBotMasqueradingAsHuman = playerDetails?.m_botsMasqueradeAsHumans ?? false,
			HumanReplacedByBot = playerDetails != null && playerDetails.ReplacedWithBots,
			AccountID = actorData.GetOriginalAccountId(),
			TitleID = lobbyPlayerInfo?.TitleID ?? 0,
			TitleLevel = lobbyPlayerInfo?.TitleLevel ?? 0,
			BannerID = lobbyPlayerInfo?.BannerID ?? 0,
			EmblemID = lobbyPlayerInfo?.EmblemID ?? 0,
			RibbonID = lobbyPlayerInfo?.RibbonID ?? 0,
			AbilityEntries = abilityData.abilityEntries.Select(entry => new MatchResultsStatline.AbilityEntry
			{
				AbilityModId = entry != null
				               && entry.ability != null
				               && entry.ability.CurrentAbilityMod != null
					? entry.ability.CurrentAbilityMod.m_abilityScopeId
					: -1
			}).ToArray(),
			CatalystHasPrepPhase = abilityData.GetActiveCards().ContainsWhere(card =>
				card != null && card.GetAbilityRunPhase() == AbilityRunPhase.Prep),
			CatalystHasDashPhase = abilityData.GetActiveCards().ContainsWhere(card =>
				card != null && card.GetAbilityRunPhase() == AbilityRunPhase.Dash),
			CatalystHasBlastPhase = abilityData.GetActiveCards().ContainsWhere(card =>
				card != null && card.GetAbilityRunPhase() == AbilityRunPhase.Combat),
			TotalPlayerKills = actorBehavior.totalPlayerKills,
			TotalDeaths = actorBehavior.totalDeaths,
			TotalPlayerDamage = actorBehavior.totalPlayerDamage,
			TotalPlayerAssists = actorBehavior.totalPlayerAssists,
			TotalPlayerHealingFromAbility = actorBehavior.totalPlayerHealingFromAbility,
			TotalPlayerAbsorb = actorBehavior.totalPlayerAbsorb,
			TotalPlayerDamageReceived = actorBehavior.totalPlayerDamageReceived,
			TotalPlayerTurns = actorBehavior.totalPlayerTurns,
			TotalPlayerLockInTime = actorBehavior.totalPlayerLockInTime,
			TotalPlayerContribution = actorBehavior.totalPlayerContribution
		};
	}
}
