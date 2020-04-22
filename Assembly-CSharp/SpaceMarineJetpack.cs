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
		if (base.Targeter == null)
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
			base.Targeter = new AbilityUtil_Targeter_Jetpack(this, m_landingShape, m_penetrateLineOfSight);
		}
		AbilityUtil_Targeter_Jetpack abilityUtil_Targeter_Jetpack = base.Targeter as AbilityUtil_Targeter_Jetpack;
		if (abilityUtil_Targeter_Jetpack == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			int num;
			if (!HasAbsorbOnCasterPerEnemyHit())
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
				if (!GetEffectOnSelf().m_applyEffect)
				{
					if (m_abilityMod != null)
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
						num = (m_abilityMod.m_effectToSelfOnCast.m_applyEffect ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					goto IL_00a8;
				}
			}
			num = 1;
			goto IL_00a8;
			IL_00a8:
			int affectsCaster;
			if (num != 0)
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
				affectsCaster = 2;
			}
			else
			{
				affectsCaster = 0;
			}
			abilityUtil_Targeter_Jetpack.m_affectsCaster = (AbilityUtil_Targeter.AffectsActor)affectsCaster;
			return;
		}
	}

	private void SetCachedFields()
	{
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedEffectOnSelf = m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf);
		}
		else
		{
			cachedEffectOnSelf = m_effectOnSelf;
		}
		m_cachedEffectOnSelf = cachedEffectOnSelf;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnSelf != null)
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
			result = m_cachedEffectOnSelf;
		}
		else
		{
			result = m_effectOnSelf;
		}
		return result;
	}

	private int GetDamage()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_damage;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
		}
		return result;
	}

	public int CooldownResetHealthThreshold()
	{
		int result;
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
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_cooldownResetThreshold.GetModifiedValue(0);
		}
		return result;
	}

	private bool HasAbsorbOnCasterPerEnemyHit()
	{
		int result;
		if (m_abilityMod != null)
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
			if (m_abilityMod.m_effectOnCasterPerEnemyHit.m_applyEffect)
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
				result = ((m_abilityMod.m_effectOnCasterPerEnemyHit.m_effectData.m_absorbAmount > 0) ? 1 : 0);
				goto IL_005a;
			}
		}
		result = 0;
		goto IL_005a;
		IL_005a:
		return (byte)result != 0;
	}

	private StandardActorEffectData GetEffectOnEnemies()
	{
		StandardActorEffectData standardActorEffectData = null;
		if (m_applyDebuffs)
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
			standardActorEffectData = m_debuffData;
		}
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_additionalEffectOnEnemy.GetModifiedValue(standardActorEffectData);
				}
			}
		}
		return standardActorEffectData;
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
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeter.GetActorsInRange();
			int num = actorsInRange.Count - 1;
			int num3 = dictionary[AbilityTooltipSymbol.Absorb] = num * m_abilityMod.m_effectOnCasterPerEnemyHit.m_effectData.m_absorbAmount;
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineJetpack abilityMod_SpaceMarineJetpack = modAsBase as AbilityMod_SpaceMarineJetpack;
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_SpaceMarineJetpack)
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
			effectInfo = abilityMod_SpaceMarineJetpack.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf);
		}
		else
		{
			effectInfo = m_effectOnSelf;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnSelf", m_effectOnSelf);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SpaceMarineJetpack)
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
			val = abilityMod_SpaceMarineJetpack.m_damageMod.GetModifiedValue(m_damage);
		}
		else
		{
			val = m_damage;
		}
		AddTokenInt(tokens, "Damage", empty, val);
		m_debuffData.AddTooltipTokens(tokens, "DebuffData");
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SpaceMarineJetpack))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_SpaceMarineJetpack);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
