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
		AbilityUtil_Targeter_Charge item = new AbilityUtil_Targeter_Charge(ability, GetDestShape(), IgnoreLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, IncludeEnemies(), IncludeAllies());
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		list.Add(item);
		return list;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		m_targetSelMod = (modBase as TargetSelectMod_ChargeSingleStep);
	}

	protected override void OnTargetSelModRemoved()
	{
		m_targetSelMod = null;
	}

	public AbilityAreaShape GetDestShape()
	{
		AbilityAreaShape result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_destShapeMod.GetModifiedValue(m_destShape);
		}
		else
		{
			result = m_destShape;
		}
		return result;
	}

	public override bool HandleCustomTargetValidation(Ability ability, ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null && boardSquareSafe.IsBaselineHeight())
		{
			if (boardSquareSafe != caster.GetCurrentBoardSquare())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						int numSquaresInPath;
						return KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out numSquaresInPath);
					}
					}
				}
			}
		}
		return false;
	}

	public override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
