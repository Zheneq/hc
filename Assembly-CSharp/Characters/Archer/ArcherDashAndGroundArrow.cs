// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ArcherDashAndGroundArrow : Ability
{
	[Header("-- Targeting")]
	public float m_groundArrowMaxRange = 6f;
	public float m_groundArrowMinRange = 1f;
	public bool m_groundArrowPenetratesLoS;
	public float m_laserWidth = 1f;
	public float m_laserRange = 5.5f;
	[Header("-- Oil Slick")]
	public StandardGroundEffectInfo m_groundEffect;
	public StandardBarrierData m_stopMovementBarrierData;
	[Header("-- Sequences")]
	public GameObject m_dashSequencePrefab;
	public GameObject m_arrowProjectileSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ArcherDashAndGroundArrow";
		}
		Setup();
	}

	private void Setup()
	{
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}
#if SERVER
	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = base.CalcPointsOfInterestForCamera(targets, caster);
		list.Add(GetShapePos(targets[0], m_groundEffect.m_groundEffectData.shape, caster, null));
		return list;
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_dashSequencePrefab,
				targets[0].FreePos,
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource),
			new ServerClientUtils.SequenceStartData(
				m_arrowProjectileSequencePrefab,
				GetShapePos(targets[0], m_groundEffect.m_groundEffectData.shape, caster, null),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> list = new List<NonActorTargetInfo>();
		Vector3 shapePos = GetShapePos(targets[0], m_groundEffect.m_groundEffectData.shape, caster, list);
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(shapePos));
		positionHitResults.AddEffect(new StandardGroundEffect(
			AsEffectSource(),
			Board.Get().GetSquareFromVec3(shapePos),
			shapePos,
			null,
			caster,
			m_groundEffect.m_groundEffectData));
		abilityResults.StorePositionHit(positionHitResults);
		abilityResults.StoreNonActorTargetInfo(list);
	}

	// added in rogues
	private Vector3 GetShapePos(AbilityTarget currentTarget, AbilityAreaShape shape, ActorData caster, List<NonActorTargetInfo> nonActorTargets)
	{
		Vector3 freePos = currentTarget.FreePos;
		Vector3 normalized = (caster.GetSquareAtPhaseStart().ToVector3() - freePos).normalized;
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			normalized,
			m_laserRange,
			m_laserWidth,
			caster,
			caster.GetOtherTeams(),
			false,
			1,
			false,
			false,
			out laserCoords.end,
			nonActorTargets);
		return AreaEffectUtils.GetCenterOfShape(shape, laserCoords.end, Board.Get().GetSquareFromVec3(laserCoords.end));
	}
#endif
}
