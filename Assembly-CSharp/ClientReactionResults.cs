using System.Collections.Generic;
using UnityEngine;

public class ClientReactionResults
{
	public enum ExtraFlags
	{
		None = 0,
		ClientExecuteOnFirstDamagingHit = 1,
		TriggerOnFirstDamageIfReactOnAttacker = 2,
		TriggerOnFirstDamageOnReactionCaster = 4
	}

	public ClientEffectResults m_effectResults;

	public List<ServerClientUtils.SequenceStartData> m_seqStartDataList;

	public byte m_extraFlags;

	private bool m_playedReaction;

	public ClientReactionResults(ClientEffectResults effectResults, List<ServerClientUtils.SequenceStartData> seqStartDataList, byte extraFlags)
	{
		m_effectResults = effectResults;
		m_seqStartDataList = seqStartDataList;
		m_extraFlags = extraFlags;
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
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		foreach (ServerClientUtils.SequenceStartData seqStartData in m_seqStartDataList)
		{
			if (seqStartData != null)
			{
				if (seqStartData.HasSequencePrefab())
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasUnexecutedReactionOnActor(ActorData actor)
	{
		return m_effectResults.HasUnexecutedHitOnActor(actor);
	}

	public bool ReactionHitsDone()
	{
		int result;
		if (PlayedReaction())
		{
			result = (m_effectResults.DoneHitting() ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool PlayedReaction()
	{
		return m_playedReaction;
	}

	public void PlayReaction()
	{
		if (m_playedReaction)
		{
			return;
		}
		while (true)
		{
			m_playedReaction = true;
			if (HasSequencesToStart())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = m_seqStartDataList.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ServerClientUtils.SequenceStartData current = enumerator.Current;
								current.CreateSequencesFromData(OnReactionHitActor, OnReactionHitPosition);
							}
							while (true)
							{
								switch (7)
								{
								default:
									return;
								case 0:
									break;
								}
							}
						}
					}
					}
				}
			}
			if (ClientAbilityResults.DebugTraceOn)
			{
				Log.Warning(ClientAbilityResults.s_clientHitResultHeader + GetDebugDescription() + ": no Sequence to start, executing results directly");
			}
			m_effectResults.RunClientEffectHits();
			return;
		}
	}

	internal void OnReactionHitActor(ActorData target)
	{
		m_effectResults.OnEffectHitActor(target);
	}

	internal void OnReactionHitPosition(Vector3 position)
	{
		m_effectResults.OnEffectHitPosition(position);
	}

	internal byte GetExtraFlags()
	{
		return m_extraFlags;
	}

	internal ActorData GetCaster()
	{
		return m_effectResults.GetCaster();
	}

	internal Dictionary<ActorData, ClientActorHitResults> GetActorHitResults()
	{
		return m_effectResults.GetActorHitResults();
	}

	internal Dictionary<Vector3, ClientPositionHitResults> GetPosHitResults()
	{
		return m_effectResults.GetPosHitResults();
	}

	internal string GetDebugDescription()
	{
		return m_effectResults.GetDebugDescription();
	}
}
