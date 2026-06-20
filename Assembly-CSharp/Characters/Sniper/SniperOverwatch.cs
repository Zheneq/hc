// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SniperOverwatch : Ability
{
	public GameplayResponseForActor m_onEnemyMoveThrough;
	public bool m_penetrateLos;
	public float m_range = 10f;
	public int m_duration = 1;
	public int m_maxHits = 1;
	public bool m_removeOnTurnEndIfEnemyMovedThrough = true;
	public List<GameObject> m_barrierSequencePrefabs;

	private AbilityMod_SniperOverwatch m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Overwatch";
		}
		Targeter = new AbilityUtil_Targeter_Line(this, m_range, m_penetrateLos);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return m_range;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_onEnemyMoveThrough.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Duration", string.Empty, m_duration);
		m_onEnemyMoveThrough.AddTooltipTokens(tokens, "DroneBarrier", false, null);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SniperOverwatch))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = abilityMod as AbilityMod_SniperOverwatch;
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
	}

	public int GetBarrierDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_durationMod.GetModifiedValue(m_duration)
			: m_duration;
	}

	public int GetEnemyMaxHits()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyMaxHitsMod.GetModifiedValue(m_maxHits)
			: m_maxHits;
	}

	public StandardEffectInfo GetOnMovedThroughEffectInfo()
	{
		return m_abilityMod != null && m_abilityMod.m_useEnemyHitEffectOverride
			? m_abilityMod.m_enemyHitEffectOverride
			: m_onEnemyMoveThrough.m_effect;
	}

	public GameplayResponseForActor GetOnEnemyMovedThroughResponse()
	{
		if (m_abilityMod != null)
		{
			GameplayResponseForActor response = m_onEnemyMoveThrough.GetShallowCopy();
			response.m_effect = GetOnMovedThroughEffectInfo();
			response.m_damage = m_abilityMod.m_damageMod.GetModifiedValue(m_onEnemyMoveThrough.m_damage);
			return response;
		}
		return m_onEnemyMoveThrough;
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			caster.GetFreePos(),
			Quaternion.LookRotation(targets[0].AimDirection),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		Vector3 aimDirection = targets[0].AimDirection;
		float maxDistanceInWorld = m_range * Board.Get().squareSize;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(
			caster.GetLoSCheckPos(), aimDirection, maxDistanceInWorld, m_penetrateLos, caster);
		Vector3 center = (caster.GetFreePos() + laserEndPoint) / 2f;
		center.y = caster.GetFreePos().y;
		Vector3 facingDir = Vector3.Cross(Vector3.up, aimDirection);
		float width = (laserEndPoint - caster.GetLoSCheckPos()).magnitude / Board.Get().squareSize;
		Barrier barrier = new Barrier(
			m_abilityName,
			center,
			facingDir,
			width,
			true,
			BlockingRules.ForNobody,
			BlockingRules.ForNobody,
			BlockingRules.ForNobody,
			BlockingRules.ForNobody,
			GetBarrierDuration(),
			caster,
			m_barrierSequencePrefabs,
			true,
			GetOnEnemyMovedThroughResponse(),
			null,
			GetEnemyMaxHits(),
			false,
			abilityResults.SequenceSource
			)
		{
			m_removeAtTurnEndIfEnemyMovedThrough = m_removeOnTurnEndIfEnemyMovedThrough
		};
		barrier.SetSourceAbility(this);
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(caster.GetFreePos()));
		positionHitResults.AddBarrier(barrier);
		abilityResults.StorePositionHit(positionHitResults);
	}
#endif
}
