using System.Collections.Generic;
using UnityEngine;

public class ThiefBasicAttack : Ability
{
	[Header("-- Targeter")]
	public bool m_targeterMultiTarget = true;

	public float m_targeterMaxAngle = 120f;

	public float m_targeterMinInterpDistance = 1.5f;

	public float m_targeterMaxInterpDistance = 6f;

	[Header("-- Damage")]
	public int m_laserDamageAmount = 3;

	public int m_laserSubsequentDamageAmount = 3;

	public int m_extraDamageForSingleHit;

	public int m_extraDamageForHittingPowerup;

	[Header("-- Healing")]
	public int m_healOnSelfIfHitEnemyAndPowerup;

	[Header("-- Energy")]
	public int m_energyGainPerLaserHit;

	public int m_energyGainPerPowerupHit;

	[Header("-- Laser Properties")]
	public float m_laserRange = 5f;

	public float m_laserWidth = 0.5f;

	public int m_laserMaxTargets = 1;

	public int m_laserCount = 2;

	public bool m_laserPenetrateLos;

	[Header("-- PowerUp/Spoils Interaction")]
	public bool m_stopOnPowerupHit = true;

	public bool m_includeSpoilsPowerups = true;

	public bool m_ignorePickupTeamRestriction;

	[Header("-- Sequences --")]
	public GameObject m_onCastSequencePrefab;

	public GameObject m_powerupReturnPrefab;

	private AbilityMod_ThiefBasicAttack m_abilityMod;

