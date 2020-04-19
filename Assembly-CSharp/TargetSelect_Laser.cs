using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_Laser : GenericAbility_TargetSelectBase
{
	[Separator("Targeting Properties", true)]
	public float m_laserRange = 5f;

	public float m_laserWidth = 1f;

	public int m_maxTargets;

	[Separator("AoE around start", true)]
	public float m_aoeRadiusAroundStart;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public GameObject m_aoeAtStartSequencePrefab;

	private TargetSelectMod_Laser m_targetSelMod;

	public override string GetUsageForEditor()
	{
		return base.GetContextUsageStr(ContextKeys.\u0011.\u0012(), "on every non-caster hit actor, order in which they are hit in laser", true) + base.GetContextUsageStr(ContextKeys.\u0018.\u0012(), "on every non-caster hit actor, distance from caster", true);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.\u0011.\u0012());
		names.Add(ContextKeys.\u0018.\u0012());
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		AbilityUtil_Targeter abilityUtil_Targeter;
		if (this.GetAoeRadiusAroundStart() <= 0f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Laser.CreateTargeters(Ability)).MethodHandle;
			}
			abilityUtil_Targeter = new AbilityUtil_Targeter_Laser(ability, this.GetLaserWidth(), this.GetLaserRange(), base.IgnoreLos(), this.GetMaxTargets(), base.IncludeAllies(), base.IncludeCaster());
		}
		else
		{
			abilityUtil_Targeter = new AbilityUtil_Targeter_ClaymoreSlam(ability, this.GetLaserRange(), this.GetLaserWidth(), this.GetMaxTargets(), 360f, this.GetAoeRadiusAroundStart(), 0f, base.IgnoreLos(), true, false, false, false);
		}
		abilityUtil_Targeter.SetAffectedGroups(base.IncludeEnemies(), base.IncludeAllies(), base.IncludeCaster());
		list.Add(abilityUtil_Targeter);
		return list;
	}

	public float GetLaserRange()
	{
		return (this.m_targetSelMod == null) ? this.m_laserRange : this.m_targetSelMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Laser.GetLaserWidth()).MethodHandle;
			}
			result = this.m_targetSelMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if (this.m_targetSelMod != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Laser.GetMaxTargets()).MethodHandle;
			}
			result = this.m_targetSelMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public float GetAoeRadiusAroundStart()
	{
		float result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Laser.GetAoeRadiusAroundStart()).MethodHandle;
			}
			result = this.m_targetSelMod.m_aoeRadiusAroundStartMod.GetModifiedValue(this.m_aoeRadiusAroundStart);
		}
		else
		{
			result = this.m_aoeRadiusAroundStart;
		}
		return result;
	}

	public override bool CanShowTargeterRangePreview(TargetData[] targetData)
	{
		return true;
	}

	public override float GetTargeterRangePreviewRadius(Ability ability, ActorData caster)
	{
		return this.GetLaserRange();
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_Laser);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}
}
