using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_CapsuleAoE : AbilityUtil_Targeter
{
	private float m_radiusAroundStart = 2f;

	private float m_radiusAroundEnd = 2f;

	private float m_rangeFromLine = 2f;

	private bool m_penetrateLoS;

	public bool UseEndPosAsDamageOriginIfOverlap;

	public AbilityUtil_Targeter_CapsuleAoE.StartSquareDelegate GetDefaultStartSquare;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_CapsuleAoE(Ability ability, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS) : base(ability)
	{
		this.m_radiusAroundStart = radiusAroundStart;
		this.m_radiusAroundEnd = radiusAroundEnd;
		this.m_rangeFromLine = rangeFromDir;
		this.m_penetrateLoS = penetrateLoS;
		this.m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
		bool shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_CapsuleAoE..ctor(Ability, float, float, float, int, bool, bool)).MethodHandle;
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

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		BoardSquare boardSquare2;
		if (currentTargetIndex > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_CapsuleAoE.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
			boardSquare2 = Board.\u000E().\u000E(targets[currentTargetIndex - 1].GridPos);
		}
		else
		{
			boardSquare2 = boardSquare;
		}
		BoardSquare boardSquare3 = boardSquare2;
		if (this.m_ability.GetExpectedNumberOfTargeters() == 1)
		{
			boardSquare3 = targetingActor.\u0012();
		}
		else if (this.m_ability.GetExpectedNumberOfTargeters() == 0)
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
			if (this.GetDefaultStartSquare != null)
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
				boardSquare3 = this.GetDefaultStartSquare();
			}
		}
		Vector3 vector = boardSquare3.ToVector3();
		Vector3 vector2 = boardSquare.ToVector3();
		bool flag = this.m_rangeFromLine > 0f;
		bool flag2 = this.m_radiusAroundStart > 0f;
		bool flag3 = this.m_radiusAroundEnd > 0f;
		if (flag)
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
			float widthInSquares = this.m_rangeFromLine * 2f;
			if (this.m_highlights.Count == 0)
			{
				GameObject item = TargeterUtils.CreateLaserBoxHighlight(vector, vector2, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
				this.m_highlights.Add(item);
			}
			if (vector == vector2)
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
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_highlights[0].SetActive(false);
					}
				}
			}
			else
			{
				if (this.m_highlights.Count > 0 && this.m_highlights[0] != null)
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
					this.m_highlights[0].SetActive(true);
				}
				TargeterUtils.RefreshLaserBoxHighlight(this.m_highlights[0], vector, vector2, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
			}
		}
		if (flag2)
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
			if (this.m_highlights.Count != 1)
			{
				if (this.m_highlights.Count != 0 || flag)
				{
					goto IL_25E;
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
			GameObject item2 = TargeterUtils.CreateCircleHighlight(vector, this.m_radiusAroundStart, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
			this.m_highlights.Add(item2);
			IL_25E:
			int index = (!flag) ? 0 : 1;
			TargeterUtils.RefreshCircleHighlight(this.m_highlights[index], vector, TargeterUtils.HeightAdjustType.FromPathArrow);
		}
		if (flag3)
		{
			if (this.m_highlights.Count == 2)
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
				if (flag2)
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
					if (flag)
					{
						goto IL_31B;
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
			if (this.m_highlights.Count == 1)
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
				if (flag2 ^ flag)
				{
					goto IL_31B;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (this.m_highlights.Count != 0)
			{
				goto IL_34F;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (flag2)
			{
				goto IL_34F;
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
			if (flag)
			{
				goto IL_34F;
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
			IL_31B:
			GameObject item3 = TargeterUtils.CreateCircleHighlight(vector2, this.m_radiusAroundEnd, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
			this.m_highlights.Add(item3);
			IL_34F:
			int num = 0;
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
				num++;
			}
			if (flag2)
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
				num++;
			}
			TargeterUtils.RefreshCircleHighlight(this.m_highlights[num], vector2, TargeterUtils.HeightAdjustType.FromPathArrow);
		}
		List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(vector, vector2, this.m_radiusAroundStart, this.m_radiusAroundEnd, this.m_rangeFromLine, this.m_penetrateLoS, targetingActor, null, null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadiusOfLine);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInRadiusOfLine, targetingActor.\u0016());
		foreach (ActorData actorData in actorsInRadiusOfLine)
		{
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
				Vector3 damageOrigin = vector;
				if (this.UseEndPosAsDamageOriginIfOverlap)
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
					Vector3 a = actorData.\u0016();
					a.y = vector2.y;
					if ((a - vector2).sqrMagnitude <= Mathf.Epsilon)
					{
						damageOrigin = vector2;
					}
				}
				base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
				if (this.m_radiusAroundStart > 0f)
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
					float num2 = VectorUtils.HorizontalPlaneDistInSquares(vector, actorData.\u0016());
					if (num2 <= this.m_radiusAroundStart * Board.\u000E().squareSize)
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
						base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Near, true);
					}
				}
				if (this.m_radiusAroundEnd > 0f)
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
					float num3 = VectorUtils.HorizontalPlaneDistInSquares(vector2, actorData.\u0016());
					if (num3 <= this.m_radiusAroundEnd * Board.\u000E().squareSize)
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
			}
		}
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
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
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInRadiusOfLine(this.m_indicatorHandler, vector, vector2, this.m_radiusAroundStart, this.m_radiusAroundEnd, this.m_rangeFromLine, this.m_penetrateLoS, targetingActor);
			base.HideUnusedSquareIndicators();
		}
	}

	public delegate BoardSquare StartSquareDelegate();
}
