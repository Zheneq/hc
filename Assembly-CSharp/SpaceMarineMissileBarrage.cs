using System.Collections.Generic;
using UnityEngine;

public class SpaceMarineMissileBarrage : Ability
{
	[Space(10f)]
	public int m_missiles = 3;

	public int m_damage = 3;

	public float m_radius = 5f;

	public bool m_penetrateLineOfSight;

	public AbilityPriority m_damageRunPriority;

	public StandardEffectInfo m_effectOnTargets;

	[Separator("Whether damage can be reacted to", true)]
	public bool m_considerDamageAsDirect;

	[Separator("Sequences and Anim", true)]
	public GameObject m_buffSequence;

	public GameObject m_missileSequence;

	public int m_missileLaunchAnimIndex = 11;

	private AbilityMod_SpaceMarineMissileBarrage m_abilityMod;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, m_radius, m_penetrateLineOfSight, true, false, m_missiles);
		AbilityUtil_Targeter targeter = base.Targeter;
		int affectsCaster;
		if (GetModdedEffectForSelf() != null)
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
			affectsCaster = (GetModdedEffectForSelf().m_applyEffect ? 1 : 0);
		}
		else
		{
			affectsCaster = 0;
		}
		targeter.SetAffectedGroups(true, false, (byte)affectsCaster != 0);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, ModdedDamage()));
		ModdedEffectOnTargets().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int numActorsInRange = base.Targeter.GetNumActorsInRange();
		dictionary.Add(AbilityTooltipSymbol.Damage, ModdedDamage() + ModdedExtraDamagePerTarget() * (numActorsInRange - 1));
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineMissileBarrage abilityMod_SpaceMarineMissileBarrage = modAsBase as AbilityMod_SpaceMarineMissileBarrage;
		AddTokenInt(tokens, "DelayTurns", string.Empty, 1);
		AddTokenInt(tokens, "Missiles", string.Empty, m_missiles);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SpaceMarineMissileBarrage)
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
			val = abilityMod_SpaceMarineMissileBarrage.m_damageMod.GetModifiedValue(m_damage);
		}
		else
		{
			val = m_damage;
		}
		AddTokenInt(tokens, "Damage", empty, val);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTargets, "EffectOnTargets");
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		int result;
		if (animIndex != m_missileLaunchAnimIndex)
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
			result = (base.CanTriggerAnimAtIndexForTaunt(animIndex) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SpaceMarineMissileBarrage))
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
			m_abilityMod = (abilityMod as AbilityMod_SpaceMarineMissileBarrage);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public int ModdedDamage()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_damageMod.GetModifiedValue(m_damage) : m_damage;
	}

	public int ModdedMissileActiveDuration()
	{
		int result;
		if (m_abilityMod == null)
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
			result = 1;
		}
		else
		{
			result = m_abilityMod.m_activeDurationMod.GetModifiedValue(1);
		}
		return result;
	}

	public StandardEffectInfo ModdedEffectOnTargets()
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_abilityMod.m_missileHitEffectOverride.m_applyEffect)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return m_abilityMod.m_missileHitEffectOverride;
					}
				}
			}
		}
		return m_effectOnTargets;
	}

	public int ModdedExtraDamagePerTarget()
	{
		int result;
		if (m_abilityMod == null)
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
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_extraDamagePerTarget.GetModifiedValue(0);
		}
		return result;
	}
}
