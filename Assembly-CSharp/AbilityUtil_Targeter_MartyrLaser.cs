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
		float result;
		if (m_delegateLaserWidth != null)
		{
			while (true)
			{
				switch (1)
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
			result = m_delegateLaserWidth();
		}
		else
		{
			result = m_width;
		}
		return result;
	}

	public override float GetDistance()
	{
		float result;
		if (m_delegateLaserRange != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_delegateLaserRange();
		}
		else
		{
			result = m_distance;
		}
		return result;
	}

	public override bool GetPenetrateLoS()
	{
		return (m_delegatePenetrateLos == null) ? m_penetrateLoS : m_delegatePenetrateLos();
	}

	public override int GetLaserMaxTargets()
	{
		int result;
		if (m_delegateMaxTargets != null)
		{
			while (true)
			{
				switch (7)
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
			result = m_delegateMaxTargets();
		}
		else
		{
			result = m_maxLaserTargets;
		}
		return result;
	}

	public override float GetConeRadius()
	{
		float result;
		if (m_delegateConeRadius != null)
		{
			while (true)
			{
				switch (1)
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
			result = m_delegateConeRadius();
		}
		else
		{
			result = m_coneLengthRadiusInSquares;
		}
		return result;
	}

	public float GetInnerConeRadius()
	{
		return (m_delegateInnerConeRadius == null) ? m_coneInnerRadius : m_delegateInnerConeRadius();
	}

	public override bool GetConeAffectsTarget(ActorData potentialTarget, ActorData targetingActor)
	{
		if (potentialTarget == targetingActor)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_coneAffectsCaster;
				}
			}
		}
		if (potentialTarget.GetTeam() == targetingActor.GetTeam())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_coneAffectsAllies;
				}
			}
		}
		return m_coneAffectsEnemies;
	}

	public override void AddTargetedActor(ActorData actor, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary)
	{
		base.AddTargetedActor(actor, damageOrigin, targetingActor, subjectType);
		Vector3 vector = damageOrigin - actor.GetTravelBoardSquareWorldPosition();
		vector.y = 0f;
		float num = (GetInnerConeRadius() + GameWideData.Get().m_actorTargetingRadiusInSquares) * Board.Get().squareSize;
		if (!(num < vector.magnitude))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AddActorInRange(actor, damageOrigin, targetingActor, AbilityTooltipSubject.Tertiary, true);
			return;
		}
	}

	protected override void AllocateConeHighlights()
	{
		if (m_highlights.Count != 1)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			float radiusInWorld = (GetConeRadius() + m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
			GameObject item = HighlightUtils.Get().CreateConeCursor(radiusInWorld, GetConeWidthAngle());
			m_highlights.Add(item);
			if (!(GetInnerConeRadius() > 0f))
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				float radiusInWorld2 = (GetInnerConeRadius() + m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
				GameObject gameObject = HighlightUtils.Get().CreateConeCursor(radiusInWorld2, GetConeWidthAngle());
				UIDynamicCone component = gameObject.GetComponent<UIDynamicCone>();
				if (component != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					component.SetConeObjectActive(false);
				}
				m_highlights.Add(gameObject);
				return;
			}
		}
	}
}
