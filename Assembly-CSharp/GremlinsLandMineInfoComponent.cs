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
			m_dropMinesAbility = (component.GetAbilityOfType(typeof(GremlinsDropMines)) as GremlinsDropMines);
			m_bigBangAbility = (component.GetAbilityOfType(typeof(GremlinsBigBang)) as GremlinsBigBang);
			m_bombingRunAbility = (component.GetAbilityOfType(typeof(GremlinsBombingRun)) as GremlinsBombingRun);
			m_basicAttackAbility = (component.GetAbilityOfType(typeof(GremlinsMultiTargeterBasicAttack)) as GremlinsMultiTargeterBasicAttack);
			m_ultAbility = (component.GetAbilityOfType(typeof(GremlinsMultiTargeterApocolypse)) as GremlinsMultiTargeterApocolypse);
		}
	}

	public int GetMineDuration()
	{
		int num = m_mineDuration;
		if (m_dropMinesAbility != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_dropMinesAbility.GetMod() != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num = m_dropMinesAbility.GetMod().m_mineDurationMod.GetModifiedValue(num);
			}
		}
		if (m_bigBangAbility != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_bigBangAbility.GetMod() != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num = m_bigBangAbility.GetMod().m_mineDurationMod.GetModifiedValue(num);
			}
		}
		if (m_bombingRunAbility != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_bombingRunAbility.GetMod() != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				num = m_bombingRunAbility.GetMod().m_mineDurationMod.GetModifiedValue(num);
			}
		}
		if (m_basicAttackAbility != null && m_basicAttackAbility.GetMod() != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			num = m_basicAttackAbility.GetMod().m_mineDurationMod.GetModifiedValue(num);
		}
		if (m_ultAbility != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_ultAbility.GetMod() != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				num = m_ultAbility.GetMod().m_mineDurationMod.GetModifiedValue(num);
			}
		}
		return num;
	}

	public int GetDamageOnMovedOver()
	{
		int num = m_damageAmount;
		if (m_dropMinesAbility != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_dropMinesAbility.GetMod() != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num = m_dropMinesAbility.GetMod().m_mineDamageMod.GetModifiedValue(num);
			}
		}
		if (m_bigBangAbility != null && m_bigBangAbility.GetMod() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			num = m_bigBangAbility.GetMod().m_mineDamageMod.GetModifiedValue(num);
		}
		if (m_bombingRunAbility != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_bombingRunAbility.GetMod() != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				num = m_bombingRunAbility.GetMod().m_mineDamageMod.GetModifiedValue(num);
			}
		}
		if (m_basicAttackAbility != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_basicAttackAbility.GetMod() != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num = m_basicAttackAbility.GetMod().m_mineDamageMod.GetModifiedValue(num);
			}
		}
		if (m_ultAbility != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_ultAbility.GetMod() != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				num = m_ultAbility.GetMod().m_mineDamageMod.GetModifiedValue(num);
			}
		}
		return num;
	}

	public StandardEffectInfo GetEnemyHitEffectOnMovedOver()
	{
		StandardEffectInfo standardEffectInfo = m_enemyHitEffect;
		if (m_dropMinesAbility != null && m_dropMinesAbility.GetMod() != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			standardEffectInfo = m_dropMinesAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(standardEffectInfo);
		}
		if (m_bigBangAbility != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_bigBangAbility.GetMod() != null)
			{
				standardEffectInfo = m_bigBangAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(standardEffectInfo);
			}
		}
		if (m_bombingRunAbility != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_bombingRunAbility.GetMod() != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				standardEffectInfo = m_bombingRunAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(standardEffectInfo);
			}
		}
		if (m_basicAttackAbility != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_basicAttackAbility.GetMod() != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				standardEffectInfo = m_basicAttackAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(standardEffectInfo);
			}
		}
		if (m_ultAbility != null && m_ultAbility.GetMod() != null)
		{
			standardEffectInfo = m_ultAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(standardEffectInfo);
		}
		return standardEffectInfo;
	}

	public int GetEnergyOnExplosion()
	{
		int num = m_energyGainOnExplosion;
		if (m_dropMinesAbility != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_dropMinesAbility.GetMod() != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				num = m_dropMinesAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(num);
			}
		}
		if (m_bigBangAbility != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_bigBangAbility.GetMod() != null)
			{
				num = m_bigBangAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(num);
			}
		}
		if (m_bombingRunAbility != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_bombingRunAbility.GetMod() != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				num = m_bombingRunAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(num);
			}
		}
		if (m_basicAttackAbility != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_basicAttackAbility.GetMod() != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				num = m_basicAttackAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(num);
			}
		}
		if (m_ultAbility != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_ultAbility.GetMod() != null)
			{
				num = m_ultAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(num);
			}
		}
		return num;
	}
}
