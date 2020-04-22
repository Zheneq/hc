using System.Collections.Generic;
using UnityEngine;

public class ExoTetherTrap : Ability
{
	[Header("-- Targeting and Direct Damage")]
	[Space(20f)]
	public int m_laserDamageAmount = 5;

	public LaserTargetingInfo m_laserInfo;

	public StandardActorEffectData m_baseEffectData;

	public StandardEffectInfo m_laserOnHitEffect;

	[Header("-- Tether Info")]
	public float m_tetherDistance = 5f;

	public int m_tetherBreakDamage = 20;

	public StandardEffectInfo m_tetherBreakEffect;

	public bool m_breakTetherOnNonGroundBasedMovement;

	[Header("-- Extra Damage based on distance")]
	public float m_extraDamagePerMoveDist;

	public int m_maxExtraDamageFromMoveDist;

	[Header("-- Cooldown Reduction if tether didn't break")]
	public int m_cdrOnTetherEndIfNotTriggered;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	public GameObject m_beamSequence;

	public GameObject m_tetherBreakHitSequence;

	private AbilityMod_ExoTetherTrap m_abilityMod;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedTetherBreakEffect;

	private StandardActorEffectData m_cachedBaseEffectData;

	private StandardEffectInfo m_cachedLaserOnHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Exo Tether Trap";
		}
		SetupTargeter();
	}

	public void SetupTargeter()
	{
		SetCachedFields();
		AbilityUtil_Targeter_ExoTether abilityUtil_Targeter_ExoTether = new AbilityUtil_Targeter_ExoTether(this, GetLaserInfo(), GetLaserInfo());
		abilityUtil_Targeter_ExoTether.SetAffectedGroups(true, false, false);
		base.Targeter = abilityUtil_Targeter_ExoTether;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_laserInfo;
		LaserTargetingInfo cachedLaserInfo;
		if ((bool)m_abilityMod)
		{
			cachedLaserInfo = m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo);
		}
		else
		{
			cachedLaserInfo = m_laserInfo;
		}
		m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedTetherBreakEffect;
		if ((bool)m_abilityMod)
		{
			cachedTetherBreakEffect = m_abilityMod.m_tetherBreakEffectMod.GetModifiedValue(m_tetherBreakEffect);
		}
		else
		{
			cachedTetherBreakEffect = m_tetherBreakEffect;
		}
		m_cachedTetherBreakEffect = cachedTetherBreakEffect;
		StandardActorEffectData cachedBaseEffectData;
		if ((bool)m_abilityMod)
		{
			cachedBaseEffectData = m_abilityMod.m_baseEffectDataMod.GetModifiedValue(m_baseEffectData);
		}
		else
		{
			cachedBaseEffectData = m_baseEffectData.GetShallowCopy();
		}
		m_cachedBaseEffectData = cachedBaseEffectData;
		if (m_beamSequence != null)
		{
			m_cachedBaseEffectData.m_sequencePrefabs = new GameObject[1]
			{
				m_beamSequence
			};
		}
		StandardEffectInfo cachedLaserOnHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedLaserOnHitEffect = m_abilityMod.m_laserOnHitEffectMod.GetModifiedValue(m_laserOnHitEffect);
		}
		else
		{
			cachedLaserOnHitEffect = m_laserOnHitEffect;
		}
		m_cachedLaserOnHitEffect = cachedLaserOnHitEffect;
	}

	public int GetLaserDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			result = m_laserDamageAmount;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserInfo != null)
		{
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	public StandardActorEffectData GetBaseEffectData()
	{
		return (m_cachedBaseEffectData == null) ? m_baseEffectData : m_cachedBaseEffectData;
	}

	public StandardEffectInfo GetLaserOnHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedLaserOnHitEffect != null)
		{
			result = m_cachedLaserOnHitEffect;
		}
		else
		{
			result = m_laserOnHitEffect;
		}
		return result;
	}

	public float GetTetherDistance()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_tetherDistanceMod.GetModifiedValue(m_tetherDistance);
		}
		else
		{
			result = m_tetherDistance;
		}
		return result;
	}

	public int GetTetherBreakDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_tetherBreakDamageMod.GetModifiedValue(m_tetherBreakDamage);
		}
		else
		{
			result = m_tetherBreakDamage;
		}
		return result;
	}

	public StandardEffectInfo GetTetherBreakEffect()
	{
		return (m_cachedTetherBreakEffect == null) ? m_tetherBreakEffect : m_cachedTetherBreakEffect;
	}

	public bool BreakTetherOnNonGroundBasedMovement()
	{
		return (!m_abilityMod) ? m_breakTetherOnNonGroundBasedMovement : m_abilityMod.m_breakTetherOnNonGroundBasedMovementMod.GetModifiedValue(m_breakTetherOnNonGroundBasedMovement);
	}

	public float GetExtraDamagePerMoveDist()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamagePerMoveDistMod.GetModifiedValue(m_extraDamagePerMoveDist);
		}
		else
		{
			result = m_extraDamagePerMoveDist;
		}
		return result;
	}

	public int GetMaxExtraDamageFromMoveDist()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxExtraDamageFromMoveDistMod.GetModifiedValue(m_maxExtraDamageFromMoveDist);
		}
		else
		{
			result = m_maxExtraDamageFromMoveDist;
		}
		return result;
	}

	public int GetCdrOnTetherEndIfNotTriggered()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrOnTetherEndIfNotTriggeredMod.GetModifiedValue(m_cdrOnTetherEndIfNotTriggered);
		}
		else
		{
			result = m_cdrOnTetherEndIfNotTriggered;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoTetherTrap abilityMod_ExoTetherTrap = modAsBase as AbilityMod_ExoTetherTrap;
		StandardActorEffectData standardActorEffectData = (!abilityMod_ExoTetherTrap) ? m_baseEffectData : abilityMod_ExoTetherTrap.m_baseEffectDataMod.GetModifiedValue(m_baseEffectData);
		standardActorEffectData.AddTooltipTokens(tokens, "TetherBaseEffectData", abilityMod_ExoTetherTrap != null, m_baseEffectData);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_ExoTetherTrap)
		{
			effectInfo = abilityMod_ExoTetherTrap.m_laserOnHitEffectMod.GetModifiedValue(m_laserOnHitEffect);
		}
		else
		{
			effectInfo = m_laserOnHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "LaserOnHitEffect", m_laserOnHitEffect);
		AddTokenInt(tokens, "Damage_FirstTurn", string.Empty, (!abilityMod_ExoTetherTrap) ? m_laserDamageAmount : abilityMod_ExoTetherTrap.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount));
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ExoTetherTrap)
		{
			val = abilityMod_ExoTetherTrap.m_tetherBreakDamageMod.GetModifiedValue(m_tetherBreakDamage);
		}
		else
		{
			val = m_tetherBreakDamage;
		}
		AddTokenInt(tokens, "Damage_TetherBreak", empty, val);
		float num;
		if ((bool)abilityMod_ExoTetherTrap)
		{
			num = abilityMod_ExoTetherTrap.m_tetherDistanceMod.GetModifiedValue(m_tetherDistance);
		}
		else
		{
			num = m_tetherDistance;
		}
		AddTokenInt(tokens, "TetherDistance", "distance from starting position", (int)num);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_ExoTetherTrap)
		{
			effectInfo2 = abilityMod_ExoTetherTrap.m_tetherBreakEffectMod.GetModifiedValue(m_tetherBreakEffect);
		}
		else
		{
			effectInfo2 = m_tetherBreakEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "TetherBreakEffect", m_tetherBreakEffect);
		AddTokenInt(tokens, "CdrOnTetherEndIfNotTriggered", string.Empty, (!abilityMod_ExoTetherTrap) ? m_cdrOnTetherEndIfNotTriggered : abilityMod_ExoTetherTrap.m_cdrOnTetherEndIfNotTriggeredMod.GetModifiedValue(m_cdrOnTetherEndIfNotTriggered));
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetLaserDamageAmount());
		GetBaseEffectData().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, GetLaserDamageAmount());
		return symbolToValue;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ExoTetherTrap))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ExoTetherTrap);
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
