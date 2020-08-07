using System.Collections.Generic;
using UnityEngine;

public class ClientMovementResults
{
	public ActorData m_triggeringMover;

	public BoardSquarePathInfo m_triggeringPath;

	public List<ServerClientUtils.SequenceStartData> m_seqStartDataList;

	private bool m_alreadyReacted;

	public ClientEffectResults m_effectResults;

	public ClientBarrierResults m_barrierResults;

	public ClientAbilityResults m_powerupResults;

	public ClientAbilityResults m_gameModeResults;

	public ClientMovementResults(ActorData triggeringMover, BoardSquarePathInfo triggeringPath, List<ServerClientUtils.SequenceStartData> seqStartDataList, ClientEffectResults effectResults, ClientBarrierResults barrierResults, ClientAbilityResults powerupResults, ClientAbilityResults gameModeResults)
	{
		m_triggeringMover = triggeringMover;
		m_triggeringPath = triggeringPath;
		m_seqStartDataList = seqStartDataList;
		m_effectResults = effectResults;
		m_barrierResults = barrierResults;
		m_powerupResults = powerupResults;
		m_gameModeResults = gameModeResults;
		if (m_effectResults != null)
		{
			m_effectResults.MarkActorHitsAsMovementHits();
		}
		if (m_barrierResults != null)
		{
			m_barrierResults.MarkActorHitsAsMovementHits();
		}
		if (m_powerupResults != null)
		{
			m_powerupResults.MarkActorHitsAsMovementHits();
		}
		if (m_gameModeResults != null)
		{
			m_gameModeResults.MarkActorHitsAsMovementHits();
		}
		m_alreadyReacted = false;
	}

	public bool TriggerMatchesMovement(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (m_alreadyReacted)
		{
			return false;
		}
		if (mover != m_triggeringMover)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return MovementUtils.ArePathSegmentsEquivalent_FromBeginning(m_triggeringPath, curPath);
	}

