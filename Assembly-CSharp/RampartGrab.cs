using System.Collections.Generic;
using UnityEngine;

public class RampartGrab : Ability
{
	[Header("-- On Hit Damage and Effect")]
	public int m_damageAmount = 10;

	public int m_damageAfterFirstHit;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Knockback Targeting")]
	public bool m_chooseEndPosition = true;

	public int m_maxTargets = 1;

	public float m_laserRange = 3f;

	public float m_laserWidth = 2f;

	public bool m_penetrateLos;

	[Header("-- Targeting Ranges")]
	public float m_destinationSelectRange = 1f;

	public int m_destinationAngleDegWithBack = 90;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private float m_knockbackDistance = 100f;

	private AbilityMod_RampartGrab m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Grab";
		}
		if (GetNumTargets() != 2)
		{
			Debug.LogError("Need 2 entries in Target Data");
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		ClearTargeters();
		if (ChooseEndPosition())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					ClearTargeters();
					AbilityUtil_Targeter_Laser item = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserRange(), PenetrateLos(), GetMaxTargets());
					base.Targeters.Add(item);
					AbilityUtil_Targeter_RampartGrab abilityUtil_Targeter_RampartGrab = new AbilityUtil_Targeter_RampartGrab(this, AbilityAreaShape.SingleSquare, m_knockbackDistance, KnockbackType.PullToSource, GetLaserRange(), GetLaserWidth(), PenetrateLos(), GetMaxTargets());
					abilityUtil_Targeter_RampartGrab.SetUseMultiTargetUpdate(true);
					base.Targeters.Add(abilityUtil_Targeter_RampartGrab);
					return;
				}
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_KnockbackLaser(this, GetLaserWidth(), GetLaserRange(), PenetrateLos(), GetMaxTargets(), m_knockbackDistance, m_knockbackDistance, KnockbackType.PullToSourceActor, false);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result;
		if (ChooseEndPosition())
		{
			result = 2;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = ((!m_abilityMod) ? m_enemyHitEffect : m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect));
	}

	public int GetDamageAmount()
	{
		return (!m_abilityMod) ? m_damageAmount : m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
	}

	public int GetDamageAfterFirstHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAfterFirstHitMod.GetModifiedValue(m_damageAfterFirstHit);
		}
		else
		{
			result = m_damageAfterFirstHit;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return (m_cachedEnemyHitEffect == null) ? m_enemyHitEffect : m_cachedEnemyHitEffect;
	}

	public bool ChooseEndPosition()
	{
		return (!m_abilityMod) ? m_chooseEndPosition : m_abilityMod.m_chooseEndPositionMod.GetModifiedValue(m_chooseEndPosition);
	}

	public int GetMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			result = m_maxTargets;
		}
		return result;
	}

	public float GetLaserRange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
		}
		else
		{
			result = m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public bool PenetrateLos()
	{
		return (!m_abilityMod) ? m_penetrateLos : m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
	}

	public float GetDestinationSelectRange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_destinationSelectRangeMod.GetModifiedValue(m_destinationSelectRange);
		}
		else
		{
			result = m_destinationSelectRange;
		}
		return result;
	}

	public int GetDestinationAngleDegWithBack()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_destinationAngleDegWithBackMod.GetModifiedValue(m_destinationAngleDegWithBack);
		}
		else
		{
			result = m_destinationAngleDegWithBack;
		}
		return result;
	}

	public int CalcDamageForOrderIndex(int hitOrder)
	{
		int damageAfterFirstHit = GetDamageAfterFirstHit();
		if (damageAfterFirstHit > 0)
		{
			if (hitOrder > 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return damageAfterFirstHit;
					}
				}
			}
		}
		return GetDamageAmount();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartGrab abilityMod_RampartGrab = modAsBase as AbilityMod_RampartGrab;
		AddTokenInt(tokens, "DamageAmount", string.Empty, (!abilityMod_RampartGrab) ? m_damageAmount : abilityMod_RampartGrab.m_damageAmountMod.GetModifiedValue(m_damageAmount));
		AddTokenInt(tokens, "DamageAfterFirstHit", string.Empty, m_damageAfterFirstHit);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_RampartGrab)
		{
			effectInfo = abilityMod_RampartGrab.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "MaxTargets", string.Empty, (!abilityMod_RampartGrab) ? m_maxTargets : abilityMod_RampartGrab.m_maxTargetsMod.GetModifiedValue(m_maxTargets));
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RampartGrab)
		{
			val = abilityMod_RampartGrab.m_destinationAngleDegWithBackMod.GetModifiedValue(m_destinationAngleDegWithBack);
		}
		else
		{
			val = m_destinationAngleDegWithBack;
		}
		AddTokenInt(tokens, "DestinationAngleDegWithBack", empty, val);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
		{
			if (base.Targeter is AbilityUtil_Targeter_Laser)
			{
				AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = base.Targeter as AbilityUtil_Targeter_Laser;
				List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext = abilityUtil_Targeter_Laser.GetHitActorContext();
				int num = 0;
				while (true)
				{
					if (num < hitActorContext.Count)
					{
						AbilityUtil_Targeter_Laser.HitActorContext hitActorContext2 = hitActorContext[num];
						if (hitActorContext2.actor == targetActor)
						{
							results.m_damage = CalcDamageForOrderIndex(num);
							break;
						}
						num++;
						continue;
					}
					break;
				}
			}
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex == 0)
		{
			return true;
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				bool result = false;
				if (boardSquareSafe != caster.GetCurrentBoardSquare())
				{
					float num = VectorUtils.HorizontalPlaneDistInSquares(boardSquareSafe.ToVector3(), caster.GetTravelBoardSquareWorldPosition());
					if (num <= GetDestinationSelectRange())
					{
						Vector3 from = -1f * currentTargets[0].AimDirection;
						Vector3 to = boardSquareSafe.ToVector3() - caster.GetTravelBoardSquareWorldPosition();
						from.y = 0f;
						to.y = 0f;
						int num2 = Mathf.RoundToInt(Vector3.Angle(from, to));
						if (num2 <= GetDestinationAngleDegWithBack())
						{
							result = true;
						}
					}
				}
				return result;
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartGrab))
		{
			m_abilityMod = (abilityMod as AbilityMod_RampartGrab);
		}
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
