using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcherShieldGeneratorArrow : Ability
{
	[Header("-- Ground effect")]
	public bool m_penetrateLoS;

	public bool m_affectsEnemies;

	public bool m_affectsAllies;

	public bool m_affectsCaster;

	public int m_lessAbsorbPerTurn = 5;

	public StandardGroundEffectInfo m_groundEffectInfo;

	public StandardEffectInfo m_directHitEnemyEffect;

	public StandardEffectInfo m_directHitAllyEffect;

	[Header("-- Extra effect for shielding that last different number of turns from main effect, etc")]
	public StandardEffectInfo m_extraAllyHitEffect;

	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ArcherShieldGeneratorArrow m_abilityMod;

	private Archer_SyncComponent m_syncComp;

	private StandardGroundEffectInfo m_cachedGroundEffect;

	private StandardEffectInfo m_cachedDirectHitEnemyEffect;

	private StandardEffectInfo m_cachedDirectHitAllyEffect;

	private StandardEffectInfo m_cachedExtraAllyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Shield Generator Arrow";
		}
		this.m_syncComp = base.GetComponent<Archer_SyncComponent>();
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.GetGroundEffectInfo().m_groundEffectData.shape, this.PenetrateLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.AffectsEnemies(), this.AffectsAllies(), (!this.AffectsCaster()) ? AbilityUtil_Targeter.AffectsActor.Never : AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherShieldGeneratorArrow))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ArcherShieldGeneratorArrow);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	private void SetCachedFields()
	{
		this.m_cachedGroundEffect = this.m_groundEffectInfo;
		StandardEffectInfo cachedDirectHitEnemyEffect;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.SetCachedFields()).MethodHandle;
			}
			cachedDirectHitEnemyEffect = this.m_abilityMod.m_directHitEnemyEffectMod.GetModifiedValue(this.m_directHitEnemyEffect);
		}
		else
		{
			cachedDirectHitEnemyEffect = this.m_directHitEnemyEffect;
		}
		this.m_cachedDirectHitEnemyEffect = cachedDirectHitEnemyEffect;
		StandardEffectInfo cachedDirectHitAllyEffect;
		if (this.m_abilityMod)
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
			cachedDirectHitAllyEffect = this.m_abilityMod.m_directHitAllyEffectMod.GetModifiedValue(this.m_directHitAllyEffect);
		}
		else
		{
			cachedDirectHitAllyEffect = this.m_directHitAllyEffect;
		}
		this.m_cachedDirectHitAllyEffect = cachedDirectHitAllyEffect;
		StandardEffectInfo cachedExtraAllyHitEffect;
		if (this.m_abilityMod)
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
			cachedExtraAllyHitEffect = this.m_abilityMod.m_extraAllyHitEffectMod.GetModifiedValue(this.m_extraAllyHitEffect);
		}
		else
		{
			cachedExtraAllyHitEffect = this.m_extraAllyHitEffect;
		}
		this.m_cachedExtraAllyHitEffect = cachedExtraAllyHitEffect;
	}

	public bool PenetrateLoS()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.PenetrateLoS()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLoSMod.GetModifiedValue(this.m_penetrateLoS);
		}
		else
		{
			result = this.m_penetrateLoS;
		}
		return result;
	}

	public bool AffectsEnemies()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.AffectsEnemies()).MethodHandle;
			}
			result = this.m_abilityMod.m_affectsEnemiesMod.GetModifiedValue(this.m_affectsEnemies);
		}
		else
		{
			result = this.m_affectsEnemies;
		}
		return result;
	}

	public bool AffectsAllies()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.AffectsAllies()).MethodHandle;
			}
			result = this.m_abilityMod.m_affectsAlliesMod.GetModifiedValue(this.m_affectsAllies);
		}
		else
		{
			result = this.m_affectsAllies;
		}
		return result;
	}

	public bool AffectsCaster()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.AffectsCaster()).MethodHandle;
			}
			result = this.m_abilityMod.m_affectsCasterMod.GetModifiedValue(this.m_affectsCaster);
		}
		else
		{
			result = this.m_affectsCaster;
		}
		return result;
	}

	private StandardGroundEffectInfo GetGroundEffectInfo()
	{
		StandardGroundEffectInfo result;
		if (this.m_cachedGroundEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.GetGroundEffectInfo()).MethodHandle;
			}
			result = this.m_cachedGroundEffect;
		}
		else
		{
			result = this.m_groundEffectInfo;
		}
		return result;
	}

	public int GetLessAbsorbPerTurn()
	{
		return (!this.m_abilityMod) ? this.m_lessAbsorbPerTurn : this.m_abilityMod.m_lessAbsorbPerTurnMod.GetModifiedValue(this.m_lessAbsorbPerTurn);
	}

	public StandardEffectInfo GetDirectHitEnemyEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedDirectHitEnemyEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.GetDirectHitEnemyEffect()).MethodHandle;
			}
			result = this.m_cachedDirectHitEnemyEffect;
		}
		else
		{
			result = this.m_directHitEnemyEffect;
		}
		return result;
	}

	public StandardEffectInfo GetDirectHitAllyEffect()
	{
		return (this.m_cachedDirectHitAllyEffect == null) ? this.m_directHitAllyEffect : this.m_cachedDirectHitAllyEffect;
	}

	public StandardEffectInfo GetExtraAllyHitEffect()
	{
		return (this.m_cachedExtraAllyHitEffect == null) ? this.m_extraAllyHitEffect : this.m_cachedExtraAllyHitEffect;
	}

	public int GetCooldownReductionOnDash()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_cooldownReductionOnDash.GetModifiedValue(0);
	}

	public int GetExtraAbsorbPerEnemyHit()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.GetExtraAbsorbPerEnemyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraAbsorbPerEnemyHit.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraAbsorbIfEnemyHit()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_extraAbsorbIfEnemyHit.GetModifiedValue(0);
	}

	public int GetExtraAbsorbIfOnlyOneAllyHit()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.GetExtraAbsorbIfOnlyOneAllyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraAbsorbIfOnlyOneAllyHit.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "LessAbsorbPerTurn", string.Empty, this.m_lessAbsorbPerTurn, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_directHitEnemyEffect, "DirectHitEnemyEffect", this.m_directHitEnemyEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_directHitAllyEffect, "DirectHitAllyEffect", this.m_directHitAllyEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_extraAllyHitEffect, "ExtraAllyHitEffect", this.m_extraAllyHitEffect, true);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_groundEffectInfo.m_applyGroundEffect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.CalculateNameplateTargetingNumbers()).MethodHandle;
			}
			this.m_groundEffectInfo.m_groundEffectData.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
		}
		if (this.AffectsAllies())
		{
			this.GetDirectHitAllyEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		}
		if (this.AffectsCaster())
		{
			this.GetDirectHitAllyEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		}
		if (this.AffectsEnemies())
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
			this.GetDirectHitEnemyEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		}
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldGeneratorArrow.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (targetActor.GetTeam() == base.ActorData.GetTeam())
			{
				int num = this.m_syncComp.m_extraAbsorbForShieldGenerator;
				List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
				if (!actorsInRange.IsNullOrEmpty<AbilityUtil_Targeter.ActorTarget>())
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
					int num2 = actorsInRange.Count((AbilityUtil_Targeter.ActorTarget t) => t.m_actor.GetTeam() != base.ActorData.GetTeam());
					if (actorsInRange.Count - num2 == 1)
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
						num += this.GetExtraAbsorbIfOnlyOneAllyHit();
					}
					num += this.GetExtraAbsorbPerEnemyHit() * num2;
					if (num2 > 0)
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
						num += this.GetExtraAbsorbIfEnemyHit();
					}
				}
				int num3 = this.GetDirectHitAllyEffect().m_effectData.m_absorbAmount + num;
				StandardEffectInfo extraAllyHitEffect = this.GetExtraAllyHitEffect();
				if (extraAllyHitEffect.m_applyEffect)
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
					if (extraAllyHitEffect.m_effectData.m_absorbAmount > 0)
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
						num3 += extraAllyHitEffect.m_effectData.m_absorbAmount;
					}
				}
				dictionary[AbilityTooltipSymbol.Absorb] = num3;
			}
		}
		return dictionary;
	}
}
