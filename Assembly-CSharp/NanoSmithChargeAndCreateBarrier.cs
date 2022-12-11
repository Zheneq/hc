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
		Targeter = new AbilityUtil_Targeter_BarrierWithCharge(this, m_barrierData.m_width, m_snapToGrid);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public override bool CustomTargetValidation(
		ActorData caster,
		AbilityTarget target,
		int targetIndex,
		List<AbilityTarget> currentTargets)
	{
		BoardSquare square = Board.Get().GetSquare(target.GridPos);
		return KnockbackUtils.BuildStraightLineChargePath(caster, square) != null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
