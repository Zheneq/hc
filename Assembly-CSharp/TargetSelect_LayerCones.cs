using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect_LayerCones : GenericAbility_TargetSelectBase
{
	public delegate int NumActiveLayerDelegate(int maxLayers);

	[Separator("Targeting Properties", true)]
	public float m_coneWidthAngle = 90f;

	public List<float> m_coneRadiusList;

	[Separator("Sequences", true)]
	public GameObject m_coneSequencePrefab;

	public NumActiveLayerDelegate m_delegateNumActiveLayers;

	private TargetSelectMod_LayerCones m_targetSelMod;

	private List<float> m_cachedRadiusList = new List<float>();

	public override string GetUsageForEditor()
	{
		return GetContextUsageStr(ContextKeys._0003.GetName(), "on every hit actor, 0-based index of smallest cone with a hit, with smallest cone first") + GetContextUsageStr(ContextKeys._000F.GetName(), "Non-actor specific context, number of layers active", false);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys._0003.GetName());
		names.Add(ContextKeys._000F.GetName());
	}

	public override void Initialize()
	{
		base.Initialize();
		if (m_targetSelMod != null)
		{
			if (m_targetSelMod.m_useConeRadiusOverrides)
			{
				m_cachedRadiusList = new List<float>(m_targetSelMod.m_coneRadiusOverrides);
				goto IL_0061;
			}
		}
		m_cachedRadiusList = new List<float>(m_coneRadiusList);
		goto IL_0061;
		IL_0061:
		m_cachedRadiusList.Sort();
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_LayerCones abilityUtil_Targeter_LayerCones = new AbilityUtil_Targeter_LayerCones(ability, GetConeWidthAngle(), m_cachedRadiusList, 0f, IgnoreLos());
		abilityUtil_Targeter_LayerCones.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		list.Add(abilityUtil_Targeter_LayerCones);
		return list;
	}

	public float GetConeWidthAngle()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle);
		}
		else
		{
			result = m_coneWidthAngle;
		}
		return result;
	}

	public float GetMaxConeRadius()
	{
		float result = 0f;
		int numActiveLayers = GetNumActiveLayers();
		if (numActiveLayers > 0)
		{
			result = m_cachedRadiusList[numActiveLayers - 1];
		}
		return result;
	}

	public int GetNumActiveLayers()
	{
		if (m_delegateNumActiveLayers != null)
		{
			return m_delegateNumActiveLayers(m_cachedRadiusList.Count);
		}
		return m_cachedRadiusList.Count;
	}

	public int GetLayerCount()
	{
		return m_cachedRadiusList.Count;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		m_targetSelMod = (modBase as TargetSelectMod_LayerCones);
	}

	protected override void OnTargetSelModRemoved()
	{
		m_targetSelMod = null;
	}
}
