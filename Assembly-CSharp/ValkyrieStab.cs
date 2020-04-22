using System.Collections.Generic;
using UnityEngine;

public class ValkyrieStab : Ability
{
	[Header("-- Targeting")]
	public float m_coneWidthMinAngle = 10f;

	public float m_coneWidthMaxAngle = 70f;

	public float m_coneBackwardOffset;

	public float m_coneMinLength = 2.5f;

	public float m_coneMaxLength = 5f;

	public AreaEffectUtils.StretchConeStyle m_coneStretchStyle;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets = 5;

	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 20;

	public int m_lessDamagePerTarget = 3;

	public StandardEffectInfo m_targetHitEffect;

	[Header("-- Sequences")]
	public GameObject m_centerProjectileSequencePrefab;

	public GameObject m_sideProjectileSequencePrefab;

	private Valkyrie_SyncComponent m_syncComp;

	private AbilityMod_ValkyrieStab m_abilityMod;

	private StandardEffectInfo m_cachedTargetHitEffect;

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
			m_abilityName = "Spear Poke";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_syncComp = GetComponent<Valkyrie_SyncComponent>();
		SetCachedFields();
		AbilityUtil_Targeter_ReverseStretchCone abilityUtil_Targeter_ReverseStretchCone = (AbilityUtil_Targeter_ReverseStretchCone)(base.Targeter = new AbilityUtil_Targeter_ReverseStretchCone(this, GetConeMinLength(), GetConeMaxLength(), GetConeWidthMinAngle(), GetConeWidthMaxAngle(), m_coneStretchStyle, GetConeBackwardOffset(), PenetrateLineOfSight()));
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeMaxLength() + GetConeBackwardOffset();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedTargetHitEffect;
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
			cachedTargetHitEffect = m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect);
		}
		else
		{
			cachedTargetHitEffect = m_targetHitEffect;
		}
		m_cachedTargetHitEffect = cachedTargetHitEffect;
	}

	public float GetConeWidthMinAngle()
	{
		return (!m_abilityMod) ? m_coneWidthMinAngle : m_abilityMod.m_coneWidthMinAngleMod.GetModifiedValue(m_coneWidthMinAngle);
	}

	public float GetConeWidthMaxAngle()
	{
		float result;
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
			result = m_abilityMod.m_coneWidthMaxAngleMod.GetModifiedValue(m_coneWidthMaxAngle);
		}
		else
		{
			result = m_coneWidthMaxAngle;
		}
		return result;
	}

	public float GetConeBackwardOffset()
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
			result = m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
		}
		else
		{
			result = m_coneBackwardOffset;
		}
		return result;
	}

	public float GetConeMinLength()
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
			result = m_abilityMod.m_coneMinLengthMod.GetModifiedValue(m_coneMinLength);
		}
		else
		{
			result = m_coneMinLength;
		}
		return result;
	}

	public float GetConeMaxLength()
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
			result = m_abilityMod.m_coneMaxLengthMod.GetModifiedValue(m_coneMaxLength);
		}
		else
		{
			result = m_coneMaxLength;
		}
		return result;
	}

	public bool PenetrateLineOfSight()
	{
		return (!m_abilityMod) ? m_penetrateLineOfSight : m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
	}

	public int GetMaxTargets()
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
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			result = m_maxTargets;
		}
		return result;
	}

	public int GetDamageAmount()
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
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public int GetLessDamagePerTarget()
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
			result = m_abilityMod.m_lessDamagePerTargetMod.GetModifiedValue(m_lessDamagePerTarget);
		}
		else
		{
			result = m_lessDamagePerTarget;
		}
		return result;
	}

	public int GetExtraDamageOnSpearTip()
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
			result = m_abilityMod.m_extraDamageOnSpearTip.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraDamageFirstTarget()
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
			result = m_abilityMod.m_extraDamageFirstTarget.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		return (m_cachedTargetHitEffect == null) ? m_targetHitEffect : m_cachedTargetHitEffect;
	}

	public int GetExtraAbsorbNextShieldBlockPerHit()
	{
		return m_abilityMod ? m_abilityMod.m_perHitExtraAbsorbNextShieldBlock.GetModifiedValue(0) : 0;
	}

	public int GetMaxExtraAbsorbNextShieldBlock()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_maxExtraAbsorbNextShieldBlock.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ValkyrieStab))
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
			m_abilityMod = (abilityMod as AbilityMod_ValkyrieStab);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetDamageAmount()));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeter.GetActorsInRange();
		List<ActorData> list = new List<ActorData>();
		int num = 0;
		using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityUtil_Targeter.ActorTarget current = enumerator.Current;
				list.Add(current.m_actor);
				if (current.m_actor == targetActor)
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
					if (current.m_subjectTypes.Contains(AbilityTooltipSubject.Far))
					{
						num = GetExtraDamageOnSpearTip();
					}
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		int damageAmount = GetDamageAmount();
		bool flag = true;
		damageAmount += GetExtraDamageFirstTarget();
		foreach (ActorData item in list)
		{
			if (item == targetActor)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						dictionary[AbilityTooltipSymbol.Damage] = damageAmount + num;
						return dictionary;
					}
				}
			}
			if (!(m_syncComp == null))
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
				if (m_syncComp.m_skipDamageReductionForNextStab)
				{
					goto IL_0139;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			damageAmount = Mathf.Max(0, damageAmount - GetLessDamagePerTarget());
			goto IL_0139;
			IL_0139:
			if (flag)
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
				flag = false;
				damageAmount -= GetExtraDamageFirstTarget();
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage", "damage in the cone", GetDamageAmount());
		AddTokenInt(tokens, "LessDamagePerTarget", string.Empty, m_lessDamagePerTarget);
		AddTokenInt(tokens, "Cone_MinAngle", "smallest angle of the damage cone", (int)GetConeWidthMinAngle());
		AddTokenInt(tokens, "Cone_MaxAngle", "largest angle of the damage cone", (int)GetConeWidthMaxAngle());
		AddTokenInt(tokens, "Cone_MinLength", "shortest range of the damage cone", Mathf.RoundToInt(GetConeMinLength()));
		AddTokenInt(tokens, "Cone_MaxLength", "longest range of the damage cone", Mathf.RoundToInt(GetConeMaxLength()));
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffect, "TargetHitEffect", m_targetHitEffect);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = GetConeMinLength() * Board.Get().squareSize;
		max = GetConeMaxLength() * Board.Get().squareSize;
		return true;
	}
}
