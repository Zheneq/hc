using System.Collections.Generic;
using UnityEngine;

public class ScoundrelRunAndGun : Ability
{
	public int m_damageAmount = 20;

	public float m_damageRadius = 5f;

	public bool m_penetrateLineOfSight;

	[Header("-- For energy refund from mod --")]
	public bool m_energyRefundAffectedByBuff;

	private AbilityMod_ScoundrelRunAndGun m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Run and Gun";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		ClearTargeters();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					base.Targeter = new AbilityUtil_Targeter_ChargeAoE(this, m_damageRadius, m_damageRadius, m_damageRadius, -1, false, m_penetrateLineOfSight);
					return;
				}
			}
		}
		ClearTargeters();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			base.Targeters.Add(new AbilityUtil_Targeter_ChargeAoE(this, m_damageRadius, m_damageRadius, m_damageRadius, -1, false, m_penetrateLineOfSight));
			base.Targeters[i].SetUseMultiTargetUpdate(true);
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private int ModdedDamageAmount()
	{
		int num = m_damageAmount;
		if (m_abilityMod != null)
		{
			num = Mathf.Max(0, m_abilityMod.m_damageMod.GetModifiedValue(num));
		}
		return num;
	}

	private int GetTechPointGainWithNoHits()
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_techPointGainWithNoHits.GetModifiedValue(0);
				}
			}
		}
		return 0;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetTechPointGainWithNoHits() > 0)
		{
			if (base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) == 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return GetTechPointGainWithNoHits();
					}
				}
			}
		}
		return 0;
	}

	public override bool StatusAdjustAdditionalTechPointForTargeting()
	{
		return m_energyRefundAffectedByBuff;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScoundrelRunAndGun))
		{
			m_abilityMod = (abilityMod as AbilityMod_ScoundrelRunAndGun);
			SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, ModdedDamageAmount());
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result = 1;
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_numTargeters > 1)
			{
				result = m_abilityMod.m_numTargeters;
			}
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (!(boardSquareSafe == null) && boardSquareSafe.IsValidForGameplay())
		{
			if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
			{
				bool flag = true;
				if (GetExpectedNumberOfTargeters() < 2)
				{
					return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false) != null;
				}
				BoardSquare boardSquare;
				if (targetIndex == 0)
				{
					boardSquare = caster.GetCurrentBoardSquare();
				}
				else
				{
					boardSquare = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
				}
				bool flag2 = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe, boardSquare, false) != null;
				float squareSize = Board.Get().squareSize;
				float num = Vector3.Distance(boardSquare.ToVector3(), boardSquareSafe.ToVector3());
				int num2;
				if (num >= m_abilityMod.m_minDistanceBetweenSteps * squareSize)
				{
					num2 = ((num <= m_abilityMod.m_maxDistanceBetweenSteps * squareSize) ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
				bool flag3 = (byte)num2 != 0;
				if (flag2 && flag3)
				{
					if (m_abilityMod.m_minDistanceBetweenAnySteps > 0f)
					{
						for (int i = 0; i < targetIndex; i++)
						{
							BoardSquare boardSquareSafe2 = Board.Get().GetSquare(currentTargets[i].GridPos);
							flag3 &= (Vector3.Distance(boardSquareSafe2.ToVector3(), boardSquareSafe.ToVector3()) >= m_abilityMod.m_minDistanceBetweenAnySteps * squareSize);
						}
					}
				}
				int result;
				if (flag2)
				{
					result = (flag3 ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			}
		}
		return false;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
