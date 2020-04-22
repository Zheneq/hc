using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect_DualMeetingLasers : GenericAbility_TargetSelectBase
{
	public delegate int LaserCountDelegate(AbilityTarget currentTarget, ActorData caster);

	public delegate float ExtraAoeRadiusDelegate(AbilityTarget currentTarget, ActorData targetingActor, float baseRadius);

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

	public LaserCountDelegate m_delegateLaserCount;

	public ExtraAoeRadiusDelegate m_delegateExtraAoeRadius;

	private TargetSelectMod_DualMeetingLasers m_targetSelMod;

	public override string GetUsageForEditor()
	{
		return GetContextUsageStr(ContextKeys._001A.GetName(), "on every hit actor, 1 if in AoE, 0 otherwise") + GetContextUsageStr(ContextKeys._0013.GetName(), "on every actor, distance of cursor pos from min distance, for interpolation");
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys._001A.GetName());
		names.Add(ContextKeys._0013.GetName());
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_ScampDualLasers abilityUtil_Targeter_ScampDualLasers = new AbilityUtil_Targeter_ScampDualLasers(ability, GetLaserWidth(), GetMinMeetingDistFromCaster(), GetMaxMeetingDistFromCaster(), GetLaserStartForwardOffset(), GetLaserStartSideOffset(), GetAoeBaseRadius(), GetAoeMinRadius(), GetAoeMaxRadius(), GetAoeRadiusChangePerUnitFromMin(), GetRadiusMultIfPartialBlock(), AoeIgnoreMinCoverDist());
		abilityUtil_Targeter_ScampDualLasers.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		list.Add(abilityUtil_Targeter_ScampDualLasers);
		return list;
	}

	public override bool CanShowTargeterRangePreview(TargetData[] targetData)
	{
		return true;
	}

	public override float GetTargeterRangePreviewRadius(Ability ability, ActorData caster)
	{
		return GetMaxMeetingDistFromCaster() + GetAoeMinRadius();
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		m_targetSelMod = (modBase as TargetSelectMod_DualMeetingLasers);
	}

	protected override void OnTargetSelModRemoved()
	{
		m_targetSelMod = null;
	}

	public float GetLaserWidth()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public float GetMinMeetingDistFromCaster()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_minMeetingDistFromCasterMod.GetModifiedValue(m_minMeetingDistFromCaster);
		}
		else
		{
			result = m_minMeetingDistFromCaster;
		}
		return result;
	}

	public float GetMaxMeetingDistFromCaster()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_maxMeetingDistFromCasterMod.GetModifiedValue(m_maxMeetingDistFromCaster);
		}
		else
		{
			result = m_maxMeetingDistFromCaster;
		}
		return result;
	}

	public float GetLaserStartForwardOffset()
	{
		return (m_targetSelMod == null) ? m_laserStartForwardOffset : m_targetSelMod.m_laserStartForwardOffsetMod.GetModifiedValue(m_laserStartForwardOffset);
	}

	public float GetLaserStartSideOffset()
	{
		return (m_targetSelMod == null) ? m_laserStartSideOffset : m_targetSelMod.m_laserStartSideOffsetMod.GetModifiedValue(m_laserStartSideOffset);
	}

	public float GetAoeBaseRadius()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_aoeBaseRadiusMod.GetModifiedValue(m_aoeBaseRadius);
		}
		else
		{
			result = m_aoeBaseRadius;
		}
		return result;
	}

	public float GetAoeMinRadius()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_aoeMinRadiusMod.GetModifiedValue(m_aoeMinRadius);
		}
		else
		{
			result = m_aoeMinRadius;
		}
		return result;
	}

	public float GetAoeMaxRadius()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_aoeMaxRadiusMod.GetModifiedValue(m_aoeMaxRadius);
		}
		else
		{
			result = m_aoeMaxRadius;
		}
		return result;
	}

	public float GetAoeRadiusChangePerUnitFromMin()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_aoeRadiusChangePerUnitFromMinMod.GetModifiedValue(m_aoeRadiusChangePerUnitFromMin);
		}
		else
		{
			result = m_aoeRadiusChangePerUnitFromMin;
		}
		return result;
	}

	public float GetRadiusMultIfPartialBlock()
	{
		return (m_targetSelMod == null) ? m_radiusMultIfPartialBlock : m_targetSelMod.m_radiusMultIfPartialBlockMod.GetModifiedValue(m_radiusMultIfPartialBlock);
	}

	public bool AoeIgnoreMinCoverDist()
	{
		return (m_targetSelMod == null) ? m_aoeIgnoreMinCoverDist : m_targetSelMod.m_aoeIgnoreMinCoverDistMod.GetModifiedValue(m_aoeIgnoreMinCoverDist);
	}
}
