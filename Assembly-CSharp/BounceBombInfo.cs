using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BounceBombInfo
{
	public AbilityAreaShape shape = AbilityAreaShape.Three_x_Three;

	public float width = 0.5f;

	public float maxDistancePerBounce = 5f;

	public float maxTotalDistance = 10f;

	public int maxBounces;

	public bool startPosIgnoreLos;

	public float startOffsetDistance;

	public Vector3 GetAdjustedStartPosition(Vector3 aimDirection, ActorData caster)
	{
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		float num = startOffsetDistance * Board.Get().squareSize;
		if (!startPosIgnoreLos)
		{
			if (startOffsetDistance > 0f)
			{
				float totalMaxDistanceInSquares = num + Board.Get().squareSize;
				Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors;
				List<ActorData> orderedHitActors;
				List<Vector3> list = VectorUtils.CalculateBouncingLaserEndpoints(travelBoardSquareWorldPositionForLos, aimDirection, num, totalMaxDistanceInSquares, 1, caster, width, -1, true, null, false, out bounceHitActors, out orderedHitActors, null);
				if (list.Count < 2)
				{
					travelBoardSquareWorldPositionForLos += num * aimDirection;
				}
				goto IL_00bb;
			}
		}
		travelBoardSquareWorldPositionForLos += num * aimDirection;
		goto IL_00bb;
		IL_00bb:
		return travelBoardSquareWorldPositionForLos;
	}

	public Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> FindBounceHitActors(Vector3 aimDirection, ActorData caster, out List<Vector3> bounceEndPoints, List<List<NonActorTargetInfo>> nonActorTargetInfoInSegment, float maxDistancePerBounceOverride = -1f, float maxTotalDistanceOverride = -1f, bool includeInvisibles = true)
	{
		Vector3 adjustedStartPosition = GetAdjustedStartPosition(aimDirection, caster);
		float num;
		if (maxDistancePerBounceOverride > 0f)
		{
			num = maxDistancePerBounceOverride;
		}
		else
		{
			num = maxDistancePerBounce;
		}
		float maxDistancePerBounceInSquares = num;
		float totalMaxDistanceInSquares = (!(maxTotalDistanceOverride > 0f)) ? maxTotalDistance : maxTotalDistanceOverride;
		List<Team> list = new List<Team>();
		list.Add(caster.GetOpposingTeam());
		bounceEndPoints = VectorUtils.CalculateBouncingLaserEndpoints(adjustedStartPosition, aimDirection, maxDistancePerBounceInSquares, totalMaxDistanceInSquares, maxBounces, caster, width, 1, includeInvisibles, list, false, out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors, out List<ActorData> _, nonActorTargetInfoInSegment);
		Vector3 start = adjustedStartPosition;
		if (bounceEndPoints.Count > 0)
		{
			if (bounceEndPoints.Count > 1)
			{
				start = bounceEndPoints[bounceEndPoints.Count - 2];
			}
			Vector3 end = bounceEndPoints[bounceEndPoints.Count - 1];
			if (AreaEffectUtils.GetEndPointForValidGameplaySquare(start, end, out Vector3 adjustedEndPoint))
			{
				bounceEndPoints[bounceEndPoints.Count - 1] = adjustedEndPoint;
			}
		}
		return bounceHitActors;
	}
}
