// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MantaDashThroughWall : Ability
{
	[Header("-- Charge Targeting")]
	public float m_aoeConeWidth = 180f;
	public float m_aoeConeLength = 2f;
	public float m_aoeThroughWallConeWidth = 180f;
	public float m_aoeThroughWallConeLength = 2f;
	public float m_width = 1f;
	public float m_maxRange = 10f;
	public float m_maxWidthOfWall = 1f;
	public Color m_normalHighlightColor = Color.green;
	public Color m_throughWallsHighlightColor = Color.yellow;
	public float m_extraTotalDistanceIfThroughWalls = 1.5f;
	public bool m_clampConeToWall = true;
	public bool m_aoeWithMiss;
	[Tooltip("backward offset not used for through-walls case")]
	public float m_coneBackwardOffset = 0.5f;
	[Header("-- Normal On Hit Damage, Effect, etc")]
	public int m_directHitDamage = 20;
	public StandardEffectInfo m_directEnemyHitEffect;
	public bool m_directHitIgnoreCover = true;
	[Space(10f)]
	public int m_aoeDamage = 10;
	public StandardEffectInfo m_aoeEnemyHitEffect;
	[Space(10f)]
	public int m_aoeThroughWallsDamage = 10;
	public StandardEffectInfo m_aoeThroughWallsEffect;
	[Header("-- Sequences")]
	public GameObject m_aoeHitSequencePrefab;

	private const int c_maxTargetsHit = 1;
	private const bool c_penetrateLoS = false;
	private Manta_SyncComponent m_syncComp;
	private AbilityMod_MantaDashThroughWall m_abilityMod;
	private StandardEffectInfo m_cachedDirectEnemyHitEffect;
	private StandardEffectInfo m_cachedAoeEnemyHitEffect;
	private StandardEffectInfo m_cachedAoeThroughWallsEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Dash Through Wall";
		}
		m_syncComp = GetComponent<Manta_SyncComponent>();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_DashThroughWall(
			this,
			GetWidth(),
			GetMaxRange(),
			GetMaxWidthOfWall(),
			GetAoeConeWidth(),
			GetAoeThroughWallConeWidth(),
			GetAoeConeLength(),
			GetAoeThroughWallConeLength(),
			m_extraTotalDistanceIfThroughWalls,
			m_coneBackwardOffset,
			DirectHitIgnoreCover(),
			m_clampConeToWall,
			m_aoeWithMiss);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxRange();
	}

	private void SetCachedFields()
	{
		m_cachedDirectEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_directEnemyHitEffectMod.GetModifiedValue(m_directEnemyHitEffect)
			: m_directEnemyHitEffect;
		m_cachedAoeEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_aoeEnemyHitEffectMod.GetModifiedValue(m_aoeEnemyHitEffect)
			: m_aoeEnemyHitEffect;
		m_cachedAoeThroughWallsEffect = m_abilityMod != null
			? m_abilityMod.m_aoeThroughWallsEffectMod.GetModifiedValue(m_aoeThroughWallsEffect)
			: m_aoeThroughWallsEffect;
	}

	public float GetAoeConeWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeConeWidthMod.GetModifiedValue(m_aoeConeWidth)
			: m_aoeConeWidth;
	}

	public float GetAoeConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeConeLengthMod.GetModifiedValue(m_aoeConeLength)
			: m_aoeConeLength;
	}

	public float GetAoeThroughWallConeWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeThroughWallConeWidthMod.GetModifiedValue(m_aoeThroughWallConeWidth)
			: m_aoeThroughWallConeWidth;
	}

	public float GetAoeThroughWallConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeThroughWallConeLengthMod.GetModifiedValue(m_aoeThroughWallConeLength)
			: m_aoeThroughWallConeLength;
	}

	public float GetWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_widthMod.GetModifiedValue(m_width)
			: m_width;
	}

	public float GetMaxRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxRangeMod.GetModifiedValue(m_maxRange)
			: m_maxRange;
	}

	public float GetMaxWidthOfWall()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxWidthOfWallMod.GetModifiedValue(m_maxWidthOfWall)
			: m_maxWidthOfWall;
	}

	public float GetExtraTotalDistanceIfThroughWalls()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraTotalDistanceIfThroughWallsMod.GetModifiedValue(m_extraTotalDistanceIfThroughWalls)
			: m_extraTotalDistanceIfThroughWalls;
	}

	public bool ClampConeToWall()
	{
		return m_abilityMod != null
			? m_abilityMod.m_clampConeToWallMod.GetModifiedValue(m_clampConeToWall)
			: m_clampConeToWall;
	}

	public bool AoeWithMiss()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeWithMissMod.GetModifiedValue(m_aoeWithMiss)
			: m_aoeWithMiss;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset)
			: m_coneBackwardOffset;
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

	public bool DirectHitIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_directHitIgnoreCoverMod.GetModifiedValue(m_directHitIgnoreCover)
			: m_directHitIgnoreCover;
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

	public int GetAoeThroughWallsDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeThroughWallsDamageMod.GetModifiedValue(m_aoeThroughWallsDamage)
			: m_aoeThroughWallsDamage;
	}

	public StandardEffectInfo GetAoeThroughWallsEffect()
	{
		return m_cachedAoeThroughWallsEffect ?? m_aoeThroughWallsEffect;
	}

	public StandardEffectInfo GetAdditionalDirtyFightingExplosionEffect()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_additionalDirtyFightingExplosionEffect.operation == AbilityModPropertyEffectInfo.ModOp.Override
				? m_abilityMod.m_additionalDirtyFightingExplosionEffect.effectInfo
				: null;
	}

	public int GetMaxTargets()
	{
		return 1;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaDashThroughWall))
		{
			m_abilityMod = abilityMod as AbilityMod_MantaDashThroughWall;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DirectHitDamage", string.Empty, m_directHitDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_directEnemyHitEffect, "DirectEnemyHitEffect", m_directEnemyHitEffect);
		AddTokenInt(tokens, "AoeDamage", string.Empty, m_aoeDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_aoeEnemyHitEffect, "AoeEnemyHitEffect", m_aoeEnemyHitEffect);
		AddTokenInt(tokens, "AoeThroughWallsDamage", string.Empty, m_aoeThroughWallsDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_aoeThroughWallsEffect, "AoeThroughWallsEffect", m_aoeThroughWallsEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDirectHitDamage());
		GetDirectEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetAoeDamage());
		GetAoeEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Tertiary, GetAoeThroughWallsDamage());
		GetAoeThroughWallsEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			int damage = 0;
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				damage += GetDirectHitDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				damage += GetAoeDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Tertiary))
			{
				damage += GetAoeThroughWallsDamage();
			}

			// added in rogues
			// if (m_syncComp != null)
			// {
			// 	damage += m_syncComp.GetDirtyFightingExtraDamage(targetActor);
			// }

			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (m_syncComp == null)
		{
			return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
		}
		
		int energy = 0;
		foreach (AbilityUtil_Targeter.ActorTarget actorTarget in Targeters[currentTargeterIndex].GetActorsInRange())
		{
			energy += m_syncComp.GetDirtyFightingExtraTP(actorTarget.m_actor);
		}
		return energy;
	}

	// removed in rogues
	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return symbolType == AbilityTooltipSymbol.Damage
		       && m_syncComp != null
			? m_syncComp.GetAccessoryStringForDamage(targetActor, ActorData, this)
			: null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	internal static BoardSquare GetSquareBeyondWall(
		Vector3 startPos,
		Vector3 endPos,
		ActorData targetingActor,
		float penetrationDistance,
		ref Vector3 coneStartPos,
		ref Vector3 perpendicularFromWall)
	{
		float quarter = 0.25f * Board.Get().squareSize;
		int steps = Mathf.CeilToInt(penetrationDistance / quarter);
		Vector3 vector = endPos - startPos;
		float dist = vector.magnitude;
		vector.Normalize();
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(startPos, vector, dist, false, targetingActor);
		Vector3 vector2D = laserEndPoint;
		BoardSquare boardSquare = null;
		int i = 0;
		while (boardSquare == null)
		{
			vector2D += quarter * vector;
			boardSquare = Board.Get().GetSquareFromVec3(vector2D);
			if (boardSquare != null && !boardSquare.IsValidForGameplay())
			{
				boardSquare = null;
			}
			if (++i > steps)
			{
				break;
			}
		}
		if (boardSquare == null || !boardSquare.IsValidForGameplay())
		{
			boardSquare = Board.Get().GetSquareFromVec3(laserEndPoint);
			coneStartPos = endPos;
		}

		if (boardSquare != null)
		{
			Vector3 occupantLoSPos = boardSquare.GetOccupantLoSPos();
			Vector3 direction = (occupantLoSPos - startPos).normalized;
			direction.y = 0f;
			if (Mathf.Abs(direction.x) > 0.3f && Mathf.Abs(direction.z) > 0.3f)
			{
				float x = direction.x;
				direction.x = 0f;
				if (!VectorUtils.RaycastInDirection(occupantLoSPos, -1f * direction.normalized, Board.Get().squareSize, out _))
				{
					direction.z = 0f;
					direction.x = x;
					VectorUtils.RaycastInDirection(occupantLoSPos, -1f * direction.normalized, Board.Get().squareSize, out _);
				}
			}
			int angleWithHorizontal = Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(direction));
			perpendicularFromWall = VectorUtils.HorizontalAngleToClosestCardinalDirection(angleWithHorizontal);
			coneStartPos = occupantLoSPos - perpendicularFromWall * 0.5f;
		}
		return boardSquare;
	}
	
