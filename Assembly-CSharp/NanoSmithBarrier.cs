using System.Collections.Generic;
using UnityEngine;

public class NanoSmithBarrier : Ability
{
	[Header("-- Barrier Info")]
	public bool m_snapToGrid = true;

	public StandardBarrierData m_barrierData;

	[Separator("Sequences", true)]
	public GameObject m_castSeqPrefab;

	[TextArea(1, 10)]
	public string m_notes;

	private AbilityMod_NanoSmithBarrier m_abilityMod;

	private StandardBarrierData m_cachedBarrierData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Kinetic Barrier";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		StandardBarrierData barrierData = GetBarrierData();
		float width = barrierData.m_width;
		if (GetExpectedNumberOfTargeters() < 2)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					base.Targeter = new AbilityUtil_Targeter_Barrier(this, barrierData.m_width, m_snapToGrid);
					return;
				}
			}
		}
		ClearTargeters();
		base.Targeters.Add(new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false));
		AbilityUtil_Targeter_Barrier abilityUtil_Targeter_Barrier = new AbilityUtil_Targeter_Barrier(this, width, m_snapToGrid, false, false);
		abilityUtil_Targeter_Barrier.SetUseMultiTargetUpdate(true);
		base.Targeters.Add(abilityUtil_Targeter_Barrier);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_barrierData.ReportAbilityTooltipNumbers(ref numbers);
		return numbers;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, GetNumTargets());
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (GetExpectedNumberOfTargeters() > 1 && targetIndex > 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
					bool flag = false;
					BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[0].GridPos);
					return boardSquareSafe2 == boardSquareSafe;
				}
				}
			}
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithBarrier abilityMod_NanoSmithBarrier = modAsBase as AbilityMod_NanoSmithBarrier;
		StandardBarrierData standardBarrierData;
		if ((bool)abilityMod_NanoSmithBarrier)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			standardBarrierData = abilityMod_NanoSmithBarrier.m_barrierDataMod.GetModifiedValue(m_barrierData);
		}
		else
		{
			standardBarrierData = m_barrierData;
		}
		StandardBarrierData standardBarrierData2 = standardBarrierData;
		standardBarrierData2.AddTooltipTokens(tokens, "BarrierData", abilityMod_NanoSmithBarrier != null, m_barrierData);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_NanoSmithBarrier))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_NanoSmithBarrier);
			m_cachedBarrierData = m_abilityMod.m_barrierDataMod.GetModifiedValue(m_barrierData);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		m_cachedBarrierData = null;
		SetupTargeter();
	}

	private StandardBarrierData GetBarrierData()
	{
		StandardBarrierData result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_barrierData;
		}
		else
		{
			result = m_cachedBarrierData;
		}
		return result;
	}
}
