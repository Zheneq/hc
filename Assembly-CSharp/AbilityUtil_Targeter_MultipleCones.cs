using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_MultipleCones : AbilityUtil_Targeter
{
	public class ConeDimensions
	{
		public float m_coneAngle;
		public float m_coneRadius;

		public ConeDimensions(float angle, float radiusInSquares)
		{
			m_coneAngle = angle;
			m_coneRadius = radiusInSquares;
		}
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	public List<ConeDimensions> m_coneDimensions;
	public float m_maxConeAngle;
	public float m_maxConeLengthRadius;
	public bool m_penetrateLoS;
	public float m_coneBackwardOffsetInSquares;
	public bool m_useCursorHighlight = true;
	public bool m_includeEnemies = true;
	public bool m_includeAllies;
	public bool m_includeCaster;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public IsAffectingCasterDelegate m_affectCasterDelegate;

	public AbilityUtil_Targeter_MultipleCones(
		Ability ability,
		List<ConeDimensions> coneDimensions,
		float coneBackwardOffsetInSquares,
		bool penetrateLoS,
		bool useCursorHighlight,
		bool affectEnemies = true,
		bool affectAllies = false,
		bool affectCaster = false)
		: base(ability)
	{
		m_coneDimensions = coneDimensions;
		m_maxConeLengthRadius = -1f;
		m_maxConeAngle = 0f;
		foreach (ConeDimensions d in m_coneDimensions)
		{
			if (d.m_coneRadius > m_maxConeLengthRadius)
			{
				m_maxConeLengthRadius = d.m_coneRadius;
			}
			if (d.m_coneAngle > m_maxConeAngle)
			{
				m_maxConeAngle = d.m_coneAngle;
			}
		}
		m_penetrateLoS = penetrateLoS;
		m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		m_useCursorHighlight = useCursorHighlight;
		m_includeEnemies = affectEnemies;
		m_includeAllies = affectAllies;
		m_includeCaster = affectCaster;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		SetAffectedGroups(m_includeEnemies, m_includeAllies, m_includeCaster);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		Vector3 casterPos = targetingActor.GetLoSCheckPos();
		Vector3 vec = currentTarget != null ? currentTarget.AimDirection : targetingActor.transform.forward;
		float num = VectorUtils.HorizontalAngle_Deg(vec);
		CreateConeCursorHighlights(casterPos, num);
		List<ActorData> actors = AreaEffectUtils.GetActorsInCone(
			casterPos,
			num,
			m_maxConeAngle,
			m_maxConeLengthRadius,
			m_coneBackwardOffsetInSquares,
			m_penetrateLoS,
			targetingActor,
			TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies),
			null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		actors.Remove(targetingActor);
		if (m_includeCaster
		    && (m_affectCasterDelegate == null || m_affectCasterDelegate(targetingActor, actors)))
		{
			actors.Add(targetingActor);
		}
		foreach (ActorData actor in actors)
		{
			if (ShouldAddActor(actor, targetingActor))
			{
				AddActorInRange(actor, casterPos, targetingActor);
			}
		}
		DrawInvalidSquareIndicators(currentTarget, targetingActor);
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		if (actor == caster)
		{
			return m_includeCaster;
		}
		return actor.GetTeam() == caster.GetTeam() && m_includeAllies 
		       || actor.GetTeam() != caster.GetTeam() && m_includeEnemies;
	}

	public void CreateConeCursorHighlights(Vector3 casterPos, float aimDir_degrees)
	{
		Vector3 vector = VectorUtils.AngleDegreesToVector(aimDir_degrees);
		float d = m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = casterPos + new Vector3(0f, y, 0f) - vector * d;
		if (m_highlights == null || m_highlights.Count < m_coneDimensions.Count)
		{
			m_highlights = new List<GameObject>();
			for (int i = 0; i < m_coneDimensions.Count; i++)
			{
				float radiusInWorld = (m_coneDimensions[i].m_coneRadius + m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
				GameObject gameObject = HighlightUtils.Get().CreateConeCursor(radiusInWorld, m_coneDimensions[i].m_coneAngle);
				UIDynamicCone component = gameObject.GetComponent<UIDynamicCone>();
				if (component != null)
				{
					component.SetConeObjectActive(false);
				}
				m_highlights.Add(gameObject);
			}
		}
		for (int j = 0; j < m_highlights.Count; j++)
		{
			m_highlights[j].transform.position = position;
			m_highlights[j].transform.rotation = Quaternion.LookRotation(vector);
		}
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		ResetSquareIndicatorIndexToUse();
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetLoSCheckPos();
		Vector3 vector;
		if (currentTarget == null)
		{
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 vec = vector;
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
		AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, m_maxConeAngle, m_maxConeLengthRadius, m_coneBackwardOffsetInSquares, targetingActor, m_penetrateLoS);
		HideUnusedSquareIndicators();
	}
}
