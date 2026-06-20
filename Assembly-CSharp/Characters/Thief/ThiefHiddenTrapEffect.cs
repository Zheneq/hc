// ROGUES
// SERVER
using UnityEngine;

#if SERVER
// added in rogues
public class ThiefHiddenTrapEffect : StandardGroundEffect
{
	private int m_extraDamagePerTurn;
	private int m_maxExtraDamage;

	public ThiefHiddenTrapEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		Vector3 shapeFreePos,
		ActorData target,
		ActorData caster,
		GroundEffectField fieldInfo,
		int extraDamagePerTurn,
		int maxExtraDamage)
		: base(parent, targetSquare, shapeFreePos, target, caster, fieldInfo)
	{
		m_extraDamagePerTurn = extraDamagePerTurn;
		m_maxExtraDamage = maxExtraDamage;
	}

	public override void SetupActorHitResults(ref ActorHitResults actorHitRes, BoardSquare targetSquare)
	{
		base.SetupActorHitResults(ref actorHitRes, targetSquare);
		int extraDamage = m_time.age * m_extraDamagePerTurn;
		extraDamage = Mathf.Clamp(extraDamage, 0, m_maxExtraDamage);
		if (extraDamage > 0)
		{
			actorHitRes.AddBaseDamage(extraDamage);
		}
	}
}
#endif
