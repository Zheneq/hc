using System.Collections.Generic;
using UnityEngine;

public class ArcherArrowRain : Ability
{
	[Separator("Targeting Info")]
	public float m_startRadius = 3f;
	public float m_endRadius = 3f;
	public float m_lineRadius = 3f;
	public float m_minRangeBetween = 1f;
	public float m_maxRangeBetween = 4f;
	[Header("-- Whether require LoS to end square of line")]
	public bool m_linePenetrateLoS;
	[Header("-- Whether check LoS for gameplay hits")]
	public bool m_aoePenetrateLoS;
	public int m_maxTargets = 5;
	[Separator("Enemy Hit")]
	public int m_damage = 40;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;
	public GameObject m_hitAreaSequencePrefab;
	
	private AbilityMod_ArcherArrowRain m_abilityMod;
	private ArcherHealingDebuffArrow m_healArrowAbility;
	private AbilityData.ActionType m_healArrowActionType = AbilityData.ActionType.INVALID_ACTION;
	private AbilityData m_abilityData;
	private ActorTargeting m_actorTargeting;
	private Archer_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedAdditionalEnemyHitEffect;
	private StandardEffectInfo m_cachedSingleEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Arrow Rain";
		}
		m_abilityData = GetComponent<AbilityData>();
		if (m_abilityData != null)
		{
			m_healArrowAbility = GetAbilityOfType(typeof(ArcherHealingDebuffArrow)) as ArcherHealingDebuffArrow;
			if (m_healArrowAbility != null)
			{
				m_healArrowActionType = m_abilityData.GetActionTypeOfAbility(m_healArrowAbility);
			}
		}
		m_actorTargeting = GetComponent<ActorTargeting>();
		m_syncComp = GetComponent<Archer_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeters.Clear();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_CapsuleAoE abilityUtil_Targeter_CapsuleAoE = new AbilityUtil_Targeter_CapsuleAoE(
				this, GetStartRadius(), GetEndRadius(), GetLineRadius(), GetMaxTargets(), false, AoePenetrateLoS());
			abilityUtil_Targeter_CapsuleAoE.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_CapsuleAoE.ShowArcToShape = false;
			Targeters.Add(abilityUtil_Targeter_CapsuleAoE);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return GetTargetData().Length;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex <= 0)
		{
			return base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
		}
		
		BoardSquare prevTargetSquare = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (prevTargetSquare != null && targetSquare != null)
		{
			float range = Vector3.Distance(prevTargetSquare.ToVector3(), targetSquare.ToVector3());
			if (range <= GetMaxRangeBetween() * Board.Get().squareSize && range >= GetMinRangeBetween() * Board.Get().squareSize)
			{
				return LinePenetrateLoS() || prevTargetSquare.GetLOS(targetSquare.x, targetSquare.y);
			}
		}
		return false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherArrowRain))
		{
			m_abilityMod = abilityMod as AbilityMod_ArcherArrowRain;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedAdditionalEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_additionalEnemyHitEffect.GetModifiedValue(null)
			: null;
		m_cachedSingleEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_singleEnemyHitEffectMod.GetModifiedValue(null)
			: null;
	}

	public float GetStartRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_startRadiusMod.GetModifiedValue(m_startRadius)
			: m_startRadius;
	}

	public float GetEndRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_endRadiusMod.GetModifiedValue(m_endRadius)
			: m_endRadius;
	}

	public float GetLineRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lineRadiusMod.GetModifiedValue(m_lineRadius)
			: m_lineRadius;
	}

	public float GetMinRangeBetween()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minRangeBetweenMod.GetModifiedValue(m_minRangeBetween)
			: m_minRangeBetween;
	}

	public float GetMaxRangeBetween()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxRangeBetweenMod.GetModifiedValue(m_maxRangeBetween)
			: m_maxRangeBetween;
	}

	public bool LinePenetrateLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_linePenetrateLoSMod.GetModifiedValue(m_linePenetrateLoS)
			: m_linePenetrateLoS;
	}

	public bool AoePenetrateLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoePenetrateLoSMod.GetModifiedValue(m_aoePenetrateLoS)
			: m_aoePenetrateLoS;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public int GetDamageBelowHealthThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageBelowHealthThresholdMod.GetModifiedValue(GetDamage())
			: GetDamage();
	}

	public float GetHealthThresholdForBonusDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healthThresholdForDamageMod.GetModifiedValue(0f)
			: 0f;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public StandardEffectInfo GetAdditionalEnemyHitEffect()
	{
		return m_cachedAdditionalEnemyHitEffect;
	}

	public StandardEffectInfo GetSingleEnemyHitEffect()
	{
		return m_cachedSingleEnemyHitEffect;
	}

	public int GetTechPointRefundNoHits()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointRefundNoHits.GetModifiedValue(0)
			: 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int damage = GetDamage();
		if (targetActor.GetHitPointPercent() <= GetHealthThresholdForBonusDamage())
		{
			damage = GetDamageBelowHealthThreshold();
		}
		if (IsReactionHealTarget(targetActor))
		{
			damage += m_healArrowAbility.GetExtraDamageToThisTargetFromCaster();
		}
		dictionary[AbilityTooltipSymbol.Damage] = damage;
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = Targeters[currentTargeterIndex].GetActorsInRange();
		foreach (AbilityUtil_Targeter.ActorTarget target in actorsInRange)
		{
			if (IsReactionHealTarget(target.m_actor))
			{
				return m_healArrowAbility.GetTechPointsPerHeal();
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	private bool IsReactionHealTarget(ActorData targetActor)
	{
		if (m_syncComp.m_healReactionTargetActor == targetActor.ActorIndex && !m_syncComp.ActorHasUsedHealReaction(ActorData))
		{
			return true;
		}
		if (m_healArrowActionType != AbilityData.ActionType.INVALID_ACTION && m_actorTargeting != null)
		{
			List<AbilityTarget> abilityTargetsInRequest = m_actorTargeting.GetAbilityTargetsInRequest(m_healArrowActionType);
			if (abilityTargetsInRequest != null && abilityTargetsInRequest.Count > 0)
			{
				BoardSquare square = Board.Get().GetSquare(abilityTargetsInRequest[0].GridPos);
				ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(square, true, false, ActorData);
				if (targetableActorOnSquare == targetActor)
				{
					return true;
				}
			}
		}
		return false;
	}
}
