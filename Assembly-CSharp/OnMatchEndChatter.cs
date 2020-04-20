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
				RuntimeMethodHandle runtimeMethodHandle = methodof(OnMatchEndChatter.ShouldPlayChatter(GameEventManager.EventType, GameEventManager.GameEventArgs, ChatterComponent)).MethodHandle;
			}
			return false;
		}
		GameEventManager.MatchEndedArgs matchEndedArgs = args as GameEventManager.MatchEndedArgs;
		if (matchEndedArgs == null)
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
			Log.Error("Missing args for Match Ended game event.", new object[0]);
			return false;
		}
		if (GameManager.Get() != null)
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
			if (GameManager.Get().GameConfig != null)
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
				if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
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
					return false;
				}
			}
		}
		ActorData component2 = component.gameObject.GetComponent<ActorData>();
		Team team = component2.GetTeam();
		bool flag;
		if (matchEndedArgs.result == GameResult.TeamAWon)
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
			if (team == Team.TeamA)
			{
				flag = true;
				goto IL_E3;
			}
		}
		if (matchEndedArgs.result == GameResult.TeamBWon)
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
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
