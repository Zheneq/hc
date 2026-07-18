// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only, missing in reactor
#if SERVER
public class TempSpoilVfxEffect : Effect
{
	private List<VfxSpawnData> m_vfxSpawnDataList;
	private AbilityPriority m_spawnPhase = AbilityPriority.INVALID;
	private bool m_pastSpawnPhase;

	public TempSpoilVfxEffect(EffectSource parent, ActorData caster, List<VfxSpawnData> vfxSpawnDataList, AbilityPriority spawnPhase)
		: base(parent, null, null, caster)
	{
		m_vfxSpawnDataList = vfxSpawnDataList;
		m_spawnPhase = spawnPhase;
		m_time.duration = 1;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_vfxSpawnDataList != null)
		{
			foreach (VfxSpawnData vfxSpawnData in m_vfxSpawnDataList)
			{
				PowerUp.ExtraParams extraParams = new PowerUp.ExtraParams();
				extraParams.m_pickupTeamAsInt = 2;
				if (vfxSpawnData.m_restrictPickupTeam)
				{
					extraParams.m_pickupTeamAsInt = (int)base.Caster.GetTeam();
				}
				Sequence.IExtraSequenceParams[] extraParams2 = new Sequence.IExtraSequenceParams[]
				{
					extraParams
				};
				ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(vfxSpawnData.m_prefab, vfxSpawnData.m_spawnSquare, null, base.Caster, base.SequenceSource, extraParams2);
				list.Add(item);
			}
		}
		return list;
	}

	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		base.OnAbilityPhaseEnd(phase);
		if (phase >= m_spawnPhase)
		{
			m_pastSpawnPhase = true;
		}
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || m_pastSpawnPhase;
	}

	public class VfxSpawnData
	{
		public BoardSquare m_spawnSquare;
		public GameObject m_prefab;
		public bool m_restrictPickupTeam = true;

		public VfxSpawnData(BoardSquare spawnSquare, GameObject seqPrefab, bool restrictPickupByTeam)
		{
			m_spawnSquare = spawnSquare;
			m_prefab = seqPrefab;
			m_restrictPickupTeam = restrictPickupByTeam;
		}
	}
}
#endif
