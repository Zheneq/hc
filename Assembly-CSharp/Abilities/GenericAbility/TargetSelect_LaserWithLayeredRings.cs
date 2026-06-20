using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect_LaserWithLayeredRings : GenericAbility_TargetSelectBase
{
	[Separator("Laser Targeting Info", true)]
	public float m_laserWidth;

	public float m_laserRange = 5f;

	public int m_laserMaxTargets = 1;

	[Separator("Aoe Targeting Info", true)]
	public List<float> m_aoeRadiusList = new List<float>();

	public bool m_clampAoeCenterToCursor = true;

	public float m_minRangeIfClampToCursor;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private List<RadiusToLayerIndex> m_radiusToLayerList = new List<RadiusToLayerIndex>();

	private float m_cachedOuterRadius = 1f;

	public override string GetUsageForEditor()
	{
		return GetContextUsageStr(ContextKeys.s_InAoe.GetName(), "1 if target is in AoE, 0 otherwise") + GetContextUsageStr(ContextKeys.s_Layer.GetName(), "for indicating which layer a target in AoE (sorted from inner to outer)") + GetContextUsageStr(ContextKeys.s_DistFromStart.GetName(), "distance from start of AoE center, in squares");
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.s_InAoe.GetName());
		names.Add(ContextKeys.s_Layer.GetName());
		names.Add(ContextKeys.s_DistFromStart.GetName());
	}

	public override void Initialize()
	{
		m_radiusToLayerList.Clear();
		for (int i = 0; i < m_aoeRadiusList.Count; i++)
		{
			m_radiusToLayerList.Add(new RadiusToLayerIndex(m_aoeRadiusList[i]));
		}
		while (true)
		{
			if (m_radiusToLayerList.Count == 0)
			{
				Log.Error(string.Concat(GetType(), " has empty aoe radius list"));
				m_radiusToLayerList.Add(new RadiusToLayerIndex(1f));
			}
			RadiusToLayerIndex.SortAndSetLayerIndex(m_radiusToLayerList);
			m_cachedOuterRadius = m_radiusToLayerList[m_radiusToLayerList.Count - 1].m_radius;
			return;
		}
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		AbilityUtil_Targeter_LaserWithLayeredRings abilityUtil_Targeter_LaserWithLayeredRings = new AbilityUtil_Targeter_LaserWithLayeredRings(ability, m_laserWidth, m_laserRange, IgnoreLos(), m_laserMaxTargets, m_clampAoeCenterToCursor, false, m_aoeRadiusList);
		abilityUtil_Targeter_LaserWithLayeredRings.m_minRangeIfClampToCursor = m_minRangeIfClampToCursor;
		abilityUtil_Targeter_LaserWithLayeredRings.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
		if (m_laserWidth <= 0f)
		{
			abilityUtil_Targeter_LaserWithLayeredRings.SetShowArcToShape(true);
		}
		list.Add(abilityUtil_Targeter_LaserWithLayeredRings);
		return list;
	}

	public List<RadiusToLayerIndex> GetRadiusToLayerList()
	{
		return m_radiusToLayerList;
	}
}
