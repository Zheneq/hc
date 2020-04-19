using System;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreCharge : Ability
{
	[Header("-- Charge Targeting")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	public float m_width = 1f;

	public float m_maxRange = 10f;

	[Header("-- Normal On Hit Damage, Effect, etc")]
	public int m_directHitDamage = 0x14;

	public StandardEffectInfo m_directEnemyHitEffect;

	public bool m_directHitIgnoreCover = true;

	[Space(10f)]
	public int m_aoeDamage = 0xA;

	public StandardEffectInfo m_aoeEnemyHitEffect;

	[Header("-- Extra Damage from Charge Path Length")]
	public int m_extraDirectHitDamagePerSquare;

	[Header("-- Heal On Self")]
	public int m_healOnSelfPerTargetHit;

	[Header("-- Other On Hit Config")]
	public int m_cooldownOnHit = -1;

	public bool m_chaseHitActor;

	[Header("-- Charge Anim")]
	[Tooltip("Whether to set up charge like battlemonk charge with pivots and recovery")]
	public bool m_chargeWithPivotAndRecovery;

	[Tooltip("Only relevant if using pivot and recovery charge setup")]
	public float m_recoveryTime = 0.5f;

	[Header("-- Sequences")]
	public GameObject m_chargeSequencePrefab;

	public GameObject m_aoeHitSequencePrefab;

	private const int c_maxBounces = 0;

	private const int c_maxTargetsHit = 1;

	private const bool c_penetrateLoS = false;

	private AbilityMod_ClaymoreCharge m_abilityMod;

	private Claymore_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedDirectEnemyHitEffect;

	private StandardEffectInfo m_cachedAoeEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.Start()).MethodHandle;
			}
			this.m_abilityName = "Berserker Charge";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.m_syncComp = base.GetComponent<Claymore_SyncComponent>();
		this.SetCachedFields();
		AbilityUtil_Targeter_ClaymoreCharge abilityUtil_Targeter_ClaymoreCharge = new AbilityUtil_Targeter_ClaymoreCharge(this, this.GetChargeWidth(), this.GetChargeRange(), this.GetAoeShape(), this.DirectHitIgnoreCover());
		if (this.GetHealOnSelfPerTargetHit() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.SetupTargeter()).MethodHandle;
			}
			AbilityUtil_Targeter_ClaymoreCharge abilityUtil_Targeter_ClaymoreCharge2 = abilityUtil_Targeter_ClaymoreCharge;
			if (ClaymoreCharge.<>f__am$cache0 == null)
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
				ClaymoreCharge.<>f__am$cache0 = ((ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0);
			}
			abilityUtil_Targeter_ClaymoreCharge2.m_affectCasterDelegate = ClaymoreCharge.<>f__am$cache0;
		}
		base.Targeter = abilityUtil_Targeter_ClaymoreCharge;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetChargeRange();
	}

	private void SetCachedFields()
	{
		this.m_cachedDirectEnemyHitEffect = ((!this.m_abilityMod) ? this.m_directEnemyHitEffect : this.m_abilityMod.m_directEnemyHitEffectMod.GetModifiedValue(this.m_directEnemyHitEffect));
		this.m_cachedAoeEnemyHitEffect = ((!this.m_abilityMod) ? this.m_aoeEnemyHitEffect : this.m_abilityMod.m_aoeEnemyHitEffectMod.GetModifiedValue(this.m_aoeEnemyHitEffect));
	}

	public AbilityAreaShape GetAoeShape()
	{
		AbilityAreaShape result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetAoeShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_aoeShapeMod.GetModifiedValue(this.m_aoeShape);
		}
		else
		{
			result = this.m_aoeShape;
		}
		return result;
	}

	public float GetChargeWidth()
	{
		return (!this.m_abilityMod) ? this.m_width : this.m_abilityMod.m_widthMod.GetModifiedValue(this.m_width);
	}

	public float GetChargeRange()
	{
		return (!this.m_abilityMod) ? this.m_maxRange : this.m_abilityMod.m_maxRangeMod.GetModifiedValue(this.m_maxRange);
	}

	public bool DirectHitIgnoreCover()
	{
		return (!this.m_abilityMod) ? this.m_directHitIgnoreCover : this.m_abilityMod.m_directHitIgnoreCoverMod.GetModifiedValue(this.m_directHitIgnoreCover);
	}

	public int GetDirectHitDamage()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetDirectHitDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_directHitDamageMod.GetModifiedValue(this.m_directHitDamage);
		}
		else
		{
			result = this.m_directHitDamage;
		}
		return result;
	}

	public StandardEffectInfo GetDirectEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedDirectEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetDirectEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedDirectEnemyHitEffect;
		}
		else
		{
			result = this.m_directEnemyHitEffect;
		}
		return result;
	}

	public int GetAoeDamage()
	{
		return (!this.m_abilityMod) ? this.m_aoeDamage : this.m_abilityMod.m_aoeDamageMod.GetModifiedValue(this.m_aoeDamage);
	}

	public StandardEffectInfo GetAoeEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAoeEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetAoeEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedAoeEnemyHitEffect;
		}
		else
		{
			result = this.m_aoeEnemyHitEffect;
		}
		return result;
	}

	public int GetExtraDirectHitDamagePerSquare()
	{
		return (!this.m_abilityMod) ? this.m_extraDirectHitDamagePerSquare : this.m_abilityMod.m_extraDirectHitDamagePerSquareMod.GetModifiedValue(this.m_extraDirectHitDamagePerSquare);
	}

	public int GetHealOnSelfPerTargetHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetHealOnSelfPerTargetHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_healOnSelfPerTargetHitMod.GetModifiedValue(this.m_healOnSelfPerTargetHit);
		}
		else
		{
			result = this.m_healOnSelfPerTargetHit;
		}
		return result;
	}

	public int GetCooldownOnHit()
	{
		return (!this.m_abilityMod) ? this.m_cooldownOnHit : this.m_abilityMod.m_cooldownOnHitMod.GetModifiedValue(this.m_cooldownOnHit);
	}

	public bool GetChaseHitActor()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetChaseHitActor()).MethodHandle;
			}
			result = this.m_abilityMod.m_chaseHitActorMod.GetModifiedValue(this.m_chaseHitActor);
		}
		else
		{
			result = this.m_chaseHitActor;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		return 1;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreCharge abilityMod_ClaymoreCharge = modAsBase as AbilityMod_ClaymoreCharge;
		string name = "DirectHitDamage";
		string empty = string.Empty;
		int val;
		if (abilityMod_ClaymoreCharge)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_ClaymoreCharge.m_directHitDamageMod.GetModifiedValue(this.m_directHitDamage);
		}
		else
		{
			val = this.m_directHitDamage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_ClaymoreCharge) ? this.m_directEnemyHitEffect : abilityMod_ClaymoreCharge.m_directEnemyHitEffectMod.GetModifiedValue(this.m_directEnemyHitEffect), "DirectEnemyHitEffect", this.m_directEnemyHitEffect, true);
		base.AddTokenInt(tokens, "AoeDamage", string.Empty, (!abilityMod_ClaymoreCharge) ? this.m_aoeDamage : abilityMod_ClaymoreCharge.m_aoeDamageMod.GetModifiedValue(this.m_aoeDamage), false);
		StandardEffectInfo effectInfo;
		if (abilityMod_ClaymoreCharge)
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
			effectInfo = abilityMod_ClaymoreCharge.m_aoeEnemyHitEffectMod.GetModifiedValue(this.m_aoeEnemyHitEffect);
		}
		else
		{
			effectInfo = this.m_aoeEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "AoeEnemyHitEffect", this.m_aoeEnemyHitEffect, true);
		string name2 = "ExtraDirectHitDamagePerSquare";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ClaymoreCharge)
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
			val2 = abilityMod_ClaymoreCharge.m_extraDirectHitDamagePerSquareMod.GetModifiedValue(this.m_extraDirectHitDamagePerSquare);
		}
		else
		{
			val2 = this.m_extraDirectHitDamagePerSquare;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "HealOnSelfPerTargetHit", string.Empty, (!abilityMod_ClaymoreCharge) ? this.m_healOnSelfPerTargetHit : abilityMod_ClaymoreCharge.m_healOnSelfPerTargetHitMod.GetModifiedValue(this.m_healOnSelfPerTargetHit), false);
		string name3 = "CooldownOnHit";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_ClaymoreCharge)
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
			val3 = abilityMod_ClaymoreCharge.m_cooldownOnHitMod.GetModifiedValue(this.m_cooldownOnHit);
		}
		else
		{
			val3 = this.m_cooldownOnHit;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDirectHitDamage());
		this.GetDirectEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.GetAoeDamage());
		this.GetAoeEnemyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetHealOnSelfPerTargetHit());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
				}
				int num = this.GetDirectHitDamage();
				if (this.GetExtraDirectHitDamagePerSquare() > 0)
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
					if (base.Targeter is AbilityUtil_Targeter_ClaymoreCharge)
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
						AbilityUtil_Targeter_ClaymoreCharge abilityUtil_Targeter_ClaymoreCharge = base.Targeter as AbilityUtil_Targeter_ClaymoreCharge;
						int num2 = Mathf.Max(0, abilityUtil_Targeter_ClaymoreCharge.LastUpdatePathSquareCount - 1);
						num += num2 * this.GetExtraDirectHitDamagePerSquare();
					}
				}
				dictionary[AbilityTooltipSymbol.Damage] = num;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.GetAoeDamage();
			}
			else if (this.GetHealOnSelfPerTargetHit() > 0)
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
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
				{
					int value = this.GetHealOnSelfPerTargetHit() * Mathf.Max(0, base.Targeter.GetActorsInRange().Count - 1);
					dictionary[AbilityTooltipSymbol.Healing] = value;
				}
			}
		}
		return dictionary;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		string result;
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetAccessoryTargeterNumberString(ActorData, AbilityTooltipSymbol, int)).MethodHandle;
			}
			result = this.m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
		}
		else
		{
			result = null;
		}
		return result;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public static List<ActorData> GetActorsOnPath(BoardSquarePathInfo path, List<Team> relevantTeams, ActorData caster)
	{
		List<ActorData> list = new List<ActorData>();
		if (path != null)
		{
			for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
			{
				ActorData occupantActor = boardSquarePathInfo.square.OccupantActor;
				if (occupantActor != null && AreaEffectUtils.IsActorTargetable(occupantActor, relevantTeams))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetActorsOnPath(BoardSquarePathInfo, List<Team>, ActorData)).MethodHandle;
					}
					if (relevantTeams != null)
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
						if (!relevantTeams.Contains(occupantActor.\u000E()))
						{
							goto IL_69;
						}
					}
					list.Add(occupantActor);
				}
				IL_69:;
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
		return list;
	}

	public unsafe static float GetMaxPotentialChargeDistance(Vector3 startPos, Vector3 endPos, Vector3 aimDir, float laserMaxDistInWorld, ActorData mover, out BoardSquare pathEndSquare)
	{
		float result = laserMaxDistInWorld;
		pathEndSquare = KnockbackUtils.GetLastValidBoardSquareInLine(startPos, endPos, false, false, laserMaxDistInWorld + 0.5f);
		BoardSquare y = Board.\u000E().\u000E(startPos);
		if (pathEndSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetMaxPotentialChargeDistance(Vector3, Vector3, Vector3, float, ActorData, BoardSquare*)).MethodHandle;
			}
			if (pathEndSquare != y)
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
				Vector3 pointToProject = pathEndSquare.ToVector3();
				Vector3 projectionPoint = VectorUtils.GetProjectionPoint(aimDir, startPos, pointToProject);
				float num = (projectionPoint - startPos).magnitude + 0.5f;
				if (num < laserMaxDistInWorld)
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
					result = num;
				}
				return result;
			}
		}
		result = 0.5f;
		return result;
	}

	public unsafe static BoardSquare GetTrimmedDestinationInPath(BoardSquarePathInfo chargePath, out bool differentFromInputDest)
	{
		differentFromInputDest = false;
		BoardSquare result = null;
		if (chargePath != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetTrimmedDestinationInPath(BoardSquarePathInfo, bool*)).MethodHandle;
			}
			BoardSquarePathInfo boardSquarePathInfo = chargePath;
			result = boardSquarePathInfo.square;
			int num = 0;
			while (boardSquarePathInfo.next != null)
			{
				BoardSquare square = boardSquarePathInfo.next.square;
				if (boardSquarePathInfo.square.\u0015())
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
					if (!square.\u0015())
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
						if (num > 0)
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
							result = boardSquarePathInfo.square;
							differentFromInputDest = true;
							break;
						}
					}
				}
				result = square;
				boardSquarePathInfo = boardSquarePathInfo.next;
				num++;
			}
		}
		return result;
	}

	public static BoardSquare GetChargeDestinationSquare(Vector3 startPos, Vector3 chargeDestPos, ActorData lastChargeHitActor, BoardSquare initialPathEndSquare, ActorData caster, bool trimBeforeFirstInvalid)
	{
		BoardSquare boardSquare;
		if (lastChargeHitActor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClaymoreCharge.GetChargeDestinationSquare(Vector3, Vector3, ActorData, BoardSquare, ActorData, bool)).MethodHandle;
			}
			boardSquare = lastChargeHitActor.\u0012();
		}
		else
		{
			if (initialPathEndSquare != null)
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
				boardSquare = initialPathEndSquare;
			}
			else
			{
				boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(startPos, chargeDestPos, true, false, float.MaxValue);
			}
			BoardSquare startSquare = Board.\u000E().\u000E(startPos);
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquare, startSquare, true);
			if (boardSquarePathInfo != null && trimBeforeFirstInvalid)
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
				bool flag;
				boardSquare = ClaymoreCharge.GetTrimmedDestinationInPath(boardSquarePathInfo, out flag);
			}
		}
		return boardSquare;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreCharge))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ClaymoreCharge);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
