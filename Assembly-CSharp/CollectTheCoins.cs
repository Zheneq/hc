using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CollectTheCoins : NetworkBehaviour
{
	public enum CollectTheCoins_VictoryCondition
	{
		TeamMustHaveMostCoins,
		TeamMustNotHaveMostCoins
	}

	[Serializable]
	public class CoinAbilityMod
	{
		public float m_minCoinsForAnyBonus = -1f;

		public float m_maxCoinsForAnyBonus = -1f;

		public float m_bonusForHavingMin;

		public float m_bonusPerCoinOverMin;

		public float m_maxBonus = -1f;

		public bool BeingActiveMatters()
		{
			if (m_bonusForHavingMin == 0f)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (m_bonusPerCoinOverMin == 0f)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
			}
			return true;
		}

		public float GetBonusForCoins(int numCoins)
		{
			int num;
			if (!((float)numCoins >= m_minCoinsForAnyBonus))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = ((m_minCoinsForAnyBonus == -1f) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			bool flag = (byte)num != 0;
			int num2;
			if (!((float)numCoins <= m_maxCoinsForAnyBonus))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				num2 = ((m_maxCoinsForAnyBonus == -1f) ? 1 : 0);
			}
			else
			{
				num2 = 1;
			}
			bool flag2 = (byte)num2 != 0;
			if (flag)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (flag2)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
						{
							float num3 = m_bonusForHavingMin + m_bonusPerCoinOverMin * ((float)numCoins - m_minCoinsForAnyBonus);
							if (m_maxBonus >= 0f)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								num3 = Mathf.Min(num3, m_maxBonus);
							}
							return num3;
						}
						}
					}
				}
			}
			return 0f;
		}

		public float GetBonus_Client(ActorData actor)
		{
			if (!BeingActiveMatters())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return 0f;
					}
				}
			}
			int numCoins = 0;
			if (NetworkClient.active)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				numCoins = Get().GetCoinsForActor_Client(actor);
			}
			return GetBonusForCoins(numCoins);
		}
	}

	public class ClientSideData
	{
		public Dictionary<ActorData, int> m_actorsToCoins_unresolved;

		public Dictionary<BoardSquare, int> m_squaresToCoins_unresolved;

		public Dictionary<BoardSquare, List<GameObject>> m_squaresToSpillsVisuals;

		public Dictionary<BoardSquare, List<GameObject>> m_squaresToCoinVisuals;

		public ClientSideData()
		{
			m_actorsToCoins_unresolved = new Dictionary<ActorData, int>();
			m_squaresToCoins_unresolved = new Dictionary<BoardSquare, int>();
			m_squaresToSpillsVisuals = new Dictionary<BoardSquare, List<GameObject>>();
			m_squaresToCoinVisuals = new Dictionary<BoardSquare, List<GameObject>>();
		}
	}

	[Header("ObjectivePoints")]
	public int m_objPointsPerCoinOnTurnStart;

	public int m_objPointAdjustWhenHasMostCoinsOnTurnStart;

	public bool m_gainObjPointPerCoinPickedUp;

	public bool m_loseObjPointPerCoinDropped;

	[Header("Game Rules")]
	public int m_numCoinsToAwardPerCoinPowerup = 1;

	public int m_numCoinsToAwardPerNonCoinPowerup;

	public bool m_canSpillCoinsOnPowerupLocations;

	public bool m_dropCoinsOnDeath = true;

	public bool m_dropCoinsOnKnockback;

	public bool m_dropCoinsOnEvade;

	public bool m_evadersCanPickUpCoins = true;

	public bool m_knockbackedMoversCanPickUpCoins;

	[Header("Sequences")]
	public GameObject m_coinPickedUpSequence;

	public GameObject m_coinsDroppingSequence;

	public GameObject m_coinsSpillingSequence;

	[Header("Persistent Objects")]
	public GameObject m_coinOnGroundPrefab;

	public GameObject m_spillInAirPrefab;

	public float m_spillVerticalOffset = 1f;

	[Header("Scoundrel Trick Shot Mods")]
	public CoinAbilityMod m_bouncingLaserDamage;

	public CoinAbilityMod m_bouncingLaserTotalDistance;

	public CoinAbilityMod m_bouncingLaserBounceDistance;

	public CoinAbilityMod m_bouncingLaserBounces;

	public CoinAbilityMod m_bouncingLaserReduceBackupPlanCooldown;

	public CoinAbilityMod m_bouncingLaserPierces;

	private uint m_sequenceSourceId;

	private ClientSideData m_clientData;

	private static CollectTheCoins s_instance;

	private SequenceSource _sequenceSource;

	internal SequenceSource SequenceSource
	{
		get
		{
			if (_sequenceSource == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				_sequenceSource = new SequenceSource(null, null, m_sequenceSourceId, false);
			}
			return _sequenceSource;
		}
	}

	private void Awake()
	{
		if (s_instance == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			s_instance = this;
		}
		else
		{
			Log.Error("Multiple CollectTheCoins components in this scene, remove extraneous ones.");
		}
		if (NetworkServer.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			SequenceSource sequenceSource = new SequenceSource(null, null, false);
			m_sequenceSourceId = sequenceSource.RootID;
		}
		m_clientData = new ClientSideData();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public static CollectTheCoins Get()
	{
		return s_instance;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint sequenceSourceId = reader.ReadUInt32();
		sbyte b = reader.ReadSByte();
		sbyte b2 = reader.ReadSByte();
		m_sequenceSourceId = sequenceSourceId;
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		Dictionary<BoardSquare, int> dictionary2 = new Dictionary<BoardSquare, int>();
		for (sbyte b3 = 0; b3 < b; b3 = (sbyte)(b3 + 1))
		{
			sbyte b4 = reader.ReadSByte();
			sbyte b5 = reader.ReadSByte();
			ActorData key = GameFlowData.Get().FindActorByActorIndex(b4);
			dictionary.Add(key, b5);
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (sbyte b6 = 0; b6 < b2; b6 = (sbyte)(b6 + 1))
			{
				sbyte b7 = reader.ReadSByte();
				sbyte b8 = reader.ReadSByte();
				sbyte b9 = reader.ReadSByte();
				BoardSquare boardSquare = Board.Get().GetBoardSquare(b7, b8);
				dictionary2.Add(boardSquare, b9);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				SynchCoinVisualsToDictionary(dictionary2);
				m_clientData.m_actorsToCoins_unresolved = dictionary;
				m_clientData.m_squaresToCoins_unresolved = dictionary2;
				return;
			}
		}
	}

	public int GetCoinsForActor_Client(ActorData actor)
	{
		if (m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor))
		{
			return m_clientData.m_actorsToCoins_unresolved[actor];
		}
		return 0;
	}

	public void ExecuteClientGameModeEvent(ClientGameModeEvent gameModeEvent)
	{
		if (gameModeEvent == null)
		{
			return;
		}
		GameModeEventType eventType = gameModeEvent.m_eventType;
		if (eventType == GameModeEventType.Ctc_CoinPickedUp)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					BoardSquare square = gameModeEvent.m_square;
					if (m_clientData.m_squaresToCoins_unresolved.ContainsKey(square))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
							{
								int numCoins = m_clientData.m_squaresToCoins_unresolved[square];
								OnActorGainedCoins_Client(gameModeEvent.m_primaryActor, numCoins);
								RemoveCoinVisualsFromSquare(square);
								m_clientData.m_squaresToCoins_unresolved[square] = 0;
								m_clientData.m_squaresToCoins_unresolved.Remove(square);
								return;
							}
							}
						}
					}
					return;
				}
				}
			}
		}
		switch (eventType)
		{
		case GameModeEventType.Ctc_CoinsDropped:
		{
			BoardSquare square2 = gameModeEvent.m_square;
			OnActorDroppedCoins_Client(gameModeEvent.m_primaryActor, square2);
			break;
		}
		case GameModeEventType.Ctc_NonCoinPowerupTouched:
			OnActorGainedCoins_Client(gameModeEvent.m_primaryActor, m_numCoinsToAwardPerNonCoinPowerup);
			break;
		case GameModeEventType.Ctc_CoinPowerupTouched:
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				OnActorGainedCoins_Client(gameModeEvent.m_primaryActor, m_numCoinsToAwardPerCoinPowerup);
				return;
			}
		default:
			Debug.LogError("CollectTheCoins trying to handle non-Ctc event type " + eventType.ToString() + ".");
			break;
		}
	}

	public void OnActorGainedCoins_Client(ActorData actor, int numCoins)
	{
		if (actor == null)
		{
			Debug.LogError("CollectTheCoins (client)-- trying to assign coins to a null actor.");
			return;
		}
		if (numCoins == 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError($"CollectTheCoins (client)-- trying to assign 0 spawn coins to actor {actor.GetDebugName()}.");
					return;
				}
			}
		}
		if (!m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			m_clientData.m_actorsToCoins_unresolved.Add(actor, numCoins);
		}
		else
		{
			m_clientData.m_actorsToCoins_unresolved[actor] += numCoins;
		}
		if (!m_gainObjPointPerCoinPickedUp)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			Team team = actor.GetTeam();
			ObjectivePoints.Get().AdjustUnresolvedPoints(numCoins, team);
			return;
		}
	}

	public void OnActorDroppedCoins_Client(ActorData actor, BoardSquare square)
	{
		if (actor == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("CollectTheCoins (client)-- null actor trying to drop coins.");
					return;
				}
			}
		}
		if (square == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogError($"CollectTheCoins (client)-- actor {actor.GetDebugName()} trying to drop coins on a null square.");
					return;
				}
			}
		}
		if (!m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError($"CollectTheCoins (client)-- actor {actor.GetDebugName()} trying to drop coins on square {BoardSquare._001D(square)}, but that actor isn't in m_clientData.m_actorsToCoins_unresolved.");
					return;
				}
			}
		}
		int num = m_clientData.m_actorsToCoins_unresolved[actor];
		m_clientData.m_actorsToCoins_unresolved[actor] = 0;
		m_clientData.m_actorsToCoins_unresolved.Remove(actor);
		if (num == 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogError($"CollectTheCoins (client)-- actor {actor.GetDebugName()} trying to drop coins on square {BoardSquare._001D(square)}, but that actor has 0 coins.");
					return;
				}
			}
		}
		AddCoinSpillVisualToSquare(square, num);
		if (!m_loseObjPointPerCoinDropped)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			Team team = actor.GetTeam();
			ObjectivePoints.Get().AdjustUnresolvedPoints(-num, team);
			return;
		}
	}

	public void AddCoinVisualToSquare(BoardSquare square)
	{
		List<GameObject> list;
		if (!m_clientData.m_squaresToCoinVisuals.ContainsKey(square))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			list = new List<GameObject>();
			m_clientData.m_squaresToCoinVisuals.Add(square, list);
		}
		else
		{
			list = m_clientData.m_squaresToCoinVisuals[square];
		}
		Vector3 position = square.ToVector3();
		GameObject item = UnityEngine.Object.Instantiate(m_coinOnGroundPrefab, position, Quaternion.identity);
		list.Add(item);
	}

	public void RemoveCoinVisualsFromSquare(BoardSquare square)
	{
		if (!m_clientData.m_squaresToCoinVisuals.ContainsKey(square))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		List<GameObject> list = m_clientData.m_squaresToCoinVisuals[square];
		for (int i = 0; i < list.Count; i++)
		{
			GameObject obj = list[i];
			UnityEngine.Object.Destroy(obj);
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			list.Clear();
			m_clientData.m_squaresToCoinVisuals.Remove(square);
			return;
		}
	}

	public void SynchCoinVisualsToDictionary(Dictionary<BoardSquare, int> squaresToCoins)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		foreach (KeyValuePair<BoardSquare, List<GameObject>> squaresToCoinVisual in m_clientData.m_squaresToCoinVisuals)
		{
			BoardSquare key = squaresToCoinVisual.Key;
			int count = squaresToCoinVisual.Value.Count;
			int num;
			if (squaresToCoins.ContainsKey(key))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = squaresToCoins[key];
			}
			else
			{
				num = 0;
			}
			if (count > num)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				list.Add(key);
			}
		}
		using (List<BoardSquare>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				BoardSquare current2 = enumerator2.Current;
				RemoveCoinVisualsFromSquare(current2);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		using (Dictionary<BoardSquare, int>.Enumerator enumerator3 = squaresToCoins.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				KeyValuePair<BoardSquare, int> current3 = enumerator3.Current;
				BoardSquare key2 = current3.Key;
				int value = current3.Value;
				int num2;
				if (m_clientData.m_squaresToCoinVisuals.ContainsKey(key2))
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					num2 = m_clientData.m_squaresToCoinVisuals[key2].Count;
				}
				else
				{
					num2 = 0;
				}
				if (value > num2)
				{
					for (int i = num2; i < value; i++)
					{
						AddCoinVisualToSquare(key2);
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void AddCoinSpillVisualToSquare(BoardSquare square, int numCoinsInSpill)
	{
		if (m_spillInAirPrefab == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		List<GameObject> list;
		if (!m_clientData.m_squaresToSpillsVisuals.ContainsKey(square))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			list = new List<GameObject>();
			m_clientData.m_squaresToSpillsVisuals.Add(square, list);
		}
		else
		{
			list = m_clientData.m_squaresToSpillsVisuals[square];
		}
		Vector3 position = square.ToVector3() + Vector3.up * m_spillVerticalOffset;
		GameObject item = UnityEngine.Object.Instantiate(m_spillInAirPrefab, position, Quaternion.identity);
		list.Add(item);
	}

	public void ClearCoinSpillVisuals()
	{
		using (Dictionary<BoardSquare, List<GameObject>>.Enumerator enumerator = m_clientData.m_squaresToSpillsVisuals.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				List<GameObject> value = enumerator.Current.Value;
				for (int i = 0; i < value.Count; i++)
				{
					GameObject obj = value[i];
					UnityEngine.Object.Destroy(obj);
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						value.Clear();
						goto IL_0063;
					}
				}
				IL_0063:;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_clientData.m_squaresToSpillsVisuals.Clear();
	}

	public void Client_OnActorDeath(ActorData actor)
	{
		if (!m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor) || m_clientData.m_actorsToCoins_unresolved[actor] <= 0)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			BoardSquare travelBoardSquare = actor.GetTravelBoardSquare();
			OnActorDroppedCoins_Client(actor, travelBoardSquare);
			return;
		}
	}

	public void OnTurnTick()
	{
		ClearCoinSpillVisuals();
	}

	private void CalculateCoins_Server(out int coinsTeamA, out int coinsTeamB)
	{
		coinsTeamA = 0;
		coinsTeamB = 0;
	}

	private void CalculateCoins_Client(out int coinsTeamA, out int coinsTeamB)
	{
		coinsTeamA = 0;
		coinsTeamB = 0;
		if (m_clientData.m_actorsToCoins_unresolved == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			using (Dictionary<ActorData, int>.Enumerator enumerator = m_clientData.m_actorsToCoins_unresolved.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ActorData, int> current = enumerator.Current;
					if (current.Value != 0)
					{
						if (current.Key.GetTeam() == Team.TeamA)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							coinsTeamA += current.Value;
						}
						else if (current.Key.GetTeam() == Team.TeamB)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							coinsTeamB += current.Value;
						}
					}
				}
				while (true)
				{
					switch (3)
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

	public static bool AreCtcVictoryConditionsMetForTeam(CollectTheCoins_VictoryCondition[] conditions, Team checkTeam)
	{
		if (Get() == null)
		{
			return true;
		}
		if (conditions != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (conditions.Length != 0)
			{
				if (checkTeam != 0)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (checkTeam != Team.TeamB)
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
				int coinsTeamA;
				int coinsTeamB;
				if (NetworkServer.active)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					Get().CalculateCoins_Server(out coinsTeamA, out coinsTeamB);
				}
				else
				{
					Get().CalculateCoins_Client(out coinsTeamA, out coinsTeamB);
				}
				int num;
				int num2;
				if (checkTeam == Team.TeamA)
				{
					num = coinsTeamA;
					num2 = coinsTeamB;
				}
				else
				{
					num = coinsTeamB;
					num2 = coinsTeamA;
				}
				foreach (CollectTheCoins_VictoryCondition collectTheCoins_VictoryCondition in conditions)
				{
					if (collectTheCoins_VictoryCondition == CollectTheCoins_VictoryCondition.TeamMustHaveMostCoins)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (num <= num2)
						{
							return false;
						}
					}
					else
					{
						if (collectTheCoins_VictoryCondition != CollectTheCoins_VictoryCondition.TeamMustNotHaveMostCoins)
						{
							continue;
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (num <= num2)
						{
							continue;
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							return false;
						}
					}
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					return true;
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return true;
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}
}
