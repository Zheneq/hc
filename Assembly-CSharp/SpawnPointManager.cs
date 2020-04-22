using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
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
		private class ScoreInfo
		{
			internal float m_score;

			internal bool m_cantSpawn;

			internal bool m_tooCloseToEnemy;

			internal bool m_tooCloseToFriendly;

			internal bool m_avoidIfPossible;
		}

		private Dictionary<BoardSquare, ScoreInfo> m_scores;

		private ActorData m_actorSpawning;

		private BoardSquare m_preferedSpawnLocation;

		internal int NumFavoredSquares
		{
			get;
			private set;
		}

		internal int NumFriendTooCloseSquares
		{
			get;
			private set;
		}

		internal int NumEnemyTooCloseSquares
		{
			get;
			private set;
		}

		internal SpawnSquareComparer(List<BoardSquare> spawnSquareList, ActorData actorSpawning, bool avoidLastDeathPosition, List<ActorData> spawnedActors, BoardSquare preferedSpawnLocation, HashSet<BoardSquare> squaresToAvoid, HashSet<BoardSquare> squaresNotAllowed, bool onlyAvoidVisibleEnemies, bool allowOccupiedSquares)
		{
			m_scores = new Dictionary<BoardSquare, ScoreInfo>(spawnSquareList.Count);
			m_actorSpawning = actorSpawning;
			m_preferedSpawnLocation = preferedSpawnLocation;
			SpawnPointManager spawnPointManager = Get();
			float chooseWeightEnemyProximity = spawnPointManager.m_chooseWeightEnemyProximity;
			float chooseWeightFriendProximity = spawnPointManager.m_chooseWeightFriendProximity;
			float num = spawnPointManager.m_chooseDontCareDistance * Board.Get().squareSize;
			float num2 = (spawnedActors == null) ? (spawnPointManager.m_minDistToFriend * Board.Get().squareSize) : (spawnPointManager.m_startMinDistToFriend * Board.Get().squareSize);
			num2 *= num2;
			float num3;
			if (spawnedActors != null)
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
				while (true)
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
			for (int num5 = 0; num5 < spawnSquareList.Count; num5++)
			{
				BoardSquare boardSquare = spawnSquareList[num5];
				Vector3 b = boardSquare.ToVector3();
				m_scores[boardSquare] = new ScoreInfo();
				ScoreInfo scoreInfo = m_scores[boardSquare];
				int avoidIfPossible;
				if (squaresToAvoid != null)
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
					avoidIfPossible = (squaresToAvoid.Contains(boardSquare) ? 1 : 0);
				}
				else
				{
					avoidIfPossible = 0;
				}
				scoreInfo.m_avoidIfPossible = ((byte)avoidIfPossible != 0);
				if (spawnPointManager.CanSpawnOnSquare(m_actorSpawning, boardSquare, allowOccupiedSquares))
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
					if (squaresNotAllowed != null && !squaresNotAllowed.Contains(boardSquare))
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
						if (avoidLastDeathPosition)
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
							float sqrMagnitude = (actorSpawning.LastDeathPosition - b).sqrMagnitude;
							float num6 = Mathf.Max(0f, num - Mathf.Sqrt(sqrMagnitude));
							float num7 = num6 * spawnPointManager.m_chooseWeightDeathProximity;
							if (num6 < 0f && num7 > 0f)
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
								num7 = 0f;
							}
							m_scores[boardSquare].m_score = num7;
						}
						else
						{
							m_scores[boardSquare].m_score = 0f;
						}
						goto IL_024d;
					}
				}
				m_scores[boardSquare].m_cantSpawn = true;
				goto IL_024d;
				IL_024d:
				for (int i = 0; i < list2.Count; i++)
				{
					ActorData actorData = list2[i];
					if (!(actorData != null))
					{
						continue;
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
					if (actorData.IsDead())
					{
						continue;
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
					if (actorData.PlayerIndex == PlayerData.s_invalidPlayerIndex)
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
						break;
					}
					if (!(actorData.GetCurrentBoardSquare() != null))
					{
						continue;
					}
					BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
					float sqrMagnitude2 = (currentBoardSquare.ToVector3() - b).sqrMagnitude;
					bool flag = actorData.GetTeam() == m_actorSpawning.GetTeam();
					if (flag)
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
						if (sqrMagnitude2 < num2)
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
							m_scores[boardSquare].m_tooCloseToFriendly = true;
						}
					}
					if (!flag)
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
						if (!(sqrMagnitude2 < num4))
						{
							continue;
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
						if (onlyAvoidVisibleEnemies)
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
							if (!actorData.IsVisibleToOpposingTeam())
							{
								continue;
							}
						}
						m_scores[boardSquare].m_tooCloseToEnemy = true;
						continue;
					}
					float num8 = Mathf.Max(0f, num - Mathf.Sqrt(sqrMagnitude2));
					float num9 = num8 * ((actorData.GetTeam() != m_actorSpawning.GetTeam()) ? chooseWeightEnemyProximity : chooseWeightFriendProximity);
					if (num8 < 0f && num9 > 0f)
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
						num9 = 0f;
					}
					m_scores[boardSquare].m_score += num9;
				}
				if (!m_scores[boardSquare].m_cantSpawn)
				{
					if (m_scores[boardSquare].m_tooCloseToEnemy)
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
						NumEnemyTooCloseSquares++;
					}
					else if (m_scores[boardSquare].m_tooCloseToFriendly)
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
						NumFriendTooCloseSquares++;
					}
					else if (!m_scores[boardSquare].m_avoidIfPossible)
					{
						NumFavoredSquares++;
					}
				}
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

		public int Compare(BoardSquare x, BoardSquare y)
		{
			if (x == null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (y == null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return 0;
							}
						}
					}
					return 1;
				}
			}
			if (y == null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					return -1;
				}
			}
			if (m_scores[y].m_cantSpawn != m_scores[x].m_cantSpawn)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					int result;
					if (m_scores[x].m_cantSpawn)
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
						result = 1;
					}
					else
					{
						result = -1;
					}
					return result;
				}
			}
			if (m_preferedSpawnLocation != null)
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
				if (!(m_preferedSpawnLocation == x))
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
					if (!(m_preferedSpawnLocation == y))
					{
						goto IL_0109;
					}
					while (true)
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
				if (m_preferedSpawnLocation == x)
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
					result2 = -1;
				}
				else
				{
					result2 = 1;
				}
				return result2;
			}
			goto IL_0109;
			IL_0109:
			if (m_scores[y].m_avoidIfPossible != m_scores[x].m_avoidIfPossible)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						int result3;
						if (m_scores[x].m_avoidIfPossible)
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
							result3 = 1;
						}
						else
						{
							result3 = -1;
						}
						return result3;
					}
					}
				}
			}
			if (m_scores[y].m_tooCloseToEnemy != m_scores[x].m_tooCloseToEnemy)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						int result4;
						if (m_scores[x].m_tooCloseToEnemy)
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
							result4 = 1;
						}
						else
						{
							result4 = -1;
						}
						return result4;
					}
					}
				}
			}
			if (m_scores[y].m_tooCloseToFriendly != m_scores[x].m_tooCloseToFriendly)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						int result5;
						if (m_scores[x].m_tooCloseToFriendly)
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
							result5 = 1;
						}
						else
						{
							result5 = -1;
						}
						return result5;
					}
					}
				}
			}
			return m_scores[y].m_score.CompareTo(m_scores[x].m_score);
		}

		public void _001D(List<BoardSquare> _001D, float _000E = 20f)
		{
			if (_001D == null)
			{
				return;
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
				if (m_scores == null)
				{
					return;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					bool flag = false;
					for (int i = 0; i < _001D.Count; i++)
					{
						BoardSquare boardSquare = _001D[i];
						if (!m_scores.ContainsKey(boardSquare))
						{
							continue;
						}
						if (m_scores[boardSquare].m_cantSpawn)
						{
							Debug.DrawRay(boardSquare.ToVector3(), 1.5f * Vector3.up, Color.red, _000E);
							continue;
						}
						if (m_scores[boardSquare].m_avoidIfPossible)
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
							flag = true;
							Debug.DrawRay(boardSquare.ToVector3(), new Vector3(0.5f, 1.5f, 0f), 0.5f * (Color.yellow + Color.red), _000E);
							continue;
						}
						if (!m_scores[boardSquare].m_tooCloseToEnemy)
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
							if (!m_scores[boardSquare].m_tooCloseToFriendly)
							{
								Debug.DrawRay(boardSquare.ToVector3(), 1.5f * Vector3.up, Color.white, _000E);
								if (flag)
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
									Debug.LogWarning("Respawn: square to avoid not sorted toward end of list");
								}
								continue;
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
						Debug.DrawRay(boardSquare.ToVector3(), new Vector3(0f, 1.5f, 0.5f), Color.magenta, _000E);
						if (flag)
						{
							Debug.LogWarning("Respawn: square to avoid not sorted toward end of list");
						}
					}
					while (true)
					{
						switch (2)
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
		if (m_playersSelectRespawn || m_respawnMethod != 0)
		{
			return;
		}
		ClearSpawnPoints();
		Board board = Board.Get();
		PowerUpManager powerUpManager = PowerUpManager.Get();
		int num = (int)((float)board.GetMaxX() * m_generatePerimeterSize);
		int num2 = board.GetMaxX() - num;
		int num3 = (int)((float)board.GetMaxY() * m_generatePerimeterSize);
		int num4 = board.GetMaxY() - num3;
		for (int i = 0; i < board.GetMaxX(); i++)
		{
			for (int j = 0; j < board.GetMaxY(); j++)
			{
				if (i >= num)
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
					if (i <= num2 && j >= num3)
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
						if (j <= num4)
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
							continue;
						}
					}
				}
				BoardSquare boardSquare = board.GetBoardSquare(i, j);
				if (!(boardSquare != null))
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
				if (!boardSquare.IsBaselineHeight() || boardSquare.IsInBrushRegion() || !((float)(boardSquare.height - board.BaselineHeight) < 0.5f))
				{
					continue;
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
				if (powerUpManager.IsPowerUpSpawnPoint(boardSquare))
				{
					continue;
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
				int team;
				if ((i + j) % 2 == 0)
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
					team = 0;
				}
				else
				{
					team = 1;
				}
				AddSpawnPoint((Team)team, i, j);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					goto end_IL_0190;
				}
				continue;
				end_IL_0190:
				break;
			}
		}
	}

	public Vector3 GetSpawnFacing(Vector3 spawnPosition)
	{
		Vector3 result = new Vector3(1f, 0f, 0f);
		if (m_initialSpawnLookAtPoint != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Vector3 vector = m_initialSpawnLookAtPoint.transform.position - spawnPosition;
			vector.y = 0f;
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
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
				result = ((vector.x > 0f) ? new Vector3(1f, 0f, 0f) : new Vector3(-1f, 0f, 0f));
			}
			else if (!(vector.z > 0f))
			{
				result = new Vector3(0f, 0f, -1f);
			}
			else
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
				result = new Vector3(0f, 0f, 1f);
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (m_spawnPointsTeamA == null)
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
						m_spawnPointsTeamA = new SpawnPointCoord[1];
					}
					else
					{
						Array.Resize(ref m_spawnPointsTeamA, m_spawnPointsTeamA.Length + 1);
					}
					m_spawnPointsTeamA[m_spawnPointsTeamA.Length - 1] = new SpawnPointCoord(x, y);
					return;
				}
			}
		}
		if (team != Team.TeamB)
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
			if (m_spawnPointsTeamB == null)
			{
				m_spawnPointsTeamB = new SpawnPointCoord[1];
			}
			else
			{
				Array.Resize(ref m_spawnPointsTeamB, m_spawnPointsTeamB.Length + 1);
			}
			m_spawnPointsTeamB[m_spawnPointsTeamB.Length - 1] = new SpawnPointCoord(x, y);
			return;
		}
	}

	internal List<BoardSquare> GetSpawnSquaresList(Team team, RespawnMethod respawnMethod)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (respawnMethod == RespawnMethod.RespawnOnlyAtInitialSpawnPoints)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (team == Team.TeamA)
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
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				list = m_spawnRegionsTeamA.GetSquaresInRegion();
			}
			else
			{
				list = m_spawnRegionsTeamB.GetSquaresInRegion();
			}
			List<BoardSquare> list2 = list;
			if (_003C_003Ef__am_0024cache0 == null)
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
				_003C_003Ef__am_0024cache0 = ((BoardSquare s) => !s.IsBaselineHeight());
			}
			list2.RemoveAll(_003C_003Ef__am_0024cache0);
		}
		else if (team == Team.TeamA)
		{
			if (m_spawnPointsTeamA != null)
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
				for (int i = 0; i < m_spawnPointsTeamA.Length; i++)
				{
					if (m_spawnPointsTeamA[i] != null)
					{
						BoardSquare boardSquare = Board.Get().GetBoardSquare(m_spawnPointsTeamA[i].x, m_spawnPointsTeamA[i].y);
						list.Add(boardSquare);
					}
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
		}
		else if (m_spawnPointsTeamB != null)
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
			for (int j = 0; j < m_spawnPointsTeamB.Length; j++)
			{
				if (m_spawnPointsTeamB[j] == null)
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
				BoardSquare boardSquare2 = Board.Get().GetBoardSquare(m_spawnPointsTeamB[j].x, m_spawnPointsTeamB[j].y);
				if ((bool)boardSquare2)
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
					list.Add(boardSquare2);
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
			while (true)
			{
				switch (1)
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
			if (MovementUtils.CanStopOnSquare(square))
			{
				if (!allowOccupiedSquares)
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
					if (!(square.occupant == null))
					{
						goto IL_0053;
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
				result = true;
			}
		}
		goto IL_0053;
		IL_0053:
		return result;
	}

	private int SortByProximityWeights(List<BoardSquare> spawnSquareList, ActorData actorSpawning, bool avoidLastDeathPosition, List<ActorData> actorsSpawned, HashSet<BoardSquare> squaresToAvoid, HashSet<BoardSquare> squaresNotAllowed, bool onlyAvoidVisibleEnemies, bool allowOccupiedSquares, int minimumFavoredSquares)
	{
		object obj;
		if (actorSpawning != null)
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
			if (m_respawnMethod == RespawnMethod.RespawnOnlyAtInitialSpawnPoints)
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
				obj = actorSpawning.InitialSpawnSquare;
				goto IL_0038;
			}
		}
		obj = null;
		goto IL_0038;
		IL_0038:
		BoardSquare preferedSpawnLocation = (BoardSquare)obj;
		SpawnSquareComparer spawnSquareComparer = new SpawnSquareComparer(spawnSquareList, actorSpawning, avoidLastDeathPosition, actorsSpawned, preferedSpawnLocation, squaresToAvoid, squaresNotAllowed, onlyAvoidVisibleEnemies, allowOccupiedSquares);
		spawnSquareList.Sort(spawnSquareComparer);
		int num = spawnSquareComparer.NumFriendTooCloseSquares + spawnSquareComparer.NumFavoredSquares;
		int num2 = spawnSquareComparer.NumEnemyTooCloseSquares + num;
		int result;
		if (spawnSquareComparer.NumFavoredSquares <= minimumFavoredSquares)
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
		List<BoardSquare> spawnSquaresList = GetSpawnSquaresList(spawner.GetTeam(), m_respawnMethod);
		int num = SortByProximityWeights(spawnSquaresList, spawner, avoidLastDeathPosition, spawnedActors, null, squaresToAvoid, false, false, 0);
		if (num > 0)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			int num2;
			if (spawnedActors != null)
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
				num2 = ((spawnedActors.Count == 0) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			int index = (num2 != 0) ? GameplayRandom.Range(0, spawnSquaresList.Count) : 0;
			boardSquare = spawnSquaresList[index];
			bool flag = CanSpawnOnSquare(spawner, boardSquare);
			if (!flag)
			{
				Log.Error("Debugging, spawn square already occupied");
				for (int i = 0; i < spawnSquaresList.Count; i++)
				{
					if (CanSpawnOnSquare(spawner, spawnSquaresList[i]))
					{
						boardSquare = spawnSquaresList[i];
						flag = true;
						break;
					}
				}
			}
			if (!flag)
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
				Log.Error("Debugging, failed to find respawn square");
			}
		}
		else
		{
			SpawnPointCoord[] array;
			if (spawner.GetTeam() == Team.TeamA)
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
				array = m_spawnPointsTeamA;
			}
			else
			{
				array = m_spawnPointsTeamB;
			}
			SpawnPointCoord[] array2 = array;
			foreach (SpawnPointCoord spawnPointCoord in array2)
			{
				List<BoardSquare> result = new List<BoardSquare>(8);
				Board.Get().GetAllAdjacentSquares(spawnPointCoord.x, spawnPointCoord.y, ref result);
				using (List<BoardSquare>.Enumerator enumerator = result.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BoardSquare current = enumerator.Current;
						if (CanSpawnOnSquare(spawner, current))
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
							spawnSquaresList.Add(current);
						}
					}
					while (true)
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
			num = SortByProximityWeights(spawnSquaresList, spawner, avoidLastDeathPosition, spawnedActors, squaresToAvoid, null, false, false, 0);
			if (num > 0)
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
				boardSquare = spawnSquaresList[0];
			}
		}
		if (boardSquare == null)
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
			Board board = Board.Get();
			for (int k = 0; k < board.GetMaxX(); k++)
			{
				for (int l = 0; l < board.GetMaxY(); l++)
				{
					BoardSquare boardSquare2 = board.GetBoardSquare(k, l);
					if (CanSpawnOnSquare(spawner, boardSquare2))
					{
						spawnSquaresList.Add(boardSquare2);
					}
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						goto end_IL_0259;
					}
					continue;
					end_IL_0259:
					break;
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
			num = SortByProximityWeights(spawnSquaresList, spawner, avoidLastDeathPosition, spawnedActors, squaresToAvoid, null, false, false, 0);
			if (num > 0)
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
				boardSquare = spawnSquaresList[0];
			}
		}
		if (boardSquare == null)
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
			Log.Error("Failed to find a spawn square for " + spawner.DisplayName);
		}
		else if (boardSquare.occupant != null)
		{
			Log.Error("Debugging, spawn square found is occupied");
		}
		return boardSquare;
	}

	internal BoardSquare GetInitialSpawnSquare(ActorData spawner, List<ActorData> spawnedActors)
	{
		BoardSquare boardSquare = null;
		List<BoardSquare> squaresInRegion;
		if (spawner.GetTeam() == Team.TeamA)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			squaresInRegion = m_initialSpawnPointsTeamA.GetSquaresInRegion();
		}
		else
		{
			squaresInRegion = m_initialSpawnPointsTeamB.GetSquaresInRegion();
		}
		if (squaresInRegion != null)
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
			if (squaresInRegion.Count != 0)
			{
				using (List<BoardSquare>.Enumerator enumerator = squaresInRegion.GetEnumerator())
				{
					while (true)
					{
						if (!enumerator.MoveNext())
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
							break;
						}
						BoardSquare current = enumerator.Current;
						if (CanSpawnOnSquare(spawner, current))
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									boardSquare = current;
									goto end_IL_0070;
								}
							}
						}
					}
					end_IL_0070:;
				}
				if (boardSquare == null)
				{
					Log.Error("Couldn't find an initial spawn square for actor on team " + spawner.GetTeam().ToString() + ", make sure Initial Spawn Points are set up.");
					foreach (BoardSquare item in squaresInRegion)
					{
						List<BoardSquare> result = new List<BoardSquare>(8);
						Board.Get().GetAllAdjacentSquares(item.x, item.y, ref result);
						foreach (BoardSquare item2 in result)
						{
							if (CanSpawnOnSquare(spawner, item2))
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										boardSquare = item2;
										goto end_IL_014a;
									}
								}
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
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					Log.Error("Couldn't even find a viable spawn square adjacent to any initial spawn squares for actor on team " + spawner.GetTeam().ToString() + ", make sure Initial Spawn Points are set up.");
					for (int i = 0; i < 128; i++)
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
						if (boardSquare == null)
						{
							Board board = Board.Get();
							int x = GameplayRandom.Range(0, board.GetMaxX());
							int y = GameplayRandom.Range(0, board.GetMaxY());
							BoardSquare boardSquare2 = board.GetBoardSquare(x, y);
							if (CanSpawnOnSquare(spawner, boardSquare2))
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
								boardSquare = boardSquare2;
							}
							continue;
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
						break;
					}
				}
				return boardSquare;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return GetSpawnSquare(spawner, false, spawnedActors);
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.color = ActorData.s_teamAColor;
		if (m_spawnPointsTeamA != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SpawnPointCoord[] spawnPointsTeamA = m_spawnPointsTeamA;
			foreach (SpawnPointCoord spawnPointCoord in spawnPointsTeamA)
			{
				BoardSquare boardSquare = (!(Board.Get() == null)) ? Board.Get().GetBoardSquare(spawnPointCoord.x, spawnPointCoord.y) : null;
				if (boardSquare != null)
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
					Gizmos.DrawWireSphere(boardSquare.ToVector3(), 0.7f);
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
		Gizmos.color = ActorData.s_teamBColor;
		if (m_spawnPointsTeamB != null)
		{
			SpawnPointCoord[] spawnPointsTeamB = m_spawnPointsTeamB;
			foreach (SpawnPointCoord spawnPointCoord2 in spawnPointsTeamB)
			{
				object obj;
				if (Board.Get() == null)
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
					obj = null;
				}
				else
				{
					obj = Board.Get().GetBoardSquare(spawnPointCoord2.x, spawnPointCoord2.y);
				}
				BoardSquare boardSquare2 = (BoardSquare)obj;
				if (boardSquare2 != null)
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
					Gizmos.DrawWireSphere(boardSquare2.ToVector3(), 0.7f);
				}
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
		if (GameFlowData.Get() == null)
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
			if (GameFlowData.Get().activeOwnedActorData == null)
			{
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			List<BoardSquare> spawnSquaresList = GetSpawnSquaresList(GameFlowData.Get().activeOwnedActorData.GetTeam(), m_respawnMethod);
			int num = SortByProximityWeights(spawnSquaresList, GameFlowData.Get().activeOwnedActorData, true, null, null, null, false, false, 0);
			Gizmos.color = ActorData.s_teamBColor;
			for (int k = 0; k < spawnSquaresList.Count; k++)
			{
				BoardSquare boardSquare3 = spawnSquaresList[k];
				if (k == 0)
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
				Gizmos.DrawSphere(boardSquare3.ToVector3(), 0.3f);
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
}
