// ROGUES
// SERVER
using UnityEngine;

// reactor-only, missing in rogues
public class PowerUp_VisionBuff : PowerUp_Standard_Ability
{
	[Separator("Vision Buff")]
	public float m_visionRadius = 8.5f;
	public bool m_visionUseStraightLineDist;
	public int m_visionDuration = 2;
	public VisionProviderInfo.BrushRevealType m_visionBrushRevealType = VisionProviderInfo.BrushRevealType.Always;
	public bool m_visionAreaIgnoreLos = true;
	public bool m_visionAreaCanFunctionInGlobalBlind = true;
	[Separator("Sequence on actor with vision buff", true)]
	public GameObject m_visionBuffSeqPrefab;

#if SERVER
	// custom
	public override ActorHitResults CreateActorHitResults(
		PowerUp powerUp,
		ActorData targetActor,
		Vector3 origin,
		StandardPowerUpAbilityModData powerupMod,
		EffectSource effectSourceOverride,
		bool isDirectActorHit)
	{
		ActorHitResults actorHitResults = base.CreateActorHitResults(
			powerUp, targetActor, origin, powerupMod, effectSourceOverride, isDirectActorHit);
		actorHitResults.AddEffect(new ActorVisionProviderEffect(
			AsEffectSource(),
			targetActor,
			m_visionDuration,
			m_visionRadius,
			m_visionBrushRevealType,
			m_visionAreaIgnoreLos,
			m_visionBuffSeqPrefab,
			m_visionAreaCanFunctionInGlobalBlind,
			m_visionUseStraightLineDist));
		return actorHitResults;
	}
#endif
}
