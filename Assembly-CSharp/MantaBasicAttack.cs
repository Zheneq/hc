using System.Collections.Generic;
using UnityEngine;

public class MantaBasicAttack : Ability
{
	[Header("-- Targeting")]
	public float m_coneWidthAngle = 180f;

	public float m_coneBackwardOffset;

	public float m_coneLengthInner = 1.5f;

	public float m_coneLengthThroughWalls = 2.5f;

	[Header("-- Damage")]
	public int m_damageAmountInner = 28;

	public int m_damageAmountThroughWalls = 10;

	public StandardEffectInfo m_effectInner;

	public StandardEffectInfo m_effectOuter;

	[Header("-- Sequences")]
	public GameObject m_throughWallsConeSequence;

	private Manta_SyncComponent m_syncComp;

	private AbilityMod_MantaBasicAttack m_abilityMod;

	private int c_innerConeIdentifier = 1;

	private StandardEffectInfo m_cachedEffectInner;

	private StandardEffectInfo m_cachedEffectOuter;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Crush & Quake";
		}
		m_syncComp = GetComponent<Manta_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		float coneWidthAngle = GetConeWidthAngle();
		List<AbilityUtil_Targeter_MultipleCones.ConeDimensions> list = new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>();
		list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneWidthAngle, GetConeLengthInner()));
		list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneWidthAngle, GetConeLengthThroughWalls()));
		base.Targeter = new AbilityUtil_Targeter_MultipleCones(this, list, m_coneBackwardOffset, true, true);
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Art --</color>\nOn Sequence, for HitActorGroupOnAnimEventSequence components, use:\n" + c_innerConeIdentifier + " for Inner cone group identifier\n";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeLengthThroughWalls();
	}

	private void SetCachedFields()
	{
		m_cachedEffectInner = ((!m_abilityMod) ? m_effectInner : m_abilityMod.m_effectInnerMod.GetModifiedValue(m_effectInner));
		m_cachedEffectOuter = ((!m_abilityMod) ? m_effectOuter : m_abilityMod.m_effectOuterMod.GetModifiedValue(m_effectOuter));
	}

	public float GetConeWidthAngle()
	{
		return (!m_abilityMod) ? m_coneWidthAngle : m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle);
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
		}
		else
		{
			result = m_coneBackwardOffset;
		}
		return result;
	}

	public float GetConeLengthInner()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneLengthInnerMod.GetModifiedValue(m_coneLengthInner);
		}
		else
		{
			result = m_coneLengthInner;
		}
		return result;
	}

	public float GetConeLengthThroughWalls()
	{
		return (!m_abilityMod) ? m_coneLengthThroughWalls : m_abilityMod.m_coneLengthThroughWallsMod.GetModifiedValue(m_coneLengthThroughWalls);
	}

	public int GetDamageAmountInner()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAmountInnerMod.GetModifiedValue(m_damageAmountInner);
		}
		else
		{
			result = m_damageAmountInner;
		}
		return result;
	}

	public int GetDamageAmountThroughWalls()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAmountThroughWallsMod.GetModifiedValue(m_damageAmountThroughWalls);
		}
		else
		{
			result = m_damageAmountThroughWalls;
		}
		return result;
	}

	public int GetExtraDamageNoLoS()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageNoLoSMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public StandardEffectInfo GetEffectInner()
	{
		StandardEffectInfo result;
		if (m_cachedEffectInner != null)
		{
			result = m_cachedEffectInner;
		}
		else
		{
			result = m_effectInner;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOuter()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOuter != null)
		{
			result = m_cachedEffectOuter;
		}
		else
		{
			result = m_effectOuter;
		}
		return result;
	}

	public StandardEffectInfo GetAdditionalDirtyFightingExplosionEffect()
	{
		if ((bool)m_abilityMod)
		{
			if (m_abilityMod.m_additionalDirtyFightingExplosionEffect.operation == AbilityModPropertyEffectInfo.ModOp.Override)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return m_abilityMod.m_additionalDirtyFightingExplosionEffect.effectInfo;
					}
				}
			}
		}
		return null;
	}

	public bool ShouldDisruptBrushInCone()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = (m_abilityMod.m_disruptBrushInConeMod.GetModifiedValue(false) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_MantaBasicAttack))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_MantaBasicAttack);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Near, m_damageAmountInner));
		m_effectInner.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Near);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, m_damageAmountThroughWalls));
		m_effectOuter.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Far);
		return numbers;
	}

	public override List<int> _001D()
	{
		List<int> list = base._001D();
		int num = Mathf.Abs(m_damageAmountInner - m_damageAmountThroughWalls);
		if (num != 0)
		{
			list.Add(num);
		}
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmountInner", string.Empty, m_damageAmountInner);
		AddTokenInt(tokens, "DamageAmountThroughWalls", string.Empty, m_damageAmountThroughWalls);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectInner, "EffectInner", m_effectInner);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOuter, "EffectOuter", m_effectOuter);
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			int num = 0;
			if (!base.ActorData.CurrentBoardSquare._0013(targetActor.CurrentBoardSquare.x, targetActor.CurrentBoardSquare.y))
			{
				num += GetExtraDamageNoLoS();
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Near))
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmountInner() + num;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Far))
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmountThroughWalls() + num;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					int num = 0;
					List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
					using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AbilityUtil_Targeter.ActorTarget current = enumerator.Current;
							num += m_syncComp.GetDirtyFightingExtraTP(current.m_actor);
						}
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return num;
							}
						}
					}
				}
				}
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
		{
			if (m_syncComp != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return m_syncComp.GetAccessoryStringForDamage(targetActor, base.ActorData, this);
					}
				}
			}
		}
		return null;
	}

	public override bool ForceIgnoreCover(ActorData targetActor)
	{
		if (targetActor != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject.Far, targetActor, base.ActorData.GetTravelBoardSquareWorldPosition(), base.ActorData);
				}
			}
		}
		return false;
	}

	private bool InsideNearRadius(ActorData targetActor, Vector3 damageOrigin)
	{
		float num = GetConeLengthInner() * Board.Get().squareSize;
		Vector3 vector = targetActor.GetTravelBoardSquareWorldPosition() - damageOrigin;
		vector.y = 0f;
		float num2 = vector.magnitude;
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			num2 -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
		}
		return num2 <= num;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.Near)
		{
			if (subjectType != AbilityTooltipSubject.Far)
			{
				return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
			}
		}
		if (targetingActor.CurrentBoardSquare._0013(targetActor.CurrentBoardSquare.x, targetActor.CurrentBoardSquare.y))
		{
			if (InsideNearRadius(targetActor, damageOrigin))
			{
				return subjectType == AbilityTooltipSubject.Near;
			}
			return subjectType == AbilityTooltipSubject.Far;
		}
		return subjectType == AbilityTooltipSubject.Far;
	}
}
