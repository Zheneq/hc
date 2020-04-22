using System.Collections.Generic;
using UnityEngine;

public class ScampSuitToggle : Ability
{
	[Separator("Whether shield down mode is free action", true)]
	public bool m_shieldDownModeFreeAction;

	[Separator("Cooldowns", true)]
	public int m_cooldownCreateSuit = 2;

	public int m_cooldownRefillShield = 2;

	[Header("-- Cooldown override for when suit is destroyed")]
	public int m_cooldownOverrideOnSuitDestroy = 2;

	[Separator("Energy to Shield (shield = energy x multiplier)", true)]
	public float m_energyToShieldMult = 1f;

	[Separator("Clear Energy Orbs on cast", true)]
	public bool m_clearEnergyOrbsOnCast = true;

	[Separator("Extra Orbs to spawn on suit lost", true)]
	public int m_extraOrbsToSpawnOnSuitLost;

	[Separator("Passive Energy Regen", true)]
	public int m_passiveEnergyRegen;

	[Separator("Effect to apply when suit is gained or lost (applied on start of turn)", true)]
	public bool m_considerRespawnForSuitGainEffect;

	public StandardEffectInfo m_effectForSuitGained;

	public StandardEffectInfo m_effectForSuitLost;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	[Header("-- for setting anim param, only spawned when adding new suit")]
	public GameObject m_addSuitAnimSeqPrefab;

	private AbilityMod_ScampSuitToggle m_abilityMod;

	private Scamp_SyncComponent m_syncComp;

	private Passive_Scamp m_passive;

	private StandardEffectInfo m_cachedEffectForSuitGained;

