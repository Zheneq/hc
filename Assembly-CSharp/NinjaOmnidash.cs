using System;
using System.Collections.Generic;
using UnityEngine;

public class NinjaOmnidash : Ability
{
	[Separator("On Hit Stuff", true)]
	public int m_baseDamage = 0x3C;

	public int m_damageChangePerEnemyAfterFirst = -0xA;

	public int m_minDamage;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Effect for single hit --")]
	public StandardEffectInfo m_singleHitEnemyEffect;

	public StandardEffectInfo m_extraSingleHitEnemyEffect;

	[Separator("Energy gain on Marked hit", true)]
	public int m_energyGainPerMarkedHit;

	[Separator("For Dash", true)]
	public bool m_skipEvade;

	[Space(10f)]
	public bool m_isTeleport = true;

	public float m_dashRadiusAtStart = 3f;

	public float m_dashRadiusMiddle;

	public float m_dashRadiusAtEnd;

	public bool m_dashPenetrateLineOfSight;

	[Header("-- Whether can queue movement evade")]
	public bool m_canQueueMoveAfterEvade = true;

	[Separator("[Deathmark] Effect", "magenta")]
	public bool m_applyDeathmarkEffect = true;

	[Separator("Cooldown Reset on other ability", true)]
	public int m_cdrOnAbility;

	public AbilityData.ActionType m_cdrAbilityTarget = AbilityData.ActionType.ABILITY_2;

	[Header("-- Sequences --")]
	public GameObject m_onCastSequencePrefab;

	private TargetData[] m_noEvadeTargetData = new TargetData[0];

	private AbilityMod_NinjaOmnidash m_abilityMod;

	private Ninja_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedSingleHitEnemyEffect;

	private StandardEffectInfo m_cachedExtraSingleHitEnemyEffect;

