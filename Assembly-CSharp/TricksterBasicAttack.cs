using System.Collections.Generic;
using UnityEngine;

public class TricksterBasicAttack : Ability
{
	[Header("-- Laser Targeting")]
	public LaserTargetingInfo m_laserInfo;

	[Header("-- Damage and Effect")]
	public int m_laserDamageAmount = 3;

	public int m_laserSubsequentDamageAmount = 2;

	public int m_extraDamageForSingleHit;

	public StandardEffectInfo m_enemySingleHitHitEffect;

	public StandardEffectInfo m_enemyMultiHitEffect;

	[Header("-- Effect on Self for Multi Hit")]
	public StandardEffectInfo m_selfEffectForMultiHit;

	[Header("-- Energy Gain --")]
	public int m_energyGainPerLaserHit;

	[Header("-- For spawning spoils")]
	public SpoilsSpawnData m_spoilSpawnInfo;

	public bool m_onlySpawnSpoilOnMultiHit = true;

	[Header("-- Sequences")]
	public GameObject m_projectileSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	private AbilityMod_TricksterBasicAttack m_abilityMod;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedEnemySingleHitHitEffect;

	private StandardEffectInfo m_cachedEnemyMultiHitEffect;

	private StandardEffectInfo m_cachedSelfEffectForMultiHit;

	private SpoilsSpawnData m_cachedSpoilSpawnInfo;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Trickster Laser";
		}
		Setup();
	}

	private void Setup()
	{
		m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_TricksterLaser(this, m_afterImageSyncComp, GetLaserInfo(), 2);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	public override TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return TargetingParadigm.Position;
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserInfo;
		if ((bool)m_abilityMod)
		{
			cachedLaserInfo = m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo);
		}
		else
		{
			cachedLaserInfo = m_laserInfo;
		}
		m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedEnemySingleHitHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemySingleHitHitEffect = m_abilityMod.m_enemySingleHitHitEffectMod.GetModifiedValue(m_enemySingleHitHitEffect);
		}
		else
		{
			cachedEnemySingleHitHitEffect = m_enemySingleHitHitEffect;
		}
		m_cachedEnemySingleHitHitEffect = cachedEnemySingleHitHitEffect;
		StandardEffectInfo cachedEnemyMultiHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyMultiHitEffect = m_abilityMod.m_enemyMultiHitEffectMod.GetModifiedValue(m_enemyMultiHitEffect);
		}
		else
		{
			cachedEnemyMultiHitEffect = m_enemyMultiHitEffect;
		}
		m_cachedEnemyMultiHitEffect = cachedEnemyMultiHitEffect;
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

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserInfo != null)
		{
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	public int GetLaserDamageAmount()
	{
		return (!m_abilityMod) ? m_laserDamageAmount : m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount);
	}

	public int GetLaserSubsequentDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount);
		}
		else
		{
			result = m_laserSubsequentDamageAmount;
		}
		return result;
	}

	public int GetExtraDamageForSingleHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit);
		}
		else
		{
			result = m_extraDamageForSingleHit;
		}
		return result;
	}

	public StandardEffectInfo GetEnemySingleHitHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemySingleHitHitEffect != null)
		{
			result = m_cachedEnemySingleHitHitEffect;
		}
		else
		{
			result = m_enemySingleHitHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyMultiHitEffect != null)
		{
			result = m_cachedEnemyMultiHitEffect;
		}
		else
		{
			result = m_enemyMultiHitEffect;
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

	public int GetEnergyGainPerLaserHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyGainPerLaserHitMod.GetModifiedValue(m_energyGainPerLaserHit);
		}
		else
		{
			result = m_energyGainPerLaserHit;
		}
		return result;
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
		return ActorMultiHitContext.CalcDamageFromNumHits(numHits, numFromCover, GetLaserDamageAmount(), GetLaserSubsequentDamageAmount());
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetLaserDamageAmount());
		GetEnemySingleHitHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override List<int> _001D()
	{
		List<int> list = base._001D();
		list.Add(GetLaserSubsequentDamageAmount());
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		AbilityUtil_Targeter_TricksterLaser abilityUtil_Targeter_TricksterLaser = base.Targeter as AbilityUtil_Targeter_TricksterLaser;
		if (abilityUtil_Targeter_TricksterLaser != null)
		{
			if (abilityUtil_Targeter_TricksterLaser.m_actorToHitCount.ContainsKey(targetActor))
			{
				int num = abilityUtil_Targeter_TricksterLaser.m_actorToHitCount[targetActor];
				int numFromCover = abilityUtil_Targeter_TricksterLaser.m_actorToCoverCount[targetActor];
				int num2 = CalcDamageFromNumHits(num, numFromCover);
				if (num == 1)
				{
					if (GetExtraDamageForSingleHit() > 0)
					{
						num2 += GetExtraDamageForSingleHit();
					}
				}
				dictionary[AbilityTooltipSymbol.Damage] = num2;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetEnergyGainPerLaserHit() > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					int tooltipSubjectCountTotalWithDuplicates = base.Targeter.GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject.Primary);
					return tooltipSubjectCountTotalWithDuplicates * GetEnergyGainPerLaserHit();
				}
				}
			}
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterBasicAttack abilityMod_TricksterBasicAttack = modAsBase as AbilityMod_TricksterBasicAttack;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_TricksterBasicAttack)
		{
			val = abilityMod_TricksterBasicAttack.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			val = m_laserDamageAmount;
		}
		AddTokenInt(tokens, "LaserDamageAmount", empty, val);
		AddTokenInt(tokens, "LaserSubsequentDamageAmount", string.Empty, (!abilityMod_TricksterBasicAttack) ? m_laserSubsequentDamageAmount : abilityMod_TricksterBasicAttack.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount));
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_TricksterBasicAttack)
		{
			val2 = abilityMod_TricksterBasicAttack.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit);
		}
		else
		{
			val2 = m_extraDamageForSingleHit;
		}
		AddTokenInt(tokens, "ExtraDamageForSingleHit", empty2, val2);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_TricksterBasicAttack)
		{
			effectInfo = abilityMod_TricksterBasicAttack.m_enemySingleHitHitEffectMod.GetModifiedValue(m_enemySingleHitHitEffect);
		}
		else
		{
			effectInfo = m_enemySingleHitHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemySingleHitHitEffect", m_enemySingleHitHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_TricksterBasicAttack) ? m_enemyMultiHitEffect : abilityMod_TricksterBasicAttack.m_enemyMultiHitEffectMod.GetModifiedValue(m_enemyMultiHitEffect), "EnemyMultiHitEffect", m_enemyMultiHitEffect);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_TricksterBasicAttack)
		{
			effectInfo2 = abilityMod_TricksterBasicAttack.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit);
		}
		else
		{
			effectInfo2 = m_selfEffectForMultiHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "SelfEffectForMultiHit", m_selfEffectForMultiHit);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_TricksterBasicAttack)
		{
			val3 = abilityMod_TricksterBasicAttack.m_energyGainPerLaserHitMod.GetModifiedValue(m_energyGainPerLaserHit);
		}
		else
		{
			val3 = m_energyGainPerLaserHit;
		}
		AddTokenInt(tokens, "EnergyGainPerLaserHit", empty3, val3);
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
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterBasicAttack))
		{
			m_abilityMod = (abilityMod as AbilityMod_TricksterBasicAttack);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
