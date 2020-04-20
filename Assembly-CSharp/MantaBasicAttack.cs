using System;
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
	public int m_damageAmountInner = 0x1C;

	public int m_damageAmountThroughWalls = 0xA;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Crush & Quake";
		}
		this.m_syncComp = base.GetComponent<Manta_SyncComponent>();
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		float coneWidthAngle = this.GetConeWidthAngle();
		base.Targeter = new AbilityUtil_Targeter_MultipleCones(this, new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>
		{
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneWidthAngle, this.GetConeLengthInner()),
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneWidthAngle, this.GetConeLengthThroughWalls())
		}, this.m_coneBackwardOffset, true, true, true, false, false);
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Art --</color>\nOn Sequence, for HitActorGroupOnAnimEventSequence components, use:\n" + this.c_innerConeIdentifier + " for Inner cone group identifier\n";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetConeLengthThroughWalls();
	}

	private void SetCachedFields()
	{
		this.m_cachedEffectInner = ((!this.m_abilityMod) ? this.m_effectInner : this.m_abilityMod.m_effectInnerMod.GetModifiedValue(this.m_effectInner));
		this.m_cachedEffectOuter = ((!this.m_abilityMod) ? this.m_effectOuter : this.m_abilityMod.m_effectOuterMod.GetModifiedValue(this.m_effectOuter));
	}

	public float GetConeWidthAngle()
	{
		return (!this.m_abilityMod) ? this.m_coneWidthAngle : this.m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(this.m_coneWidthAngle);
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
		}
		else
		{
			result = this.m_coneBackwardOffset;
		}
		return result;
	}

	public float GetConeLengthInner()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneLengthInnerMod.GetModifiedValue(this.m_coneLengthInner);
		}
		else
		{
			result = this.m_coneLengthInner;
		}
		return result;
	}

	public float GetConeLengthThroughWalls()
	{
		return (!this.m_abilityMod) ? this.m_coneLengthThroughWalls : this.m_abilityMod.m_coneLengthThroughWallsMod.GetModifiedValue(this.m_coneLengthThroughWalls);
	}

	public int GetDamageAmountInner()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageAmountInnerMod.GetModifiedValue(this.m_damageAmountInner);
		}
		else
		{
			result = this.m_damageAmountInner;
		}
		return result;
	}

	public int GetDamageAmountThroughWalls()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageAmountThroughWallsMod.GetModifiedValue(this.m_damageAmountThroughWalls);
		}
		else
		{
			result = this.m_damageAmountThroughWalls;
		}
		return result;
	}

	public int GetExtraDamageNoLoS()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageNoLoSMod.GetModifiedValue(0);
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
		if (this.m_cachedEffectInner != null)
		{
			result = this.m_cachedEffectInner;
		}
		else
		{
			result = this.m_effectInner;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOuter()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOuter != null)
		{
			result = this.m_cachedEffectOuter;
		}
		else
		{
			result = this.m_effectOuter;
		}
		return result;
	}

	public StandardEffectInfo GetAdditionalDirtyFightingExplosionEffect()
	{
		if (this.m_abilityMod)
		{
			if (this.m_abilityMod.m_additionalDirtyFightingExplosionEffect.operation == AbilityModPropertyEffectInfo.ModOp.Override)
			{
				return this.m_abilityMod.m_additionalDirtyFightingExplosionEffect.effectInfo;
			}
		}
		return null;
	}

	public bool ShouldDisruptBrushInCone()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_disruptBrushInConeMod.GetModifiedValue(false);
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaBasicAttack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_MantaBasicAttack);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Near, this.m_damageAmountInner));
		this.m_effectInner.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Near);
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, this.m_damageAmountThroughWalls));
		this.m_effectOuter.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Far);
		return list;
	}

	public override List<int> symbol_001D()
	{
		List<int> list = base.symbol_001D();
		int num = Mathf.Abs(this.m_damageAmountInner - this.m_damageAmountThroughWalls);
		if (num != 0)
		{
			list.Add(num);
		}
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "DamageAmountInner", string.Empty, this.m_damageAmountInner, false);
		base.AddTokenInt(tokens, "DamageAmountThroughWalls", string.Empty, this.m_damageAmountThroughWalls, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectInner, "EffectInner", this.m_effectInner, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOuter, "EffectOuter", this.m_effectOuter, true);
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			int num = 0;
			if (!base.ActorData.CurrentBoardSquare.symbol_0013(targetActor.CurrentBoardSquare.x, targetActor.CurrentBoardSquare.y))
			{
				num += this.GetExtraDamageNoLoS();
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Near))
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.GetDamageAmountInner() + num;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Far))
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.GetDamageAmountThroughWalls() + num;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.m_syncComp != null)
		{
			int num = 0;
			List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
			using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
					num += this.m_syncComp.GetDirtyFightingExtraTP(actorTarget.m_actor);
				}
			}
			return num;
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
		{
			if (this.m_syncComp != null)
			{
				return this.m_syncComp.GetAccessoryStringForDamage(targetActor, base.ActorData, this);
			}
		}
		return null;
	}

	public override bool ForceIgnoreCover(ActorData targetActor)
	{
		if (targetActor != null)
		{
			return this.DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject.Far, targetActor, base.ActorData.GetTravelBoardSquareWorldPosition(), base.ActorData);
		}
		return false;
	}

	private bool InsideNearRadius(ActorData targetActor, Vector3 damageOrigin)
	{
		float num = this.GetConeLengthInner() * Board.Get().squareSize;
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
		bool flag = targetingActor.CurrentBoardSquare.symbol_0013(targetActor.CurrentBoardSquare.x, targetActor.CurrentBoardSquare.y);
		bool result;
		if (flag)
		{
			if (this.InsideNearRadius(targetActor, damageOrigin))
			{
				result = (subjectType == AbilityTooltipSubject.Near);
			}
			else
			{
				result = (subjectType == AbilityTooltipSubject.Far);
			}
		}
		else
		{
			result = (subjectType == AbilityTooltipSubject.Far);
		}
		return result;
	}
}
