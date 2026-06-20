// ROGUES
// SERVER
using System.Collections.Generic;

public class Passive_Ninja : Passive
{
	public class NinjaRewindMemoryEntry
	{
		public int m_turn;
		public int m_health;
		public int m_squareX;
		public int m_squareY;
		public List<int> m_abilityCooldowns = new List<int>();

		public NinjaRewindMemoryEntry(int turn, int health, BoardSquare square, AbilityData abilityData)
		{
			m_turn = turn;
			m_health = health;
			m_squareX = square != null ? square.x : -1;
			m_squareY = square != null ? square.y : -1;
			for (int i = 0; i < 5; i++)
			{
				m_abilityCooldowns.Add(abilityData != null
					? abilityData.GetCooldownRemaining((AbilityData.ActionType)i)
					: 0);
			}
		}
	}

	public int m_numRewindTurns = 1;
	
#if SERVER
	// added in rogues
	private Ninja_SyncComponent m_syncComp;
	// added in rogues
	private AbilityData m_abilityData;
	// added in rogues
	private NinjaVanish m_vanishAbility;
	// added in rogues
	private NinjaShurikenOrDash m_dashAbility;
	// added in rogues
	private List<int> m_cooldownsForRewind = new List<int>();
	// added in rogues
	private List<NinjaRewindMemoryEntry> m_rewindMemory = new List<NinjaRewindMemoryEntry>();
	// added in rogues
	internal StandardEffectInfo m_vanishSelfEffectOnTurnStart;
	// added in rogues
	internal Dictionary<ActorData, StandardEffectInfo> m_dashActorToExtraEffectMap = new Dictionary<ActorData, StandardEffectInfo>();

	// added in rogues
	protected override void OnStartup()
	{
		base.OnStartup();
		m_syncComp = Owner.GetComponent<Ninja_SyncComponent>();
		m_abilityData = Owner.GetComponent<AbilityData>();
		if (m_abilityData != null)
		{
			m_vanishAbility = m_abilityData.GetAbilityOfType(typeof(NinjaVanish)) as NinjaVanish;
			m_dashAbility = m_abilityData.GetAbilityOfType(typeof(NinjaShurikenOrDash)) as NinjaShurikenOrDash;
		}
		for (int i = 0; i < 5; i++)
		{
			m_cooldownsForRewind.Add(0);
		}
	}

	// added in rogues
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (!Owner.IsDead() && !Owner.IsActorVisibleToAnyEnemy())
		{
			Owner.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.TeleportingNinjaStats.TurnsNotVisibleToEnemies);
		}
		HandleDashAbilityOnTurnStart();
		HandleVanishAbilityOnTurnStart();
		HandleRewindOnTurnStart();
	}

	// added in rogues
	private void HandleDashAbilityOnTurnStart()
	{
		foreach (ActorData actorData in m_dashActorToExtraEffectMap.Keys)
		{
			if (actorData != null && actorData.GetTeam() != Owner.GetTeam() && !actorData.IsDead())
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
				actorHitResults.AddStandardEffectInfo(m_dashActorToExtraEffectMap[actorData]);
				MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, actorHitResults, m_dashAbility);
			}
		}
		m_dashActorToExtraEffectMap.Clear();
	}

	// added in rogues
	private void HandleVanishAbilityOnTurnStart()
	{
		if (m_vanishSelfEffectOnTurnStart != null)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
			actorHitResults.AddStandardEffectInfo(m_vanishSelfEffectOnTurnStart);
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, actorHitResults, m_vanishAbility);
			m_vanishSelfEffectOnTurnStart = null;
		}
	}

	// added in rogues
	private void HandleRewindOnTurnStart()
	{
		m_rewindMemory.Add(
			new NinjaRewindMemoryEntry(
				GameFlowData.Get().CurrentTurn,
				Owner.HitPoints,
				Owner.GetCurrentBoardSquare(),
				m_abilityData));
		while (m_rewindMemory.Count > m_numRewindTurns + 1)
		{
			m_rewindMemory.RemoveAt(0);
		}
		if (m_syncComp != null)
		{
			if (m_rewindMemory.Count == m_numRewindTurns + 1)
			{
				NinjaRewindMemoryEntry ninjaRewindMemoryEntry = m_rewindMemory[0];
				m_syncComp.Networkm_rewindHToHp = (short)ninjaRewindMemoryEntry.m_health;
				m_syncComp.Networkm_rewindToSquareX = (short)ninjaRewindMemoryEntry.m_squareX;
				m_syncComp.Networkm_rewindToSquareY = (short)ninjaRewindMemoryEntry.m_squareY;
			}
			else
			{
				m_syncComp.Networkm_rewindHToHp = 0;
				m_syncComp.ClearSquareForRewind();
			}
		}
		if (m_rewindMemory.Count == m_numRewindTurns + 1)
		{
			NinjaRewindMemoryEntry ninjaRewindMemoryEntry2 = m_rewindMemory[0];
			for (int i = 0; i < ninjaRewindMemoryEntry2.m_abilityCooldowns.Count; i++)
			{
				if (i >= m_cooldownsForRewind.Count)
				{
					return;
				}
				m_cooldownsForRewind[i] = ninjaRewindMemoryEntry2.m_abilityCooldowns[i];
			}
		}
		else
		{
			for (int j = 0; j < m_cooldownsForRewind.Count; j++)
			{
				m_cooldownsForRewind[j] = -1;
			}
		}
	}

	// added in rogues
	public int GetRemainingCooldownForRewind(int actionTypeInt)
	{
		return m_abilityData != null
		       && actionTypeInt >= 0
		       && actionTypeInt < m_cooldownsForRewind.Count
			? m_cooldownsForRewind[actionTypeInt]
			: 0;
	}
#endif
}
