using System;
using System.Collections.Generic;
using UnityEngine;

public class SorceressHealingCrossBeam : Ability
{
	[Header("-- Enemy Hit Damage and Effect")]
	public int m_damageAmount = 0xA;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Ally Hit Heal and Effect")]
	public int m_healAmount = 5;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- Targeting")]
	public float m_width = 1f;

	public float m_distance = 15f;

	public int m_numLasers = 4;

	public bool m_alsoHealSelf = true;

	public bool m_penetrateLineOfSight;

	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_beamSequencePrefab;

	public GameObject m_centerSequencePrefab;

	public GameObject m_healSequencePrefab;

	private AbilityUtil_Targeter_CrossBeam m_customTargeter;

	private AbilityMod_SorceressHealingCrossBeam m_abilityMod;

	private void Start()
	{
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.m_customTargeter = new AbilityUtil_Targeter_CrossBeam(this, this.GetNumLasers(), this.GetLaserRange(), this.GetLaserWidth(), this.m_penetrateLineOfSight, true, this.m_alsoHealSelf);
		this.m_customTargeter.SetKnockbackParams(this.GetKnockbackDistance(), this.GetKnockbackType(), this.GetKnockbackThresholdDistance());
		base.Targeter = this.m_customTargeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRange();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_damageAmount));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, this.m_healAmount));
		if (this.m_alsoHealSelf)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, this.m_healAmount));
		}
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			List<AbilityUtil_Targeter_CrossBeam.HitActorContext> hitActorContext = this.m_customTargeter.GetHitActorContext();
			int numTargetsInLaser = 0;
			using (List<AbilityUtil_Targeter_CrossBeam.HitActorContext>.Enumerator enumerator = hitActorContext.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityUtil_Targeter_CrossBeam.HitActorContext hitActorContext2 = enumerator.Current;
					if (hitActorContext2.actor == targetActor)
					{
						numTargetsInLaser = hitActorContext2.totalTargetsInLaser;
						goto IL_8B;
					}
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
				}
			}
			IL_8B:
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				dictionary[AbilityTooltipSymbol.Damage] = this.GetDamageAmount(numTargetsInLaser);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
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
				dictionary[AbilityTooltipSymbol.Healing] = this.GetHealAmount(numTargetsInLaser);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				dictionary[AbilityTooltipSymbol.Healing] = this.GetHealAmount(numTargetsInLaser);
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressHealingCrossBeam abilityMod_SorceressHealingCrossBeam = modAsBase as AbilityMod_SorceressHealingCrossBeam;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_SorceressHealingCrossBeam)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_SorceressHealingCrossBeam.m_normalDamageMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_SorceressHealingCrossBeam)
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
			effectInfo = abilityMod_SorceressHealingCrossBeam.m_enemyEffectOverride.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		string name2 = "HealAmount";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_SorceressHealingCrossBeam)
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
			val2 = abilityMod_SorceressHealingCrossBeam.m_normalHealingMod.GetModifiedValue(this.m_healAmount);
		}
		else
		{
			val2 = this.m_healAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_SorceressHealingCrossBeam)
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
			effectInfo2 = abilityMod_SorceressHealingCrossBeam.m_allyEffectOverride.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			effectInfo2 = this.m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyHitEffect", this.m_allyHitEffect, true);
		string name3 = "NumLasers";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_SorceressHealingCrossBeam)
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
			val3 = abilityMod_SorceressHealingCrossBeam.m_laserNumberMod.GetModifiedValue(this.m_numLasers);
		}
		else
		{
			val3 = this.m_numLasers;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SorceressHealingCrossBeam))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SorceressHealingCrossBeam);
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

	private int GetNumLasers()
	{
		int result = this.m_numLasers;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.GetNumLasers()).MethodHandle;
			}
			result = Mathf.Max(1, this.m_abilityMod.m_laserNumberMod.GetModifiedValue(this.m_numLasers));
		}
		return result;
	}

	private int GetDamageAmount(int numTargetsInLaser)
	{
		int result = this.m_damageAmount;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.GetDamageAmount(int)).MethodHandle;
			}
			if (numTargetsInLaser == 1)
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
				if (this.m_abilityMod.m_useSingleTargetHitMods)
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
					return this.m_abilityMod.m_singleTargetDamageMod.GetModifiedValue(this.m_damageAmount);
				}
			}
			result = this.m_abilityMod.m_normalDamageMod.GetModifiedValue(this.m_damageAmount);
		}
		return result;
	}

	private int GetHealAmount(int numTargetsInLaser)
	{
		int result = this.m_healAmount;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.GetHealAmount(int)).MethodHandle;
			}
			if (numTargetsInLaser == 1)
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
				if (this.m_abilityMod.m_useSingleTargetHitMods)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					return this.m_abilityMod.m_singleTargetHealingMod.GetModifiedValue(this.m_healAmount);
				}
			}
			result = this.m_abilityMod.m_normalHealingMod.GetModifiedValue(this.m_healAmount);
		}
		return result;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_enemyEffectOverride.GetModifiedValue(this.m_enemyHitEffect) : this.m_enemyHitEffect;
	}

	private StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.GetAllyHitEffect()).MethodHandle;
			}
			result = this.m_allyHitEffect;
		}
		else
		{
			result = this.m_abilityMod.m_allyEffectOverride.GetModifiedValue(this.m_allyHitEffect);
		}
		return result;
	}

	private float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.GetLaserWidth()).MethodHandle;
			}
			result = this.m_width;
		}
		else
		{
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_width);
		}
		return result;
	}

	private float GetLaserRange()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.GetLaserRange()).MethodHandle;
			}
			result = this.m_distance;
		}
		else
		{
			result = this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_distance);
		}
		return result;
	}

	private float GetKnockbackDistance()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_knockbackDistance : 0f;
	}

	private KnockbackType GetKnockbackType()
	{
		KnockbackType result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.GetKnockbackType()).MethodHandle;
			}
			result = KnockbackType.AwayFromSource;
		}
		else
		{
			result = this.m_abilityMod.m_knockbackType;
		}
		return result;
	}

	private float GetKnockbackThresholdDistance()
	{
		float result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SorceressHealingCrossBeam.GetKnockbackThresholdDistance()).MethodHandle;
			}
			result = -1f;
		}
		else
		{
			result = this.m_abilityMod.m_knockbackThresholdDistance;
		}
		return result;
	}
}
