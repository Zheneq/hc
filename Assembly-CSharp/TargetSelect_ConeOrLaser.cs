using AbilityContextNamespace;
using System.Collections.Generic;
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
		return GetContextUsageStr(ContextKeys.s_DistFromStart.GetName(), "distance from start of cone position, in squares") + GetContextUsageStr(s_cvarInCone.GetName(), "Whether the target hit is in cone") + GetContextUsageStr(ContextKeys.s_AngleFromCenter.GetName(), "angle from center of cone");
	}

	public override void ListContextNamesForEditor(List<string> keys)
	{
		keys.Add(ContextKeys.s_DistFromStart.GetName());
		keys.Add(s_cvarInCone.GetName());
		keys.Add(ContextKeys.s_AngleFromCenter.GetName());
	}

	public override void Initialize()
	{
		base.Initialize();
		SetCachedFields();
		ConeTargetingInfo coneInfo = GetConeInfo();
		coneInfo.m_affectsEnemies = IncludeEnemies();
		coneInfo.m_affectsAllies = IncludeAllies();
		coneInfo.m_affectsCaster = IncludeCaster();
		LaserTargetingInfo laserInfo = GetLaserInfo();
		laserInfo.affectsEnemies = IncludeEnemies();
		laserInfo.affectsAllies = IncludeAllies();
		laserInfo.affectsCaster = IncludeCaster();
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_ConeOrLaser item = new AbilityUtil_Targeter_ConeOrLaser(ability, GetConeInfo(), GetLaserInfo(), GetConeDistThreshold());
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		list.Add(item);
		return list;
	}

	public bool ShouldUseCone(Vector3 freePos, ActorData caster)
	{
		Vector3 vector = freePos - caster.GetTravelBoardSquareWorldPosition();
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return magnitude <= GetConeDistThreshold();
	}

	private void SetCachedFields()
	{
		ConeTargetingInfo cachedConeInfo;
		if (m_targetSelMod != null)
		{
			cachedConeInfo = m_targetSelMod.m_coneInfoMod.GetModifiedValue(m_coneInfo);
		}
		else
		{
			cachedConeInfo = m_coneInfo;
		}
		m_cachedConeInfo = cachedConeInfo;
		LaserTargetingInfo cachedLaserInfo;
		if (m_targetSelMod != null)
		{
			cachedLaserInfo = m_targetSelMod.m_laserInfoMod.GetModifiedValue(m_laserInfo);
		}
		else
		{
			cachedLaserInfo = m_laserInfo;
		}
		m_cachedLaserInfo = cachedLaserInfo;
	}

	public float GetConeDistThreshold()
	{
		return (m_targetSelMod == null) ? m_coneDistThreshold : m_targetSelMod.m_coneDistThresholdMod.GetModifiedValue(m_coneDistThreshold);
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return (m_cachedConeInfo == null) ? m_coneInfo : m_cachedConeInfo;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserInfo != null)
		{
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		m_targetSelMod = (modBase as TargetSelectMod_ConeOrLaser);
	}

	protected override void OnTargetSelModRemoved()
	{
		m_targetSelMod = null;
	}
}
