using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_MultipleCones : AbilityUtil_Targeter
{
	public List<AbilityUtil_Targeter_MultipleCones.ConeDimensions> m_coneDimensions;

	public float m_maxConeAngle;

	public float m_maxConeLengthRadius;

	public bool m_penetrateLoS;

	public float m_coneBackwardOffsetInSquares;

	public bool m_useCursorHighlight = true;

	public bool m_includeEnemies = true;

	public bool m_includeAllies;

	public bool m_includeCaster;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_MultipleCones.IsAffectingCasterDelegate m_affectCasterDelegate;

	public AbilityUtil_Targeter_MultipleCones(Ability ability, List<AbilityUtil_Targeter_MultipleCones.ConeDimensions> coneDimensions, float coneBackwardOffsetInSquares, bool penetrateLoS, bool useCursorHighlight, bool affectEnemies = true, bool affectAllies = false, bool affectCaster = false) : base(ability)
	{
		this.m_coneDimensions = coneDimensions;
		this.m_maxConeLengthRadius = -1f;
		this.m_maxConeAngle = 0f;
		for (int i = 0; i < this.m_coneDimensions.Count; i++)
		{
			if (this.m_coneDimensions[i].m_coneRadius > this.m_maxConeLengthRadius)
			{
				this.m_maxConeLengthRadius = this.m_coneDimensions[i].m_coneRadius;
			}
			if (this.m_coneDimensions[i].m_coneAngle > this.m_maxConeAngle)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MultipleCones..ctor(Ability, List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>, float, bool, bool, bool, bool, bool)).MethodHandle;
				}
				this.m_maxConeAngle = this.m_coneDimensions[i].m_coneAngle;
			}
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
		this.m_penetrateLoS = penetrateLoS;
		this.m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		this.m_useCursorHighlight = useCursorHighlight;
		this.m_includeEnemies = affectEnemies;
		this.m_includeAllies = affectAllies;
		this.m_includeCaster = affectCaster;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		base.SetAffectedGroups(this.m_includeEnemies, this.m_includeAllies, this.m_includeCaster);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector;
		if (currentTarget == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MultipleCones.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 vec = vector;
		float num = VectorUtils.HorizontalAngle_Deg(vec);
		this.CreateConeCursorHighlights(travelBoardSquareWorldPositionForLos, num);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(travelBoardSquareWorldPositionForLos, num, this.m_maxConeAngle, this.m_maxConeLengthRadius, this.m_coneBackwardOffsetInSquares, this.m_penetrateLoS, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, this.m_affectsAllies, this.m_affectsEnemies), null, false, default(Vector3));
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
		actorsInCone.Remove(targetingActor);
		if (this.m_includeCaster)
		{
			if (this.m_affectCasterDelegate != null)
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
				if (!this.m_affectCasterDelegate(targetingActor, actorsInCone))
				{
					goto IL_DE;
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
			actorsInCone.Add(targetingActor);
		}
		IL_DE:
		using (List<ActorData>.Enumerator enumerator = actorsInCone.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actor = enumerator.Current;
				if (this.ShouldAddActor(actor, targetingActor))
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
					base.AddActorInRange(actor, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Primary, false);
				}
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
		this.DrawInvalidSquareIndicators(currentTarget, targetingActor);
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		bool result = false;
		if (actor == caster)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MultipleCones.ShouldAddActor(ActorData, ActorData)).MethodHandle;
			}
			result = this.m_includeCaster;
		}
		else
		{
			if (actor.GetTeam() == caster.GetTeam())
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
				if (this.m_includeAllies)
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
					return true;
				}
			}
			if (actor.GetTeam() != caster.GetTeam() && this.m_includeEnemies)
			{
				result = true;
			}
		}
		return result;
	}

	public void CreateConeCursorHighlights(Vector3 casterPos, float aimDir_degrees)
	{
		Vector3 vector = VectorUtils.AngleDegreesToVector(aimDir_degrees);
		float d = this.m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = casterPos + new Vector3(0f, y, 0f) - vector * d;
		if (this.m_highlights == null || this.m_highlights.Count < this.m_coneDimensions.Count)
		{
			this.m_highlights = new List<GameObject>();
			for (int i = 0; i < this.m_coneDimensions.Count; i++)
			{
				float radiusInWorld = (this.m_coneDimensions[i].m_coneRadius + this.m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
				GameObject gameObject = HighlightUtils.Get().CreateConeCursor(radiusInWorld, this.m_coneDimensions[i].m_coneAngle);
				UIDynamicCone component = gameObject.GetComponent<UIDynamicCone>();
				if (component != null)
				{
					component.SetConeObjectActive(false);
				}
				this.m_highlights.Add(gameObject);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MultipleCones.CreateConeCursorHighlights(Vector3, float)).MethodHandle;
			}
		}
		for (int j = 0; j < this.m_highlights.Count; j++)
		{
			this.m_highlights[j].transform.position = position;
			this.m_highlights[j].transform.rotation = Quaternion.LookRotation(vector);
		}
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			Vector3 vector;
			if (currentTarget == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MultipleCones.DrawInvalidSquareIndicators(AbilityTarget, ActorData)).MethodHandle;
				}
				vector = targetingActor.transform.forward;
			}
			else
			{
				vector = currentTarget.AimDirection;
			}
			Vector3 vec = vector;
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, this.m_maxConeAngle, this.m_maxConeLengthRadius, this.m_coneBackwardOffsetInSquares, targetingActor, this.m_penetrateLoS, null);
			base.HideUnusedSquareIndicators();
		}
	}

	public class ConeDimensions
	{
		public float m_coneAngle;

		public float m_coneRadius;

		public ConeDimensions(float angle, float radiusInSquares)
		{
			this.m_coneAngle = angle;
			this.m_coneRadius = radiusInSquares;
		}
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);
}
