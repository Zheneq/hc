using System.Collections.Generic;
using UnityEngine;

public class NekoBoomerangDisc : Ability
{
	[Separator("Targeting")]
	public float m_laserLength = 6.5f;
	public float m_laserWidth = 1f;
	public float m_aoeRadiusAtEnd = 1f;
	public int m_maxTargets;
	[Header("-- Disc return end radius")]
	public float m_discReturnEndRadius;
	[Separator("Damage stuff")]
	public int m_directDamage = 25;
	public int m_returnTripDamage = 10;
	public bool m_returnTripIgnoreCover = true;
	[Header("-- Extra Damage")]
	public int m_extraDamageIfHitByReturnDisc;
	public int m_extraReturnDamageIfHitNoOne;
	[Separator("Shielding for target hit on throw (applied on start of next turn)")]
	public int m_shieldPerTargetHitOnThrow;
	public StandardActorEffectData m_shieldEffectData;
	[Header("Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_returnTripSequencePrefab;
	public GameObject m_persistentDiscSequencePrefab;

	private AbilityMod_NekoBoomerangDisc m_abilityMod;
	private Neko_SyncComponent m_syncComp;
	private StandardActorEffectData m_cachedShieldEffectData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Boomerang Disc";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Neko_SyncComponent>();
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_NekoDisc(
			this,
			GetLaserWidth(),
			GetLaserLength(),
			GetAoeRadiusAtEnd(),
			false,
			GetMaxTargets(),
			false,
			true)
		{
			m_affectCasterDelegate = TargeterIncludeCaster
		};
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar)
	{
		return GetShieldPerTargetHitOnThrow() > 0 && actorsSoFar.Count > 0;
	}

	private void SetCachedFields()
	{
		m_cachedShieldEffectData = m_abilityMod != null
			? m_abilityMod.m_shieldEffectDataMod.GetModifiedValue(m_shieldEffectData)
			: m_shieldEffectData;
	}

	public float GetLaserLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserLengthMod.GetModifiedValue(m_laserLength)
			: m_laserLength;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public float GetAoeRadiusAtEnd()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeRadiusAtEndMod.GetModifiedValue(m_aoeRadiusAtEnd)
			: m_aoeRadiusAtEnd;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public float GetDiscReturnEndRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(m_discReturnEndRadius)
			: m_discReturnEndRadius;
	}

	public int GetDirectDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_directDamageMod.GetModifiedValue(m_directDamage)
			: m_directDamage;
	}

	public int GetReturnTripDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_returnTripDamageMod.GetModifiedValue(m_returnTripDamage)
			: m_returnTripDamage;
	}

	public bool ReturnTripIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(m_returnTripIgnoreCover)
			: m_returnTripIgnoreCover;
	}

	public int GetExtraDamageIfHitByReturnDisc()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageIfHitByReturnDiscMod.GetModifiedValue(m_extraDamageIfHitByReturnDisc)
			: m_extraDamageIfHitByReturnDisc;
	}

	public int GetExtraReturnDamageIfHitNoOne()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraReturnDamageIfHitNoOneMod.GetModifiedValue(m_extraReturnDamageIfHitNoOne)
			: m_extraReturnDamageIfHitNoOne;
	}

	public int GetShieldPerTargetHitOnThrow()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldPerTargetHitOnThrowMod.GetModifiedValue(m_shieldPerTargetHitOnThrow)
			: m_shieldPerTargetHitOnThrow;
	}

	public StandardActorEffectData GetShieldEffectData()
	{
		return m_cachedShieldEffectData ?? m_shieldEffectData;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "DirectDamage", string.Empty, m_directDamage);
		AddTokenInt(tokens, "ReturnTripDamage", string.Empty, m_returnTripDamage);
		AddTokenInt(tokens, "ExtraDamageIfHitByReturnDisc", string.Empty, m_extraDamageIfHitByReturnDisc);
		AddTokenInt(tokens, "ExtraReturnDamageIfHitNoOne", string.Empty, m_extraReturnDamageIfHitNoOne);
		AddTokenInt(tokens, "ShieldPerTargetHitOnThrow", string.Empty, m_shieldPerTargetHitOnThrow);
		m_shieldEffectData.AddTooltipTokens(tokens, "ShieldEffectData");
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_directDamage);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			int damage = GetDirectDamage();
			if (m_syncComp != null
			    && GetExtraDamageIfHitByReturnDisc() > 0
			    && m_syncComp.IsActorTargetedByReturningDiscs(targetActor))
			{
				damage += GetExtraDamageIfHitByReturnDisc();
			}
			results.m_damage = damage;
		}
		else if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
		{
			int visibleActorsCountByTooltipSubject = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			results.m_absorb = GetShieldPerTargetHitOnThrow() * visibleActorsCountByTooltipSubject;
		}
		return true;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserLength() + GetAoeRadiusAtEnd();
	}

	public static BoardSquare GetDiscEndSquare(Vector3 startPos, Vector3 endPos)
	{
		Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(startPos, endPos);
		return KnockbackUtils.GetLastValidBoardSquareInLine(startPos, coneLosCheckPos, true, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NekoBoomerangDisc))
		{
			m_abilityMod = abilityMod as AbilityMod_NekoBoomerangDisc;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public override int GetTheatricsSortPriority(AbilityData.ActionType actionType)
	{
		return 3;
	}
}
