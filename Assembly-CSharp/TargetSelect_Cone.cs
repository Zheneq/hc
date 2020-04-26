using AbilityContextNamespace;
using System.Collections.Generic;
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
		return GetContextUsageStr(ContextKeys._0018.GetName(), "distance from start of cone position, in squares");
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys._0018.GetName());
	}

	public override void Initialize()
	{
		SetCachedFields();
		ConeTargetingInfo coneInfo = GetConeInfo();
		coneInfo.m_affectsAllies = IncludeAllies();
		coneInfo.m_affectsEnemies = IncludeEnemies();
		coneInfo.m_affectsCaster = IncludeCaster();
		coneInfo.m_penetrateLos = IgnoreLos();
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		ConeTargetingInfo coneInfo = GetConeInfo();
		AbilityUtil_Targeter_DirectionCone item = new AbilityUtil_Targeter_DirectionCone(ability, coneInfo.m_widthAngleDeg, coneInfo.m_radiusInSquares, coneInfo.m_backwardsOffset, coneInfo.m_penetrateLos, true, coneInfo.m_affectsEnemies, coneInfo.m_affectsAllies, coneInfo.m_affectsCaster);
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		list.Add(item);
		return list;
	}

	private void SetCachedFields()
	{
		m_cachedConeInfo = ((m_targetSelMod == null) ? m_coneInfo : m_targetSelMod.m_coneInfoMod.GetModifiedValue(m_coneInfo));
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return (m_cachedConeInfo == null) ? m_coneInfo : m_cachedConeInfo;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		m_targetSelMod = (modBase as TargetSelectMod_Cone);
	}

	protected override void OnTargetSelModRemoved()
	{
		m_targetSelMod = null;
	}
}
