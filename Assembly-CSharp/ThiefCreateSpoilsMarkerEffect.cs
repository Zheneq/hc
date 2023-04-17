// ROGUES
// SERVER
using UnityEngine;

#if SERVER
// added in rogues
public class ThiefCreateSpoilsMarkerEffect : Effect
{
	private GameObject m_persistentSequencePrefab;

	public ThiefCreateSpoilsMarkerEffect(EffectSource parent, ActorData caster, GameObject persistentSequencePrefab)
		: base(parent, null, caster, caster)
	{
		m_effectName = "Thief Spoil Marker";
		m_persistentSequencePrefab = persistentSequencePrefab;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(
			m_persistentSequencePrefab,
			Caster.GetCurrentBoardSquare(),
			null,
			Caster,
			SequenceSource);
	}
}
#endif
