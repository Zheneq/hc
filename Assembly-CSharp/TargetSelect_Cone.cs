using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_Cone : GenericAbility_TargetSelectBase
{
	[Separator("Input Params", true)]
	public ConeTargetingInfo m_coneInfo;

	[Separator("Sequences", true)]
	public GameObject m_coneSequencePrefab;

	private TargetSelectMod_Cone m_targetSelMod;

	private ConeTargetingInfo m_cachedConeInfo;

	public override string GetUsageForEditor()
	{
		return base.GetContextUsageStr(ContextKeys.symbol_0018.GetName(), "distance from start of cone position, in squares", true);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.symbol_0018.GetName());
	}

	public override void Initialize()
	{
		this.SetCachedFields();
		ConeTargetingInfo coneInfo = this.GetConeInfo();
		coneInfo.m_affectsAllies = base.IncludeAllies();
		coneInfo.m_affectsEnemies = base.IncludeEnemies();
		coneInfo.m_affectsCaster = base.IncludeCaster();
		coneInfo.m_penetrateLos = base.IgnoreLos();
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		ConeTargetingInfo coneInfo = this.GetConeInfo();
		AbilityUtil_Targeter_DirectionCone item = new AbilityUtil_Targeter_DirectionCone(ability, coneInfo.m_widthAngleDeg, coneInfo.m_radiusInSquares, coneInfo.m_backwardsOffset, coneInfo.m_penetrateLos, true, coneInfo.m_affectsEnemies, coneInfo.m_affectsAllies, coneInfo.m_affectsCaster, -1, false);
		return new List<AbilityUtil_Targeter>
		{
			item
		};
	}

	private void SetCachedFields()
	{
		this.m_cachedConeInfo = ((this.m_targetSelMod == null) ? this.m_coneInfo : this.m_targetSelMod.m_coneInfoMod.GetModifiedValue(this.m_coneInfo));
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return (this.m_cachedConeInfo == null) ? this.m_coneInfo : this.m_cachedConeInfo;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_Cone);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}
}
