using System;
using System.Collections.Generic;
using UnityEngine;

public class NekoFanOfDiscs : Ability
{
	[Separator("Targeting", true)]
	public int m_numDiscs = 5;

	public float m_minAngleForLaserFan = 90f;

	public float m_totalAngleForLaserFan = 288f;

	public float m_angleInterpMinDist = 1f;

	public float m_angleInterpMaxDist = 6f;

	[Space(10f)]
	public float m_laserRange = 6f;

	public float m_laserWidth = 1f;

	public float m_aoeRadiusAtEnd = 1f;

	public int m_maxTargetsPerLaser;

	public float m_interpStepInSquares = 1f;

	[Header("-- Disc return end radius")]
	public float m_discReturnEndRadius;

	[Separator("Hit On Throw", true)]
	public int m_directDamage = 0x19;

	public int m_directSubsequentHitDamage = 0xF;

	public StandardEffectInfo m_directEnemyHitEffect;

	[Separator("Return Trip", true)]
	public int m_returnTripDamage = 0xA;

	public int m_returnTripSubsequentHitDamage = 5;

	public bool m_returnTripIgnoreCover = true;

	public int m_returnTripEnergyOnCasterPerDiscMiss;

	[Separator("Effect on Self for misses", true)]
	public StandardEffectInfo m_effectOnSelfIfMissOnCast;

	public StandardEffectInfo m_effectOnSelfIfMissOnDiscReturn;

	[Separator("Zero Energy cost after N consecutive use", true)]
	public int m_zeroEnergyRequiredTurns;

