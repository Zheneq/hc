using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_BouncingLaser : GenericAbility_TargetSelectBase
{
	[Separator("Input Params", true)]
	public float m_laserWidth = 1f;

	public float m_distPerBounce = 7.5f;

	public float m_maxTotalDist = 10f;

	public int m_maxBounces = 1;

	public int m_maxTargets = 1;

	[Separator("Sequences", true)]
	public GameObject m_bounceLaserSequencePrefab;

	private const string c_endpointIndex = "EndpointIndex";

	private const string c_hitOrder = "HitOrder";

	public static ContextNameKeyPair s_cvarEndpointIndex = new ContextNameKeyPair("EndpointIndex");

	public static ContextNameKeyPair s_cvarHitOrder = new ContextNameKeyPair("HitOrder");

	public override string GetUsageForEditor()
	{
		return base.GetContextUsageStr("HitOrder", "on every enemy, order in which they are hit.", true) + base.GetContextUsageStr("EndpointIndex", "on every enemy, 0-based index of which segment hit that enemy", true);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add("HitOrder");
		names.Add("EndpointIndex");
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_BounceLaser abilityUtil_Targeter_BounceLaser = new AbilityUtil_Targeter_BounceLaser(ability, this.m_laserWidth, this.m_distPerBounce, this.m_maxTotalDist, this.m_maxBounces, this.m_maxTargets, false);
		abilityUtil_Targeter_BounceLaser.SetAffectedGroups(this.m_includeEnemies, this.m_includeAllies, this.m_includeCaster);
		return new List<AbilityUtil_Targeter>
		{
			abilityUtil_Targeter_BounceLaser
		};
	}
}
