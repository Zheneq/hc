using System.Collections.Generic;
using UnityEngine;

public class TricksterCreateDamageFields : Ability
{
	[Header("-- Targeting --")]
	public bool m_addFieldAroundSelf = true;
	public bool m_useInitialShapeOverride;
	public AbilityAreaShape m_initialShapeOverride = AbilityAreaShape.Three_x_Three;
	[Header("-- Ground Field Info --")]
	public GroundEffectField m_groundFieldInfo;
	[Header("-- Self Effect for Multi Hit")]
	public StandardEffectInfo m_selfEffectForMultiHit;
	[Header("-- Extra Enemy Hit Effect On Cast")]
	public StandardEffectInfo m_extraEnemyEffectOnCast;
	[Header("-- Spoil spawn info")]
	public bool m_spawnSpoilForEnemyHit = true;
	public bool m_spawnSpoilForAllyHit;
	public SpoilsSpawnData m_spoilSpawnInfo;
	public bool m_onlySpawnSpoilOnMultiHit = true;
	[Header("-- use [Cast Sequence Prefab] to time spawning of ground effect (including temp satellite)")]
	public GameObject m_castSequencePrefab;
	[Header("   use [Temp Satellite Sequence Prefab] for satellites above each ground field")]
	public GameObject m_tempSatelliteSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;
	private AbilityMod_TricksterCreateDamageFields m_abilityMod;
	private StandardEffectInfo m_cachedSelfEffectForMultiHit;
	private StandardEffectInfo m_cachedExtraEnemyEffectOnCast;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Ground Fields";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_afterImageSyncComp == null)
		{
			m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		}
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_TricksterFlare(
			this,
			m_afterImageSyncComp,
			UseInitialShapeOverride() ? GetInitialShapeOverride() : GetGroundFieldInfo().shape,
			GetGroundFieldInfo().penetrateLos,
			GetGroundFieldInfo().IncludeEnemies(),
			GetGroundFieldInfo().IncludeAllies(),
			AddFieldAroundSelf());
	}

	private void SetCachedFields()
	{
		m_cachedSelfEffectForMultiHit = m_abilityMod != null
			? m_abilityMod.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit)
			: m_selfEffectForMultiHit;
		m_cachedExtraEnemyEffectOnCast = m_abilityMod != null
			? m_abilityMod.m_extraEnemyEffectOnCastMod.GetModifiedValue(m_extraEnemyEffectOnCast)
			: m_extraEnemyEffectOnCast;
	}

	public bool AddFieldAroundSelf()
	{
		return m_abilityMod != null
			? m_abilityMod.m_addFieldAroundSelfMod.GetModifiedValue(m_addFieldAroundSelf)
			: m_addFieldAroundSelf;
	}

	public bool UseInitialShapeOverride()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useInitialShapeOverrideMod.GetModifiedValue(m_useInitialShapeOverride)
			: m_useInitialShapeOverride;
	}

	public AbilityAreaShape GetInitialShapeOverride()
	{
		return m_abilityMod != null
			? m_abilityMod.m_initialShapeOverrideMod.GetModifiedValue(m_initialShapeOverride)
			: m_initialShapeOverride;
	}

	public GroundEffectField GetGroundFieldInfo()
	{
		return m_abilityMod != null
			? m_abilityMod.m_groundFieldInfoMod.GetModifiedValue(m_groundFieldInfo)
			: m_groundFieldInfo;
	}

	public StandardEffectInfo GetSelfEffectForMultiHit()
	{
		return m_cachedSelfEffectForMultiHit ?? m_selfEffectForMultiHit;
	}

	public StandardEffectInfo GetExtraEnemyEffectOnCast()
	{
		return m_cachedExtraEnemyEffectOnCast ?? m_extraEnemyEffectOnCast;
	}

	public bool SpawnSpoilForEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_spawnSpoilForEnemyHitMod.GetModifiedValue(m_spawnSpoilForEnemyHit)
			: m_spawnSpoilForEnemyHit;
	}

	public bool SpawnSpoilForAllyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_spawnSpoilForAllyHitMod.GetModifiedValue(m_spawnSpoilForAllyHit)
			: m_spawnSpoilForAllyHit;
	}

	public bool OnlySpawnSpoilOnMultiHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_onlySpawnSpoilOnMultiHitMod.GetModifiedValue(m_onlySpawnSpoilOnMultiHit)
			: m_onlySpawnSpoilOnMultiHit;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GroundEffectField groundFieldInfo = GetGroundFieldInfo();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, groundFieldInfo.damageAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Secondary, groundFieldInfo.healAmount);
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Secondary, groundFieldInfo.energyGain);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return AddFieldAroundSelf()
		       || m_afterImageSyncComp == null
		       || m_afterImageSyncComp.HasVaidAfterImages();
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		GroundEffectField groundFieldInfo = GetGroundFieldInfo();
		if (groundFieldInfo.IncludeEnemies())
		{
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeter,
				targetActor,
				currentTargeterIndex,
				groundFieldInfo.damageAmount,
				groundFieldInfo.subsequentDamageAmount);
		}
		if (groundFieldInfo.IncludeAllies())
		{
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeter,
				targetActor,
				currentTargeterIndex,
				groundFieldInfo.healAmount,
				groundFieldInfo.subsequentHealAmount,
				AbilityTooltipSymbol.Healing,
				AbilityTooltipSubject.Secondary);
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeter,
				targetActor,
				currentTargeterIndex,
				groundFieldInfo.energyGain,
				groundFieldInfo.subsequentEnergyGain,
				AbilityTooltipSymbol.Energy,
				AbilityTooltipSubject.Secondary);
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterCreateDamageFields abilityMod_TricksterCreateDamageFields = modAsBase as AbilityMod_TricksterCreateDamageFields;
		m_groundFieldInfo.AddTooltipTokens(tokens, "GroundEffect");
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCreateDamageFields != null
			? abilityMod_TricksterCreateDamageFields.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit)
			: m_selfEffectForMultiHit, "SelfEffectForMultiHit", m_selfEffectForMultiHit);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCreateDamageFields != null
			? abilityMod_TricksterCreateDamageFields.m_extraEnemyEffectOnCastMod.GetModifiedValue(m_extraEnemyEffectOnCast)
			: m_extraEnemyEffectOnCast, "ExtraEnemyEffectOnCast", m_extraEnemyEffectOnCast);
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages())
		{
			if (afterImage == null || afterImage.IsDead())
			{
				continue;
			}
			m_afterImageSyncComp.TurnToPosition(afterImage, targetPos);
			Animator modelAnimator = afterImage.GetModelAnimator();
			modelAnimator.SetInteger("Attack", animationIndex);
			modelAnimator.SetBool("CinematicCam", cinecam);
			modelAnimator.SetTrigger("StartAttack");
		}
	}

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages())
		{
			if (afterImage == null || afterImage.IsDead())
			{
				continue;
			}
			Animator modelAnimator = afterImage.GetModelAnimator();
			modelAnimator.SetInteger("Attack", 0);
			modelAnimator.SetBool("CinematicCam", false);
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterCreateDamageFields))
		{
			m_abilityMod = abilityMod as AbilityMod_TricksterCreateDamageFields;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
