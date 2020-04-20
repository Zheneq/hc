using System;
using System.Collections.Generic;
using UnityEngine;

public class BlasterDelayedLaser : Ability
{
	[Header("-- Laser Data")]
	public bool m_penetrateLineOfSight = true;

	public float m_length = 8f;

	public float m_width = 2f;

	[Header("-- Initial Placement Phase")]
	public AbilityPriority m_placementPhase = AbilityPriority.Prep_Offense;

	[Header("-- Delay Data")]
	public int m_turnsBeforeTriggering = 1;

	public bool m_remoteTriggerMode = true;

	public bool m_remoteTriggerIsFreeAction = true;

	public int m_triggerAnimationIndex = 0xB;

	public bool m_triggerAimAtBlaster;

	[Header("-- On Hit")]
	public int m_damageAmount = 0x28;

	public StandardEffectInfo m_effectOnHit;

	public int m_extraDamageToNearEnemy;

	public float m_nearDistance;

	[Header("-- On Cast Hit Effect")]
	public StandardEffectInfo m_onCastEnemyHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	[Header("    For Satellite, persistent")]
	public GameObject m_laserGroundSequencePrefab;

	[Header("    For laser firing, with gameplay hits")]
	public GameObject m_laserTriggerSequencePrefab;

	[Header("    For laser firing, only on caster")]
	public GameObject m_laserTriggerOnCasterSequencePrefab;

	private AbilityUtil_Targeter_Laser m_laserTargeter;

	private AbilityMod_BlasterDelayedLaser m_abilityMod;

	private BlasterOvercharge m_overchargeAbility;

	private Blaster_SyncComponent m_syncComponent;

	private StandardEffectInfo m_cachedEffectOnHit;

	private StandardEffectInfo m_cachedOnCastEnemyHitEffect;

