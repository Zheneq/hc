using System.Collections.Generic;
using UnityEngine;

public class TricksterMakeBarriers : Ability
{
	[Header("-- Barrier Info")]
	public bool m_linkBarriers = true;
	public StandardBarrierData m_barrierData;
	[Header("-- Spoils Spawn on Ally Moved Through")]
	public SpoilsSpawnData m_spoilsSpawnOnEnemyMovedThrough;
	[Header("-- Spoils Spawn on Enemy Moved Through")]
	public SpoilsSpawnData m_spoilsSpawnOnAllyMovedThrough;
	[Header("-- Sequences -----------------------------")]
	public GameObject m_castSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;
	private TricksterMakeBarriers_Damage m_chainAbility;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Hollusionary Wallogram";
		}
		m_sequencePrefab = m_castSequencePrefab;
		m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		Ability[] chainAbilities = GetChainAbilities();
		if (chainAbilities.Length > 0)
		{
			Ability ability = chainAbilities[0];
			TricksterMakeBarriers_Damage tricksterMakeBarriersDamage = ability as TricksterMakeBarriers_Damage;
			if (ability != null && !ReferenceEquals(tricksterMakeBarriersDamage, null))
			{
				m_chainAbility = tricksterMakeBarriersDamage;
			}
		}
		Targeter = new AbilityUtil_Targeter_TricksterBarriers(
			this,
			m_afterImageSyncComp,
			GetRangeFromLine(),
			GetLineEndOffset(),
			GetRadiusAroundOrigin(),
			GetCapsulePenetrateLos());
		ResetTooltipAndTargetingNumbers();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_chainAbility != null)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_chainAbility.m_damageAmount);
			m_chainAbility.m_enemyOnHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		}
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_afterImageSyncComp.GetValidAfterImages().Count > 0;
	}

	public float GetRangeFromLine()
	{
		return m_chainAbility != null
			? m_chainAbility.m_rangeFromLine
			: 0f;
	}

	public float GetLineEndOffset()
	{
		return m_chainAbility != null
			? m_chainAbility.m_lineEndOffset
			: 0f;
	}

	public float GetRadiusAroundOrigin()
	{
		return m_chainAbility != null
			? m_chainAbility.m_radiusAroundOrigin
			: 0f;
	}

	public bool GetCapsulePenetrateLos()
	{
		return m_chainAbility != null && m_chainAbility.m_capsulePenetrateLos;
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
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
