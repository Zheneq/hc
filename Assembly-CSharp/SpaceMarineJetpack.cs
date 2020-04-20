using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMarineJetpack : Ability
{
	public int m_damage = 0xA;

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
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		if (base.Targeter == null)
		{
			base.Targeter = new AbilityUtil_Targeter_Jetpack(this, this.m_landingShape, this.m_penetrateLineOfSight);
		}
		AbilityUtil_Targeter_Jetpack abilityUtil_Targeter_Jetpack = base.Targeter as AbilityUtil_Targeter_Jetpack;
		if (abilityUtil_Targeter_Jetpack != null)
		{
			bool flag;
			if (!this.HasAbsorbOnCasterPerEnemyHit())
			{
				if (!this.GetEffectOnSelf().m_applyEffect)
				{
					if (this.m_abilityMod != null)
					{
						flag = this.m_abilityMod.m_effectToSelfOnCast.m_applyEffect;
					}
					else
					{
						flag = false;
					}
					goto IL_A8;
				}
			}
			flag = true;
			IL_A8:
			bool flag2 = flag;
			AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = abilityUtil_Targeter_Jetpack;
			AbilityUtil_Targeter.AffectsActor affectsCaster;
			if (flag2)
			{
				affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
			}
			else
			{
				affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
			}
			abilityUtil_Targeter_Shape.m_affectsCaster = affectsCaster;
		}
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnSelf;
		if (this.m_abilityMod)
		{
			cachedEffectOnSelf = this.m_abilityMod.m_effectOnSelfMod.GetModifiedValue(this.m_effectOnSelf);
		}
		else
		{
			cachedEffectOnSelf = this.m_effectOnSelf;
		}
		this.m_cachedEffectOnSelf = cachedEffectOnSelf;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnSelf != null)
		{
			result = this.m_cachedEffectOnSelf;
		}
		else
		{
			result = this.m_effectOnSelf;
		}
		return result;
	}

	private int GetDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_damage;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
		}
		return result;
	}

	public int CooldownResetHealthThreshold()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_cooldownResetThreshold.GetModifiedValue(0);
		}
		return result;
	}

	private bool HasAbsorbOnCasterPerEnemyHit()
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_effectOnCasterPerEnemyHit.m_applyEffect)
			{
				return this.m_abilityMod.m_effectOnCasterPerEnemyHit.m_effectData.m_absorbAmount > 0;
			}
		}
		return false;
	}

	private StandardActorEffectData GetEffectOnEnemies()
	{
		StandardActorEffectData standardActorEffectData = null;
		if (this.m_applyDebuffs)
		{
			standardActorEffectData = this.m_debuffData;
		}
		if (this.m_abilityMod != null)
		{
			return this.m_abilityMod.m_additionalEffectOnEnemy.GetModifiedValue(standardActorEffectData);
		}
		return standardActorEffectData;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetEffectOnSelf().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.m_damage);
		StandardActorEffectData effectOnEnemies = this.GetEffectOnEnemies();
		if (effectOnEnemies != null)
		{
			effectOnEnemies.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamage());
		StandardActorEffectData effectOnEnemies = this.GetEffectOnEnemies();
		if (effectOnEnemies != null)
		{
			effectOnEnemies.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		}
		if (this.HasAbsorbOnCasterPerEnemyHit())
		{
			AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Self, 1);
		}
		else
		{
			this.GetEffectOnSelf().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		}
		base.AppendTooltipNumbersFromBaseModEffects(ref result, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (this.HasAbsorbOnCasterPerEnemyHit())
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeter.GetActorsInRange();
			int num = actorsInRange.Count - 1;
			int value = num * this.m_abilityMod.m_effectOnCasterPerEnemyHit.m_effectData.m_absorbAmount;
			dictionary[AbilityTooltipSymbol.Absorb] = value;
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineJetpack abilityMod_SpaceMarineJetpack = modAsBase as AbilityMod_SpaceMarineJetpack;
		StandardEffectInfo effectInfo;
		if (abilityMod_SpaceMarineJetpack)
		{
			effectInfo = abilityMod_SpaceMarineJetpack.m_effectOnSelfMod.GetModifiedValue(this.m_effectOnSelf);
		}
		else
		{
			effectInfo = this.m_effectOnSelf;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnSelf", this.m_effectOnSelf, true);
		string name = "Damage";
		string empty = string.Empty;
		int val;
		if (abilityMod_SpaceMarineJetpack)
		{
			val = abilityMod_SpaceMarineJetpack.m_damageMod.GetModifiedValue(this.m_damage);
		}
		else
		{
			val = this.m_damage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		this.m_debuffData.AddTooltipTokens(tokens, "DebuffData", false, null);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SpaceMarineJetpack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SpaceMarineJetpack);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
