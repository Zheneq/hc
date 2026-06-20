// ROGUES
// SERVER

using UnityEngine;

#if SERVER
// added in rogues
public class SenseiDelayedHealAoeEffect : DelayedPbAoeEffect
{
	private SenseiHealAoE m_healAoeAbility;

	public SenseiDelayedHealAoeEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		int duration,
		int hitStartDelay,
		int damage,
		StandardEffectInfo enemyHitEffect,
		int allyHeal,
		StandardEffectInfo allyHitEffect,
		int selfHeal,
		StandardEffectInfo selfHitEffect,
		bool ignoreSourceEnergyGains,
		int energyGainPerEnemyHit,
		int energyGainPerAllyHit,
		int energyGainOnSelfHit,
		float radiusInSquares,
		bool penetrateLos,
		int animIndex,
		GameObject persistentSequencePrefab,
		GameObject triggerSequencePrefab)
		: base(
			parent,
			targetSquare,
			target,
			caster,
			duration,
			hitStartDelay,
			damage,
			enemyHitEffect,
			allyHeal,
			allyHitEffect,
			selfHeal,
			selfHitEffect,
			ignoreSourceEnergyGains,
			energyGainPerEnemyHit,
			energyGainPerAllyHit,
			energyGainOnSelfHit,
			radiusInSquares,
			penetrateLos,
			animIndex,
			persistentSequencePrefab,
			triggerSequencePrefab)
	{
		if (parent.Ability != null && parent.Ability is SenseiHealAoE healAoeAbility)
		{
			m_healAoeAbility = healAoeAbility;
		}
	}

	public override int GetAllyHeal()
	{
		int num = m_allyHealAmount;
		if (m_healAoeAbility != null)
		{
			num += m_healAoeAbility.CalcExtraHealFromBide();
		}
		return num;
	}

	public override int GetSelfHeal()
	{
		int num = m_selfHealAmount;
		if (m_healAoeAbility != null)
		{
			num += m_healAoeAbility.CalcExtraHealFromBide();
		}
		return num;
	}
}
#endif
