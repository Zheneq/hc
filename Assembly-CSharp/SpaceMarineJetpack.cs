using System.Collections.Generic;
using UnityEngine;

public class SpaceMarineJetpack : Ability
{
	public int m_damage = 10;
	public bool m_penetrateLineOfSight;
	[Header("-- Effect on Self --")]
	public StandardEffectInfo m_effectOnSelf;
	public AbilityAreaShape m_landingShape = AbilityAreaShape.Three_x_Three;
	public bool m_applyDebuffs = true;
	public StandardActorEffectData m_debuffData;

	private AbilityMod_SpaceMarineJetpack m_abilityMod;
	private StandardEffectInfo m_cachedEffectOnSelf;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		if (Targeter == null)
		{
			Targeter = new AbilityUtil_Targeter_Jetpack(this, m_landingShape, m_penetrateLineOfSight);
		}
		if (Targeter is AbilityUtil_Targeter_Jetpack abilityUtil_Targeter_Jetpack)
		{
			abilityUtil_Targeter_Jetpack.m_affectsCaster =
				HasAbsorbOnCasterPerEnemyHit()
				|| GetEffectOnSelf().m_applyEffect
				|| m_abilityMod != null && m_abilityMod.m_effectToSelfOnCast.m_applyEffect
					? AbilityUtil_Targeter.AffectsActor.Always
					: AbilityUtil_Targeter.AffectsActor.Never;
		}
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnSelf = m_abilityMod != null
			? m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf)
			: m_effectOnSelf;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		return m_cachedEffectOnSelf ?? m_effectOnSelf;
	}

	private int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public int CooldownResetHealthThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownResetThreshold.GetModifiedValue(0)
			: 0;
	}

	private bool HasAbsorbOnCasterPerEnemyHit()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_effectOnCasterPerEnemyHit.m_applyEffect
		       && m_abilityMod.m_effectOnCasterPerEnemyHit.m_effectData.m_absorbAmount > 0;
	}

	private StandardActorEffectData GetEffectOnEnemies()
	{
		StandardActorEffectData standardActorEffectData = m_applyDebuffs ? m_debuffData : null;
		return m_abilityMod != null
			? m_abilityMod.m_additionalEffectOnEnemy.GetModifiedValue(standardActorEffectData)
			: standardActorEffectData;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetEffectOnSelf().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damage);
		GetEffectOnEnemies()?.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamage());
		GetEffectOnEnemies()?.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		if (HasAbsorbOnCasterPerEnemyHit())
		{
			AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		}
		else
		{
			GetEffectOnSelf().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (HasAbsorbOnCasterPerEnemyHit())
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>
			{
				[AbilityTooltipSymbol.Absorb] = (Targeter.GetActorsInRange().Count - 1) * m_abilityMod.m_effectOnCasterPerEnemyHit.m_effectData.m_absorbAmount
			};
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineJetpack abilityMod_SpaceMarineJetpack = modAsBase as AbilityMod_SpaceMarineJetpack;
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SpaceMarineJetpack != null
			? abilityMod_SpaceMarineJetpack.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf)
			: m_effectOnSelf, "EffectOnSelf", m_effectOnSelf);
		AddTokenInt(tokens, "Damage", string.Empty, abilityMod_SpaceMarineJetpack != null
			? abilityMod_SpaceMarineJetpack.m_damageMod.GetModifiedValue(m_damage)
			: m_damage);
		m_debuffData.AddTooltipTokens(tokens, "DebuffData");
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SpaceMarineJetpack))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = abilityMod as AbilityMod_SpaceMarineJetpack;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
