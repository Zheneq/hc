using System.Collections.Generic;
using UnityEngine;

public class NanoSmithChargeAndCreateBarrier : Ability
{
	[Header("-- Barrier ")]
	public bool m_snapToGrid;

	public StandardBarrierData m_barrierData;

	[Header("-- Sequences ------------------------------")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Charge and Create Barrier";
		}
		base.Targeter = new AbilityUtil_Targeter_BarrierWithCharge(this, m_barrierData.m_width, m_snapToGrid);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe) != null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
