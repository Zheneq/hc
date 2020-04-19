using System;
using System.Collections.Generic;
using UnityEngine;

public class MartyrAoeOnReactHit : Ability
{
	[Header("-- Targeting --")]
	public bool m_canTargetEnemy = true;

	public bool m_canTargetAlly = true;

	public bool m_canTargetSelf = true;

	[Space(10f)]
	public bool m_targetingIgnoreLos;

	[Header("-- Base Effect Data")]
	public StandardActorEffectData m_enemyBaseEffectData;

	public StandardActorEffectData m_allyBaseEffectData;

	[Header("-- Extra Shielding for Allies")]
	public int m_extraAbsorbPerCrystal;

	[Header("-- For React Area --")]
	public float m_reactBaseRadius = 1.5f;

	public float m_reactRadiusPerCrystal = 0.25f;

	public bool m_reactOnlyOncePerTurn;

	public bool m_reactPenetrateLos;

	public bool m_reactIncludeEffectTarget = true;

	[Header("-- On React Hit --")]
	public int m_reactAoeDamage = 0xA;

	public int m_reactDamagePerCrystal = 3;

	public StandardEffectInfo m_reactEnemyHitEffect;

	public int m_reactHealOnTarget;

	public int m_reactEnergyOnCasterPerReact;

	[Header("-- Cooldown reduction if no reacts")]
	public int m_cdrIfNoReactionTriggered;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	public GameObject m_onReactTriggerSequencePrefab;

	private Martyr_SyncComponent m_syncComp;

	private AbilityMod_MartyrAoeOnReactHit m_abilityMod;

	private StandardActorEffectData m_cachedEnemyBaseEffectData;

	private StandardActorEffectData m_cachedAllyBaseEffectData;

