using System.Collections.Generic;
using UnityEngine;

public class ValkyrieGuard : Ability
{
	[Header("-- Shield effect")]
	public StandardEffectInfo m_shieldEffectInfo;
	[Header("-- Hit reactions")]
	public int m_techPointGainPerCoveredHit = 5;
	public int m_techPointGainPerTooCloseForCoverHit;
	public StandardEffectInfo m_coveredHitReactionEffect;
	public StandardEffectInfo m_tooCloseForCoverHitReactionEffect;
	[Header("-- Duration --")]
	public int m_coverDuration = 1;
	public bool m_coverLastsForever = true;
	[Header("-- Cover Ignore Min Dist?")]
	public bool m_coverIgnoreMinDist = true;
	[Header("-- Sequences")]
	public GameObject m_removeShieldSequencePrefab;
	public GameObject m_applyShieldSequencePrefab;

	private Valkyrie_SyncComponent m_syncComponent;
	private AbilityMod_ValkyrieGuard m_abilityMod;
	private StandardEffectInfo m_cachedShieldEffectInfo;
	private StandardEffectInfo m_cachedCoveredHitReactionEffect;
	private StandardEffectInfo m_cachedTooCloseForCoverHitReactionEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Aim Shield";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComponent = GetComponent<Valkyrie_SyncComponent>();
		Targeter = new AbilityUtil_Targeter_ValkyrieGuard(
			this,
			1f,
			true,
			false,
			false)
		{
			m_addCasterToActorsInRange = true
		};
		SetCachedFields();
	}

	private void SetCachedFields()
	{
		m_cachedShieldEffectInfo = m_abilityMod != null
			? m_abilityMod.m_shieldEffectInfoMod.GetModifiedValue(m_shieldEffectInfo)
			: m_shieldEffectInfo;
		m_cachedCoveredHitReactionEffect = m_abilityMod != null
			? m_abilityMod.m_coveredHitReactionEffectMod.GetModifiedValue(m_coveredHitReactionEffect)
			: m_coveredHitReactionEffect;
		m_cachedTooCloseForCoverHitReactionEffect = m_abilityMod != null
			? m_abilityMod.m_tooCloseForCoverHitReactionEffectMod.GetModifiedValue(m_tooCloseForCoverHitReactionEffect)
			: m_tooCloseForCoverHitReactionEffect;
	}

	public StandardEffectInfo GetShieldEffectInfo()
	{
		return m_cachedShieldEffectInfo ?? m_shieldEffectInfo;
	}

	public int GetTechPointGainPerCoveredHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainPerCoveredHitMod.GetModifiedValue(m_techPointGainPerCoveredHit)
			: m_techPointGainPerCoveredHit;
	}

	public int GetTechPointGainPerTooCloseForCoverHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainPerTooCloseForCoverHitMod.GetModifiedValue(m_techPointGainPerTooCloseForCoverHit)
			: m_techPointGainPerTooCloseForCoverHit;
	}

	public StandardEffectInfo GetCoveredHitReactionEffect()
	{
		return m_cachedCoveredHitReactionEffect ?? m_coveredHitReactionEffect;
	}

	public StandardEffectInfo GetTooCloseForCoverHitReactionEffect()
	{
		return m_cachedTooCloseForCoverHitReactionEffect ?? m_tooCloseForCoverHitReactionEffect;
	}

	public int GetExtraDamageNextShieldThrowPerCoveredHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageNextShieldThrowPerCoveredHitMod.GetModifiedValue(0)
			: 0;
	}

	public int GetMaxExtraDamageNextShieldThrow()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxExtraDamageNextShieldThrow.GetModifiedValue(0)
			: 0;
	}

	public int GetCoverDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coverDurationMod.GetModifiedValue(m_coverDuration)
			: m_coverDuration;
	}

	public bool CoverLastsForever()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coverLastsForeverMod.GetModifiedValue(m_coverLastsForever)
			: m_coverLastsForever;
	}

	public AbilityModCooldownReduction GetCooldownReductionOnNoBlock()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownReductionNoBlocks
			: null;
	}

	public bool CoverIgnoreMinDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coverIgnoreMinDistMod.GetModifiedValue(m_coverIgnoreMinDist)
			: m_coverIgnoreMinDist;
	}

	public int GetExtraAbsorb()
	{
		return m_syncComponent != null
			? m_syncComponent.m_extraAbsorbForGuard
			: 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieGuard))
		{
			m_abilityMod = abilityMod as AbilityMod_ValkyrieGuard;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, m_shieldEffectInfo, "ShieldEffectInfo", m_shieldEffectInfo);
		AddTokenInt(tokens, "TechPointGainPerCoveredHit", string.Empty, m_techPointGainPerCoveredHit);
		AddTokenInt(tokens, "TechPointGainPerTooCloseForCoverHit", string.Empty, m_techPointGainPerTooCloseForCoverHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_coveredHitReactionEffect, "CoveredHitReactionEffect", m_coveredHitReactionEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_tooCloseForCoverHitReactionEffect, "TooCloseForCoverHitReactionEffect", m_tooCloseForCoverHitReactionEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self, 0));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor == ActorData)
		{
			dictionary[AbilityTooltipSymbol.Absorb] = GetExtraAbsorb();
		}
		return dictionary;
	}

	public override Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		if (targetSquare == null)
		{
			return base.GetRotateToTargetPos(targets, caster);
		}
		VectorUtils.GetDirectionAndOffsetToClosestSide(targetSquare, targets[0].FreePos, false, out Vector3 offset);
		return targetSquare.ToVector3() + offset;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return caster != null
		       && caster.GetAbilityData() != null
		       && !caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ValkyrieDashAoE));
	}

	public override TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return TargetingParadigm.Direction;
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = 1f;
		max = 1f;
		return true;
	}
}
