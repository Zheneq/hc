using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameplayUtils
{
	public static GameObject FindInChildren(this GameObject go, string name, int layerBitMask = 0)
	{
		if (layerBitMask == 0)
		{
			int num;
			if (Camera.main == null)
			{
				num = -1;
			}
			else
			{
				num = Camera.main.cullingMask;
			}
			layerBitMask = num;
		}
		List<GameObject> list = new List<GameObject>();
		GameObject gameObject = go.FindInChildrenRecursive(name, layerBitMask, false, list);
		GameObject result;
		if (gameObject == null)
		{
			if (list.Count > 0)
			{
				result = list[0];
				goto IL_008d;
			}
		}
		result = gameObject;
		goto IL_008d;
		IL_008d:
		return result;
	}

	private static GameObject FindInChildrenRecursive(this GameObject obj, string name, int layerBitMask, bool ignoreRootJnt, List<GameObject> potentialRetVals)
	{
		if (obj.name == name)
		{
			if (((1 << obj.layer) & layerBitMask) != 0)
			{
				return obj;
			}
			potentialRetVals.Add(obj);
		}
		bool flag = obj.name == "root_JNT";
		if (ignoreRootJnt && flag)
		{
			return null;
		}
		int num;
		if (!ignoreRootJnt)
		{
			num = (flag ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool ignoreRootJnt2 = (byte)num != 0;
		IEnumerator enumerator = obj.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				GameObject gameObject = transform.gameObject.FindInChildrenRecursive(name, layerBitMask, ignoreRootJnt2, potentialRetVals);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						disposable.Dispose();
						goto end_IL_00b5;
					}
				}
			}
			end_IL_00b5:;
		}
		return null;
	}

	public static void SetLayerRecursively(this GameObject obj, LayerMask layer)
	{
		obj.layer = layer;
		IEnumerator enumerator = obj.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				transform.gameObject.SetLayerRecursively(layer);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						disposable.Dispose();
						goto end_IL_0055;
					}
				}
			}
			end_IL_0055:;
		}
	}

	public static void SetActiveIfNeeded(this GameObject obj, bool desiredActive)
	{
		if (obj.activeSelf != desiredActive)
		{
			obj.SetActive(desiredActive);
		}
	}

	public static bool IsMinion(MonoBehaviour entity)
	{
		return entity.GetComponent<MinionData>() != null;
	}

	public static bool IsMinion(GameObject obj)
	{
		return obj.GetComponent<MinionData>() != null;
	}

	public static bool IsBot(ActorData actor)
	{
		bool result = false;
		if (actor != null)
		{
			result = (actor.GetComponent<BotController>() != null);
		}
		return result;
	}

	public static bool IsBot(MonoBehaviour entity)
	{
		return IsBot(entity.gameObject);
	}

	public static bool IsBot(GameObject obj)
	{
		ActorData component = obj.GetComponent<ActorData>();
		return IsBot(component);
	}

	public static bool IsPlayerControlled(ActorData actor)
	{
		bool result = false;
		if (actor != null)
		{
			result = (actor.PlayerIndex != PlayerData.s_invalidPlayerIndex);
		}
		return result;
	}

	public static bool IsPlayerControlled(MonoBehaviour entity)
	{
		bool result = false;
		ActorData component = entity.GetComponent<ActorData>();
		if (component != null)
		{
			result = (component.PlayerIndex != PlayerData.s_invalidPlayerIndex);
		}
		return result;
	}

	public static bool IsPlayerControlled(GameObject obj)
	{
		ActorData component = obj.GetComponent<ActorData>();
		return IsPlayerControlled(component);
	}

	public static bool IsHumanControlled(ActorData actor)
	{
		bool result = false;
		if (actor != null)
		{
			result = actor.IsHumanControlled();
		}
		return result;
	}

	public static bool IsHumanControlled(MonoBehaviour entity)
	{
		return IsHumanControlled(entity.gameObject);
	}

	public static bool IsHumanControlled(GameObject obj)
	{
		ActorData component = obj.GetComponent<ActorData>();
		return IsHumanControlled(component);
	}

	public static bool IsValidPlayer(ActorData actor)
	{
		int result;
		if (actor != null)
		{
			result = ((actor.PlayerIndex != PlayerData.s_invalidPlayerIndex) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public static List<Team> GetOtherTeamsThan(Team team)
	{
		List<Team> list = new List<Team>();
		if (team != 0)
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
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(team);
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (IsPlayerControlled(current))
				{
					ActorBehavior actorBehavior = current.GetActorBehavior();
					num += actorBehavior.totalDeaths;
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return num;
				}
			}
		}
	}

	public static int GetTotalTeamCredits(Team team)
	{
		int num = 0;
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(team);
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (IsPlayerControlled(current))
				{
					ItemData component = current.GetComponent<ItemData>();
					num += component.GetNetWorth();
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return num;
				}
			}
		}
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
		if (actor == null)
		{
			return ActorData.s_invalidActorIndex;
		}
		return actor.ActorIndex;
	}

	public static ActorData GetActorOfActorIndex(int actorIndex)
	{
		if (actorIndex == ActorData.s_invalidActorIndex)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		return GameFlowData.Get().FindActorByActorIndex(actorIndex);
	}

	public static MatchResultsStats GenerateStatsFromGame(Team perspectiveTeam, int perspectivePlayerId)
	{
		Team team = (perspectiveTeam == Team.TeamB) ? Team.TeamB : Team.TeamA;
		int num;
		if (perspectiveTeam == Team.TeamB)
		{
			num = 0;
		}
		else
		{
			num = 1;
		}
		Team team2 = (Team)num;
		MatchResultsStats matchResultsStats = new MatchResultsStats();
		matchResultsStats.FriendlyStatlines = (from actorData in GameFlowData.Get().GetPlayerAndBotTeamMembers(team)
			select GenerateStatlineFromGame(actorData, perspectiveTeam, perspectivePlayerId)).ToArray();
		matchResultsStats.EnemyStatlines = (from actorData in GameFlowData.Get().GetPlayerAndBotTeamMembers(team2)
			select GenerateStatlineFromGame(actorData, perspectiveTeam, perspectivePlayerId)).ToArray();
		int redScore;
		if (ObjectivePoints.Get() != null)
		{
			redScore = ObjectivePoints.Get().GetPointsForTeam(Team.TeamA);
		}
		else
		{
			redScore = 0;
		}
		matchResultsStats.RedScore = redScore;
		int blueScore;
		if (ObjectivePoints.Get() != null)
		{
			blueScore = ObjectivePoints.Get().GetPointsForTeam(Team.TeamB);
		}
		else
		{
			blueScore = 0;
		}
		matchResultsStats.BlueScore = blueScore;
		matchResultsStats.GameTime = GameFlowData.Get().GetGameTime();
		object victoryCondition;
		if (ObjectivePoints.Get() != null)
		{
			victoryCondition = ObjectivePoints.Get().m_victoryCondition;
		}
		else
		{
			victoryCondition = "TurnsLeft@GameModes";
		}
		matchResultsStats.VictoryCondition = (string)victoryCondition;
		int victoryConditionTurns;
		if (ObjectivePoints.Get() != null)
		{
			victoryConditionTurns = ObjectivePoints.Get().m_timeLimitTurns;
		}
		else
		{
			victoryConditionTurns = 20;
		}
		matchResultsStats.VictoryConditionTurns = victoryConditionTurns;
		return matchResultsStats;
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
		LobbyTeamInfo teamInfo = GameManager.Get().TeamInfo;
		LobbyPlayerInfo lobbyPlayerInfo = teamInfo.TeamPlayerInfo.FirstOrDefault((LobbyPlayerInfo element) => element.PlayerId == lobbyPlayerInfoId);
		ActorBehavior actorBehavior = actorData.GetActorBehavior();
		AbilityData abilityData = actorData.GetAbilityData();
		MatchResultsStatline matchResultsStatline = new MatchResultsStatline();
		matchResultsStatline.Actor = actorData;
		matchResultsStatline.PlayerId = lobbyPlayerInfoId;
		matchResultsStatline.DisplayName = actorData.GetDisplayName();
		matchResultsStatline.Character = actorData.m_characterType;
		matchResultsStatline.IsPerspective = (lobbyPlayerInfoId == perspectivePlayerId);
		matchResultsStatline.IsAlly = (perspectiveTeam == actorData.GetTeam());
		int isHumanControlled;
		if (playerDetails != null)
		{
			isHumanControlled = (playerDetails.IsHumanControlled ? 1 : 0);
		}
		else
		{
			isHumanControlled = 0;
		}
		matchResultsStatline.IsHumanControlled = ((byte)isHumanControlled != 0);
		matchResultsStatline.IsBotMasqueradingAsHuman = (playerDetails?.m_botsMasqueradeAsHumans ?? false);
		int humanReplacedByBot;
		if (playerDetails != null)
		{
			humanReplacedByBot = (playerDetails.ReplacedWithBots ? 1 : 0);
		}
		else
		{
			humanReplacedByBot = 0;
		}
		matchResultsStatline.HumanReplacedByBot = ((byte)humanReplacedByBot != 0);
		matchResultsStatline.AccountID = actorData.GetOriginalAccountId();
		int titleID;
		if (lobbyPlayerInfo != null)
		{
			titleID = lobbyPlayerInfo.TitleID;
		}
		else
		{
			titleID = 0;
		}
		matchResultsStatline.TitleID = titleID;
		matchResultsStatline.TitleLevel = (lobbyPlayerInfo?.TitleLevel ?? 0);
		matchResultsStatline.BannerID = (lobbyPlayerInfo?.BannerID ?? 0);
		int emblemID;
		if (lobbyPlayerInfo != null)
		{
			emblemID = lobbyPlayerInfo.EmblemID;
		}
		else
		{
			emblemID = 0;
		}
		matchResultsStatline.EmblemID = emblemID;
		int ribbonID;
		if (lobbyPlayerInfo != null)
		{
			ribbonID = lobbyPlayerInfo.RibbonID;
		}
		else
		{
			ribbonID = 0;
		}
		matchResultsStatline.RibbonID = ribbonID;
		AbilityData.AbilityEntry[] abilityEntries = abilityData.abilityEntries;
		
		matchResultsStatline.AbilityEntries = abilityEntries.Select(delegate(AbilityData.AbilityEntry entry)
			{
				MatchResultsStatline.AbilityEntry result4 = default(MatchResultsStatline.AbilityEntry);
				int abilityModId;
				if (entry != null)
				{
					if (entry.ability != null)
					{
						if (entry.ability.CurrentAbilityMod != null)
						{
							abilityModId = entry.ability.CurrentAbilityMod.m_abilityScopeId;
							goto IL_006e;
						}
					}
				}
				abilityModId = -1;
				goto IL_006e;
				IL_006e:
				result4.AbilityModId = abilityModId;
				return result4;
			}).ToArray();
		IEnumerable<Card> activeCards = abilityData.GetActiveCards();
		
		matchResultsStatline.CatalystHasPrepPhase = activeCards.ContainsWhere(delegate(Card card)
			{
				int result3;
				if (card != null)
				{
					result3 = ((card.GetAbilityRunPhase() == AbilityRunPhase.Prep) ? 1 : 0);
				}
				else
				{
					result3 = 0;
				}
				return (byte)result3 != 0;
			});
		matchResultsStatline.CatalystHasDashPhase = abilityData.GetActiveCards().ContainsWhere(delegate(Card card)
		{
			int result2;
			if (card != null)
			{
				result2 = ((card.GetAbilityRunPhase() == AbilityRunPhase.Dash) ? 1 : 0);
			}
			else
			{
				result2 = 0;
			}
			return (byte)result2 != 0;
		});
		matchResultsStatline.CatalystHasBlastPhase = abilityData.GetActiveCards().ContainsWhere(delegate(Card card)
		{
			int result;
			if (card != null)
			{
				result = ((card.GetAbilityRunPhase() == AbilityRunPhase.Combat) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		});
		matchResultsStatline.TotalPlayerKills = actorBehavior.totalPlayerKills;
		matchResultsStatline.TotalDeaths = actorBehavior.totalDeaths;
		matchResultsStatline.TotalPlayerDamage = actorBehavior.totalPlayerDamage;
		matchResultsStatline.TotalPlayerAssists = actorBehavior.totalPlayerAssists;
		matchResultsStatline.TotalPlayerHealingFromAbility = actorBehavior.totalPlayerHealingFromAbility;
		matchResultsStatline.TotalPlayerAbsorb = actorBehavior.totalPlayerAbsorb;
		matchResultsStatline.TotalPlayerDamageReceived = actorBehavior.totalPlayerDamageReceived;
		matchResultsStatline.TotalPlayerTurns = actorBehavior.totalPlayerTurns;
		matchResultsStatline.TotalPlayerLockInTime = actorBehavior.totalPlayerLockInTime;
		matchResultsStatline.TotalPlayerContribution = actorBehavior.totalPlayerContribution;
		return matchResultsStatline;
	}
}
