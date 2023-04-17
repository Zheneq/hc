using System.Collections.Generic;
using UnityEngine;

public class ThiefSmokeBomb : Ability
{
	[Header("-- Bomb Damage")]
	public int m_extraDamageOnCast;
	[Header("-- Bomb Targeting (shape is in Smoke Field Info)")]
	public bool m_penetrateLos;
	public int m_maxAngleWithFirstSegment;
	public float m_maxDistanceWithFirst;
	public float m_minDistanceBetweenBombs = 1f;
	[Header("-- On Cast Hit Effect")]
	public StandardEffectInfo m_bombHitEffectInfo;
	[Header("-- Smoke Field")]
	public GroundEffectField m_smokeFieldInfo;
	[Header("-- Barrier (will make square out of 4 barriers around ground field)")]
	public bool m_addBarriers = true;
	public float m_barrierSquareWidth = 3f;
	public StandardBarrierData m_barrierData;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ThiefSmokeBomb m_abilityMod;
	private StandardEffectInfo m_cachedBombHitEffectInfo;
	private GroundEffectField m_cachedSmokeFieldInfo;
	private StandardBarrierData m_cachedBarrierData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Smoke Bomb";
		}
		if (m_barrierSquareWidth <= 0f)
		{
			Debug.LogWarning("Thief Smoke Bomb, Barrier Data has 0 width, setting to 3");
			m_barrierSquareWidth = 3f;
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		AbilityAreaShape shape = GetSmokeFieldInfo().shape;
		GroundEffectField fieldData = GetSmokeFieldInfo();
		if (GetExpectedNumberOfTargeters() > 1)
		{
			ClearTargeters();
			for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
			{
				AbilityUtil_Targeter_Shape targeter = new AbilityUtil_Targeter_Shape(
					this,
					shape,
					PenetrateLos(),
					AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
					true,
					fieldData.healAmount > 0 && !fieldData.ignoreNonCasterAllies)
				{
					m_affectCasterDelegate = (caster, actorsSoFar, casterInShape) =>
						fieldData.healAmount > 0 && casterInShape
				};
				targeter.SetTooltipSubjectTypes();
				Targeters.Add(targeter);
			}
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_Shape(
				this,
				shape,
				PenetrateLos(),
				AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
				true,
				fieldData.healAmount > 0 && !fieldData.ignoreNonCasterAllies)
			{
				m_affectCasterDelegate = (caster, actorsSoFar, casterInShape) =>
					fieldData.healAmount > 0 && casterInShape
			};
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, GetNumTargets());
	}

	private void SetCachedFields()
	{
		m_cachedBombHitEffectInfo = m_abilityMod != null
			? m_abilityMod.m_bombHitEffectInfoMod.GetModifiedValue(m_bombHitEffectInfo)
			: m_bombHitEffectInfo;
		m_cachedSmokeFieldInfo = m_abilityMod != null
			? m_abilityMod.m_smokeFieldInfoMod.GetModifiedValue(m_smokeFieldInfo)
			: m_smokeFieldInfo;
		m_cachedBarrierData = m_abilityMod != null
			? m_abilityMod.m_barrierDataMod.GetModifiedValue(m_barrierData)
			: m_barrierData;
	}

	public int GetExtraDamageOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageOnCastMod.GetModifiedValue(m_extraDamageOnCast)
			: m_extraDamageOnCast;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public int GetMaxAngleWithFirstSegment()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxAngleWithFirstSegmentMod.GetModifiedValue(m_maxAngleWithFirstSegment)
			: m_maxAngleWithFirstSegment;
	}

	// actually max distance from caster, but does not affect first bomb
	public float GetMaxDistanceWithFirst()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxDistanceWithFirstMod.GetModifiedValue(m_maxDistanceWithFirst)
			: m_maxDistanceWithFirst;
	}

	public float GetMinDistanceBetweenBombs()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minDistanceBetweenBombsMod.GetModifiedValue(m_minDistanceBetweenBombs)
			: m_minDistanceBetweenBombs;
	}

	public StandardEffectInfo GetBombHitEffectInfo()
	{
		return m_cachedBombHitEffectInfo ?? m_bombHitEffectInfo;
	}

	public GroundEffectField GetSmokeFieldInfo()
	{
		return m_cachedSmokeFieldInfo ?? m_smokeFieldInfo;
	}

	public bool AddBarriers()
	{
		return m_abilityMod != null
			? m_abilityMod.m_addBarriersMod.GetModifiedValue(m_addBarriers)
			: m_addBarriers;
	}

	public float GetBarrierSquareWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_barrierSquareWidthMod.GetModifiedValue(m_barrierSquareWidth)
			: m_barrierSquareWidth;
	}

	public StandardBarrierData GetBarrierData()
	{
		return m_cachedBarrierData ?? m_barrierData;
	}

	public override bool CustomTargetValidation(
		ActorData caster,
		AbilityTarget target,
		int targetIndex,
		List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null
		    || !targetSquare.IsValidForGameplay()
		    || (targetIndex == 0 && targetSquare == caster.GetCurrentBoardSquare()))
		{
			return false;
		}

		Vector3 targetPos = targetSquare.ToVector3();
		Vector3 firstSegEndPos = targetIndex <= 0
			? targetPos
			: Board.Get().GetSquare(currentTargets[0].GridPos).ToVector3();
		AbilityAreaShape shape = GetSmokeFieldInfo().shape;
		bool isValid = true;
		if (targetIndex > 0)
		{
			Vector3 to = targetPos - caster.GetFreePos();
			to.y = 0f;
			bool isValidAngle = true;
			if (GetMaxAngleWithFirstSegment() > 0)
			{
				BoardSquare firstTargetSquare = Board.Get().GetSquare(currentTargets[0].GridPos);
				Vector3 firstTargetPosAdjusted =
					AreaEffectUtils.GetCenterOfShape(shape, currentTargets[0].FreePos, firstTargetSquare);
				Vector3 firstTargetDir = firstTargetPosAdjusted - caster.GetFreePos();
				firstTargetDir.y = 0f;
				int angle = Mathf.RoundToInt(Vector3.Angle(firstTargetDir, to));
				isValidAngle = angle <= GetMaxAngleWithFirstSegment();
			}

			Vector3 targetPosAdjusted = AreaEffectUtils.GetCenterOfShape(shape, targetPos, targetSquare);
			Vector3 targetDir = targetPosAdjusted - caster.GetFreePos();
			targetDir.y = 0f;
			float distToTarget = targetDir.magnitude;
			bool isValidDist = GetMaxDistanceWithFirst() <= 0f
			                   || distToTarget <= GetMaxDistanceWithFirst() * Board.Get().squareSize;
			if (!isValidAngle || !isValidDist)
			{
				isValid = false;
			}
		}

		if (isValid)
		{
			float shapeCenterMinDistInWorld = 0.71f * Board.Get().squareSize;
			float minDistInWorld = GetMinDistanceBetweenBombs() * Board.Get().squareSize;
			for (int i = 0; i < targetIndex; i++)
			{
				if (!isValid)
				{
					break;
				}
				BoardSquare prevTargetSquare = Board.Get().GetSquare(currentTargets[i].GridPos);
				Vector3 prevTargetPosAdjusted = AreaEffectUtils.GetCenterOfShape(shape, currentTargets[i].FreePos, prevTargetSquare);
				isValid = CheckMinDistConstraint(prevTargetPosAdjusted, targetSquare, shape, shapeCenterMinDistInWorld, minDistInWorld);
			}
			int expectedNumberOfTargeters = GetExpectedNumberOfTargeters();
			if (isValid && targetIndex < expectedNumberOfTargeters - 1)
			{
				List<AbilityTarget> list = new List<AbilityTarget>();
				for (int j = 0; j < expectedNumberOfTargeters; j++)
				{
					list.Add(target.GetCopy());
				}
				for (int k = 0; k < targetIndex; k++)
				{
					list[k].SetPosAndDir(currentTargets[k].GridPos, currentTargets[k].FreePos, Vector3.forward);
				}
				list[targetIndex].SetPosAndDir(targetSquare.GetGridPos(), target.FreePos, Vector3.forward);
				float currentRangeInSquares = AbilityUtils.GetCurrentRangeInSquares(this, caster, 0);
				isValid = CanTargetFutureClicks(
					caster,
					firstSegEndPos,
					targetIndex,
					list,
					targetIndex,
					expectedNumberOfTargeters,
					currentRangeInSquares);
			}
		}
		return isValid;
	}

	public bool CanTargetFutureClicks(
		ActorData caster,
		Vector3 firstSegEndPos,
		int lastSelectedTargetIndex,
		List<AbilityTarget> targetEntries,
		int numTargetsFromPlayerInput,
		int numClicks,
		float abilityMaxRange)
	{
		if (lastSelectedTargetIndex >= numClicks - 1)
		{
			return true;
		}
		Vector3 vec = firstSegEndPos - caster.GetFreePos();
		float coneWidthDegrees = Mathf.Min(360f, 2f * GetMaxAngleWithFirstSegment() + 25f);
		AreaEffectUtils.GetMaxConeBounds(
			caster.GetFreePos(),
			VectorUtils.HorizontalAngle_Deg(vec),
			coneWidthDegrees,
			abilityMaxRange,
			0f,
			out int minX,
			out int maxX,
			out int minY,
			out int maxY);
		AbilityAreaShape shape = GetSmokeFieldInfo().shape;
		AbilityData abilityData = caster.GetAbilityData();
		BoardSquare casterSquare = caster.GetCurrentBoardSquare();
		float shapeCenterMinDistInWorld = 0.71f * Board.Get().squareSize;
		float minDistInWorld = GetMinDistanceBetweenBombs() * Board.Get().squareSize;
		bool isValid = false;
		for (int i = minX; i < maxX; i++)
		{
			if (isValid)
			{
				break;
			}
			for (int j = minY; j < maxY; j++)
			{
				if (isValid)
				{
					break;
				}
				BoardSquare square = Board.Get().GetSquareFromIndex(i, j);
				if (square == null
				    || !square.IsValidForGameplay()
				    || !casterSquare.GetLOS(square.x, square.y)
				    || !abilityData.IsTargetSquareInRangeOfAbilityFromSquare(square, casterSquare, abilityMaxRange, 0f))
				{
					continue;
				}
				Vector3 pos = square.ToVector3();
				bool isValidSquare = true;
				bool isValidAngle = true;
				int maxAngleWithFirstSegment = GetMaxAngleWithFirstSegment();
				if (maxAngleWithFirstSegment > 0)
				{
					BoardSquare firstTargetSquare = Board.Get().GetSquare(targetEntries[0].GridPos);
					if (numTargetsFromPlayerInput > 0)
					{
						Vector3 firstTargetPosAdjusted = AreaEffectUtils.GetCenterOfShape(shape, targetEntries[0].FreePos, firstTargetSquare);
						Vector3 firstTargetPosDir = firstTargetPosAdjusted - caster.GetFreePos();
						Vector3 posDir = pos - caster.GetFreePos();
						int angle = Mathf.RoundToInt(Vector3.Angle(firstTargetPosDir, posDir));
						isValidAngle = angle <= maxAngleWithFirstSegment;
					}
					else
					{
						for (int k = 0; k < 4; k++)
						{
							if (!isValidSquare)
							{
								break;
							}

							Vector3 firstTargetPosWithOffset = firstTargetSquare.ToVector3() + 0.1f * VectorUtils.AngleDegreesToVector(45f + k * 90f);
							Vector3 firstTargetPosWithOffsetAdjusted = AreaEffectUtils.GetCenterOfShape(shape, firstTargetPosWithOffset, firstTargetSquare);
							Vector3 firstTargetPosWithOffsetDir = firstTargetPosWithOffsetAdjusted - caster.GetFreePos();
							Vector3 posDir = pos - caster.GetFreePos();
							int angle = Mathf.RoundToInt(Vector3.Angle(firstTargetPosWithOffsetDir, posDir));
							isValidAngle &= angle <= maxAngleWithFirstSegment;
						}
					}
				}

				Vector3 posAdjusted = AreaEffectUtils.GetCenterOfShape(shape, pos, square);
				Vector3 posAdjustedDir = posAdjusted - caster.GetFreePos();
				posAdjustedDir.y = 0f;
				float posAdjustedDist = posAdjustedDir.magnitude;
				bool isValidDist = GetMaxDistanceWithFirst() <= 0f
				                   || posAdjustedDist <= GetMaxDistanceWithFirst() * Board.Get().squareSize;
				if (!isValidAngle || !isValidDist)
				{
					isValidSquare = false;
				}

				if (isValidSquare)
				{
					for (int k = 0; k <= lastSelectedTargetIndex; k++)
					{
						if (!isValidSquare)
						{
							break;
						}

						BoardSquare prevTargetSquare = Board.Get().GetSquare(targetEntries[k].GridPos);
						if (prevTargetSquare == square)
						{
							isValidSquare = false;
						}
						else if (k < numTargetsFromPlayerInput)
						{
							Vector3 prevTargetPos = targetEntries[k].FreePos;
							Vector3 prevTargetPosAdjusted = AreaEffectUtils.GetCenterOfShape(shape, prevTargetPos, prevTargetSquare);
							isValidSquare = CheckMinDistConstraint(prevTargetPosAdjusted, square, shape, shapeCenterMinDistInWorld, minDistInWorld);
						}
						else
						{
							for (int l = 0; l < 4; l++)
							{
								if (!isValidSquare)
								{
									break;
								}

								Vector3 prevTargetPosWithOffset = prevTargetSquare.ToVector3() + 0.1f * VectorUtils.AngleDegreesToVector(45f + l * 90f);
								Vector3 prevTargetPosWithOffsetAdjusted = AreaEffectUtils.GetCenterOfShape(shape, prevTargetPosWithOffset, prevTargetSquare);
								isValidSquare = CheckMinDistConstraint(prevTargetPosWithOffsetAdjusted, square, shape, shapeCenterMinDistInWorld, minDistInWorld);
							}
						}
					}
				}

				if (isValidSquare && lastSelectedTargetIndex < numClicks - 1)
				{
					targetEntries[lastSelectedTargetIndex + 1].SetPosAndDir(square.GetGridPos(), pos, Vector3.forward);
					isValidSquare = CanTargetFutureClicks(
						caster,
						firstSegEndPos,
						lastSelectedTargetIndex + 1,
						targetEntries,
						numTargetsFromPlayerInput,
						numClicks,
						abilityMaxRange);
				}

				isValid = isValidSquare;
			}
		}
		return isValid;
	}

	private bool CheckMinDistConstraint(
		Vector3 centerOfShapePrev,
		BoardSquare candidateSquare,
		AbilityAreaShape fieldShape,
		float shapeCenterMinDistInWorld,
		float minDistInWorld)
	{
		Vector3 pos = candidateSquare.ToVector3();
		
		for (int i = 0; i < 4; i++)
		{
			Vector3 posWithOffset = pos + 0.1f * VectorUtils.AngleDegreesToVector(45f + i * 90f);
			Vector3 posWithOffsetAdjusted = AreaEffectUtils.GetCenterOfShape(fieldShape, posWithOffset, candidateSquare);
			Vector3 posWithOffsetAdjustedVector = posWithOffsetAdjusted - centerOfShapePrev;
			posWithOffsetAdjustedVector.y = 0f;
			float dist = posWithOffsetAdjustedVector.magnitude;
			if (dist < shapeCenterMinDistInWorld
			    || (minDistInWorld > 0f && dist < minDistInWorld))
			{
				return false;
			}
		}
		return true;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, 1);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetSmokeFieldInfo().healAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetSmokeFieldInfo().healAmount);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		BoardSquare firstTargetSquare = Board.Get().GetSquare(Targeters[0].LastUpdatingGridPos);
		int damageAmount = GetSmokeFieldInfo().damageAmount;
		int subsequentDamageAmount = GetSmokeFieldInfo().subsequentDamageAmount;
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			if (i > 0)
			{
				BoardSquare targetSquare = Board.Get().GetSquare(Targeters[i].LastUpdatingGridPos);
				if (targetSquare == null || targetSquare == firstTargetSquare)
				{
					continue;
				}
			}
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeters[i],
				targetActor,
				currentTargeterIndex,
				damageAmount + GetExtraDamageOnCast(),
				subsequentDamageAmount);
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefSmokeBomb abilityMod_ThiefSmokeBomb = modAsBase as AbilityMod_ThiefSmokeBomb;
		AddTokenInt(tokens, "ExtraDamageOnCast", string.Empty, abilityMod_ThiefSmokeBomb != null
			? abilityMod_ThiefSmokeBomb.m_extraDamageOnCastMod.GetModifiedValue(m_extraDamageOnCast)
			: m_extraDamageOnCast);
		AddTokenInt(tokens, "MaxAngleWithFirstSegment", string.Empty, abilityMod_ThiefSmokeBomb != null
			? abilityMod_ThiefSmokeBomb.m_maxAngleWithFirstSegmentMod.GetModifiedValue(m_maxAngleWithFirstSegment)
			: m_maxAngleWithFirstSegment);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ThiefSmokeBomb != null
			? abilityMod_ThiefSmokeBomb.m_bombHitEffectInfoMod.GetModifiedValue(m_bombHitEffectInfo)
			: m_bombHitEffectInfo, "BombHitEffectInfo", m_bombHitEffectInfo);
		StandardBarrierData barrierData = abilityMod_ThiefSmokeBomb != null
			? abilityMod_ThiefSmokeBomb.m_barrierDataMod.GetModifiedValue(m_barrierData)
			: m_barrierData;
		barrierData.AddTooltipTokens(tokens, "BarrierData", abilityMod_ThiefSmokeBomb != null, m_barrierData);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefSmokeBomb))
		{
			m_abilityMod = abilityMod as AbilityMod_ThiefSmokeBomb;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
