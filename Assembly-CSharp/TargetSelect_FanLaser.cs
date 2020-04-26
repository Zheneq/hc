using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect_FanLaser : GenericAbility_TargetSelectBase
{
	[Separator("Targeting Properties", true)]
	public LaserTargetingInfo m_laserInfo;

	[Space(10f)]
	public int m_laserCount = 3;

	public float m_angleInBetween = 10f;

	public bool m_changeAngleByCursorDistance = true;

	public float m_targeterMinAngle;

	public float m_targeterMaxAngle = 180f;

	public float m_startAngleOffset;

	[Header("-- For Interpolating Angle")]
	public float m_targeterMinInterpDistance = 0.5f;

	public float m_targeterMaxInterpDistance = 4f;

	[Separator("Laser End Aoe Radius", true)]
	public float m_laserEndAoeRadius;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public override string GetUsageForEditor()
	{
		return GetContextUsageStr(ContextKeys._0019.GetName(), "on every hit actor, number of laser hits on target") + GetContextUsageStr(ContextKeys._001A.GetName(), "on every hit actor, 1 if in laser end AoE, 0 otherwise");
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys._0019.GetName());
		names.Add(ContextKeys._001A.GetName());
	}

	public override void Initialize()
	{
		m_laserInfo.affectsEnemies = m_includeEnemies;
		m_laserInfo.affectsAllies = m_includeAllies;
		m_laserInfo.affectsCaster = m_includeCaster;
		m_laserInfo.penetrateLos = m_ignoreLos;
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_NekoDiscsFan abilityUtil_Targeter_NekoDiscsFan = new AbilityUtil_Targeter_NekoDiscsFan(ability, m_targeterMinAngle, m_targeterMaxAngle, m_targeterMinInterpDistance, m_targeterMaxInterpDistance, m_laserInfo.range, m_laserInfo.width, m_laserEndAoeRadius, m_laserInfo.maxTargets, m_laserCount, m_ignoreLos, 0f, m_startAngleOffset);
		abilityUtil_Targeter_NekoDiscsFan.SetFixedAngle(m_changeAngleByCursorDistance, m_angleInBetween);
		abilityUtil_Targeter_NekoDiscsFan.SetUseHitActorPosForLaserEnd(false);
		abilityUtil_Targeter_NekoDiscsFan.SetShowEndSquareHighlight(false);
		abilityUtil_Targeter_NekoDiscsFan.SetAffectedGroups(m_includeEnemies, m_includeAllies, m_includeCaster);
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		list.Add(abilityUtil_Targeter_NekoDiscsFan);
		return list;
	}
}
