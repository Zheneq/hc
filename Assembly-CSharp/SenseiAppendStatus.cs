using System;
using System.Collections.Generic;
using UnityEngine;

public class SenseiAppendStatus : Ability
{
	[Separator("Targeting", true)]
	public SenseiAppendStatus.TargetingMode m_targetingMode;

	[Header("    (( Targeting: If using ActorSquare mode ))")]
	public bool m_canTargetAlly = true;

	public bool m_canTargetEnemy = true;

	public bool m_canTagetSelf;

	public bool m_targetingIgnoreLos;

	[Header("-- Whether to check barriers for enemy targeting")]
	public bool m_checkBarrierForLosIfTargetEnemy = true;

	[Header("    (( Targeting: If using Laser mode ))")]
	public LaserTargetingInfo m_laserInfo;

	[Separator("On Cast Hit Stuff", true)]
	public int m_energyToAllyTargetOnCast;

	public StandardActorEffectData m_enemyCastHitEffectData;

	public StandardActorEffectData m_allyCastHitEffectData;

	[Separator("For Append Effect", true)]
	public bool m_endEffectIfAppendedStatus = true;

	public AbilityPriority m_earliestPriorityToConsider;

	public bool m_delayEffectApply = true;

	public bool m_requireDamageToTransfer = true;

	[Header("-- Effect to append --")]
	public StandardEffectInfo m_effectAddedOnEnemyAttack;

	public StandardEffectInfo m_effectAddedOnAllyAttack;

	[Space(10f)]
	public int m_energyGainOnAllyAppendHit;

	[Header("-- Sequences --")]
	public GameObject m_castOnEnemySequencePrefab;

	public GameObject m_castOnAllySequencePrefab;

	public GameObject m_statusApplyOnAllySequencePrefab;

	public GameObject m_statusApplyOnEnemySequencePrefab;

	private AbilityMod_SenseiAppendStatus m_abilityMod;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardActorEffectData m_cachedEnemyCastHitEffectData;

	private StandardActorEffectData m_cachedAllyCastHitEffectData;

	private StandardEffectInfo m_cachedEffectAddedOnEnemyAttack;

