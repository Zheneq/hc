// ROGUES
// SERVER
//using EffectSystem;

using Newtonsoft.Json;
using UnityEngine;

// server-only
#if SERVER
public class ActorHitParameters
{
	public DamageSource DamageSource;

	public ActorHitParameters(ActorData target, Vector3 origin)
	{
		Target = target;
		Origin = origin;
		Caster = null;
		AbilityResults = null;
		Effect = null;
		Barrier = null;
		DamageSource = null;
	}

	public ActorHitParameters(ServerAbilityUtils.TriggeringPathInfo path)
	{
		Target = path.m_mover;
		Origin = path.m_triggeringPathSegment.square.ToVector3();
		Caster = null;
		AbilityResults = null;
		Effect = null;
		Barrier = null;
		DamageSource = null;
	}

	public ActorData Target { get; set; }

	public ActorData Caster { get; set; }

	[JsonIgnore]
	public AbilityResults AbilityResults { get; set; }

	public Ability Ability
	{
		get
		{
			AbilityResults abilityResults = AbilityResults;
			if (abilityResults == null)
			{
				return null;
			}
			return abilityResults.Ability;
		}
	}

	public global::Effect Effect { get; set; }

	public Barrier Barrier { get; set; }

	public Vector3 Origin { get; set; }

	// rogues?
	//public EffectTemplate EffectOrigin { get; set; }

	public Ability GetRelevantAbility()
	{
		if (Ability != null)
		{
			return Ability;
		}
		if (Effect != null && Effect.Parent.IsAbility())
		{
			return Effect.Parent.Ability;
		}
		if (Barrier != null && Barrier.GetSourceAbility() != null)
		{
			return Barrier.GetSourceAbility();
		}
		return null;
	}

	// TODO ABILITIES
	public EffectSource GetEffectSource()
	{
		if (AbilityResults != null)
		{
			// custom
			return new EffectSource(Ability);
			// rogues
			//return new EffectSource(Ability, AbilityResults);
		}
		if (Effect != null)
		{
			return Effect.Parent;
		}
		if (Barrier != null)
		{
			return new EffectSource("Barrier", null, null);
		}
		return null;
	}
}
#endif
