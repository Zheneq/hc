using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class IceborgDamageArea : GenericAbility_Container
{
	[Separator("Targeting, Max Ranges", true)]
	public float m_initialCastMaxRange = 5.5f;

	public float m_moveAreaCastMaxRange = 1.45f;

	public bool m_targetingAreaCheckLos = true;

	public bool m_moveAreaTargetingCheckLos;

	[Separator("Movement Adjust Type for Moving Field", true)]
	public MovementAdjustment m_moveAreaMovementAdjustType;

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
		string usageForEditor = base.GetUsageForEditor();
		usageForEditor += ContextVars.GetContextUsageStr(IceborgConeOrLaser.s_cvarHasSlow.GetName(), "Set on enemies hit, 1 if has Slow, 0 otherwise");
		return usageForEditor + ContextVars.GetContextUsageStr(s_cvarTurnsSinceInitialCast.GetName(), "turns since initial cast, 0 on first turn", false);
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(IceborgConeOrLaser.s_cvarHasSlow.GetName());
		contextNamesForEditor.Add(s_cvarTurnsSinceInitialCast.GetName());
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		SetCachedFields();
		if (GetTargetSelectComp() is TargetSelect_Shape)
		{
			TargetSelect_Shape targetSelect_Shape = GetTargetSelectComp() as TargetSelect_Shape;
			targetSelect_Shape.m_isMovingShapeDelegate = IsMovingShape;
			targetSelect_Shape.m_moveStartSquareDelegate = GetMoveStartSquare;
			targetSelect_Shape.m_moveStartFreePosDelegate = GetMoveStartFreePos;
		}
		if (base.Targeter is AbilityUtil_Targeter_MovingShape)
		{
			AbilityUtil_Targeter_MovingShape abilityUtil_Targeter_MovingShape = base.Targeter as AbilityUtil_Targeter_MovingShape;
			abilityUtil_Targeter_MovingShape.m_delegateIsMovingShape = IsMovingShape;
			abilityUtil_Targeter_MovingShape.m_delegateMoveStartSquare = GetMoveStartSquare;
			abilityUtil_Targeter_MovingShape.m_delegateMoveStartFreePos = GetMoveStartFreePos;
		}
		if (m_animationIndexForMoveArea >= 0)
		{
			return;
		}
		while (true)
		{
			m_animationIndexForMoveArea = 0;
			return;
		}
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "ExtraDamageOnInitialCast", string.Empty, m_extraDamageOnInitialCast);
		AddTokenInt(tokens, "GroundFieldDamageChangePerTurn", string.Empty, m_groundFieldDamageChangePerTurn);
		AddTokenInt(tokens, "MinDamage", string.Empty, m_minDamage);
		AddTokenInt(tokens, "ShieldPerEnemyHit", string.Empty, m_shieldPerEnemyHit);
		AddTokenInt(tokens, "ShieldDuration", string.Empty, m_shieldDuration);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnEnemyIfHitPreviousTurn, "EffectOnEnemyIfHitPreviousTurn", m_effectOnEnemyIfHitPreviousTurn);
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Iceborg_SyncComponent>();
		}
		if (!(m_syncComp != null))
		{
			return;
		}
		while (true)
		{
			m_syncComp.AddTooltipTokens(tokens);
			return;
		}
	}

	private void SetCachedFields()
	{
		GroundEffectField cachedGroundFieldData;
		if (m_abilityMod != null)
		{
			cachedGroundFieldData = m_abilityMod.m_groundFieldDataMod.GetModifiedValue(m_groundFieldData);
		}
		else
		{
			cachedGroundFieldData = m_groundFieldData;
		}
		m_cachedGroundFieldData = cachedGroundFieldData;
		m_cachedEffectOnEnemyIfHitPreviousTurn = ((!(m_abilityMod != null)) ? m_effectOnEnemyIfHitPreviousTurn : m_abilityMod.m_effectOnEnemyIfHitPreviousTurnMod.GetModifiedValue(m_effectOnEnemyIfHitPreviousTurn));
	}

	public float GetInitialCastMaxRange()
	{
		float result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_initialCastMaxRangeMod.GetModifiedValue(m_initialCastMaxRange);
		}
		else
		{
			result = m_initialCastMaxRange;
		}
		return result;
	}

	public float GetMoveAreaCastMaxRange()
	{
		float result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_moveAreaCastMaxRangeMod.GetModifiedValue(m_moveAreaCastMaxRange);
		}
		else
		{
			result = m_moveAreaCastMaxRange;
		}
		return result;
	}

	public bool TargetingAreaCheckLos()
	{
		return (!(m_abilityMod != null)) ? m_targetingAreaCheckLos : m_abilityMod.m_targetingAreaCheckLosMod.GetModifiedValue(m_targetingAreaCheckLos);
	}

	public bool AddGroundField()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_addGroundFieldMod.GetModifiedValue(m_addGroundField);
		}
		else
		{
			result = m_addGroundField;
		}
		return result;
	}

	public bool StopMoversWithSlowStatus()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_stopMoversWithSlowStatusMod.GetModifiedValue(m_stopMoversWithSlowStatus);
		}
		else
		{
			result = m_stopMoversWithSlowStatus;
		}
		return result;
	}

	public bool StopMoverIfHitPreviousTurn()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_stopMoverIfHitPreviousTurnMod.GetModifiedValue(m_stopMoverIfHitPreviousTurn);
		}
		else
		{
			result = m_stopMoverIfHitPreviousTurn;
		}
		return result;
	}

	public GroundEffectField GetGroundFieldData()
	{
		GroundEffectField result;
		if (m_cachedGroundFieldData != null)
		{
			result = m_cachedGroundFieldData;
		}
		else
		{
			result = m_groundFieldData;
		}
		return result;
	}

	public int GetExtraDamageOnInitialCast()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraDamageOnInitialCastMod.GetModifiedValue(m_extraDamageOnInitialCast);
		}
		else
		{
			result = m_extraDamageOnInitialCast;
		}
		return result;
	}

	public int GetGroundFieldDamageChangePerTurn()
	{
		return (!(m_abilityMod != null)) ? m_groundFieldDamageChangePerTurn : m_abilityMod.m_groundFieldDamageChangePerTurnMod.GetModifiedValue(m_groundFieldDamageChangePerTurn);
	}

	public int GetMinDamage()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_minDamageMod.GetModifiedValue(m_minDamage);
		}
		else
		{
			result = m_minDamage;
		}
		return result;
	}

	public int GetShieldPerEnemyHit()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_shieldPerEnemyHitMod.GetModifiedValue(m_shieldPerEnemyHit);
		}
		else
		{
			result = m_shieldPerEnemyHit;
		}
		return result;
	}

	public int GetShieldDuration()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_shieldDurationMod.GetModifiedValue(m_shieldDuration);
		}
		else
		{
			result = m_shieldDuration;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnEnemyIfHitPreviousTurn()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnEnemyIfHitPreviousTurn != null)
		{
			result = m_cachedEffectOnEnemyIfHitPreviousTurn;
		}
		else
		{
			result = m_effectOnEnemyIfHitPreviousTurn;
		}
		return result;
	}

	public bool ApplyDelayedAoeEffect()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_applyDelayedAoeEffectMod.GetModifiedValue(m_applyDelayedAoeEffect);
		}
		else
		{
			result = m_applyDelayedAoeEffect;
		}
		return result;
	}

	public bool ApplyNovaCoreIfHitPreviousTurn()
	{
		return (!(m_abilityMod != null)) ? m_applyNovaCoreIfHitPreviousTurn : m_abilityMod.m_applyNovaCoreIfHitPreviousTurnMod.GetModifiedValue(m_applyNovaCoreIfHitPreviousTurn);
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		IceborgConeOrLaser.SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, GetShieldPerEnemyHit(), actorHitContext, results);
		if (targetActor.GetTeam() == caster.GetTeam())
		{
			return;
		}
		if (!CanCastToMoveArea())
		{
			if (GetExtraDamageOnInitialCast() > 0)
			{
				results.m_damage += GetExtraDamageOnInitialCast();
			}
		}
		if (!CanCastToMoveArea())
		{
			return;
		}
		while (true)
		{
			if (GetGroundFieldDamageChangePerTurn() != 0)
			{
				while (true)
				{
					int turnsSinceInitialCast = m_syncComp.GetTurnsSinceInitialCast();
					results.m_damage += turnsSinceInitialCast * GetGroundFieldDamageChangePerTurn();
					return;
				}
			}
			return;
		}
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		object result;
		if (m_syncComp != null)
		{
			result = m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
		}
		else
		{
			result = null;
		}
		return (string)result;
	}

	public bool IsMovingShape(ActorData caster)
	{
		return CanCastToMoveArea();
	}

	public BoardSquare GetMoveStartSquare(AbilityTarget target, ActorData caster)
	{
		BoardSquare result = caster.GetCurrentBoardSquare();
		if (IsMovingShape(caster))
		{
			BoardSquare boardSquare = Board.Get().GetSquare(m_syncComp.m_damageAreaCenterX, m_syncComp.m_damageAreaCenterY);
			if (boardSquare != null)
			{
				result = boardSquare;
			}
		}
		return result;
	}

	public Vector3 GetMoveStartFreePos(AbilityTarget target, ActorData caster)
	{
		if (IsMovingShape(caster))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_syncComp.m_damageAreaFreePos;
				}
			}
		}
		return caster.GetTravelBoardSquareWorldPosition();
	}

	public bool CanCastToMoveArea()
	{
		int result;
		if (m_syncComp != null && m_syncComp.m_damageAreaCanMoveThisTurn)
		{
			if (m_syncComp.m_damageAreaCenterX >= 0)
			{
				result = ((m_syncComp.m_damageAreaCenterY >= 0) ? 1 : 0);
				goto IL_0050;
			}
		}
		result = 0;
		goto IL_0050;
		IL_0050:
		return (byte)result != 0;
	}

	public override bool IsFreeAction()
	{
		return CanCastToMoveArea();
	}

	public override int GetModdedCost()
	{
		if (CanCastToMoveArea())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return 0;
				}
			}
		}
		return base.GetModdedCost();
	}

	public override MovementAdjustment GetMovementAdjustment()
	{
		if (CanCastToMoveArea())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_moveAreaMovementAdjustType;
				}
			}
		}
		return base.GetMovementAdjustment();
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (boardSquareSafe != null && boardSquareSafe.IsValidForGameplay())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					BoardSquare boardSquare = caster.GetCurrentBoardSquare();
					Vector3 b = boardSquare.ToVector3();
					float num = GetInitialCastMaxRange();
					bool flag = TargetingAreaCheckLos();
					if (CanCastToMoveArea())
					{
						BoardSquare boardSquare2 = Board.Get().GetSquare(m_syncComp.m_damageAreaCenterX, m_syncComp.m_damageAreaCenterY);
						if (boardSquare2 != null)
						{
							if (boardSquareSafe == boardSquare2)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										return false;
									}
								}
							}
							boardSquare = boardSquare2;
							num = GetMoveAreaCastMaxRange();
							GroundEffectField groundFieldData = GetGroundFieldData();
							b = AreaEffectUtils.GetCenterOfShape(groundFieldData.shape, m_syncComp.m_damageAreaFreePos, boardSquare2);
						}
						flag = m_moveAreaTargetingCheckLos;
					}
					float num2 = VectorUtils.HorizontalPlaneDistInSquares(boardSquareSafe.ToVector3(), b);
					bool flag2 = num2 <= num;
					bool flag3 = true;
					if (flag2)
					{
						if (flag)
						{
							flag3 = boardSquare.LOSDistanceIsOne_zq(boardSquareSafe.x, boardSquareSafe.y);
						}
					}
					int result;
					if (flag2)
					{
						result = (flag3 ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				}
				}
			}
		}
		return false;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (CanCastToMoveArea())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return (ActorModelData.ActionAnimationType)m_animationIndexForMoveArea;
				}
			}
		}
		return base.GetActionAnimType();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return !CanCastToMoveArea();
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		if (!CanCastToMoveArea())
		{
			return GetInitialCastMaxRange();
		}
		return 0f;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_IceborgDamageArea);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
