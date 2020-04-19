using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect_ChargeSingleStep : GenericAbility_TargetSelectBase
{
	[Separator("Targeting Properties", true)]
	public AbilityAreaShape m_destShape;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private TargetSelectMod_ChargeSingleStep m_targetSelMod;

	public override string GetUsageForEditor()
	{
		return "Intended for single click charge abilities. Can add shape field to hit targets on destination.";
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_Charge item = new AbilityUtil_Targeter_Charge(ability, this.GetDestShape(), base.IgnoreLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, base.IncludeEnemies(), base.IncludeAllies());
		return new List<AbilityUtil_Targeter>
		{
			item
		};
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_ChargeSingleStep);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}

	public AbilityAreaShape GetDestShape()
	{
		AbilityAreaShape result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_ChargeSingleStep.GetDestShape()).MethodHandle;
			}
			result = this.m_targetSelMod.m_destShapeMod.GetModifiedValue(this.m_destShape);
		}
		else
		{
			result = this.m_destShape;
		}
		return result;
	}

	public override bool HandleCustomTargetValidation(Ability ability, ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (boardSquare != null && boardSquare.\u0016())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_ChargeSingleStep.HandleCustomTargetValidation(Ability, ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquare != caster.\u0012())
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
				int num;
				return KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquare, caster.\u0012(), false, out num);
			}
		}
		return false;
	}

	public override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
