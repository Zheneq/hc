// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Shift, Fade, Regroup
public class ExampleAbility_Flash : Ability
{
	[Separator("Effect on Caster")]
	public bool m_applyEffect;
	public StandardActorEffectData m_effectToApply;
	[Separator("Whether require to dash next to another actor")]
	public bool m_requireTargetNextToActor;
	[Header("-- Range to consider actor as valid dash-to target")]
	public float m_dashToTargetSelectRange = 8.5f;
	[Header("-- Distance around target to consider as valid squares")]
	public float m_dashToOtherRange = 1.5f;
	public bool m_canDashNextToAllies = true;
	public bool m_canDashNextToEnemies = true;
	[Separator("Sequences")]
	public GameObject m_startSquareSequence;
	public GameObject m_endSquareSequence;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Flash";
		}
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			false,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false);
		Targeter.ShowArcToShape = false;
		if (m_tags != null && !m_tags.Contains(AbilityTags.UseTeleportUIEffect))
		{
			m_tags.Add(AbilityTags.UseTeleportUIEffect);
		}
		
#if SERVER
		// custom
		// TODO HACK stealth abilities
		if (IsStealthEvade())
		{
			if (m_tags != null)
			{
				if (!m_tags.Contains(AbilityTags.DontBreakCasterInvisibilityOnCast))
				{
					m_tags.Add(AbilityTags.DontBreakCasterInvisibilityOnCast);
				}
			}
			else
			{
				Log.Error($"Failed to add stealth evade tag to {GetDebugIdentifier()}!");
			}
		}
		// end custom
#endif
	}

	public override bool IsFlashAbility()
	{
		return true;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return m_requireTargetNextToActor && m_dashToTargetSelectRange > 0f
		       || base.CanShowTargetableRadiusPreview();
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		if (m_requireTargetNextToActor && m_dashToTargetSelectRange > 0f)
		{
			return m_dashToTargetSelectRange;
		}
		return base.GetTargetableRadiusInSquares(caster);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (!m_requireTargetNextToActor)
		{
			return base.CustomCanCastValidation(caster);
		}
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, m_canDashNextToAllies, m_canDashNextToEnemies);
		float num = m_dashToTargetSelectRange;
		if (num <= 0f)
		{
			num = 50f;
		}
		List<ActorData> actors = AreaEffectUtils.GetActorsInRadius(caster.GetFreePos(), num, true, caster, relevantTeams, null);
		actors.Remove(caster);
		if (NetworkClient.active)
		{
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		}
		else
		{
			TargeterUtils.RemoveActorsInvisibleToActor(ref actors, caster);
		}
		return actors.Count > 0;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null
		    || !targetSquare.IsValidForGameplay()
		    || targetSquare == caster.GetCurrentBoardSquare())
		{
			return false;
		}

		if (!m_requireTargetNextToActor)
		{
			return true;
		}
		ActorData targetingActor = NetworkServer.active
			? caster
			: GameFlowData.Get().activeOwnedActorData;
		List<ActorData> actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(targetingActor);
		bool isValid = false;
		Vector3 casterPos = caster.GetCurrentBoardSquare().ToVector3();
		foreach (ActorData targetActor in actorsVisibleToActor)
		{
			if (isValid)
			{
				break;
			}
			bool isInRangeFromCaster = true;
			if (m_dashToTargetSelectRange > 0f)
			{
				isInRangeFromCaster = AreaEffectUtils.IsSquareInConeByActorRadius(
					targetActor.GetCurrentBoardSquare(),
					casterPos,
					0f,
					360f,
					m_dashToTargetSelectRange,
					0f,
					true,
					caster);
			}
			if (!isInRangeFromCaster)
			{
				continue;
			}
			bool isTargetVisible = NetworkClient.active
				? targetActor.IsActorVisibleToClient()
				: targetActor.IsActorVisibleToActor(caster);
			if (!isTargetVisible || targetActor == caster)
			{
				continue;
			}
			bool isAlly = targetActor.GetTeam() == caster.GetTeam();
			if (!isAlly && m_canDashNextToEnemies || isAlly && m_canDashNextToAllies)
			{
				bool isInRangeFromTarget = AreaEffectUtils.IsSquareInConeByActorRadius(
					targetSquare,
					targetActor.GetFreePos(),
					0f,
					360f,
					m_dashToOtherRange,
					0f,
					true,
					caster);
				isValid = isValid || isInRangeFromTarget;
			}
		}
		return isValid;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Teleport;
	}

	internal override ActorData.TeleportType GetEvasionTeleportType()
	{
		return ActorData.TeleportType.Evasion_AdjustToVision;
	}

#if SERVER
	// added in rogues
	internal override bool IsStealthEvade()
	{
		if (!m_applyEffect)
		{
			return false;
		}
		foreach (StatusType statusChange in m_effectToApply.m_statusChanges)
		{
			if (statusChange == StatusType.InvisibleToEnemies)
			{
				return true;
			}
		}
		return false;
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_startSquareSequence,
				caster.GetSquareAtPhaseStart(),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource),
			new ServerClientUtils.SequenceStartData(
				m_endSquareSequence,
				Board.Get().GetSquare(targets[0].GridPos),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		if (m_applyEffect)
		{
			actorHitResults.AddEffect(new StandardActorEffect(
				AsEffectSource(),
				caster.GetCurrentBoardSquare(),
				caster,
				caster,
				m_effectToApply));
		}
		abilityResults.StoreActorHit(actorHitResults);
	}
#endif
}
