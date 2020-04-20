﻿using System;
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
		str += ContextVars.GetDebugString(IceborgConeOrLaser.s_cvarHasSlow.GetName(), "Set on enemies hit, 1 if has Slow, 0 otherwise", true);
		return str + ContextVars.GetDebugString(IceborgDamageArea.s_cvarTurnsSinceInitialCast.GetName(), "turns since initial cast, 0 on first turn", false);
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(IceborgConeOrLaser.s_cvarHasSlow.GetName());
		contextNamesForEditor.Add(IceborgDamageArea.s_cvarTurnsSinceInitialCast.GetName());
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		this.SetCachedFields();
		if (this.GetTargetSelectComp() is TargetSelect_Shape)
		{
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
			this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		}
		if (this.m_syncComp != null)
		{
			this.m_syncComp.AddTooltipTokens(tokens);
		}
	}

	private void SetCachedFields()
	{
		GroundEffectField cachedGroundFieldData;
		if (this.m_abilityMod != null)
		{
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
		if (targetActor.GetTeam() != caster.GetTeam())
		{
			if (!this.CanCastToMoveArea())
			{
				if (this.GetExtraDamageOnInitialCast() > 0)
				{
					results.m_damage += this.GetExtraDamageOnInitialCast();
				}
			}
			if (this.CanCastToMoveArea())
			{
				if (this.GetGroundFieldDamageChangePerTurn() != 0)
				{
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
		BoardSquare result = caster.GetCurrentBoardSquare();
		if (this.IsMovingShape(caster))
		{
			BoardSquare boardSquare = Board.Get().GetBoardSquare((int)this.m_syncComp.m_damageAreaCenterX, (int)this.m_syncComp.m_damageAreaCenterY);
			if (boardSquare != null)
			{
				result = boardSquare;
			}
		}
		return result;
	}

	public Vector3 GetMoveStartFreePos(AbilityTarget target, ActorData caster)
	{
		if (this.IsMovingShape(caster))
		{
			return this.m_syncComp.m_damageAreaFreePos;
		}
		return caster.GetTravelBoardSquareWorldPosition();
	}

	public bool CanCastToMoveArea()
	{
		if (this.m_syncComp != null && this.m_syncComp.m_damageAreaCanMoveThisTurn)
		{
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
			return 0;
		}
		return base.GetModdedCost();
	}

	public override Ability.MovementAdjustment GetMovementAdjustment()
	{
		if (this.CanCastToMoveArea())
		{
			return this.m_moveAreaMovementAdjustType;
		}
		return base.GetMovementAdjustment();
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null && boardSquareSafe.IsBaselineHeight())
		{
			BoardSquare boardSquare = caster.GetCurrentBoardSquare();
			Vector3 b = boardSquare.ToVector3();
			float num = this.GetInitialCastMaxRange();
			bool flag = this.TargetingAreaCheckLos();
			if (this.CanCastToMoveArea())
			{
				BoardSquare boardSquare2 = Board.Get().GetBoardSquare((int)this.m_syncComp.m_damageAreaCenterX, (int)this.m_syncComp.m_damageAreaCenterY);
				if (boardSquare2 != null)
				{
					if (boardSquareSafe == boardSquare2)
					{
						return false;
					}
					boardSquare = boardSquare2;
					num = this.GetMoveAreaCastMaxRange();
					GroundEffectField groundFieldData = this.GetGroundFieldData();
					b = AreaEffectUtils.GetCenterOfShape(groundFieldData.shape, this.m_syncComp.m_damageAreaFreePos, boardSquare2);
				}
				flag = this.m_moveAreaTargetingCheckLos;
			}
			float num2 = VectorUtils.HorizontalPlaneDistInSquares(boardSquareSafe.ToVector3(), b);
			bool flag2 = num2 <= num;
			bool flag3 = true;
			if (flag2)
			{
				if (flag)
				{
					flag3 = boardSquare.symbol_0013(boardSquareSafe.x, boardSquareSafe.y);
				}
			}
			bool result;
			if (flag2)
			{
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
