using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_Barrier : GenericAbility_TargetSelectBase
{
	[Separator("Targeting Properties. Width should be overridden by ability most of the time, since it contains barrier data", true)]
	public float m_barrierWidth = 1f;

	[Header("-- For Laser Targeting in front and behind barrier. Used if ranges are > 0")]
	public float m_laserRangeFront = -1f;

	public float m_laserRangeBack = -1f;

	[Header("-- Whether lasers length ignore walls")]
	public bool m_laserRangeIgnoreLos = true;

	private bool m_snapToGrid = true;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public override string GetUsageForEditor()
	{
		return "For placing a barrier, with optional laser templates in front and back.\n" + base.GetContextUsageStr(ContextKeys.symbol_000E.GetName(), "non-actor specific context, set as width of barrier", false) + base.GetContextUsageStr(ContextKeys.symbol_0015.GetName(), "non-actor specific context, center position of barrier", false) + base.GetContextUsageStr(ContextKeys.symbol_0009.GetName(), "non-actor specific context, facing direction of barrier", false);
	}

	public override void ListContextNamesForEditor(List<string> keys)
	{
		keys.Add(ContextKeys.symbol_000E.GetName());
		keys.Add(ContextKeys.symbol_0015.GetName());
		keys.Add(ContextKeys.symbol_0009.GetName());
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		float barrierWidth = this.m_barrierWidth;
		if (ability.GetExpectedNumberOfTargeters() < 2)
		{
			list.Add(new AbilityUtil_Targeter_BarrierWithLasers(ability, barrierWidth, this.m_laserRangeFront, this.m_laserRangeBack, base.IgnoreLos(), this.m_laserRangeIgnoreLos));
		}
		else
		{
			list.Add(new AbilityUtil_Targeter_Shape(ability, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible));
			AbilityUtil_Targeter_BarrierWithLasers abilityUtil_Targeter_BarrierWithLasers = new AbilityUtil_Targeter_BarrierWithLasers(ability, barrierWidth, this.m_laserRangeFront, this.m_laserRangeBack, base.IgnoreLos(), this.m_laserRangeIgnoreLos);
			abilityUtil_Targeter_BarrierWithLasers.SetUseMultiTargetUpdate(true);
			list.Add(abilityUtil_Targeter_BarrierWithLasers);
		}
		return list;
	}

	private List<ActorData> GetLaserHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, base.IncludeAllies(), base.IncludeEnemies());
		Vector3 barrierPos;
		Vector3 barrierFacing;
		TargetSelect_Barrier.GetBarrierPositionAndFacing(targets, this.m_snapToGrid, out barrierPos, out barrierFacing);
		return TargetSelect_Barrier.GetLaserHitActors(barrierPos, barrierFacing, caster, nonActorTargetInfo, this.m_snapToGrid, this.m_barrierWidth, this.m_laserRangeFront, this.m_laserRangeBack, this.m_ignoreLos, this.m_laserRangeIgnoreLos, relevantTeams);
	}

	public unsafe static void GetBarrierPositionAndFacing(List<AbilityTarget> targets, bool snapToGrid, out Vector3 position, out Vector3 facing)
	{
		facing = targets[0].AimDirection;
		position = targets[0].FreePos;
		if (snapToGrid)
		{
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(targets[0].GridPos);
			if (boardSquareSafe != null)
			{
				Vector3 freePos = targets[0].FreePos;
				if (targets.Count > 1)
				{
					freePos = targets[1].FreePos;
				}
				facing = VectorUtils.GetDirectionToClosestSide(boardSquareSafe, freePos);
				position = boardSquareSafe.ToVector3() + 0.5f * Board.Get().squareSize * facing;
			}
		}
	}

	public static List<ActorData> GetLaserHitActors(Vector3 barrierPos, Vector3 barrierFacing, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo, bool snapToGrid, float barrierWidth, float laserRangeFront, float laserRangeBack, bool ignoreLos, bool laserRangeIgnoreLos, List<Team> teams)
	{
		List<ActorData> list = new List<ActorData>();
		Vector3 startPos = barrierPos;
		startPos.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		if (laserRangeFront > 0f)
		{
			Vector3 vector;
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(startPos, barrierFacing, laserRangeFront, barrierWidth, caster, teams, ignoreLos, -1, laserRangeIgnoreLos, true, out vector, nonActorTargetInfo, null, true, true);
			list.AddRange(actorsInLaser);
		}
		if (laserRangeBack > 0f)
		{
			Vector3 vector2;
			List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(startPos, -1f * barrierFacing, laserRangeBack, barrierWidth, caster, teams, ignoreLos, -1, laserRangeIgnoreLos, true, out vector2, nonActorTargetInfo, null, true, true);
			using (List<ActorData>.Enumerator enumerator = actorsInLaser2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData item = enumerator.Current;
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
		}
		return list;
	}
}
