// ROGUES
// SERVER

#if SERVER
// added in rogues
public class SamuraiMarkEffect : StandardActorEffect
{
	public SamuraiMarkEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, StandardActorEffectData data)
		: base(parent, targetSquare, target, caster, data)
	{
	}

	public SamuraiMarkEffect(ActorHitParameters hitParams, StandardActorEffectData data) : base(hitParams, data)
	{
	}
}
#endif
