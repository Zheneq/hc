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
	public int m_reactAoeDamage = 10;

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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "MartyrAoeOnReactHit";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Martyr_SyncComponent>();
		}
		SetCachedFields();
		AbilityUtil_Targeter_AoE_AroundActor abilityUtil_Targeter_AoE_AroundActor = new AbilityUtil_Targeter_AoE_AroundActor(this, 1f, ReactPenetrateLos(), true, false, -1, CanTargetEnemy(), CanTargetAlly(), CanTargetSelf());
		abilityUtil_Targeter_AoE_AroundActor.m_customRadiusDelegate = GetRadiusForTargeter;
		base.Targeter = abilityUtil_Targeter_AoE_AroundActor;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedEnemyBaseEffectData;
		if ((bool)m_abilityMod)
		{
			cachedEnemyBaseEffectData = m_abilityMod.m_enemyBaseEffectDataMod.GetModifiedValue(m_enemyBaseEffectData);
		}
		else
		{
			cachedEnemyBaseEffectData = m_enemyBaseEffectData;
		}
		m_cachedEnemyBaseEffectData = cachedEnemyBaseEffectData;
		StandardActorEffectData cachedAllyBaseEffectData;
		if ((bool)m_abilityMod)
		{
			cachedAllyBaseEffectData = m_abilityMod.m_allyBaseEffectDataMod.GetModifiedValue(m_allyBaseEffectData);
		}
		else
		{
			cachedAllyBaseEffectData = m_allyBaseEffectData;
		}
		m_cachedAllyBaseEffectData = cachedAllyBaseEffectData;
		StandardEffectInfo cachedReactEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedReactEnemyHitEffect = m_abilityMod.m_reactEnemyHitEffectMod.GetModifiedValue(m_reactEnemyHitEffect);
		}
		else
		{
			cachedReactEnemyHitEffect = m_reactEnemyHitEffect;
		}
		m_cachedReactEnemyHitEffect = cachedReactEnemyHitEffect;
	}

	public bool CanTargetEnemy()
	{
		return (!m_abilityMod) ? m_canTargetEnemy : m_abilityMod.m_canTargetEnemyMod.GetModifiedValue(m_canTargetEnemy);
	}

	public bool CanTargetAlly()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canTargetAllyMod.GetModifiedValue(m_canTargetAlly);
		}
		else
		{
			result = m_canTargetAlly;
		}
		return result;
	}

	public bool CanTargetSelf()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canTargetSelfMod.GetModifiedValue(m_canTargetSelf);
		}
		else
		{
			result = m_canTargetSelf;
		}
		return result;
	}

	public bool TargetingIgnoreLos()
	{
		return (!m_abilityMod) ? m_targetingIgnoreLos : m_abilityMod.m_targetingIgnoreLosMod.GetModifiedValue(m_targetingIgnoreLos);
	}

	public StandardActorEffectData GetEnemyBaseEffectData()
	{
		StandardActorEffectData result;
		if (m_cachedEnemyBaseEffectData != null)
		{
			result = m_cachedEnemyBaseEffectData;
		}
		else
		{
			result = m_enemyBaseEffectData;
		}
		return result;
	}

	public StandardActorEffectData GetAllyBaseEffectData()
	{
		StandardActorEffectData result;
		if (m_cachedAllyBaseEffectData != null)
		{
			result = m_cachedAllyBaseEffectData;
		}
		else
		{
			result = m_allyBaseEffectData;
		}
		return result;
	}

	public int GetExtraAbsorbPerCrystal()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraAbsorbPerCrystalMod.GetModifiedValue(m_extraAbsorbPerCrystal);
		}
		else
		{
			result = m_extraAbsorbPerCrystal;
		}
		return result;
	}

	public float GetReactBaseRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_reactBaseRadiusMod.GetModifiedValue(m_reactBaseRadius);
		}
		else
		{
			result = m_reactBaseRadius;
		}
		return result;
	}

	public float GetReactRadiusPerCrystal()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_reactRadiusPerCrystalMod.GetModifiedValue(m_reactRadiusPerCrystal);
		}
		else
		{
			result = m_reactRadiusPerCrystal;
		}
		return result;
	}

	public bool ReactOnlyOncePerTurn()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_reactOnlyOncePerTurnMod.GetModifiedValue(m_reactOnlyOncePerTurn);
		}
		else
		{
			result = m_reactOnlyOncePerTurn;
		}
		return result;
	}

	public bool ReactPenetrateLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_reactPenetrateLosMod.GetModifiedValue(m_reactPenetrateLos);
		}
		else
		{
			result = m_reactPenetrateLos;
		}
		return result;
	}

	public bool ReactIncludeEffectTarget()
	{
		return (!m_abilityMod) ? m_reactIncludeEffectTarget : m_abilityMod.m_reactIncludeEffectTargetMod.GetModifiedValue(m_reactIncludeEffectTarget);
	}

	public int GetReactAoeDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_reactAoeDamageMod.GetModifiedValue(m_reactAoeDamage);
		}
		else
		{
			result = m_reactAoeDamage;
		}
		return result;
	}

	public int GetReactDamagePerCrystal()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_reactDamagePerCrystalMod.GetModifiedValue(m_reactDamagePerCrystal);
		}
		else
		{
			result = m_reactDamagePerCrystal;
		}
		return result;
	}

	public StandardEffectInfo GetReactEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedReactEnemyHitEffect != null)
		{
			result = m_cachedReactEnemyHitEffect;
		}
		else
		{
			result = m_reactEnemyHitEffect;
		}
		return result;
	}

	public int GetReactHealOnTarget()
	{
		return (!m_abilityMod) ? m_reactHealOnTarget : m_abilityMod.m_reactHealOnTargetMod.GetModifiedValue(m_reactHealOnTarget);
	}

	public int GetReactEnergyOnCasterPerReact()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_reactEnergyOnCasterPerReactMod.GetModifiedValue(m_reactEnergyOnCasterPerReact);
		}
		else
		{
			result = m_reactEnergyOnCasterPerReact;
		}
		return result;
	}

	public int GetCdrIfNoReactionTriggered()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrIfNoReactionTriggeredMod.GetModifiedValue(m_cdrIfNoReactionTriggered);
		}
		else
		{
			result = m_cdrIfNoReactionTriggered;
		}
		return result;
	}

	public float GetRadiusForTargeter(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return GetCurrentRadius();
	}

	public float GetCurrentRadius()
	{
		float num = GetReactBaseRadius();
		if (m_syncComp != null)
		{
			if (GetReactRadiusPerCrystal() > 0f)
			{
				int num2 = Mathf.Max(0, m_syncComp.DamageCrystals);
				num += GetReactRadiusPerCrystal() * (float)num2;
			}
		}
		return num;
	}

	public int GetTotalDamage()
	{
		int num = GetReactAoeDamage();
		if (m_syncComp != null)
		{
			if (GetReactDamagePerCrystal() > 0)
			{
				int num2 = Mathf.Max(0, m_syncComp.DamageCrystals);
				num += GetReactDamagePerCrystal() * num2;
			}
		}
		return num;
	}

	public int GetCurrentExtraAbsorb(ActorData caster)
	{
		int num = 0;
		if (m_syncComp != null)
		{
			if (GetExtraAbsorbPerCrystal() > 0)
			{
				int num2 = m_syncComp.SpentDamageCrystals(caster);
				num += num2 * GetExtraAbsorbPerCrystal();
			}
		}
		return num;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, CanTargetEnemy(), CanTargetAlly(), CanTargetSelf(), ValidateCheckPath.Ignore, !TargetingIgnoreLos(), true);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(caster, CanTargetEnemy(), CanTargetAlly(), CanTargetSelf(), ValidateCheckPath.Ignore, !TargetingIgnoreLos(), true);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Ally, 1);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, 1);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		results.m_absorb = 0;
		results.m_damage = 0;
		results.m_healing = 0;
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) <= 0)
		{
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) <= 0)
			{
				if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
				{
					results.m_damage = GetTotalDamage();
				}
				goto IL_00a0;
			}
		}
		ActorData actorData = base.ActorData;
		int num = results.m_absorb = GetAllyBaseEffectData().m_absorbAmount + GetCurrentExtraAbsorb(actorData);
		goto IL_00a0;
		IL_00a0:
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_MartyrAoeOnReactHit abilityMod_MartyrAoeOnReactHit = modAsBase as AbilityMod_MartyrAoeOnReactHit;
		StandardActorEffectData standardActorEffectData;
		if ((bool)abilityMod_MartyrAoeOnReactHit)
		{
			standardActorEffectData = abilityMod_MartyrAoeOnReactHit.m_enemyBaseEffectDataMod.GetModifiedValue(m_enemyBaseEffectData);
		}
		else
		{
			standardActorEffectData = m_enemyBaseEffectData;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "EnemyBaseEffectData", abilityMod_MartyrAoeOnReactHit != null, m_enemyBaseEffectData);
		StandardActorEffectData standardActorEffectData3;
		if ((bool)abilityMod_MartyrAoeOnReactHit)
		{
			standardActorEffectData3 = abilityMod_MartyrAoeOnReactHit.m_allyBaseEffectDataMod.GetModifiedValue(m_allyBaseEffectData);
		}
		else
		{
			standardActorEffectData3 = m_allyBaseEffectData;
		}
		StandardActorEffectData standardActorEffectData4 = standardActorEffectData3;
		standardActorEffectData4.AddTooltipTokens(tokens, "AllyBaseEffectData", abilityMod_MartyrAoeOnReactHit != null, m_allyBaseEffectData);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_MartyrAoeOnReactHit)
		{
			val = abilityMod_MartyrAoeOnReactHit.m_extraAbsorbPerCrystalMod.GetModifiedValue(m_extraAbsorbPerCrystal);
		}
		else
		{
			val = m_extraAbsorbPerCrystal;
		}
		AddTokenInt(tokens, "ExtraAbsorbPerCrystal", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_MartyrAoeOnReactHit)
		{
			val2 = abilityMod_MartyrAoeOnReactHit.m_reactAoeDamageMod.GetModifiedValue(m_reactAoeDamage);
		}
		else
		{
			val2 = m_reactAoeDamage;
		}
		AddTokenInt(tokens, "ReactAoeDamage", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_MartyrAoeOnReactHit)
		{
			val3 = abilityMod_MartyrAoeOnReactHit.m_reactDamagePerCrystalMod.GetModifiedValue(m_reactDamagePerCrystal);
		}
		else
		{
			val3 = m_reactDamagePerCrystal;
		}
		AddTokenInt(tokens, "ReactDamagePerCrystal", empty3, val3);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_MartyrAoeOnReactHit)
		{
			effectInfo = abilityMod_MartyrAoeOnReactHit.m_reactEnemyHitEffectMod.GetModifiedValue(m_reactEnemyHitEffect);
		}
		else
		{
			effectInfo = m_reactEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "ReactEnemyHitEffect", m_reactEnemyHitEffect);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_MartyrAoeOnReactHit)
		{
			val4 = abilityMod_MartyrAoeOnReactHit.m_reactHealOnTargetMod.GetModifiedValue(m_reactHealOnTarget);
		}
		else
		{
			val4 = m_reactHealOnTarget;
		}
		AddTokenInt(tokens, "ReactHealOnTarget", empty4, val4);
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_MartyrAoeOnReactHit)
		{
			val5 = abilityMod_MartyrAoeOnReactHit.m_reactEnergyOnCasterPerReactMod.GetModifiedValue(m_reactEnergyOnCasterPerReact);
		}
		else
		{
			val5 = m_reactEnergyOnCasterPerReact;
		}
		AddTokenInt(tokens, "ReactEnergyOnCasterPerReact", empty5, val5);
		string empty6 = string.Empty;
		int val6;
		if ((bool)abilityMod_MartyrAoeOnReactHit)
		{
			val6 = abilityMod_MartyrAoeOnReactHit.m_cdrIfNoReactionTriggeredMod.GetModifiedValue(m_cdrIfNoReactionTriggered);
		}
		else
		{
			val6 = m_cdrIfNoReactionTriggered;
		}
		AddTokenInt(tokens, "CdrIfNoReactionTriggered", empty6, val6);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_MartyrAoeOnReactHit))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_MartyrAoeOnReactHit);
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
