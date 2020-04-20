using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierConeOrLaser : Ability
{
	[Separator("Targeting", true)]
	public float m_coneDistThreshold = 4f;

	[Header("  Targeting: For Cone")]
	public ConeTargetingInfo m_coneInfo;

	[Header("  Targeting: For Laser")]
	public LaserTargetingInfo m_laserInfo;

	[Separator("On Hit", true)]
	public int m_coneDamage = 0xA;

	public StandardEffectInfo m_coneEnemyHitEffect;

	[Space(10f)]
	public int m_laserDamage = 0x14;

	public StandardEffectInfo m_laserEnemyHitEffect;

	[Separator("Extra Damage", true)]
	public int m_extraDamageForAlternating;

	[Space(5f)]
	public float m_closeDistThreshold = -1f;

	public int m_extraDamageForNearTarget;

	public int m_extraDamageForFromCover;

	[Space(5f)]
	public int m_extraDamageToEvaders;

	[Separator("Extra Energy (per target hit)", true)]
	public int m_extraEnergyForCone;

	public int m_extraEnergyForLaser;

	[Separator("Animation Indices", true)]
	public int m_onCastLaserAnimIndex = 1;

	public int m_onCastConeAnimIndex = 6;

	[Separator("Sequences", true)]
	public GameObject m_coneSequencePrefab;

	public GameObject m_laserSequencePrefab;

	public AbilityMod_SoldierConeOrLaser m_abilityMod;

	private Soldier_SyncComponent m_syncComp;

	private AbilityData m_abilityData;

	private SoldierStimPack m_stimAbility;

	private ConeTargetingInfo m_cachedConeInfo;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedConeEnemyHitEffect;

	private StandardEffectInfo m_cachedLaserEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Soldier Cone Or Laser";
		}
		this.Setup();
	}

	private void Setup()
	{
		if (this.m_syncComp == null)
		{
			this.m_syncComp = base.GetComponent<Soldier_SyncComponent>();
		}
		if (this.m_abilityData == null)
		{
			this.m_abilityData = base.GetComponent<AbilityData>();
		}
		if (this.m_abilityData != null && this.m_stimAbility == null)
		{
			this.m_stimAbility = (this.m_abilityData.GetAbilityOfType(typeof(SoldierStimPack)) as SoldierStimPack);
		}
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_ConeOrLaser(this, this.GetConeInfo(), this.GetLaserInfo(), this.m_coneDistThreshold);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserInfo().range;
	}

	private void SetCachedFields()
	{
		ConeTargetingInfo cachedConeInfo;
		if (this.m_abilityMod)
		{
			cachedConeInfo = this.m_abilityMod.m_coneInfoMod.GetModifiedValue(this.m_coneInfo);
		}
		else
		{
			cachedConeInfo = this.m_coneInfo;
		}
		this.m_cachedConeInfo = cachedConeInfo;
		LaserTargetingInfo cachedLaserInfo;
		if (this.m_abilityMod)
		{
			cachedLaserInfo = this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo);
		}
		else
		{
			cachedLaserInfo = this.m_laserInfo;
		}
		this.m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedConeEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedConeEnemyHitEffect = this.m_abilityMod.m_coneEnemyHitEffectMod.GetModifiedValue(this.m_coneEnemyHitEffect);
		}
		else
		{
			cachedConeEnemyHitEffect = this.m_coneEnemyHitEffect;
		}
		this.m_cachedConeEnemyHitEffect = cachedConeEnemyHitEffect;
		StandardEffectInfo cachedLaserEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedLaserEnemyHitEffect = this.m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(this.m_laserEnemyHitEffect);
		}
		else
		{
			cachedLaserEnemyHitEffect = this.m_laserEnemyHitEffect;
		}
		this.m_cachedLaserEnemyHitEffect = cachedLaserEnemyHitEffect;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		ConeTargetingInfo result;
		if (this.m_cachedConeInfo != null)
		{
			result = this.m_cachedConeInfo;
		}
		else
		{
			result = this.m_coneInfo;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
		{
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	public int GetConeDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneDamageMod.GetModifiedValue(this.m_coneDamage);
		}
		else
		{
			result = this.m_coneDamage;
		}
		return result;
	}

	public StandardEffectInfo GetConeEnemyHitEffect()
	{
		return (this.m_cachedConeEnemyHitEffect == null) ? this.m_coneEnemyHitEffect : this.m_cachedConeEnemyHitEffect;
	}

	public int GetLaserDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserDamageMod.GetModifiedValue(this.m_laserDamage);
		}
		else
		{
			result = this.m_laserDamage;
		}
		return result;
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedLaserEnemyHitEffect != null)
		{
			result = this.m_cachedLaserEnemyHitEffect;
		}
		else
		{
			result = this.m_laserEnemyHitEffect;
		}
		return result;
	}

	public int GetExtraDamageForAlternating()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageForAlternatingMod.GetModifiedValue(this.m_extraDamageForAlternating);
		}
		else
		{
			result = this.m_extraDamageForAlternating;
		}
		return result;
	}

	public float GetCloseDistThreshold()
	{
		return (!this.m_abilityMod) ? this.m_closeDistThreshold : this.m_abilityMod.m_closeDistThresholdMod.GetModifiedValue(this.m_closeDistThreshold);
	}

	public int GetExtraDamageForNearTarget()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageForNearTargetMod.GetModifiedValue(this.m_extraDamageForNearTarget);
		}
		else
		{
			result = this.m_extraDamageForNearTarget;
		}
		return result;
	}

	public int GetExtraDamageForFromCover()
	{
		return (!this.m_abilityMod) ? this.m_extraDamageForFromCover : this.m_abilityMod.m_extraDamageForFromCoverMod.GetModifiedValue(this.m_extraDamageForFromCover);
	}

	public int GetExtraDamageToEvaders()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageToEvadersMod.GetModifiedValue(this.m_extraDamageToEvaders);
		}
		else
		{
			result = this.m_extraDamageToEvaders;
		}
		return result;
	}

	public int GetExtraEnergyForCone()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraEnergyForConeMod.GetModifiedValue(this.m_extraEnergyForCone);
		}
		else
		{
			result = this.m_extraEnergyForCone;
		}
		return result;
	}

	public int GetExtraEnergyForLaser()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraEnergyForLaserMod.GetModifiedValue(this.m_extraEnergyForLaser);
		}
		else
		{
			result = this.m_extraEnergyForLaser;
		}
		return result;
	}

	public bool ShouldUseExtraDamageForNearTarget(ActorData target, ActorData caster)
	{
		if (this.GetExtraDamageForNearTarget() > 0)
		{
			Vector3 vector = target.GetTravelBoardSquareWorldPosition() - caster.GetTravelBoardSquareWorldPosition();
			vector.y = 0f;
			return vector.magnitude < this.GetCloseDistThreshold() * Board.Get().squareSize;
		}
		return false;
	}

	public bool HasConeDamageMod()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = (this.m_abilityMod.m_coneDamageMod.operation != AbilityModPropertyInt.ModOp.Ignore);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool HasLaserDamageMod()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = (this.m_abilityMod.m_laserDamageMod.operation != AbilityModPropertyInt.ModOp.Ignore);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool HasNearDistThresholdMod()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = (this.m_abilityMod.m_closeDistThresholdMod.operation != AbilityModPropertyFloat.ModOp.Ignore);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool HasExtraDamageForNearTargetMod()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = (this.m_abilityMod.m_extraDamageForNearTargetMod.operation != AbilityModPropertyInt.ModOp.Ignore);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool HasConeEnergyMod()
	{
		return this.m_abilityMod != null && this.m_abilityMod.m_extraEnergyForConeMod.operation != AbilityModPropertyInt.ModOp.Ignore;
	}

	public bool HasLaserEnergyMod()
	{
		return this.m_abilityMod != null && this.m_abilityMod.m_extraEnergyForLaserMod.operation != AbilityModPropertyInt.ModOp.Ignore;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetConeDamage());
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.GetLaserDamage());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				dictionary = new Dictionary<AbilityTooltipSymbol, int>();
				int num = 0;
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
				{
					num = this.GetConeDamage();
					if (this.GetExtraDamageForAlternating() > 0)
					{
						if (this.m_syncComp)
						{
							if ((int)this.m_syncComp.m_lastPrimaryUsedMode == 2)
							{
								num += this.GetExtraDamageForAlternating();
							}
						}
					}
				}
				else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
				{
					num = this.GetLaserDamage();
					if (this.GetExtraDamageForAlternating() > 0)
					{
						if (this.m_syncComp && (int)this.m_syncComp.m_lastPrimaryUsedMode == 1)
						{
							num += this.GetExtraDamageForAlternating();
						}
					}
				}
				ActorData actorData = base.ActorData;
				if (actorData != null)
				{
					if (this.ShouldUseExtraDamageForNearTarget(targetActor, actorData))
					{
						num += this.GetExtraDamageForNearTarget();
					}
					if (this.GetExtraDamageForFromCover() > 0)
					{
						if (actorData.GetActorCover().HasAnyCover(false))
						{
							num += this.GetExtraDamageForFromCover();
						}
					}
				}
				dictionary[AbilityTooltipSymbol.Damage] = num;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if ((this.GetExtraEnergyForCone() <= 0 && this.GetExtraEnergyForLaser() <= 0) || !(base.Targeter is AbilityUtil_Targeter_ConeOrLaser))
		{
			return 0;
		}
		AbilityUtil_Targeter_ConeOrLaser abilityUtil_Targeter_ConeOrLaser = base.Targeter as AbilityUtil_Targeter_ConeOrLaser;
		int visibleActorsCountByTooltipSubject = abilityUtil_Targeter_ConeOrLaser.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
		if (abilityUtil_Targeter_ConeOrLaser.m_updatingWithCone)
		{
			return visibleActorsCountByTooltipSubject * this.GetExtraEnergyForCone();
		}
		return visibleActorsCountByTooltipSubject * this.GetExtraEnergyForLaser();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "ConeDamage", string.Empty, this.m_coneDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_coneEnemyHitEffect, "ConeEnemyHitEffect", null, true);
		base.AddTokenInt(tokens, "LaserDamage", string.Empty, this.m_laserDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_laserEnemyHitEffect, "LaserEnemyHitEffect", null, true);
		base.AddTokenInt(tokens, "ExtraDamageForAlternating", string.Empty, this.m_extraDamageForAlternating, false);
		base.AddTokenInt(tokens, "ExtraDamageForNearTarget", string.Empty, this.m_extraDamageForNearTarget, false);
		base.AddTokenInt(tokens, "ExtraDamageForFromCover", string.Empty, this.m_extraDamageForFromCover, false);
		base.AddTokenInt(tokens, "ExtraDamageToEvaders", string.Empty, this.m_extraDamageToEvaders, false);
		base.AddTokenInt(tokens, "ExtraEnergyForCone", string.Empty, this.m_extraEnergyForCone, false);
		base.AddTokenInt(tokens, "ExtraEnergyForLaser", string.Empty, this.m_extraEnergyForLaser, false);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = this.m_coneDistThreshold - 0.1f;
		max = this.m_coneDistThreshold + 0.1f;
		return true;
	}

	public override bool ForceIgnoreCover(ActorData targetActor)
	{
		if (this.m_abilityData != null)
		{
			if (this.m_stimAbility != null)
			{
				if (this.m_stimAbility.BasicAttackIgnoreCover())
				{
					return this.m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack));
				}
			}
		}
		return false;
	}

	public override bool ForceReduceCoverEffectiveness(ActorData targetActor)
	{
		if (this.m_abilityData != null)
		{
			if (this.m_stimAbility != null)
			{
				if (this.m_stimAbility.BasicAttackReduceCoverEffectiveness())
				{
					return this.m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack));
				}
			}
		}
		return false;
	}

	private bool ShouldUseCone(Vector3 freePos, ActorData caster)
	{
		Vector3 vector = freePos - caster.GetTravelBoardSquareWorldPosition();
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return magnitude <= this.m_coneDistThreshold;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		bool result;
		if (animIndex != this.m_onCastConeAnimIndex)
		{
			result = (animIndex == this.m_onCastLaserAnimIndex);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		if (targets != null)
		{
			if (caster != null)
			{
				bool flag = this.ShouldUseCone(targets[0].FreePos, caster);
				ActorModelData.ActionAnimationType result;
				if (flag)
				{
					result = (ActorModelData.ActionAnimationType)this.m_onCastConeAnimIndex;
				}
				else
				{
					result = (ActorModelData.ActionAnimationType)this.m_onCastLaserAnimIndex;
				}
				return result;
			}
		}
		return base.GetActionAnimType();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierConeOrLaser))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SoldierConeOrLaser);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public enum LastUsedModeFlag
	{
		None,
		Cone,
		Laser
	}
}
