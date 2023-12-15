// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

// TODO TITUS check rogues code
public class ClaymoreCharge : Ability
{
	[Header("-- Charge Targeting")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;
	public float m_width = 1f;
	public float m_maxRange = 10f;
	[Header("-- Normal On Hit Damage, Effect, etc")]
	public int m_directHitDamage = 20;
	public StandardEffectInfo m_directEnemyHitEffect;
	public bool m_directHitIgnoreCover = true;
	// added in rogues
	// public OnHitAuthoredData OnDirectHitData;
	[Space(10f)]
	public int m_aoeDamage = 10;
	public StandardEffectInfo m_aoeEnemyHitEffect;
	// added in rogues
	// public OnHitAuthoredData OnAoEHitData;
	[Header("-- Extra Damage from Charge Path Length")]
	public int m_extraDirectHitDamagePerSquare;
	[Header("-- Heal On Self")]
	public int m_healOnSelfPerTargetHit;
	[Header("-- Other On Hit Config")]
	public int m_cooldownOnHit = -1;
	public bool m_chaseHitActor;
	[Header("-- Charge Anim")]
	[Tooltip("Whether to set up charge like battlemonk charge with pivots and recovery")]
	public bool m_chargeWithPivotAndRecovery;
	[Tooltip("Only relevant if using pivot and recovery charge setup")]
	public float m_recoveryTime = 0.5f;
	[Header("-- Sequences")]
	public GameObject m_chargeSequencePrefab;
	public GameObject m_aoeHitSequencePrefab;

	private const int c_maxBounces = 0;
	private const int c_maxTargetsHit = 1;
	private const bool c_penetrateLoS = false;

	private AbilityMod_ClaymoreCharge m_abilityMod;
	
	// removed in rogues
	private Claymore_SyncComponent m_syncComp;
	
	private StandardEffectInfo m_cachedDirectEnemyHitEffect;
	private StandardEffectInfo m_cachedAoeEnemyHitEffect;
#if SERVER
	// added in rogues
	private Passive_Claymore m_passive;
#endif

	// rogues
	// private OnHitAuthoredData m_cachedDirectOnHitData
	// {
	// 	get
	// 	{
	// 		if (!m_abilityMod)
	// 		{
	// 			return this.OnDirectHitData;
	// 		}
	// 		return m_abilityMod.m_directHitDataMod.GetModdedOnHitData(this.OnDirectHitData);
	// 	}
	// }

	// rogues
	// private OnHitAuthoredData m_cachedAoEOnHitData
	// {
	// 	get
	// 	{
	// 		if (!m_abilityMod)
	// 		{
	// 			return this.OnAoEHitData;
	// 		}
	// 		return m_abilityMod.m_aoeHitDataMod.GetModdedOnHitData(this.OnAoEHitData);
	// 	}
	// }

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Berserker Charge";
		}
#if SERVER
		// added in rogues
		m_passive = GetComponent<PassiveData>().GetPassiveOfType(typeof(Passive_Claymore)) as Passive_Claymore;
		// rogues?
		// m_abilityMod = (TalentManager.Get().GetAbilityMod(CharacterType.Claymore, CachedActionType) as AbilityMod_ClaymoreCharge);
#endif
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		// removed in rogues
		m_syncComp = GetComponent<Claymore_SyncComponent>();
		
		SetCachedFields();
		AbilityUtil_Targeter_ClaymoreCharge targeter = new AbilityUtil_Targeter_ClaymoreCharge(
			this,
			GetChargeWidth(),
			GetChargeRange(),
			GetAoeShape(),
			DirectHitIgnoreCover());
		if (GetHealOnSelfPerTargetHit() > 0)
		{
			targeter.m_affectCasterDelegate = (ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0;
		}
		Targeter = targeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetChargeRange();
	}

	private void SetCachedFields()
	{
		m_cachedDirectEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_directEnemyHitEffectMod.GetModifiedValue(m_directEnemyHitEffect)
			: m_directEnemyHitEffect;
		m_cachedAoeEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_aoeEnemyHitEffectMod.GetModifiedValue(m_aoeEnemyHitEffect)
			: m_aoeEnemyHitEffect;
	}

