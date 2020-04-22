using System.Collections.Generic;
using UnityEngine;

public class SorceressHealingKnockback : Ability
{
	[Header("-- On Cast")]
	public int m_onCastHealAmount;

	public int m_onCastAllyEnergyGain;

	[Header("-- On Detonate")]
	public int m_onDetonateDamageAmount;

	public StandardEffectInfo m_onDetonateEnemyEffect;

	public float m_knockbackDistance;

	public bool m_penetrateLoS;

	public AbilityAreaShape m_aoeShape;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	public GameObject m_effectSequence;

	public GameObject m_detonateSequence;

	public GameObject m_detonateGameplayHitSequence;

	private AbilityMod_SorceressHealingKnockback m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Always;
		base.Targeter = new AbilityUtil_Targeter_HealingKnockback(this, m_aoeShape, m_penetrateLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, true, affectsCaster, affectsBestTarget, GetKnockbackDistance(), m_knockbackType);
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		if (m_onCastHealAmount > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Primary, m_onCastHealAmount));
		}
		if (m_onDetonateDamageAmount > 0)
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
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_onDetonateDamageAmount));
		}
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		if (m_onCastHealAmount > 0)
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
			number.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Primary, m_onCastHealAmount));
		}
		if (GetDamageAmount() > 0)
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
			number.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, GetDamageAmount()));
		}
		if (GetOnCastAllyEnergyGain() > 0)
		{
			AbilityTooltipHelper.ReportEnergy(ref number, AbilityTooltipSubject.Ally, GetOnCastAllyEnergyGain());
		}
		return number;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
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
				dictionary[AbilityTooltipSymbol.Healing] = GetHealAmount(targetActor);
			}
		}
		return dictionary;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, false, true, true, ValidateCheckPath.Ignore, true, true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressHealingKnockback abilityMod_SorceressHealingKnockback = modAsBase as AbilityMod_SorceressHealingKnockback;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SorceressHealingKnockback)
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
			val = abilityMod_SorceressHealingKnockback.m_normalHealingMod.GetModifiedValue(m_onCastHealAmount);
		}
		else
		{
			val = m_onCastHealAmount;
		}
		AddTokenInt(tokens, "OnCastHealAmount_Normal", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_SorceressHealingKnockback)
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
			val2 = abilityMod_SorceressHealingKnockback.m_lowHealthHealingMod.GetModifiedValue(m_onCastHealAmount);
		}
		else
		{
			val2 = m_onCastHealAmount;
		}
		AddTokenInt(tokens, "OnCastHealAmount_LowHealth", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_SorceressHealingKnockback)
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
			val3 = abilityMod_SorceressHealingKnockback.m_onCastAllyEnergyGainMod.GetModifiedValue(m_onCastAllyEnergyGain);
		}
		else
		{
			val3 = m_onCastAllyEnergyGain;
		}
		AddTokenInt(tokens, "OnCastAllyEnergyGain", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_SorceressHealingKnockback)
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
			val4 = abilityMod_SorceressHealingKnockback.m_damageMod.GetModifiedValue(m_onDetonateDamageAmount);
		}
		else
		{
			val4 = m_onDetonateDamageAmount;
		}
		AddTokenInt(tokens, "OnDetonateDamageAmount", empty4, val4);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_SorceressHealingKnockback)
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
			effectInfo = abilityMod_SorceressHealingKnockback.m_enemyHitEffectOverride.GetModifiedValue(m_onDetonateEnemyEffect);
		}
		else
		{
			effectInfo = m_onDetonateEnemyEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "OnDetonateEnemyEffect", m_onDetonateEnemyEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SorceressHealingKnockback))
		{
			m_abilityMod = (abilityMod as AbilityMod_SorceressHealingKnockback);
			SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private int GetHealAmount(ActorData target)
	{
		int result = m_onCastHealAmount;
		if (m_abilityMod != null)
		{
			float num = (float)target.HitPoints / (float)target.GetMaxHitPoints();
			if (num < m_abilityMod.m_lowHealthThreshold)
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
				result = m_abilityMod.m_lowHealthHealingMod.GetModifiedValue(m_onCastHealAmount);
			}
			else
			{
				result = m_abilityMod.m_normalHealingMod.GetModifiedValue(m_onCastHealAmount);
			}
		}
		return result;
	}

	public int GetOnCastAllyEnergyGain()
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
			result = m_abilityMod.m_onCastAllyEnergyGainMod.GetModifiedValue(m_onCastAllyEnergyGain);
		}
		else
		{
			result = m_onCastAllyEnergyGain;
		}
		return result;
	}

	private int GetDamageAmount()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_onDetonateDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_onDetonateDamageAmount);
		}
		return result;
	}

	private StandardEffectInfo GetOnDetonateEnemyEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
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
			result = m_onDetonateEnemyEffect;
		}
		else
		{
			result = m_abilityMod.m_enemyHitEffectOverride.GetModifiedValue(m_onDetonateEnemyEffect);
		}
		return result;
	}

	private float GetKnockbackDistance()
	{
		float result;
		if (m_abilityMod == null)
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
			result = m_knockbackDistance;
		}
		else
		{
			result = m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance);
		}
		return result;
	}
}
