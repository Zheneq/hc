using System;
using System.Collections.Generic;
using UnityEngine;

public class BlasterKnockbackCone : Ability
{
	[Header("-- Cone Limits")]
	public float m_minLength;

	public float m_maxLength;

	public float m_minAngle;

	public float m_maxAngle;

	public AreaEffectUtils.StretchConeStyle m_stretchStyle = AreaEffectUtils.StretchConeStyle.DistanceSquared;

	public float m_coneBackwardOffset;

	public bool m_penetrateLineOfSight;

	[Header("-- On Hit")]
	public int m_damageAmountNormal;

	public bool m_removeOverchargeEffectOnCast;

	public StandardEffectInfo m_enemyEffectNormal;

	public StandardEffectInfo m_enemyEffectOvercharged;

	[Header("-- Knockback on Enemy")]
	public float m_knockbackDistance;

	public float m_extraKnockbackDistOnOvercharged;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	[Header("-- Knockback on Self")]
	public float m_knockbackDistanceOnSelf;

	public KnockbackType m_knockbackTypeOnSelf = KnockbackType.BackwardAgainstAimDir;

	[Header("-- Set Overcharge as Free Action after cast?")]
	public bool m_overchargeAsFreeActionAfterCast;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_overchargedCastSequencePrefab;

	public GameObject m_unstoppableSetterSequencePrefab;

	private AbilityMod_BlasterKnockbackCone m_abilityMod;

	private BlasterOvercharge m_overchargeAbility;

	private Blaster_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedEnemyEffectNormal;

