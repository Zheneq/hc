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
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Knockback Cone";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_overchargeAbility = GetAbilityOfType<BlasterOvercharge>();
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_StretchCone(this, GetMinLength(), GetMaxLength(), GetMinAngle(), GetMaxAngle(), m_stretchStyle, GetConeBackwardOffset(), PenetrateLineOfSight());
		AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = base.Targeter as AbilityUtil_Targeter_StretchCone;
		abilityUtil_Targeter_StretchCone.InitKnockbackData(GetKnockbackDistance(), m_knockbackType, GetKnockbackDistanceOnSelf(), m_knockbackTypeOnSelf);
		abilityUtil_Targeter_StretchCone.SetExtraKnockbackDist(GetExtraKnockbackDistOnOvercharged());
		abilityUtil_Targeter_StretchCone.m_useExtraKnockbackDistDelegate = delegate
		{
			int result;
			if (m_syncComp != null)
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
				result = ((m_syncComp.m_overchargeBuffs > 0) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxLength();
	}

	private void SetCachedFields()
	{
		m_cachedEnemyEffectNormal = ((!m_abilityMod) ? m_enemyEffectNormal : m_abilityMod.m_enemyEffectNormalMod.GetModifiedValue(m_enemyEffectNormal));
		StandardEffectInfo cachedEnemyEffectOvercharged;
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
			cachedEnemyEffectOvercharged = m_abilityMod.m_enemyEffectOverchargedMod.GetModifiedValue(m_enemyEffectOvercharged);
		}
		else
		{
			cachedEnemyEffectOvercharged = m_enemyEffectOvercharged;
		}
		m_cachedEnemyEffectOvercharged = cachedEnemyEffectOvercharged;
	}

	public float GetMinLength()
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
			result = m_abilityMod.m_minLengthMod.GetModifiedValue(m_minLength);
		}
		else
		{
			result = m_minLength;
		}
		return result;
	}

	public float GetMaxLength()
	{
		return (!m_abilityMod) ? m_maxLength : m_abilityMod.m_maxLengthMod.GetModifiedValue(m_maxLength);
	}

	public float GetMinAngle()
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
			result = m_abilityMod.m_minAngleMod.GetModifiedValue(m_minAngle);
		}
		else
		{
			result = m_minAngle;
		}
		return result;
	}

	public float GetMaxAngle()
	{
		return (!m_abilityMod) ? m_maxAngle : m_abilityMod.m_maxAngleMod.GetModifiedValue(m_maxAngle);
	}

	public float GetConeBackwardOffset()
	{
		float result;
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
			result = m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
		}
		else
		{
			result = m_coneBackwardOffset;
		}
		return result;
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
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
			result = m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
		}
		else
		{
			result = m_penetrateLineOfSight;
		}
		return result;
	}

	public int GetDamageAmountNormal()
	{
		return (!m_abilityMod) ? m_damageAmountNormal : m_abilityMod.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal);
	}

	public StandardEffectInfo GetEnemyEffectNormal()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyEffectNormal != null)
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
			result = m_cachedEnemyEffectNormal;
		}
		else
		{
			result = m_enemyEffectNormal;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyEffectOvercharged()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyEffectOvercharged != null)
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
			result = m_cachedEnemyEffectOvercharged;
		}
		else
		{
			result = m_enemyEffectOvercharged;
		}
		return result;
	}

	public float GetKnockbackDistance()
	{
		float result;
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
			result = m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance);
		}
		else
		{
			result = m_knockbackDistance;
		}
		return result;
	}

	public float GetExtraKnockbackDistOnOvercharged()
	{
		float result;
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
			result = m_abilityMod.m_extraKnockbackDistOnOverchargedMod.GetModifiedValue(m_extraKnockbackDistOnOvercharged);
		}
		else
		{
			result = m_extraKnockbackDistOnOvercharged;
		}
		return result;
	}

	public float GetKnockbackDistanceOnSelf()
	{
		float result;
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
			result = m_abilityMod.m_knockbackDistanceOnSelfMod.GetModifiedValue(m_knockbackDistanceOnSelf);
		}
		else
		{
			result = m_knockbackDistanceOnSelf;
		}
		return result;
	}

	public bool OverchargeAsFreeActionAfterCast()
	{
		return (!m_abilityMod) ? m_overchargeAsFreeActionAfterCast : m_abilityMod.m_overchargeAsFreeActionAfterCastMod.GetModifiedValue(m_overchargeAsFreeActionAfterCast);
	}

	public int GetCurrentModdedDamage()
	{
		if (AmOvercharged(base.ActorData))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return GetDamageAmountNormal() + m_overchargeAbility.GetExtraDamage() + GetMultiStackOverchargeDamage();
				}
			}
		}
		return GetDamageAmountNormal();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterKnockbackCone abilityMod_BlasterKnockbackCone = modAsBase as AbilityMod_BlasterKnockbackCone;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_BlasterKnockbackCone)
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
			val = abilityMod_BlasterKnockbackCone.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal);
		}
		else
		{
			val = m_damageAmountNormal;
		}
		AddTokenInt(tokens, "Damage", empty, val);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_BlasterKnockbackCone)
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
			effectInfo = abilityMod_BlasterKnockbackCone.m_enemyEffectNormalMod.GetModifiedValue(m_enemyEffectNormal);
		}
		else
		{
			effectInfo = m_enemyEffectNormal;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyEffectNormal", m_enemyEffectNormal);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_BlasterKnockbackCone)
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
			effectInfo2 = abilityMod_BlasterKnockbackCone.m_enemyEffectOverchargedMod.GetModifiedValue(m_enemyEffectOvercharged);
		}
		else
		{
			effectInfo2 = m_enemyEffectOvercharged;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EnemyEffectOvercharged", m_enemyEffectOvercharged);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, GetCurrentModdedDamage()));
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetCurrentModdedDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				dictionary[AbilityTooltipSymbol.Damage] = GetCurrentModdedDamage();
			}
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BlasterKnockbackCone))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_BlasterKnockbackCone);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public override MovementAdjustment GetMovementAdjustment()
	{
		if (base.ActorData.GetActorStatus().IsKnockbackImmune())
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
					return MovementAdjustment.ReducedMovement;
				}
			}
		}
		AbilityData abilityData = base.ActorData.GetAbilityData();
		List<AbilityData.AbilityEntry> queuedOrAimingAbilities = abilityData.GetQueuedOrAimingAbilities();
		using (List<AbilityData.AbilityEntry>.Enumerator enumerator = queuedOrAimingAbilities.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityData.AbilityEntry current = enumerator.Current;
				Card_Standard_Ability card_Standard_Ability = current.ability as Card_Standard_Ability;
				if (card_Standard_Ability != null)
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
					if (card_Standard_Ability.m_applyEffect)
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
						StatusType[] statusChanges = card_Standard_Ability.m_effect.m_statusChanges;
						int num = 0;
						while (num < statusChanges.Length)
						{
							StatusType statusType = statusChanges[num];
							if (statusType != StatusType.KnockbackImmune)
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
								if (statusType != StatusType.Unstoppable)
								{
									num++;
									continue;
								}
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							return MovementAdjustment.ReducedMovement;
						}
					}
				}
			}
			while (true)
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
		if (m_syncComp == null)
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
			m_syncComp = GetComponent<Blaster_SyncComponent>();
		}
		return m_syncComp.m_overchargeBuffs > 0;
	}

	private int GetMultiStackOverchargeDamage()
	{
		if (m_syncComp != null)
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
			if (m_syncComp.m_overchargeBuffs > 1 && m_overchargeAbility != null)
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
				if (m_overchargeAbility.GetExtraDamageForMultiCast() > 0)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return m_overchargeAbility.GetExtraDamageForMultiCast();
						}
					}
				}
			}
		}
		return 0;
	}
}
