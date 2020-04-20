using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackerHuntingCrossbow : Ability
{
	public int m_laserDamageAmount = 5;

	public LaserTargetingInfo m_laserInfo;

	[Header("-- Effect Data for <Tracked> effect")]
	public StandardActorEffectData m_huntedEffectData;

	private TrackerDroneTrackerComponent m_droneTracker;

	private AbilityMod_TrackerHuntingCrossbow m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Hunting Crossbow";
		}
		this.m_droneTracker = base.GetComponent<TrackerDroneTrackerComponent>();
		if (this.m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Laser(this, this.GetLaserWidth(), this.GetLaserLength(), this.m_laserInfo.penetrateLos, this.GetLaserMaxTargets(), false, false);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserLength();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerHuntingCrossbow abilityMod_TrackerHuntingCrossbow = modAsBase as AbilityMod_TrackerHuntingCrossbow;
		int num;
		if (abilityMod_TrackerHuntingCrossbow)
		{
			num = abilityMod_TrackerHuntingCrossbow.m_damageOnUntrackedMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			num = this.m_laserDamageAmount;
		}
		int val = num;
		tokens.Add(new TooltipTokenInt("Damage", "damage amount", val));
		this.GetHuntedEffect().AddTooltipTokens(tokens, "TrackedEffect", false, null);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_laserDamageAmount);
		this.GetHuntedEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				if (this.m_droneTracker != null)
				{
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					bool flag = this.m_droneTracker.IsTrackingActor(targetActor.ActorIndex);
					int num;
					if (flag)
					{
						num = this.GetDamageOnTracked();
					}
					else
					{
						num = this.GetDamageOnUntracked();
					}
					int num2 = num;
					if (this.m_abilityMod != null)
					{
						ActorData actorData = base.ActorData;
						bool flag2;
						if (this.m_abilityMod.m_requireFunctioningBrush)
						{
							flag2 = actorData.IsHiddenInBrush();
						}
						else
						{
							flag2 = actorData.GetCurrentBoardSquare().IsInBrushRegion();
						}
						bool flag3 = flag2;
						if (flag3)
						{
							num2 += this.GetExtraDamageWhileInBrush();
						}
						if (this.GetDamageChangeAfterFirstHit() != 0)
						{
							AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = base.Targeter as AbilityUtil_Targeter_Laser;
							List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext = abilityUtil_Targeter_Laser.GetHitActorContext();
							using (List<AbilityUtil_Targeter_Laser.HitActorContext>.Enumerator enumerator = hitActorContext.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									AbilityUtil_Targeter_Laser.HitActorContext hitActorContext2 = enumerator.Current;
									if (hitActorContext2.actor == targetActor && hitActorContext2.hitOrderIndex != 0)
									{
										num2 += this.GetDamageChangeAfterFirstHit();
									}
								}
							}
						}
					}
					dictionary[AbilityTooltipSymbol.Damage] = Mathf.Max(0, num2);
				}
			}
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TrackerHuntingCrossbow))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_TrackerHuntingCrossbow);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private int GetDamageOnUntracked()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_laserDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageOnUntrackedMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		return result;
	}

	private int GetDamageOnTracked()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_laserDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageOnTrackedMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		return result;
	}

	private int GetDamageChangeAfterFirstHit()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_damageChangeOnSubsequentTargets : 0;
	}

	private int GetExtraDamageWhileInBrush()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = Mathf.Max(0, this.m_abilityMod.m_extraDamageWhenInBrush);
		}
		return result;
	}

	private int GetLaserMaxTargets()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(this.m_laserInfo.maxTargets) : this.m_laserInfo.maxTargets;
	}

	private float GetLaserWidth()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserInfo.width) : this.m_laserInfo.width;
	}

	private float GetLaserLength()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_laserLengthMod.GetModifiedValue(this.m_laserInfo.range) : this.m_laserInfo.range;
	}

	public StandardActorEffectData GetHuntedEffect()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_huntedEffectDataOverride.GetModifiedValue(this.m_huntedEffectData) : this.m_huntedEffectData;
	}
}