	[Header("Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_returnTripSequencePrefab;

	public GameObject m_persistentDiscSequencePrefab;

	private AbilityMod_NekoFanOfDiscs m_abilityMod;

	private Neko_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedDirectEnemyHitEffect;

	private StandardEffectInfo m_cachedEffectOnSelfIfMissOnCast;

	private StandardEffectInfo m_cachedEffectOnSelfIfMissOnDiscReturn;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.Start()).MethodHandle;
			}
			this.m_abilityName = "Fan of Discs";
		}
		this.m_syncComp = base.GetComponent<Neko_SyncComponent>();
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_NekoDiscsFan(this, this.GetMinAngleForLaserFan(), this.GetMaxAngleForLaserFan(), this.GetAngleInterpMinDist(), this.GetAngleInterpMaxDist(), this.GetLaserRange(), this.GetLaserWidth(), this.GetAoeRadius(), this.GetMaxTargetsPerLaser(), this.GetNumDiscs(), false, this.m_interpStepInSquares, 0f);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_directDamage),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, this.m_returnTripDamage)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		Ability.AddNameplateValueForOverlap(ref result, base.Targeter, targetActor, currentTargeterIndex, this.GetDirectDamage(), this.GetDirectSubsequentHitDamage(), AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		return result;
	}

	public override int GetModdedCost()
	{
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetModdedCost()).MethodHandle;
			}
			if (this.GetZeroEnergyRequiredTurns() > 0)
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
				if (this.m_syncComp.m_numUltConsecUsedTurns >= this.GetZeroEnergyRequiredTurns())
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
					return 0;
				}
			}
		}
		return base.GetModdedCost();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedDirectEnemyHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.SetCachedFields()).MethodHandle;
			}
			cachedDirectEnemyHitEffect = this.m_abilityMod.m_directEnemyHitEffectMod.GetModifiedValue(this.m_directEnemyHitEffect);
		}
		else
		{
			cachedDirectEnemyHitEffect = this.m_directEnemyHitEffect;
		}
		this.m_cachedDirectEnemyHitEffect = cachedDirectEnemyHitEffect;
		StandardEffectInfo cachedEffectOnSelfIfMissOnCast;
		if (this.m_abilityMod)
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
			cachedEffectOnSelfIfMissOnCast = this.m_abilityMod.m_effectOnSelfIfMissOnCastMod.GetModifiedValue(this.m_effectOnSelfIfMissOnCast);
		}
		else
		{
			cachedEffectOnSelfIfMissOnCast = this.m_effectOnSelfIfMissOnCast;
		}
		this.m_cachedEffectOnSelfIfMissOnCast = cachedEffectOnSelfIfMissOnCast;
		StandardEffectInfo cachedEffectOnSelfIfMissOnDiscReturn;
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
			cachedEffectOnSelfIfMissOnDiscReturn = this.m_abilityMod.m_effectOnSelfIfMissOnDiscReturnMod.GetModifiedValue(this.m_effectOnSelfIfMissOnDiscReturn);
		}
		else
		{
			cachedEffectOnSelfIfMissOnDiscReturn = this.m_effectOnSelfIfMissOnDiscReturn;
		}
		this.m_cachedEffectOnSelfIfMissOnDiscReturn = cachedEffectOnSelfIfMissOnDiscReturn;
	}

	public int GetNumDiscs()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetNumDiscs()).MethodHandle;
			}
			result = this.m_abilityMod.m_numDiscsMod.GetModifiedValue(this.m_numDiscs);
		}
		else
		{
			result = this.m_numDiscs;
		}
		return result;
	}

	public float GetMinAngleForLaserFan()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetMinAngleForLaserFan()).MethodHandle;
			}
			result = this.m_abilityMod.m_minAngleForLaserFanMod.GetModifiedValue(this.m_minAngleForLaserFan);
		}
		else
		{
			result = this.m_minAngleForLaserFan;
		}
		return result;
	}

	public float GetMaxAngleForLaserFan()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetMaxAngleForLaserFan()).MethodHandle;
			}
			result = this.m_abilityMod.m_totalAngleForLaserFanMod.GetModifiedValue(this.m_totalAngleForLaserFan);
		}
		else
		{
			result = this.m_totalAngleForLaserFan;
		}
		return result;
	}

	public float GetAngleInterpMinDist()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetAngleInterpMinDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_angleInterpMinDistMod.GetModifiedValue(this.m_angleInterpMinDist);
		}
		else
		{
			result = this.m_angleInterpMinDist;
		}
		return result;
	}

	public float GetAngleInterpMaxDist()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetAngleInterpMaxDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_angleInterpMaxDistMod.GetModifiedValue(this.m_angleInterpMaxDist);
		}
		else
		{
			result = this.m_angleInterpMaxDist;
		}
		return result;
	}

	public float GetLaserRange()
	{
		return (!this.m_abilityMod) ? this.m_laserRange : this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
	}

	public float GetLaserWidth()
	{
		return (!this.m_abilityMod) ? this.m_laserWidth : this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
	}

	public float GetAoeRadius()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetAoeRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_aoeRadiusAtEndMod.GetModifiedValue(this.m_aoeRadiusAtEnd);
		}
		else
		{
			result = this.m_aoeRadiusAtEnd;
		}
		return result;
	}

	public int GetMaxTargetsPerLaser()
	{
		return (!this.m_abilityMod) ? this.m_maxTargetsPerLaser : this.m_abilityMod.m_maxTargetsPerLaserMod.GetModifiedValue(this.m_maxTargetsPerLaser);
	}

	public float GetInterpStepInSquares()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetInterpStepInSquares()).MethodHandle;
			}
			result = this.m_abilityMod.m_interpStepInSquaresMod.GetModifiedValue(this.m_interpStepInSquares);
		}
		else
		{
			result = this.m_interpStepInSquares;
		}
		return result;
	}

	public float GetDiscReturnEndRadius()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetDiscReturnEndRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(this.m_discReturnEndRadius);
		}
		else
		{
			result = this.m_discReturnEndRadius;
		}
		return result;
	}

	public int GetDirectDamage()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetDirectDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_directDamageMod.GetModifiedValue(this.m_directDamage);
		}
		else
		{
			result = this.m_directDamage;
		}
		return result;
	}

	public int GetDirectSubsequentHitDamage()
	{
		return (!this.m_abilityMod) ? this.m_directSubsequentHitDamage : this.m_abilityMod.m_directSubsequentHitDamageMod.GetModifiedValue(this.m_directSubsequentHitDamage);
	}

	public StandardEffectInfo GetDirectEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedDirectEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetDirectEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedDirectEnemyHitEffect;
		}
		else
		{
			result = this.m_directEnemyHitEffect;
		}
		return result;
	}

	public int GetReturnTripDamage()
	{
		return (!this.m_abilityMod) ? this.m_returnTripDamage : this.m_abilityMod.m_returnTripDamageMod.GetModifiedValue(this.m_returnTripDamage);
	}

	public int GetReturnTripSubsequentHitDamage()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetReturnTripSubsequentHitDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_returnTripSubsequentHitDamageMod.GetModifiedValue(this.m_returnTripSubsequentHitDamage);
		}
		else
		{
			result = this.m_returnTripSubsequentHitDamage;
		}
		return result;
	}

	public bool ReturnTripIgnoreCover()
	{
		return (!this.m_abilityMod) ? this.m_returnTripIgnoreCover : this.m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(this.m_returnTripIgnoreCover);
	}

	public int GetReturnTripEnergyOnCasterPerDiscMiss()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetReturnTripEnergyOnCasterPerDiscMiss()).MethodHandle;
			}
			result = this.m_abilityMod.m_returnTripEnergyOnCasterPerDiscMissMod.GetModifiedValue(this.m_returnTripEnergyOnCasterPerDiscMiss);
		}
		else
		{
			result = this.m_returnTripEnergyOnCasterPerDiscMiss;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelfIfMissOnCast()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnSelfIfMissOnCast != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetEffectOnSelfIfMissOnCast()).MethodHandle;
			}
			result = this.m_cachedEffectOnSelfIfMissOnCast;
		}
		else
		{
			result = this.m_effectOnSelfIfMissOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelfIfMissOnDiscReturn()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnSelfIfMissOnDiscReturn != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetEffectOnSelfIfMissOnDiscReturn()).MethodHandle;
			}
			result = this.m_cachedEffectOnSelfIfMissOnDiscReturn;
		}
		else
		{
			result = this.m_effectOnSelfIfMissOnDiscReturn;
		}
		return result;
	}

	public int GetZeroEnergyRequiredTurns()
	{
		return (!this.m_abilityMod) ? this.m_zeroEnergyRequiredTurns : this.m_abilityMod.m_zeroEnergyRequiredTurnsMod.GetModifiedValue(this.m_zeroEnergyRequiredTurns);
	}

	private LaserTargetingInfo GetLaserInfo()
	{
		return new LaserTargetingInfo
		{
			affectsEnemies = true,
			range = this.GetLaserRange(),
			width = this.GetLaserWidth(),
			maxTargets = this.GetMaxTargetsPerLaser()
		};
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "NumDiscs", string.Empty, this.m_numDiscs, false);
		base.AddTokenInt(tokens, "MaxTargetsPerLaser", string.Empty, this.m_maxTargetsPerLaser, false);
		base.AddTokenInt(tokens, "DirectDamage", string.Empty, this.m_directDamage, false);
		base.AddTokenInt(tokens, "DirectSubsequentHitDamage", string.Empty, this.m_directSubsequentHitDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_directEnemyHitEffect, "DirectEnemyHitEffect", this.m_directEnemyHitEffect, true);
		base.AddTokenInt(tokens, "ReturnTripDamage", string.Empty, this.m_returnTripDamage, false);
		base.AddTokenInt(tokens, "ReturnTripSubsequentHitDamage", string.Empty, this.m_returnTripSubsequentHitDamage, false);
		base.AddTokenInt(tokens, "ReturnTripEnergyOnCasterPerDiscMiss", string.Empty, this.m_returnTripEnergyOnCasterPerDiscMiss, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnSelfIfMissOnCast, "EffectOnSelfIfMissOnCast", this.m_effectOnSelfIfMissOnCast, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnSelfIfMissOnDiscReturn, "EffectOnSelfIfMissOnDiscReturn", this.m_effectOnSelfIfMissOnDiscReturn, true);
		base.AddTokenInt(tokens, "ZeroEnergyRequiredTurns", string.Empty, this.m_zeroEnergyRequiredTurns, false);
	}

	public override int GetTheatricsSortPriority(AbilityData.ActionType actionType)
	{
		return 3;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRange() + this.GetAoeRadius();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NekoFanOfDiscs))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_NekoFanOfDiscs);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	private unsafe Dictionary<ActorData, int> GetHitActorsAndHitCount(List<AbilityTarget> targets, ActorData caster, out List<List<ActorData>> actorsForSequence, out List<BoardSquare> targetSquares, out int numLasersWithHits, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		targetSquares = new List<BoardSquare>();
		List<Vector3> list;
		Dictionary<ActorData, int> hitActorsAndHitCount = AbilityCommon_FanLaser.GetHitActorsAndHitCount(targets, caster, this.GetLaserInfo(), this.GetNumDiscs(), this.GetMaxAngleForLaserFan() / (float)this.GetNumDiscs(), true, this.GetMinAngleForLaserFan(), this.GetMaxAngleForLaserFan(), this.GetAngleInterpMinDist(), this.GetAngleInterpMaxDist(), out actorsForSequence, out list, out numLasersWithHits, nonActorTargetInfo, false, this.m_interpStepInSquares, 0f);
		Vector3 startPos = caster.\u0015();
		for (int i = 0; i < list.Count; i++)
		{
			Vector3 vector = list[i];
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(startPos, vector);
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(vector, this.GetAoeRadius(), false, caster, caster.\u0012(), nonActorTargetInfo, true, coneLosCheckPos);
			using (List<ActorData>.Enumerator enumerator = actorsInRadius.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					if (!actorsForSequence[i].Contains(actorData))
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetHitActorsAndHitCount(List<AbilityTarget>, ActorData, List<List<ActorData>>*, List<BoardSquare>*, int*, List<NonActorTargetInfo>)).MethodHandle;
						}
						if (!hitActorsAndHitCount.ContainsKey(actorData))
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
							hitActorsAndHitCount.Add(actorData, 1);
						}
						else
						{
							Dictionary<ActorData, int> dictionary;
							ActorData key;
							(dictionary = hitActorsAndHitCount)[key = actorData] = dictionary[key] + 1;
						}
						if (!actorsForSequence[i].Contains(actorData))
						{
							actorsForSequence[i].Add(actorData);
						}
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		targetSquares = NekoFanOfDiscs.GetDiscSquaresFromEndPositions(list, caster.\u0015());
		return hitActorsAndHitCount;
	}

	public static List<BoardSquare> GetDiscSquaresFromEndPositions(List<Vector3> endPositions, Vector3 startPos)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		using (List<Vector3>.Enumerator enumerator = endPositions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Vector3 vector = enumerator.Current;
				BoardSquare boardSquare = NekoBoomerangDisc.GetDiscEndSquare(startPos, vector);
				if (list.Contains(boardSquare))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(NekoFanOfDiscs.GetDiscSquaresFromEndPositions(List<Vector3>, Vector3)).MethodHandle;
					}
					Vector3 pos = vector;
					bool flag = false;
					int i = 1;
					while (i < 3)
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
						if (flag)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								goto IL_F7;
							}
						}
						else
						{
							List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(boardSquare, i, true);
							AreaEffectUtils.SortSquaresByDistanceToPos(ref squaresInBorderLayer, pos);
							using (List<BoardSquare>.Enumerator enumerator2 = squaresInBorderLayer.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									BoardSquare boardSquare2 = enumerator2.Current;
									if (boardSquare2.\u0016())
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
										if (!list.Contains(boardSquare2))
										{
											boardSquare = boardSquare2;
											flag = true;
											goto IL_D1;
										}
									}
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
							IL_D1:
							i++;
						}
					}
				}
				IL_F7:
				list.Add(boardSquare);
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
		return list;
	}
}
