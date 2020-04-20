using System;
using System.Collections.Generic;
using UnityEngine;

public class TricksterFlare : Ability
{
	[Header("-- Targeting ")]
	public AbilityAreaShape m_flareShape = AbilityAreaShape.Three_x_Three;

	public bool m_flarePenetrateLos;

	public bool m_flareAroundSelf = true;

	[Header("-- Enemy Hit")]
	public bool m_includeEnemies = true;

	public int m_flareDamageAmount = 3;

	public int m_flareSubsequentDamageAmount = 2;

	public StandardEffectInfo m_enemyHitEffect;

	[Space(10f)]
	public bool m_useEnemyMultiHitEffect;

	public StandardEffectInfo m_enemyMultipleHitEffect;

	[Header("-- Ally Hit")]
	public bool m_includeAllies;

	public int m_flareHealAmount;

	public int m_flareSubsequentHealAmount;

	public StandardEffectInfo m_allyHitEffect;

	[Space(10f)]
	public bool m_useAllyMultiHitEffect;

	public StandardEffectInfo m_allyMultipleHitEffect;

	[Header("-- Self Hit")]
	public StandardEffectInfo m_selfHitEffectForMultiHit;

	[Header("-- Spoil spawn info")]
	public bool m_spawnSpoilForEnemyHit = true;

	public bool m_spawnSpoilForAllyHit;

	public SpoilsSpawnData m_spoilSpawnInfo;

	public bool m_onlySpawnSpoilOnMultiHit = true;

	[Header("-- Sequences ----------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Flare";
		}
		this.m_afterImageSyncComp = base.GetComponent<TricksterAfterImageNetworkBehaviour>();
		this.m_sequencePrefab = this.m_castSequencePrefab;
		base.Targeter = new AbilityUtil_Targeter_TricksterFlare(this, this.m_afterImageSyncComp, this.m_flareShape, this.m_flarePenetrateLos, this.m_includeEnemies, this.m_includeAllies, this.m_flareAroundSelf);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_flareDamageAmount);
		this.m_enemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Secondary, this.m_flareHealAmount);
		this.m_allyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		if (this.m_flareSubsequentDamageAmount > 0)
		{
			if (this.m_flareSubsequentDamageAmount != this.m_flareDamageAmount)
			{
				AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Tertiary, this.m_flareSubsequentDamageAmount);
			}
		}
		if (this.m_flareSubsequentHealAmount > 0)
		{
			if (this.m_flareSubsequentHealAmount != this.m_flareHealAmount)
			{
				AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Quaternary, this.m_flareSubsequentHealAmount);
			}
		}
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (!this.m_flareAroundSelf)
		{
			if (this.m_afterImageSyncComp != null)
			{
				return this.m_afterImageSyncComp.HasVaidAfterImages();
			}
		}
		return true;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		if (this.m_includeEnemies)
		{
			Ability.AddNameplateValueForOverlap(ref result, base.Targeter, targetActor, currentTargeterIndex, this.m_flareDamageAmount, this.m_flareSubsequentDamageAmount, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		}
		if (this.m_includeAllies)
		{
			Ability.AddNameplateValueForOverlap(ref result, base.Targeter, targetActor, currentTargeterIndex, this.m_flareHealAmount, this.m_flareSubsequentHealAmount, AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Secondary);
		}
		return result;
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		foreach (ActorData actorData in validAfterImages)
		{
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

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		foreach (ActorData actorData in validAfterImages)
		{
			if (actorData != null && !actorData.IsDead())
			{
				Animator modelAnimator = actorData.GetModelAnimator();
				modelAnimator.SetInteger("Attack", 0);
				modelAnimator.SetBool("CinematicCam", false);
			}
		}
	}
}
