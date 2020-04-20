﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CanvasLayerInfo
{
	public string CanvasLayerName;

	public bool OnlyOneSceneActiveAtATime;

	public int LayerPriority;

	public const int PaddingBetweenLayers = 0x3C;

	public const int PaddingBetweenBatchCanvases = 0xA;

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
		get
		{
			return this.m_parentInfo;
		}
	}

	public int SetSceneVisible(IEnumerable<SceneType> aScenes, bool visible, SceneVisibilityParameters parameters)
	{
		int num = 0;
		List<SceneType> list = new List<SceneType>(aScenes);
		if (list.Count == 0)
		{
			return num;
		}
		for (int i = 0; i < this.Scenes.Count; i++)
		{
			if (list.Contains(this.Scenes[i].RuntimeScene.GetSceneType()))
			{
				this.Scenes[i].RuntimeScene.SetVisible(visible, parameters);
				this.Scenes[i].SetBatchScenesVisible(visible);
				num++;
			}
			else if (parameters.TurnOffAllOtherScenesInCanvasLayer)
			{
				this.Scenes[i].RuntimeScene.SetVisible(false, parameters);
				this.Scenes[i].SetBatchScenesVisible(false);
			}
		}
		return num;
	}

	private void SetupCanvas(GameObject container, GameObject parent, Canvas aCanvas, int canvasLayerOrder)
	{
		UIManager.ReparentTransform(container.transform, parent.transform);
		aCanvas.worldCamera = this.m_parentInfo.ParentInfo.ActiveCamera;
		aCanvas.renderMode = this.m_parentInfo.ParentInfo.CamType;
		aCanvas.sortingOrder = canvasLayerOrder;
		CanvasScaler canvasScaler = container.AddComponent<CanvasScaler>();
		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
		canvasScaler.matchWidthOrHeight = 0.5f;
		canvasScaler.referencePixelsPerUnit = 100f;
		GraphicRaycaster graphicRaycaster = container.AddComponent<GraphicRaycaster>();
		graphicRaycaster.ignoreReversedGraphics = true;
		graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
		container.SetLayerRecursively(this.m_parentInfo.ObjectLayerValue);
	}

	private bool CreateCanvasBatchType(CanvasBatchType type)
	{
		bool result = false;
		if (type == CanvasBatchType.Static)
		{
			if (this.StaticBatchLayerCanvas == null)
			{
				GameObject gameObject = new GameObject("Static Batch Canvas");
				this.StaticBatchLayerCanvas = gameObject.AddComponent<Canvas>();
				this.SetupCanvas(gameObject, this.ScenesContainer, this.StaticBatchLayerCanvas, (this.LayerPriority + 1) * 0x3C - 0x28);
				this.StaticCanvasScenes = new GameObject[this.SceneDisplayInfos.Length];
				for (int i = 0; i < this.SceneDisplayInfos.Length; i++)
				{
					this.StaticCanvasScenes[i] = new GameObject("(SceneContainer)" + this.SceneDisplayInfos[i].SceneName, new Type[]
					{
						typeof(RectTransform)
					});
					UIManager.ReparentTransform(this.StaticCanvasScenes[i].transform, gameObject.gameObject.transform);
					(this.StaticCanvasScenes[i].transform as RectTransform).anchorMin = Vector2.zero;
					(this.StaticCanvasScenes[i].transform as RectTransform).anchorMax = Vector2.one;
					(this.StaticCanvasScenes[i].transform as RectTransform).sizeDelta = Vector2.zero;
				}
				result = true;
			}
		}
		else if (type == CanvasBatchType.SemiStatic)
		{
			if (this.SemiStaticBatchLayerCanvas == null)
			{
				GameObject gameObject2 = new GameObject("Semi Static Batch Canvas");
				this.SemiStaticBatchLayerCanvas = gameObject2.AddComponent<Canvas>();
				this.SetupCanvas(gameObject2, this.ScenesContainer, this.SemiStaticBatchLayerCanvas, (this.LayerPriority + 1) * 0x3C - 0x1E);
				this.SemiStaticCanvasScenes = new GameObject[this.SceneDisplayInfos.Length];
				for (int j = 0; j < this.SceneDisplayInfos.Length; j++)
				{
					this.SemiStaticCanvasScenes[j] = new GameObject("(SceneContainer)" + this.SceneDisplayInfos[j].SceneName, new Type[]
					{
						typeof(RectTransform)
					});
					UIManager.ReparentTransform(this.SemiStaticCanvasScenes[j].transform, gameObject2.gameObject.transform);
					(this.SemiStaticCanvasScenes[j].transform as RectTransform).anchorMin = Vector2.zero;
					(this.SemiStaticCanvasScenes[j].transform as RectTransform).anchorMax = Vector2.one;
					(this.SemiStaticCanvasScenes[j].transform as RectTransform).sizeDelta = Vector2.zero;
				}
				result = true;
			}
		}
		else if (type == CanvasBatchType.CameraMovement)
		{
			if (this.CameraMovementBatchLayerCanvas == null)
			{
				GameObject gameObject3 = new GameObject("Camera Movement Batch Canvas");
				this.CameraMovementBatchLayerCanvas = gameObject3.AddComponent<Canvas>();
				this.SetupCanvas(gameObject3, this.ScenesContainer, this.CameraMovementBatchLayerCanvas, (this.LayerPriority + 1) * 0x3C - 0x14);
				this.CameraMovementCanvasScenes = new GameObject[this.SceneDisplayInfos.Length];
				for (int k = 0; k < this.SceneDisplayInfos.Length; k++)
				{
					this.CameraMovementCanvasScenes[k] = new GameObject("(SceneContainer)" + this.SceneDisplayInfos[k].SceneName, new Type[]
					{
						typeof(RectTransform)
					});
					UIManager.ReparentTransform(this.CameraMovementCanvasScenes[k].transform, gameObject3.gameObject.transform);
					(this.CameraMovementCanvasScenes[k].transform as RectTransform).anchorMin = Vector2.zero;
					(this.CameraMovementCanvasScenes[k].transform as RectTransform).anchorMax = Vector2.one;
					(this.CameraMovementCanvasScenes[k].transform as RectTransform).sizeDelta = Vector2.zero;
				}
				result = true;
			}
		}
		else if (type == CanvasBatchType.PerFrame)
		{
			if (this.PerFrameBatchLayerCanvas == null)
			{
				GameObject gameObject4 = new GameObject("Per Frame Batch Canvas");
				this.PerFrameBatchLayerCanvas = gameObject4.AddComponent<Canvas>();
				this.SetupCanvas(gameObject4, this.ScenesContainer, this.PerFrameBatchLayerCanvas, (this.LayerPriority + 1) * 0x3C - 0xA);
				this.PerFrameCanvasScenes = new GameObject[this.SceneDisplayInfos.Length];
				for (int l = 0; l < this.SceneDisplayInfos.Length; l++)
				{
					this.PerFrameCanvasScenes[l] = new GameObject("(SceneContainer)" + this.SceneDisplayInfos[l].SceneName, new Type[]
					{
						typeof(RectTransform)
					});
					UIManager.ReparentTransform(this.PerFrameCanvasScenes[l].transform, gameObject4.gameObject.transform);
					(this.PerFrameCanvasScenes[l].transform as RectTransform).anchorMin = Vector2.zero;
					(this.PerFrameCanvasScenes[l].transform as RectTransform).anchorMax = Vector2.one;
					(this.PerFrameCanvasScenes[l].transform as RectTransform).sizeDelta = Vector2.zero;
				}
				result = true;
			}
		}
		return result;
	}

	private GameObject GetSceneContainer(CanvasBatchType batchType, SceneType sceneType)
	{
		GameObject result = null;
		using (List<RuntimeSceneInfo>.Enumerator enumerator = this.Scenes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				RuntimeSceneInfo runtimeSceneInfo = enumerator.Current;
				if (runtimeSceneInfo.RuntimeScene.GetSceneType() == sceneType)
				{
					if (batchType == CanvasBatchType.Static)
					{
						result = runtimeSceneInfo.RuntimeStaticSceneContainer;
					}
					else if (batchType == CanvasBatchType.SemiStatic)
					{
						result = runtimeSceneInfo.RuntimeSemiStaticSceneContainer;
					}
					else if (batchType == CanvasBatchType.CameraMovement)
					{
						result = runtimeSceneInfo.RuntimeCameraMovementSceneContainer;
					}
					else if (batchType == CanvasBatchType.PerFrame)
					{
						result = runtimeSceneInfo.RuntimePerFrameSceneContainer;
					}
				}
			}
		}
		return result;
	}

	private void AddBatchObjectToCanvas(_CanvasBatchingObject batchObject)
	{
		if (batchObject != null)
		{
			if (this.CreateCanvasBatchType(batchObject.m_BatchType))
			{
				using (List<RuntimeSceneInfo>.Enumerator enumerator = this.Scenes.GetEnumerator())
				{
					IL_124:
					while (enumerator.MoveNext())
					{
						RuntimeSceneInfo runtimeSceneInfo = enumerator.Current;
						for (int i = 0; i < this.SceneDisplayInfos.Length; i++)
						{
							if (this.SceneDisplayInfos[i].m_SceneType == runtimeSceneInfo.RuntimeScene.GetSceneType())
							{
								if (batchObject.m_BatchType == CanvasBatchType.Static)
								{
									runtimeSceneInfo.RuntimeStaticSceneContainer = this.StaticCanvasScenes[i];
								}
								else if (batchObject.m_BatchType == CanvasBatchType.SemiStatic)
								{
									runtimeSceneInfo.RuntimeSemiStaticSceneContainer = this.SemiStaticCanvasScenes[i];
								}
								else if (batchObject.m_BatchType == CanvasBatchType.CameraMovement)
								{
									runtimeSceneInfo.RuntimeCameraMovementSceneContainer = this.CameraMovementCanvasScenes[i];
								}
								else if (batchObject.m_BatchType == CanvasBatchType.PerFrame)
								{
									runtimeSceneInfo.RuntimePerFrameSceneContainer = this.PerFrameCanvasScenes[i];
								}
								goto IL_124;
							}
						}
					}
				}
			}
			GameObject sceneContainer = this.GetSceneContainer(batchObject.m_BatchType, batchObject.m_SceneType);
			UIManager.ReparentTransform(batchObject.gameObject.transform, sceneContainer.transform);
			(batchObject.gameObject.transform as RectTransform).anchorMin = Vector2.zero;
			(batchObject.gameObject.transform as RectTransform).anchorMax = Vector2.one;
			(batchObject.gameObject.transform as RectTransform).sizeDelta = Vector2.zero;
		}
	}

	private void ExtractBatchingObjects(RuntimeSceneInfo sceneInfo)
	{
		if (this.m_parentInfo.ParentInfo.CamType != RenderMode.WorldSpace)
		{
			Transform[] sceneContainers = sceneInfo.RuntimeScene.GetSceneContainers();
			List<_CanvasBatchingObject> list = new List<_CanvasBatchingObject>();
			for (int i = 0; i < sceneContainers.Length; i++)
			{
				_CanvasBatchingObject[] componentsInChildren = sceneContainers[i].GetComponentsInChildren<_CanvasBatchingObject>();
				list.AddRange(componentsInChildren);
			}
			using (List<_CanvasBatchingObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					_CanvasBatchingObject canvasBatchingObject = enumerator.Current;
					canvasBatchingObject.m_SceneType = sceneInfo.RuntimeScene.GetSceneType();
					this.AddBatchObjectToCanvas(canvasBatchingObject);
				}
			}
		}
	}

	private void UpdateBatchScenes(RuntimeSceneInfo info, int index)
	{
		if (this.StaticCanvasScenes != null)
		{
			info.RuntimeStaticSceneContainer = this.StaticCanvasScenes[index];
		}
		if (this.SemiStaticCanvasScenes != null)
		{
			info.RuntimeSemiStaticSceneContainer = this.SemiStaticCanvasScenes[index];
		}
		if (this.CameraMovementCanvasScenes != null)
		{
			info.RuntimeCameraMovementSceneContainer = this.CameraMovementCanvasScenes[index];
		}
		if (this.PerFrameCanvasScenes != null)
		{
			info.RuntimePerFrameSceneContainer = this.PerFrameCanvasScenes[index];
		}
	}

	public Canvas GetBatchCanvas(IUIScene theScene, CanvasBatchType type)
	{
		for (int i = 0; i < this.Scenes.Count; i++)
		{
			if (this.Scenes[i].RuntimeScene.GetSceneType() == theScene.GetSceneType())
			{
				if (type == CanvasBatchType.Static)
				{
					return this.StaticBatchLayerCanvas;
				}
				if (type == CanvasBatchType.SemiStatic)
				{
					return this.SemiStaticBatchLayerCanvas;
				}
				if (type == CanvasBatchType.CameraMovement)
				{
					return this.CameraMovementBatchLayerCanvas;
				}
				if (type == CanvasBatchType.PerFrame)
				{
					return this.PerFrameBatchLayerCanvas;
				}
			}
		}
		return null;
	}

	public Canvas GetDefaultCanvas(IUIScene theScene)
	{
		for (int i = 0; i < this.Scenes.Count; i++)
		{
			if (this.Scenes[i].RuntimeScene.GetSceneType() == theScene.GetSceneType())
			{
				return this.DefaultLayerCanvas;
			}
		}
		return null;
	}

	public Canvas GetDefaultCanvas(SceneType theScene)
	{
		for (int i = 0; i < this.Scenes.Count; i++)
		{
			if (this.Scenes[i].RuntimeScene.GetSceneType() == theScene)
			{
				return this.DefaultLayerCanvas;
			}
		}
		return null;
	}

	public int GetNameplateCanvasLayer()
	{
		int result = -1;
		for (int i = 0; i < this.Scenes.Count; i++)
		{
			if (this.Scenes[i].RuntimeScene.GetSceneType() == SceneType.HUD)
			{
				if (this.Scenes[i].RuntimeCameraMovementSceneContainer != null)
				{
					Canvas component = this.Scenes[i].RuntimeCameraMovementSceneContainer.GetComponent<Canvas>();
					if (component != null)
					{
						result = component.sortingOrder;
						return result;
					}
				}
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	public RuntimeSceneInfo RegisterUIScene(IUIScene scene)
	{
		for (int i = 0; i < this.SceneDisplayInfos.Length; i++)
		{
			if (this.SceneDisplayInfos[i].m_SceneType == scene.GetSceneType())
			{
				RuntimeSceneInfo runtimeSceneInfo = new RuntimeSceneInfo();
				runtimeSceneInfo.DisplayInfo = this.SceneDisplayInfos[i];
				runtimeSceneInfo.RuntimeScene = scene;
				runtimeSceneInfo.RuntimeSceneContainer = this.DefaultCanvasScenes[i];
				this.UpdateBatchScenes(runtimeSceneInfo, i);
				this.Scenes.Add(runtimeSceneInfo);
				this.ExtractBatchingObjects(runtimeSceneInfo);
				Transform[] sceneContainers = scene.GetSceneContainers();
				for (int j = 0; j < sceneContainers.Length; j++)
				{
					UIManager.ReparentTransform(sceneContainers[j], runtimeSceneInfo.RuntimeSceneContainer.transform);
					if (this.ParentInfo.ParentInfo.CamType != RenderMode.WorldSpace)
					{
						(sceneContainers[j] as RectTransform).anchorMin = Vector2.zero;
						(sceneContainers[j] as RectTransform).anchorMax = Vector2.one;
						(sceneContainers[j] as RectTransform).sizeDelta = Vector2.zero;
					}
				}
				return runtimeSceneInfo;
			}
		}
		return null;
	}

	public void Init(UILayerManager parentInfo)
	{
		if (!this.init)
		{
			this.init = true;
			this.m_parentInfo = parentInfo;
			this.DefaultCanvasScenes = new GameObject[this.SceneDisplayInfos.Length];
			this.DefaultWorldContainer = new GameObject();
			this.DefaultWorldContainer.name = "Default Canvas";
			UIManager.ReparentTransform(this.DefaultWorldContainer.transform, this.ScenesContainer.gameObject.transform);
			bool flag = this.m_parentInfo.ParentInfo.CamType == RenderMode.WorldSpace;
			if (!flag)
			{
				this.DefaultLayerCanvas = this.DefaultWorldContainer.AddComponent<Canvas>();
				this.DefaultLayerCanvas.worldCamera = this.m_parentInfo.ParentInfo.ActiveCamera;
				this.DefaultLayerCanvas.renderMode = this.m_parentInfo.ParentInfo.CamType;
				this.DefaultLayerCanvas.sortingOrder = (this.LayerPriority + 1) * 0x3C;
				CanvasScaler canvasScaler = this.DefaultWorldContainer.AddComponent<CanvasScaler>();
				canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
				canvasScaler.matchWidthOrHeight = 0.5f;
				canvasScaler.referencePixelsPerUnit = 100f;
				GraphicRaycaster graphicRaycaster = this.DefaultWorldContainer.AddComponent<GraphicRaycaster>();
				graphicRaycaster.ignoreReversedGraphics = true;
				graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
			}
			for (int i = 0; i < this.SceneDisplayInfos.Length; i++)
			{
				if (flag)
				{
					this.DefaultCanvasScenes[i] = new GameObject("(SceneContainer)" + this.SceneDisplayInfos[i].SceneName);
					UIManager.ReparentTransform(this.DefaultCanvasScenes[i].transform, this.DefaultWorldContainer.gameObject.transform);
				}
				else
				{
					this.DefaultCanvasScenes[i] = new GameObject("(SceneContainer)" + this.SceneDisplayInfos[i].SceneName, new Type[]
					{
						typeof(RectTransform)
					});
					UIManager.ReparentTransform(this.DefaultCanvasScenes[i].transform, this.DefaultLayerCanvas.gameObject.transform);
					(this.DefaultCanvasScenes[i].transform as RectTransform).anchorMin = Vector2.zero;
					(this.DefaultCanvasScenes[i].transform as RectTransform).anchorMax = Vector2.one;
					(this.DefaultCanvasScenes[i].transform as RectTransform).sizeDelta = Vector2.zero;
				}
			}
		}
	}

	public List<UISceneDisplayInfo> SetGameState(UIManager.ClientState newState)
	{
		if (this.m_parentInfo.ParentInfo.CamType != RenderMode.WorldSpace)
		{
			this.DefaultLayerCanvas.worldCamera = this.m_parentInfo.ParentInfo.ActiveCamera;
			if (this.StaticBatchLayerCanvas != null)
			{
				this.StaticBatchLayerCanvas.worldCamera = this.m_parentInfo.ParentInfo.ActiveCamera;
			}
			if (this.SemiStaticBatchLayerCanvas != null)
			{
				this.SemiStaticBatchLayerCanvas.worldCamera = this.m_parentInfo.ParentInfo.ActiveCamera;
			}
			if (this.CameraMovementBatchLayerCanvas != null)
			{
				this.CameraMovementBatchLayerCanvas.worldCamera = this.m_parentInfo.ParentInfo.ActiveCamera;
			}
			if (this.PerFrameBatchLayerCanvas != null)
			{
				this.PerFrameBatchLayerCanvas.worldCamera = this.m_parentInfo.ParentInfo.ActiveCamera;
			}
		}
		List<UISceneDisplayInfo> list = new List<UISceneDisplayInfo>();
		int i = 0;
		while (i < this.SceneDisplayInfos.Length)
		{
			if (newState != UIManager.ClientState.InGame)
			{
				goto IL_145;
			}
			if (!this.SceneDisplayInfos[i].m_InGame)
			{
				goto IL_145;
			}
			goto IL_161;
			IL_18C:
			i++;
			continue;
			IL_145:
			if (newState != UIManager.ClientState.InFrontEnd || !this.SceneDisplayInfos[i].m_InFrontEnd)
			{
				goto IL_18C;
			}
			IL_161:
			if (this.SceneDisplayInfos[i].m_SceneType != SceneType.TestScene)
			{
				list.Add(this.SceneDisplayInfos[i]);
				goto IL_18C;
			}
			goto IL_18C;
		}
		return list;
	}
}
