// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RampartAimShield : Ability
{
	[Header("-- Shield Barrier (Barrier Data specified on Passive)")]
	public bool m_allowAimAtDiagonals;
	[Header("-- Sequences")]
	public GameObject m_removeShieldSequencePrefab;
	public GameObject m_applyShieldSequencePrefab;

	private bool m_snapToGrid = true;
	private Passive_Rampart m_passive;

#if SERVER
	// added in rogues
	private Barrier m_lastGatheredBarrier;
	private Vector3 m_lastGatheredBarrierFacing = Vector3.forward;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Aim Shield";
		}
		Setup();
		ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		if (m_passive == null)
		{
			m_passive = (GetComponent<PassiveData>().GetPassiveOfType(typeof(Passive_Rampart)) as Passive_Rampart);
		}
		float width = m_passive != null ? m_passive.GetShieldBarrierData().m_width : 3f;
		Targeter = new AbilityUtil_Targeter_Barrier(this, width, m_snapToGrid, m_allowAimAtDiagonals, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_passive != null)
		{
			m_passive.GetShieldBarrierData().ReportAbilityTooltipNumbers(ref numbers);
		}
		return numbers;
	}

#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_passive != null)
		{
			m_passive.SetShieldBarrier(m_lastGatheredBarrier, m_lastGatheredBarrierFacing);
		}
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetBarrierPositionAndFacing(targets, out Vector3 targetPos, out Vector3 vector);
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_removeShieldSequencePrefab, Vector3.zero, null, caster, additionalData.m_sequenceSource);
		ServerClientUtils.SequenceStartData item2 = new ServerClientUtils.SequenceStartData(m_applyShieldSequencePrefab, targetPos, Quaternion.LookRotation(vector), null, caster, additionalData.m_sequenceSource);
		list.Add(item);
		list.Add(item2);
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		if (m_passive.GetCurrentShieldBarrier() != null)
		{
			PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(Vector3.zero));
			positionHitResults.AddBarrierForRemoval(m_passive.GetCurrentShieldBarrier(), true);
			abilityResults.StorePositionHit(positionHitResults);
		}

		GetBarrierPositionAndFacing(targets, out Vector3 vector, out Vector3 vector2);
		Barrier barrier = new Barrier(m_abilityName, vector, vector2, caster, m_passive.GetShieldBarrierData(), true, abilityResults.SequenceSource);
		barrier.SetSourceAbility(this);
		PositionHitResults positionHitResults2 = new PositionHitResults(new PositionHitParameters(vector));
		positionHitResults2.AddBarrier(barrier);
		if (ServerAbilityUtils.CurrentlyGatheringRealResults())
		{
			m_lastGatheredBarrier = barrier;
			m_lastGatheredBarrierFacing = vector2;
		}
		abilityResults.StorePositionHit(positionHitResults2);
	}

	// added in rogues
	private void GetBarrierPositionAndFacing(List<AbilityTarget> targets, out Vector3 position, out Vector3 facing)
	{
		facing = targets[0].AimDirection;
		position = targets[0].FreePos;
		if (m_snapToGrid)
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			if (square != null)
			{
				facing = VectorUtils.GetDirectionAndOffsetToClosestSide(square, targets[0].FreePos, m_allowAimAtDiagonals, out Vector3 vector);
				position = square.ToVector3() + vector;
			}
		}
	}
#endif
}
