using System;
using UnityEngine;

[Serializable]
public class OnMatchEndChatter : ScriptableObject, IChatterData
{
	public enum MatchResultType
	{
		Victory,
		Defeat
	}

	public ChatterData m_baseData = new ChatterData();

	public MatchResultType m_matchResult;

	public ChatterData GetCommonData()
	{
		return m_baseData;
	}

	public GameEventManager.EventType GetActivateOnEvent()
	{
		return GameEventManager.EventType.MatchEnded;
	}

	public bool ShouldPlayChatter(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args, ChatterComponent component)
	{
		if (!ChatterData.ShouldPlayChatter(this, eventType, args, component))
		{
			while (true)
			{
				return false;
			}
		}
		GameEventManager.MatchEndedArgs matchEndedArgs = args as GameEventManager.MatchEndedArgs;
		if (matchEndedArgs == null)
		{
			while (true)
			{
				Log.Error("Missing args for Match Ended game event.");
				return false;
			}
		}
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameConfig != null)
			{
				if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
			}
		}
		ActorData component2 = component.gameObject.GetComponent<ActorData>();
		Team team = component2.GetTeam();
		int num;
		if (matchEndedArgs.result == GameResult.TeamAWon)
		{
			if (team == Team.TeamA)
			{
				num = 1;
				goto IL_00e3;
			}
		}
		if (matchEndedArgs.result == GameResult.TeamBWon)
		{
			num = ((team == Team.TeamB) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		goto IL_00e3;
		IL_00e3:
		bool flag = (byte)num != 0;
		if (flag != (m_matchResult == MatchResultType.Victory))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return true;
	}
}
