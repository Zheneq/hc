using System.Collections.Generic;
using UnityEngine;

public class ClientPowerupResults
{
	private List<ServerClientUtils.SequenceStartData> m_seqStartDataList;

	private ClientAbilityResults m_powerupAbilityResults;

	public ClientPowerupResults(List<ServerClientUtils.SequenceStartData> seqStartDataList, ClientAbilityResults clientAbilityResults)
	{
		m_seqStartDataList = seqStartDataList;
		m_powerupAbilityResults = clientAbilityResults;
	}

	public void SerializeToStream(ref IBitStream stream)
	{
		AbilityResultsUtils.SerializeSequenceStartDataListToStream(ref stream, m_seqStartDataList);
		m_powerupAbilityResults.SerializeToStream(ref stream);
	}

	public bool HasSequencesToStart()
	{
		if (m_seqStartDataList == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (m_seqStartDataList.Count == 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = m_seqStartDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ServerClientUtils.SequenceStartData current = enumerator.Current;
				if (current != null && current.HasSequencePrefab())
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public void RunResults()
	{
		if (HasSequencesToStart())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					foreach (ServerClientUtils.SequenceStartData seqStartData in m_seqStartDataList)
					{
						seqStartData.CreateSequencesFromData(OnPowerupHitActor, OnPowerupHitPosition);
					}
					return;
				}
			}
		}
		if (ClientAbilityResults.WarningEnabled)
		{
			Log.Warning(ClientAbilityResults.s_clientHitResultHeader + GetDebugDescription() + ": no Sequence to start, executing results directly");
		}
		m_powerupAbilityResults.RunClientAbilityHits();
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_powerupAbilityResults.GetDebugDescription();
				}
			}
		}
		return "Powerup UNKNWON";
	}
}
