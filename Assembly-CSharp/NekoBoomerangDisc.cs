using System;
using System.Collections.Generic;
using UnityEngine;

public class NekoBoomerangDisc : Ability
{
	[Separator("Targeting", true)]
	public float m_laserLength = 6.5f;

	public float m_laserWidth = 1f;

	public float m_aoeRadiusAtEnd = 1f;

	public int m_maxTargets;

	[Header("-- Disc return end radius")]
	public float m_discReturnEndRadius;

	[Separator("Damage stuff", true)]
	public int m_directDamage = 0x19;

	public int m_returnTripDamage = 0xA;

	public bool m_returnTripIgnoreCover = true;

	[Header("-- Extra Damage")]
	public int m_extraDamageIfHitByReturnDisc;

	public int m_extraReturnDamageIfHitNoOne;

	[Separator("Shielding for target hit on throw (applied on start of next turn)", true)]
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Boomerang Disc";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_syncComp = base.GetComponent<Neko_SyncComponent>();
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_NekoDisc(this, this.GetLaserWidth(), this.GetLaserLength(), this.GetAoeRadiusAtEnd(), false, this.GetMaxTargets(), false, true)
		{
			m_affectCasterDelegate = new AbilityUtil_Targeter_Laser.IsAffectingCasterDelegate(this.TargeterIncludeCaster)
		};
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar)
	{
		bool result;
		if (this.GetShieldPerTargetHitOnThrow() > 0)
		{
			result = (actorsSoFar.Count > 0);
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedShieldEffectData;
		if (this.m_abilityMod)
		{
			cachedShieldEffectData = this.m_abilityMod.m_shieldEffectDataMod.GetModifiedValue(this.m_shieldEffectData);
		}
		else
		{
			cachedShieldEffectData = this.m_shieldEffectData;
		}
		this.m_cachedShieldEffectData = cachedShieldEffectData;
	}

	public float GetLaserLength()
	{
		return (!this.m_abilityMod) ? this.m_laserLength : this.m_abilityMod.m_laserLengthMod.GetModifiedValue(this.m_laserLength);
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public float GetAoeRadiusAtEnd()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_aoeRadiusAtEndMod.GetModifiedValue(this.m_aoeRadiusAtEnd);
		}
		else
		{
			result = this.m_aoeRadiusAtEnd;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public float GetDiscReturnEndRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(this.m_discReturnEndRadius);
		}
		else
		{
			result = this.m_discReturnEndRadius;
		}
		return result;
	}

	public int GetDirectDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_directDamageMod.GetModifiedValue(this.m_directDamage);
		}
		else
		{
			result = this.m_directDamage;
		}
		return result;
	}

	public int GetReturnTripDamage()
	{
		return (!this.m_abilityMod) ? this.m_returnTripDamage : this.m_abilityMod.m_returnTripDamageMod.GetModifiedValue(this.m_returnTripDamage);
	}

	public bool ReturnTripIgnoreCover()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(this.m_returnTripIgnoreCover);
		}
		else
		{
			result = this.m_returnTripIgnoreCover;
		}
		return result;
	}

	public int GetExtraDamageIfHitByReturnDisc()
	{
		return (!this.m_abilityMod) ? this.m_extraDamageIfHitByReturnDisc : this.m_abilityMod.m_extraDamageIfHitByReturnDiscMod.GetModifiedValue(this.m_extraDamageIfHitByReturnDisc);
	}

	public int GetExtraReturnDamageIfHitNoOne()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraReturnDamageIfHitNoOneMod.GetModifiedValue(this.m_extraReturnDamageIfHitNoOne);
		}
		else
		{
			result = this.m_extraReturnDamageIfHitNoOne;
		}
		return result;
	}

	public int GetShieldPerTargetHitOnThrow()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_shieldPerTargetHitOnThrowMod.GetModifiedValue(this.m_shieldPerTargetHitOnThrow);
		}
		else
		{
			result = this.m_shieldPerTargetHitOnThrow;
		}
		return result;
	}

	public StandardActorEffectData GetShieldEffectData()
	{
		StandardActorEffectData result;
		if (this.m_cachedShieldEffectData != null)
		{
			result = this.m_cachedShieldEffectData;
		}
		else
		{
			result = this.m_shieldEffectData;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "DirectDamage", string.Empty, this.m_directDamage, false);
		base.AddTokenInt(tokens, "ReturnTripDamage", string.Empty, this.m_returnTripDamage, false);
		base.AddTokenInt(tokens, "ExtraDamageIfHitByReturnDisc", string.Empty, this.m_extraDamageIfHitByReturnDisc, false);
		base.AddTokenInt(tokens, "ExtraReturnDamageIfHitNoOne", string.Empty, this.m_extraReturnDamageIfHitNoOne, false);
		base.AddTokenInt(tokens, "ShieldPerTargetHitOnThrow", string.Empty, this.m_shieldPerTargetHitOnThrow, false);
		this.m_shieldEffectData.AddTooltipTokens(tokens, "ShieldEffectData", false, null);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_directDamage);
		AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Self, 1);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			int num = this.GetDirectDamage();
			if (this.m_syncComp != null)
			{
				if (this.GetExtraDamageIfHitByReturnDisc() > 0)
				{
					if (this.m_syncComp.IsActorTargetedByReturningDiscs(targetActor))
					{
						num += this.GetExtraDamageIfHitByReturnDisc();
					}
				}
			}
			results.m_damage = num;
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
		{
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			int absorb = this.GetShieldPerTargetHitOnThrow() * visibleActorsCountByTooltipSubject;
			results.m_absorb = absorb;
		}
		return true;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserLength() + this.GetAoeRadiusAtEnd();
	}

	public static BoardSquare GetDiscEndSquare(Vector3 startPos, Vector3 endPos)
	{
		Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(startPos, endPos);
		return KnockbackUtils.GetLastValidBoardSquareInLine(startPos, coneLosCheckPos, true, true, float.MaxValue);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NekoBoomerangDisc))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_NekoBoomerangDisc);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public override int GetTheatricsSortPriority(AbilityData.ActionType actionType)
	{
		return 3;
	}
}
