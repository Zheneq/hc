using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientEffectStartData
{
	public int m_effectGUID;

	public List<ServerClientUtils.SequenceStartData> m_sequenceStartDataList;

	public ActorData m_caster;

	public ActorData m_effectTarget;

	public List<StatusType> m_statuses;

	public List<StatusType> m_statusesOnTurnStart;

	public int m_absorb;

	public int m_expectedHoT;

	public bool m_isBuff;

	public bool m_isDebuff;

	public bool m_hasMovementDebuff;

	public ClientEffectStartData(int effectGUID, List<ServerClientUtils.SequenceStartData> sequenceStartDataList, ActorData effectTarget, ActorData caster, List<StatusType> statuses, List<StatusType> statusesOnTurnStart, int absorb, int expectedHoT, bool isBuff, bool isDebuff, bool hasMovementDebuff)
	{
		this.m_effectGUID = effectGUID;
		this.m_sequenceStartDataList = sequenceStartDataList;
		this.m_effectTarget = effectTarget;
		this.m_caster = caster;
		this.m_statuses = statuses;
		this.m_statusesOnTurnStart = statusesOnTurnStart;
		this.m_absorb = absorb;
		this.m_expectedHoT = expectedHoT;
		this.m_isBuff = isBuff;
		this.m_isDebuff = isDebuff;
		this.m_hasMovementDebuff = hasMovementDebuff;
	}

	internal void OnClientEffectStartSequenceHitActor(ActorData target)
	{
	}

	internal void OnClientEffectStartSequenceHitPosition(Vector3 position)
	{
	}
}
