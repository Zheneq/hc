using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_ConeOrLaser : GenericAbility_TargetSelectBase
{
	[Separator("Targeting Properties", true)]
	public float m_coneDistThreshold = 4f;

	[Header("  Targeting: For Cone")]
	public ConeTargetingInfo m_coneInfo;

	[Header("  Targeting: For Laser")]
	public LaserTargetingInfo m_laserInfo;

	[Separator("Sequences", true)]
	public GameObject m_coneSequencePrefab;

	public GameObject m_laserSequencePrefab;

	public static ContextNameKeyPair s_cvarInCone = new ContextNameKeyPair("InCone");

	private TargetSelectMod_ConeOrLaser m_targetSelMod;

	private ConeTargetingInfo m_cachedConeInfo;

	private LaserTargetingInfo m_cachedLaserInfo;

	public override string GetUsageForEditor()
	{
		return base.GetContextUsageStr(ContextKeys.\u0018.\u0012(), "distance from start of cone position, in squares", true) + base.GetContextUsageStr(TargetSelect_ConeOrLaser.s_cvarInCone.\u0012(), "Whether the target hit is in cone", true) + base.GetContextUsageStr(ContextKeys.\u001D.\u0012(), "angle from center of cone", true);
	}

	public override void ListContextNamesForEditor(List<string> keys)
	{
		keys.Add(ContextKeys.\u0018.\u0012());
		keys.Add(TargetSelect_ConeOrLaser.s_cvarInCone.\u0012());
		keys.Add(ContextKeys.\u001D.\u0012());
	}

	public override void Initialize()
	{
		base.Initialize();
		this.SetCachedFields();
		ConeTargetingInfo coneInfo = this.GetConeInfo();
		coneInfo.m_affectsEnemies = base.IncludeEnemies();
		coneInfo.m_affectsAllies = base.IncludeAllies();
		coneInfo.m_affectsCaster = base.IncludeCaster();
		LaserTargetingInfo laserInfo = this.GetLaserInfo();
		laserInfo.affectsEnemies = base.IncludeEnemies();
		laserInfo.affectsAllies = base.IncludeAllies();
		laserInfo.affectsCaster = base.IncludeCaster();
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_ConeOrLaser item = new AbilityUtil_Targeter_ConeOrLaser(ability, this.GetConeInfo(), this.GetLaserInfo(), this.GetConeDistThreshold());
		return new List<AbilityUtil_Targeter>
		{
			item
		};
	}

	public bool ShouldUseCone(Vector3 freePos, ActorData caster)
	{
		Vector3 vector = freePos - caster.\u0016();
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return magnitude <= this.GetConeDistThreshold();
	}

	private void SetCachedFields()
	{
		ConeTargetingInfo cachedConeInfo;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_ConeOrLaser.SetCachedFields()).MethodHandle;
			}
			cachedConeInfo = this.m_targetSelMod.m_coneInfoMod.GetModifiedValue(this.m_coneInfo);
		}
		else
		{
			cachedConeInfo = this.m_coneInfo;
		}
		this.m_cachedConeInfo = cachedConeInfo;
		LaserTargetingInfo cachedLaserInfo;
		if (this.m_targetSelMod != null)
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
			cachedLaserInfo = this.m_targetSelMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo);
		}
		else
		{
			cachedLaserInfo = this.m_laserInfo;
		}
		this.m_cachedLaserInfo = cachedLaserInfo;
	}

	public float GetConeDistThreshold()
	{
		return (this.m_targetSelMod == null) ? this.m_coneDistThreshold : this.m_targetSelMod.m_coneDistThresholdMod.GetModifiedValue(this.m_coneDistThreshold);
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return (this.m_cachedConeInfo == null) ? this.m_coneInfo : this.m_cachedConeInfo;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_ConeOrLaser.GetLaserInfo()).MethodHandle;
			}
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_ConeOrLaser);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}
}
