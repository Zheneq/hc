using System.Collections.Generic;
using UnityEngine;

public class SorceressHealingCrossBeam : Ability
{
	[Header("-- Enemy Hit Damage and Effect")]
	public int m_damageAmount = 10;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Ally Hit Heal and Effect")]
	public int m_healAmount = 5;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- Targeting")]
	public float m_width = 1f;

	public float m_distance = 15f;

	public int m_numLasers = 4;

	public bool m_alsoHealSelf = true;

	public bool m_penetrateLineOfSight;

	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_beamSequencePrefab;

	public GameObject m_centerSequencePrefab;

	public GameObject m_healSequencePrefab;

	private AbilityUtil_Targeter_CrossBeam m_customTargeter;

	private AbilityMod_SorceressHealingCrossBeam m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_customTargeter = new AbilityUtil_Targeter_CrossBeam(this, GetNumLasers(), GetLaserRange(), GetLaserWidth(), m_penetrateLineOfSight, true, m_alsoHealSelf);
		m_customTargeter.SetKnockbackParams(GetKnockbackDistance(), GetKnockbackType(), GetKnockbackThresholdDistance());
		base.Targeter = m_customTargeter;
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
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_healAmount));
		if (m_alsoHealSelf)
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
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, m_healAmount));
		}
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			List<AbilityUtil_Targeter_CrossBeam.HitActorContext> hitActorContext = m_customTargeter.GetHitActorContext();
			int numTargetsInLaser = 0;
			using (List<AbilityUtil_Targeter_CrossBeam.HitActorContext>.Enumerator enumerator = hitActorContext.GetEnumerator())
			{
				while (true)
				{
					if (!enumerator.MoveNext())
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								if (1 == 0)
								{
									/*OpCode not supported: LdMemberToken*/;
								}
								goto end_IL_0035;
							}
						}
					}
					AbilityUtil_Targeter_CrossBeam.HitActorContext current = enumerator.Current;
					if (current.actor == targetActor)
					{
						numTargetsInLaser = current.totalTargetsInLaser;
						break;
					}
				}
				end_IL_0035:;
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmount(numTargetsInLaser);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
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
				dictionary[AbilityTooltipSymbol.Healing] = GetHealAmount(numTargetsInLaser);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				dictionary[AbilityTooltipSymbol.Healing] = GetHealAmount(numTargetsInLaser);
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressHealingCrossBeam abilityMod_SorceressHealingCrossBeam = modAsBase as AbilityMod_SorceressHealingCrossBeam;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SorceressHealingCrossBeam)
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
			val = abilityMod_SorceressHealingCrossBeam.m_normalDamageMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_SorceressHealingCrossBeam)
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
			effectInfo = abilityMod_SorceressHealingCrossBeam.m_enemyEffectOverride.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", m_enemyHitEffect);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_SorceressHealingCrossBeam)
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
			val2 = abilityMod_SorceressHealingCrossBeam.m_normalHealingMod.GetModifiedValue(m_healAmount);
		}
		else
		{
			val2 = m_healAmount;
		}
		AddTokenInt(tokens, "HealAmount", empty2, val2);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_SorceressHealingCrossBeam)
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
			effectInfo2 = abilityMod_SorceressHealingCrossBeam.m_allyEffectOverride.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			effectInfo2 = m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyHitEffect", m_allyHitEffect);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_SorceressHealingCrossBeam)
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
			val3 = abilityMod_SorceressHealingCrossBeam.m_laserNumberMod.GetModifiedValue(m_numLasers);
		}
		else
		{
			val3 = m_numLasers;
		}
		AddTokenInt(tokens, "NumLasers", empty3, val3);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SorceressHealingCrossBeam))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_SorceressHealingCrossBeam);
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

	private int GetNumLasers()
	{
		int result = m_numLasers;
		if (m_abilityMod != null)
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
			result = Mathf.Max(1, m_abilityMod.m_laserNumberMod.GetModifiedValue(m_numLasers));
		}
		return result;
	}

	private int GetDamageAmount(int numTargetsInLaser)
	{
		int result = m_damageAmount;
		if (m_abilityMod != null)
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
			if (numTargetsInLaser == 1)
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
				if (m_abilityMod.m_useSingleTargetHitMods)
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
					result = m_abilityMod.m_singleTargetDamageMod.GetModifiedValue(m_damageAmount);
					goto IL_0083;
				}
			}
			result = m_abilityMod.m_normalDamageMod.GetModifiedValue(m_damageAmount);
		}
		goto IL_0083;
		IL_0083:
		return result;
	}

	private int GetHealAmount(int numTargetsInLaser)
	{
		int result = m_healAmount;
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
			if (numTargetsInLaser == 1)
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
				if (m_abilityMod.m_useSingleTargetHitMods)
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
					result = m_abilityMod.m_singleTargetHealingMod.GetModifiedValue(m_healAmount);
					goto IL_0083;
				}
			}
			result = m_abilityMod.m_normalHealingMod.GetModifiedValue(m_healAmount);
		}
		goto IL_0083;
		IL_0083:
		return result;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_enemyEffectOverride.GetModifiedValue(m_enemyHitEffect) : m_enemyHitEffect;
	}

	private StandardEffectInfo GetAllyHitEffect()
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
			result = m_allyHitEffect;
		}
		else
		{
			result = m_abilityMod.m_allyEffectOverride.GetModifiedValue(m_allyHitEffect);
		}
		return result;
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
			result = m_distance;
		}
		else
		{
			result = m_abilityMod.m_laserRangeMod.GetModifiedValue(m_distance);
		}
		return result;
	}

	private float GetKnockbackDistance()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_knockbackDistance : 0f;
	}

	private KnockbackType GetKnockbackType()
	{
		int result;
		if (m_abilityMod == null)
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
			result = 4;
		}
		else
		{
			result = (int)m_abilityMod.m_knockbackType;
		}
		return (KnockbackType)result;
	}

	private float GetKnockbackThresholdDistance()
	{
		float result;
		if (m_abilityMod == null)
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
			result = -1f;
		}
		else
		{
			result = m_abilityMod.m_knockbackThresholdDistance;
		}
		return result;
	}
}
