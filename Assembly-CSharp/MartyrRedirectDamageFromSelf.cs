using System.Collections.Generic;
using UnityEngine;

public class MartyrRedirectDamageFromSelf : MartyrLaserBase
{
	[Header("-- Damage reduction and redirection")]
	public float m_damageReductionOnCaster = 0.5f;

	public float m_damageRedirectToTarget = 0.5f;

	public int m_techPointGainPerRedirect = 3;

	public StandardEffectInfo m_selfHitEffect;

	public bool m_affectsEnemies = true;

	public bool m_affectsAllies;

	public bool m_penetratesLoS;

	public StandardEffectInfo m_effectOnTarget;

	[Header("-- Self protection")]
	public int m_baseAbsorb;

	public int m_absorbPerCrystalSpent = 5;

	public List<MartyrProtectAllyThreshold> m_thresholdBasedCrystalBonuses;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	public GameObject m_projectileSequence;

	public GameObject m_redirectProjectileSequence;

	private Martyr_SyncComponent m_syncComponent;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private StandardEffectInfo m_cachedEffectOnTarget;

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
			m_abilityName = "Martyr Redirect Damage From Self";
		}
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		SetCachedFields();
		SetupTargeter();
		ResetTooltipAndTargetingNumbers();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return m_syncComponent;
	}

	protected void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, GetPenetratesLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, m_affectsEnemies, m_affectsAllies, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Always);
		(base.Targeter as AbilityUtil_Targeter_Shape).m_affectCasterDelegate = delegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
		{
			int currentAbsorb = GetCurrentAbsorb(caster);
			return currentAbsorb > 0;
		};
	}

	private void SetCachedFields()
	{
		m_cachedSelfHitEffect = m_selfHitEffect;
		m_cachedEffectOnTarget = m_effectOnTarget;
	}

	public float GetDamageReductionOnCaster()
	{
		return m_damageReductionOnCaster;
	}

	public float GetDamageRedirectToTarget()
	{
		return m_damageRedirectToTarget;
	}

	public int GetTechPointGainPerRedirect()
	{
		return m_techPointGainPerRedirect;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedSelfHitEffect != null)
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
			result = m_cachedSelfHitEffect;
		}
		else
		{
			result = m_selfHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnTarget()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnTarget != null)
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
			result = m_cachedEffectOnTarget;
		}
		else
		{
			result = m_effectOnTarget;
		}
		return result;
	}

	public int GetAbsorbAmountPerCrystalSpent()
	{
		return m_absorbPerCrystalSpent;
	}

	public int GetBaseAbsorbAmount()
	{
		return m_baseAbsorb;
	}

	public bool GetPenetratesLoS()
	{
		return m_penetratesLoS;
	}

	public float GetMaxRange()
	{
		return GetRangeInSquares(0);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod.AddToken_EffectInfo(tokens, m_selfHitEffect, "SelfEffect", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTarget, "TargetEffect", m_effectOnTarget);
		tokens.Add(new TooltipTokenInt("BaseAbsorb", "Absorb with no crystal bonus", GetBaseAbsorbAmount()));
		tokens.Add(new TooltipTokenInt("AbsorbPerCrystal", "Absorb added per crystal spent", GetAbsorbAmountPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("WidthPerCrystal", "Width added per crystal spent", GetBonusWidthPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("LengthPerCrystal", "Length added per crystal spent", GetBonusLengthPerCrystalSpent()));
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = base.CalculateNameplateTargetingNumbers();
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
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
			int currentAbsorb = GetCurrentAbsorb(base.ActorData);
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, base.ActorData, currentAbsorb, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self);
		}
		return symbolToValue;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(caster, m_affectsEnemies, m_affectsAllies, false, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, m_affectsEnemies, m_affectsAllies, false, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
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
				switch (5)
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
		return GetBaseAbsorbAmount() + m_syncComponent.SpentDamageCrystals(caster) * GetAbsorbAmountPerCrystalSpent() + num2;
	}
}
