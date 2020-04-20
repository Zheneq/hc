using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientReactionResults
{
	private ClientEffectResults m_effectResults;

	private List<ServerClientUtils.SequenceStartData> m_seqStartDataList;

	private byte m_extraFlags;

	private bool m_playedReaction;

	public ClientReactionResults(ClientEffectResults effectResults, List<ServerClientUtils.SequenceStartData> seqStartDataList, byte extraFlags)
	{
		this.m_effectResults = effectResults;
		this.m_seqStartDataList = seqStartDataList;
		this.m_extraFlags = extraFlags;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientReactionResults.HasSequencesToStart()).MethodHandle;
			}
			return false;
		}
		if (this.m_seqStartDataList.Count == 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			return false;
		}
		foreach (ServerClientUtils.SequenceStartData sequenceStartData in this.m_seqStartDataList)
		{
			if (sequenceStartData != null)
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
				if (sequenceStartData.HasSequencePrefab())
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasUnexecutedReactionOnActor(ActorData actor)
	{
		return this.m_effectResults.HasUnexecutedHitOnActor(actor);
	}

	public bool ReactionHitsDone()
	{
		bool result;
		if (this.PlayedReaction())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientReactionResults.ReactionHitsDone()).MethodHandle;
			}
			result = this.m_effectResults.DoneHitting();
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool PlayedReaction()
	{
		return this.m_playedReaction;
	}

	public void PlayReaction()
	{
		if (!this.m_playedReaction)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientReactionResults.PlayReaction()).MethodHandle;
			}
			this.m_playedReaction = true;
			if (this.HasSequencesToStart())
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = this.m_seqStartDataList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ServerClientUtils.SequenceStartData sequenceStartData = enumerator.Current;
						sequenceStartData.CreateSequencesFromData(new SequenceSource.ActorDelegate(this.OnReactionHitActor), new SequenceSource.Vector3Delegate(this.OnReactionHitPosition));
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			else
			{
				if (ClientAbilityResults.LogMissingSequences)
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
					Log.Warning(ClientAbilityResults.s_clientHitResultHeader + this.GetDebugDescription() + ": no Sequence to start, executing results directly", new object[0]);
				}
				this.m_effectResults.RunClientEffectHits();
			}
		}
	}

	internal void OnReactionHitActor(ActorData target)
	{
		this.m_effectResults.OnEffectHitActor(target);
	}

	internal void OnReactionHitPosition(Vector3 position)
	{
		this.m_effectResults.OnEffectHitPosition(position);
	}

	internal byte GetExtraFlags()
	{
		return this.m_extraFlags;
	}

	internal ActorData GetCaster()
	{
		return this.m_effectResults.GetCaster();
	}

	internal Dictionary<ActorData, ClientActorHitResults> GetActorHitResults()
	{
		return this.m_effectResults.GetActorHitResults();
	}

	internal Dictionary<Vector3, ClientPositionHitResults> GetPosHitResults()
	{
		return this.m_effectResults.GetPosHitResults();
	}

	internal string GetDebugDescription()
	{
		return this.m_effectResults.GetDebugDescription();
	}

	public enum ExtraFlags
	{
		None,
		ClientExecuteOnFirstDamagingHit,
		TriggerOnFirstDamageIfReactOnAttacker,
		TriggerOnFirstDamageOnReactionCaster = 4
	}
}
