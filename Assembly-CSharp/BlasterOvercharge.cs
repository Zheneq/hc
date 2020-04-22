using System.Collections.Generic;
using UnityEngine;

public class BlasterOvercharge : Ability
{
	[Header("-- For managing Overcharge effect, please use on hit effect for gameplay")]
	public StandardActorEffectData m_overchargeEffectData;

	[Header("-- How many stacks are allowed")]
	public int m_maxCastCount = 1;

	[Header("-- Extra damage added for all attacks except Lurker Mine")]
	public int m_extraDamage = 10;

	[Header("-- Extra Damage for Lurker Mine")]
	public int m_extraDamageForDelayedLaser;

	[Header("-- Extra Damage for multiple stacks")]
	public int m_extraDamageForMultiCast;

	[Header("-- Count - Number of times extra damage applies")]
	public int m_extraDamageCount = 1;

	[Header("-- On Cast")]
	public StandardEffectInfo m_effectOnSelfOnCast;

	[Header("-- Extra Effects for other abilities")]
	public StandardEffectInfo m_extraEffectOnOtherAbilities;

	public List<AbilityData.ActionType> m_extraEffectActionTypes;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_BlasterOvercharge m_abilityMod;

	private Blaster_SyncComponent m_syncComp;

	private BlasterKnockbackCone m_ultAbility;

	private StandardEffectInfo m_cachedEffectOnSelfOnCast;

	private StandardEffectInfo m_cachedExtraEffectOnOtherAbilities;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_syncComp == null)
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
			m_syncComp = GetComponent<Blaster_SyncComponent>();
		}
		if (m_ultAbility == null)
		{
			m_ultAbility = (GetComponent<AbilityData>().GetAbilityOfType(typeof(BlasterKnockbackCone)) as BlasterKnockbackCone);
		}
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always);
		base.Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnSelfOnCast;
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
			cachedEffectOnSelfOnCast = m_abilityMod.m_effectOnSelfOnCastMod.GetModifiedValue(m_effectOnSelfOnCast);
		}
		else
		{
			cachedEffectOnSelfOnCast = m_effectOnSelfOnCast;
		}
		m_cachedEffectOnSelfOnCast = cachedEffectOnSelfOnCast;
		StandardEffectInfo cachedExtraEffectOnOtherAbilities;
		if (m_abilityMod != null)
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
			cachedExtraEffectOnOtherAbilities = m_abilityMod.m_extraEffectOnOtherAbilitiesMod.GetModifiedValue(m_extraEffectOnOtherAbilities);
		}
		else
		{
			cachedExtraEffectOnOtherAbilities = m_extraEffectOnOtherAbilities;
		}
		m_cachedExtraEffectOnOtherAbilities = cachedExtraEffectOnOtherAbilities;
	}

	public int GetMaxCastCount()
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
			result = m_abilityMod.m_maxCastCountMod.GetModifiedValue(m_maxCastCount);
		}
		else
		{
			result = m_maxCastCount;
		}
		return result;
	}

	public int GetExtraDamage()
	{
		int result;
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
			result = m_abilityMod.m_extraDamageMod.GetModifiedValue(m_extraDamage);
		}
		else
		{
			result = m_extraDamage;
		}
		return result;
	}

	public int GetExtraDamageForDelayedLaser()
	{
		int result;
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
			result = m_abilityMod.m_extraDamageForDelayedLaserMod.GetModifiedValue(m_extraDamageForDelayedLaser);
		}
		else
		{
			result = m_extraDamageForDelayedLaser;
		}
		return result;
	}

	public int GetExtraDamageForMultiCast()
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
			result = m_abilityMod.m_extraDamageForMultiCastMod.GetModifiedValue(m_extraDamageForMultiCast);
		}
		else
		{
			result = m_extraDamageForMultiCast;
		}
		return result;
	}

	public int GetExtraDamageCount()
	{
		int result;
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
			result = m_abilityMod.m_extraDamageCountMod.GetModifiedValue(m_extraDamageCount);
		}
		else
		{
			result = m_extraDamageCount;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelfOnCast()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnSelfOnCast != null)
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
			result = m_cachedEffectOnSelfOnCast;
		}
		else
		{
			result = m_effectOnSelfOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEffectOnOtherAbilities()
	{
		StandardEffectInfo result;
		if (m_cachedExtraEffectOnOtherAbilities != null)
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
			result = m_cachedExtraEffectOnOtherAbilities;
		}
		else
		{
			result = m_extraEffectOnOtherAbilities;
		}
		return result;
	}

	public List<AbilityData.ActionType> GetExtraEffectTargetActionTypes()
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
			if (m_abilityMod.m_useExtraEffectActionTypeOverride)
			{
				return m_abilityMod.m_extraEffectActionTypesOverride;
			}
		}
		return m_extraEffectActionTypes;
	}

	public override bool IsFreeAction()
	{
		if (m_ultAbility != null && m_ultAbility.OverchargeAsFreeActionAfterCast())
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
			if (GameFlowData.Get() != null)
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
				if (m_syncComp.m_lastUltCastTurn > 0)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return GameFlowData.Get().CurrentTurn > m_syncComp.m_lastUltCastTurn;
						}
					}
				}
			}
		}
		return base.IsFreeAction();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetEffectOnSelfOnCast().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					int maxCastCount = GetMaxCastCount();
					if (maxCastCount > 0)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return m_syncComp.m_overchargeBuffs < maxCastCount;
							}
						}
					}
					return m_syncComp.m_overchargeBuffs <= 0;
				}
				}
			}
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterOvercharge abilityMod_BlasterOvercharge = modAsBase as AbilityMod_BlasterOvercharge;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_BlasterOvercharge)
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
			val = abilityMod_BlasterOvercharge.m_maxCastCountMod.GetModifiedValue(m_maxCastCount);
		}
		else
		{
			val = m_maxCastCount;
		}
		AddTokenInt(tokens, "MaxCastCount", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_BlasterOvercharge)
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
			val2 = abilityMod_BlasterOvercharge.m_extraDamageForDelayedLaserMod.GetModifiedValue(m_extraDamageForDelayedLaser);
		}
		else
		{
			val2 = m_extraDamageForDelayedLaser;
		}
		AddTokenInt(tokens, "ExtraDamageForLurkerMine", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_BlasterOvercharge)
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
			val3 = abilityMod_BlasterOvercharge.m_extraDamageForMultiCastMod.GetModifiedValue(m_extraDamageForMultiCast);
		}
		else
		{
			val3 = m_extraDamageForMultiCast;
		}
		AddTokenInt(tokens, "ExtraDamageForMultiCast", empty3, val3);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_BlasterOvercharge)
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
			effectInfo = abilityMod_BlasterOvercharge.m_effectOnSelfOnCastMod.GetModifiedValue(m_effectOnSelfOnCast);
		}
		else
		{
			effectInfo = m_effectOnSelfOnCast;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnSelfOnCast", m_effectOnSelfOnCast);
		AbilityMod.AddToken_EffectInfo(tokens, m_extraEffectOnOtherAbilities, "ExtraEffectOnOtherAbilities", m_extraEffectOnOtherAbilities);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_BlasterOvercharge)
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
			val4 = abilityMod_BlasterOvercharge.m_extraDamageMod.GetModifiedValue(m_extraDamage);
		}
		else
		{
			val4 = m_extraDamage;
		}
		AddTokenInt(tokens, "OverchargeExtraDamage", empty4, val4);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BlasterOvercharge))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_BlasterOvercharge);
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
