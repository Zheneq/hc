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

	private int m_lowestPositiveHeight = 0x270F;

	private int m_lastValidGuidedHeight = 0x1869F;

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
			return Board.s_squareSizeStatic;
		}
		private set
		{
			Board.s_squareSizeStatic = value;
		}
	}

	public float squareSize
	{
		get
		{
			return this.m_squareSize;
		}
	}

	public int BaselineHeight
	{
		get
		{
			if (this.m_baselineHeight >= 0)
			{
				return this.m_baselineHeight;
			}
			return this.m_lowestPositiveHeight;
		}
	}

	public float LosCheckHeight
	{
		get
		{
			return (float)this.BaselineHeight + BoardSquare.s_LoSHeightOffset;
		}
	}

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

	public static Board Get()
	{
		if (Board.s_board == null)
		{
			if (Application.isEditor && !Application.isPlaying)
			{
				Board.s_board = UnityEngine.Object.FindObjectOfType<Board>();
				if (Board.s_board != null)
				{
					Board.s_board.ReevaluateBoard();
				}
			}
		}
		return Board.s_board;
	}

	private void Awake()
	{
		base.enabled = false;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameFlowDataStarted);
		Board.s_board = this;
		if (this.m_LOSHighlightsParent == null)
		{
			this.m_LOSHighlightsParent = GameObject.Find("LOSHighlights");
		}
		if (this.m_LOSHighlightsParent != null)
		{
			this.m_LOSHighlightsParent.layer = LayerMask.NameToLayer("FogOfWar");
			this.m_LOSHighlightsParent.SetActive(true);
		}
		if (this.m_LOSHighlightsParent != null)
		{
			if (!NetworkClient.active)
			{
				if (NetworkServer.active)
				{
					UnityEngine.Object.DestroyImmediate(this.m_LOSHighlightsParent);
					this.m_LOSHighlightsParent = null;
				}
			}
		}
		GameObject gameObject = GameObject.Find("Camera Guide Mesh");
		if (gameObject)
		{
			this.m_cameraGuideMeshCollider = gameObject.GetComponent<MeshCollider>();
		}
		this.ReevaluateBoard();
		this.m_showLOS = true;
		Board.s_squareSizeStatic = this.m_squareSize;
		Board.BaselineHeightStatic = this.BaselineHeight;
		this.m_normalPathBuildScratchPool = new BuildNormalPathNodePool();
		this.m_normalPathNodeHeap = new BuildNormalPathHeap(0x3C);
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameFlowDataStarted);
		}
		this.m_normalPathBuildScratchPool = null;
		Board.s_board = null;
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
		Camera main = Camera.main;
		ActorData actorData = null;
		if (GameFlowData.Get() != null)
		{
			actorData = GameFlowData.Get().activeOwnedActorData;
		}
		if (main)
		{
			Vector3 position = main.transform.position;
			Vector3 mousePosition = Input.mousePosition;
			Vector3 direction = main.ScreenPointToRay(mousePosition).direction;
			Vector3 up = Vector3.up;
			float d = ((float)this.m_baselineHeight - position.y) / Vector3.Dot(direction, up);
			this.PlayerMouseIntersectionPos = position + direction * d;
			if (actorData)
			{
				Vector3 position2 = actorData.transform.position;
				this.PlayerMouseLookDir = (this.PlayerMouseIntersectionPos - position2).normalized;
			}
			bool flag;
			if (ControlpadGameplay.Get() != null && ControlpadGameplay.Get().UsingControllerInput)
			{
				flag = false;
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				this.PlayerLookDir = this.PlayerMouseLookDir;
				this.PlayerFreePos = this.PlayerMouseIntersectionPos;
			}
			else
			{
				this.PlayerLookDir = ControlpadGameplay.Get().ControllerAimDir;
				this.PlayerFreePos = ControlpadGameplay.Get().ControllerAimPos;
			}
			this.PlayerFreeSquare = this.GetBoardSquare(this.PlayerFreePos);
			this.PlayerFreeCornerPos = Board.symbol_000E(this.PlayerFreePos, this.PlayerFreeSquare);
			this.RecalcClampedSelections();
			HighlightUtils.Get().UpdateCursorPositions();
			HighlightUtils.Get().UpdateRangeIndicatorHighlight();
			HighlightUtils.Get().UpdateMouseoverCoverHighlight();
			HighlightUtils.Get().UpdateShowAffectedSquareFlag();
		}
		if (Input.GetMouseButtonUp(2))
		{
			bool applyToAllJoints = false;
			float amount = 300f;
			this.ApplyForceOnDead(this.PlayerFreeSquare, amount, new Vector3(0f, 1f, 0f), applyToAllJoints);
		}
	}

	public static Vector3 symbol_000E(Vector3 symbol_001D, BoardSquare symbol_000E)
	{
		float num = Board.SquareSizeStatic / 2f;
		float x;
		float z;
		if (symbol_000E == null)
		{
			x = symbol_001D.x;
			z = symbol_001D.z;
		}
		else
		{
			float worldX = symbol_000E.worldX;
			if (symbol_001D.x > worldX)
			{
				x = worldX + num;
			}
			else
			{
				x = worldX - num;
			}
			float worldY = symbol_000E.worldY;
			if (symbol_001D.z > worldY)
			{
				z = worldY + num;
			}
			else
			{
				z = worldY - num;
			}
		}
		Vector3 result = new Vector3(x, symbol_001D.y, z);
		return result;
	}

	public bool MarkedForUpdateValidSquares
	{
		get
		{
			return this.m_needToUpdateValidSquares;
		}
	}

	public void MarkForUpdateValidSquares(bool value = true)
	{
		this.m_needToUpdateValidSquares = value;
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
		ActorController component = activeOwnedActorData.GetComponent<ActorController>();
		if (component == null)
		{
			return;
		}
		component.RecalcAndHighlightValidSquares();
		HashSet<BoardSquare> squaresToClampTo = component.GetSquaresToClampTo();
		Vector3 vector = Vector3.zero;
		BoardSquare boardSquare = null;
		bool flag;
		if (activeOwnedActorData.GetActorTurnSM().AmDecidingMovement())
		{
			if (this.PlayerFreeSquare != null)
			{
				if (this.PlayerFreeSquare.occupant != null)
				{
					ActorData component2 = this.PlayerFreeSquare.occupant.GetComponent<ActorData>();
					if (component2 != null)
					{
						if (component2.IsVisibleToClient())
						{
							flag = true;
							goto IL_109;
						}
					}
					flag = false;
					IL_109:
					goto IL_10E;
				}
			}
		}
		flag = false;
		IL_10E:
		if (squaresToClampTo != null && squaresToClampTo.Count != 0)
		{
			if (!squaresToClampTo.Contains(this.PlayerFreeSquare))
			{
				if (!flag)
				{
					float x = this.PlayerFreePos.x;
					float z = this.PlayerFreePos.z;
					float num = float.MaxValue;
					using (HashSet<BoardSquare>.Enumerator enumerator = squaresToClampTo.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare boardSquare2 = enumerator.Current;
							float num2 = boardSquare2.worldX - x;
							float num3 = boardSquare2.worldY - z;
							float num4 = num2 * num2 + num3 * num3;
							if (num4 <= num)
							{
								num = num4;
								boardSquare = boardSquare2;
							}
						}
					}
					if (boardSquare != null)
					{
						vector = boardSquare.CalcNearestPositionOnSquareEdge(this.PlayerFreePos);
						goto IL_21F;
					}
					goto IL_21F;
				}
			}
		}
		vector = this.PlayerFreePos;
		boardSquare = this.PlayerFreeSquare;
		IL_21F:
		this.PlayerClampedPos = vector;
		this.PlayerClampedSquare = boardSquare;
		this.PlayerClampedCornerPos = Board.symbol_000E(vector, boardSquare);
	}

	public void ReevaluateBoard()
	{
		this.m_maxX = 0;
		this.m_maxY = 0;
		this.m_maxHeight = 0;
		this.m_lastValidGuidedHeight = 0x1869F;
		this.m_lowestPositiveHeight = 0x1869F;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				BoardSquare component = transform.GetComponent<BoardSquare>();
				component.ReevaluateSquare();
				if (component.height > 0 && component.height < this.m_lowestPositiveHeight)
				{
					this.m_lowestPositiveHeight = component.height;
				}
				if (component.height > this.m_maxHeight)
				{
					this.m_maxHeight = component.height;
				}
				if (component.x + 1 > this.m_maxX)
				{
					this.m_maxX = component.x + 1;
				}
				if (component.y + 1 > this.m_maxY)
				{
					this.m_maxY = component.y + 1;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		this.m_boardSquares = new BoardSquare[this.m_maxX, this.m_maxY];
		IEnumerator enumerator2 = base.transform.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				Transform transform2 = (Transform)obj2;
				BoardSquare component2 = transform2.GetComponent<BoardSquare>();
				if (component2)
				{
					this.m_boardSquares[component2.x, component2.y] = component2;
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_minimap.SetupMinimap();
		}
	}

	public void SetLOSVisualEffect(bool enable)
	{
		if (this.m_showLOS != enable)
		{
			this.m_showLOS = enable;
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				GameFlowData.Get().activeOwnedActorData.GetFogOfWar().SetVisibleShadeOfAllSquares();
			}
		}
	}

	public void ToggleLOS()
	{
		this.SetLOSVisualEffect(!this.m_showLOS);
	}

	public GameObject GetLOSHighlightsParent()
	{
		return this.m_LOSHighlightsParent;
	}

	public void ApplyForceOnDead(BoardSquare square, float amount, Vector3 overrideDir, bool applyToAllJoints)
	{
		if (square != null)
		{
			List<GameObject> players = GameFlowData.Get().GetPlayers();
			foreach (GameObject gameObject in players)
			{
				if (gameObject != null)
				{
					ActorData component = gameObject.GetComponent<ActorData>();
					if (component)
					{
						if (component.IsModelAnimatorDisabled())
						{
							if (applyToAllJoints)
							{
								if (component.GetActorModelData() != null)
								{
									ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(square.ToVector3() + 0.2f * Vector3.up, overrideDir);
									component.GetActorModelData().ApplyImpulseOnRagdoll(impulseInfo, null);
									continue;
								}
							}
							component.ApplyForceFromPoint(square.ToVector3(), amount, overrideDir);
						}
					}
				}
			}
		}
	}

	public void SetThinCover(int x, int y, ActorCover.CoverDirections side, ThinCover.CoverType coverType)
	{
		this.m_boardSquares[x, y].SetThinCover(side, coverType);
	}

	public float GetSquareHeight(int x, int y)
	{
		float result = 0f;
		if (this.m_boardSquares != null)
		{
			if (x >= 0)
			{
				if (x < this.GetMaxX())
				{
					if (y >= 0)
					{
						if (y < this.GetMaxY())
						{
							result = (float)this.m_boardSquares[x, y].height;
						}
					}
				}
			}
		}
		return result;
	}

	public float symbol_000E(Vector3 symbol_001D, bool drawDebug)
	{
		if (this.m_cameraGuideMeshCollider)
		{
			RaycastHit raycastHit = default(RaycastHit);
			Ray ray = new Ray(symbol_001D + Vector3.up * (float)this.m_maxHeight, Vector3.down);
			if (this.m_cameraGuideMeshCollider.Raycast(ray, out raycastHit, 5000f))
			{
				this.m_lastValidGuidedHeight = (int)raycastHit.point.y;
				if (drawDebug)
				{
					Debug.DrawLine(ray.origin, raycastHit.point);
				}
			}
			else if (drawDebug)
			{
				Debug.DrawLine(ray.origin, ray.origin + ray.direction * 5000f, new Color(1f, 0f, 0f));
			}
		}
		else
		{
			this.m_lastValidGuidedHeight = this.m_maxHeight;
		}
		if (this.m_lastValidGuidedHeight != 0x1869F)
		{
			return (float)this.m_lastValidGuidedHeight;
		}
		return (float)this.m_lowestPositiveHeight;
	}

	public int GetMaxX()
	{
		return this.m_maxX;
	}

	public int GetMaxY()
	{
		return this.m_maxY;
	}

	public BoardSquare GetBoardSquareSafe(float x, float y)
	{
		BoardSquare result = null;
		int num = Mathf.RoundToInt(x / this.squareSize);
		int num2 = Mathf.RoundToInt(y / this.squareSize);
		if (num >= 0)
		{
			if (num < this.GetMaxX() && num2 >= 0)
			{
				if (num2 < this.GetMaxY())
				{
					result = this.m_boardSquares[num, num2];
				}
			}
		}
		return result;
	}

	public BoardSquare GetBoardSquareUnsafe(float x, float y)
	{
		int num = Mathf.RoundToInt(x / this.squareSize);
		int num2 = Mathf.RoundToInt(y / this.squareSize);
		num = Mathf.Clamp(num, 0, this.GetMaxX() - 1);
		num2 = Mathf.Clamp(num2, 0, this.GetMaxY() - 1);
		return this.m_boardSquares[num, num2];
	}

	public BoardSquare GetBoardSquare(Vector3 vector2D)
	{
		return this.GetBoardSquareSafe(vector2D.x, vector2D.z);
	}

	public BoardSquare GetBoardSquare(Vector2 vector)
	{
		return this.GetBoardSquareSafe(vector.x, vector.y);
	}

	public BoardSquare GetBoardSquare(Transform transform)
	{
		BoardSquare result = null;
		if (transform != null)
		{
			result = this.GetBoardSquareSafe(transform.position.x, transform.position.z);
		}
		return result;
	}

	public BoardSquare GetBoardSquare(int x, int y)
	{
		BoardSquare result = null;
		if (x >= 0)
		{
			if (x < this.GetMaxX())
			{
				if (y >= 0)
				{
					if (y < this.GetMaxY())
					{
						result = this.m_boardSquares[x, y];
					}
				}
			}
		}
		return result;
	}

	public BoardSquare GetBoardSquareSafe(GridPos gridPos)
	{
		BoardSquare result = null;
		if (gridPos.x >= 0)
		{
			if (gridPos.x < this.GetMaxX())
			{
				if (gridPos.y >= 0)
				{
					if (gridPos.y < this.GetMaxY())
					{
						result = this.m_boardSquares[gridPos.x, gridPos.y];
					}
				}
			}
		}
		return result;
	}

	public void ResetGame()
	{
		this.ClearVisibleShade();
	}

	public void ClearVisibleShade()
	{
		bool flag = false;
		BoardSquare[,] boardSquares = this.m_boardSquares;
		int length = boardSquares.GetLength(0);
		int length2 = boardSquares.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				BoardSquare boardSquare = boardSquares[i, j];
				boardSquare.SetVisibleShade(0, ref flag);
			}
		}
		if (flag)
		{
			if (GameEventManager.Get() != null)
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.BoardSquareVisibleShadeChanged, null);
			}
		}
	}

	public unsafe void GetStraightAdjacentSquares(int x, int y, ref List<BoardSquare> result)
	{
		if (result == null)
		{
			result = new List<BoardSquare>(4);
		}
		if (this.GetBoardSquare(x + 1, y) != null)
		{
			result.Add(this.GetBoardSquare(x + 1, y));
		}
		if (this.GetBoardSquare(x - 1, y) != null)
		{
			result.Add(this.GetBoardSquare(x - 1, y));
		}
		if (this.GetBoardSquare(x, y + 1) != null)
		{
			result.Add(this.GetBoardSquare(x, y + 1));
		}
		if (this.GetBoardSquare(x, y - 1) != null)
		{
			result.Add(this.GetBoardSquare(x, y - 1));
		}
	}

	public unsafe void GetDiagonallyAdjacentSquares(int x, int y, ref List<BoardSquare> result)
	{
		if (result == null)
		{
			result = new List<BoardSquare>(4);
		}
		if (this.GetBoardSquare(x + 1, y + 1) != null)
		{
			result.Add(this.GetBoardSquare(x + 1, y + 1));
		}
		if (this.GetBoardSquare(x + 1, y - 1) != null)
		{
			result.Add(this.GetBoardSquare(x + 1, y - 1));
		}
		if (this.GetBoardSquare(x - 1, y + 1) != null)
		{
			result.Add(this.GetBoardSquare(x - 1, y + 1));
		}
		if (this.GetBoardSquare(x - 1, y - 1) != null)
		{
			result.Add(this.GetBoardSquare(x - 1, y - 1));
		}
	}

	public void GetAllAdjacentSquares(int x, int y, ref List<BoardSquare> result)
	{
		if (result == null)
		{
			result = new List<BoardSquare>(8);
		}
		this.GetStraightAdjacentSquares(x, y, ref result);
		this.GetDiagonallyAdjacentSquares(x, y, ref result);
	}

	public BoardSquare symbol_0013(float symbol_001D, float symbol_000E)
	{
		BoardSquare boardSquareSafe = this.GetBoardSquareSafe(symbol_001D, symbol_000E);
		return this.symbol_0018(boardSquareSafe, null);
	}

	public BoardSquare symbol_0018(BoardSquare symbol_001D, BoardSquare symbol_000E = null)
	{
		BoardSquare result = null;
		if (symbol_001D != null)
		{
			bool flag = symbol_001D == symbol_000E;
			if (symbol_001D.IsBaselineHeight())
			{
				if (!(symbol_000E == null))
				{
					if (flag)
					{
						goto IL_66;
					}
				}
				return symbol_001D;
			}
			IL_66:
			List<BoardSquare> list = null;
			this.GetAllAdjacentSquares(symbol_001D.x, symbol_001D.y, ref list);
			if (symbol_000E != null)
			{
				list.Remove(symbol_000E);
			}
			float worldX = symbol_001D.worldX;
			float worldY = symbol_001D.worldY;
			list.Sort(delegate(BoardSquare sq1, BoardSquare sq2)
			{
				float num = (sq1.worldX - worldX) * (sq1.worldX - worldX) + (sq1.worldY - worldY) * (sq1.worldY - worldY);
				float value = (sq2.worldX - worldX) * (sq2.worldX - worldX) + (sq2.worldY - worldY) * (sq2.worldY - worldY);
				return num.CompareTo(value);
			});
			using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare boardSquare = enumerator.Current;
					if (boardSquare.IsBaselineHeight())
					{
						return boardSquare;
					}
				}
			}
		}
		return result;
	}

	public bool symbol_000E(BoardSquare symbol_001D, BoardSquare symbol_000E)
	{
		bool flag;
		if (symbol_001D.x == symbol_000E.x)
		{
			flag = (symbol_001D.y != symbol_000E.y);
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		bool flag3 = symbol_001D.x >= symbol_000E.x - 1 && symbol_001D.x <= symbol_000E.x + 1;
		bool flag4;
		if (symbol_001D.y >= symbol_000E.y - 1)
		{
			flag4 = (symbol_001D.y <= symbol_000E.y + 1);
		}
		else
		{
			flag4 = false;
		}
		bool result = flag4;
		if (flag2)
		{
			if (flag3)
			{
				return result;
			}
		}
		return false;
	}

	public bool symbol_0012(BoardSquare symbol_001D, BoardSquare symbol_000E)
	{
		if (symbol_001D.x == symbol_000E.x)
		{
			if (symbol_001D.y != symbol_000E.y + 1)
			{
				if (symbol_001D.y != symbol_000E.y - 1)
				{
					goto IL_5D;
				}
			}
			return true;
		}
		IL_5D:
		if (symbol_001D.y == symbol_000E.y)
		{
			if (symbol_001D.x != symbol_000E.x + 1)
			{
				if (symbol_001D.x != symbol_000E.x - 1)
				{
					goto IL_AD;
				}
			}
			return true;
		}
		IL_AD:
		return false;
	}

	public bool symbol_0015(BoardSquare symbol_001D, BoardSquare symbol_000E)
	{
		bool flag = this.symbol_000E(symbol_001D, symbol_000E);
		return flag && symbol_001D.x != symbol_000E.x && symbol_001D.y != symbol_000E.y;
	}

	public List<BoardSquare> symbol_000E(Bounds symbol_001D, Func<BoardSquare, bool> symbol_000E = null)
	{
		if (!Mathf.Approximately(symbol_001D.center.y, 0f))
		{
			Log.Error("code error: Board.GetSquaresInBox bounds.center.y must be zero!", new object[0]);
		}
		Vector3 min = symbol_001D.min;
		Vector3 max = symbol_001D.max;
		min.y = 0f;
		max.y = 0f;
		int num = Mathf.Max(0, (int)(min.x / this.squareSize));
		int num2 = Mathf.Max(0, (int)(min.z / this.squareSize));
		int num3 = Mathf.Min(this.m_maxX, (int)(max.x / this.squareSize) + 1);
		int num4 = Mathf.Min(this.m_maxY, (int)(max.z / this.squareSize) + 1);
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = num; i < num3; i++)
		{
			for (int j = num2; j < num4; j++)
			{
				BoardSquare boardSquare = Board.Get().GetBoardSquare(i, j);
				Vector3 point = new Vector3(boardSquare.worldX, 0f, boardSquare.worldY);
				if (symbol_001D.Contains(point))
				{
					if (symbol_000E != null)
					{
						if (!symbol_000E(boardSquare))
						{
							goto IL_15F;
						}
					}
					list.Add(boardSquare);
				}
				IL_15F:;
			}
		}
		return list;
	}

	public List<BoardSquare> GetSquaresInRect(BoardSquare a, BoardSquare b)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (a != null)
		{
			if (b != null)
			{
				int num = Mathf.Min(a.x, b.x);
				int num2 = Mathf.Max(a.x, b.x);
				int num3 = Mathf.Min(a.y, b.y);
				int num4 = Mathf.Max(a.y, b.y);
				for (int i = num3; i <= num4; i++)
				{
					for (int j = num; j <= num2; j++)
					{
						BoardSquare boardSquare = Board.Get().GetBoardSquare(j, i);
						list.Add(boardSquare);
					}
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
		Gizmos.DrawWireSphere(this.PlayerFreePos, 0.5f);
		if (this.PlayerFreeSquare != null)
		{
			Gizmos.DrawWireCube(this.PlayerFreeSquare.ToVector3(), new Vector3(1.7f, 1.7f, 1.7f));
		}
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(this.PlayerClampedPos, 0.4f);
		if (this.PlayerClampedSquare != null)
		{
			Gizmos.DrawWireCube(this.PlayerClampedSquare.ToVector3(), new Vector3(1.6f, 1.6f, 1.6f));
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(this.PlayerFreeCornerPos, 0.75f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.PlayerClampedCornerPos, 0.66f);
		this.DrawBoardGridGizmo();
	}

	private void DrawBoardGridGizmo()
	{
		if (this.m_maxX > 0)
		{
			if (this.m_maxY > 0)
			{
				Color white = Color.white;
				white.a = 0.3f;
				Gizmos.color = white;
				BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_maxX / 2, this.m_maxY / 2);
				if (boardSquare != null)
				{
					int num = this.m_maxX / 2;
					int num2 = this.m_maxY / 2;
					float squareSize = Board.Get().squareSize;
					Vector3 a = new Vector3(1f, 0f, 0f);
					Vector3 a2 = new Vector3(0f, 0f, 1f);
					float num3 = ((float)num - 0.5f) * squareSize;
					float num4 = ((float)num2 - 0.5f) * squareSize;
					Vector3 vector = boardSquare.ToVector3();
					vector.y = HighlightUtils.GetHighlightHeight();
					float num5 = vector.x - num3;
					for (int i = 0; i < num * 2; i++)
					{
						Vector3 a3 = vector;
						a3.x = num5 + squareSize * (float)i;
						Vector3 b = a2 * num4;
						Gizmos.DrawLine(a3 + b, a3 - b);
					}
					float num6 = vector.z - num4;
					for (int j = 0; j < num2 * 2; j++)
					{
						Vector3 a4 = vector;
						a4.z = num6 + squareSize * (float)j;
						Vector3 b2 = a * num3;
						Gizmos.DrawLine(a4 + b2, a4 - b2);
					}
				}
			}
		}
	}
}
