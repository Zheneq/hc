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

	public bool LengthIgnoreWorldGeo
	{
		get;
		set;
	}

	public bool AddConeOnFirstLaserHit
	{
		get;
		set;
	}

	public AbilityUtil_Targeter_SpaceMarineBasicAttack(Ability ability, float laserWidthInSquares, float laserLengthInSquares, int laserMaxTargets, float coneWidthAngle, float coneRadiusInSquares, bool ignoreLos)
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
		if (m_highlights != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_highlights.Count >= 2)
			{
				goto IL_0076;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(m_laserPart.CreateHighlightObject(this));
		m_highlights.Add(m_conePart.CreateHighlightObject(this));
		goto IL_0076;
		IL_0076:
		GameObject highlightObj = m_highlights[0];
		GameObject gameObject = m_highlights[1];
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 aimDirection = currentTarget.AimDirection;
		List<Team> affectedTeams = GetAffectedTeams();
		m_laserPart.m_lengthIgnoreWorldGeo = LengthIgnoreWorldGeo;
		Vector3 endPos;
		List<ActorData> list = m_lastAddedLaserHitActors = m_laserPart.GetHitActors(travelBoardSquareWorldPositionForLos, aimDirection, targetingActor, affectedTeams, out endPos);
		for (int i = 0; i < list.Count; i++)
		{
			AddActorInRange(list[i], travelBoardSquareWorldPositionForLos, targetingActor);
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			m_laserPart.AdjustHighlight(highlightObj, travelBoardSquareWorldPositionForLos, endPos);
			bool showCone = false;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.forward;
			if (list.Count > 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (AddConeOnFirstLaserHit)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					showCone = true;
					vector = list[0].GetTravelBoardSquareWorldPositionForLos();
					vector2 = currentTarget.AimDirection;
					List<ActorData> hitActors = m_conePart.GetHitActors(vector, vector2, targetingActor, affectedTeams);
					for (int j = 0; j < hitActors.Count; j++)
					{
						AddActorInRange(hitActors[j], vector, targetingActor, AbilityTooltipSubject.Secondary);
					}
					m_conePart.AdjustHighlight(gameObject, vector, vector2);
					gameObject.SetActive(true);
					goto IL_01e5;
				}
			}
			gameObject.SetActive(false);
			goto IL_01e5;
			IL_01e5:
			HandleHiddenSquareIndicators(targetingActor, travelBoardSquareWorldPositionForLos, endPos, showCone, vector, VectorUtils.HorizontalAngle_Deg(vector2));
			return;
		}
	}

	private void HandleHiddenSquareIndicators(ActorData targetingActor, Vector3 laserStartPos, Vector3 laserEndPos, bool showCone, Vector3 coneStartPos, float forwardAngle)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ResetSquareIndicatorIndexToUse();
			m_laserPart.ShowHiddenSquares(m_indicatorHandler, laserStartPos, laserEndPos, targetingActor, m_ignoreLos);
			if (showCone)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				m_conePart.ShowHiddenSquares(m_indicatorHandler, coneStartPos, forwardAngle, targetingActor, m_ignoreLos);
			}
			HideUnusedSquareIndicators();
			return;
		}
	}
}
