using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
	[Header("-- RESPAWN MANUAL SELECTION LOCATION --")]
	public bool m_playersSelectRespawn = true;

	public bool m_spawnInDuringMovement = true;

	public int m_respawnDelay = 2;

	[Tooltip("Used only when Players Select Respawn = true and Respawn Method = RespawnAnywhere")]
	public float m_respawnInnerRadius;

	[Tooltip("Used only when Players Select Respawn = true and Respawn Method = RespawnAnywhere")]
	public float m_respawnOuterRadius = 3f;

	[Header("-- RESPAWN AUTOMATIC LOCATION --")]
	public SpawnPointManager.RespawnMethod m_respawnMethod;

	private SpawnPointManager.SpawnPointCoord[] m_spawnPointsTeamA;

	private SpawnPointManager.SpawnPointCoord[] m_spawnPointsTeamB;

	[Tooltip("Used if Respawn Method = RespawnInGraveyards")]
	public BoardRegion m_spawnRegionsTeamA;

	[Tooltip("Used if Respawn Method = RespawnInGraveyards")]
	public BoardRegion m_spawnRegionsTeamB;

	public BoardRegion m_initialSpawnPointsTeamA;

	public BoardRegion m_initialSpawnPointsTeamB;

	public SpawnLookAtPoint m_initialSpawnLookAtPoint;

	public float m_startMinDistToFriend = 1f;

	public float m_startMinDistToEnemy = 16f;

	public float m_minDistToFriend = 4f;

	public float m_minDistToEnemy = 8f;

	public float m_generatePerimeterSize = 0.15f;

	public bool m_onlyAvoidVisibleEnemies = true;

	public bool m_brushHidesRespawnFlares = true;

	public bool m_respawnActorsCanCollectPowerUps = true;

	public bool m_respawnActorsCanBeHitDuringMovement = true;

	[Tooltip("Algorithm will keep relaxing requirements until more than this many square are valid to pick between")]
	public int m_minValidRespawnSquaresForPlayerSelection = 2;

	internal float m_chooseWeightFriendProximity = 1f;

	internal float m_chooseWeightEnemyProximity = -2f;

	internal float m_chooseWeightDeathProximity = -1f;

	internal float m_chooseDontCareDistance = 100f;

	private static SpawnPointManager s_instance;

	public static SpawnPointManager Get()
	{
		return SpawnPointManager.s_instance;
	}

	private void Awake()
	{
		SpawnPointManager.s_instance = this;
	}

	private void OnDestroy()
	{
		SpawnPointManager.s_instance = null;
	}

	private void Start()
	{
		this.m_initialSpawnPointsTeamA.Initialize();
		this.m_initialSpawnPointsTeamB.Initialize();
		this.m_spawnRegionsTeamA.Initialize();
		this.m_spawnRegionsTeamB.Initialize();
		if (!this.m_playersSelectRespawn && this.m_respawnMethod == SpawnPointManager.RespawnMethod.RespawnAnywhere)
		{
			this.ClearSpawnPoints();
			Board board = Board.Get();
			PowerUpManager powerUpManager = PowerUpManager.Get();
			int num = (int)((float)board.GetMaxX() * this.m_generatePerimeterSize);
			int num2 = board.GetMaxX() - num;
			int num3 = (int)((float)board.GetMaxY() * this.m_generatePerimeterSize);
			int num4 = board.GetMaxY() - num3;
			for (int i = 0; i < board.GetMaxX(); i++)
			{
				int j = 0;
				while (j < board.GetMaxY())
				{
					if (i < num)
					{
						goto IL_E3;
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.Start()).MethodHandle;
					}
					if (i > num2 || j < num3)
					{
						goto IL_E3;
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
					if (j > num4)
					{
						goto IL_E3;
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
					IL_17B:
					j++;
					continue;
					IL_E3:
					BoardSquare boardSquare = board.GetBoardSquare(i, j);
					if (!(boardSquare != null))
					{
						goto IL_17B;
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
					if (!boardSquare.IsBaselineHeight() || boardSquare.IsInBrushRegion() || (float)(boardSquare.height - board.BaselineHeight) >= 0.5f)
					{
						goto IL_17B;
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
					if (!powerUpManager.IsPowerUpSpawnPoint(boardSquare))
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
						Team team;
						if ((i + j) % 2 == 0)
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
							team = Team.TeamA;
						}
						else
						{
							team = Team.TeamB;
						}
						this.AddSpawnPoint(team, i, j);
						goto IL_17B;
					}
					goto IL_17B;
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

	public Vector3 GetSpawnFacing(Vector3 spawnPosition)
	{
		Vector3 result = new Vector3(1f, 0f, 0f);
		if (this.m_initialSpawnLookAtPoint != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.GetSpawnFacing(Vector3)).MethodHandle;
			}
			Vector3 vector = this.m_initialSpawnLookAtPoint.transform.position - spawnPosition;
			vector.y = 0f;
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
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
				if (vector.x > 0f)
				{
					result = new Vector3(1f, 0f, 0f);
				}
				else
				{
					result = new Vector3(-1f, 0f, 0f);
				}
			}
			else if (vector.z > 0f)
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
				result = new Vector3(0f, 0f, 1f);
			}
			else
			{
				result = new Vector3(0f, 0f, -1f);
			}
		}
		return result;
	}

	public void ClearSpawnPoints()
	{
		Array.Resize<SpawnPointManager.SpawnPointCoord>(ref this.m_spawnPointsTeamA, 0);
		Array.Resize<SpawnPointManager.SpawnPointCoord>(ref this.m_spawnPointsTeamB, 0);
	}

	public void AddSpawnPoint(Team team, int x, int y)
	{
		if (team == Team.TeamA)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.AddSpawnPoint(Team, int, int)).MethodHandle;
			}
			if (this.m_spawnPointsTeamA == null)
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
				this.m_spawnPointsTeamA = new SpawnPointManager.SpawnPointCoord[1];
			}
			else
			{
				Array.Resize<SpawnPointManager.SpawnPointCoord>(ref this.m_spawnPointsTeamA, this.m_spawnPointsTeamA.Length + 1);
			}
			this.m_spawnPointsTeamA[this.m_spawnPointsTeamA.Length - 1] = new SpawnPointManager.SpawnPointCoord(x, y);
		}
		else if (team == Team.TeamB)
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
			if (this.m_spawnPointsTeamB == null)
			{
				this.m_spawnPointsTeamB = new SpawnPointManager.SpawnPointCoord[1];
			}
			else
			{
				Array.Resize<SpawnPointManager.SpawnPointCoord>(ref this.m_spawnPointsTeamB, this.m_spawnPointsTeamB.Length + 1);
			}
			this.m_spawnPointsTeamB[this.m_spawnPointsTeamB.Length - 1] = new SpawnPointManager.SpawnPointCoord(x, y);
		}
	}

	internal List<BoardSquare> GetSpawnSquaresList(Team team, SpawnPointManager.RespawnMethod respawnMethod)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (respawnMethod == SpawnPointManager.RespawnMethod.RespawnOnlyAtInitialSpawnPoints)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.GetSpawnSquaresList(Team, SpawnPointManager.RespawnMethod)).MethodHandle;
			}
			if (team == Team.TeamA)
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
				list = this.m_initialSpawnPointsTeamA.GetSquaresInRegion();
			}
			else
			{
				list = this.m_initialSpawnPointsTeamB.GetSquaresInRegion();
			}
		}
		else if (respawnMethod == SpawnPointManager.RespawnMethod.RespawnInGraveyards)
		{
			if (team == Team.TeamA)
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
				list = this.m_spawnRegionsTeamA.GetSquaresInRegion();
			}
			else
			{
				list = this.m_spawnRegionsTeamB.GetSquaresInRegion();
			}
			List<BoardSquare> list2 = list;
			if (SpawnPointManager.<>f__am$cache0 == null)
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
				SpawnPointManager.<>f__am$cache0 = ((BoardSquare s) => !s.IsBaselineHeight());
			}
			list2.RemoveAll(SpawnPointManager.<>f__am$cache0);
		}
		else if (team == Team.TeamA)
		{
			if (this.m_spawnPointsTeamA != null)
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
				for (int i = 0; i < this.m_spawnPointsTeamA.Length; i++)
				{
					if (this.m_spawnPointsTeamA[i] != null)
					{
						BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_spawnPointsTeamA[i].x, this.m_spawnPointsTeamA[i].y);
						list.Add(boardSquare);
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
		else if (this.m_spawnPointsTeamB != null)
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
			for (int j = 0; j < this.m_spawnPointsTeamB.Length; j++)
			{
				if (this.m_spawnPointsTeamB[j] != null)
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
					BoardSquare boardSquare2 = Board.Get().GetBoardSquare(this.m_spawnPointsTeamB[j].x, this.m_spawnPointsTeamB[j].y);
					if (boardSquare2)
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
						list.Add(boardSquare2);
					}
				}
			}
		}
		return list;
	}

	public bool CanSpawnOnSquare(ActorData spawner, BoardSquare square, bool allowOccupiedSquares = false)
	{
		bool result = false;
		if (square != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.CanSpawnOnSquare(ActorData, BoardSquare, bool)).MethodHandle;
			}
			if (MovementUtils.CanStopOnSquare(square))
			{
				if (!allowOccupiedSquares)
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
					if (!(square.occupant == null))
					{
						return result;
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
				result = true;
			}
		}
		return result;
	}

	private int SortByProximityWeights(List<BoardSquare> spawnSquareList, ActorData actorSpawning, bool avoidLastDeathPosition, List<ActorData> actorsSpawned, HashSet<BoardSquare> squaresToAvoid, HashSet<BoardSquare> squaresNotAllowed, bool onlyAvoidVisibleEnemies, bool allowOccupiedSquares, int minimumFavoredSquares)
	{
		BoardSquare boardSquare;
		if (actorSpawning != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.SortByProximityWeights(List<BoardSquare>, ActorData, bool, List<ActorData>, HashSet<BoardSquare>, HashSet<BoardSquare>, bool, bool, int)).MethodHandle;
			}
			if (this.m_respawnMethod == SpawnPointManager.RespawnMethod.RespawnOnlyAtInitialSpawnPoints)
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
				boardSquare = actorSpawning.InitialSpawnSquare;
				goto IL_38;
			}
		}
		boardSquare = null;
		IL_38:
		BoardSquare preferedSpawnLocation = boardSquare;
		SpawnPointManager.SpawnSquareComparer spawnSquareComparer = new SpawnPointManager.SpawnSquareComparer(spawnSquareList, actorSpawning, avoidLastDeathPosition, actorsSpawned, preferedSpawnLocation, squaresToAvoid, squaresNotAllowed, onlyAvoidVisibleEnemies, allowOccupiedSquares);
		spawnSquareList.Sort(spawnSquareComparer);
		int num = spawnSquareComparer.NumFriendTooCloseSquares + spawnSquareComparer.NumFavoredSquares;
		int num2 = spawnSquareComparer.NumEnemyTooCloseSquares + num;
		int result;
		if (spawnSquareComparer.NumFavoredSquares <= minimumFavoredSquares)
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
			result = ((num > minimumFavoredSquares) ? num : num2);
		}
		else
		{
			result = spawnSquareComparer.NumFavoredSquares;
		}
		return result;
	}

	internal BoardSquare GetSpawnSquare(ActorData spawner, bool avoidLastDeathPosition = true, List<ActorData> spawnedActors = null, HashSet<BoardSquare> squaresToAvoid = null)
	{
		BoardSquare boardSquare = null;
		List<BoardSquare> spawnSquaresList = this.GetSpawnSquaresList(spawner.GetTeam(), this.m_respawnMethod);
		int num = this.SortByProximityWeights(spawnSquaresList, spawner, avoidLastDeathPosition, spawnedActors, null, squaresToAvoid, false, false, 0);
		if (num > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.GetSpawnSquare(ActorData, bool, List<ActorData>, HashSet<BoardSquare>)).MethodHandle;
			}
			bool flag;
			if (spawnedActors != null)
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
				flag = (spawnedActors.Count == 0);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			int index = (!flag2) ? 0 : GameplayRandom.Range(0, spawnSquaresList.Count);
			boardSquare = spawnSquaresList[index];
			bool flag3 = this.CanSpawnOnSquare(spawner, boardSquare, false);
			if (!flag3)
			{
				Log.Error("Debugging, spawn square already occupied", new object[0]);
				for (int i = 0; i < spawnSquaresList.Count; i++)
				{
					if (this.CanSpawnOnSquare(spawner, spawnSquaresList[i], false))
					{
						boardSquare = spawnSquaresList[i];
						flag3 = true;
						break;
					}
				}
			}
			if (!flag3)
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
				Log.Error("Debugging, failed to find respawn square", new object[0]);
			}
		}
		else
		{
			SpawnPointManager.SpawnPointCoord[] array;
			if (spawner.GetTeam() == Team.TeamA)
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
				array = this.m_spawnPointsTeamA;
			}
			else
			{
				array = this.m_spawnPointsTeamB;
			}
			foreach (SpawnPointManager.SpawnPointCoord spawnPointCoord in array)
			{
				List<BoardSquare> list = new List<BoardSquare>(8);
				Board.Get().GetAllAdjacentSquares(spawnPointCoord.x, spawnPointCoord.y, ref list);
				using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BoardSquare boardSquare2 = enumerator.Current;
						if (this.CanSpawnOnSquare(spawner, boardSquare2, false))
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
							spawnSquaresList.Add(boardSquare2);
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
			}
			num = this.SortByProximityWeights(spawnSquaresList, spawner, avoidLastDeathPosition, spawnedActors, squaresToAvoid, null, false, false, 0);
			if (num > 0)
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
				boardSquare = spawnSquaresList[0];
			}
		}
		if (boardSquare == null)
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
			Board board = Board.Get();
			for (int k = 0; k < board.GetMaxX(); k++)
			{
				for (int l = 0; l < board.GetMaxY(); l++)
				{
					BoardSquare boardSquare3 = board.GetBoardSquare(k, l);
					if (this.CanSpawnOnSquare(spawner, boardSquare3, false))
					{
						spawnSquaresList.Add(boardSquare3);
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			num = this.SortByProximityWeights(spawnSquaresList, spawner, avoidLastDeathPosition, spawnedActors, squaresToAvoid, null, false, false, 0);
			if (num > 0)
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
				boardSquare = spawnSquaresList[0];
			}
		}
		if (boardSquare == null)
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
			Log.Error("Failed to find a spawn square for " + spawner.DisplayName, new object[0]);
		}
		else if (boardSquare.occupant != null)
		{
			Log.Error("Debugging, spawn square found is occupied", new object[0]);
		}
		return boardSquare;
	}

	internal BoardSquare GetInitialSpawnSquare(ActorData spawner, List<ActorData> spawnedActors)
	{
		BoardSquare boardSquare = null;
		List<BoardSquare> squaresInRegion;
		if (spawner.GetTeam() == Team.TeamA)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.GetInitialSpawnSquare(ActorData, List<ActorData>)).MethodHandle;
			}
			squaresInRegion = this.m_initialSpawnPointsTeamA.GetSquaresInRegion();
		}
		else
		{
			squaresInRegion = this.m_initialSpawnPointsTeamB.GetSquaresInRegion();
		}
		if (squaresInRegion != null)
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
			if (squaresInRegion.Count != 0)
			{
				using (List<BoardSquare>.Enumerator enumerator = squaresInRegion.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BoardSquare boardSquare2 = enumerator.Current;
						if (this.CanSpawnOnSquare(spawner, boardSquare2, false))
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
							boardSquare = boardSquare2;
							goto IL_BC;
						}
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
				}
				IL_BC:
				if (boardSquare == null)
				{
					Log.Error("Couldn't find an initial spawn square for actor on team " + spawner.GetTeam().ToString() + ", make sure Initial Spawn Points are set up.", new object[0]);
					foreach (BoardSquare boardSquare3 in squaresInRegion)
					{
						List<BoardSquare> list = new List<BoardSquare>(8);
						Board.Get().GetAllAdjacentSquares(boardSquare3.x, boardSquare3.y, ref list);
						foreach (BoardSquare boardSquare4 in list)
						{
							if (this.CanSpawnOnSquare(spawner, boardSquare4, false))
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
								boardSquare = boardSquare4;
								break;
							}
						}
						if (boardSquare != null)
						{
							break;
						}
					}
				}
				if (boardSquare == null)
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
					Log.Error("Couldn't even find a viable spawn square adjacent to any initial spawn squares for actor on team " + spawner.GetTeam().ToString() + ", make sure Initial Spawn Points are set up.", new object[0]);
					int i = 0;
					while (i < 0x80)
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
						if (!(boardSquare == null))
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								return boardSquare;
							}
						}
						else
						{
							Board board = Board.Get();
							int x = GameplayRandom.Range(0, board.GetMaxX());
							int y = GameplayRandom.Range(0, board.GetMaxY());
							BoardSquare boardSquare5 = board.GetBoardSquare(x, y);
							if (this.CanSpawnOnSquare(spawner, boardSquare5, false))
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
								boardSquare = boardSquare5;
							}
							i++;
						}
					}
				}
				return boardSquare;
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
		}
		return this.GetSpawnSquare(spawner, false, spawnedActors, null);
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.color = ActorData.s_teamAColor;
		if (this.m_spawnPointsTeamA != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.OnDrawGizmos()).MethodHandle;
			}
			foreach (SpawnPointManager.SpawnPointCoord spawnPointCoord in this.m_spawnPointsTeamA)
			{
				BoardSquare boardSquare = (!(Board.Get() == null)) ? Board.Get().GetBoardSquare(spawnPointCoord.x, spawnPointCoord.y) : null;
				if (boardSquare != null)
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
					Gizmos.DrawWireSphere(boardSquare.ToVector3(), 0.7f);
				}
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
		Gizmos.color = ActorData.s_teamBColor;
		if (this.m_spawnPointsTeamB != null)
		{
			foreach (SpawnPointManager.SpawnPointCoord spawnPointCoord2 in this.m_spawnPointsTeamB)
			{
				BoardSquare boardSquare2;
				if (Board.Get() == null)
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
					boardSquare2 = null;
				}
				else
				{
					boardSquare2 = Board.Get().GetBoardSquare(spawnPointCoord2.x, spawnPointCoord2.y);
				}
				BoardSquare boardSquare3 = boardSquare2;
				if (boardSquare3 != null)
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
					Gizmos.DrawWireSphere(boardSquare3.ToVector3(), 0.7f);
				}
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
		if (!(GameFlowData.Get() == null))
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
			if (!(GameFlowData.Get().activeOwnedActorData == null))
			{
				List<BoardSquare> spawnSquaresList = this.GetSpawnSquaresList(GameFlowData.Get().activeOwnedActorData.GetTeam(), this.m_respawnMethod);
				int num = this.SortByProximityWeights(spawnSquaresList, GameFlowData.Get().activeOwnedActorData, true, null, null, null, false, false, 0);
				Gizmos.color = ActorData.s_teamBColor;
				for (int k = 0; k < spawnSquaresList.Count; k++)
				{
					BoardSquare boardSquare4 = spawnSquaresList[k];
					if (k == 0)
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
						Gizmos.color = Color.green;
					}
					else if (k >= num)
					{
						Gizmos.color = Color.black;
					}
					else
					{
						float num2 = ((float)spawnSquaresList.Count - (float)k) / (float)spawnSquaresList.Count;
						Gizmos.color = new Color(num2, num2, num2);
					}
					Gizmos.DrawSphere(boardSquare4.ToVector3(), 0.3f);
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
		}
	}

	[Serializable]
	public class SpawnPointCoord
	{
		public int x;

		public int y;

		public SpawnPointCoord(int inputX, int inputY)
		{
			this.x = inputX;
			this.y = inputY;
		}
	}

	public enum RespawnMethod
	{
		RespawnAnywhere,
		RespawnOnlyAtInitialSpawnPoints,
		RespawnInGraveyards
	}

	private class SpawnSquareComparer : IComparer<BoardSquare>
	{
		private Dictionary<BoardSquare, SpawnPointManager.SpawnSquareComparer.ScoreInfo> m_scores;

		private ActorData m_actorSpawning;

		private BoardSquare m_preferedSpawnLocation;

		internal SpawnSquareComparer(List<BoardSquare> spawnSquareList, ActorData actorSpawning, bool avoidLastDeathPosition, List<ActorData> spawnedActors, BoardSquare preferedSpawnLocation, HashSet<BoardSquare> squaresToAvoid, HashSet<BoardSquare> squaresNotAllowed, bool onlyAvoidVisibleEnemies, bool allowOccupiedSquares)
		{
			this.m_scores = new Dictionary<BoardSquare, SpawnPointManager.SpawnSquareComparer.ScoreInfo>(spawnSquareList.Count);
			this.m_actorSpawning = actorSpawning;
			this.m_preferedSpawnLocation = preferedSpawnLocation;
			SpawnPointManager spawnPointManager = SpawnPointManager.Get();
			float chooseWeightEnemyProximity = spawnPointManager.m_chooseWeightEnemyProximity;
			float chooseWeightFriendProximity = spawnPointManager.m_chooseWeightFriendProximity;
			float num = spawnPointManager.m_chooseDontCareDistance * Board.Get().squareSize;
			float num2 = (spawnedActors == null) ? (spawnPointManager.m_minDistToFriend * Board.Get().squareSize) : (spawnPointManager.m_startMinDistToFriend * Board.Get().squareSize);
			num2 *= num2;
			float num3;
			if (spawnedActors != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.SpawnSquareComparer..ctor(List<BoardSquare>, ActorData, bool, List<ActorData>, BoardSquare, HashSet<BoardSquare>, HashSet<BoardSquare>, bool, bool)).MethodHandle;
				}
				num3 = spawnPointManager.m_startMinDistToEnemy * Board.Get().squareSize;
			}
			else
			{
				num3 = spawnPointManager.m_minDistToEnemy * Board.Get().squareSize;
			}
			float num4 = num3;
			num4 *= num4;
			List<ActorData> list;
			if (spawnedActors == null)
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
				list = GameFlowData.Get().GetActors();
			}
			else
			{
				list = spawnedActors;
			}
			List<ActorData> list2 = list;
			int i = 0;
			while (i < spawnSquareList.Count)
			{
				BoardSquare boardSquare = spawnSquareList[i];
				Vector3 b = boardSquare.ToVector3();
				this.m_scores[boardSquare] = new SpawnPointManager.SpawnSquareComparer.ScoreInfo();
				SpawnPointManager.SpawnSquareComparer.ScoreInfo scoreInfo = this.m_scores[boardSquare];
				bool avoidIfPossible;
				if (squaresToAvoid != null)
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
					avoidIfPossible = squaresToAvoid.Contains(boardSquare);
				}
				else
				{
					avoidIfPossible = false;
				}
				scoreInfo.m_avoidIfPossible = avoidIfPossible;
				if (!spawnPointManager.CanSpawnOnSquare(this.m_actorSpawning, boardSquare, allowOccupiedSquares))
				{
					goto IL_238;
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
				if (squaresNotAllowed == null || squaresNotAllowed.Contains(boardSquare))
				{
					goto IL_238;
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
				if (avoidLastDeathPosition)
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
					float sqrMagnitude = (actorSpawning.LastDeathPosition - b).sqrMagnitude;
					float num5 = Mathf.Max(0f, num - Mathf.Sqrt(sqrMagnitude));
					float num6 = num5 * spawnPointManager.m_chooseWeightDeathProximity;
					if (num5 < 0f && num6 > 0f)
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
						num6 = 0f;
					}
					this.m_scores[boardSquare].m_score = num6;
				}
				else
				{
					this.m_scores[boardSquare].m_score = 0f;
				}
				IL_24D:
				for (int j = 0; j < list2.Count; j++)
				{
					ActorData actorData = list2[j];
					if (actorData != null)
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
						if (!actorData.IsDead())
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
							if (actorData.PlayerIndex != PlayerData.s_invalidPlayerIndex)
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
								if (actorData.GetCurrentBoardSquare() != null)
								{
									BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
									float sqrMagnitude2 = (currentBoardSquare.ToVector3() - b).sqrMagnitude;
									bool flag = actorData.GetTeam() == this.m_actorSpawning.GetTeam();
									if (flag)
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
										if (sqrMagnitude2 < num2)
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
											this.m_scores[boardSquare].m_tooCloseToFriendly = true;
										}
									}
									if (!flag)
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
										if (sqrMagnitude2 < num4)
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
											if (onlyAvoidVisibleEnemies)
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
												if (!actorData.IsVisibleToOpposingTeam())
												{
													goto IL_379;
												}
											}
											this.m_scores[boardSquare].m_tooCloseToEnemy = true;
										}
										IL_379:;
									}
									else
									{
										float num7 = Mathf.Max(0f, num - Mathf.Sqrt(sqrMagnitude2));
										float num8 = num7 * ((actorData.GetTeam() != this.m_actorSpawning.GetTeam()) ? chooseWeightEnemyProximity : chooseWeightFriendProximity);
										if (num7 < 0f && num8 > 0f)
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
											num8 = 0f;
										}
										this.m_scores[boardSquare].m_score += num8;
									}
								}
							}
						}
					}
				}
				if (!this.m_scores[boardSquare].m_cantSpawn)
				{
					if (this.m_scores[boardSquare].m_tooCloseToEnemy)
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
						this.NumEnemyTooCloseSquares++;
					}
					else if (this.m_scores[boardSquare].m_tooCloseToFriendly)
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
						this.NumFriendTooCloseSquares++;
					}
					else if (!this.m_scores[boardSquare].m_avoidIfPossible)
					{
						this.NumFavoredSquares++;
					}
				}
				i++;
				continue;
				IL_238:
				this.m_scores[boardSquare].m_cantSpawn = true;
				goto IL_24D;
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

		internal int NumFavoredSquares { get; private set; }

		internal int NumFriendTooCloseSquares { get; private set; }

		internal int NumEnemyTooCloseSquares { get; private set; }

		public int Compare(BoardSquare x, BoardSquare y)
		{
			if (x == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.SpawnSquareComparer.Compare(BoardSquare, BoardSquare)).MethodHandle;
				}
				if (y == null)
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
					return 0;
				}
				return 1;
			}
			else
			{
				if (y == null)
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
					return -1;
				}
				if (this.m_scores[y].m_cantSpawn != this.m_scores[x].m_cantSpawn)
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
					int result;
					if (this.m_scores[x].m_cantSpawn)
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
						result = 1;
					}
					else
					{
						result = -1;
					}
					return result;
				}
				if (this.m_preferedSpawnLocation != null)
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
					if (!(this.m_preferedSpawnLocation == x))
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
						if (!(this.m_preferedSpawnLocation == y))
						{
							goto IL_109;
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
					}
					int result2;
					if (this.m_preferedSpawnLocation == x)
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
						result2 = -1;
					}
					else
					{
						result2 = 1;
					}
					return result2;
				}
				IL_109:
				if (this.m_scores[y].m_avoidIfPossible != this.m_scores[x].m_avoidIfPossible)
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
					int result3;
					if (this.m_scores[x].m_avoidIfPossible)
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
						result3 = 1;
					}
					else
					{
						result3 = -1;
					}
					return result3;
				}
				if (this.m_scores[y].m_tooCloseToEnemy != this.m_scores[x].m_tooCloseToEnemy)
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
					int result4;
					if (this.m_scores[x].m_tooCloseToEnemy)
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
						result4 = 1;
					}
					else
					{
						result4 = -1;
					}
					return result4;
				}
				if (this.m_scores[y].m_tooCloseToFriendly != this.m_scores[x].m_tooCloseToFriendly)
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
					int result5;
					if (this.m_scores[x].m_tooCloseToFriendly)
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
						result5 = 1;
					}
					else
					{
						result5 = -1;
					}
					return result5;
				}
				return this.m_scores[y].m_score.CompareTo(this.m_scores[x].m_score);
			}
		}

		public void \u001D(List<BoardSquare> \u001D, float \u000E = 20f)
		{
			if (\u001D != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnPointManager.SpawnSquareComparer.\u001D(List<BoardSquare>, float)).MethodHandle;
				}
				if (this.m_scores != null)
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
					bool flag = false;
					for (int i = 0; i < \u001D.Count; i++)
					{
						BoardSquare boardSquare = \u001D[i];
						if (this.m_scores.ContainsKey(boardSquare))
						{
							if (this.m_scores[boardSquare].m_cantSpawn)
							{
								Debug.DrawRay(boardSquare.ToVector3(), 1.5f * Vector3.up, Color.red, \u000E);
							}
							else if (this.m_scores[boardSquare].m_avoidIfPossible)
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
								flag = true;
								Debug.DrawRay(boardSquare.ToVector3(), new Vector3(0.5f, 1.5f, 0f), 0.5f * (Color.yellow + Color.red), \u000E);
							}
							else
							{
								if (!this.m_scores[boardSquare].m_tooCloseToEnemy)
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
									if (this.m_scores[boardSquare].m_tooCloseToFriendly)
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
									}
									else
									{
										Debug.DrawRay(boardSquare.ToVector3(), 1.5f * Vector3.up, Color.white, \u000E);
										if (flag)
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
											Debug.LogWarning("Respawn: square to avoid not sorted toward end of list");
											goto IL_1AA;
										}
										goto IL_1AA;
									}
								}
								Debug.DrawRay(boardSquare.ToVector3(), new Vector3(0f, 1.5f, 0.5f), Color.magenta, \u000E);
								if (flag)
								{
									Debug.LogWarning("Respawn: square to avoid not sorted toward end of list");
								}
							}
						}
						IL_1AA:;
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
				}
			}
		}

		private class ScoreInfo
		{
			internal float m_score;

			internal bool m_cantSpawn;

			internal bool m_tooCloseToEnemy;

			internal bool m_tooCloseToFriendly;

			internal bool m_avoidIfPossible;
		}
	}
}
