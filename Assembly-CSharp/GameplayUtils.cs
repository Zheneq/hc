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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameObject.FindInChildren(string, int)).MethodHandle;
			}
			int num;
			if (Camera.main == null)
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
		if (gameObject == null)
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
			if (list.Count > 0)
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
				return list[0];
			}
		}
		return gameObject;
	}

	private static GameObject FindInChildrenRecursive(this GameObject obj, string name, int layerBitMask, bool ignoreRootJnt, List<GameObject> potentialRetVals)
	{
		if (obj.name == name)
		{
			if ((1 << obj.layer & layerBitMask) != 0)
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
		bool flag2;
		if (!ignoreRootJnt)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameObject.FindInChildrenRecursive(string, int, bool, List<GameObject>)).MethodHandle;
			}
			flag2 = flag;
		}
		else
		{
			flag2 = true;
		}
		bool ignoreRootJnt2 = flag2;
		IEnumerator enumerator = obj.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj2 = enumerator.Current;
				Transform transform = (Transform)obj2;
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				disposable.Dispose();
			}
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
				object obj2 = enumerator.Current;
				Transform transform = (Transform)obj2;
				transform.gameObject.SetLayerRecursively(layer);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameObject.SetLayerRecursively(LayerMask)).MethodHandle;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
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
				disposable.Dispose();
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayUtils.IsBot(ActorData)).MethodHandle;
			}
			result = (actor.GetComponent<BotController>() != null);
		}
		return result;
	}

	public static bool IsBot(MonoBehaviour entity)
	{
		return GameplayUtils.IsBot(entity.gameObject);
	}

	public static bool IsBot(GameObject obj)
	{
		ActorData component = obj.GetComponent<ActorData>();
		return GameplayUtils.IsBot(component);
	}

	public static bool IsPlayerControlled(ActorData actor)
	{
		bool result = false;
		if (actor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayUtils.IsPlayerControlled(ActorData)).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayUtils.IsPlayerControlled(MonoBehaviour)).MethodHandle;
			}
			result = (component.PlayerIndex != PlayerData.s_invalidPlayerIndex);
		}
		return result;
	}

	public static bool IsPlayerControlled(GameObject obj)
	{
		ActorData component = obj.GetComponent<ActorData>();
		return GameplayUtils.IsPlayerControlled(component);
	}

	public static bool IsHumanControlled(ActorData actor)
	{
		bool result = false;
		if (actor != null)
		{
			result = actor.GetIsHumanControlled();
		}
		return result;
	}

	public static bool IsHumanControlled(MonoBehaviour entity)
	{
		return GameplayUtils.IsHumanControlled(entity.gameObject);
	}

	public static bool IsHumanControlled(GameObject obj)
	{
		ActorData component = obj.GetComponent<ActorData>();
		return GameplayUtils.IsHumanControlled(component);
	}

	public static bool IsValidPlayer(ActorData actor)
	{
		bool result;
		if (actor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayUtils.IsValidPlayer(ActorData)).MethodHandle;
			}
			result = (actor.PlayerIndex != PlayerData.s_invalidPlayerIndex);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public static List<Team> GetOtherTeamsThan(Team team)
	{
		List<Team> list = new List<Team>();
		if (team != Team.TeamA)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayUtils.GetOtherTeamsThan(Team)).MethodHandle;
			}
			list.Add(Team.TeamA);
		}
		if (team != Team.TeamB)
		{
			list.Add(Team.TeamB);
		}
		if (team != Team.Objects)
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
				ActorData actorData = enumerator.Current;
				if (GameplayUtils.IsPlayerControlled(actorData))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayUtils.GetTotalTeamDeaths(Team)).MethodHandle;
					}
					ActorBehavior actorBehavior = actorData.GetActorBehavior();
					num += actorBehavior.totalDeaths;
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return num;
	}

	public static int GetTotalTeamCredits(Team team)
	{
		int num = 0;
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(team);
		using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (GameplayUtils.IsPlayerControlled(actorData))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayUtils.GetTotalTeamCredits(Team)).MethodHandle;
					}
					ItemData component = actorData.GetComponent<ItemData>();
					num += component.GetNetWorth();
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
		return num;
	}

	public static int GetScoreForVariableType(ActorData actor, TargeterUtils.VariableType variableType)
	{
		int result;
		switch (variableType)
		{
		case TargeterUtils.VariableType.MechanicPoints:
			result = actor.MechanicPoints;
			break;
		case TargeterUtils.VariableType.Energy:
			result = actor.TechPoints;
			break;
		case TargeterUtils.VariableType.HitPoints:
			result = actor.HitPoints;
			break;
		default:
			result = 0;
			break;
		}
		return result;
	}

	public static int GetActorIndexOfActor(ActorData actor)
	{
		int result;
		if (actor == null)
		{
			result = ActorData.s_invalidActorIndex;
		}
		else
		{
			result = actor.ActorIndex;
		}
		return result;
	}

	public static ActorData GetActorOfActorIndex(int actorIndex)
	{
		ActorData result;
		if (actorIndex == ActorData.s_invalidActorIndex)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayUtils.GetActorOfActorIndex(int)).MethodHandle;
			}
			result = null;
		}
		else
		{
			result = GameFlowData.Get().FindActorByActorIndex(actorIndex);
		}
		return result;
	}

	public static MatchResultsStats GenerateStatsFromGame(Team perspectiveTeam, int perspectivePlayerId)
	{
		Team team = (perspectiveTeam != Team.TeamB) ? Team.TeamA : Team.TeamB;
		Team team2;
		if (perspectiveTeam == Team.TeamB)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayUtils.GenerateStatsFromGame(Team, int)).MethodHandle;
			}
			team2 = Team.TeamA;
		}
		else
		{
			team2 = Team.TeamB;
		}
		Team team3 = team2;
		MatchResultsStats matchResultsStats = new MatchResultsStats();
		matchResultsStats.FriendlyStatlines = (from actorData in GameFlowData.Get().GetPlayerAndBotTeamMembers(team)
		select GameplayUtils.GenerateStatlineFromGame(actorData, perspectiveTeam, perspectivePlayerId)).ToArray<MatchResultsStatline>();
		matchResultsStats.EnemyStatlines = (from actorData in GameFlowData.Get().GetPlayerAndBotTeamMembers(team3)
		select GameplayUtils.GenerateStatlineFromGame(actorData, perspectiveTeam, perspectivePlayerId)).ToArray<MatchResultsStatline>();
		MatchResultsStats matchResultsStats2 = matchResultsStats;
		int redScore;
		if (ObjectivePoints.Get() != null)
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
			redScore = ObjectivePoints.Get().GetPointsForTeam(Team.TeamA);
		}
		else
		{
			redScore = 0;
		}
		matchResultsStats2.RedScore = redScore;
		MatchResultsStats matchResultsStats3 = matchResultsStats;
		int blueScore;
		if (ObjectivePoints.Get() != null)
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
			blueScore = ObjectivePoints.Get().GetPointsForTeam(Team.TeamB);
		}
		else
		{
			blueScore = 0;
		}
		matchResultsStats3.BlueScore = blueScore;
		matchResultsStats.GameTime = GameFlowData.Get().GetGameTime();
		MatchResultsStats matchResultsStats4 = matchResultsStats;
		string victoryCondition;
		if (ObjectivePoints.Get() != null)
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
			victoryCondition = ObjectivePoints.Get().m_victoryCondition;
		}
		else
		{
			victoryCondition = "TurnsLeft@GameModes";
		}
		matchResultsStats4.VictoryCondition = victoryCondition;
		MatchResultsStats matchResultsStats5 = matchResultsStats;
		int victoryConditionTurns;
		if (ObjectivePoints.Get() != null)
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
			victoryConditionTurns = ObjectivePoints.Get().m_timeLimitTurns;
		}
		else
		{
			victoryConditionTurns = 0x14;
		}
		matchResultsStats5.VictoryConditionTurns = victoryConditionTurns;
		return matchResultsStats;
	}

	public static MatchResultsStatline GenerateStatlineFromGame(ActorData actorData, Team perspectiveTeam, int perspectivePlayerId)
	{
		PlayerDetails playerDetails = null;
		if (actorData.PlayerData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayUtils.GenerateStatlineFromGame(ActorData, Team, int)).MethodHandle;
			}
			playerDetails = GameFlow.Get().playerDetails.TryGetValue(actorData.PlayerData.GetPlayer());
		}
		int lobbyPlayerInfoId = -1;
		if (playerDetails != null)
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
			lobbyPlayerInfoId = playerDetails.m_lobbyPlayerInfoId;
		}
		LobbyTeamInfo teamInfo = GameManager.Get().TeamInfo;
		LobbyPlayerInfo lobbyPlayerInfo = teamInfo.TeamPlayerInfo.FirstOrDefault((LobbyPlayerInfo element) => element.PlayerId == lobbyPlayerInfoId);
		ActorBehavior actorBehavior = actorData.GetActorBehavior();
		AbilityData abilityData = actorData.GetAbilityData();
		MatchResultsStatline matchResultsStatline = new MatchResultsStatline();
		matchResultsStatline.Actor = actorData;
		matchResultsStatline.PlayerId = lobbyPlayerInfoId;
		matchResultsStatline.DisplayName = actorData.GetFancyDisplayName();
		matchResultsStatline.Character = actorData.m_characterType;
		matchResultsStatline.IsPerspective = (lobbyPlayerInfoId == perspectivePlayerId);
		matchResultsStatline.IsAlly = (perspectiveTeam == actorData.GetTeam());
		MatchResultsStatline matchResultsStatline2 = matchResultsStatline;
		bool isHumanControlled;
		if (playerDetails != null)
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
			isHumanControlled = playerDetails.IsHumanControlled;
		}
		else
		{
			isHumanControlled = false;
		}
		matchResultsStatline2.IsHumanControlled = isHumanControlled;
		matchResultsStatline.IsBotMasqueradingAsHuman = (playerDetails != null && playerDetails.m_botsMasqueradeAsHumans);
		MatchResultsStatline matchResultsStatline3 = matchResultsStatline;
		bool humanReplacedByBot;
		if (playerDetails != null)
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
			humanReplacedByBot = playerDetails.ReplacedWithBots;
		}
		else
		{
			humanReplacedByBot = false;
		}
		matchResultsStatline3.HumanReplacedByBot = humanReplacedByBot;
		matchResultsStatline.AccountID = actorData.GetAccountIdWithSomeConditionB_zq();
		MatchResultsStatline matchResultsStatline4 = matchResultsStatline;
		int titleID;
		if (lobbyPlayerInfo != null)
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
			titleID = lobbyPlayerInfo.TitleID;
		}
		else
		{
			titleID = 0;
		}
		matchResultsStatline4.TitleID = titleID;
		matchResultsStatline.TitleLevel = ((lobbyPlayerInfo == null) ? 0 : lobbyPlayerInfo.TitleLevel);
		matchResultsStatline.BannerID = ((lobbyPlayerInfo == null) ? 0 : lobbyPlayerInfo.BannerID);
		MatchResultsStatline matchResultsStatline5 = matchResultsStatline;
		int emblemID;
		if (lobbyPlayerInfo != null)
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
			emblemID = lobbyPlayerInfo.EmblemID;
		}
		else
		{
			emblemID = 0;
		}
		matchResultsStatline5.EmblemID = emblemID;
		MatchResultsStatline matchResultsStatline6 = matchResultsStatline;
		int ribbonID;
		if (lobbyPlayerInfo != null)
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
			ribbonID = lobbyPlayerInfo.RibbonID;
		}
		else
		{
			ribbonID = 0;
		}
		matchResultsStatline6.RibbonID = ribbonID;
		MatchResultsStatline matchResultsStatline7 = matchResultsStatline;
		IEnumerable<AbilityData.AbilityEntry> abilityEntries = abilityData.abilityEntries;
		if (GameplayUtils.<>f__am$cache0 == null)
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
			GameplayUtils.<>f__am$cache0 = delegate(AbilityData.AbilityEntry entry)
			{
				MatchResultsStatline.AbilityEntry result = default(MatchResultsStatline.AbilityEntry);
				int abilityModId;
				if (entry != null)
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
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(GameplayUtils.<GenerateStatlineFromGame>m__0(AbilityData.AbilityEntry)).MethodHandle;
					}
					if (entry.ability != null)
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
						if (entry.ability.CurrentAbilityMod != null)
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
							abilityModId = entry.ability.CurrentAbilityMod.m_abilityScopeId;
							goto IL_6E;
						}
					}
				}
				abilityModId = -1;
				IL_6E:
				result.AbilityModId = abilityModId;
				return result;
			};
		}
		matchResultsStatline7.AbilityEntries = abilityEntries.Select(GameplayUtils.<>f__am$cache0).ToArray<MatchResultsStatline.AbilityEntry>();
		MatchResultsStatline matchResultsStatline8 = matchResultsStatline;
		IEnumerable<Card> activeCards = abilityData.GetActiveCards();
		if (GameplayUtils.<>f__am$cache1 == null)
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
			GameplayUtils.<>f__am$cache1 = delegate(Card card)
			{
				bool result;
				if (card != null)
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
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(GameplayUtils.<GenerateStatlineFromGame>m__1(Card)).MethodHandle;
					}
					result = (card.GetAbilityRunPhase() == AbilityRunPhase.Prep);
				}
				else
				{
					result = false;
				}
				return result;
			};
		}
		matchResultsStatline8.CatalystHasPrepPhase = activeCards.ContainsWhere(GameplayUtils.<>f__am$cache1);
		matchResultsStatline.CatalystHasDashPhase = abilityData.GetActiveCards().ContainsWhere(delegate(Card card)
		{
			bool result;
			if (card != null)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(GameplayUtils.<GenerateStatlineFromGame>m__2(Card)).MethodHandle;
				}
				result = (card.GetAbilityRunPhase() == AbilityRunPhase.Dash);
			}
			else
			{
				result = false;
			}
			return result;
		});
		matchResultsStatline.CatalystHasBlastPhase = abilityData.GetActiveCards().ContainsWhere(delegate(Card card)
		{
			bool result;
			if (card != null)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(GameplayUtils.<GenerateStatlineFromGame>m__3(Card)).MethodHandle;
				}
				result = (card.GetAbilityRunPhase() == AbilityRunPhase.Combat);
			}
			else
			{
				result = false;
			}
			return result;
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
