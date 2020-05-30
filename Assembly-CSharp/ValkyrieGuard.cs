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
		AbilityUtil_Targeter_ValkyrieGuard abilityUtil_Targeter_ValkyrieGuard = new AbilityUtil_Targeter_ValkyrieGuard(this, 1f, true, false, false);
		abilityUtil_Targeter_ValkyrieGuard.m_addCasterToActorsInRange = true;
		base.Targeter = abilityUtil_Targeter_ValkyrieGuard;
		SetCachedFields();
	}

	private void SetCachedFields()
	{
		m_cachedShieldEffectInfo = ((!m_abilityMod) ? m_shieldEffectInfo : m_abilityMod.m_shieldEffectInfoMod.GetModifiedValue(m_shieldEffectInfo));
		StandardEffectInfo cachedCoveredHitReactionEffect;
		if ((bool)m_abilityMod)
		{
			cachedCoveredHitReactionEffect = m_abilityMod.m_coveredHitReactionEffectMod.GetModifiedValue(m_coveredHitReactionEffect);
		}
		else
		{
			cachedCoveredHitReactionEffect = m_coveredHitReactionEffect;
		}
		m_cachedCoveredHitReactionEffect = cachedCoveredHitReactionEffect;
		StandardEffectInfo cachedTooCloseForCoverHitReactionEffect;
		if ((bool)m_abilityMod)
		{
			cachedTooCloseForCoverHitReactionEffect = m_abilityMod.m_tooCloseForCoverHitReactionEffectMod.GetModifiedValue(m_tooCloseForCoverHitReactionEffect);
		}
		else
		{
			cachedTooCloseForCoverHitReactionEffect = m_tooCloseForCoverHitReactionEffect;
		}
		m_cachedTooCloseForCoverHitReactionEffect = cachedTooCloseForCoverHitReactionEffect;
	}

	public StandardEffectInfo GetShieldEffectInfo()
	{
		StandardEffectInfo result;
		if (m_cachedShieldEffectInfo != null)
		{
			result = m_cachedShieldEffectInfo;
		}
		else
		{
			result = m_shieldEffectInfo;
		}
		return result;
	}

	public int GetTechPointGainPerCoveredHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_techPointGainPerCoveredHitMod.GetModifiedValue(m_techPointGainPerCoveredHit);
		}
		else
		{
			result = m_techPointGainPerCoveredHit;
		}
		return result;
	}

	public int GetTechPointGainPerTooCloseForCoverHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_techPointGainPerTooCloseForCoverHitMod.GetModifiedValue(m_techPointGainPerTooCloseForCoverHit);
		}
		else
		{
			result = m_techPointGainPerTooCloseForCoverHit;
		}
		return result;
	}

	public StandardEffectInfo GetCoveredHitReactionEffect()
	{
		StandardEffectInfo result;
		if (m_cachedCoveredHitReactionEffect != null)
		{
			result = m_cachedCoveredHitReactionEffect;
		}
		else
		{
			result = m_coveredHitReactionEffect;
		}
		return result;
	}

	public StandardEffectInfo GetTooCloseForCoverHitReactionEffect()
	{
		StandardEffectInfo result;
		if (m_cachedTooCloseForCoverHitReactionEffect != null)
		{
			result = m_cachedTooCloseForCoverHitReactionEffect;
		}
		else
		{
			result = m_tooCloseForCoverHitReactionEffect;
		}
		return result;
	}

	public int GetExtraDamageNextShieldThrowPerCoveredHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageNextShieldThrowPerCoveredHitMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetMaxExtraDamageNextShieldThrow()
	{
		return m_abilityMod ? m_abilityMod.m_maxExtraDamageNextShieldThrow.GetModifiedValue(0) : 0;
	}

	public int GetCoverDuration()
	{
		return (!m_abilityMod) ? m_coverDuration : m_abilityMod.m_coverDurationMod.GetModifiedValue(m_coverDuration);
	}

	public bool CoverLastsForever()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coverLastsForeverMod.GetModifiedValue(m_coverLastsForever);
		}
		else
		{
			result = m_coverLastsForever;
		}
		return result;
	}

	public AbilityModCooldownReduction GetCooldownReductionOnNoBlock()
	{
		object result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cooldownReductionNoBlocks;
		}
		else
		{
			result = null;
		}
		return (AbilityModCooldownReduction)result;
	}

	public bool CoverIgnoreMinDist()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coverIgnoreMinDistMod.GetModifiedValue(m_coverIgnoreMinDist);
		}
		else
		{
			result = m_coverIgnoreMinDist;
		}
		return result;
	}

	public int GetExtraAbsorb()
	{
		if ((bool)m_syncComponent)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_syncComponent.m_extraAbsorbForGuard;
				}
			}
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieGuard))
		{
			m_abilityMod = (abilityMod as AbilityMod_ValkyrieGuard);
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
		if (targetActor == base.ActorData)
		{
			dictionary[AbilityTooltipSymbol.Absorb] = GetExtraAbsorb();
		}
		return dictionary;
	}

	public override Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(targets[0].GridPos);
		if (boardSquareSafe != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					VectorUtils.GetDirectionAndOffsetToClosestSide(boardSquareSafe, targets[0].FreePos, false, out Vector3 offset);
					return boardSquareSafe.ToVector3() + offset;
				}
				}
			}
		}
		return base.GetRotateToTargetPos(targets, caster);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		int result;
		if (caster != null)
		{
			if (caster.GetAbilityData() != null)
			{
				result = ((!caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ValkyrieDashAoE))) ? 1 : 0);
				goto IL_0059;
			}
		}
		result = 0;
		goto IL_0059;
		IL_0059:
		return (byte)result != 0;
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
