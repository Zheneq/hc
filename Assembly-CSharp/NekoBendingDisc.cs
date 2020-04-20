using System;
using System.Collections.Generic;
using UnityEngine;

public class NekoBendingDisc : Ability
{
	[Header("Targeting")]
	public float m_laserWidth = 1f;

	public float m_minRangeBeforeBend = 1f;

	public float m_maxRangeBeforeBend = 5.5f;

	public float m_maxTotalRange = 7.5f;

	public float m_maxBendAngle = 45f;

	public int m_maxTargets;

	public bool m_startTargeterFadeAtActorRadius = true;

	[Header("Damage stuff")]
	public int m_directDamage = 0x19;

	public int m_returnTripDamage = 0xA;

	public bool m_returnTripIgnoreCover = true;

	[Header("Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_returnTripSequencePrefab;

	public GameObject m_persistentDiscSequencePrefab;

	private Neko_SyncComponent m_syncComp;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Bending Boomerang Disc";
		}
		this.m_syncComp = base.GetComponent<Neko_SyncComponent>();
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeters.Clear();
		for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = new AbilityUtil_Targeter_BendingLaser(this, this.GetLaserWidth(), this.GetMinRangeBeforeBend(), this.GetMaxRangeBeforeBend(), this.GetMaxTotalRange(), this.GetMaxBendAngle(), false, this.GetMaxTargets(), false, false);
			abilityUtil_Targeter_BendingLaser.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_BendingLaser.m_startFadeAtActorRadius = this.m_startTargeterFadeAtActorRadius;
			base.Targeters.Add(abilityUtil_Targeter_BendingLaser);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (!base.Targeters.IsNullOrEmpty<AbilityUtil_Targeter>())
		{
			AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = base.Targeters[0] as AbilityUtil_Targeter_BendingLaser;
			if (abilityUtil_Targeter_BendingLaser.DidStopShort())
			{
				return 1;
			}
		}
		return 2;
	}

	public override bool ShouldAutoConfirmIfTargetingOnEndTurn()
	{
		return true;
	}

	private Vector3 GetTargeterClampedAimDirection(Vector3 aimDir, List<AbilityTarget> targets)
	{
		aimDir.y = 0f;
		aimDir.Normalize();
		float maxBendAngle = this.GetMaxBendAngle();
		Vector3 aimDirection = targets[0].AimDirection;
		if (maxBendAngle > 0f)
		{
			if (maxBendAngle < 360f)
			{
				aimDir = Vector3.RotateTowards(aimDirection, aimDir, 0.0174532924f * maxBendAngle, 0f);
			}
		}
		return aimDir;
	}

	private float GetClampedRangeInSquares(ActorData targetingActor, AbilityTarget currentTarget)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float magnitude = (currentTarget.FreePos - travelBoardSquareWorldPositionForLos).magnitude;
		if (magnitude < this.GetMinRangeBeforeBend() * Board.Get().squareSize)
		{
			return this.GetMinRangeBeforeBend();
		}
		if (magnitude > this.GetMaxRangeBeforeBend() * Board.Get().squareSize)
		{
			return this.GetMaxRangeBeforeBend();
		}
		return magnitude / Board.Get().squareSize;
	}

	private float GetDistanceRemaining(ActorData targetingActor, AbilityTarget previousTarget, out Vector3 bendPos)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float clampedRangeInSquares = this.GetClampedRangeInSquares(targetingActor, previousTarget);
		bendPos = travelBoardSquareWorldPositionForLos + previousTarget.AimDirection * clampedRangeInSquares * Board.Get().squareSize;
		return this.GetMaxTotalRange() - clampedRangeInSquares;
	}

	public float GetMinRangeBeforeBend()
	{
		return this.m_minRangeBeforeBend;
	}

	public float GetMaxRangeBeforeBend()
	{
		return this.m_maxRangeBeforeBend;
	}

	public float GetMaxTotalRange()
	{
		return this.m_maxTotalRange;
	}

	public float GetMaxBendAngle()
	{
		return this.m_maxBendAngle;
	}

	public float GetLaserWidth()
	{
		return this.m_laserWidth;
	}

	public int GetMaxTargets()
	{
		return this.m_maxTargets;
	}

	public int GetDirectDamage()
	{
		return this.m_directDamage;
	}

	public int GetReturnTripDamage()
	{
		return this.m_returnTripDamage;
	}

	public bool GetReturnTripIgnoresCover()
	{
		return this.m_returnTripIgnoreCover;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "DirectDamage", string.Empty, this.m_directDamage, false);
		base.AddTokenInt(tokens, "ReturnTripDamage", string.Empty, this.m_returnTripDamage, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_directDamage),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, this.m_returnTripDamage)
		};
	}
}
