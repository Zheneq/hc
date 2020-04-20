using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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
		return base.GetContextUsageStr(ContextKeys.\u0019.GetName(), "on every hit actor, number of laser hits on target", true) + base.GetContextUsageStr(ContextKeys.\u001A.GetName(), "on every hit actor, 1 if in laser end AoE, 0 otherwise", true);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.\u0019.GetName());
		names.Add(ContextKeys.\u001A.GetName());
	}

	public override void Initialize()
	{
		this.m_laserInfo.affectsEnemies = this.m_includeEnemies;
		this.m_laserInfo.affectsAllies = this.m_includeAllies;
		this.m_laserInfo.affectsCaster = this.m_includeCaster;
		this.m_laserInfo.penetrateLos = this.m_ignoreLos;
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_NekoDiscsFan abilityUtil_Targeter_NekoDiscsFan = new AbilityUtil_Targeter_NekoDiscsFan(ability, this.m_targeterMinAngle, this.m_targeterMaxAngle, this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance, this.m_laserInfo.range, this.m_laserInfo.width, this.m_laserEndAoeRadius, this.m_laserInfo.maxTargets, this.m_laserCount, this.m_ignoreLos, 0f, this.m_startAngleOffset);
		abilityUtil_Targeter_NekoDiscsFan.SetFixedAngle(this.m_changeAngleByCursorDistance, this.m_angleInBetween);
		abilityUtil_Targeter_NekoDiscsFan.SetUseHitActorPosForLaserEnd(false);
		abilityUtil_Targeter_NekoDiscsFan.SetShowEndSquareHighlight(false);
		abilityUtil_Targeter_NekoDiscsFan.SetAffectedGroups(this.m_includeEnemies, this.m_includeAllies, this.m_includeCaster);
		return new List<AbilityUtil_Targeter>
		{
			abilityUtil_Targeter_NekoDiscsFan
		};
	}
}
