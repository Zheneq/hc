// ROGUES
// SERVER
using UnityEngine;

// added in rogues
#if SERVER
public class PositionHitParameters
{
	public DamageSource DamageSource;

	public PositionHitParameters(Vector3 pos)
	{
		Pos = pos;
		Caster = null;
		Ability = null;
		Effect = null;
		Barrier = null;
		DamageSource = null;
	}

	public ActorData Caster { get; set; }
	public Ability Ability { get; set; }
	public Effect Effect { get; set; }
	public Barrier Barrier { get; set; }
	public Vector3 Pos { get; set; }

	public Ability GetRelevantAbility()
	{
		if (Ability != null)
		{
			return Ability;
		}
		if (Effect != null && Effect.Parent.IsAbility())
		{
			return this.Effect.Parent.Ability;
		}
		if (Barrier != null && Barrier.GetSourceAbility() != null)
		{
			return Barrier.GetSourceAbility();
		}
		return null;
	}
}
#endif
