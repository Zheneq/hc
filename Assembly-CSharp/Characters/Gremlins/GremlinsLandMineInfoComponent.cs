// ROGUES
// SERVER
using UnityEngine;

public class GremlinsLandMineInfoComponent : MonoBehaviour
{
	[Header("-- On Cast Hit Damage")]
	public int m_directHitDamageAmount = 25;
	public int m_directHitSubsequentDamageAmount = 10;
	[Header("-- Mine Info")]
	public int m_mineDuration = 5;
	public int m_damageAmount = 25;
	public StandardEffectInfo m_enemyHitEffect;
	[Space(10f)]
	public int m_energyGainOnExplosion;
	[Header("-- Sequences")]
	public GameObject m_persistentSequencePrefab;
	public GameObject m_explosionSequencePrefab;
	[HideInInspector]
	public AbilityAreaShape m_explosionShape;
	[HideInInspector]
	public bool m_detonateOnTimeout;
	[HideInInspector]
	public bool m_penetrateLos;
	[TextArea(1, 10)]
	public string m_notes;

	private GremlinsDropMines m_dropMinesAbility;
	private GremlinsBigBang m_bigBangAbility;
	private GremlinsBombingRun m_bombingRunAbility;
	private GremlinsMultiTargeterBasicAttack m_basicAttackAbility;
	private GremlinsMultiTargeterApocolypse m_ultAbility;

	private void Start()
	{
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			m_dropMinesAbility = component.GetAbilityOfType(typeof(GremlinsDropMines)) as GremlinsDropMines;
			m_bigBangAbility = component.GetAbilityOfType(typeof(GremlinsBigBang)) as GremlinsBigBang;
			m_bombingRunAbility = component.GetAbilityOfType(typeof(GremlinsBombingRun)) as GremlinsBombingRun;
			m_basicAttackAbility = component.GetAbilityOfType(typeof(GremlinsMultiTargeterBasicAttack)) as GremlinsMultiTargeterBasicAttack;
			m_ultAbility = component.GetAbilityOfType(typeof(GremlinsMultiTargeterApocolypse)) as GremlinsMultiTargeterApocolypse;
		}
	}

	public int GetMineDuration()
	{
		int duration = m_mineDuration;
		if (m_dropMinesAbility != null && m_dropMinesAbility.GetMod() != null)
		{
			duration = m_dropMinesAbility.GetMod().m_mineDurationMod.GetModifiedValue(duration);
		}
		if (m_bigBangAbility != null && m_bigBangAbility.GetMod() != null)
		{
			duration = m_bigBangAbility.GetMod().m_mineDurationMod.GetModifiedValue(duration);
		}
		if (m_bombingRunAbility != null && m_bombingRunAbility.GetMod() != null)
		{
			duration = m_bombingRunAbility.GetMod().m_mineDurationMod.GetModifiedValue(duration);
		}
		if (m_basicAttackAbility != null && m_basicAttackAbility.GetMod() != null)
		{
			duration = m_basicAttackAbility.GetMod().m_mineDurationMod.GetModifiedValue(duration);
		}
		if (m_ultAbility != null && m_ultAbility.GetMod() != null)
		{
			duration = m_ultAbility.GetMod().m_mineDurationMod.GetModifiedValue(duration);
		}
		return duration;
	}

	public int GetDamageOnMovedOver()
	{
		int damage = m_damageAmount;
		if (m_dropMinesAbility != null && m_dropMinesAbility.GetMod() != null)
		{
			damage = m_dropMinesAbility.GetMod().m_mineDamageMod.GetModifiedValue(damage);
		}
		if (m_bigBangAbility != null && m_bigBangAbility.GetMod() != null)
		{
			damage = m_bigBangAbility.GetMod().m_mineDamageMod.GetModifiedValue(damage);
		}
		if (m_bombingRunAbility != null && m_bombingRunAbility.GetMod() != null)
		{
			damage = m_bombingRunAbility.GetMod().m_mineDamageMod.GetModifiedValue(damage);
		}
		if (m_basicAttackAbility != null && m_basicAttackAbility.GetMod() != null)
		{
			damage = m_basicAttackAbility.GetMod().m_mineDamageMod.GetModifiedValue(damage);
		}
		if (m_ultAbility != null && m_ultAbility.GetMod() != null)
		{
			damage = m_ultAbility.GetMod().m_mineDamageMod.GetModifiedValue(damage);
		}
		return damage;
	}

	public StandardEffectInfo GetEnemyHitEffectOnMovedOver()
	{
		StandardEffectInfo hitEffect = m_enemyHitEffect;
		if (m_dropMinesAbility != null && m_dropMinesAbility.GetMod() != null)
		{
			hitEffect = m_dropMinesAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(hitEffect);
		}
		if (m_bigBangAbility != null && m_bigBangAbility.GetMod() != null)
		{
			hitEffect = m_bigBangAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(hitEffect);
		}
		if (m_bombingRunAbility != null && m_bombingRunAbility.GetMod() != null)
		{
			hitEffect = m_bombingRunAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(hitEffect);
		}
		if (m_basicAttackAbility != null && m_basicAttackAbility.GetMod() != null)
		{
			hitEffect = m_basicAttackAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(hitEffect);
		}
		if (m_ultAbility != null && m_ultAbility.GetMod() != null)
		{
			hitEffect = m_ultAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(hitEffect);
		}
		return hitEffect;
	}

	public int GetEnergyOnExplosion()
	{
		int energy = m_energyGainOnExplosion;
		if (m_dropMinesAbility != null && m_dropMinesAbility.GetMod() != null)
		{
			energy = m_dropMinesAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(energy);
		}
		if (m_bigBangAbility != null && m_bigBangAbility.GetMod() != null)
		{
			energy = m_bigBangAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(energy);
		}
		if (m_bombingRunAbility != null && m_bombingRunAbility.GetMod() != null)
		{
			energy = m_bombingRunAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(energy);
		}
		if (m_basicAttackAbility != null && m_basicAttackAbility.GetMod() != null)
		{
			energy = m_basicAttackAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(energy);
		}
		if (m_ultAbility != null && m_ultAbility.GetMod() != null)
		{
			energy = m_ultAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(energy);
		}
		return energy;
	}
	
#if SERVER
	public GremlinsLandMineEffect CreateLandmineEffect(EffectSource source, ActorData caster, BoardSquare targetSquare)
	{
		return new GremlinsLandMineEffect(
			source,
			caster,
			targetSquare,
			GetMineDuration(),
			GetDamageOnMovedOver(),
			GetEnemyHitEffectOnMovedOver(),
			GetEnergyOnExplosion(),
			m_explosionShape,
			m_detonateOnTimeout,
			m_penetrateLos,
			m_persistentSequencePrefab,
			m_explosionSequencePrefab);
	}
#endif
}
