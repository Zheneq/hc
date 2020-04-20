using System;
using System.Collections.Generic;
using UnityEngine;

public class ExoAnchorLaser : Ability
{
	[Space(20f)]
	[Header("-- First Cast Damage (non-anchored)")]
	public int m_laserDamageAmount = 0x19;

	public LaserTargetingInfo m_laserInfo;

	[Header("-- Barrier For Beam")]
	public StandardBarrierData m_laserBarrier;

	[Header("-- Cone to Sweep Across")]
	public int m_sweepDamageAmount = 0x19;

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
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.Start()).MethodHandle;
			}
			this.m_abilityName = "Anchor Laser";
		}
		this.SetupTargeter();
		ActorStatus component = base.GetComponent<ActorStatus>();
		if (component != null)
		{
			component.AddAbilityForPassivePendingStatus(this);
		}
	}

	private void OnDestroy()
	{
		ActorStatus component = base.GetComponent<ActorStatus>();
		if (component != null)
		{
			component.RemoveAbilityForPassivePendingStatus(this);
		}
	}

	public void SetupTargeter()
	{
		this.SetCachedFields();
		if (this.m_syncComponent == null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.SetupTargeter()).MethodHandle;
			}
			this.m_syncComponent = base.GetComponent<Exo_SyncComponent>();
		}
		if (this.m_syncComponent == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Error("Missing Exo_SyncComponent on Exo's actorData prefab. ExoAnchorLaser won't function!", new object[0]);
		}
		AbilityUtil_Targeter abilityUtil_Targeter = new AbilityUtil_Targeter_SweepSingleClickCone(this, this.GetMinConeAngle(), this.GetMaxConeAngle(), this.GetLaserInfo().range, this.m_sweepConeBackwardOffset, 0.2f, this.GetLaserInfo(), this.m_syncComponent);
		abilityUtil_Targeter.SetAffectedGroups(true, false, false);
		base.Targeter = abilityUtil_Targeter;
	}

	public override string GetSetupNotesForEditor()
	{
		return string.Concat(new string[]
		{
			"<color=cyan>-- For Art --</color>\n",
			Ability.SetupNoteVarName("Laser Extend Sequence Prefab"),
			"\nFor initial cast, when laser is not already out. Only for gameplay hits and timing of when actual visual show up, no vfx spawned.\n\n",
			Ability.SetupNoteVarName("Sweep Sequence Prefab"),
			"\nfor laser visual, rotation of the actor, removing the previous laser, and gameplay hit timing when sweeping\n\n",
			Ability.SetupNoteVarName("Unanchor Anim Sequence Prefab"),
			"\nfor setting idle type when un-anchoring and removing the previous laser vfx\n\n",
			Ability.SetupNoteVarName("Persistent Laser Barrier Sequence"),
			"\nfor persistent laser visuals (which is a barrier internally), and optionally ExoLaserHittingWallSequence for a continuing impact vfx\n\n"
		});
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserInfo().range;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return base.CanTriggerAnimAtIndexForTaunt(animIndex) || animIndex == this.m_animIndexForSweep;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		if (this.m_syncComponent != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetActionAnimType(List<AbilityTarget>, ActorData)).MethodHandle;
			}
			if (this.m_syncComponent.m_anchored)
			{
				return (ActorModelData.ActionAnimationType)this.m_animIndexForSweep;
			}
		}
		return base.GetActionAnimType();
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserInfo;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.SetCachedFields()).MethodHandle;
			}
			cachedLaserInfo = this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo);
		}
		else
		{
			cachedLaserInfo = this.m_laserInfo;
		}
		this.m_cachedLaserInfo = cachedLaserInfo;
		StandardBarrierData cachedLaserBarrier;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedLaserBarrier = this.m_abilityMod.m_laserBarrierMod.GetModifiedValue(this.m_laserBarrier);
		}
		else
		{
			cachedLaserBarrier = this.m_laserBarrier;
		}
		this.m_cachedLaserBarrier = cachedLaserBarrier;
		StandardEffectInfo cachedEffectOnCaster;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEffectOnCaster = this.m_abilityMod.m_effectOnCasterMod.GetModifiedValue(this.m_effectOnCaster);
		}
		else
		{
			cachedEffectOnCaster = this.m_effectOnCaster;
		}
		this.m_cachedEffectOnCaster = cachedEffectOnCaster;
		this.m_cachedEffectOnAnchorEnd = ((!this.m_abilityMod) ? this.m_effectOnAnchorEnd : this.m_abilityMod.m_effectOnAnchorEndMod.GetModifiedValue(this.m_effectOnAnchorEnd));
	}

	public int GetLaserDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_laserDamageAmount : this.m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount);
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetLaserInfo()).MethodHandle;
			}
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	public StandardBarrierData GetLaserBarrier()
	{
		StandardBarrierData result;
		if (this.m_cachedLaserBarrier != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetLaserBarrier()).MethodHandle;
			}
			result = this.m_cachedLaserBarrier;
		}
		else
		{
			result = this.m_laserBarrier;
		}
		return result;
	}

	public int GetSweepDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetSweepDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_sweepDamageAmountMod.GetModifiedValue(this.m_sweepDamageAmount);
		}
		else
		{
			result = this.m_sweepDamageAmount;
		}
		return result;
	}

	public float GetSweepConeBackwardOffset()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetSweepConeBackwardOffset()).MethodHandle;
			}
			result = this.m_abilityMod.m_sweepConeBackwardOffsetMod.GetModifiedValue(this.m_sweepConeBackwardOffset);
		}
		else
		{
			result = this.m_sweepConeBackwardOffset;
		}
		return result;
	}

	public float GetMinConeAngle()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetMinConeAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_minConeAngleMod.GetModifiedValue(this.m_minConeAngle);
		}
		else
		{
			result = this.m_minConeAngle;
		}
		return result;
	}

	public float GetMaxConeAngle()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetMaxConeAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxConeAngleMod.GetModifiedValue(this.m_maxConeAngle);
		}
		else
		{
			result = this.m_maxConeAngle;
		}
		return result;
	}

	public int GetExtraDamagePerTurnAnchored()
	{
		return (!this.m_abilityMod) ? this.m_extraDamagePerTurnAnchored : this.m_abilityMod.m_extraDamagePerTurnAnchoredMod.GetModifiedValue(this.m_extraDamagePerTurnAnchored);
	}

	public int GetMaxExtraDamageForAnchored()
	{
		return (!this.m_abilityMod) ? this.m_maxExtraDamageForAnchored : this.m_abilityMod.m_maxExtraDamageForAnchoredMod.GetModifiedValue(this.m_maxExtraDamageForAnchored);
	}

	public float GetExtraDamageAtZeroDist()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetExtraDamageAtZeroDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageAtZeroDistMod.GetModifiedValue(this.m_extraDamageAtZeroDist);
		}
		else
		{
			result = this.m_extraDamageAtZeroDist;
		}
		return result;
	}

	public float GetExtraDamageChangePerDist()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetExtraDamageChangePerDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageChangePerDistMod.GetModifiedValue(this.m_extraDamageChangePerDist);
		}
		else
		{
			result = this.m_extraDamageChangePerDist;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnCaster()
	{
		return (this.m_cachedEffectOnCaster == null) ? this.m_effectOnCaster : this.m_cachedEffectOnCaster;
	}

	public int GetCooldownOnEnd()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetCooldownOnEnd()).MethodHandle;
			}
			result = this.m_abilityMod.m_cooldownOnEndMod.GetModifiedValue(this.m_cooldownOnEnd);
		}
		else
		{
			result = this.m_cooldownOnEnd;
		}
		return result;
	}

	public int GetAnchoredTechPointCost()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetAnchoredTechPointCost()).MethodHandle;
			}
			result = this.m_abilityMod.m_anchoredTechPointCostMod.GetModifiedValue(this.m_anchoredTechPointCost);
		}
		else
		{
			result = this.m_anchoredTechPointCost;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnAnchorEnd()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnAnchorEnd != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetEffectOnAnchorEnd()).MethodHandle;
			}
			result = this.m_cachedEffectOnAnchorEnd;
		}
		else
		{
			result = this.m_effectOnAnchorEnd;
		}
		return result;
	}

	public bool ShouldUpdateMovementOnAnchorChange()
	{
		List<StatusType> list = this.m_statusWhenAnchoredAndNotSweeping;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.ShouldUpdateMovementOnAnchorChange()).MethodHandle;
			}
			if (this.m_abilityMod.m_useStatusWhenAnchoredAndNotSweepingOverride)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				list = this.m_abilityMod.m_statusWhenAnchoredAndNotSweepingOverride;
			}
		}
		bool result;
		if (list != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			result = (list.Count > 0);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool HasPendingStatusTurnOfAnchorEnd(StatusType status)
	{
		List<StatusType> list = this.m_statusWhenAnchoredAndNotSweeping;
		if (this.m_abilityMod != null && this.m_abilityMod.m_useStatusWhenAnchoredAndNotSweepingOverride)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.HasPendingStatusTurnOfAnchorEnd(StatusType)).MethodHandle;
			}
			list = this.m_abilityMod.m_statusWhenAnchoredAndNotSweepingOverride;
		}
		if (list != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			return list.Contains(status);
		}
		return false;
	}

	public int GetTotalDamage(Vector3 startPos, Vector3 hitPos, int baseDamage, bool checkDurationBonus)
	{
		int num = baseDamage;
		if (this.m_syncComponent != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetTotalDamage(Vector3, Vector3, int, bool)).MethodHandle;
			}
			if (this.GetExtraDamagePerTurnAnchored() > 0)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (checkDurationBonus)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					int turnsAnchored = (int)this.m_syncComponent.m_turnsAnchored;
					int num2 = Mathf.Max(0, turnsAnchored) * this.GetExtraDamagePerTurnAnchored();
					if (this.GetMaxExtraDamageForAnchored() > 0)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						num2 = Mathf.Min(num2, this.GetMaxExtraDamageForAnchored());
					}
					num += num2;
				}
			}
			if (this.GetExtraDamageAtZeroDist() <= 0f)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.GetExtraDamageChangePerDist() <= 0f)
				{
					return num;
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			float num3 = this.GetExtraDamageAtZeroDist();
			float num4 = VectorUtils.HorizontalPlaneDistInSquares(startPos, hitPos) - 1.4f;
			if (num4 > 0f)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				num3 += this.GetExtraDamageChangePerDist() * num4;
			}
			num += Mathf.Max(0, Mathf.RoundToInt(num3));
		}
		return num;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetLaserDamageAmount());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		ActorData actorData = base.ActorData;
		int totalDamage;
		if (this.m_syncComponent != null && this.m_syncComponent.m_anchored)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			totalDamage = this.GetTotalDamage(actorData.GetTravelBoardSquareWorldPosition(), targetActor.GetTravelBoardSquareWorldPosition(), this.GetSweepDamageAmount(), true);
		}
		else
		{
			totalDamage = this.GetTotalDamage(actorData.GetTravelBoardSquareWorldPosition(), targetActor.GetTravelBoardSquareWorldPosition(), this.GetLaserDamageAmount(), false);
		}
		Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, totalDamage, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		return result;
	}

	public override int GetModdedCost()
	{
		if (this.m_syncComponent != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetModdedCost()).MethodHandle;
			}
			if (this.m_syncComponent.m_wasAnchoredOnTurnStart)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.GetAnchoredTechPointCost();
			}
		}
		return base.GetModdedCost();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (this.m_syncComponent != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetActionAnimType()).MethodHandle;
			}
			if (this.m_syncComponent.m_anchored)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.m_anchoredActionAnimType;
			}
		}
		return base.GetActionAnimType();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoAnchorLaser abilityMod_ExoAnchorLaser = modAsBase as AbilityMod_ExoAnchorLaser;
		int num;
		if (abilityMod_ExoAnchorLaser)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			num = (int)abilityMod_ExoAnchorLaser.m_maxConeAngleMod.GetModifiedValue(this.m_maxConeAngle);
		}
		else
		{
			num = (int)this.m_maxConeAngle;
		}
		int val = num;
		base.AddTokenInt(tokens, "Sweep_Angle", "max angle from the previous direction to sweep the laser", val, false);
		string name = "Damage_Laser";
		string empty = string.Empty;
		int val2;
		if (abilityMod_ExoAnchorLaser)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_ExoAnchorLaser.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			val2 = this.m_laserDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val2, false);
		StandardBarrierData standardBarrierData;
		if (abilityMod_ExoAnchorLaser)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			standardBarrierData = abilityMod_ExoAnchorLaser.m_laserBarrierMod.GetModifiedValue(this.m_laserBarrier);
		}
		else
		{
			standardBarrierData = this.m_laserBarrier;
		}
		StandardBarrierData standardBarrierData2 = standardBarrierData;
		standardBarrierData2.AddTooltipTokens(tokens, "Laser_Barrier", abilityMod_ExoAnchorLaser != null, this.m_laserBarrier);
		string name2 = "Damage_Sweep";
		string empty2 = string.Empty;
		int val3;
		if (abilityMod_ExoAnchorLaser)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			val3 = abilityMod_ExoAnchorLaser.m_sweepDamageAmountMod.GetModifiedValue(this.m_sweepDamageAmount);
		}
		else
		{
			val3 = this.m_sweepDamageAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val3, false);
		base.AddTokenInt(tokens, "ExtraDamagePerTurnAnchored", string.Empty, (!abilityMod_ExoAnchorLaser) ? this.m_extraDamagePerTurnAnchored : abilityMod_ExoAnchorLaser.m_extraDamagePerTurnAnchoredMod.GetModifiedValue(this.m_extraDamagePerTurnAnchored), false);
		string name3 = "MaxExtraDamageForAnchored";
		string empty3 = string.Empty;
		int val4;
		if (abilityMod_ExoAnchorLaser)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			val4 = abilityMod_ExoAnchorLaser.m_maxExtraDamageForAnchoredMod.GetModifiedValue(this.m_maxExtraDamageForAnchored);
		}
		else
		{
			val4 = this.m_maxExtraDamageForAnchored;
		}
		base.AddTokenInt(tokens, name3, empty3, val4, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_ExoAnchorLaser)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo = abilityMod_ExoAnchorLaser.m_effectOnCasterMod.GetModifiedValue(this.m_effectOnCaster);
		}
		else
		{
			effectInfo = this.m_effectOnCaster;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnCaster", this.m_effectOnCaster, true);
		string name4 = "Cooldown";
		string empty4 = string.Empty;
		int val5;
		if (abilityMod_ExoAnchorLaser)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			val5 = abilityMod_ExoAnchorLaser.m_cooldownOnEndMod.GetModifiedValue(this.m_cooldownOnEnd);
		}
		else
		{
			val5 = this.m_cooldownOnEnd;
		}
		base.AddTokenInt(tokens, name4, empty4, val5, false);
		string name5 = "AnchoredTechPointCost";
		string empty5 = string.Empty;
		int val6;
		if (abilityMod_ExoAnchorLaser)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			val6 = abilityMod_ExoAnchorLaser.m_anchoredTechPointCostMod.GetModifiedValue(this.m_anchoredTechPointCost);
		}
		else
		{
			val6 = this.m_anchoredTechPointCost;
		}
		base.AddTokenInt(tokens, name5, empty5, val6, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_ExoAnchorLaser)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo2 = abilityMod_ExoAnchorLaser.m_effectOnAnchorEndMod.GetModifiedValue(this.m_effectOnAnchorEnd);
		}
		else
		{
			effectInfo2 = this.m_effectOnAnchorEnd;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnAnchorEnd", this.m_effectOnAnchorEnd, true);
	}

	public override string GetFullTooltip()
	{
		if (this.m_syncComponent != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetFullTooltip()).MethodHandle;
			}
			if (this.m_syncComponent.m_anchored && !string.IsNullOrEmpty(this.m_anchoredToolTip))
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (string.IsNullOrEmpty(this.m_anchoredFinalFullTooltip))
				{
					return TooltipTokenEntry.GetTooltipWithSubstitutes(this.m_anchoredToolTip, null, false);
				}
				return TooltipTokenEntry.GetTooltipWithSubstitutes(this.m_anchoredFinalFullTooltip, null, false);
			}
		}
		return base.GetFullTooltip();
	}

	public override void SetUnlocalizedTooltipAndStatusTypes(AbilityMod mod = null)
	{
		if (!string.IsNullOrEmpty(this.m_anchoredToolTip))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.SetUnlocalizedTooltipAndStatusTypes(AbilityMod)).MethodHandle;
			}
			List<TooltipTokenEntry> tooltipTokenEntries = base.GetTooltipTokenEntries(mod);
			this.m_anchoredFinalFullTooltip = TooltipTokenEntry.GetTooltipWithSubstitutes(this.m_anchoredToolTip, tooltipTokenEntries, false);
		}
		base.SetUnlocalizedTooltipAndStatusTypes(mod);
	}

	public override bool HasPassivePendingStatus(StatusType status, ActorData owner)
	{
		if (this.m_syncComponent != null && this.m_syncComponent.m_wasAnchoredOnTurnStart)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.HasPassivePendingStatus(StatusType, ActorData)).MethodHandle;
			}
			if (owner != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!owner.GetAbilityData().HasQueuedAction(AbilityData.ActionType.ABILITY_4))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					return this.HasPendingStatusTurnOfAnchorEnd(status);
				}
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoAnchorLaser))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ExoAnchorLaser);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public float GetRotateToTargetDuration(float sweepAngle)
	{
		if (this.m_syncComponent != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.GetRotateToTargetDuration(float)).MethodHandle;
			}
			if (this.m_syncComponent.m_anchored)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_syncComponent.m_turnsAnchored > 0)
				{
					return sweepAngle / this.m_turnToTargetSweepDegreesPerSecond;
				}
			}
		}
		return 0.2f;
	}

	public override bool ShouldRotateToTargetPos()
	{
		if (this.m_syncComponent != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoAnchorLaser.ShouldRotateToTargetPos()).MethodHandle;
			}
			if (this.m_syncComponent.m_anchored)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_syncComponent.m_turnsAnchored > 0)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					return false;
				}
			}
		}
		return true;
	}
}
