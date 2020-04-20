using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BendingLaser : AbilityUtil_Targeter
{
	public float m_width = 1f;

	public float m_minDistanceBeforeBend = 5f;

	public float m_maxDistanceBeforeBend = 10f;

	public float m_maxTotalDistance = 15f;

	public float m_maxBendAngle = 45f;

	public bool m_penetrateLoS;

	public int m_maxTargets = -1;

	public bool m_showAngleIndicators = true;

	public bool m_startFadeAtActorRadius;

	private bool m_stoppedShort;

	private List<int> m_highlightsToFade = new List<int>();

	private UIRectangleCursor m_laserStartRect;

	private UIRectangleCursor m_laserEndRect;

	private const int numHighlightObjects = 4;

	private const int laserHighlightStartIndex = 0;

	private const int laserHighlightEndIndex = 1;

	private const int leftSideHighlightIndex = 2;

	private const int rightSideHighlightIndex = 3;

	private const float ghostedHighlightOpacity = 0.06f;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public List<ActorData> m_ordererdHitActors = new List<ActorData>();

	public AbilityUtil_Targeter_BendingLaser(Ability ability, float width, float minDistanceBeforeBend, float maxDistanceBeforeBend, float totalDistance, float maxBendAngle, bool penetrateLoS, int maxTargets = -1, bool affectsAllies = false, bool affectsCaster = false) : base(ability)
	{
		this.m_width = width;
		this.m_minDistanceBeforeBend = minDistanceBeforeBend;
		this.m_maxDistanceBeforeBend = maxDistanceBeforeBend;
		this.m_maxTotalDistance = totalDistance;
		this.m_maxBendAngle = maxBendAngle;
		this.m_penetrateLoS = penetrateLoS;
		this.m_maxTargets = maxTargets;
		this.m_affectsAllies = affectsAllies;
		base.SetAffectedGroups(true, this.m_affectsAllies, affectsCaster);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public bool DidStopShort()
	{
		return this.m_stoppedShort;
	}

	private float GetClampedRangeInSquares(ActorData targetingActor, AbilityTarget currentTarget)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float magnitude = (currentTarget.FreePos - travelBoardSquareWorldPositionForLos).magnitude;
		if (magnitude < this.m_minDistanceBeforeBend * Board.Get().squareSize)
		{
			return this.m_minDistanceBeforeBend;
		}
		if (magnitude > this.m_maxDistanceBeforeBend * Board.Get().squareSize)
		{
			return this.m_maxDistanceBeforeBend;
		}
		return magnitude / Board.Get().squareSize;
	}

	private unsafe float GetDistanceRemaining(ActorData targetingActor, AbilityTarget previousTarget, out Vector3 bendPos)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		bendPos = travelBoardSquareWorldPositionForLos;
		if (this.m_stoppedShort)
		{
			return 0f;
		}
		float clampedRangeInSquares = this.GetClampedRangeInSquares(targetingActor, previousTarget);
		bendPos = travelBoardSquareWorldPositionForLos + previousTarget.AimDirection * clampedRangeInSquares * Board.Get().squareSize;
		return this.m_maxTotalDistance - clampedRangeInSquares;
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (this.m_highlights.Count >= 4)
		{
			GameObject gameObject = this.m_highlights[2];
			GameObject gameObject2 = this.m_highlights[3];
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, new List<AbilityTarget>
		{
			currentTarget
		});
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		float num = this.m_width * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		base.ClearActorsInRange();
		this.m_ordererdHitActors.Clear();
		List<ActorData> list = new List<ActorData>();
		this.m_stoppedShort = false;
		this.m_highlightsToFade.Clear();
		bool flag;
		if (this.m_showAngleIndicators)
		{
			flag = (currentTargetIndex == 0);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		VectorUtils.LaserCoords laserCoords;
		float num2;
		Vector3 vector;
		float num3;
		if (currentTargetIndex == 0)
		{
			laserCoords.start = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			num2 = this.GetClampedRangeInSquares(targetingActor, currentTarget);
			vector = currentTarget.AimDirection;
			num3 = GameWideData.Get().m_laserInitialOffsetInSquares;
		}
		else
		{
			Vector3 aimDirection = targets[currentTargetIndex - 1].AimDirection;
			num2 = this.GetDistanceRemaining(targetingActor, targets[0], out laserCoords.start);
			Vector3 a = currentTarget.FreePos;
			if ((currentTarget.FreePos - targets[currentTargetIndex - 1].FreePos).magnitude < Mathf.Epsilon)
			{
				a += aimDirection * 10f;
			}
			vector = a - laserCoords.start;
			vector.y = 0f;
			vector.Normalize();
			num3 = -0.2f;
			if (this.m_maxBendAngle > 0f)
			{
				if (this.m_maxBendAngle < 360f)
				{
					vector = Vector3.RotateTowards(aimDirection, vector, 0.0174532924f * this.m_maxBendAngle, 0f);
				}
			}
			laserCoords.start = VectorUtils.GetAdjustedStartPosWithOffset(laserCoords.start, laserCoords.start + vector, num3);
		}
		if (currentTargetIndex > 0)
		{
			Vector3 lineEndPoint = VectorUtils.GetLineEndPoint(laserCoords.start, vector, num2 * Board.SquareSizeStatic);
			num2 = Mathf.Min(VectorUtils.HorizontalPlaneDistInSquares(lineEndPoint, laserCoords.start), num2);
		}
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector, num2, this.m_width, targetingActor, base.GetAffectedTeams(), this.m_penetrateLoS, 0, false, false, out laserCoords.end, null, null, currentTargetIndex > 0, true);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInLaser, laserCoords.start);
		bool flag3 = false;
		int num4 = this.m_maxTargets;
		if (currentTargetIndex > 0 && this.m_ability != null)
		{
			if (currentTargetIndex < this.m_ability.Targeters.Count)
			{
				AbilityUtil_Targeter abilityUtil_Targeter = this.m_ability.Targeters[currentTargetIndex - 1];
				for (int i = actorsInLaser.Count - 1; i >= 0; i--)
				{
					ActorData actor = actorsInLaser[i];
					bool flag4;
					if (abilityUtil_Targeter.IsActorInTargetRange(actor, out flag4))
					{
						actorsInLaser.RemoveAt(i);
					}
				}
				flag3 = (abilityUtil_Targeter.GetNumActorsInRange() > 0);
				num4 -= abilityUtil_Targeter.GetNumActorsInRange();
			}
		}
		if (actorsInLaser.Contains(targetingActor))
		{
			actorsInLaser.Remove(targetingActor);
		}
		if (actorsInLaser.Count > num4)
		{
			actorsInLaser.RemoveRange(num4, actorsInLaser.Count - num4);
		}
		float num5 = (laserCoords.end - laserCoords.start).magnitude;
		if (currentTargetIndex == 0)
		{
			if (num5 < num2 * Board.Get().squareSize - 0.1f)
			{
				this.m_stoppedShort = true;
			}
		}
		num5 -= num3;
		if (currentTargetIndex == 0)
		{
			laserCoords.start = VectorUtils.GetAdjustedStartPosWithOffset(laserCoords.start, laserCoords.end, num3);
		}
		float num6 = num5;
		float num7 = 0f;
		float lengthInSquares = this.m_maxTotalDistance - num2 + 0.5f * num;
		if (this.m_highlights.IsNullOrEmpty<GameObject>())
		{
			this.m_highlights = new List<GameObject>(4);
			this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(num, num6, null));
			this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(num, num7, null));
			this.m_laserStartRect = this.m_highlights[0].GetComponent<UIRectangleCursor>();
			this.m_laserEndRect = this.m_highlights[1].GetComponent<UIRectangleCursor>();
			this.m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(lengthInSquares, 0.2f, true, Color.cyan));
			this.m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(lengthInSquares, 0.2f, true, Color.cyan));
			this.m_highlights[2].SetActive(flag2);
			this.m_highlights[3].SetActive(flag2);
		}
		bool flag5;
		if (num4 > 0)
		{
			flag5 = (actorsInLaser.Count == num4);
		}
		else
		{
			flag5 = false;
		}
		bool flag6 = flag5;
		if (flag6)
		{
			Vector3 travelBoardSquareWorldPosition = actorsInLaser[actorsInLaser.Count - 1].GetTravelBoardSquareWorldPosition();
			travelBoardSquareWorldPosition.y = laserCoords.start.y;
			num6 = (travelBoardSquareWorldPosition - laserCoords.start).magnitude;
			if (this.m_startFadeAtActorRadius)
			{
				num6 -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.SquareSizeStatic;
			}
			num7 = num5 - num6;
			if (this.m_laserStartRect != null)
			{
				if (num7 < this.m_laserStartRect.m_lengthPerCorner)
				{
					flag6 = false;
					num6 = num5;
				}
				else
				{
					num7 += this.m_laserStartRect.m_distCasterToInterior;
					if (currentTargetIndex != 0)
					{
						num6 += num3 + this.m_laserStartRect.m_distCasterToStart;
					}
					num6 += this.m_laserStartRect.m_lengthPerCorner;
				}
			}
		}
		HighlightUtils.Get().ResizeRectangularCursor(num, num6, this.m_highlights[0]);
		this.m_laserStartRect.SetRectangleEndVisible(!flag6);
		this.m_laserEndRect.SetRectangleStartVisible(!flag6);
		this.m_highlights[1].SetActive(flag6);
		if (flag6)
		{
			HighlightUtils.Get().ResizeRectangularCursor(num, num7, this.m_highlights[1]);
			this.m_highlightsToFade.Add(1);
		}
		Vector3 normalized = (laserCoords.end - laserCoords.start).normalized;
		this.m_highlights[0].transform.position = laserCoords.start + new Vector3(0f, y, 0f);
		this.m_highlights[0].transform.rotation = Quaternion.LookRotation(normalized);
		if (flag6)
		{
			this.m_highlights[1].transform.position = laserCoords.start + normalized * (num6 - this.m_laserEndRect.m_lengthPerCorner - this.m_laserEndRect.m_distCasterToInterior) + new Vector3(0f, y, 0f);
			this.m_highlights[1].transform.rotation = Quaternion.LookRotation(normalized);
		}
		if (num4 > 0)
		{
			int num8 = 0;
			for (int j = 0; j < actorsInLaser.Count; j++)
			{
				ActorData actorData = actorsInLaser[j];
				Vector3 vector2 = laserCoords.start;
				if (currentTargetIndex > 0)
				{
					if (Board.Get().GetBoardSquare(vector2) == actorData.GetCurrentBoardSquare())
					{
						vector2 = targetingActor.GetTravelBoardSquareWorldPositionForLos();
					}
				}
				base.AddActorInRange(actorData, vector2, targetingActor, AbilityTooltipSubject.Primary, false);
				if (!flag3 && j == 0)
				{
					base.AddActorInRange(actorData, vector2, targetingActor, AbilityTooltipSubject.Near, true);
				}
				if (currentTargetIndex > 0)
				{
					base.SetIgnoreCoverMinDist(actorData, true);
				}
				list.Add(actorData);
				this.m_ordererdHitActors.Add(actorData);
				num8++;
			}
			if (this.m_affectsTargetingActor)
			{
				base.AddActorInRange(targetingActor, laserCoords.start, targetingActor, AbilityTooltipSubject.Secondary, false);
			}
		}
		else
		{
			this.m_highlightsToFade.Add(0);
		}
		if (flag2)
		{
			GameObject gameObject = this.m_highlights[2];
			GameObject gameObject2 = this.m_highlights[3];
			Vector3 aimDirection2 = currentTarget.AimDirection;
			aimDirection2.y = 0f;
			if (aimDirection2.magnitude > 0f)
			{
				if (!this.m_stoppedShort)
				{
					gameObject.SetActive(true);
					gameObject2.SetActive(true);
					float num9 = VectorUtils.HorizontalAngle_Deg(aimDirection2);
					float angle = num9 + this.m_maxBendAngle;
					float angle2 = num9 - this.m_maxBendAngle;
					Vector3 vector3 = laserCoords.end;
					vector3 -= aimDirection2 * ((num + 0.2f) * 0.5f);
					vector3.y = HighlightUtils.GetHighlightHeight();
					gameObject.transform.position = vector3;
					gameObject2.transform.position = vector3;
					gameObject.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle));
					gameObject2.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle2));
					HighlightUtils.Get().AdjustDynamicLineSegmentLength(gameObject, lengthInSquares);
					HighlightUtils.Get().AdjustDynamicLineSegmentLength(gameObject2, lengthInSquares);
					goto IL_A33;
				}
			}
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
		}
		IL_A33:
		this.DrawInvalidSquareIndicators(currentTarget, targetingActor, laserCoords.start, laserCoords.end);
	}

	public override void AdjustOpacityWhileTargeting()
	{
		base.AdjustOpacityWhileTargeting();
		if (!this.m_highlights.IsNullOrEmpty<GameObject>())
		{
			using (List<int>.Enumerator enumerator = this.m_highlightsToFade.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int index = enumerator.Current;
					AbilityUtil_Targeter.SetTargeterHighlightOpacity(new List<GameObject>
					{
						this.m_highlights[index]
					}, 0.06f);
				}
			}
		}
	}

	public override void UpdateConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateConfirmedTargeting(currentTarget, targetingActor);
		if (!this.m_highlights.IsNullOrEmpty<GameObject>())
		{
			using (List<int>.Enumerator enumerator = this.m_highlightsToFade.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int index = enumerator.Current;
					AbilityUtil_Targeter.SetTargeterHighlightOpacity(new List<GameObject>
					{
						this.m_highlights[index]
					}, 0.06f);
				}
			}
		}
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, startPos, endPos, this.m_width, targetingActor, this.m_penetrateLoS, null, null, true);
			base.HideUnusedSquareIndicators();
		}
	}
}
