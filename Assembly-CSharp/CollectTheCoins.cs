using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CollectTheCoins : NetworkBehaviour
{
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
	public CollectTheCoins.CoinAbilityMod m_bouncingLaserDamage;

	public CollectTheCoins.CoinAbilityMod m_bouncingLaserTotalDistance;

	public CollectTheCoins.CoinAbilityMod m_bouncingLaserBounceDistance;

	public CollectTheCoins.CoinAbilityMod m_bouncingLaserBounces;

	public CollectTheCoins.CoinAbilityMod m_bouncingLaserReduceBackupPlanCooldown;

	public CollectTheCoins.CoinAbilityMod m_bouncingLaserPierces;

	private uint m_sequenceSourceId;

	private CollectTheCoins.ClientSideData m_clientData;

	private static CollectTheCoins s_instance;

	private SequenceSource _sequenceSource;

	private void Awake()
	{
		if (CollectTheCoins.s_instance == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.Awake()).MethodHandle;
			}
			CollectTheCoins.s_instance = this;
		}
		else
		{
			Log.Error("Multiple CollectTheCoins components in this scene, remove extraneous ones.", new object[0]);
		}
		if (NetworkServer.active)
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
			SequenceSource sequenceSource = new SequenceSource(null, null, false, null, null);
			this.m_sequenceSourceId = sequenceSource.RootID;
		}
		this.m_clientData = new CollectTheCoins.ClientSideData();
	}

	private void OnDestroy()
	{
		CollectTheCoins.s_instance = null;
	}

	public static CollectTheCoins Get()
	{
		return CollectTheCoins.s_instance;
	}

	internal SequenceSource SequenceSource
	{
		get
		{
			if (this._sequenceSource == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.get_SequenceSource()).MethodHandle;
				}
				this._sequenceSource = new SequenceSource(null, null, this.m_sequenceSourceId, false);
			}
			return this._sequenceSource;
		}
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint sequenceSourceId = reader.ReadUInt32();
		sbyte b = reader.ReadSByte();
		sbyte b2 = reader.ReadSByte();
		this.m_sequenceSourceId = sequenceSourceId;
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		Dictionary<BoardSquare, int> dictionary2 = new Dictionary<BoardSquare, int>();
		sbyte b3 = 0;
		while ((int)b3 < (int)b)
		{
			sbyte b4 = reader.ReadSByte();
			sbyte b5 = reader.ReadSByte();
			ActorData key = GameFlowData.Get().FindActorByActorIndex((int)b4);
			dictionary.Add(key, (int)b5);
			b3 = (sbyte)((int)b3 + 1);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.OnDeserialize(NetworkReader, bool)).MethodHandle;
		}
		sbyte b6 = 0;
		while ((int)b6 < (int)b2)
		{
			sbyte b7 = reader.ReadSByte();
			sbyte b8 = reader.ReadSByte();
			sbyte b9 = reader.ReadSByte();
			BoardSquare key2 = Board.\u000E().\u0016((int)b7, (int)b8);
			dictionary2.Add(key2, (int)b9);
			b6 = (sbyte)((int)b6 + 1);
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
		this.SynchCoinVisualsToDictionary(dictionary2);
		this.m_clientData.m_actorsToCoins_unresolved = dictionary;
		this.m_clientData.m_squaresToCoins_unresolved = dictionary2;
	}

	public int GetCoinsForActor_Client(ActorData actor)
	{
		if (this.m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor))
		{
			return this.m_clientData.m_actorsToCoins_unresolved[actor];
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.ExecuteClientGameModeEvent(ClientGameModeEvent)).MethodHandle;
			}
			BoardSquare square = gameModeEvent.m_square;
			if (this.m_clientData.m_squaresToCoins_unresolved.ContainsKey(square))
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
				int numCoins = this.m_clientData.m_squaresToCoins_unresolved[square];
				this.OnActorGainedCoins_Client(gameModeEvent.m_primaryActor, numCoins);
				this.RemoveCoinVisualsFromSquare(square);
				this.m_clientData.m_squaresToCoins_unresolved[square] = 0;
				this.m_clientData.m_squaresToCoins_unresolved.Remove(square);
			}
		}
		else if (eventType == GameModeEventType.Ctc_CoinsDropped)
		{
			BoardSquare square2 = gameModeEvent.m_square;
			this.OnActorDroppedCoins_Client(gameModeEvent.m_primaryActor, square2);
		}
		else if (eventType == GameModeEventType.Ctc_NonCoinPowerupTouched)
		{
			this.OnActorGainedCoins_Client(gameModeEvent.m_primaryActor, this.m_numCoinsToAwardPerNonCoinPowerup);
		}
		else
		{
			if (eventType != GameModeEventType.Ctc_CoinPowerupTouched)
			{
				Debug.LogError("CollectTheCoins trying to handle non-Ctc event type " + eventType.ToString() + ".");
				return;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.OnActorGainedCoins_Client(gameModeEvent.m_primaryActor, this.m_numCoinsToAwardPerCoinPowerup);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.OnActorGainedCoins_Client(ActorData, int)).MethodHandle;
			}
			Debug.LogError(string.Format("CollectTheCoins (client)-- trying to assign 0 spawn coins to actor {0}.", actor.\u0018()));
			return;
		}
		if (!this.m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor))
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
			this.m_clientData.m_actorsToCoins_unresolved.Add(actor, numCoins);
		}
		else
		{
			Dictionary<ActorData, int> actorsToCoins_unresolved;
			(actorsToCoins_unresolved = this.m_clientData.m_actorsToCoins_unresolved)[actor] = actorsToCoins_unresolved[actor] + numCoins;
		}
		if (this.m_gainObjPointPerCoinPickedUp)
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
			Team teamToAdjust = actor.\u000E();
			ObjectivePoints.Get().AdjustUnresolvedPoints(numCoins, teamToAdjust);
		}
	}

	public void OnActorDroppedCoins_Client(ActorData actor, BoardSquare square)
	{
		if (actor == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.OnActorDroppedCoins_Client(ActorData, BoardSquare)).MethodHandle;
			}
			Debug.LogError("CollectTheCoins (client)-- null actor trying to drop coins.");
			return;
		}
		if (square == null)
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
			Debug.LogError(string.Format("CollectTheCoins (client)-- actor {0} trying to drop coins on a null square.", actor.\u0018()));
			return;
		}
		if (!this.m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor))
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
			Debug.LogError(string.Format("CollectTheCoins (client)-- actor {0} trying to drop coins on square {1}, but that actor isn't in m_clientData.m_actorsToCoins_unresolved.", actor.\u0018(), BoardSquare.\u001D(square, false)));
			return;
		}
		int num = this.m_clientData.m_actorsToCoins_unresolved[actor];
		this.m_clientData.m_actorsToCoins_unresolved[actor] = 0;
		this.m_clientData.m_actorsToCoins_unresolved.Remove(actor);
		if (num == 0)
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
			Debug.LogError(string.Format("CollectTheCoins (client)-- actor {0} trying to drop coins on square {1}, but that actor has 0 coins.", actor.\u0018(), BoardSquare.\u001D(square, false)));
			return;
		}
		this.AddCoinSpillVisualToSquare(square, num);
		if (this.m_loseObjPointPerCoinDropped)
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
			Team teamToAdjust = actor.\u000E();
			ObjectivePoints.Get().AdjustUnresolvedPoints(-num, teamToAdjust);
		}
	}

	public void AddCoinVisualToSquare(BoardSquare square)
	{
		List<GameObject> list;
		if (!this.m_clientData.m_squaresToCoinVisuals.ContainsKey(square))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.AddCoinVisualToSquare(BoardSquare)).MethodHandle;
			}
			list = new List<GameObject>();
			this.m_clientData.m_squaresToCoinVisuals.Add(square, list);
		}
		else
		{
			list = this.m_clientData.m_squaresToCoinVisuals[square];
		}
		Vector3 position = square.ToVector3();
		GameObject item = UnityEngine.Object.Instantiate<GameObject>(this.m_coinOnGroundPrefab, position, Quaternion.identity);
		list.Add(item);
	}

	public void RemoveCoinVisualsFromSquare(BoardSquare square)
	{
		if (!this.m_clientData.m_squaresToCoinVisuals.ContainsKey(square))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.RemoveCoinVisualsFromSquare(BoardSquare)).MethodHandle;
			}
			return;
		}
		List<GameObject> list = this.m_clientData.m_squaresToCoinVisuals[square];
		for (int i = 0; i < list.Count; i++)
		{
			GameObject obj = list[i];
			UnityEngine.Object.Destroy(obj);
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		list.Clear();
		this.m_clientData.m_squaresToCoinVisuals.Remove(square);
	}

	public void SynchCoinVisualsToDictionary(Dictionary<BoardSquare, int> squaresToCoins)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		foreach (KeyValuePair<BoardSquare, List<GameObject>> keyValuePair in this.m_clientData.m_squaresToCoinVisuals)
		{
			BoardSquare key = keyValuePair.Key;
			int count = keyValuePair.Value.Count;
			int num;
			if (squaresToCoins.ContainsKey(key))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.SynchCoinVisualsToDictionary(Dictionary<BoardSquare, int>)).MethodHandle;
				}
				num = squaresToCoins[key];
			}
			else
			{
				num = 0;
			}
			if (count > num)
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
				list.Add(key);
			}
		}
		using (List<BoardSquare>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				BoardSquare square = enumerator2.Current;
				this.RemoveCoinVisualsFromSquare(square);
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
		using (Dictionary<BoardSquare, int>.Enumerator enumerator3 = squaresToCoins.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				KeyValuePair<BoardSquare, int> keyValuePair2 = enumerator3.Current;
				BoardSquare key2 = keyValuePair2.Key;
				int value = keyValuePair2.Value;
				int num2;
				if (this.m_clientData.m_squaresToCoinVisuals.ContainsKey(key2))
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
					num2 = this.m_clientData.m_squaresToCoinVisuals[key2].Count;
				}
				else
				{
					num2 = 0;
				}
				if (value > num2)
				{
					for (int i = num2; i < value; i++)
					{
						this.AddCoinVisualToSquare(key2);
					}
					for (;;)
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public void AddCoinSpillVisualToSquare(BoardSquare square, int numCoinsInSpill)
	{
		if (this.m_spillInAirPrefab == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.AddCoinSpillVisualToSquare(BoardSquare, int)).MethodHandle;
			}
			return;
		}
		List<GameObject> list;
		if (!this.m_clientData.m_squaresToSpillsVisuals.ContainsKey(square))
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
			list = new List<GameObject>();
			this.m_clientData.m_squaresToSpillsVisuals.Add(square, list);
		}
		else
		{
			list = this.m_clientData.m_squaresToSpillsVisuals[square];
		}
		Vector3 position = square.ToVector3() + Vector3.up * this.m_spillVerticalOffset;
		GameObject item = UnityEngine.Object.Instantiate<GameObject>(this.m_spillInAirPrefab, position, Quaternion.identity);
		list.Add(item);
	}

	public void ClearCoinSpillVisuals()
	{
		using (Dictionary<BoardSquare, List<GameObject>>.Enumerator enumerator = this.m_clientData.m_squaresToSpillsVisuals.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<BoardSquare, List<GameObject>> keyValuePair = enumerator.Current;
				List<GameObject> value = keyValuePair.Value;
				for (int i = 0; i < value.Count; i++)
				{
					GameObject obj = value[i];
					UnityEngine.Object.Destroy(obj);
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.ClearCoinSpillVisuals()).MethodHandle;
				}
				value.Clear();
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_clientData.m_squaresToSpillsVisuals.Clear();
	}

	public void Client_OnActorDeath(ActorData actor)
	{
		if (this.m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor) && this.m_clientData.m_actorsToCoins_unresolved[actor] > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.Client_OnActorDeath(ActorData)).MethodHandle;
			}
			BoardSquare square = actor.\u000E();
			this.OnActorDroppedCoins_Client(actor, square);
		}
	}

	public void OnTurnTick()
	{
		this.ClearCoinSpillVisuals();
	}

	private void CalculateCoins_Server(out int coinsTeamA, out int coinsTeamB)
	{
		coinsTeamA = 0;
		coinsTeamB = 0;
	}

	private unsafe void CalculateCoins_Client(out int coinsTeamA, out int coinsTeamB)
	{
		coinsTeamA = 0;
		coinsTeamB = 0;
		if (this.m_clientData.m_actorsToCoins_unresolved != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.CalculateCoins_Client(int*, int*)).MethodHandle;
			}
			using (Dictionary<ActorData, int>.Enumerator enumerator = this.m_clientData.m_actorsToCoins_unresolved.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
					if (keyValuePair.Value != 0)
					{
						if (keyValuePair.Key.\u000E() == Team.TeamA)
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
							coinsTeamA += keyValuePair.Value;
						}
						else if (keyValuePair.Key.\u000E() == Team.TeamB)
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
							coinsTeamB += keyValuePair.Value;
						}
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
		}
	}

	public static bool AreCtcVictoryConditionsMetForTeam(CollectTheCoins.CollectTheCoins_VictoryCondition[] conditions, Team checkTeam)
	{
		if (CollectTheCoins.Get() == null)
		{
			return true;
		}
		if (conditions != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.AreCtcVictoryConditionsMetForTeam(CollectTheCoins.CollectTheCoins_VictoryCondition[], Team)).MethodHandle;
			}
			if (conditions.Length != 0)
			{
				if (checkTeam != Team.TeamA)
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
					if (checkTeam != Team.TeamB)
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
				int num;
				int num2;
				if (NetworkServer.active)
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
					CollectTheCoins.Get().CalculateCoins_Server(out num, out num2);
				}
				else
				{
					CollectTheCoins.Get().CalculateCoins_Client(out num, out num2);
				}
				int num3;
				int num4;
				if (checkTeam == Team.TeamA)
				{
					num3 = num;
					num4 = num2;
				}
				else
				{
					num3 = num2;
					num4 = num;
				}
				foreach (CollectTheCoins.CollectTheCoins_VictoryCondition collectTheCoins_VictoryCondition in conditions)
				{
					if (collectTheCoins_VictoryCondition == CollectTheCoins.CollectTheCoins_VictoryCondition.TeamMustHaveMostCoins)
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
						if (num3 <= num4)
						{
							return false;
						}
					}
					else if (collectTheCoins_VictoryCondition == CollectTheCoins.CollectTheCoins_VictoryCondition.TeamMustNotHaveMostCoins)
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
						if (num3 > num4)
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
							return false;
						}
					}
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				return true;
			}
			for (;;)
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
		bool result;
		return result;
	}

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
			if (this.m_bonusForHavingMin == 0f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.CoinAbilityMod.BeingActiveMatters()).MethodHandle;
				}
				if (this.m_bonusPerCoinOverMin == 0f)
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
					return false;
				}
			}
			return true;
		}

		public float GetBonusForCoins(int numCoins)
		{
			bool flag;
			if ((float)numCoins < this.m_minCoinsForAnyBonus)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.CoinAbilityMod.GetBonusForCoins(int)).MethodHandle;
				}
				flag = (this.m_minCoinsForAnyBonus == -1f);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			bool flag3;
			if ((float)numCoins > this.m_maxCoinsForAnyBonus)
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
				flag3 = (this.m_maxCoinsForAnyBonus == -1f);
			}
			else
			{
				flag3 = true;
			}
			bool flag4 = flag3;
			if (flag2)
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
				if (flag4)
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
					float num = this.m_bonusForHavingMin + this.m_bonusPerCoinOverMin * ((float)numCoins - this.m_minCoinsForAnyBonus);
					if (this.m_maxBonus >= 0f)
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
						num = Mathf.Min(num, this.m_maxBonus);
					}
					return num;
				}
			}
			return 0f;
		}

		public float GetBonus_Client(ActorData actor)
		{
			if (!this.BeingActiveMatters())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CollectTheCoins.CoinAbilityMod.GetBonus_Client(ActorData)).MethodHandle;
				}
				return 0f;
			}
			int numCoins = 0;
			if (NetworkClient.active)
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
				numCoins = CollectTheCoins.Get().GetCoinsForActor_Client(actor);
			}
			return this.GetBonusForCoins(numCoins);
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
			this.m_actorsToCoins_unresolved = new Dictionary<ActorData, int>();
			this.m_squaresToCoins_unresolved = new Dictionary<BoardSquare, int>();
			this.m_squaresToSpillsVisuals = new Dictionary<BoardSquare, List<GameObject>>();
			this.m_squaresToCoinVisuals = new Dictionary<BoardSquare, List<GameObject>>();
		}
	}
}
