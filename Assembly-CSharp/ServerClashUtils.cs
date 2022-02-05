// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

// server-only
#if SERVER
public static class ServerClashUtils
{
	private static List<BoardSquare> GetPotentialBumpDestinationSquaresWithDistanceFromSquare(BoardSquare center, float distance, List<BoardSquare> reservedSquares, List<ClashResolutionEntry> clashResolutions)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (center == null)
		{
			return list;
		}
		if (distance == 0f)
		{
			list.Add(center);
			return list;
		}
		int num = Mathf.FloorToInt(distance);
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				BoardSquare squareFromIndex = Board.Get().GetSquareFromIndex(center.x + i, center.y + j);
				if (squareFromIndex != null && AreMoveCostsEqual(center.HorizontalDistanceOnBoardTo(squareFromIndex), distance, false) && squareFromIndex.IsValidForGameplay() && (reservedSquares == null || !reservedSquares.Contains(squareFromIndex)))
				{
					bool flag = false;
					bool flag2 = false;
					BoardSquarePathInfo boardSquarePathInfo = null;
					for (int k = 0; k < clashResolutions.Count; k++)
					{
						BoardSquarePathInfo boardSquarePathInfo2 = clashResolutions[k].Actor.GetActorMovement().BuildCompletePathTo(center, squareFromIndex, true, null);
						if (k == 0)
						{
							boardSquarePathInfo = boardSquarePathInfo2;
							if (boardSquarePathInfo == null)
							{
								flag = true;
								break;
							}
						}
						else
						{
							if (boardSquarePathInfo2 == null)
							{
								flag2 = true;
								break;
							}
							if (!AreMoveCostsEqual(boardSquarePathInfo2.FindMoveCostToEnd(), boardSquarePathInfo.FindMoveCostToEnd(), false))
							{
								flag2 = true;
								break;
							}
						}
					}
					if (!flag && !flag2)
					{
						list.Add(squareFromIndex);
					}
				}
			}
		}
		return list;
	}

	private static bool IsSquareAlreadyClaimedByResolutionEntry(List<ClashResolutionEntry> resolvedEntries, BoardSquare square)
	{
		bool result;
		if (resolvedEntries == null)
		{
			result = false;
		}
		else if (square == null)
		{
			result = false;
		}
		else
		{
			result = false;
			foreach (ClashResolutionEntry entry in resolvedEntries)
			{
				if (entry.m_newDestSquare == square)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	public static void ResolveClashMovement(List<MovementRequest> storedMovementRequests, MovementClashCollection clashCollection, bool forChaseMovement)
	{
		if (clashCollection == null || clashCollection.m_clashes == null || clashCollection.m_clashes.Count == 0)
		{
			return;
		}
		clashCollection.SortClashes();
		List<BoardSquare> list = new List<BoardSquare>();
		foreach (ActorData actorData in ServerActionBuffer.Get().GetStationaryActors())
		{
			list.Add(actorData.CurrentBoardSquare);
		}
		List<ActorData> list2 = new List<ActorData>();
		foreach (ActorData actorData2 in GameFlowData.Get().GetActors())
		{
			if (!actorData2.IsDead() && actorData2.GetComponent<Passive_TricksterAfterImage>() != null)
			{
				list2.Add(actorData2);
			}
		}
		foreach (ActorData actorData3 in list2)
		{
			if (!list.Contains(actorData3.CurrentBoardSquare))
			{
				list.Add(actorData3.CurrentBoardSquare);
			}
		}
		foreach (MovementRequest movementRequest in storedMovementRequests)
		{
			if (movementRequest.WasEverChasing() && !forChaseMovement)
			{
				if (!list.Contains(movementRequest.m_actor.CurrentBoardSquare))
				{
					list.Add(movementRequest.m_actor.CurrentBoardSquare);
				}
			}
			else if (!list.Contains(movementRequest.m_targetSquare))
			{
				list.Add(movementRequest.m_targetSquare);
			}
		}
		foreach (MovementClash movementClash in clashCollection.m_clashes)
		{
			if (!movementClash.m_continuing && movementClash.m_chase == forChaseMovement)
			{
				int num = movementClash.m_teamAClashers.Count + movementClash.m_teamBClashers.Count;
				BoardSquare clashSquare = movementClash.m_clashSquare;
				int num2 = 0;
				int num3 = 0;
				List<BoardSquare> list3 = null;
				List<ClashResolutionEntry> list4 = new List<ClashResolutionEntry>(num);
				foreach (MovementClashParticipant clashParticipant in movementClash.m_teamAClashers)
				{
					list4.Add(new ClashResolutionEntry(clashParticipant));
				}
				using (List<MovementClashParticipant>.Enumerator enumerator4 = movementClash.m_teamBClashers.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						MovementClashParticipant clashParticipant2 = enumerator4.Current;
						list4.Add(new ClashResolutionEntry(clashParticipant2));
					}
				}
				if (false)  // TODO LOW unreachable code
                {
					float distance2 = 1f + 0.5f * (float)num2;
					list3 = GetPotentialBumpDestinationSquaresWithDistanceFromSquare(clashSquare, distance2, list, list4);
					num3 = list3.Count;
					num2++;
				}
				if (num2 > 6 || num3 >= num)
				{
					if (num3 < num)
					{
						num2 = 0;
						num3 = 0;
						list3 = new List<BoardSquare>();
						while (num2 <= 12 && num3 < num)
						{
							float distance = 1f + 0.5f * (float)num2;
							List<BoardSquare> potentialBumpDestinationSquaresWithDistanceFromSquare = GetPotentialBumpDestinationSquaresWithDistanceFromSquare(clashSquare, distance, list, list4);
							for (int i = 0; i < potentialBumpDestinationSquaresWithDistanceFromSquare.Count; i++)
							{
								list3.Add(potentialBumpDestinationSquaresWithDistanceFromSquare[i]);
							}
							num3 = list3.Count;
							num2++;
						}
					}
					List<ClashResolutionEntry> list5 = new List<ClashResolutionEntry>();
					while (list5.Count < list4.Count)
					{
						ClashResolutionEntry clashResolutionEntry = null;
						float num4 = 0f;
						int index = -1;
						foreach (ClashResolutionEntry clashResolutionEntry2 in list4)
						{
							if (!list5.Contains(clashResolutionEntry2))
							{
								for (int j = 0; j < list3.Count; j++)
								{
									BoardSquare boardSquare = list3[j];
									if (!IsSquareAlreadyClaimedByResolutionEntry(list5, boardSquare))
									{
										BoardSquarePathInfo boardSquarePathInfo = clashResolutionEntry2.Actor.GetActorMovement().BuildPathTo_IgnoreBarriers(clashSquare, boardSquare);
										if (boardSquarePathInfo == null)
										{
											Debug.LogError(string.Format("While trying to stabilize clashes, failed to build a path from {0} to {1} for actor {2}", BoardSquare.DebugString(clashSquare, false), BoardSquare.DebugString(boardSquare, false), clashResolutionEntry2.Actor.DebugNameString()));
										}
										else
										{
											float num5 = boardSquarePathInfo.FindDistanceToEnd();
											int num6 = 0;
											float num7 = 0f;
											BoardSquarePathInfo prev = clashResolutionEntry2.m_clashParticipant.OriginalPath.GetPathEndpoint().prev;
											BoardSquarePathInfo next = boardSquarePathInfo.next;
											while (prev != null && next != null)
											{
												if (prev.square == next.square)
												{
													num6++;
													prev = prev.prev;
													next = next.next;
												}
												else
												{
													if (Board.Get().GetSquaresAreCardinallyAdjacent(prev.square, next.square))
													{
														num7 = 0.2f;
														break;
													}
													if (Board.Get().GetSquaresAreDiagonallyAdjacent(prev.square, next.square))
													{
														num7 = 0.1f;
														break;
													}
													num7 = 0f;
													break;
												}
											}
											float num8 = num5 * 10f - (float)num6 - num7;
											if (clashResolutionEntry == null || num8 < num4)
											{
												num4 = num8;
												clashResolutionEntry = clashResolutionEntry2;
												index = j;
											}
										}
									}
								}
							}
						}
						clashResolutionEntry.m_newDestSquare = list3[index];
						list5.Add(clashResolutionEntry);
					}
					foreach (ClashResolutionEntry clashResolutionEntry3 in list4)
					{
						BoardSquare newDestSquare = clashResolutionEntry3.m_newDestSquare;
						clashResolutionEntry3.BumpToNewDestinationSquare(newDestSquare);
						list.Add(newDestSquare);
					}
					continue;
				}
			}
		}
	}

	public static bool AreMoveCostsEqual(float cost1, float cost2, bool infiniteCostsConsideredEqual = false)
	{
		bool flag = cost1 == cost2 || Mathf.Abs(cost1 - cost2) < 0.1f;
		return flag && ((cost1 != float.MaxValue && cost2 != float.MaxValue) || infiniteCostsConsideredEqual);
	}

	public static MovementClashCollection IdentifyClashSegments_Movement(List<MovementRequest> stabilizedMovementRequests, bool forChaseMovement)
	{
		MovementClashCollection movementClashCollection = new MovementClashCollection();
		List<MovementRequest> list = new List<MovementRequest>();
		for (int i = 0; i < stabilizedMovementRequests.Count; i++)
		{
			MovementRequest movementRequest = stabilizedMovementRequests[i];
			if (forChaseMovement == movementRequest.WasEverChasing())
			{
				movementRequest.m_path.ResetClashingOfPath();
				movementRequest.m_path.CalcAndSetMoveCostToEnd();
				list.Add(movementRequest);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			for (int k = j + 1; k < list.Count; k++)
			{
				MovementRequest movementRequest2 = list[j];
				MovementRequest movementRequest3 = list[k];
				if (movementRequest2.m_actor.GetTeam() != movementRequest3.m_actor.GetTeam())
				{
					BoardSquarePathInfo boardSquarePathInfo = movementRequest2.m_path;
					BoardSquarePathInfo boardSquarePathInfo2 = movementRequest3.m_path;
					while (boardSquarePathInfo != null && boardSquarePathInfo2 != null)
					{
						if (boardSquarePathInfo.square == boardSquarePathInfo2.square && AreMoveCostsEqual(boardSquarePathInfo.moveCost, boardSquarePathInfo2.moveCost, false) && boardSquarePathInfo.IsPathEndpoint() == boardSquarePathInfo2.IsPathEndpoint())
						{
							boardSquarePathInfo.m_moverClashesHere = true;
							boardSquarePathInfo2.m_moverClashesHere = true;
							movementClashCollection.AddClash(movementRequest2, boardSquarePathInfo, movementRequest3, boardSquarePathInfo2);
						}
						if (boardSquarePathInfo.moveCost == boardSquarePathInfo2.moveCost)
						{
							boardSquarePathInfo = boardSquarePathInfo.next;
							boardSquarePathInfo2 = boardSquarePathInfo2.next;
						}
						else if (boardSquarePathInfo.moveCost > boardSquarePathInfo2.moveCost)
						{
							boardSquarePathInfo2 = boardSquarePathInfo2.next;
						}
						else
						{
							boardSquarePathInfo = boardSquarePathInfo.next;
						}
					}
				}
			}
		}
		return movementClashCollection;
	}

	public static MovementClashCollection IdentifyClashSegments_Evade(List<ServerEvadeUtils.EvadeInfo> evades)
	{
		MovementClashCollection movementClashCollection = new MovementClashCollection();
		for (int i = 0; i < evades.Count; i++)
		{
			evades[i].m_evadePath.ResetClashingOfPath();
			evades[i].m_evadePath.CalcAndSetMoveCostToEnd();
		}
		for (int j = 0; j < evades.Count; j++)
		{
			for (int k = j + 1; k < evades.Count; k++)
			{
				ServerEvadeUtils.EvadeInfo evadeInfo = evades[j];
				ServerEvadeUtils.EvadeInfo evadeInfo2 = evades[k];
				if (evadeInfo.GetMover().GetTeam() != evadeInfo2.GetMover().GetTeam() && !evadeInfo.IsDestinationReserved() && !evadeInfo2.IsDestinationReserved())
				{
					BoardSquarePathInfo boardSquarePathInfo = evadeInfo.m_evadePath;
					BoardSquarePathInfo boardSquarePathInfo2 = evadeInfo2.m_evadePath;
					while (boardSquarePathInfo != null && boardSquarePathInfo2 != null)
					{
						if (boardSquarePathInfo.square == boardSquarePathInfo2.square && AreMoveCostsEqual(boardSquarePathInfo.moveCost, boardSquarePathInfo2.moveCost, false) && boardSquarePathInfo.IsPathEndpoint() == boardSquarePathInfo2.IsPathEndpoint())
						{
							boardSquarePathInfo.m_moverClashesHere = true;
							boardSquarePathInfo2.m_moverClashesHere = true;
							movementClashCollection.AddClash(evadeInfo, boardSquarePathInfo, evadeInfo2, boardSquarePathInfo2);
						}
						if (boardSquarePathInfo.moveCost == boardSquarePathInfo2.moveCost)
						{
							boardSquarePathInfo = boardSquarePathInfo.next;
							boardSquarePathInfo2 = boardSquarePathInfo2.next;
						}
						else if (boardSquarePathInfo.moveCost > boardSquarePathInfo2.moveCost)
						{
							boardSquarePathInfo2 = boardSquarePathInfo2.next;
						}
						else
						{
							boardSquarePathInfo = boardSquarePathInfo.next;
						}
					}
				}
			}
		}
		return movementClashCollection;
	}

	private class ClashResolutionEntry
	{
		public MovementClashParticipant m_clashParticipant;

		public BoardSquare m_newDestSquare;

		public ClashResolutionEntry(MovementClashParticipant clashParticipant)
		{
			m_clashParticipant = clashParticipant;
		}

		public ActorData Actor
		{
			get
			{
				return m_clashParticipant.Actor;
			}
		}

		public BoardSquarePathInfo OriginalPath
		{
			get
			{
				return m_clashParticipant.OriginalPath;
			}
		}

		public void BumpToNewDestinationSquare(BoardSquare newDest)
		{
			m_clashParticipant.BumpToNewDestinationSquare(newDest);
		}
	}

	public class MovementClashParticipant
	{
		private MovementRequest m_request;
		private ServerEvadeUtils.EvadeInfo m_evade;
		public BoardSquarePathInfo m_segmentOnPath;

		public MovementClashParticipant(MovementRequest request, BoardSquarePathInfo segmentOnPath)
		{
			m_request = request;
			m_segmentOnPath = segmentOnPath;
		}

		public MovementClashParticipant(ServerEvadeUtils.EvadeInfo evade, BoardSquarePathInfo segmentOnPath)
		{
			m_evade = evade;
			m_segmentOnPath = segmentOnPath;
		}

		public ActorData Actor
		{
			get
			{
				if (m_request != null)
				{
					return m_request.m_actor;
				}
				return m_evade.GetMover();
			}
		}

		public bool WasEverChasing
		{
			get
			{
				return m_request != null && m_request.WasEverChasing();
			}
		}

		public BoardSquarePathInfo OriginalPath
		{
			get
			{
				if (m_request != null)
				{
					return m_request.m_path;
				}
				return m_evade.m_evadePath;
			}
		}

		public void BumpToNewDestinationSquare(BoardSquare newDest)
		{
			if (newDest == null)
			{
				Log.Error("{act} MoveStabilizationErr: Trying to bump to null square");
				return;
			}
			BoardSquarePathInfo pathEndpoint = OriginalPath.GetPathEndpoint();
			BoardSquare square = pathEndpoint.square;
			BoardSquarePathInfo boardSquarePathInfo = Actor.GetActorMovement().BuildPathTo_IgnoreBarriers(square, newDest);
			if (boardSquarePathInfo == null)
			{
				Log.Error("{act} MoveStabilizationErr: Failed to build a bump path.  Movement is now untrustworthy.");
				return;
			}
			BoardSquarePathInfo next = boardSquarePathInfo.next;
			next.prev = pathEndpoint;
			pathEndpoint.next = next;
			pathEndpoint.m_moverClashesHere = true;
			while (next != null)
			{
				next.m_moverBumpedFromClash = true;
				next = next.next;
			}
			OriginalPath.CalcAndSetMoveCostToEnd();
			if (m_request != null)
			{
				m_request.m_targetSquare = newDest;
				return;
			}
			if (m_evade != null)
			{
				m_evade.ModifyDestination(newDest);
			}
		}
	}

	public class MovementClash : IComparable
	{
		public List<MovementClashParticipant> m_teamAClashers;
		public List<MovementClashParticipant> m_teamBClashers;
		public bool m_chase;
		public BoardSquare m_clashSquare;
		public float m_moveCost;
		public bool m_continuing;

		public MovementClash(MovementClashParticipant first, MovementClashParticipant second)
		{
			if (first.Actor.GetTeam() == second.Actor.GetTeam())
			{
				Debug.LogError("Trying to create a movement clash, but the participants are on the same team.");
			}
			if (first.WasEverChasing != second.WasEverChasing)
			{
				Debug.LogError("Trying to create a movement clash, but the participants have inequal chase-ness.");
			}
			if (first.m_segmentOnPath.square != second.m_segmentOnPath.square)
			{
				Debug.LogError("Trying to create a movement clash, but the participants aren't on the same square.");
			}
			if (first.m_segmentOnPath.IsPathEndpoint() != second.m_segmentOnPath.IsPathEndpoint())
			{
				Debug.LogError("Trying to create a movement clash, but the participants have inequal path-ending-ness.");
			}
			if (first.m_segmentOnPath.moveCost != second.m_segmentOnPath.moveCost)
			{
				Debug.LogError("Trying to create a movement clash, but the participants have inequal move-costs-so-far.");
			}
			m_chase = first.WasEverChasing;
			m_clashSquare = first.m_segmentOnPath.square;
			m_continuing = !first.m_segmentOnPath.IsPathEndpoint();
			m_moveCost = first.m_segmentOnPath.moveCost;
			m_teamAClashers = new List<MovementClashParticipant>();
			m_teamBClashers = new List<MovementClashParticipant>();
			AddClashers(first, second);
		}

		public void AddClashers(MovementClashParticipant first, MovementClashParticipant second)
		{
			if (first.Actor.GetTeam() == Team.TeamA)
			{
				m_teamAClashers.Add(first);
			}
			else if (first.Actor.GetTeam() == Team.TeamB)
			{
				m_teamBClashers.Add(first);
			}
			else
			{
				Debug.LogError("Trying to create a movement clash, but the first participant is not on Team A or Team B.");
			}
			if (second.Actor.GetTeam() == Team.TeamA)
			{
				m_teamAClashers.Add(second);
				return;
			}
			if (second.Actor.GetTeam() == Team.TeamB)
			{
				m_teamBClashers.Add(second);
				return;
			}
			Debug.LogError("Trying to create a movement clash, but the second participant is not on Team A or Team B.");
		}

		public List<MovementClashParticipant> GetParticipantsOfTeam(Team team)
		{
			if (team == Team.TeamA)
			{
				return m_teamAClashers;
			}
			if (team == Team.TeamB)
			{
				return m_teamBClashers;
			}
			return null;
		}

		public List<MovementClashParticipant> GetAllParticipants()
		{
			List<MovementClashParticipant> list = new List<MovementClashParticipant>(m_teamAClashers.Count + m_teamBClashers.Count);
			list.AddRange(m_teamAClashers);
			list.AddRange(m_teamBClashers);
			return list;
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			MovementClash movementClash = obj as MovementClash;
			if (movementClash == null)
			{
				throw new ArgumentException("Object is not a MovementClash");
			}
			if (m_chase != movementClash.m_chase)
			{
				return m_chase.CompareTo(movementClash.m_chase);
			}
			if (m_moveCost != movementClash.m_moveCost)
			{
				return m_moveCost.CompareTo(movementClash.m_moveCost);
			}
			if (m_continuing != movementClash.m_continuing)
			{
				return m_continuing.CompareTo(movementClash.m_continuing);
			}
			return 0;
		}
	}

	public class MovementClashCollection
	{
		public List<MovementClash> m_clashes;

		public MovementClashCollection()
		{
			m_clashes = new List<MovementClash>();
		}

		public void AddClash(MovementRequest firstRequest, BoardSquarePathInfo firstSegment, MovementRequest secondRequest, BoardSquarePathInfo secondSegment)
		{
			MovementClashParticipant first = new MovementClashParticipant(firstRequest, firstSegment);
			MovementClashParticipant second = new MovementClashParticipant(secondRequest, secondSegment);
			MovementClash movementClash = FindExistingClashFor(firstRequest.WasEverChasing(), firstSegment.square, firstSegment.moveCost, !firstSegment.IsPathEndpoint());
			if (movementClash == null)
			{
				MovementClash item = new MovementClash(first, second);
				m_clashes.Add(item);
				return;
			}
			movementClash.AddClashers(first, second);
		}

		public void AddClash(ServerEvadeUtils.EvadeInfo firstEvade, BoardSquarePathInfo firstSegment, ServerEvadeUtils.EvadeInfo secondEvade, BoardSquarePathInfo secondSegment)
		{
			MovementClashParticipant first = new MovementClashParticipant(firstEvade, firstSegment);
			MovementClashParticipant second = new MovementClashParticipant(secondEvade, secondSegment);
			MovementClash movementClash = FindExistingClashFor(false, firstSegment.square, firstSegment.moveCost, !firstSegment.IsPathEndpoint());
			if (movementClash == null)
			{
				MovementClash item = new MovementClash(first, second);
				m_clashes.Add(item);
				return;
			}
			movementClash.AddClashers(first, second);
		}

		public MovementClash FindExistingClashFor(bool chase, BoardSquare clashSquare, float moveCost, bool continuing)
		{
			if (m_clashes == null || m_clashes.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < m_clashes.Count; i++)
			{
				MovementClash movementClash = m_clashes[i];
				if (movementClash.m_chase == chase && !(movementClash.m_clashSquare != clashSquare) && movementClash.m_moveCost == moveCost && movementClash.m_continuing == continuing)
				{
					return movementClash;
				}
			}
			return null;
		}

		public bool IsClashSegment(MovementRequest request, BoardSquarePathInfo segment)
		{
			return FindExistingClashFor(request.WasEverChasing(), segment.square, segment.moveCost, !segment.IsPathEndpoint()) != null;
		}

		public void SortClashes()
		{
			m_clashes.Sort();
		}
	}
}
#endif
