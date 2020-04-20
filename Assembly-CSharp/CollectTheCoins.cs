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
			CollectTheCoins.s_instance = this;
		}
		else
		{
			Log.Error("Multiple CollectTheCoins components in this scene, remove extraneous ones.", new object[0]);
		}
		if (NetworkServer.active)
		{
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
		sbyte b6 = 0;
		while ((int)b6 < (int)b2)
		{
			sbyte b7 = reader.ReadSByte();
			sbyte b8 = reader.ReadSByte();
			sbyte b9 = reader.ReadSByte();
			BoardSquare boardSquare = Board.Get().GetBoardSquare((int)b7, (int)b8);
			dictionary2.Add(boardSquare, (int)b9);
			b6 = (sbyte)((int)b6 + 1);
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
			BoardSquare square = gameModeEvent.m_square;
			if (this.m_clientData.m_squaresToCoins_unresolved.ContainsKey(square))
			{
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
			Debug.LogError(string.Format("CollectTheCoins (client)-- trying to assign 0 spawn coins to actor {0}.", actor.GetDebugName()));
			return;
		}
		if (!this.m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor))
		{
			this.m_clientData.m_actorsToCoins_unresolved.Add(actor, numCoins);
		}
		else
		{
			Dictionary<ActorData, int> actorsToCoins_unresolved;
			(actorsToCoins_unresolved = this.m_clientData.m_actorsToCoins_unresolved)[actor] = actorsToCoins_unresolved[actor] + numCoins;
		}
		if (this.m_gainObjPointPerCoinPickedUp)
		{
			Team team = actor.GetTeam();
			ObjectivePoints.Get().AdjustUnresolvedPoints(numCoins, team);
		}
	}

	public void OnActorDroppedCoins_Client(ActorData actor, BoardSquare square)
	{
		if (actor == null)
		{
			Debug.LogError("CollectTheCoins (client)-- null actor trying to drop coins.");
			return;
		}
		if (square == null)
		{
			Debug.LogError(string.Format("CollectTheCoins (client)-- actor {0} trying to drop coins on a null square.", actor.GetDebugName()));
			return;
		}
		if (!this.m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor))
		{
			Debug.LogError(string.Format("CollectTheCoins (client)-- actor {0} trying to drop coins on square {1}, but that actor isn't in m_clientData.m_actorsToCoins_unresolved.", actor.GetDebugName(), BoardSquare.symbol_001D(square, false)));
			return;
		}
		int num = this.m_clientData.m_actorsToCoins_unresolved[actor];
		this.m_clientData.m_actorsToCoins_unresolved[actor] = 0;
		this.m_clientData.m_actorsToCoins_unresolved.Remove(actor);
		if (num == 0)
		{
			Debug.LogError(string.Format("CollectTheCoins (client)-- actor {0} trying to drop coins on square {1}, but that actor has 0 coins.", actor.GetDebugName(), BoardSquare.symbol_001D(square, false)));
			return;
		}
		this.AddCoinSpillVisualToSquare(square, num);
		if (this.m_loseObjPointPerCoinDropped)
		{
			Team team = actor.GetTeam();
			ObjectivePoints.Get().AdjustUnresolvedPoints(-num, team);
		}
	}

	public void AddCoinVisualToSquare(BoardSquare square)
	{
		List<GameObject> list;
		if (!this.m_clientData.m_squaresToCoinVisuals.ContainsKey(square))
		{
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
			return;
		}
		List<GameObject> list = this.m_clientData.m_squaresToCoinVisuals[square];
		for (int i = 0; i < list.Count; i++)
		{
			GameObject obj = list[i];
			UnityEngine.Object.Destroy(obj);
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
				num = squaresToCoins[key];
			}
			else
			{
				num = 0;
			}
			if (count > num)
			{
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
				}
			}
		}
	}

	public void AddCoinSpillVisualToSquare(BoardSquare square, int numCoinsInSpill)
	{
		if (this.m_spillInAirPrefab == null)
		{
			return;
		}
		List<GameObject> list;
		if (!this.m_clientData.m_squaresToSpillsVisuals.ContainsKey(square))
		{
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
				value.Clear();
			}
		}
		this.m_clientData.m_squaresToSpillsVisuals.Clear();
	}

	public void Client_OnActorDeath(ActorData actor)
	{
		if (this.m_clientData.m_actorsToCoins_unresolved.ContainsKey(actor) && this.m_clientData.m_actorsToCoins_unresolved[actor] > 0)
		{
			BoardSquare travelBoardSquare = actor.GetTravelBoardSquare();
			this.OnActorDroppedCoins_Client(actor, travelBoardSquare);
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
			using (Dictionary<ActorData, int>.Enumerator enumerator = this.m_clientData.m_actorsToCoins_unresolved.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
					if (keyValuePair.Value != 0)
					{
						if (keyValuePair.Key.GetTeam() == Team.TeamA)
						{
							coinsTeamA += keyValuePair.Value;
						}
						else if (keyValuePair.Key.GetTeam() == Team.TeamB)
						{
							coinsTeamB += keyValuePair.Value;
						}
					}
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
			if (conditions.Length != 0)
			{
				if (checkTeam != Team.TeamA)
				{
					if (checkTeam != Team.TeamB)
					{
						return true;
					}
				}
				int num;
				int num2;
				if (NetworkServer.active)
				{
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
						if (num3 <= num4)
						{
							return false;
						}
					}
					else if (collectTheCoins_VictoryCondition == CollectTheCoins.CollectTheCoins_VictoryCondition.TeamMustNotHaveMostCoins)
					{
						if (num3 > num4)
						{
							return false;
						}
					}
				}
				return true;
			}
		}
		return true;
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
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
				if (this.m_bonusPerCoinOverMin == 0f)
				{
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
				flag3 = (this.m_maxCoinsForAnyBonus == -1f);
			}
			else
			{
				flag3 = true;
			}
			bool flag4 = flag3;
			if (flag2)
			{
				if (flag4)
				{
					float num = this.m_bonusForHavingMin + this.m_bonusPerCoinOverMin * ((float)numCoins - this.m_minCoinsForAnyBonus);
					if (this.m_maxBonus >= 0f)
					{
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
				return 0f;
			}
			int numCoins = 0;
			if (NetworkClient.active)
			{
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
