using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CanvasLayerInfo
{
    public string CanvasLayerName;
    public bool OnlyOneSceneActiveAtATime;
    public int LayerPriority;

    public const int PaddingBetweenLayers = 60;
    public const int PaddingBetweenBatchCanvases = 10;

    public UISceneDisplayInfo[] SceneDisplayInfos;
    [HideInInspector]
    public Canvas StaticBatchLayerCanvas;
    public Canvas SemiStaticBatchLayerCanvas;
    public Canvas CameraMovementBatchLayerCanvas;
    public Canvas PerFrameBatchLayerCanvas;
    public GameObject DefaultWorldContainer;
    public Canvas DefaultLayerCanvas;
    public GameObject ScenesContainer;
    private bool init;
    private GameObject[] DefaultCanvasScenes;
    private GameObject[] StaticCanvasScenes;
    private GameObject[] SemiStaticCanvasScenes;
    private GameObject[] CameraMovementCanvasScenes;
    private GameObject[] PerFrameCanvasScenes;

    private List<RuntimeSceneInfo> Scenes = new List<RuntimeSceneInfo>();

    [NonSerialized]
    private UILayerManager m_parentInfo;

    public UILayerManager ParentInfo => m_parentInfo;

    public int SetSceneVisible(IEnumerable<SceneType> aScenes, bool visible, SceneVisibilityParameters parameters)
    {
        int num = 0;
        List<SceneType> scenesToUpdate = new List<SceneType>(aScenes);
        if (scenesToUpdate.Count == 0)
        {
            return num;
        }

        foreach (RuntimeSceneInfo sceneInfo in Scenes)
        {
            if (scenesToUpdate.Contains(sceneInfo.RuntimeScene.GetSceneType()))
            {
                sceneInfo.RuntimeScene.SetVisible(visible, parameters);
                sceneInfo.SetBatchScenesVisible(visible);
                num++;
            }
            else if (parameters.TurnOffAllOtherScenesInCanvasLayer)
            {
                sceneInfo.RuntimeScene.SetVisible(false, parameters);
                sceneInfo.SetBatchScenesVisible(false);
            }
        }

        return num;
    }

    private void SetupCanvas(GameObject container, GameObject parent, Canvas aCanvas, int canvasLayerOrder)
    {
        UIManager.ReparentTransform(container.transform, parent.transform);
        aCanvas.worldCamera = m_parentInfo.ParentInfo.ActiveCamera;
        aCanvas.renderMode = m_parentInfo.ParentInfo.CamType;
        aCanvas.sortingOrder = canvasLayerOrder;
        CanvasScaler canvasScaler = container.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
        canvasScaler.matchWidthOrHeight = 0.5f;
        canvasScaler.referencePixelsPerUnit = 100f;
        GraphicRaycaster graphicRaycaster = container.AddComponent<GraphicRaycaster>();
        graphicRaycaster.ignoreReversedGraphics = true;
        graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
        container.SetLayerRecursively(m_parentInfo.ObjectLayerValue);
    }

    private bool CreateCanvasBatchType(CanvasBatchType type)
    {
        bool result = false;
        switch (type)
        {
            case CanvasBatchType.Static:
            {
                if (StaticBatchLayerCanvas == null)
                {
                    GameObject canvasObject = new GameObject("Static Batch Canvas");
                    StaticBatchLayerCanvas = canvasObject.AddComponent<Canvas>();
                    SetupCanvas(canvasObject, ScenesContainer, StaticBatchLayerCanvas, (LayerPriority + 1) * 60 - 40);
                    StaticCanvasScenes = new GameObject[SceneDisplayInfos.Length];
                    for (int i = 0; i < SceneDisplayInfos.Length; i++)
                    {
                        StaticCanvasScenes[i] = new GameObject(
                            "(SceneContainer)" + SceneDisplayInfos[i].SceneName,
                            typeof(RectTransform));
                        UIManager.ReparentTransform(StaticCanvasScenes[i].transform, canvasObject.gameObject.transform);
                        RectTransform rectTransform = StaticCanvasScenes[i].transform as RectTransform;
                        rectTransform.anchorMin = Vector2.zero;
                        rectTransform.anchorMax = Vector2.one;
                        rectTransform.sizeDelta = Vector2.zero;
                    }

                    result = true;
                }

                break;
            }
            case CanvasBatchType.SemiStatic:
            {
                if (SemiStaticBatchLayerCanvas == null)
                {
                    GameObject canvasObject = new GameObject("Semi Static Batch Canvas");
                    SemiStaticBatchLayerCanvas = canvasObject.AddComponent<Canvas>();
                    SetupCanvas(
                        canvasObject,
                        ScenesContainer,
                        SemiStaticBatchLayerCanvas,
                        (LayerPriority + 1) * 60 - 30);
                    SemiStaticCanvasScenes = new GameObject[SceneDisplayInfos.Length];
                    for (int i = 0; i < SceneDisplayInfos.Length; i++)
                    {
                        SemiStaticCanvasScenes[i] = new GameObject(
                            "(SceneContainer)" + SceneDisplayInfos[i].SceneName,
                            typeof(RectTransform));
                        UIManager.ReparentTransform(
                            SemiStaticCanvasScenes[i].transform,
                            canvasObject.gameObject.transform);
                        RectTransform rectTransform = SemiStaticCanvasScenes[i].transform as RectTransform;
                        rectTransform.anchorMin = Vector2.zero;
                        rectTransform.anchorMax = Vector2.one;
                        rectTransform.sizeDelta = Vector2.zero;
                    }

                    result = true;
                }

                break;
            }
            case CanvasBatchType.CameraMovement:
            {
                if (CameraMovementBatchLayerCanvas == null)
                {
                    GameObject canvasObject = new GameObject("Camera Movement Batch Canvas");
                    CameraMovementBatchLayerCanvas = canvasObject.AddComponent<Canvas>();
                    SetupCanvas(
                        canvasObject,
                        ScenesContainer,
                        CameraMovementBatchLayerCanvas,
                        (LayerPriority + 1) * 60 - 20);
                    CameraMovementCanvasScenes = new GameObject[SceneDisplayInfos.Length];
                    for (int i = 0; i < SceneDisplayInfos.Length; i++)
                    {
                        CameraMovementCanvasScenes[i] = new GameObject(
                            "(SceneContainer)" + SceneDisplayInfos[i].SceneName,
                            typeof(RectTransform));
                        UIManager.ReparentTransform(
                            CameraMovementCanvasScenes[i].transform,
                            canvasObject.gameObject.transform);
                        RectTransform rectTransform = CameraMovementCanvasScenes[i].transform as RectTransform;
                        rectTransform.anchorMin = Vector2.zero;
                        rectTransform.anchorMax = Vector2.one;
                        rectTransform.sizeDelta = Vector2.zero;
                    }

                    result = true;
                }

                break;
            }
            case CanvasBatchType.PerFrame:
            {
                if (PerFrameBatchLayerCanvas == null)
                {
                    GameObject canvasObject = new GameObject("Per Frame Batch Canvas");
                    PerFrameBatchLayerCanvas = canvasObject.AddComponent<Canvas>();
                    SetupCanvas(canvasObject, ScenesContainer, PerFrameBatchLayerCanvas, (LayerPriority + 1) * 60 - 10);
                    PerFrameCanvasScenes = new GameObject[SceneDisplayInfos.Length];
                    for (int i = 0; i < SceneDisplayInfos.Length; i++)
                    {
                        PerFrameCanvasScenes[i] = new GameObject(
                            "(SceneContainer)" + SceneDisplayInfos[i].SceneName,
                            typeof(RectTransform));
                        UIManager.ReparentTransform(
                            PerFrameCanvasScenes[i].transform,
                            canvasObject.gameObject.transform);
                        RectTransform rectTransform = PerFrameCanvasScenes[i].transform as RectTransform;
                        rectTransform.anchorMin = Vector2.zero;
                        rectTransform.anchorMax = Vector2.one;
                        rectTransform.sizeDelta = Vector2.zero;
                    }

                    result = true;
                }

                break;
            }
        }

        return result;
    }

    private GameObject GetSceneContainer(CanvasBatchType batchType, SceneType sceneType)
    {
        GameObject result = null;
        foreach (RuntimeSceneInfo scene in Scenes)
        {
            if (scene.RuntimeScene.GetSceneType() != sceneType)
            {
                continue;
            }

            switch (batchType)
            {
                case CanvasBatchType.Static:
                    result = scene.RuntimeStaticSceneContainer;
                    break;
                case CanvasBatchType.SemiStatic:
                    result = scene.RuntimeSemiStaticSceneContainer;
                    break;
                case CanvasBatchType.CameraMovement:
                    result = scene.RuntimeCameraMovementSceneContainer;
                    break;
                case CanvasBatchType.PerFrame:
                    result = scene.RuntimePerFrameSceneContainer;
                    break;
            }
        }

        return result;
    }

    private void AddBatchObjectToCanvas(_CanvasBatchingObject batchObject)
    {
        if (batchObject == null)
        {
            return;
        }

        if (CreateCanvasBatchType(batchObject.m_BatchType))
        {
            foreach (RuntimeSceneInfo scene in Scenes)
            {
                for (int i = 0; i < SceneDisplayInfos.Length; i++)
                {
                    if (SceneDisplayInfos[i].m_SceneType != scene.RuntimeScene.GetSceneType())
                    {
                        continue;
                    }

                    switch (batchObject.m_BatchType)
                    {
                        case CanvasBatchType.Static:
                            scene.RuntimeStaticSceneContainer = StaticCanvasScenes[i];
                            break;
                        case CanvasBatchType.SemiStatic:
                            scene.RuntimeSemiStaticSceneContainer = SemiStaticCanvasScenes[i];
                            break;
                        case CanvasBatchType.CameraMovement:
                            scene.RuntimeCameraMovementSceneContainer = CameraMovementCanvasScenes[i];
                            break;
                        case CanvasBatchType.PerFrame:
                            scene.RuntimePerFrameSceneContainer = PerFrameCanvasScenes[i];
                            break;
                    }

                    break;
                }
            }
        }

        GameObject sceneContainer = GetSceneContainer(batchObject.m_BatchType, batchObject.m_SceneType);
        UIManager.ReparentTransform(batchObject.gameObject.transform, sceneContainer.transform);
        RectTransform gameObjectTransform = batchObject.gameObject.transform as RectTransform;
        gameObjectTransform.anchorMin = Vector2.zero;
        gameObjectTransform.anchorMax = Vector2.one;
        gameObjectTransform.sizeDelta = Vector2.zero;
    }

    private void ExtractBatchingObjects(RuntimeSceneInfo sceneInfo)
    {
        if (m_parentInfo.ParentInfo.CamType == RenderMode.WorldSpace)
        {
            return;
        }

        Transform[] sceneContainers = sceneInfo.RuntimeScene.GetSceneContainers();
        List<_CanvasBatchingObject> list = new List<_CanvasBatchingObject>();
        foreach (Transform sceneContainer in sceneContainers)
        {
            _CanvasBatchingObject[] componentsInChildren =
                sceneContainer.GetComponentsInChildren<_CanvasBatchingObject>();
            list.AddRange(componentsInChildren);
        }

        foreach (_CanvasBatchingObject current in list)
        {
            current.m_SceneType = sceneInfo.RuntimeScene.GetSceneType();
            AddBatchObjectToCanvas(current);
        }
    }

    private void UpdateBatchScenes(RuntimeSceneInfo info, int index)
    {
        if (StaticCanvasScenes != null)
        {
            info.RuntimeStaticSceneContainer = StaticCanvasScenes[index];
        }

        if (SemiStaticCanvasScenes != null)
        {
            info.RuntimeSemiStaticSceneContainer = SemiStaticCanvasScenes[index];
        }

        if (CameraMovementCanvasScenes != null)
        {
            info.RuntimeCameraMovementSceneContainer = CameraMovementCanvasScenes[index];
        }

        if (PerFrameCanvasScenes != null)
        {
            info.RuntimePerFrameSceneContainer = PerFrameCanvasScenes[index];
        }
    }

    public Canvas GetBatchCanvas(IUIScene theScene, CanvasBatchType type)
    {
        foreach (RuntimeSceneInfo scene in Scenes)
        {
            if (scene.RuntimeScene.GetSceneType() != theScene.GetSceneType())
            {
                continue;
            }

            switch (type)
            {
                case CanvasBatchType.Static:
                    return StaticBatchLayerCanvas;
                case CanvasBatchType.SemiStatic:
                    return SemiStaticBatchLayerCanvas;
                case CanvasBatchType.CameraMovement:
                    return CameraMovementBatchLayerCanvas;
                case CanvasBatchType.PerFrame:
                    return PerFrameBatchLayerCanvas;
            }
        }

        return null;
    }

    public Canvas GetDefaultCanvas(IUIScene theScene)
    {
        foreach (RuntimeSceneInfo scene in Scenes)
        {
            if (scene.RuntimeScene.GetSceneType() == theScene.GetSceneType())
            {
                return DefaultLayerCanvas;
            }
        }

        return null;
    }

    public Canvas GetDefaultCanvas(SceneType theScene)
    {
        foreach (RuntimeSceneInfo scene in Scenes)
        {
            if (scene.RuntimeScene.GetSceneType() == theScene)
            {
                return DefaultLayerCanvas;
            }
        }

        return null;
    }

    public int GetNameplateCanvasLayer()
    {
        foreach (RuntimeSceneInfo scene in Scenes)
        {
            if (scene.RuntimeScene.GetSceneType() != SceneType.HUD
                || scene.RuntimeCameraMovementSceneContainer == null)
            {
                continue;
            }

            Canvas canvas = scene.RuntimeCameraMovementSceneContainer.GetComponent<Canvas>();
            if (canvas != null)
            {
                return canvas.sortingOrder;
            }
        }

        return -1;
    }

    public RuntimeSceneInfo RegisterUIScene(IUIScene scene)
    {
        for (int i = 0; i < SceneDisplayInfos.Length; i++)
        {
            if (SceneDisplayInfos[i].m_SceneType != scene.GetSceneType())
            {
                continue;
            }

            RuntimeSceneInfo runtimeSceneInfo = new RuntimeSceneInfo
            {
                DisplayInfo = SceneDisplayInfos[i],
                RuntimeScene = scene,
                RuntimeSceneContainer = DefaultCanvasScenes[i]
            };

            UpdateBatchScenes(runtimeSceneInfo, i);
            Scenes.Add(runtimeSceneInfo);
            ExtractBatchingObjects(runtimeSceneInfo);

            foreach (Transform sceneContainer in scene.GetSceneContainers())
            {
                UIManager.ReparentTransform(sceneContainer, runtimeSceneInfo.RuntimeSceneContainer.transform);
                if (ParentInfo.ParentInfo.CamType != RenderMode.WorldSpace)
                {
                    RectTransform rectTransform = sceneContainer as RectTransform;
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.sizeDelta = Vector2.zero;
                }
            }

            return runtimeSceneInfo;
        }

        return null;
    }

    public void Init(UILayerManager parentInfo)
    {
        if (init)
        {
            return;
        }

        init = true;
        m_parentInfo = parentInfo;
        DefaultCanvasScenes = new GameObject[SceneDisplayInfos.Length];
        DefaultWorldContainer = new GameObject();
        DefaultWorldContainer.name = "Default Canvas";
        UIManager.ReparentTransform(DefaultWorldContainer.transform, ScenesContainer.gameObject.transform);

        bool isWorldSpace = m_parentInfo.ParentInfo.CamType == RenderMode.WorldSpace;
        if (!isWorldSpace)
        {
            DefaultLayerCanvas = DefaultWorldContainer.AddComponent<Canvas>();
            DefaultLayerCanvas.worldCamera = m_parentInfo.ParentInfo.ActiveCamera;
            DefaultLayerCanvas.renderMode = m_parentInfo.ParentInfo.CamType;
            DefaultLayerCanvas.sortingOrder = (LayerPriority + 1) * 60;
            CanvasScaler canvasScaler = DefaultWorldContainer.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasScaler.matchWidthOrHeight = 0.5f;
            canvasScaler.referencePixelsPerUnit = 100f;
            GraphicRaycaster graphicRaycaster = DefaultWorldContainer.AddComponent<GraphicRaycaster>();
            graphicRaycaster.ignoreReversedGraphics = true;
            graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
        }

        for (int i = 0; i < SceneDisplayInfos.Length; i++)
        {
            if (isWorldSpace)
            {
                DefaultCanvasScenes[i] = new GameObject("(SceneContainer)" + SceneDisplayInfos[i].SceneName);
                UIManager.ReparentTransform(
                    DefaultCanvasScenes[i].transform,
                    DefaultWorldContainer.gameObject.transform);
                continue;
            }

            DefaultCanvasScenes[i] = new GameObject(
                "(SceneContainer)" + SceneDisplayInfos[i].SceneName,
                typeof(RectTransform));
            UIManager.ReparentTransform(DefaultCanvasScenes[i].transform, DefaultLayerCanvas.gameObject.transform);
            RectTransform rectTransform = DefaultCanvasScenes[i].transform as RectTransform;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
        }
    }

    public List<UISceneDisplayInfo> SetGameState(UIManager.ClientState newState)
    {
        if (m_parentInfo.ParentInfo.CamType != RenderMode.WorldSpace)
        {
            DefaultLayerCanvas.worldCamera = m_parentInfo.ParentInfo.ActiveCamera;
            if (StaticBatchLayerCanvas != null)
            {
                StaticBatchLayerCanvas.worldCamera = m_parentInfo.ParentInfo.ActiveCamera;
            }

            if (SemiStaticBatchLayerCanvas != null)
            {
                SemiStaticBatchLayerCanvas.worldCamera = m_parentInfo.ParentInfo.ActiveCamera;
            }

            if (CameraMovementBatchLayerCanvas != null)
            {
                CameraMovementBatchLayerCanvas.worldCamera = m_parentInfo.ParentInfo.ActiveCamera;
            }

            if (PerFrameBatchLayerCanvas != null)
            {
                PerFrameBatchLayerCanvas.worldCamera = m_parentInfo.ParentInfo.ActiveCamera;
            }
        }

        List<UISceneDisplayInfo> result = new List<UISceneDisplayInfo>();
        foreach (UISceneDisplayInfo sceneDisplayInfo in SceneDisplayInfos)
        {
            if ((newState != UIManager.ClientState.InGame || !sceneDisplayInfo.m_InGame)
                && (newState != UIManager.ClientState.InFrontEnd || !sceneDisplayInfo.m_InFrontEnd))
            {
                continue;
            }

            if (sceneDisplayInfo.m_SceneType != SceneType.TestScene)
            {
                result.Add(sceneDisplayInfo);
            }
        }

        return result;
    }
}