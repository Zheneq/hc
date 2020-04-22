using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BrushRegion : BoardRegion
{
	public GameObject m_functioningVFX;

	public GameObject m_disruptedVFX;

	public List<GameObject> m_perSquareFunctioningVFX;

	public List<GameObject> m_perSquareDisruptedVFX;

	private Dictionary<BoardSquare, byte> m_exteriorSquareFlags;

	private GameObject m_borderVfxParentFunctioning;

	private GameObject m_borderVfxParentDisrupted;

	private bool m_lastBorderCanBeVisible = true;

	private List<PKFxFX> m_borderVfxListFunctioning = new List<PKFxFX>();

	private List<PKFxFX> m_borderVfxListDisrupted = new List<PKFxFX>();

	public override void Initialize()
	{
		base.Initialize();
		if (m_quads.Length > 0)
		{
			if (!(m_quads[0].m_corner1 == null))
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
				if (!(m_quads[0].m_corner2 == null))
				{
					Vector3 position = m_quads[0].m_corner1.position;
					BoardSquare boardSquareUnsafe = Board.Get().GetBoardSquareUnsafe(position.x, position.z);
					Vector3 position2 = m_quads[0].m_corner2.position;
					BoardSquare boardSquareUnsafe2 = Board.Get().GetBoardSquareUnsafe(position2.x, position2.z);
					if (boardSquareUnsafe != null)
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
						if (boardSquareUnsafe2 != null)
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
							Vector3 position3 = (boardSquareUnsafe.ToVector3() + boardSquareUnsafe2.ToVector3()) * 0.5f;
							if (m_functioningVFX != null)
							{
								m_functioningVFX.transform.position = position3;
							}
							if (m_disruptedVFX != null)
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
								m_disruptedVFX.transform.position = position3;
							}
						}
					}
					goto IL_016d;
				}
			}
			Log.Error("BrushRegion has null corners; set them or remove the region entirely.");
		}
		goto IL_016d;
		IL_016d:
		m_perSquareFunctioningVFX = new List<GameObject>();
		m_perSquareDisruptedVFX = new List<GameObject>();
		m_exteriorSquareFlags = new Dictionary<BoardSquare, byte>();
		List<BoardSquare> squaresInRegion = GetSquaresInRegion();
		using (List<BoardSquare>.Enumerator enumerator = squaresInRegion.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				if (current.IsBaselineHeight())
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
					Vector3 position4 = current.ToVector3();
					if (HighlightUtils.Get() != null)
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
						if (HighlightUtils.Get().m_brushDisruptedSquarePrefab != null)
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
							GameObject gameObject = UnityEngine.Object.Instantiate(HighlightUtils.Get().m_brushDisruptedSquarePrefab);
							gameObject.transform.position = position4;
							gameObject.transform.parent = BrushCoordinator.Get().transform;
							m_perSquareDisruptedVFX.Add(gameObject);
						}
						if (HighlightUtils.Get().m_brushFunctioningSquarePrefab != null)
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
							GameObject gameObject2 = UnityEngine.Object.Instantiate(HighlightUtils.Get().m_brushFunctioningSquarePrefab);
							gameObject2.transform.position = position4;
							gameObject2.transform.parent = BrushCoordinator.Get().transform;
							m_perSquareFunctioningVFX.Add(gameObject2);
						}
					}
					byte sideFlags = 0;
					MaskSideFlagForSquare(ref sideFlags, current, squaresInRegion);
					if (sideFlags != 0)
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
						m_exteriorSquareFlags[current] = sideFlags;
					}
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
		float y;
		if (HighlightUtils.Get() != null)
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
			y = HighlightUtils.Get().m_brushBorderHeightOffset;
		}
		else
		{
			y = 0f;
		}
		Vector3 localPosition = new Vector3(0f, y, 0f);
		if (m_borderVfxParentFunctioning != null)
		{
			Log.Error("Brush region border vfx parent already exists when initializing region");
			UnityEngine.Object.Destroy(m_borderVfxParentFunctioning);
		}
		m_borderVfxParentFunctioning = new GameObject("BrushBorderParent_Functioning");
		m_borderVfxParentFunctioning.transform.localPosition = localPosition;
		m_borderVfxParentFunctioning.transform.localRotation = Quaternion.identity;
		if (GameFlowData.Get() != null)
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
			m_borderVfxParentFunctioning.transform.parent = GameFlowData.Get().GetBrushBordersRoot().transform;
		}
		if (m_borderVfxParentDisrupted != null)
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
			Log.Error("Brush region border vfx parent already exists when initializing region");
			UnityEngine.Object.Destroy(m_borderVfxParentDisrupted);
		}
		m_borderVfxParentDisrupted = new GameObject("BrushBorderParent_Disrupted");
		m_borderVfxParentDisrupted.transform.localPosition = localPosition;
		m_borderVfxParentDisrupted.transform.localRotation = Quaternion.identity;
		if (GameFlowData.Get() != null)
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
			m_borderVfxParentDisrupted.transform.parent = GameFlowData.Get().GetBrushBordersRoot().transform;
		}
		m_borderVfxListFunctioning.Clear();
		m_borderVfxListDisrupted.Clear();
		object obj;
		if (HighlightUtils.Get() != null)
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
			obj = HighlightUtils.Get().m_brushFunctioningBorderPrefab;
		}
		else
		{
			obj = null;
		}
		GameObject functioningPrefab = (GameObject)obj;
		object obj2;
		if (HighlightUtils.Get() != null)
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
			obj2 = HighlightUtils.Get().m_brushDisruptedBorderPrefab;
		}
		else
		{
			obj2 = null;
		}
		GameObject disruptedPrefab = (GameObject)obj2;
		using (Dictionary<BoardSquare, byte>.Enumerator enumerator2 = m_exteriorSquareFlags.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<BoardSquare, byte> current2 = enumerator2.Current;
				AddSideVfxPrefabs(current2.Key, current2.Value, functioningPrefab, disruptedPrefab, m_borderVfxParentFunctioning, m_borderVfxParentDisrupted, m_borderVfxListFunctioning, m_borderVfxListDisrupted);
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
		m_borderVfxParentDisrupted.SetActive(false);
		m_borderVfxParentFunctioning.SetActive(false);
	}

	public static bool HasTeamMemberInRegion(Team team, int regionIndex)
	{
		if (regionIndex >= 0)
		{
			bool result = false;
			List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(team);
			using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (current.GetTravelBoardSquareBrushRegion() == regionIndex)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								if (1 == 0)
								{
									/*OpCode not supported: LdMemberToken*/;
								}
								return true;
							}
						}
					}
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return result;
					}
				}
			}
		}
		return false;
	}

	public void UpdateBorderVisibility(bool functioning)
	{
		bool flag = !BrushCoordinator.Get().DisableAllBrush();
		int num;
		if (!(CameraManager.Get() == null))
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
			num = ((!CameraManager.Get().ShouldHideBrushVfx()) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool flag2 = (byte)num != 0;
		if (functioning == m_borderVfxParentFunctioning.activeSelf)
		{
			if (m_lastBorderCanBeVisible == flag2)
			{
				goto IL_00e6;
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
		m_borderVfxParentFunctioning.SetActive(functioning);
		for (int i = 0; i < m_borderVfxListFunctioning.Count; i++)
		{
			PKFxFX pKFxFX = m_borderVfxListFunctioning[i];
			if (!(pKFxFX != null))
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
			if (functioning)
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
				if (flag && flag2)
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
					pKFxFX.StartEffect();
					continue;
				}
			}
			pKFxFX.TerminateEffect();
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
		goto IL_00e6;
		IL_00e6:
		if (functioning == m_borderVfxParentDisrupted.activeSelf || m_lastBorderCanBeVisible != flag2)
		{
			m_borderVfxParentDisrupted.SetActive(!functioning);
			for (int j = 0; j < m_borderVfxListDisrupted.Count; j++)
			{
				PKFxFX pKFxFX2 = m_borderVfxListDisrupted[j];
				if (!(pKFxFX2 != null))
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
				if (!functioning)
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
					if (flag)
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
						if (flag2)
						{
							pKFxFX2.StartEffect();
							continue;
						}
					}
				}
				pKFxFX2.TerminateEffect();
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
		m_lastBorderCanBeVisible = flag2;
	}

	internal byte GetExteriorSideFlags(BoardSquare square)
	{
		int result;
		if (m_exteriorSquareFlags.ContainsKey(square))
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
			result = m_exteriorSquareFlags[square];
		}
		else
		{
			result = 0;
		}
		return (byte)result;
	}

	internal void DrawOutlineGizmos(bool functioning)
	{
		Color color;
		if (functioning)
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
			color = Color.white;
		}
		else
		{
			color = Color.red;
		}
		Gizmos.color = color;
		if (m_exteriorSquareFlags == null)
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
			foreach (KeyValuePair<BoardSquare, byte> exteriorSquareFlag in m_exteriorSquareFlags)
			{
				DrawDebugGizmos(exteriorSquareFlag.Key, exteriorSquareFlag.Value);
			}
			return;
		}
	}

	private static void MaskSideFlagForSquare(ref byte sideFlags, BoardSquare centerSquare, List<BoardSquare> squaresInSet)
	{
		BoardSquare boardSquare = Board.Get().GetBoardSquare(centerSquare.x, centerSquare.y + 1);
		BoardSquare boardSquare2 = Board.Get().GetBoardSquare(centerSquare.x, centerSquare.y - 1);
		BoardSquare boardSquare3 = Board.Get().GetBoardSquare(centerSquare.x - 1, centerSquare.y);
		BoardSquare boardSquare4 = Board.Get().GetBoardSquare(centerSquare.x + 1, centerSquare.y);
		ApplyMarkForSide(ref sideFlags, SideFlags.Up, boardSquare, squaresInSet);
		ApplyMarkForSide(ref sideFlags, SideFlags.Down, boardSquare2, squaresInSet);
		ApplyMarkForSide(ref sideFlags, SideFlags.Left, boardSquare3, squaresInSet);
		ApplyMarkForSide(ref sideFlags, SideFlags.Right, boardSquare4, squaresInSet);
	}

	private static void ApplyMarkForSide(ref byte sideFlags, SideFlags mask, BoardSquare squareToTest, List<BoardSquare> squaresInSet)
	{
		if (!(squareToTest == null))
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
			if (squareToTest.IsBaselineHeight())
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
				if (squaresInSet.Contains(squareToTest))
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
					break;
				}
			}
		}
		sideFlags |= (byte)mask;
	}

	private static void AddSideVfxPrefabs(BoardSquare square, byte sideFlags, GameObject functioningPrefab, GameObject disruptedPrefab, GameObject functioningRoot, GameObject disruptedRoot, List<PKFxFX> functioningVfxList, List<PKFxFX> disruptedVfxList)
	{
		if (square != null)
		{
			Vector3 vfxPos = square.ToVector3();
			IEnumerator enumerator = Enum.GetValues(typeof(SideFlags)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					SideFlags sideFlags2 = (SideFlags)enumerator.Current;
					if (((int)sideFlags & (int)sideFlags2) != 0)
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
						AddSideVfxForSide(sideFlags2, vfxPos, functioningPrefab, disruptedPrefab, functioningRoot, disruptedRoot, functioningVfxList, disruptedVfxList);
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
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							disposable.Dispose();
							goto end_IL_007f;
						}
					}
				}
				end_IL_007f:;
			}
		}
	}

	private static void AddSideVfxForSide(SideFlags side, Vector3 vfxPos, GameObject functioningPrefab, GameObject disruptedPrefab, GameObject functioningRoot, GameObject disruptedRoot, List<PKFxFX> functioningVfxList, List<PKFxFX> disruptedVfxList)
	{
		Quaternion rotationForSidePrefab = GetRotationForSidePrefab(side);
		if (functioningPrefab != null)
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
			GameObject gameObject = UnityEngine.Object.Instantiate(functioningPrefab);
			gameObject.transform.localPosition = vfxPos;
			gameObject.transform.localRotation = rotationForSidePrefab;
			gameObject.transform.parent = functioningRoot.transform;
			PKFxFX[] componentsInChildren = gameObject.GetComponentsInChildren<PKFxFX>();
			PKFxFX[] array = componentsInChildren;
			foreach (PKFxFX item in array)
			{
				functioningVfxList.Add(item);
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
		if (!(disruptedPrefab != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(disruptedPrefab);
			gameObject2.transform.localPosition = vfxPos;
			gameObject2.transform.localRotation = rotationForSidePrefab;
			gameObject2.transform.parent = disruptedRoot.transform;
			PKFxFX[] componentsInChildren2 = gameObject2.GetComponentsInChildren<PKFxFX>();
			PKFxFX[] array2 = componentsInChildren2;
			foreach (PKFxFX item2 in array2)
			{
				disruptedVfxList.Add(item2);
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private static Quaternion GetRotationForSidePrefab(SideFlags side)
	{
		if (side == SideFlags.Up)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return Quaternion.identity;
				}
			}
		}
		switch (side)
		{
		case SideFlags.Down:
			return Quaternion.Euler(0f, 180f, 0f);
		case SideFlags.Left:
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				return Quaternion.Euler(0f, 270f, 0f);
			}
		case SideFlags.Right:
			return Quaternion.Euler(0f, 90f, 0f);
		default:
			return Quaternion.identity;
		}
	}

	private static void DrawDebugGizmos(BoardSquare square, byte sideFlags)
	{
		if (!(square != null))
		{
			return;
		}
		Vector3 a = square.ToVector3();
		Vector3 b = new Vector3(0f, 0f, 0.5f * Board.Get().squareSize);
		Vector3 b2 = new Vector3(0.5f * Board.Get().squareSize, 0f, 0f);
		if ((sideFlags & 1) != 0)
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
			Gizmos.DrawLine(a + b - b2, a + b + b2);
		}
		if ((sideFlags & 2) != 0)
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
			Gizmos.DrawLine(a - b + b2, a - b - b2);
		}
		if ((sideFlags & 4) != 0)
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
			Gizmos.DrawLine(a - b2 + b, a - b2 - b);
		}
		if ((sideFlags & 8) == 0)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			Gizmos.DrawLine(a + b2 + b, a + b2 - b);
			return;
		}
	}
}
