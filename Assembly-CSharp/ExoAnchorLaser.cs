using System.Collections.Generic;
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
		AbilityUtil_Targeter abilityUtil_Targeter = new AbilityUtil_Targeter_SweepSingleClickCone(this, GetMinConeAngle(), GetMaxConeAngle(), GetLaserInfo().range, m_sweepConeBackwardOffset, 0.2f, GetLaserInfo(), m_syncComponent);
		abilityUtil_Targeter.SetAffectedGroups(true, false, false);
		base.Targeter = abilityUtil_Targeter;
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Art --</color>\n" + Ability.SetupNoteVarName("Laser Extend Sequence Prefab") + "\nFor initial cast, when laser is not already out. Only for gameplay hits and timing of when actual visual show up, no vfx spawned.\n\n" + Ability.SetupNoteVarName("Sweep Sequence Prefab") + "\nfor laser visual, rotation of the actor, removing the previous laser, and gameplay hit timing when sweeping\n\n" + Ability.SetupNoteVarName("Unanchor Anim Sequence Prefab") + "\nfor setting idle type when un-anchoring and removing the previous laser vfx\n\n" + Ability.SetupNoteVarName("Persistent Laser Barrier Sequence") + "\nfor persistent laser visuals (which is a barrier internally), and optionally ExoLaserHittingWallSequence for a continuing impact vfx\n\n";
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
		if (m_syncComponent != null)
		{
			if (m_syncComponent.m_anchored)
			{
				return (ActorModelData.ActionAnimationType)m_animIndexForSweep;
			}
		}
		return base.GetActionAnimType();
	}

	private void SetCachedFields()
	{
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
		StandardBarrierData cachedLaserBarrier;
		if ((bool)m_abilityMod)
		{
			cachedLaserBarrier = m_abilityMod.m_laserBarrierMod.GetModifiedValue(m_laserBarrier);
		}
		else
		{
			cachedLaserBarrier = m_laserBarrier;
		}
		m_cachedLaserBarrier = cachedLaserBarrier;
		StandardEffectInfo cachedEffectOnCaster;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnCaster = m_abilityMod.m_effectOnCasterMod.GetModifiedValue(m_effectOnCaster);
		}
		else
		{
			cachedEffectOnCaster = m_effectOnCaster;
		}
		m_cachedEffectOnCaster = cachedEffectOnCaster;
		m_cachedEffectOnAnchorEnd = ((!m_abilityMod) ? m_effectOnAnchorEnd : m_abilityMod.m_effectOnAnchorEndMod.GetModifiedValue(m_effectOnAnchorEnd));
	}

	public int GetLaserDamageAmount()
	{
		return (!m_abilityMod) ? m_laserDamageAmount : m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount);
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

	public StandardBarrierData GetLaserBarrier()
	{
		StandardBarrierData result;
		if (m_cachedLaserBarrier != null)
		{
			result = m_cachedLaserBarrier;
		}
		else
		{
			result = m_laserBarrier;
		}
		return result;
	}

	public int GetSweepDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_sweepDamageAmountMod.GetModifiedValue(m_sweepDamageAmount);
		}
		else
		{
			result = m_sweepDamageAmount;
		}
		return result;
	}

	public float GetSweepConeBackwardOffset()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_sweepConeBackwardOffsetMod.GetModifiedValue(m_sweepConeBackwardOffset);
		}
		else
		{
			result = m_sweepConeBackwardOffset;
		}
		return result;
	}

	public float GetMinConeAngle()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_minConeAngleMod.GetModifiedValue(m_minConeAngle);
		}
		else
		{
			result = m_minConeAngle;
		}
		return result;
	}

	public float GetMaxConeAngle()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxConeAngleMod.GetModifiedValue(m_maxConeAngle);
		}
		else
		{
			result = m_maxConeAngle;
		}
		return result;
	}

	public int GetExtraDamagePerTurnAnchored()
	{
		return (!m_abilityMod) ? m_extraDamagePerTurnAnchored : m_abilityMod.m_extraDamagePerTurnAnchoredMod.GetModifiedValue(m_extraDamagePerTurnAnchored);
	}

	public int GetMaxExtraDamageForAnchored()
	{
		return (!m_abilityMod) ? m_maxExtraDamageForAnchored : m_abilityMod.m_maxExtraDamageForAnchoredMod.GetModifiedValue(m_maxExtraDamageForAnchored);
	}

	public float GetExtraDamageAtZeroDist()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageAtZeroDistMod.GetModifiedValue(m_extraDamageAtZeroDist);
		}
		else
		{
			result = m_extraDamageAtZeroDist;
		}
		return result;
	}

	public float GetExtraDamageChangePerDist()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageChangePerDistMod.GetModifiedValue(m_extraDamageChangePerDist);
		}
		else
		{
			result = m_extraDamageChangePerDist;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnCaster()
	{
		return (m_cachedEffectOnCaster == null) ? m_effectOnCaster : m_cachedEffectOnCaster;
	}

	public int GetCooldownOnEnd()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cooldownOnEndMod.GetModifiedValue(m_cooldownOnEnd);
		}
		else
		{
			result = m_cooldownOnEnd;
		}
		return result;
	}

	public int GetAnchoredTechPointCost()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_anchoredTechPointCostMod.GetModifiedValue(m_anchoredTechPointCost);
		}
		else
		{
			result = m_anchoredTechPointCost;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnAnchorEnd()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnAnchorEnd != null)
		{
			result = m_cachedEffectOnAnchorEnd;
		}
		else
		{
			result = m_effectOnAnchorEnd;
		}
		return result;
	}

	public bool ShouldUpdateMovementOnAnchorChange()
	{
		List<StatusType> list = m_statusWhenAnchoredAndNotSweeping;
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_useStatusWhenAnchoredAndNotSweepingOverride)
			{
				list = m_abilityMod.m_statusWhenAnchoredAndNotSweepingOverride;
			}
		}
		int result;
		if (list != null)
		{
			result = ((list.Count > 0) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool HasPendingStatusTurnOfAnchorEnd(StatusType status)
	{
		List<StatusType> list = m_statusWhenAnchoredAndNotSweeping;
		if (m_abilityMod != null && m_abilityMod.m_useStatusWhenAnchoredAndNotSweepingOverride)
		{
			list = m_abilityMod.m_statusWhenAnchoredAndNotSweepingOverride;
		}
		if (list != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return list.Contains(status);
				}
			}
		}
		return false;
	}

	public int GetTotalDamage(Vector3 startPos, Vector3 hitPos, int baseDamage, bool checkDurationBonus)
	{
		int num = baseDamage;
		if (m_syncComponent != null)
		{
			if (GetExtraDamagePerTurnAnchored() > 0)
			{
				if (checkDurationBonus)
				{
					int turnsAnchored = m_syncComponent.m_turnsAnchored;
					int num2 = Mathf.Max(0, turnsAnchored) * GetExtraDamagePerTurnAnchored();
					if (GetMaxExtraDamageForAnchored() > 0)
					{
						num2 = Mathf.Min(num2, GetMaxExtraDamageForAnchored());
					}
					num += num2;
				}
			}
			if (!(GetExtraDamageAtZeroDist() > 0f))
			{
				if (!(GetExtraDamageChangePerDist() > 0f))
				{
					goto IL_010a;
				}
			}
			float num3 = GetExtraDamageAtZeroDist();
			float num4 = VectorUtils.HorizontalPlaneDistInSquares(startPos, hitPos) - 1.4f;
			if (num4 > 0f)
			{
				num3 += GetExtraDamageChangePerDist() * num4;
			}
			num += Mathf.Max(0, Mathf.RoundToInt(num3));
		}
		goto IL_010a;
		IL_010a:
		return num;
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
		ActorData actorData = base.ActorData;
		int totalDamage;
		if (m_syncComponent != null && m_syncComponent.m_anchored)
		{
			totalDamage = GetTotalDamage(actorData.GetTravelBoardSquareWorldPosition(), targetActor.GetTravelBoardSquareWorldPosition(), GetSweepDamageAmount(), true);
		}
		else
		{
			totalDamage = GetTotalDamage(actorData.GetTravelBoardSquareWorldPosition(), targetActor.GetTravelBoardSquareWorldPosition(), GetLaserDamageAmount(), false);
		}
		Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, totalDamage);
		return symbolToValue;
	}

	public override int GetModdedCost()
	{
		if (m_syncComponent != null)
		{
			if (m_syncComponent.m_wasAnchoredOnTurnStart)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return GetAnchoredTechPointCost();
					}
				}
			}
		}
		return base.GetModdedCost();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (m_syncComponent != null)
		{
			if (m_syncComponent.m_anchored)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return m_anchoredActionAnimType;
					}
				}
			}
		}
		return base.GetActionAnimType();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoAnchorLaser abilityMod_ExoAnchorLaser = modAsBase as AbilityMod_ExoAnchorLaser;
		float num;
		if ((bool)abilityMod_ExoAnchorLaser)
		{
			num = abilityMod_ExoAnchorLaser.m_maxConeAngleMod.GetModifiedValue(m_maxConeAngle);
		}
		else
		{
			num = m_maxConeAngle;
		}
		int val = (int)num;
		AddTokenInt(tokens, "Sweep_Angle", "max angle from the previous direction to sweep the laser", val);
		string empty = string.Empty;
		int val2;
		if ((bool)abilityMod_ExoAnchorLaser)
		{
			val2 = abilityMod_ExoAnchorLaser.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			val2 = m_laserDamageAmount;
		}
		AddTokenInt(tokens, "Damage_Laser", empty, val2);
		StandardBarrierData standardBarrierData;
		if ((bool)abilityMod_ExoAnchorLaser)
		{
			standardBarrierData = abilityMod_ExoAnchorLaser.m_laserBarrierMod.GetModifiedValue(m_laserBarrier);
		}
		else
		{
			standardBarrierData = m_laserBarrier;
		}
		StandardBarrierData standardBarrierData2 = standardBarrierData;
		standardBarrierData2.AddTooltipTokens(tokens, "Laser_Barrier", abilityMod_ExoAnchorLaser != null, m_laserBarrier);
		string empty2 = string.Empty;
		int val3;
		if ((bool)abilityMod_ExoAnchorLaser)
		{
			val3 = abilityMod_ExoAnchorLaser.m_sweepDamageAmountMod.GetModifiedValue(m_sweepDamageAmount);
		}
		else
		{
			val3 = m_sweepDamageAmount;
		}
		AddTokenInt(tokens, "Damage_Sweep", empty2, val3);
		AddTokenInt(tokens, "ExtraDamagePerTurnAnchored", string.Empty, (!abilityMod_ExoAnchorLaser) ? m_extraDamagePerTurnAnchored : abilityMod_ExoAnchorLaser.m_extraDamagePerTurnAnchoredMod.GetModifiedValue(m_extraDamagePerTurnAnchored));
		string empty3 = string.Empty;
		int val4;
		if ((bool)abilityMod_ExoAnchorLaser)
		{
			val4 = abilityMod_ExoAnchorLaser.m_maxExtraDamageForAnchoredMod.GetModifiedValue(m_maxExtraDamageForAnchored);
		}
		else
		{
			val4 = m_maxExtraDamageForAnchored;
		}
		AddTokenInt(tokens, "MaxExtraDamageForAnchored", empty3, val4);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_ExoAnchorLaser)
		{
			effectInfo = abilityMod_ExoAnchorLaser.m_effectOnCasterMod.GetModifiedValue(m_effectOnCaster);
		}
		else
		{
			effectInfo = m_effectOnCaster;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnCaster", m_effectOnCaster);
		string empty4 = string.Empty;
		int val5;
		if ((bool)abilityMod_ExoAnchorLaser)
		{
			val5 = abilityMod_ExoAnchorLaser.m_cooldownOnEndMod.GetModifiedValue(m_cooldownOnEnd);
		}
		else
		{
			val5 = m_cooldownOnEnd;
		}
		AddTokenInt(tokens, "Cooldown", empty4, val5);
		string empty5 = string.Empty;
		int val6;
		if ((bool)abilityMod_ExoAnchorLaser)
		{
			val6 = abilityMod_ExoAnchorLaser.m_anchoredTechPointCostMod.GetModifiedValue(m_anchoredTechPointCost);
		}
		else
		{
			val6 = m_anchoredTechPointCost;
		}
		AddTokenInt(tokens, "AnchoredTechPointCost", empty5, val6);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_ExoAnchorLaser)
		{
			effectInfo2 = abilityMod_ExoAnchorLaser.m_effectOnAnchorEndMod.GetModifiedValue(m_effectOnAnchorEnd);
		}
		else
		{
			effectInfo2 = m_effectOnAnchorEnd;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnAnchorEnd", m_effectOnAnchorEnd);
	}

	public override string GetFullTooltip()
	{
		if (m_syncComponent != null)
		{
			if (m_syncComponent.m_anchored && !string.IsNullOrEmpty(m_anchoredToolTip))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (string.IsNullOrEmpty(m_anchoredFinalFullTooltip))
						{
							return TooltipTokenEntry.GetTooltipWithSubstitutes(m_anchoredToolTip, null);
						}
						return TooltipTokenEntry.GetTooltipWithSubstitutes(m_anchoredFinalFullTooltip, null);
					}
				}
			}
		}
		return base.GetFullTooltip();
	}

	public override void SetUnlocalizedTooltipAndStatusTypes(AbilityMod mod = null)
	{
		if (!string.IsNullOrEmpty(m_anchoredToolTip))
		{
			List<TooltipTokenEntry> tooltipTokenEntries = GetTooltipTokenEntries(mod);
			m_anchoredFinalFullTooltip = TooltipTokenEntry.GetTooltipWithSubstitutes(m_anchoredToolTip, tooltipTokenEntries);
		}
		base.SetUnlocalizedTooltipAndStatusTypes(mod);
	}

	public override bool HasPassivePendingStatus(StatusType status, ActorData owner)
	{
		if (m_syncComponent != null && m_syncComponent.m_wasAnchoredOnTurnStart)
		{
			if (owner != null)
			{
				if (!owner.GetAbilityData().HasQueuedAction(AbilityData.ActionType.ABILITY_4))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return HasPendingStatusTurnOfAnchorEnd(status);
						}
					}
				}
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoAnchorLaser))
		{
			m_abilityMod = (abilityMod as AbilityMod_ExoAnchorLaser);
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
		if (m_syncComponent != null)
		{
			if (m_syncComponent.m_anchored)
			{
				if (m_syncComponent.m_turnsAnchored > 0)
				{
					return sweepAngle / m_turnToTargetSweepDegreesPerSecond;
				}
			}
		}
		return 0.2f;
	}

	public override bool ShouldRotateToTargetPos()
	{
		if (m_syncComponent != null)
		{
			if (m_syncComponent.m_anchored)
			{
				if (m_syncComponent.m_turnsAnchored > 0)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
			}
		}
		return true;
	}
}
