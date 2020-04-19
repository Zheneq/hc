using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class IceborgDamageArea : GenericAbility_Container
{
	[Separator("Targeting, Max Ranges", true)]
	public float m_initialCastMaxRange = 5.5f;

	public float m_moveAreaCastMaxRange = 1.45f;

	public bool m_targetingAreaCheckLos = true;

	public bool m_moveAreaTargetingCheckLos;

	[Separator("Movement Adjust Type for Moving Field", true)]
	public Ability.MovementAdjustment m_moveAreaMovementAdjustType;

	[Separator("Whether to add damage field", true)]
	public bool m_addGroundField;

	public bool m_stopMoversWithSlowStatus;

	public bool m_stopMoverIfHitPreviousTurn;

	public GroundEffectField m_groundFieldData;

	[Separator("Extra Damage on Initial Cast", true)]
	public int m_extraDamageOnInitialCast;

	[Separator("Damage change on ground field per turn", true)]
	public int m_groundFieldDamageChangePerTurn;

	[Separator("Min Damage", true)]
	public int m_minDamage;

	[Separator("Shielding per enemy hit on cast", true)]
	public int m_shieldPerEnemyHit;

	public int m_shieldDuration = 1;

	[Separator("Effect to apply if target has been hit by this ability on previous turn", true)]
	public StandardEffectInfo m_effectOnEnemyIfHitPreviousTurn;

	[Separator("Apply Nova effect?", true)]
	public bool m_applyDelayedAoeEffect;

	public bool m_applyNovaCoreIfHitPreviousTurn;

	[Separator("Animation index for moving field", true)]
	public int m_animationIndexForMoveArea;

	[Separator("Sequence for moving field", true)]
	public GameObject m_moveFieldSeqPrefab;

	[Header("-- For timing of removing existing field")]
	public GameObject m_fieldRemoveOnMoveSeqPrefab;

	private AbilityMod_IceborgDamageArea m_abilityMod;

	private Iceborg_SyncComponent m_syncComp;

	public static ContextNameKeyPair s_cvarTurnsSinceInitialCast = new ContextNameKeyPair("TurnsSinceInitialCast");

	private GroundEffectField m_cachedGroundFieldData;

	private StandardEffectInfo m_cachedEffectOnEnemyIfHitPreviousTurn;

	public override string GetUsageForEditor()
	{
		string str = base.GetUsageForEditor();
		str += ContextVars.\u0015(IceborgConeOrLaser.s_cvarHasSlow.\u0012(), "Set on enemies hit, 1 if has Slow, 0 otherwise", true);
		return str + ContextVars.\u0015(IceborgDamageArea.s_cvarTurnsSinceInitialCast.\u0012(), "turns since initial cast, 0 on first turn", false);
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(IceborgConeOrLaser.s_cvarHasSlow.\u0012());
		contextNamesForEditor.Add(IceborgDamageArea.s_cvarTurnsSinceInitialCast.\u0012());
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		this.SetCachedFields();
		if (this.GetTargetSelectComp() is TargetSelect_Shape)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.SetupTargetersAndCachedVars()).MethodHandle;
			}
			TargetSelect_Shape targetSelect_Shape = this.GetTargetSelectComp() as TargetSelect_Shape;
			targetSelect_Shape.m_isMovingShapeDelegate = new TargetSelect_Shape.IsMovingShapeDelegate(this.IsMovingShape);
			targetSelect_Shape.m_moveStartSquareDelegate = new TargetSelect_Shape.GetMoveStartSquareDelegate(this.GetMoveStartSquare);
			targetSelect_Shape.m_moveStartFreePosDelegate = new TargetSelect_Shape.GetMoveStartFreePosDelegate(this.GetMoveStartFreePos);
		}
		if (base.Targeter is AbilityUtil_Targeter_MovingShape)
		{
			AbilityUtil_Targeter_MovingShape abilityUtil_Targeter_MovingShape = base.Targeter as AbilityUtil_Targeter_MovingShape;
			abilityUtil_Targeter_MovingShape.m_delegateIsMovingShape = new AbilityUtil_Targeter_MovingShape.IsMovingShapeDelegate(this.IsMovingShape);
			abilityUtil_Targeter_MovingShape.m_delegateMoveStartSquare = new AbilityUtil_Targeter_MovingShape.MoveStartSquareDelegate(this.GetMoveStartSquare);
			abilityUtil_Targeter_MovingShape.m_delegateMoveStartFreePos = new AbilityUtil_Targeter_MovingShape.MoveStartFreePosDelegate(this.GetMoveStartFreePos);
		}
		if (this.m_animationIndexForMoveArea < 0)
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
			this.m_animationIndexForMoveArea = 0;
		}
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "ExtraDamageOnInitialCast", string.Empty, this.m_extraDamageOnInitialCast, false);
		base.AddTokenInt(tokens, "GroundFieldDamageChangePerTurn", string.Empty, this.m_groundFieldDamageChangePerTurn, false);
		base.AddTokenInt(tokens, "MinDamage", string.Empty, this.m_minDamage, false);
		base.AddTokenInt(tokens, "ShieldPerEnemyHit", string.Empty, this.m_shieldPerEnemyHit, false);
		base.AddTokenInt(tokens, "ShieldDuration", string.Empty, this.m_shieldDuration, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnEnemyIfHitPreviousTurn, "EffectOnEnemyIfHitPreviousTurn", this.m_effectOnEnemyIfHitPreviousTurn, true);
		if (this.m_syncComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		}
		if (this.m_syncComp != null)
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
			this.m_syncComp.AddTooltipTokens(tokens);
		}
	}

	private void SetCachedFields()
	{
		GroundEffectField cachedGroundFieldData;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.SetCachedFields()).MethodHandle;
			}
			cachedGroundFieldData = this.m_abilityMod.m_groundFieldDataMod.GetModifiedValue(this.m_groundFieldData);
		}
		else
		{
			cachedGroundFieldData = this.m_groundFieldData;
		}
		this.m_cachedGroundFieldData = cachedGroundFieldData;
		this.m_cachedEffectOnEnemyIfHitPreviousTurn = ((!(this.m_abilityMod != null)) ? this.m_effectOnEnemyIfHitPreviousTurn : this.m_abilityMod.m_effectOnEnemyIfHitPreviousTurnMod.GetModifiedValue(this.m_effectOnEnemyIfHitPreviousTurn));
	}

	public float GetInitialCastMaxRange()
	{
		float result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetInitialCastMaxRange()).MethodHandle;
			}
			result = this.m_abilityMod.m_initialCastMaxRangeMod.GetModifiedValue(this.m_initialCastMaxRange);
		}
		else
		{
			result = this.m_initialCastMaxRange;
		}
		return result;
	}

	public float GetMoveAreaCastMaxRange()
	{
		float result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetMoveAreaCastMaxRange()).MethodHandle;
			}
			result = this.m_abilityMod.m_moveAreaCastMaxRangeMod.GetModifiedValue(this.m_moveAreaCastMaxRange);
		}
		else
		{
			result = this.m_moveAreaCastMaxRange;
		}
		return result;
	}

	public bool TargetingAreaCheckLos()
	{
		return (!(this.m_abilityMod != null)) ? this.m_targetingAreaCheckLos : this.m_abilityMod.m_targetingAreaCheckLosMod.GetModifiedValue(this.m_targetingAreaCheckLos);
	}

	public bool AddGroundField()
	{
		bool result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.AddGroundField()).MethodHandle;
			}
			result = this.m_abilityMod.m_addGroundFieldMod.GetModifiedValue(this.m_addGroundField);
		}
		else
		{
			result = this.m_addGroundField;
		}
		return result;
	}

	public bool StopMoversWithSlowStatus()
	{
		bool result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.StopMoversWithSlowStatus()).MethodHandle;
			}
			result = this.m_abilityMod.m_stopMoversWithSlowStatusMod.GetModifiedValue(this.m_stopMoversWithSlowStatus);
		}
		else
		{
			result = this.m_stopMoversWithSlowStatus;
		}
		return result;
	}

	public bool StopMoverIfHitPreviousTurn()
	{
		bool result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.StopMoverIfHitPreviousTurn()).MethodHandle;
			}
			result = this.m_abilityMod.m_stopMoverIfHitPreviousTurnMod.GetModifiedValue(this.m_stopMoverIfHitPreviousTurn);
		}
		else
		{
			result = this.m_stopMoverIfHitPreviousTurn;
		}
		return result;
	}

	public GroundEffectField GetGroundFieldData()
	{
		GroundEffectField result;
		if (this.m_cachedGroundFieldData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetGroundFieldData()).MethodHandle;
			}
			result = this.m_cachedGroundFieldData;
		}
		else
		{
			result = this.m_groundFieldData;
		}
		return result;
	}

	public int GetExtraDamageOnInitialCast()
	{
		int result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetExtraDamageOnInitialCast()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageOnInitialCastMod.GetModifiedValue(this.m_extraDamageOnInitialCast);
		}
		else
		{
			result = this.m_extraDamageOnInitialCast;
		}
		return result;
	}

	public int GetGroundFieldDamageChangePerTurn()
	{
		return (!(this.m_abilityMod != null)) ? this.m_groundFieldDamageChangePerTurn : this.m_abilityMod.m_groundFieldDamageChangePerTurnMod.GetModifiedValue(this.m_groundFieldDamageChangePerTurn);
	}

	public int GetMinDamage()
	{
		int result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetMinDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_minDamageMod.GetModifiedValue(this.m_minDamage);
		}
		else
		{
			result = this.m_minDamage;
		}
		return result;
	}

	public int GetShieldPerEnemyHit()
	{
		int result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetShieldPerEnemyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_shieldPerEnemyHitMod.GetModifiedValue(this.m_shieldPerEnemyHit);
		}
		else
		{
			result = this.m_shieldPerEnemyHit;
		}
		return result;
	}

	public int GetShieldDuration()
	{
		int result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetShieldDuration()).MethodHandle;
			}
			result = this.m_abilityMod.m_shieldDurationMod.GetModifiedValue(this.m_shieldDuration);
		}
		else
		{
			result = this.m_shieldDuration;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnEnemyIfHitPreviousTurn()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnEnemyIfHitPreviousTurn != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetEffectOnEnemyIfHitPreviousTurn()).MethodHandle;
			}
			result = this.m_cachedEffectOnEnemyIfHitPreviousTurn;
		}
		else
		{
			result = this.m_effectOnEnemyIfHitPreviousTurn;
		}
		return result;
	}

	public bool ApplyDelayedAoeEffect()
	{
		bool result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.ApplyDelayedAoeEffect()).MethodHandle;
			}
			result = this.m_abilityMod.m_applyDelayedAoeEffectMod.GetModifiedValue(this.m_applyDelayedAoeEffect);
		}
		else
		{
			result = this.m_applyDelayedAoeEffect;
		}
		return result;
	}

	public bool ApplyNovaCoreIfHitPreviousTurn()
	{
		return (!(this.m_abilityMod != null)) ? this.m_applyNovaCoreIfHitPreviousTurn : this.m_abilityMod.m_applyNovaCoreIfHitPreviousTurnMod.GetModifiedValue(this.m_applyNovaCoreIfHitPreviousTurn);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		IceborgConeOrLaser.SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, this.GetShieldPerEnemyHit(), actorHitContext, results);
		if (targetActor.\u000E() != caster.\u000E())
		{
			if (!this.CanCastToMoveArea())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.PostProcessTargetingNumbers(ActorData, int, Dictionary<ActorData, ActorHitContext>, ContextVars, ActorData, TargetingNumberUpdateScratch)).MethodHandle;
				}
				if (this.GetExtraDamageOnInitialCast() > 0)
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
					results.m_damage += this.GetExtraDamageOnInitialCast();
				}
			}
			if (this.CanCastToMoveArea())
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
				if (this.GetGroundFieldDamageChangePerTurn() != 0)
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
					int turnsSinceInitialCast = this.m_syncComp.GetTurnsSinceInitialCast();
					results.m_damage += turnsSinceInitialCast * this.GetGroundFieldDamageChangePerTurn();
				}
			}
		}
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		string result;
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetAccessoryTargeterNumberString(ActorData, AbilityTooltipSymbol, int)).MethodHandle;
			}
			result = this.m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
		}
		else
		{
			result = null;
		}
		return result;
	}

	public bool IsMovingShape(ActorData caster)
	{
		return this.CanCastToMoveArea();
	}

	public BoardSquare GetMoveStartSquare(AbilityTarget target, ActorData caster)
	{
		BoardSquare result = caster.\u0012();
		if (this.IsMovingShape(caster))
		{
			BoardSquare boardSquare = Board.\u000E().\u0016((int)this.m_syncComp.m_damageAreaCenterX, (int)this.m_syncComp.m_damageAreaCenterY);
			if (boardSquare != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetMoveStartSquare(AbilityTarget, ActorData)).MethodHandle;
				}
				result = boardSquare;
			}
		}
		return result;
	}

	public Vector3 GetMoveStartFreePos(AbilityTarget target, ActorData caster)
	{
		if (this.IsMovingShape(caster))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetMoveStartFreePos(AbilityTarget, ActorData)).MethodHandle;
			}
			return this.m_syncComp.m_damageAreaFreePos;
		}
		return caster.\u0016();
	}

	public bool CanCastToMoveArea()
	{
		if (this.m_syncComp != null && this.m_syncComp.m_damageAreaCanMoveThisTurn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.CanCastToMoveArea()).MethodHandle;
			}
			if (this.m_syncComp.m_damageAreaCenterX >= 0)
			{
				return this.m_syncComp.m_damageAreaCenterY >= 0;
			}
		}
		return false;
	}

	public override bool IsFreeAction()
	{
		return this.CanCastToMoveArea();
	}

	public override int GetModdedCost()
	{
		if (this.CanCastToMoveArea())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetModdedCost()).MethodHandle;
			}
			return 0;
		}
		return base.GetModdedCost();
	}

	public override Ability.MovementAdjustment GetMovementAdjustment()
	{
		if (this.CanCastToMoveArea())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetMovementAdjustment()).MethodHandle;
			}
			return this.m_moveAreaMovementAdjustType;
		}
		return base.GetMovementAdjustment();
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (boardSquare != null && boardSquare.\u0016())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			BoardSquare boardSquare2 = caster.\u0012();
			Vector3 b = boardSquare2.ToVector3();
			float num = this.GetInitialCastMaxRange();
			bool flag = this.TargetingAreaCheckLos();
			if (this.CanCastToMoveArea())
			{
				BoardSquare boardSquare3 = Board.\u000E().\u0016((int)this.m_syncComp.m_damageAreaCenterX, (int)this.m_syncComp.m_damageAreaCenterY);
				if (boardSquare3 != null)
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
					if (boardSquare == boardSquare3)
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
						return false;
					}
					boardSquare2 = boardSquare3;
					num = this.GetMoveAreaCastMaxRange();
					GroundEffectField groundFieldData = this.GetGroundFieldData();
					b = AreaEffectUtils.GetCenterOfShape(groundFieldData.shape, this.m_syncComp.m_damageAreaFreePos, boardSquare3);
				}
				flag = this.m_moveAreaTargetingCheckLos;
			}
			float num2 = VectorUtils.HorizontalPlaneDistInSquares(boardSquare.ToVector3(), b);
			bool flag2 = num2 <= num;
			bool flag3 = true;
			if (flag2)
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
				if (flag)
				{
					flag3 = boardSquare2.\u0013(boardSquare.x, boardSquare.y);
				}
			}
			bool result;
			if (flag2)
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
				result = flag3;
			}
			else
			{
				result = false;
			}
			return result;
		}
		return false;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (this.CanCastToMoveArea())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageArea.GetActionAnimType()).MethodHandle;
			}
			return (ActorModelData.ActionAnimationType)this.m_animationIndexForMoveArea;
		}
		return base.GetActionAnimType();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return !this.CanCastToMoveArea();
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		if (!this.CanCastToMoveArea())
		{
			return this.GetInitialCastMaxRange();
		}
		return 0f;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_IceborgDamageArea);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