	private void Start()
	{
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.m_syncComponent = base.GetComponent<Blaster_SyncComponent>();
		this.m_overchargeAbility = (base.GetComponent<AbilityData>().GetAbilityOfType(typeof(BlasterOvercharge)) as BlasterOvercharge);
		this.SetCachedFields();
		this.m_laserTargeter = new AbilityUtil_Targeter_BlasterDelayedLaser(this, this.m_syncComponent, this.TriggerAimAtBlaster(), this.GetWidth(), this.GetLength(), this.PenetrateLineOfSight(), -1, false, false);
		base.Targeter = this.m_laserTargeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		bool result;
		if (this.m_syncComponent != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.CanShowTargetableRadiusPreview()).MethodHandle;
			}
			result = !this.m_syncComponent.m_canActivateDelayedLaser;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLength();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnHit;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.SetCachedFields()).MethodHandle;
			}
			cachedEffectOnHit = this.m_abilityMod.m_effectOnHitMod.GetModifiedValue(this.m_effectOnHit);
		}
		else
		{
			cachedEffectOnHit = this.m_effectOnHit;
		}
		this.m_cachedEffectOnHit = cachedEffectOnHit;
		this.m_cachedOnCastEnemyHitEffect = ((!this.m_abilityMod) ? this.m_onCastEnemyHitEffect : this.m_abilityMod.m_onCastEnemyHitEffectMod.GetModifiedValue(this.m_onCastEnemyHitEffect));
	}

	public bool PenetrateLineOfSight()
	{
		return (!this.m_abilityMod) ? this.m_penetrateLineOfSight : this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
	}

	public float GetLength()
	{
		return (!this.m_abilityMod) ? this.m_length : this.m_abilityMod.m_lengthMod.GetModifiedValue(this.m_length);
	}

	public float GetWidth()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.GetWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_widthMod.GetModifiedValue(this.m_width);
		}
		else
		{
			result = this.m_width;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_damageAmount : this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
	}

	public StandardEffectInfo GetEffectOnHit()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnHit != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.GetEffectOnHit()).MethodHandle;
			}
			result = this.m_cachedEffectOnHit;
		}
		else
		{
			result = this.m_effectOnHit;
		}
		return result;
	}

	public StandardEffectInfo GetOnCastEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedOnCastEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.GetOnCastEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedOnCastEnemyHitEffect;
		}
		else
		{
			result = this.m_onCastEnemyHitEffect;
		}
		return result;
	}

	public bool TriggerAimAtBlaster()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.TriggerAimAtBlaster()).MethodHandle;
			}
			result = this.m_abilityMod.m_triggerAimAtBlasterMod.GetModifiedValue(this.m_triggerAimAtBlaster);
		}
		else
		{
			result = this.m_triggerAimAtBlaster;
		}
		return result;
	}

	public int GetExtraDamageToNearEnemy()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.GetExtraDamageToNearEnemy()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageToNearEnemyMod.GetModifiedValue(this.m_extraDamageToNearEnemy);
		}
		else
		{
			result = this.m_extraDamageToNearEnemy;
		}
		return result;
	}

	public float GetNearDistance()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.GetNearDistance()).MethodHandle;
			}
			result = this.m_abilityMod.m_nearDistanceMod.GetModifiedValue(this.m_nearDistance);
		}
		else
		{
			result = this.m_nearDistance;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_damageAmount)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		ActorData actorData = base.ActorData;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				if (actorData != null)
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
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					int num = this.GetDamageAmount();
					if (this.m_syncComponent != null && this.m_syncComponent.m_overchargeBuffs > 0)
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
						if (this.m_overchargeAbility != null && this.m_overchargeAbility.GetExtraDamageForDelayedLaser() > 0)
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
							num += this.m_overchargeAbility.GetExtraDamageForDelayedLaser();
						}
					}
					Vector3 vector;
					if (this.m_syncComponent.m_canActivateDelayedLaser)
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
						vector = this.m_syncComponent.m_delayedLaserStartPos;
					}
					else
					{
						vector = actorData.GetTravelBoardSquareWorldPosition();
					}
					Vector3 b = vector;
					if (this.GetExtraDamageToNearEnemy() > 0 && this.GetNearDistance() > 0f)
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
						float num2 = this.GetNearDistance() * Board.Get().squareSize;
						Vector3 vector2 = targetActor.GetTravelBoardSquareWorldPosition() - b;
						vector2.y = 0f;
						if (vector2.magnitude <= num2)
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
							num += this.GetExtraDamageToNearEnemy();
						}
					}
					dictionary[AbilityTooltipSymbol.Damage] = num;
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterDelayedLaser abilityMod_BlasterDelayedLaser = modAsBase as AbilityMod_BlasterDelayedLaser;
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_BlasterDelayedLaser) ? this.m_damageAmount : abilityMod_BlasterDelayedLaser.m_damageAmountMod.GetModifiedValue(this.m_damageAmount), false);
		StandardEffectInfo effectInfo;
		if (abilityMod_BlasterDelayedLaser)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			effectInfo = abilityMod_BlasterDelayedLaser.m_effectOnHitMod.GetModifiedValue(this.m_effectOnHit);
		}
		else
		{
			effectInfo = this.m_effectOnHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnHit", this.m_effectOnHit, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_BlasterDelayedLaser)
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
			effectInfo2 = abilityMod_BlasterDelayedLaser.m_onCastEnemyHitEffectMod.GetModifiedValue(this.m_onCastEnemyHitEffect);
		}
		else
		{
			effectInfo2 = this.m_onCastEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "OnCastEnemyHitEffect", this.m_onCastEnemyHitEffect, true);
		string name = "ExtraDamageToNearEnemy";
		string empty = string.Empty;
		int val;
		if (abilityMod_BlasterDelayedLaser)
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
			val = abilityMod_BlasterDelayedLaser.m_extraDamageToNearEnemyMod.GetModifiedValue(this.m_extraDamageToNearEnemy);
		}
		else
		{
			val = this.m_extraDamageToNearEnemy;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "MaxTurnsBeforeTrigger", string.Empty, this.m_turnsBeforeTriggering, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BlasterDelayedLaser))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_BlasterDelayedLaser);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public override bool IsFreeAction()
	{
		if (this.m_remoteTriggerMode)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.IsFreeAction()).MethodHandle;
			}
			if (this.m_syncComponent != null)
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
				if (this.m_syncComponent.m_canActivateDelayedLaser)
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
					return this.m_remoteTriggerIsFreeAction;
				}
				return base.IsFreeAction();
			}
		}
		return base.IsFreeAction();
	}

	public override AbilityPriority GetRunPriority()
	{
		if (this.m_remoteTriggerMode && this.m_syncComponent != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.GetRunPriority()).MethodHandle;
			}
			if (this.m_syncComponent.m_canActivateDelayedLaser)
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
				if (GameFlowData.Get().CurrentTurn > this.m_syncComponent.m_lastPlacementTurn)
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
					return base.GetRunPriority();
				}
			}
		}
		return this.m_placementPhase;
	}

	public override TargetData[] GetTargetData()
	{
		if (this.m_remoteTriggerMode)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.GetTargetData()).MethodHandle;
			}
			if (this.m_syncComponent != null)
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
				if (this.m_syncComponent.m_canActivateDelayedLaser)
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
					return new TargetData[0];
				}
				return base.GetTargetData();
			}
		}
		return base.GetTargetData();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (this.m_syncComponent != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.GetActionAnimType()).MethodHandle;
			}
			if (this.m_syncComponent.m_canActivateDelayedLaser)
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
				return ActorModelData.ActionAnimationType.None;
			}
		}
		return base.GetActionAnimType();
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		if (this.m_syncComponent != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDelayedLaser.CanTriggerAnimAtIndexForTaunt(int)).MethodHandle;
			}
			if (this.m_syncComponent.m_canActivateDelayedLaser)
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
				if (animIndex == (int)base.GetActionAnimType())
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
					return true;
				}
			}
		}
		return false;
	}
}
