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
		Vector3 vector = caster.GetTravelBoardSquareWorldPositionForLos();
		float num = this.startOffsetDistance * Board.Get().squareSize;
		if (!this.startPosIgnoreLos)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BounceBombInfo.GetAdjustedStartPosition(Vector3, ActorData)).MethodHandle;
			}
			if (this.startOffsetDistance > 0f)
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
				float totalMaxDistanceInSquares = num + Board.Get().squareSize;
				Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary;
				List<ActorData> list2;
				List<Vector3> list = VectorUtils.CalculateBouncingLaserEndpoints(vector, aimDirection, num, totalMaxDistanceInSquares, 1, caster, this.width, -1, true, null, false, out dictionary, out list2, null, false, true);
				if (list.Count < 2)
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
					vector += num * aimDirection;
				}
				return vector;
			}
		}
		vector += num * aimDirection;
		return vector;
	}

	public unsafe Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> FindBounceHitActors(Vector3 aimDirection, ActorData caster, out List<Vector3> bounceEndPoints, List<List<NonActorTargetInfo>> nonActorTargetInfoInSegment, float maxDistancePerBounceOverride = -1f, float maxTotalDistanceOverride = -1f, bool includeInvisibles = true)
	{
		Vector3 adjustedStartPosition = this.GetAdjustedStartPosition(aimDirection, caster);
		float num;
		if (maxDistancePerBounceOverride > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BounceBombInfo.FindBounceHitActors(Vector3, ActorData, List<Vector3>*, List<List<NonActorTargetInfo>>, float, float, bool)).MethodHandle;
			}
			num = maxDistancePerBounceOverride;
		}
		else
		{
			num = this.maxDistancePerBounce;
		}
		float maxDistancePerBounceInSquares = num;
		float totalMaxDistanceInSquares = (maxTotalDistanceOverride <= 0f) ? this.maxTotalDistance : maxTotalDistanceOverride;
		List<Team> list = new List<Team>();
		list.Add(caster.GetOpposingTeam());
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> result;
		List<ActorData> list2;
		bounceEndPoints = VectorUtils.CalculateBouncingLaserEndpoints(adjustedStartPosition, aimDirection, maxDistancePerBounceInSquares, totalMaxDistanceInSquares, this.maxBounces, caster, this.width, 1, includeInvisibles, list, false, out result, out list2, nonActorTargetInfoInSegment, false, true);
		Vector3 start = adjustedStartPosition;
		if (bounceEndPoints.Count > 0)
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
			if (bounceEndPoints.Count > 1)
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
				start = bounceEndPoints[bounceEndPoints.Count - 2];
			}
			Vector3 end = bounceEndPoints[bounceEndPoints.Count - 1];
			Vector3 value;
			if (AreaEffectUtils.GetEndPointForValidGameplaySquare(start, end, out value))
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
				bounceEndPoints[bounceEndPoints.Count - 1] = value;
			}
		}
		return result;
	}
}
