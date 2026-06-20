// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RampartBarricade_Prep : Ability
{
	[Header("-- Barrier Aiming")]
	public bool m_allowAimAtDiagonals;
	[Header("-- Knockback Hit Damage and Effect")]
	public int m_damageAmount = 10;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Laser and Knockback")]
	public float m_laserRange = 2f;
	public bool m_laserLengthIgnoreLos = true;
	public bool m_penetrateLos;
	public float m_knockbackDistance = 2f;
	public KnockbackType m_knockbackType;
	[Header("-- Sequences")]
	public GameObject m_removeShieldSequencePrefab;
	public GameObject m_applyShieldSequencePrefab;

	private bool m_snapToGrid = true;
	private Passive_Rampart m_passive;
	private AbilityMod_RampartBarricade_Prep m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffect;

#if SERVER
	private Barrier m_lastGatheredBarrier;
	private Vector3 m_lastGatheredBarrierFacing = Vector3.forward;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Barricade";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_passive == null)
		{
			m_passive = GetComponent<PassiveData>().GetPassiveOfType(typeof(Passive_Rampart)) as Passive_Rampart;
		}
		if (m_passive != null)
		{
			m_passive.SetCachedShieldBarrierData(m_abilityMod != null ? m_abilityMod.m_shieldBarrierDataMod : null);
		}
		else
		{
			Log.Error(GetDebugIdentifier("") + " did not find passive component Passive_Rampart");
		}
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_RampartKnockbackBarrier(this, GetLaserWidth(), GetLaserRange(), LaserLengthIgnoreLos(), GetKnockbackDistance(), m_knockbackType, PenetrateLos(), m_snapToGrid, AllowAimAtDiagonals());
		Targeter.SetAffectedGroups(true, false, AffectCaster());
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
	}

	public bool AllowAimAtDiagonals()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allowAimAtDiagonalsMod.GetModifiedValue(m_allowAimAtDiagonals)
			: m_allowAimAtDiagonals;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange)
			: m_laserRange;
	}

	public bool LaserLengthIgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserLengthIgnoreLosMod.GetModifiedValue(m_laserLengthIgnoreLos)
			: m_laserLengthIgnoreLos;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public float GetKnockbackDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance)
			: m_knockbackDistance;
	}

	public float GetLaserWidth()
	{
		return m_passive.GetShieldBarrierData().m_width;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_knockbackType;
	}

	public bool AffectCaster()
	{
		return m_abilityMod != null && m_abilityMod.m_effectToSelfOnCast.m_applyEffect;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AppendTooltipNumbersFromBaseModEffects(ref numbers);
		return numbers;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartBarricade_Prep))
		{
			m_abilityMod = abilityMod as AbilityMod_RampartBarricade_Prep;
		}
		Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartBarricade_Prep mod = modAsBase as AbilityMod_RampartBarricade_Prep;
		AddTokenInt(tokens, "DamageAmount", "", mod != null
			? mod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, mod != null
			? mod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		Passive_Rampart passive = GetComponent<Passive_Rampart>();
		if (passive != null && passive.m_normalShieldBarrierData != null)
		{
			passive.m_normalShieldBarrierData.AddTooltipTokens(tokens, "ShieldBarrier");
		}
	}

	public override Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		GetBarrierPositionAndFacing(targets, out Vector3 position, out Vector3 facing);
		return position + facing;
	}

	public void GetBarrierPositionAndFacing(List<AbilityTarget> targets, out Vector3 position, out Vector3 facing)
	{
		facing = targets[0].AimDirection;
		position = targets[0].FreePos;
		if (m_snapToGrid)
		{
			BoardSquare targetPos = Board.Get().GetSquare(targets[0].GridPos);
			if (targetPos != null)
			{
				facing = VectorUtils.GetDirectionAndOffsetToClosestSide(targetPos, targets[0].FreePos, AllowAimAtDiagonals(), out Vector3 offset);
				position = targetPos.ToVector3() + offset;
			}
		}
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
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		return null;
	}

	// added in rogues
	public override bool UseTargeterGridPosForCameraBounds()
	{
		return false;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster == target)
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.RampartStats.HitsBlockedByShield);
		}
	}
#endif
}