	public bool HasSequencesToStart()
	{
		if (m_seqStartDataList == null)
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
		if (m_seqStartDataList.Count == 0)
		{
			while (true)
			{
				switch (1)
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
					while (true)
					{
						switch (3)
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

	public void ReactToMovement()
	{
		if (HasSequencesToStart())
		{
			using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = m_seqStartDataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ServerClientUtils.SequenceStartData current = enumerator.Current;
					current.CreateSequencesFromData(OnMoveResultsHitActor, OnMoveResultsHitPosition);
				}
			}
		}
		else
		{
			if (ClientAbilityResults.DebugTraceOn)
			{
				Log.Warning(ClientAbilityResults.s_clientHitResultHeader + GetDebugDescription() + ": no Sequence to start, executing results directly");
			}
			if (m_effectResults != null)
			{
				m_effectResults.RunClientEffectHits();
			}
			else if (m_barrierResults != null)
			{
				m_barrierResults.RunClientBarrierHits();
			}
			else if (m_powerupResults != null)
			{
				m_powerupResults.RunClientAbilityHits();
			}
			else if (m_gameModeResults != null)
			{
				m_gameModeResults.RunClientAbilityHits();
			}
		}
		m_alreadyReacted = true;
	}

	internal void OnMoveResultsHitActor(ActorData target)
	{
		if (m_effectResults != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_effectResults.OnEffectHitActor(target);
					return;
				}
			}
		}
		if (m_barrierResults != null)
		{
			m_barrierResults.OnBarrierHitActor(target);
			return;
		}
		if (m_powerupResults != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_powerupResults.OnAbilityHitActor(target);
					return;
				}
			}
		}
		if (m_gameModeResults == null)
		{
			return;
		}
		while (true)
		{
			m_gameModeResults.OnAbilityHitActor(target);
			return;
		}
	}

	internal void OnMoveResultsHitPosition(Vector3 position)
	{
		if (m_effectResults != null)
		{
			m_effectResults.OnEffectHitPosition(position);
			return;
		}
		if (m_barrierResults != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_barrierResults.OnBarrierHitPosition(position);
					return;
				}
			}
		}
		if (m_powerupResults != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_powerupResults.OnAbilityHitPosition(position);
					return;
				}
			}
		}
		if (m_gameModeResults == null)
		{
			return;
		}
		while (true)
		{
			m_gameModeResults.OnAbilityHitPosition(position);
			return;
		}
	}

	internal bool DoneHitting()
	{
		if (m_effectResults != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_effectResults.DoneHitting();
				}
			}
		}
		if (m_barrierResults != null)
		{
			return m_barrierResults.DoneHitting();
		}
		if (m_powerupResults != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_powerupResults.DoneHitting();
				}
			}
		}
		if (m_gameModeResults != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_gameModeResults.DoneHitting();
				}
			}
		}
		Debug.LogError("ClientMovementResults has neither effect results nor barrier results nor powerup results.  Assuming it's done hitting...");
		return true;
	}

	public bool HasUnexecutedHitOnActor(ActorData actor)
	{
		bool result = false;
		if (m_effectResults != null)
		{
			result = m_effectResults.HasUnexecutedHitOnActor(actor);
		}
		else if (m_barrierResults != null)
		{
			result = m_barrierResults.HasUnexecutedHitOnActor(actor);
		}
		else if (m_powerupResults != null)
		{
			result = m_powerupResults.HasUnexecutedHitOnActor(actor);
		}
		else if (m_gameModeResults != null)
		{
			result = m_gameModeResults.HasUnexecutedHitOnActor(actor);
		}
		return result;
	}

	public bool HasEffectHitResults()
	{
		return m_effectResults != null;
	}

	public bool HasBarrierHitResults()
	{
		return m_barrierResults != null;
	}

	public bool HasPowerupHitResults()
	{
		return m_powerupResults != null;
	}

	public bool HasGameModeHitResults()
	{
		return m_gameModeResults != null;
	}

	public bool ContainsSequenceSource(SequenceSource sequenceSource)
	{
		return sequenceSource != null && ContainsSequenceSourceID(sequenceSource.RootID);
	}

	public bool ContainsSequenceSourceID(uint id)
	{
		bool result = false;
		if (m_seqStartDataList != null)
		{
			int num = 0;
			while (true)
			{
				if (num < m_seqStartDataList.Count)
				{
					if (m_seqStartDataList[num].ContainsSequenceSourceID(id))
					{
						result = true;
						break;
					}
					num++;
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
		if (m_effectResults != null)
		{
			str = m_effectResults.GetDebugDescription();
		}
		else if (m_barrierResults != null)
		{
			str = m_barrierResults.GetDebugDescription();
		}
		else if (m_powerupResults != null)
		{
			str = m_powerupResults.GetDebugDescription();
		}
		else if (m_gameModeResults != null)
		{
			str = m_gameModeResults.GetDebugDescription();
		}
		return str + " triggering on " + m_triggeringMover.DebugNameString();
	}

	internal void ExecuteUnexecutedClientHits()
	{
		if (m_effectResults != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_effectResults.ExecuteUnexecutedClientHits();
					return;
				}
			}
		}
		if (m_barrierResults != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_barrierResults.ExecuteUnexecutedClientHits();
					return;
				}
			}
		}
		if (m_powerupResults != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_powerupResults.ExecuteUnexecutedClientHits();
					return;
				}
			}
		}
		if (m_gameModeResults == null)
		{
			return;
		}
		while (true)
		{
			m_gameModeResults.ExecuteUnexecutedClientHits();
			return;
		}
	}

	internal void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		if (m_effectResults != null)
		{
			m_effectResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
			return;
		}
		if (m_barrierResults != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_barrierResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
					return;
				}
			}
		}
		if (m_powerupResults != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_powerupResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
					return;
				}
			}
		}
		if (m_gameModeResults == null)
		{
			return;
		}
		while (true)
		{
			m_gameModeResults.ExecuteReactionHitsWithExtraFlagsOnActor(targetActor, caster, hasDamage, hasHealing);
			return;
		}
	}

	internal string UnexecutedHitsDebugStr()
	{
		string text = "\n\tUnexecuted hits:\n\t\tMovement hit on " + m_triggeringMover.DebugNameString() + "\n";
		if (m_effectResults != null)
		{
			text += m_effectResults.UnexecutedHitsDebugStr();
		}
		else if (m_barrierResults != null)
		{
			text += m_barrierResults.UnexecutedHitsDebugStr();
		}
		else if (m_powerupResults != null)
		{
			text += m_powerupResults.UnexecutedHitsDebugStr();
		}
		else if (m_gameModeResults != null)
		{
			text += m_gameModeResults.UnexecutedHitsDebugStr();
		}
		return text;
	}
}
