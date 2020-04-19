using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_ThiefFanLaser : AbilityUtil_Targeter
{
	private float m_minAngle;

	private float m_maxAngle;

	protected float m_fixedAngleInBetween = 10f;

	protected bool m_changeAngleByCursorDist = true;

	protected float m_interpMinDistanceInSquares;

	protected float m_interpMaxDistanceInSquares;

	protected float m_interpStep;

	protected float m_startAngleOffset;

	protected int m_count;

	public float m_rangeInSquares;

	protected float m_widthInSquares;

	private int m_maxTargets;

	protected bool m_penetrateLos;

	private bool m_highlightPowerup;

	private bool m_stopOnPowerUp;

	private bool m_includeSpoils;

	private bool m_pickUpIgnoreTeamRestriction;

	private bool m_useHitActorPosForLaserEnd = true;

	private int m_maxPowerupCount;

	protected List<Vector3> m_laserEndPoints;

	public HashSet<PowerUp> m_powerupsHitSoFar;

	public List<bool> m_hitActorInLaser;

	public List<bool> m_hitPowerupInLaser;

	public int m_lastNumAlliesHit;

	public int m_lastNumEnemiesHit;

	public AbilityUtil_Targeter_ThiefFanLaser.IsAffectingCasterDelegate m_affectCasterDelegate;

	private List<ActorData> m_actorsHitSoFar = new List<ActorData>();

	public AbilityUtil_Targeter_ThiefFanLaser.CustomDamageOriginDelegate m_customDamageOriginDelegate;

	public AbilityUtil_Targeter_ThiefFanLaser.LaserLengthDelegate m_delegateLaserLength;

	public AbilityUtil_Targeter_ThiefFanLaser.LaserWidthDelegate m_delegateLaserWidth;

	private TargeterPart_Laser m_laserPart;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public Dictionary<ActorData, int> m_actorToHitCount = new Dictionary<ActorData, int>();

	public AbilityUtil_Targeter_ThiefFanLaser(Ability ability, float minAngle, float maxAngle, float angleInterpMinDistance, float angleInterpMaxDistance, float rangeInSquares, float widthInSquares, int maxTargets, int count, bool penetrateLos, bool highlightPowerup, bool stopOnPowerUp, bool includeSpoils, bool pickUpIgnoreTeamRestriction, int maxPowerupCount, float interpStep = 0f, float startAngleOffset = 0f) : base(ability)
	{
		this.m_minAngle = Mathf.Max(0f, minAngle);
		this.m_maxAngle = maxAngle;
		this.m_interpMinDistanceInSquares = angleInterpMinDistance;
		this.m_interpMaxDistanceInSquares = angleInterpMaxDistance;
		this.m_interpStep = interpStep;
		this.m_startAngleOffset = startAngleOffset;
		this.m_rangeInSquares = rangeInSquares;
		this.m_widthInSquares = widthInSquares;
		this.m_count = count;
		this.m_maxTargets = maxTargets;
		this.m_penetrateLos = penetrateLos;
		this.m_highlightPowerup = highlightPowerup;
		this.m_stopOnPowerUp = stopOnPowerUp;
		this.m_includeSpoils = includeSpoils;
		this.m_pickUpIgnoreTeamRestriction = pickUpIgnoreTeamRestriction;
		this.m_maxPowerupCount = maxPowerupCount;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_powerupsHitSoFar = new HashSet<PowerUp>();
		this.m_hitActorInLaser = new List<bool>();
		this.m_hitPowerupInLaser = new List<bool>();
		this.m_laserPart = new TargeterPart_Laser(this.m_widthInSquares, this.m_rangeInSquares, this.m_penetrateLos, this.m_maxTargets);
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		for (int i = 0; i < this.m_count; i++)
		{
			SquareInsideChecker_Box item = new SquareInsideChecker_Box(this.m_widthInSquares);
			this.m_squarePosCheckerList.Add(item);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ThiefFanLaser..ctor(Ability, float, float, float, float, float, float, int, int, bool, bool, bool, bool, bool, int, float, float)).MethodHandle;
		}
	}

	public void SetIncludeTeams(bool includeAllies, bool includeEnemies, bool includeSelf = false)
	{
		this.m_affectsAllies = includeAllies;
		this.m_affectsEnemies = includeEnemies;
		this.m_affectsTargetingActor = includeSelf;
	}

	public void SetFixedAngle(bool changeAngleByCursorDist, float fixedAngle)
	{
		this.m_changeAngleByCursorDist = changeAngleByCursorDist;
		this.m_fixedAngleInBetween = fixedAngle;
	}

	public void SetUseHitActorPosForLaserEnd(bool val)
	{
		this.m_useHitActorPosForLaserEnd = val;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		this.m_actorToHitCount.Clear();
		this.m_powerupsHitSoFar.Clear();
		this.m_hitActorInLaser.Clear();
		this.m_hitPowerupInLaser.Clear();
		base.ResetSquareIndicatorIndexToUse();
		this.m_lastNumAlliesHit = 0;
		this.m_lastNumEnemiesHit = 0;
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= this.m_count)
			{
				goto IL_D3;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ThiefFanLaser.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_laserEndPoints = new List<Vector3>();
		for (int i = 0; i < this.m_count; i++)
		{
			this.m_highlights.Add(this.m_laserPart.CreateHighlightObject(this));
			this.m_laserEndPoints.Add(currentTarget.FreePos);
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		IL_D3:
		float num = this.m_fixedAngleInBetween;
		if (this.m_changeAngleByCursorDist)
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
			float num2;
			if (this.m_count > 1)
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
				num2 = this.CalculateFanAngleDegrees(currentTarget, targetingActor, this.m_interpStep);
			}
			else
			{
				num2 = 0f;
			}
			float num3 = num2;
			float num4;
			if (this.m_count > 1)
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
				num4 = num3 / (float)(this.m_count - 1);
			}
			else
			{
				num4 = 0f;
			}
			num = num4;
		}
		float num5 = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection) + this.m_startAngleOffset;
		float num6 = num5 - 0.5f * (float)(this.m_count - 1) * num;
		if (this.m_affectsTargetingActor)
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
			base.AddActorInRange(targetingActor, targetingActor.\u0016(), targetingActor, AbilityTooltipSubject.Self, false);
		}
		bool flag = false;
		bool flag2 = false;
		for (int j = 0; j < this.m_count; j++)
		{
			Vector3 vector = VectorUtils.AngleDegreesToVector(num6 + (float)j * num);
			Vector3 vector2 = targetingActor.\u0015();
			VectorUtils.LaserCoords laserCoords;
			List<PowerUp> list;
			List<ActorData> laserHitActors = this.GetLaserHitActors(vector2, vector, targetingActor, out laserCoords, out list);
			flag2 = (laserHitActors.Count > 0);
			flag = (list.Count > 0);
			using (List<ActorData>.Enumerator enumerator = laserHitActors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					if (actorData.\u000E() == targetingActor.\u000E())
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
						this.m_lastNumAlliesHit++;
					}
					else
					{
						this.m_lastNumEnemiesHit++;
					}
					Vector3 vector3 = laserCoords.start;
					if (this.m_customDamageOriginDelegate != null)
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
						vector3 = this.m_customDamageOriginDelegate(actorData, targetingActor, vector3);
					}
					base.AddActorInRange(actorData, vector3, targetingActor, AbilityTooltipSubject.Primary, true);
					ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
					actorHitContext.\u001D = vector3;
					if (this.m_actorToHitCount.ContainsKey(actorData))
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
						Dictionary<ActorData, int> actorToHitCount;
						ActorData key;
						(actorToHitCount = this.m_actorToHitCount)[key = actorData] = actorToHitCount[key] + 1;
					}
					else
					{
						this.m_actorToHitCount[actorData] = 1;
					}
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_hitPowerupInLaser.Add(flag);
			this.m_hitActorInLaser.Add(flag2);
			this.m_laserEndPoints[j] = laserCoords.end;
			GameObject highlightObj = this.m_highlights[j];
			float magnitude = (laserCoords.end - laserCoords.start).magnitude;
			this.m_laserPart.AdjustHighlight(highlightObj, vector2, vector2 + magnitude * vector, true);
			this.UpdateLaserEndPointsForHiddenSquares(laserCoords.start, laserCoords.end, j, targetingActor);
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		int u001D = ContextKeys.\u0019.\u0012();
		int u001D2 = ContextKeys.\u001A.\u0012();
		using (Dictionary<ActorData, int>.Enumerator enumerator2 = this.m_actorToHitCount.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<ActorData, int> keyValuePair = enumerator2.Current;
				ActorHitContext actorHitContext2 = this.m_actorContextVars[keyValuePair.Key];
				actorHitContext2.\u0015.\u0016(u001D, keyValuePair.Value);
				actorHitContext2.\u0015.\u0016(u001D2, 0);
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.HandlePowerupHighlight(targetingActor, this.m_count);
		bool flag3;
		if (this.m_affectCasterDelegate == null)
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
			flag3 = this.m_affectsTargetingActor;
		}
		else
		{
			flag3 = this.m_affectCasterDelegate(targetingActor, flag2, flag);
		}
		if (flag3)
		{
			base.AddActorInRange(targetingActor, targetingActor.\u0016(), targetingActor, AbilityTooltipSubject.Self, true);
		}
		if (this.ShouldShowHiddenSquareIndicator(targetingActor))
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
			this.HandleShowHiddenSquares(targetingActor);
		}
		base.HideUnusedSquareIndicators();
	}

	protected virtual void UpdateLaserEndPointsForHiddenSquares(Vector3 startPos, Vector3 endPos, int index, ActorData targetingActor)
	{
		SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[index] as SquareInsideChecker_Box;
		squareInsideChecker_Box.UpdateBoxProperties(startPos, endPos, targetingActor);
	}

	protected virtual void HandleShowHiddenSquares(ActorData targetingActor)
	{
		for (int i = 0; i < this.m_squarePosCheckerList.Count; i++)
		{
			SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[i] as SquareInsideChecker_Box;
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, squareInsideChecker_Box.GetStartPos(), squareInsideChecker_Box.GetEndPos(), this.m_widthInSquares, targetingActor, this.m_penetrateLos, null, this.m_squarePosCheckerList, true);
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ThiefFanLaser.HandleShowHiddenSquares(ActorData)).MethodHandle;
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> previousTargets)
	{
		base.ClearActorsInRange();
		this.m_powerupsHitSoFar.Clear();
		this.m_hitActorInLaser.Clear();
		this.m_hitPowerupInLaser.Clear();
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 1)
			{
				goto IL_76;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ThiefFanLaser.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(this.m_laserPart.CreateHighlightObject(this));
		IL_76:
		Vector3 vector = currentTarget.AimDirection;
		if (currentTargetIndex > 0)
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
			if (this.m_maxAngle > 0f)
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
				Vector3 aimDirection = previousTargets[currentTargetIndex - 1].AimDirection;
				float num = Vector3.Angle(vector, aimDirection);
				if (num > this.m_maxAngle)
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
					vector = Vector3.RotateTowards(vector, aimDirection, 0.0174532924f * (num - this.m_maxAngle), 0f);
				}
			}
		}
		Vector3 vector2 = targetingActor.\u0015();
		VectorUtils.LaserCoords laserCoords;
		List<PowerUp> list;
		List<ActorData> laserHitActors = this.GetLaserHitActors(vector2, vector, targetingActor, out laserCoords, out list);
		using (List<ActorData>.Enumerator enumerator = laserHitActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actor = enumerator.Current;
				base.AddActorInRange(actor, laserCoords.start, targetingActor, AbilityTooltipSubject.Primary, true);
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		bool flag;
		if (this.m_affectCasterDelegate == null)
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
			flag = this.m_affectsTargetingActor;
		}
		else
		{
			flag = this.m_affectCasterDelegate(targetingActor, laserHitActors.Count > 0, list.Count > 0);
		}
		if (flag)
		{
			base.AddActorInRange(targetingActor, targetingActor.\u0016(), targetingActor, AbilityTooltipSubject.Self, true);
		}
		this.m_hitActorInLaser.Add(laserHitActors.Count > 0);
		this.m_hitPowerupInLaser.Add(list.Count > 0);
		float num2 = this.m_rangeInSquares;
		float num3 = this.m_widthInSquares;
		if (this.m_delegateLaserLength != null)
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
			num2 = this.m_delegateLaserLength(targetingActor, num2);
		}
		if (this.m_delegateLaserWidth != null)
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
			num3 = this.m_delegateLaserWidth(targetingActor, num3);
		}
		this.m_laserPart.UpdateDimensions(num3, num2);
		GameObject highlightObj = this.m_highlights[0];
		float magnitude = (laserCoords.end - laserCoords.start).magnitude;
		this.m_laserPart.AdjustHighlight(highlightObj, vector2, vector2 + magnitude * vector, true);
		this.HandlePowerupHighlight(targetingActor, 1);
		if (this.ShouldShowHiddenSquareIndicator(targetingActor))
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
			base.ResetSquareIndicatorIndexToUse();
			this.m_laserPart.ShowHiddenSquares(this.m_indicatorHandler, laserCoords.start, laserCoords.end, targetingActor, this.m_penetrateLos);
			base.HideUnusedSquareIndicators();
		}
	}

	private float CalculateFanAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor, float interpStep)
	{
		float value = (currentTarget.FreePos - targetingActor.\u0016()).magnitude / Board.\u000E().squareSize;
		float num = Mathf.Clamp(value, this.m_interpMinDistanceInSquares, this.m_interpMaxDistanceInSquares) - this.m_interpMinDistanceInSquares;
		if (interpStep > 0f)
		{
			float num2 = num % interpStep;
			num -= num2;
		}
		return Mathf.Max(this.m_minAngle, this.m_maxAngle * (1f - num / (this.m_interpMaxDistanceInSquares - this.m_interpMinDistanceInSquares)));
	}

	private void HandlePowerupHighlight(ActorData targetingActor, int startingFromIndex)
	{
		int num = startingFromIndex;
		if (this.m_highlightPowerup)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ThiefFanLaser.HandlePowerupHighlight(ActorData, int)).MethodHandle;
			}
			using (HashSet<PowerUp>.Enumerator enumerator = this.m_powerupsHitSoFar.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PowerUp powerUp = enumerator.Current;
					if (this.m_highlights.Count <= num)
					{
						this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
					}
					Vector3 position = powerUp.boardSquare.ToVector3();
					position.y = HighlightUtils.GetHighlightHeight();
					this.m_highlights[num].transform.position = position;
					this.m_highlights[num].SetActive(true);
					num++;
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			for (int i = num; i < this.m_highlights.Count; i++)
			{
				this.m_highlights[i].SetActive(false);
			}
		}
	}

	private unsafe List<ActorData> GetLaserHitActors(Vector3 startPos, Vector3 direction, ActorData targetingActor, out VectorUtils.LaserCoords coords, out List<PowerUp> powerupsHit)
	{
		float num = this.m_rangeInSquares;
		float num2 = this.m_widthInSquares;
		if (this.m_delegateLaserLength != null)
		{
			num = this.m_delegateLaserLength(targetingActor, num);
		}
		if (this.m_delegateLaserWidth != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ThiefFanLaser.GetLaserHitActors(Vector3, Vector3, ActorData, VectorUtils.LaserCoords*, List<PowerUp>*)).MethodHandle;
			}
			num2 = this.m_delegateLaserWidth(targetingActor, num2);
		}
		return ThiefBasicAttack.GetHitActorsInDirectionStatic(startPos, direction, targetingActor, num, num2, this.m_penetrateLos, this.m_maxTargets, this.m_affectsAllies, this.m_affectsEnemies, false, this.m_maxPowerupCount, this.m_highlightPowerup, this.m_stopOnPowerUp, this.m_includeSpoils, this.m_pickUpIgnoreTeamRestriction, this.m_powerupsHitSoFar, out coords, out powerupsHit, null, true, this.m_useHitActorPosForLaserEnd);
	}

	private bool ShouldShowHiddenSquareIndicator(ActorData targetingActor)
	{
		return targetingActor == GameFlowData.Get().activeOwnedActorData;
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, bool hitEnemy, bool powerupHit);

	public delegate Vector3 CustomDamageOriginDelegate(ActorData potentialActor, ActorData caster, Vector3 defaultPos);

	public delegate float LaserLengthDelegate(ActorData caster, float baseLength);

	public delegate float LaserWidthDelegate(ActorData caster, float baseWidth);
}
