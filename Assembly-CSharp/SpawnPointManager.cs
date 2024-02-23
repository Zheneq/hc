using System;
using System.Collections.Generic;
using System.Text;
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
	public RespawnMethod m_respawnMethod;
	private SpawnPointCoord[] m_spawnPointsTeamA;
	private SpawnPointCoord[] m_spawnPointsTeamB;
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
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Start()
	{
		m_initialSpawnPointsTeamA.Initialize();
		m_initialSpawnPointsTeamB.Initialize();
		m_spawnRegionsTeamA.Initialize();
		m_spawnRegionsTeamB.Initialize();
		if (!m_playersSelectRespawn && m_respawnMethod == RespawnMethod.RespawnAnywhere)
		{
			ClearSpawnPoints();
			Board board = Board.Get();
			PowerUpManager powerUpManager = PowerUpManager.Get();
			int x1 = (int)(board.GetMaxX() * m_generatePerimeterSize);
			int x2 = board.GetMaxX() - x1;
			int y1 = (int)(board.GetMaxY() * m_generatePerimeterSize);
			int y2 = board.GetMaxY() - y1;
			for (int i = 0; i < board.GetMaxX(); i++)
			{
				for (int j = 0; j < board.GetMaxY(); j++)
				{
					if (i < x1 || i > x2 || j < y1 || j > y2)
					{
						BoardSquare square = board.GetSquareFromIndex(i, j);
						if (square != null
							&& square.IsValidForGameplay()
							&& !square.IsInBrush()
							&& square.height - board.BaselineHeight < 0.5f
							&& !powerUpManager.IsPowerUpSpawnPoint(square))
						{
							Team team = (i + j) % 2 == 0 ? Team.TeamA : Team.TeamB;
							AddSpawnPoint(team, i, j);
						}
					}
				}
			}
		}
	}

	public Vector3 GetSpawnFacing(Vector3 spawnPosition)
	{
		Vector3 result = new Vector3(1f, 0f, 0f);
		if (m_initialSpawnLookAtPoint != null)
		{
			Vector3 vector = m_initialSpawnLookAtPoint.transform.position - spawnPosition;
			vector.y = 0f;
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
			{
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
		Array.Resize(ref m_spawnPointsTeamA, 0);
		Array.Resize(ref m_spawnPointsTeamB, 0);
	}

	public void AddSpawnPoint(Team team, int x, int y)
	{
		if (team == Team.TeamA)
		{
			if (m_spawnPointsTeamA == null)
			{
				m_spawnPointsTeamA = new SpawnPointCoord[1];
			}
			else
			{
				Array.Resize(ref m_spawnPointsTeamA, m_spawnPointsTeamA.Length + 1);
			}
			m_spawnPointsTeamA[m_spawnPointsTeamA.Length - 1] = new SpawnPointCoord(x, y);
		}
		else if (team == Team.TeamB)
		{
			if (m_spawnPointsTeamB == null)
			{
				m_spawnPointsTeamB = new SpawnPointCoord[1];
			}
			else
			{
				Array.Resize(ref m_spawnPointsTeamB, m_spawnPointsTeamB.Length + 1);
			}
			m_spawnPointsTeamB[m_spawnPointsTeamB.Length - 1] = new SpawnPointCoord(x, y);
		}
	}

	internal List<BoardSquare> GetSpawnSquaresList(Team team, RespawnMethod respawnMethod)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (respawnMethod == RespawnMethod.RespawnOnlyAtInitialSpawnPoints)
		{
			if (team == Team.TeamA)
			{
				list = m_initialSpawnPointsTeamA.GetSquaresInRegion();
			}
			else
			{
				list = m_initialSpawnPointsTeamB.GetSquaresInRegion();
			}
		}
		else if (respawnMethod == RespawnMethod.RespawnInGraveyards)
		{
			if (team == Team.TeamA)
			{
				list = m_spawnRegionsTeamA.GetSquaresInRegion();
			}
			else
			{
				list = m_spawnRegionsTeamB.GetSquaresInRegion();
			}
			list.RemoveAll((BoardSquare s) => !s.IsValidForGameplay());
		}
		else if (team == Team.TeamA)
		{
			if (m_spawnPointsTeamA != null)
			{
				foreach (SpawnPointCoord point in m_spawnPointsTeamA)
				{
					if (point != null)
					{
						BoardSquare square = Board.Get().GetSquareFromIndex(point.x, point.y);
						list.Add(square);
					}
				}
			}
		}
		else if (m_spawnPointsTeamB != null)
		{
			foreach (SpawnPointCoord point in m_spawnPointsTeamB)
			{
				if (point != null)
				{
					BoardSquare square = Board.Get().GetSquareFromIndex(point.x, point.y);
					if (square)
					{
						list.Add(square);
					}
				}
			}
		}
		return list;
	}

	public bool CanSpawnOnSquare(ActorData spawner, BoardSquare square, bool allowOccupiedSquares = false)
	{
		return square != null
			&& MovementUtils.CanStopOnSquare(square)
			&& (allowOccupiedSquares || square.occupant == null);
	}

	private int SortByProximityWeights(List<BoardSquare> spawnSquareList, ActorData actorSpawning, bool avoidLastDeathPosition, List<ActorData> actorsSpawned, HashSet<BoardSquare> squaresToAvoid, HashSet<BoardSquare> squaresNotAllowed, bool onlyAvoidVisibleEnemies, bool allowOccupiedSquares, int minimumFavoredSquares)
	{
		BoardSquare preferedSpawnLocation;
		if (actorSpawning != null && m_respawnMethod == RespawnMethod.RespawnOnlyAtInitialSpawnPoints)
		{
			preferedSpawnLocation = actorSpawning.InitialSpawnSquare;
		}
		else
		{
			preferedSpawnLocation = null;
		}
		SpawnSquareComparer spawnSquareComparer = new SpawnSquareComparer(spawnSquareList, actorSpawning, avoidLastDeathPosition, actorsSpawned, preferedSpawnLocation, squaresToAvoid, squaresNotAllowed, onlyAvoidVisibleEnemies, allowOccupiedSquares);
		spawnSquareList.Sort(spawnSquareComparer);
		int num = spawnSquareComparer.NumFriendTooCloseSquares + spawnSquareComparer.NumFavoredSquares;
		int num2 = spawnSquareComparer.NumEnemyTooCloseSquares + num;
		int result;
		if (spawnSquareComparer.NumFavoredSquares <= minimumFavoredSquares)
		{
			result = num > minimumFavoredSquares ? num : num2;
		}
		else
		{
			result = spawnSquareComparer.NumFavoredSquares;
		}
		return result;
	}

	internal BoardSquare GetSpawnSquare(ActorData spawner, bool avoidLastDeathPosition = true, List<ActorData> spawnedActors = null, HashSet<BoardSquare> squaresToAvoid = null)
	{
		BoardSquare result = null;
		List<BoardSquare> spawnSquaresList = GetSpawnSquaresList(spawner.GetTeam(), m_respawnMethod);
		int num = SortByProximityWeights(spawnSquaresList, spawner, avoidLastDeathPosition, spawnedActors, null, squaresToAvoid, false, false, 0);
		if (num > 0)
		{
			int index = spawnedActors != null && spawnedActors.Count == 0
				? GameplayRandom.Range(0, spawnSquaresList.Count)
				: 0;
			result = spawnSquaresList[index];
			bool canSpawn = CanSpawnOnSquare(spawner, result, false);
			if (!canSpawn)
			{
				Log.Error("Debugging, spawn square already occupied");
				for (int i = 0; i < spawnSquaresList.Count; i++)
				{
					if (CanSpawnOnSquare(spawner, spawnSquaresList[i], false))
					{
						result = spawnSquaresList[i];
						canSpawn = true;
						break;
					}
				}
			}
			if (!canSpawn)
			{
				Log.Error("Debugging, failed to find respawn square");
			}
		}
		else
		{
			SpawnPointCoord[] spawnPoints = spawner.GetTeam() == Team.TeamA ? m_spawnPointsTeamA : m_spawnPointsTeamB;
			foreach (SpawnPointCoord spawnPointCoord in spawnPoints)
			{
				List<BoardSquare> list = new List<BoardSquare>(8);
				Board.Get().GetAllAdjacentSquares(spawnPointCoord.x, spawnPointCoord.y, ref list);
				foreach (BoardSquare square in list)
				{
					if (CanSpawnOnSquare(spawner, square, false))
					{
						spawnSquaresList.Add(square);
					}
				}
			}
			num = SortByProximityWeights(spawnSquaresList, spawner, avoidLastDeathPosition, spawnedActors, squaresToAvoid, null, false, false, 0);
			if (num > 0)
			{
				result = spawnSquaresList[0];
			}
		}
		if (result == null)
		{
			for (int i = 0; i < Board.Get().GetMaxX(); i++)
			{
				for (int j = 0; j < Board.Get().GetMaxY(); j++)
				{
					BoardSquare square = Board.Get().GetSquareFromIndex(i, j);
					if (CanSpawnOnSquare(spawner, square, false))
					{
						spawnSquaresList.Add(square);
					}
				}
			}
			num = SortByProximityWeights(spawnSquaresList, spawner, avoidLastDeathPosition, spawnedActors, squaresToAvoid, null, false, false, 0);
			if (num > 0)
			{
				result = spawnSquaresList[0];
			}
		}
		if (result == null)
		{
			Log.Error(new StringBuilder().Append("Failed to find a spawn square for ").Append(spawner.DisplayName).ToString());
		}
		else if (result.occupant != null)
		{
			Log.Error("Debugging, spawn square found is occupied");
		}
		return result;
	}

	internal BoardSquare GetInitialSpawnSquare(ActorData spawner, List<ActorData> spawnedActors)
	{
		BoardSquare result = null;
		List<BoardSquare> squaresInRegion;
		if (spawner.GetTeam() == Team.TeamA)
		{
			squaresInRegion = m_initialSpawnPointsTeamA.GetSquaresInRegion();
		}
		else
		{
			squaresInRegion = m_initialSpawnPointsTeamB.GetSquaresInRegion();
		}
		if (squaresInRegion != null && squaresInRegion.Count != 0)
		{
			foreach (BoardSquare square in squaresInRegion)
			{
				if (CanSpawnOnSquare(spawner, square, false))
				{
					result = square;
					break;
				}
			}
			if (result == null)
			{
				Log.Error(new StringBuilder().Append("Couldn't find an initial spawn square for actor on team ").Append(spawner.GetTeam().ToString()).Append(", make sure Initial Spawn Points are set up.").ToString());
				foreach (BoardSquare boardSquare3 in squaresInRegion)
				{
					List<BoardSquare> list = new List<BoardSquare>(8);
					Board.Get().GetAllAdjacentSquares(boardSquare3.x, boardSquare3.y, ref list);
					foreach (BoardSquare square in list)
					{
						if (CanSpawnOnSquare(spawner, square, false))
						{
							result = square;
							break;
						}
					}
					if (result != null)
					{
						break;
					}
				}
			}
			if (result == null)
			{
				Log.Error(new StringBuilder().Append("Couldn't even find a viable spawn square adjacent to any initial spawn squares for actor on team ").Append(spawner.GetTeam().ToString()).Append(", make sure Initial Spawn Points are set up.").ToString());
				for (int i = 0; i < 128; i++)
				{
					if (result == null)
					{
						int x = GameplayRandom.Range(0, Board.Get().GetMaxX());
						int y = GameplayRandom.Range(0, Board.Get().GetMaxY());
						BoardSquare square = Board.Get().GetSquareFromIndex(x, y);
						if (CanSpawnOnSquare(spawner, square, false))
						{
							result = square;
						}
					}
				}
			}
			return result;
		}
		return GetSpawnSquare(spawner, false, spawnedActors, null);
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.color = ActorData.s_teamAColor;
		Board board = Board.Get();
		if (m_spawnPointsTeamA != null)
		{
			foreach (SpawnPointCoord spawnPointCoord in m_spawnPointsTeamA)
			{
				BoardSquare square = board != null ? board.GetSquareFromIndex(spawnPointCoord.x, spawnPointCoord.y) : null;
				if (square != null)
				{
					Gizmos.DrawWireSphere(square.ToVector3(), 0.7f);
				}
			}
		}
		Gizmos.color = ActorData.s_teamBColor;
		if (m_spawnPointsTeamB != null)
		{
			foreach (SpawnPointCoord spawnPointCoord in m_spawnPointsTeamB)
			{
				BoardSquare square = board?.GetSquareFromIndex(spawnPointCoord.x, spawnPointCoord.y);
				if (square != null)
				{
					Gizmos.DrawWireSphere(square.ToVector3(), 0.7f);
				}
			}
		}
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
		{
			List<BoardSquare> spawnSquaresList = GetSpawnSquaresList(GameFlowData.Get().activeOwnedActorData.GetTeam(), m_respawnMethod);
			int num = SortByProximityWeights(spawnSquaresList, GameFlowData.Get().activeOwnedActorData, true, null, null, null, false, false, 0);
			Gizmos.color = ActorData.s_teamBColor;
			for (int i = 0; i < spawnSquaresList.Count; i++)
			{
				BoardSquare square = spawnSquaresList[i];
				if (i == 0)
				{
					Gizmos.color = Color.green;
				}
				else if (i >= num)
				{
					Gizmos.color = Color.black;
				}
				else
				{
					float num2 = (spawnSquaresList.Count - (float)i) / spawnSquaresList.Count;
					Gizmos.color = new Color(num2, num2, num2);
				}
				Gizmos.DrawSphere(square.ToVector3(), 0.3f);
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
			x = inputX;
			y = inputY;
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
		private Dictionary<BoardSquare, ScoreInfo> m_scores;
		private ActorData m_actorSpawning;
		private BoardSquare m_preferedSpawnLocation;

		internal SpawnSquareComparer(List<BoardSquare> spawnSquareList, ActorData actorSpawning, bool avoidLastDeathPosition, List<ActorData> spawnedActors, BoardSquare preferedSpawnLocation, HashSet<BoardSquare> squaresToAvoid, HashSet<BoardSquare> squaresNotAllowed, bool onlyAvoidVisibleEnemies, bool allowOccupiedSquares)
		{
			m_scores = new Dictionary<BoardSquare, ScoreInfo>(spawnSquareList.Count);
			m_actorSpawning = actorSpawning;
			m_preferedSpawnLocation = preferedSpawnLocation;

			SpawnPointManager spawnPointManager = Get();
			float chooseDontCareDist = spawnPointManager.m_chooseDontCareDistance * Board.Get().squareSize;
			float minDistToFriend = spawnedActors != null
				? spawnPointManager.m_startMinDistToFriend * Board.Get().squareSize
				: spawnPointManager.m_minDistToFriend * Board.Get().squareSize;
			float minDistToFriendSqr = minDistToFriend * minDistToFriend;
			float minDistToEnemy = spawnedActors != null
				? spawnPointManager.m_startMinDistToEnemy * Board.Get().squareSize
				: spawnPointManager.m_minDistToEnemy * Board.Get().squareSize;
			float minDistToEnemySqr = minDistToEnemy * minDistToEnemy;
			List<ActorData> actors = spawnedActors == null
				? GameFlowData.Get().GetActors()
				: spawnedActors;

			foreach (BoardSquare square in spawnSquareList)
			{
				Vector3 squarePos = square.ToVector3();
				m_scores[square] = new ScoreInfo
				{
					m_avoidIfPossible = squaresToAvoid != null && squaresToAvoid.Contains(square)
				};
				if (!spawnPointManager.CanSpawnOnSquare(m_actorSpawning, square, allowOccupiedSquares)
					|| squaresNotAllowed == null
					|| squaresNotAllowed.Contains(square))
				{
					m_scores[square].m_cantSpawn = true;
				}
				else if (avoidLastDeathPosition)
				{
					float distToDeathSqr = (actorSpawning.LastDeathPosition - squarePos).sqrMagnitude;
					float distDelta = Mathf.Max(0f, chooseDontCareDist - Mathf.Sqrt(distToDeathSqr));
					float score = distDelta * spawnPointManager.m_chooseWeightDeathProximity;
					if (distDelta < 0f && score > 0f)
					{
						score = 0f;
					}
					m_scores[square].m_score = score;
				}
				else
				{
					m_scores[square].m_score = 0f;
				}
				foreach (ActorData actorData in actors)
				{
					if (actorData != null
						&& !actorData.IsDead()
						&& actorData.PlayerIndex != PlayerData.s_invalidPlayerIndex
						&& actorData.GetCurrentBoardSquare() != null)
					{
						BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
						float distSqr = (currentBoardSquare.ToVector3() - squarePos).sqrMagnitude;
						bool isAlly = actorData.GetTeam() == m_actorSpawning.GetTeam();
						if (isAlly && distSqr < minDistToFriendSqr)
						{
							m_scores[square].m_tooCloseToFriendly = true;
						}
						if (isAlly)
						{
							float distDelta = Mathf.Max(0f, chooseDontCareDist - Mathf.Sqrt(distSqr));
							float proximityWeight = actorData.GetTeam() != m_actorSpawning.GetTeam()
								? spawnPointManager.m_chooseWeightEnemyProximity
								: spawnPointManager.m_chooseWeightFriendProximity;
							float score = distDelta * proximityWeight;
							if (distDelta < 0f && score > 0f)
							{
								score = 0f;
							}
							m_scores[square].m_score += score;
						}
						else if (distSqr < minDistToEnemySqr
								&& (!onlyAvoidVisibleEnemies || actorData.IsActorVisibleToAnyEnemy()))
						{
							m_scores[square].m_tooCloseToEnemy = true;
						}
					}
				}
				if (!m_scores[square].m_cantSpawn)
				{
					if (m_scores[square].m_tooCloseToEnemy)
					{
						NumEnemyTooCloseSquares++;
					}
					else if (m_scores[square].m_tooCloseToFriendly)
					{
						NumFriendTooCloseSquares++;
					}
					else if (!m_scores[square].m_avoidIfPossible)
					{
						NumFavoredSquares++;
					}
				}
			}
		}

		internal int NumFavoredSquares { get; private set; }
		internal int NumFriendTooCloseSquares { get; private set; }
		internal int NumEnemyTooCloseSquares { get; private set; }
		public int Compare(BoardSquare x, BoardSquare y)
		{
			if (x == null)
			{
				if (y == null)
				{
					return 0;
				}
				return 1;
			}
			else
			{
				if (y == null)
				{
					return -1;
				}
				if (m_scores[y].m_cantSpawn != m_scores[x].m_cantSpawn)
				{
					if (m_scores[x].m_cantSpawn)
					{
						return 1;
					}
					else
					{
						return -1;
					}
				}
				if (m_preferedSpawnLocation != null)
				{
					if (m_preferedSpawnLocation == x || m_preferedSpawnLocation == y)
					{
						if (m_preferedSpawnLocation == x)
						{
							return -1;
						}
						else
						{
							return 1;
						}
					}
				}
				if (m_scores[y].m_avoidIfPossible != m_scores[x].m_avoidIfPossible)
				{
					if (m_scores[x].m_avoidIfPossible)
					{
						return 1;
					}
					else
					{
						return -1;
					}
				}
				if (m_scores[y].m_tooCloseToEnemy != m_scores[x].m_tooCloseToEnemy)
				{
					if (m_scores[x].m_tooCloseToEnemy)
					{
						return 1;
					}
					else
					{
						return -1;
					}
				}
				if (m_scores[y].m_tooCloseToFriendly != m_scores[x].m_tooCloseToFriendly)
				{
					if (m_scores[x].m_tooCloseToFriendly)
					{
						return 1;
					}
					else
					{
						return -1;
					}
				}
				return m_scores[y].m_score.CompareTo(m_scores[x].m_score);
			}
		}

		public void Debug_DrawGizmo(List<BoardSquare> spawnSquareList, float timeToDisplay = 20f)
		{
			if (spawnSquareList != null && m_scores != null)
			{
				bool flag = false;
				foreach (BoardSquare square in spawnSquareList)
				{
					if (m_scores.ContainsKey(square))
					{
						if (m_scores[square].m_cantSpawn)
						{
							Debug.DrawRay(square.ToVector3(), 1.5f * Vector3.up, Color.red, timeToDisplay);
						}
						else if (m_scores[square].m_avoidIfPossible)
						{
							flag = true;
							Debug.DrawRay(square.ToVector3(), new Vector3(0.5f, 1.5f, 0f), 0.5f * (Color.yellow + Color.red), timeToDisplay);
						}
						else if (m_scores[square].m_tooCloseToEnemy || m_scores[square].m_tooCloseToFriendly)
						{
							Debug.DrawRay(square.ToVector3(), new Vector3(0f, 1.5f, 0.5f), Color.magenta, timeToDisplay);
							if (flag)
							{
								Debug.LogWarning("Respawn: square to avoid not sorted toward end of list");
							}
						}
						else
						{
							Debug.DrawRay(square.ToVector3(), 1.5f * Vector3.up, Color.white, timeToDisplay);
							if (flag)
							{
								Debug.LogWarning("Respawn: square to avoid not sorted toward end of list");
							}
						}
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
