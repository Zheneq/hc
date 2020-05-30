using System.Collections.Generic;
using UnityEngine;

public class NinjaOmnidash : Ability
{
	[Separator("On Hit Stuff", true)]
	public int m_baseDamage = 60;

	public int m_damageChangePerEnemyAfterFirst = -10;

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
			base.Targeter = new AbilityUtil_Targeter_AoE_AroundActor(this, GetDashRadiusAtStart(), DashPenetrateLineOfSight(), true, false, -1, false, false);
			return;
		}
		AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, GetDashRadiusAtStart(), GetDashRadiusAtEnd(), GetDashRadiusMiddle(), -1, false, DashPenetrateLineOfSight());
		abilityUtil_Targeter_ChargeAoE.ShowTeleportLines = DashIsTeleport();
		abilityUtil_Targeter_ChargeAoE.AllowChargeThroughInvalidSquares = DashIsTeleport();
		abilityUtil_Targeter_ChargeAoE.m_shouldAddCasterDelegate = ShouldAddCasterForTargeter;
		base.Targeter = abilityUtil_Targeter_ChargeAoE;
	}

	private bool ShouldAddCasterForTargeter(ActorData caster, List<ActorData> addedSoFar)
	{
		if (GetDeathmarkTriggerSelfHeal() > 0)
		{
			if (m_syncComp != null)
			{
				for (int i = 0; i < addedSoFar.Count; i++)
				{
					if (!IsActorMarked(addedSoFar[i]))
					{
						continue;
					}
					while (true)
					{
						return true;
					}
				}
				while (true)
				{
					return false;
				}
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
		if (SkipEvade())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return ActorData.MovementType.None;
				}
			}
		}
		return DashIsTeleport() ? ActorData.MovementType.Teleport : ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		int result;
		if (CanQueueMoveAfterEvade())
		{
			result = ((!SkipEvade()) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override TargetData[] GetTargetData()
	{
		if (SkipEvade())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_noEvadeTargetData;
				}
			}
		}
		return base.GetTargetData();
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
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		m_cachedSingleHitEnemyEffect = ((!m_abilityMod) ? m_singleHitEnemyEffect : m_abilityMod.m_singleHitEnemyEffectMod.GetModifiedValue(m_singleHitEnemyEffect));
		StandardEffectInfo cachedExtraSingleHitEnemyEffect;
		if ((bool)m_abilityMod)
		{
			cachedExtraSingleHitEnemyEffect = m_abilityMod.m_extraSingleHitEnemyEffectMod.GetModifiedValue(m_extraSingleHitEnemyEffect);
		}
		else
		{
			cachedExtraSingleHitEnemyEffect = m_extraSingleHitEnemyEffect;
		}
		m_cachedExtraSingleHitEnemyEffect = cachedExtraSingleHitEnemyEffect;
	}

	public int GetBaseDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_baseDamageMod.GetModifiedValue(m_baseDamage);
		}
		else
		{
			result = m_baseDamage;
		}
		return result;
	}

	public int GetDamageChangePerEnemyAfterFirst()
	{
		return (!m_abilityMod) ? m_damageChangePerEnemyAfterFirst : m_abilityMod.m_damageChangePerEnemyAfterFirstMod.GetModifiedValue(m_damageChangePerEnemyAfterFirst);
	}

	public int GetMinDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_minDamageMod.GetModifiedValue(m_minDamage);
		}
		else
		{
			result = m_minDamage;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
		{
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetSingleHitEnemyEffect()
	{
		return (m_cachedSingleHitEnemyEffect == null) ? m_singleHitEnemyEffect : m_cachedSingleHitEnemyEffect;
	}

	public StandardEffectInfo GetExtraSingleHitEnemyEffect()
	{
		return (m_cachedExtraSingleHitEnemyEffect == null) ? m_extraSingleHitEnemyEffect : m_cachedExtraSingleHitEnemyEffect;
	}

	public int GetEnergyGainPerMarkedHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyGainPerMarkedHitMod.GetModifiedValue(m_energyGainPerMarkedHit);
		}
		else
		{
			result = m_energyGainPerMarkedHit;
		}
		return result;
	}

	public bool SkipEvade()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_skipEvadeMod.GetModifiedValue(m_skipEvade);
		}
		else
		{
			result = m_skipEvade;
		}
		return result;
	}

	public bool DashIsTeleport()
	{
		return (!m_abilityMod) ? m_isTeleport : m_abilityMod.m_isTeleportMod.GetModifiedValue(m_isTeleport);
	}

	public float GetDashRadiusAtStart()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashRadiusAtStartMod.GetModifiedValue(m_dashRadiusAtStart);
		}
		else
		{
			result = m_dashRadiusAtStart;
		}
		return result;
	}

	public float GetDashRadiusMiddle()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashRadiusMiddleMod.GetModifiedValue(m_dashRadiusMiddle);
		}
		else
		{
			result = m_dashRadiusMiddle;
		}
		return result;
	}

	public float GetDashRadiusAtEnd()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashRadiusAtEndMod.GetModifiedValue(m_dashRadiusAtEnd);
		}
		else
		{
			result = m_dashRadiusAtEnd;
		}
		return result;
	}

	public bool DashPenetrateLineOfSight()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_dashPenetrateLineOfSightMod.GetModifiedValue(m_dashPenetrateLineOfSight);
		}
		else
		{
			result = m_dashPenetrateLineOfSight;
		}
		return result;
	}

	public bool CanQueueMoveAfterEvade()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canQueueMoveAfterEvadeMod.GetModifiedValue(m_canQueueMoveAfterEvade);
		}
		else
		{
			result = m_canQueueMoveAfterEvade;
		}
		return result;
	}

	public bool ApplyDeathmarkEffect()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_applyDeathmarkEffectMod.GetModifiedValue(m_applyDeathmarkEffect);
		}
		else
		{
			result = m_applyDeathmarkEffect;
		}
		return result;
	}

	public int GetCdrOnAbility()
	{
		return (!m_abilityMod) ? m_cdrOnAbility : m_abilityMod.m_cdrOnAbilityMod.GetModifiedValue(m_cdrOnAbility);
	}

	public bool IsActorMarked(ActorData actor)
	{
		int result;
		if (m_syncComp != null)
		{
			result = (m_syncComp.ActorHasDeathmark(actor) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public int CalcDamageForNumEnemies(int numEnemies)
	{
		int b = GetBaseDamage() + Mathf.Max(0, numEnemies - 1) * GetDamageChangePerEnemyAfterFirst();
		b = Mathf.Max(GetMinDamage(), b);
		return Mathf.Max(0, b);
	}

	public int GetDeathmarkTriggerDamage()
	{
		int result = 0;
		if (m_syncComp != null)
		{
			result = ((!(m_abilityMod != null)) ? m_syncComp.m_deathmarkOnTriggerDamage : m_abilityMod.m_deathmarkDamageMod.GetModifiedValue(m_syncComp.m_deathmarkOnTriggerDamage));
		}
		return result;
	}

	public int GetDeathmarkTriggerSelfHeal()
	{
		int result = 0;
		if (m_syncComp != null)
		{
			result = ((!(m_abilityMod != null)) ? m_syncComp.m_deathmarkOnTriggerCasterHeal : m_abilityMod.m_deathmarkCasterHealMod.GetModifiedValue(m_syncComp.m_deathmarkOnTriggerCasterHeal));
		}
		return result;
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
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			int num = results.m_damage = CalcDamageForNumEnemies(visibleActorsCountByTooltipSubject);
		}
		else if (targetActor == base.ActorData && GetDeathmarkTriggerSelfHeal() > 0)
		{
			int num2 = 0;
			if (m_syncComp != null)
			{
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
				for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
				{
					if (m_syncComp.ActorHasDeathmark(visibleActorsInRangeByTooltipSubject[i]))
					{
						num2 += GetDeathmarkTriggerSelfHeal();
					}
				}
			}
			results.m_healing = num2;
		}
		return true;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		if (GetEnergyGainPerMarkedHit() > 0)
		{
			if (m_syncComp != null)
			{
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
				for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
				{
					if (IsActorMarked(visibleActorsInRangeByTooltipSubject[i]))
					{
						num += GetEnergyGainPerMarkedHit();
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
			if (m_syncComp != null)
			{
				if (GetDeathmarkTriggerDamage() > 0)
				{
					if (IsActorMarked(targetActor))
					{
						return "\n+ " + AbilityUtils.CalculateDamageForTargeter(base.ActorData, targetActor, this, GetDeathmarkTriggerDamage(), false);
					}
				}
			}
		}
		return null;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool result = false;
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (boardSquareSafe != caster.GetCurrentBoardSquare())
				{
					if (DashIsTeleport())
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
		if (abilityMod.GetType() != typeof(AbilityMod_NinjaOmnidash))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_NinjaOmnidash);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
