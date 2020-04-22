using System.Collections.Generic;
using UnityEngine;

public class SparkBasicAttack : Ability
{
	[Space(10f)]
	public int m_laserDamageAmount = 5;

	public int m_healOnCasterOnTick;

	public bool m_healCasterOnIniialAttach;

	public LaserTargetingInfo m_laserInfo;

	public StandardEffectInfo m_laserHitEffect;

	[Header("-- Energy on Caster Per Turn")]
	public int m_energyOnCasterPerTurn = 5;

	[Separator("Tether", true)]
	public float m_tetherDistance = 5f;

	[Header("-- Tether Duration")]
	public int m_tetherDuration;

	public int m_additionalEnergizedDamage = 2;

	[Header("-- Extra Energy Gain On Caster --")]
	public int m_maxBonusEnergyFromGrowingGain;

	public int m_bonusEnergyGrowthRate;

	[Header("-- Animation on Pulse")]
	public int m_pulseAnimIndex;

	public int m_energizedPulseAnimIndex;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	public GameObject m_pulseSequence;

	public GameObject m_energizedPulseSequence;

	public GameObject m_beamSequence;

	public GameObject m_targetPersistentSequence;

	private AbilityMod_SparkBasicAttack m_abilityMod;

	private SparkEnergized m_energizedAbility;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardActorEffectData m_cachedEffectData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Spark Damage Beam";
		}
		Setup();
	}

	public void Setup()
	{
		SetCachedFields();
		if (m_energizedAbility == null)
		{
			AbilityData component = GetComponent<AbilityData>();
			if (component != null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_energizedAbility = (component.GetAbilityOfType(typeof(SparkEnergized)) as SparkEnergized);
			}
		}
		if (base.Targeter != null)
		{
			base.Targeter.ResetTargeter(true);
		}
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, GetLaserInfo());
		int affectsCaster;
		if (m_healCasterOnIniialAttach)
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
			affectsCaster = ((GetHealOnCasterPerTurn() > 0) ? 1 : 0);
		}
		else
		{
			affectsCaster = 0;
		}
		abilityUtil_Targeter_Laser.SetAffectedGroups(true, false, (byte)affectsCaster != 0);
		if (_003C_003Ef__am_0024cache0 == null)
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
			_003C_003Ef__am_0024cache0 = ((ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0);
		}
		abilityUtil_Targeter_Laser.m_affectCasterDelegate = _003C_003Ef__am_0024cache0;
		base.Targeter = abilityUtil_Targeter_Laser;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	public int GetInitialDamage()
	{
		int result;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_initialDamageMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			result = m_laserDamageAmount;
		}
		return result;
	}

	public int GetPerTurnDamage()
	{
		return GetEnemyTetherEffectData().m_damagePerTurn;
	}

	public int GetAdditionalDamageOnRadiated()
	{
		return (!m_abilityMod) ? m_additionalEnergizedDamage : m_abilityMod.m_additionalDamageOnRadiatedMod.GetModifiedValue(m_additionalEnergizedDamage);
	}

	public int GetEnergyOnCasterPerTurn()
	{
		int num;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = m_abilityMod.m_energyOnCasterPerTurnMod.GetModifiedValue(m_energyOnCasterPerTurn);
		}
		else
		{
			num = m_energyOnCasterPerTurn;
		}
		int num2 = num;
		if (m_energizedAbility != null)
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
			num2 = m_energizedAbility.CalcEnergyOnSelfPerTurn(num2);
		}
		return num2;
	}

	public int GetHealOnCasterPerTurn()
	{
		int num = (!m_abilityMod) ? m_healOnCasterOnTick : m_abilityMod.m_healOnCasterOnTickMod.GetModifiedValue(m_healOnCasterOnTick);
		if (m_energizedAbility != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = m_energizedAbility.CalcHealOnSelfPerTurn(num);
		}
		return num;
	}

	public float GetTetherDistance()
	{
		float result;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_tetherDistanceMod.GetModifiedValue(m_tetherDistance);
		}
		else
		{
			result = m_tetherDistance;
		}
		return result;
	}

	public int GetTetherDuration()
	{
		int result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_tetherDurationMod.GetModifiedValue(m_tetherDuration);
		}
		else
		{
			result = m_tetherDuration;
		}
		return result;
	}

	public bool UseBonusDamageOverTime()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = (m_abilityMod.m_useBonusDamageOverTime ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public int GetBonusDamageGrowRate()
	{
		return m_abilityMod ? m_abilityMod.m_bonusDamageIncreaseRateMod.GetModifiedValue(0) : 0;
	}

	public int GetMaxBonusDamage()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_maxBonusDamageAmountMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetBonusDamageFromTetherAge(int age)
	{
		int num = 0;
		if (UseBonusDamageOverTime())
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
			int maxBonusDamage = GetMaxBonusDamage();
			int bonusDamageGrowRate = GetBonusDamageGrowRate();
			if (bonusDamageGrowRate > 0)
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
				num = age * bonusDamageGrowRate;
			}
			if (maxBonusDamage > 0)
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
				num = Mathf.Min(num, maxBonusDamage);
			}
		}
		return num;
	}

	public int GetEnergyGainCyclePeriod()
	{
		int num;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = m_abilityMod.m_energyGainCyclePeriod.GetModifiedValue(0);
		}
		else
		{
			num = 1;
		}
		int b = num;
		return Mathf.Max(1, b);
	}

	public int GetEnergyGainPerCycle()
	{
		return m_abilityMod ? m_abilityMod.m_energyGainPerCycle.GetModifiedValue(0) : 0;
	}

	public int GetMaxBonusEnergyFromGrowingGain()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_maxBonusEnergyFromGrowingGainMod.GetModifiedValue(m_maxBonusEnergyFromGrowingGain);
		}
		else
		{
			result = m_maxBonusEnergyFromGrowingGain;
		}
		return result;
	}

	public int GetBonusEnergyGrowthRate()
	{
		int result;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_bonusEnergyGrowthRateMod.GetModifiedValue(m_bonusEnergyGrowthRate);
		}
		else
		{
			result = m_bonusEnergyGrowthRate;
		}
		return result;
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo laserInfo = m_laserInfo;
		object mod;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			mod = m_abilityMod.m_laserInfoMod;
		}
		else
		{
			mod = null;
		}
		m_cachedLaserInfo = laserInfo.GetModifiedCopy((AbilityModPropertyLaserInfo)mod);
		StandardEffectInfo standardEffectInfo;
		if ((bool)m_abilityMod)
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
			standardEffectInfo = m_abilityMod.m_tetherBaseEffectOverride.GetModifiedValue(m_laserHitEffect);
		}
		else
		{
			standardEffectInfo = m_laserHitEffect.GetShallowCopy();
		}
		StandardEffectInfo standardEffectInfo2 = standardEffectInfo;
		m_cachedEffectData = standardEffectInfo2.m_effectData;
		m_cachedEffectData.m_sequencePrefabs = new GameObject[2]
		{
			m_targetPersistentSequence,
			m_beamSequence
		};
	}

	public StandardActorEffectData GetEnemyTetherEffectData()
	{
		StandardActorEffectData result;
		if (m_cachedEffectData != null)
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
			result = m_cachedEffectData;
		}
		else
		{
			result = m_laserHitEffect.m_effectData;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserInfo != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage_FirstTurn", "damage on initial attach", m_laserDamageAmount);
		AddTokenInt(tokens, "Damage_PerTurnAfterFirst", "damage on subsequent turns", m_laserHitEffect.m_effectData.m_damagePerTurn);
		AddTokenInt(tokens, "Damage_AdditionalOnRadiated", "additional damage on radiated", m_additionalEnergizedDamage);
		AddTokenInt(tokens, "Heal_OnCasterPerTurn", "heal on caster per turn", m_healOnCasterOnTick);
		AddTokenInt(tokens, "EnergyOnCasterPerTurn", string.Empty, m_energyOnCasterPerTurn);
		AddTokenInt(tokens, "MaxBonusEnergyFromGrowingGain", string.Empty, m_maxBonusEnergyFromGrowingGain);
		AddTokenInt(tokens, "BonusEnergyGrowthRate", string.Empty, m_bonusEnergyGrowthRate);
		AddTokenInt(tokens, "TetherDuration", string.Empty, m_tetherDuration);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		if (GetInitialDamage() > 0)
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
			AbilityTooltipHelper.ReportDamage(ref number, AbilityTooltipSubject.Primary, GetInitialDamage());
		}
		GetEnemyTetherEffectData().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Primary);
		if (m_healCasterOnIniialAttach)
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
			if (GetHealOnCasterPerTurn() > 0)
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
				AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, GetHealOnCasterPerTurn());
			}
		}
		return number;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
		int result;
		if (visibleActorsCountByTooltipSubject > 0)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = GetEnergyOnCasterPerTurn();
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public override List<int> _001D()
	{
		List<int> list = base._001D();
		list.Add(m_laserHitEffect.m_effectData.m_damagePerTurn);
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SparkBasicAttack))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_SparkBasicAttack);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
