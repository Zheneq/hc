using System.Collections.Generic;
using UnityEngine;

public class SorceressDebuffLaser : Ability
{
	public bool m_penetrateLineOfSight;

	public float m_width = 1f;

	public float m_distance = 15f;

	[Header("-- Hit Effects")]
	public StandardEffectInfo m_enemyHitEffect;

	public StandardEffectInfo m_allyHitEffect;

	public StandardEffectInfo m_casterHitEffect;

	private AbilityMod_SorceressDebuffLaser m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserRange(), m_penetrateLineOfSight, -1, GetAllyHitEffect().m_applyEffect, GetCasterHitEffect().m_applyEffect);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		m_allyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		m_casterHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressDebuffLaser abilityMod_SorceressDebuffLaser = modAsBase as AbilityMod_SorceressDebuffLaser;
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_SorceressDebuffLaser)
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
			effectInfo = abilityMod_SorceressDebuffLaser.m_enemyHitEffectOverride.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", m_enemyHitEffect);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_SorceressDebuffLaser)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo2 = abilityMod_SorceressDebuffLaser.m_allyHitEffectOverride.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			effectInfo2 = m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyHitEffect", m_allyHitEffect);
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_SorceressDebuffLaser)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo3 = abilityMod_SorceressDebuffLaser.m_casterHitEffectOverride.GetModifiedValue(m_casterHitEffect);
		}
		else
		{
			effectInfo3 = m_casterHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "CasterHitEffect", m_casterHitEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SorceressDebuffLaser))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_SorceressDebuffLaser);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private float GetLaserWidth()
	{
		float result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (5)
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
			result = m_width;
		}
		else
		{
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_width);
		}
		return result;
	}

	private float GetLaserRange()
	{
		float result;
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
			result = m_distance;
		}
		else
		{
			result = m_abilityMod.m_laserRangeMod.GetModifiedValue(m_distance);
		}
		return result;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
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
			result = m_enemyHitEffect;
		}
		else
		{
			result = m_abilityMod.m_enemyHitEffectOverride.GetModifiedValue(m_enemyHitEffect);
		}
		return result;
	}

	private StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (5)
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
			result = m_allyHitEffect;
		}
		else
		{
			result = m_abilityMod.m_allyHitEffectOverride.GetModifiedValue(m_allyHitEffect);
		}
		return result;
	}

	private StandardEffectInfo GetCasterHitEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
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
			result = m_casterHitEffect;
		}
		else
		{
			result = m_abilityMod.m_casterHitEffectOverride.GetModifiedValue(m_casterHitEffect);
		}
		return result;
	}

	private bool HasAdditionalEffectIfHit()
	{
		return m_abilityMod != null && m_abilityMod.m_additionalEffectOnSelfIfHit.m_applyEffect;
	}

	private int GetEnemyEffectDuration()
	{
		int result = m_enemyHitEffect.m_effectData.m_duration;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_enemyEffectDurationMod.GetModifiedValue(GetEnemyHitEffect().m_effectData.m_duration);
		}
		return result;
	}

	private int GetAllyEffectDuration()
	{
		int result = m_allyHitEffect.m_effectData.m_duration;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_allyEffectDurationMod.GetModifiedValue(GetAllyHitEffect().m_effectData.m_duration);
		}
		return result;
	}

	private int GetCasterEffectDuration()
	{
		int result = m_casterHitEffect.m_effectData.m_duration;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (5)
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
			result = m_abilityMod.m_casterEffectDurationMod.GetModifiedValue(GetCasterHitEffect().m_effectData.m_duration);
		}
		return result;
	}

	private int GetCooldownReduction(int numHit)
	{
		int result = 0;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (1)
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
			result = m_abilityMod.m_cooldownReductionOnNumHit.GetModifiedValue(numHit);
			result += m_abilityMod.m_cooldownFlatReduction;
			result = Mathf.Clamp(result, 0, m_abilityMod.m_maxCooldownReduction);
		}
		return result;
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> points = new List<Vector3>();
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		Vector3 aimDirection = targets[0].AimDirection;
		float maxDistanceInWorld = GetLaserRange() * Board.Get().squareSize;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, aimDirection, maxDistanceInWorld, m_penetrateLineOfSight, caster);
		AreaEffectUtils.AddBoxExtremaToList(ref points, travelBoardSquareWorldPositionForLos, laserEndPoint, GetLaserWidth());
		return points;
	}
}
