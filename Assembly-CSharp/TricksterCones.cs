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
		base.Targeter = new AbilityUtil_Targeter_TricksterCones(this, GetConeInfo(), 3, GetNumCones, GetConeOrigins, GetConeDirections, GetFreePosForAim, true, false);
	}

	public int GetNumCones()
	{
		return m_afterImageSyncComp.GetValidAfterImages().Count + 1;
	}

	public Vector3 GetFreePosForAim(AbilityTarget currentTarget, ActorData caster)
	{
		List<ActorData> list = new List<ActorData>();
		list.Add(caster);
		list.AddRange(m_afterImageSyncComp.GetValidAfterImages());
		m_afterImageSyncComp.CalcTargetingCenterAndAimAtPos(currentTarget.FreePos, caster, list, false, out Vector3 _, out Vector3 freePosForAim);
		return freePosForAim;
	}

	public List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		List<ActorData> list2 = new List<ActorData>();
		list2.Add(caster);
		list2.AddRange(m_afterImageSyncComp.GetValidAfterImages());
		foreach (ActorData item in list2)
		{
			list.Add(item.GetLoSCheckPos());
		}
		return list;
	}

	public List<Vector3> GetConeDirections(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		List<ActorData> list2 = new List<ActorData>();
		list2.Add(caster);
		list2.AddRange(m_afterImageSyncComp.GetValidAfterImages());
		using (List<ActorData>.Enumerator enumerator = list2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				list.Add(targeterFreePos - current.GetFreePos());
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return list;
					}
					/*OpCode not supported: LdMemberToken*/;
					return list;
				}
			}
		}
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
		m_cachedConeInfo = ((!m_abilityMod) ? m_coneInfo : m_abilityMod.m_coneInfoMod.GetModifiedValue(m_coneInfo));
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
		StandardEffectInfo cachedEnemyMultipleHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyMultipleHitEffect = m_abilityMod.m_enemyMultipleHitEffectMod.GetModifiedValue(m_enemyMultipleHitEffect);
		}
		else
		{
			cachedEnemyMultipleHitEffect = m_enemyMultipleHitEffect;
		}
		m_cachedEnemyMultipleHitEffect = cachedEnemyMultipleHitEffect;
		StandardEffectInfo cachedAllyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedAllyHitEffect = m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = m_allyHitEffect;
		}
		m_cachedAllyHitEffect = cachedAllyHitEffect;
		StandardEffectInfo cachedAllyMultipleHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedAllyMultipleHitEffect = m_abilityMod.m_allyMultipleHitEffectMod.GetModifiedValue(m_allyMultipleHitEffect);
		}
		else
		{
			cachedAllyMultipleHitEffect = m_allyMultipleHitEffect;
		}
		m_cachedAllyMultipleHitEffect = cachedAllyMultipleHitEffect;
		StandardEffectInfo cachedSelfHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedSelfHitEffect = m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect);
		}
		else
		{
			cachedSelfHitEffect = m_selfHitEffect;
		}
		m_cachedSelfHitEffect = cachedSelfHitEffect;
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
		SpoilsSpawnData cachedSpoilSpawnInfo;
		if ((bool)m_abilityMod)
		{
			cachedSpoilSpawnInfo = m_abilityMod.m_spoilSpawnInfoMod.GetModifiedValue(m_spoilSpawnInfo);
		}
		else
		{
			cachedSpoilSpawnInfo = m_spoilSpawnInfo;
		}
		m_cachedSpoilSpawnInfo = cachedSpoilSpawnInfo;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		ConeTargetingInfo result;
		if (m_cachedConeInfo != null)
		{
			result = m_cachedConeInfo;
		}
		else
		{
			result = m_coneInfo;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public int GetSubsequentDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_subsequentDamageAmountMod.GetModifiedValue(m_subsequentDamageAmount);
		}
		else
		{
			result = m_subsequentDamageAmount;
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

	public bool UseEnemyMultiHitEffect()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_useEnemyMultiHitEffectMod.GetModifiedValue(m_useEnemyMultiHitEffect);
		}
		else
		{
			result = m_useEnemyMultiHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyMultipleHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyMultipleHitEffect != null)
		{
			result = m_cachedEnemyMultipleHitEffect;
		}
		else
		{
			result = m_enemyMultipleHitEffect;
		}
		return result;
	}

	public int GetAllyHealAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount);
		}
		else
		{
			result = m_allyHealAmount;
		}
		return result;
	}

	public int GetAllySubsequentHealAmount()
	{
		return (!m_abilityMod) ? m_allySubsequentHealAmount : m_abilityMod.m_allySubsequentHealAmountMod.GetModifiedValue(m_allySubsequentHealAmount);
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyHitEffect != null)
		{
			result = m_cachedAllyHitEffect;
		}
		else
		{
			result = m_allyHitEffect;
		}
		return result;
	}

	public bool UseAllyMultiHitEffect()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_useAllyMultiHitEffectMod.GetModifiedValue(m_useAllyMultiHitEffect);
		}
		else
		{
			result = m_useAllyMultiHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAllyMultipleHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyMultipleHitEffect != null)
		{
			result = m_cachedAllyMultipleHitEffect;
		}
		else
		{
			result = m_allyMultipleHitEffect;
		}
		return result;
	}

	public int GetSelfHealAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfHealAmountMod.GetModifiedValue(m_selfHealAmount);
		}
		else
		{
			result = m_selfHealAmount;
		}
		return result;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedSelfHitEffect != null)
		{
			result = m_cachedSelfHitEffect;
		}
		else
		{
			result = m_selfHitEffect;
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

	public int GetCooldownReductionPerHitByClone()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cooldownReductionPerHitByCloneMod.GetModifiedValue(m_cooldownReductionPerHitByClone);
		}
		else
		{
			result = m_cooldownReductionPerHitByClone;
		}
		return result;
	}

	public bool SpawnSpoilForEnemyHit()
	{
		return (!m_abilityMod) ? m_spawnSpoilForEnemyHit : m_abilityMod.m_spawnSpoilForEnemyHitMod.GetModifiedValue(m_spawnSpoilForEnemyHit);
	}

	public bool SpawnSpoilForAllyHit()
	{
		return (!m_abilityMod) ? m_spawnSpoilForAllyHit : m_abilityMod.m_spawnSpoilForAllyHitMod.GetModifiedValue(m_spawnSpoilForAllyHit);
	}

	public SpoilsSpawnData GetSpoilSpawnInfo()
	{
		SpoilsSpawnData result;
		if (m_cachedSpoilSpawnInfo != null)
		{
			result = m_cachedSpoilSpawnInfo;
		}
		else
		{
			result = m_spoilSpawnInfo;
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
		AbilityUtil_Targeter_TricksterCones abilityUtil_Targeter_TricksterCones = base.Targeter as AbilityUtil_Targeter_TricksterCones;
		ActorData actorData = base.ActorData;
		if (abilityUtil_Targeter_TricksterCones != null)
		{
			if (actorData != null)
			{
				if (abilityUtil_Targeter_TricksterCones.m_actorToHitCount.ContainsKey(targetActor))
				{
					int num = abilityUtil_Targeter_TricksterCones.m_actorToHitCount[targetActor];
					int numFromCover = abilityUtil_Targeter_TricksterCones.m_actorToCoverCount[targetActor];
					if (actorData.GetTeam() != targetActor.GetTeam())
					{
						int num3 = dictionary[AbilityTooltipSymbol.Damage] = CalcDamageFromNumHits(num, numFromCover);
					}
					else if (actorData != targetActor)
					{
						int num5 = dictionary[AbilityTooltipSymbol.Healing] = GetAllyHealAmount() + (num - 1) * GetAllySubsequentHealAmount();
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterCones abilityMod_TricksterCones = modAsBase as AbilityMod_TricksterCones;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_TricksterCones)
		{
			val = abilityMod_TricksterCones.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_TricksterCones)
		{
			val2 = abilityMod_TricksterCones.m_subsequentDamageAmountMod.GetModifiedValue(m_subsequentDamageAmount);
		}
		else
		{
			val2 = m_subsequentDamageAmount;
		}
		AddTokenInt(tokens, "SubsequentDamageAmount", empty2, val2);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_TricksterCones)
		{
			effectInfo = abilityMod_TricksterCones.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", m_enemyHitEffect);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_TricksterCones)
		{
			effectInfo2 = abilityMod_TricksterCones.m_enemyMultipleHitEffectMod.GetModifiedValue(m_enemyMultipleHitEffect);
		}
		else
		{
			effectInfo2 = m_enemyMultipleHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EnemyMultipleHitEffect", m_enemyMultipleHitEffect);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_TricksterCones)
		{
			val3 = abilityMod_TricksterCones.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount);
		}
		else
		{
			val3 = m_allyHealAmount;
		}
		AddTokenInt(tokens, "AllyHealAmount", empty3, val3);
		AddTokenInt(tokens, "AllySubsequentHealAmount", string.Empty, (!abilityMod_TricksterCones) ? m_allySubsequentHealAmount : abilityMod_TricksterCones.m_allySubsequentHealAmountMod.GetModifiedValue(m_allySubsequentHealAmount));
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_TricksterCones)
		{
			effectInfo3 = abilityMod_TricksterCones.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			effectInfo3 = m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "AllyHitEffect", m_allyHitEffect);
		StandardEffectInfo effectInfo4;
		if ((bool)abilityMod_TricksterCones)
		{
			effectInfo4 = abilityMod_TricksterCones.m_allyMultipleHitEffectMod.GetModifiedValue(m_allyMultipleHitEffect);
		}
		else
		{
			effectInfo4 = m_allyMultipleHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo4, "AllyMultipleHitEffect", m_allyMultipleHitEffect);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_TricksterCones)
		{
			val4 = abilityMod_TricksterCones.m_selfHealAmountMod.GetModifiedValue(m_selfHealAmount);
		}
		else
		{
			val4 = m_selfHealAmount;
		}
		AddTokenInt(tokens, "SelfHealAmount", empty4, val4);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_TricksterCones) ? m_selfHitEffect : abilityMod_TricksterCones.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect), "SelfHitEffect", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_TricksterCones) ? m_selfEffectForMultiHit : abilityMod_TricksterCones.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit), "SelfEffectForMultiHit", m_selfEffectForMultiHit);
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_TricksterCones)
		{
			val5 = abilityMod_TricksterCones.m_cooldownReductionPerHitByCloneMod.GetModifiedValue(m_cooldownReductionPerHitByClone);
		}
		else
		{
			val5 = m_cooldownReductionPerHitByClone;
		}
		AddTokenInt(tokens, "CooldownReductionPerHitByClone", empty5, val5);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_TricksterCones))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_TricksterCones);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
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
				switch (6)
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
}
