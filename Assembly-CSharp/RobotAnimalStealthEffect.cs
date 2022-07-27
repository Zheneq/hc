// ROGUES
// SERVER
#if SERVER
public class RobotAnimalStealthEffect : StandardActorEffect
{
	public RobotAnimalStealthEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data)
		: base(parent, targetSquare, target, caster, data)
	{
	}
}
#endif
