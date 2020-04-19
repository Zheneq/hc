using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BoardSquare : MonoBehaviour
{
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

	public ThinCover.CoverType \u001D(ActorCover.CoverDirections \u001D)
	{
		return this.m_thinCoverTypes[(int)\u001D];
	}

	public void SetThinCover(ActorCover.CoverDirections squareSide, ThinCover.CoverType coverType)
	{
		this.m_thinCoverTypes[(int)squareSide] = coverType;
	}

	public bool \u001D()
	{
		for (int i = 0; i < this.m_thinCoverTypes.Length; i++)
		{
			if (this.m_thinCoverTypes[i] != ThinCover.CoverType.None)
			{
				return true;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.\u001D()).MethodHandle;
		}
		return false;
	}

	public bool \u000E()
	{
		for (int i = 0; i < this.m_thinCoverTypes.Length; i++)
		{
			if (this.m_thinCoverTypes[i] == ThinCover.CoverType.Full)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.\u000E()).MethodHandle;
				}
				return true;
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
		return false;
	}

	public GridPos \u001D()
	{
		return this.m_pos;
	}

	public int x
	{
		get
		{
			return this.m_pos.x;
		}
	}

	public int y
	{
		get
		{
			return this.m_pos.y;
		}
	}

	public float worldX
	{
		get
		{
			return this.m_pos.worldX;
		}
	}

	public float worldY
	{
		get
		{
			return this.m_pos.worldY;
		}
	}

	public int height
	{
		get
		{
			return this.m_pos.height;
		}
	}

	public GameObject occupant
	{
		get
		{
			return this.m_occupant;
		}
		set
		{
			this.m_occupant = value;
			if (this.m_occupant != null)
			{
				this.m_occupantActor = this.m_occupant.GetComponent<ActorData>();
			}
			else
			{
				this.m_occupantActor = null;
			}
		}
	}

	public ActorData OccupantActor
	{
		get
		{
			return this.m_occupantActor;
		}
		set
		{
			if (value == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.set_OccupantActor(ActorData)).MethodHandle;
				}
				this.occupant = null;
			}
			else
			{
				this.occupant = value.gameObject;
			}
		}
	}

	public int BrushRegion { get; set; }

	public bool \u0012()
	{
		return this.BrushRegion != -1;
	}

	public bool \u0015()
	{
		int num = 1;
		return this.height <= Board.\u000E().BaselineHeight + num;
	}

	public bool \u0016()
	{
		return this.height == Board.BaselineHeightStatic;
	}

	internal Bounds CameraBounds
	{
		get
		{
			Bounds? internalCameraBounds = this.m_internalCameraBounds;
			if (internalCameraBounds == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.get_CameraBounds()).MethodHandle;
				}
				if (this.m_vertices != null)
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
					if (this.m_vertices.Length > 1)
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
						Vector3 vector = Vector3.zero;
						foreach (Vector3 b in this.m_vertices)
						{
							vector += b;
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
						vector /= (float)this.m_vertices.Length;
						vector += Vector3.up;
						Vector3 size = 2.5f * (this.m_vertices[0] - vector);
						size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
						this.m_internalCameraBounds = new Bounds?(new Bounds(vector, size));
					}
				}
			}
			return this.m_internalCameraBounds.Value;
		}
	}

	internal Bounds? WorldBounds
	{
		get
		{
			Bounds? internalWorldBounds = this.m_internalWorldBounds;
			if (internalWorldBounds == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.get_WorldBounds()).MethodHandle;
				}
				if (this.m_vertices != null && this.m_vertices.Length > 1)
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
					Vector3 vector = (this.m_vertices[0] + this.m_vertices[1]) / 2f;
					Vector3 size = this.m_vertices[0] - vector;
					size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
					Bounds value = new Bounds(vector, size);
					for (int i = 2; i < this.m_vertices.Length; i++)
					{
						value.Encapsulate(this.m_vertices[i]);
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
					this.m_internalWorldBounds = new Bounds?(value);
				}
			}
			return this.m_internalWorldBounds;
		}
	}

	public Vector3 \u001D(BoardSquare.CornerType \u001D)
	{
		return this.m_vertices[(int)\u001D];
	}

	public Vector3 ToVector3()
	{
		return new Vector3(this.m_pos.worldX, (float)this.height, this.m_pos.worldY);
	}

	private void CalculateVertices(Board board, out Vector3[] vertices)
	{
		vertices = new Vector3[4];
		float squareSize = board.squareSize;
		float num = 0.5f * squareSize;
		float num2 = (float)this.m_pos.x * board.squareSize;
		float num3 = (float)this.m_pos.y * board.squareSize;
		vertices[0] = new Vector3(num2 - num, (float)this.m_pos.height + this.m_highlightOffset, num3 - num);
		vertices[1] = new Vector3(num2 + num, (float)this.m_pos.height + this.m_highlightOffset, num3 - num);
		vertices[2] = new Vector3(num2 + num, (float)this.m_pos.height + this.m_highlightOffset, num3 + num);
		vertices[3] = new Vector3(num2 - num, (float)this.m_pos.height + this.m_highlightOffset, num3 + num);
	}

	public float \u001D()
	{
		return this.m_highlightOffset;
	}

	internal Vector3 CalcNearestPositionOnSquareEdge(Vector3 point)
	{
		int num = 0;
		int num2 = 1;
		float num3 = float.MaxValue;
		float num4 = float.MaxValue;
		for (int i = 0; i < this.m_vertices.Length; i++)
		{
			float sqrMagnitude = (point - this.m_vertices[i]).sqrMagnitude;
			if (sqrMagnitude < num3)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.CalcNearestPositionOnSquareEdge(Vector3)).MethodHandle;
				}
				num4 = num3;
				num2 = num;
				num3 = sqrMagnitude;
				num = i;
			}
			else if (sqrMagnitude < num4)
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
				num4 = sqrMagnitude;
				num2 = i;
			}
		}
		Vector3 a = this.m_vertices[num] - this.m_vertices[num2];
		float magnitude = a.magnitude;
		Vector3 vector = Vector3.Project(point - this.m_vertices[num2], a / magnitude);
		if (vector.sqrMagnitude > magnitude * magnitude)
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
			vector = this.m_vertices[num];
		}
		else
		{
			vector += this.m_vertices[num2];
		}
		return vector;
	}

	internal Plane CalcSidePlane(SideFlags side)
	{
		Plane result = default(Plane);
		if ((byte)(side & SideFlags.Up) != 0)
		{
			result = new Plane(Vector3.forward, this.\u001D(BoardSquare.CornerType.UpperLeft));
		}
		else if ((byte)(side & SideFlags.Down) != 0)
		{
			result = new Plane(Vector3.back, this.\u001D(BoardSquare.CornerType.LowerLeft));
		}
		else if ((byte)(side & SideFlags.Left) != 0)
		{
			result = new Plane(Vector3.left, this.\u001D(BoardSquare.CornerType.UpperLeft));
		}
		else if ((byte)(side & SideFlags.Right) != 0)
		{
			result = new Plane(Vector3.right, this.\u001D(BoardSquare.CornerType.UpperRight));
		}
		return result;
	}

	internal Bounds CalcSideBounds(SideFlags side)
	{
		Bounds result = default(Bounds);
		if ((byte)(side & SideFlags.Up) != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.CalcSideBounds(SideFlags)).MethodHandle;
			}
			result.SetMinMax(this.\u001D(BoardSquare.CornerType.UpperLeft), this.\u001D(BoardSquare.CornerType.UpperRight) + Vector3.up);
		}
		if ((byte)(side & SideFlags.Down) != 0)
		{
			result.SetMinMax(this.\u001D(BoardSquare.CornerType.LowerLeft), this.\u001D(BoardSquare.CornerType.LowerRight) + Vector3.up);
		}
		if ((byte)(side & SideFlags.Left) != 0)
		{
			result.SetMinMax(this.\u001D(BoardSquare.CornerType.LowerLeft), this.\u001D(BoardSquare.CornerType.UpperLeft) + Vector3.up);
		}
		if ((byte)(side & SideFlags.Right) != 0)
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
			result.SetMinMax(this.\u001D(BoardSquare.CornerType.LowerRight), this.\u001D(BoardSquare.CornerType.UpperRight) + Vector3.up);
		}
		return result;
	}

	public float[] CalculateLOS(Board board)
	{
		float[] array = new float[board.\u000E() * board.\u0012()];
		for (int i = 0; i < board.\u000E(); i++)
		{
			for (int j = 0; j < board.\u0012(); j++)
			{
				BoardSquare x = board.\u0016(i, j);
				if (x == this)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.CalculateLOS(Board)).MethodHandle;
					}
					array[i + j * board.\u000E()] = 1f;
				}
				else
				{
					array[i + j * board.\u000E()] = VectorUtils.GetLineOfSightPercentDistance(this.x, this.y, i, j, board, BoardSquare.s_LoSHeightOffset, "LineOfSight");
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
		return array;
	}

	public bool \u0013(int \u001D, int \u000E)
	{
		bool result = false;
		if (Board.\u000E().m_losLookup != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.\u0013(int, int)).MethodHandle;
			}
			bool flag;
			if (Board.\u000E().m_losLookup.GetLOSDistance(this.m_pos.x, this.m_pos.y, \u001D, \u000E) == 1f)
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
				flag = true;
			}
			else
			{
				flag = false;
			}
			result = flag;
		}
		return result;
	}

	public float \u000E(int \u001D, int \u000E)
	{
		float result = 0f;
		if (Board.\u000E().m_losLookup != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.\u000E(int, int)).MethodHandle;
			}
			result = Board.\u000E().m_losLookup.GetLOSDistance(this.m_pos.x, this.m_pos.y, \u001D, \u000E);
		}
		return result;
	}

	public void Setup(Board board, Material meshMaterial, GameObject losHighlightsParent)
	{
		this.SetupGridPosProp(board.squareSize);
		this.CalculateVertices(board, out this.m_vertices);
		if (this.height != -1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.Setup(Board, Material, GameObject)).MethodHandle;
			}
			float x = (float)this.m_pos.x * board.squareSize;
			float z = (float)this.m_pos.y * board.squareSize;
			this.m_LOSHighlightObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
			this.m_LOSHighlightObj.name = string.Format("highlight{0}x{1}", this.m_pos.x, this.m_pos.y);
			UnityEngine.Object.DestroyImmediate(this.m_LOSHighlightObj.GetComponent<MeshCollider>());
			this.m_LOSHighlightObj.transform.parent = losHighlightsParent.transform;
			this.m_LOSHighlightObj.transform.position = new Vector3(x, (float)this.m_pos.height + this.m_highlightOffset, z);
			this.m_LOSHighlightObj.transform.Rotate(new Vector3(90f, 0f, 0f));
			this.m_LOSHighlightObj.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
			MeshRenderer component = this.m_LOSHighlightObj.GetComponent<MeshRenderer>();
			component.material = meshMaterial;
			component.enabled = true;
			component.shadowCastingMode = ShadowCastingMode.Off;
			component.receiveShadows = false;
			this.m_LOSHighlightObj.layer = LayerMask.NameToLayer("FogOfWar");
		}
	}

	private void Start()
	{
		if (this.m_LOSHighlightObj)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.Start()).MethodHandle;
			}
			this.m_LOSHighlightObj.layer = LayerMask.NameToLayer("FogOfWar");
			this.m_LOSHighlightMesh = this.m_LOSHighlightObj.GetComponent<MeshFilter>().mesh;
		}
	}

	public void ReevaluateSquare()
	{
		this.m_pos.x = this.m_gridPosProp.m_x;
		this.m_pos.y = this.m_gridPosProp.m_y;
		this.m_pos.height = this.m_gridPosProp.m_height;
	}

	private void Awake()
	{
		this.BrushRegion = -1;
		for (int i = 0; i < this.m_thinCoverTypes.Length; i++)
		{
			this.m_thinCoverTypes[i] = ThinCover.CoverType.None;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.Awake()).MethodHandle;
		}
	}

	private void OnDestroy()
	{
		this.m_occupant = null;
		this.m_occupantActor = null;
	}

	public void SetupGridPosProp(float squareSize)
	{
		if (this.m_gridPosProp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.SetupGridPosProp(float)).MethodHandle;
			}
			this.m_gridPosProp = new GridPosProp();
		}
		this.m_gridPosProp.m_x = (int)(base.transform.position.x / squareSize);
		this.m_gridPosProp.m_y = (int)(base.transform.position.z / squareSize);
		this.m_gridPosProp.m_height = (int)base.transform.localScale.y;
		this.ReevaluateSquare();
	}

	public float HorizontalDistanceOnBoardTo(BoardSquare other)
	{
		float a = (float)Mathf.Abs(this.x - other.x);
		float b = (float)Mathf.Abs(this.y - other.y);
		float num = Mathf.Min(a, b);
		float num2 = Mathf.Max(a, b);
		return num2 - num + num * 1.5f;
	}

	public float HorizontalDistanceInSquaresTo(BoardSquare other)
	{
		Vector3 a = new Vector3((float)this.x, 0f, (float)this.y);
		Vector3 b = new Vector3((float)other.x, 0f, (float)other.y);
		return (a - b).magnitude;
	}

	public float HorizontalDistanceInSquaresTo_Squared(BoardSquare other)
	{
		Vector3 a = new Vector3((float)this.x, 0f, (float)this.y);
		Vector3 b = new Vector3((float)other.x, 0f, (float)other.y);
		return (a - b).sqrMagnitude;
	}

	public float HorizontalDistanceInWorldTo(BoardSquare other)
	{
		float num = this.m_pos.worldX - other.m_pos.worldX;
		float num2 = this.m_pos.worldY - other.m_pos.worldY;
		return Mathf.Sqrt(num * num + num2 * num2);
	}

	public float HorizontalDistanceInSquaresToPos(Vector3 worldPos)
	{
		Vector3 b = this.ToVector3();
		Vector3 vector = worldPos - b;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return magnitude / Board.\u000E().squareSize;
	}

	public int VerticalDistanceTo(BoardSquare other)
	{
		return other.height - this.height;
	}

	public Color \u001D()
	{
		if (this.m_LOSHighlightMesh != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.\u001D()).MethodHandle;
			}
			if (this.m_LOSHighlightMesh.colors.Length > 0)
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
				return this.m_LOSHighlightMesh.colors[0];
			}
		}
		return Color.white;
	}

	public unsafe void SetVisibleShade(int visibilityFlags, ref bool anySquareShadeChanged)
	{
		if (this.m_LOSHighlightMesh)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.SetVisibleShade(int, bool*)).MethodHandle;
			}
			bool flag = (visibilityFlags & 8) != 0;
			bool flag2 = (visibilityFlags & 4) != 0;
			bool flag3 = (visibilityFlags & 1) != 0;
			bool flag4 = (visibilityFlags & 2) != 0;
			Color lhs;
			if (!flag)
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
				if (!Board.\u000E().m_showLOS)
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
				}
				else
				{
					if (flag2)
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
						lhs = BoardSquare.s_objectiveColor;
						goto IL_C2;
					}
					if (flag3)
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
						lhs = BoardSquare.s_visibleBySelfColor;
						goto IL_C2;
					}
					if (flag4)
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
						lhs = BoardSquare.s_visibleByTeamColor;
						goto IL_C2;
					}
					lhs = BoardSquare.s_notVisibleColor;
					goto IL_C2;
				}
			}
			lhs = BoardSquare.s_revealedColor;
			IL_C2:
			bool flag5 = false;
			if (!Board.\u000E().m_showLOS)
			{
				this.m_lastVisibleFlag = -1;
				flag5 = true;
			}
			bool flag6 = (int)this.m_lastVisibleFlag != (int)((sbyte)visibilityFlags);
			if (flag5)
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
				flag6 = (lhs != this.\u001D());
			}
			if (flag6)
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
				if (!flag5)
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
					this.m_lastVisibleFlag = (sbyte)visibilityFlags;
				}
				Mesh loshighlightMesh = this.m_LOSHighlightMesh;
				Vector3[] vertices = loshighlightMesh.vertices;
				Color32[] array = new Color32[vertices.Length];
				int i = 0;
				Color32 color = new Color32((byte)(lhs.r * 255f), (byte)(lhs.g * 255f), (byte)(lhs.b * 255f), (byte)(lhs.a * 255f));
				while (i < vertices.Length)
				{
					array[i] = color;
					i++;
				}
				loshighlightMesh.colors32 = array;
				anySquareShadeChanged = true;
			}
		}
	}

	private Color \u001D(int \u001D)
	{
		bool flag = (\u001D & 8) != 0;
		bool flag2 = (\u001D & 4) != 0;
		bool flag3 = (\u001D & 1) != 0;
		bool flag4 = (\u001D & 2) != 0;
		InfluenceType influenceType = this.\u001D(false);
		Color result;
		if (influenceType == InfluenceType.InfluencedByA)
		{
			result = new Color(ActorData.s_teamAColor.r, ActorData.s_teamAColor.g, ActorData.s_teamAColor.b);
		}
		else if (influenceType == InfluenceType.InfluencedByB)
		{
			result = new Color(ActorData.s_teamBColor.r, ActorData.s_teamBColor.g, ActorData.s_teamBColor.b);
		}
		else if (influenceType == InfluenceType.Contested)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.\u001D(int)).MethodHandle;
			}
			result = new Color(ActorData.s_hostilePlayerColor.r, ActorData.s_hostilePlayerColor.g, ActorData.s_hostilePlayerColor.b);
		}
		else
		{
			result = new Color(1f, 1f, 1f);
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!flag2)
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
					if (!flag4)
					{
						num = 0.25f;
						goto IL_126;
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
			num = 0.5f;
		}
		IL_126:
		result.r *= num;
		result.g *= num;
		result.b *= num;
		result.a = 0.25f;
		return result;
	}

	public Vector3 \u001D()
	{
		Vector3 result = new Vector3(this.worldX, (float)this.height, this.worldY);
		return result;
	}

	public Vector3 \u000E()
	{
		Vector3 result = this.\u001D();
		result.y += BoardSquare.s_LoSHeightOffset;
		return result;
	}

	public Vector3 \u0012()
	{
		Vector3 result = new Vector3(this.worldX, (float)this.height, this.worldY);
		if (Board.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.\u0012()).MethodHandle;
			}
			result.y = (float)Board.\u000E().BaselineHeight;
		}
		return result;
	}

	public InfluenceType \u001D(bool \u001D)
	{
		InfluenceType result;
		if (this.occupant != null && this.occupant.GetComponent<ActorData>() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.\u001D(bool)).MethodHandle;
			}
			ActorData component = this.occupant.GetComponent<ActorData>();
			if (component.\u000E() == Team.TeamA)
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
				result = InfluenceType.InfluencedByA;
			}
			else if (component.\u000E() == Team.TeamB)
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
				result = InfluenceType.InfluencedByB;
			}
			else
			{
				result = InfluenceType.Contested;
			}
		}
		else
		{
			float num = 1E+08f;
			float num2 = 1E+08f;
			float num3 = 36f;
			List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(Team.TeamA);
			foreach (ActorData actorData in allTeamMembers)
			{
				if (actorData.\u000E())
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
				}
				else
				{
					if (GameplayUtils.IsPlayerControlled(actorData))
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
						if (!\u001D)
						{
							continue;
						}
					}
					BoardSquare other = actorData.\u0012();
					float b = this.HorizontalDistanceInSquaresTo_Squared(other);
					num = Mathf.Min(num, b);
				}
			}
			List<ActorData> allTeamMembers2 = GameFlowData.Get().GetAllTeamMembers(Team.TeamB);
			using (List<ActorData>.Enumerator enumerator2 = allTeamMembers2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actorData2 = enumerator2.Current;
					if (!actorData2.\u000E())
					{
						if (GameplayUtils.IsPlayerControlled(actorData2))
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
							if (!\u001D)
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
								continue;
							}
						}
						BoardSquare other2 = actorData2.\u0012();
						float b2 = this.HorizontalDistanceInSquaresTo_Squared(other2);
						num2 = Mathf.Min(num2, b2);
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
			if (num > num3)
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
				if (num2 > num3)
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
					return InfluenceType.NoInfluence;
				}
			}
			if (Mathf.Abs(num - num2) > 0.01f)
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
				if (num < num2)
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
					result = InfluenceType.InfluencedByA;
				}
				else
				{
					result = InfluenceType.InfluencedByB;
				}
			}
			else
			{
				result = InfluenceType.Contested;
			}
		}
		return result;
	}

	public static string \u001D(BoardSquare \u001D, bool \u000E = false)
	{
		string text;
		if (\u001D == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BoardSquare.\u001D(BoardSquare, bool)).MethodHandle;
			}
			text = "(null)";
		}
		else
		{
			text = \u001D.\u001D().ToString();
			if (\u000E)
			{
				if (\u001D.\u0016())
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
		Team,
		Objective = 4,
		Revealed = 8
	}
}
