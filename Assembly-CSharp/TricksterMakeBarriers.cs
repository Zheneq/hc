// ROGUES
// SERVER
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
			if (ability != null && ability is TricksterMakeBarriers_Damage tricksterMakeBarriersDamage)
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
#if SERVER
			// added in rogues
			m_afterImageSyncComp.TurnToPosition(
				i == 0 ? caster : validAfterImages[i - 1],
				validAfterImages[i].GetFreePos());
#endif
			Animator modelAnimator = validAfterImages[i].GetModelAnimator();
			modelAnimator.SetInteger("Attack", animationIndex);
			modelAnimator.SetBool("CinematicCam", cinecam);
			modelAnimator.SetTrigger("StartAttack");
		}
#if SERVER
		// added in rogues
		if (validAfterImages.Count > 0)
		{
			m_afterImageSyncComp.TurnToPosition(validAfterImages[validAfterImages.Count - 1], caster.GetFreePos());
		}
#endif
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
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(m_castSequencePrefab,
			targets[0].FreePos,
			null,
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(targets[0].FreePos));
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		float squareSize = Board.Get().squareSize;
		List<Barrier> list = new List<Barrier>();
		int num = validAfterImages.Count > 1
			? validAfterImages.Count
			: validAfterImages.Count - 1;
		for (int i = 0; i <= num; i++)
		{
			BoardSquare src = null;
			BoardSquare dst = null;
			if (i == 0)
			{
				src = caster.GetCurrentBoardSquare();
				dst = validAfterImages[i].GetCurrentBoardSquare();
			}
			else if (i == validAfterImages.Count)
			{
				src = validAfterImages[i - 1].GetCurrentBoardSquare();
				dst = caster.GetCurrentBoardSquare();
			}
			else if (i < validAfterImages.Count)
			{
				src = validAfterImages[i - 1].GetCurrentBoardSquare();
				dst = validAfterImages[i].GetCurrentBoardSquare();
			}
			Vector3 vector = dst.ToVector3() - src.ToVector3();
			vector.y = 0f;
			float magnitude = vector.magnitude;
			vector.Normalize();
			Vector3 center = src.ToVector3() + 0.5f * magnitude * vector;
			center.y = Board.Get().BaselineHeight;
			Vector3 facingDir = Vector3.Cross(vector, Vector3.up);
			m_barrierData.m_width = magnitude / squareSize + 0.05f;
			Barrier barrier = new Barrier(m_abilityName, center, facingDir, caster, m_barrierData);
			barrier.SetSourceAbility(this);
			barrier.m_spoilsSpawnOnEnemyMovedThrough = m_spoilsSpawnOnEnemyMovedThrough;
			barrier.m_spoilsSpawnOnAllyMovedThrough = m_spoilsSpawnOnAllyMovedThrough;
			positionHitResults.AddBarrier(barrier);
			list.Add(barrier);
		}
		if (m_linkBarriers)
		{
			BarrierManager.Get().LinkBarriers(list, new LinkedBarrierData());
		}
		abilityResults.StorePositionHit(positionHitResults);
	}
#endif
}
