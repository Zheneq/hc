using System;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiSwordDash : Ability
{
	[Header("-- Targeting")]
	public float m_damageRadius = 2f;

	public float m_damageRadiusAtStart = 2f;

	public float m_damageRadiusAtEnd;

	public bool m_penetrateLineOfSight;

	[Space(5f)]
	public bool m_canMoveAfterEvade;

	[Header("-- MaxTargets -> How many targets can be hit total | MaxDamageTargets-> how many targets can be damaged")]
	public int m_maxTargets;

	public int m_maxDamageTargets = 1;

	[Header("-- Enemy Hits, Dash Phase")]
	public int m_dashDamage = 0x1E;

	public int m_dashLessDamagePerTarget = 5;

	public StandardEffectInfo m_dashEnemyHitEffect;

	[Header("-- Effect on Self")]
	public StandardEffectInfo m_dashSelfHitEffect;

	[Header("-- Mark data")]
	public StandardEffectInfo m_markEffectInfo;

	[Header("-- Energy Refund if target dashed away")]
	public int m_energyRefundIfTargetDashedAway;

	public bool m_energyRefundIgnoreBuff = true;

	[Separator("For Chain Ability (Knockback phase)", true)]
	public int m_knockbackDamage = 0x1E;

	public int m_knockbackLessDamagePerTarget = 5;

	public float m_knockbackExtraDamageFromDamageTakenMult;

	[Space(10f)]
	public int m_knockbackExtraDamageByDist;

	public int m_knockbackExtraDamageChangePerDist;

	[Header("-- Knockback")]
	public float m_knockbackDist = 2f;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	[Separator("Sequences - Dash Phase (for knockback sequences, see chain ability)", true)]
	public GameObject m_castSequencePrefab;

	public GameObject m_afterimageSequencePrefab;

	private SamuraiAfterimageStrike m_chainedStrike;

	private AbilityMod_SamuraiSwordDash m_abilityMod;

	private Samurai_SyncComponent m_syncComponent;

	private StandardEffectInfo m_cachedDashEnemyHitEffect;

	private StandardEffectInfo m_cachedDashSelfHitEffect;

	private StandardEffectInfo m_cachedMarkEffectInfo;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Sword Dash";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.m_chainedStrike == null)
		{
			foreach (Ability ability in this.m_chainAbilities)
			{
				this.m_chainedStrike = (ability as SamuraiAfterimageStrike);
				if (this.m_chainedStrike != null)
				{
					goto IL_5A;
				}
			}
		}
		IL_5A:
		this.m_syncComponent = base.ActorData.GetComponent<Samurai_SyncComponent>();
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_SamuraiShowdown(this, this.GetDamageRadiusAtStart(), this.GetDamageRadiusAtEnd(), this.GetDamageRadius(), this.GetMaxTargets(), false, this.PenetrateLineOfSight(), this.m_knockbackDist, this.m_knockbackType);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedDashEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedDashEnemyHitEffect = this.m_abilityMod.m_dashEnemyHitEffectMod.GetModifiedValue(this.m_dashEnemyHitEffect);
		}
		else
		{
			cachedDashEnemyHitEffect = this.m_dashEnemyHitEffect;
		}
		this.m_cachedDashEnemyHitEffect = cachedDashEnemyHitEffect;
		StandardEffectInfo cachedDashSelfHitEffect;
		if (this.m_abilityMod)
		{
			cachedDashSelfHitEffect = this.m_abilityMod.m_dashSelfHitEffectMod.GetModifiedValue(this.m_dashSelfHitEffect);
		}
		else
		{
			cachedDashSelfHitEffect = this.m_dashSelfHitEffect;
		}
		this.m_cachedDashSelfHitEffect = cachedDashSelfHitEffect;
		StandardEffectInfo cachedMarkEffectInfo;
		if (this.m_abilityMod)
		{
			cachedMarkEffectInfo = this.m_abilityMod.m_markEffectInfoMod.GetModifiedValue(this.m_markEffectInfo);
		}
		else
		{
			cachedMarkEffectInfo = this.m_markEffectInfo;
		}
		this.m_cachedMarkEffectInfo = cachedMarkEffectInfo;
	}

	public float GetDamageRadius()
	{
		return (!this.m_abilityMod) ? this.m_damageRadius : this.m_abilityMod.m_damageRadiusMod.GetModifiedValue(this.m_damageRadius);
	}

	public float GetDamageRadiusAtStart()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageRadiusAtStartMod.GetModifiedValue(this.m_damageRadiusAtStart);
		}
		else
		{
			result = this.m_damageRadiusAtStart;
		}
		return result;
	}

	public float GetDamageRadiusAtEnd()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageRadiusAtEndMod.GetModifiedValue(this.m_damageRadiusAtEnd);
		}
		else
		{
			result = this.m_damageRadiusAtEnd;
		}
		return result;
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
		}
		else
		{
			result = this.m_penetrateLineOfSight;
		}
		return result;
	}

	public bool CanMoveAfterEvade()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canMoveAfterEvadeMod.GetModifiedValue(this.m_canMoveAfterEvade);
		}
		else
		{
			result = this.m_canMoveAfterEvade;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public int GetMaxDamageTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxDamageTargetsMod.GetModifiedValue(this.m_maxDamageTargets);
		}
		else
		{
			result = this.m_maxDamageTargets;
		}
		return result;
	}

	public int GetDashDamage()
	{
		return (!this.m_abilityMod) ? this.m_dashDamage : this.m_abilityMod.m_dashDamageMod.GetModifiedValue(this.m_dashDamage);
	}

	public int GetDashLessDamagePerTarget()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_dashLessDamagePerTargetMod.GetModifiedValue(this.m_dashLessDamagePerTarget);
		}
		else
		{
			result = this.m_dashLessDamagePerTarget;
		}
		return result;
	}

	public StandardEffectInfo GetDashEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedDashEnemyHitEffect != null)
		{
			result = this.m_cachedDashEnemyHitEffect;
		}
		else
		{
			result = this.m_dashEnemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetDashSelfHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedDashSelfHitEffect != null)
		{
			result = this.m_cachedDashSelfHitEffect;
		}
		else
		{
			result = this.m_dashSelfHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetMarkEffectInfo()
	{
		StandardEffectInfo result;
		if (this.m_cachedMarkEffectInfo != null)
		{
			result = this.m_cachedMarkEffectInfo;
		}
		else
		{
			result = this.m_markEffectInfo;
		}
		return result;
	}

	public int GetEnergyRefundIfTargetDashedAway()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_energyRefundIfTargetDashedAwayMod.GetModifiedValue(this.m_energyRefundIfTargetDashedAway);
		}
		else
		{
			result = this.m_energyRefundIfTargetDashedAway;
		}
		return result;
	}

	public int GetKnockbackDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_knockbackDamageMod.GetModifiedValue(this.m_knockbackDamage);
		}
		else
		{
			result = this.m_knockbackDamage;
		}
		return result;
	}

	public int GetKnockbackLessDamagePerTarget()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_knockbackLessDamagePerTargetMod.GetModifiedValue(this.m_knockbackLessDamagePerTarget);
		}
		else
		{
			result = this.m_knockbackLessDamagePerTarget;
		}
		return result;
	}

	public float GetKnockbackExtraDamageFromDamageTakenMult()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_knockbackExtraDamageFromDamageTakenMultMod.GetModifiedValue(this.m_knockbackExtraDamageFromDamageTakenMult);
		}
		else
		{
			result = this.m_knockbackExtraDamageFromDamageTakenMult;
		}
		return result;
	}

	public int GetKnockbackExtraDamageByDist()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_knockbackExtraDamageByDistMod.GetModifiedValue(this.m_knockbackExtraDamageByDist);
		}
		else
		{
			result = this.m_knockbackExtraDamageByDist;
		}
		return result;
	}

	public int GetKnockbackExtraDamageChangePerDist()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_knockbackExtraDamageChangePerDistMod.GetModifiedValue(this.m_knockbackExtraDamageChangePerDist);
		}
		else
		{
			result = this.m_knockbackExtraDamageChangePerDist;
		}
		return result;
	}

	public float GetKnockbackDist()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_knockbackDistMod.GetModifiedValue(this.m_knockbackDist);
		}
		else
		{
			result = this.m_knockbackDist;
		}
		return result;
	}

	public KnockbackType GetKnockbackType()
	{
		return (!this.m_abilityMod) ? this.m_knockbackType : this.m_abilityMod.m_knockbackTypeMod.GetModifiedValue(this.m_knockbackType);
	}

	public int CalcExtraDamageForDashDist(BoardSquare startSquare, BoardSquare endSquare)
	{
		int num = 0;
		if (this.GetKnockbackExtraDamageByDist() <= 0)
		{
			if (this.GetKnockbackExtraDamageChangePerDist() == 0)
			{
				return num;
			}
		}
		num = this.GetKnockbackExtraDamageByDist();
		if (endSquare != null)
		{
			if (startSquare != null)
			{
				if (this.GetKnockbackExtraDamageChangePerDist() != 0)
				{
					int num2;
					if (KnockbackUtils.CanBuildStraightLineChargePath(base.ActorData, endSquare, startSquare, false, out num2))
					{
						int num3 = num2 - 2;
						if (num3 > 0)
						{
							num += num3 * this.GetKnockbackExtraDamageChangePerDist();
						}
					}
				}
			}
		}
		return num;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		int num = this.GetDashDamage();
		if (this.m_chainedStrike != null)
		{
			num += this.GetKnockbackDamage();
		}
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, num));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		AbilityUtil_Targeter_SamuraiShowdown abilityUtil_Targeter_SamuraiShowdown = base.Targeter as AbilityUtil_Targeter_SamuraiShowdown;
		if (abilityUtil_Targeter_SamuraiShowdown != null && abilityUtil_Targeter_SamuraiShowdown.OrderedHitActors.Contains(targetActor))
		{
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			bool flag = true;
			if (this.GetMaxDamageTargets() > 0)
			{
				int num = abilityUtil_Targeter_SamuraiShowdown.OrderedHitActors.IndexOf(targetActor);
				if (num >= this.GetMaxDamageTargets())
				{
					flag = false;
				}
			}
			if (flag)
			{
				int num2 = this.GetDashDamage();
				int num3 = this.GetDashLessDamagePerTarget();
				if (this.m_chainedStrike != null)
				{
					num2 += this.GetKnockbackDamage();
					num3 += this.m_knockbackLessDamagePerTarget;
				}
				int num4 = num2 - num3 * (base.Targeter.GetNumActorsInRange() - 1);
				num4 = Mathf.Max(0, num4);
				Ability.AddNameplateValueForSingleHit(ref dictionary, base.Targeter, targetActor, num4, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
				Dictionary<AbilityTooltipSymbol, int> dictionary2;
				if (this.m_syncComponent != null)
				{
					(dictionary2 = dictionary)[AbilityTooltipSymbol.Damage] = dictionary2[AbilityTooltipSymbol.Damage] + this.m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
				}
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeter.LastUpdatingGridPos);
				BoardSquare currentBoardSquare = base.ActorData.CurrentBoardSquare;
				(dictionary2 = dictionary)[AbilityTooltipSymbol.Damage] = dictionary2[AbilityTooltipSymbol.Damage] + this.CalcExtraDamageForDashDist(currentBoardSquare, boardSquareSafe);
			}
			else
			{
				dictionary[AbilityTooltipSymbol.Damage] = 0;
			}
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		SamuraiAfterimageStrike x = null;
		foreach (Ability ability in this.m_chainAbilities)
		{
			x = (ability as SamuraiAfterimageStrike);
			if (x != null)
			{
				break;
			}
		}
		int num = this.m_dashDamage;
		int num2 = this.m_dashLessDamagePerTarget;
		if (x != null)
		{
			num += this.m_knockbackDamage;
			num2 += this.m_knockbackLessDamagePerTarget;
		}
		base.AddTokenInt(tokens, "DamageAmount", "includes chained AfterimageStrike if present", num, false);
		base.AddTokenInt(tokens, "LessDamagePerTarget", "includes chained AfterimageStrike if present", num2, false);
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "MaxDamageTargets", string.Empty, this.m_maxDamageTargets, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_dashEnemyHitEffect, "DashEnemyHitEffect", this.m_dashEnemyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_dashSelfHitEffect, "DashSelfHitEffect", this.m_dashSelfHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_markEffectInfo, "MarkEffectInfo", this.m_markEffectInfo, true);
		base.AddTokenInt(tokens, "KnockbackDamage", string.Empty, this.m_knockbackDamage, false);
		base.AddTokenInt(tokens, "KnockbackLessDamagePerTarget", string.Empty, this.m_knockbackLessDamagePerTarget, false);
		base.AddTokenInt(tokens, "KnockbackExtraDamageByDist", string.Empty, this.m_knockbackExtraDamageByDist, false);
		base.AddTokenInt(tokens, "KnockbackExtraDamageChangePerDist", string.Empty, this.m_knockbackExtraDamageChangePerDist, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
				{
					return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false) != null;
				}
			}
		}
		return false;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return this.CanMoveAfterEvade();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiSwordDash))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SamuraiSwordDash);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
