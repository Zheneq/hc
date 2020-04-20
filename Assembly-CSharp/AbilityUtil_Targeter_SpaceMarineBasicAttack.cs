using System;
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

	public AbilityUtil_Targeter_SpaceMarineBasicAttack(Ability ability, float laserWidthInSquares, float laserLengthInSquares, int laserMaxTargets, float coneWidthAngle, float coneRadiusInSquares, bool ignoreLos) : base(ability)
	{
		this.m_widthInSquares = laserWidthInSquares;
		this.m_lengthInSquares = laserLengthInSquares;
		this.m_laserMaxTargets = laserMaxTargets;
		this.m_coneWidthAngle = coneWidthAngle;
		this.m_coneRadiusInSquares = coneRadiusInSquares;
		this.m_ignoreLos = ignoreLos;
		this.m_laserPart = new TargeterPart_Laser(this.m_widthInSquares, this.m_lengthInSquares, this.m_ignoreLos, this.m_laserMaxTargets);
		this.m_conePart = new TargeterPart_Cone(this.m_coneWidthAngle, this.m_coneRadiusInSquares, this.m_ignoreLos, 0f);
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public bool LengthIgnoreWorldGeo { get; set; }

	public bool AddConeOnFirstLaserHit { get; set; }

	public List<ActorData> GetLastLaserHitActors()
	{
		return this.m_lastAddedLaserHitActors;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 2)
			{
				goto IL_76;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(this.m_laserPart.CreateHighlightObject(this));
		this.m_highlights.Add(this.m_conePart.CreateHighlightObject(this));
		IL_76:
		GameObject highlightObj = this.m_highlights[0];
		GameObject gameObject = this.m_highlights[1];
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 aimDirection = currentTarget.AimDirection;
		List<Team> affectedTeams = base.GetAffectedTeams();
		this.m_laserPart.m_lengthIgnoreWorldGeo = this.LengthIgnoreWorldGeo;
		Vector3 vector;
		List<ActorData> hitActors = this.m_laserPart.GetHitActors(travelBoardSquareWorldPositionForLos, aimDirection, targetingActor, affectedTeams, out vector);
		this.m_lastAddedLaserHitActors = hitActors;
		for (int i = 0; i < hitActors.Count; i++)
		{
			base.AddActorInRange(hitActors[i], travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Primary, false);
		}
		this.m_laserPart.AdjustHighlight(highlightObj, travelBoardSquareWorldPositionForLos, vector, true);
		bool showCone = false;
		Vector3 vector2 = Vector3.zero;
		Vector3 vector3 = Vector3.forward;
		if (hitActors.Count > 0)
		{
			if (this.AddConeOnFirstLaserHit)
			{
				showCone = true;
				vector2 = hitActors[0].GetTravelBoardSquareWorldPositionForLos();
				vector3 = currentTarget.AimDirection;
				List<ActorData> hitActors2 = this.m_conePart.GetHitActors(vector2, vector3, targetingActor, affectedTeams);
				for (int j = 0; j < hitActors2.Count; j++)
				{
					base.AddActorInRange(hitActors2[j], vector2, targetingActor, AbilityTooltipSubject.Secondary, false);
				}
				this.m_conePart.AdjustHighlight(gameObject, vector2, vector3);
				gameObject.SetActive(true);
				goto IL_1E5;
			}
		}
		gameObject.SetActive(false);
		IL_1E5:
		this.HandleHiddenSquareIndicators(targetingActor, travelBoardSquareWorldPositionForLos, vector, showCone, vector2, VectorUtils.HorizontalAngle_Deg(vector3));
	}

	private void HandleHiddenSquareIndicators(ActorData targetingActor, Vector3 laserStartPos, Vector3 laserEndPos, bool showCone, Vector3 coneStartPos, float forwardAngle)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			this.m_laserPart.ShowHiddenSquares(this.m_indicatorHandler, laserStartPos, laserEndPos, targetingActor, this.m_ignoreLos);
			if (showCone)
			{
				this.m_conePart.ShowHiddenSquares(this.m_indicatorHandler, coneStartPos, forwardAngle, targetingActor, this.m_ignoreLos);
			}
			base.HideUnusedSquareIndicators();
		}
	}
}
