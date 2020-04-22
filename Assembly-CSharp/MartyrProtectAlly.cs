using System.Collections.Generic;
using UnityEngine;

public class MartyrProtectAlly : MartyrLaserBase
{
	[Header("-- Damage reduction and redirection")]
	public float m_damageReductionOnTarget = 0.5f;

	public float m_damageRedirectToCaster = 0.5f;

	public int m_techPointGainPerRedirect = 3;

	public StandardEffectInfo m_laserHitEffect;

	[Space(10f)]
	public bool m_affectsEnemies;

	public bool m_affectsAllies = true;

	public bool m_penetratesLoS;

	[Header("-- Thorns effect on protected ally")]
	public StandardEffectInfo m_thornsEffect;

	public StandardEffectInfo m_returnEffectOnEnemy;

	public int m_thornsDamagePerHit;

	[Header("-- Absorb & Crystal Bonuses, Self")]
	public StandardEffectInfo m_effectOnSelf;

	public int m_baseAbsorb;

	public int m_absorbPerCrystalSpent = 5;

	[Header("-- Absorb on Ally")]
	public int m_baseAbsorbOnAlly;

	public int m_absorbOnAllyPerCrystalSpent = 5;

	public List<MartyrProtectAllyThreshold> m_thresholdBasedCrystalBonuses;

	[Header("-- Extra Energy per damage redirect")]
	public float m_extraEnergyPerRedirectDamage;

	[Header("-- Heal per damage redirect on next turn")]
	public float m_healOnTurnStartPerRedirectDamage;

	[Header("-- Sequences")]
	public GameObject m_allyShieldSequence;

	public GameObject m_projectileSequence;

	public GameObject m_redirectProjectileSequence;

	public GameObject m_thornsProjectileSequence;

	[Tooltip("Ignored if no effect or absorb is applied on the caster")]
	public GameObject m_selfShieldSequence;

	private Martyr_SyncComponent m_syncComponent;

	private AbilityMod_MartyrProtectAlly m_abilityMod;

	private StandardEffectInfo m_cachedLaserHitEffect;

	private StandardEffectInfo m_cachedThornsEffect;

	private StandardEffectInfo m_cachedReturnEffectOnEnemy;

