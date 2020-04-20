using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackerTeslaPrison : TrackerDrone
{
	[Header("-- Wall Config and Barrier Data")]
	public TrackerTeslaPrison.PrisonWallSegmentType m_wallSegmentType = TrackerTeslaPrison.PrisonWallSegmentType.SquareMadeOfCornersAndMidsection;

	public bool m_targeterPenetrateLos = true;

	public StandardBarrierData m_prisonBarrierData;

	[Header("-- If Wall Segement Type is Square Made Of Corners and Midsection, Dimentions")]
	public int m_squareCornerLength = 1;

	public int m_squareMidsectionLength = 1;

	[Header("-- If Wall Segment Type is Regular Polygon")]
	public int m_prisonSides = 8;

	public float m_prisonRadius = 2f;

	[Header("-- Move Drone")]
	public bool m_moveDrone = true;

	[Header("-- Additional Effect to enemies in shape --")]
	public AbilityAreaShape m_additionalEffectShape = AbilityAreaShape.Three_x_Three;

	public StandardEffectInfo m_additionalEffectOnEnemiesInShape;

	[Header("-- Sequences -------------------------------------------------")]
	public bool m_createCastSequenceIfMovingDrone;

	public GameObject m_castSequencePrefab;

	private AbilityMod_TrackerTeslaPrison m_ultAbilityMod;

	private StandardBarrierData m_cachedBarrierData;

	private StandardEffectInfo m_cachedAdditionalEffectOnEnemiesInShape;

	protected override bool UseAltMovement()
	{
		return true;
	}

	public AbilityMod_TrackerTeslaPrison GetUltMod()
	{
		return this.m_ultAbilityMod;
	}

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTeslaPrison.Start()).MethodHandle;
			}
			this.m_abilityName = "Tesla Prison";
		}
		if (this.m_prisonSides < 3)
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
			this.m_prisonSides = 4;
		}
		if (this.m_squareCornerLength <= 0)
		{
			this.m_squareCornerLength = 1;
		}
		if (this.m_squareMidsectionLength < 0)
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
			this.m_squareMidsectionLength = 0;
		}
		this.m_droneTracker = base.GetComponent<TrackerDroneTrackerComponent>();
		if (this.m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
			this.m_moveDrone = false;
		}
		this.m_visionProvider = base.GetComponent<ActorAdditionalVisionProviders>();
		if (this.m_visionProvider == null)
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
			Debug.LogError("No additional vision provider component");
		}
		this.Setup();
		base.ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		this.SetCachedFields();
		if (this.m_droneInfoComp == null)
		{
			this.m_droneInfoComp = base.GetComponent<TrackerDroneInfoComponent>();
		}
		if (this.m_droneInfoComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTeslaPrison.Setup()).MethodHandle;
			}
			Debug.LogError("No Drone Info component");
		}
		if (this.m_droneTracker == null)
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
			this.m_droneTracker = base.GetComponent<TrackerDroneTrackerComponent>();
		}
		if (this.m_droneTracker == null)
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
			Debug.LogError("No drone tracker component");
			this.m_moveDrone = false;
		}
		if (this.m_moveDrone)
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
			bool flag;
			if (!this.m_droneInfoComp.GetUntrackedHitEffect().m_applyEffect)
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
				flag = (this.m_droneInfoComp.GetDamageOnUntracked(true) > 0);
			}
			else
			{
				flag = true;
			}
			bool hitUntrackedTargets = flag;
			base.Targeter = new AbilityUtil_Targeter_TeslaPrison(this, this.m_wallSegmentType, this.m_squareCornerLength, this.m_squareMidsectionLength, this.m_prisonSides, this.m_prisonRadius, this.m_droneTracker, this.m_droneInfoComp.m_travelTargeterEndRadius, this.m_droneInfoComp.m_travelTargeterEndRadius, this.m_droneInfoComp.m_travelTargeterLineRadius, -1, false, this.m_droneInfoComp.m_targetingIgnoreLos, this.m_droneInfoComp.m_droneTravelHitTargets, hitUntrackedTargets);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_TeslaPrison(this, this.m_wallSegmentType, this.m_squareCornerLength, this.m_squareMidsectionLength, this.m_prisonSides, this.m_prisonRadius, AbilityAreaShape.SingleSquare, false);
		}
	}

	private void SetCachedFields()
	{
		StandardBarrierData cachedBarrierData;
		if (this.m_ultAbilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTeslaPrison.SetCachedFields()).MethodHandle;
			}
			cachedBarrierData = this.m_ultAbilityMod.m_barrierDataMod.GetModifiedValue(this.m_prisonBarrierData);
		}
		else
		{
			cachedBarrierData = this.m_prisonBarrierData;
		}
		this.m_cachedBarrierData = cachedBarrierData;
		StandardEffectInfo cachedAdditionalEffectOnEnemiesInShape;
		if (this.m_ultAbilityMod)
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
			cachedAdditionalEffectOnEnemiesInShape = this.m_ultAbilityMod.m_additionalEffectOnEnemiesInShapeMod.GetModifiedValue(this.m_additionalEffectOnEnemiesInShape);
		}
		else
		{
			cachedAdditionalEffectOnEnemiesInShape = this.m_additionalEffectOnEnemiesInShape;
		}
		this.m_cachedAdditionalEffectOnEnemiesInShape = cachedAdditionalEffectOnEnemiesInShape;
	}

	private StandardBarrierData GetPrisonBarrierData()
	{
		StandardBarrierData result;
		if (this.m_ultAbilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTeslaPrison.GetPrisonBarrierData()).MethodHandle;
			}
			result = this.m_prisonBarrierData;
		}
		else
		{
			result = this.m_cachedBarrierData;
		}
		return result;
	}

	private StandardEffectInfo GetAdditionalEffectOnEnemiesInShape()
	{
		return (this.m_cachedAdditionalEffectOnEnemiesInShape == null) ? this.m_additionalEffectOnEnemiesInShape : this.m_cachedAdditionalEffectOnEnemiesInShape;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerTeslaPrison abilityMod_TrackerTeslaPrison = modAsBase as AbilityMod_TrackerTeslaPrison;
		StandardBarrierData standardBarrierData;
		if (abilityMod_TrackerTeslaPrison)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTeslaPrison.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			standardBarrierData = abilityMod_TrackerTeslaPrison.m_barrierDataMod.GetModifiedValue(this.m_prisonBarrierData);
		}
		else
		{
			standardBarrierData = this.m_prisonBarrierData;
		}
		StandardBarrierData standardBarrierData2 = standardBarrierData;
		standardBarrierData2.AddTooltipTokens(tokens, "Wall", false, null);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_droneInfoComp != null)
		{
			if (this.m_moveDrone)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTeslaPrison.CalculateAbilityTooltipNumbers()).MethodHandle;
				}
				if (this.m_droneInfoComp != null)
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
					AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_droneInfoComp.GetDamageOnTracked(true));
					this.m_droneInfoComp.m_droneHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
					AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_droneInfoComp.GetDamageOnUntracked(true));
					this.m_droneInfoComp.m_untrackedDroneHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
				}
			}
			this.m_prisonBarrierData.m_onEnemyMovedThrough.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Quaternary);
			this.m_prisonBarrierData.m_onAllyMovedThrough.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = this.CalculateAbilityTooltipNumbers();
		int i = 0;
		while (i < list.Count)
		{
			AbilityTooltipNumber abilityTooltipNumber = list[i];
			if (abilityTooltipNumber.m_subject != AbilityTooltipSubject.Primary)
			{
				goto IL_66;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTeslaPrison.CalculateNameplateTargetingNumbers()).MethodHandle;
			}
			if (abilityTooltipNumber.m_symbol != AbilityTooltipSymbol.Damage)
			{
				goto IL_66;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			list[i].m_value = this.m_droneInfoComp.GetDamageOnTracked(true);
			IL_9D:
			i++;
			continue;
			IL_66:
			if (abilityTooltipNumber.m_subject == AbilityTooltipSubject.Secondary && abilityTooltipNumber.m_symbol == AbilityTooltipSymbol.Damage)
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
				list[i].m_value = this.m_droneInfoComp.GetDamageOnUntracked(true);
				goto IL_9D;
			}
			goto IL_9D;
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		return list;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!this.m_moveDrone)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTeslaPrison.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			return true;
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null && boardSquareSafe.IsBaselineHeight())
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
			if (caster.GetCurrentBoardSquare() != null)
			{
				float num = this.m_droneInfoComp.m_targeterMaxRangeFromDrone * Board.Get().squareSize;
				float num2 = this.m_droneInfoComp.GetTargeterMaxRangeFromCaster(true) * Board.Get().squareSize;
				Vector3 b = caster.GetTravelBoardSquareWorldPosition();
				if (this.m_droneTracker.DroneIsActive())
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
					BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_droneTracker.BoardX(), this.m_droneTracker.BoardY());
					if (boardSquare != null)
					{
						if (boardSquare == boardSquareSafe)
						{
							return false;
						}
						b = boardSquare.ToVector3();
					}
				}
				if (num > 0f)
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
					if (Vector3.Distance(boardSquareSafe.ToVector3(), b) > num)
					{
						return false;
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				bool result;
				if (num2 > 0f)
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
					result = (Vector3.Distance(boardSquareSafe.ToVector3(), caster.GetCurrentBoardSquare().ToVector3()) <= num2);
				}
				else
				{
					result = true;
				}
				return result;
			}
		}
		return false;
	}

	public override List<int> \u001D()
	{
		List<int> list = base.\u001D();
		if (this.m_prisonBarrierData != null)
		{
			list.Add(this.m_prisonBarrierData.m_onEnemyMovedThrough.m_damage);
		}
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TrackerTeslaPrison))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTeslaPrison.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_ultAbilityMod = (abilityMod as AbilityMod_TrackerTeslaPrison);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_ultAbilityMod = null;
		this.Setup();
	}

	public enum PrisonWallSegmentType
	{
		RegularPolygon,
		SquareMadeOfCornersAndMidsection
	}
}
