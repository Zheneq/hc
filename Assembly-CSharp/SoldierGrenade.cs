using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGrenade : Ability
{
	[Header("-- Targeting --")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three;

	public bool m_penetrateLos;

	[Header("-- On Hit Stuff --")]
	public int m_damageAmount = 0xA;

	public StandardEffectInfo m_enemyHitEffect;

	[Space(10f)]
	public int m_allyHealAmount;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_SoldierGrenade m_abilityMod;

	private AbilityData m_abilityData;

	private SoldierStimPack m_stimAbility;

	private List<SoldierGrenade.ShapeToDamage> m_cachedShapeToDamage = new List<SoldierGrenade.ShapeToDamage>();

	private List<AbilityAreaShape> m_shapes = new List<AbilityAreaShape>();

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.Start()).MethodHandle;
			}
			this.m_abilityName = "Grenade";
		}
		this.Setup();
	}

	private void Setup()
	{
		if (this.m_abilityData == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.Setup()).MethodHandle;
			}
			this.m_abilityData = base.GetComponent<AbilityData>();
		}
		if (this.m_stimAbility == null && this.m_abilityData != null)
		{
			this.m_stimAbility = (this.m_abilityData.GetAbilityOfType(typeof(SoldierStimPack)) as SoldierStimPack);
		}
		this.SetCachedFields();
		this.m_cachedShapeToDamage.Clear();
		this.m_cachedShapeToDamage.Add(new SoldierGrenade.ShapeToDamage(this.GetShape(), this.GetDamageAmount()));
		if (this.m_abilityMod != null)
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
			if (this.m_abilityMod.m_useAdditionalShapeOverride)
			{
				for (int i = 0; i < this.m_abilityMod.m_additionalShapeToDamageOverride.Count; i++)
				{
					SoldierGrenade.ShapeToDamage shapeToDamage = this.m_abilityMod.m_additionalShapeToDamageOverride[i];
					this.m_cachedShapeToDamage.Add(new SoldierGrenade.ShapeToDamage(shapeToDamage.m_shape, shapeToDamage.m_damage));
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		this.m_cachedShapeToDamage.Sort();
		this.m_shapes.Clear();
		for (int j = 0; j < this.m_cachedShapeToDamage.Count; j++)
		{
			this.m_shapes.Add(this.m_cachedShapeToDamage[j].m_shape);
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		List<AbilityTooltipSubject> subjects = new List<AbilityTooltipSubject>
		{
			AbilityTooltipSubject.Primary
		};
		base.Targeter = new AbilityUtil_Targeter_MultipleShapes(this, this.m_shapes, subjects, this.PenetrateLos(), this.IncludeEnemies(), this.IncludeAllies(), false);
	}

	public int GetDamageForShapeIndex(int shapeIndex)
	{
		if (this.m_cachedShapeToDamage != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.GetDamageForShapeIndex(int)).MethodHandle;
			}
			if (shapeIndex < this.m_cachedShapeToDamage.Count)
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
				return this.m_cachedShapeToDamage[shapeIndex].m_damage;
			}
		}
		return this.GetDamageAmount();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyHitEffect;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.SetCachedFields()).MethodHandle;
			}
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		StandardEffectInfo cachedAllyHitEffect;
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
			cachedAllyHitEffect = this.m_abilityMod.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = this.m_allyHitEffect;
		}
		this.m_cachedAllyHitEffect = cachedAllyHitEffect;
	}

	public AbilityAreaShape GetShape()
	{
		AbilityAreaShape result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.GetShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_shapeMod.GetModifiedValue(this.m_shape);
		}
		else
		{
			result = this.m_shape;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		return (!this.m_abilityMod) ? this.m_penetrateLos : this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
	}

	public int GetDamageAmount()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.GetDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public int GetAllyHealAmount()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.GetAllyHealAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_allyHealAmountMod.GetModifiedValue(this.m_allyHealAmount);
		}
		else
		{
			result = this.m_allyHealAmount;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.GetAllyHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllyHitEffect;
		}
		else
		{
			result = this.m_allyHitEffect;
		}
		return result;
	}

	public bool IncludeEnemies()
	{
		bool result;
		if (this.GetDamageAmount() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.IncludeEnemies()).MethodHandle;
			}
			result = this.GetEnemyHitEffect().m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IncludeAllies()
	{
		bool result;
		if (this.GetAllyHealAmount() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.IncludeAllies()).MethodHandle;
			}
			result = this.GetAllyHitEffect().m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageAmount());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetAllyHealAmount());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetAllyHealAmount());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		ActorData actorData = base.ActorData;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (actorData != null)
			{
				dictionary = new Dictionary<AbilityTooltipSymbol, int>();
				List<AbilityUtil_Targeter_MultipleShapes.HitActorContext> hitActorContext = (base.Targeter as AbilityUtil_Targeter_MultipleShapes).GetHitActorContext();
				foreach (AbilityUtil_Targeter_MultipleShapes.HitActorContext hitActorContext2 in hitActorContext)
				{
					if (hitActorContext2.m_actor == targetActor && targetActor.\u000E() != actorData.\u000E())
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
						dictionary[AbilityTooltipSymbol.Damage] = this.GetDamageForShapeIndex(hitActorContext2.m_hitShapeIndex);
						break;
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SoldierGrenade abilityMod_SoldierGrenade = modAsBase as AbilityMod_SoldierGrenade;
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_SoldierGrenade) ? this.m_damageAmount : abilityMod_SoldierGrenade.m_damageAmountMod.GetModifiedValue(this.m_damageAmount), false);
		StandardEffectInfo effectInfo;
		if (abilityMod_SoldierGrenade)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			effectInfo = abilityMod_SoldierGrenade.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		string name = "AllyHealAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_SoldierGrenade)
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
			val = abilityMod_SoldierGrenade.m_allyHealAmountMod.GetModifiedValue(this.m_allyHealAmount);
		}
		else
		{
			val = this.m_allyHealAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_SoldierGrenade)
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
			effectInfo2 = abilityMod_SoldierGrenade.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			effectInfo2 = this.m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyHitEffect", this.m_allyHitEffect, true);
	}

	public override float GetRangeInSquares(int targetIndex)
	{
		float num = base.GetRangeInSquares(targetIndex);
		if (this.m_abilityData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.GetRangeInSquares(int)).MethodHandle;
			}
			if (this.m_stimAbility != null)
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
				if (this.m_stimAbility.GetGrenadeExtraRange() > 0f)
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
					if (this.m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack)))
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
						num += this.m_stimAbility.GetGrenadeExtraRange();
					}
				}
			}
		}
		return num;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierGrenade))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierGrenade.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SoldierGrenade);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	[Serializable]
	public class ShapeToDamage : ShapeToDataBase
	{
		public int m_damage;

		public ShapeToDamage(AbilityAreaShape shape, int damage)
		{
			this.m_shape = shape;
			this.m_damage = damage;
		}
	}
}
