using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_SpaceMarineBasicAttack : AbilityUtil_Targeter
{
	private float m_widthInSquares = 1f;
	public float m_lengthInSquares = 15f;
	private int m_laserMaxTargets = -1;
	private float m_coneWidthAngle = 90f;
	private float m_coneRadiusInSquares = 1f;
	private bool m_ignoreLos;
	private TargeterPart_Laser m_laserPart;
	private TargeterPart_Cone m_conePart;
	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;
	private List<ActorData> m_lastAddedLaserHitActors = new List<ActorData>();

	public bool LengthIgnoreWorldGeo { get; set; }

	public bool AddConeOnFirstLaserHit { get; set; }

	public AbilityUtil_Targeter_SpaceMarineBasicAttack(
		Ability ability,
		float laserWidthInSquares,
		float laserLengthInSquares,
		int laserMaxTargets,
		float coneWidthAngle,
		float coneRadiusInSquares,
		bool ignoreLos)
		: base(ability)
	{
		m_widthInSquares = laserWidthInSquares;
		m_lengthInSquares = laserLengthInSquares;
		m_laserMaxTargets = laserMaxTargets;
		m_coneWidthAngle = coneWidthAngle;
		m_coneRadiusInSquares = coneRadiusInSquares;
		m_ignoreLos = ignoreLos;
		m_laserPart = new TargeterPart_Laser(m_widthInSquares, m_lengthInSquares, m_ignoreLos, m_laserMaxTargets);
		m_conePart = new TargeterPart_Cone(m_coneWidthAngle, m_coneRadiusInSquares, m_ignoreLos, 0f);
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public List<ActorData> GetLastLaserHitActors()
	{
		return m_lastAddedLaserHitActors;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		if (m_highlights == null || m_highlights.Count < 2)
		{
			m_highlights = new List<GameObject>
			{
				m_laserPart.CreateHighlightObject(this),
				m_conePart.CreateHighlightObject(this)
			};
		}
		Vector3 losCheckPos = targetingActor.GetLoSCheckPos();
		List<Team> affectedTeams = GetAffectedTeams();
		m_laserPart.m_lengthIgnoreWorldGeo = LengthIgnoreWorldGeo;
		m_lastAddedLaserHitActors = m_laserPart.GetHitActors(
			losCheckPos, currentTarget.AimDirection, targetingActor, affectedTeams, out Vector3 endPos);
		foreach (ActorData hitActor in m_lastAddedLaserHitActors)
		{
			AddActorInRange(hitActor, losCheckPos, targetingActor);
		}
		m_laserPart.AdjustHighlight(m_highlights[0], losCheckPos, endPos);
		bool showCone = false;
		Vector3 coneStartPos = Vector3.zero;
		Vector3 aimDir = Vector3.forward;
		if (m_lastAddedLaserHitActors.Count > 0 && AddConeOnFirstLaserHit)
		{
			showCone = true;
			coneStartPos = m_lastAddedLaserHitActors[0].GetLoSCheckPos();
			aimDir = currentTarget.AimDirection;
			List<ActorData> hitActors = m_conePart.GetHitActors(coneStartPos, aimDir, targetingActor, affectedTeams);
			foreach (var hitActor in hitActors)
			{
				AddActorInRange(hitActor, coneStartPos, targetingActor, AbilityTooltipSubject.Secondary);
			}
			m_conePart.AdjustHighlight(m_highlights[1], coneStartPos, aimDir);
			m_highlights[1].SetActive(true);
		}
		else
		{
			m_highlights[1].SetActive(false);
		}
		HandleHiddenSquareIndicators(targetingActor, losCheckPos, endPos, showCone, coneStartPos, VectorUtils.HorizontalAngle_Deg(aimDir));
	}

	private void HandleHiddenSquareIndicators(
		ActorData targetingActor,
		Vector3 laserStartPos,
		Vector3 laserEndPos,
		bool showCone,
		Vector3 coneStartPos,
		float forwardAngle)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			m_laserPart.ShowHiddenSquares(m_indicatorHandler, laserStartPos, laserEndPos, targetingActor, m_ignoreLos);
			if (showCone)
			{
				m_conePart.ShowHiddenSquares(m_indicatorHandler, coneStartPos, forwardAngle, targetingActor, m_ignoreLos);
			}
			HideUnusedSquareIndicators();
		}
	}
}
