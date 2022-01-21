using AbilityContextNamespace;
using System.Collections.Generic;
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
		return GetContextUsageStr(ContextKeys.s_HitOrder.GetName(), "on every non-caster hit actor, order in which they are hit in laser") + GetContextUsageStr(ContextKeys.s_DistFromStart.GetName(), "on every non-caster hit actor, distance from caster");
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.s_HitOrder.GetName());
		names.Add(ContextKeys.s_DistFromStart.GetName());
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		AbilityUtil_Targeter abilityUtil_Targeter;
		if (GetAoeRadiusAroundStart() <= 0f)
		{
			abilityUtil_Targeter = new AbilityUtil_Targeter_Laser(ability, GetLaserWidth(), GetLaserRange(), IgnoreLos(), GetMaxTargets(), IncludeAllies(), IncludeCaster());
		}
		else
		{
			abilityUtil_Targeter = new AbilityUtil_Targeter_ClaymoreSlam(ability, GetLaserRange(), GetLaserWidth(), GetMaxTargets(), 360f, GetAoeRadiusAroundStart(), 0f, IgnoreLos());
		}
		abilityUtil_Targeter.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
		list.Add(abilityUtil_Targeter);
		return list;
	}

	public float GetLaserRange()
	{
		return (m_targetSelMod == null) ? m_laserRange : m_targetSelMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
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

	public int GetMaxTargets()
	{
		int result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			result = m_maxTargets;
		}
		return result;
	}

	public float GetAoeRadiusAroundStart()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_aoeRadiusAroundStartMod.GetModifiedValue(m_aoeRadiusAroundStart);
		}
		else
		{
			result = m_aoeRadiusAroundStart;
		}
		return result;
	}

	public override bool CanShowTargeterRangePreview(TargetData[] targetData)
	{
		return true;
	}

	public override float GetTargeterRangePreviewRadius(Ability ability, ActorData caster)
	{
		return GetLaserRange();
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		m_targetSelMod = (modBase as TargetSelectMod_Laser);
	}

	protected override void OnTargetSelModRemoved()
	{
		m_targetSelMod = null;
	}
}