	private int c_maxPowerupPerLaser = 1;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Strong Arms";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		float targeterMaxAngle = GetTargeterMaxAngle();
		bool stopOnPowerUp = StopOnPowerupHit();
		bool flag = GetHealOnSelfIfHitEnemyAndPowerup() > 0;
		if (TargeterMultiTarget())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					ClearTargeters();
					for (int i = 0; i < GetLaserCount(); i++)
					{
						AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = new AbilityUtil_Targeter_ThiefFanLaser(this, 0f, targeterMaxAngle, m_targeterMinInterpDistance, m_targeterMaxInterpDistance, GetLaserRange(), GetLaserWidth(), GetLaserMaxTargets(), GetLaserCount(), LaserPenetrateLos(), true, stopOnPowerUp, IncludeSpoilsPowerups(), IgnorePickupTeamRestriction(), c_maxPowerupPerLaser);
						abilityUtil_Targeter_ThiefFanLaser.SetUseMultiTargetUpdate(true);
						if (flag)
						{
							
							abilityUtil_Targeter_ThiefFanLaser.m_affectCasterDelegate = delegate(ActorData caster, bool hitEnemy, bool hitPowerup)
								{
									int result2;
									if (hitEnemy)
									{
										result2 = (hitPowerup ? 1 : 0);
									}
									else
									{
										result2 = 0;
									}
									return (byte)result2 != 0;
								};
						}
						base.Targeters.Add(abilityUtil_Targeter_ThiefFanLaser);
					}
					while (true)
					{
						switch (4)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				}
			}
		}
		AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser2 = new AbilityUtil_Targeter_ThiefFanLaser(this, 0f, targeterMaxAngle, m_targeterMinInterpDistance, m_targeterMaxInterpDistance, GetLaserRange(), GetLaserWidth(), GetLaserMaxTargets(), GetLaserCount(), LaserPenetrateLos(), true, stopOnPowerUp, IncludeSpoilsPowerups(), IgnorePickupTeamRestriction(), c_maxPowerupPerLaser);
		if (flag)
		{
			
			abilityUtil_Targeter_ThiefFanLaser2.m_affectCasterDelegate = delegate(ActorData caster, bool hitEnemy, bool hitPowerup)
				{
					int result;
					if (hitEnemy)
					{
						result = (hitPowerup ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				};
		}
		base.Targeter = abilityUtil_Targeter_ThiefFanLaser2;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return (!TargeterMultiTarget()) ? 1 : GetLaserCount();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	public bool TargeterMultiTarget()
	{
		return m_targeterMultiTarget;
	}

	public float GetTargeterMaxAngle()
	{
		float b;
		if ((bool)m_abilityMod)
		{
			b = m_abilityMod.m_targeterMaxAngleMod.GetModifiedValue(m_targeterMaxAngle);
		}
		else
		{
			b = m_targeterMaxAngle;
		}
		return Mathf.Max(1f, b);
	}

	public int GetLaserDamageAmount()
	{
		return (!m_abilityMod) ? m_laserDamageAmount : m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount);
	}

	public int GetLaserSubsequentDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount);
		}
		else
		{
			result = m_laserSubsequentDamageAmount;
		}
		return result;
	}

	public int GetExtraDamageForSingleHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit);
		}
		else
		{
			result = m_extraDamageForSingleHit;
		}
		return result;
	}

	public int GetExtraDamageForHittingPowerup()
	{
		return (!m_abilityMod) ? m_extraDamageForHittingPowerup : m_abilityMod.m_extraDamageForHittingPowerupMod.GetModifiedValue(m_extraDamageForHittingPowerup);
	}

	public int GetHealOnSelfIfHitEnemyAndPowerup()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healOnSelfIfHitEnemyAndPowerupMod.GetModifiedValue(m_healOnSelfIfHitEnemyAndPowerup);
		}
		else
		{
			result = m_healOnSelfIfHitEnemyAndPowerup;
		}
		return result;
	}

	public int GetEnergyGainPerLaserHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyGainPerLaserHitMod.GetModifiedValue(m_energyGainPerLaserHit);
		}
		else
		{
			result = m_energyGainPerLaserHit;
		}
		return result;
	}

	public int GetEnergyGainPerPowerupHit()
	{
		return (!m_abilityMod) ? m_energyGainPerPowerupHit : m_abilityMod.m_energyGainPerPowerupHitMod.GetModifiedValue(m_energyGainPerPowerupHit);
	}

	public float GetLaserRange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
		}
		else
		{
			result = m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public int GetLaserMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets);
		}
		else
		{
			result = m_laserMaxTargets;
		}
		return result;
	}

	public int GetLaserCount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserCountMod.GetModifiedValue(m_laserCount);
		}
		else
		{
			result = m_laserCount;
		}
		return result;
	}

	public bool LaserPenetrateLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserPenetrateLosMod.GetModifiedValue(m_laserPenetrateLos);
		}
		else
		{
			result = m_laserPenetrateLos;
		}
		return result;
	}

	public bool StopOnPowerupHit()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_stopOnPowerupHitMod.GetModifiedValue(m_stopOnPowerupHit);
		}
		else
		{
			result = m_stopOnPowerupHit;
		}
		return result;
	}

	public bool IncludeSpoilsPowerups()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_includeSpoilsPowerupsMod.GetModifiedValue(m_includeSpoilsPowerups);
		}
		else
		{
			result = m_includeSpoilsPowerups;
		}
		return result;
	}

	public bool IgnorePickupTeamRestriction()
	{
		return (!m_abilityMod) ? m_ignorePickupTeamRestriction : m_abilityMod.m_ignorePickupTeamRestrictionMod.GetModifiedValue(m_ignorePickupTeamRestriction);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, 1);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			AccumulateDamageFromTargeter(targetActor, base.Targeter, dictionary);
		}
		else
		{
			for (int i = 0; i <= currentTargeterIndex; i++)
			{
				AccumulateDamageFromTargeter(targetActor, base.Targeters[i], dictionary);
			}
		}
		return dictionary;
	}

	private void AccumulateDamageFromTargeter(ActorData targetActor, AbilityUtil_Targeter targeter, Dictionary<AbilityTooltipSymbol, int> symbolToDamage)
	{
		AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = targeter as AbilityUtil_Targeter_ThiefFanLaser;
		int num;
		if (abilityUtil_Targeter_ThiefFanLaser != null)
		{
			if (abilityUtil_Targeter_ThiefFanLaser.m_powerupsHitSoFar != null)
			{
				num = ((abilityUtil_Targeter_ThiefFanLaser.m_powerupsHitSoFar.Count > 0) ? 1 : 0);
				goto IL_0042;
			}
		}
		num = 0;
		goto IL_0042;
		IL_0042:
		bool flag = (byte)num != 0;
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return;
		}
		while (true)
		{
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				int tooltipSubjectCountOnActor = targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
				int num2 = 0;
				if (tooltipSubjectCountOnActor > 0)
				{
					num2 = GetLaserDamageAmount() + (tooltipSubjectCountOnActor - 1) * GetLaserSubsequentDamageAmount();
					if (flag)
					{
						num2 += GetExtraDamageForHittingPowerup();
					}
					if (tooltipSubjectCountOnActor == 1)
					{
						num2 += GetExtraDamageForSingleHit();
					}
					symbolToDamage[AbilityTooltipSymbol.Damage] = num2;
				}
			}
			if (!(targetActor == base.ActorData))
			{
				return;
			}
			while (true)
			{
				if (flag)
				{
					symbolToDamage[AbilityTooltipSymbol.Healing] = GetHealOnSelfIfHitEnemyAndPowerup();
				}
				return;
			}
		}
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		int energyGainPerLaserHit = GetEnergyGainPerLaserHit();
		int energyGainPerPowerupHit = GetEnergyGainPerPowerupHit();
		for (int i = 0; i < base.Targeters.Count; i++)
		{
			if (i <= currentTargeterIndex)
			{
				AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = base.Targeters[i] as AbilityUtil_Targeter_ThiefFanLaser;
				if (abilityUtil_Targeter_ThiefFanLaser == null)
				{
					continue;
				}
				for (int j = 0; j < abilityUtil_Targeter_ThiefFanLaser.m_hitPowerupInLaser.Count; j++)
				{
					if (j >= abilityUtil_Targeter_ThiefFanLaser.m_hitActorInLaser.Count)
					{
						break;
					}
					if (abilityUtil_Targeter_ThiefFanLaser.m_hitPowerupInLaser[j])
					{
						num += energyGainPerPowerupHit;
					}
				}
				if (energyGainPerLaserHit > 0)
				{
					int num2 = 0;
					foreach (KeyValuePair<ActorData, int> item in abilityUtil_Targeter_ThiefFanLaser.m_actorToHitCount)
					{
						num2 += item.Value;
					}
					num += num2 * energyGainPerLaserHit;
				}
				continue;
			}
			break;
		}
		return num;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefBasicAttack abilityMod_ThiefBasicAttack = modAsBase as AbilityMod_ThiefBasicAttack;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ThiefBasicAttack)
		{
			val = abilityMod_ThiefBasicAttack.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			val = m_laserDamageAmount;
		}
		AddTokenInt(tokens, "LaserDamageAmount", empty, val);
		AddTokenInt(tokens, "LaserSubsequentDamageAmount", string.Empty, (!abilityMod_ThiefBasicAttack) ? m_laserSubsequentDamageAmount : abilityMod_ThiefBasicAttack.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount));
		AddTokenInt(tokens, "LaserDamageTotalCombined", string.Empty, m_laserDamageAmount + m_laserSubsequentDamageAmount);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ThiefBasicAttack)
		{
			val2 = abilityMod_ThiefBasicAttack.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit);
		}
		else
		{
			val2 = m_extraDamageForSingleHit;
		}
		AddTokenInt(tokens, "ExtraDamageForSingleHit", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_ThiefBasicAttack)
		{
			val3 = abilityMod_ThiefBasicAttack.m_extraDamageForHittingPowerupMod.GetModifiedValue(m_extraDamageForHittingPowerup);
		}
		else
		{
			val3 = m_extraDamageForHittingPowerup;
		}
		AddTokenInt(tokens, "ExtraDamageForHittingPowerup", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_ThiefBasicAttack)
		{
			val4 = abilityMod_ThiefBasicAttack.m_healOnSelfIfHitEnemyAndPowerupMod.GetModifiedValue(m_healOnSelfIfHitEnemyAndPowerup);
		}
		else
		{
			val4 = m_healOnSelfIfHitEnemyAndPowerup;
		}
		AddTokenInt(tokens, "HealOnSelfIfHitEnemyAndPowerup", empty4, val4);
		AddTokenInt(tokens, "EnergyGainPerLaserHit", string.Empty, (!abilityMod_ThiefBasicAttack) ? m_energyGainPerLaserHit : abilityMod_ThiefBasicAttack.m_energyGainPerLaserHitMod.GetModifiedValue(m_energyGainPerLaserHit));
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_ThiefBasicAttack)
		{
			val5 = abilityMod_ThiefBasicAttack.m_energyGainPerPowerupHitMod.GetModifiedValue(m_energyGainPerPowerupHit);
		}
		else
		{
			val5 = m_energyGainPerPowerupHit;
		}
		AddTokenInt(tokens, "EnergyGainPerPowerupHit", empty5, val5);
		string empty6 = string.Empty;
		int val6;
		if ((bool)abilityMod_ThiefBasicAttack)
		{
			val6 = abilityMod_ThiefBasicAttack.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets);
		}
		else
		{
			val6 = m_laserMaxTargets;
		}
		AddTokenInt(tokens, "LaserMaxTargets", empty6, val6);
		string empty7 = string.Empty;
		int val7;
		if ((bool)abilityMod_ThiefBasicAttack)
		{
			val7 = abilityMod_ThiefBasicAttack.m_laserCountMod.GetModifiedValue(m_laserCount);
		}
		else
		{
			val7 = m_laserCount;
		}
		AddTokenInt(tokens, "LaserCount", empty7, val7);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = m_targeterMinInterpDistance * Board.Get().squareSize;
		max = m_targeterMaxInterpDistance * Board.Get().squareSize;
		return true;
	}

	private List<ActorData> GetHitActorsInDirection(Vector3 direction, ActorData caster, HashSet<PowerUp> powerupsHitPreviously, out VectorUtils.LaserCoords endPoints, out List<PowerUp> powerupsHit, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return GetHitActorsInDirectionStatic(caster.GetTravelBoardSquareWorldPositionForLos(), direction, caster, GetLaserRange(), GetLaserWidth(), LaserPenetrateLos(), GetLaserMaxTargets(), false, true, true, c_maxPowerupPerLaser, true, StopOnPowerupHit(), IncludeSpoilsPowerups(), IgnorePickupTeamRestriction(), powerupsHitPreviously, out endPoints, out powerupsHit, nonActorTargetInfo, false);
	}

	public static List<ActorData> GetHitActorsInDirectionStatic(Vector3 startLosCheckPos, Vector3 direction, ActorData caster, float distanceInSquares, float widthInSquares, bool penetrateLos, int maxActorTargets, bool includeAllies, bool includeEnemies, bool includeInvisibles, int maxPowerupsCount, bool shouldIncludePowerups, bool stopOnPowerupHit, bool includeSpoils, bool ignoreTeamRestriction, HashSet<PowerUp> powerupsHitSoFar, out VectorUtils.LaserCoords outEndPoints, out List<PowerUp> outPowerupsHit, List<NonActorTargetInfo> nonActorTargetInfo, bool forClient, bool stopEndPosOnHitActor = true)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, includeAllies, includeEnemies);
		List<PowerUp> powerups = new List<PowerUp>();
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = startLosCheckPos;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, direction, distanceInSquares, widthInSquares, caster, relevantTeams, penetrateLos, maxActorTargets, false, includeInvisibles, out laserCoords.end, nonActorTargetInfo);
		List<ActorData> list = actorsInLaser;
		Vector3 end = laserCoords.end;
		if (maxActorTargets > 0 && actorsInLaser.Count > 0)
		{
			if (stopEndPosOnHitActor)
			{
				laserCoords.end = actorsInLaser[actorsInLaser.Count - 1].GetTravelBoardSquareWorldPositionForLos();
			}
		}
		if (shouldIncludePowerups)
		{
			List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(laserCoords.start, end, widthInSquares / 2f, true, caster);
			using (List<BoardSquare>.Enumerator enumerator = squaresInBox.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare current = enumerator.Current;
					PowerUp powerUp = null;
					if (!forClient)
					{
						List<PowerUp> serverPowerUpsOnSquare = PowerUpManager.Get().GetServerPowerUpsOnSquare(current);
						foreach (PowerUp item in serverPowerUpsOnSquare)
						{
							if (CanPowerupBeStolen(item, powerupsHitSoFar, ignoreTeamRestriction, caster))
							{
								powerUp = item;
							}
						}
					}
					else
					{
						List<PowerUp> clientPowerUpsOnSquare = PowerUpManager.Get().GetClientPowerUpsOnSquare(current);
						using (List<PowerUp>.Enumerator enumerator3 = clientPowerUpsOnSquare.GetEnumerator())
						{
							while (true)
							{
								if (!enumerator3.MoveNext())
								{
									break;
								}
								PowerUp current3 = enumerator3.Current;
								if (CanPowerupBeStolen(current3, powerupsHitSoFar, ignoreTeamRestriction, caster))
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											break;
										default:
											powerUp = current3;
											goto end_IL_0165;
										}
									}
								}
							}
							end_IL_0165:;
						}
					}
					if (CanPowerupBeStolen(powerUp, powerupsHitSoFar, ignoreTeamRestriction, caster))
					{
						if (!powerups.Contains(powerUp))
						{
							if (!powerUp.m_isSpoil || includeSpoils)
							{
								powerups.Add(powerUp);
							}
						}
					}
				}
			}
			if (powerups.Count > 0)
			{
				TargeterUtils.SortPowerupsByDistanceToPos(ref powerups, startLosCheckPos);
				if (maxPowerupsCount > 0 && powerups.Count > maxPowerupsCount)
				{
					int count = powerups.Count - maxPowerupsCount;
					powerups.RemoveRange(maxPowerupsCount, count);
				}
				if (stopOnPowerupHit)
				{
					PowerUp powerUp2 = powerups[0];
					float magnitude = (powerUp2.boardSquare.ToVector3() - startLosCheckPos).magnitude;
					if (list.Count > 0)
					{
						float magnitude2 = (list[0].GetTravelBoardSquareWorldPositionForLos() - startLosCheckPos).magnitude;
						if (magnitude < magnitude2)
						{
							list.Clear();
						}
					}
					laserCoords.end = powerUp2.boardSquare.ToVector3();
				}
				powerupsHitSoFar.UnionWith(powerups);
			}
		}
		outEndPoints = laserCoords;
		outPowerupsHit = powerups;
		return list;
	}

	private static bool CanPowerupBeStolen(PowerUp powerUp, HashSet<PowerUp> powerupsHitSoFar, bool ignoreTeamRestriction, ActorData thief)
	{
		if (!(powerUp == null))
		{
			if (!(powerUp.boardSquare == null))
			{
				if (!ignoreTeamRestriction && !powerUp.TeamAllowedForPickUp(thief.GetTeam()))
				{
					return false;
				}
				if (powerupsHitSoFar.Contains(powerUp))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (!powerUp.CanBeStolen())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				return true;
			}
		}
		return false;
	}

	private float CalculateFanAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float value = (currentTarget.FreePos - targetingActor.GetTravelBoardSquareWorldPosition()).magnitude / Board.Get().squareSize;
		float num = Mathf.Clamp(value, m_targeterMinInterpDistance, m_targeterMaxInterpDistance) - m_targeterMinInterpDistance;
		return GetTargeterMaxAngle() * (1f - num / (m_targeterMaxInterpDistance - m_targeterMinInterpDistance));
	}

	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(fanAngleDegrees, GetTargeterMaxAngle(), m_targeterMinInterpDistance, m_targeterMaxInterpDistance);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ThiefBasicAttack))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ThiefBasicAttack);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
