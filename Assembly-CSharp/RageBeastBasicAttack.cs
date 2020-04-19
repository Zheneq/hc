using System;
using System.Collections.Generic;
using UnityEngine;

public class RageBeastBasicAttack : Ability
{
	public float m_coneWidthAngle = 180f;

	public float m_coneBackwardOffset;

	public float m_coneLengthInner = 1.5f;

	public float m_coneLengthOuter = 2.5f;

	public int m_damageAmountInner = 5;

	public int m_damageAmountOuter = 3;

	public StandardEffectInfo m_effectInner;

	public StandardEffectInfo m_effectOuter;

	public int m_tpGainInner;

	public int m_tpGainOuter;

	public bool m_penetrateLineOfSight;

	private AbilityMod_RageBeastBasicAttack m_abilityMod;

	private StandardEffectInfo m_cachedEffectInner;

	private StandardEffectInfo m_cachedEffectOuter;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.Start()).MethodHandle;
			}
			this.m_abilityName = "Flurry";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		float angle = this.ModdedConeAngle();
		base.Targeter = new AbilityUtil_Targeter_MultipleCones(this, new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>
		{
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, this.ModdedInnerRadius()),
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(angle, this.ModdedOuterRadius())
		}, this.m_coneBackwardOffset, this.m_penetrateLineOfSight, true, true, false, false);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.ModdedOuterRadius();
	}

	private void SetCachedFields()
	{
		this.m_cachedEffectInner = ((!this.m_abilityMod) ? this.m_effectInner : this.m_abilityMod.m_effectInnerMod.GetModifiedValue(this.m_effectInner));
		StandardEffectInfo cachedEffectOuter;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.SetCachedFields()).MethodHandle;
			}
			cachedEffectOuter = this.m_abilityMod.m_effectOuterMod.GetModifiedValue(this.m_effectOuter);
		}
		else
		{
			cachedEffectOuter = this.m_effectOuter;
		}
		this.m_cachedEffectOuter = cachedEffectOuter;
	}

	public StandardEffectInfo GetEffectInner()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectInner != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.GetEffectInner()).MethodHandle;
			}
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
		return (this.m_cachedEffectOuter == null) ? this.m_effectOuter : this.m_cachedEffectOuter;
	}

	private float ModdedConeAngle()
	{
		float result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.ModdedConeAngle()).MethodHandle;
			}
			result = this.m_coneWidthAngle;
		}
		else
		{
			result = this.m_abilityMod.m_coneAngleMod.GetModifiedValue(this.m_coneWidthAngle);
		}
		return result;
	}

	private float ModdedInnerRadius()
	{
		float result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.ModdedInnerRadius()).MethodHandle;
			}
			result = this.m_coneLengthInner;
		}
		else
		{
			result = this.m_abilityMod.m_coneInnerRadiusMod.GetModifiedValue(this.m_coneLengthInner);
		}
		return result;
	}

	private float ModdedOuterRadius()
	{
		float result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.ModdedOuterRadius()).MethodHandle;
			}
			result = this.m_coneLengthOuter;
		}
		else
		{
			result = this.m_abilityMod.m_coneOuterRadiusMod.GetModifiedValue(this.m_coneLengthOuter);
		}
		return result;
	}

	private int ModdedInnerDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.ModdedInnerDamage()).MethodHandle;
			}
			result = this.m_damageAmountInner;
		}
		else
		{
			result = this.m_abilityMod.m_innerDamageMod.GetModifiedValue(this.m_damageAmountInner);
		}
		return result;
	}

	private int ModdedOuterDamage()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_outerDamageMod.GetModifiedValue(this.m_damageAmountOuter) : this.m_damageAmountOuter;
	}

	private int ModdedDamagePerAdjacentEnemy()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.ModdedDamagePerAdjacentEnemy()).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_extraDamagePerAdjacentEnemy;
		}
		return result;
	}

	private int ModdedTechPointsPerAdjacentEnemy()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.ModdedTechPointsPerAdjacentEnemy()).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_extraTechPointsPerAdjacentEnemy;
		}
		return result;
	}

	private int ModdedInnerTpGain()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.ModdedInnerTpGain()).MethodHandle;
			}
			result = this.m_tpGainInner;
		}
		else
		{
			result = this.m_abilityMod.m_innerTpGain.GetModifiedValue(this.m_tpGainInner);
		}
		return result;
	}

	private int ModdedOuterTpGain()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_outerTpGain.GetModifiedValue(this.m_tpGainOuter) : this.m_tpGainOuter;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Near, this.m_damageAmountInner));
		this.m_effectInner.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Near);
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, this.m_damageAmountOuter));
		this.m_effectOuter.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Far);
		return list;
	}

	public override List<int> \u001D()
	{
		List<int> list = base.\u001D();
		int num = Mathf.Abs(this.m_damageAmountInner - this.m_damageAmountOuter);
		if (num != 0)
		{
			list.Add(num);
		}
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastBasicAttack abilityMod_RageBeastBasicAttack = modAsBase as AbilityMod_RageBeastBasicAttack;
		string name = "DamageAmountInner";
		string empty = string.Empty;
		int val;
		if (abilityMod_RageBeastBasicAttack)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_RageBeastBasicAttack.m_innerDamageMod.GetModifiedValue(this.m_damageAmountInner);
		}
		else
		{
			val = this.m_damageAmountInner;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "DamageAmountOuter";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_RageBeastBasicAttack)
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
			val2 = abilityMod_RageBeastBasicAttack.m_outerDamageMod.GetModifiedValue(this.m_damageAmountOuter);
		}
		else
		{
			val2 = this.m_damageAmountOuter;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_RageBeastBasicAttack)
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
			effectInfo = abilityMod_RageBeastBasicAttack.m_effectInnerMod.GetModifiedValue(this.m_effectInner);
		}
		else
		{
			effectInfo = this.m_effectInner;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectInner", this.m_effectInner, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_RageBeastBasicAttack)
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
			effectInfo2 = abilityMod_RageBeastBasicAttack.m_effectOuterMod.GetModifiedValue(this.m_effectOuter);
		}
		else
		{
			effectInfo2 = this.m_effectOuter;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOuter", this.m_effectOuter, true);
		string name3 = "TpGainInner";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_RageBeastBasicAttack)
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
			val3 = abilityMod_RageBeastBasicAttack.m_innerTpGain.GetModifiedValue(this.m_tpGainInner);
		}
		else
		{
			val3 = this.m_tpGainInner;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "TpGainOuter";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_RageBeastBasicAttack)
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
			val4 = abilityMod_RageBeastBasicAttack.m_outerTpGain.GetModifiedValue(this.m_tpGainOuter);
		}
		else
		{
			val4 = this.m_tpGainOuter;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastBasicAttack))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_RageBeastBasicAttack);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	private unsafe void GetExtraDamageAndTPForCurrentLocation(bool visibleActorsOnly, out int damageAmount, out int techPointAmount)
	{
		damageAmount = this.ModdedDamagePerAdjacentEnemy();
		techPointAmount = this.ModdedTechPointsPerAdjacentEnemy();
		if (damageAmount == 0)
		{
			if (techPointAmount == 0)
			{
				return;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.GetExtraDamageAndTPForCurrentLocation(bool, int*, int*)).MethodHandle;
			}
		}
		int num = 0;
		List<BoardSquare> list = new List<BoardSquare>();
		Board.\u000E().\u0015(base.ActorData.\u0012().x, base.ActorData.\u0012().y, ref list);
		using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				if (boardSquare.OccupantActor != null)
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
					if (boardSquare.OccupantActor.\u000E() != base.ActorData.\u000E())
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
						if (!boardSquare.OccupantActor.IgnoreForAbilityHits)
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
							if (visibleActorsOnly)
							{
								if (!boardSquare.OccupantActor.\u0018())
								{
									continue;
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
							}
							num++;
						}
					}
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		damageAmount *= num;
		techPointAmount *= num;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			int num = 0;
			int num2 = 0;
			this.GetExtraDamageAndTPForCurrentLocation(true, out num, out num2);
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Near))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
				}
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedInnerDamage() + num;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Far))
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
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedOuterDamage() + num;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int num = 0;
		int num2 = 0;
		this.GetExtraDamageAndTPForCurrentLocation(true, out num, out num2);
		int num3 = 0;
		if (this.ModdedInnerTpGain() > 0 || num2 > 0)
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Near);
			num3 += visibleActorsInRangeByTooltipSubject.Count * (this.ModdedInnerTpGain() + num2);
		}
		if (this.ModdedOuterTpGain() > 0 || num2 > 0)
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject2 = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Far);
			num3 += visibleActorsInRangeByTooltipSubject2.Count * (this.ModdedOuterTpGain() + num2);
		}
		return num3;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.Near)
		{
			if (subjectType != AbilityTooltipSubject.Far)
			{
				return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RageBeastBasicAttack.DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject, ActorData, Vector3, ActorData)).MethodHandle;
			}
		}
		float num = this.ModdedInnerRadius() * Board.\u000E().squareSize;
		Vector3 vector = targetActor.\u0016() - damageOrigin;
		vector.y = 0f;
		float num2 = vector.magnitude;
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			num2 -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.\u000E().squareSize;
		}
		bool flag;
		if (num2 > num)
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
			flag = false;
		}
		else
		{
			flag = true;
		}
		bool result;
		if (subjectType == AbilityTooltipSubject.Near)
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
			result = flag;
		}
		else
		{
			result = !flag;
		}
		return result;
	}
}
