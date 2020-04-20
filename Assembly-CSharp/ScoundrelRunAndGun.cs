using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoundrelRunAndGun : Ability
{
	public int m_damageAmount = 0x14;

	public float m_damageRadius = 5f;

	public bool m_penetrateLineOfSight;

	[Header("-- For energy refund from mod --")]
	public bool m_energyRefundAffectedByBuff;

	private AbilityMod_ScoundrelRunAndGun m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelRunAndGun.Start()).MethodHandle;
			}
			this.m_abilityName = "Run and Gun";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.ClearTargeters();
		if (this.GetExpectedNumberOfTargeters() < 2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelRunAndGun.SetupTargeter()).MethodHandle;
			}
			base.Targeter = new AbilityUtil_Targeter_ChargeAoE(this, this.m_damageRadius, this.m_damageRadius, this.m_damageRadius, -1, false, this.m_penetrateLineOfSight);
		}
		else
		{
			base.ClearTargeters();
			for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
			{
				base.Targeters.Add(new AbilityUtil_Targeter_ChargeAoE(this, this.m_damageRadius, this.m_damageRadius, this.m_damageRadius, -1, false, this.m_penetrateLineOfSight));
				base.Targeters[i].SetUseMultiTargetUpdate(true);
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private int ModdedDamageAmount()
	{
		int num = this.m_damageAmount;
		if (this.m_abilityMod != null)
		{
			num = Mathf.Max(0, this.m_abilityMod.m_damageMod.GetModifiedValue(num));
		}
		return num;
	}

	private int GetTechPointGainWithNoHits()
	{
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelRunAndGun.GetTechPointGainWithNoHits()).MethodHandle;
			}
			return this.m_abilityMod.m_techPointGainWithNoHits.GetModifiedValue(0);
		}
		return 0;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.GetTechPointGainWithNoHits() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelRunAndGun.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			if (base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) == 0)
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
				return this.GetTechPointGainWithNoHits();
			}
		}
		return 0;
	}

	public override bool StatusAdjustAdditionalTechPointForTargeting()
	{
		return this.m_energyRefundAffectedByBuff;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScoundrelRunAndGun))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ScoundrelRunAndGun);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_damageAmount)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, this.ModdedDamageAmount(), AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result = 1;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelRunAndGun.GetExpectedNumberOfTargeters()).MethodHandle;
			}
			if (this.m_abilityMod.m_numTargeters > 1)
			{
				result = this.m_abilityMod.m_numTargeters;
			}
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null) && boardSquareSafe.IsBaselineHeight())
		{
			if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
			{
				bool result;
				if (this.GetExpectedNumberOfTargeters() < 2)
				{
					result = (KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false) != null);
				}
				else
				{
					BoardSquare boardSquare;
					if (targetIndex == 0)
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
						boardSquare = caster.GetCurrentBoardSquare();
					}
					else
					{
						boardSquare = Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 1].GridPos);
					}
					bool flag = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe, boardSquare, false) != null;
					float squareSize = Board.Get().squareSize;
					float num = Vector3.Distance(boardSquare.ToVector3(), boardSquareSafe.ToVector3());
					bool flag2;
					if (num >= this.m_abilityMod.m_minDistanceBetweenSteps * squareSize)
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
						flag2 = (num <= this.m_abilityMod.m_maxDistanceBetweenSteps * squareSize);
					}
					else
					{
						flag2 = false;
					}
					bool flag3 = flag2;
					if (flag && flag3)
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
						if (this.m_abilityMod.m_minDistanceBetweenAnySteps > 0f)
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
							for (int i = 0; i < targetIndex; i++)
							{
								BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[i].GridPos);
								flag3 &= (Vector3.Distance(boardSquareSafe2.ToVector3(), boardSquareSafe.ToVector3()) >= this.m_abilityMod.m_minDistanceBetweenAnySteps * squareSize);
							}
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
					bool flag4;
					if (flag)
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
						flag4 = flag3;
					}
					else
					{
						flag4 = false;
					}
					result = flag4;
				}
				return result;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelRunAndGun.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
		}
		return false;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
