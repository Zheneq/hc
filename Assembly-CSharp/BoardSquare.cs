using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BoardSquare : MonoBehaviour
{
	public enum CornerType : byte
	{
		LowerLeft,
		LowerRight,
		UpperRight,
		UpperLeft
	}

	public enum VisibilityFlags : byte
	{
		Self = 1,
		Team = 2,
		Objective = 4,
		Revealed = 8
	}

	private ThinCover.CoverType[] m_thinCoverTypes = new ThinCover.CoverType[4];

	public GridPosProp m_gridPosProp;

	private GridPos m_pos = GridPos.s_invalid;

	[SerializeField]
	private GameObject m_LOSHighlightObj;

	private Mesh m_LOSHighlightMesh;

	private float m_highlightOffset = 0.05f;

	private GameObject m_occupant;

	private ActorData m_occupantActor;

	[SerializeField]
	private Vector3[] m_vertices;

	private Bounds? m_internalCameraBounds;

	private Bounds? m_internalWorldBounds;

	public static Color s_LOSHighlightColor = new Color(1f, 1f, 1f, 0.25f);

	public static Color s_mouseOverHighlightColor = new Color(0f, 1f, 0f, 0.5f);

	public static Color s_mouseOverOutOfRangeColor = new Color(0f, 0f, 0f, 0.5f);

	public static Color s_moveableHighlightColor = new Color(0f, 1f, 1f);

	public static Color s_targetableByAbilityHighlightColor = new Color(0.8f, 0.2f, 0f);

	public static Color s_respawnOptionHighlightColor = new Color(0.2f, 0.8f, 0f);

	public static float s_LoSHeightOffset = 1.6f;

	public static int s_boardSquareLayer = 9;

	private static Color s_revealedColor = new Color(0.25f, 0.25f, 0.5f, 0f);

	private static Color s_objectiveColor = new Color(0.5f, 0.75f, 0.5f, 0f);

	private static Color s_visibleBySelfColor = new Color(0f, 0f, 0f, 0f);

	private static Color s_visibleByTeamColor = new Color(0f, 0f, 0f, 0f);

	private static Color s_notVisibleColor = new Color(1f, 1f, 1f, 1f);

	private sbyte m_lastVisibleFlag = -1;

	public int x => m_pos.x;

	public int y => m_pos.y;

	public float worldX => m_pos.worldX;

	public float worldY => m_pos.worldY;

	public int height => m_pos.height;

	public GameObject occupant
	{
		get
		{
			return m_occupant;
		}
		set
		{
			m_occupant = value;
			if (m_occupant != null)
			{
				m_occupantActor = m_occupant.GetComponent<ActorData>();
			}
			else
			{
				m_occupantActor = null;
			}
		}
	}

	public ActorData OccupantActor
	{
		get
		{
			return m_occupantActor;
		}
		set
		{
			if (value == null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						occupant = null;
						return;
					}
				}
			}
			occupant = value.gameObject;
		}
	}

	public int BrushRegion
	{
		get;
		set;
	}

	internal Bounds CameraBounds
	{
		get
		{
			Bounds? internalCameraBounds = m_internalCameraBounds;
			if (!internalCameraBounds.HasValue)
			{
				if (m_vertices != null)
				{
					if (m_vertices.Length > 1)
					{
						Vector3 zero = Vector3.zero;
						Vector3[] vertices = m_vertices;
						foreach (Vector3 vector in vertices)
						{
							zero += vector;
						}
						zero /= (float)m_vertices.Length;
						zero += Vector3.up;
						Vector3 vector2 = 2.5f * (m_vertices[0] - zero);
						m_internalCameraBounds = new Bounds(size: new Vector3(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y), Mathf.Abs(vector2.z)), center: zero);
					}
				}
			}
			return m_internalCameraBounds.Value;
		}
	}

	internal Bounds? WorldBounds
	{
		get
		{
			Bounds? internalWorldBounds = m_internalWorldBounds;
			if (!internalWorldBounds.HasValue)
			{
				if (m_vertices != null && m_vertices.Length > 1)
				{
					Vector3 vector = (m_vertices[0] + m_vertices[1]) / 2f;
					Vector3 vector2 = m_vertices[0] - vector;
					vector2 = new Vector3(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y), Mathf.Abs(vector2.z));
					Bounds value = new Bounds(vector, vector2);
					for (int i = 2; i < m_vertices.Length; i++)
					{
						value.Encapsulate(m_vertices[i]);
					}
					m_internalWorldBounds = value;
				}
			}
			return m_internalWorldBounds;
		}
	}

	public ThinCover.CoverType GetCoverInDirection(ActorCover.CoverDirections direction)
	{
		return m_thinCoverTypes[(int)direction];
	}

	public void SetThinCover(ActorCover.CoverDirections squareSide, ThinCover.CoverType coverType)
	{
		m_thinCoverTypes[(int)squareSide] = coverType;
	}

	public bool IsExposedFromAnyDirection_zq()
	{
		for (int i = 0; i < m_thinCoverTypes.Length; i++)
		{
			if (m_thinCoverTypes[i] != 0)
			{
				return true;
			}
		}
		while (true)
		{
			return false;
		}
	}

	public bool HasFullCoverFromAnyDirection_zq()
	{
		for (int i = 0; i < m_thinCoverTypes.Length; i++)
		{
			if (m_thinCoverTypes[i] != ThinCover.CoverType.Full)
			{
				continue;
			}
			while (true)
			{
				return true;
			}
		}
		while (true)
		{
			return false;
		}
	}

	public GridPos GetGridPos()
	{
		return m_pos;
	}

	public bool IsInBrushRegion()
	{
		return BrushRegion != -1;
	}

	public bool _0015()
	{
		int num = 1;
		return height <= Board.Get().BaselineHeight + num;
	}

	public bool IsBaselineHeight()
	{
		return height == Board.BaselineHeightStatic;
	}

	public Vector3 GetVerticesAtCorner_zq(CornerType corner)
	{
		return m_vertices[(uint)corner];
	}

	public Vector3 ToVector3()
	{
		return new Vector3(m_pos.worldX, height, m_pos.worldY);
	}

	private void CalculateVertices(Board board, out Vector3[] vertices)
	{
		vertices = new Vector3[4];
		float squareSize = board.squareSize;
		float num = 0.5f * squareSize;
		float num2 = (float)m_pos.x * board.squareSize;
		float num3 = (float)m_pos.y * board.squareSize;
		vertices[0] = new Vector3(num2 - num, (float)m_pos.height + m_highlightOffset, num3 - num);
		vertices[1] = new Vector3(num2 + num, (float)m_pos.height + m_highlightOffset, num3 - num);
		vertices[2] = new Vector3(num2 + num, (float)m_pos.height + m_highlightOffset, num3 + num);
		vertices[3] = new Vector3(num2 - num, (float)m_pos.height + m_highlightOffset, num3 + num);
	}

	public float GetHighlightOffset()
	{
		return m_highlightOffset;
	}

	internal Vector3 CalcNearestPositionOnSquareEdge(Vector3 point)
	{
		int num = 0;
		int num2 = 1;
		float num3 = float.MaxValue;
		float num4 = float.MaxValue;
		for (int i = 0; i < m_vertices.Length; i++)
		{
			float sqrMagnitude = (point - m_vertices[i]).sqrMagnitude;
			if (sqrMagnitude < num3)
			{
				num4 = num3;
				num2 = num;
				num3 = sqrMagnitude;
				num = i;
			}
			else if (sqrMagnitude < num4)
			{
				num4 = sqrMagnitude;
				num2 = i;
			}
		}
		Vector3 a = m_vertices[num] - m_vertices[num2];
		float magnitude = a.magnitude;
		Vector3 a2 = Vector3.Project(point - m_vertices[num2], a / magnitude);
		if (a2.sqrMagnitude > magnitude * magnitude)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_vertices[num];
				}
			}
		}
		return a2 + m_vertices[num2];
	}

	internal Plane CalcSidePlane(SideFlags side)
	{
		Plane result = default(Plane);
		if ((side & SideFlags.Up) != 0)
		{
			result = new Plane(Vector3.forward, GetVerticesAtCorner_zq(CornerType.UpperLeft));
		}
		else if ((side & SideFlags.Down) != 0)
		{
			result = new Plane(Vector3.back, GetVerticesAtCorner_zq(CornerType.LowerLeft));
		}
		else if ((side & SideFlags.Left) != 0)
		{
			result = new Plane(Vector3.left, GetVerticesAtCorner_zq(CornerType.UpperLeft));
		}
		else if ((side & SideFlags.Right) != 0)
		{
			result = new Plane(Vector3.right, GetVerticesAtCorner_zq(CornerType.UpperRight));
		}
		return result;
	}

	internal Bounds CalcSideBounds(SideFlags side)
	{
		Bounds result = default(Bounds);
		if ((side & SideFlags.Up) != 0)
		{
			result.SetMinMax(GetVerticesAtCorner_zq(CornerType.UpperLeft), GetVerticesAtCorner_zq(CornerType.UpperRight) + Vector3.up);
		}
		if ((side & SideFlags.Down) != 0)
		{
			result.SetMinMax(GetVerticesAtCorner_zq(CornerType.LowerLeft), GetVerticesAtCorner_zq(CornerType.LowerRight) + Vector3.up);
		}
		if ((side & SideFlags.Left) != 0)
		{
			result.SetMinMax(GetVerticesAtCorner_zq(CornerType.LowerLeft), GetVerticesAtCorner_zq(CornerType.UpperLeft) + Vector3.up);
		}
		if ((side & SideFlags.Right) != 0)
		{
			result.SetMinMax(GetVerticesAtCorner_zq(CornerType.LowerRight), GetVerticesAtCorner_zq(CornerType.UpperRight) + Vector3.up);
		}
		return result;
	}

	public float[] CalculateLOS(Board board)
	{
		float[] array = new float[board.GetMaxX() * board.GetMaxY()];
		for (int i = 0; i < board.GetMaxX(); i++)
		{
			for (int j = 0; j < board.GetMaxY(); j++)
			{
				BoardSquare boardSquare = board.GetSquare(i, j);
				if (boardSquare == this)
				{
					array[i + j * board.GetMaxX()] = 1f;
				}
				else
				{
					array[i + j * board.GetMaxX()] = VectorUtils.GetLineOfSightPercentDistance(x, y, i, j, board, s_LoSHeightOffset, "LineOfSight");
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					goto end_IL_009b;
				}
				continue;
				end_IL_009b:
				break;
			}
		}
		return array;
	}

	public bool LOSDistanceIsOne_zq(int xDest, int yDest)
	{
		if (Board.Get().m_losLookup != null)
		{
			return Board.Get().m_losLookup.GetLOSDistance(m_pos.x, m_pos.y, xDest, yDest) == 1f;
		}
		return false;
	}

	public float GetLOSDistance(int xDest, int yDest)
	{
		float result = 0f;
		if (Board.Get().m_losLookup != null)
		{
			result = Board.Get().m_losLookup.GetLOSDistance(m_pos.x, m_pos.y, xDest, yDest);
		}
		return result;
	}

	public void Setup(Board board, Material meshMaterial, GameObject losHighlightsParent)
	{
		SetupGridPosProp(board.squareSize);
		CalculateVertices(board, out m_vertices);
		if (height == -1)
		{
			return;
		}
		while (true)
		{
			float x = (float)m_pos.x * board.squareSize;
			float z = (float)m_pos.y * board.squareSize;
			m_LOSHighlightObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
			m_LOSHighlightObj.name = $"highlight{m_pos.x}x{m_pos.y}";
			Object.DestroyImmediate(m_LOSHighlightObj.GetComponent<MeshCollider>());
			m_LOSHighlightObj.transform.parent = losHighlightsParent.transform;
			m_LOSHighlightObj.transform.position = new Vector3(x, (float)m_pos.height + m_highlightOffset, z);
			m_LOSHighlightObj.transform.Rotate(new Vector3(90f, 0f, 0f));
			m_LOSHighlightObj.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
			MeshRenderer component = m_LOSHighlightObj.GetComponent<MeshRenderer>();
			component.material = meshMaterial;
			component.enabled = true;
			component.shadowCastingMode = ShadowCastingMode.Off;
			component.receiveShadows = false;
			m_LOSHighlightObj.layer = LayerMask.NameToLayer("FogOfWar");
			return;
		}
	}

	private void Start()
	{
		if (!m_LOSHighlightObj)
		{
			return;
		}
		while (true)
		{
			m_LOSHighlightObj.layer = LayerMask.NameToLayer("FogOfWar");
			m_LOSHighlightMesh = m_LOSHighlightObj.GetComponent<MeshFilter>().mesh;
			return;
		}
	}

	public void ReevaluateSquare()
	{
		m_pos.x = m_gridPosProp.m_x;
		m_pos.y = m_gridPosProp.m_y;
		m_pos.height = m_gridPosProp.m_height;
	}

	private void Awake()
	{
		BrushRegion = -1;
		for (int i = 0; i < m_thinCoverTypes.Length; i++)
		{
			m_thinCoverTypes[i] = ThinCover.CoverType.None;
		}
		while (true)
		{
			return;
		}
	}

	private void OnDestroy()
	{
		m_occupant = null;
		m_occupantActor = null;
	}

	public void SetupGridPosProp(float squareSize)
	{
		if (m_gridPosProp == null)
		{
			m_gridPosProp = new GridPosProp();
		}
		GridPosProp gridPosProp = m_gridPosProp;
		Vector3 position = base.transform.position;
		gridPosProp.m_x = (int)(position.x / squareSize);
		GridPosProp gridPosProp2 = m_gridPosProp;
		Vector3 position2 = base.transform.position;
		gridPosProp2.m_y = (int)(position2.z / squareSize);
		GridPosProp gridPosProp3 = m_gridPosProp;
		Vector3 localScale = base.transform.localScale;
		gridPosProp3.m_height = (int)localScale.y;
		ReevaluateSquare();
	}

	public float HorizontalDistanceOnBoardTo(BoardSquare other)
	{
		float a = Mathf.Abs(x - other.x);
		float b = Mathf.Abs(y - other.y);
		float num = Mathf.Min(a, b);
		float num2 = Mathf.Max(a, b);
		return num2 - num + num * 1.5f;
	}

	public float HorizontalDistanceInSquaresTo(BoardSquare other)
	{
		Vector3 a = new Vector3(x, 0f, y);
		Vector3 b = new Vector3(other.x, 0f, other.y);
		return (a - b).magnitude;
	}

	public float HorizontalDistanceInSquaresTo_Squared(BoardSquare other)
	{
		Vector3 a = new Vector3(x, 0f, y);
		Vector3 b = new Vector3(other.x, 0f, other.y);
		return (a - b).sqrMagnitude;
	}

	public float HorizontalDistanceInWorldTo(BoardSquare other)
	{
		float num = m_pos.worldX - other.m_pos.worldX;
		float num2 = m_pos.worldY - other.m_pos.worldY;
		return Mathf.Sqrt(num * num + num2 * num2);
	}

	public float HorizontalDistanceInSquaresToPos(Vector3 worldPos)
	{
		Vector3 b = ToVector3();
		Vector3 vector = worldPos - b;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return magnitude / Board.Get().squareSize;
	}

	public int VerticalDistanceTo(BoardSquare other)
	{
		return other.height - height;
	}

	public Color GetLOSHighlightMeshBaseColor()
	{
		Color result;
		if (m_LOSHighlightMesh != null)
		{
			if (m_LOSHighlightMesh.colors.Length > 0)
			{
				result = m_LOSHighlightMesh.colors[0];
				goto IL_005e;
			}
		}
		result = Color.white;
		goto IL_005e;
		IL_005e:
		return result;
	}

	public void SetVisibleShade(int visibilityFlags, ref bool anySquareShadeChanged)
	{
		if (!m_LOSHighlightMesh)
		{
			return;
		}
		while (true)
		{
			bool flag = (visibilityFlags & 8) != 0;
			bool flag2 = (visibilityFlags & 4) != 0;
			bool flag3 = (visibilityFlags & 1) != 0;
			bool flag4 = (visibilityFlags & 2) != 0;
			Color lhs;
			if (!flag)
			{
				if (Board.Get().m_showLOS)
				{
					if (flag2)
					{
						lhs = s_objectiveColor;
					}
					else if (flag3)
					{
						lhs = s_visibleBySelfColor;
					}
					else if (flag4)
					{
						lhs = s_visibleByTeamColor;
					}
					else
					{
						lhs = s_notVisibleColor;
					}
					goto IL_00c2;
				}
			}
			lhs = s_revealedColor;
			goto IL_00c2;
			IL_00c2:
			bool flag5 = false;
			if (!Board.Get().m_showLOS)
			{
				m_lastVisibleFlag = -1;
				flag5 = true;
			}
			bool flag6 = m_lastVisibleFlag != (sbyte)visibilityFlags;
			if (flag5)
			{
				flag6 = (lhs != GetLOSHighlightMeshBaseColor());
			}
			if (!flag6)
			{
				return;
			}
			while (true)
			{
				if (!flag5)
				{
					m_lastVisibleFlag = (sbyte)visibilityFlags;
				}
				Mesh lOSHighlightMesh = m_LOSHighlightMesh;
				Vector3[] vertices = lOSHighlightMesh.vertices;
				Color32[] array = new Color32[vertices.Length];
				int i = 0;
				Color32 color = new Color32((byte)(lhs.r * 255f), (byte)(lhs.g * 255f), (byte)(lhs.b * 255f), (byte)(lhs.a * 255f));
				for (; i < vertices.Length; i++)
				{
					array[i] = color;
				}
				lOSHighlightMesh.colors32 = array;
				anySquareShadeChanged = true;
				return;
			}
		}
	}

	private Color _001D(int _001D)
	{
		bool flag = (_001D & 8) != 0;
		bool flag2 = (_001D & 4) != 0;
		bool flag3 = (_001D & 1) != 0;
		bool flag4 = (_001D & 2) != 0;
		InfluenceType influenceType = this._001D(false);
		Color result;
		if (influenceType == InfluenceType.InfluencedByA)
		{
			result = new Color(ActorData.s_teamAColor.r, ActorData.s_teamAColor.g, ActorData.s_teamAColor.b);
		}
		else if (influenceType == InfluenceType.InfluencedByB)
		{
			result = new Color(ActorData.s_teamBColor.r, ActorData.s_teamBColor.g, ActorData.s_teamBColor.b);
		}
		else if (influenceType != InfluenceType.Contested)
		{
			result = new Color(1f, 1f, 1f);
		}
		else
		{
			result = new Color(ActorData.s_hostilePlayerColor.r, ActorData.s_hostilePlayerColor.g, ActorData.s_hostilePlayerColor.b);
		}
		float num;
		if (flag3)
		{
			num = 1f;
		}
		else
		{
			if (!flag)
			{
				if (!flag2)
				{
					if (!flag4)
					{
						num = 0.25f;
						goto IL_0126;
					}
				}
			}
			num = 0.5f;
		}
		goto IL_0126;
		IL_0126:
		result.r *= num;
		result.g *= num;
		result.b *= num;
		result.a = 0.25f;
		return result;
	}

	public Vector3 GetWorldPosition()
	{
		return new Vector3(worldX, height, worldY);
	}

	public Vector3 GetWorldPositionForLoS()
	{
		Vector3 worldPosition = GetWorldPosition();
		worldPosition.y += s_LoSHeightOffset;
		return worldPosition;
	}

	public Vector3 GetBaselineHeight()
	{
		Vector3 result = new Vector3(worldX, height, worldY);
		if (Board.Get() != null)
		{
			result.y = Board.Get().BaselineHeight;
		}
		return result;
	}

	public InfluenceType _001D(bool _001D)
	{
		if (occupant != null && occupant.GetComponent<ActorData>() != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					ActorData component = occupant.GetComponent<ActorData>();
					if (component.GetTeam() == Team.TeamA)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return InfluenceType.InfluencedByA;
							}
						}
					}
					if (component.GetTeam() == Team.TeamB)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return InfluenceType.InfluencedByB;
							}
						}
					}
					return InfluenceType.Contested;
				}
				}
			}
		}
		float num = 1E+08f;
		float num2 = 1E+08f;
		float num3 = 36f;
		List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(Team.TeamA);
		foreach (ActorData item in allTeamMembers)
		{
			if (item.IsDead())
			{
			}
			else
			{
				if (GameplayUtils.IsPlayerControlled(item))
				{
					if (!_001D)
					{
						continue;
					}
				}
				BoardSquare currentBoardSquare = item.GetCurrentBoardSquare();
				float b = HorizontalDistanceInSquaresTo_Squared(currentBoardSquare);
				num = Mathf.Min(num, b);
			}
		}
		List<ActorData> allTeamMembers2 = GameFlowData.Get().GetAllTeamMembers(Team.TeamB);
		using (List<ActorData>.Enumerator enumerator2 = allTeamMembers2.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ActorData current2 = enumerator2.Current;
				if (!current2.IsDead())
				{
					if (GameplayUtils.IsPlayerControlled(current2))
					{
						if (!_001D)
						{
							continue;
						}
					}
					BoardSquare currentBoardSquare2 = current2.GetCurrentBoardSquare();
					float b2 = HorizontalDistanceInSquaresTo_Squared(currentBoardSquare2);
					num2 = Mathf.Min(num2, b2);
				}
			}
		}
		if (num > num3)
		{
			if (num2 > num3)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return InfluenceType.NoInfluence;
					}
				}
			}
		}
		if (Mathf.Abs(num - num2) > 0.01f)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (num < num2)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return InfluenceType.InfluencedByA;
							}
						}
					}
					return InfluenceType.InfluencedByB;
				}
			}
		}
		return InfluenceType.Contested;
	}

	public static string _001D(BoardSquare _001D, bool _000E = false)
	{
		string text;
		if (_001D == null)
		{
			text = "(null)";
		}
		else
		{
			text = _001D.GetGridPos().ToString();
			if (_000E)
			{
				if (_001D.IsBaselineHeight())
				{
					text += "[valid]";
				}
				else
				{
					text += "[invalid]";
				}
			}
		}
		return text;
	}
}
