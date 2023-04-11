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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Hollusionary Wallogram - Damage";
		}
		m_sequencePrefab = m_castSequencePrefab;
		m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		Targeter = new AbilityUtil_Targeter_TricksterBarriers(
			this,
			m_afterImageSyncComp,
			GetRangeFromLine(),
			GetLineEndOffset(),
			GetRadiusAroundOrigin(),
			GetPenetrateLos(),
			false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		m_enemyOnHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_afterImageSyncComp.GetValidAfterImages().Count > 0;
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

	public float GetRangeFromLine()
	{
		return m_rangeFromLine;
	}

	public float GetLineEndOffset()
	{
		return m_lineEndOffset;
	}

	public float GetRadiusAroundOrigin()
	{
		return m_radiusAroundOrigin;
	}

	public bool GetPenetrateLos()
	{
		return m_capsulePenetrateLos;
	}
}
