using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_MultipleShapes : AbilityUtil_Targeter
{
	private List<AbilityUtil_Targeter_MultipleShapes.MultiShapeData> m_shapes;

	private const float c_heightOffset = 0.1f;

	private float m_curSpeed;

	public AbilityUtil_Targeter_MultipleShapes.IsAffectingCasterDelegate m_affectCasterDelegate;

	public AbilityUtil_Targeter_MultipleShapes.CenterSquareDelegate m_centerSquareDelegate;

	public bool m_alwaysIncludeShapeCenterActor;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<AbilityUtil_Targeter_MultipleShapes.HitActorContext> m_hitActorContext = new List<AbilityUtil_Targeter_MultipleShapes.HitActorContext>();

	public AbilityUtil_Targeter_MultipleShapes(Ability ability, List<AbilityUtil_Targeter_MultipleShapes.MultiShapeData> shapeDataList) : base(ability)
	{
		this.m_shapes = shapeDataList;
		this.m_shapes.Sort();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public AbilityUtil_Targeter_MultipleShapes(Ability ability, List<AbilityAreaShape> shapes, List<AbilityTooltipSubject> subjects, bool penetrateLoS, bool affectEnemies = true, bool affectAllies = false, bool affectSelf = false) : base(ability)
	{
		this.m_shapes = new List<AbilityUtil_Targeter_MultipleShapes.MultiShapeData>();
		for (int i = 0; i < shapes.Count; i++)
		{
			int index = Mathf.Min(i, subjects.Count - 1);
			AbilityTooltipSubject subjectEnemyInShape = subjects[index];
			AbilityUtil_Targeter_MultipleShapes.MultiShapeData multiShapeData = new AbilityUtil_Targeter_MultipleShapes.MultiShapeData();
			multiShapeData.m_shape = shapes[i];
			multiShapeData.m_subjectEnemyInShape = subjectEnemyInShape;
			multiShapeData.m_penetrateLoS = penetrateLoS;
			multiShapeData.m_affectEnemies = affectEnemies;
			multiShapeData.m_affectAllies = affectAllies;
			multiShapeData.m_affectSelf = affectSelf;
			this.m_shapes.Add(multiShapeData);
		}
		this.m_shapes.Sort();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public List<AbilityUtil_Targeter_MultipleShapes.HitActorContext> GetHitActorContext()
	{
		return this.m_hitActorContext;
	}

	private bool IsInputValid()
	{
		return this.m_shapes.Count > 0;
	}

	private BoardSquare GetTargetSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (this.m_centerSquareDelegate != null)
		{
			return this.m_centerSquareDelegate(currentTarget, targetingActor);
		}
		return Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
	}

	private Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor, AbilityAreaShape shape)
	{
		BoardSquare targetSquare = this.GetTargetSquare(currentTarget, targetingActor);
		if (targetSquare != null)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, currentTarget.FreePos, targetSquare);
			centerOfShape.y = targetingActor.GetTravelBoardSquareWorldPosition().y + 0.1f;
			return centerOfShape;
		}
		return Vector3.zero;
	}

	public override void UpdateHighlightPosAfterClick(AbilityTarget target, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (this.m_highlights != null && this.m_shapes != null)
		{
			int i = 0;
			while (i < this.m_highlights.Count)
			{
				if (i >= this.m_shapes.Count)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						return;
					}
				}
				else
				{
					AbilityAreaShape shape = this.m_shapes[i].m_shape;
					Vector3 highlightGoalPos = this.GetHighlightGoalPos(target, targetingActor, shape);
					this.m_highlights[i].transform.position = highlightGoalPos;
					i++;
				}
			}
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public unsafe void HandleKnockbackArrowForActor(ActorData knockbackTarget, Vector3 origin, AbilityTarget abilityTarget, AbilityUtil_Targeter_MultipleShapes.MultiShapeData data, ref int nextMovementArrowIndex)
	{
		if (data.m_knockbackEnemies)
		{
			BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(knockbackTarget, data.m_knockbackType, abilityTarget.AimDirection, origin, data.m_knockbackDistance);
			nextMovementArrowIndex = base.AddMovementArrowWithPrevious(knockbackTarget, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, nextMovementArrowIndex, false);
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		this.m_hitActorContext.Clear();
		base.ClearArrows();
		int num = 0;
		if (!this.IsInputValid())
		{
			return;
		}
		bool flag = GameFlowData.Get().activeOwnedActorData == targetingActor;
		if (flag)
		{
			base.ResetSquareIndicatorIndexToUse();
		}
		BoardSquare targetSquare = this.GetTargetSquare(currentTarget, targetingActor);
		if (targetSquare != null)
		{
			List<ActorData> list = new List<ActorData>();
			bool flag2 = false;
			int num2 = -1;
			for (int i = 0; i < this.m_shapes.Count; i++)
			{
				AbilityAreaShape shape = this.m_shapes[i].m_shape;
				Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, currentTarget.FreePos, targetSquare);
				AbilityTooltipSubject subjectEnemyInShape = this.m_shapes[i].m_subjectEnemyInShape;
				AbilityTooltipSubject subjectAllyInShape = this.m_shapes[i].m_subjectAllyInShape;
				List<Team> affectedTeams = this.m_shapes[i].GetAffectedTeams(targetingActor);
				bool penetrateLoS = this.m_shapes[i].m_penetrateLoS;
				List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(shape, currentTarget.FreePos, targetSquare, penetrateLoS, targetingActor, affectedTeams, null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
				using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (!list.Contains(actorData))
						{
							if (actorData.GetTeam() != targetingActor.GetTeam())
							{
								base.AddActorInRange(actorData, centerOfShape, targetingActor, subjectEnemyInShape, false);
								this.HandleKnockbackArrowForActor(actorData, centerOfShape, currentTarget, this.m_shapes[i], ref num);
								list.Add(actorData);
								this.m_hitActorContext.Add(new AbilityUtil_Targeter_MultipleShapes.HitActorContext(actorData, i));
							}
							else if (actorData != targetingActor)
							{
								base.AddActorInRange(actorData, centerOfShape, targetingActor, subjectAllyInShape, false);
								list.Add(actorData);
								this.m_hitActorContext.Add(new AbilityUtil_Targeter_MultipleShapes.HitActorContext(actorData, i));
							}
							else
							{
								flag2 = true;
								if (num2 < 0)
								{
									num2 = i;
								}
							}
						}
					}
				}
			}
			if (!flag2 && !this.m_affectsTargetingActor)
			{
				if (this.m_affectCasterDelegate == null)
				{
					goto IL_2D0;
				}
				if (!this.m_affectCasterDelegate(targetingActor, list))
				{
					goto IL_2D0;
				}
			}
			base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Primary, false);
			list.Add(targetingActor);
			num2 = Mathf.Max(0, num2);
			this.m_hitActorContext.Add(new AbilityUtil_Targeter_MultipleShapes.HitActorContext(targetingActor, num2));
			IL_2D0:
			ActorData occupantActor = targetSquare.OccupantActor;
			if (this.m_alwaysIncludeShapeCenterActor)
			{
				if (occupantActor != null)
				{
					if (!list.Contains(occupantActor))
					{
						base.AddActorInRange(occupantActor, occupantActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Primary, false);
						list.Add(occupantActor);
						this.m_hitActorContext.Add(new AbilityUtil_Targeter_MultipleShapes.HitActorContext(occupantActor, 0));
					}
				}
			}
			if (this.m_highlights != null)
			{
				if (this.m_highlights.Count >= this.m_shapes.Count)
				{
					goto IL_400;
				}
			}
			this.m_highlights = new List<GameObject>();
			for (int j = 0; j < this.m_shapes.Count; j++)
			{
				AbilityAreaShape shape2 = this.m_shapes[j].m_shape;
				Vector3 highlightGoalPos = this.GetHighlightGoalPos(currentTarget, targetingActor, shape2);
				GameObject gameObject = HighlightUtils.Get().CreateShapeCursor(shape2, targetingActor == GameFlowData.Get().activeOwnedActorData);
				gameObject.transform.position = highlightGoalPos;
				this.m_highlights.Add(gameObject);
			}
			IL_400:
			float curSpeed = this.m_curSpeed;
			for (int k = 0; k < this.m_shapes.Count; k++)
			{
				AbilityAreaShape shape3 = this.m_shapes[k].m_shape;
				Vector3 highlightGoalPos2 = this.GetHighlightGoalPos(currentTarget, targetingActor, shape3);
				float curSpeed2 = this.m_curSpeed;
				this.m_highlights[k].transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos2, this.m_highlights[k], ref curSpeed2);
				curSpeed = curSpeed2;
			}
			this.m_curSpeed = curSpeed;
			if (flag)
			{
				AbilityUtil_Targeter_MultipleShapes.MultiShapeData multiShapeData = this.m_shapes[this.m_shapes.Count - 1];
				AreaEffectUtils.OperateOnSquaresInShape(this.m_indicatorHandler, multiShapeData.m_shape, currentTarget.FreePos, targetSquare, multiShapeData.m_penetrateLoS, targetingActor, null);
			}
		}
		for (int l = 0; l < this.m_hitActorContext.Count; l++)
		{
			AbilityUtil_Targeter_MultipleShapes.HitActorContext hitActorContext = this.m_hitActorContext[l];
			ActorHitContext actorHitContext = this.m_actorContextVars[hitActorContext.m_actor];
			actorHitContext.symbol_0015.SetInt(TargetSelect_Shape.s_cvarShapeLayer.GetHash(), hitActorContext.m_hitShapeIndex);
		}
		if (flag)
		{
			base.HideUnusedSquareIndicators();
		}
	}

	public class MultiShapeData : ShapeToDataBase
	{
		public bool m_penetrateLoS = true;

		public bool m_affectEnemies = true;

		public bool m_affectAllies;

		public bool m_affectSelf;

		public AbilityTooltipSubject m_subjectEnemyInShape = AbilityTooltipSubject.Primary;

		public AbilityTooltipSubject m_subjectAllyInShape = AbilityTooltipSubject.Secondary;

		public bool m_knockbackEnemies;

		public float m_knockbackDistance;

		public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

		public List<Team> GetAffectedTeams(ActorData caster)
		{
			List<Team> list = new List<Team>();
			if (this.m_affectEnemies)
			{
				list.Add(caster.GetOpposingTeam());
			}
			if (this.m_affectAllies)
			{
				list.Add(caster.GetTeam());
			}
			return list;
		}
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	public delegate BoardSquare CenterSquareDelegate(AbilityTarget currentTarget, ActorData caster);

	public struct HitActorContext
	{
		public ActorData m_actor;

		public int m_hitShapeIndex;

		public HitActorContext(ActorData a, int index)
		{
			this.m_actor = a;
			this.m_hitShapeIndex = index;
		}
	}
}
