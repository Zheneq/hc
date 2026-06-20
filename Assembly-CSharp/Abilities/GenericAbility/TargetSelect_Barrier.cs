using AbilityContextNamespace;
using System.Collections.Generic;
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
		return "For placing a barrier, with optional laser templates in front and back.\n" + GetContextUsageStr(ContextKeys.s_BarrierWidth.GetName(), "non-actor specific context, set as width of barrier", false) + GetContextUsageStr(ContextKeys.s_CenterPos.GetName(), "non-actor specific context, center position of barrier", false) + GetContextUsageStr(ContextKeys.s_FacingDir.GetName(), "non-actor specific context, facing direction of barrier", false);
	}

	public override void ListContextNamesForEditor(List<string> keys)
	{
		keys.Add(ContextKeys.s_BarrierWidth.GetName());
		keys.Add(ContextKeys.s_CenterPos.GetName());
		keys.Add(ContextKeys.s_FacingDir.GetName());
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		float barrierWidth = m_barrierWidth;
		if (ability.GetExpectedNumberOfTargeters() < 2)
		{
			list.Add(new AbilityUtil_Targeter_BarrierWithLasers(ability, barrierWidth, m_laserRangeFront, m_laserRangeBack, IgnoreLos(), m_laserRangeIgnoreLos));
		}
		else
		{
			list.Add(new AbilityUtil_Targeter_Shape(ability, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false));
			AbilityUtil_Targeter_BarrierWithLasers abilityUtil_Targeter_BarrierWithLasers = new AbilityUtil_Targeter_BarrierWithLasers(ability, barrierWidth, m_laserRangeFront, m_laserRangeBack, IgnoreLos(), m_laserRangeIgnoreLos);
			abilityUtil_Targeter_BarrierWithLasers.SetUseMultiTargetUpdate(true);
			list.Add(abilityUtil_Targeter_BarrierWithLasers);
		}
		return list;
	}

	private List<ActorData> GetLaserHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies());
		GetBarrierPositionAndFacing(targets, m_snapToGrid, out Vector3 position, out Vector3 facing);
		return GetLaserHitActors(position, facing, caster, nonActorTargetInfo, m_snapToGrid, m_barrierWidth, m_laserRangeFront, m_laserRangeBack, m_ignoreLos, m_laserRangeIgnoreLos, relevantTeams);
	}

	public static void GetBarrierPositionAndFacing(List<AbilityTarget> targets, bool snapToGrid, out Vector3 position, out Vector3 facing)
	{
		facing = targets[0].AimDirection;
		position = targets[0].FreePos;
		if (!snapToGrid)
		{
			return;
		}
		while (true)
		{
			BoardSquare boardSquareSafe = Board.Get().GetSquare(targets[0].GridPos);
			if (!(boardSquareSafe != null))
			{
				return;
			}
			while (true)
			{
				Vector3 freePos = targets[0].FreePos;
				if (targets.Count > 1)
				{
					freePos = targets[1].FreePos;
				}
				facing = VectorUtils.GetDirectionToClosestSide(boardSquareSafe, freePos);
				position = boardSquareSafe.ToVector3() + 0.5f * Board.Get().squareSize * facing;
				return;
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
			Vector3 laserEndPos;
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(startPos, barrierFacing, laserRangeFront, barrierWidth, caster, teams, ignoreLos, -1, laserRangeIgnoreLos, true, out laserEndPos, nonActorTargetInfo, null, true);
			list.AddRange(actorsInLaser);
		}
		if (laserRangeBack > 0f)
		{
			Vector3 laserEndPos2;
			List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(startPos, -1f * barrierFacing, laserRangeBack, barrierWidth, caster, teams, ignoreLos, -1, laserRangeIgnoreLos, true, out laserEndPos2, nonActorTargetInfo, null, true);
			using (List<ActorData>.Enumerator enumerator = actorsInLaser2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (!list.Contains(current))
					{
						list.Add(current);
					}
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return list;
					}
				}
			}
		}
		return list;
	}
}