	private StandardEffectInfo m_cachedEnemyEffectOvercharged;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.Start()).MethodHandle;
			}
			this.m_abilityName = "Knockback Cone";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.m_overchargeAbility = base.GetAbilityOfType<BlasterOvercharge>();
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_StretchCone(this, this.GetMinLength(), this.GetMaxLength(), this.GetMinAngle(), this.GetMaxAngle(), this.m_stretchStyle, this.GetConeBackwardOffset(), this.PenetrateLineOfSight());
		AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = base.Targeter as AbilityUtil_Targeter_StretchCone;
		abilityUtil_Targeter_StretchCone.InitKnockbackData(this.GetKnockbackDistance(), this.m_knockbackType, this.GetKnockbackDistanceOnSelf(), this.m_knockbackTypeOnSelf);
		abilityUtil_Targeter_StretchCone.SetExtraKnockbackDist(this.GetExtraKnockbackDistOnOvercharged());
		abilityUtil_Targeter_StretchCone.m_useExtraKnockbackDistDelegate = delegate(ActorData caster)
		{
			bool result;
			if (this.m_syncComp != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.<SetupTargeter>m__0(ActorData)).MethodHandle;
				}
				result = (this.m_syncComp.m_overchargeBuffs > 0);
			}
			else
			{
				result = false;
			}
			return result;
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetMaxLength();
	}

	private void SetCachedFields()
	{
		this.m_cachedEnemyEffectNormal = ((!this.m_abilityMod) ? this.m_enemyEffectNormal : this.m_abilityMod.m_enemyEffectNormalMod.GetModifiedValue(this.m_enemyEffectNormal));
		StandardEffectInfo cachedEnemyEffectOvercharged;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.SetCachedFields()).MethodHandle;
			}
			cachedEnemyEffectOvercharged = this.m_abilityMod.m_enemyEffectOverchargedMod.GetModifiedValue(this.m_enemyEffectOvercharged);
		}
		else
		{
			cachedEnemyEffectOvercharged = this.m_enemyEffectOvercharged;
		}
		this.m_cachedEnemyEffectOvercharged = cachedEnemyEffectOvercharged;
	}

	public float GetMinLength()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetMinLength()).MethodHandle;
			}
			result = this.m_abilityMod.m_minLengthMod.GetModifiedValue(this.m_minLength);
		}
		else
		{
			result = this.m_minLength;
		}
		return result;
	}

	public float GetMaxLength()
	{
		return (!this.m_abilityMod) ? this.m_maxLength : this.m_abilityMod.m_maxLengthMod.GetModifiedValue(this.m_maxLength);
	}

	public float GetMinAngle()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetMinAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_minAngleMod.GetModifiedValue(this.m_minAngle);
		}
		else
		{
			result = this.m_minAngle;
		}
		return result;
	}

	public float GetMaxAngle()
	{
		return (!this.m_abilityMod) ? this.m_maxAngle : this.m_abilityMod.m_maxAngleMod.GetModifiedValue(this.m_maxAngle);
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetConeBackwardOffset()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
		}
		else
		{
			result = this.m_coneBackwardOffset;
		}
		return result;
	}

	public bool PenetrateLineOfSight()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.PenetrateLineOfSight()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
		}
		else
		{
			result = this.m_penetrateLineOfSight;
		}
		return result;
	}

	public int GetDamageAmountNormal()
	{
		return (!this.m_abilityMod) ? this.m_damageAmountNormal : this.m_abilityMod.m_damageAmountNormalMod.GetModifiedValue(this.m_damageAmountNormal);
	}

	public StandardEffectInfo GetEnemyEffectNormal()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyEffectNormal != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetEnemyEffectNormal()).MethodHandle;
			}
			result = this.m_cachedEnemyEffectNormal;
		}
		else
		{
			result = this.m_enemyEffectNormal;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyEffectOvercharged()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyEffectOvercharged != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetEnemyEffectOvercharged()).MethodHandle;
			}
			result = this.m_cachedEnemyEffectOvercharged;
		}
		else
		{
			result = this.m_enemyEffectOvercharged;
		}
		return result;
	}

	public float GetKnockbackDistance()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetKnockbackDistance()).MethodHandle;
			}
			result = this.m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(this.m_knockbackDistance);
		}
		else
		{
			result = this.m_knockbackDistance;
		}
		return result;
	}

	public float GetExtraKnockbackDistOnOvercharged()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetExtraKnockbackDistOnOvercharged()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraKnockbackDistOnOverchargedMod.GetModifiedValue(this.m_extraKnockbackDistOnOvercharged);
		}
		else
		{
			result = this.m_extraKnockbackDistOnOvercharged;
		}
		return result;
	}

	public float GetKnockbackDistanceOnSelf()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetKnockbackDistanceOnSelf()).MethodHandle;
			}
			result = this.m_abilityMod.m_knockbackDistanceOnSelfMod.GetModifiedValue(this.m_knockbackDistanceOnSelf);
		}
		else
		{
			result = this.m_knockbackDistanceOnSelf;
		}
		return result;
	}

	public bool OverchargeAsFreeActionAfterCast()
	{
		return (!this.m_abilityMod) ? this.m_overchargeAsFreeActionAfterCast : this.m_abilityMod.m_overchargeAsFreeActionAfterCastMod.GetModifiedValue(this.m_overchargeAsFreeActionAfterCast);
	}

	public int GetCurrentModdedDamage()
	{
		if (this.AmOvercharged(base.ActorData))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetCurrentModdedDamage()).MethodHandle;
			}
			return this.GetDamageAmountNormal() + this.m_overchargeAbility.GetExtraDamage() + this.GetMultiStackOverchargeDamage();
		}
		return this.GetDamageAmountNormal();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterKnockbackCone abilityMod_BlasterKnockbackCone = modAsBase as AbilityMod_BlasterKnockbackCone;
		string name = "Damage";
		string empty = string.Empty;
		int val;
		if (abilityMod_BlasterKnockbackCone)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_BlasterKnockbackCone.m_damageAmountNormalMod.GetModifiedValue(this.m_damageAmountNormal);
		}
		else
		{
			val = this.m_damageAmountNormal;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_BlasterKnockbackCone)
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
			effectInfo = abilityMod_BlasterKnockbackCone.m_enemyEffectNormalMod.GetModifiedValue(this.m_enemyEffectNormal);
		}
		else
		{
			effectInfo = this.m_enemyEffectNormal;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyEffectNormal", this.m_enemyEffectNormal, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_BlasterKnockbackCone)
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
			effectInfo2 = abilityMod_BlasterKnockbackCone.m_enemyEffectOverchargedMod.GetModifiedValue(this.m_enemyEffectOvercharged);
		}
		else
		{
			effectInfo2 = this.m_enemyEffectOvercharged;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EnemyEffectOvercharged", this.m_enemyEffectOvercharged, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.GetCurrentModdedDamage())
		};
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetCurrentModdedDamage());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				dictionary[AbilityTooltipSymbol.Damage] = this.GetCurrentModdedDamage();
			}
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BlasterKnockbackCone))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_BlasterKnockbackCone);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public override Ability.MovementAdjustment GetMovementAdjustment()
	{
		if (base.ActorData.\u000E().IsKnockbackImmune(true))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetMovementAdjustment()).MethodHandle;
			}
			return Ability.MovementAdjustment.ReducedMovement;
		}
		AbilityData abilityData = base.ActorData.\u000E();
		List<AbilityData.AbilityEntry> queuedOrAimingAbilities = abilityData.GetQueuedOrAimingAbilities();
		using (List<AbilityData.AbilityEntry>.Enumerator enumerator = queuedOrAimingAbilities.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityData.AbilityEntry abilityEntry = enumerator.Current;
				Card_Standard_Ability card_Standard_Ability = abilityEntry.ability as Card_Standard_Ability;
				if (card_Standard_Ability != null)
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
					if (card_Standard_Ability.m_applyEffect)
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
						StatusType[] statusChanges = card_Standard_Ability.m_effect.m_statusChanges;
						int i = 0;
						while (i < statusChanges.Length)
						{
							StatusType statusType = statusChanges[i];
							if (statusType != StatusType.KnockbackImmune)
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
								if (statusType != StatusType.Unstoppable)
								{
									i++;
									continue;
								}
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							return Ability.MovementAdjustment.ReducedMovement;
						}
					}
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return base.GetMovementAdjustment();
	}

	private bool AmOvercharged(ActorData caster)
	{
		if (this.m_syncComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.AmOvercharged(ActorData)).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Blaster_SyncComponent>();
		}
		return this.m_syncComp.m_overchargeBuffs > 0;
	}

	private int GetMultiStackOverchargeDamage()
	{
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterKnockbackCone.GetMultiStackOverchargeDamage()).MethodHandle;
			}
			if (this.m_syncComp.m_overchargeBuffs > 1 && this.m_overchargeAbility != null)
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
				if (this.m_overchargeAbility.GetExtraDamageForMultiCast() > 0)
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
					return this.m_overchargeAbility.GetExtraDamageForMultiCast();
				}
			}
		}
		return 0;
	}
}
