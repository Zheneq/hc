// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;

// server-only -- missing in reactor
#if SERVER
public class ServerKnockbackStabilizer
{
	public void StabilizeKnockbacks(Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks, List<BoardSquare> additionalInvalidSquares)
	{
		bool flag = true;
		while (flag)
		{
			bool flag2 = StabilizeKnockbacksForValidDestination(incomingKnockbacks, additionalInvalidSquares);
			if (!flag2)
			{
				flag2 = StabilizeKnockbacksVsObstacles(incomingKnockbacks);
			}
			if (!flag2)
			{
				flag2 = StabilizeKnockbacksVsStationaries(incomingKnockbacks);
			}
			if (!flag2)
			{
				flag2 = StabilizeKnockbacksVsKnockbackees(incomingKnockbacks);
			}
			flag = flag2;
		}
	}

	private bool StabilizeKnockbacksForValidDestination(Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks, List<BoardSquare> additionalInvalidSquares)
	{
		bool flag = false;
		foreach (KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> knockbackEntry in incomingKnockbacks)
		{
			flag |= StabilizeKnockbackEntryForValidDestination(knockbackEntry, additionalInvalidSquares);
		}
		return flag;
	}

	private bool StabilizeKnockbackEntryForValidDestination(KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> knockbackEntry, List<BoardSquare> additionalInvalidSquares)
	{
		bool result = false;
		BoardSquarePathInfo boardSquarePathInfo = knockbackEntry.Value.GetKnockbackPath().GetPathEndpoint();
		BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
		bool flag = boardSquarePathInfo.square != null && boardSquarePathInfo.square.IsValidForGameplay() && !additionalInvalidSquares.Contains(boardSquarePathInfo.square);
		while (!flag)
		{
			if (boardSquarePathInfo.prev == null)
			{
				flag = true;
			}
			else
			{
				boardSquarePathInfo.square = null;
				boardSquarePathInfo.prev.next = null;
				boardSquarePathInfo = boardSquarePathInfo.prev;
				flag = (boardSquarePathInfo.square != null && boardSquarePathInfo.square.IsValidForGameplay());
			}
		}
		if (boardSquarePathInfo2 != boardSquarePathInfo)
		{
			knockbackEntry.Value.OnKnockbackPathStabilized(boardSquarePathInfo.square);
			result = true;
		}
		return result;
	}

	private bool StabilizeKnockbacksVsObstacles(Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks)
	{
		bool flag = false;
		foreach (KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> knockbackEntry in incomingKnockbacks)
		{
			flag |= StabilizeKnockbackEntryVsObstacles(knockbackEntry);
		}
		return flag;
	}

	private bool StabilizeKnockbackEntryVsObstacles(KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> knockbackEntry)
	{
		bool result = false;
		ActorData key = knockbackEntry.Key;
		BoardSquarePathInfo boardSquarePathInfo = knockbackEntry.Value.GetKnockbackPath();
		BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo.next;
		BoardSquarePathInfo prev = boardSquarePathInfo.prev;
		while (boardSquarePathInfo2 != null)
		{
			bool flag = (prev != null && BarrierManager.Get() != null && BarrierManager.Get().IsMovementBlockedOnCrossover(key, prev.square, boardSquarePathInfo.square)) || (BarrierManager.Get() != null && BarrierManager.Get().IsMovementBlocked(key, boardSquarePathInfo.square, boardSquarePathInfo2.square));
			if (flag)
			{
				boardSquarePathInfo.next = null;
				boardSquarePathInfo2.prev = null;
				boardSquarePathInfo2 = null;
				knockbackEntry.Value.OnKnockbackPathStabilized(boardSquarePathInfo.square);
				result = true;
			}
			else
			{
				boardSquarePathInfo = boardSquarePathInfo2;
				prev = boardSquarePathInfo2.prev;
				boardSquarePathInfo2 = boardSquarePathInfo2.next;
			}
		}
		return result;
	}

	private bool StabilizeKnockbacksVsStationaries(Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks)
	{
		bool result = false;
		foreach (ActorData actorData in GetStationaryActorsWrtKnockback(incomingKnockbacks))
		{
			if (!actorData.IsDead())
			{
				BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
				foreach (KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> keyValuePair in incomingKnockbacks)
				{
					ServerKnockbackManager.KnockbackHits value = keyValuePair.Value;
					if (value.GetKnockbackEndSquare() == currentBoardSquare)
					{
						BoardSquarePathInfo boardSquarePathInfo = value.GetKnockbackPath().BackUpOnceFromEnd();
						if (value.GetKnockbackEndSquare() != boardSquarePathInfo.square)
						{
							value.OnKnockbackPathStabilized(boardSquarePathInfo.square);
							result = true;
						}
						else
						{
							Log.Error($"Failed to back up {keyValuePair.Key} in StabilizeKnockbacksVsStationaries.");
						}
					}
				}
			}
		}
		return result;
	}

	private bool StabilizeKnockbacksVsKnockbackees(Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks)
	{
		bool flag = false;
		List<KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits>> list = incomingKnockbacks.ToList<KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits>>();
		list.Sort(delegate(KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> x, KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> y)
		{
			float num2 = x.Value.GetKnockbackEndSquare().HorizontalDistanceOnBoardTo(x.Key.GetCurrentBoardSquare());
			float value3 = y.Value.GetKnockbackEndSquare().HorizontalDistanceOnBoardTo(y.Key.GetCurrentBoardSquare());
			return num2.CompareTo(value3);
		});
		int num = 0;
		while (num < list.Count && !flag)
		{
			ServerKnockbackManager.KnockbackHits value = list[num].Value;
			for (int i = num + 1; i < list.Count; i++)
			{
				KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> keyValuePair = list[i];
				ServerKnockbackManager.KnockbackHits value2 = keyValuePair.Value;
				if (value.GetKnockbackEndSquare() == value2.GetKnockbackEndSquare())
				{
					BoardSquarePathInfo boardSquarePathInfo = value2.GetKnockbackPath().BackUpOnceFromEnd();
					if (boardSquarePathInfo.square != value2.GetKnockbackEndSquare())
					{
						value2.OnKnockbackPathStabilized(boardSquarePathInfo.square);
						flag = true;
					}
					else
					{
						Log.Error($"Failed to back up {keyValuePair.Key} in StabilizeKnockbacksVsKnockbackees.");
					}
				}
			}
			num++;
		}
		return flag;
	}

	private List<ActorData> GetStationaryActorsWrtKnockback(Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks)
	{
		List<ActorData> list = new List<ActorData>(GameFlowData.Get().GetActors());
		foreach (ActorData item in incomingKnockbacks.Keys)
		{
			if (list.Contains(item))
			{
				list.Remove(item);
			}
		}
		return list;
	}
}
#endif
