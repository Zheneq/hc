using System;
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
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChargeAndCreateBarrier.Start()).MethodHandle;
			}
			this.m_abilityName = "Charge and Create Barrier";
		}
		base.Targeter = new AbilityUtil_Targeter_BarrierWithCharge(this, this.m_barrierData.m_width, this.m_snapToGrid);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare destination = Board.\u000E().\u000E(target.GridPos);
		return KnockbackUtils.BuildStraightLineChargePath(caster, destination) != null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
