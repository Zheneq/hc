using System;
using System.Collections.Generic;
using System.Text;
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

	public UILayerManager ParentInfo
	{
		get { return m_parentInfo; }
	}

	public int SetSceneVisible(IEnumerable<SceneType> aScenes, bool visible, SceneVisibilityParameters parameters)
	{
		int num = 0;
		List<SceneType> list = new List<SceneType>(aScenes);
		if (list.Count == 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return num;
				}
			}
		}
		for (int i = 0; i < Scenes.Count; i++)
		{
			if (list.Contains(Scenes[i].RuntimeScene.GetSceneType()))
			{
				Scenes[i].RuntimeScene.SetVisible(visible, parameters);
				Scenes[i].SetBatchScenesVisible(visible);
				num++;
			}
			else if (parameters.TurnOffAllOtherScenesInCanvasLayer)
			{
				Scenes[i].RuntimeScene.SetVisible(false, parameters);
				Scenes[i].SetBatchScenesVisible(false);
			}
		}
		while (true)
		{
			return num;
		}
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
		if (type == CanvasBatchType.Static)
		{
			if (StaticBatchLayerCanvas == null)
			{
				GameObject gameObject = new GameObject("Static Batch Canvas");
				StaticBatchLayerCanvas = gameObject.AddComponent<Canvas>();
				SetupCanvas(gameObject, ScenesContainer, StaticBatchLayerCanvas, (LayerPriority + 1) * 60 - 40);
				StaticCanvasScenes = new GameObject[SceneDisplayInfos.Length];
				for (int i = 0; i < SceneDisplayInfos.Length; i++)
				{
					StaticCanvasScenes[i] = new GameObject(new StringBuilder().Append("(SceneContainer)").Append(SceneDisplayInfos[i].SceneName).ToString(), typeof(RectTransform));
					UIManager.ReparentTransform(StaticCanvasScenes[i].transform, gameObject.gameObject.transform);
					(StaticCanvasScenes[i].transform as RectTransform).anchorMin = Vector2.zero;
					(StaticCanvasScenes[i].transform as RectTransform).anchorMax = Vector2.one;
					(StaticCanvasScenes[i].transform as RectTransform).sizeDelta = Vector2.zero;
				}
				result = true;
			}
		}
		else if (type == CanvasBatchType.SemiStatic)
		{
			if (SemiStaticBatchLayerCanvas == null)
			{
				GameObject gameObject2 = new GameObject("Semi Static Batch Canvas");
				SemiStaticBatchLayerCanvas = gameObject2.AddComponent<Canvas>();
				SetupCanvas(gameObject2, ScenesContainer, SemiStaticBatchLayerCanvas, (LayerPriority + 1) * 60 - 30);
				SemiStaticCanvasScenes = new GameObject[SceneDisplayInfos.Length];
				for (int j = 0; j < SceneDisplayInfos.Length; j++)
				{
					SemiStaticCanvasScenes[j] = new GameObject(new StringBuilder().Append("(SceneContainer)").Append(SceneDisplayInfos[j].SceneName).ToString(), typeof(RectTransform));
					UIManager.ReparentTransform(SemiStaticCanvasScenes[j].transform, gameObject2.gameObject.transform);
					(SemiStaticCanvasScenes[j].transform as RectTransform).anchorMin = Vector2.zero;
					(SemiStaticCanvasScenes[j].transform as RectTransform).anchorMax = Vector2.one;
					(SemiStaticCanvasScenes[j].transform as RectTransform).sizeDelta = Vector2.zero;
				}
				result = true;
			}
		}
		else if (type == CanvasBatchType.CameraMovement)
		{
			if (CameraMovementBatchLayerCanvas == null)
			{
				GameObject gameObject3 = new GameObject("Camera Movement Batch Canvas");
				CameraMovementBatchLayerCanvas = gameObject3.AddComponent<Canvas>();
				SetupCanvas(gameObject3, ScenesContainer, CameraMovementBatchLayerCanvas, (LayerPriority + 1) * 60 - 20);
				CameraMovementCanvasScenes = new GameObject[SceneDisplayInfos.Length];
				for (int k = 0; k < SceneDisplayInfos.Length; k++)
				{
					CameraMovementCanvasScenes[k] = new GameObject(new StringBuilder().Append("(SceneContainer)").Append(SceneDisplayInfos[k].SceneName).ToString(), typeof(RectTransform));
					UIManager.ReparentTransform(CameraMovementCanvasScenes[k].transform, gameObject3.gameObject.transform);
					(CameraMovementCanvasScenes[k].transform as RectTransform).anchorMin = Vector2.zero;
					(CameraMovementCanvasScenes[k].transform as RectTransform).anchorMax = Vector2.one;
					(CameraMovementCanvasScenes[k].transform as RectTransform).sizeDelta = Vector2.zero;
				}
				result = true;
			}
		}
		else if (type == CanvasBatchType.PerFrame)
		{
			if (PerFrameBatchLayerCanvas == null)
			{
				GameObject gameObject4 = new GameObject("Per Frame Batch Canvas");
				PerFrameBatchLayerCanvas = gameObject4.AddComponent<Canvas>();
				SetupCanvas(gameObject4, ScenesContainer, PerFrameBatchLayerCanvas, (LayerPriority + 1) * 60 - 10);
				PerFrameCanvasScenes = new GameObject[SceneDisplayInfos.Length];
				for (int l = 0; l < SceneDisplayInfos.Length; l++)
				{
					PerFrameCanvasScenes[l] = new GameObject(new StringBuilder().Append("(SceneContainer)").Append(SceneDisplayInfos[l].SceneName).ToString(), typeof(RectTransform));
					UIManager.ReparentTransform(PerFrameCanvasScenes[l].transform, gameObject4.gameObject.transform);
					(PerFrameCanvasScenes[l].transform as RectTransform).anchorMin = Vector2.zero;
					(PerFrameCanvasScenes[l].transform as RectTransform).anchorMax = Vector2.one;
					(PerFrameCanvasScenes[l].transform as RectTransform).sizeDelta = Vector2.zero;
				}
				result = true;
			}
		}
		return result;
	}

	private GameObject GetSceneContainer(CanvasBatchType batchType, SceneType sceneType)
	{
		GameObject result = null;
		using (List<RuntimeSceneInfo>.Enumerator enumerator = Scenes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				RuntimeSceneInfo current = enumerator.Current;
				if (current.RuntimeScene.GetSceneType() == sceneType)
				{
					if (batchType == CanvasBatchType.Static)
					{
						result = current.RuntimeStaticSceneContainer;
					}
					else if (batchType == CanvasBatchType.SemiStatic)
					{
						result = current.RuntimeSemiStaticSceneContainer;
					}
					else if (batchType == CanvasBatchType.CameraMovement)
					{
						result = current.RuntimeCameraMovementSceneContainer;
					}
					else if (batchType == CanvasBatchType.PerFrame)
					{
						result = current.RuntimePerFrameSceneContainer;
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	private void AddBatchObjectToCanvas(_CanvasBatchingObject batchObject)
	{
		if (!(batchObject != null))
		{
			return;
		}
		while (true)
		{
			if (CreateCanvasBatchType(batchObject.m_BatchType))
			{
				using (List<RuntimeSceneInfo>.Enumerator enumerator = Scenes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RuntimeSceneInfo current = enumerator.Current;
						int num = 0;
						while (true)
						{
							if (num >= SceneDisplayInfos.Length)
							{
								break;
							}
							if (SceneDisplayInfos[num].m_SceneType == current.RuntimeScene.GetSceneType())
							{
								if (batchObject.m_BatchType == CanvasBatchType.Static)
								{
									current.RuntimeStaticSceneContainer = StaticCanvasScenes[num];
								}
								else if (batchObject.m_BatchType == CanvasBatchType.SemiStatic)
								{
									current.RuntimeSemiStaticSceneContainer = SemiStaticCanvasScenes[num];
								}
								else if (batchObject.m_BatchType == CanvasBatchType.CameraMovement)
								{
									current.RuntimeCameraMovementSceneContainer = CameraMovementCanvasScenes[num];
								}
								else if (batchObject.m_BatchType == CanvasBatchType.PerFrame)
								{
									current.RuntimePerFrameSceneContainer = PerFrameCanvasScenes[num];
								}
								break;
							}
							num++;
						}
					}
				}
			}
			GameObject sceneContainer = GetSceneContainer(batchObject.m_BatchType, batchObject.m_SceneType);
			UIManager.ReparentTransform(batchObject.gameObject.transform, sceneContainer.transform);
			(batchObject.gameObject.transform as RectTransform).anchorMin = Vector2.zero;
			(batchObject.gameObject.transform as RectTransform).anchorMax = Vector2.one;
			(batchObject.gameObject.transform as RectTransform).sizeDelta = Vector2.zero;
			return;
		}
	}

	private void ExtractBatchingObjects(RuntimeSceneInfo sceneInfo)
	{
		if (m_parentInfo.ParentInfo.CamType == RenderMode.WorldSpace)
		{
			return;
		}
		while (true)
		{
			Transform[] sceneContainers = sceneInfo.RuntimeScene.GetSceneContainers();
			List<_CanvasBatchingObject> list = new List<_CanvasBatchingObject>();
			for (int i = 0; i < sceneContainers.Length; i++)
			{
				_CanvasBatchingObject[] componentsInChildren = sceneContainers[i].GetComponentsInChildren<_CanvasBatchingObject>();
				list.AddRange(componentsInChildren);
			}
			while (true)
			{
				using (List<_CanvasBatchingObject>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						_CanvasBatchingObject current = enumerator.Current;
						current.m_SceneType = sceneInfo.RuntimeScene.GetSceneType();
						AddBatchObjectToCanvas(current);
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
		if (PerFrameCanvasScenes == null)
		{
			return;
		}
		while (true)
		{
			info.RuntimePerFrameSceneContainer = PerFrameCanvasScenes[index];
			return;
		}
	}

	public Canvas GetBatchCanvas(IUIScene theScene, CanvasBatchType type)
	{
		for (int i = 0; i < Scenes.Count; i++)
		{
			if (Scenes[i].RuntimeScene.GetSceneType() != theScene.GetSceneType())
			{
				continue;
			}
			if (type == CanvasBatchType.Static)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return StaticBatchLayerCanvas;
					}
				}
			}
			if (type == CanvasBatchType.SemiStatic)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return SemiStaticBatchLayerCanvas;
					}
				}
			}
			switch (type)
			{
			case CanvasBatchType.CameraMovement:
				while (true)
				{
					return CameraMovementBatchLayerCanvas;
				}
			case CanvasBatchType.PerFrame:
				return PerFrameBatchLayerCanvas;
			}
		}
		while (true)
		{
			return null;
		}
	}

	public Canvas GetDefaultCanvas(IUIScene theScene)
	{
		for (int i = 0; i < Scenes.Count; i++)
		{
			if (Scenes[i].RuntimeScene.GetSceneType() != theScene.GetSceneType())
			{
				continue;
			}
			while (true)
			{
				return DefaultLayerCanvas;
			}
		}
		return null;
	}

	public Canvas GetDefaultCanvas(SceneType theScene)
	{
		for (int i = 0; i < Scenes.Count; i++)
		{
			if (Scenes[i].RuntimeScene.GetSceneType() != theScene)
			{
				continue;
			}
			while (true)
			{
				return DefaultLayerCanvas;
			}
		}
		while (true)
		{
			return null;
		}
	}

	public int GetNameplateCanvasLayer()
	{
		int result = -1;
		int num = 0;
		while (true)
		{
			if (num < Scenes.Count)
			{
				if (Scenes[num].RuntimeScene.GetSceneType() == SceneType.HUD)
				{
					if (Scenes[num].RuntimeCameraMovementSceneContainer != null)
					{
						Canvas component = Scenes[num].RuntimeCameraMovementSceneContainer.GetComponent<Canvas>();
						if (component != null)
						{
							result = component.sortingOrder;
							break;
						}
					}
				}
				num++;
				continue;
			}
			break;
		}
		return result;
	}

	public RuntimeSceneInfo RegisterUIScene(IUIScene scene)
	{
		for (int i = 0; i < SceneDisplayInfos.Length; i++)
		{
			if (SceneDisplayInfos[i].m_SceneType != scene.GetSceneType())
			{
				continue;
			}
			while (true)
			{
				RuntimeSceneInfo runtimeSceneInfo = new RuntimeSceneInfo();
				runtimeSceneInfo.DisplayInfo = SceneDisplayInfos[i];
				runtimeSceneInfo.RuntimeScene = scene;
				runtimeSceneInfo.RuntimeSceneContainer = DefaultCanvasScenes[i];
				UpdateBatchScenes(runtimeSceneInfo, i);
				Scenes.Add(runtimeSceneInfo);
				ExtractBatchingObjects(runtimeSceneInfo);
				Transform[] sceneContainers = scene.GetSceneContainers();
				for (int j = 0; j < sceneContainers.Length; j++)
				{
					UIManager.ReparentTransform(sceneContainers[j], runtimeSceneInfo.RuntimeSceneContainer.transform);
					if (ParentInfo.ParentInfo.CamType != RenderMode.WorldSpace)
					{
						(sceneContainers[j] as RectTransform).anchorMin = Vector2.zero;
						(sceneContainers[j] as RectTransform).anchorMax = Vector2.one;
						(sceneContainers[j] as RectTransform).sizeDelta = Vector2.zero;
					}
				}
				while (true)
				{
					return runtimeSceneInfo;
				}
			}
		}
		return null;
	}

	public void Init(UILayerManager parentInfo)
	{
		if (init)
		{
			return;
		}
		while (true)
		{
			init = true;
			m_parentInfo = parentInfo;
			DefaultCanvasScenes = new GameObject[SceneDisplayInfos.Length];
			DefaultWorldContainer = new GameObject();
			DefaultWorldContainer.name = "Default Canvas";
			UIManager.ReparentTransform(DefaultWorldContainer.transform, ScenesContainer.gameObject.transform);
			bool flag = m_parentInfo.ParentInfo.CamType == RenderMode.WorldSpace;
			if (!flag)
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
				if (flag)
				{
					DefaultCanvasScenes[i] = new GameObject(new StringBuilder().Append("(SceneContainer)").Append(SceneDisplayInfos[i].SceneName).ToString());
					UIManager.ReparentTransform(DefaultCanvasScenes[i].transform, DefaultWorldContainer.gameObject.transform);
					continue;
				}
				DefaultCanvasScenes[i] = new GameObject(new StringBuilder().Append("(SceneContainer)").Append(SceneDisplayInfos[i].SceneName).ToString(), typeof(RectTransform));
				UIManager.ReparentTransform(DefaultCanvasScenes[i].transform, DefaultLayerCanvas.gameObject.transform);
				(DefaultCanvasScenes[i].transform as RectTransform).anchorMin = Vector2.zero;
				(DefaultCanvasScenes[i].transform as RectTransform).anchorMax = Vector2.one;
				(DefaultCanvasScenes[i].transform as RectTransform).sizeDelta = Vector2.zero;
			}
			return;
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
		List<UISceneDisplayInfo> list = new List<UISceneDisplayInfo>();
		for (int i = 0; i < SceneDisplayInfos.Length; i++)
		{
			if (newState == UIManager.ClientState.InGame)
			{
				if (SceneDisplayInfos[i].m_InGame)
				{
					goto IL_0161;
				}
			}
			if (newState != 0 || !SceneDisplayInfos[i].m_InFrontEnd)
			{
				continue;
			}
			goto IL_0161;
			IL_0161:
			if (SceneDisplayInfos[i].m_SceneType != SceneType.TestScene)
			{
				list.Add(SceneDisplayInfos[i]);
			}
		}
		while (true)
		{
			return list;
		}
	}
}
