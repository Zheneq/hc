using System;
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
            if (m_quads[0].m_corner1 != null && m_quads[0].m_corner2 != null)
            {
                Vector3 position1 = m_quads[0].m_corner1.position;
                BoardSquare square1 = Board.Get().GetSquareClosestToPos(position1.x, position1.z);
                Vector3 position2 = m_quads[0].m_corner2.position;
                BoardSquare square2 = Board.Get().GetSquareClosestToPos(position2.x, position2.z);
                if (square1 != null && square2 != null)
                {
                    Vector3 center = (square1.ToVector3() + square2.ToVector3()) * 0.5f;
                    if (m_functioningVFX != null)
                    {
                        m_functioningVFX.transform.position = center;
                    }

                    if (m_disruptedVFX != null)
                    {
                        m_disruptedVFX.transform.position = center;
                    }
                }
            }
            else
            {
                Log.Error("BrushRegion has null corners; set them or remove the region entirely.");
            }
        }

        m_perSquareFunctioningVFX = new List<GameObject>();
        m_perSquareDisruptedVFX = new List<GameObject>();
        m_exteriorSquareFlags = new Dictionary<BoardSquare, byte>();
        List<BoardSquare> squaresInRegion = GetSquaresInRegion();
        foreach (BoardSquare square in squaresInRegion)
        {
            if (!square.IsValidForGameplay())
            {
                continue;
            }

            Vector3 position = square.ToVector3();
            if (HighlightUtils.Get() != null)
            {
                if (HighlightUtils.Get().m_brushDisruptedSquarePrefab != null)
                {
                    GameObject gameObject =
                        UnityEngine.Object.Instantiate(HighlightUtils.Get().m_brushDisruptedSquarePrefab);
                    gameObject.transform.position = position;
                    gameObject.transform.parent = BrushCoordinator.Get().transform;
                    m_perSquareDisruptedVFX.Add(gameObject);
                }

                if (HighlightUtils.Get().m_brushFunctioningSquarePrefab != null)
                {
                    GameObject gameObject2 =
                        UnityEngine.Object.Instantiate(HighlightUtils.Get().m_brushFunctioningSquarePrefab);
                    gameObject2.transform.position = position;
                    gameObject2.transform.parent = BrushCoordinator.Get().transform;
                    m_perSquareFunctioningVFX.Add(gameObject2);
                }
            }

            byte sideFlags = 0;
            MaskSideFlagForSquare(ref sideFlags, square, squaresInRegion);
            if (sideFlags != 0)
            {
                m_exteriorSquareFlags[square] = sideFlags;
            }
        }

        float y = HighlightUtils.Get() != null
            ? HighlightUtils.Get().m_brushBorderHeightOffset
            : 0f;
        Vector3 localPosition = new Vector3(0f, y, 0f);
        if (m_borderVfxParentFunctioning != null)
        {
            Log.Error("Brush region border vfx parent already exists when initializing region");
            UnityEngine.Object.Destroy(m_borderVfxParentFunctioning);
        }

        m_borderVfxParentFunctioning = new GameObject("BrushBorderParent_Functioning")
        {
            transform =
            {
                localPosition = localPosition,
                localRotation = Quaternion.identity
            }
        };
        if (GameFlowData.Get() != null)
        {
            m_borderVfxParentFunctioning.transform.parent = GameFlowData.Get().GetBrushBordersRoot().transform;
        }

        if (m_borderVfxParentDisrupted != null)
        {
            Log.Error("Brush region border vfx parent already exists when initializing region");
            UnityEngine.Object.Destroy(m_borderVfxParentDisrupted);
        }

        m_borderVfxParentDisrupted = new GameObject("BrushBorderParent_Disrupted")
        {
            transform =
            {
                localPosition = localPosition,
                localRotation = Quaternion.identity
            }
        };
        if (GameFlowData.Get() != null)
        {
            m_borderVfxParentDisrupted.transform.parent = GameFlowData.Get().GetBrushBordersRoot().transform;
        }

        m_borderVfxListFunctioning.Clear();
        m_borderVfxListDisrupted.Clear();
        GameObject functioningPrefab = HighlightUtils.Get()?.m_brushFunctioningBorderPrefab;
        GameObject disruptedPrefab = HighlightUtils.Get()?.m_brushDisruptedBorderPrefab;
        foreach (KeyValuePair<BoardSquare, byte> flag in m_exteriorSquareFlags)
        {
            AddSideVfxPrefabs(
                flag.Key,
                flag.Value,
                functioningPrefab,
                disruptedPrefab,
                m_borderVfxParentFunctioning,
                m_borderVfxParentDisrupted,
                m_borderVfxListFunctioning,
                m_borderVfxListDisrupted);
        }

        m_borderVfxParentDisrupted.SetActive(false);
        m_borderVfxParentFunctioning.SetActive(false);
    }

    public static bool HasTeamMemberInRegion(Team team, int regionIndex)
    {
        if (regionIndex < 0)
        {
            return false;
        }

        foreach (ActorData actor in GameFlowData.Get().GetAllTeamMembers(team))
        {
            if (actor.GetBrushRegion() == regionIndex)
            {
                return true;
            }
        }

        return false;
    }

    public void UpdateBorderVisibility(bool functioning)
    {
        bool isBrushEnabled = !BrushCoordinator.Get().DisableAllBrush();
        bool isBrushVfxEnabled = CameraManager.Get() == null || !CameraManager.Get().ShouldHideBrushVfx();
        if (functioning != m_borderVfxParentFunctioning.activeSelf || m_lastBorderCanBeVisible != isBrushVfxEnabled)
        {
            m_borderVfxParentFunctioning.SetActive(functioning);
            foreach (PKFxFX fx in m_borderVfxListFunctioning)
            {
                if (fx == null)
                {
                    continue;
                }

                if (functioning && isBrushEnabled && isBrushVfxEnabled)
                {
                    fx.StartEffect();
                }
                else
                {
                    fx.TerminateEffect();
                }
            }
        }

        if (functioning == m_borderVfxParentDisrupted.activeSelf || m_lastBorderCanBeVisible != isBrushVfxEnabled)
        {
            m_borderVfxParentDisrupted.SetActive(!functioning);
            foreach (PKFxFX fx in m_borderVfxListDisrupted)
            {
                if (fx == null)
                {
                    continue;
                }

                if (!functioning && isBrushEnabled && isBrushVfxEnabled)
                {
                    fx.StartEffect();
                }
                else
                {
                    fx.TerminateEffect();
                }
            }
        }

        m_lastBorderCanBeVisible = isBrushVfxEnabled;
    }

    internal byte GetExteriorSideFlags(BoardSquare square)
    {
        return m_exteriorSquareFlags.TryGetValue(square, out byte result) ? result : (byte)0;
    }

    internal void DrawOutlineGizmos(bool functioning)
    {
        Gizmos.color = functioning ? Color.white : Color.red;
        if (m_exteriorSquareFlags == null)
        {
            return;
        }

        foreach (KeyValuePair<BoardSquare, byte> exteriorSquareFlag in m_exteriorSquareFlags)
        {
            DrawDebugGizmos(exteriorSquareFlag.Key, exteriorSquareFlag.Value);
        }
    }

    private static void MaskSideFlagForSquare(
        ref byte sideFlags,
        BoardSquare centerSquare,
        List<BoardSquare> squaresInSet)
    {
        BoardSquare boardSquare = Board.Get().GetSquareFromIndex(centerSquare.x, centerSquare.y + 1);
        BoardSquare boardSquare2 = Board.Get().GetSquareFromIndex(centerSquare.x, centerSquare.y - 1);
        BoardSquare boardSquare3 = Board.Get().GetSquareFromIndex(centerSquare.x - 1, centerSquare.y);
        BoardSquare boardSquare4 = Board.Get().GetSquareFromIndex(centerSquare.x + 1, centerSquare.y);
        ApplyMarkForSide(ref sideFlags, SideFlags.Up, boardSquare, squaresInSet);
        ApplyMarkForSide(ref sideFlags, SideFlags.Down, boardSquare2, squaresInSet);
        ApplyMarkForSide(ref sideFlags, SideFlags.Left, boardSquare3, squaresInSet);
        ApplyMarkForSide(ref sideFlags, SideFlags.Right, boardSquare4, squaresInSet);
    }

    private static void ApplyMarkForSide(
        ref byte sideFlags,
        SideFlags mask,
        BoardSquare squareToTest,
        List<BoardSquare> squaresInSet)
    {
        if (squareToTest == null || !squareToTest.IsValidForGameplay() || !squaresInSet.Contains(squareToTest))
        {
            sideFlags |= (byte)mask;
        }
    }

    private static void AddSideVfxPrefabs(
        BoardSquare square,
        byte sideFlags,
        GameObject functioningPrefab,
        GameObject disruptedPrefab,
        GameObject functioningRoot,
        GameObject disruptedRoot,
        List<PKFxFX> functioningVfxList,
        List<PKFxFX> disruptedVfxList)
    {
        if (square == null)
        {
            return;
        }

        Vector3 vfxPos = square.ToVector3();
        foreach (SideFlags side in Enum.GetValues(typeof(SideFlags)))
        {
            if (((int)sideFlags & (int)side) != 0)
            {
                AddSideVfxForSide(
                    side,
                    vfxPos,
                    functioningPrefab,
                    disruptedPrefab,
                    functioningRoot,
                    disruptedRoot,
                    functioningVfxList,
                    disruptedVfxList);
            }
        }
    }

    private static void AddSideVfxForSide(
        SideFlags side,
        Vector3 vfxPos,
        GameObject functioningPrefab,
        GameObject disruptedPrefab,
        GameObject functioningRoot,
        GameObject disruptedRoot,
        List<PKFxFX> functioningVfxList,
        List<PKFxFX> disruptedVfxList)
    {
        Quaternion rotationForSidePrefab = GetRotationForSidePrefab(side);
        if (functioningPrefab != null)
        {
            GameObject fxObject = UnityEngine.Object.Instantiate(functioningPrefab);
            fxObject.transform.localPosition = vfxPos;
            fxObject.transform.localRotation = rotationForSidePrefab;
            fxObject.transform.parent = functioningRoot.transform;
            foreach (PKFxFX fx in fxObject.GetComponentsInChildren<PKFxFX>())
            {
                functioningVfxList.Add(fx);
            }
        }

        if (disruptedPrefab != null)
        {
            GameObject fxObject = UnityEngine.Object.Instantiate(disruptedPrefab);
            fxObject.transform.localPosition = vfxPos;
            fxObject.transform.localRotation = rotationForSidePrefab;
            fxObject.transform.parent = disruptedRoot.transform;
            foreach (PKFxFX fx in fxObject.GetComponentsInChildren<PKFxFX>())
            {
                disruptedVfxList.Add(fx);
            }
        }
    }

    private static Quaternion GetRotationForSidePrefab(SideFlags side)
    {
        switch (side)
        {
            case SideFlags.Up:
                return Quaternion.identity;
            case SideFlags.Down:
                return Quaternion.Euler(0f, 180f, 0f);
            case SideFlags.Left:
                return Quaternion.Euler(0f, 270f, 0f);
            case SideFlags.Right:
                return Quaternion.Euler(0f, 90f, 0f);
            default:
                return Quaternion.identity;
        }
    }

    private static void DrawDebugGizmos(BoardSquare square, byte sideFlags)
    {
        if (square == null)
        {
            return;
        }

        Vector3 pos = square.ToVector3();
        Vector3 forward = new Vector3(0f, 0f, 0.5f * Board.Get().squareSize);
        Vector3 left = new Vector3(0.5f * Board.Get().squareSize, 0f, 0f);

        if ((sideFlags & (byte)SideFlags.Up) != 0)
        {
            Gizmos.DrawLine(pos + forward - left, pos + forward + left);
        }

        if ((sideFlags & (byte)SideFlags.Down) != 0)
        {
            Gizmos.DrawLine(pos - forward + left, pos - forward - left);
        }

        if ((sideFlags & (byte)SideFlags.Left) != 0)
        {
            Gizmos.DrawLine(pos - left + forward, pos - left - forward);
        }

        if ((sideFlags & (byte)SideFlags.Right) != 0)
        {
            Gizmos.DrawLine(pos + left + forward, pos + left - forward);
        }
    }
}