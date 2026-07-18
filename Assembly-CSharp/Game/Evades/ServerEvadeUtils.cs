// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only
#if SERVER
public static class ServerEvadeUtils
{
	public static void RemoveInvalidChargeEndPositions(ref List<Vector3> endPoints)
	{
		for (int i = endPoints.Count - 1; i > 0; i--)
		{
			Vector3 start = endPoints[i - 1];
			Vector3 end = endPoints[i];
			BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(start, end, true);
			if (lastValidBoardSquareInLine != null && lastValidBoardSquareInLine.IsValidForGameplay())
			{
				break;
			}
			endPoints.RemoveAt(i);
		}
	}

	public static void GetLastSegmentInfo(
		Vector3 startPos,
		List<Vector3> endPoints,
		out Vector3 lastSegStartPos,
		out Vector3 lastSegDir,
		out float lastSegLength)
	{
		Vector3 vector = endPoints[endPoints.Count - 1];
		if (endPoints.Count == 1)
		{
			lastSegStartPos = startPos;
		}
		else
		{
			lastSegStartPos = endPoints[endPoints.Count - 2];
			Vector3 vector2 = vector - lastSegStartPos;
			float magnitude = vector2.magnitude;
			vector2.Normalize();
			float num = Mathf.Min(0.5f, magnitude / 2f);
			lastSegStartPos += vector2 * num;
		}
		lastSegDir = vector - lastSegStartPos;
		lastSegLength = lastSegDir.magnitude;
		lastSegDir.Normalize();
	}

	public static Vector3 GetChargeBestSquareTestDirection(ChargeSegment[] chargeSegments)
	{
		int num = chargeSegments.Length;
		Vector3 result;
		if (num >= 2)
		{
			if (num > 2 && chargeSegments[num - 1].m_pos == chargeSegments[num - 2].m_pos)
			{
				BoardSquare pos = chargeSegments[num - 3].m_pos;
				BoardSquare pos2 = chargeSegments[num - 2].m_pos;
				result = pos.ToVector3() - pos2.ToVector3();
			}
			else
			{
				result = chargeSegments[num - 2].m_pos.ToVector3() - chargeSegments[num - 1].m_pos.ToVector3();
			}
		}
		else
		{
			result = Vector3.forward;
		}
		result.y = 0f;
		result.Normalize();
		return result;
	}