	private StandardEffectInfo m_cachedEffectOnSelf;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Martyr Protect Ally";
		}
		Setup();
		ResetTooltipAndTargetingNumbers();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return m_syncComponent;
	}

	protected void Setup()
	{
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, PenetratesLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, AffectsEnemies(), AffectsAllies(), AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Always);
		(base.Targeter as AbilityUtil_Targeter_Shape).m_affectCasterDelegate = delegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
		{
			int currentAbsorb = GetCurrentAbsorb(caster);
			return currentAbsorb > 0;
		};
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedLaserHitEffect;
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
			cachedLaserHitEffect = m_abilityMod.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect);
		}
		else
		{
			cachedLaserHitEffect = m_laserHitEffect;
		}
		m_cachedLaserHitEffect = cachedLaserHitEffect;
		StandardEffectInfo cachedThornsEffect;
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
			cachedThornsEffect = m_abilityMod.m_thornsEffectMod.GetModifiedValue(m_thornsEffect);
		}
		else
		{
			cachedThornsEffect = m_thornsEffect;
		}
		m_cachedThornsEffect = cachedThornsEffect;
		m_cachedReturnEffectOnEnemy = ((!m_abilityMod) ? m_returnEffectOnEnemy : m_abilityMod.m_returnEffectOnEnemyMod.GetModifiedValue(m_returnEffectOnEnemy));
		StandardEffectInfo cachedEffectOnSelf;
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
			cachedEffectOnSelf = m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf);
		}
		else
		{
			cachedEffectOnSelf = m_effectOnSelf;
		}
		m_cachedEffectOnSelf = cachedEffectOnSelf;
	}

	public float GetDamageReductionOnTarget()
	{
		return (!m_abilityMod) ? m_damageReductionOnTarget : m_abilityMod.m_damageReductionOnTargetMod.GetModifiedValue(m_damageReductionOnTarget);
	}

	public float GetDamageRedirectToCaster()
	{
		return (!m_abilityMod) ? m_damageRedirectToCaster : m_abilityMod.m_damageRedirectToCasterMod.GetModifiedValue(m_damageRedirectToCaster);
	}

	public int GetTechPointGainPerRedirect()
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
			result = m_abilityMod.m_techPointGainPerRedirectMod.GetModifiedValue(m_techPointGainPerRedirect);
		}
		else
		{
			result = m_techPointGainPerRedirect;
		}
		return result;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedLaserHitEffect != null)
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
			result = m_cachedLaserHitEffect;
		}
		else
		{
			result = m_laserHitEffect;
		}
		return result;
	}

	public bool AffectsEnemies()
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
			result = m_abilityMod.m_affectsEnemiesMod.GetModifiedValue(m_affectsEnemies);
		}
		else
		{
			result = m_affectsEnemies;
		}
		return result;
	}

	public bool AffectsAllies()
	{
		bool result;
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
			result = m_abilityMod.m_affectsAlliesMod.GetModifiedValue(m_affectsAllies);
		}
		else
		{
			result = m_affectsAllies;
		}
		return result;
	}

	public bool PenetratesLoS()
	{
		return (!m_abilityMod) ? m_penetratesLoS : m_abilityMod.m_penetratesLoSMod.GetModifiedValue(m_penetratesLoS);
	}

	public StandardEffectInfo GetThornsEffect()
	{
		StandardEffectInfo result;
		if (m_cachedThornsEffect != null)
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
			result = m_cachedThornsEffect;
		}
		else
		{
			result = m_thornsEffect;
		}
		return result;
	}

	public StandardEffectInfo GetReturnEffectOnEnemy()
	{
		StandardEffectInfo result;
		if (m_cachedReturnEffectOnEnemy != null)
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
			result = m_cachedReturnEffectOnEnemy;
		}
		else
		{
			result = m_returnEffectOnEnemy;
		}
		return result;
	}

	public int GetThornsDamagePerHit()
	{
		int result;
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
			result = m_abilityMod.m_thornsDamagePerHitMod.GetModifiedValue(m_thornsDamagePerHit);
		}
		else
		{
			result = m_thornsDamagePerHit;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnSelf != null)
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
			result = m_cachedEffectOnSelf;
		}
		else
		{
			result = m_effectOnSelf;
		}
		return result;
	}

	public int GetBaseAbsorb()
	{
		int result;
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
			result = m_abilityMod.m_baseAbsorbMod.GetModifiedValue(m_baseAbsorb);
		}
		else
		{
			result = m_baseAbsorb;
		}
		return result;
	}

	public int GetAbsorbPerCrystalSpent()
	{
		return (!m_abilityMod) ? m_absorbPerCrystalSpent : m_abilityMod.m_absorbPerCrystalSpentMod.GetModifiedValue(m_absorbPerCrystalSpent);
	}

	public int GetBaseAbsorbOnAlly()
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
			result = m_abilityMod.m_baseAbsorbOnAllyMod.GetModifiedValue(m_baseAbsorbOnAlly);
		}
		else
		{
			result = m_baseAbsorbOnAlly;
		}
		return result;
	}

	public int GetAbsorbOnAllyPerCrystalSpent()
	{
		return (!m_abilityMod) ? m_absorbOnAllyPerCrystalSpent : m_abilityMod.m_absorbOnAllyPerCrystalSpentMod.GetModifiedValue(m_absorbOnAllyPerCrystalSpent);
	}

	public float GetExtraEnergyPerRedirectDamage()
	{
		float result;
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
			result = m_abilityMod.m_extraEnergyPerRedirectDamageMod.GetModifiedValue(m_extraEnergyPerRedirectDamage);
		}
		else
		{
			result = m_extraEnergyPerRedirectDamage;
		}
		return result;
	}

	public float GetHealOnTurnStartPerRedirectDamage()
	{
		return (!m_abilityMod) ? m_healOnTurnStartPerRedirectDamage : m_abilityMod.m_healOnTurnStartPerRedirectDamageMod.GetModifiedValue(m_healOnTurnStartPerRedirectDamage);
	}

	private int GetCurrentAbsorb(ActorData caster)
	{
		MartyrProtectAllyThreshold martyrProtectAllyThreshold = GetCurrentPowerEntry(caster) as MartyrProtectAllyThreshold;
		int num;
		if (martyrProtectAllyThreshold != null)
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
			num = martyrProtectAllyThreshold.m_additionalAbsorb;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return GetBaseAbsorb() + m_syncComponent.SpentDamageCrystals(caster) * GetAbsorbPerCrystalSpent() + num2;
	}

	private int GetCurrentAbsorbForAlly(ActorData caster)
	{
		MartyrProtectAllyThreshold martyrProtectAllyThreshold = GetCurrentPowerEntry(caster) as MartyrProtectAllyThreshold;
		int num;
		if (martyrProtectAllyThreshold != null)
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
			num = martyrProtectAllyThreshold.m_additionalAbsorbOnAlly;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return GetBaseAbsorbOnAlly() + m_syncComponent.SpentDamageCrystals(caster) * GetAbsorbOnAllyPerCrystalSpent() + num2;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_MartyrProtectAlly abilityMod_MartyrProtectAlly = modAsBase as AbilityMod_MartyrProtectAlly;
		string empty = string.Empty;
		float val;
		if ((bool)abilityMod_MartyrProtectAlly)
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
			val = abilityMod_MartyrProtectAlly.m_damageReductionOnTargetMod.GetModifiedValue(m_damageReductionOnTarget);
		}
		else
		{
			val = m_damageReductionOnTarget;
		}
		AddTokenFloatAsPct(tokens, "DamageReductionOnTarget_Pct", empty, val);
		string empty2 = string.Empty;
		float val2;
		if ((bool)abilityMod_MartyrProtectAlly)
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
			val2 = abilityMod_MartyrProtectAlly.m_damageRedirectToCasterMod.GetModifiedValue(m_damageRedirectToCaster);
		}
		else
		{
			val2 = m_damageRedirectToCaster;
		}
		AddTokenFloatAsPct(tokens, "DamageRedirectToCaster_Pct", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_MartyrProtectAlly)
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
			val3 = abilityMod_MartyrProtectAlly.m_techPointGainPerRedirectMod.GetModifiedValue(m_techPointGainPerRedirect);
		}
		else
		{
			val3 = m_techPointGainPerRedirect;
		}
		AddTokenInt(tokens, "TechPointGainPerRedirect", empty3, val3);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_MartyrProtectAlly) ? m_laserHitEffect : abilityMod_MartyrProtectAlly.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect), "LaserHitEffect", m_laserHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_MartyrProtectAlly) ? m_thornsEffect : abilityMod_MartyrProtectAlly.m_thornsEffectMod.GetModifiedValue(m_thornsEffect), "ThornsEffect", m_thornsEffect);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_MartyrProtectAlly)
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
			effectInfo = abilityMod_MartyrProtectAlly.m_returnEffectOnEnemyMod.GetModifiedValue(m_returnEffectOnEnemy);
		}
		else
		{
			effectInfo = m_returnEffectOnEnemy;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "ReturnEffectOnEnemy", m_returnEffectOnEnemy);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_MartyrProtectAlly)
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
			val4 = abilityMod_MartyrProtectAlly.m_thornsDamagePerHitMod.GetModifiedValue(m_thornsDamagePerHit);
		}
		else
		{
			val4 = m_thornsDamagePerHit;
		}
		AddTokenInt(tokens, "ThornsDamagePerHit", empty4, val4);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_MartyrProtectAlly)
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
			effectInfo2 = abilityMod_MartyrProtectAlly.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf);
		}
		else
		{
			effectInfo2 = m_effectOnSelf;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnSelf", m_effectOnSelf);
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_MartyrProtectAlly)
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
			val5 = abilityMod_MartyrProtectAlly.m_baseAbsorbMod.GetModifiedValue(m_baseAbsorb);
		}
		else
		{
			val5 = m_baseAbsorb;
		}
		AddTokenInt(tokens, "BaseAbsorb", empty5, val5);
		AddTokenInt(tokens, "AbsorbPerCrystalSpent", string.Empty, (!abilityMod_MartyrProtectAlly) ? m_absorbPerCrystalSpent : abilityMod_MartyrProtectAlly.m_absorbPerCrystalSpentMod.GetModifiedValue(m_absorbPerCrystalSpent));
		string empty6 = string.Empty;
		int val6;
		if ((bool)abilityMod_MartyrProtectAlly)
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
			val6 = abilityMod_MartyrProtectAlly.m_baseAbsorbOnAllyMod.GetModifiedValue(m_baseAbsorbOnAlly);
		}
		else
		{
			val6 = m_baseAbsorbOnAlly;
		}
		AddTokenInt(tokens, "BaseAbsorbOnAlly", empty6, val6);
		string empty7 = string.Empty;
		int val7;
		if ((bool)abilityMod_MartyrProtectAlly)
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
			val7 = abilityMod_MartyrProtectAlly.m_absorbOnAllyPerCrystalSpentMod.GetModifiedValue(m_absorbOnAllyPerCrystalSpent);
		}
		else
		{
			val7 = m_absorbOnAllyPerCrystalSpent;
		}
		AddTokenInt(tokens, "AbsorbOnAllyPerCrystalSpent", empty7, val7);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetLaserHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		GetEffectOnSelf().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = base.CalculateNameplateTargetingNumbers();
		GetLaserHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Ally, 1);
		GetEffectOnSelf().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor == base.ActorData)
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
			int currentAbsorb = GetCurrentAbsorb(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, currentAbsorb, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self);
		}
		else
		{
			int currentAbsorbForAlly = GetCurrentAbsorbForAlly(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, currentAbsorbForAlly, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Ally);
		}
		return symbolToValue;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(caster, AffectsEnemies(), AffectsAllies(), false, ValidateCheckPath.Ignore, !PenetratesLoS(), false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, AffectsEnemies(), AffectsAllies(), false, ValidateCheckPath.Ignore, !PenetratesLoS(), false);
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		using (List<MartyrProtectAllyThreshold>.Enumerator enumerator = m_thresholdBasedCrystalBonuses.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MartyrProtectAllyThreshold current = enumerator.Current;
				list.Add(current);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return list;
					}
					/*OpCode not supported: LdMemberToken*/;
					return list;
				}
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_MartyrProtectAlly))
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
			m_abilityMod = (abilityMod as AbilityMod_MartyrProtectAlly);
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
