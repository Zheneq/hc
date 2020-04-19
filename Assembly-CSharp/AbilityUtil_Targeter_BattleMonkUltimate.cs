using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BattleMonkUltimate : AbilityUtil_Targeter_Shape
{
	private AbilityAreaShape m_enemyShape;

	private bool m_enemyShapePenetratesLoS;

	private bool m_groundBasedMovement;

	private bool m_allowChargeThroughInvalidSquares = true;

	public AbilityUtil_Targeter_BattleMonkUltimate(Ability ability, AbilityAreaShape allyShape, bool allyShapePenetratesLoS, AbilityAreaShape enemyShape, bool enemyShapePenetratesLoS, bool groundBasedMovement) : base(ability, allyShape, allyShapePenetratesLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, true, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible)
	{
		this.m_enemyShape = enemyShape;
		this.m_enemyShapePenetratesLoS = enemyShapePenetratesLoS;
		this.m_groundBasedMovement = groundBasedMovement;
		base.SetAffectedGroups(true, true, true);
		this.m_showArcToShape = false;
	}

	private GameObject EnemyHighlight
	{
		get
		{
			if (this.m_highlights != null && this.m_highlights.Count > 1)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BattleMonkUltimate.get_EnemyHighlight()).MethodHandle;
				}
				return this.m_highlights[1];
			}
			return null;
		}
		set
		{
			if (this.m_highlights == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BattleMonkUltimate.set_EnemyHighlight(GameObject)).MethodHandle;
				}
				this.m_highlights = new List<GameObject>();
			}
			while (this.m_highlights.Count <= 1)
			{
				this.m_highlights.Add(null);
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_highlights[1] != null)
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
				base.DestroyObjectAndMaterials(this.m_highlights[1]);
				this.m_highlights[1] = null;
			}
			this.m_highlights[1] = value;
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		if (this.m_affectsEnemies)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BattleMonkUltimate.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (this.EnemyHighlight == null)
			{
				this.EnemyHighlight = HighlightUtils.Get().CreateShapeCursor(this.m_enemyShape, targetingActor == GameFlowData.Get().activeOwnedActorData);
			}
			this.EnemyHighlight.transform.position = base.Highlight.transform.position;
			this.EnemyHighlight.SetActive(true);
			BoardSquare gameplayRefSquare = base.GetGameplayRefSquare(currentTarget, targetingActor);
			if (gameplayRefSquare != null)
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
				List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_enemyShape, currentTarget.FreePos, gameplayRefSquare, this.m_enemyShapePenetratesLoS, targetingActor, targetingActor.\u0012(), null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
				Vector3 highlightGoalPos = base.GetHighlightGoalPos(currentTarget, targetingActor);
				foreach (ActorData actorData in actorsInShape)
				{
					if (!(actorData != targetingActor))
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
						if (this.m_affectsCaster != AbilityUtil_Targeter.AffectsActor.Possible)
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
					base.AddActorInRange(actorData, highlightGoalPos, targetingActor, AbilityTooltipSubject.Primary, false);
				}
			}
		}
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		BoardSquare boardSquare2 = targetingActor.\u0012();
		BoardSquarePathInfo boardSquarePathInfo;
		if (this.m_groundBasedMovement)
		{
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, boardSquare2, this.m_allowChargeThroughInvalidSquares);
		}
		else
		{
			boardSquarePathInfo = new BoardSquarePathInfo();
			boardSquarePathInfo.square = boardSquare2;
			BoardSquarePathInfo boardSquarePathInfo2 = new BoardSquarePathInfo();
			boardSquarePathInfo2.square = boardSquare;
			boardSquarePathInfo.next = boardSquarePathInfo2;
			boardSquarePathInfo2.prev = boardSquarePathInfo;
		}
		base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
	}

	protected override bool HandleAddActorInShape(ActorData potentialTarget, ActorData targetingActor, AbilityTarget currentTarget, Vector3 damageOrigin, ActorData bestTarget)
	{
		return potentialTarget.\u000E() == targetingActor.\u000E() && base.HandleAddActorInShape(potentialTarget, targetingActor, currentTarget, damageOrigin, bestTarget);
	}
}
