using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalDrag : Ability
{
	public float m_width = 1f;

	public float m_distance = 3f;

	public int m_maxTargets = 1;

	public bool m_penetrateLineOfSight;

	public int m_damage;

	public StandardEffectInfo m_casterEffect;

	public StandardEffectInfo m_targetEffect;

	private AbilityMod_RobotAnimalDrag m_abilityMod;

	private StandardEffectInfo m_cachedCasterEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Death Snuggle";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserDistance(), m_penetrateLineOfSight, m_maxTargets, false, GetCasterEffect().m_applyEffect);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserDistance();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedCasterEffect;
		if ((bool)m_abilityMod)
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
			cachedCasterEffect = m_abilityMod.m_casterEffectMod.GetModifiedValue(m_casterEffect);
		}
		else
		{
			cachedCasterEffect = m_casterEffect;
		}
		m_cachedCasterEffect = cachedCasterEffect;
	}

	public StandardEffectInfo GetCasterEffect()
	{
		StandardEffectInfo result;
		if (m_cachedCasterEffect != null)
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
			result = m_cachedCasterEffect;
		}
		else
		{
			result = m_casterEffect;
		}
		return result;
	}

	private float GetLaserDistance()
	{
		float result;
		if (m_abilityMod == null)
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
			result = m_distance;
		}
		else
		{
			result = m_abilityMod.m_distanceMod.GetModifiedValue(m_distance);
		}
		return result;
	}

	private float GetLaserWidth()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_widthMod.GetModifiedValue(m_width) : m_width;
	}

	public int GetDamage()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
		}
		else
		{
			result = m_damage;
		}
		return result;
	}

	public bool HasEffectOnNextTurnStart()
	{
		int result;
		if (m_abilityMod == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = 0;
		}
		else if (!m_abilityMod.m_enemyEffectOnNextTurnStart.m_applyEffect)
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
			if (m_abilityMod.m_powerUpsToSpawn != null)
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
				result = ((m_abilityMod.m_powerUpsToSpawn.Count > 0) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public StandardEffectInfo EffectInfoOnNextTurnStart()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
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
			result = new StandardEffectInfo();
		}
		else
		{
			result = m_abilityMod.m_enemyEffectOnNextTurnStart;
		}
		return result;
	}

	public List<PowerUp> GetModdedPowerUpsToSpawn()
	{
		object result;
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
			result = null;
		}
		else
		{
			result = m_abilityMod.m_powerUpsToSpawn;
		}
		return (List<PowerUp>)result;
	}

	public AbilityAreaShape GetModdedPowerUpsToSpawnShape()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_powerUpsSpawnShape : AbilityAreaShape.SingleSquare;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamage());
		GetCasterEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		m_targetEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalDrag abilityMod_RobotAnimalDrag = modAsBase as AbilityMod_RobotAnimalDrag;
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_RobotAnimalDrag)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			effectInfo = abilityMod_RobotAnimalDrag.m_casterEffectMod.GetModifiedValue(m_casterEffect);
		}
		else
		{
			effectInfo = m_casterEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "CasterEffect", m_casterEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetEffect, "TargetEffect", null, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RobotAnimalDrag))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_RobotAnimalDrag);
					Setup();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
