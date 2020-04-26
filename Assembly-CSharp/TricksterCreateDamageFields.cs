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
		GroundEffectField groundFieldInfo = GetGroundFieldInfo();
		AbilityAreaShape num;
		if (UseInitialShapeOverride())
		{
			num = GetInitialShapeOverride();
		}
		else
		{
			num = groundFieldInfo.shape;
		}
		AbilityAreaShape shape = num;
		base.Targeter = new AbilityUtil_Targeter_TricksterFlare(this, m_afterImageSyncComp, shape, groundFieldInfo.penetrateLos, groundFieldInfo.IncludeEnemies(), groundFieldInfo.IncludeAllies(), AddFieldAroundSelf());
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedSelfEffectForMultiHit;
		if ((bool)m_abilityMod)
		{
			cachedSelfEffectForMultiHit = m_abilityMod.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit);
		}
		else
		{
			cachedSelfEffectForMultiHit = m_selfEffectForMultiHit;
		}
		m_cachedSelfEffectForMultiHit = cachedSelfEffectForMultiHit;
		m_cachedExtraEnemyEffectOnCast = ((!m_abilityMod) ? m_extraEnemyEffectOnCast : m_abilityMod.m_extraEnemyEffectOnCastMod.GetModifiedValue(m_extraEnemyEffectOnCast));
	}

	public bool AddFieldAroundSelf()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_addFieldAroundSelfMod.GetModifiedValue(m_addFieldAroundSelf);
		}
		else
		{
			result = m_addFieldAroundSelf;
		}
		return result;
	}

	public bool UseInitialShapeOverride()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_useInitialShapeOverrideMod.GetModifiedValue(m_useInitialShapeOverride);
		}
		else
		{
			result = m_useInitialShapeOverride;
		}
		return result;
	}

	public AbilityAreaShape GetInitialShapeOverride()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_initialShapeOverrideMod.GetModifiedValue(m_initialShapeOverride);
		}
		else
		{
			result = m_initialShapeOverride;
		}
		return result;
	}

	public GroundEffectField GetGroundFieldInfo()
	{
		GroundEffectField result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_groundFieldInfoMod.GetModifiedValue(m_groundFieldInfo);
		}
		else
		{
			result = m_groundFieldInfo;
		}
		return result;
	}

	public StandardEffectInfo GetSelfEffectForMultiHit()
	{
		StandardEffectInfo result;
		if (m_cachedSelfEffectForMultiHit != null)
		{
			result = m_cachedSelfEffectForMultiHit;
		}
		else
		{
			result = m_selfEffectForMultiHit;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEnemyEffectOnCast()
	{
		return (m_cachedExtraEnemyEffectOnCast == null) ? m_extraEnemyEffectOnCast : m_cachedExtraEnemyEffectOnCast;
	}

	public bool SpawnSpoilForEnemyHit()
	{
		return (!m_abilityMod) ? m_spawnSpoilForEnemyHit : m_abilityMod.m_spawnSpoilForEnemyHitMod.GetModifiedValue(m_spawnSpoilForEnemyHit);
	}

	public bool SpawnSpoilForAllyHit()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_spawnSpoilForAllyHitMod.GetModifiedValue(m_spawnSpoilForAllyHit);
		}
		else
		{
			result = m_spawnSpoilForAllyHit;
		}
		return result;
	}

	public bool OnlySpawnSpoilOnMultiHit()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_onlySpawnSpoilOnMultiHitMod.GetModifiedValue(m_onlySpawnSpoilOnMultiHit);
		}
		else
		{
			result = m_onlySpawnSpoilOnMultiHit;
		}
		return result;
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
		if (!AddFieldAroundSelf())
		{
			if (m_afterImageSyncComp != null)
			{
				return m_afterImageSyncComp.HasVaidAfterImages();
			}
		}
		return true;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		GroundEffectField groundFieldInfo = GetGroundFieldInfo();
		if (groundFieldInfo.IncludeEnemies())
		{
			Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeter, targetActor, currentTargeterIndex, groundFieldInfo.damageAmount, groundFieldInfo.subsequentDamageAmount);
		}
		if (groundFieldInfo.IncludeAllies())
		{
			Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeter, targetActor, currentTargeterIndex, groundFieldInfo.healAmount, groundFieldInfo.subsequentHealAmount, AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Secondary);
			Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeter, targetActor, currentTargeterIndex, groundFieldInfo.energyGain, groundFieldInfo.subsequentEnergyGain, AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Secondary);
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterCreateDamageFields abilityMod_TricksterCreateDamageFields = modAsBase as AbilityMod_TricksterCreateDamageFields;
		m_groundFieldInfo.AddTooltipTokens(tokens, "GroundEffect");
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_TricksterCreateDamageFields)
		{
			effectInfo = abilityMod_TricksterCreateDamageFields.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit);
		}
		else
		{
			effectInfo = m_selfEffectForMultiHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "SelfEffectForMultiHit", m_selfEffectForMultiHit);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_TricksterCreateDamageFields)
		{
			effectInfo2 = abilityMod_TricksterCreateDamageFields.m_extraEnemyEffectOnCastMod.GetModifiedValue(m_extraEnemyEffectOnCast);
		}
		else
		{
			effectInfo2 = m_extraEnemyEffectOnCast;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "ExtraEnemyEffectOnCast", m_extraEnemyEffectOnCast);
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current != null)
				{
					if (!current.IsDead())
					{
						m_afterImageSyncComp.TurnToPosition(current, targetPos);
						Animator modelAnimator = current.GetModelAnimator();
						modelAnimator.SetInteger("Attack", animationIndex);
						modelAnimator.SetBool("CinematicCam", cinecam);
						modelAnimator.SetTrigger("StartAttack");
					}
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		foreach (ActorData item in validAfterImages)
		{
			if (item != null)
			{
				if (!item.IsDead())
				{
					Animator modelAnimator = item.GetModelAnimator();
					modelAnimator.SetInteger("Attack", 0);
					modelAnimator.SetBool("CinematicCam", false);
				}
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_TricksterCreateDamageFields))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_TricksterCreateDamageFields);
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