	public AbilityMod_NinjaOmnidash GetMod()
	{
		return this.m_abilityMod;
	}

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Omnidash";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		if (this.m_syncComp == null)
		{
			this.m_syncComp = base.GetComponent<Ninja_SyncComponent>();
		}
		if (this.SkipEvade())
		{
			base.Targeter = new AbilityUtil_Targeter_AoE_AroundActor(this, this.GetDashRadiusAtStart(), this.DashPenetrateLineOfSight(), true, false, -1, false, false, true);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_ChargeAoE(this, this.GetDashRadiusAtStart(), this.GetDashRadiusAtEnd(), this.GetDashRadiusMiddle(), -1, false, this.DashPenetrateLineOfSight())
			{
				ShowTeleportLines = this.DashIsTeleport(),
				AllowChargeThroughInvalidSquares = this.DashIsTeleport(),
				m_shouldAddCasterDelegate = new AbilityUtil_Targeter_ChargeAoE.ShouldAddCasterDelegate(this.ShouldAddCasterForTargeter)
			};
		}
	}

	private bool ShouldAddCasterForTargeter(ActorData caster, List<ActorData> addedSoFar)
	{
		if (this.GetDeathmarkTriggerSelfHeal() > 0)
		{
			if (this.m_syncComp != null)
			{
				for (int i = 0; i < addedSoFar.Count; i++)
				{
					if (this.IsActorMarked(addedSoFar[i]))
					{
						return true;
					}
				}
				return false;
			}
		}
		return false;
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\nIf using [Skip Evade] option, please set phase to one that is not Evasion.\nPlease edit [Deathmark] info on Ninja sync component.";
	}

	internal override ActorData.MovementType GetMovementType()
	{
		if (this.SkipEvade())
		{
			return ActorData.MovementType.None;
		}
		return (!this.DashIsTeleport()) ? ActorData.MovementType.Charge : ActorData.MovementType.Teleport;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		bool result;
		if (this.CanQueueMoveAfterEvade())
		{
			result = !this.SkipEvade();
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override TargetData[] GetTargetData()
	{
		if (this.SkipEvade())
		{
			return this.m_noEvadeTargetData;
		}
		return base.GetTargetData();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "BaseDamage", string.Empty, this.m_baseDamage, false);
		base.AddTokenInt(tokens, "DamageChangePerEnemyAfterFirst", string.Empty, Mathf.Abs(this.m_damageChangePerEnemyAfterFirst), false);
		base.AddTokenInt(tokens, "MinDamage", string.Empty, this.m_minDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_singleHitEnemyEffect, "SingleHitEnemyEffect", this.m_singleHitEnemyEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_extraSingleHitEnemyEffect, "ExtraSingleHitEnemyEffect", this.m_extraSingleHitEnemyEffect, true);
		base.AddTokenInt(tokens, "EnergyGainPerMarkedHit", string.Empty, this.m_energyGainPerMarkedHit, false);
		base.AddTokenInt(tokens, "CdrOnAbility", string.Empty, this.m_cdrOnAbility, false);
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
		this.m_cachedSingleHitEnemyEffect = ((!this.m_abilityMod) ? this.m_singleHitEnemyEffect : this.m_abilityMod.m_singleHitEnemyEffectMod.GetModifiedValue(this.m_singleHitEnemyEffect));
		StandardEffectInfo cachedExtraSingleHitEnemyEffect;
		if (this.m_abilityMod)
		{
			cachedExtraSingleHitEnemyEffect = this.m_abilityMod.m_extraSingleHitEnemyEffectMod.GetModifiedValue(this.m_extraSingleHitEnemyEffect);
		}
		else
		{
			cachedExtraSingleHitEnemyEffect = this.m_extraSingleHitEnemyEffect;
		}
		this.m_cachedExtraSingleHitEnemyEffect = cachedExtraSingleHitEnemyEffect;
	}

	public int GetBaseDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_baseDamageMod.GetModifiedValue(this.m_baseDamage);
		}
		else
		{
			result = this.m_baseDamage;
		}
		return result;
	}

	public int GetDamageChangePerEnemyAfterFirst()
	{
		return (!this.m_abilityMod) ? this.m_damageChangePerEnemyAfterFirst : this.m_abilityMod.m_damageChangePerEnemyAfterFirstMod.GetModifiedValue(this.m_damageChangePerEnemyAfterFirst);
	}

	public int GetMinDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_minDamageMod.GetModifiedValue(this.m_minDamage);
		}
		else
		{
			result = this.m_minDamage;
		}
		return result;
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

	public StandardEffectInfo GetSingleHitEnemyEffect()
	{
		return (this.m_cachedSingleHitEnemyEffect == null) ? this.m_singleHitEnemyEffect : this.m_cachedSingleHitEnemyEffect;
	}

	public StandardEffectInfo GetExtraSingleHitEnemyEffect()
	{
		return (this.m_cachedExtraSingleHitEnemyEffect == null) ? this.m_extraSingleHitEnemyEffect : this.m_cachedExtraSingleHitEnemyEffect;
	}

	public int GetEnergyGainPerMarkedHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_energyGainPerMarkedHitMod.GetModifiedValue(this.m_energyGainPerMarkedHit);
		}
		else
		{
			result = this.m_energyGainPerMarkedHit;
		}
		return result;
	}

	public bool SkipEvade()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_skipEvadeMod.GetModifiedValue(this.m_skipEvade);
		}
		else
		{
			result = this.m_skipEvade;
		}
		return result;
	}

	public bool DashIsTeleport()
	{
		return (!this.m_abilityMod) ? this.m_isTeleport : this.m_abilityMod.m_isTeleportMod.GetModifiedValue(this.m_isTeleport);
	}

	public float GetDashRadiusAtStart()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_dashRadiusAtStartMod.GetModifiedValue(this.m_dashRadiusAtStart);
		}
		else
		{
			result = this.m_dashRadiusAtStart;
		}
		return result;
	}

	public float GetDashRadiusMiddle()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_dashRadiusMiddleMod.GetModifiedValue(this.m_dashRadiusMiddle);
		}
		else
		{
			result = this.m_dashRadiusMiddle;
		}
		return result;
	}

	public float GetDashRadiusAtEnd()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_dashRadiusAtEndMod.GetModifiedValue(this.m_dashRadiusAtEnd);
		}
		else
		{
			result = this.m_dashRadiusAtEnd;
		}
		return result;
	}

	public bool DashPenetrateLineOfSight()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_dashPenetrateLineOfSightMod.GetModifiedValue(this.m_dashPenetrateLineOfSight);
		}
		else
		{
			result = this.m_dashPenetrateLineOfSight;
		}
		return result;
	}

	public bool CanQueueMoveAfterEvade()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canQueueMoveAfterEvadeMod.GetModifiedValue(this.m_canQueueMoveAfterEvade);
		}
		else
		{
			result = this.m_canQueueMoveAfterEvade;
		}
		return result;
	}

	public bool ApplyDeathmarkEffect()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_applyDeathmarkEffectMod.GetModifiedValue(this.m_applyDeathmarkEffect);
		}
		else
		{
			result = this.m_applyDeathmarkEffect;
		}
		return result;
	}

	public int GetCdrOnAbility()
	{
		return (!this.m_abilityMod) ? this.m_cdrOnAbility : this.m_abilityMod.m_cdrOnAbilityMod.GetModifiedValue(this.m_cdrOnAbility);
	}

	public bool IsActorMarked(ActorData actor)
	{
		bool result;
		if (this.m_syncComp != null)
		{
			result = this.m_syncComp.ActorHasDeathmark(actor);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public int CalcDamageForNumEnemies(int numEnemies)
	{
		int b = this.GetBaseDamage() + Mathf.Max(0, numEnemies - 1) * this.GetDamageChangePerEnemyAfterFirst();
		b = Mathf.Max(this.GetMinDamage(), b);
		return Mathf.Max(0, b);
	}

	public int GetDeathmarkTriggerDamage()
	{
		int result = 0;
		if (this.m_syncComp != null)
		{
			result = ((!(this.m_abilityMod != null)) ? this.m_syncComp.m_deathmarkOnTriggerDamage : this.m_abilityMod.m_deathmarkDamageMod.GetModifiedValue(this.m_syncComp.m_deathmarkOnTriggerDamage));
		}
		return result;
	}

	public int GetDeathmarkTriggerSelfHeal()
	{
		int result = 0;
		if (this.m_syncComp != null)
		{
			result = ((!(this.m_abilityMod != null)) ? this.m_syncComp.m_deathmarkOnTriggerCasterHeal : this.m_abilityMod.m_deathmarkCasterHealMod.GetModifiedValue(this.m_syncComp.m_deathmarkOnTriggerCasterHeal));
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetBaseDamage());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetDeathmarkTriggerSelfHeal());
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			int damage = this.CalcDamageForNumEnemies(visibleActorsCountByTooltipSubject);
			results.m_damage = damage;
		}
		else if (targetActor == base.ActorData && this.GetDeathmarkTriggerSelfHeal() > 0)
		{
			int num = 0;
			if (this.m_syncComp != null)
			{
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
				for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
				{
					if (this.m_syncComp.ActorHasDeathmark(visibleActorsInRangeByTooltipSubject[i]))
					{
						num += this.GetDeathmarkTriggerSelfHeal();
					}
				}
			}
			results.m_healing = num;
		}
		return true;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		if (this.GetEnergyGainPerMarkedHit() > 0)
		{
			if (this.m_syncComp != null)
			{
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
				for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
				{
					if (this.IsActorMarked(visibleActorsInRangeByTooltipSubject[i]))
					{
						num += this.GetEnergyGainPerMarkedHit();
					}
				}
			}
		}
		return num;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
		{
			if (this.m_syncComp != null)
			{
				if (this.GetDeathmarkTriggerDamage() > 0)
				{
					if (this.IsActorMarked(targetActor))
					{
						return "\n+ " + AbilityUtils.CalculateDamageForTargeter(base.ActorData, targetActor, this, this.GetDeathmarkTriggerDamage(), false).ToString();
					}
				}
			}
		}
		return null;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool result = false;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (boardSquareSafe != caster.GetCurrentBoardSquare())
				{
					if (this.DashIsTeleport())
					{
						result = true;
					}
					else
					{
						result = (KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe) != null);
					}
				}
			}
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NinjaOmnidash))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_NinjaOmnidash);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