	public static ChargeSegment[] ProcessChargeDodgeForStopOnTargetHit(
		BoardSquare destination,
		List<AbilityTarget> targets,
		ActorData caster,
		ChargeInfo charge,
		List<EvadeInfo> evades)
	{
		if (charge.m_chargeSegments[charge.m_chargeSegments.Length - 1].m_end == BoardSquarePathInfo.ChargeEndType.Miss
		    || (!charge.IsValidEvadeDestination(destination, evades) && charge.m_evadeDest != destination))
		{
			return charge.m_chargeSegments;
		}
		
		ChargeSegment[] result = charge.m_chargeSegments;
		List<BoardSquare> invalidEvadeDestinations = new List<BoardSquare>();
		foreach (EvadeInfo evadeInfo in evades)
		{
			if (evadeInfo.GetMover() != null && evadeInfo.GetMover().GetPassiveData() != null)
			{
				evadeInfo.GetMover().GetPassiveData().AddInvalidEvadeDestinations(evades, invalidEvadeDestinations);
			}
		}
		if (!invalidEvadeDestinations.Contains(destination))
		{
			result = new ChargeSegment[charge.m_chargeSegments.Length - 1];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = charge.m_chargeSegments[i]; // we aren't copying here, why the charade
			}
			result[result.Length - 1].m_end = BoardSquarePathInfo.ChargeEndType.Miss;
		}
		return result;
	}

	public static ChargeSegment[] GetChargeSegmentForStopOnTargetHit(
		ActorData caster,
		List<Vector3> endPoints,
		BoardSquare dest,
		float dashRecoverTime)
	{
		ChargeSegment[] array;
		if (dest == null)
		{
			Log.Error("Failed to find a destination square for Battle Monk Bounding Leap.");
			array = new ChargeSegment[]
			{
				new ChargeSegment()
			};
			array[0].m_pos = caster.GetCurrentBoardSquare();
		}
		else
		{
			List<ChargeSegment> list = new List<ChargeSegment>();
			list.Add(new ChargeSegment
			{
				m_pos = caster.GetCurrentBoardSquare()
			});
			for (int i = 0; i < endPoints.Count; i++)
			{
				ChargeSegment chargeSegment = new ChargeSegment();
				Vector3 vector;
				if (i == 0)
				{
					vector = list[i].m_pos.ToVector3();
				}
				else
				{
					vector = endPoints[i - 1];
				}
				Vector3 vector2 = (endPoints[i] - vector).normalized / 2f;
				if (i > 0)
				{
					BoardSquare pos = list[list.Count - 1].m_pos;
					BoardSquare squareFromVec = Board.Get().GetSquareFromVec3(vector + vector2);
					if (squareFromVec != null && squareFromVec != pos && squareFromVec.IsValidForGameplay())
					{
						list.Add(new ChargeSegment
						{
							m_pos = squareFromVec,
							m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
							m_end = BoardSquarePathInfo.ChargeEndType.Pivot
						});
					}
				}
				if (i == endPoints.Count - 1)
				{
					chargeSegment.m_pos = dest;
					float maxDistance = Vector3.Distance(vector, dest.ToVector3());
					BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(vector, endPoints[i], true, false, maxDistance);
					if (lastValidBoardSquareInLine != null && lastValidBoardSquareInLine.IsValidForGameplay())
					{
						chargeSegment.m_pos = lastValidBoardSquareInLine;
					}
				}
				else
				{
					chargeSegment.m_pos = Board.Get().GetSquareFromVec3(endPoints[i] - vector2);
					if (chargeSegment.m_pos != null && !chargeSegment.m_pos.IsValidForGameplay())
					{
						BoardSquare lastValidBoardSquareInLine2 = KnockbackUtils.GetLastValidBoardSquareInLine(vector, endPoints[i], true, false, float.MaxValue);
						if (lastValidBoardSquareInLine2 != null && lastValidBoardSquareInLine2.IsValidForGameplay())
						{
							chargeSegment.m_pos = lastValidBoardSquareInLine2;
						}
					}
				}
				chargeSegment.m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement;
				chargeSegment.m_end = BoardSquarePathInfo.ChargeEndType.Pivot;
				list.Add(chargeSegment);
			}
			if (dest.occupant != null && dest.occupant != caster.gameObject)
			{
				list.Add(new ChargeSegment());
				int count = list.Count;
				list[count - 2].m_end = BoardSquarePathInfo.ChargeEndType.Impact;
				list[count - 1] = new ChargeSegment();
				list[count - 1].m_pos = list[count - 2].m_pos;
				list[count - 1].m_reverseFacing = true;
				list[count - 1].m_segmentMovementDuration = dashRecoverTime;
				list[count - 1].m_cycle = BoardSquarePathInfo.ChargeCycleType.Recovery;
				list[count - 1].m_end = BoardSquarePathInfo.ChargeEndType.Recovery;
			}
			else
			{
				list[list.Count - 1].m_end = BoardSquarePathInfo.ChargeEndType.Miss;
			}
			array = list.ToArray();
		}
		return array;
	}

	public class ChargeSegment
	{
		public BoardSquare m_pos;
		public BoardSquarePathInfo.ChargeCycleType m_cycle;
		public BoardSquarePathInfo.ChargeEndType m_end;
		public float m_segmentMovementSpeed;
		public float m_segmentMovementDuration;
		public bool m_reverseFacing;

		public ChargeSegment Clone()
		{
			return new ChargeSegment
			{
				m_pos = m_pos,
				m_cycle = m_cycle,
				m_end = m_end,
				m_segmentMovementSpeed = m_segmentMovementSpeed,
				m_segmentMovementDuration = m_segmentMovementDuration,
				m_reverseFacing = m_reverseFacing
			};
		}
	}

	public abstract class EvadeInfo
	{
		public AbilityRequest m_request;
		public BoardSquare m_evadeDest;
		public BoardSquare m_bounceOffSquare;
		public BoardSquarePathInfo m_evadePath;

		public virtual ActorData GetMover()
		{
			return m_request.m_caster;
		}

		public virtual void MarkAsInvalid()
		{
			m_request.m_stillValid = false;
		}

		public virtual bool IsStillValid()
		{
			return m_request.m_stillValid;
		}

		public abstract BoardSquare GetEvadeStart();

		public abstract float GetEvadeDistance();

		public abstract float GetEvadePathDistance(BoardSquare theoreticalDest);

		public abstract BoardSquare GetIdealDestination();

		public abstract void ModifyDestination(BoardSquare newDestination);

		public virtual void ProcessEvadeDodge(List<EvadeInfo> allEvades)
		{
		}

		public abstract void StorePath();

		public abstract bool IsValidEvadeDestination(BoardSquare targetSquare, List<EvadeInfo> allEvades);

		public abstract Vector3 GetBestSquareTestVector();

		public abstract bool IsDestinationReserved();

		public abstract void ResetDestinationData();

		public virtual ActorData.MovementType GetMovementType()
		{
			return m_request != null && m_request.m_ability != null
				? m_request.m_ability.GetMovementType()
				: ActorData.MovementType.Flight;
		}

		public virtual bool IsStealthEvade()
		{
			return m_request != null && m_request.m_ability != null && m_request.m_ability.IsStealthEvade();
		}

		protected bool IsValidEvadeDestinationForTeleport(BoardSquare square, List<EvadeInfo> allEvades)
		{
			if (square == null || !square.IsValidForGameplay())
			{
				return false;
			}
			
			bool isDestinationUnoccupied;
			if (square.occupant == null)
			{
				isDestinationUnoccupied = true;
			}
			else
			{
				isDestinationUnoccupied = false;
				ActorData occupantActor = square.occupant.GetComponent<ActorData>();
				foreach (EvadeInfo evadeInfo in allEvades)
				{
					if (evadeInfo.GetMover() == null || occupantActor == evadeInfo.GetMover())
					{
						isDestinationUnoccupied = true;
						break;
					}
				}
			}

			if (!isDestinationUnoccupied)
			{
				return false;
			}

			foreach (EvadeInfo evadeInfo in allEvades)
			{
				if (evadeInfo.m_evadeDest != square)
				{
					continue;
				}
				
				if (GetMover().GetTeam() == evadeInfo.GetMover().GetTeam())
				{
					return false;
				}

				float myDist = GetEvadePathDistance(square);
				float otherDist = evadeInfo.GetEvadePathDistance(square);
				if (!ServerClashUtils.AreMoveCostsEqual(myDist, otherDist))
				{
					return false;
				}
			}

			return true;
		}

		public int GetNumSquaresInPath()
		{
			if (m_evadePath != null)
			{
				return m_evadePath.GetNumSquaresToEnd(true);
			}
			return 0;
		}

		public List<BoardSquare> GetSquaresInPath()
		{
			List<BoardSquare> list = new List<BoardSquare>();
			for (BoardSquarePathInfo boardSquarePathInfo = m_evadePath; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
			{
				if (boardSquarePathInfo.square != null)
				{
					list.Add(boardSquarePathInfo.square);
				}
			}
			return list;
		}
	}

	public class TeleportInfo : EvadeInfo
	{
		public BoardSquare m_teleportStart;

		public TeleportInfo(AbilityRequest teleportRequest)
		{
			m_request = teleportRequest;
			m_teleportStart = m_request.m_caster.GetCurrentBoardSquare();
			m_evadeDest = null;
		}

		public override Vector3 GetBestSquareTestVector()
		{
			BoardSquare evadeStart = GetEvadeStart();
			BoardSquare idealDestination = GetIdealDestination();
			Vector3 result = evadeStart.ToVector3() - idealDestination.ToVector3();
			result.y = 0f;
			result.Normalize();
			return result;
		}

		public override BoardSquare GetEvadeStart()
		{
			return m_teleportStart;
		}

		public override float GetEvadeDistance()
		{
			return m_teleportStart.HorizontalDistanceOnBoardTo(GetIdealDestination());
		}

		public override float GetEvadePathDistance(BoardSquare theoreticalDest)
		{
			if (theoreticalDest == null || !theoreticalDest.IsValidForGameplay())
			{
				return float.MaxValue;
			}
			if (theoreticalDest == m_request.m_caster.CurrentBoardSquare)
			{
				return 0f;
			}
			BoardSquarePathInfo boardSquarePathInfo = MovementUtils.Build2PointTeleportPath(m_request.m_caster, theoreticalDest);
			if (boardSquarePathInfo == null)
			{
				return float.MaxValue;
			}
			boardSquarePathInfo.CalcAndSetMoveCostToEnd();
			return boardSquarePathInfo.FindMoveCostToEnd();
		}

		public override BoardSquare GetIdealDestination()
		{
			return m_request.m_ability.GetIdealDestination(m_request.m_targets, m_request.m_caster, m_request.m_additionalData);
		}

		public override void ModifyDestination(BoardSquare newDestination)
		{
			m_evadeDest = newDestination;
		}

		public override bool IsValidEvadeDestination(BoardSquare square, List<EvadeInfo> allEvades)
		{
			return base.IsValidEvadeDestinationForTeleport(square, allEvades);
		}

		public override void StorePath()
		{
			m_evadePath = MovementUtils.Build2PointTeleportPath(m_request.m_caster, m_evadeDest);
		}

		public override bool IsDestinationReserved()
		{
			return m_request != null && m_request.m_ability != null && m_request.m_ability.IsEvadeDestinationReserved();
		}

		public override void ResetDestinationData()
		{
			m_evadeDest = null;
			m_bounceOffSquare = null;
			m_evadePath = null;
		}
	}

	public class ChargeInfo : EvadeInfo
	{
		public ChargeSegment[] m_chargeSegments;

		public ChargeInfo(AbilityRequest chargeRequest)
		{
			m_request = chargeRequest;
			m_chargeSegments = m_request.m_ability.GetChargePath(m_request.m_targets, m_request.m_caster, m_request.m_additionalData);
			m_bounceOffSquare = m_request.m_ability.GetBounceOffSquare(m_request.m_targets, m_request.m_caster, m_chargeSegments);
			m_evadeDest = null;
		}

		public BoardSquare GetValidChargeTestSourceSquare()
		{
			return m_request.m_ability.GetValidChargeTestSourceSquare(m_chargeSegments);
		}

		public bool CanChargeThroughInvalidSquaresForDestination()
		{
			return m_request.m_ability.CanChargeThroughInvalidSquaresForDestination();
		}

		// TODO SAMURAI
		public override Vector3 GetBestSquareTestVector()
		{
			return m_request.m_ability.GetChargeBestSquareTestVector(m_chargeSegments);
		}

		public override BoardSquare GetEvadeStart()
		{
			if (m_chargeSegments.Length > 2)
			{
				return m_chargeSegments[m_chargeSegments.Length - 2].m_pos;
			}
			return m_chargeSegments[0].m_pos;
		}

		public override float GetEvadeDistance()
		{
			float num = 0f;
			for (int i = 0; i < m_chargeSegments.Length - 1; i++)
			{
				num += m_chargeSegments[i].m_pos.HorizontalDistanceOnBoardTo(m_chargeSegments[i + 1].m_pos);
			}
			return num;
		}

		public override float GetEvadePathDistance(BoardSquare theoreticalDest)
		{
			if (theoreticalDest == null || !theoreticalDest.IsValidForGameplay())
			{
				return float.MaxValue;
			}
			bool flag = false;
			List<ChargeSegment> list = new List<ChargeSegment>(m_chargeSegments.Length);
			for (int i = 0; i < m_chargeSegments.Length; i++)
			{
				list.Add(m_chargeSegments[i].Clone());
				if (i > 0)
				{
					flag |= (list[i].m_pos != list[i - 1].m_pos);
				}
			}
			if (!flag)
			{
				return 0f;
			}
			int num = m_chargeSegments.Length - 1;
			if (num > 1 && list[num - 1].m_pos == theoreticalDest)
			{
				list.RemoveAt(num);
			}
			else if (num >= 0 && list[num].m_pos != theoreticalDest)
			{
				list[num].m_pos = theoreticalDest;
			}
			BoardSquarePathInfo boardSquarePathInfo = ServerEvadeManager.BuildPathForCharge(m_request.m_caster, list.ToArray(), m_request.m_ability.GetMovementType(), m_request.m_ability.CalcMovementSpeed(GetEvadeDistance()), m_request.m_ability.GetChargeThroughInvalidSquares());
			if (boardSquarePathInfo == null)
			{
				return float.MaxValue;
			}
			if (boardSquarePathInfo.next == null)
			{
				return 0f;
			}
			if (boardSquarePathInfo.next != null && boardSquarePathInfo.next.square == boardSquarePathInfo.square && boardSquarePathInfo.next.next == null)
			{
				return 0f;
			}
			boardSquarePathInfo.CalcAndSetMoveCostToEnd();
			return boardSquarePathInfo.FindMoveCostToEnd();
		}

		public override BoardSquare GetIdealDestination()
		{
			return m_chargeSegments[m_chargeSegments.Length - 1].m_pos;
		}

		public override void ModifyDestination(BoardSquare newDestination)
		{
			m_chargeSegments[m_chargeSegments.Length - 1].m_pos = newDestination;
			m_evadeDest = newDestination;
		}

		public override void ProcessEvadeDodge(List<EvadeInfo> allEvades)
		{
			m_chargeSegments = m_request.m_ability.ProcessChargeDodge(m_request.m_targets, m_request.m_caster, this, allEvades);
			BoardSquare pos = m_chargeSegments[m_chargeSegments.Length - 1].m_pos;
			if (pos != m_evadeDest)
			{
				m_evadeDest = pos;
			}
		}

		public bool OtherEvadeHasSameBounceOffSquare(EvadeInfo evadeInfo, List<EvadeInfo> allEvades)
		{
			bool result = false;
			if (evadeInfo.m_bounceOffSquare != null)
			{
				for (int i = 0; i < allEvades.Count; i++)
				{
					if (allEvades[i] != evadeInfo && evadeInfo.m_bounceOffSquare == allEvades[i].m_bounceOffSquare)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public override bool IsValidEvadeDestination(BoardSquare square, List<EvadeInfo> allEvades)
		{
			if (square == null || !square.IsValidForGameplay())
			{
				return false;
			}
			
			bool isDestinationUnoccupied;
			if (square.occupant == null)
			{
				isDestinationUnoccupied = true;
			}
			else
			{
				ActorData occupantActor = square.occupant.GetComponent<ActorData>();
				if (occupantActor == m_request.m_caster)
				{
					isDestinationUnoccupied = true;
				}
				else
				{
					isDestinationUnoccupied = false;
					foreach (EvadeInfo evadeInfo in allEvades)
					{
						if (evadeInfo.GetMover() == null || occupantActor == evadeInfo.GetMover())
						{
							isDestinationUnoccupied = true;
							break;
						}
					}
				}
			}

			bool canBuildPath = KnockbackUtils.BuildStraightLineChargePath(
				             m_request.m_caster,
				             square,
				             GetValidChargeTestSourceSquare(),
				             CanChargeThroughInvalidSquaresForDestination()) != null;

			if (!isDestinationUnoccupied || !canBuildPath)
			{
				return false;
			}

			foreach (EvadeInfo evadeInfo in allEvades)
			{
				if (evadeInfo.m_evadeDest != square)
				{
					continue;
				}
				
				if (GetMover().GetTeam() == evadeInfo.GetMover().GetTeam())
				{
					return false;
				}

				float myDist = GetEvadePathDistance(square);
				float otherDist = evadeInfo.GetEvadePathDistance(square);
				if (!ServerClashUtils.AreMoveCostsEqual(myDist, otherDist))
				{
					return false;
				}
			}

			return true;
		}

		public override void StorePath()
		{
			m_evadePath = ServerEvadeManager.BuildPathForCharge(m_request.m_caster, m_chargeSegments, m_request.m_ability.GetMovementType(), m_request.m_ability.CalcMovementSpeed(GetEvadeDistance()), m_request.m_ability.GetChargeThroughInvalidSquares());
		}

		public override bool IsDestinationReserved()
		{
			return m_request != null && m_request.m_ability != null && m_request.m_ability.IsEvadeDestinationReserved();
		}

		public override void ResetDestinationData()
		{
			m_evadeDest = null;
			m_chargeSegments = m_request.m_ability.GetChargePath(m_request.m_targets, m_request.m_caster, m_request.m_additionalData);
			m_bounceOffSquare = m_request.m_ability.GetBounceOffSquare(m_request.m_targets, m_request.m_caster, m_chargeSegments);
			m_evadeDest = null;
		}
	}

	public class NonPlayerEvadeData
	{
		public ActorData m_mover;
		public ActorData.MovementType m_movementType = ActorData.MovementType.None;
		public BoardSquare m_start;
		public BoardSquare m_idealDestination;
		public float m_moveSpeed = 8f;
		public Vector3 m_facingDirection = Vector3.zero;
		public bool m_reserveDestination;

		public NonPlayerEvadeData(ActorData mover, BoardSquare start, BoardSquare destination, ActorData.MovementType movementType, float moveSpeed, Vector3 facingDirection, bool reserveDestination)
		{
			m_mover = mover;
			m_movementType = movementType;
			m_start = start;
			m_idealDestination = destination;
			m_moveSpeed = moveSpeed;
			m_facingDirection = facingDirection;
			m_reserveDestination = reserveDestination;
		}

		public virtual bool ShouldAddToEvades()
		{
			return m_mover == null || !GameplayUtils.IsValidPlayer(m_mover);
		}

		public bool IsTeleport()
		{
			return m_movementType == ActorData.MovementType.Teleport || m_movementType == ActorData.MovementType.Flight;
		}

		public bool IsCharge()
		{
			return m_movementType == ActorData.MovementType.Charge || m_movementType == ActorData.MovementType.WaypointFlight;
		}
	}

	public class NonPlayerTeleportInfo : EvadeInfo
	{
		public bool m_valid;
		public ActorData m_mover;
		public BoardSquare m_teleportStart;
		public BoardSquare m_idealDestination;
		public NonPlayerEvadeData m_attachedEvadeData;

		public NonPlayerTeleportInfo(ActorData mover, BoardSquare teleportStart, BoardSquare idealDestination, NonPlayerEvadeData attachedEvadeData)
		{
			m_valid = true;
			m_mover = mover;
			m_teleportStart = teleportStart;
			m_idealDestination = idealDestination;
			m_attachedEvadeData = attachedEvadeData;
			m_evadeDest = null;
		}

		public override ActorData GetMover()
		{
			return m_mover;
		}

		public override void MarkAsInvalid()
		{
			m_valid = false;
		}

		public override bool IsStillValid()
		{
			return m_valid;
		}

		public override BoardSquare GetEvadeStart()
		{
			return m_teleportStart;
		}

		public override float GetEvadeDistance()
		{
			return m_teleportStart.HorizontalDistanceOnBoardTo(GetIdealDestination());
		}

		public override float GetEvadePathDistance(BoardSquare theoreticalDest)
		{
			if (theoreticalDest == null || !theoreticalDest.IsValidForGameplay())
			{
				return float.MaxValue;
			}
			if (theoreticalDest == m_teleportStart)
			{
				return 0f;
			}
			BoardSquarePathInfo boardSquarePathInfo = MovementUtils.Build2PointTeleportPath(m_teleportStart, theoreticalDest);
			if (boardSquarePathInfo == null)
			{
				return float.MaxValue;
			}
			boardSquarePathInfo.CalcAndSetMoveCostToEnd();
			return boardSquarePathInfo.FindMoveCostToEnd();
		}

		public override BoardSquare GetIdealDestination()
		{
			return m_idealDestination;
		}

		public override void ModifyDestination(BoardSquare newDestination)
		{
			m_evadeDest = newDestination;
		}

		public override bool IsValidEvadeDestination(BoardSquare targetSquare, List<EvadeInfo> allEvades)
		{
			return base.IsValidEvadeDestinationForTeleport(targetSquare, allEvades);
		}

		public override Vector3 GetBestSquareTestVector()
		{
			BoardSquare evadeStart = GetEvadeStart();
			BoardSquare idealDestination = GetIdealDestination();
			Vector3 result = evadeStart.ToVector3() - idealDestination.ToVector3();
			result.y = 0f;
			result.Normalize();
			return result;
		}

		public override void StorePath()
		{
			m_evadePath = MovementUtils.Build2PointTeleportPath(m_teleportStart, m_evadeDest);
		}

		public override bool IsDestinationReserved()
		{
			return m_attachedEvadeData.m_reserveDestination;
		}

		public override void ResetDestinationData()
		{
			m_evadeDest = null;
			m_bounceOffSquare = null;
			m_evadePath = null;
		}
	}
}
#endif