#if SERVER
	// added in rogues
	public override bool GetChargeThroughInvalidSquares()
	{
		return true;
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 coneStartPos = Vector3.zero;
		Vector3 perpendicularFromWall = Vector3.right;
		BoardSquare pathDestination = GetPathDestination(targets, caster, ref coneStartPos, ref perpendicularFromWall);
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<ActorData> chargeHitActors = GetChargeHitActors(
			targets, loSCheckPos, caster, out _, null, out bool traveledFullDistance);
		ServerEvadeUtils.ChargeSegment[] chargeSegments = new ServerEvadeUtils.ChargeSegment[2];
		chargeSegments[0] = new ServerEvadeUtils.ChargeSegment
		{
			m_pos = caster.GetCurrentBoardSquare(),
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_end = BoardSquarePathInfo.ChargeEndType.None
		};
		chargeSegments[1] = new ServerEvadeUtils.ChargeSegment
		{
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement
		};
		if (!traveledFullDistance && chargeHitActors.Count == 0)
		{
			chargeSegments[1].m_end = BoardSquarePathInfo.ChargeEndType.Recovery;
		}
		else if (chargeHitActors.Count > 0 || m_aoeWithMiss)
		{
			chargeSegments[1].m_end = BoardSquarePathInfo.ChargeEndType.Impact;
		}
		else
		{
			chargeSegments[1].m_end = BoardSquarePathInfo.ChargeEndType.Miss;
		}
		chargeSegments[1].m_pos = pathDestination;
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

	// added in rogues
	public override BoardSquare GetIdealDestination(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 coneStartPos = Vector3.zero;
		Vector3 perpendicularFromWall = Vector3.right;
		return GetPathDestination(targets, caster, ref coneStartPos, ref perpendicularFromWall);
	}

	// added in rogues
	private BoardSquare GetPathDestination(List<AbilityTarget> targets, ActorData caster, ref Vector3 coneStartPos, ref Vector3 perpendicularFromWall)
	{
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<ActorData> chargeHitActors = GetChargeHitActors(
			targets, loSCheckPos, caster, out Vector3 chargeEndPoint, null, out bool traveledFullDistance);
		Vector3 vector2 = chargeEndPoint - loSCheckPos;
		float magnitude = vector2.magnitude;
		vector2.Normalize();
		BoardSquare result;
		if (GetMaxTargets() > 0 && chargeHitActors.Count >= GetMaxTargets())
		{
			result = chargeHitActors[chargeHitActors.Count - 1].GetCurrentBoardSquare();
		}
		else
		{
			BoardSquare boardSquare = null;
			if (!traveledFullDistance)
			{
				float num = GetMaxWidthOfWall() * Board.Get().squareSize;
				num = Mathf.Min(num, (GetMaxRange() + m_extraTotalDistanceIfThroughWalls) * Board.Get().squareSize - magnitude);
				Vector3 endPos = chargeEndPoint + vector2 * num;
				boardSquare = GetSquareBeyondWall(loSCheckPos, endPos, caster, num, ref coneStartPos, ref perpendicularFromWall);
			}
			if (boardSquare == null)
			{
				float num2 = Mathf.Min(0.5f, magnitude / 2f);
				chargeEndPoint -= vector2 * num2;
				boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(loSCheckPos, chargeEndPoint, true);
				if (boardSquare == null)
				{
					boardSquare = caster.GetSquareAtPhaseStart();
				}
				if (Board.Get().GetSquareFromVec3(chargeEndPoint) == null)
				{
					coneStartPos = boardSquare.ToVector3();
				}
			}
			result = boardSquare;
		}
		return result;
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<ActorData> chargeHitActors = GetChargeHitActors(
			targets, loSCheckPos, caster, out Vector3 chargeEndPoint, null, out bool traveledFullDistance);
		List<ActorData> hitActors = additionalData.m_abilityResults.HitActorList();
		bool hitEnv = chargeHitActors.Count == 0 && !traveledFullDistance;
		Vector3 targetPos = Vector3.zero;
		Vector3 direction = (chargeEndPoint - loSCheckPos).normalized;
		if (hitEnv)
		{
			Vector3 perpendicularFromWall = direction;
			GetPathDestination(targets, caster, ref targetPos, ref perpendicularFromWall);
			if (m_clampConeToWall)
			{
				direction = perpendicularFromWall.normalized;
			}
		}
		else
		{
			targetPos = chargeEndPoint - direction * m_coneBackwardOffset * Board.Get().squareSize;
		}
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_aoeHitSequencePrefab, targetPos, hitActors.ToArray(), caster, additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> chargeHitActors = GetChargeHitActors(targets, loSCheckPos, caster, out var vector, nonActorTargetInfo, out var flag);
		int num = 0;
		List<ActorData> aoeHitActors = null;
		bool flag2 = !flag && chargeHitActors.Count == 0;
		if (m_aoeWithMiss || flag2 || chargeHitActors.Count > 0)
		{
			Vector3 coneStartPos = Vector3.zero;
			Vector3 direction = (vector - loSCheckPos).normalized;
			if (flag2)
			{
				Vector3 perpendicularFromWall = direction;
				GetPathDestination(targets, caster, ref coneStartPos, ref perpendicularFromWall);
				if (m_clampConeToWall)
				{
					direction = perpendicularFromWall.normalized;
				}
			}
			else
			{
				coneStartPos = vector - direction * m_coneBackwardOffset * Board.Get().squareSize;
				if (Board.Get().GetSquareFromVec3(coneStartPos) == null)
				{
					Vector3 zero = Vector3.zero;
					coneStartPos = GetPathDestination(targets, caster, ref coneStartPos, ref zero).ToVector3();
				}
			}
			aoeHitActors = GetAoeHitActors(coneStartPos, direction, caster, nonActorTargetInfo, flag2);
			foreach (ActorData item in chargeHitActors)
			{
				aoeHitActors.Remove(item);
			}
		}
		else
		{
			aoeHitActors = new List<ActorData>();
		}
		foreach (ActorData actorData in chargeHitActors)
		{
			Vector3 origin = DirectHitIgnoreCover() ? actorData.GetFreePos() : loSCheckPos;
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, origin));
			int directHitDamage = GetDirectHitDamage();
			actorHitResults.SetBaseDamage(directHitDamage);
			actorHitResults.AddStandardEffectInfo(GetDirectEnemyHitEffect());
			List<Effect> effects = ServerEffectManager.Get().GetEffectsOnTargetByCaster(actorData, caster, typeof(MantaDirtyFightingEffect));
			if (!effects.IsNullOrEmpty() && (effects[0] as MantaDirtyFightingEffect).IsActive())
			{
				StandardEffectInfo dirtyFightingEffect = GetAdditionalDirtyFightingExplosionEffect();
				if (dirtyFightingEffect != null)
				{
					actorHitResults.AddStandardEffectInfo(dirtyFightingEffect);
				}
			}
			abilityResults.StoreActorHit(actorHitResults);
			num++;
		}
		foreach (ActorData actorData in aoeHitActors)
		{
			ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			if (chargeHitActors.Count > 0)
			{
				actorHitResults2.SetBaseDamage(GetAoeDamage());
				actorHitResults2.AddStandardEffectInfo(GetAoeEnemyHitEffect());
			}
			else
			{
				actorHitResults2.SetBaseDamage(GetAoeThroughWallsDamage());
				actorHitResults2.AddStandardEffectInfo(GetAoeThroughWallsEffect());
				actorHitResults2.AddHitResultsTag(HitResultsTags.HittingThroughWalls);
			}
			List<Effect> effects = ServerEffectManager.Get().GetEffectsOnTargetByCaster(actorData, caster, typeof(MantaDirtyFightingEffect));
			if (!effects.IsNullOrEmpty() && (effects[0] as MantaDirtyFightingEffect).IsActive())
			{
				StandardEffectInfo dirtyFightingEffect = GetAdditionalDirtyFightingExplosionEffect();
				if (dirtyFightingEffect != null)
				{
					actorHitResults2.AddStandardEffectInfo(dirtyFightingEffect);
				}
			}
			abilityResults.StoreActorHit(actorHitResults2);
			num++;
		}
		if (num == 0
		    && m_abilityMod != null
		    && m_abilityMod.m_cooldownReductionsWhenNoHits.HasCooldownReduction())
		{
			ActorHitResults actorHitResults3 = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			actorHitResults3.SetIgnoreTechpointInteractionForHit(true);
			m_abilityMod.m_cooldownReductionsWhenNoHits.AppendCooldownMiscEvents(actorHitResults3, true, 0, 0);
			abilityResults.StoreActorHit(actorHitResults3);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 casterPos = caster.GetFreePos(caster.GetSquareAtPhaseStart());
		List<ActorData> chargeHitActors = GetChargeHitActors(targets, casterPos, caster, out var chargeEndPoint, null, out _);
		list.Add(chargeEndPoint);
		if (chargeHitActors != null)
		{
			foreach (ActorData hitActor in chargeHitActors)
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
	private List<ActorData> GetChargeHitActors(
		List<AbilityTarget> targets,
		Vector3 startPos,
		ActorData caster,
		out Vector3 chargeEndPoint,
		List<NonActorTargetInfo> nonActorTargetInfo,
		out bool traveledFullDistance)
	{
		Vector3 aimDirection = targets[0].AimDirection;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			startPos,
			aimDirection,
			GetMaxRange(),
			GetWidth(),
			caster,
			caster.GetOtherTeams(),
			false,
			0,
			false,
			true,
			out chargeEndPoint,
			nonActorTargetInfo,
			null,
			true);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInLaser);
		if (GetMaxTargets() > 0)
		{
			TargeterUtils.LimitActorsToMaxNumber(ref actorsInLaser, GetMaxTargets());
		}
		float magnitude;
		if (actorsInLaser.Count > 0)
		{
			Vector3 vector = actorsInLaser[actorsInLaser.Count - 1].GetFreePos() - startPos;
			vector.y = 0f;
			magnitude = (Vector3.Dot(vector, aimDirection) * aimDirection).magnitude;
		}
		else
		{
			magnitude = (startPos - chargeEndPoint).magnitude;
		}
		chargeEndPoint = startPos + aimDirection * magnitude;
		float num = (GetMaxRange() - 0.25f) * Board.Get().squareSize;
		traveledFullDistance = magnitude >= num;
		return actorsInLaser;
	}

	// added in rogues
	private List<ActorData> GetAoeHitActors(
		Vector3 coneStart, Vector3 aimDirection, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo, bool throughWall)
	{
		float coneWidthDegrees = GetAoeConeWidth();
		float coneLengthRadiusInSquares = GetAoeConeLength();
		if (throughWall)
		{
			coneWidthDegrees = GetAoeThroughWallConeWidth();
			coneLengthRadiusInSquares = GetAoeThroughWallConeLength();
		}
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
			coneStart,
			VectorUtils.HorizontalAngle_Deg(aimDirection),
			coneWidthDegrees,
			coneLengthRadiusInSquares,
			0f,
			false,
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		actorsInCone.Remove(caster);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInCone);
		return actorsInCone;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.BaseDamage > 0 && results.HasHitResultsTag(HitResultsTags.HittingThroughWalls))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.MantaStats.NumHitsThroughWalls);
		}
	}
#endif
}
