using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ExoAnchorLaser : Ability
{
	[Space(20f)]
	[Header("-- First Cast Damage (non-anchored)")]
	public int m_laserDamageAmount = 25;
	public LaserTargetingInfo m_laserInfo;
	[Header("-- Barrier For Beam")]
	public StandardBarrierData m_laserBarrier;
	[Header("-- Cone to Sweep Across")]
	public int m_sweepDamageAmount = 25;
	public float m_sweepConeBackwardOffset;
	public float m_minConeAngle;
	public float m_maxConeAngle = 180f;
	public ActorModelData.ActionAnimationType m_anchoredActionAnimType;
	public float m_turnToTargetSweepDegreesPerSecond = 90f;
	[Header("-- Extra Damage: for anchored turns")]
	public int m_extraDamagePerTurnAnchored;
	public int m_maxExtraDamageForAnchored;
	[Header("-- Extra Damage: for distance")]
	public float m_extraDamageAtZeroDist;
	public float m_extraDamageChangePerDist;
	[Header("-- Effect while anchored and cooldown when finished")]
	public StandardEffectInfo m_effectOnCaster;
	public int m_cooldownOnEnd;
	public int m_anchoredTechPointCost;
	public StandardEffectInfo m_effectOnAnchorEnd;
	[Header("-- Pending Status for Anchored and NOT using sweep")]
	public List<StatusType> m_statusWhenAnchoredAndNotSweeping;
	[Header("-- Alternate Tooltip while anchored")]
	public string m_anchoredToolTip;
	[HideInInspector]
	public string m_anchoredFinalFullTooltip;
	[Header("-- Animation --")]
	public int m_animIndexForSweep = 6;
	[Header("-- Sequences")]
	public GameObject m_laserExtendSequencePrefab;
	public GameObject m_sweepSequencePrefab;
	public GameObject m_unanchorAnimSequencePrefab;
	public GameObject m_persistentLaserBarrierSequence;

	private Exo_SyncComponent m_syncComponent;
	private AbilityMod_ExoAnchorLaser m_abilityMod;
	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardBarrierData m_cachedLaserBarrier;
	private StandardEffectInfo m_cachedEffectOnCaster;
	private StandardEffectInfo m_cachedEffectOnAnchorEnd;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Anchor Laser";
		}
		SetupTargeter();
		ActorStatus component = GetComponent<ActorStatus>();
		if (component != null)
		{
			component.AddAbilityForPassivePendingStatus(this);
		}
	}

	private void OnDestroy()
	{
		ActorStatus component = GetComponent<ActorStatus>();
		if (component != null)
		{
			component.RemoveAbilityForPassivePendingStatus(this);
		}
	}

	public void SetupTargeter()
	{
		SetCachedFields();
		if (m_syncComponent == null)
		{
			m_syncComponent = GetComponent<Exo_SyncComponent>();
		}
		if (m_syncComponent == null)
		{
			Log.Error("Missing Exo_SyncComponent on Exo's actorData prefab. ExoAnchorLaser won't function!");
		}
		AbilityUtil_Targeter targeter = new AbilityUtil_Targeter_SweepSingleClickCone(
			this,
			GetMinConeAngle(),
			GetMaxConeAngle(),
			GetLaserInfo().range,
			m_sweepConeBackwardOffset,
			0.2f,
			GetLaserInfo(),
			m_syncComponent);
		targeter.SetAffectedGroups(true, false, false);
		Targeter = targeter;
	}

	public override string GetSetupNotesForEditor()
	{
		return new StringBuilder().Append("<color=cyan>-- For Art --</color>\n").Append(SetupNoteVarName("Laser Extend Sequence Prefab")).Append("\nFor initial cast, when laser is not already out. Only for gameplay hits and timing of when actual visual show up, no vfx spawned.\n\n").Append(SetupNoteVarName("Sweep Sequence Prefab")).Append("\nfor laser visual, rotation of the actor, removing the previous laser, and gameplay hit timing when sweeping\n\n").Append(SetupNoteVarName("Unanchor Anim Sequence Prefab")).Append("\nfor setting idle type when un-anchoring and removing the previous laser vfx\n\n").Append(SetupNoteVarName("Persistent Laser Barrier Sequence")).Append("\nfor persistent laser visuals (which is a barrier internally), and optionally ExoLaserHittingWallSequence for a continuing impact vfx\n\n").ToString();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return base.CanTriggerAnimAtIndexForTaunt(animIndex) || animIndex == m_animIndexForSweep;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		if (m_syncComponent != null && m_syncComponent.m_anchored)
		{
			return (ActorModelData.ActionAnimationType)m_animIndexForSweep;
		}
		return base.GetActionAnimType();
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_abilityMod != null
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		m_cachedLaserBarrier = m_abilityMod != null
			? m_abilityMod.m_laserBarrierMod.GetModifiedValue(m_laserBarrier)
			: m_laserBarrier;
		m_cachedEffectOnCaster = m_abilityMod != null
			? m_abilityMod.m_effectOnCasterMod.GetModifiedValue(m_effectOnCaster)
			: m_effectOnCaster;
		m_cachedEffectOnAnchorEnd = m_abilityMod != null
			? m_abilityMod.m_effectOnAnchorEndMod.GetModifiedValue(m_effectOnAnchorEnd)
			: m_effectOnAnchorEnd;
	}

	public int GetLaserDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public StandardBarrierData GetLaserBarrier()
	{
		return m_cachedLaserBarrier ?? m_laserBarrier;
	}

	public int GetSweepDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_sweepDamageAmountMod.GetModifiedValue(m_sweepDamageAmount)
			: m_sweepDamageAmount;
	}

	public float GetSweepConeBackwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_sweepConeBackwardOffsetMod.GetModifiedValue(m_sweepConeBackwardOffset)
			: m_sweepConeBackwardOffset;
	}

	public float GetMinConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minConeAngleMod.GetModifiedValue(m_minConeAngle)
			: m_minConeAngle;
	}

	public float GetMaxConeAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxConeAngleMod.GetModifiedValue(m_maxConeAngle)
			: m_maxConeAngle;
	}

	public int GetExtraDamagePerTurnAnchored()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamagePerTurnAnchoredMod.GetModifiedValue(m_extraDamagePerTurnAnchored)
			: m_extraDamagePerTurnAnchored;
	}

	public int GetMaxExtraDamageForAnchored()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxExtraDamageForAnchoredMod.GetModifiedValue(m_maxExtraDamageForAnchored)
			: m_maxExtraDamageForAnchored;
	}

	public float GetExtraDamageAtZeroDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageAtZeroDistMod.GetModifiedValue(m_extraDamageAtZeroDist)
			: m_extraDamageAtZeroDist;
	}

	public float GetExtraDamageChangePerDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageChangePerDistMod.GetModifiedValue(m_extraDamageChangePerDist)
			: m_extraDamageChangePerDist;
	}

	public StandardEffectInfo GetEffectOnCaster()
	{
		return m_cachedEffectOnCaster ?? m_effectOnCaster;
	}

	public int GetCooldownOnEnd()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownOnEndMod.GetModifiedValue(m_cooldownOnEnd)
			: m_cooldownOnEnd;
	}

	public int GetAnchoredTechPointCost()
	{
		return m_abilityMod != null
			? m_abilityMod.m_anchoredTechPointCostMod.GetModifiedValue(m_anchoredTechPointCost)
			: m_anchoredTechPointCost;
	}

	public StandardEffectInfo GetEffectOnAnchorEnd()
	{
		return m_cachedEffectOnAnchorEnd ?? m_effectOnAnchorEnd;
	}

	public bool ShouldUpdateMovementOnAnchorChange()
	{
		List<StatusType> list = m_statusWhenAnchoredAndNotSweeping;
		if (m_abilityMod != null && m_abilityMod.m_useStatusWhenAnchoredAndNotSweepingOverride)
		{
			list = m_abilityMod.m_statusWhenAnchoredAndNotSweepingOverride;
		}
		return list != null && list.Count > 0;
	}

	public bool HasPendingStatusTurnOfAnchorEnd(StatusType status)
	{
		List<StatusType> list = m_statusWhenAnchoredAndNotSweeping;
		if (m_abilityMod != null && m_abilityMod.m_useStatusWhenAnchoredAndNotSweepingOverride)
		{
			list = m_abilityMod.m_statusWhenAnchoredAndNotSweepingOverride;
		}
		return list != null && list.Contains(status);
	}

	public int GetTotalDamage(Vector3 startPos, Vector3 hitPos, int baseDamage, bool checkDurationBonus)
	{
		int damage = baseDamage;
		if (m_syncComponent != null)
		{
			if (GetExtraDamagePerTurnAnchored() > 0 && checkDurationBonus)
			{
				int turnsAnchored = m_syncComponent.m_turnsAnchored;
				int extraDamageForAnchor = Mathf.Max(0, turnsAnchored) * GetExtraDamagePerTurnAnchored();
				if (GetMaxExtraDamageForAnchored() > 0)
				{
					extraDamageForAnchor = Mathf.Min(extraDamageForAnchor, GetMaxExtraDamageForAnchored());
				}
				damage += extraDamageForAnchor;
			}
			if (GetExtraDamageAtZeroDist() > 0f || GetExtraDamageChangePerDist() > 0f)
			{
				float extraDamageAtZeroDist = GetExtraDamageAtZeroDist();
				float distAdjusted = VectorUtils.HorizontalPlaneDistInSquares(startPos, hitPos) - 1.4f;
				if (distAdjusted > 0f)
				{
					extraDamageAtZeroDist += GetExtraDamageChangePerDist() * distAdjusted;
				}
				damage += Mathf.Max(0, Mathf.RoundToInt(extraDamageAtZeroDist));
			}
		}
		return damage;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetLaserDamageAmount());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		int totalDamage = m_syncComponent != null && m_syncComponent.m_anchored
			? GetTotalDamage(ActorData.GetFreePos(), targetActor.GetFreePos(), GetSweepDamageAmount(), true)
			: GetTotalDamage(ActorData.GetFreePos(), targetActor.GetFreePos(), GetLaserDamageAmount(), false);
		AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, totalDamage);
		return symbolToValue;
	}

	public override int GetModdedCost()
	{
		return m_syncComponent != null && m_syncComponent.m_wasAnchoredOnTurnStart
			? GetAnchoredTechPointCost()
			: base.GetModdedCost();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		return m_syncComponent != null && m_syncComponent.m_anchored
			? m_anchoredActionAnimType
			: base.GetActionAnimType();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoAnchorLaser abilityMod_ExoAnchorLaser = modAsBase as AbilityMod_ExoAnchorLaser;
		AddTokenInt(tokens, "Sweep_Angle", "max angle from the previous direction to sweep the laser", (int)(abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_maxConeAngleMod.GetModifiedValue(m_maxConeAngle)
			: m_maxConeAngle));
		AddTokenInt(tokens, "Damage_Laser", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		StandardBarrierData standardBarrierData = abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_laserBarrierMod.GetModifiedValue(m_laserBarrier)
			: m_laserBarrier;
		standardBarrierData.AddTooltipTokens(tokens, "Laser_Barrier", abilityMod_ExoAnchorLaser != null, m_laserBarrier);
		AddTokenInt(tokens, "Damage_Sweep", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_sweepDamageAmountMod.GetModifiedValue(m_sweepDamageAmount)
			: m_sweepDamageAmount);
		AddTokenInt(tokens, "ExtraDamagePerTurnAnchored", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_extraDamagePerTurnAnchoredMod.GetModifiedValue(m_extraDamagePerTurnAnchored)
			: m_extraDamagePerTurnAnchored);
		AddTokenInt(tokens, "MaxExtraDamageForAnchored", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_maxExtraDamageForAnchoredMod.GetModifiedValue(m_maxExtraDamageForAnchored)
			: m_maxExtraDamageForAnchored);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_effectOnCasterMod.GetModifiedValue(m_effectOnCaster)
			: m_effectOnCaster, "EffectOnCaster", m_effectOnCaster);
		AddTokenInt(tokens, "Cooldown", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_cooldownOnEndMod.GetModifiedValue(m_cooldownOnEnd)
			: m_cooldownOnEnd);
		AddTokenInt(tokens, "AnchoredTechPointCost", string.Empty, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_anchoredTechPointCostMod.GetModifiedValue(m_anchoredTechPointCost)
			: m_anchoredTechPointCost);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ExoAnchorLaser != null
			? abilityMod_ExoAnchorLaser.m_effectOnAnchorEndMod.GetModifiedValue(m_effectOnAnchorEnd)
			: m_effectOnAnchorEnd, "EffectOnAnchorEnd", m_effectOnAnchorEnd);
	}

	public override string GetFullTooltip()
	{
		return m_syncComponent != null
		       && m_syncComponent.m_anchored
		       && !string.IsNullOrEmpty(m_anchoredToolTip)
			? TooltipTokenEntry.GetTooltipWithSubstitutes(string.IsNullOrEmpty(m_anchoredFinalFullTooltip)
				? m_anchoredToolTip
				: m_anchoredFinalFullTooltip, 
				null)
			: base.GetFullTooltip();
	}

	public override void SetUnlocalizedTooltipAndStatusTypes(AbilityMod mod = null)
	{
		if (!string.IsNullOrEmpty(m_anchoredToolTip))
		{
			m_anchoredFinalFullTooltip = TooltipTokenEntry.GetTooltipWithSubstitutes(m_anchoredToolTip, GetTooltipTokenEntries(mod));
		}
		base.SetUnlocalizedTooltipAndStatusTypes(mod);
	}

	public override bool HasPassivePendingStatus(StatusType status, ActorData owner)
	{
		return m_syncComponent != null
		       && m_syncComponent.m_wasAnchoredOnTurnStart
		       && owner != null
		       && !owner.GetAbilityData().HasQueuedAction(AbilityData.ActionType.ABILITY_4)
		       && HasPendingStatusTurnOfAnchorEnd(status);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoAnchorLaser))
		{
			m_abilityMod = abilityMod as AbilityMod_ExoAnchorLaser;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public float GetRotateToTargetDuration(float sweepAngle)
	{
		return m_syncComponent != null
		       && m_syncComponent.m_anchored
		       && m_syncComponent.m_turnsAnchored > 0
			? sweepAngle / m_turnToTargetSweepDegreesPerSecond
			: 0.2f;
	}

	public override bool ShouldRotateToTargetPos()
	{
		return m_syncComponent == null
		       || !m_syncComponent.m_anchored
		       || m_syncComponent.m_turnsAnchored <= 0;
	}
}
