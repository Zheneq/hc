using System.Collections.Generic;
using UnityEngine;

public class NinjaOmnidash : Ability
{
	[Separator("On Hit Stuff")]
	public int m_baseDamage = 60;
	public int m_damageChangePerEnemyAfterFirst = -10;
	public int m_minDamage;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Effect for single hit --")]
	public StandardEffectInfo m_singleHitEnemyEffect;
	public StandardEffectInfo m_extraSingleHitEnemyEffect;
	[Separator("Energy gain on Marked hit")]
	public int m_energyGainPerMarkedHit;
	[Separator("For Dash")]
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
	[Separator("Cooldown Reset on other ability")]
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
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Omnidash";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Ninja_SyncComponent>();
		}
		if (SkipEvade())
		{
			Targeter = new AbilityUtil_Targeter_AoE_AroundActor(
				this,
				GetDashRadiusAtStart(),
				DashPenetrateLineOfSight(),
				true,
				false, 
				-1,
				false,
				false);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_ChargeAoE(
				this,
				GetDashRadiusAtStart(),
				GetDashRadiusAtEnd(),
				GetDashRadiusMiddle(),
				-1,
				false,
				DashPenetrateLineOfSight())
			{
				ShowTeleportLines = DashIsTeleport(),
				AllowChargeThroughInvalidSquares = DashIsTeleport(),
				m_shouldAddCasterDelegate = ShouldAddCasterForTargeter
			};
		}
	}

	private bool ShouldAddCasterForTargeter(ActorData caster, List<ActorData> addedSoFar)
	{
		if (GetDeathmarkTriggerSelfHeal() <= 0 || m_syncComp == null)
		{
			return false;
		}
		foreach (ActorData actor in addedSoFar)
		{
			if (IsActorMarked(actor))
			{
				return true;
			}
		}
		return false;
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\n" +
		       "If using [Skip Evade] option, please set phase to one that is not Evasion.\n" +
		       "Please edit [Deathmark] info on Ninja sync component.";
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return SkipEvade()
			? ActorData.MovementType.None
			: DashIsTeleport()
				? ActorData.MovementType.Teleport
				: ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return CanQueueMoveAfterEvade() && !SkipEvade();
	}

	public override TargetData[] GetTargetData()
	{
		return SkipEvade()
			? m_noEvadeTargetData
			: base.GetTargetData();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "BaseDamage", string.Empty, m_baseDamage);
		AddTokenInt(tokens, "DamageChangePerEnemyAfterFirst", string.Empty, Mathf.Abs(m_damageChangePerEnemyAfterFirst));
		AddTokenInt(tokens, "MinDamage", string.Empty, m_minDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_singleHitEnemyEffect, "SingleHitEnemyEffect", m_singleHitEnemyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_extraSingleHitEnemyEffect, "ExtraSingleHitEnemyEffect", m_extraSingleHitEnemyEffect);
		AddTokenInt(tokens, "EnergyGainPerMarkedHit", string.Empty, m_energyGainPerMarkedHit);
		AddTokenInt(tokens, "CdrOnAbility", string.Empty, m_cdrOnAbility);
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedSingleHitEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_singleHitEnemyEffectMod.GetModifiedValue(m_singleHitEnemyEffect)
			: m_singleHitEnemyEffect;
		m_cachedExtraSingleHitEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_extraSingleHitEnemyEffectMod.GetModifiedValue(m_extraSingleHitEnemyEffect)
			: m_extraSingleHitEnemyEffect;
	}

	public int GetBaseDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_baseDamageMod.GetModifiedValue(m_baseDamage)
			: m_baseDamage;
	}

	public int GetDamageChangePerEnemyAfterFirst()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageChangePerEnemyAfterFirstMod.GetModifiedValue(m_damageChangePerEnemyAfterFirst)
			: m_damageChangePerEnemyAfterFirst;
	}

	public int GetMinDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minDamageMod.GetModifiedValue(m_minDamage)
			: m_minDamage;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public StandardEffectInfo GetSingleHitEnemyEffect()
	{
		return m_cachedSingleHitEnemyEffect ?? m_singleHitEnemyEffect;
	}

	public StandardEffectInfo GetExtraSingleHitEnemyEffect()
	{
		return m_cachedExtraSingleHitEnemyEffect ?? m_extraSingleHitEnemyEffect;
	}

	public int GetEnergyGainPerMarkedHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyGainPerMarkedHitMod.GetModifiedValue(m_energyGainPerMarkedHit)
			: m_energyGainPerMarkedHit;
	}

	public bool SkipEvade()
	{
		return m_abilityMod != null
			? m_abilityMod.m_skipEvadeMod.GetModifiedValue(m_skipEvade)
			: m_skipEvade;
	}

	public bool DashIsTeleport()
	{
		return m_abilityMod != null
			? m_abilityMod.m_isTeleportMod.GetModifiedValue(m_isTeleport)
			: m_isTeleport;
	}

	public float GetDashRadiusAtStart()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashRadiusAtStartMod.GetModifiedValue(m_dashRadiusAtStart)
			: m_dashRadiusAtStart;
	}

	public float GetDashRadiusMiddle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashRadiusMiddleMod.GetModifiedValue(m_dashRadiusMiddle)
			: m_dashRadiusMiddle;
	}

	public float GetDashRadiusAtEnd()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashRadiusAtEndMod.GetModifiedValue(m_dashRadiusAtEnd)
			: m_dashRadiusAtEnd;
	}

	public bool DashPenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashPenetrateLineOfSightMod.GetModifiedValue(m_dashPenetrateLineOfSight)
			: m_dashPenetrateLineOfSight;
	}

	public bool CanQueueMoveAfterEvade()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canQueueMoveAfterEvadeMod.GetModifiedValue(m_canQueueMoveAfterEvade)
			: m_canQueueMoveAfterEvade;
	}

	public bool ApplyDeathmarkEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_applyDeathmarkEffectMod.GetModifiedValue(m_applyDeathmarkEffect)
			: m_applyDeathmarkEffect;
	}

	public int GetCdrOnAbility()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnAbilityMod.GetModifiedValue(m_cdrOnAbility)
			: m_cdrOnAbility;
	}

	public bool IsActorMarked(ActorData actor)
	{
		return m_syncComp != null && m_syncComp.ActorHasDeathmark(actor);
	}

	public int CalcDamageForNumEnemies(int numEnemies)
	{
		int damage = GetBaseDamage() + Mathf.Max(0, numEnemies - 1) * GetDamageChangePerEnemyAfterFirst();
		damage = Mathf.Max(GetMinDamage(), damage);
		return Mathf.Max(0, damage);
	}

	public int GetDeathmarkTriggerDamage()
	{
		return m_syncComp != null
			? m_abilityMod != null
				? m_abilityMod.m_deathmarkDamageMod.GetModifiedValue(m_syncComp.m_deathmarkOnTriggerDamage)
				: m_syncComp.m_deathmarkOnTriggerDamage
			: 0;
	}

	public int GetDeathmarkTriggerSelfHeal()
	{
		return m_syncComp != null
			? m_abilityMod != null
				? m_abilityMod.m_deathmarkCasterHealMod.GetModifiedValue(m_syncComp.m_deathmarkOnTriggerCasterHeal)
				: m_syncComp.m_deathmarkOnTriggerCasterHeal
			: 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetBaseDamage());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetDeathmarkTriggerSelfHeal());
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			int targets = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			results.m_damage = CalcDamageForNumEnemies(targets);
		}
		else if (targetActor == ActorData && GetDeathmarkTriggerSelfHeal() > 0)
		{
			int healing = 0;
			if (m_syncComp != null)
			{
				foreach (ActorData actor in Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy))
				{
					if (m_syncComp.ActorHasDeathmark(actor))
					{
						healing += GetDeathmarkTriggerSelfHeal();
					}
				}
			}
			results.m_healing = healing;
		}
		return true;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int techPoints = 0;
		if (GetEnergyGainPerMarkedHit() > 0 && m_syncComp != null)
		{
			foreach (ActorData actor in Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy))
			{
				if (IsActorMarked(actor))
				{
					techPoints += GetEnergyGainPerMarkedHit();
				}
			}
		}
		return techPoints;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return symbolType == AbilityTooltipSymbol.Damage
		       && m_syncComp != null
		       && GetDeathmarkTriggerDamage() > 0
		       && IsActorMarked(targetActor)
			? "\n+ " + AbilityUtils.CalculateDamageForTargeter(
				ActorData, targetActor, this, GetDeathmarkTriggerDamage(), false)
			: null;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return targetSquare != null
		       && targetSquare.IsValidForGameplay()
		       && targetSquare != caster.GetCurrentBoardSquare()
		       && (DashIsTeleport() || KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare) != null);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NinjaOmnidash))
		{
			m_abilityMod = abilityMod as AbilityMod_NinjaOmnidash;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
