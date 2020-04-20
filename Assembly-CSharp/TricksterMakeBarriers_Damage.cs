using System;
using System.Collections.Generic;
using UnityEngine;

public class TricksterMakeBarriers_Damage : Ability
{
	[Header("-- Capsule AOE")]
	public float m_rangeFromLine = 1f;

	public float m_lineEndOffset;

	public float m_radiusAroundOrigin = 1f;

	public bool m_capsulePenetrateLos;

	[Header("-- Enemy Hit Damage and Effect")]
	public int m_damageAmount = 5;

	public StandardEffectInfo m_enemyOnHitEffect;

	[Header("-- Sequences -----------------------------")]
	public GameObject m_castSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Hollusionary Wallogram - Damage";
		}
		this.m_sequencePrefab = this.m_castSequencePrefab;
		this.m_afterImageSyncComp = base.GetComponent<TricksterAfterImageNetworkBehaviour>();
		base.Targeter = new AbilityUtil_Targeter_TricksterBarriers(this, this.m_afterImageSyncComp, this.GetRangeFromLine(), this.GetLineEndOffset(), this.GetRadiusAroundOrigin(), this.GetPenetrateLos(), false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_damageAmount);
		this.m_enemyOnHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		return validAfterImages.Count > 0;
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = this.m_afterImageSyncComp.GetValidAfterImages(true);
		for (int i = 0; i < validAfterImages.Count; i++)
		{
			Animator modelAnimator = validAfterImages[i].GetModelAnimator();
			modelAnimator.SetInteger("Attack", animationIndex);
			modelAnimator.SetBool("CinematicCam", cinecam);
			modelAnimator.SetTrigger("StartAttack");
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

	public float GetRangeFromLine()
	{
		return this.m_rangeFromLine;
	}

	public float GetLineEndOffset()
	{
		return this.m_lineEndOffset;
	}

	public float GetRadiusAroundOrigin()
	{
		return this.m_radiusAroundOrigin;
	}

	public bool GetPenetrateLos()
	{
		return this.m_capsulePenetrateLos;
	}
}
