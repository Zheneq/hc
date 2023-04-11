// ROGUES
// SERVER
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
		m_afterImageSyncComp.CalcTargetingCenterAndAimAtPos(
			currentTarget.FreePos,
			caster,
			afterImages,
			false,
			out _,
			out Vector3 freePosForAim);
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
		if (abilityUtil_Targeter_TricksterCones != null
		    && actorData != null
		    && abilityUtil_Targeter_TricksterCones.m_actorToHitCount.TryGetValue(targetActor, out int numHits))
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
	
#if SERVER
	// added in rogues
	public override List<int> GetAdditionalBrushRegionsToDisrupt(ActorData caster, List<AbilityTarget> targets)
	{
		List<int> list = new List<int>();
		if (m_afterImageSyncComp == null)
		{
			return list;
		}
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages())
		{
			if (afterImage.IsInBrush())
			{
				list.Add(afterImage.GetBrushRegion());
			}
		}
		return list;
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetActorToHpDeltaMap(
			targets,
			caster,
			out List<VectorUtils.LaserCoords> endPoints,
			out Dictionary<ActorData, List<ActorData>> actorToHittingActors,
			out _, 
			out _,
			out _,
			null);
		List<List<ActorData>> targetActorLists = new List<List<ActorData>>();
		for (int i = 0; i < 3; i++)
		{
			targetActorLists.Add(new List<ActorData>());
		}
		foreach (ActorData actorData in actorToHittingActors.Keys)
		{
			int num = Mathf.Clamp(actorToHittingActors[actorData].Count - 1, 0, 2);
			for (int j = 0; j <= num; j++)
			{
				targetActorLists[j].Add(actorData);
			}
		}
		list.Add(new ServerClientUtils.SequenceStartData(
			m_projectileSequencePrefab,
			endPoints[0].end,
			targetActorLists[0].ToArray(),
			caster,
			additionalData.m_sequenceSource,
			new ScoundrelBlindFireSequence.ConeExtraParams
			{
				halfAngleDegrees = 0.5f * GetConeInfo().m_widthAngleDeg,
				maxDistInSquares = GetConeInfo().m_radiusInSquares
			}.ToArray()));
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		int index = 1;
		foreach (ActorData afterImage in validAfterImages)
		{
			if (index >= endPoints.Count)
			{
				Debug.LogError("number of end points did not match number of after images");
				break;
			}
			list.Add(new ServerClientUtils.SequenceStartData(
				m_projectileSequencePrefab,
				endPoints[index].end,
				targetActorLists[index].ToArray(),
				afterImage,
				additionalData.m_sequenceSource,
				new Sequence.IExtraSequenceParams[]
				{
					new ScoundrelBlindFireSequence.ConeExtraParams
					{
						halfAngleDegrees = 0.5f * GetConeInfo().m_widthAngleDeg,
						maxDistInSquares = GetConeInfo().m_radiusInSquares
					}
				}));
			index++;
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Dictionary<ActorData, int> actorToHpDeltaMap = GetActorToHpDeltaMap(
			targets,
			caster,
			out _,
			out Dictionary<ActorData, List<ActorData>> actorToHittingActors,
			out Dictionary<ActorData, int> actorToCoverCount,
			out int numConesWithHits,
			out int numActorsHitByClones,
			nonActorTargetInfo);
		foreach (ActorData actorData in actorToHpDeltaMap.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
			List<ActorData> hittingActors = actorToHittingActors[actorData];
			int numHits = hittingActors.Count;
			bool isSpawningSpoil = !OnlySpawnSpoilOnMultiHit() || numHits > 1;
			StandardEffectInfo standardEffectInfo;
			if (actorData == caster)
			{
				actorHitResults.SetBaseHealing(actorToHpDeltaMap[actorData]);
				standardEffectInfo = GetSelfHitEffect();
				if (numConesWithHits > 1)
				{
					actorHitResults.AddStandardEffectInfo(GetSelfEffectForMultiHit());
				}
				int cooldownReduction = GetCooldownReductionPerHitByClone() * numActorsHitByClones;
				if (cooldownReduction > 0)
				{
					actorHitResults.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(m_cooldownReductionActionType, -1 * cooldownReduction));
				}
			}
			else if (actorData.GetTeam() == caster.GetTeam())
			{
				actorHitResults.SetBaseHealing(actorToHpDeltaMap[actorData]);
				standardEffectInfo = numHits > 1 && UseAllyMultiHitEffect() ? GetAllyMultipleHitEffect() : GetAllyHitEffect();
				if (isSpawningSpoil && SpawnSpoilForAllyHit())
				{
					actorHitResults.AddSpoilSpawnData(new SpoilSpawnDataForAbilityHit(actorData, caster.GetTeam(), GetSpoilSpawnInfo()));
				}
			}
			else
			{
				actorHitResults.SetBaseDamage(actorToHpDeltaMap[actorData]);
				standardEffectInfo = numHits > 1 && UseEnemyMultiHitEffect() ? GetEnemyMultipleHitEffect() : GetEnemyHitEffect();
				if (isSpawningSpoil && SpawnSpoilForEnemyHit())
				{
					actorHitResults.AddSpoilSpawnData(new SpoilSpawnDataForAbilityHit(actorData, caster.GetTeam(), GetSpoilSpawnInfo()));
				}
				foreach (ActorData hittingActor in hittingActors)
				{
					actorHitResults.AddActorToReveal(hittingActor);
				}
			}
			if (standardEffectInfo != null)
			{
				actorHitResults.AddStandardEffectInfo(standardEffectInfo);
			}
			if (actorToCoverCount[actorData] > 0)
			{
				actorHitResults.OverrideAsInCover();
			}
			if (numHits >= 3)
			{
				actorHitResults.AddHitResultsTag(HitResultsTags.TripleHit);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private Dictionary<ActorData, int> GetActorToHpDeltaMap(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<VectorUtils.LaserCoords> endPoints,
		out Dictionary<ActorData, List<ActorData>> actorToHittingActors,
		out Dictionary<ActorData, int> actorToCoverCount,
		out int numConesWithHits,
		out int numActorsHitByClones,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		endPoints = new List<VectorUtils.LaserCoords>();
		actorToHittingActors = new Dictionary<ActorData, List<ActorData>>();
		actorToCoverCount = new Dictionary<ActorData, int>();
		numConesWithHits = 0;
		numActorsHitByClones = 0;
		List<ActorData> list = new List<ActorData>();
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		List<ActorData> allTargetingActors = new List<ActorData>();
		allTargetingActors.Add(caster);
		allTargetingActors.AddRange(validAfterImages);
		m_afterImageSyncComp.CalcTargetingCenterAndAimAtPos(
			targets[0].FreePos,
			caster,
			allTargetingActors, 
			true,
			out _,
			out Vector3 freePosForAim);
		ConeTargetingInfo coneInfo = GetConeInfo();
		for (int i = 0; i < allTargetingActors.Count; i++)
		{
			ActorData targetingActor = allTargetingActors[i];
			Vector3 loSCheckPos = targetingActor.GetLoSCheckPos();
			Vector3 aimDir = freePosForAim - targetingActor.GetFreePos();
			aimDir.y = 0f;
			aimDir.Normalize();
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDir);
			List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
				loSCheckPos,
				coneCenterAngleDegrees,
				coneInfo.m_widthAngleDeg,
				coneInfo.m_radiusInSquares,
				coneInfo.m_backwardsOffset,
				coneInfo.m_penetrateLos,
				caster,
				TargeterUtils.GetRelevantTeams(caster, coneInfo.m_affectsAllies, coneInfo.m_affectsEnemies),
				nonActorTargetInfo);
			actorsInCone.Remove(caster);
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = loSCheckPos;
			laserCoords.end = loSCheckPos + aimDir;
			endPoints.Add(laserCoords);
			if (actorsInCone.Count > 0)
			{
				numConesWithHits++;
			}
			foreach (ActorData hitActor in actorsInCone)
			{
				if (i > 0 && !list.Contains(hitActor))
				{
					list.Add(hitActor);
				}
				// custom
				bool isInCover = hitActor.GetActorCover().IsInCoverWrt(laserCoords.start);
				// rogues
				// bool isInCover = actorData2.GetActorCover().IsInCoverWrt(laserCoords.start, out hitChanceBracketType);
				if (actorToHittingActors.TryGetValue(hitActor, out List<ActorData> hittingActors))
				{
					hittingActors.Add(targetingActor);
					actorToCoverCount[hitActor] += isInCover ? 1 : 0;
				}
				else
				{
					actorToHittingActors[hitActor] = new List<ActorData> { targetingActor };
					actorToCoverCount[hitActor] = isInCover ? 1 : 0;
				}
			}
		}
		foreach (ActorData hitActor in actorToHittingActors.Keys)
		{
			int numHits = actorToHittingActors[hitActor].Count;
			int numFromCover = actorToCoverCount[hitActor];
			if (hitActor.GetTeam() != caster.GetTeam())
			{
				dictionary[hitActor] = CalcDamageFromNumHits(numHits, numFromCover);
			}
			else
			{
				dictionary[hitActor] = GetAllyHealAmount() + (numHits - 1) * GetAllySubsequentHealAmount();
			}
		}
		numActorsHitByClones = list.Count;
		if (GetSelfHealAmount() > 0
		    || GetSelfHitEffect().m_applyEffect
		    || (GetSelfEffectForMultiHit().m_applyEffect && numConesWithHits > 1)
		    || GetCooldownReductionPerHitByClone() * numActorsHitByClones > 0)
		{
			dictionary[caster] = GetSelfHealAmount();
			actorToHittingActors[caster] = new List<ActorData> { caster };
		}
		return dictionary;
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = base.CalcPointsOfInterestForCamera(targets, caster);
		if (m_afterImageSyncComp == null)
		{
			return list;
		}
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages())
		{
			if (afterImage.GetCurrentBoardSquare() != null)
			{
				list.Add(afterImage.GetCurrentBoardSquare().ToVector3());
			}
		}
		return list;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster != target)
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.TricksterStats.TargetsHitByPhotonSpray);
		}
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.HasHitResultsTag(HitResultsTags.TripleHit))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.TricksterStats.TargetsHitByThreeImages);
		}
	}
#endif
}
