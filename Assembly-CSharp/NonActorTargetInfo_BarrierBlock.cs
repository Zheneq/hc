// ROGUES
// SERVER
using UnityEngine;

// server-only
#if SERVER
public class NonActorTargetInfo_BarrierBlock : NonActorTargetInfo
{
	public Barrier m_barrier;
	public Vector3 m_crossPos;
	public Vector3 m_fromPos;

	public NonActorTargetInfo_BarrierBlock(Barrier barrier, Vector3 crossPos, Vector3 fromPos)
	{
		m_barrier = barrier;
		m_crossPos = crossPos;
		m_fromPos = fromPos;
	}

	public override string GetDebugIdentifier()
	{
        return $"BarrierCrossHit: {m_barrier} | crossPos: {m_crossPos}";
	}

	public MovementResults GetReactHitResults(ActorData caster)
	{
		if (m_barrier == null)
		{
			Log.Error("Barrier in barrier block data is null, skipping");
			return null;
		}
		ActorData caster2 = m_barrier.Caster;
		Ability sourceAbility = m_barrier.GetSourceAbility();
		if (caster2 != null && sourceAbility != null && m_barrier.GetResponseOnShotBlock() != null)
		{
			BarrierResponseOnShot responseOnShotBlock = m_barrier.GetResponseOnShotBlock();
			SequenceSource sequenceSource = new SequenceSource(null, null, true, null, null);
			MovementResults movementResults = new MovementResults(MovementStage.INVALID);
			movementResults.SetupTriggerData(caster2, null);
			movementResults.SetupGameplayDataForAbility(sourceAbility, caster2);
			movementResults.SetupSequenceData(null, caster2.GetCurrentBoardSquare(), sequenceSource, null, true);
			m_barrier.GetResponseOnShotBlock().AddToReactionResult(movementResults, caster, caster2);
			Vector3 incomingDir = m_crossPos - m_fromPos;
			incomingDir.y = 0f;
			Vector3 targetPos = m_crossPos;
			if (responseOnShotBlock.m_useShooterPosAsReactionSequenceTargetPos)
			{
				targetPos = caster.GetFreePos();
			}
			targetPos.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
			Vector3 collisionNormal = m_barrier.GetCollisionNormal(incomingDir);
			ServerClientUtils.SequenceStartData startData = new ServerClientUtils.SequenceStartData(responseOnShotBlock.m_onShotSequencePrefab, targetPos, Quaternion.LookRotation(collisionNormal), null, caster2, sequenceSource, null);
			movementResults.AddSequenceStartOverride(startData, sequenceSource, true);
			return movementResults;
		}
		if (Application.isEditor && AbilityResults.DebugTraceOn)
		{
			Debug.LogWarning("    No barrier owner or source ability when trying to generate react hit results");
		}
		return null;
	}

	public void AddPositionReactionHitToAbilityResults(ActorData caster, PositionHitResults posHitRes, AbilityResults abilityResults, bool storePositionHit)
	{
		MovementResults reactHitResults = this.GetReactHitResults(caster);
		if (reactHitResults != null)
		{
			posHitRes.AddReactionOnPositionHit(reactHitResults);
			abilityResults.IntegrateHpDeltaForPositionReactResult(reactHitResults);
			if (storePositionHit)
			{
				abilityResults.StorePositionHit(posHitRes);
			}
		}
	}
}
#endif
