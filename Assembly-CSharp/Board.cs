using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Board : MonoBehaviour, IGameEventListener
{
	private int m_maxX;
	private int m_maxY;
	private int m_maxHeight;
	private int m_lowestPositiveHeight = 9999;
	private int m_lastValidGuidedHeight = 99999;
	private BoardSquare[,] m_boardSquares;
	public HashSet<BoardSquare> m_highlightedBoardSquares = new HashSet<BoardSquare>();
	public GameObject m_LOSHighlightsParent;
	private MeshCollider m_cameraGuideMeshCollider;
	public bool m_showLOS;
	public float m_squareSize = 1f;
	public int m_baselineHeight = -1;
	public LOSLookup m_losLookup;

	private static float s_squareSizeStatic = 1.5f;
	internal static int BaselineHeightStatic;

	private static Board s_board;

	internal BuildNormalPathNodePool m_normalPathBuildScratchPool;
	internal BuildNormalPathHeap m_normalPathNodeHeap;
	private bool m_needToUpdateValidSquares = true;

	internal static float SquareSizeStatic
	{
		get
		{
			return s_squareSizeStatic;
		}
		private set
		{
			s_squareSizeStatic = value;
		}
	}

	public float squareSize => m_squareSize;
	public int BaselineHeight
	{
		get
		{
			if (m_baselineHeight >= 0)
			{
				return m_baselineHeight;
			}
			return m_lowestPositiveHeight;
		}
	}

	public float LosCheckHeight => BaselineHeight + BoardSquare.s_LoSHeightOffset;
	public Vector3 PlayerLookDir { get; private set; }
	public Vector3 PlayerMouseIntersectionPos { get; private set; }
	public Vector3 PlayerMouseLookDir { get; private set; }
	public Vector3 MouseBoardSquareIntersectionPos { get; private set; }
	public Vector3 PlayerFreePos { get; private set; }
	public Vector3 PlayerFreeCornerPos { get; private set; }
	public BoardSquare PlayerFreeSquare { get; private set; }
	public Vector3 PlayerClampedPos { get; private set; }
	public Vector3 PlayerClampedCornerPos { get; private set; }
	public BoardSquare PlayerClampedSquare { get; private set; }
	public bool MouseOverSquareInRange { get; set; }
	public bool MarkedForUpdateValidSquares => m_needToUpdateValidSquares;

	public static Board Get()
	{
		if (s_board == null && Application.isEditor && !Application.isPlaying)
		{
			s_board = UnityEngine.Object.FindObjectOfType<Board>();
			if (s_board != null)
			{
				s_board.ReevaluateBoard();
			}
		}
		return s_board;
	}

	private void Awake()
	{
		base.enabled = false;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameFlowDataStarted);
		s_board = this;
		if (m_LOSHighlightsParent == null)
		{
			m_LOSHighlightsParent = GameObject.Find("LOSHighlights");
		}
		if (m_LOSHighlightsParent != null)
		{
			m_LOSHighlightsParent.layer = LayerMask.NameToLayer("FogOfWar");
			m_LOSHighlightsParent.SetActive(true);
		}
		if (m_LOSHighlightsParent != null && !NetworkClient.active && NetworkServer.active)
		{
			UnityEngine.Object.DestroyImmediate(m_LOSHighlightsParent);
			m_LOSHighlightsParent = null;
		}
		GameObject cameraGuideMeshGO = GameObject.Find("Camera Guide Mesh");
		if (cameraGuideMeshGO != null)
		{
			m_cameraGuideMeshCollider = cameraGuideMeshGO.GetComponent<MeshCollider>();
		}
		ReevaluateBoard();
		m_showLOS = true;
		s_squareSizeStatic = m_squareSize;
		BaselineHeightStatic = BaselineHeight;
		m_normalPathBuildScratchPool = new BuildNormalPathNodePool();
		m_normalPathNodeHeap = new BuildNormalPathHeap(60);
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameFlowDataStarted);
		}
		m_normalPathBuildScratchPool = null;
		s_board = null;
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GameFlowDataStarted)
		{
			base.enabled = true;
		}
	}

	private void Update()
	{
		ActorData activeOwnedActorData = GameFlowData.Get()?.activeOwnedActorData;
		if (Camera.main != null)
		{
			Vector3 cameraPosition = Camera.main.transform.position;
			Vector3 mousePosition = Input.mousePosition;
			Vector3 direction = Camera.main.ScreenPointToRay(mousePosition).direction;
			float d = (m_baselineHeight - cameraPosition.y) / Vector3.Dot(direction, Vector3.up);
			PlayerMouseIntersectionPos = cameraPosition + direction * d;
			if (activeOwnedActorData != null)
			{
				PlayerMouseLookDir = (PlayerMouseIntersectionPos - activeOwnedActorData.transform.position).normalized;
			}
			if (ControlpadGameplay.Get() == null || !ControlpadGameplay.Get().UsingControllerInput)
			{
				PlayerLookDir = PlayerMouseLookDir;
				PlayerFreePos = PlayerMouseIntersectionPos;
			}
			else
			{
				PlayerLookDir = ControlpadGameplay.Get().ControllerAimDir;
				PlayerFreePos = ControlpadGameplay.Get().ControllerAimPos;
			}
			PlayerFreeSquare = GetSquareFromVec3(PlayerFreePos);
			PlayerFreeCornerPos = GetBestCornerPos(PlayerFreePos, PlayerFreeSquare);
			RecalcClampedSelections();
			HighlightUtils.Get().UpdateCursorPositions();
			HighlightUtils.Get().UpdateRangeIndicatorHighlight();
			HighlightUtils.Get().UpdateMouseoverCoverHighlight();
			HighlightUtils.Get().UpdateShowAffectedSquareFlag();
		}
		if (Input.GetMouseButtonUp(2))
		{
			bool applyToAllJoints = false;
			float amount = 300f;
			ApplyForceOnDead(PlayerFreeSquare, amount, new Vector3(0f, 1f, 0f), applyToAllJoints);
		}
	}

	public static Vector3 GetBestCornerPos(Vector3 freePos, BoardSquare closestSquare)
	{
		float half = SquareSizeStatic / 2f;
		float x;
		float z;
		if (closestSquare == null)
		{
			x = freePos.x;
			z = freePos.z;
		}
		else
		{
			x = (freePos.x > closestSquare.worldX) ? (closestSquare.worldX + half) : (closestSquare.worldX - half);
			z = (freePos.z > closestSquare.worldY) ? (closestSquare.worldY + half) : (closestSquare.worldY - half);
		}
		return new Vector3(x, freePos.y, z);
	}

	public void MarkForUpdateValidSquares(bool value = true)
	{
		m_needToUpdateValidSquares = value;
	}

	private void RecalcClampedSelections()
	{
		if (GameFlowData.Get() == null)
		{
			return;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData == null)
		{
			return;
		}
		ActorController actorController = activeOwnedActorData.GetComponent<ActorController>();
		if (actorController == null)
		{
			return;
		}

		actorController.RecalcAndHighlightValidSquares();
		HashSet<BoardSquare> squaresToClampTo = actorController.GetSquaresToClampTo();
		bool isChase;
		if (activeOwnedActorData.GetActorTurnSM().AmDecidingMovement()
			&& PlayerFreeSquare != null
			&& PlayerFreeSquare.occupant != null)
		{
			ActorData occupantActorData = PlayerFreeSquare.occupant.GetComponent<ActorData>();
			isChase = occupantActorData != null && occupantActorData.IsActorVisibleToClient();
		}
		else
		{
			isChase = false;
		}

		Vector3 vector = Vector3.zero;
		BoardSquare target = null;
		if (squaresToClampTo != null
			&& squaresToClampTo.Count != 0
			&& !squaresToClampTo.Contains(PlayerFreeSquare)
			&& !isChase)
		{
			float x = PlayerFreePos.x;
			float z = PlayerFreePos.z;
			float minDistSquared = float.MaxValue;
			foreach (BoardSquare square in squaresToClampTo)
			{
				float distX = square.worldX - x;
				float distY = square.worldY - z;
				float distSquared = distX * distX + distY * distY;
				if (distSquared <= minDistSquared)
				{
					minDistSquared = distSquared;
					target = square;
				}
			}
			if (target != null)
			{
				vector = target.CalcNearestPositionOnSquareEdge(PlayerFreePos);
			}
		}
		else
		{
			vector = PlayerFreePos;
			target = PlayerFreeSquare;
		}
		PlayerClampedPos = vector;
		PlayerClampedSquare = target;
		PlayerClampedCornerPos = GetBestCornerPos(vector, target);
	}

	public void ReevaluateBoard()
	{
		m_maxX = 0;
		m_maxY = 0;
		m_maxHeight = 0;
		m_lastValidGuidedHeight = 99999;
		m_lowestPositiveHeight = 99999;
		foreach (Transform transform in base.transform)
		{
			BoardSquare square = transform.GetComponent<BoardSquare>();
			square.ReevaluateSquare();
			if (square.height > 0 && square.height < m_lowestPositiveHeight)
			{
				m_lowestPositiveHeight = square.height;
			}
			if (square.height > m_maxHeight)
			{
				m_maxHeight = square.height;
			}
			if (square.x + 1 > m_maxX)
			{
				m_maxX = square.x + 1;
			}
			if (square.y + 1 > m_maxY)
			{
				m_maxY = square.y + 1;
			}
		}
		m_boardSquares = new BoardSquare[m_maxX, m_maxY];
		foreach (Transform transform in base.transform)
		{
			BoardSquare square = transform.GetComponent<BoardSquare>();
			if (square != null)
			{
				m_boardSquares[square.x, square.y] = square;
			}
		}
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_minimap.SetupMinimap();
		}
	}

	public void SetLOSVisualEffect(bool enable)
	{
		if (m_showLOS != enable)
		{
			m_showLOS = enable;
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				GameFlowData.Get().activeOwnedActorData.GetFogOfWar().SetVisibleShadeOfAllSquares();
			}
		}
	}

	public void ToggleLOS()
	{
		SetLOSVisualEffect(!m_showLOS);
	}

	public GameObject GetLOSHighlightsParent()
	{
		return m_LOSHighlightsParent;
	}

	public void ApplyForceOnDead(BoardSquare square, float amount, Vector3 overrideDir, bool applyToAllJoints)
	{
		if (square == null)
		{
			return;
		}
		foreach (GameObject player in GameFlowData.Get().GetPlayers())
		{
			if (player != null)
			{
				ActorData actorData = player.GetComponent<ActorData>();
				if (actorData != null && actorData.IsInRagdoll())
				{
					if (applyToAllJoints && actorData.GetActorModelData() != null)
					{
						ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(square.ToVector3() + 0.2f * Vector3.up, overrideDir);
						actorData.GetActorModelData().ApplyImpulseOnRagdoll(impulseInfo, null);
					}
					else
					{
						actorData.ApplyForceFromPoint(square.ToVector3(), amount, overrideDir);
					}
				}
			}
		}
	}

	public void SetThinCover(int x, int y, ActorCover.CoverDirections side, ThinCover.CoverType coverType)
	{
		m_boardSquares[x, y].SetThinCover(side, coverType);
	}

	public float GetHeightAt(int x, int y)
	{
		if (m_boardSquares != null
			&& x >= 0
			&& x < GetMaxX()
			&& y >= 0
			&& y < GetMaxY())
		{
			return m_boardSquares[x, y].height;
		}
		return 0f;
	}

	public float GetCameraGuideHeightAt(Vector3 worldPos, bool debugRay)
	{
		if (m_cameraGuideMeshCollider != null)
		{
			Ray ray = new Ray(worldPos + Vector3.up * m_maxHeight, Vector3.down);
			if (m_cameraGuideMeshCollider.Raycast(ray, out RaycastHit hitInfo, 5000f))
			{
				m_lastValidGuidedHeight = (int)hitInfo.point.y;
				if (debugRay)
				{
					Debug.DrawLine(ray.origin, hitInfo.point);
				}
			}
			else if (debugRay)
			{
				Debug.DrawLine(ray.origin, ray.origin + ray.direction * 5000f, new Color(1f, 0f, 0f));
			}
		}
		else
		{
			m_lastValidGuidedHeight = m_maxHeight;
		}
		if (m_lastValidGuidedHeight != 99999)  // != m_lastValidGuidedHeight?
		{
			return m_lastValidGuidedHeight;
		}
		return m_lowestPositiveHeight;
	}

	public int GetMaxX()
	{
		return m_maxX;
	}

	public int GetMaxY()
	{
		return m_maxY;
	}

	public BoardSquare GetSquareFromPos(float x, float y)
	{
		int xd = Mathf.RoundToInt(x / squareSize);
		int yd = Mathf.RoundToInt(y / squareSize);
		if (xd >= 0
			&& xd < GetMaxX()
			&& yd >= 0
			&& yd < GetMaxY())
		{
			return m_boardSquares[xd, yd];
		}
		return null;
	}

	public BoardSquare GetSquareClosestToPos(float x, float y)
	{
		int xd = Mathf.RoundToInt(x / squareSize);
		int yd = Mathf.RoundToInt(y / squareSize);
		xd = Mathf.Clamp(xd, 0, GetMaxX() - 1);
		yd = Mathf.Clamp(yd, 0, GetMaxY() - 1);
		return m_boardSquares[xd, yd];
	}

	public BoardSquare GetSquareFromVec3(Vector3 vec)
	{
		return GetSquareFromPos(vec.x, vec.z);
	}

	public BoardSquare GetSquareFromVec2(Vector2 vec)
	{
		return GetSquareFromPos(vec.x, vec.y);
	}

	public BoardSquare GetSquareFromTransform(Transform trans)
	{
		if (trans != null)
		{
			return GetSquareFromPos(trans.position.x, trans.position.z);
		}
		return null;
	}

	public BoardSquare GetSquareFromIndex(int x, int y)
	{
		if (x >= 0
			&& x < GetMaxX()
			&& y >= 0
			&& y < GetMaxY())
		{
			return m_boardSquares[x, y];
		}
		return null;
	}

	public BoardSquare GetSquare(GridPos pos)
	{
		if (pos.x >= 0
			&& pos.x < GetMaxX()
			&& pos.y >= 0
			&& pos.y < GetMaxY())
		{
			return m_boardSquares[pos.x, pos.y];
		}
		return null;
	}

	public void ResetGame()
	{
		ClearVisibleShade();
	}

	public void ClearVisibleShade()
	{
		bool anySquareShadeChanged = false;
		int lenX = m_boardSquares.GetLength(0);
		int lenY = m_boardSquares.GetLength(1);
		for (int i = 0; i < lenX; i++)
		{
			for (int j = 0; j < lenY; j++)
			{
				BoardSquare boardSquare = m_boardSquares[i, j];
				boardSquare.SetVisibleShade(0, ref anySquareShadeChanged);
			}
		}
		if (anySquareShadeChanged && GameEventManager.Get() != null)
		{
			GameEventManager.Get().FireEvent(GameEventManager.EventType.BoardSquareVisibleShadeChanged, null);
		}
	}

	public void GetCardinalAdjacentSquares(int x, int y, ref List<BoardSquare> output)
	{
		if (output == null)
		{
			output = new List<BoardSquare>(4);
		}
		if (GetSquareFromIndex(x + 1, y) != null)
		{
			output.Add(GetSquareFromIndex(x + 1, y));
		}
		if (GetSquareFromIndex(x - 1, y) != null)
		{
			output.Add(GetSquareFromIndex(x - 1, y));
		}
		if (GetSquareFromIndex(x, y + 1) != null)
		{
			output.Add(GetSquareFromIndex(x, y + 1));
		}
		if (GetSquareFromIndex(x, y - 1) != null)
		{
			output.Add(GetSquareFromIndex(x, y - 1));
		}
	}

	public void GetDiagonalAdjacentSquares(int x, int y, ref List<BoardSquare> output)
	{
		if (output == null)
		{
			output = new List<BoardSquare>(4);
		}
		if (GetSquareFromIndex(x + 1, y + 1) != null)
		{
			output.Add(GetSquareFromIndex(x + 1, y + 1));
		}
		if (GetSquareFromIndex(x + 1, y - 1) != null)
		{
			output.Add(GetSquareFromIndex(x + 1, y - 1));
		}
		if (GetSquareFromIndex(x - 1, y + 1) != null)
		{
			output.Add(GetSquareFromIndex(x - 1, y + 1));
		}
		if (GetSquareFromIndex(x - 1, y - 1) != null)
		{
			output.Add(GetSquareFromIndex(x - 1, y - 1));
		}
	}

	public void GetAllAdjacentSquares(int x, int y, ref List<BoardSquare> output)
	{
		if (output == null)
		{
			output = new List<BoardSquare>(8);
		}
		GetCardinalAdjacentSquares(x, y, ref output);
		GetDiagonalAdjacentSquares(x, y, ref output);
	}

	public BoardSquare GetClosestValidForGameplaySquareTo(float worldX, float worldY)
	{
		return GetClosestValidForGameplaySquareTo(GetSquareFromPos(worldX, worldY));
	}

	public BoardSquare GetClosestValidForGameplaySquareTo(BoardSquare bestSquare, BoardSquare excludeSquare = null)
	{
		if (bestSquare == null)
		{
			return null;
		}
		if (bestSquare.IsValidForGameplay()
			&& (excludeSquare == null || bestSquare != excludeSquare))
		{
			return bestSquare;
		}
		List<BoardSquare> adjacent = null;
		GetAllAdjacentSquares(bestSquare.x, bestSquare.y, ref adjacent);
		if (excludeSquare != null)
		{
			adjacent.Remove(excludeSquare);
		}
		adjacent.Sort(delegate (BoardSquare sq1, BoardSquare sq2)
		{
			float dist1Squared = (sq1.worldX - bestSquare.worldX) * (sq1.worldX - bestSquare.worldX)
				+ (sq1.worldY - bestSquare.worldY) * (sq1.worldY - bestSquare.worldY);
			float dist2Squared = (sq2.worldX - bestSquare.worldX) * (sq2.worldX - bestSquare.worldX)
				+ (sq2.worldY - bestSquare.worldY) * (sq2.worldY - bestSquare.worldY);
			return dist1Squared.CompareTo(dist2Squared);
		});
		foreach (BoardSquare square in adjacent)
		{
			if (square.IsValidForGameplay())
			{
				return square;
			}
		}
		return null;
	}

	public bool GetSquaresAreAdjacent(BoardSquare a, BoardSquare b)
	{
		return (a.x != b.x || a.y != b.y) &&
			a.x >= b.x - 1 && a.x <= b.x + 1 &&
			a.y >= b.y - 1 && a.y <= b.y + 1;
	}

	public bool GetSquaresAreCardinallyAdjacent(BoardSquare a, BoardSquare b)
	{
		return a.x == b.x && (a.y == b.y + 1 || a.y == b.y - 1)
			|| a.y == b.y && (a.x == b.x + 1 || a.x == b.x - 1);
	}

	public bool GetSquaresAreDiagonallyAdjacent(BoardSquare a, BoardSquare b)
	{
		return GetSquaresAreAdjacent(a, b) && a.x != b.x && a.y != b.y;
	}

	public List<BoardSquare> GetSquaresInBox(Bounds bounds, Func<BoardSquare, bool> validateFunc = null)
	{
		if (!Mathf.Approximately(bounds.center.y, 0f))
		{
			Log.Error("code error: Board.GetSquaresInBox bounds.center.y must be zero!");
		}

		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		min.y = 0f;
		max.y = 0f;
		int xStart = Mathf.Max(0, (int)(min.x / squareSize));
		int yStart = Mathf.Max(0, (int)(min.z / squareSize));
		int xEnd = Mathf.Min(m_maxX, (int)(max.x / squareSize) + 1);
		int yEnd = Mathf.Min(m_maxY, (int)(max.z / squareSize) + 1);

		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = xStart; i < xEnd; i++)
		{
			for (int j = yStart; j < yEnd; j++)
			{
				BoardSquare square = Get().GetSquareFromIndex(i, j);
				Vector3 point = new Vector3(square.worldX, 0f, square.worldY);
				if (bounds.Contains(point)
					&& (validateFunc == null || validateFunc(square)))
				{
					list.Add(square);
				}
			}
		}
		return list;
	}

	public List<BoardSquare> GetSquaresBoundedBy(BoardSquare square1, BoardSquare square2)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (square1 != null && square2 != null)
		{
			int xStart = Mathf.Min(square1.x, square2.x);
			int xEnd = Mathf.Max(square1.x, square2.x);
			int yStart = Mathf.Min(square1.y, square2.y);
			int yEnd = Mathf.Max(square1.y, square2.y);
			for (int i = yStart; i <= yEnd; i++)
			{
				for (int j = xStart; j <= xEnd; j++)
				{
					list.Add(Get().GetSquareFromIndex(j, i));
				}
			}
		}
		return list;
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(PlayerFreePos, 0.5f);
		if (PlayerFreeSquare != null)
		{
			Gizmos.DrawWireCube(PlayerFreeSquare.ToVector3(), new Vector3(1.7f, 1.7f, 1.7f));
		}
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(PlayerClampedPos, 0.4f);
		if (PlayerClampedSquare != null)
		{
			Gizmos.DrawWireCube(PlayerClampedSquare.ToVector3(), new Vector3(1.6f, 1.6f, 1.6f));
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(PlayerFreeCornerPos, 0.75f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(PlayerClampedCornerPos, 0.66f);
		DrawBoardGridGizmo();
	}

	private void DrawBoardGridGizmo()
	{
		if (m_maxX <= 0 || m_maxY <= 0)
		{
			return;
		}

		Color white = Color.white;
		white.a = 0.3f;
		Gizmos.color = white;
		BoardSquare square = Get().GetSquareFromIndex(m_maxX / 2, m_maxY / 2);
		if (square == null)
		{
			return;
		}
		int halfMaxX = m_maxX / 2;
		int halfMaxY = m_maxY / 2;
		Vector3 dirX = new Vector3(1f, 0f, 0f);
		Vector3 dirY = new Vector3(0f, 0f, 1f);
		float halfMaxDistX = (halfMaxX - 0.5f) * Get().squareSize;
		float halfMaxDistY = (halfMaxY - 0.5f) * Get().squareSize;
		Vector3 vector = square.ToVector3();
		vector.y = HighlightUtils.GetHighlightHeight();
		float num5 = vector.x - halfMaxDistX;
		for (int i = 0; i < halfMaxX * 2; i++)
		{
			Vector3 a3 = vector;
			a3.x = num5 + Get().squareSize * i;
			Vector3 b = dirY * halfMaxDistY;
			Gizmos.DrawLine(a3 + b, a3 - b);
		}
		float num6 = vector.z - halfMaxDistY;
		for (int j = 0; j < halfMaxY * 2; j++)
		{
			Vector3 a4 = vector;
			a4.z = num6 + Get().squareSize * j;
			Vector3 b2 = dirX * halfMaxDistX;
			Gizmos.DrawLine(a4 + b2, a4 - b2);
		}
	}
}