	private StandardEffectInfo m_cachedEffectAddedOnAllyAttack;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "SenseiAppendStatus";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		if (this.m_targetingMode == SenseiAppendStatus.TargetingMode.ActorSquare)
		{
			AbilityUtil_Targeter.AffectsActor affectsActor;
			if (this.CanTagetSelf())
			{
				affectsActor = AbilityUtil_Targeter.AffectsActor.Possible;
			}
			else
			{
				affectsActor = AbilityUtil_Targeter.AffectsActor.Never;
			}
			AbilityUtil_Targeter.AffectsActor affectsCaster = affectsActor;
			base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.CanTargetAlly(), this.CanTargetEnemy(), affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_Laser(this, this.GetLaserInfo());
		}
	}

	public override string GetSetupNotesForEditor()
	{
		return string.Concat(new string[]
		{
			"<color=cyan>-- For Art --</color>\n",
			Ability.SetupNoteVarName("Cast On Enemy Sequence Prefab"),
			"\nFor initial cast, it targeting Enemy\n\n",
			Ability.SetupNoteVarName("Cast On Ally Sequence Prefab"),
			"\nFor initial casst, if targeting Ally ...\n\n",
			Ability.SetupNoteVarName("Status Apply On Ally Sequence Prefab"),
			"\nFor impact on target that actually adds buff/debuff\n\n",
			Ability.SetupNoteVarName("Status Apply On Enemy Sequence Prefab"),
			"\nFor impact on target that actually adds buff/debuff\n\n"
		});
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		this.m_cachedLaserInfo = ((!this.m_abilityMod) ? this.m_laserInfo : this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo));
		StandardActorEffectData cachedEnemyCastHitEffectData;
		if (this.m_abilityMod)
		{
			cachedEnemyCastHitEffectData = this.m_abilityMod.m_enemyCastHitEffectDataMod.GetModifiedValue(this.m_enemyCastHitEffectData);
		}
		else
		{
			cachedEnemyCastHitEffectData = this.m_enemyCastHitEffectData;
		}
		this.m_cachedEnemyCastHitEffectData = cachedEnemyCastHitEffectData;
		StandardActorEffectData cachedAllyCastHitEffectData;
		if (this.m_abilityMod)
		{
			cachedAllyCastHitEffectData = this.m_abilityMod.m_allyCastHitEffectDataMod.GetModifiedValue(this.m_allyCastHitEffectData);
		}
		else
		{
			cachedAllyCastHitEffectData = this.m_allyCastHitEffectData;
		}
		this.m_cachedAllyCastHitEffectData = cachedAllyCastHitEffectData;
		this.m_cachedEffectAddedOnEnemyAttack = ((!this.m_abilityMod) ? this.m_effectAddedOnEnemyAttack : this.m_abilityMod.m_effectAddedOnEnemyAttackMod.GetModifiedValue(this.m_effectAddedOnEnemyAttack));
		StandardEffectInfo cachedEffectAddedOnAllyAttack;
		if (this.m_abilityMod)
		{
			cachedEffectAddedOnAllyAttack = this.m_abilityMod.m_effectAddedOnAllyAttackMod.GetModifiedValue(this.m_effectAddedOnAllyAttack);
		}
		else
		{
			cachedEffectAddedOnAllyAttack = this.m_effectAddedOnAllyAttack;
		}
		this.m_cachedEffectAddedOnAllyAttack = cachedEffectAddedOnAllyAttack;
	}

	public bool CanTargetAlly()
	{
		return (!this.m_abilityMod) ? this.m_canTargetAlly : this.m_abilityMod.m_canTargetAllyMod.GetModifiedValue(this.m_canTargetAlly);
	}

	public bool CanTargetEnemy()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canTargetEnemyMod.GetModifiedValue(this.m_canTargetEnemy);
		}
		else
		{
			result = this.m_canTargetEnemy;
		}
		return result;
	}

	public bool CanTagetSelf()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canTagetSelfMod.GetModifiedValue(this.m_canTagetSelf);
		}
		else
		{
			result = this.m_canTagetSelf;
		}
		return result;
	}

	public bool TargetingIgnoreLos()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_targetingIgnoreLosMod.GetModifiedValue(this.m_targetingIgnoreLos);
		}
		else
		{
			result = this.m_targetingIgnoreLos;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return (this.m_cachedLaserInfo == null) ? this.m_laserInfo : this.m_cachedLaserInfo;
	}

	public StandardActorEffectData GetEnemyCastHitEffectData()
	{
		StandardActorEffectData result;
		if (this.m_cachedEnemyCastHitEffectData != null)
		{
			result = this.m_cachedEnemyCastHitEffectData;
		}
		else
		{
			result = this.m_enemyCastHitEffectData;
		}
		return result;
	}

	public StandardActorEffectData GetAllyCastHitEffectData()
	{
		return (this.m_cachedAllyCastHitEffectData == null) ? this.m_allyCastHitEffectData : this.m_cachedAllyCastHitEffectData;
	}

	public int GetEnergyToAllyTargetOnCast()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_energyToAllyTargetOnCastMod.GetModifiedValue(this.m_energyToAllyTargetOnCast);
		}
		else
		{
			result = this.m_energyToAllyTargetOnCast;
		}
		return result;
	}

	public bool EndEffectIfAppendedStatus()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_endEffectIfAppendedStatusMod.GetModifiedValue(this.m_endEffectIfAppendedStatus);
		}
		else
		{
			result = this.m_endEffectIfAppendedStatus;
		}
		return result;
	}

	public StandardEffectInfo GetEffectAddedOnEnemyAttack()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectAddedOnEnemyAttack != null)
		{
			result = this.m_cachedEffectAddedOnEnemyAttack;
		}
		else
		{
			result = this.m_effectAddedOnEnemyAttack;
		}
		return result;
	}

	public StandardEffectInfo GetEffectAddedOnAllyAttack()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectAddedOnAllyAttack != null)
		{
			result = this.m_cachedEffectAddedOnAllyAttack;
		}
		else
		{
			result = this.m_effectAddedOnAllyAttack;
		}
		return result;
	}

	public int GetEnergyGainOnAllyAppendHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_energyGainOnAllyAppendHitMod.GetModifiedValue(this.m_energyGainOnAllyAppendHit);
		}
		else
		{
			result = this.m_energyGainOnAllyAppendHit;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportEnergy(ref result, AbilityTooltipSubject.Ally, this.GetEnergyToAllyTargetOnCast());
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (this.m_targetingMode == SenseiAppendStatus.TargetingMode.ActorSquare)
		{
			ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
			return base.CanTargetActorInDecision(caster, currentBestActorTarget, this.CanTargetEnemy(), this.CanTargetAlly(), this.CanTagetSelf(), Ability.ValidateCheckPath.Ignore, !this.TargetingIgnoreLos(), true, false);
		}
		return true;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_targetingMode == SenseiAppendStatus.TargetingMode.ActorSquare)
		{
			return base.HasTargetableActorsInDecision(caster, this.CanTargetEnemy(), this.CanTargetAlly(), this.CanTagetSelf(), Ability.ValidateCheckPath.Ignore, !this.TargetingIgnoreLos(), true, false);
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		this.m_enemyCastHitEffectData.AddTooltipTokens(tokens, "EnemyCastHitEffectData", false, null);
		this.m_allyCastHitEffectData.AddTooltipTokens(tokens, "AllyCastHitEffectData", false, null);
		base.AddTokenInt(tokens, "EnergyToAllyTargetOnCast", string.Empty, this.m_energyToAllyTargetOnCast, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectAddedOnEnemyAttack, "EffectAddedOnEnemyAttack", this.m_effectAddedOnEnemyAttack, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectAddedOnAllyAttack, "EffectAddedOnAllyAttack", this.m_effectAddedOnAllyAttack, true);
		base.AddTokenInt(tokens, "EnergyGainOnAllyAppendHit", string.Empty, this.m_energyGainOnAllyAppendHit, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiAppendStatus))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SenseiAppendStatus);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public enum TargetingMode
	{
		ActorSquare,
		Laser
	}
}
