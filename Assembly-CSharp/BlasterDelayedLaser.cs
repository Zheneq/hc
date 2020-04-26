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

	public int m_triggerAnimationIndex = 11;

	public bool m_triggerAimAtBlaster;

	[Header("-- On Hit")]
	public int m_damageAmount = 40;

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
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_syncComponent = GetComponent<Blaster_SyncComponent>();
		m_overchargeAbility = (GetComponent<AbilityData>().GetAbilityOfType(typeof(BlasterOvercharge)) as BlasterOvercharge);
		SetCachedFields();
		m_laserTargeter = new AbilityUtil_Targeter_BlasterDelayedLaser(this, m_syncComponent, TriggerAimAtBlaster(), GetWidth(), GetLength(), PenetrateLineOfSight());
		base.Targeter = m_laserTargeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		int result;
		if (m_syncComponent != null)
		{
			result = ((!m_syncComponent.m_canActivateDelayedLaser) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLength();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnHit;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnHit = m_abilityMod.m_effectOnHitMod.GetModifiedValue(m_effectOnHit);
		}
		else
		{
			cachedEffectOnHit = m_effectOnHit;
		}
		m_cachedEffectOnHit = cachedEffectOnHit;
		m_cachedOnCastEnemyHitEffect = ((!m_abilityMod) ? m_onCastEnemyHitEffect : m_abilityMod.m_onCastEnemyHitEffectMod.GetModifiedValue(m_onCastEnemyHitEffect));
	}

	public bool PenetrateLineOfSight()
	{
		return (!m_abilityMod) ? m_penetrateLineOfSight : m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
	}

	public float GetLength()
	{
		return (!m_abilityMod) ? m_length : m_abilityMod.m_lengthMod.GetModifiedValue(m_length);
	}

	public float GetWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_widthMod.GetModifiedValue(m_width);
		}
		else
		{
			result = m_width;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		return (!m_abilityMod) ? m_damageAmount : m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
	}

	public StandardEffectInfo GetEffectOnHit()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnHit != null)
		{
			result = m_cachedEffectOnHit;
		}
		else
		{
			result = m_effectOnHit;
		}
		return result;
	}

	public StandardEffectInfo GetOnCastEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedOnCastEnemyHitEffect != null)
		{
			result = m_cachedOnCastEnemyHitEffect;
		}
		else
		{
			result = m_onCastEnemyHitEffect;
		}
		return result;
	}

	public bool TriggerAimAtBlaster()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_triggerAimAtBlasterMod.GetModifiedValue(m_triggerAimAtBlaster);
		}
		else
		{
			result = m_triggerAimAtBlaster;
		}
		return result;
	}

	public int GetExtraDamageToNearEnemy()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageToNearEnemyMod.GetModifiedValue(m_extraDamageToNearEnemy);
		}
		else
		{
			result = m_extraDamageToNearEnemy;
		}
		return result;
	}

	public float GetNearDistance()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_nearDistanceMod.GetModifiedValue(m_nearDistance);
		}
		else
		{
			result = m_nearDistance;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		ActorData actorData = base.ActorData;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				if (actorData != null)
				{
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					int num = GetDamageAmount();
					if (m_syncComponent != null && m_syncComponent.m_overchargeBuffs > 0)
					{
						if (m_overchargeAbility != null && m_overchargeAbility.GetExtraDamageForDelayedLaser() > 0)
						{
							num += m_overchargeAbility.GetExtraDamageForDelayedLaser();
						}
					}
					Vector3 vector;
					if (m_syncComponent.m_canActivateDelayedLaser)
					{
						vector = m_syncComponent.m_delayedLaserStartPos;
					}
					else
					{
						vector = actorData.GetTravelBoardSquareWorldPosition();
					}
					Vector3 b = vector;
					if (GetExtraDamageToNearEnemy() > 0 && GetNearDistance() > 0f)
					{
						float num2 = GetNearDistance() * Board.Get().squareSize;
						Vector3 vector2 = targetActor.GetTravelBoardSquareWorldPosition() - b;
						vector2.y = 0f;
						if (vector2.magnitude <= num2)
						{
							num += GetExtraDamageToNearEnemy();
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
		AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_BlasterDelayedLaser) ? m_damageAmount : abilityMod_BlasterDelayedLaser.m_damageAmountMod.GetModifiedValue(m_damageAmount));
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_BlasterDelayedLaser)
		{
			effectInfo = abilityMod_BlasterDelayedLaser.m_effectOnHitMod.GetModifiedValue(m_effectOnHit);
		}
		else
		{
			effectInfo = m_effectOnHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnHit", m_effectOnHit);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_BlasterDelayedLaser)
		{
			effectInfo2 = abilityMod_BlasterDelayedLaser.m_onCastEnemyHitEffectMod.GetModifiedValue(m_onCastEnemyHitEffect);
		}
		else
		{
			effectInfo2 = m_onCastEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "OnCastEnemyHitEffect", m_onCastEnemyHitEffect);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_BlasterDelayedLaser)
		{
			val = abilityMod_BlasterDelayedLaser.m_extraDamageToNearEnemyMod.GetModifiedValue(m_extraDamageToNearEnemy);
		}
		else
		{
			val = m_extraDamageToNearEnemy;
		}
		AddTokenInt(tokens, "ExtraDamageToNearEnemy", empty, val);
		AddTokenInt(tokens, "MaxTurnsBeforeTrigger", string.Empty, m_turnsBeforeTriggering);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BlasterDelayedLaser))
		{
			m_abilityMod = (abilityMod as AbilityMod_BlasterDelayedLaser);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public override bool IsFreeAction()
	{
		if (m_remoteTriggerMode)
		{
			if (m_syncComponent != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (m_syncComponent.m_canActivateDelayedLaser)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									return m_remoteTriggerIsFreeAction;
								}
							}
						}
						return base.IsFreeAction();
					}
				}
			}
		}
		return base.IsFreeAction();
	}

	public override AbilityPriority GetRunPriority()
	{
		if (m_remoteTriggerMode && m_syncComponent != null)
		{
			if (m_syncComponent.m_canActivateDelayedLaser)
			{
				if (GameFlowData.Get().CurrentTurn > m_syncComponent.m_lastPlacementTurn)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return base.GetRunPriority();
						}
					}
				}
			}
		}
		return m_placementPhase;
	}

	public override TargetData[] GetTargetData()
	{
		if (m_remoteTriggerMode)
		{
			if (m_syncComponent != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (m_syncComponent.m_canActivateDelayedLaser)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									return new TargetData[0];
								}
							}
						}
						return base.GetTargetData();
					}
				}
			}
		}
		return base.GetTargetData();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (m_syncComponent != null)
		{
			if (m_syncComponent.m_canActivateDelayedLaser)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return ActorModelData.ActionAnimationType.None;
					}
				}
			}
		}
		return base.GetActionAnimType();
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		if (m_syncComponent != null)
		{
			if (m_syncComponent.m_canActivateDelayedLaser)
			{
				if (animIndex == (int)base.GetActionAnimType())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
		}
		return false;
	}
}