	public AbilityAreaShape GetAoeShape()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_aoeShapeMod.GetModifiedValue(m_aoeShape) 
			: m_aoeShape;
	}

	public float GetChargeWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_widthMod.GetModifiedValue(m_width)
			: m_width;
	}

	public float GetChargeRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxRangeMod.GetModifiedValue(m_maxRange)
			: m_maxRange;
	}

	public bool DirectHitIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_directHitIgnoreCoverMod.GetModifiedValue(m_directHitIgnoreCover)
			: m_directHitIgnoreCover;
	}

	public int GetDirectHitDamage()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_directHitDamageMod.GetModifiedValue(m_directHitDamage) 
			: m_directHitDamage;
	}

	public StandardEffectInfo GetDirectEnemyHitEffect()
	{
		return m_cachedDirectEnemyHitEffect ?? m_directEnemyHitEffect;
	}

	public int GetAoeDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeDamageMod.GetModifiedValue(m_aoeDamage)
			: m_aoeDamage;
	}

	public StandardEffectInfo GetAoeEnemyHitEffect()
	{
		return m_cachedAoeEnemyHitEffect ?? m_aoeEnemyHitEffect;
	}

	public int GetExtraDirectHitDamagePerSquare()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDirectHitDamagePerSquareMod.GetModifiedValue(m_extraDirectHitDamagePerSquare)
			: m_extraDirectHitDamagePerSquare;
	}

	public int GetHealOnSelfPerTargetHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_healOnSelfPerTargetHitMod.GetModifiedValue(m_healOnSelfPerTargetHit) 
			: m_healOnSelfPerTargetHit;
	}

	public int GetCooldownOnHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownOnHitMod.GetModifiedValue(m_cooldownOnHit)
			: m_cooldownOnHit;
	}

	public bool GetChaseHitActor()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_chaseHitActorMod.GetModifiedValue(m_chaseHitActor) 
			: m_chaseHitActor;
	}

	public int GetMaxTargets()
	{
		return 1;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		// rogues
		// m_cachedDirectOnHitData.AddTooltipTokens(tokens);
		// m_cachedAoEOnHitData.AddTooltipTokens(tokens);
		AbilityMod_ClaymoreCharge abilityMod_ClaymoreCharge = modAsBase as AbilityMod_ClaymoreCharge;
		AddTokenInt(tokens, "DirectHitDamage", string.Empty, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_directHitDamageMod.GetModifiedValue(m_directHitDamage)
			: m_directHitDamage);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_directEnemyHitEffectMod.GetModifiedValue(m_directEnemyHitEffect)
			: m_directEnemyHitEffect, "DirectEnemyHitEffect", m_directEnemyHitEffect);
		AddTokenInt(tokens, "AoeDamage", string.Empty, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_aoeDamageMod.GetModifiedValue(m_aoeDamage)
			: m_aoeDamage);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_aoeEnemyHitEffectMod.GetModifiedValue(m_aoeEnemyHitEffect)
			: m_aoeEnemyHitEffect, "AoeEnemyHitEffect", m_aoeEnemyHitEffect);
		AddTokenInt(tokens, "ExtraDirectHitDamagePerSquare", string.Empty, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_extraDirectHitDamagePerSquareMod.GetModifiedValue(m_extraDirectHitDamagePerSquare)
			: m_extraDirectHitDamagePerSquare);
		AddTokenInt(tokens, "HealOnSelfPerTargetHit", string.Empty, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_healOnSelfPerTargetHitMod.GetModifiedValue(m_healOnSelfPerTargetHit)
			: m_healOnSelfPerTargetHit);
		AddTokenInt(tokens, "CooldownOnHit", string.Empty, abilityMod_ClaymoreCharge != null
			? abilityMod_ClaymoreCharge.m_cooldownOnHitMod.GetModifiedValue(m_cooldownOnHit)
			: m_cooldownOnHit);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDirectHitDamage());
		GetDirectEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetAoeDamage());
		GetAoeEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealOnSelfPerTargetHit());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				int damage = GetDirectHitDamage();
				if (GetExtraDirectHitDamagePerSquare() > 0
				    && Targeter is AbilityUtil_Targeter_ClaymoreCharge)
				{
					AbilityUtil_Targeter_ClaymoreCharge targeter = Targeter as AbilityUtil_Targeter_ClaymoreCharge;
					int chargeDist = Mathf.Max(0, targeter.LastUpdatePathSquareCount - 1);
					damage += chargeDist * GetExtraDirectHitDamagePerSquare();
				}
				dictionary[AbilityTooltipSymbol.Damage] = damage;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetAoeDamage();
			}
			else if (GetHealOnSelfPerTargetHit() > 0 && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				dictionary[AbilityTooltipSymbol.Healing] = GetHealOnSelfPerTargetHit() * Mathf.Max(0, Targeter.GetActorsInRange().Count - 1);
			}
		}
		return dictionary;
	}

	// removed in rogues
	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return m_syncComp != null
			? m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, ActorData)
			: null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	// removed in rogues
	public static List<ActorData> GetActorsOnPath(BoardSquarePathInfo path, List<Team> relevantTeams, ActorData caster)
	{
		List<ActorData> list = new List<ActorData>();
		if (path == null)
		{
			return list;
		}
		for (BoardSquarePathInfo step = path; step != null; step = step.next)
		{
			ActorData occupantActor = step.square.OccupantActor;
			if (occupantActor != null
			    && AreaEffectUtils.IsActorTargetable(occupantActor, relevantTeams)
			    && (relevantTeams == null || relevantTeams.Contains(occupantActor.GetTeam())))
			{
				list.Add(occupantActor);
			}
		}
		return list;
	}

	// removed in rogues
	public static float GetMaxPotentialChargeDistance(
		Vector3 startPos,
		Vector3 endPos,
		Vector3 aimDir,
		float laserMaxDistInWorld,
		ActorData mover,
		out BoardSquare pathEndSquare)
	{
		float result = laserMaxDistInWorld;
		pathEndSquare = KnockbackUtils.GetLastValidBoardSquareInLine(
			startPos,
			endPos,
			false,
			false,
			laserMaxDistInWorld + 0.5f);
		BoardSquare boardSquare = Board.Get().GetSquareFromVec3(startPos);
		if (pathEndSquare != null && pathEndSquare != boardSquare)
		{
			Vector3 pointToProject = pathEndSquare.ToVector3();
			Vector3 projectionPoint = VectorUtils.GetProjectionPoint(aimDir, startPos, pointToProject);
			float num = (projectionPoint - startPos).magnitude + 0.5f;
			if (num < laserMaxDistInWorld)
			{
				result = num;
			}
		}
		else
		{
			result = 0.5f;
		}
		return result;
	}

	// removed in rogues
	public static BoardSquare GetTrimmedDestinationInPath(BoardSquarePathInfo chargePath, out bool differentFromInputDest)
	{
		differentFromInputDest = false;
		if (chargePath == null)
		{
			return null;
		}
		BoardSquarePathInfo boardSquarePathInfo = chargePath;
		BoardSquare result = boardSquarePathInfo.square;
		int num = 0;
		while (boardSquarePathInfo.next != null)
		{
			BoardSquare square = boardSquarePathInfo.next.square;
			if (boardSquarePathInfo.square.IsValidForKnockbackAndCharge()
			    && !square.IsValidForKnockbackAndCharge()
			    && num > 0)
			{
				result = boardSquarePathInfo.square;
				differentFromInputDest = true;
				break;
			}
			result = square;
			boardSquarePathInfo = boardSquarePathInfo.next;
			num++;
		}
		return result;
	}

	// removed in rogues
	public static BoardSquare GetChargeDestinationSquare(
		Vector3 startPos,
		Vector3 chargeDestPos,
		ActorData lastChargeHitActor,
		BoardSquare initialPathEndSquare,
		ActorData caster,
		bool trimBeforeFirstInvalid)
	{
		BoardSquare destination = null;
		if (lastChargeHitActor != null)
		{
			destination = lastChargeHitActor.GetCurrentBoardSquare();
		}
		else
		{
			if (initialPathEndSquare != null)
			{
				destination = initialPathEndSquare;
			}
			else
			{
				destination = KnockbackUtils.GetLastValidBoardSquareInLine(startPos, chargeDestPos, true);
			}
			BoardSquare start = Board.Get().GetSquareFromVec3(startPos);
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(
				caster,
				destination,
				start,
				true);
			if (boardSquarePathInfo != null && trimBeforeFirstInvalid)
			{
				destination = GetTrimmedDestinationInPath(boardSquarePathInfo, out bool _);
			}
		}
		return destination;
	}
	
