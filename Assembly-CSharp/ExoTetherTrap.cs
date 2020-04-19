using System;
using System.Collections.Generic;
using UnityEngine;

public class ExoTetherTrap : Ability
{
	[Header("-- Targeting and Direct Damage")]
	[Space(20f)]
	public int m_laserDamageAmount = 5;

	public LaserTargetingInfo m_laserInfo;

	public StandardActorEffectData m_baseEffectData;

	public StandardEffectInfo m_laserOnHitEffect;

	[Header("-- Tether Info")]
	public float m_tetherDistance = 5f;

	public int m_tetherBreakDamage = 0x14;

	public StandardEffectInfo m_tetherBreakEffect;

	public bool m_breakTetherOnNonGroundBasedMovement;

	[Header("-- Extra Damage based on distance")]
	public float m_extraDamagePerMoveDist;

	public int m_maxExtraDamageFromMoveDist;

	[Header("-- Cooldown Reduction if tether didn't break")]
	public int m_cdrOnTetherEndIfNotTriggered;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	public GameObject m_beamSequence;

	public GameObject m_tetherBreakHitSequence;

	private AbilityMod_ExoTetherTrap m_abilityMod;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedTetherBreakEffect;

	private StandardActorEffectData m_cachedBaseEffectData;

	private StandardEffectInfo m_cachedLaserOnHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Exo Tether Trap";
		}
		this.SetupTargeter();
	}

	public void SetupTargeter()
	{
		this.SetCachedFields();
		AbilityUtil_Targeter_ExoTether abilityUtil_Targeter_ExoTether = new AbilityUtil_Targeter_ExoTether(this, this.GetLaserInfo(), this.GetLaserInfo());
		abilityUtil_Targeter_ExoTether.SetAffectedGroups(true, false, false);
		base.Targeter = abilityUtil_Targeter_ExoTether;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserInfo().range;
	}

	private void SetCachedFields()
	{
		this.m_cachedLaserInfo = this.m_laserInfo;
		LaserTargetingInfo cachedLaserInfo;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoTetherTrap.SetCachedFields()).MethodHandle;
			}
			cachedLaserInfo = this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo);
		}
		else
		{
			cachedLaserInfo = this.m_laserInfo;
		}
		this.m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedTetherBreakEffect;
		if (this.m_abilityMod)
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
			cachedTetherBreakEffect = this.m_abilityMod.m_tetherBreakEffectMod.GetModifiedValue(this.m_tetherBreakEffect);
		}
		else
		{
			cachedTetherBreakEffect = this.m_tetherBreakEffect;
		}
		this.m_cachedTetherBreakEffect = cachedTetherBreakEffect;
		StandardActorEffectData cachedBaseEffectData;
		if (this.m_abilityMod)
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
			cachedBaseEffectData = this.m_abilityMod.m_baseEffectDataMod.GetModifiedValue(this.m_baseEffectData);
		}
		else
		{
			cachedBaseEffectData = this.m_baseEffectData.GetShallowCopy();
		}
		this.m_cachedBaseEffectData = cachedBaseEffectData;
		if (this.m_beamSequence != null)
		{
			this.m_cachedBaseEffectData.m_sequencePrefabs = new GameObject[]
			{
				this.m_beamSequence
			};
		}
		StandardEffectInfo cachedLaserOnHitEffect;
		if (this.m_abilityMod)
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
			cachedLaserOnHitEffect = this.m_abilityMod.m_laserOnHitEffectMod.GetModifiedValue(this.m_laserOnHitEffect);
		}
		else
		{
			cachedLaserOnHitEffect = this.m_laserOnHitEffect;
		}
		this.m_cachedLaserOnHitEffect = cachedLaserOnHitEffect;
	}

	public int GetLaserDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoTetherTrap.GetLaserDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			result = this.m_laserDamageAmount;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoTetherTrap.GetLaserInfo()).MethodHandle;
			}
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	public StandardActorEffectData GetBaseEffectData()
	{
		return (this.m_cachedBaseEffectData == null) ? this.m_baseEffectData : this.m_cachedBaseEffectData;
	}

	public StandardEffectInfo GetLaserOnHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedLaserOnHitEffect != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoTetherTrap.GetLaserOnHitEffect()).MethodHandle;
			}
			result = this.m_cachedLaserOnHitEffect;
		}
		else
		{
			result = this.m_laserOnHitEffect;
		}
		return result;
	}

	public float GetTetherDistance()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoTetherTrap.GetTetherDistance()).MethodHandle;
			}
			result = this.m_abilityMod.m_tetherDistanceMod.GetModifiedValue(this.m_tetherDistance);
		}
		else
		{
			result = this.m_tetherDistance;
		}
		return result;
	}

	public int GetTetherBreakDamage()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoTetherTrap.GetTetherBreakDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_tetherBreakDamageMod.GetModifiedValue(this.m_tetherBreakDamage);
		}
		else
		{
			result = this.m_tetherBreakDamage;
		}
		return result;
	}

	public StandardEffectInfo GetTetherBreakEffect()
	{
		return (this.m_cachedTetherBreakEffect == null) ? this.m_tetherBreakEffect : this.m_cachedTetherBreakEffect;
	}

	public bool BreakTetherOnNonGroundBasedMovement()
	{
		return (!this.m_abilityMod) ? this.m_breakTetherOnNonGroundBasedMovement : this.m_abilityMod.m_breakTetherOnNonGroundBasedMovementMod.GetModifiedValue(this.m_breakTetherOnNonGroundBasedMovement);
	}

	public float GetExtraDamagePerMoveDist()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoTetherTrap.GetExtraDamagePerMoveDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamagePerMoveDistMod.GetModifiedValue(this.m_extraDamagePerMoveDist);
		}
		else
		{
			result = this.m_extraDamagePerMoveDist;
		}
		return result;
	}

	public int GetMaxExtraDamageFromMoveDist()
	{
		int result;
		if (this.m_abilityMod)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoTetherTrap.GetMaxExtraDamageFromMoveDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxExtraDamageFromMoveDistMod.GetModifiedValue(this.m_maxExtraDamageFromMoveDist);
		}
		else
		{
			result = this.m_maxExtraDamageFromMoveDist;
		}
		return result;
	}

	public int GetCdrOnTetherEndIfNotTriggered()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoTetherTrap.GetCdrOnTetherEndIfNotTriggered()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrOnTetherEndIfNotTriggeredMod.GetModifiedValue(this.m_cdrOnTetherEndIfNotTriggered);
		}
		else
		{
			result = this.m_cdrOnTetherEndIfNotTriggered;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoTetherTrap abilityMod_ExoTetherTrap = modAsBase as AbilityMod_ExoTetherTrap;
		StandardActorEffectData standardActorEffectData = (!abilityMod_ExoTetherTrap) ? this.m_baseEffectData : abilityMod_ExoTetherTrap.m_baseEffectDataMod.GetModifiedValue(this.m_baseEffectData);
		standardActorEffectData.AddTooltipTokens(tokens, "TetherBaseEffectData", abilityMod_ExoTetherTrap != null, this.m_baseEffectData);
		StandardEffectInfo effectInfo;
		if (abilityMod_ExoTetherTrap)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoTetherTrap.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			effectInfo = abilityMod_ExoTetherTrap.m_laserOnHitEffectMod.GetModifiedValue(this.m_laserOnHitEffect);
		}
		else
		{
			effectInfo = this.m_laserOnHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "LaserOnHitEffect", this.m_laserOnHitEffect, true);
		base.AddTokenInt(tokens, "Damage_FirstTurn", string.Empty, (!abilityMod_ExoTetherTrap) ? this.m_laserDamageAmount : abilityMod_ExoTetherTrap.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount), false);
		string name = "Damage_TetherBreak";
		string empty = string.Empty;
		int val;
		if (abilityMod_ExoTetherTrap)
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
			val = abilityMod_ExoTetherTrap.m_tetherBreakDamageMod.GetModifiedValue(this.m_tetherBreakDamage);
		}
		else
		{
			val = this.m_tetherBreakDamage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "TetherDistance";
		string desc = "distance from starting position";
		int num;
		if (abilityMod_ExoTetherTrap)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num = (int)abilityMod_ExoTetherTrap.m_tetherDistanceMod.GetModifiedValue(this.m_tetherDistance);
		}
		else
		{
			num = (int)this.m_tetherDistance;
		}
		base.AddTokenInt(tokens, name2, desc, num, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_ExoTetherTrap)
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
			effectInfo2 = abilityMod_ExoTetherTrap.m_tetherBreakEffectMod.GetModifiedValue(this.m_tetherBreakEffect);
		}
		else
		{
			effectInfo2 = this.m_tetherBreakEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "TetherBreakEffect", this.m_tetherBreakEffect, true);
		base.AddTokenInt(tokens, "CdrOnTetherEndIfNotTriggered", string.Empty, (!abilityMod_ExoTetherTrap) ? this.m_cdrOnTetherEndIfNotTriggered : abilityMod_ExoTetherTrap.m_cdrOnTetherEndIfNotTriggeredMod.GetModifiedValue(this.m_cdrOnTetherEndIfNotTriggered), false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetLaserDamageAmount());
		this.GetBaseEffectData().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, this.GetLaserDamageAmount(), AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoTetherTrap))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoTetherTrap.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ExoTetherTrap);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
