using System.Collections.Generic;
using UnityEngine;

public class MartyrSpendCrystals : Ability
{
	public enum TargetingMode
	{
		OnSelf,
		Aoe
	}

	public TargetingMode m_targetingMode;

	[Header("-- Self Healing & Absorb")]
	public StandardEffectInfo m_spentCrystalsEffect;

	[Header("-- Self Healing")]
	public int m_selfHealBase;

	public int m_selfHealPerCrystalSpent;

	public int m_selfHealPerEnemyHit;

	public bool m_selfHealIsOverTime = true;

	[Space(10f)]
	public int m_extraSelfHealPerTurnAtMaxEnergy;

	public int m_maxExtraSelfHealForMaxEnergy;

	[Header("-- Self Absorb")]
	public int m_selfAbsorbBase;

	public int m_selfAbsorbPerCrystalSpent;

	[Header("-- Enemy Hit (if using AoE targeting)")]
	public float m_aoeRadiusBase = 2f;

	public float m_aoeRadiuePerCrystal = 0.5f;

	public bool m_penetrateLos;

	public int m_damageBase;

	public int m_damagePerCrystal;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Ally Hit (if using AoE targeting")]
	public int m_allyHealBase;

	public int m_allyHealPerCrystal;

	public int m_allyHealPerEnemyHit;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- Energy Use --")]
	public bool m_clearEnergyOnCast = true;

	public int m_selfEnergyGainOnCast;

	[Header("-- Cooldown Reduction on other abilities")]
	public AbilityData.ActionType m_protectAllyActionType = AbilityData.ActionType.INVALID_ACTION;

	public int m_cdrOnProtectAllyAbility;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	private Martyr_SyncComponent m_syncComponent;

	private AbilityMod_MartyrSpendCrystals m_abilityMod;

	private StandardEffectInfo m_cachedSpentCrystalsEffect;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Martyr Spend Crystals";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		if (m_targetingMode == TargetingMode.OnSelf)
		{
			base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always);
			base.Targeter.ShowArcToShape = false;
			return;
		}
		AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = new AbilityUtil_Targeter_AoE_Smooth(this, 1f, PenetrateLos(), IncludeEnemies(), IncludeAllies());
		abilityUtil_Targeter_AoE_Smooth.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), true);
		abilityUtil_Targeter_AoE_Smooth.m_customRadiusDelegate = GetTargeterRadius;
		base.Targeter = abilityUtil_Targeter_AoE_Smooth;
	}

	private float GetTargeterRadius(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return GetCurrentAoeRadius(targetingActor);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedSpentCrystalsEffect;
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
			cachedSpentCrystalsEffect = m_abilityMod.m_spentCrystalsEffectMod.GetModifiedValue(m_spentCrystalsEffect);
		}
		else
		{
			cachedSpentCrystalsEffect = m_spentCrystalsEffect;
		}
		m_cachedSpentCrystalsEffect = cachedSpentCrystalsEffect;
		StandardEffectInfo cachedEnemyHitEffect;
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
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		m_cachedAllyHitEffect = ((!m_abilityMod) ? m_allyHitEffect : m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect));
	}

	public StandardEffectInfo GetSpentCrystalsEffect()
	{
		StandardEffectInfo result;
		if (m_cachedSpentCrystalsEffect != null)
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
			result = m_cachedSpentCrystalsEffect;
		}
		else
		{
			result = m_spentCrystalsEffect;
		}
		return result;
	}

	public int GetSelfHealBase()
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
			result = m_abilityMod.m_selfHealBaseMod.GetModifiedValue(m_selfHealBase);
		}
		else
		{
			result = m_selfHealBase;
		}
		return result;
	}

	public int GetSelfHealPerCrystalSpent()
	{
		return (!m_abilityMod) ? m_selfHealPerCrystalSpent : m_abilityMod.m_selfHealPerCrystalSpentMod.GetModifiedValue(m_selfHealPerCrystalSpent);
	}

	public int GetSelfHealPerEnemyHit()
	{
		return (!m_abilityMod) ? m_selfHealPerEnemyHit : m_abilityMod.m_selfHealPerEnemyHitMod.GetModifiedValue(m_selfHealPerEnemyHit);
	}

	public bool SelfHealIsOverTime()
	{
		bool result;
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
			result = m_abilityMod.m_selfHealIsOverTimeMod.GetModifiedValue(m_selfHealIsOverTime);
		}
		else
		{
			result = m_selfHealIsOverTime;
		}
		return result;
	}

	public int GetExtraSelfHealPerTurnAtMaxEnergy()
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
			result = m_abilityMod.m_extraSelfHealPerTurnAtMaxEnergyMod.GetModifiedValue(m_extraSelfHealPerTurnAtMaxEnergy);
		}
		else
		{
			result = m_extraSelfHealPerTurnAtMaxEnergy;
		}
		return result;
	}

	public int GetMaxExtraSelfHealForMaxEnergy()
	{
		return (!m_abilityMod) ? m_maxExtraSelfHealForMaxEnergy : m_abilityMod.m_maxExtraSelfHealForMaxEnergyMod.GetModifiedValue(m_maxExtraSelfHealForMaxEnergy);
	}

	public int GetSelfAbsorbBase()
	{
		return (!m_abilityMod) ? m_selfAbsorbBase : m_abilityMod.m_selfAbsorbBaseMod.GetModifiedValue(m_selfAbsorbBase);
	}

	public int GetSelfAbsorbPerCrystalSpent()
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
			result = m_abilityMod.m_selfAbsorbPerCrystalSpentMod.GetModifiedValue(m_selfAbsorbPerCrystalSpent);
		}
		else
		{
			result = m_selfAbsorbPerCrystalSpent;
		}
		return result;
	}

	public float GetAoeRadiusBase()
	{
		float result;
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
			result = m_abilityMod.m_aoeRadiusBaseMod.GetModifiedValue(m_aoeRadiusBase);
		}
		else
		{
			result = m_aoeRadiusBase;
		}
		return result;
	}

	public float GetAoeRadiuePerCrystal()
	{
		return (!m_abilityMod) ? m_aoeRadiuePerCrystal : m_abilityMod.m_aoeRadiuePerCrystalMod.GetModifiedValue(m_aoeRadiuePerCrystal);
	}

	public bool PenetrateLos()
	{
		bool result;
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
			result = m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
		}
		else
		{
			result = m_penetrateLos;
		}
		return result;
	}

	public int GetDamageBase()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_damageBaseMod.GetModifiedValue(m_damageBase);
		}
		else
		{
			result = m_damageBase;
		}
		return result;
	}

	public int GetDamagePerCrystal()
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
			result = m_abilityMod.m_damagePerCrystalMod.GetModifiedValue(m_damagePerCrystal);
		}
		else
		{
			result = m_damagePerCrystal;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
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
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public int GetAllyHealBase()
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
			result = m_abilityMod.m_allyHealBaseMod.GetModifiedValue(m_allyHealBase);
		}
		else
		{
			result = m_allyHealBase;
		}
		return result;
	}

	public int GetAllyHealPerCrystal()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_allyHealPerCrystalMod.GetModifiedValue(m_allyHealPerCrystal);
		}
		else
		{
			result = m_allyHealPerCrystal;
		}
		return result;
	}

	public int GetAllyHealPerEnemyHit()
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
			result = m_abilityMod.m_allyHealPerEnemyHitMod.GetModifiedValue(m_allyHealPerEnemyHit);
		}
		else
		{
			result = m_allyHealPerEnemyHit;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyHitEffect != null)
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
			result = m_cachedAllyHitEffect;
		}
		else
		{
			result = m_allyHitEffect;
		}
		return result;
	}

	public bool ClearEnergyOnCast()
	{
		return (!m_abilityMod) ? m_clearEnergyOnCast : m_abilityMod.m_clearEnergyOnCastMod.GetModifiedValue(m_clearEnergyOnCast);
	}

	public int GetSelfEnergyGainOnCast()
	{
		return (!m_abilityMod) ? m_selfEnergyGainOnCast : m_abilityMod.m_selfEnergyGainOnCastMod.GetModifiedValue(m_selfEnergyGainOnCast);
	}

	public int GetCdrOnProtectAllyAbility()
	{
		return (!m_abilityMod) ? m_cdrOnProtectAllyAbility : m_abilityMod.m_cdrOnProtectAllyAbilityMod.GetModifiedValue(m_cdrOnProtectAllyAbility);
	}

	public float GetCurrentAoeRadius(ActorData caster)
	{
		float num = 0f;
		if (m_syncComponent != null)
		{
			num = GetAoeRadiuePerCrystal() * (float)m_syncComponent.SpentDamageCrystals(caster);
		}
		return GetAoeRadiusBase() + num;
	}

	public bool IncludeEnemies()
	{
		int result;
		if (GetDamageBase() <= 0)
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
			if (GetDamagePerCrystal() <= 0)
			{
				result = (GetEnemyHitEffect().m_applyEffect ? 1 : 0);
				goto IL_0035;
			}
		}
		result = 1;
		goto IL_0035;
		IL_0035:
		return (byte)result != 0;
	}

	public bool IncludeAllies()
	{
		int result;
		if (GetAllyHealBase() <= 0 && GetAllyHealPerCrystal() <= 0)
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
			result = (GetAllyHitEffect().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private int GetCurrentAbsorbOnSelf(ActorData caster)
	{
		return GetSelfAbsorbBase() + m_syncComponent.SpentDamageCrystals(caster) * GetSelfAbsorbPerCrystalSpent();
	}

	private int GetCurrentHealingOnSelf(ActorData caster, int numEnemiesHit)
	{
		int num = 0;
		if (GetSelfHealPerEnemyHit() > 0)
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
			num += GetSelfHealPerEnemyHit() * numEnemiesHit;
		}
		if (GetExtraSelfHealPerTurnAtMaxEnergy() > 0)
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
			if (m_syncComponent != null)
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
				if (m_syncComponent.m_syncNumTurnsAtFullEnergy > 1)
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
					int num2 = GetExtraSelfHealPerTurnAtMaxEnergy() * (m_syncComponent.m_syncNumTurnsAtFullEnergy - 1);
					if (GetMaxExtraSelfHealForMaxEnergy() > 0)
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
						num2 = Mathf.Min(GetMaxExtraSelfHealForMaxEnergy(), num2);
					}
					num += num2;
				}
			}
		}
		return GetSelfHealBase() + m_syncComponent.SpentDamageCrystals(caster) * GetSelfHealPerCrystalSpent() + num;
	}

	private int GetCurrentHealingOnAlly(ActorData caster, int numEnemiesHit)
	{
		int num = 0;
		if (GetSelfHealPerEnemyHit() > 0)
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
			num = GetAllyHealPerEnemyHit() * numEnemiesHit;
		}
		return GetAllyHealBase() + m_syncComponent.SpentDamageCrystals(caster) * GetAllyHealPerCrystal() + num;
	}

	private int GetCurrentDamage(ActorData caster)
	{
		return GetDamageBase() + m_syncComponent.SpentDamageCrystals(caster) * GetDamagePerCrystal();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_MartyrSpendCrystals abilityMod_MartyrSpendCrystals = modAsBase as AbilityMod_MartyrSpendCrystals;
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			effectInfo = abilityMod_MartyrSpendCrystals.m_spentCrystalsEffectMod.GetModifiedValue(m_spentCrystalsEffect);
		}
		else
		{
			effectInfo = m_spentCrystalsEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "SpentCrystalsEffect", m_spentCrystalsEffect);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val = abilityMod_MartyrSpendCrystals.m_selfHealBaseMod.GetModifiedValue(m_selfHealBase);
		}
		else
		{
			val = m_selfHealBase;
		}
		AddTokenInt(tokens, "SelfHealBase", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val2 = abilityMod_MartyrSpendCrystals.m_selfHealPerCrystalSpentMod.GetModifiedValue(m_selfHealPerCrystalSpent);
		}
		else
		{
			val2 = m_selfHealPerCrystalSpent;
		}
		AddTokenInt(tokens, "SelfHealPerCrystalSpent", empty2, val2);
		AddTokenInt(tokens, "SelfHealPerEnemyHit", string.Empty, (!abilityMod_MartyrSpendCrystals) ? m_selfHealPerEnemyHit : abilityMod_MartyrSpendCrystals.m_selfHealPerEnemyHitMod.GetModifiedValue(m_selfHealPerEnemyHit));
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val3 = abilityMod_MartyrSpendCrystals.m_extraSelfHealPerTurnAtMaxEnergyMod.GetModifiedValue(m_extraSelfHealPerTurnAtMaxEnergy);
		}
		else
		{
			val3 = m_extraSelfHealPerTurnAtMaxEnergy;
		}
		AddTokenInt(tokens, "ExtraSelfHealPerTurnAtMaxEnergy", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val4 = abilityMod_MartyrSpendCrystals.m_maxExtraSelfHealForMaxEnergyMod.GetModifiedValue(m_maxExtraSelfHealForMaxEnergy);
		}
		else
		{
			val4 = m_maxExtraSelfHealForMaxEnergy;
		}
		AddTokenInt(tokens, "MaxExtraSelfHealForMaxEnergy", empty4, val4);
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val5 = abilityMod_MartyrSpendCrystals.m_selfAbsorbBaseMod.GetModifiedValue(m_selfAbsorbBase);
		}
		else
		{
			val5 = m_selfAbsorbBase;
		}
		AddTokenInt(tokens, "SelfAbsorbBase", empty5, val5);
		string empty6 = string.Empty;
		int val6;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val6 = abilityMod_MartyrSpendCrystals.m_selfAbsorbPerCrystalSpentMod.GetModifiedValue(m_selfAbsorbPerCrystalSpent);
		}
		else
		{
			val6 = m_selfAbsorbPerCrystalSpent;
		}
		AddTokenInt(tokens, "SelfAbsorbPerCrystalSpent", empty6, val6);
		string empty7 = string.Empty;
		float val7;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val7 = abilityMod_MartyrSpendCrystals.m_aoeRadiusBaseMod.GetModifiedValue(m_aoeRadiusBase);
		}
		else
		{
			val7 = m_aoeRadiusBase;
		}
		AddTokenFloat(tokens, "AoeRadiusBase", empty7, val7);
		string empty8 = string.Empty;
		float val8;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val8 = abilityMod_MartyrSpendCrystals.m_aoeRadiuePerCrystalMod.GetModifiedValue(m_aoeRadiuePerCrystal);
		}
		else
		{
			val8 = m_aoeRadiuePerCrystal;
		}
		AddTokenFloat(tokens, "AoeRadiuePerCrystal", empty8, val8);
		string empty9 = string.Empty;
		int val9;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val9 = abilityMod_MartyrSpendCrystals.m_damageBaseMod.GetModifiedValue(m_damageBase);
		}
		else
		{
			val9 = m_damageBase;
		}
		AddTokenInt(tokens, "DamageBase", empty9, val9);
		string empty10 = string.Empty;
		int val10;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val10 = abilityMod_MartyrSpendCrystals.m_damagePerCrystalMod.GetModifiedValue(m_damagePerCrystal);
		}
		else
		{
			val10 = m_damagePerCrystal;
		}
		AddTokenInt(tokens, "DamagePerCrystal", empty10, val10);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			effectInfo2 = abilityMod_MartyrSpendCrystals.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo2 = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EnemyHitEffect", m_enemyHitEffect);
		string empty11 = string.Empty;
		int val11;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val11 = abilityMod_MartyrSpendCrystals.m_allyHealBaseMod.GetModifiedValue(m_allyHealBase);
		}
		else
		{
			val11 = m_allyHealBase;
		}
		AddTokenInt(tokens, "AllyHealBase", empty11, val11);
		string empty12 = string.Empty;
		int val12;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val12 = abilityMod_MartyrSpendCrystals.m_allyHealPerCrystalMod.GetModifiedValue(m_allyHealPerCrystal);
		}
		else
		{
			val12 = m_allyHealPerCrystal;
		}
		AddTokenInt(tokens, "AllyHealPerCrystal", empty12, val12);
		string empty13 = string.Empty;
		int val13;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val13 = abilityMod_MartyrSpendCrystals.m_allyHealPerEnemyHitMod.GetModifiedValue(m_allyHealPerEnemyHit);
		}
		else
		{
			val13 = m_allyHealPerEnemyHit;
		}
		AddTokenInt(tokens, "AllyHealPerEnemyHit", empty13, val13);
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			effectInfo3 = abilityMod_MartyrSpendCrystals.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			effectInfo3 = m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "AllyHitEffect", m_allyHitEffect);
		string empty14 = string.Empty;
		int val14;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val14 = abilityMod_MartyrSpendCrystals.m_selfEnergyGainOnCastMod.GetModifiedValue(m_selfEnergyGainOnCast);
		}
		else
		{
			val14 = m_selfEnergyGainOnCast;
		}
		AddTokenInt(tokens, "SelfEnergyGainOnCast", empty14, val14);
		string empty15 = string.Empty;
		int val15;
		if ((bool)abilityMod_MartyrSpendCrystals)
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
			val15 = abilityMod_MartyrSpendCrystals.m_cdrOnProtectAllyAbilityMod.GetModifiedValue(m_cdrOnProtectAllyAbility);
		}
		else
		{
			val15 = m_cdrOnProtectAllyAbility;
		}
		AddTokenInt(tokens, "CdrOnProtectAllyAbility", empty15, val15);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = base.CalculateNameplateTargetingNumbers();
		GetSpentCrystalsEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, 1);
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, 1);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		ActorData actorData = base.ActorData;
		int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
		if (targetActor == actorData)
		{
			int currentAbsorbOnSelf = GetCurrentAbsorbOnSelf(actorData);
			int currentHealingOnSelf = GetCurrentHealingOnSelf(actorData, visibleActorsCountByTooltipSubject);
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, actorData, currentAbsorbOnSelf, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self);
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, actorData, currentHealingOnSelf, AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self);
		}
		else
		{
			List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
			if (tooltipSubjectTypes != null)
			{
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
				{
					int num = symbolToValue[AbilityTooltipSymbol.Damage] = GetCurrentDamage(actorData);
				}
				else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
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
					int num2 = symbolToValue[AbilityTooltipSymbol.Healing] = GetCurrentHealingOnAlly(actorData, visibleActorsCountByTooltipSubject);
				}
			}
		}
		return symbolToValue;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetSelfEnergyGainOnCast() > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return GetSelfEnergyGainOnCast();
				}
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_syncComponent != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_syncComponent.DamageCrystals > 0;
				}
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrSpendCrystals))
		{
			m_abilityMod = (abilityMod as AbilityMod_MartyrSpendCrystals);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
