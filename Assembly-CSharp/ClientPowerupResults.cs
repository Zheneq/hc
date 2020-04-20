using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientPowerupResults
{
	private List<ServerClientUtils.SequenceStartData> m_seqStartDataList;

	private ClientAbilityResults m_powerupAbilityResults;

	public ClientPowerupResults(List<ServerClientUtils.SequenceStartData> seqStartDataList, ClientAbilityResults clientAbilityResults)
	{
		this.m_seqStartDataList = seqStartDataList;
		this.m_powerupAbilityResults = clientAbilityResults;
	}

	public bool HasSequencesToStart()
	{
		if (this.m_seqStartDataList == null)
		{
			return false;
		}
		if (this.m_seqStartDataList.Count == 0)
		{
			return false;
		}
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = this.m_seqStartDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ServerClientUtils.SequenceStartData sequenceStartData = enumerator.Current;
				if (sequenceStartData != null && sequenceStartData.HasSequencePrefab())
				{
					return true;
				}
			}
		}
		return false;
	}

	public void RunResults()
	{
		if (this.HasSequencesToStart())
		{
			foreach (ServerClientUtils.SequenceStartData sequenceStartData in this.m_seqStartDataList)
			{
				sequenceStartData.CreateSequencesFromData(new SequenceSource.ActorDelegate(this.OnPowerupHitActor), new SequenceSource.Vector3Delegate(this.OnPowerupHitPosition));
			}
		}
		else
		{
			if (ClientAbilityResults.LogMissingSequences)
			{
				Log.Warning(ClientAbilityResults.s_clientHitResultHeader + this.GetDebugDescription() + ": no Sequence to start, executing results directly", new object[0]);
			}
			this.m_powerupAbilityResults.RunClientAbilityHits();
		}
	}

	internal void OnPowerupHitActor(ActorData target)
	{
		this.m_powerupAbilityResults.OnAbilityHitActor(target);
	}

	internal void OnPowerupHitPosition(Vector3 position)
	{
		this.m_powerupAbilityResults.OnAbilityHitPosition(position);
	}

	internal string GetDebugDescription()
	{
		if (this.m_powerupAbilityResults != null)
		{
			return this.m_powerupAbilityResults.GetDebugDescription();
		}
		return "Powerup UNKNWON";
	}
}
