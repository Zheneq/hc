using System;
using System.Collections.Generic;
using UnityEngine;

public class RampartBarricade_Prep : Ability
{
	[Header("-- Barrier Aiming")]
	public bool m_allowAimAtDiagonals;

	[Header("-- Knockback Hit Damage and Effect")]
	public int m_damageAmount = 0xA;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Laser and Knockback")]
	public float m_laserRange = 2f;

	public bool m_laserLengthIgnoreLos = true;

	public bool m_penetrateLos;

	public float m_knockbackDistance = 2f;

	public KnockbackType m_knockbackType;

	[Header("-- Sequences")]
	public GameObject m_removeShieldSequencePrefab;

	public GameObject m_applyShieldSequencePrefab;

	private bool m_snapToGrid = true;

	private Passive_Rampart m_passive;

	private AbilityMod_RampartBarricade_Prep m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Barricade";
		}
		this.Setup();
	}

	private void Setup()
	{
		if (this.m_passive == null)
		{
			this.m_passive = (base.GetComponent<PassiveData>().GetPassiveOfType(typeof(Passive_Rampart)) as Passive_Rampart);
		}
		if (this.m_passive != null)
		{
			Passive_Rampart passive = this.m_passive;
			AbilityModPropertyBarrierDataV2 cachedShieldBarrierData;
			if (this.m_abilityMod != null)
			{
				cachedShieldBarrierData = this.m_abilityMod.m_shieldBarrierDataMod;
			}
			else
			{
				cachedShieldBarrierData = null;
			}
			passive.SetCachedShieldBarrierData(cachedShieldBarrierData);
		}
		else
		{
			Log.Error(base.GetDebugIdentifier(string.Empty) + " did not find passive component Passive_Rampart", new object[0]);
		}
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_RampartKnockbackBarrier(this, this.GetLaserWidth(), this.GetLaserRange(), this.LaserLengthIgnoreLos(), this.GetKnockbackDistance(), this.m_knockbackType, this.PenetrateLos(), this.m_snapToGrid, this.AllowAimAtDiagonals());
		base.Targeter.SetAffectedGroups(true, false, this.AffectCaster());
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	public bool AllowAimAtDiagonals()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_allowAimAtDiagonalsMod.GetModifiedValue(this.m_allowAimAtDiagonals);
		}
		else
		{
			result = this.m_allowAimAtDiagonals;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_damageAmount : this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
		{
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public float GetLaserRange()
	{
		return (!this.m_abilityMod) ? this.m_laserRange : this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
	}

	public bool LaserLengthIgnoreLos()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserLengthIgnoreLosMod.GetModifiedValue(this.m_laserLengthIgnoreLos);
		}
		else
		{
			result = this.m_laserLengthIgnoreLos;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		return (!this.m_abilityMod) ? this.m_penetrateLos : this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
	}

	public float GetKnockbackDistance()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(this.m_knockbackDistance);
		}
		else
		{
			result = this.m_knockbackDistance;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		return this.m_passive.GetShieldBarrierData().m_width;
	}

	public KnockbackType GetKnockbackType()
	{
		return this.m_knockbackType;
	}

	public bool AffectCaster()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_effectToSelfOnCast.m_applyEffect;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamageAmount());
		this.m_enemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		base.AppendTooltipNumbersFromBaseModEffects(ref result, AbilityTooltipSubject.Primary, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartBarricade_Prep))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_RampartBarricade_Prep);
		}
		this.Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartBarricade_Prep abilityMod_RampartBarricade_Prep = modAsBase as AbilityMod_RampartBarricade_Prep;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_RampartBarricade_Prep)
		{
			val = abilityMod_RampartBarricade_Prep.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_RampartBarricade_Prep)
		{
			effectInfo = abilityMod_RampartBarricade_Prep.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		Passive_Rampart component = base.GetComponent<Passive_Rampart>();
		if (component != null)
		{
			if (component.m_normalShieldBarrierData != null)
			{
				component.m_normalShieldBarrierData.AddTooltipTokens(tokens, "ShieldBarrier", false, null);
			}
		}
	}

	public override Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		Vector3 a;
		Vector3 b;
		this.GetBarrierPositionAndFacing(targets, out a, out b);
		return a + b;
	}

	public void GetBarrierPositionAndFacing(List<AbilityTarget> targets, out Vector3 position, out Vector3 facing)
	{
		facing = targets[0].AimDirection;
		position = targets[0].FreePos;
		if (this.m_snapToGrid)
		{
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(targets[0].GridPos);
			if (boardSquareSafe != null)
			{
				Vector3 b;
				facing = VectorUtils.GetDirectionAndOffsetToClosestSide(boardSquareSafe, targets[0].FreePos, this.AllowAimAtDiagonals(), out b);
				position = boardSquareSafe.ToVector3() + b;
			}
		}
	}
}
