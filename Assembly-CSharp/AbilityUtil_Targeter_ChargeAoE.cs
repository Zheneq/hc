using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_ChargeAoE : AbilityUtil_Targeter
{
	protected float m_radiusAroundStart = 2f;

	protected float m_radiusAroundEnd = 2f;

	protected float m_rangeFromLine = 2f;

	protected bool m_penetrateLoS;

	protected int m_maxTargets;

	public bool AllowChargeThroughInvalidSquares;

	public bool ShowTeleportLines;

	public bool SkipEvadeMovementLines;

	public bool ForceAddTargetingActor;

	public bool UseEndPosAsDamageOriginIfOverlap;

	public bool TrimPathOnTargetHit;

	public bool UseLineColorOverride;

	public Color LineColorOverride = Color.white;

	public AbilityUtil_Targeter_ChargeAoE.ShouldAddActorDelegate m_shouldAddTargetDelegate;

	public AbilityUtil_Targeter_ChargeAoE.ShouldAddCasterDelegate m_shouldAddCasterDelegate;

	public List<ActorData> OrderedHitActors = new List<ActorData>();

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_ChargeAoE(Ability ability, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS) : base(ability)
	{
		this.m_radiusAroundStart = radiusAroundStart;
		this.m_radiusAroundEnd = radiusAroundEnd;
		this.m_rangeFromLine = rangeFromDir;
		this.m_penetrateLoS = penetrateLoS;
		this.m_maxTargets = maxTargets;
		this.m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
		bool shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ChargeAoE..ctor(Ability, float, float, float, int, bool, bool)).MethodHandle;
			}
			shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		}
		else
		{
			shouldShowActorRadius = true;
		}
		this.m_shouldShowActorRadius = shouldShowActorRadius;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	protected virtual bool UseRadiusAroundLine(AbilityTarget currentTarget)
	{
		return this.m_rangeFromLine > 0f;
	}

	protected virtual bool UseRadiusAroundStart(AbilityTarget currentTarget)
	{
		return this.m_radiusAroundStart > 0f;
	}

	protected virtual bool UseRadiusAroundEnd(AbilityTarget currentTarget)
	{
		return this.m_radiusAroundEnd > 0f;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		base.ClearActorsInRange();
		this.OrderedHitActors.Clear();
		BoardSquarePathInfo boardSquarePathInfo = null;
		float startRadiusInSquares = (!this.UseRadiusAroundStart(currentTarget)) ? 0f : this.m_radiusAroundStart;
		float num;
		if (this.UseRadiusAroundEnd(currentTarget))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ChargeAoE.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
			num = this.m_radiusAroundEnd;
		}
		else
		{
			num = 0f;
		}
		float endRadiusInSquares = num;
		float num2 = (!this.UseRadiusAroundLine(currentTarget)) ? 0f : this.m_rangeFromLine;
		BoardSquare boardSquare2 = null;
		if (boardSquare != null)
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
			if (currentTargetIndex != 0 && targets != null)
			{
				if (this.IsUsingMultiTargetUpdate())
				{
					goto IL_C4;
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
			boardSquare2 = targetingActor.\u0012();
			goto IL_FA;
		}
		IL_C4:
		if (boardSquare != null)
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
			boardSquare2 = Board.\u000E().\u000E(targets[currentTargetIndex - 1].GridPos);
		}
		IL_FA:
		List<Team> affectedTeams = base.GetAffectedTeams();
		if (boardSquare2 != null && boardSquare != null)
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
			if (this.TrimPathOnTargetHit)
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
				if (num2 > 0f)
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
					bool flag;
					Vector3 vector;
					Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(targetingActor, boardSquare2.ToVector3(), boardSquare.ToVector3(), out flag, out vector, null);
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
						boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(boardSquare2.ToVector3(), abilityLineEndpoint, false, false, float.MaxValue);
					}
				}
			}
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, boardSquare2, this.AllowChargeThroughInvalidSquares);
		}
		if (boardSquarePathInfo != null)
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
			if (boardSquarePathInfo.next != null)
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
				if (this.TrimPathOnTargetHit)
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
					if (num2 > 0f)
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
						BoardSquare boardSquare3;
						TargetSelect_ChargeAoE.TrimChargePathOnActorHit(boardSquarePathInfo, boardSquare2, num2, targetingActor, affectedTeams, false, out boardSquare3);
					}
				}
			}
		}
		if (!this.SkipEvadeMovementLines)
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
			if (this.ShowTeleportLines)
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
				base.InstantiateTeleportPathUIEffect();
				base.UpdateEffectOnCaster(currentTarget, targetingActor);
				base.UpdateTargetAreaEffect(currentTarget, targetingActor);
			}
			else if (this.UseLineColorOverride)
			{
				base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, this.LineColorOverride, 0, false);
			}
			else
			{
				base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
			}
		}
		List<ActorData> list = null;
		if (this.m_shouldAddCasterDelegate != null)
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
			list = new List<ActorData>();
		}
		if (boardSquarePathInfo != null)
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
			List<Vector3> list2 = KnockbackUtils.BuildDrawablePath(boardSquarePathInfo, true);
			if (list2.Count >= 2)
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
				Vector3 vector2 = list2[0];
				Vector3 vector3 = list2[list2.Count - 1];
				bool flag2 = this.UseRadiusAroundLine(currentTarget);
				bool flag3 = this.UseRadiusAroundStart(currentTarget);
				bool flag4 = this.UseRadiusAroundEnd(currentTarget);
				float widthInSquares = this.m_rangeFromLine * 2f;
				if (this.m_highlights.Count == 0)
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
					GameObject item = TargeterUtils.CreateLaserBoxHighlight(vector2, vector3, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
					this.m_highlights.Add(item);
					GameObject item2 = TargeterUtils.CreateCircleHighlight(vector2, this.m_radiusAroundStart, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
					this.m_highlights.Add(item2);
					GameObject item3 = TargeterUtils.CreateCircleHighlight(vector3, this.m_radiusAroundEnd, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
					this.m_highlights.Add(item3);
				}
				if (flag2)
				{
					if (vector2 == vector3)
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
						if (this.m_highlights.Count > 0)
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
							if (this.m_highlights[0] != null)
							{
								this.m_highlights[0].SetActive(false);
							}
						}
					}
					else
					{
						if (this.m_highlights.Count > 0)
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
							if (this.m_highlights[0] != null)
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
								this.m_highlights[0].SetActive(true);
							}
						}
						TargeterUtils.RefreshLaserBoxHighlight(this.m_highlights[0], vector2, vector3, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
					}
				}
				else if (this.m_highlights.Count > 0)
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
					if (this.m_highlights[0] != null)
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
						this.m_highlights[0].SetActive(false);
					}
				}
				if (flag3)
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
					this.m_highlights[1].SetActive(true);
					TargeterUtils.RefreshCircleHighlight(this.m_highlights[1], vector2, TargeterUtils.HeightAdjustType.FromPathArrow);
				}
				else if (this.m_highlights.Count > 1 && this.m_highlights[1] != null)
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
					this.m_highlights[1].SetActive(false);
				}
				if (flag4)
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
					this.m_highlights[2].SetActive(true);
					TargeterUtils.RefreshCircleHighlight(this.m_highlights[2], vector3, TargeterUtils.HeightAdjustType.FromPathArrow);
				}
				else if (this.m_highlights.Count > 2)
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
					if (this.m_highlights[2] != null)
					{
						this.m_highlights[2].SetActive(false);
					}
				}
				List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(vector2, vector3, startRadiusInSquares, endRadiusInSquares, num2, this.m_penetrateLoS, targetingActor, base.GetAffectedTeams(), null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadiusOfLine);
				TargeterUtils.SortActorsByDistanceToPos(ref actorsInRadiusOfLine, targetingActor.\u0016());
				TargeterUtils.LimitActorsToMaxNumber(ref actorsInRadiusOfLine, this.m_maxTargets);
				using (List<ActorData>.Enumerator enumerator = actorsInRadiusOfLine.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (base.GetAffectsTarget(actorData, targetingActor))
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
							if (this.m_shouldAddTargetDelegate != null)
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
								if (!this.m_shouldAddTargetDelegate(actorData, currentTarget, actorsInRadiusOfLine, targetingActor, this.m_ability))
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
									continue;
								}
							}
							Vector3 damageOrigin = vector2;
							if (this.UseEndPosAsDamageOriginIfOverlap)
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
								Vector3 a = actorData.\u0016();
								a.y = vector3.y;
								if ((a - vector3).sqrMagnitude <= Mathf.Epsilon)
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
									damageOrigin = vector3;
								}
							}
							base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
							this.OrderedHitActors.Add(actorData);
							if (this.m_shouldAddCasterDelegate != null)
							{
								list.Add(actorData);
							}
							if (this.UseRadiusAroundStart(currentTarget))
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
								BoardSquare testSquare = actorData.\u0012();
								if (AreaEffectUtils.IsSquareInConeByActorRadius(testSquare, vector2, 0f, 360f, this.m_radiusAroundStart, 0f, this.m_penetrateLoS, targetingActor, false, default(Vector3)))
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
									base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Far, true);
								}
							}
							bool flag5 = false;
							if (this.UseRadiusAroundEnd(currentTarget))
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
								BoardSquare testSquare2 = actorData.\u0012();
								if (AreaEffectUtils.IsSquareInConeByActorRadius(testSquare2, vector3, 0f, 360f, this.m_radiusAroundEnd, 0f, this.m_penetrateLoS, targetingActor, false, default(Vector3)))
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
									base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Far, true);
									flag5 = true;
								}
							}
							ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
							ContextVars u = actorHitContext.\u0015;
							int u001D = ContextKeys.\u0004.\u0012();
							int u000E;
							if (flag5)
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
								u000E = 1;
							}
							else
							{
								u000E = 0;
							}
							u.\u0016(u001D, u000E);
						}
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
				if (targetingActor == GameFlowData.Get().activeOwnedActorData)
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
					base.ResetSquareIndicatorIndexToUse();
					AreaEffectUtils.OperateOnSquaresInRadiusOfLine(this.m_indicatorHandler, vector2, vector3, startRadiusInSquares, endRadiusInSquares, num2, this.m_penetrateLoS, targetingActor);
					base.HideUnusedSquareIndicators();
				}
			}
		}
		if (!this.ForceAddTargetingActor)
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
			if (this.m_shouldAddCasterDelegate == null)
			{
				return;
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
			if (!this.m_shouldAddCasterDelegate(targetingActor, list))
			{
				return;
			}
		}
		base.AddActorInRange(targetingActor, targetingActor.\u0016(), targetingActor, AbilityTooltipSubject.Self, false);
	}

	public delegate bool ShouldAddActorDelegate(ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability);

	public delegate bool ShouldAddCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);
}
