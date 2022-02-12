// ROGUES
// SERVER
using System.Collections.Generic;

// server-only missing in reactor
#if SERVER
public class SparkEnergizedEffect : StandardActorEffect
{
	public int m_cinematicsRequested = -1;

	public SparkEnergizedEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, StandardActorEffectData standardActorEffectData, int cinematicsRequested)
		: base(parent, targetSquare, target, caster, standardActorEffectData)
	{
		m_cinematicsRequested = cinematicsRequested;
	}

	public override void OnStart()
	{
		base.OnStart();
		if (Target != null)
		{
			SetEnergizedFlag(true);
		}
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (Target != null)
		{
			SetEnergizedFlag(false);
		}
	}

	private void SetEnergizedFlag(bool energized)
	{
		if (Target == null)
		{
			return;
		}
		if (ServerEffectManager.Get().HasEffectByCaster(Target, Caster, typeof(SparkBasicAttackEffect)))
		{
			List<Effect> effectsOnTargetByCaster = ServerEffectManager.Get().GetEffectsOnTargetByCaster(Target, Caster, typeof(SparkBasicAttackEffect));
			foreach (Effect effect in effectsOnTargetByCaster)
			{
				SparkBasicAttackEffect sparkBasicAttackEffect = effect as SparkBasicAttackEffect;
				sparkBasicAttackEffect.SetEnergized(energized);
				if (energized)
				{
					sparkBasicAttackEffect.SetEnergizedTauntRequested(m_cinematicsRequested);
				}
			}
		}
		if (ServerEffectManager.Get().HasEffectByCaster(Target, Caster, typeof(SparkHealingBeamEffect)))
		{
			List<Effect> effectsOnTargetByCaster = ServerEffectManager.Get().GetEffectsOnTargetByCaster(Target, Caster, typeof(SparkHealingBeamEffect));
			foreach (Effect effect in effectsOnTargetByCaster)
			{
				SparkHealingBeamEffect sparkHealingBeamEffect = effect as SparkHealingBeamEffect;
				sparkHealingBeamEffect.SetEnergized(energized);
				if (energized)
				{
					sparkHealingBeamEffect.SetEnergizedTauntRequested(m_cinematicsRequested);
				}
			}
		}
	}
}
#endif
