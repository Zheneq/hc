// ROGUES
// SERVER

using System.Collections.Generic;

#if SERVER
// added in rogues
public class RampartUnstoppableEffect : StandardActorEffect
{
	public RampartUnstoppableEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, StandardActorEffectData baseEffectData)
		: base(parent, targetSquare, target, caster, baseEffectData)
	{
	}

	public override void OnGainingEffect(Effect effect)
	{
		List<StatusType> statuses = GetStatuses();
		if (statuses != null && statuses.Contains(StatusType.Unstoppable))
		{
			List<StatusType> effectStatuses = effect.GetStatuses();
			bool flag = effectStatuses != null
			            && (effectStatuses.Contains(StatusType.Snared)
			                || effectStatuses.Contains(StatusType.Rooted)
			                || effectStatuses.Contains(StatusType.CrippledMovement)
			                || effectStatuses.Contains(StatusType.CantSprint_UnlessUnstoppable));
			if (flag)
			{
				Caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.RampartStats.MovementDebuffsAndKnockbacksPreventedByUnstoppable);
			}
		}
		base.OnGainingEffect(effect);
	}

	public override void OnExecutedActorHitOnTarget(ActorData hitCaster, ActorHitResults results)
	{
		List<StatusType> statuses = GetStatuses();
		if (statuses != null
		    && statuses.Contains(StatusType.Unstoppable)
		    && hitCaster.GetTeam() != Target.GetTeam()
		    && results.HasKnockback)
		{
			Caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.RampartStats.MovementDebuffsAndKnockbacksPreventedByUnstoppable);
		}
		base.OnExecutedActorHitOnTarget(hitCaster, results);
	}
}
#endif
