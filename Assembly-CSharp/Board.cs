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

	public static Board \u000E()
	{
		if (Board.s_board == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u000E()).MethodHandle;
			}
			if (Application.isEditor && !Application.isPlaying)
			{
				Board.s_board = UnityEngine.Object.FindObjectOfType<Board>();
				if (Board.s_board != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.Awake()).MethodHandle;
			}
			this.m_LOSHighlightsParent.layer = LayerMask.NameToLayer("FogOfWar");
			this.m_LOSHighlightsParent.SetActive(true);
		}
		if (this.m_LOSHighlightsParent != null)
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
			if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.OnDestroy()).MethodHandle;
			}
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameFlowDataStarted);
		}
		this.m_normalPathBuildScratchPool = null;
		Board.s_board = null;
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GameFlowDataStarted)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.IGameEventListener.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.Update()).MethodHandle;
			}
			Vector3 position = main.transform.position;
			Vector3 mousePosition = Input.mousePosition;
			Vector3 direction = main.ScreenPointToRay(mousePosition).direction;
			Vector3 up = Vector3.up;
			float d = ((float)this.m_baselineHeight - position.y) / Vector3.Dot(direction, up);
			this.PlayerMouseIntersectionPos = position + direction * d;
			if (actorData)
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
				Vector3 position2 = actorData.transform.position;
				this.PlayerMouseLookDir = (this.PlayerMouseIntersectionPos - position2).normalized;
			}
			bool flag;
			if (ControlpadGameplay.Get() != null && ControlpadGameplay.Get().UsingControllerInput)
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
			this.PlayerFreeSquare = this.\u000E(this.PlayerFreePos);
			this.PlayerFreeCornerPos = Board.\u000E(this.PlayerFreePos, this.PlayerFreeSquare);
			this.RecalcClampedSelections();
			HighlightUtils.Get().UpdateCursorPositions();
			HighlightUtils.Get().UpdateRangeIndicatorHighlight();
			HighlightUtils.Get().UpdateMouseoverCoverHighlight();
			HighlightUtils.Get().UpdateShowAffectedSquareFlag();
		}
		if (Input.GetMouseButtonUp(2))
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
			bool applyToAllJoints = false;
			float amount = 300f;
			this.ApplyForceOnDead(this.PlayerFreeSquare, amount, new Vector3(0f, 1f, 0f), applyToAllJoints);
		}
	}

	public static Vector3 \u000E(Vector3 \u001D, BoardSquare \u000E)
	{
		float num = Board.SquareSizeStatic / 2f;
		float x;
		float z;
		if (\u000E == null)
		{
			x = \u001D.x;
			z = \u001D.z;
		}
		else
		{
			float worldX = \u000E.worldX;
			if (\u001D.x > worldX)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u000E(Vector3, BoardSquare)).MethodHandle;
				}
				x = worldX + num;
			}
			else
			{
				x = worldX - num;
			}
			float worldY = \u000E.worldY;
			if (\u001D.z > worldY)
			{
				z = worldY + num;
			}
			else
			{
				z = worldY - num;
			}
		}
		Vector3 result = new Vector3(x, \u001D.y, z);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.RecalcClampedSelections()).MethodHandle;
			}
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
		if (activeOwnedActorData.\u000E().AmDecidingMovement())
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
			if (this.PlayerFreeSquare != null)
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
				if (this.PlayerFreeSquare.occupant != null)
				{
					ActorData component2 = this.PlayerFreeSquare.occupant.GetComponent<ActorData>();
					if (component2 != null)
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
						if (component2.\u0018())
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!squaresToClampTo.Contains(this.PlayerFreeSquare))
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
					if (boardSquare != null)
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
		this.PlayerClampedCornerPos = Board.\u000E(vector, boardSquare);
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(Board.ReevaluateBoard()).MethodHandle;
					}
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
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_maxY = component.y + 1;
				}
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
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_boardSquares[component2.x, component2.y] = component2;
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
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
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
				disposable2.Dispose();
			}
		}
		if (HUD_UI.Get() != null)
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
			HUD_UI.Get().m_mainScreenPanel.m_minimap.SetupMinimap();
		}
	}

	public void SetLOSVisualEffect(bool enable)
	{
		if (this.m_showLOS != enable)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.SetLOSVisualEffect(bool)).MethodHandle;
			}
			this.m_showLOS = enable;
			if (GameFlowData.Get().activeOwnedActorData != null)
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
				GameFlowData.Get().activeOwnedActorData.\u000E().SetVisibleShadeOfAllSquares();
			}
		}
	}

	public void ToggleLOS()
	{
		this.SetLOSVisualEffect(!this.m_showLOS);
	}

	public GameObject \u000E()
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(Board.ApplyForceOnDead(BoardSquare, float, Vector3, bool)).MethodHandle;
					}
					ActorData component = gameObject.GetComponent<ActorData>();
					if (component)
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
						if (component.\u0012())
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
							if (applyToAllJoints)
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
								if (component.\u000E() != null)
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
									ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(square.ToVector3() + 0.2f * Vector3.up, overrideDir);
									component.\u000E().ApplyImpulseOnRagdoll(impulseInfo, null);
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

	public float \u000E(int \u001D, int \u000E)
	{
		float result = 0f;
		if (this.m_boardSquares != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u000E(int, int)).MethodHandle;
			}
			if (\u001D >= 0)
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
				if (\u001D < this.\u000E())
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
					if (\u000E >= 0)
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
						if (\u000E < this.\u0012())
						{
							result = (float)this.m_boardSquares[\u001D, \u000E].height;
						}
					}
				}
			}
		}
		return result;
	}

	public float \u000E(Vector3 \u001D, bool \u000E)
	{
		if (this.m_cameraGuideMeshCollider)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u000E(Vector3, bool)).MethodHandle;
			}
			RaycastHit raycastHit = default(RaycastHit);
			Ray ray = new Ray(\u001D + Vector3.up * (float)this.m_maxHeight, Vector3.down);
			if (this.m_cameraGuideMeshCollider.Raycast(ray, out raycastHit, 5000f))
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
				this.m_lastValidGuidedHeight = (int)raycastHit.point.y;
				if (\u000E)
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
					Debug.DrawLine(ray.origin, raycastHit.point);
				}
			}
			else if (\u000E)
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
				Debug.DrawLine(ray.origin, ray.origin + ray.direction * 5000f, new Color(1f, 0f, 0f));
			}
		}
		else
		{
			this.m_lastValidGuidedHeight = this.m_maxHeight;
		}
		if (this.m_lastValidGuidedHeight != 0x1869F)
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
			return (float)this.m_lastValidGuidedHeight;
		}
		return (float)this.m_lowestPositiveHeight;
	}

	public int \u000E()
	{
		return this.m_maxX;
	}

	public int \u0012()
	{
		return this.m_maxY;
	}

	public BoardSquare \u0012(float \u001D, float \u000E)
	{
		BoardSquare result = null;
		int num = Mathf.RoundToInt(\u001D / this.squareSize);
		int num2 = Mathf.RoundToInt(\u000E / this.squareSize);
		if (num >= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u0012(float, float)).MethodHandle;
			}
			if (num < this.\u000E() && num2 >= 0)
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
				if (num2 < this.\u0012())
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
					result = this.m_boardSquares[num, num2];
				}
			}
		}
		return result;
	}

	public BoardSquare \u0015(float \u001D, float \u000E)
	{
		int num = Mathf.RoundToInt(\u001D / this.squareSize);
		int num2 = Mathf.RoundToInt(\u000E / this.squareSize);
		num = Mathf.Clamp(num, 0, this.\u000E() - 1);
		num2 = Mathf.Clamp(num2, 0, this.\u0012() - 1);
		return this.m_boardSquares[num, num2];
	}

	public BoardSquare \u000E(Vector3 \u001D)
	{
		return this.\u0012(\u001D.x, \u001D.z);
	}

	public BoardSquare \u000E(Vector2 \u001D)
	{
		return this.\u0012(\u001D.x, \u001D.y);
	}

	public BoardSquare \u000E(Transform \u001D)
	{
		BoardSquare result = null;
		if (\u001D != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u000E(Transform)).MethodHandle;
			}
			result = this.\u0012(\u001D.position.x, \u001D.position.z);
		}
		return result;
	}

	public BoardSquare \u0016(int \u001D, int \u000E)
	{
		BoardSquare result = null;
		if (\u001D >= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u0016(int, int)).MethodHandle;
			}
			if (\u001D < this.\u000E())
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
				if (\u000E >= 0)
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
					if (\u000E < this.\u0012())
					{
						result = this.m_boardSquares[\u001D, \u000E];
					}
				}
			}
		}
		return result;
	}

	public BoardSquare \u000E(GridPos \u001D)
	{
		BoardSquare result = null;
		if (\u001D.x >= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u000E(GridPos)).MethodHandle;
			}
			if (\u001D.x < this.\u000E())
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
				if (\u001D.y >= 0)
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
					if (\u001D.y < this.\u0012())
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
						result = this.m_boardSquares[\u001D.x, \u001D.y];
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.ClearVisibleShade()).MethodHandle;
			}
			if (GameEventManager.Get() != null)
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.BoardSquareVisibleShadeChanged, null);
			}
		}
	}

	public unsafe void \u000E(int \u001D, int \u000E, ref List<BoardSquare> \u0012)
	{
		if (\u0012 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u000E(int, int, List<BoardSquare>*)).MethodHandle;
			}
			\u0012 = new List<BoardSquare>(4);
		}
		if (this.\u0016(\u001D + 1, \u000E) != null)
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
			\u0012.Add(this.\u0016(\u001D + 1, \u000E));
		}
		if (this.\u0016(\u001D - 1, \u000E) != null)
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
			\u0012.Add(this.\u0016(\u001D - 1, \u000E));
		}
		if (this.\u0016(\u001D, \u000E + 1) != null)
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
			\u0012.Add(this.\u0016(\u001D, \u000E + 1));
		}
		if (this.\u0016(\u001D, \u000E - 1) != null)
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
			\u0012.Add(this.\u0016(\u001D, \u000E - 1));
		}
	}

	public unsafe void \u0012(int \u001D, int \u000E, ref List<BoardSquare> \u0012)
	{
		if (\u0012 == null)
		{
			\u0012 = new List<BoardSquare>(4);
		}
		if (this.\u0016(\u001D + 1, \u000E + 1) != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u0012(int, int, List<BoardSquare>*)).MethodHandle;
			}
			\u0012.Add(this.\u0016(\u001D + 1, \u000E + 1));
		}
		if (this.\u0016(\u001D + 1, \u000E - 1) != null)
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
			\u0012.Add(this.\u0016(\u001D + 1, \u000E - 1));
		}
		if (this.\u0016(\u001D - 1, \u000E + 1) != null)
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
			\u0012.Add(this.\u0016(\u001D - 1, \u000E + 1));
		}
		if (this.\u0016(\u001D - 1, \u000E - 1) != null)
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
			\u0012.Add(this.\u0016(\u001D - 1, \u000E - 1));
		}
	}

	public void \u0015(int \u001D, int \u000E, ref List<BoardSquare> \u0012)
	{
		if (\u0012 == null)
		{
			\u0012 = new List<BoardSquare>(8);
		}
		this.\u000E(\u001D, \u000E, ref \u0012);
		this.\u0012(\u001D, \u000E, ref \u0012);
	}

	public BoardSquare \u0013(float \u001D, float \u000E)
	{
		BoardSquare u001D = this.\u0012(\u001D, \u000E);
		return this.\u0018(u001D, null);
	}

	public BoardSquare \u0018(BoardSquare \u001D, BoardSquare \u000E = null)
	{
		BoardSquare result = null;
		if (\u001D != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u0018(BoardSquare, BoardSquare)).MethodHandle;
			}
			bool flag = \u001D == \u000E;
			if (\u001D.\u0016())
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
				if (!(\u000E == null))
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
					if (flag)
					{
						goto IL_66;
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
				return \u001D;
			}
			IL_66:
			List<BoardSquare> list = null;
			this.\u0015(\u001D.x, \u001D.y, ref list);
			if (\u000E != null)
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
				list.Remove(\u000E);
			}
			float worldX = \u001D.worldX;
			float worldY = \u001D.worldY;
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
					if (boardSquare.\u0016())
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
						return boardSquare;
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
		}
		return result;
	}

	public bool \u000E(BoardSquare \u001D, BoardSquare \u000E)
	{
		bool flag;
		if (\u001D.x == \u000E.x)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u000E(BoardSquare, BoardSquare)).MethodHandle;
			}
			flag = (\u001D.y != \u000E.y);
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		bool flag3 = \u001D.x >= \u000E.x - 1 && \u001D.x <= \u000E.x + 1;
		bool flag4;
		if (\u001D.y >= \u000E.y - 1)
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
			flag4 = (\u001D.y <= \u000E.y + 1);
		}
		else
		{
			flag4 = false;
		}
		bool result = flag4;
		if (flag2)
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
			if (flag3)
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
				return result;
			}
		}
		return false;
	}

	public bool \u0012(BoardSquare \u001D, BoardSquare \u000E)
	{
		if (\u001D.x == \u000E.x)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u0012(BoardSquare, BoardSquare)).MethodHandle;
			}
			if (\u001D.y != \u000E.y + 1)
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
				if (\u001D.y != \u000E.y - 1)
				{
					goto IL_5D;
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
		IL_5D:
		if (\u001D.y == \u000E.y)
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
			if (\u001D.x != \u000E.x + 1)
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
				if (\u001D.x != \u000E.x - 1)
				{
					goto IL_AD;
				}
			}
			return true;
		}
		IL_AD:
		return false;
	}

	public bool \u0015(BoardSquare \u001D, BoardSquare \u000E)
	{
		bool flag = this.\u000E(\u001D, \u000E);
		return flag && \u001D.x != \u000E.x && \u001D.y != \u000E.y;
	}

	public List<BoardSquare> \u000E(Bounds \u001D, Func<BoardSquare, bool> \u000E = null)
	{
		if (!Mathf.Approximately(\u001D.center.y, 0f))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u000E(Bounds, Func<BoardSquare, bool>)).MethodHandle;
			}
			Log.Error("code error: Board.GetSquaresInBox bounds.center.y must be zero!", new object[0]);
		}
		Vector3 min = \u001D.min;
		Vector3 max = \u001D.max;
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
				BoardSquare boardSquare = Board.\u000E().\u0016(i, j);
				Vector3 point = new Vector3(boardSquare.worldX, 0f, boardSquare.worldY);
				if (\u001D.Contains(point))
				{
					if (\u000E != null)
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
						if (!\u000E(boardSquare))
						{
							goto IL_15F;
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
					list.Add(boardSquare);
				}
				IL_15F:;
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
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		return list;
	}

	public List<BoardSquare> \u000E(BoardSquare \u001D, BoardSquare \u000E)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (\u001D != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.\u000E(BoardSquare, BoardSquare)).MethodHandle;
			}
			if (\u000E != null)
			{
				int num = Mathf.Min(\u001D.x, \u000E.x);
				int num2 = Mathf.Max(\u001D.x, \u000E.x);
				int num3 = Mathf.Min(\u001D.y, \u000E.y);
				int num4 = Mathf.Max(\u001D.y, \u000E.y);
				for (int i = num3; i <= num4; i++)
				{
					for (int j = num; j <= num2; j++)
					{
						BoardSquare item = Board.\u000E().\u0016(j, i);
						list.Add(item);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.OnDrawGizmos()).MethodHandle;
			}
			Gizmos.DrawWireCube(this.PlayerFreeSquare.ToVector3(), new Vector3(1.7f, 1.7f, 1.7f));
		}
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(this.PlayerClampedPos, 0.4f);
		if (this.PlayerClampedSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Board.DrawBoardGridGizmo()).MethodHandle;
			}
			if (this.m_maxY > 0)
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
				Color white = Color.white;
				white.a = 0.3f;
				Gizmos.color = white;
				BoardSquare boardSquare = Board.\u000E().\u0016(this.m_maxX / 2, this.m_maxY / 2);
				if (boardSquare != null)
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
					int num = this.m_maxX / 2;
					int num2 = this.m_maxY / 2;
					float squareSize = Board.\u000E().squareSize;
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
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					float num6 = vector.z - num4;
					for (int j = 0; j < num2 * 2; j++)
					{
						Vector3 a4 = vector;
						a4.z = num6 + squareSize * (float)j;
						Vector3 b2 = a * num3;
						Gizmos.DrawLine(a4 + b2, a4 - b2);
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
			}
		}
	}
}
