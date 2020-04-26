using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BattleMonkHamstring : Ability
{
	[Header("-- Laser")]
	public int m_laserDamageAmount = 5;

	public int m_damageAfterFirstHit;

	public LaserTargetingInfo m_laserInfo;

	public StandardEffectInfo m_laserHitEffect;

	[Header("-- Explosion")]
	public bool m_explodeOnActorHit;

	public AbilityAreaShape m_explodeShape = AbilityAreaShape.Three_x_Three;

	public int m_explosionDamageAmount;

	public StandardEffectInfo m_explosionHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSelfSequencePrefab;

	public GameObject m_projectileSequencePrefab;

	private AbilityMod_BattleMonkHamstring m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Hamstring";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (ShouldExplodeOnActorHit())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					base.Targeter = new AbilityUtil_Targeter_LaserWithShape(this, GetExplodeShape(), GetLaserWidth(), GetLaserRange(), m_laserInfo.penetrateLos, GetMaxTargets(), m_laserInfo.affectsAllies, m_laserInfo.affectsCaster, m_laserInfo.affectsEnemies);
					return;
				}
			}
		}
		if (GetMaxBounces() > 0)
		{
			base.Targeter = new AbilityUtil_Targeter_BounceLaser(this, GetLaserWidth(), GetDistancePerBounce(), GetLaserRange(), GetMaxBounces(), GetMaxTargets(), false);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserRange(), m_laserInfo.penetrateLos, GetMaxTargets(), m_laserInfo.affectsAllies, m_laserInfo.affectsCaster);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	public int GetLaserDamage()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount) : m_laserDamageAmount;
	}

	public int GetDamageAfterFirstHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAfterFirstHitMod.GetModifiedValue(m_damageAfterFirstHit);
		}
		else
		{
			result = m_damageAfterFirstHit;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_widthMod.GetModifiedValue(m_laserInfo.width) : m_laserInfo.width;
	}

	public float GetLaserRange()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_laserInfo.range;
		}
		else
		{
			result = m_abilityMod.m_rangeMod.GetModifiedValue(m_laserInfo.range);
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_laserInfo.maxTargets;
		}
		else
		{
			result = m_abilityMod.m_maxTargetMod.GetModifiedValue(m_laserInfo.maxTargets);
		}
		return result;
	}

	public int GetMaxBounces()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_maxBounces.GetModifiedValue(0);
		}
		return result;
	}

	public float GetDistancePerBounce()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = 0f;
		}
		else
		{
			result = m_abilityMod.m_distancePerBounce.GetModifiedValue(0f);
		}
		return result;
	}

	public GameObject GetProjectileSequence()
	{
		GameObject result;
		if (m_abilityMod == null)
		{
			result = m_projectileSequencePrefab;
		}
		else
		{
			result = m_abilityMod.m_projectileSequencePrefab.GetModifiedValue(m_projectileSequencePrefab);
		}
		return result;
	}

	public bool ShouldExplodeOnActorHit()
	{
		bool result;
		if (m_abilityMod == null)
		{
			result = m_explodeOnActorHit;
		}
		else
		{
			result = m_abilityMod.m_explodeOnActorHitMod.GetModifiedValue(m_explodeOnActorHit);
		}
		return result;
	}

	public int GetExplosionDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_explosionDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount);
		}
		return result;
	}

	public AbilityAreaShape GetExplodeShape()
	{
		AbilityAreaShape result;
		if (m_abilityMod == null)
		{
			result = m_explodeShape;
		}
		else
		{
			result = m_abilityMod.m_explodeShapeMod.GetModifiedValue(m_explodeShape);
		}
		return result;
	}

	public int CalcDamageForOrderIndex(int hitOrder)
	{
		int damageAfterFirstHit = GetDamageAfterFirstHit();
		if (damageAfterFirstHit > 0)
		{
			if (hitOrder > 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return damageAfterFirstHit;
					}
				}
			}
		}
		return GetLaserDamage();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		if (m_explodeOnActorHit)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_explosionDamageAmount);
			m_explosionHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		}
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetLaserDamage());
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		if (ShouldExplodeOnActorHit())
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetExplosionDamage());
			m_explosionHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		}
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
		{
			if (base.Targeter is AbilityUtil_Targeter_LaserWithShape)
			{
				List<ActorData> lastLaserHitActors = (base.Targeter as AbilityUtil_Targeter_LaserWithShape).GetLastLaserHitActors();
				for (int i = 0; i < lastLaserHitActors.Count; i++)
				{
					if (targetActor == lastLaserHitActors[i])
					{
						results.m_damage = CalcDamageForOrderIndex(i);
						break;
					}
				}
			}
			else if (base.Targeter is AbilityUtil_Targeter_BounceLaser)
			{
				ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = (base.Targeter as AbilityUtil_Targeter_BounceLaser).GetHitActorContext();
				for (int j = 0; j < hitActorContext.Count; j++)
				{
					AbilityUtil_Targeter_BounceLaser.HitActorContext hitActorContext2 = hitActorContext[j];
					if (hitActorContext2.actor == targetActor)
					{
						results.m_damage = CalcDamageForOrderIndex(j);
						break;
					}
				}
			}
			else if (base.Targeter is AbilityUtil_Targeter_Laser)
			{
				List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext3 = (base.Targeter as AbilityUtil_Targeter_Laser).GetHitActorContext();
				for (int k = 0; k < hitActorContext3.Count; k++)
				{
					AbilityUtil_Targeter_Laser.HitActorContext hitActorContext4 = hitActorContext3[k];
					if (hitActorContext4.actor == targetActor)
					{
						results.m_damage = CalcDamageForOrderIndex(k);
						break;
					}
				}
			}
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0)
		{
			results.m_damage = GetExplosionDamage();
		}
		return true;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
					{
						dictionary[AbilityTooltipSymbol.Damage] = GetLaserDamage();
					}
					else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
					{
						dictionary[AbilityTooltipSymbol.Damage] = GetExplosionDamage();
					}
					return dictionary;
				}
				}
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkHamstring abilityMod_BattleMonkHamstring = modAsBase as AbilityMod_BattleMonkHamstring;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_BattleMonkHamstring)
		{
			val = abilityMod_BattleMonkHamstring.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			val = m_laserDamageAmount;
		}
		AddTokenInt(tokens, "LaserDamageAmount", empty, val);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_BattleMonkHamstring)
		{
			if (abilityMod_BattleMonkHamstring.m_useLaserHitEffectOverride)
			{
				effectInfo = abilityMod_BattleMonkHamstring.m_laserHitEffectOverride;
				goto IL_0086;
			}
		}
		effectInfo = m_laserHitEffect;
		goto IL_0086;
		IL_0101:
		StandardEffectInfo effectInfo2;
		AbilityMod.AddToken_EffectInfo(tokens, (StandardEffectInfo)effectInfo2, "ExplosionHitEffect", m_explosionHitEffect);
		return;
		IL_0086:
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "LaserHitEffect", m_laserHitEffect);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_BattleMonkHamstring)
		{
			val2 = abilityMod_BattleMonkHamstring.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount);
		}
		else
		{
			val2 = m_explosionDamageAmount;
		}
		AddTokenInt(tokens, "ExplosionDamageAmount", empty2, val2);
		if ((bool)abilityMod_BattleMonkHamstring)
		{
			if (abilityMod_BattleMonkHamstring.m_useExplosionHitEffectOverride)
			{
				effectInfo2 = abilityMod_BattleMonkHamstring.m_explosionHitEffectOverride;
				goto IL_0101;
			}
		}
		effectInfo2 = m_explosionHitEffect;
		goto IL_0101;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BattleMonkHamstring))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_BattleMonkHamstring);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
