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

	public static void ResolveClashMovement(
		List<MovementRequest> storedMovementRequests,
		MovementClashCollection clashCollection,
		bool forChaseMovement)
	{
		if (clashCollection?.m_clashes == null || clashCollection.m_clashes.Count == 0)
		{
			return;
		}
		clashCollection.SortClashes();
		List<BoardSquare> invalidSquares = new List<BoardSquare>();
		foreach (ActorData stationaryActor in ServerActionBuffer.Get().GetStationaryActors())
		{
			invalidSquares.Add(stationaryActor.CurrentBoardSquare);
		}
		List<ActorData> afterImageActors = new List<ActorData>();
		foreach (ActorData actor in GameFlowData.Get().GetActors())
		{
			if (!actor.IsDead() && actor.GetComponent<Passive_TricksterAfterImage>() != null)
			{
				afterImageActors.Add(actor);
			}
		}
		foreach (ActorData afterImageActor in afterImageActors)
		{
			if (!invalidSquares.Contains(afterImageActor.CurrentBoardSquare))
			{
				invalidSquares.Add(afterImageActor.CurrentBoardSquare);
			}
		}
		foreach (MovementRequest movementRequest in storedMovementRequests)
		{
			if (!movementRequest.WasEverChasing() || forChaseMovement)
			{
				if (!invalidSquares.Contains(movementRequest.m_targetSquare))
				{
					invalidSquares.Add(movementRequest.m_targetSquare);
				}
			}
			else
			{
				if (!invalidSquares.Contains(movementRequest.m_actor.CurrentBoardSquare))
				{
					invalidSquares.Add(movementRequest.m_actor.CurrentBoardSquare);
				}
			}
		}
		foreach (MovementClash movementClash in clashCollection.m_clashes)
		{
			if (movementClash.m_continuing || movementClash.m_chase != forChaseMovement)
			{
				continue;
			}
			int clasherNum = movementClash.m_teamAClashers.Count + movementClash.m_teamBClashers.Count;
			BoardSquare clashSquare = movementClash.m_clashSquare;
			int bumpDistance = 0;
			int bumpDestinationNum = 0;
			List<BoardSquare> bumpDestinations = null;
			List<ClashResolutionEntry> resolutionEntries = new List<ClashResolutionEntry>(clasherNum);
			foreach (MovementClashParticipant clashParticipant in movementClash.m_teamAClashers)
			{
				resolutionEntries.Add(new ClashResolutionEntry(clashParticipant));
			}
			foreach (MovementClashParticipant clashParticipant in movementClash.m_teamBClashers)
			{
				resolutionEntries.Add(new ClashResolutionEntry(clashParticipant));
			}
			while (false)  // TODO
			{
				float distance = 1f + 0.5f * (float)bumpDistance;
				bumpDestinations = GetPotentialBumpDestinationSquaresWithDistanceFromSquare(
					clashSquare, distance, invalidSquares, resolutionEntries);
				bumpDestinationNum = bumpDestinations.Count;
				bumpDistance++;
			}
			if (bumpDistance <= 6 && bumpDestinationNum < clasherNum)
			{
				continue;
			}
			if (bumpDestinationNum < clasherNum)
			{
				bumpDistance = 0;
				bumpDestinationNum = 0;
				bumpDestinations = new List<BoardSquare>();
				while (bumpDistance <= 12 && bumpDestinationNum < clasherNum)
				{
					float distance = 1f + 0.5f * bumpDistance;
					List<BoardSquare> potentialBumpDestinations = GetPotentialBumpDestinationSquaresWithDistanceFromSquare(
							clashSquare, distance, invalidSquares, resolutionEntries);
					foreach (BoardSquare bumpDestination in potentialBumpDestinations)
					{
						bumpDestinations.Add(bumpDestination);
					}
					bumpDestinationNum = bumpDestinations.Count;
					bumpDistance++;
				}
			}
			List<ClashResolutionEntry> processedResolutionEntries = new List<ClashResolutionEntry>();
			while (processedResolutionEntries.Count < resolutionEntries.Count)
			{
				ClashResolutionEntry bestResolvedEntry = null;
				float bestWeight = 0f;
				int destinationSquareIndex = -1;
				foreach (ClashResolutionEntry resolutionEntry in resolutionEntries)
				{
					if (processedResolutionEntries.Contains(resolutionEntry))
					{
						continue;
					}
					for (int j = 0; j < bumpDestinations.Count; j++)
					{
						BoardSquare dest = bumpDestinations[j];
						if (IsSquareAlreadyClaimedByResolutionEntry(processedResolutionEntries, dest))
						{
							continue;
						}
						BoardSquarePathInfo path = resolutionEntry.Actor.GetActorMovement().BuildPathTo_IgnoreBarriers(clashSquare, dest);
						if (path == null)
						{
							Debug.LogError(
								$"While trying to stabilize clashes, failed to build a path " +
								$"from {BoardSquare.DebugString(clashSquare)} to {BoardSquare.DebugString(dest)} " +
								$"for actor {resolutionEntry.Actor.DebugNameString()}");
							continue;
						}
						float dist = path.FindDistanceToEnd();
						int clashSteps = 0;
						float lastStepWeight = 0f;
						BoardSquarePathInfo originalStep = resolutionEntry.m_clashParticipant.OriginalPath.GetPathEndpoint().prev;
						BoardSquarePathInfo clashStep = path.next;
						while (originalStep != null && clashStep != null)
						{
							if (originalStep.square == clashStep.square)
							{
								clashSteps++;
								originalStep = originalStep.prev;
								clashStep = clashStep.next;
							}
							else
							{
								if (Board.Get().GetSquaresAreCardinallyAdjacent(originalStep.square, clashStep.square))
								{
									lastStepWeight = 0.2f;
									break;
								}
								if (Board.Get().GetSquaresAreDiagonallyAdjacent(originalStep.square, clashStep.square))
								{
									lastStepWeight = 0.1f;
									break;
								}
								lastStepWeight = 0f;
								break;
							}
						}
						float weight = dist * 10f - clashSteps - lastStepWeight;
						if (bestResolvedEntry == null || weight < bestWeight)
						{
							bestWeight = weight;
							bestResolvedEntry = resolutionEntry;
							destinationSquareIndex = j;
						}
					}
				}
				bestResolvedEntry.m_newDestSquare = bumpDestinations[destinationSquareIndex];
				processedResolutionEntries.Add(bestResolvedEntry);
			}
			foreach (ClashResolutionEntry resolutionEntry in resolutionEntries)
			{
				BoardSquare newDestSquare = resolutionEntry.m_newDestSquare;
				resolutionEntry.BumpToNewDestinationSquare(newDestSquare);
				invalidSquares.Add(newDestSquare);
			}
		}
	}

	public static bool AreMoveCostsEqual(float cost1, float cost2, bool infiniteCostsConsideredEqual = false)
	{
		bool flag = cost1 == cost2 || Mathf.Abs(cost1 - cost2) < 0.1f;
		return flag && ((cost1 != float.MaxValue && cost2 != float.MaxValue) || infiniteCostsConsideredEqual);
	}

	public static MovementClashCollection IdentifyClashSegments_Movement(
		List<MovementRequest> stabilizedMovementRequests,
		bool forChaseMovement)
	{
		MovementClashCollection movementClashCollection = new MovementClashCollection();
		List<MovementRequest> requestsToProcess = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in stabilizedMovementRequests)
		{
			if (forChaseMovement == movementRequest.WasEverChasing())
			{
				movementRequest.m_path.ResetClashingOfPath();
				movementRequest.m_path.CalcAndSetMoveCostToEnd();
				requestsToProcess.Add(movementRequest);
			}
		}
		for (int i = 0; i < requestsToProcess.Count; i++)
		{
			for (int j = i + 1; j < requestsToProcess.Count; j++)
			{
				MovementRequest requestA = requestsToProcess[i];
				MovementRequest requestB = requestsToProcess[j];
				if (requestA.m_actor.GetTeam() != requestB.m_actor.GetTeam())
				{
					BoardSquarePathInfo stepA = requestA.m_path;
					BoardSquarePathInfo stepB = requestB.m_path;
					while (stepA != null && stepB != null)
					{
						if (stepA.square == stepB.square
						    && AreMoveCostsEqual(stepA.moveCost, stepB.moveCost)
						    && stepA.IsPathEndpoint() == stepB.IsPathEndpoint())
						{
							stepA.m_moverClashesHere = true;
							stepB.m_moverClashesHere = true;
							movementClashCollection.AddClash(requestA, stepA, requestB, stepB);
						}
						if (stepA.moveCost == stepB.moveCost)
						{
							stepA = stepA.next;
							stepB = stepB.next;
						}
						else if (stepA.moveCost > stepB.moveCost)
						{
							stepB = stepB.next;
						}
						else
						{
							stepA = stepA.next;
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
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in evades)
		{
			evadeInfo.m_evadePath.ResetClashingOfPath();
			evadeInfo.m_evadePath.CalcAndSetMoveCostToEnd();
		}
		for (int i = 0; i < evades.Count; i++)
		{
			for (int j = i + 1; j < evades.Count; j++)
			{
				ServerEvadeUtils.EvadeInfo evadeA = evades[i];
				ServerEvadeUtils.EvadeInfo evadeB = evades[j];
				if (evadeA.GetMover().GetTeam() != evadeB.GetMover().GetTeam()
				    && !evadeA.IsDestinationReserved() && !evadeB.IsDestinationReserved())
				{
					BoardSquarePathInfo stepA = evadeA.m_evadePath;
					BoardSquarePathInfo stepB = evadeB.m_evadePath;
					while (stepA != null && stepB != null)
					{
						if (stepA.square == stepB.square
						    && AreMoveCostsEqual(stepA.moveCost, stepB.moveCost)
						    && stepA.IsPathEndpoint() == stepB.IsPathEndpoint())
						{
							stepA.m_moverClashesHere = true;
							stepB.m_moverClashesHere = true;
							movementClashCollection.AddClash(evadeA, stepA, evadeB, stepB);
						}
						if (stepA.moveCost == stepB.moveCost)
						{
							stepA = stepA.next;
							stepB = stepB.next;
						}
						else if (stepA.moveCost > stepB.moveCost)
						{
							stepB = stepB.next;
						}
						else
						{
							stepA = stepA.next;
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

		public ActorData Actor => m_clashParticipant.Actor;
		public BoardSquarePathInfo OriginalPath => m_clashParticipant.OriginalPath;

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

		public ActorData Actor => m_request != null ? m_request.m_actor : m_evade.GetMover();
		public bool WasEverChasing => m_request != null && m_request.WasEverChasing();
		public BoardSquarePathInfo OriginalPath => m_request != null ? m_request.m_path : m_evade.m_evadePath;

		public void BumpToNewDestinationSquare(BoardSquare newDest)
		{
			if (newDest == null)
			{
				Log.Error("{act} MoveStabilizationErr: Trying to bump to null square");
				return;
			}
			BoardSquarePathInfo pathEndpoint = OriginalPath.GetPathEndpoint();
			BoardSquare endpointSquare = pathEndpoint.square;
			BoardSquarePathInfo boardSquarePathInfo = Actor.GetActorMovement().BuildPathTo_IgnoreBarriers(endpointSquare, newDest);
			if (boardSquarePathInfo == null)
			{
				Log.Error("{act} MoveStabilizationErr: Failed to build a bump path.  Movement is now untrustworthy.");
				return;
			}
			BoardSquarePathInfo step = boardSquarePathInfo.next;
			step.prev = pathEndpoint;
			pathEndpoint.next = step;
			pathEndpoint.m_moverClashesHere = true;
			while (step != null)
			{
				step.m_moverBumpedFromClash = true;
				step = step.next;
			}
			OriginalPath.CalcAndSetMoveCostToEnd();
			if (m_request != null)
			{
				m_request.m_targetSquare = newDest;
			}
			else if (m_evade != null)
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
			switch (team)
			{
				case Team.TeamA:
					return m_teamAClashers;
				case Team.TeamB:
					return m_teamBClashers;
				default:
					return null;
			}
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
			if (!(obj is MovementClash movementClash))
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

		public void AddClash(
			MovementRequest firstRequest,
			BoardSquarePathInfo firstSegment,
			MovementRequest secondRequest,
			BoardSquarePathInfo secondSegment)
		{
			MovementClashParticipant first = new MovementClashParticipant(firstRequest, firstSegment);
			MovementClashParticipant second = new MovementClashParticipant(secondRequest, secondSegment);
			MovementClash movementClash = FindExistingClashFor(
				firstRequest.WasEverChasing(), firstSegment.square, firstSegment.moveCost, !firstSegment.IsPathEndpoint());
			if (movementClash == null)
			{
				MovementClash item = new MovementClash(first, second);
				m_clashes.Add(item);
			}
			else
			{
				movementClash.AddClashers(first, second);
			}
		}

		public void AddClash(
			ServerEvadeUtils.EvadeInfo firstEvade,
			BoardSquarePathInfo firstSegment,
			ServerEvadeUtils.EvadeInfo secondEvade,
			BoardSquarePathInfo secondSegment)
		{
			MovementClashParticipant first = new MovementClashParticipant(firstEvade, firstSegment);
			MovementClashParticipant second = new MovementClashParticipant(secondEvade, secondSegment);
			MovementClash movementClash = FindExistingClashFor(
				false, firstSegment.square, firstSegment.moveCost, !firstSegment.IsPathEndpoint());
			if (movementClash == null)
			{
				MovementClash item = new MovementClash(first, second);
				m_clashes.Add(item);
			}
			else
			{
				movementClash.AddClashers(first, second);
			}
		}

		public MovementClash FindExistingClashFor(bool chase, BoardSquare clashSquare, float moveCost, bool continuing)
		{
			if (m_clashes == null || m_clashes.Count == 0)
			{
				return null;
			}
			foreach (MovementClash movementClash in m_clashes)
			{
				if (movementClash.m_chase == chase
				    && movementClash.m_clashSquare == clashSquare
				    && movementClash.m_moveCost == moveCost
				    && movementClash.m_continuing == continuing)
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
