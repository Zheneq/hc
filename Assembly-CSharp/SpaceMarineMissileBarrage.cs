using System;
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

	public int m_missileLaunchAnimIndex = 0xB;

	private AbilityMod_SpaceMarineMissileBarrage m_abilityMod;

	private void Start()
	{
		this.Setup();
	}

	private void Setup()
	{
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, this.m_radius, this.m_penetrateLineOfSight, true, false, this.m_missiles);
		AbilityUtil_Targeter targeter = base.Targeter;
		bool affectsEnemies = true;
		bool affectsAllies = false;
		bool affectsCaster;
		if (base.GetModdedEffectForSelf() != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpaceMarineMissileBarrage.Setup()).MethodHandle;
			}
			affectsCaster = base.GetModdedEffectForSelf().m_applyEffect;
		}
		else
		{
			affectsCaster = false;
		}
		targeter.SetAffectedGroups(affectsEnemies, affectsAllies, affectsCaster);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.ModdedDamage()));
		this.ModdedEffectOnTargets().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Enemy);
		base.AppendTooltipNumbersFromBaseModEffects(ref list, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int numActorsInRange = base.Targeter.GetNumActorsInRange();
		dictionary.Add(AbilityTooltipSymbol.Damage, this.ModdedDamage() + this.ModdedExtraDamagePerTarget() * (numActorsInRange - 1));
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineMissileBarrage abilityMod_SpaceMarineMissileBarrage = modAsBase as AbilityMod_SpaceMarineMissileBarrage;
		base.AddTokenInt(tokens, "DelayTurns", string.Empty, 1, false);
		base.AddTokenInt(tokens, "Missiles", string.Empty, this.m_missiles, false);
		string name = "Damage";
		string empty = string.Empty;
		int val;
		if (abilityMod_SpaceMarineMissileBarrage)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpaceMarineMissileBarrage.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_SpaceMarineMissileBarrage.m_damageMod.GetModifiedValue(this.m_damage);
		}
		else
		{
			val = this.m_damage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnTargets, "EffectOnTargets", null, true);
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		bool result;
		if (animIndex != this.m_missileLaunchAnimIndex)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpaceMarineMissileBarrage.CanTriggerAnimAtIndexForTaunt(int)).MethodHandle;
			}
			result = base.CanTriggerAnimAtIndexForTaunt(animIndex);
		}
		else
		{
			result = true;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SpaceMarineMissileBarrage))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpaceMarineMissileBarrage.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SpaceMarineMissileBarrage);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public int ModdedDamage()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage) : this.m_damage;
	}

	public int ModdedMissileActiveDuration()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpaceMarineMissileBarrage.ModdedMissileActiveDuration()).MethodHandle;
			}
			result = 1;
		}
		else
		{
			result = this.m_abilityMod.m_activeDurationMod.GetModifiedValue(1);
		}
		return result;
	}

	public StandardEffectInfo ModdedEffectOnTargets()
	{
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpaceMarineMissileBarrage.ModdedEffectOnTargets()).MethodHandle;
			}
			if (this.m_abilityMod.m_missileHitEffectOverride.m_applyEffect)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.m_abilityMod.m_missileHitEffectOverride;
			}
		}
		return this.m_effectOnTargets;
	}

	public int ModdedExtraDamagePerTarget()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpaceMarineMissileBarrage.ModdedExtraDamagePerTarget()).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_extraDamagePerTarget.GetModifiedValue(0);
		}
		return result;
	}
}
