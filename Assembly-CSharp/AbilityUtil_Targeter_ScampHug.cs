using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ScampHug : AbilityUtil_Targeter
{
	private float m_dashWidthInSquares;

	public float m_dashRangeInSquares;

	private AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	private bool m_directHitIgnoreCover;

	public AbilityUtil_Targeter_ScampHug.IsAffectingCasterDelegate m_affectCasterDelegate;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private ScampHug.TargetingMode m_targetingMode;

	private Scamp_SyncComponent m_syncComp;

	private float m_enemyKnockbackDist;

	private KnockbackType m_enemyKnockbackType;

	public AbilityUtil_Targeter_ScampHug(Ability ability, Scamp_SyncComponent syncComp, ScampHug.TargetingMode targetingMode, float dashWidthInSquares, float dashRangeInSquares, AbilityAreaShape aoeShape, bool directHitIgnoreCover, float enemyKnockbackDist, KnockbackType enemyKnockbackType) : base(ability)
	{
		this.m_syncComp = syncComp;
		this.m_targetingMode = targetingMode;
		this.m_dashWidthInSquares = dashWidthInSquares;
		this.m_dashRangeInSquares = dashRangeInSquares;
		this.m_aoeShape = aoeShape;
		this.m_directHitIgnoreCover = directHitIgnoreCover;
		this.m_enemyKnockbackDist = enemyKnockbackDist;
		this.m_enemyKnockbackType = enemyKnockbackType;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public int LastUpdatePathSquareCount { get; set; }

	private bool IsInKnockbackMode()
	{
		bool result;
		if (this.m_syncComp != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ScampHug.IsInKnockbackMode()).MethodHandle;
			}
			result = this.m_syncComp.m_suitWasActiveOnTurnStart;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		this.LastUpdatePathSquareCount = 0;
		if (this.m_highlights != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ScampHug.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (this.m_highlights.Count >= 3)
			{
				goto IL_CD;
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
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_aoeShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
		IL_CD:
		GameObject gameObject = this.m_highlights[0];
		GameObject gameObject2 = this.m_highlights[1];
		GameObject gameObject3 = this.m_highlights[2];
		if (this.IsInKnockbackMode())
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
			bool flag = this.m_targetingMode == ScampHug.TargetingMode.Laser;
			ActorData actorData;
			List<ActorData> list;
			BoardSquare boardSquare;
			ScampHug.GetHitActorsAndKnockbackDestinationStatic(currentTarget, targetingActor, this.m_targetingMode, false, this.m_dashWidthInSquares, this.m_dashRangeInSquares, this.m_aoeShape, out actorData, out list, out boardSquare);
			bool active = false;
			Vector3 damageOrigin = Vector3.zero;
			if (actorData != null)
			{
				Vector3 vector;
				if (this.m_directHitIgnoreCover)
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
					vector = actorData.\u0016();
				}
				else
				{
					vector = targetingActor.\u0016();
				}
				Vector3 damageOrigin2 = vector;
				base.AddActorInRange(actorData, damageOrigin2, targetingActor, AbilityTooltipSubject.Primary, false);
				BoardSquare boardSquare2 = actorData.\u0012();
				active = true;
				Vector3 position = boardSquare2.ToVector3();
				position.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = position;
				damageOrigin = actorData.\u0012().ToVector3();
			}
			else if (!flag)
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
				active = true;
				Vector3 position2 = boardSquare.ToVector3();
				position2.y = HighlightUtils.GetHighlightHeight();
				gameObject2.transform.position = position2;
				damageOrigin = boardSquare.ToVector3();
			}
			using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actor = enumerator.Current;
					base.AddActorInRange(actor, damageOrigin, targetingActor, AbilityTooltipSubject.Secondary, false);
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (this.m_affectCasterDelegate != null)
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
				if (this.m_affectCasterDelegate(targetingActor, this.GetVisibleActorsInRange()))
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
					base.AddActorInRange(targetingActor, targetingActor.\u0016(), targetingActor, AbilityTooltipSubject.Self, false);
				}
			}
			gameObject2.SetActive(active);
			Vector3 vector2 = targetingActor.\u0015();
			Vector3 vector3 = boardSquare.ToVector3();
			Vector3 aimDir = vector3 - vector2;
			aimDir.y = 0f;
			float distance = aimDir.magnitude / Board.SquareSizeStatic;
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildKnockbackPath(targetingActor, KnockbackType.ForwardAlongAimDir, aimDir, vector3, distance);
			int num = 0;
			base.EnableAllMovementArrows();
			num = base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
			if (this.m_enemyKnockbackDist > 0f)
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
				if (actorData != null)
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
					if (!list.Contains(actorData))
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
						list.Add(actorData);
					}
				}
				using (List<ActorData>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ActorData actorData2 = enumerator2.Current;
						Vector3 aimDir2 = actorData2.\u0016() - vector3;
						aimDir.y = 0f;
						if (aimDir.sqrMagnitude > 0f)
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
							aimDir.Normalize();
						}
						BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData2, this.m_enemyKnockbackType, aimDir2, vector3, this.m_enemyKnockbackDist);
						num = base.AddMovementArrowWithPrevious(actorData2, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
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
				}
			}
			base.SetMovementArrowEnabledFromIndex(num, false);
			if (boardSquarePathInfo != null)
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
				this.LastUpdatePathSquareCount = boardSquarePathInfo.GetNumSquaresToEnd(true);
			}
			if (flag)
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
				Vector3 a = vector3;
				if (boardSquarePathInfo != null)
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
					BoardSquarePathInfo pathEndpoint = boardSquarePathInfo.GetPathEndpoint();
					a = pathEndpoint.square.ToVector3();
				}
				Vector3 lhs = a - vector2;
				lhs.y = 0f;
				float d = Vector3.Dot(lhs, currentTarget.AimDirection) + 0.5f;
				Vector3 endPos = vector2 + d * currentTarget.AimDirection;
				endPos.y = HighlightUtils.GetHighlightHeight();
				HighlightUtils.Get().RotateAndResizeRectangularCursor(gameObject, vector2, endPos, this.m_dashWidthInSquares);
			}
			else
			{
				gameObject3.transform.position = gameObject2.transform.position;
			}
			gameObject.SetActive(flag);
			gameObject3.SetActive(!flag);
		}
		else
		{
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
			BoardSquare boardSquare3 = Board.\u000E().\u000E(currentTarget.GridPos);
			if (boardSquare3 != null)
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
				Vector3 position3 = boardSquare3.ToVector3();
				position3.y = HighlightUtils.GetHighlightHeight();
				gameObject3.transform.position = position3;
				gameObject3.SetActive(true);
				BoardSquarePathInfo path2 = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare3, targetingActor.\u0012(), false);
				base.EnableAllMovementArrows();
				int fromIndex = base.AddMovementArrowWithPrevious(targetingActor, path2, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
				base.SetMovementArrowEnabledFromIndex(fromIndex, false);
			}
			else
			{
				gameObject3.SetActive(false);
			}
		}
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);
}
