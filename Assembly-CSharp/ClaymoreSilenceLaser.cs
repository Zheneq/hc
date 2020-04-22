using System.Collections.Generic;
using UnityEngine;

public class ClaymoreSilenceLaser : Ability
{
	[Header("-- Targeting")]
	public float m_laserRange = 4f;

	public float m_laserWidth = 1f;

	public int m_laserMaxTargets;

	public bool m_penetrateLos;

	[Header("-- Hit Damage/Effects")]
	public int m_onCastDamageAmount;

	public StandardActorEffectData m_enemyHitEffectData;

	[Header("-- On Reaction Hit/Explosion Triggered")]
	public int m_effectExplosionDamage = 10;

	public int m_explosionDamageAfterFirstHit;

	public bool m_explosionReduceCooldownOnlyIfHitByAlly;

	public int m_explosionCooldownReduction;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_effectOnExplosionSequencePrefab;

	private AbilityMod_ClaymoreSilenceLaser m_abilityMod;

	private StandardActorEffectData m_cachedEnemyHitEffectData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Dirty Fighting";
		}
		SetupTargeter();
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
		StandardActorEffectData cachedEnemyHitEffectData;
		if ((bool)m_abilityMod)
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
			cachedEnemyHitEffectData = m_abilityMod.m_enemyHitEffectDataMod.GetModifiedValue(m_enemyHitEffectData);
		}
		else
		{
			cachedEnemyHitEffectData = m_enemyHitEffectData;
		}
		m_cachedEnemyHitEffectData = cachedEnemyHitEffectData;
	}

	public float GetLaserRange()
	{
		float result;
		if ((bool)m_abilityMod)
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
		return (!m_abilityMod) ? m_laserWidth : m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
	}

	public int GetLaserMaxTargets()
	{
		return (!m_abilityMod) ? m_laserMaxTargets : m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets);
	}

	public bool GetPenetrateLos()
	{
		bool result;
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
			result = m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
		}
		else
		{
			result = m_penetrateLos;
		}
		return result;
	}

	public int GetOnCastDamageAmount()
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
			result = m_abilityMod.m_onCastDamageAmountMod.GetModifiedValue(m_onCastDamageAmount);
		}
		else
		{
			result = m_onCastDamageAmount;
		}
		return result;
	}

	public StandardActorEffectData GetEnemyHitEffectData()
	{
		StandardActorEffectData result;
		if (m_cachedEnemyHitEffectData != null)
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
			result = m_cachedEnemyHitEffectData;
		}
		else
		{
			result = m_enemyHitEffectData;
		}
		return result;
	}

	public int GetEffectExplosionDamage()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_effectExplosionDamageMod.GetModifiedValue(m_effectExplosionDamage);
		}
		else
		{
			result = m_effectExplosionDamage;
		}
		return result;
	}

	public int GetExplosionDamageAfterFirstHit()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_explosionDamageAfterFirstHitMod.GetModifiedValue(m_explosionDamageAfterFirstHit);
		}
		else
		{
			result = m_explosionDamageAfterFirstHit;
		}
		return result;
	}

	public bool ExplosionReduceCooldownOnlyIfHitByAlly()
	{
		return (!m_abilityMod) ? m_explosionReduceCooldownOnlyIfHitByAlly : m_abilityMod.m_explosionReduceCooldownOnlyIfHitByAllyMod.GetModifiedValue(m_explosionReduceCooldownOnlyIfHitByAlly);
	}

	public int GetExplosionCooldownReduction()
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
			result = m_abilityMod.m_explosionCooldownReductionMod.GetModifiedValue(m_explosionCooldownReduction);
		}
		else
		{
			result = m_explosionCooldownReduction;
		}
		return result;
	}

	public bool CanExplodeOncePerTurn()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = (m_abilityMod.m_canExplodeOncePerTurnMod.GetModifiedValue(false) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public int CalcExplosionDamageForOrderIndex(int hitOrder)
	{
		int explosionDamageAfterFirstHit = GetExplosionDamageAfterFirstHit();
		if (explosionDamageAfterFirstHit > 0 && hitOrder > 0)
		{
			return explosionDamageAfterFirstHit;
		}
		return GetEffectExplosionDamage();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserRange(), GetPenetrateLos(), GetLaserMaxTargets());
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreSilenceLaser abilityMod_ClaymoreSilenceLaser = modAsBase as AbilityMod_ClaymoreSilenceLaser;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ClaymoreSilenceLaser)
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
			val = abilityMod_ClaymoreSilenceLaser.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets);
		}
		else
		{
			val = m_laserMaxTargets;
		}
		AddTokenInt(tokens, "LaserMaxTargets", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ClaymoreSilenceLaser)
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
			val2 = abilityMod_ClaymoreSilenceLaser.m_onCastDamageAmountMod.GetModifiedValue(m_onCastDamageAmount);
		}
		else
		{
			val2 = m_onCastDamageAmount;
		}
		AddTokenInt(tokens, "OnCastDamageAmount", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_ClaymoreSilenceLaser)
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
			val3 = abilityMod_ClaymoreSilenceLaser.m_effectExplosionDamageMod.GetModifiedValue(m_effectExplosionDamage);
		}
		else
		{
			val3 = m_effectExplosionDamage;
		}
		AddTokenInt(tokens, "EffectExplosionDamage", empty3, val3);
		AddTokenInt(tokens, "ExplosionDamageAfterFirstHit", string.Empty, m_explosionDamageAfterFirstHit);
		StandardActorEffectData standardActorEffectData;
		if ((bool)abilityMod_ClaymoreSilenceLaser)
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
			standardActorEffectData = abilityMod_ClaymoreSilenceLaser.m_enemyHitEffectDataMod.GetModifiedValue(m_enemyHitEffectData);
		}
		else
		{
			standardActorEffectData = m_enemyHitEffectData;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "EnemyHitEffectData", abilityMod_ClaymoreSilenceLaser != null, m_enemyHitEffectData);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (GetOnCastDamageAmount() > 0)
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
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetOnCastDamageAmount());
		}
		else
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetEffectExplosionDamage());
		}
		GetEnemyHitEffectData().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (GetOnCastDamageAmount() <= 0)
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
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
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
				if (base.Targeter is AbilityUtil_Targeter_Laser)
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
								results.m_damage = CalcExplosionDamageForOrderIndex(num);
								break;
							}
							num++;
							continue;
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						break;
					}
				}
			}
		}
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ClaymoreSilenceLaser))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_ClaymoreSilenceLaser);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
