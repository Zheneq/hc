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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientPowerupResults.HasSequencesToStart()).MethodHandle;
			}
			return false;
		}
		if (this.m_seqStartDataList.Count == 0)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			return false;
		}
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = this.m_seqStartDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ServerClientUtils.SequenceStartData sequenceStartData = enumerator.Current;
				if (sequenceStartData != null && sequenceStartData.HasSequencePrefab())
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					return true;
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public void RunResults()
	{
		if (this.HasSequencesToStart())
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientPowerupResults.RunResults()).MethodHandle;
			}
			foreach (ServerClientUtils.SequenceStartData sequenceStartData in this.m_seqStartDataList)
			{
				sequenceStartData.CreateSequencesFromData(new SequenceSource.ActorDelegate(this.OnPowerupHitActor), new SequenceSource.Vector3Delegate(this.OnPowerupHitPosition));
			}
		}
		else
		{
			if (ClientAbilityResults.\u001D)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientPowerupResults.GetDebugDescription()).MethodHandle;
			}
			return this.m_powerupAbilityResults.GetDebugDescription();
		}
		return "Powerup UNKNWON";
	}
}
