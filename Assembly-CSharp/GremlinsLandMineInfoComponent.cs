using System;
using UnityEngine;

public class GremlinsLandMineInfoComponent : MonoBehaviour
{
	[Header("-- On Cast Hit Damage")]
	public int m_directHitDamageAmount = 0x19;

	public int m_directHitSubsequentDamageAmount = 0xA;

	[Header("-- Mine Info")]
	public int m_mineDuration = 5;

	public int m_damageAmount = 0x19;

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

	[TextArea(1, 0xA)]
	public string m_notes;

	private GremlinsDropMines m_dropMinesAbility;

	private GremlinsBigBang m_bigBangAbility;

	private GremlinsBombingRun m_bombingRunAbility;

	private GremlinsMultiTargeterBasicAttack m_basicAttackAbility;

	private GremlinsMultiTargeterApocolypse m_ultAbility;

	private void Start()
	{
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
		{
			this.m_dropMinesAbility = (component.GetAbilityOfType(typeof(GremlinsDropMines)) as GremlinsDropMines);
			this.m_bigBangAbility = (component.GetAbilityOfType(typeof(GremlinsBigBang)) as GremlinsBigBang);
			this.m_bombingRunAbility = (component.GetAbilityOfType(typeof(GremlinsBombingRun)) as GremlinsBombingRun);
			this.m_basicAttackAbility = (component.GetAbilityOfType(typeof(GremlinsMultiTargeterBasicAttack)) as GremlinsMultiTargeterBasicAttack);
			this.m_ultAbility = (component.GetAbilityOfType(typeof(GremlinsMultiTargeterApocolypse)) as GremlinsMultiTargeterApocolypse);
		}
	}

	public int GetMineDuration()
	{
		int num = this.m_mineDuration;
		if (this.m_dropMinesAbility != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsLandMineInfoComponent.GetMineDuration()).MethodHandle;
			}
			if (this.m_dropMinesAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.m_dropMinesAbility.GetMod().m_mineDurationMod.GetModifiedValue(num);
			}
		}
		if (this.m_bigBangAbility != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_bigBangAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.m_bigBangAbility.GetMod().m_mineDurationMod.GetModifiedValue(num);
			}
		}
		if (this.m_bombingRunAbility != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_bombingRunAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.m_bombingRunAbility.GetMod().m_mineDurationMod.GetModifiedValue(num);
			}
		}
		if (this.m_basicAttackAbility != null && this.m_basicAttackAbility.GetMod() != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			num = this.m_basicAttackAbility.GetMod().m_mineDurationMod.GetModifiedValue(num);
		}
		if (this.m_ultAbility != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_ultAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.m_ultAbility.GetMod().m_mineDurationMod.GetModifiedValue(num);
			}
		}
		return num;
	}

	public int GetDamageOnMovedOver()
	{
		int num = this.m_damageAmount;
		if (this.m_dropMinesAbility != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsLandMineInfoComponent.GetDamageOnMovedOver()).MethodHandle;
			}
			if (this.m_dropMinesAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.m_dropMinesAbility.GetMod().m_mineDamageMod.GetModifiedValue(num);
			}
		}
		if (this.m_bigBangAbility != null && this.m_bigBangAbility.GetMod() != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			num = this.m_bigBangAbility.GetMod().m_mineDamageMod.GetModifiedValue(num);
		}
		if (this.m_bombingRunAbility != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_bombingRunAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.m_bombingRunAbility.GetMod().m_mineDamageMod.GetModifiedValue(num);
			}
		}
		if (this.m_basicAttackAbility != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_basicAttackAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.m_basicAttackAbility.GetMod().m_mineDamageMod.GetModifiedValue(num);
			}
		}
		if (this.m_ultAbility != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_ultAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.m_ultAbility.GetMod().m_mineDamageMod.GetModifiedValue(num);
			}
		}
		return num;
	}

	public StandardEffectInfo GetEnemyHitEffectOnMovedOver()
	{
		StandardEffectInfo standardEffectInfo = this.m_enemyHitEffect;
		if (this.m_dropMinesAbility != null && this.m_dropMinesAbility.GetMod() != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsLandMineInfoComponent.GetEnemyHitEffectOnMovedOver()).MethodHandle;
			}
			standardEffectInfo = this.m_dropMinesAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(standardEffectInfo);
		}
		if (this.m_bigBangAbility != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_bigBangAbility.GetMod() != null)
			{
				standardEffectInfo = this.m_bigBangAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(standardEffectInfo);
			}
		}
		if (this.m_bombingRunAbility != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_bombingRunAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				standardEffectInfo = this.m_bombingRunAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(standardEffectInfo);
			}
		}
		if (this.m_basicAttackAbility != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_basicAttackAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				standardEffectInfo = this.m_basicAttackAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(standardEffectInfo);
			}
		}
		if (this.m_ultAbility != null && this.m_ultAbility.GetMod() != null)
		{
			standardEffectInfo = this.m_ultAbility.GetMod().m_effectOnEnemyOverride.GetModifiedValue(standardEffectInfo);
		}
		return standardEffectInfo;
	}

	public int GetEnergyOnExplosion()
	{
		int num = this.m_energyGainOnExplosion;
		if (this.m_dropMinesAbility != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GremlinsLandMineInfoComponent.GetEnergyOnExplosion()).MethodHandle;
			}
			if (this.m_dropMinesAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.m_dropMinesAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(num);
			}
		}
		if (this.m_bigBangAbility != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_bigBangAbility.GetMod() != null)
			{
				num = this.m_bigBangAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(num);
			}
		}
		if (this.m_bombingRunAbility != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_bombingRunAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.m_bombingRunAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(num);
			}
		}
		if (this.m_basicAttackAbility != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_basicAttackAbility.GetMod() != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				num = this.m_basicAttackAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(num);
			}
		}
		if (this.m_ultAbility != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_ultAbility.GetMod() != null)
			{
				num = this.m_ultAbility.GetMod().m_energyOnMineExplosionMod.GetModifiedValue(num);
			}
		}
		return num;
	}
}
