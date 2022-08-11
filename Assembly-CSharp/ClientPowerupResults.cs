using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ClientPowerupResults
{
	[JsonProperty]
	private List<ServerClientUtils.SequenceStartData> m_seqStartDataList;
	[JsonProperty]
	private ClientAbilityResults m_powerupAbilityResults;

	public ClientPowerupResults(
		List<ServerClientUtils.SequenceStartData> seqStartDataList,
		ClientAbilityResults clientAbilityResults)
	{
		m_seqStartDataList = seqStartDataList;
		m_powerupAbilityResults = clientAbilityResults;
	}

	public bool HasSequencesToStart()
	{
		if (m_seqStartDataList == null || m_seqStartDataList.Count == 0)
		{
			return false;
		}
		foreach (ServerClientUtils.SequenceStartData current in m_seqStartDataList)
		{
			if (current != null && current.HasSequencePrefab())
			{
				return true;
			}
		}
		return false;
	}

	public void RunResults()
	{
		if (HasSequencesToStart())
		{
			foreach (ServerClientUtils.SequenceStartData seqStartData in m_seqStartDataList)
			{
				seqStartData.CreateSequencesFromData(OnPowerupHitActor, OnPowerupHitPosition);
			}
		}
		else
		{
			if (ClientAbilityResults.DebugTraceOn)
			{
				Log.Warning(ClientAbilityResults.s_clientHitResultHeader + GetDebugDescription() + ": no Sequence to start, executing results directly");
			}
			m_powerupAbilityResults.RunClientAbilityHits();
		}
	}

	internal void OnPowerupHitActor(ActorData target)
	{
		m_powerupAbilityResults.OnAbilityHitActor(target);
	}

	internal void OnPowerupHitPosition(Vector3 position)
	{
		m_powerupAbilityResults.OnAbilityHitPosition(position);
	}

	internal string GetDebugDescription()
	{
		if (m_powerupAbilityResults != null)
		{
			return m_powerupAbilityResults.GetDebugDescription();
		}
		return "Powerup UNKNWON";
	}
}