#if SERVER
	// added in rogues
	public static BoardSquare GetClosestSquareToLaser(BoardSquare currentDest, Vector3 startPos, Vector3 endPos)
	{
		Vector3 vector = endPos - startPos;
		vector.y = 0f;
		vector.Normalize();
		List<BoardSquare> adjacentSquares = new List<BoardSquare>();
		Board.Get().GetAllAdjacentSquares(currentDest.x, currentDest.y, ref adjacentSquares);
		Vector3 vector2 = currentDest.ToVector3() - startPos;
		vector2.y = 0f;
		Vector3 vector3 = Vector3.Dot(vector2, vector) * vector + startPos;
		Vector3 vector4 = currentDest.ToVector3();
		vector4.y = vector3.y;
		float magnitude = (vector4 - vector3).magnitude;
		BoardSquare result = currentDest;
		foreach (BoardSquare square in adjacentSquares)
		{
			if (square.IsValidForGameplay())
			{
				Vector3 pos = square.ToVector3();
				pos.y = vector3.y;
				if ((pos - vector3).magnitude < magnitude)
				{
					result = square;
				}
			}
		}
		return result;
	}
#endif

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreCharge))
		{
			m_abilityMod = abilityMod as AbilityMod_ClaymoreCharge;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// added in rogues
	public override BoardSquare GetValidChargeTestSourceSquare(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		if (m_chargeWithPivotAndRecovery)
		{
			return chargeSegments[chargeSegments.Length - 1].m_pos;
		}
		return base.GetValidChargeTestSourceSquare(chargeSegments);
	}

	// added in rogues
	public override Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		if (m_chargeWithPivotAndRecovery)
		{
			return ServerEvadeUtils.GetChargeBestSquareTestDirection(chargeSegments);
		}
		return base.GetChargeBestSquareTestVector(chargeSegments);
	}

	// added in rogues
	public override bool GetChargeThroughInvalidSquares()
	{
		return true;
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		BoardSquare pathDestinationAndEndPoints = GetPathDestinationAndEndPoints(targets, caster, out Vector3 endPoint);
		if (m_chargeWithPivotAndRecovery)
		{
			ServerEvadeUtils.ChargeSegment[] chargeSegments = ServerEvadeUtils.GetChargeSegmentForStopOnTargetHit(
				caster, 
				new List<Vector3>
				{
					endPoint
				},
				pathDestinationAndEndPoints,
				m_recoveryTime);
			float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(chargeSegments));
			foreach (ServerEvadeUtils.ChargeSegment segment in chargeSegments)
			{
				if (segment.m_cycle == BoardSquarePathInfo.ChargeCycleType.Movement)
				{
					segment.m_segmentMovementSpeed = segmentMovementSpeed;
				}
			}
			return chargeSegments;
		}
		else
		{
			ServerEvadeUtils.ChargeSegment[] chargeSegments = new ServerEvadeUtils.ChargeSegment[2];
			chargeSegments[0] = new ServerEvadeUtils.ChargeSegment
			{
				m_pos = caster.GetCurrentBoardSquare(),
				m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
				m_end = BoardSquarePathInfo.ChargeEndType.Impact
			};
			chargeSegments[1] = new ServerEvadeUtils.ChargeSegment
			{
				m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
				m_pos = Board.Get().GetSquare(targets[0].GridPos)
			};
			Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
			List<ActorData> bounceHitActors = GetBounceHitActors(
				targets,
				loSCheckPos,
				caster, 
				out Vector3 _,
				null);
			chargeSegments[chargeSegments.Length - 1].m_end = bounceHitActors.Count > 0
				? BoardSquarePathInfo.ChargeEndType.Impact
				: BoardSquarePathInfo.ChargeEndType.Miss;
			chargeSegments[chargeSegments.Length - 1].m_pos = pathDestinationAndEndPoints;
			float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(chargeSegments));
			foreach (ServerEvadeUtils.ChargeSegment segment in chargeSegments)
			{
				if (segment.m_cycle == BoardSquarePathInfo.ChargeCycleType.Movement)
				{
					segment.m_segmentMovementSpeed = segmentMovementSpeed;
				}
			}
			return chargeSegments;
		}
	}

	// added in rogues
	public override BoardSquare GetIdealDestination(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return GetPathDestinationAndEndPoints(targets, caster, out Vector3 _);
	}

	// added in rogues
	private BoardSquare GetPathDestinationAndEndPoints(List<AbilityTarget> targets, ActorData caster, out Vector3 endPoint)
	{
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<ActorData> bounceHitActors = GetBounceHitActors(targets, loSCheckPos, caster, out Vector3 vector, null);
		float magnitude = (vector - loSCheckPos).magnitude;
		float num = Mathf.Min(0.5f, magnitude / 2f);
		Vector3 end = vector - targets[0].AimDirection * num;
		BoardSquare boardSquare;
		if (GetMaxTargets() > 0 && bounceHitActors.Count >= GetMaxTargets())
		{
			boardSquare = bounceHitActors[bounceHitActors.Count - 1].GetCurrentBoardSquare();
		}
		else
		{
			boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(loSCheckPos, end, true);
			boardSquare = GetClosestSquareToLaser(boardSquare, loSCheckPos, vector);
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(
				caster,
				boardSquare,
				caster.GetSquareAtPhaseStart(),
				true);
			if (boardSquarePathInfo != null && boardSquarePathInfo.next != null)
			{
				BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
				int num2 = 0;
				while (boardSquarePathInfo2.next != null)
				{
					BoardSquare square = boardSquarePathInfo2.next.square;
					if (boardSquarePathInfo2.square.IsValidForKnockbackAndCharge()
					    && !square.IsValidForKnockbackAndCharge()
					    && num2 > 0)
					{
						boardSquare = boardSquarePathInfo2.square;
						break;
					}
					boardSquarePathInfo2 = boardSquarePathInfo2.next;
					num2++;
				}
			}
		}
		endPoint = vector;
		return boardSquare;
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<ActorData> bounceHitActors = GetBounceHitActors(targets, loSCheckPos, caster, out Vector3 item, null);
		BouncingShotSequence.ExtraParams extraParams = new BouncingShotSequence.ExtraParams
		{
			laserTargets = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>()
		};
		foreach (ActorData key in bounceHitActors)
		{
			extraParams.laserTargets.Add(key, new AreaEffectUtils.BouncingLaserInfo(loSCheckPos, 0));
		}
		extraParams.segmentPts = new List<Vector3>
		{
			item
		};
		list.Add(new ServerClientUtils.SequenceStartData(
			m_chargeSequencePrefab,
			caster.GetCurrentBoardSquare(),
			new ActorData[0],
			caster,
			additionalData.m_sequenceSource,
			extraParams.ToArray()));
		if (bounceHitActors.Count > 0)
		{
			List<ActorData> hitActors = additionalData.m_abilityResults.HitActorList();
			foreach (ActorData actor in bounceHitActors)
			{
				hitActors.Remove(actor);
			}
			list.Add(new ServerClientUtils.SequenceStartData(
				m_aoeHitSequencePrefab,
				caster.GetFreePos(),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource));
		}
		return list;
	}

	// rogues?
	// public override void GetAbilityStatusData(out Dictionary<string, string> statusData, bool includeNames = false)
	// {
	// 	base.GetAbilityStatusData(out statusData);
	// 	base.GatherVisibleStatusTooltips(ref statusData, m_cachedDirectOnHitData.m_effectTemplateFields, includeNames);
	// 	base.GatherVisibleStatusTooltips(ref statusData, m_cachedDirectOnHitData.m_allyHitEffectTemplateFields, includeNames);
	// 	base.GatherVisibleStatusTooltips(ref statusData, m_cachedDirectOnHitData.m_enemyHitEffectTemplateFields, includeNames);
	// 	base.GatherVisibleStatusTooltips(ref statusData, m_cachedAoEOnHitData.m_effectTemplateFields, includeNames);
	// 	base.GatherVisibleStatusTooltips(ref statusData, m_cachedAoEOnHitData.m_allyHitEffectTemplateFields, includeNames);
	// 	base.GatherVisibleStatusTooltips(ref statusData, m_cachedAoEOnHitData.m_enemyHitEffectTemplateFields, includeNames);
	// }

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<ActorData> bounceHitActors = GetBounceHitActors(
			targets,
			loSCheckPos,
			caster,
			out Vector3 _,
			nonActorTargetInfo);
		List<ActorData> aoeHitActors;
		if (bounceHitActors.Count > 0)
		{
			aoeHitActors = GetAoeHitActors(caster, caster, nonActorTargetInfo);
			foreach (ActorData item in bounceHitActors)
			{
				aoeHitActors.Remove(item);
			}
		}
		else
		{
			aoeHitActors = new List<ActorData>();
		}
		bool appliedCooldown = false;
		bool appliedForceChase = false;
		bool appliedAddToStock = true;
		int numHitActors = 0;
		foreach (ActorData actorData in bounceHitActors)
		{
			Vector3 origin = DirectHitIgnoreCover() ? actorData.GetFreePos() : loSCheckPos;
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, origin));
			int damage = GetDirectHitDamage();
			if (GetExtraDirectHitDamagePerSquare() > 0)
			{
				int numSquaresInProcessedEvade = ServerActionBuffer.Get().GetNumSquaresInProcessedEvade(caster);
				int dist = Mathf.Max(0, numSquaresInProcessedEvade - 1);
				damage += dist * GetExtraDirectHitDamagePerSquare();
			}
			actorHitResults.SetBaseDamage(damage);
			actorHitResults.AddStandardEffectInfo(GetDirectEnemyHitEffect());
			if (GetCooldownOnHit() >= 0 && !appliedCooldown)
			{
				AbilityData.ActionType actionTypeOfAbility = caster.GetAbilityData().GetActionTypeOfAbility(this);
				actorHitResults.AddMiscHitEvent(new MiscHitEventData_OverrideCooldown(actionTypeOfAbility, GetCooldownOnHit()));
				appliedCooldown = true;
			}
			if (GetChaseHitActor() && !appliedForceChase)
			{
				actorHitResults.AddMiscHitEvent(new MiscHitEventData(MiscHitEventType.CasterForceChaseTarget));
				appliedForceChase = true;
			}
			if (!appliedAddToStock)
			{
				actorHitResults.AddMiscHitEvent(new MiscHitEventData_AddToCasterStock(m_passive.GetHitIndicatorActionType(), bounceHitActors.Count + aoeHitActors.Count));
				appliedAddToStock = true;
			}
			ActorHitContext actorContext = new ActorHitContext();
			ContextVars abilityContext = new ContextVars();
			NumericHitResultScratch numericHitResultScratch = new NumericHitResultScratch();
			if (actorData.GetTeam() == caster.GetTeam())
			{
				// TODO TITUS ability results
				// GenericAbility_Container.CalcIntFieldValues(actorData, caster, actorContext, abilityContext, m_cachedDirectOnHitData.m_allyHitIntFields, numericHitResultScratch);
				// GenericAbility_Container.SetNumericFieldsOnHitResults(actorHitResults, numericHitResultScratch);
				// GenericAbility_Container.SetKnockbackFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedDirectOnHitData.m_allyHitKnockbackFields);
				// GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedDirectOnHitData.m_allyHitCooldownReductionFields, 1);
				// GenericAbility_Container.SetEffectFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedDirectOnHitData.m_allyHitEffectFields);
				// GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedDirectOnHitData.m_allyHitEffectTemplateFields);
			}
			else
			{
				// TODO TITUS ability results
				// GenericAbility_Container.CalcIntFieldValues(actorData, caster, actorContext, abilityContext, m_cachedDirectOnHitData.m_enemyHitIntFields, numericHitResultScratch);
				// GenericAbility_Container.SetNumericFieldsOnHitResults(actorHitResults, numericHitResultScratch);
				// GenericAbility_Container.SetKnockbackFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedDirectOnHitData.m_enemyHitKnockbackFields);
				// GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedDirectOnHitData.m_enemyHitCooldownReductionFields, 1);
				// GenericAbility_Container.SetEffectFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedDirectOnHitData.m_enemyHitEffectFields);
				// GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedDirectOnHitData.m_enemyHitEffectTemplateFields);
			}
			abilityResults.StoreActorHit(actorHitResults);
			numHitActors++;
		}
		foreach (ActorData actorData in aoeHitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			actorHitResults.SetBaseDamage(GetAoeDamage());
			actorHitResults.AddStandardEffectInfo(GetAoeEnemyHitEffect());
			ActorHitContext actorContext = new ActorHitContext();
			ContextVars abilityContext = new ContextVars();
			NumericHitResultScratch numericHitResultScratch = new NumericHitResultScratch();
			if (actorData.GetTeam() == caster.GetTeam())
			{
				// TODO TITUS ability results
				// GenericAbility_Container.CalcIntFieldValues(actorData, caster, actorContext, abilityContext, m_cachedAoEOnHitData.m_allyHitIntFields, numericHitResultScratch);
				// GenericAbility_Container.SetNumericFieldsOnHitResults(actorHitResults, numericHitResultScratch);
				// GenericAbility_Container.SetKnockbackFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedAoEOnHitData.m_allyHitKnockbackFields);
				// GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedAoEOnHitData.m_allyHitCooldownReductionFields, 1);
				// GenericAbility_Container.SetEffectFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedAoEOnHitData.m_allyHitEffectFields);
				// GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedAoEOnHitData.m_allyHitEffectTemplateFields);
			}
			else
			{
				// TODO TITUS ability results
				// GenericAbility_Container.CalcIntFieldValues(actorData, caster, actorContext, abilityContext, m_cachedAoEOnHitData.m_enemyHitIntFields, numericHitResultScratch);
				// GenericAbility_Container.SetNumericFieldsOnHitResults(actorHitResults, numericHitResultScratch);
				// GenericAbility_Container.SetKnockbackFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedAoEOnHitData.m_enemyHitKnockbackFields);
				// GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedAoEOnHitData.m_enemyHitCooldownReductionFields, 1);
				// GenericAbility_Container.SetEffectFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedAoEOnHitData.m_enemyHitEffectFields);
				// GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(actorData, caster, actorContext, abilityContext, actorHitResults, m_cachedAoEOnHitData.m_enemyHitEffectTemplateFields);
			}
			abilityResults.StoreActorHit(actorHitResults);
			numHitActors++;
		}
		if (GetHealOnSelfPerTargetHit() > 0 && numHitActors > 0)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			actorHitResults.SetBaseHealing(GetHealOnSelfPerTargetHit() * numHitActors);
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 freePos = caster.GetFreePos(caster.GetSquareAtPhaseStart());
		List<ActorData> bounceHitActors = GetBounceHitActors(targets, freePos, caster, out Vector3 item, null);
		list.Add(item);
		if (bounceHitActors != null)
		{
			foreach (ActorData hitActor in bounceHitActors)
			{
				list.Add(hitActor.GetFreePos());
			}
		}
		foreach (AbilityTarget target in targets)
		{
			list.Add(target.FreePos);
		}
		return list;
	}

	// added in rogues
	private List<ActorData> GetBounceHitActors(
		List<AbilityTarget> targets,
		Vector3 startPos,
		ActorData caster,
		out Vector3 bounceEndPoint,
		List<NonActorTargetInfo> nonActorTargetInfoInSegments)
	{
		bounceEndPoint = VectorUtils.GetLaserEndPoint(
			startPos,
			targets[0].AimDirection,
			GetChargeRange() * Board.Get().squareSize,
			false,
			caster,
			nonActorTargetInfoInSegments);
		float magnitude = (bounceEndPoint - startPos).magnitude;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			startPos,
			targets[0].AimDirection,
			magnitude / Board.Get().squareSize,
			GetChargeWidth(),
			caster,
			caster.GetEnemyTeamAsList(),
			false,
			0,
			true,
			true,
			out Vector3 _,
			nonActorTargetInfoInSegments);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInLaser);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInLaser, startPos);
		TargeterUtils.LimitActorsToMaxNumber(ref actorsInLaser, GetMaxTargets());
		if (!actorsInLaser.IsNullOrEmpty())
		{
			bounceEndPoint = actorsInLaser[0].GetLoSCheckPos();
		}
		return actorsInLaser;
	}

	// added in rogues
	private List<ActorData> GetAoeHitActors(ActorData centerHitActor, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list;
		if (centerHitActor != null)
		{
			list = AreaEffectUtils.GetActorsInShape(
				GetAoeShape(),
				centerHitActor.GetFreePos(),
				centerHitActor.GetCurrentBoardSquare(),
				false,
				caster,
				caster.GetEnemyTeam(),
				nonActorTargetInfo);
			list.Remove(centerHitActor);
			ServerAbilityUtils.RemoveEvadersFromHitTargets(ref list);
		}
		else
		{
			list = new List<ActorData>();
		}
		return list;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.ClaymoreStats.DirectHitsWithCharge);
	}
#endif
}
