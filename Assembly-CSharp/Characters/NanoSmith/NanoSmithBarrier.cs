// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

// empty in rogues
public class NanoSmithBarrier : Ability
{
	[Header("-- Barrier Info")]
	public bool m_snapToGrid = true;
	public StandardBarrierData m_barrierData;
	[Separator("Sequences")]
	public GameObject m_castSeqPrefab;
	[TextArea(1, 10)]
	public string m_notes;

	private AbilityMod_NanoSmithBarrier m_abilityMod;
	private StandardBarrierData m_cachedBarrierData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Kinetic Barrier";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		StandardBarrierData barrierData = GetBarrierData();
		float width = barrierData.m_width;
		if (GetExpectedNumberOfTargeters() < 2)
		{
			Targeter = new AbilityUtil_Targeter_Barrier(this, barrierData.m_width, m_snapToGrid);
			return;
		}
		ClearTargeters();
		Targeters.Add(new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false));
		AbilityUtil_Targeter_Barrier targeter = new AbilityUtil_Targeter_Barrier(this, width, m_snapToGrid, false, false);
		targeter.SetUseMultiTargetUpdate(true);
		Targeters.Add(targeter);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_barrierData.ReportAbilityTooltipNumbers(ref numbers);
		return numbers;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, GetNumTargets());
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (GetExpectedNumberOfTargeters() > 1 && targetIndex > 0)
		{
			return Board.Get().GetSquare(currentTargets[0].GridPos) == Board.Get().GetSquare(target.GridPos);
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithBarrier abilityMod_NanoSmithBarrier = modAsBase as AbilityMod_NanoSmithBarrier;
		StandardBarrierData barrierData = abilityMod_NanoSmithBarrier != null
			? abilityMod_NanoSmithBarrier.m_barrierDataMod.GetModifiedValue(m_barrierData)
			: m_barrierData;
		barrierData.AddTooltipTokens(tokens, "BarrierData", abilityMod_NanoSmithBarrier != null, m_barrierData);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithBarrier))
		{
			m_abilityMod = abilityMod as AbilityMod_NanoSmithBarrier;
			m_cachedBarrierData = m_abilityMod.m_barrierDataMod.GetModifiedValue(m_barrierData);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		m_cachedBarrierData = null;
		SetupTargeter();
	}

	private StandardBarrierData GetBarrierData()
	{
		return m_abilityMod != null ? m_cachedBarrierData : m_barrierData;
	}
	
#if SERVER
	// custom
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		GetBarrierLocation(targets, out Vector3 center, out Vector3 aimDirection);
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			center,
			Quaternion.LookRotation(aimDirection),
			null,
			caster,
			additionalData.m_sequenceSource);
	}
	
	// custom
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		GetBarrierLocation(targets, out Vector3 pos, out Vector3 aimDirection);
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(pos));
		StandardBarrierData barrierData = GetBarrierData();
		Barrier barrier = new Barrier(m_abilityName, pos, aimDirection, caster, barrierData);
		barrier.SetSourceAbility(this);
		positionHitResults.AddBarrier(barrier);
		abilityResults.StorePositionHit(positionHitResults);
	}
	
	// custom
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.NanoSmithStats.BarrierHits);
		}
	}
	
	// custom
	private void GetBarrierLocation(List<AbilityTarget> targets, out Vector3 center, out Vector3 aimDirection)
	{
		aimDirection = Vector3.forward;
		center = targets[0].FreePos;
		if (m_snapToGrid)
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			if (square != null)
			{
				center = square.ToVector3();
				Vector3 freePos = targets[1].FreePos;
				aimDirection = VectorUtils.GetDirectionAndOffsetToClosestSide(square, freePos, false, out Vector3 offset);
				center += offset;
			}
		}
	}
#endif
}
