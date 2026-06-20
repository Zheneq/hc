// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// added in rogues
#if SERVER
public class MartyrHealOverTimeEffect : StandardActorEffect
{
	private Martyr_SyncComponent m_syncComp;
	private int m_baseHealPerTurn;
	private int m_extraHealPerCrystal;
	private int m_extraHealIfHasAoeOnReact;
	private StandardEffectInfo m_extraEffectOnLowHealth;
	private float m_lowHealthThreshold;

	public MartyrHealOverTimeEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, StandardActorEffectData data, Martyr_SyncComponent syncComp, int baseHealPerTurn, int extraHealPerCrystal, int extraHealIfHasAoeOnReact, StandardEffectInfo extraEffectOnLowHealth, float lowHealthThreshold)
		: base(parent, targetSquare, target, caster, data)
	{
		m_syncComp = syncComp;
		m_baseHealPerTurn = baseHealPerTurn;
		m_extraHealPerCrystal = extraHealPerCrystal;
		m_extraHealIfHasAoeOnReact = extraHealIfHasAoeOnReact;
		m_extraEffectOnLowHealth = extraEffectOnLowHealth;
		m_lowHealthThreshold = lowHealthThreshold;
	}

	public override int GetHealingPerTurn()
	{
		int healPerTurn = 0;
		if (Target.GetTeam() == Caster.GetTeam())
		{
			healPerTurn = m_baseHealPerTurn;
			if (m_syncComp != null)
			{
				if (m_extraHealIfHasAoeOnReact > 0 && m_syncComp.ActorHasAoeOnReactEffect(Target))
				{
					healPerTurn += m_extraHealIfHasAoeOnReact;
				}
				int spentDamageCrystals = m_syncComp.SpentDamageCrystals(Caster);
				if (m_extraHealPerCrystal > 0 && spentDamageCrystals > 0)
				{
					healPerTurn += spentDamageCrystals * m_extraHealPerCrystal;
				}
			}
		}
		return healPerTurn;
	}

	public override int GetExpectedHealOverTimeTotal()
	{
		int result = 0;
		if (m_time.duration > 0)
		{
			int turnsPassed = Mathf.Max(m_time.age, m_perTurnHitDelay);
			turnsPassed = Mathf.Max(1, turnsPassed);
			int perTurnBonusNum = m_time.duration - turnsPassed;
			GetExpectedHealValues(out int expectedPerTurn, out int extraThisTurn);
			result = Mathf.Max(0, expectedPerTurn * perTurnBonusNum + extraThisTurn);
		}
		return result;
	}

	public override int GetExpectedHealOverTimeThisTurn()
	{
		if (m_time.duration > 0 && m_time.age >= m_perTurnHitDelay)
		{
			GetExpectedHealValues(out int expectedPerTurn, out int extraThisTurn);
			return Mathf.Max(0, expectedPerTurn + extraThisTurn);
		}
		return 0;
	}

	private void GetExpectedHealValues(out int expectedPerTurn, out int extraThisTurn)
	{
		expectedPerTurn = 0;
		extraThisTurn = 0;
		int expected = m_baseHealPerTurn;
		int extra = 0;
		if (m_syncComp != null)
		{
			int spentDamageCrystals = m_syncComp.SpentDamageCrystals(Caster);
			if (m_extraHealPerCrystal > 0 && spentDamageCrystals > 0)
			{
				expected += spentDamageCrystals * m_extraHealPerCrystal;
			}
			if (m_extraHealIfHasAoeOnReact > 0
				&& m_time.age > 0
				&& GameFlowData.Get() != null
				&& GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
			{
				List<AbilityTarget> targetingDataOfStoredAbility = ServerActionBuffer.Get().GetTargetingDataOfStoredAbility(Caster, typeof(MartyrAoeOnReactHit));
				if (targetingDataOfStoredAbility != null && targetingDataOfStoredAbility.Count > 0)
				{
					BoardSquare square = Board.Get().GetSquare(targetingDataOfStoredAbility[0].GridPos);
					if (square != null && Target.GetCurrentBoardSquare() == square)
					{
						extra += m_extraHealIfHasAoeOnReact;
					}
				}
			}
		}
		expectedPerTurn = expected;
		extraThisTurn = extra;
	}

	public override bool CanApplyHealOverTime()
	{
		return m_baseHealPerTurn > 0 || m_extraHealPerCrystal > 0;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		ActorHitResults actorHitResults = BuildMainTargetHitResults();
		if (m_lowHealthThreshold > 0f && m_extraEffectOnLowHealth.m_applyEffect && Target.GetHitPointPercent() <= m_lowHealthThreshold)
		{
			actorHitResults.AddStandardEffectInfo(m_extraEffectOnLowHealth);
		}
		effectResults.StoreActorHit(actorHitResults);
	}
}
#endif
