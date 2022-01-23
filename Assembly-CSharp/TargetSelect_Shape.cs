using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect_Shape : GenericAbility_TargetSelectBase
{
	public delegate BoardSquare CenterSquareDelegate(AbilityTarget currentTarget, ActorData caster);

	public delegate bool IsMovingShapeDelegate(ActorData caster);

	public delegate BoardSquare GetMoveStartSquareDelegate(AbilityTarget currentTarget, ActorData caster);

	public delegate Vector3 GetMoveStartFreePosDelegate(AbilityTarget currentTarget, ActorData caster);

	[Separator("Targeting Properties", true)]
	public AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three;

	public List<AbilityAreaShape> m_additionalShapes = new List<AbilityAreaShape>();

	[Header("-- For require targeting on actors")]
	public bool m_requireTargetingOnActor;

	public bool m_canTargetOnEnemies = true;

	public bool m_canTargetOnAllies = true;

	public bool m_canTargetOnSelf = true;

	public bool m_ignoreLosToTargetActor;

	[Separator("Show targeter arc?", true)]
	public bool m_showTargeterArc;

	[Separator("Use Move Shape Targeter? (for moving a shape similar to Grey drone)", true)]
	public bool m_useMoveShapeTargeter;

	public float m_moveLineWidth = 1f;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public CenterSquareDelegate m_centerSquareDelegate;

	public IsMovingShapeDelegate m_isMovingShapeDelegate;

	public GetMoveStartSquareDelegate m_moveStartSquareDelegate;

	public GetMoveStartFreePosDelegate m_moveStartFreePosDelegate;

	private const string c_shapeLayer = "ShapeLayer";

	public static ContextNameKeyPair s_cvarShapeLayer = new ContextNameKeyPair("ShapeLayer");

	private TargetSelectMod_Shape m_targetSelMod;

	private List<AbilityAreaShape> m_shapesList = new List<AbilityAreaShape>();

	public override string GetUsageForEditor()
	{
		return GetContextUsageStr("ShapeLayer", "on every hit actor, smallest shape index that actor is hit in (0-based). Shapes are sorted from smallest to largest");
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add("ShapeLayer");
	}

	public override void Initialize()
	{
		InitShapesList();
	}

	private void InitShapesList()
	{
		m_shapesList = new List<AbilityAreaShape>();
		m_shapesList.Add(GetShape());
		List<AbilityAreaShape> collection = m_additionalShapes;
		if (m_targetSelMod != null)
		{
			if (m_useTargetDataOverride)
			{
				collection = m_targetSelMod.m_additionalShapesOverrides;
			}
		}
		m_shapesList.AddRange(collection);
		m_shapesList.Sort();
	}

	public bool RequireTargetingOnActor()
	{
		bool result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_requireTargetingOnActorMod.GetModifiedValue(m_requireTargetingOnActor);
		}
		else
		{
			result = m_requireTargetingOnActor;
		}
		return result;
	}

	public bool CanTargetOnEnemies()
	{
		return (m_targetSelMod == null) ? m_canTargetOnEnemies : m_targetSelMod.m_canTargetOnEnemiesMod.GetModifiedValue(m_canTargetOnEnemies);
	}

	public bool CanTargetOnAllies()
	{
		bool result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_canTargetOnAlliesMod.GetModifiedValue(m_canTargetOnAllies);
		}
		else
		{
			result = m_canTargetOnAllies;
		}
		return result;
	}

	public bool CanTargetOnSelf()
	{
		bool result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_canTargetOnSelfMod.GetModifiedValue(m_canTargetOnSelf);
		}
		else
		{
			result = m_canTargetOnSelf;
		}
		return result;
	}

	public bool IgnoreLosToTargetActor()
	{
		bool result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_ignoreLosToTargetActorMod.GetModifiedValue(m_ignoreLosToTargetActor);
		}
		else
		{
			result = m_ignoreLosToTargetActor;
		}
		return result;
	}

	public float GetMoveLineWidth()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_moveLineWidthMod.GetModifiedValue(m_moveLineWidth);
		}
		else
		{
			result = m_moveLineWidth;
		}
		return result;
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		List<AbilityUtil_Targeter> list2;
		if (!m_useMoveShapeTargeter)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					List<AbilityTooltipSubject> list = new List<AbilityTooltipSubject>();
					list.Add(AbilityTooltipSubject.Primary);
					List<AbilityTooltipSubject> subjects = list;
					AbilityUtil_Targeter_MultipleShapes abilityUtil_Targeter_MultipleShapes = new AbilityUtil_Targeter_MultipleShapes(ability, m_shapesList, subjects, IgnoreLos(), IncludeEnemies(), IncludeAllies(), IncludeCaster());
					abilityUtil_Targeter_MultipleShapes.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
					abilityUtil_Targeter_MultipleShapes.m_alwaysIncludeShapeCenterActor = RequireTargetingOnActor();
					abilityUtil_Targeter_MultipleShapes.SetShowArcToShape(m_showTargeterArc);
					list2 = new List<AbilityUtil_Targeter>();
					list2.Add(abilityUtil_Targeter_MultipleShapes);
					return list2;
				}
				}
			}
		}
		AbilityUtil_Targeter_MovingShape abilityUtil_Targeter_MovingShape = new AbilityUtil_Targeter_MovingShape(ability, GetShape(), IgnoreLos(), GetMoveLineWidth());
		abilityUtil_Targeter_MovingShape.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
		if (!IncludeAllies())
		{
			if (!IncludeCaster())
			{
				abilityUtil_Targeter_MovingShape.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
			}
		}
		abilityUtil_Targeter_MovingShape.SetShowArcToShape(m_showTargeterArc);
		list2 = new List<AbilityUtil_Targeter>();
		list2.Add(abilityUtil_Targeter_MovingShape);
		return list2;
	}

	public override bool HandleCanCastValidation(Ability ability, ActorData caster)
	{
		if (RequireTargetingOnActor())
		{
			return ability.HasTargetableActorsInDecision(caster, CanTargetOnEnemies(), CanTargetOnAllies(), CanTargetOnSelf(), Ability.ValidateCheckPath.Ignore, !IgnoreLosToTargetActor(), false, true);
		}
		return true;
	}

	public override bool HandleCustomTargetValidation(Ability ability, ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (RequireTargetingOnActor())
		{
			bool result = false;
			BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
			object obj;
			if (boardSquareSafe != null)
			{
				obj = boardSquareSafe.OccupantActor;
			}
			else
			{
				obj = null;
			}
			ActorData actorData = (ActorData)obj;
			if (actorData != null)
			{
				if (ability.CanTargetActorInDecision(caster, actorData, CanTargetOnEnemies(), CanTargetOnAllies(), CanTargetOnSelf(), Ability.ValidateCheckPath.Ignore, !IgnoreLosToTargetActor(), false, true))
				{
					result = true;
				}
			}
			return result;
		}
		return true;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		m_targetSelMod = (modBase as TargetSelectMod_Shape);
	}

	protected override void OnTargetSelModRemoved()
	{
		m_targetSelMod = null;
	}

	public AbilityAreaShape GetShape()
	{
		return (m_targetSelMod == null) ? m_shape : m_targetSelMod.m_shapeMod.GetModifiedValue(m_shape);
	}

	public BoardSquare GetShapeCenterSquare(AbilityTarget target, ActorData caster)
	{
		if (m_centerSquareDelegate != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_centerSquareDelegate(target, caster);
				}
			}
		}
		return Board.Get().GetSquare(target.GridPos);
	}

	public bool IsMovingShape(ActorData caster)
	{
		if (m_isMovingShapeDelegate != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_isMovingShapeDelegate(caster);
				}
			}
		}
		return false;
	}

	public BoardSquare GetMoveStartSquare(AbilityTarget target, ActorData caster)
	{
		if (m_moveStartSquareDelegate != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_moveStartSquareDelegate(target, caster);
				}
			}
		}
		return caster.GetCurrentBoardSquare();
	}

	public Vector3 GetMoveStartFreePos(AbilityTarget target, ActorData caster)
	{
		if (m_moveStartFreePosDelegate != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_moveStartFreePosDelegate(target, caster);
				}
			}
		}
		return caster.GetFreePos();
	}
}
