using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_MultipleShapes : AbilityUtil_Targeter
{
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
			if (m_affectEnemies)
			{
				list.Add(caster.GetEnemyTeam());
			}
			if (m_affectAllies)
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
			m_actor = a;
			m_hitShapeIndex = index;
		}
	}

	private List<MultiShapeData> m_shapes;

	private const float c_heightOffset = 0.1f;

	private float m_curSpeed;

	public IsAffectingCasterDelegate m_affectCasterDelegate;

	public CenterSquareDelegate m_centerSquareDelegate;

	public bool m_alwaysIncludeShapeCenterActor;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<HitActorContext> m_hitActorContext = new List<HitActorContext>();

	public AbilityUtil_Targeter_MultipleShapes(Ability ability, List<MultiShapeData> shapeDataList)
		: base(ability)
	{
		m_shapes = shapeDataList;
		m_shapes.Sort();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public AbilityUtil_Targeter_MultipleShapes(Ability ability, List<AbilityAreaShape> shapes, List<AbilityTooltipSubject> subjects, bool penetrateLoS, bool affectEnemies = true, bool affectAllies = false, bool affectSelf = false)
		: base(ability)
	{
		m_shapes = new List<MultiShapeData>();
		for (int i = 0; i < shapes.Count; i++)
		{
			int index = Mathf.Min(i, subjects.Count - 1);
			AbilityTooltipSubject subjectEnemyInShape = subjects[index];
			MultiShapeData item = new MultiShapeData
			{
				m_shape = shapes[i],
				m_subjectEnemyInShape = subjectEnemyInShape,
				m_penetrateLoS = penetrateLoS,
				m_affectEnemies = affectEnemies,
				m_affectAllies = affectAllies,
				m_affectSelf = affectSelf
			};
			m_shapes.Add(item);
		}
		m_shapes.Sort();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public List<HitActorContext> GetHitActorContext()
	{
		return m_hitActorContext;
	}

	private bool IsInputValid()
	{
		return m_shapes.Count > 0;
	}

	private BoardSquare GetTargetSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (m_centerSquareDelegate != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_centerSquareDelegate(currentTarget, targetingActor);
				}
			}
		}
		return Board.Get().GetSquare(currentTarget.GridPos);
	}

	private Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor, AbilityAreaShape shape)
	{
		BoardSquare targetSquare = GetTargetSquare(currentTarget, targetingActor);
		if (targetSquare != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, currentTarget.FreePos, targetSquare);
					Vector3 travelBoardSquareWorldPosition = targetingActor.GetFreePos();
					centerOfShape.y = travelBoardSquareWorldPosition.y + 0.1f;
					return centerOfShape;
				}
				}
			}
		}
		return Vector3.zero;
	}

	public override void UpdateHighlightPosAfterClick(AbilityTarget target, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (m_highlights == null || m_shapes == null)
		{
			return;
		}
		while (true)
		{
			int num = 0;
			while (num < m_highlights.Count)
			{
				while (true)
				{
					if (num < m_shapes.Count)
					{
						AbilityAreaShape shape = m_shapes[num].m_shape;
						Vector3 highlightGoalPos = GetHighlightGoalPos(target, targetingActor, shape);
						m_highlights[num].transform.position = highlightGoalPos;
						num++;
						goto IL_006c;
					}
					while (true)
					{
						switch (7)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				IL_006c:;
			}
			return;
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public void HandleKnockbackArrowForActor(ActorData knockbackTarget, Vector3 origin, AbilityTarget abilityTarget, MultiShapeData data, ref int nextMovementArrowIndex)
	{
		if (!data.m_knockbackEnemies)
		{
			return;
		}
		while (true)
		{
			BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(knockbackTarget, data.m_knockbackType, abilityTarget.AimDirection, origin, data.m_knockbackDistance);
			nextMovementArrowIndex = AddMovementArrowWithPrevious(knockbackTarget, path, TargeterMovementType.Knockback, nextMovementArrowIndex);
			return;
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		m_hitActorContext.Clear();
		ClearArrows();
		int nextMovementArrowIndex = 0;
		if (!IsInputValid())
		{
			while (true)
			{
				return;
			}
		}
		bool flag = GameFlowData.Get().activeOwnedActorData == targetingActor;
		if (flag)
		{
			ResetSquareIndicatorIndexToUse();
		}
		BoardSquare targetSquare = GetTargetSquare(currentTarget, targetingActor);
		List<ActorData> list;
		int num;
		if (targetSquare != null)
		{
			list = new List<ActorData>();
			bool flag2 = false;
			num = -1;
			for (int i = 0; i < m_shapes.Count; i++)
			{
				AbilityAreaShape shape = m_shapes[i].m_shape;
				Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, currentTarget.FreePos, targetSquare);
				AbilityTooltipSubject subjectEnemyInShape = m_shapes[i].m_subjectEnemyInShape;
				AbilityTooltipSubject subjectAllyInShape = m_shapes[i].m_subjectAllyInShape;
				List<Team> affectedTeams = m_shapes[i].GetAffectedTeams(targetingActor);
				bool penetrateLoS = m_shapes[i].m_penetrateLoS;
				List<ActorData> actors = AreaEffectUtils.GetActorsInShape(shape, currentTarget.FreePos, targetSquare, penetrateLoS, targetingActor, affectedTeams, null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
				using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						if (!list.Contains(current))
						{
							if (current.GetTeam() != targetingActor.GetTeam())
							{
								AddActorInRange(current, centerOfShape, targetingActor, subjectEnemyInShape);
								HandleKnockbackArrowForActor(current, centerOfShape, currentTarget, m_shapes[i], ref nextMovementArrowIndex);
								list.Add(current);
								m_hitActorContext.Add(new HitActorContext(current, i));
							}
							else if (current != targetingActor)
							{
								AddActorInRange(current, centerOfShape, targetingActor, subjectAllyInShape);
								list.Add(current);
								m_hitActorContext.Add(new HitActorContext(current, i));
							}
							else
							{
								flag2 = true;
								if (num < 0)
								{
									num = i;
								}
							}
						}
					}
				}
			}
			if (flag2 || m_affectsTargetingActor)
			{
				goto IL_0298;
			}
			if (m_affectCasterDelegate != null)
			{
				if (m_affectCasterDelegate(targetingActor, list))
				{
					goto IL_0298;
				}
			}
			goto IL_02d0;
		}
		goto IL_04e4;
		IL_02d0:
		ActorData occupantActor = targetSquare.OccupantActor;
		if (m_alwaysIncludeShapeCenterActor)
		{
			if (occupantActor != null)
			{
				if (!list.Contains(occupantActor))
				{
					AddActorInRange(occupantActor, occupantActor.GetFreePos(), targetingActor);
					list.Add(occupantActor);
					m_hitActorContext.Add(new HitActorContext(occupantActor, 0));
				}
			}
		}
		if (m_highlights != null)
		{
			if (m_highlights.Count >= m_shapes.Count)
			{
				goto IL_0400;
			}
		}
		m_highlights = new List<GameObject>();
		for (int j = 0; j < m_shapes.Count; j++)
		{
			AbilityAreaShape shape2 = m_shapes[j].m_shape;
			Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor, shape2);
			GameObject gameObject = HighlightUtils.Get().CreateShapeCursor(shape2, targetingActor == GameFlowData.Get().activeOwnedActorData);
			gameObject.transform.position = highlightGoalPos;
			m_highlights.Add(gameObject);
		}
		goto IL_0400;
		IL_04e4:
		for (int k = 0; k < m_hitActorContext.Count; k++)
		{
			HitActorContext hitActorContext = m_hitActorContext[k];
			ActorHitContext actorHitContext = m_actorContextVars[hitActorContext.m_actor];
			actorHitContext.m_contextVars.SetValue(TargetSelect_Shape.s_cvarShapeLayer.GetKey(), hitActorContext.m_hitShapeIndex);
		}
		while (true)
		{
			if (flag)
			{
				while (true)
				{
					HideUnusedSquareIndicators();
					return;
				}
			}
			return;
		}
		IL_0400:
		float curSpeed = m_curSpeed;
		for (int l = 0; l < m_shapes.Count; l++)
		{
			AbilityAreaShape shape3 = m_shapes[l].m_shape;
			Vector3 highlightGoalPos2 = GetHighlightGoalPos(currentTarget, targetingActor, shape3);
			float currentSpeed = m_curSpeed;
			m_highlights[l].transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos2, m_highlights[l], ref currentSpeed);
			curSpeed = currentSpeed;
		}
		m_curSpeed = curSpeed;
		if (flag)
		{
			MultiShapeData multiShapeData = m_shapes[m_shapes.Count - 1];
			AreaEffectUtils.OperateOnSquaresInShape(m_indicatorHandler, multiShapeData.m_shape, currentTarget.FreePos, targetSquare, multiShapeData.m_penetrateLoS, targetingActor);
		}
		goto IL_04e4;
		IL_0298:
		AddActorInRange(targetingActor, targetingActor.GetFreePos(), targetingActor);
		list.Add(targetingActor);
		num = Mathf.Max(0, num);
		m_hitActorContext.Add(new HitActorContext(targetingActor, num));
		goto IL_02d0;
	}
}
