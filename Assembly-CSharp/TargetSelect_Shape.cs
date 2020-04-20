using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_Shape : GenericAbility_TargetSelectBase
{
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

	public TargetSelect_Shape.CenterSquareDelegate m_centerSquareDelegate;

	public TargetSelect_Shape.IsMovingShapeDelegate m_isMovingShapeDelegate;

	public TargetSelect_Shape.GetMoveStartSquareDelegate m_moveStartSquareDelegate;

	public TargetSelect_Shape.GetMoveStartFreePosDelegate m_moveStartFreePosDelegate;

	private const string c_shapeLayer = "ShapeLayer";

	public static ContextNameKeyPair s_cvarShapeLayer = new ContextNameKeyPair("ShapeLayer");

	private TargetSelectMod_Shape m_targetSelMod;

	private List<AbilityAreaShape> m_shapesList = new List<AbilityAreaShape>();

	public override string GetUsageForEditor()
	{
		return base.GetContextUsageStr("ShapeLayer", "on every hit actor, smallest shape index that actor is hit in (0-based). Shapes are sorted from smallest to largest", true);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add("ShapeLayer");
	}

	public override void Initialize()
	{
		this.InitShapesList();
	}

	private void InitShapesList()
	{
		this.m_shapesList = new List<AbilityAreaShape>();
		this.m_shapesList.Add(this.GetShape());
		List<AbilityAreaShape> collection = this.m_additionalShapes;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.InitShapesList()).MethodHandle;
			}
			if (this.m_useTargetDataOverride)
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
				collection = this.m_targetSelMod.m_additionalShapesOverrides;
			}
		}
		this.m_shapesList.AddRange(collection);
		this.m_shapesList.Sort();
	}

	public bool RequireTargetingOnActor()
	{
		bool result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.RequireTargetingOnActor()).MethodHandle;
			}
			result = this.m_targetSelMod.m_requireTargetingOnActorMod.GetModifiedValue(this.m_requireTargetingOnActor);
		}
		else
		{
			result = this.m_requireTargetingOnActor;
		}
		return result;
	}

	public bool CanTargetOnEnemies()
	{
		return (this.m_targetSelMod == null) ? this.m_canTargetOnEnemies : this.m_targetSelMod.m_canTargetOnEnemiesMod.GetModifiedValue(this.m_canTargetOnEnemies);
	}

	public bool CanTargetOnAllies()
	{
		bool result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.CanTargetOnAllies()).MethodHandle;
			}
			result = this.m_targetSelMod.m_canTargetOnAlliesMod.GetModifiedValue(this.m_canTargetOnAllies);
		}
		else
		{
			result = this.m_canTargetOnAllies;
		}
		return result;
	}

	public bool CanTargetOnSelf()
	{
		bool result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.CanTargetOnSelf()).MethodHandle;
			}
			result = this.m_targetSelMod.m_canTargetOnSelfMod.GetModifiedValue(this.m_canTargetOnSelf);
		}
		else
		{
			result = this.m_canTargetOnSelf;
		}
		return result;
	}

	public bool IgnoreLosToTargetActor()
	{
		bool result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.IgnoreLosToTargetActor()).MethodHandle;
			}
			result = this.m_targetSelMod.m_ignoreLosToTargetActorMod.GetModifiedValue(this.m_ignoreLosToTargetActor);
		}
		else
		{
			result = this.m_ignoreLosToTargetActor;
		}
		return result;
	}

	public float GetMoveLineWidth()
	{
		float result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.GetMoveLineWidth()).MethodHandle;
			}
			result = this.m_targetSelMod.m_moveLineWidthMod.GetModifiedValue(this.m_moveLineWidth);
		}
		else
		{
			result = this.m_moveLineWidth;
		}
		return result;
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		if (!this.m_useMoveShapeTargeter)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.CreateTargeters(Ability)).MethodHandle;
			}
			List<AbilityTooltipSubject> subjects = new List<AbilityTooltipSubject>
			{
				AbilityTooltipSubject.Primary
			};
			AbilityUtil_Targeter_MultipleShapes abilityUtil_Targeter_MultipleShapes = new AbilityUtil_Targeter_MultipleShapes(ability, this.m_shapesList, subjects, base.IgnoreLos(), base.IncludeEnemies(), base.IncludeAllies(), base.IncludeCaster());
			abilityUtil_Targeter_MultipleShapes.SetAffectedGroups(base.IncludeEnemies(), base.IncludeAllies(), base.IncludeCaster());
			abilityUtil_Targeter_MultipleShapes.m_alwaysIncludeShapeCenterActor = this.RequireTargetingOnActor();
			abilityUtil_Targeter_MultipleShapes.SetShowArcToShape(this.m_showTargeterArc);
			return new List<AbilityUtil_Targeter>
			{
				abilityUtil_Targeter_MultipleShapes
			};
		}
		AbilityUtil_Targeter_MovingShape abilityUtil_Targeter_MovingShape = new AbilityUtil_Targeter_MovingShape(ability, this.GetShape(), base.IgnoreLos(), this.GetMoveLineWidth());
		abilityUtil_Targeter_MovingShape.SetAffectedGroups(base.IncludeEnemies(), base.IncludeAllies(), base.IncludeCaster());
		if (!base.IncludeAllies())
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
			if (!base.IncludeCaster())
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
				abilityUtil_Targeter_MovingShape.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
			}
		}
		abilityUtil_Targeter_MovingShape.SetShowArcToShape(this.m_showTargeterArc);
		return new List<AbilityUtil_Targeter>
		{
			abilityUtil_Targeter_MovingShape
		};
	}

	public override bool HandleCanCastValidation(Ability ability, ActorData caster)
	{
		return !this.RequireTargetingOnActor() || ability.HasTargetableActorsInDecision(caster, this.CanTargetOnEnemies(), this.CanTargetOnAllies(), this.CanTargetOnSelf(), Ability.ValidateCheckPath.Ignore, !this.IgnoreLosToTargetActor(), false, true);
	}

	public override bool HandleCustomTargetValidation(Ability ability, ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (this.RequireTargetingOnActor())
		{
			bool result = false;
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
			ActorData actorData;
			if (boardSquareSafe != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.HandleCustomTargetValidation(Ability, ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
				}
				actorData = boardSquareSafe.OccupantActor;
			}
			else
			{
				actorData = null;
			}
			ActorData actorData2 = actorData;
			if (actorData2 != null)
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
				bool flag = ability.CanTargetActorInDecision(caster, actorData2, this.CanTargetOnEnemies(), this.CanTargetOnAllies(), this.CanTargetOnSelf(), Ability.ValidateCheckPath.Ignore, !this.IgnoreLosToTargetActor(), false, true);
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
					result = true;
				}
			}
			return result;
		}
		return true;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_Shape);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}

	public AbilityAreaShape GetShape()
	{
		return (this.m_targetSelMod == null) ? this.m_shape : this.m_targetSelMod.m_shapeMod.GetModifiedValue(this.m_shape);
	}

	public BoardSquare GetShapeCenterSquare(AbilityTarget target, ActorData caster)
	{
		if (this.m_centerSquareDelegate != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.GetShapeCenterSquare(AbilityTarget, ActorData)).MethodHandle;
			}
			return this.m_centerSquareDelegate(target, caster);
		}
		return Board.Get().GetBoardSquareSafe(target.GridPos);
	}

	public bool IsMovingShape(ActorData caster)
	{
		if (this.m_isMovingShapeDelegate != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.IsMovingShape(ActorData)).MethodHandle;
			}
			return this.m_isMovingShapeDelegate(caster);
		}
		return false;
	}

	public BoardSquare GetMoveStartSquare(AbilityTarget target, ActorData caster)
	{
		if (this.m_moveStartSquareDelegate != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.GetMoveStartSquare(AbilityTarget, ActorData)).MethodHandle;
			}
			return this.m_moveStartSquareDelegate(target, caster);
		}
		return caster.GetCurrentBoardSquare();
	}

	public Vector3 GetMoveStartFreePos(AbilityTarget target, ActorData caster)
	{
		if (this.m_moveStartFreePosDelegate != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_Shape.GetMoveStartFreePos(AbilityTarget, ActorData)).MethodHandle;
			}
			return this.m_moveStartFreePosDelegate(target, caster);
		}
		return caster.GetTravelBoardSquareWorldPosition();
	}

	public delegate BoardSquare CenterSquareDelegate(AbilityTarget currentTarget, ActorData caster);

	public delegate bool IsMovingShapeDelegate(ActorData caster);

	public delegate BoardSquare GetMoveStartSquareDelegate(AbilityTarget currentTarget, ActorData caster);

	public delegate Vector3 GetMoveStartFreePosDelegate(AbilityTarget currentTarget, ActorData caster);
}
