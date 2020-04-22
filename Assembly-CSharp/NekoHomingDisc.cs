using System.Collections.Generic;
using UnityEngine;

public class NekoHomingDisc : Ability
{
	[Separator("Targeting", true)]
	public float m_laserLength = 6.5f;

	public float m_laserWidth = 1f;

	public int m_maxTargets = 1;

	[Header("-- Disc return end radius")]
	public float m_discReturnEndRadius;

	[Separator("On Cast Hit", true)]
	public StandardEffectInfo m_onCastEnemyHitEffect;

	[Separator("On Enemy Hit", true)]
	public int m_targetDamage = 25;

	public int m_returnTripDamage = 10;

	public bool m_returnTripIgnoreCover = true;

	public float m_extraReturnDamagePerDist;

	public StandardEffectInfo m_returnTripEnemyEffect;

	[Separator("Cooldown Reduction", true)]
	public int m_cdrIfHitNoOneOnCast;

	public int m_cdrIfHitNoOneOnReturn;

	[Header("Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_returnTripSequencePrefab;

	public GameObject m_persistentDiscSequencePrefab;

	private AbilityMod_NekoHomingDisc m_abilityMod;

	private Neko_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedOnCastEnemyHitEffect;

	private StandardEffectInfo m_cachedReturnTripEnemyEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Homing Disc";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_syncComp = GetComponent<Neko_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserLength(), false, GetMaxTargets());
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedOnCastEnemyHitEffect;
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
			cachedOnCastEnemyHitEffect = m_abilityMod.m_onCastEnemyHitEffectMod.GetModifiedValue(m_onCastEnemyHitEffect);
		}
		else
		{
			cachedOnCastEnemyHitEffect = m_onCastEnemyHitEffect;
		}
		m_cachedOnCastEnemyHitEffect = cachedOnCastEnemyHitEffect;
		StandardEffectInfo cachedReturnTripEnemyEffect;
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
			cachedReturnTripEnemyEffect = m_abilityMod.m_returnTripEnemyEffectMod.GetModifiedValue(m_returnTripEnemyEffect);
		}
		else
		{
			cachedReturnTripEnemyEffect = m_returnTripEnemyEffect;
		}
		m_cachedReturnTripEnemyEffect = cachedReturnTripEnemyEffect;
	}

	public float GetLaserLength()
	{
		float result;
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
			result = m_abilityMod.m_laserLengthMod.GetModifiedValue(m_laserLength);
		}
		else
		{
			result = m_laserLength;
		}
		return result;
	}

	public float GetLaserWidth()
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
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
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

	public float GetDiscReturnEndRadius()
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
			result = m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(m_discReturnEndRadius);
		}
		else
		{
			result = m_discReturnEndRadius;
		}
		return result;
	}

	public StandardEffectInfo GetOnCastEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedOnCastEnemyHitEffect != null)
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
			result = m_cachedOnCastEnemyHitEffect;
		}
		else
		{
			result = m_onCastEnemyHitEffect;
		}
		return result;
	}

	public int GetTargetDamage()
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
			result = m_abilityMod.m_targetDamageMod.GetModifiedValue(m_targetDamage);
		}
		else
		{
			result = m_targetDamage;
		}
		return result;
	}

	public int GetReturnTripDamage()
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
			result = m_abilityMod.m_returnTripDamageMod.GetModifiedValue(m_returnTripDamage);
		}
		else
		{
			result = m_returnTripDamage;
		}
		return result;
	}

	public bool ReturnTripIgnoreCover()
	{
		return (!m_abilityMod) ? m_returnTripIgnoreCover : m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(m_returnTripIgnoreCover);
	}

	public float GetExtraReturnDamagePerDist()
	{
		return (!m_abilityMod) ? m_extraReturnDamagePerDist : m_abilityMod.m_extraReturnDamagePerDistMod.GetModifiedValue(m_extraReturnDamagePerDist);
	}

	public StandardEffectInfo GetReturnTripEnemyEffect()
	{
		StandardEffectInfo result;
		if (m_cachedReturnTripEnemyEffect != null)
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
			result = m_cachedReturnTripEnemyEffect;
		}
		else
		{
			result = m_returnTripEnemyEffect;
		}
		return result;
	}

	public int GetCdrIfHitNoOneOnCast()
	{
		return (!m_abilityMod) ? m_cdrIfHitNoOneOnCast : m_abilityMod.m_cdrIfHitNoOneOnCastMod.GetModifiedValue(m_cdrIfHitNoOneOnCast);
	}

	public int GetCdrIfHitNoOneOnReturn()
	{
		return (!m_abilityMod) ? m_cdrIfHitNoOneOnReturn : m_abilityMod.m_cdrIfHitNoOneOnReturnMod.GetModifiedValue(m_cdrIfHitNoOneOnReturn);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AbilityMod.AddToken_EffectInfo(tokens, m_onCastEnemyHitEffect, "OnCastEnemyHitEffect", m_onCastEnemyHitEffect);
		AddTokenInt(tokens, "TargetDamage", string.Empty, m_targetDamage);
		AddTokenInt(tokens, "ReturnTripDamage", string.Empty, m_returnTripDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_returnTripEnemyEffect, "ReturnTripEnemyEffect", m_returnTripEnemyEffect);
		AddTokenInt(tokens, "CdrIfHitNoOneOnCast", string.Empty, m_cdrIfHitNoOneOnCast);
		AddTokenInt(tokens, "CdrIfHitNoOneOnReturn", string.Empty, m_cdrIfHitNoOneOnReturn);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetTargetDamage()));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, GetReturnTripDamage()));
		return list;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserLength();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_NekoHomingDisc))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_NekoHomingDisc);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
