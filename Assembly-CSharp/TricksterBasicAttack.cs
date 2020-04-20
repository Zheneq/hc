using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Trickster Laser";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_afterImageSyncComp = base.GetComponent<TricksterAfterImageNetworkBehaviour>();
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_TricksterLaser(this, this.m_afterImageSyncComp, this.GetLaserInfo(), 2);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserInfo().range;
	}

	public override Ability.TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return Ability.TargetingParadigm.Position;
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserInfo;
		if (this.m_abilityMod)
		{
			cachedLaserInfo = this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo);
		}
		else
		{
			cachedLaserInfo = this.m_laserInfo;
		}
		this.m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedEnemySingleHitHitEffect;
		if (this.m_abilityMod)
		{
			cachedEnemySingleHitHitEffect = this.m_abilityMod.m_enemySingleHitHitEffectMod.GetModifiedValue(this.m_enemySingleHitHitEffect);
		}
		else
		{
			cachedEnemySingleHitHitEffect = this.m_enemySingleHitHitEffect;
		}
		this.m_cachedEnemySingleHitHitEffect = cachedEnemySingleHitHitEffect;
		StandardEffectInfo cachedEnemyMultiHitEffect;
		if (this.m_abilityMod)
		{
			cachedEnemyMultiHitEffect = this.m_abilityMod.m_enemyMultiHitEffectMod.GetModifiedValue(this.m_enemyMultiHitEffect);
		}
		else
		{
			cachedEnemyMultiHitEffect = this.m_enemyMultiHitEffect;
		}
		this.m_cachedEnemyMultiHitEffect = cachedEnemyMultiHitEffect;
		StandardEffectInfo cachedSelfEffectForMultiHit;
		if (this.m_abilityMod)
		{
			cachedSelfEffectForMultiHit = this.m_abilityMod.m_selfEffectForMultiHitMod.GetModifiedValue(this.m_selfEffectForMultiHit);
		}
		else
		{
			cachedSelfEffectForMultiHit = this.m_selfEffectForMultiHit;
		}
		this.m_cachedSelfEffectForMultiHit = cachedSelfEffectForMultiHit;
		SpoilsSpawnData cachedSpoilSpawnInfo;
		if (this.m_abilityMod)
		{
			cachedSpoilSpawnInfo = this.m_abilityMod.m_spoilSpawnInfoMod.GetModifiedValue(this.m_spoilSpawnInfo);
		}
		else
		{
			cachedSpoilSpawnInfo = this.m_spoilSpawnInfo;
		}
		this.m_cachedSpoilSpawnInfo = cachedSpoilSpawnInfo;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
		{
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	public int GetLaserDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_laserDamageAmount : this.m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount);
	}

	public int GetLaserSubsequentDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserSubsequentDamageAmountMod.GetModifiedValue(this.m_laserSubsequentDamageAmount);
		}
		else
		{
			result = this.m_laserSubsequentDamageAmount;
		}
		return result;
	}

	public int GetExtraDamageForSingleHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(this.m_extraDamageForSingleHit);
		}
		else
		{
			result = this.m_extraDamageForSingleHit;
		}
		return result;
	}

	public StandardEffectInfo GetEnemySingleHitHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemySingleHitHitEffect != null)
		{
			result = this.m_cachedEnemySingleHitHitEffect;
		}
		else
		{
			result = this.m_enemySingleHitHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyMultiHitEffect != null)
		{
			result = this.m_cachedEnemyMultiHitEffect;
		}
		else
		{
			result = this.m_enemyMultiHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetSelfEffectForMultiHit()
	{
		StandardEffectInfo result;
		if (this.m_cachedSelfEffectForMultiHit != null)
		{
			result = this.m_cachedSelfEffectForMultiHit;
		}
		else
		{
			result = this.m_selfEffectForMultiHit;
		}
		return result;
	}

	public int GetEnergyGainPerLaserHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_energyGainPerLaserHitMod.GetModifiedValue(this.m_energyGainPerLaserHit);
		}
		else
		{
			result = this.m_energyGainPerLaserHit;
		}
		return result;
	}

	public SpoilsSpawnData GetSpoilSpawnInfo()
	{
		SpoilsSpawnData result;
		if (this.m_cachedSpoilSpawnInfo != null)
		{
			result = this.m_cachedSpoilSpawnInfo;
		}
		else
		{
			result = this.m_spoilSpawnInfo;
		}
		return result;
	}

	public bool OnlySpawnSpoilOnMultiHit()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_onlySpawnSpoilOnMultiHitMod.GetModifiedValue(this.m_onlySpawnSpoilOnMultiHit);
		}
		else
		{
			result = this.m_onlySpawnSpoilOnMultiHit;
		}
		return result;
	}

	private int CalcDamageFromNumHits(int numHits, int numFromCover)
	{
		return ActorMultiHitContext.CalcDamageFromNumHits(numHits, numFromCover, this.GetLaserDamageAmount(), this.GetLaserSubsequentDamageAmount());
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetLaserDamageAmount());
		this.GetEnemySingleHitHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override List<int> symbol_001D()
	{
		List<int> list = base.symbol_001D();
		list.Add(this.GetLaserSubsequentDamageAmount());
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
				int num2 = this.CalcDamageFromNumHits(num, numFromCover);
				if (num == 1)
				{
					if (this.GetExtraDamageForSingleHit() > 0)
					{
						num2 += this.GetExtraDamageForSingleHit();
					}
				}
				dictionary[AbilityTooltipSymbol.Damage] = num2;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.GetEnergyGainPerLaserHit() > 0)
		{
			int tooltipSubjectCountTotalWithDuplicates = base.Targeter.GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject.Primary);
			return tooltipSubjectCountTotalWithDuplicates * this.GetEnergyGainPerLaserHit();
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterBasicAttack abilityMod_TricksterBasicAttack = modAsBase as AbilityMod_TricksterBasicAttack;
		string name = "LaserDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_TricksterBasicAttack)
		{
			val = abilityMod_TricksterBasicAttack.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			val = this.m_laserDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "LaserSubsequentDamageAmount", string.Empty, (!abilityMod_TricksterBasicAttack) ? this.m_laserSubsequentDamageAmount : abilityMod_TricksterBasicAttack.m_laserSubsequentDamageAmountMod.GetModifiedValue(this.m_laserSubsequentDamageAmount), false);
		string name2 = "ExtraDamageForSingleHit";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_TricksterBasicAttack)
		{
			val2 = abilityMod_TricksterBasicAttack.m_extraDamageForSingleHitMod.GetModifiedValue(this.m_extraDamageForSingleHit);
		}
		else
		{
			val2 = this.m_extraDamageForSingleHit;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_TricksterBasicAttack)
		{
			effectInfo = abilityMod_TricksterBasicAttack.m_enemySingleHitHitEffectMod.GetModifiedValue(this.m_enemySingleHitHitEffect);
		}
		else
		{
			effectInfo = this.m_enemySingleHitHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemySingleHitHitEffect", this.m_enemySingleHitHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_TricksterBasicAttack) ? this.m_enemyMultiHitEffect : abilityMod_TricksterBasicAttack.m_enemyMultiHitEffectMod.GetModifiedValue(this.m_enemyMultiHitEffect), "EnemyMultiHitEffect", this.m_enemyMultiHitEffect, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_TricksterBasicAttack)
		{
			effectInfo2 = abilityMod_TricksterBasicAttack.m_selfEffectForMultiHitMod.GetModifiedValue(this.m_selfEffectForMultiHit);
		}
		else
		{
			effectInfo2 = this.m_selfEffectForMultiHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "SelfEffectForMultiHit", this.m_selfEffectForMultiHit, true);
		string name3 = "EnergyGainPerLaserHit";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_TricksterBasicAttack)
		{
			val3 = abilityMod_TricksterBasicAttack.m_energyGainPerLaserHitMod.GetModifiedValue(this.m_energyGainPerLaserHit);
		}
		else
		{
			val3 = this.m_energyGainPerLaserHit;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		using (List<ActorData>.Enumerator enumerator = validAfterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null)
				{
					if (!actorData.IsDead())
					{
						this.m_afterImageSyncComp.TurnToPosition(actorData, targetPos);
						Animator modelAnimator = actorData.GetModelAnimator();
						modelAnimator.SetInteger("Attack", animationIndex);
						modelAnimator.SetBool("CinematicCam", cinecam);
						modelAnimator.SetTrigger("StartAttack");
					}
				}
			}
		}
	}

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		foreach (ActorData actorData in validAfterImages)
		{
			if (actorData != null)
			{
				if (!actorData.IsDead())
				{
					Animator modelAnimator = actorData.GetModelAnimator();
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
			this.m_abilityMod = (abilityMod as AbilityMod_TricksterBasicAttack);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
