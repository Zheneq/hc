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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Hunting Crossbow";
		}
		m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		if (m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserLength(), m_laserInfo.penetrateLos, GetLaserMaxTargets());
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserLength();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerHuntingCrossbow abilityMod_TrackerHuntingCrossbow = modAsBase as AbilityMod_TrackerHuntingCrossbow;
		int num;
		if ((bool)abilityMod_TrackerHuntingCrossbow)
		{
			num = abilityMod_TrackerHuntingCrossbow.m_damageOnUntrackedMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			num = m_laserDamageAmount;
		}
		int val = num;
		tokens.Add(new TooltipTokenInt("Damage", "damage amount", val));
		GetHuntedEffect().AddTooltipTokens(tokens, "TrackedEffect");
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		GetHuntedEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				if (m_droneTracker != null)
				{
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					int num;
					if (m_droneTracker.IsTrackingActor(targetActor.ActorIndex))
					{
						num = GetDamageOnTracked();
					}
					else
					{
						num = GetDamageOnUntracked();
					}
					int num2 = num;
					if (m_abilityMod != null)
					{
						ActorData actorData = base.ActorData;
						bool num3;
						if (m_abilityMod.m_requireFunctioningBrush)
						{
							num3 = actorData.IsHiddenInBrush();
						}
						else
						{
							num3 = actorData.GetCurrentBoardSquare().IsInBrushRegion();
						}
						if (num3)
						{
							num2 += GetExtraDamageWhileInBrush();
						}
						if (GetDamageChangeAfterFirstHit() != 0)
						{
							AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = base.Targeter as AbilityUtil_Targeter_Laser;
							List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext = abilityUtil_Targeter_Laser.GetHitActorContext();
							using (List<AbilityUtil_Targeter_Laser.HitActorContext>.Enumerator enumerator = hitActorContext.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									AbilityUtil_Targeter_Laser.HitActorContext current = enumerator.Current;
									if (current.actor == targetActor && current.hitOrderIndex != 0)
									{
										num2 += GetDamageChangeAfterFirstHit();
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
		if (abilityMod.GetType() != typeof(AbilityMod_TrackerHuntingCrossbow))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_TrackerHuntingCrossbow);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private int GetDamageOnUntracked()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_laserDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageOnUntrackedMod.GetModifiedValue(m_laserDamageAmount);
		}
		return result;
	}

	private int GetDamageOnTracked()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_laserDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageOnTrackedMod.GetModifiedValue(m_laserDamageAmount);
		}
		return result;
	}

	private int GetDamageChangeAfterFirstHit()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_damageChangeOnSubsequentTargets : 0;
	}

	private int GetExtraDamageWhileInBrush()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = Mathf.Max(0, m_abilityMod.m_extraDamageWhenInBrush);
		}
		return result;
	}

	private int GetLaserMaxTargets()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(m_laserInfo.maxTargets) : m_laserInfo.maxTargets;
	}

	private float GetLaserWidth()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserInfo.width) : m_laserInfo.width;
	}

	private float GetLaserLength()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_laserLengthMod.GetModifiedValue(m_laserInfo.range) : m_laserInfo.range;
	}

	public StandardActorEffectData GetHuntedEffect()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_huntedEffectDataOverride.GetModifiedValue(m_huntedEffectData) : m_huntedEffectData;
	}
}