	private StandardEffectInfo m_cachedEffectForSuitLost;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "ScampSuitToggle";
		}
		Setup();
	}

	private void Setup()
	{
		m_passive = GetPassiveOfType<Passive_Scamp>();
		SetCachedFields();
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, false, false, AbilityUtil_Targeter.AffectsActor.Always);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "CooldownCreateSuit", string.Empty, m_cooldownCreateSuit);
		AddTokenInt(tokens, "CooldownRefillShield", string.Empty, m_cooldownRefillShield);
		AddTokenInt(tokens, "CooldownOverrideOnSuitDestroy", string.Empty, m_cooldownOverrideOnSuitDestroy);
		AddTokenInt(tokens, "ExtraOrbsToSpawnOnSuitLost", string.Empty, m_extraOrbsToSpawnOnSuitLost);
		AddTokenInt(tokens, "PassiveEnergyRegen", string.Empty, m_passiveEnergyRegen);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectForSuitGained, "EffectForSuitGained", m_effectForSuitGained);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectForSuitLost, "EffectForSuitLost", m_effectForSuitLost);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectForSuitGained;
		if (m_abilityMod != null)
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
			cachedEffectForSuitGained = m_abilityMod.m_effectForSuitGainedMod.GetModifiedValue(m_effectForSuitGained);
		}
		else
		{
			cachedEffectForSuitGained = m_effectForSuitGained;
		}
		m_cachedEffectForSuitGained = cachedEffectForSuitGained;
		StandardEffectInfo cachedEffectForSuitLost;
		if (m_abilityMod != null)
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
			cachedEffectForSuitLost = m_abilityMod.m_effectForSuitLostMod.GetModifiedValue(m_effectForSuitLost);
		}
		else
		{
			cachedEffectForSuitLost = m_effectForSuitLost;
		}
		m_cachedEffectForSuitLost = cachedEffectForSuitLost;
	}

	public bool ShieldDownModeFreeAction()
	{
		bool result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_shieldDownModeFreeActionMod.GetModifiedValue(m_shieldDownModeFreeAction);
		}
		else
		{
			result = m_shieldDownModeFreeAction;
		}
		return result;
	}

	public int GetCooldownCreateSuit()
	{
		int result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_cooldownCreateSuitMod.GetModifiedValue(m_cooldownCreateSuit);
		}
		else
		{
			result = m_cooldownCreateSuit;
		}
		return result;
	}

	public int GetCooldownRefillShield()
	{
		int result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_cooldownRefillShieldMod.GetModifiedValue(m_cooldownRefillShield);
		}
		else
		{
			result = m_cooldownRefillShield;
		}
		return result;
	}

	public int GetCooldownOverrideOnSuitDestroy()
	{
		return (!(m_abilityMod != null)) ? m_cooldownOverrideOnSuitDestroy : m_abilityMod.m_cooldownOverrideOnSuitDestroyMod.GetModifiedValue(m_cooldownOverrideOnSuitDestroy);
	}

	public float GetEnergyToShieldMult()
	{
		float result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_energyToShieldMultMod.GetModifiedValue(m_energyToShieldMult);
		}
		else
		{
			result = m_energyToShieldMult;
		}
		return result;
	}

	public bool ClearEnergyOrbsOnCast()
	{
		bool result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_clearEnergyOrbsOnCastMod.GetModifiedValue(m_clearEnergyOrbsOnCast);
		}
		else
		{
			result = m_clearEnergyOrbsOnCast;
		}
		return result;
	}

	public int GetExtraOrbsToSpawnOnSuitLost()
	{
		return (!(m_abilityMod != null)) ? m_extraOrbsToSpawnOnSuitLost : m_abilityMod.m_extraOrbsToSpawnOnSuitLostMod.GetModifiedValue(m_extraOrbsToSpawnOnSuitLost);
	}

	public int GetPassiveEnergyRegen()
	{
		int result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_passiveEnergyRegenMod.GetModifiedValue(m_passiveEnergyRegen);
		}
		else
		{
			result = m_passiveEnergyRegen;
		}
		return result;
	}

	public bool ConsiderRespawnForSuitGainEffect()
	{
		bool result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_considerRespawnForSuitGainEffectMod.GetModifiedValue(m_considerRespawnForSuitGainEffect);
		}
		else
		{
			result = m_considerRespawnForSuitGainEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEffectForSuitGained()
	{
		StandardEffectInfo result;
		if (m_cachedEffectForSuitGained != null)
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
			result = m_cachedEffectForSuitGained;
		}
		else
		{
			result = m_effectForSuitGained;
		}
		return result;
	}

	public StandardEffectInfo GetEffectForSuitLost()
	{
		StandardEffectInfo result;
		if (m_cachedEffectForSuitLost != null)
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
			result = m_cachedEffectForSuitLost;
		}
		else
		{
			result = m_effectForSuitLost;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		ActorData actorData = base.ActorData;
		int num = actorData.TechPoints + actorData.ReservedTechPoints;
		int value = Mathf.RoundToInt((float)num * GetEnergyToShieldMult());
		results.m_absorb = Mathf.Clamp(value, 1, m_passive.GetMaxSuitShield());
		return true;
	}

	public override bool IsFreeAction()
	{
		if (m_syncComp != null)
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
			if (m_syncComp.m_suitWasActiveOnTurnStart)
			{
				return base.IsFreeAction();
			}
		}
		return ShieldDownModeFreeAction();
	}

	public override int GetModdedCost()
	{
		int b = 0;
		if (base.ActorData != null)
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
			b = base.ActorData.TechPoints + base.ActorData.ReservedTechPoints;
		}
		return Mathf.Max(1, b);
	}

	public override int GetTechPointRegenContribution()
	{
		int passiveEnergyRegen = GetPassiveEnergyRegen();
		int result;
		if (passiveEnergyRegen > 0)
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
			result = passiveEnergyRegen;
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_syncComp != null)
		{
			return caster.TechPoints + caster.ReservedTechPoints > 0;
		}
		return base.CustomCanCastValidation(caster);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ScampSuitToggle))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_ScampSuitToggle);
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
