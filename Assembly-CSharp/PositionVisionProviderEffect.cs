// ROGUES
// SERVER
using UnityEngine;

// server-only
#if SERVER
public class PositionVisionProviderEffect : Effect
{
	private float m_visionRadius = 1f;
	private VisionProviderInfo.BrushRevealType m_brushRevealType;
	private bool m_ignoreLos;
	private GameObject m_persistentSequencePrefab;

	// TODO LOW probably related to blind mode?
	// missing in rogues
	private bool m_canFunctionInGlobalBlind = false;
	// missing in rogues
	private bool m_useStraightLineDist = false;

	public PositionVisionProviderEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData caster,
		int duration,
		float visionRadius,
		VisionProviderInfo.BrushRevealType brushRevealType,
		bool ignoreLos,
		GameObject persistentSequence)
		: base(parent, targetSquare, null, caster)
	{
		m_effectName = "Position Vision Provider Effect";
		m_time.duration = Mathf.Max(1, duration);
		m_visionRadius = visionRadius;
		m_brushRevealType = brushRevealType;
		m_ignoreLos = ignoreLos;
		m_persistentSequencePrefab = persistentSequence;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(
			m_persistentSequencePrefab,
			TargetSquare.ToVector3(),
			new ActorData[0],
			Caster,
			SequenceSource);
	}

	public override void OnStart()
	{
		if (Caster.GetAdditionalActorVisionProviders() != null)
		{
			Caster.GetAdditionalActorVisionProviders()
				.AddVisionProviderOnGridPos(
					TargetSquare.GetGridPos(),
					m_visionRadius,
					m_useStraightLineDist,
					m_brushRevealType,
					m_ignoreLos,
					m_canFunctionInGlobalBlind,
					BoardSquare.VisibilityFlags.Team);
		}
	}

	public override void OnEnd()
	{
		if (Caster.GetAdditionalActorVisionProviders() != null)
		{
			Caster.GetAdditionalActorVisionProviders()
				.RemoveVisionProviderOnGridPos(
					TargetSquare.GetGridPos(),
					m_visionRadius,
					m_useStraightLineDist,
					m_brushRevealType,
					m_ignoreLos,
					m_canFunctionInGlobalBlind,
					BoardSquare.VisibilityFlags.Team);
		}
	}
}
#endif
