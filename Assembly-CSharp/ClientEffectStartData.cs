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
		m_effectGUID = effectGUID;
		m_sequenceStartDataList = sequenceStartDataList;
		m_effectTarget = effectTarget;
		m_caster = caster;
		m_statuses = statuses;
		m_statusesOnTurnStart = statusesOnTurnStart;
		m_absorb = absorb;
		m_expectedHoT = expectedHoT;
		m_isBuff = isBuff;
		m_isDebuff = isDebuff;
		m_hasMovementDebuff = hasMovementDebuff;
	}

	public string Json()
	{
		string seqStarts = "";
		if (!m_sequenceStartDataList.IsNullOrEmpty())
		{
			foreach (var e in m_sequenceStartDataList)
			{
				seqStarts += (seqStarts.Length == 0 ? "" : ",\n") + e.Json();
			}
		}
		string statuses = "";
		if (!m_statuses.IsNullOrEmpty())
		{
			foreach (var e in m_statuses)
			{
				statuses += (statuses.Length == 0 ? "" : ", ") + e;
			}
		}
		string statusesATS = "";
		if (!m_statusesOnTurnStart.IsNullOrEmpty())
		{
			foreach (var e in m_statusesOnTurnStart)
			{
				statusesATS += (statusesATS.Length == 0 ? "" : ", ") + e;
			}
		}
		return $"{{" +
			$"\"effectGUID\": {m_effectGUID}, " +
			$"\"caster\": \"{m_caster?.DisplayName ?? "none"}\", " +
			$"\"effectTarget\": \"{m_effectTarget?.DisplayName ?? "none"}\", " +
			$"\"statuses\": [{statuses}], " +
			$"\"statusesOnTurnStart\": [{statusesATS}], " +
			$"\"seqStartDataList\": [{seqStarts}], " +
			$"\"absorb\": {m_absorb}, " +
			$"\"expectedHoT\": {m_expectedHoT}, " +
			$"\"isBuff\": {m_isBuff}, " +
			$"\"isDebuff\": {m_isDebuff}, " +
			$"\"hasMovementDebuff\": {m_hasMovementDebuff}" +
			$"}}";
	}

	internal void OnClientEffectStartSequenceHitActor(ActorData target)
	{
	}

	internal void OnClientEffectStartSequenceHitPosition(Vector3 position)
	{
	}
}
