using System.Collections.Generic;
using UnityEngine;

public class SniperBasicAttack : Ability
{
	public int m_laserDamageAmount = 5;

	public int m_minDamageAmount;

	public int m_damageChangePerHit;

	public LaserTargetingInfo m_laserInfo;

	public StandardEffectInfo m_laserHitEffect;

	private AbilityMod_SniperBasicAttack m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Sniper Shot";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserRange(), GetLaserPenetratesLoS(), GetMaxTargets());
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
		if (m_laserDamageAmount > 0)
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
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		}
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = base.Targeter as AbilityUtil_Targeter_Laser;
		if (abilityUtil_Targeter_Laser != null)
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
			ActorData component = GetComponent<ActorData>();
			if (component != null)
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
				List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext = abilityUtil_Targeter_Laser.GetHitActorContext();
				for (int i = 0; i < hitActorContext.Count; i++)
				{
					AbilityUtil_Targeter_Laser.HitActorContext hitActorContext2 = hitActorContext[i];
					if (!(hitActorContext2.actor == targetActor))
					{
						continue;
					}
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						if (targetActor.GetTeam() != component.GetTeam())
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
							int hitOrder = i;
							AbilityUtil_Targeter_Laser.HitActorContext hitActorContext3 = hitActorContext[i];
							int num = dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmountByHitOrder(hitOrder, hitActorContext3.squaresFromCaster);
						}
						return dictionary;
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperBasicAttack abilityMod_SniperBasicAttack = modAsBase as AbilityMod_SniperBasicAttack;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SniperBasicAttack)
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
			val = abilityMod_SniperBasicAttack.m_damageMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			val = m_laserDamageAmount;
		}
		AddTokenInt(tokens, "LaserDamageAmount", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_SniperBasicAttack)
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
			val2 = abilityMod_SniperBasicAttack.m_minDamageMod.GetModifiedValue(m_minDamageAmount);
		}
		else
		{
			val2 = m_minDamageAmount;
		}
		AddTokenInt(tokens, "MinDamageAmount", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_SniperBasicAttack)
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
			val3 = abilityMod_SniperBasicAttack.m_damageChangePerHitMod.GetModifiedValue(m_damageChangePerHit);
		}
		else
		{
			val3 = m_damageChangePerHit;
		}
		AddTokenInt(tokens, "DamageChangePerHit", empty3, val3);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", m_laserHitEffect);
	}

	public int GetDamageAmountByHitOrder(int hitOrder, float distanceFromCasterInSquares)
	{
		int num = GetBaseDamage();
		if (GetFarDistanceThreshold() > 0f)
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
			if (distanceFromCasterInSquares > GetFarDistanceThreshold() && GetFarEnemyDamageAmount() > 0)
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
				num = GetFarEnemyDamageAmount();
			}
		}
		int b = num + hitOrder * GetDamageChangePerHit();
		return Mathf.Max(GetMinDamage(), b);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SniperBasicAttack))
		{
			m_abilityMod = (abilityMod as AbilityMod_SniperBasicAttack);
			SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public float GetLaserWidth()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserInfo.width) : m_laserInfo.width;
	}

	public float GetLaserRange()
	{
		if (m_abilityMod != null)
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
					if (m_abilityMod.m_useTargetDataOverrides)
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
						if (m_abilityMod.m_targetDataOverrides.Length > 0)
						{
							return m_abilityMod.m_targetDataOverrides[0].m_range;
						}
					}
					return m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserInfo.range);
				}
			}
		}
		return m_laserInfo.range;
	}

	public bool GetLaserPenetratesLoS()
	{
		if (m_abilityMod != null)
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
			if (m_abilityMod.m_useTargetDataOverrides)
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
				if (m_abilityMod.m_targetDataOverrides.Length > 0)
				{
					return !m_abilityMod.m_targetDataOverrides[0].m_checkLineOfSight;
				}
			}
		}
		return m_laserInfo.penetrateLos;
	}

	public int GetMaxTargets()
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
			result = m_laserInfo.maxTargets;
		}
		else
		{
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_laserInfo.maxTargets);
		}
		return result;
	}

	public int GetBaseDamage()
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
			result = m_laserDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_laserDamageAmount);
		}
		return result;
	}

	public int GetMinDamage()
	{
		int b;
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
			b = m_minDamageAmount;
		}
		else
		{
			b = m_abilityMod.m_minDamageMod.GetModifiedValue(m_minDamageAmount);
		}
		return Mathf.Max(0, b);
	}

	public int GetDamageChangePerHit()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_damageChangePerHit;
		}
		else
		{
			result = m_abilityMod.m_damageChangePerHitMod.GetModifiedValue(m_damageChangePerHit);
		}
		return result;
	}

	public float GetFarDistanceThreshold()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_farDistanceThreshold : 0f;
	}

	public int GetFarEnemyDamageAmount()
	{
		int result;
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
			result = m_laserDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_farEnemyDamageMod.GetModifiedValue(m_laserDamageAmount);
		}
		return result;
	}
}
