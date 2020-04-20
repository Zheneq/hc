using System;
using UnityEngine;

[Serializable]
public class OnMatchEndChatter : ScriptableObject, IChatterData
{
	public ChatterData m_baseData = new ChatterData();

	public OnMatchEndChatter.MatchResultType m_matchResult;

	public ChatterData GetCommonData()
	{
		return this.m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.MatchEnded;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			return false;
		}
		GameEventManager.MatchEndedArgs matchEndedArgs = args as GameEventManager.MatchEndedArgs;
		if (matchEndedArgs == null)
		{
			Log.Error("Missing args for Match Ended game event.", new object[0]);
			return false;
		}
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameConfig != null)
			{
				if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
				{
					return false;
				}
			}
		}
		ActorData component2 = component.gameObject.GetComponent<ActorData>();
		Team team = component2.GetTeam();
		bool flag;
		if (matchEndedArgs.result == GameResult.TeamAWon)
		{
			if (team == Team.TeamA)
			{
				flag = true;
				goto IL_E3;
			}
		}
		if (matchEndedArgs.result == GameResult.TeamBWon)
		{
			flag = (team == Team.TeamB);
		}
		else
		{
			flag = false;
		}
		IL_E3:
		bool flag2 = flag;
		if (flag2 != (this.m_matchResult == OnMatchEndChatter.MatchResultType.Victory))
		{
			return false;
		}
		return true;
	}

	public enum MatchResultType
	{
		Victory,
		Defeat
	}
}
