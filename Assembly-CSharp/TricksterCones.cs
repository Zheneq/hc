using System.Collections.Generic;
using UnityEngine;

public class TricksterCones : Ability
{
	[Header("-- Cone Targeting")]
	public ConeTargetingInfo m_coneInfo;
	[Header("-- Enemy Hit Damage and Effects")]
	public int m_damageAmount = 3;
	public int m_subsequentDamageAmount = 2;
	public StandardEffectInfo m_enemyHitEffect;
	[Space(10f)]
	public bool m_useEnemyMultiHitEffect;
	public StandardEffectInfo m_enemyMultipleHitEffect;
	[Header("-- Ally Hit Heal and Effects")]
	public int m_allyHealAmount;
	public int m_allySubsequentHealAmount;
	public StandardEffectInfo m_allyHitEffect;
	[Space(10f)]
	public bool m_useAllyMultiHitEffect;
	public StandardEffectInfo m_allyMultipleHitEffect;
	[Header("-- Self Hit Heal and Effects")]
	public int m_selfHealAmount;
	public StandardEffectInfo m_selfHitEffect;
	public StandardEffectInfo m_selfEffectForMultiHit;
	[Header("-- Cooldown Reduction Per Enemy Hit By Clone --")]
	public int m_cooldownReductionPerHitByClone;
	public AbilityData.ActionType m_cooldownReductionActionType = AbilityData.ActionType.ABILITY_1;
	[Header("-- For spawning spoils")]
	public bool m_spawnSpoilForEnemyHit;
	public bool m_spawnSpoilForAllyHit;
	public SpoilsSpawnData m_spoilSpawnInfo;
	public bool m_onlySpawnSpoilOnMultiHit = true;
	[Header("-- Sequences")]
	public GameObject m_projectileSequencePrefab;
	public float m_impactDelayTime = 0.35f;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;
	private AbilityMod_TricksterCones m_abilityMod;
	private ConeTargetingInfo m_cachedConeInfo;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedEnemyMultipleHitEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;
	private StandardEffectInfo m_cachedAllyMultipleHitEffect;
	private StandardEffectInfo m_cachedSelfHitEffect;
	private StandardEffectInfo m_cachedSelfEffectForMultiHit;
	private SpoilsSpawnData m_cachedSpoilSpawnInfo;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Trickster Cones";
		}
		Setup();
	}

	private void Setup()
	{
		m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_TricksterCones(
			this,
			GetConeInfo(),
			3,
			GetNumCones,
			GetConeOrigins,
			GetConeDirections,
			GetFreePosForAim,
			true,
			false);
	}

	public int GetNumCones()
	{
		return m_afterImageSyncComp.GetValidAfterImages().Count + 1;
	}

	public Vector3 GetFreePosForAim(AbilityTarget currentTarget, ActorData caster)
	{
		List<ActorData> afterImages = new List<ActorData>();
		afterImages.Add(caster);
		afterImages.AddRange(m_afterImageSyncComp.GetValidAfterImages());
		Vector3 freePosForAim;
		Vector3 foo;
		m_afterImageSyncComp.CalcTargetingCenterAndAimAtPos(
			currentTarget.FreePos,
			caster,
			afterImages,
			false,
			out foo,
			out freePosForAim);
		return freePosForAim;
	}

	public List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		List<ActorData> afterImages = new List<ActorData>();
		afterImages.Add(caster);
		afterImages.AddRange(m_afterImageSyncComp.GetValidAfterImages());
		foreach (ActorData afterImage in afterImages)
		{
			list.Add(afterImage.GetLoSCheckPos());
		}
		return list;
	}

	public List<Vector3> GetConeDirections(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		List<ActorData> afterImages = new List<ActorData>();
		afterImages.Add(caster);
		afterImages.AddRange(m_afterImageSyncComp.GetValidAfterImages());
		foreach (ActorData afterImage in afterImages)
		{
			list.Add(targeterFreePos - afterImage.GetFreePos());
		}
		return list;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeInfo().m_radiusInSquares;
	}

	public override TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return TargetingParadigm.Position;
	}

	private void SetCachedFields()
	{
		m_cachedConeInfo = m_abilityMod != null
			? m_abilityMod.m_coneInfoMod.GetModifiedValue(m_coneInfo)
			: m_coneInfo;
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedEnemyMultipleHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyMultipleHitEffectMod.GetModifiedValue(m_enemyMultipleHitEffect)
			: m_enemyMultipleHitEffect;
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
		m_cachedAllyMultipleHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyMultipleHitEffectMod.GetModifiedValue(m_allyMultipleHitEffect)
			: m_allyMultipleHitEffect;
		m_cachedSelfHitEffect = m_abilityMod != null
			? m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect;
		m_cachedSelfEffectForMultiHit = m_abilityMod != null
			? m_abilityMod.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit)
			: m_selfEffectForMultiHit;
		m_cachedSpoilSpawnInfo = m_abilityMod != null
			? m_abilityMod.m_spoilSpawnInfoMod.GetModifiedValue(m_spoilSpawnInfo)
			: m_spoilSpawnInfo;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return m_cachedConeInfo ?? m_coneInfo;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetSubsequentDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_subsequentDamageAmountMod.GetModifiedValue(m_subsequentDamageAmount)
			: m_subsequentDamageAmount;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public bool UseEnemyMultiHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useEnemyMultiHitEffectMod.GetModifiedValue(m_useEnemyMultiHitEffect)
			: m_useEnemyMultiHitEffect;
	}

	public StandardEffectInfo GetEnemyMultipleHitEffect()
	{
		return m_cachedEnemyMultipleHitEffect ?? m_enemyMultipleHitEffect;
	}

	public int GetAllyHealAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount)
			: m_allyHealAmount;
	}

	public int GetAllySubsequentHealAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allySubsequentHealAmountMod.GetModifiedValue(m_allySubsequentHealAmount)
			: m_allySubsequentHealAmount;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public bool UseAllyMultiHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useAllyMultiHitEffectMod.GetModifiedValue(m_useAllyMultiHitEffect)
			: m_useAllyMultiHitEffect;
	}

	public StandardEffectInfo GetAllyMultipleHitEffect()
	{
		return m_cachedAllyMultipleHitEffect ?? m_allyMultipleHitEffect;
	}

	public int GetSelfHealAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealAmountMod.GetModifiedValue(m_selfHealAmount)
			: m_selfHealAmount;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		return m_cachedSelfHitEffect ?? m_selfHitEffect;
	}

	public StandardEffectInfo GetSelfEffectForMultiHit()
	{
		return m_cachedSelfEffectForMultiHit ?? m_selfEffectForMultiHit;
	}

	public int GetCooldownReductionPerHitByClone()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownReductionPerHitByCloneMod.GetModifiedValue(m_cooldownReductionPerHitByClone)
			: m_cooldownReductionPerHitByClone;
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

	public SpoilsSpawnData GetSpoilSpawnInfo()
	{
		return m_cachedSpoilSpawnInfo ?? m_spoilSpawnInfo;
	}

	public bool OnlySpawnSpoilOnMultiHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_onlySpawnSpoilOnMultiHitMod.GetModifiedValue(m_onlySpawnSpoilOnMultiHit)
			: m_onlySpawnSpoilOnMultiHit;
	}

	private int CalcDamageFromNumHits(int numHits, int numFromCover)
	{
		return ActorMultiHitContext.CalcDamageFromNumHits(numHits, numFromCover, GetDamageAmount(), GetSubsequentDamageAmount());
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
		GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHealAmount());
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetSelfHealAmount());
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = base.Debug_GetExpectedNumbersInTooltip();
		list.Add(m_subsequentDamageAmount);
		list.Add(m_allySubsequentHealAmount);
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		AbilityUtil_Targeter_TricksterCones abilityUtil_Targeter_TricksterCones = Targeter as AbilityUtil_Targeter_TricksterCones;
		ActorData actorData = ActorData;
		int numHits;
		if (abilityUtil_Targeter_TricksterCones != null
		    && actorData != null
		    && abilityUtil_Targeter_TricksterCones.m_actorToHitCount.TryGetValue(targetActor, out numHits))
		{
			int numFromCover = abilityUtil_Targeter_TricksterCones.m_actorToCoverCount[targetActor];
			if (actorData.GetTeam() != targetActor.GetTeam())
			{
				dictionary[AbilityTooltipSymbol.Damage] = CalcDamageFromNumHits(numHits, numFromCover);
			}
			else if (actorData != targetActor)
			{
				dictionary[AbilityTooltipSymbol.Healing] = GetAllyHealAmount() + (numHits - 1) * GetAllySubsequentHealAmount();
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterCones abilityMod_TricksterCones = modAsBase as AbilityMod_TricksterCones;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "SubsequentDamageAmount", string.Empty, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_subsequentDamageAmountMod.GetModifiedValue(m_subsequentDamageAmount)
			: m_subsequentDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_enemyMultipleHitEffectMod.GetModifiedValue(m_enemyMultipleHitEffect)
			: m_enemyMultipleHitEffect, "EnemyMultipleHitEffect", m_enemyMultipleHitEffect);
		AddTokenInt(tokens, "AllyHealAmount", string.Empty, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount)
			: m_allyHealAmount);
		AddTokenInt(tokens, "AllySubsequentHealAmount", string.Empty, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_allySubsequentHealAmountMod.GetModifiedValue(m_allySubsequentHealAmount)
			: m_allySubsequentHealAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_allyMultipleHitEffectMod.GetModifiedValue(m_allyMultipleHitEffect)
			: m_allyMultipleHitEffect, "AllyMultipleHitEffect", m_allyMultipleHitEffect);
		AddTokenInt(tokens, "SelfHealAmount", string.Empty, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_selfHealAmountMod.GetModifiedValue(m_selfHealAmount)
			: m_selfHealAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect, "SelfHitEffect", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit)
			: m_selfEffectForMultiHit, "SelfEffectForMultiHit", m_selfEffectForMultiHit);
		AddTokenInt(tokens, "CooldownReductionPerHitByClone", string.Empty, abilityMod_TricksterCones != null
			? abilityMod_TricksterCones.m_cooldownReductionPerHitByCloneMod.GetModifiedValue(m_cooldownReductionPerHitByClone)
			: m_cooldownReductionPerHitByClone);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterCones))
		{
			m_abilityMod = abilityMod as AbilityMod_TricksterCones;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
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
}
