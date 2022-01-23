using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Strong Arms";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		float targeterMaxAngle = this.GetTargeterMaxAngle();
		bool stopOnPowerUp = this.StopOnPowerupHit();
		bool flag = this.GetHealOnSelfIfHitEnemyAndPowerup() > 0;
		if (this.TargeterMultiTarget())
		{
			base.ClearTargeters();
			for (int i = 0; i < this.GetLaserCount(); i++)
			{
				AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = new AbilityUtil_Targeter_ThiefFanLaser(this, 0f, targeterMaxAngle, this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance, this.GetLaserRange(), this.GetLaserWidth(), this.GetLaserMaxTargets(), this.GetLaserCount(), this.LaserPenetrateLos(), true, stopOnPowerUp, this.IncludeSpoilsPowerups(), this.IgnorePickupTeamRestriction(), this.c_maxPowerupPerLaser, 0f, 0f);
				abilityUtil_Targeter_ThiefFanLaser.SetUseMultiTargetUpdate(true);
				if (flag)
				{
					AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser2 = abilityUtil_Targeter_ThiefFanLaser;
					
					abilityUtil_Targeter_ThiefFanLaser2.m_affectCasterDelegate = delegate(ActorData caster, bool hitEnemy, bool hitPowerup)
						{
							bool result;
							if (hitEnemy)
							{
								result = hitPowerup;
							}
							else
							{
								result = false;
							}
							return result;
						};
				}
				base.Targeters.Add(abilityUtil_Targeter_ThiefFanLaser);
			}
		}
		else
		{
			AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser3 = new AbilityUtil_Targeter_ThiefFanLaser(this, 0f, targeterMaxAngle, this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance, this.GetLaserRange(), this.GetLaserWidth(), this.GetLaserMaxTargets(), this.GetLaserCount(), this.LaserPenetrateLos(), true, stopOnPowerUp, this.IncludeSpoilsPowerups(), this.IgnorePickupTeamRestriction(), this.c_maxPowerupPerLaser, 0f, 0f);
			if (flag)
			{
				AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser4 = abilityUtil_Targeter_ThiefFanLaser3;
				
				abilityUtil_Targeter_ThiefFanLaser4.m_affectCasterDelegate = delegate(ActorData caster, bool hitEnemy, bool hitPowerup)
					{
						bool result;
						if (hitEnemy)
						{
							result = hitPowerup;
						}
						else
						{
							result = false;
						}
						return result;
					};
			}
			base.Targeter = abilityUtil_Targeter_ThiefFanLaser3;
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return (!this.TargeterMultiTarget()) ? 1 : this.GetLaserCount();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRange();
	}

	public bool TargeterMultiTarget()
	{
		return this.m_targeterMultiTarget;
	}

	public float GetTargeterMaxAngle()
	{
		float a = 1f;
		float b;
		if (this.m_abilityMod)
		{
			b = this.m_abilityMod.m_targeterMaxAngleMod.GetModifiedValue(this.m_targeterMaxAngle);
		}
		else
		{
			b = this.m_targeterMaxAngle;
		}
		return Mathf.Max(a, b);
	}

	public int GetLaserDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_laserDamageAmount : this.m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount);
	}

	public int GetLaserSubsequentDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserSubsequentDamageAmountMod.GetModifiedValue(this.m_laserSubsequentDamageAmount);
		}
		else
		{
			result = this.m_laserSubsequentDamageAmount;
		}
		return result;
	}

	public int GetExtraDamageForSingleHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(this.m_extraDamageForSingleHit);
		}
		else
		{
			result = this.m_extraDamageForSingleHit;
		}
		return result;
	}

	public int GetExtraDamageForHittingPowerup()
	{
		return (!this.m_abilityMod) ? this.m_extraDamageForHittingPowerup : this.m_abilityMod.m_extraDamageForHittingPowerupMod.GetModifiedValue(this.m_extraDamageForHittingPowerup);
	}

	public int GetHealOnSelfIfHitEnemyAndPowerup()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healOnSelfIfHitEnemyAndPowerupMod.GetModifiedValue(this.m_healOnSelfIfHitEnemyAndPowerup);
		}
		else
		{
			result = this.m_healOnSelfIfHitEnemyAndPowerup;
		}
		return result;
	}

	public int GetEnergyGainPerLaserHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_energyGainPerLaserHitMod.GetModifiedValue(this.m_energyGainPerLaserHit);
		}
		else
		{
			result = this.m_energyGainPerLaserHit;
		}
		return result;
	}

	public int GetEnergyGainPerPowerupHit()
	{
		return (!this.m_abilityMod) ? this.m_energyGainPerPowerupHit : this.m_abilityMod.m_energyGainPerPowerupHitMod.GetModifiedValue(this.m_energyGainPerPowerupHit);
	}

	public float GetLaserRange()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
		}
		else
		{
			result = this.m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public int GetLaserMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(this.m_laserMaxTargets);
		}
		else
		{
			result = this.m_laserMaxTargets;
		}
		return result;
	}

	public int GetLaserCount()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserCountMod.GetModifiedValue(this.m_laserCount);
		}
		else
		{
			result = this.m_laserCount;
		}
		return result;
	}

	public bool LaserPenetrateLos()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserPenetrateLosMod.GetModifiedValue(this.m_laserPenetrateLos);
		}
		else
		{
			result = this.m_laserPenetrateLos;
		}
		return result;
	}

	public bool StopOnPowerupHit()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_stopOnPowerupHitMod.GetModifiedValue(this.m_stopOnPowerupHit);
		}
		else
		{
			result = this.m_stopOnPowerupHit;
		}
		return result;
	}

	public bool IncludeSpoilsPowerups()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_includeSpoilsPowerupsMod.GetModifiedValue(this.m_includeSpoilsPowerups);
		}
		else
		{
			result = this.m_includeSpoilsPowerups;
		}
		return result;
	}

	public bool IgnorePickupTeamRestriction()
	{
		return (!this.m_abilityMod) ? this.m_ignorePickupTeamRestriction : this.m_abilityMod.m_ignorePickupTeamRestrictionMod.GetModifiedValue(this.m_ignorePickupTeamRestriction);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, 1);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, 1);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (this.GetExpectedNumberOfTargeters() < 2)
		{
			this.AccumulateDamageFromTargeter(targetActor, base.Targeter, dictionary);
		}
		else
		{
			for (int i = 0; i <= currentTargeterIndex; i++)
			{
				this.AccumulateDamageFromTargeter(targetActor, base.Targeters[i], dictionary);
			}
		}
		return dictionary;
	}

	private void AccumulateDamageFromTargeter(ActorData targetActor, AbilityUtil_Targeter targeter, Dictionary<AbilityTooltipSymbol, int> symbolToDamage)
	{
		AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = targeter as AbilityUtil_Targeter_ThiefFanLaser;
		bool flag;
		if (abilityUtil_Targeter_ThiefFanLaser != null)
		{
			if (abilityUtil_Targeter_ThiefFanLaser.m_powerupsHitSoFar != null)
			{
				flag = (abilityUtil_Targeter_ThiefFanLaser.m_powerupsHitSoFar.Count > 0);
				goto IL_42;
			}
		}
		flag = false;
		IL_42:
		bool flag2 = flag;
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				int tooltipSubjectCountOnActor = targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
				if (tooltipSubjectCountOnActor > 0)
				{
					int num = this.GetLaserDamageAmount() + (tooltipSubjectCountOnActor - 1) * this.GetLaserSubsequentDamageAmount();
					if (flag2)
					{
						num += this.GetExtraDamageForHittingPowerup();
					}
					if (tooltipSubjectCountOnActor == 1)
					{
						num += this.GetExtraDamageForSingleHit();
					}
					symbolToDamage[AbilityTooltipSymbol.Damage] = num;
				}
			}
			if (targetActor == base.ActorData)
			{
				if (flag2)
				{
					symbolToDamage[AbilityTooltipSymbol.Healing] = this.GetHealOnSelfIfHitEnemyAndPowerup();
				}
			}
		}
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		int energyGainPerLaserHit = this.GetEnergyGainPerLaserHit();
		int energyGainPerPowerupHit = this.GetEnergyGainPerPowerupHit();
		int i = 0;
		while (i < base.Targeters.Count)
		{
			if (i > currentTargeterIndex)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					return num;
				}
			}
			else
			{
				AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = base.Targeters[i] as AbilityUtil_Targeter_ThiefFanLaser;
				if (abilityUtil_Targeter_ThiefFanLaser != null)
				{
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
						foreach (KeyValuePair<ActorData, int> keyValuePair in abilityUtil_Targeter_ThiefFanLaser.m_actorToHitCount)
						{
							num2 += keyValuePair.Value;
						}
						num += num2 * energyGainPerLaserHit;
					}
				}
				i++;
			}
		}
		return num;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefBasicAttack abilityMod_ThiefBasicAttack = modAsBase as AbilityMod_ThiefBasicAttack;
		string name = "LaserDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_ThiefBasicAttack)
		{
			val = abilityMod_ThiefBasicAttack.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			val = this.m_laserDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "LaserSubsequentDamageAmount", string.Empty, (!abilityMod_ThiefBasicAttack) ? this.m_laserSubsequentDamageAmount : abilityMod_ThiefBasicAttack.m_laserSubsequentDamageAmountMod.GetModifiedValue(this.m_laserSubsequentDamageAmount), false);
		base.AddTokenInt(tokens, "LaserDamageTotalCombined", string.Empty, this.m_laserDamageAmount + this.m_laserSubsequentDamageAmount, false);
		string name2 = "ExtraDamageForSingleHit";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ThiefBasicAttack)
		{
			val2 = abilityMod_ThiefBasicAttack.m_extraDamageForSingleHitMod.GetModifiedValue(this.m_extraDamageForSingleHit);
		}
		else
		{
			val2 = this.m_extraDamageForSingleHit;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "ExtraDamageForHittingPowerup";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_ThiefBasicAttack)
		{
			val3 = abilityMod_ThiefBasicAttack.m_extraDamageForHittingPowerupMod.GetModifiedValue(this.m_extraDamageForHittingPowerup);
		}
		else
		{
			val3 = this.m_extraDamageForHittingPowerup;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "HealOnSelfIfHitEnemyAndPowerup";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_ThiefBasicAttack)
		{
			val4 = abilityMod_ThiefBasicAttack.m_healOnSelfIfHitEnemyAndPowerupMod.GetModifiedValue(this.m_healOnSelfIfHitEnemyAndPowerup);
		}
		else
		{
			val4 = this.m_healOnSelfIfHitEnemyAndPowerup;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		base.AddTokenInt(tokens, "EnergyGainPerLaserHit", string.Empty, (!abilityMod_ThiefBasicAttack) ? this.m_energyGainPerLaserHit : abilityMod_ThiefBasicAttack.m_energyGainPerLaserHitMod.GetModifiedValue(this.m_energyGainPerLaserHit), false);
		string name5 = "EnergyGainPerPowerupHit";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_ThiefBasicAttack)
		{
			val5 = abilityMod_ThiefBasicAttack.m_energyGainPerPowerupHitMod.GetModifiedValue(this.m_energyGainPerPowerupHit);
		}
		else
		{
			val5 = this.m_energyGainPerPowerupHit;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
		string name6 = "LaserMaxTargets";
		string empty6 = string.Empty;
		int val6;
		if (abilityMod_ThiefBasicAttack)
		{
			val6 = abilityMod_ThiefBasicAttack.m_laserMaxTargetsMod.GetModifiedValue(this.m_laserMaxTargets);
		}
		else
		{
			val6 = this.m_laserMaxTargets;
		}
		base.AddTokenInt(tokens, name6, empty6, val6, false);
		string name7 = "LaserCount";
		string empty7 = string.Empty;
		int val7;
		if (abilityMod_ThiefBasicAttack)
		{
			val7 = abilityMod_ThiefBasicAttack.m_laserCountMod.GetModifiedValue(this.m_laserCount);
		}
		else
		{
			val7 = this.m_laserCount;
		}
		base.AddTokenInt(tokens, name7, empty7, val7, false);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = this.m_targeterMinInterpDistance * Board.Get().squareSize;
		max = this.m_targeterMaxInterpDistance * Board.Get().squareSize;
		return true;
	}

	private List<ActorData> GetHitActorsInDirection(Vector3 direction, ActorData caster, HashSet<PowerUp> powerupsHitPreviously, out VectorUtils.LaserCoords endPoints, out List<PowerUp> powerupsHit, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return ThiefBasicAttack.GetHitActorsInDirectionStatic(caster.GetLoSCheckPos(), direction, caster, this.GetLaserRange(), this.GetLaserWidth(), this.LaserPenetrateLos(), this.GetLaserMaxTargets(), false, true, true, this.c_maxPowerupPerLaser, true, this.StopOnPowerupHit(), this.IncludeSpoilsPowerups(), this.IgnorePickupTeamRestriction(), powerupsHitPreviously, out endPoints, out powerupsHit, nonActorTargetInfo, false, true);
	}

	public unsafe static List<ActorData> GetHitActorsInDirectionStatic(Vector3 startLosCheckPos, Vector3 direction, ActorData caster, float distanceInSquares, float widthInSquares, bool penetrateLos, int maxActorTargets, bool includeAllies, bool includeEnemies, bool includeInvisibles, int maxPowerupsCount, bool shouldIncludePowerups, bool stopOnPowerupHit, bool includeSpoils, bool ignoreTeamRestriction, HashSet<PowerUp> powerupsHitSoFar, out VectorUtils.LaserCoords outEndPoints, out List<PowerUp> outPowerupsHit, List<NonActorTargetInfo> nonActorTargetInfo, bool forClient, bool stopEndPosOnHitActor = true)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, includeAllies, includeEnemies);
		List<PowerUp> list = new List<PowerUp>();
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = startLosCheckPos;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, direction, distanceInSquares, widthInSquares, caster, relevantTeams, penetrateLos, maxActorTargets, false, includeInvisibles, out laserCoords.end, nonActorTargetInfo, null, false, true);
		List<ActorData> list2 = actorsInLaser;
		Vector3 end = laserCoords.end;
		if (maxActorTargets > 0 && actorsInLaser.Count > 0)
		{
			if (stopEndPosOnHitActor)
			{
				laserCoords.end = actorsInLaser[actorsInLaser.Count - 1].GetLoSCheckPos();
			}
		}
		if (shouldIncludePowerups)
		{
			List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(laserCoords.start, end, widthInSquares / 2f, true, caster);
			using (List<BoardSquare>.Enumerator enumerator = squaresInBox.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare square = enumerator.Current;
					PowerUp powerUp = null;
					if (!forClient)
					{
						List<PowerUp> serverPowerUpsOnSquare = PowerUpManager.Get().GetServerPowerUpsOnSquare(square);
						foreach (PowerUp powerUp2 in serverPowerUpsOnSquare)
						{
							if (ThiefBasicAttack.CanPowerupBeStolen(powerUp2, powerupsHitSoFar, ignoreTeamRestriction, caster))
							{
								powerUp = powerUp2;
								break;
							}
						}
					}
					else
					{
						List<PowerUp> clientPowerUpsOnSquare = PowerUpManager.Get().GetClientPowerUpsOnSquare(square);
						using (List<PowerUp>.Enumerator enumerator3 = clientPowerUpsOnSquare.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								PowerUp powerUp3 = enumerator3.Current;
								if (ThiefBasicAttack.CanPowerupBeStolen(powerUp3, powerupsHitSoFar, ignoreTeamRestriction, caster))
								{
									powerUp = powerUp3;
									goto IL_1B5;
								}
							}
						}
					}
					IL_1B5:
					if (ThiefBasicAttack.CanPowerupBeStolen(powerUp, powerupsHitSoFar, ignoreTeamRestriction, caster))
					{
						if (!list.Contains(powerUp))
						{
							if (!powerUp.m_isSpoil || includeSpoils)
							{
								list.Add(powerUp);
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				TargeterUtils.SortPowerupsByDistanceToPos(ref list, startLosCheckPos);
				if (maxPowerupsCount > 0 && list.Count > maxPowerupsCount)
				{
					int count = list.Count - maxPowerupsCount;
					list.RemoveRange(maxPowerupsCount, count);
				}
				if (stopOnPowerupHit)
				{
					PowerUp powerUp4 = list[0];
					float magnitude = (powerUp4.boardSquare.ToVector3() - startLosCheckPos).magnitude;
					if (list2.Count > 0)
					{
						float magnitude2 = (list2[0].GetLoSCheckPos() - startLosCheckPos).magnitude;
						if (magnitude < magnitude2)
						{
							list2.Clear();
						}
					}
					laserCoords.end = powerUp4.boardSquare.ToVector3();
				}
				powerupsHitSoFar.UnionWith(list);
			}
		}
		outEndPoints = laserCoords;
		outPowerupsHit = list;
		return list2;
	}

	private static bool CanPowerupBeStolen(PowerUp powerUp, HashSet<PowerUp> powerupsHitSoFar, bool ignoreTeamRestriction, ActorData thief)
	{
		if (!(powerUp == null))
		{
			if (powerUp.boardSquare == null)
			{
			}
			else
			{
				if (!ignoreTeamRestriction && !powerUp.TeamAllowedForPickUp(thief.GetTeam()))
				{
					return false;
				}
				if (powerupsHitSoFar.Contains(powerUp))
				{
					return false;
				}
				if (!powerUp.CanBeStolen())
				{
					return false;
				}
				return true;
			}
		}
		return false;
	}

	private float CalculateFanAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float value = (currentTarget.FreePos - targetingActor.GetFreePos()).magnitude / Board.Get().squareSize;
		float num = Mathf.Clamp(value, this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance) - this.m_targeterMinInterpDistance;
		return this.GetTargeterMaxAngle() * (1f - num / (this.m_targeterMaxInterpDistance - this.m_targeterMinInterpDistance));
	}

	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(fanAngleDegrees, this.GetTargeterMaxAngle(), this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefBasicAttack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ThiefBasicAttack);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
