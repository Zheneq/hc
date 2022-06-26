using UnityEngine;

public class AbilityUtil_Targeter_MartyrLaser : AbilityUtil_Targeter_LaserWithCone
{
	public delegate float CustomFloatValueDelegate();
	public delegate bool CustomBoolValueDelegate();
	public delegate int CustomIntValueDelegate();

	private float m_coneInnerRadius;
	private bool m_coneAffectsCaster;
	private bool m_coneAffectsAllies;
	private bool m_coneAffectsEnemies;

	public CustomFloatValueDelegate m_delegateLaserWidth;
	public CustomFloatValueDelegate m_delegateLaserRange;
	public CustomBoolValueDelegate m_delegatePenetrateLos;
	public CustomIntValueDelegate m_delegateMaxTargets;
	public CustomFloatValueDelegate m_delegateConeRadius;
	public CustomFloatValueDelegate m_delegateInnerConeRadius;

	public AbilityUtil_Targeter_MartyrLaser(Ability ability, float width, float distance, bool penetrateLoS, int maxTargets, bool affectsEnemies, bool affectsAllies, bool affectsCaster, bool clampRangeToCursor, bool explodeOnlyOnLaserHit, float coneRadius, float coneInnerRadius, bool coneAffectsAllies, bool coneAffectsEnemies, bool coneAffectsCaster)
		: base(ability, width, distance, penetrateLoS, affectsAllies, 360f, coneRadius, 0f)
	{
		SetAffectedGroups(affectsEnemies, affectsAllies, affectsCaster);
		m_coneAffectsAllies = coneAffectsAllies;
		m_coneAffectsEnemies = coneAffectsEnemies;
		m_coneAffectsCaster = coneAffectsCaster;
		m_maxLaserTargets = maxTargets;
		m_coneInnerRadius = coneInnerRadius;
		SetClampToCursorPos(clampRangeToCursor);
		SetExplodeOnPathEnd(!explodeOnlyOnLaserHit);
		SetExplodeOnEnvironmentHit(!explodeOnlyOnLaserHit);
	}

	public override float GetWidth()
	{
		return m_delegateLaserWidth != null ? m_delegateLaserWidth() : m_width;
	}

	public override float GetDistance()
	{
		return m_delegateLaserRange != null ? m_delegateLaserRange() : m_distance;
	}

	public override bool GetPenetrateLoS()
	{
		return m_delegatePenetrateLos != null ? m_delegatePenetrateLos() : m_penetrateLoS;
	}

	public override int GetLaserMaxTargets()
	{
		return m_delegateMaxTargets != null ? m_delegateMaxTargets() : m_maxLaserTargets;
	}

	public override float GetConeRadius()
	{
		return m_delegateConeRadius != null ? m_delegateConeRadius() : m_coneLengthRadiusInSquares;
	}

	public float GetInnerConeRadius()
	{
		return m_delegateInnerConeRadius != null ? m_delegateInnerConeRadius() : m_coneInnerRadius;
	}

	public override bool GetConeAffectsTarget(ActorData potentialTarget, ActorData targetingActor)
	{
		if (potentialTarget == targetingActor)
		{
			return m_coneAffectsCaster;
		}
		if (potentialTarget.GetTeam() == targetingActor.GetTeam())
		{
			return m_coneAffectsAllies;
		}
		return m_coneAffectsEnemies;
	}

	public override void AddTargetedActor(ActorData actor, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary)
	{
		base.AddTargetedActor(actor, damageOrigin, targetingActor, subjectType);
		Vector3 vector = damageOrigin - actor.GetFreePos();
		vector.y = 0f;
		float num = (GetInnerConeRadius() + GameWideData.Get().m_actorTargetingRadiusInSquares) * Board.Get().squareSize;
		if (num < vector.magnitude)
		{
			AddActorInRange(actor, damageOrigin, targetingActor, AbilityTooltipSubject.Tertiary, true);
		}
	}

	protected override void AllocateConeHighlights()
	{
		if (m_highlights.Count == 1)
		{
			float radiusInWorld = (GetConeRadius() + m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
			GameObject item = HighlightUtils.Get().CreateConeCursor(radiusInWorld, GetConeWidthAngle());
			m_highlights.Add(item);
			if (GetInnerConeRadius() > 0f)
			{
				float radiusInWorld2 = (GetInnerConeRadius() + m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
				GameObject gameObject = HighlightUtils.Get().CreateConeCursor(radiusInWorld2, GetConeWidthAngle());
				UIDynamicCone component = gameObject.GetComponent<UIDynamicCone>();
				if (component != null)
				{
					component.SetConeObjectActive(false);
				}
				m_highlights.Add(gameObject);
			}
		}
	}
}
