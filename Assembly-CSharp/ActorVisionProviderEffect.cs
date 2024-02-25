// ROGUES
// SERVER
using UnityEngine;

// custom
#if SERVER
public class ActorVisionProviderEffect : Effect
{
	private readonly float m_visionRadius;
	private readonly VisionProviderInfo.BrushRevealType m_brushRevealType;
	private readonly bool m_ignoreLos;
	private readonly GameObject m_persistentSequencePrefab;
	private readonly bool m_canFunctionInGlobalBlind;
	private readonly bool m_useStraightLineDist;

	public ActorVisionProviderEffect(
		EffectSource parent,
		ActorData actor,
		int duration,
		float visionRadius,
		VisionProviderInfo.BrushRevealType brushRevealType,
		bool ignoreLos,
		GameObject persistentSequence,
		bool canFunctionInGlobalBlind,
		bool useStraightLineDist)
		: base(parent, actor.GetCurrentBoardSquare(), actor, actor)
	{
		m_effectName = "Actor Vision Provider Effect";
		m_time.duration = Mathf.Max(1, duration);
		m_visionRadius = visionRadius;
		m_brushRevealType = brushRevealType;
		m_ignoreLos = ignoreLos;
		m_persistentSequencePrefab = persistentSequence;
		m_canFunctionInGlobalBlind = canFunctionInGlobalBlind;
		m_useStraightLineDist = useStraightLineDist;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(
			m_persistentSequencePrefab,
			Caster.GetCurrentBoardSquare(),
			new ActorData[0],
			Caster,
			SequenceSource);
	}

	public override void OnStart()
	{
		if (Caster.GetAdditionalActorVisionProviders() != null)
		{
			Caster.GetAdditionalActorVisionProviders()
				.AddVisionProviderOnActor(
					Caster.ActorIndex,
					m_visionRadius,
					m_useStraightLineDist,
					m_brushRevealType,
					m_ignoreLos,
					m_canFunctionInGlobalBlind,
					BoardSquare.VisibilityFlags.Team);
		}

		foreach (ActorData teamMember in GameFlowData.Get().GetAllTeamMembers(Caster.GetTeam()))
		{
			teamMember.GetFogOfWar()?.ImmediateUpdateVisibilityOfSquares();
		}
	}

	public override void OnEnd()
	{
		if (Caster.GetAdditionalActorVisionProviders() != null)
		{
			Caster.GetAdditionalActorVisionProviders()
				.RemoveVisionProviderOnActor(
					Caster.ActorIndex,
					m_visionRadius,
					m_useStraightLineDist,
					m_brushRevealType,
					m_ignoreLos,
					m_canFunctionInGlobalBlind,
					BoardSquare.VisibilityFlags.Team);
		}
	}

	// TODO VISION hack?
	public override void OnTurnStart()
	{
		foreach (ActorData actor in GameFlowData.Get().GetActors())
		{
			if (actor.GetTeam() != Caster.GetTeam() && actor.IsActorVisibleToAnyEnemy())
			{
				Log.Info($"Requesting SynchronizeTeamSensitiveData for {actor.DisplayName} for actor being visible on turn start with active vision provider effect");
				actor.SynchronizeTeamSensitiveData();
			}
		}
	}

	public override bool TargetMustHaveAccuratePositionOnClients()
	{
		return true;
	}
}
#endif
