using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientMovementResults
{
	public ActorData m_triggeringMover;

	public BoardSquarePathInfo m_triggeringPath;

	public List<ServerClientUtils.SequenceStartData> m_seqStartDataList;

	private bool m_alreadyReacted;

	private ClientEffectResults m_effectResults;

	private ClientBarrierResults m_barrierResults;

	private ClientAbilityResults m_powerupResults;

	private ClientAbilityResults m_gameModeResults;

	public ClientMovementResults(ActorData triggeringMover, BoardSquarePathInfo triggeringPath, List<ServerClientUtils.SequenceStartData> seqStartDataList, ClientEffectResults effectResults, ClientBarrierResults barrierResults, ClientAbilityResults powerupResults, ClientAbilityResults gameModeResults)
	{
		this.m_triggeringMover = triggeringMover;
		this.m_triggeringPath = triggeringPath;
		this.m_seqStartDataList = seqStartDataList;
		this.m_effectResults = effectResults;
		this.m_barrierResults = barrierResults;
		this.m_powerupResults = powerupResults;
		this.m_gameModeResults = gameModeResults;
		if (this.m_effectResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults..ctor(ActorData, BoardSquarePathInfo, List<ServerClientUtils.SequenceStartData>, ClientEffectResults, ClientBarrierResults, ClientAbilityResults, ClientAbilityResults)).MethodHandle;
			}
			this.m_effectResults.MarkActorHitsAsMovementHits();
		}
		if (this.m_barrierResults != null)
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
			this.m_barrierResults.MarkActorHitsAsMovementHits();
		}
		if (this.m_powerupResults != null)
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
			this.m_powerupResults.MarkActorHitsAsMovementHits();
		}
		if (this.m_gameModeResults != null)
		{
			this.m_gameModeResults.MarkActorHitsAsMovementHits();
		}
		this.m_alreadyReacted = false;
	}

	public bool TriggerMatchesMovement(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (this.m_alreadyReacted)
		{
			return false;
		}
		if (mover != this.m_triggeringMover)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.TriggerMatchesMovement(ActorData, BoardSquarePathInfo)).MethodHandle;
			}
			return false;
		}
		return MovementUtils.ArePathSegmentsEquivalent_FromBeginning(this.m_triggeringPath, curPath);
	}

	public bool HasSequencesToStart()
	{
		if (this.m_seqStartDataList == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.HasSequencesToStart()).MethodHandle;
			}
			return false;
		}
		if (this.m_seqStartDataList.Count == 0)
		{
			for (;;)
			{
				switch (1)
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
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					return true;
				}
			}
		}
		return false;
	}

	public void ReactToMovement()
	{
		if (this.HasSequencesToStart())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.ReactToMovement()).MethodHandle;
			}
			using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = this.m_seqStartDataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ServerClientUtils.SequenceStartData sequenceStartData = enumerator.Current;
					sequenceStartData.CreateSequencesFromData(new SequenceSource.ActorDelegate(this.OnMoveResultsHitActor), new SequenceSource.Vector3Delegate(this.OnMoveResultsHitPosition));
				}
				for (;;)
				{
					switch (3)
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
			if (ClientAbilityResults.\u001D)
			{
				Log.Warning(ClientAbilityResults.s_clientHitResultHeader + this.GetDebugDescription() + ": no Sequence to start, executing results directly", new object[0]);
			}
			if (this.m_effectResults != null)
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
				this.m_effectResults.RunClientEffectHits();
			}
			else if (this.m_barrierResults != null)
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
				this.m_barrierResults.RunClientBarrierHits();
			}
			else if (this.m_powerupResults != null)
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
				this.m_powerupResults.RunClientAbilityHits();
			}
			else if (this.m_gameModeResults != null)
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
				this.m_gameModeResults.RunClientAbilityHits();
			}
		}
		this.m_alreadyReacted = true;
	}

	internal void OnMoveResultsHitActor(ActorData target)
	{
		if (this.m_effectResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.OnMoveResultsHitActor(ActorData)).MethodHandle;
			}
			this.m_effectResults.OnEffectHitActor(target);
		}
		else if (this.m_barrierResults != null)
		{
			this.m_barrierResults.OnBarrierHitActor(target);
		}
		else if (this.m_powerupResults != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_powerupResults.OnAbilityHitActor(target);
		}
		else if (this.m_gameModeResults != null)
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
			this.m_gameModeResults.OnAbilityHitActor(target);
		}
	}

	internal void OnMoveResultsHitPosition(Vector3 position)
	{
		if (this.m_effectResults != null)
		{
			this.m_effectResults.OnEffectHitPosition(position);
		}
		else if (this.m_barrierResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.OnMoveResultsHitPosition(Vector3)).MethodHandle;
			}
			this.m_barrierResults.OnBarrierHitPosition(position);
		}
		else if (this.m_powerupResults != null)
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
			this.m_powerupResults.OnAbilityHitPosition(position);
		}
		else if (this.m_gameModeResults != null)
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
			this.m_gameModeResults.OnAbilityHitPosition(position);
		}
	}

	internal bool DoneHitting()
	{
		bool result;
		if (this.m_effectResults != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.DoneHitting()).MethodHandle;
			}
			result = this.m_effectResults.DoneHitting();
		}
		else if (this.m_barrierResults != null)
		{
			result = this.m_barrierResults.DoneHitting();
		}
		else if (this.m_powerupResults != null)
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
			result = this.m_powerupResults.DoneHitting();
		}
		else if (this.m_gameModeResults != null)
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
			result = this.m_gameModeResults.DoneHitting();
		}
		else
		{
			Debug.LogError("ClientMovementResults has neither effect results nor barrier results nor powerup results.  Assuming it's done hitting...");
			result = true;
		}
		return result;
	}

	public bool HasUnexecutedHitOnActor(ActorData actor)
	{
		bool result = false;
		if (this.m_effectResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.HasUnexecutedHitOnActor(ActorData)).MethodHandle;
			}
			result = this.m_effectResults.HasUnexecutedHitOnActor(actor);
		}
		else if (this.m_barrierResults != null)
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
			result = this.m_barrierResults.HasUnexecutedHitOnActor(actor);
		}
		else if (this.m_powerupResults != null)
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
			result = this.m_powerupResults.HasUnexecutedHitOnActor(actor);
		}
		else if (this.m_gameModeResults != null)
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
			result = this.m_gameModeResults.HasUnexecutedHitOnActor(actor);
		}
		return result;
	}

	public bool HasEffectHitResults()
	{
		return this.m_effectResults != null;
	}

	public bool HasBarrierHitResults()
	{
		return this.m_barrierResults != null;
	}

	public bool HasPowerupHitResults()
	{
		return this.m_powerupResults != null;
	}

	public bool HasGameModeHitResults()
	{
		return this.m_gameModeResults != null;
	}

	public bool ContainsSequenceSource(SequenceSource sequenceSource)
	{
		return sequenceSource != null && this.ContainsSequenceSourceID(sequenceSource.RootID);
	}

	public bool ContainsSequenceSourceID(uint id)
	{
		bool result = false;
		if (this.m_seqStartDataList != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.ContainsSequenceSourceID(uint)).MethodHandle;
			}
			for (int i = 0; i < this.m_seqStartDataList.Count; i++)
			{
				if (this.m_seqStartDataList[i].ContainsSequenceSourceID(id))
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
					return true;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return result;
	}

	public string GetDebugDescription()
	{
		string str = string.Empty;
		if (this.m_effectResults != null)
		{
			str = this.m_effectResults.GetDebugDescription();
		}
		else if (this.m_barrierResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.GetDebugDescription()).MethodHandle;
			}
			str = this.m_barrierResults.GetDebugDescription();
		}
		else if (this.m_powerupResults != null)
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
			str = this.m_powerupResults.GetDebugDescription();
		}
		else if (this.m_gameModeResults != null)
		{
			str = this.m_gameModeResults.GetDebugDescription();
		}
		return str + " triggering on " + this.m_triggeringMover.\u0018();
	}

	internal void ExecuteUnexecutedClientHits()
	{
		if (this.m_effectResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.ExecuteUnexecutedClientHits()).MethodHandle;
			}
			this.m_effectResults.ExecuteUnexecutedClientHits();
		}
		else if (this.m_barrierResults != null)
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
			this.m_barrierResults.ExecuteUnexecutedClientHits();
		}
		else if (this.m_powerupResults != null)
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
			this.m_powerupResults.ExecuteUnexecutedClientHits();
		}
		else if (this.m_gameModeResults != null)
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
			this.m_gameModeResults.ExecuteUnexecutedClientHits();
		}
	}

	internal void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		if (this.m_effectResults != null)
		{
			this.m_effectResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
		}
		else if (this.m_barrierResults != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.ExecuteReactionHitsWithExtraFlagsOnActor(ActorData, ActorData, bool, bool)).MethodHandle;
			}
			this.m_barrierResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
		}
		else if (this.m_powerupResults != null)
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
			this.m_powerupResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
		}
		else if (this.m_gameModeResults != null)
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
			this.m_gameModeResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
		}
	}

	internal string UnexecutedHitsDebugStr()
	{
		string text = "\n\tUnexecuted hits:\n\t\tMovement hit on " + this.m_triggeringMover.\u0018() + "\n";
		if (this.m_effectResults != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientMovementResults.UnexecutedHitsDebugStr()).MethodHandle;
			}
			text += this.m_effectResults.UnexecutedHitsDebugStr();
		}
		else if (this.m_barrierResults != null)
		{
			text += this.m_barrierResults.UnexecutedHitsDebugStr();
		}
		else if (this.m_powerupResults != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			text += this.m_powerupResults.UnexecutedHitsDebugStr();
		}
		else if (this.m_gameModeResults != null)
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
			text += this.m_gameModeResults.UnexecutedHitsDebugStr();
		}
		return text;
	}
}
