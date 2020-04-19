using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BattleMonkHamstring : Ability
{
	[Header("-- Laser")]
	public int m_laserDamageAmount = 5;

	public int m_damageAfterFirstHit;

	public LaserTargetingInfo m_laserInfo;

	public StandardEffectInfo m_laserHitEffect;

	[Header("-- Explosion")]
	public bool m_explodeOnActorHit;

	public AbilityAreaShape m_explodeShape = AbilityAreaShape.Three_x_Three;

	public int m_explosionDamageAmount;

	public StandardEffectInfo m_explosionHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSelfSequencePrefab;

	public GameObject m_projectileSequencePrefab;

	private AbilityMod_BattleMonkHamstring m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.Start()).MethodHandle;
			}
			this.m_abilityName = "Hamstring";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.ShouldExplodeOnActorHit())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.SetupTargeter()).MethodHandle;
			}
			base.Targeter = new AbilityUtil_Targeter_LaserWithShape(this, this.GetExplodeShape(), this.GetLaserWidth(), this.GetLaserRange(), this.m_laserInfo.penetrateLos, this.GetMaxTargets(), this.m_laserInfo.affectsAllies, this.m_laserInfo.affectsCaster, this.m_laserInfo.affectsEnemies);
		}
		else if (this.GetMaxBounces() > 0)
		{
			base.Targeter = new AbilityUtil_Targeter_BounceLaser(this, this.GetLaserWidth(), this.GetDistancePerBounce(), this.GetLaserRange(), this.GetMaxBounces(), this.GetMaxTargets(), false);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_Laser(this, this.GetLaserWidth(), this.GetLaserRange(), this.m_laserInfo.penetrateLos, this.GetMaxTargets(), this.m_laserInfo.affectsAllies, this.m_laserInfo.affectsCaster);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRange();
	}

	public int GetLaserDamage()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_laserDamageMod.GetModifiedValue(this.m_laserDamageAmount) : this.m_laserDamageAmount;
	}

	public int GetDamageAfterFirstHit()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.GetDamageAfterFirstHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAfterFirstHitMod.GetModifiedValue(this.m_damageAfterFirstHit);
		}
		else
		{
			result = this.m_damageAfterFirstHit;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_widthMod.GetModifiedValue(this.m_laserInfo.width) : this.m_laserInfo.width;
	}

	public float GetLaserRange()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.GetLaserRange()).MethodHandle;
			}
			result = this.m_laserInfo.range;
		}
		else
		{
			result = this.m_abilityMod.m_rangeMod.GetModifiedValue(this.m_laserInfo.range);
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.GetMaxTargets()).MethodHandle;
			}
			result = this.m_laserInfo.maxTargets;
		}
		else
		{
			result = this.m_abilityMod.m_maxTargetMod.GetModifiedValue(this.m_laserInfo.maxTargets);
		}
		return result;
	}

	public int GetMaxBounces()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.GetMaxBounces()).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_maxBounces.GetModifiedValue(0);
		}
		return result;
	}

	public float GetDistancePerBounce()
	{
		float result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.GetDistancePerBounce()).MethodHandle;
			}
			result = 0f;
		}
		else
		{
			result = this.m_abilityMod.m_distancePerBounce.GetModifiedValue(0f);
		}
		return result;
	}

	public GameObject GetProjectileSequence()
	{
		GameObject result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.GetProjectileSequence()).MethodHandle;
			}
			result = this.m_projectileSequencePrefab;
		}
		else
		{
			result = this.m_abilityMod.m_projectileSequencePrefab.GetModifiedValue(this.m_projectileSequencePrefab);
		}
		return result;
	}

	public bool ShouldExplodeOnActorHit()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.ShouldExplodeOnActorHit()).MethodHandle;
			}
			result = this.m_explodeOnActorHit;
		}
		else
		{
			result = this.m_abilityMod.m_explodeOnActorHitMod.GetModifiedValue(this.m_explodeOnActorHit);
		}
		return result;
	}

	public int GetExplosionDamage()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.GetExplosionDamage()).MethodHandle;
			}
			result = this.m_explosionDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_explosionDamageMod.GetModifiedValue(this.m_explosionDamageAmount);
		}
		return result;
	}

	public AbilityAreaShape GetExplodeShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.GetExplodeShape()).MethodHandle;
			}
			result = this.m_explodeShape;
		}
		else
		{
			result = this.m_abilityMod.m_explodeShapeMod.GetModifiedValue(this.m_explodeShape);
		}
		return result;
	}

	public int CalcDamageForOrderIndex(int hitOrder)
	{
		int damageAfterFirstHit = this.GetDamageAfterFirstHit();
		if (damageAfterFirstHit > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.CalcDamageForOrderIndex(int)).MethodHandle;
			}
			if (hitOrder > 0)
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
				return damageAfterFirstHit;
			}
		}
		return this.GetLaserDamage();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_laserDamageAmount);
		this.m_laserHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		if (this.m_explodeOnActorHit)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_explosionDamageAmount);
			this.m_explosionHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetLaserDamage());
		this.m_laserHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		if (this.ShouldExplodeOnActorHit())
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.GetExplosionDamage());
			this.m_explosionHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		}
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (base.Targeter is AbilityUtil_Targeter_LaserWithShape)
			{
				List<ActorData> lastLaserHitActors = (base.Targeter as AbilityUtil_Targeter_LaserWithShape).GetLastLaserHitActors();
				for (int i = 0; i < lastLaserHitActors.Count; i++)
				{
					if (targetActor == lastLaserHitActors[i])
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
						results.m_damage = this.CalcDamageForOrderIndex(i);
						break;
					}
				}
			}
			else if (base.Targeter is AbilityUtil_Targeter_BounceLaser)
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
				ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = (base.Targeter as AbilityUtil_Targeter_BounceLaser).GetHitActorContext();
				for (int j = 0; j < hitActorContext.Count; j++)
				{
					if (hitActorContext[j].actor == targetActor)
					{
						results.m_damage = this.CalcDamageForOrderIndex(j);
						break;
					}
				}
			}
			else if (base.Targeter is AbilityUtil_Targeter_Laser)
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
				List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext2 = (base.Targeter as AbilityUtil_Targeter_Laser).GetHitActorContext();
				for (int k = 0; k < hitActorContext2.Count; k++)
				{
					if (hitActorContext2[k].actor == targetActor)
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
						results.m_damage = this.CalcDamageForOrderIndex(k);
						break;
					}
				}
			}
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0)
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
			results.m_damage = this.GetExplosionDamage();
		}
		return true;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
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
				dictionary[AbilityTooltipSymbol.Damage] = this.GetLaserDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.GetExplosionDamage();
			}
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkHamstring abilityMod_BattleMonkHamstring = modAsBase as AbilityMod_BattleMonkHamstring;
		string name = "LaserDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_BattleMonkHamstring)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_BattleMonkHamstring.m_laserDamageMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			val = this.m_laserDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_BattleMonkHamstring)
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
			if (abilityMod_BattleMonkHamstring.m_useLaserHitEffectOverride)
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
				effectInfo = abilityMod_BattleMonkHamstring.m_laserHitEffectOverride;
				goto IL_86;
			}
		}
		effectInfo = this.m_laserHitEffect;
		IL_86:
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "LaserHitEffect", this.m_laserHitEffect, true);
		string name2 = "ExplosionDamageAmount";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_BattleMonkHamstring)
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
			val2 = abilityMod_BattleMonkHamstring.m_explosionDamageMod.GetModifiedValue(this.m_explosionDamageAmount);
		}
		else
		{
			val2 = this.m_explosionDamageAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_BattleMonkHamstring)
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
			if (abilityMod_BattleMonkHamstring.m_useExplosionHitEffectOverride)
			{
				effectInfo2 = abilityMod_BattleMonkHamstring.m_explosionHitEffectOverride;
				goto IL_101;
			}
		}
		effectInfo2 = this.m_explosionHitEffect;
		IL_101:
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "ExplosionHitEffect", this.m_explosionHitEffect, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BattleMonkHamstring))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkHamstring.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_BattleMonkHamstring);
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
