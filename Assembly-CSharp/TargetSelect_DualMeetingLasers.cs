using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_DualMeetingLasers : GenericAbility_TargetSelectBase
{
	[Separator("Targeting - Laser", true)]
	public float m_laserWidth = 0.5f;

	public float m_minMeetingDistFromCaster = 1f;

	public float m_maxMeetingDistFromCaster = 8f;

	public float m_laserStartForwardOffset;

	public float m_laserStartSideOffset = 0.5f;

	[Separator("Targeting - AoE", true)]
	public float m_aoeBaseRadius = 2.5f;

	public float m_aoeMinRadius;

	public float m_aoeMaxRadius = -1f;

	public float m_aoeRadiusChangePerUnitFromMin = 0.1f;

	[Header("-- Multiplier on radius if not all lasers meet")]
	public float m_radiusMultIfPartialBlock = 1f;

	[Space(10f)]
	public bool m_aoeIgnoreMinCoverDist = true;

	[Separator("Sequences", true)]
	public GameObject m_laserSequencePrefab;

	[Header("-- Use if laser doesn't have impact FX that spawns on end of laser, or for temp testing")]
	public GameObject m_aoeSequencePrefab;

	public TargetSelect_DualMeetingLasers.LaserCountDelegate m_delegateLaserCount;

	public TargetSelect_DualMeetingLasers.ExtraAoeRadiusDelegate m_delegateExtraAoeRadius;

	private TargetSelectMod_DualMeetingLasers m_targetSelMod;

	public override string GetUsageForEditor()
	{
		return base.GetContextUsageStr(ContextKeys.\u001A.GetName(), "on every hit actor, 1 if in AoE, 0 otherwise", true) + base.GetContextUsageStr(ContextKeys.\u0013.GetName(), "on every actor, distance of cursor pos from min distance, for interpolation", true);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.\u001A.GetName());
		names.Add(ContextKeys.\u0013.GetName());
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_ScampDualLasers abilityUtil_Targeter_ScampDualLasers = new AbilityUtil_Targeter_ScampDualLasers(ability, this.GetLaserWidth(), this.GetMinMeetingDistFromCaster(), this.GetMaxMeetingDistFromCaster(), this.GetLaserStartForwardOffset(), this.GetLaserStartSideOffset(), this.GetAoeBaseRadius(), this.GetAoeMinRadius(), this.GetAoeMaxRadius(), this.GetAoeRadiusChangePerUnitFromMin(), this.GetRadiusMultIfPartialBlock(), this.AoeIgnoreMinCoverDist());
		abilityUtil_Targeter_ScampDualLasers.SetAffectedGroups(base.IncludeEnemies(), base.IncludeAllies(), base.IncludeCaster());
		return new List<AbilityUtil_Targeter>
		{
			abilityUtil_Targeter_ScampDualLasers
		};
	}

	public override bool CanShowTargeterRangePreview(TargetData[] targetData)
	{
		return true;
	}

	public override float GetTargeterRangePreviewRadius(Ability ability, ActorData caster)
	{
		return this.GetMaxMeetingDistFromCaster() + this.GetAoeMinRadius();
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_DualMeetingLasers);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}

	public float GetLaserWidth()
	{
		float result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_DualMeetingLasers.GetLaserWidth()).MethodHandle;
			}
			result = this.m_targetSelMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public float GetMinMeetingDistFromCaster()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_DualMeetingLasers.GetMinMeetingDistFromCaster()).MethodHandle;
			}
			result = this.m_targetSelMod.m_minMeetingDistFromCasterMod.GetModifiedValue(this.m_minMeetingDistFromCaster);
		}
		else
		{
			result = this.m_minMeetingDistFromCaster;
		}
		return result;
	}

	public float GetMaxMeetingDistFromCaster()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_DualMeetingLasers.GetMaxMeetingDistFromCaster()).MethodHandle;
			}
			result = this.m_targetSelMod.m_maxMeetingDistFromCasterMod.GetModifiedValue(this.m_maxMeetingDistFromCaster);
		}
		else
		{
			result = this.m_maxMeetingDistFromCaster;
		}
		return result;
	}

	public float GetLaserStartForwardOffset()
	{
		return (this.m_targetSelMod == null) ? this.m_laserStartForwardOffset : this.m_targetSelMod.m_laserStartForwardOffsetMod.GetModifiedValue(this.m_laserStartForwardOffset);
	}

	public float GetLaserStartSideOffset()
	{
		return (this.m_targetSelMod == null) ? this.m_laserStartSideOffset : this.m_targetSelMod.m_laserStartSideOffsetMod.GetModifiedValue(this.m_laserStartSideOffset);
	}

	public float GetAoeBaseRadius()
	{
		float result;
		if (this.m_targetSelMod != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_DualMeetingLasers.GetAoeBaseRadius()).MethodHandle;
			}
			result = this.m_targetSelMod.m_aoeBaseRadiusMod.GetModifiedValue(this.m_aoeBaseRadius);
		}
		else
		{
			result = this.m_aoeBaseRadius;
		}
		return result;
	}

	public float GetAoeMinRadius()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_DualMeetingLasers.GetAoeMinRadius()).MethodHandle;
			}
			result = this.m_targetSelMod.m_aoeMinRadiusMod.GetModifiedValue(this.m_aoeMinRadius);
		}
		else
		{
			result = this.m_aoeMinRadius;
		}
		return result;
	}

	public float GetAoeMaxRadius()
	{
		float result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_DualMeetingLasers.GetAoeMaxRadius()).MethodHandle;
			}
			result = this.m_targetSelMod.m_aoeMaxRadiusMod.GetModifiedValue(this.m_aoeMaxRadius);
		}
		else
		{
			result = this.m_aoeMaxRadius;
		}
		return result;
	}

	public float GetAoeRadiusChangePerUnitFromMin()
	{
		float result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_DualMeetingLasers.GetAoeRadiusChangePerUnitFromMin()).MethodHandle;
			}
			result = this.m_targetSelMod.m_aoeRadiusChangePerUnitFromMinMod.GetModifiedValue(this.m_aoeRadiusChangePerUnitFromMin);
		}
		else
		{
			result = this.m_aoeRadiusChangePerUnitFromMin;
		}
		return result;
	}

	public float GetRadiusMultIfPartialBlock()
	{
		return (this.m_targetSelMod == null) ? this.m_radiusMultIfPartialBlock : this.m_targetSelMod.m_radiusMultIfPartialBlockMod.GetModifiedValue(this.m_radiusMultIfPartialBlock);
	}

	public bool AoeIgnoreMinCoverDist()
	{
		return (this.m_targetSelMod == null) ? this.m_aoeIgnoreMinCoverDist : this.m_targetSelMod.m_aoeIgnoreMinCoverDistMod.GetModifiedValue(this.m_aoeIgnoreMinCoverDist);
	}

	public delegate int LaserCountDelegate(AbilityTarget currentTarget, ActorData caster);

	public delegate float ExtraAoeRadiusDelegate(AbilityTarget currentTarget, ActorData targetingActor, float baseRadius);
}
