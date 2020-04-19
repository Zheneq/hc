using System;
using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlDelayedBombDrops : Ability
{
	[Header("-- Targeting")]
	public BazookaGirlDelayedBombDrops.TargetingType m_targetingType;

	public bool m_penetrateLos;

	[Header("-- Targeting All?")]
	public bool m_targetAll;

	[Header("-- If using Shape")]
	public AbilityAreaShape m_targetingShape = AbilityAreaShape.Five_x_Five_NoCorners;

	[Header("-- If Using Cone")]
	public float m_coneWidthAngle = 270f;

	public float m_coneLength = 1.5f;

	public float m_coneBackwardOffset;

	[Header("-- Dropped Bomb Info")]
	public int m_bombDropDelay = 1;

	public AbilityPriority m_bombDropPhase = AbilityPriority.Combat_Damage;

	public int m_bombDropAnimIndexInEffect = 0xB;

	public BazookaGirlDroppedBombInfo m_bombInfo;

	[Header("-- Additional Damage from fewer hit areas, = extraDmgPeArea * Max(0, (maxNumAreas - numAreas))")]
	public int m_maxNumOfAreasForExtraDamage;

	public int m_extraDamagePerFewerArea;

	[Header("-- On Ability Hit Effect")]
	public StandardEffectInfo m_enemyOnAbilityHitEffect;

	[Header("-- Sequences ----------------------------------------")]
	public GameObject m_castSequencePrefab;

	public GameObject m_dropSiteMarkerSequencePrefab;

	public GameObject m_bombDropSequencePrefab;

	public GameObject m_targetMarkedSequencePrefab;

	[TextArea(1, 5)]
	public string m_notes;

	private AbilityMod_BazookaGirlDelayedBombDrops m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Delayed Bomb Drops";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.TargetAllEnemies())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.SetupTargeter()).MethodHandle;
			}
			base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, 30f, true, true, false, -1);
		}
		else if (this.m_targetingType == BazookaGirlDelayedBombDrops.TargetingType.Shape)
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
			base.Targeter = new AbilityUtil_Targeter_Shape(this, this.m_targetingShape, this.PenetrateLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		}
		else if (this.m_targetingType == BazookaGirlDelayedBombDrops.TargetingType.Cone)
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
			base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, this.GetConeAngle(), this.GetConeLength(), this.m_coneBackwardOffset, this.PenetrateLos(), true, true, false, false, -1, false);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetConeLength();
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.GetDamageAmount()).MethodHandle;
			}
			result = this.m_bombInfo.m_damageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_bombInfo.m_damageAmount);
		}
		return result;
	}

	public float GetConeLength()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.GetConeLength()).MethodHandle;
			}
			result = this.m_coneLength;
		}
		else
		{
			result = this.m_abilityMod.m_coneLengthMod.GetModifiedValue(this.m_coneLength);
		}
		return result;
	}

	public float GetConeAngle()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.GetConeAngle()).MethodHandle;
			}
			result = this.m_coneWidthAngle;
		}
		else
		{
			result = this.m_abilityMod.m_coneAngleMod.GetModifiedValue(this.m_coneWidthAngle);
		}
		return result;
	}

	public bool TargetAllEnemies()
	{
		bool result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.TargetAllEnemies()).MethodHandle;
			}
			result = this.m_targetAll;
		}
		else
		{
			result = this.m_abilityMod.m_targetAllMod.GetModifiedValue(this.m_targetAll);
		}
		return result;
	}

	public bool PenetrateLos()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.PenetrateLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
		}
		else
		{
			result = this.m_penetrateLos;
		}
		return result;
	}

	public int GetMaxNumOfAreasForExtraDamage()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.GetMaxNumOfAreasForExtraDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxNumOfAreasForExtraDamageMod.GetModifiedValue(this.m_maxNumOfAreasForExtraDamage);
		}
		else
		{
			result = this.m_maxNumOfAreasForExtraDamage;
		}
		return result;
	}

	public int GetExtraDamagePerFewerArea()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.GetExtraDamagePerFewerArea()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamagePerFewerAreaMod.GetModifiedValue(this.m_extraDamagePerFewerArea);
		}
		else
		{
			result = this.m_extraDamagePerFewerArea;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.m_enemyOnAbilityHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		AbilityTooltipSubject abilityTooltipSubject;
		if (this.m_bombDropDelay <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			abilityTooltipSubject = AbilityTooltipSubject.Primary;
		}
		else
		{
			abilityTooltipSubject = AbilityTooltipSubject.Tertiary;
		}
		AbilityTooltipSubject subject = abilityTooltipSubject;
		AbilityTooltipHelper.ReportDamage(ref result, subject, this.m_bombInfo.m_damageAmount);
		if (this.m_bombInfo.m_damageAmount != this.m_bombInfo.m_subsequentDamageAmount)
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
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Quaternary, this.m_bombInfo.m_subsequentDamageAmount);
		}
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (this.m_bombDropDelay <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			ActorData actorData = base.ActorData;
			List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
			List<BoardSquare> list = new List<BoardSquare>();
			using (List<ActorData>.Enumerator enumerator = visibleActorsInRangeByTooltipSubject.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData2 = enumerator.Current;
					list.Add(actorData2.\u0012());
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
			int num = 0;
			if (this.GetExtraDamagePerFewerArea() > 0)
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
				if (this.GetMaxNumOfAreasForExtraDamage() > 0)
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
					int num2 = this.GetMaxNumOfAreasForExtraDamage() - visibleActorsInRangeByTooltipSubject.Count;
					if (num2 > 0)
					{
						num = num2 * this.GetExtraDamagePerFewerArea();
					}
				}
			}
			int num3 = 0;
			bool flag = false;
			foreach (BoardSquare boardSquare in list)
			{
				Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_bombInfo.m_shape, boardSquare.ToVector3(), boardSquare);
				List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_bombInfo.m_shape, centerOfShape, boardSquare, this.PenetrateLos(), actorData, actorData.\u0012(), null);
				foreach (ActorData x in actorsInShape)
				{
					if (x == targetActor)
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
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							num3 += this.m_bombInfo.m_subsequentDamageAmount;
						}
						else
						{
							num3 += this.GetDamageAmount();
							flag = true;
						}
					}
				}
			}
			dictionary[AbilityTooltipSymbol.Damage] = num3 + num;
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlDelayedBombDrops abilityMod_BazookaGirlDelayedBombDrops = modAsBase as AbilityMod_BazookaGirlDelayedBombDrops;
		base.AddTokenInt(tokens, "Damage", string.Empty, this.m_bombInfo.m_damageAmount, false);
		string name = "MaxNumOfAreasForExtraDamage";
		string empty = string.Empty;
		int val;
		if (abilityMod_BazookaGirlDelayedBombDrops)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_BazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamageMod.GetModifiedValue(this.m_maxNumOfAreasForExtraDamage);
		}
		else
		{
			val = this.m_maxNumOfAreasForExtraDamage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "ExtraDamagePerFewerArea";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_BazookaGirlDelayedBombDrops)
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
			val2 = abilityMod_BazookaGirlDelayedBombDrops.m_extraDamagePerFewerAreaMod.GetModifiedValue(this.m_extraDamagePerFewerArea);
		}
		else
		{
			val2 = this.m_extraDamagePerFewerArea;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		bool result;
		if (animIndex != this.m_bombDropAnimIndexInEffect)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.CanTriggerAnimAtIndexForTaunt(int)).MethodHandle;
			}
			result = base.CanTriggerAnimAtIndexForTaunt(animIndex);
		}
		else
		{
			result = true;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BazookaGirlDelayedBombDrops))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_BazookaGirlDelayedBombDrops);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> result = new List<Vector3>();
		Vector3 vector = caster.\u0016();
		if (this.TargetAllEnemies())
		{
			AreaEffectUtils.AddRadiusExtremaToList(ref result, vector, 5f);
		}
		else if (this.m_targetingType == BazookaGirlDelayedBombDrops.TargetingType.Shape)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlDelayedBombDrops.CalcPointsOfInterestForCamera(List<AbilityTarget>, ActorData)).MethodHandle;
			}
			AreaEffectUtils.AddShapeCornersToList(ref result, this.m_targetingShape, targets[0]);
		}
		else if (this.m_targetingType == BazookaGirlDelayedBombDrops.TargetingType.Cone)
		{
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
			AreaEffectUtils.AddConeExtremaToList(ref result, vector, coneCenterAngleDegrees, this.GetConeAngle(), this.GetConeLength(), this.m_coneBackwardOffset);
		}
		return result;
	}

	public enum TargetingType
	{
		Shape,
		Cone
	}
}
