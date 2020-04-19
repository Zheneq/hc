using System;
using UnityEngine;

public class AbilityUtil_Targeter_MartyrLaser : AbilityUtil_Targeter_LaserWithCone
{
	private float m_coneInnerRadius;

	private bool m_coneAffectsCaster;

	private bool m_coneAffectsAllies;

	private bool m_coneAffectsEnemies;

	public AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate m_delegateLaserWidth;

	public AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate m_delegateLaserRange;

	public AbilityUtil_Targeter_MartyrLaser.CustomBoolValueDelegate m_delegatePenetrateLos;

	public AbilityUtil_Targeter_MartyrLaser.CustomIntValueDelegate m_delegateMaxTargets;

	public AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate m_delegateConeRadius;

	public AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate m_delegateInnerConeRadius;

	public AbilityUtil_Targeter_MartyrLaser(Ability ability, float width, float distance, bool penetrateLoS, int maxTargets, bool affectsEnemies, bool affectsAllies, bool affectsCaster, bool clampRangeToCursor, bool explodeOnlyOnLaserHit, float coneRadius, float coneInnerRadius, bool coneAffectsAllies, bool coneAffectsEnemies, bool coneAffectsCaster) : base(ability, width, distance, penetrateLoS, affectsAllies, 360f, coneRadius, 0f)
	{
		base.SetAffectedGroups(affectsEnemies, affectsAllies, affectsCaster);
		this.m_coneAffectsAllies = coneAffectsAllies;
		this.m_coneAffectsEnemies = coneAffectsEnemies;
		this.m_coneAffectsCaster = coneAffectsCaster;
		this.m_maxLaserTargets = maxTargets;
		this.m_coneInnerRadius = coneInnerRadius;
		base.SetClampToCursorPos(clampRangeToCursor);
		base.SetExplodeOnPathEnd(!explodeOnlyOnLaserHit);
		base.SetExplodeOnEnvironmentHit(!explodeOnlyOnLaserHit);
	}

	public override float GetWidth()
	{
		float result;
		if (this.m_delegateLaserWidth != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MartyrLaser.GetWidth()).MethodHandle;
			}
			result = this.m_delegateLaserWidth();
		}
		else
		{
			result = this.m_width;
		}
		return result;
	}

	public override float GetDistance()
	{
		float result;
		if (this.m_delegateLaserRange != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MartyrLaser.GetDistance()).MethodHandle;
			}
			result = this.m_delegateLaserRange();
		}
		else
		{
			result = this.m_distance;
		}
		return result;
	}

	public override bool GetPenetrateLoS()
	{
		return (this.m_delegatePenetrateLos == null) ? this.m_penetrateLoS : this.m_delegatePenetrateLos();
	}

	public override int GetLaserMaxTargets()
	{
		int result;
		if (this.m_delegateMaxTargets != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MartyrLaser.GetLaserMaxTargets()).MethodHandle;
			}
			result = this.m_delegateMaxTargets();
		}
		else
		{
			result = this.m_maxLaserTargets;
		}
		return result;
	}

	public override float GetConeRadius()
	{
		float result;
		if (this.m_delegateConeRadius != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MartyrLaser.GetConeRadius()).MethodHandle;
			}
			result = this.m_delegateConeRadius();
		}
		else
		{
			result = this.m_coneLengthRadiusInSquares;
		}
		return result;
	}

	public float GetInnerConeRadius()
	{
		return (this.m_delegateInnerConeRadius == null) ? this.m_coneInnerRadius : this.m_delegateInnerConeRadius();
	}

	public override bool GetConeAffectsTarget(ActorData potentialTarget, ActorData targetingActor)
	{
		if (potentialTarget == targetingActor)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MartyrLaser.GetConeAffectsTarget(ActorData, ActorData)).MethodHandle;
			}
			return this.m_coneAffectsCaster;
		}
		if (potentialTarget.\u000E() == targetingActor.\u000E())
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
			return this.m_coneAffectsAllies;
		}
		return this.m_coneAffectsEnemies;
	}

	public override void AddTargetedActor(ActorData actor, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary)
	{
		base.AddTargetedActor(actor, damageOrigin, targetingActor, subjectType);
		Vector3 vector = damageOrigin - actor.\u0016();
		vector.y = 0f;
		float num = (this.GetInnerConeRadius() + GameWideData.Get().m_actorTargetingRadiusInSquares) * Board.\u000E().squareSize;
		if (num < vector.magnitude)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MartyrLaser.AddTargetedActor(ActorData, Vector3, ActorData, AbilityTooltipSubject)).MethodHandle;
			}
			base.AddActorInRange(actor, damageOrigin, targetingActor, AbilityTooltipSubject.Tertiary, true);
		}
	}

	protected override void AllocateConeHighlights()
	{
		if (this.m_highlights.Count == 1)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MartyrLaser.AllocateConeHighlights()).MethodHandle;
			}
			float radiusInWorld = (this.GetConeRadius() + this.m_coneBackwardOffsetInSquares) * Board.\u000E().squareSize;
			GameObject item = HighlightUtils.Get().CreateConeCursor(radiusInWorld, this.GetConeWidthAngle());
			this.m_highlights.Add(item);
			if (this.GetInnerConeRadius() > 0f)
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
				float radiusInWorld2 = (this.GetInnerConeRadius() + this.m_coneBackwardOffsetInSquares) * Board.\u000E().squareSize;
				GameObject gameObject = HighlightUtils.Get().CreateConeCursor(radiusInWorld2, this.GetConeWidthAngle());
				UIDynamicCone component = gameObject.GetComponent<UIDynamicCone>();
				if (component != null)
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
					component.SetConeObjectActive(false);
				}
				this.m_highlights.Add(gameObject);
			}
		}
	}

	public delegate float CustomFloatValueDelegate();

	public delegate bool CustomBoolValueDelegate();

	public delegate int CustomIntValueDelegate();
}
