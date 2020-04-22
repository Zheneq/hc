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

	public float LosCheckHeight => (float)BaselineHeight + BoardSquare.s_LoSHeightOffset;

	public Vector3 PlayerLookDir
	{
		get;
		private set;
	}

	public Vector3 PlayerMouseIntersectionPos
	{
		get;
		private set;
	}

	public Vector3 PlayerMouseLookDir
	{
		get;
		private set;
	}

	public Vector3 MouseBoardSquareIntersectionPos
	{
		get;
		private set;
	}

	public Vector3 PlayerFreePos
	{
		get;
		private set;
	}

	public Vector3 PlayerFreeCornerPos
	{
		get;
		private set;
	}

	public BoardSquare PlayerFreeSquare
	{
		get;
		private set;
	}

	public Vector3 PlayerClampedPos
	{
		get;
		private set;
	}

	public Vector3 PlayerClampedCornerPos
	{
		get;
		private set;
	}

	public BoardSquare PlayerClampedSquare
	{
		get;
		private set;
	}

	public bool MouseOverSquareInRange
	{
		get;
		set;
	}

	public bool MarkedForUpdateValidSquares => m_needToUpdateValidSquares;

	public static Board Get()
	{
		if (s_board == null)
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
			if (Application.isEditor && !Application.isPlaying)
			{
				s_board = UnityEngine.Object.FindObjectOfType<Board>();
				if (s_board != null)
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
					s_board.ReevaluateBoard();
				}
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
			m_LOSHighlightsParent.layer = LayerMask.NameToLayer("FogOfWar");
			m_LOSHighlightsParent.SetActive(true);
		}
		if (m_LOSHighlightsParent != null)
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
			if (!NetworkClient.active)
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
				if (NetworkServer.active)
				{
					UnityEngine.Object.DestroyImmediate(m_LOSHighlightsParent);
					m_LOSHighlightsParent = null;
				}
			}
		}
		GameObject gameObject = GameObject.Find("Camera Guide Mesh");
		if ((bool)gameObject)
		{
			m_cameraGuideMeshCollider = gameObject.GetComponent<MeshCollider>();
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
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameFlowDataStarted);
		}
		m_normalPathBuildScratchPool = null;
		s_board = null;
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.GameFlowDataStarted)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			base.enabled = true;
			return;
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
		if ((bool)main)
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
			Vector3 position = main.transform.position;
			Vector3 mousePosition = Input.mousePosition;
			Vector3 direction = main.ScreenPointToRay(mousePosition).direction;
			Vector3 up = Vector3.up;
			float d = ((float)m_baselineHeight - position.y) / Vector3.Dot(direction, up);
			PlayerMouseIntersectionPos = position + direction * d;
			if ((bool)actorData)
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
				Vector3 position2 = actorData.transform.position;
				PlayerMouseLookDir = (PlayerMouseIntersectionPos - position2).normalized;
			}
			bool flag;
			if (ControlpadGameplay.Get() != null && ControlpadGameplay.Get().UsingControllerInput)
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
				flag = false;
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				PlayerLookDir = PlayerMouseLookDir;
				PlayerFreePos = PlayerMouseIntersectionPos;
			}
			else
			{
				PlayerLookDir = ControlpadGameplay.Get().ControllerAimDir;
				PlayerFreePos = ControlpadGameplay.Get().ControllerAimPos;
			}
			PlayerFreeSquare = GetBoardSquare(PlayerFreePos);
			PlayerFreeCornerPos = _000E(PlayerFreePos, PlayerFreeSquare);
			RecalcClampedSelections();
			HighlightUtils.Get().UpdateCursorPositions();
			HighlightUtils.Get().UpdateRangeIndicatorHighlight();
			HighlightUtils.Get().UpdateMouseoverCoverHighlight();
			HighlightUtils.Get().UpdateShowAffectedSquareFlag();
		}
		if (!Input.GetMouseButtonUp(2))
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
			bool applyToAllJoints = false;
			float amount = 300f;
			ApplyForceOnDead(PlayerFreeSquare, amount, new Vector3(0f, 1f, 0f), applyToAllJoints);
			return;
		}
	}

	public static Vector3 _000E(Vector3 _001D, BoardSquare _000E)
	{
		float num = SquareSizeStatic / 2f;
		float x;
		float z;
		if (_000E == null)
		{
			x = _001D.x;
			z = _001D.z;
		}
		else
		{
			float worldX = _000E.worldX;
			if (_001D.x > worldX)
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
				x = worldX + num;
			}
			else
			{
				x = worldX - num;
			}
			float worldY = _000E.worldY;
			z = ((!(_001D.z > worldY)) ? (worldY - num) : (worldY + num));
		}
		return new Vector3(x, _001D.y, z);
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (PlayerFreeSquare != null)
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
				if (PlayerFreeSquare.occupant != null)
				{
					ActorData component2 = PlayerFreeSquare.occupant.GetComponent<ActorData>();
					if (component2 != null)
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
						if (component2.IsVisibleToClient())
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
							flag = true;
							goto IL_010e;
						}
					}
					flag = false;
					goto IL_010e;
				}
			}
		}
		flag = false;
		goto IL_010e;
		IL_021f:
		PlayerClampedPos = vector;
		PlayerClampedSquare = boardSquare;
		PlayerClampedCornerPos = _000E(vector, boardSquare);
		return;
		IL_010e:
		if (squaresToClampTo != null && squaresToClampTo.Count != 0)
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
			if (!squaresToClampTo.Contains(PlayerFreeSquare))
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
				if (!flag)
				{
					Vector3 playerFreePos = PlayerFreePos;
					float x = playerFreePos.x;
					Vector3 playerFreePos2 = PlayerFreePos;
					float z = playerFreePos2.z;
					float num = float.MaxValue;
					using (HashSet<BoardSquare>.Enumerator enumerator = squaresToClampTo.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare current = enumerator.Current;
							float num2 = current.worldX - x;
							float num3 = current.worldY - z;
							float num4 = num2 * num2 + num3 * num3;
							if (num4 <= num)
							{
								num = num4;
								boardSquare = current;
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
					if (boardSquare != null)
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
						vector = boardSquare.CalcNearestPositionOnSquareEdge(PlayerFreePos);
					}
					goto IL_021f;
				}
			}
		}
		vector = PlayerFreePos;
		boardSquare = PlayerFreeSquare;
		goto IL_021f;
	}

	public void ReevaluateBoard()
	{
		m_maxX = 0;
		m_maxY = 0;
		m_maxHeight = 0;
		m_lastValidGuidedHeight = 99999;
		m_lowestPositiveHeight = 99999;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				BoardSquare component = transform.GetComponent<BoardSquare>();
				component.ReevaluateSquare();
				if (component.height > 0 && component.height < m_lowestPositiveHeight)
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
					m_lowestPositiveHeight = component.height;
				}
				if (component.height > m_maxHeight)
				{
					m_maxHeight = component.height;
				}
				if (component.x + 1 > m_maxX)
				{
					m_maxX = component.x + 1;
				}
				if (component.y + 1 > m_maxY)
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
					m_maxY = component.y + 1;
				}
			}
			while (true)
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
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						disposable.Dispose();
						goto end_IL_0118;
					}
				}
			}
			end_IL_0118:;
		}
		m_boardSquares = new BoardSquare[m_maxX, m_maxY];
		IEnumerator enumerator2 = base.transform.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				Transform transform2 = (Transform)enumerator2.Current;
				BoardSquare component2 = transform2.GetComponent<BoardSquare>();
				if ((bool)component2)
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
					m_boardSquares[component2.x, component2.y] = component2;
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
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						disposable2.Dispose();
						goto end_IL_01b9;
					}
				}
			}
			end_IL_01b9:;
		}
		if (!(HUD_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			HUD_UI.Get().m_mainScreenPanel.m_minimap.SetupMinimap();
			return;
		}
	}

	public void SetLOSVisualEffect(bool enable)
	{
		if (m_showLOS == enable)
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
			m_showLOS = enable;
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					GameFlowData.Get().activeOwnedActorData.GetFogOfWar().SetVisibleShadeOfAllSquares();
					return;
				}
			}
			return;
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
		if (square != null)
		{
			List<GameObject> players = GameFlowData.Get().GetPlayers();
			foreach (GameObject item in players)
			{
				if (item != null)
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
					ActorData component = item.GetComponent<ActorData>();
					if ((bool)component)
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
						if (component.IsModelAnimatorDisabled())
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
							if (applyToAllJoints)
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
								if (component.GetActorModelData() != null)
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
		m_boardSquares[x, y].SetThinCover(side, coverType);
	}

	public float GetSquareHeight(int x, int y)
	{
		float result = 0f;
		if (m_boardSquares != null)
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
			if (x >= 0)
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
				if (x < GetMaxX())
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
					if (y >= 0)
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
						if (y < GetMaxY())
						{
							result = m_boardSquares[x, y].height;
						}
					}
				}
			}
		}
		return result;
	}

	public float _000E(Vector3 _001D, bool drawDebug)
	{
		if ((bool)m_cameraGuideMeshCollider)
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
			RaycastHit hitInfo = default(RaycastHit);
			Ray ray = new Ray(_001D + Vector3.up * m_maxHeight, Vector3.down);
			if (m_cameraGuideMeshCollider.Raycast(ray, out hitInfo, 5000f))
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
				Vector3 point = hitInfo.point;
				m_lastValidGuidedHeight = (int)point.y;
				if (drawDebug)
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
					Debug.DrawLine(ray.origin, hitInfo.point);
				}
			}
			else if (drawDebug)
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
				Debug.DrawLine(ray.origin, ray.origin + ray.direction * 5000f, new Color(1f, 0f, 0f));
			}
		}
		else
		{
			m_lastValidGuidedHeight = m_maxHeight;
		}
		if (m_lastValidGuidedHeight != 99999)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_lastValidGuidedHeight;
				}
			}
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

	public BoardSquare GetBoardSquareSafe(float x, float y)
	{
		BoardSquare result = null;
		int num = Mathf.RoundToInt(x / squareSize);
		int num2 = Mathf.RoundToInt(y / squareSize);
		if (num >= 0)
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
			if (num < GetMaxX() && num2 >= 0)
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
				if (num2 < GetMaxY())
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
					result = m_boardSquares[num, num2];
				}
			}
		}
		return result;
	}

	public BoardSquare GetBoardSquareUnsafe(float x, float y)
	{
		int value = Mathf.RoundToInt(x / squareSize);
		int value2 = Mathf.RoundToInt(y / squareSize);
		value = Mathf.Clamp(value, 0, GetMaxX() - 1);
		value2 = Mathf.Clamp(value2, 0, GetMaxY() - 1);
		return m_boardSquares[value, value2];
	}

	public BoardSquare GetBoardSquare(Vector3 vector2D)
	{
		return GetBoardSquareSafe(vector2D.x, vector2D.z);
	}

	public BoardSquare GetBoardSquare(Vector2 vector)
	{
		return GetBoardSquareSafe(vector.x, vector.y);
	}

	public BoardSquare GetBoardSquare(Transform transform)
	{
		BoardSquare result = null;
		if (transform != null)
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
			Vector3 position = transform.position;
			float x = position.x;
			Vector3 position2 = transform.position;
			result = GetBoardSquareSafe(x, position2.z);
		}
		return result;
	}

	public BoardSquare GetBoardSquare(int x, int y)
	{
		BoardSquare result = null;
		if (x >= 0)
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
			if (x < GetMaxX())
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
				if (y >= 0)
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
					if (y < GetMaxY())
					{
						result = m_boardSquares[x, y];
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
			if (gridPos.x < GetMaxX())
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
				if (gridPos.y >= 0)
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
					if (gridPos.y < GetMaxY())
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
						result = m_boardSquares[gridPos.x, gridPos.y];
					}
				}
			}
		}
		return result;
	}

	public void ResetGame()
	{
		ClearVisibleShade();
	}

	public void ClearVisibleShade()
	{
		bool anySquareShadeChanged = false;
		BoardSquare[,] boardSquares = m_boardSquares;
		int length = boardSquares.GetLength(0);
		int length2 = boardSquares.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				BoardSquare boardSquare = boardSquares[i, j];
				boardSquare.SetVisibleShade(0, ref anySquareShadeChanged);
			}
		}
		if (!anySquareShadeChanged)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameEventManager.Get() != null)
			{
				GameEventManager.Get().FireEvent(GameEventManager.EventType.BoardSquareVisibleShadeChanged, null);
			}
			return;
		}
	}

	public void GetStraightAdjacentSquares(int x, int y, ref List<BoardSquare> result)
	{
		if (result == null)
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
			result = new List<BoardSquare>(4);
		}
		if (GetBoardSquare(x + 1, y) != null)
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
			result.Add(GetBoardSquare(x + 1, y));
		}
		if (GetBoardSquare(x - 1, y) != null)
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
			result.Add(GetBoardSquare(x - 1, y));
		}
		if (GetBoardSquare(x, y + 1) != null)
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
			result.Add(GetBoardSquare(x, y + 1));
		}
		if (!(GetBoardSquare(x, y - 1) != null))
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
			result.Add(GetBoardSquare(x, y - 1));
			return;
		}
	}

	public void GetDiagonallyAdjacentSquares(int x, int y, ref List<BoardSquare> result)
	{
		if (result == null)
		{
			result = new List<BoardSquare>(4);
		}
		if (GetBoardSquare(x + 1, y + 1) != null)
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
			result.Add(GetBoardSquare(x + 1, y + 1));
		}
		if (GetBoardSquare(x + 1, y - 1) != null)
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
			result.Add(GetBoardSquare(x + 1, y - 1));
		}
		if (GetBoardSquare(x - 1, y + 1) != null)
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
			result.Add(GetBoardSquare(x - 1, y + 1));
		}
		if (!(GetBoardSquare(x - 1, y - 1) != null))
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
			result.Add(GetBoardSquare(x - 1, y - 1));
			return;
		}
	}

	public void GetAllAdjacentSquares(int x, int y, ref List<BoardSquare> result)
	{
		if (result == null)
		{
			result = new List<BoardSquare>(8);
		}
		GetStraightAdjacentSquares(x, y, ref result);
		GetDiagonallyAdjacentSquares(x, y, ref result);
	}

	public BoardSquare _0013(float _001D, float _000E)
	{
		BoardSquare boardSquareSafe = GetBoardSquareSafe(_001D, _000E);
		return _0018(boardSquareSafe);
	}

	public BoardSquare _0018(BoardSquare _001D, BoardSquare _000E = null)
	{
		BoardSquare result = null;
		if (_001D != null)
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
			bool flag = _001D == _000E;
			if (!_001D.IsBaselineHeight())
			{
				goto IL_0066;
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
			if (!(_000E == null))
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
				if (flag)
				{
					goto IL_0066;
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
			result = _001D;
		}
		return result;
		IL_0066:
		List<BoardSquare> result2 = null;
		GetAllAdjacentSquares(_001D.x, _001D.y, ref result2);
		if (_000E != null)
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
			result2.Remove(_000E);
		}
		float worldX = _001D.worldX;
		float worldY = _001D.worldY;
		result2.Sort(delegate(BoardSquare sq1, BoardSquare sq2)
		{
			float num = (sq1.worldX - worldX) * (sq1.worldX - worldX) + (sq1.worldY - worldY) * (sq1.worldY - worldY);
			float value = (sq2.worldX - worldX) * (sq2.worldX - worldX) + (sq2.worldY - worldY) * (sq2.worldY - worldY);
			return num.CompareTo(value);
		});
		using (List<BoardSquare>.Enumerator enumerator = result2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				if (current.IsBaselineHeight())
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return current;
						}
					}
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	public bool _000E(BoardSquare _001D, BoardSquare _000E)
	{
		int num;
		if (_001D.x == _000E.x)
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
			num = ((_001D.y != _000E.y) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		bool flag2 = _001D.x >= _000E.x - 1 && _001D.x <= _000E.x + 1;
		int num2;
		if (_001D.y >= _000E.y - 1)
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
			num2 = ((_001D.y <= _000E.y + 1) ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag3 = (byte)num2 != 0;
		int result;
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
			if (flag2)
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
				result = (flag3 ? 1 : 0);
				goto IL_00c0;
			}
		}
		result = 0;
		goto IL_00c0;
		IL_00c0:
		return (byte)result != 0;
	}

	public bool _0012(BoardSquare _001D, BoardSquare _000E)
	{
		if (_001D.x == _000E.x)
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
			if (_001D.y != _000E.y + 1)
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
				if (_001D.y != _000E.y - 1)
				{
					goto IL_005d;
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
		goto IL_005d;
		IL_005d:
		if (_001D.y == _000E.y)
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
			if (_001D.x != _000E.x + 1)
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
				if (_001D.x != _000E.x - 1)
				{
					goto IL_00ad;
				}
			}
			return true;
		}
		goto IL_00ad;
		IL_00ad:
		return false;
	}

	public bool _0015(BoardSquare _001D, BoardSquare _000E)
	{
		return this._000E(_001D, _000E) && _001D.x != _000E.x && _001D.y != _000E.y;
	}

	public List<BoardSquare> _000E(Bounds _001D, Func<BoardSquare, bool> _000E = null)
	{
		Vector3 center = _001D.center;
		if (!Mathf.Approximately(center.y, 0f))
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
			Log.Error("code error: Board.GetSquaresInBox bounds.center.y must be zero!");
		}
		Vector3 min = _001D.min;
		Vector3 max = _001D.max;
		min.y = 0f;
		max.y = 0f;
		int num = Mathf.Max(0, (int)(min.x / squareSize));
		int num2 = Mathf.Max(0, (int)(min.z / squareSize));
		int num3 = Mathf.Min(m_maxX, (int)(max.x / squareSize) + 1);
		int num4 = Mathf.Min(m_maxY, (int)(max.z / squareSize) + 1);
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = num; i < num3; i++)
		{
			for (int j = num2; j < num4; j++)
			{
				BoardSquare boardSquare = Get().GetBoardSquare(i, j);
				Vector3 point = new Vector3(boardSquare.worldX, 0f, boardSquare.worldY);
				if (!_001D.Contains(point))
				{
					continue;
				}
				if (_000E != null)
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
					if (!_000E(boardSquare))
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
				}
				list.Add(boardSquare);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					goto end_IL_016b;
				}
				continue;
				end_IL_016b:
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
			return list;
		}
	}

	public List<BoardSquare> GetSquaresInRect(BoardSquare a, BoardSquare b)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (a != null)
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
						BoardSquare boardSquare = Get().GetBoardSquare(j, i);
						list.Add(boardSquare);
					}
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
			Gizmos.DrawWireCube(PlayerFreeSquare.ToVector3(), new Vector3(1.7f, 1.7f, 1.7f));
		}
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(PlayerClampedPos, 0.4f);
		if (PlayerClampedSquare != null)
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
		if (m_maxX <= 0)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_maxY <= 0)
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
				Color white = Color.white;
				white.a = 0.3f;
				Gizmos.color = white;
				BoardSquare boardSquare = Get().GetBoardSquare(m_maxX / 2, m_maxY / 2);
				if (!(boardSquare != null))
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
					int num = m_maxX / 2;
					int num2 = m_maxY / 2;
					float squareSize = Get().squareSize;
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
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						float num6 = vector.z - num4;
						for (int j = 0; j < num2 * 2; j++)
						{
							Vector3 a4 = vector;
							a4.z = num6 + squareSize * (float)j;
							Vector3 b2 = a * num3;
							Gizmos.DrawLine(a4 + b2, a4 - b2);
						}
						while (true)
						{
							switch (4)
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
	}
}
