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

	public PositionVisionProviderEffect(EffectSource parent, BoardSquare targetSquare, ActorData caster, int duration, float visionRadius, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, GameObject persistentSequence)
		: base(parent, targetSquare, null, caster)
	{
		this.m_effectName = "Position Vision Provider Effect";
		this.m_time.duration = Mathf.Max(1, duration);
		this.m_visionRadius = visionRadius;
		this.m_brushRevealType = brushRevealType;
		this.m_ignoreLos = ignoreLos;
		this.m_persistentSequencePrefab = persistentSequence;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(this.m_persistentSequencePrefab, base.TargetSquare.ToVector3(), new ActorData[0], base.Caster, base.SequenceSource, null);
	}

	public override void OnStart()
	{
		if (base.Caster.GetAdditionalActorVisionProviders() != null)
		{
			base.Caster.GetAdditionalActorVisionProviders().AddVisionProviderOnGridPos(base.TargetSquare.GetGridPos(), this.m_visionRadius, this.m_brushRevealType, this.m_ignoreLos, BoardSquare.VisibilityFlags.Team);
		}
	}

	public override void OnEnd()
	{
		if (base.Caster.GetAdditionalActorVisionProviders() != null)
		{
			base.Caster.GetAdditionalActorVisionProviders().RemoveVisionProviderOnGridPos(base.TargetSquare.GetGridPos(), this.m_visionRadius, this.m_brushRevealType, this.m_ignoreLos, BoardSquare.VisibilityFlags.Team);
		}
	}
}
#endif
