using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_AoeRadius : GenericAbility_TargetSelectBase
{
	[Separator("Targeting Properties", true)]
	public float m_radius = 1f;

	[Space(10f)]
	public bool m_useSquareCenterPos;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private TargetSelectMod_AoeRadius m_targetSelMod;

	public override string GetUsageForEditor()
	{
		return base.GetContextUsageStr(ContextKeys.\u0018.GetName(), "on every hit actor, distance from center of AoE, in squares", true);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.\u0018.GetName());
	}

	public override void Initialize()
	{
		this.m_commonProperties.SetFloat(ContextKeys.\u000D.GetHash(), this.GetRadius());
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = new AbilityUtil_Targeter_AoE_Smooth(ability, this.GetRadius(), base.IgnoreLos(), true, false, -1);
		abilityUtil_Targeter_AoE_Smooth.SetAffectedGroups(base.IncludeEnemies(), base.IncludeAllies(), base.IncludeCaster());
		abilityUtil_Targeter_AoE_Smooth.m_customCenterPosDelegate = new AbilityUtil_Targeter_AoE_Smooth.CustomCenterPosDelegate(this.GetCenterPos);
		bool flag = ability.GetTargetData().Length == 0;
		abilityUtil_Targeter_AoE_Smooth.SetShowArcToShape(!flag);
		return new List<AbilityUtil_Targeter>
		{
			abilityUtil_Targeter_AoE_Smooth
		};
	}

	public Vector3 GetCenterPos(ActorData caster, AbilityTarget currentTarget)
	{
		if (this.UseSquareCenterPos())
		{
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
			if (boardSquareSafe != null)
			{
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_AoeRadius.GetCenterPos(ActorData, AbilityTarget)).MethodHandle;
				}
				return boardSquareSafe.ToVector3();
			}
		}
		return currentTarget.FreePos;
	}

	public float GetRadius()
	{
		return (this.m_targetSelMod == null) ? this.m_radius : this.m_targetSelMod.m_radiusMod.GetModifiedValue(this.m_radius);
	}

	public bool UseSquareCenterPos()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_AoeRadius.UseSquareCenterPos()).MethodHandle;
			}
			result = this.m_targetSelMod.m_useSquareCenterPosMod.GetModifiedValue(this.m_useSquareCenterPos);
		}
		else
		{
			result = this.m_useSquareCenterPos;
		}
		return result;
	}

	public override bool CanShowTargeterRangePreview(TargetData[] targetData)
	{
		return true;
	}

	public override float GetTargeterRangePreviewRadius(Ability ability, ActorData caster)
	{
		return this.GetRadius();
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_AoeRadius);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}
}
