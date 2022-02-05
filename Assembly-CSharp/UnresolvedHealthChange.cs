// ROGUES
// SERVER
//using System;
using System.Runtime.InteropServices;

// server-only, was empty in reactor
[StructLayout(LayoutKind.Sequential, Size = 1)]  // reactor
public struct UnresolvedHealthChange
{
#if SERVER
	// TODO HIGH added nonserialized bc the struct was empty in reactor, looks weird
	[System.NonSerialized]  // custom
	public ActorData caster;
	[System.NonSerialized]  // custom
	public DamageSource src;
	[System.NonSerialized]  // custom
	public int finalHealthAdjust;
	[System.NonSerialized]  // custom
	public HealthChangeType type;
	[System.NonSerialized]  // custom
	public ActorHitResults actorHitResults;

	public void InitAsDamage(ActorData damageCaster, DamageSource damageSrc, int finalDamageAmount)
	{
		caster = damageCaster;
		src = damageSrc;
		finalHealthAdjust = finalDamageAmount;
		type = HealthChangeType.Damage;
	}

	public void InitAsHealing(ActorData healCaster, DamageSource healSrc, int finalHealAmount)
	{
		caster = healCaster;
		src = healSrc;
		finalHealthAdjust = finalHealAmount;
		type = HealthChangeType.Healing;
	}

	public void SetActorHitResults(ActorHitResults results)
	{
		actorHitResults = results;
	}

	public enum HealthChangeType
	{
		Damage,
		Healing
	}
#endif
}