	private StandardEffectInfo m_cachedReactEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.Start()).MethodHandle;
			}
			this.m_abilityName = "MartyrAoeOnReactHit";
		}
		this.Setup();
	}

	private void Setup()
	{
		if (this.m_syncComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.Setup()).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Martyr_SyncComponent>();
		}
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_AoE_AroundActor(this, 1f, this.ReactPenetrateLos(), true, false, -1, this.CanTargetEnemy(), this.CanTargetAlly(), this.CanTargetSelf())
		{
			m_customRadiusDelegate = new AbilityUtil_Targeter_AoE_Smooth.GetRadiusDelegate(this.GetRadiusForTargeter)
		};
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedEnemyBaseEffectData;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.SetCachedFields()).MethodHandle;
			}
			cachedEnemyBaseEffectData = this.m_abilityMod.m_enemyBaseEffectDataMod.GetModifiedValue(this.m_enemyBaseEffectData);
		}
		else
		{
			cachedEnemyBaseEffectData = this.m_enemyBaseEffectData;
		}
		this.m_cachedEnemyBaseEffectData = cachedEnemyBaseEffectData;
		StandardActorEffectData cachedAllyBaseEffectData;
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
			cachedAllyBaseEffectData = this.m_abilityMod.m_allyBaseEffectDataMod.GetModifiedValue(this.m_allyBaseEffectData);
		}
		else
		{
			cachedAllyBaseEffectData = this.m_allyBaseEffectData;
		}
		this.m_cachedAllyBaseEffectData = cachedAllyBaseEffectData;
		StandardEffectInfo cachedReactEnemyHitEffect;
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
			cachedReactEnemyHitEffect = this.m_abilityMod.m_reactEnemyHitEffectMod.GetModifiedValue(this.m_reactEnemyHitEffect);
		}
		else
		{
			cachedReactEnemyHitEffect = this.m_reactEnemyHitEffect;
		}
		this.m_cachedReactEnemyHitEffect = cachedReactEnemyHitEffect;
	}

	public bool CanTargetEnemy()
	{
		return (!this.m_abilityMod) ? this.m_canTargetEnemy : this.m_abilityMod.m_canTargetEnemyMod.GetModifiedValue(this.m_canTargetEnemy);
	}

	public bool CanTargetAlly()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.CanTargetAlly()).MethodHandle;
			}
			result = this.m_abilityMod.m_canTargetAllyMod.GetModifiedValue(this.m_canTargetAlly);
		}
		else
		{
			result = this.m_canTargetAlly;
		}
		return result;
	}

	public bool CanTargetSelf()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.CanTargetSelf()).MethodHandle;
			}
			result = this.m_abilityMod.m_canTargetSelfMod.GetModifiedValue(this.m_canTargetSelf);
		}
		else
		{
			result = this.m_canTargetSelf;
		}
		return result;
	}

	public bool TargetingIgnoreLos()
	{
		return (!this.m_abilityMod) ? this.m_targetingIgnoreLos : this.m_abilityMod.m_targetingIgnoreLosMod.GetModifiedValue(this.m_targetingIgnoreLos);
	}

	public StandardActorEffectData GetEnemyBaseEffectData()
	{
		StandardActorEffectData result;
		if (this.m_cachedEnemyBaseEffectData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetEnemyBaseEffectData()).MethodHandle;
			}
			result = this.m_cachedEnemyBaseEffectData;
		}
		else
		{
			result = this.m_enemyBaseEffectData;
		}
		return result;
	}

	public StandardActorEffectData GetAllyBaseEffectData()
	{
		StandardActorEffectData result;
		if (this.m_cachedAllyBaseEffectData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetAllyBaseEffectData()).MethodHandle;
			}
			result = this.m_cachedAllyBaseEffectData;
		}
		else
		{
			result = this.m_allyBaseEffectData;
		}
		return result;
	}

	public int GetExtraAbsorbPerCrystal()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetExtraAbsorbPerCrystal()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraAbsorbPerCrystalMod.GetModifiedValue(this.m_extraAbsorbPerCrystal);
		}
		else
		{
			result = this.m_extraAbsorbPerCrystal;
		}
		return result;
	}

	public float GetReactBaseRadius()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetReactBaseRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_reactBaseRadiusMod.GetModifiedValue(this.m_reactBaseRadius);
		}
		else
		{
			result = this.m_reactBaseRadius;
		}
		return result;
	}

	public float GetReactRadiusPerCrystal()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetReactRadiusPerCrystal()).MethodHandle;
			}
			result = this.m_abilityMod.m_reactRadiusPerCrystalMod.GetModifiedValue(this.m_reactRadiusPerCrystal);
		}
		else
		{
			result = this.m_reactRadiusPerCrystal;
		}
		return result;
	}

	public bool ReactOnlyOncePerTurn()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.ReactOnlyOncePerTurn()).MethodHandle;
			}
			result = this.m_abilityMod.m_reactOnlyOncePerTurnMod.GetModifiedValue(this.m_reactOnlyOncePerTurn);
		}
		else
		{
			result = this.m_reactOnlyOncePerTurn;
		}
		return result;
	}

	public bool ReactPenetrateLos()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.ReactPenetrateLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_reactPenetrateLosMod.GetModifiedValue(this.m_reactPenetrateLos);
		}
		else
		{
			result = this.m_reactPenetrateLos;
		}
		return result;
	}

	public bool ReactIncludeEffectTarget()
	{
		return (!this.m_abilityMod) ? this.m_reactIncludeEffectTarget : this.m_abilityMod.m_reactIncludeEffectTargetMod.GetModifiedValue(this.m_reactIncludeEffectTarget);
	}

	public int GetReactAoeDamage()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetReactAoeDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_reactAoeDamageMod.GetModifiedValue(this.m_reactAoeDamage);
		}
		else
		{
			result = this.m_reactAoeDamage;
		}
		return result;
	}

	public int GetReactDamagePerCrystal()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetReactDamagePerCrystal()).MethodHandle;
			}
			result = this.m_abilityMod.m_reactDamagePerCrystalMod.GetModifiedValue(this.m_reactDamagePerCrystal);
		}
		else
		{
			result = this.m_reactDamagePerCrystal;
		}
		return result;
	}

	public StandardEffectInfo GetReactEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedReactEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetReactEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedReactEnemyHitEffect;
		}
		else
		{
			result = this.m_reactEnemyHitEffect;
		}
		return result;
	}

	public int GetReactHealOnTarget()
	{
		return (!this.m_abilityMod) ? this.m_reactHealOnTarget : this.m_abilityMod.m_reactHealOnTargetMod.GetModifiedValue(this.m_reactHealOnTarget);
	}

	public int GetReactEnergyOnCasterPerReact()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetReactEnergyOnCasterPerReact()).MethodHandle;
			}
			result = this.m_abilityMod.m_reactEnergyOnCasterPerReactMod.GetModifiedValue(this.m_reactEnergyOnCasterPerReact);
		}
		else
		{
			result = this.m_reactEnergyOnCasterPerReact;
		}
		return result;
	}

	public int GetCdrIfNoReactionTriggered()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetCdrIfNoReactionTriggered()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrIfNoReactionTriggeredMod.GetModifiedValue(this.m_cdrIfNoReactionTriggered);
		}
		else
		{
			result = this.m_cdrIfNoReactionTriggered;
		}
		return result;
	}

	public float GetRadiusForTargeter(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return this.GetCurrentRadius();
	}

	public float GetCurrentRadius()
	{
		float num = this.GetReactBaseRadius();
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetCurrentRadius()).MethodHandle;
			}
			if (this.GetReactRadiusPerCrystal() > 0f)
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
				int num2 = Mathf.Max(0, this.m_syncComp.DamageCrystals);
				num += this.GetReactRadiusPerCrystal() * (float)num2;
			}
		}
		return num;
	}

	public int GetTotalDamage()
	{
		int num = this.GetReactAoeDamage();
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetTotalDamage()).MethodHandle;
			}
			if (this.GetReactDamagePerCrystal() > 0)
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
				int num2 = Mathf.Max(0, this.m_syncComp.DamageCrystals);
				num += this.GetReactDamagePerCrystal() * num2;
			}
		}
		return num;
	}

	public int GetCurrentExtraAbsorb(ActorData caster)
	{
		int num = 0;
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetCurrentExtraAbsorb(ActorData)).MethodHandle;
			}
			if (this.GetExtraAbsorbPerCrystal() > 0)
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
				int num2 = this.m_syncComp.SpentDamageCrystals(caster);
				num += num2 * this.GetExtraAbsorbPerCrystal();
			}
		}
		return num;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, this.CanTargetEnemy(), this.CanTargetAlly(), this.CanTargetSelf(), Ability.ValidateCheckPath.Ignore, !this.TargetingIgnoreLos(), true, false);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return base.HasTargetableActorsInDecision(caster, this.CanTargetEnemy(), this.CanTargetAlly(), this.CanTargetSelf(), Ability.ValidateCheckPath.Ignore, !this.TargetingIgnoreLos(), true, false);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Ally, 1);
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, 1);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		results.m_absorb = 0;
		results.m_damage = 0;
		results.m_healing = 0;
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) <= 0)
		{
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
				}
			}
			else
			{
				if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
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
					results.m_damage = this.GetTotalDamage();
					return true;
				}
				return true;
			}
		}
		ActorData actorData = base.ActorData;
		int absorb = this.GetAllyBaseEffectData().m_absorbAmount + this.GetCurrentExtraAbsorb(actorData);
		results.m_absorb = absorb;
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_MartyrAoeOnReactHit abilityMod_MartyrAoeOnReactHit = modAsBase as AbilityMod_MartyrAoeOnReactHit;
		StandardActorEffectData standardActorEffectData;
		if (abilityMod_MartyrAoeOnReactHit)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			standardActorEffectData = abilityMod_MartyrAoeOnReactHit.m_enemyBaseEffectDataMod.GetModifiedValue(this.m_enemyBaseEffectData);
		}
		else
		{
			standardActorEffectData = this.m_enemyBaseEffectData;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "EnemyBaseEffectData", abilityMod_MartyrAoeOnReactHit != null, this.m_enemyBaseEffectData);
		StandardActorEffectData standardActorEffectData3;
		if (abilityMod_MartyrAoeOnReactHit)
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
			standardActorEffectData3 = abilityMod_MartyrAoeOnReactHit.m_allyBaseEffectDataMod.GetModifiedValue(this.m_allyBaseEffectData);
		}
		else
		{
			standardActorEffectData3 = this.m_allyBaseEffectData;
		}
		StandardActorEffectData standardActorEffectData4 = standardActorEffectData3;
		standardActorEffectData4.AddTooltipTokens(tokens, "AllyBaseEffectData", abilityMod_MartyrAoeOnReactHit != null, this.m_allyBaseEffectData);
		string name = "ExtraAbsorbPerCrystal";
		string empty = string.Empty;
		int val;
		if (abilityMod_MartyrAoeOnReactHit)
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
			val = abilityMod_MartyrAoeOnReactHit.m_extraAbsorbPerCrystalMod.GetModifiedValue(this.m_extraAbsorbPerCrystal);
		}
		else
		{
			val = this.m_extraAbsorbPerCrystal;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "ReactAoeDamage";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_MartyrAoeOnReactHit)
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
			val2 = abilityMod_MartyrAoeOnReactHit.m_reactAoeDamageMod.GetModifiedValue(this.m_reactAoeDamage);
		}
		else
		{
			val2 = this.m_reactAoeDamage;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "ReactDamagePerCrystal";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_MartyrAoeOnReactHit)
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
			val3 = abilityMod_MartyrAoeOnReactHit.m_reactDamagePerCrystalMod.GetModifiedValue(this.m_reactDamagePerCrystal);
		}
		else
		{
			val3 = this.m_reactDamagePerCrystal;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_MartyrAoeOnReactHit)
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
			effectInfo = abilityMod_MartyrAoeOnReactHit.m_reactEnemyHitEffectMod.GetModifiedValue(this.m_reactEnemyHitEffect);
		}
		else
		{
			effectInfo = this.m_reactEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "ReactEnemyHitEffect", this.m_reactEnemyHitEffect, true);
		string name4 = "ReactHealOnTarget";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_MartyrAoeOnReactHit)
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
			val4 = abilityMod_MartyrAoeOnReactHit.m_reactHealOnTargetMod.GetModifiedValue(this.m_reactHealOnTarget);
		}
		else
		{
			val4 = this.m_reactHealOnTarget;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		string name5 = "ReactEnergyOnCasterPerReact";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_MartyrAoeOnReactHit)
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
			val5 = abilityMod_MartyrAoeOnReactHit.m_reactEnergyOnCasterPerReactMod.GetModifiedValue(this.m_reactEnergyOnCasterPerReact);
		}
		else
		{
			val5 = this.m_reactEnergyOnCasterPerReact;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
		string name6 = "CdrIfNoReactionTriggered";
		string empty6 = string.Empty;
		int val6;
		if (abilityMod_MartyrAoeOnReactHit)
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
			val6 = abilityMod_MartyrAoeOnReactHit.m_cdrIfNoReactionTriggeredMod.GetModifiedValue(this.m_cdrIfNoReactionTriggered);
		}
		else
		{
			val6 = this.m_cdrIfNoReactionTriggered;
		}
		base.AddTokenInt(tokens, name6, empty6, val6, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrAoeOnReactHit))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrAoeOnReactHit.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_MartyrAoeOnReactHit);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
