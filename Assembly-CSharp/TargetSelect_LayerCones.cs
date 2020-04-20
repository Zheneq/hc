using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_LayerCones : GenericAbility_TargetSelectBase
{
	[Separator("Targeting Properties", true)]
	public float m_coneWidthAngle = 90f;

	public List<float> m_coneRadiusList;

	[Separator("Sequences", true)]
	public GameObject m_coneSequencePrefab;

	public TargetSelect_LayerCones.NumActiveLayerDelegate m_delegateNumActiveLayers;

	private TargetSelectMod_LayerCones m_targetSelMod;

	private List<float> m_cachedRadiusList = new List<float>();

	public override string GetUsageForEditor()
	{
		return base.GetContextUsageStr(ContextKeys.\u0003.GetName(), "on every hit actor, 0-based index of smallest cone with a hit, with smallest cone first", true) + base.GetContextUsageStr(ContextKeys.\u000F.GetName(), "Non-actor specific context, number of layers active", false);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.\u0003.GetName());
		names.Add(ContextKeys.\u000F.GetName());
	}

	public override void Initialize()
	{
		base.Initialize();
		if (this.m_targetSelMod != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_LayerCones.Initialize()).MethodHandle;
			}
			if (this.m_targetSelMod.m_useConeRadiusOverrides)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_cachedRadiusList = new List<float>(this.m_targetSelMod.m_coneRadiusOverrides);
				goto IL_61;
			}
		}
		this.m_cachedRadiusList = new List<float>(this.m_coneRadiusList);
		IL_61:
		this.m_cachedRadiusList.Sort();
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_LayerCones abilityUtil_Targeter_LayerCones = new AbilityUtil_Targeter_LayerCones(ability, this.GetConeWidthAngle(), this.m_cachedRadiusList, 0f, base.IgnoreLos());
		abilityUtil_Targeter_LayerCones.SetAffectedGroups(base.IncludeEnemies(), base.IncludeAllies(), base.IncludeCaster());
		return new List<AbilityUtil_Targeter>
		{
			abilityUtil_Targeter_LayerCones
		};
	}

	public float GetConeWidthAngle()
	{
		float result;
		if (this.m_targetSelMod != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_LayerCones.GetConeWidthAngle()).MethodHandle;
			}
			result = this.m_targetSelMod.m_coneWidthAngleMod.GetModifiedValue(this.m_coneWidthAngle);
		}
		else
		{
			result = this.m_coneWidthAngle;
		}
		return result;
	}

	public float GetMaxConeRadius()
	{
		float result = 0f;
		int numActiveLayers = this.GetNumActiveLayers();
		if (numActiveLayers > 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_LayerCones.GetMaxConeRadius()).MethodHandle;
			}
			result = this.m_cachedRadiusList[numActiveLayers - 1];
		}
		return result;
	}

	public int GetNumActiveLayers()
	{
		if (this.m_delegateNumActiveLayers != null)
		{
			return this.m_delegateNumActiveLayers(this.m_cachedRadiusList.Count);
		}
		return this.m_cachedRadiusList.Count;
	}

	public int GetLayerCount()
	{
		return this.m_cachedRadiusList.Count;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_LayerCones);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}

	public delegate int NumActiveLayerDelegate(int maxLayers);
}
