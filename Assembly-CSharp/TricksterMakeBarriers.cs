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
			if (ability != null && ability is TricksterMakeBarriers_Damage)
			{
				m_chainAbility = (ability as TricksterMakeBarriers_Damage);
			}
		}
		base.Targeter = new AbilityUtil_Targeter_TricksterBarriers(this, m_afterImageSyncComp, GetRangeFromLine(), GetLineEndOffset(), GetRadiusAroundOrigin(), GetCapsulePenetrateLos());
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
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		return validAfterImages.Count > 0;
	}

	public float GetRangeFromLine()
	{
		float result;
		if (m_chainAbility == null)
		{
			result = 0f;
		}
		else
		{
			result = m_chainAbility.m_rangeFromLine;
		}
		return result;
	}

	public float GetLineEndOffset()
	{
		return (!(m_chainAbility == null)) ? m_chainAbility.m_lineEndOffset : 0f;
	}

	public float GetRadiusAroundOrigin()
	{
		float result;
		if (m_chainAbility == null)
		{
			result = 0f;
		}
		else
		{
			result = m_chainAbility.m_radiusAroundOrigin;
		}
		return result;
	}

	public bool GetCapsulePenetrateLos()
	{
		int result;
		if (m_chainAbility == null)
		{
			result = 0;
		}
		else
		{
			result = (m_chainAbility.m_capsulePenetrateLos ? 1 : 0);
		}
		return (byte)result != 0;
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
		while (true)
		{
			return;
		}
	}

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
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
						Animator modelAnimator = current.GetModelAnimator();
						modelAnimator.SetInteger("Attack", 0);
						modelAnimator.SetBool("CinematicCam", false);
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}
}
