using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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
		return base.GetContextUsageStr(ContextKeys.\u001A.\u0012(), "1 if target is in AoE, 0 otherwise", true) + base.GetContextUsageStr(ContextKeys.\u0003.\u0012(), "for indicating which layer a target in AoE (sorted from inner to outer)", true) + base.GetContextUsageStr(ContextKeys.\u0018.\u0012(), "distance from start of AoE center, in squares", true);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.\u001A.\u0012());
		names.Add(ContextKeys.\u0003.\u0012());
		names.Add(ContextKeys.\u0018.\u0012());
	}

	public override void Initialize()
	{
		this.m_radiusToLayerList.Clear();
		for (int i = 0; i < this.m_aoeRadiusList.Count; i++)
		{
			this.m_radiusToLayerList.Add(new RadiusToLayerIndex(this.m_aoeRadiusList[i]));
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_LaserWithLayeredRings.Initialize()).MethodHandle;
		}
		if (this.m_radiusToLayerList.Count == 0)
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
			Log.Error(base.GetType() + " has empty aoe radius list", new object[0]);
			this.m_radiusToLayerList.Add(new RadiusToLayerIndex(1f));
		}
		RadiusToLayerIndex.SortAndSetLayerIndex(this.m_radiusToLayerList);
		this.m_cachedOuterRadius = this.m_radiusToLayerList[this.m_radiusToLayerList.Count - 1].m_radius;
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		AbilityUtil_Targeter_LaserWithLayeredRings abilityUtil_Targeter_LaserWithLayeredRings = new AbilityUtil_Targeter_LaserWithLayeredRings(ability, this.m_laserWidth, this.m_laserRange, base.IgnoreLos(), this.m_laserMaxTargets, this.m_clampAoeCenterToCursor, false, this.m_aoeRadiusList);
		abilityUtil_Targeter_LaserWithLayeredRings.m_minRangeIfClampToCursor = this.m_minRangeIfClampToCursor;
		abilityUtil_Targeter_LaserWithLayeredRings.SetAffectedGroups(base.IncludeEnemies(), base.IncludeAllies(), base.IncludeCaster());
		if (this.m_laserWidth <= 0f)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_LaserWithLayeredRings.CreateTargeters(Ability)).MethodHandle;
			}
			abilityUtil_Targeter_LaserWithLayeredRings.SetShowArcToShape(true);
		}
		list.Add(abilityUtil_Targeter_LaserWithLayeredRings);
		return list;
	}

	public List<RadiusToLayerIndex> GetRadiusToLayerList()
	{
		return this.m_radiusToLayerList;
	}
}
