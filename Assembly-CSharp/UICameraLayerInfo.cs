using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UICameraLayerInfo
{
	public string LayerName;

	public int Priority;

	public CameraLayerName LayerType;

	public Camera RenderCameraPrefab;

	public Camera RenderInGameCameraPrefab;

	public RenderMode CamType;

	[HideInInspector]
	public Camera ActiveCamera;

	public GameObject CameraLayerContainer;

	public UILayerManager LayerManager;

	private bool init;

	private bool ActiveCameraIsForInGame;

	private bool CameraUsedInGame;

	private bool CameraUsedInFrontEnd;

	public int SetSceneVisible(IEnumerable<SceneType> aScenes, bool visible, SceneVisibilityParameters parameters)
	{
		return this.LayerManager.SetSceneVisible(aScenes, visible, parameters);
	}

	public void Init()
	{
		if (!this.init)
		{
			this.init = true;
			this.ActiveCameraIsForInGame = false;
			this.InstantiateCamera();
			this.LayerManager.Init(this);
			for (int i = 0; i < this.LayerManager.CanvasLayers.Length; i++)
			{
				for (int j = 0; j < this.LayerManager.CanvasLayers[i].SceneDisplayInfos.Length; j++)
				{
					this.CameraUsedInGame = (this.CameraUsedInGame || this.LayerManager.CanvasLayers[i].SceneDisplayInfos[j].m_InGame);
					this.CameraUsedInFrontEnd = (this.CameraUsedInFrontEnd || this.LayerManager.CanvasLayers[i].SceneDisplayInfos[j].m_InFrontEnd);
				}
			}
		}
	}

	private void InstantiateCamera()
	{
		if (this.CamType != RenderMode.WorldSpace)
		{
			if (this.RenderCameraPrefab != null)
			{
				if (this.ActiveCamera == null)
				{
					this.ActiveCamera = UnityEngine.Object.Instantiate<Camera>(this.RenderCameraPrefab);
					UIManager.ReparentTransform(this.ActiveCamera.transform, this.CameraLayerContainer.transform);
					this.ActiveCamera.depth = (float)this.Priority;
				}
			}
			else
			{
				Debug.LogError("No camera prefab!");
			}
		}
		else
		{
			Camera camera;
			if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
			{
				camera = this.RenderInGameCameraPrefab;
				this.ActiveCameraIsForInGame = true;
			}
			else
			{
				camera = this.RenderCameraPrefab;
				this.ActiveCameraIsForInGame = false;
			}
			if (camera != null)
			{
				if (this.ActiveCamera == null)
				{
					this.ActiveCamera = UnityEngine.Object.Instantiate<Camera>(camera);
					UIManager.ReparentTransform(this.ActiveCamera.transform, this.CameraLayerContainer.transform);
					this.ActiveCamera.depth = (float)this.Priority;
				}
			}
			else if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd)
			{
				Debug.LogError("No camera prefab!");
			}
		}
	}

	public RuntimeSceneInfo RegisterUIScene(IUIScene scene)
	{
		if (this.CamType == RenderMode.WorldSpace)
		{
			if (scene.GetSceneType() == SceneType.CharacterSelectBackground)
			{
				if (this.LayerType == CameraLayerName.EnvironmentLayer)
				{
					if (this.ActiveCamera != null)
					{
						UIManager.ReparentTransform(this.ActiveCamera.transform, FrontEndCharacterSelectBackgroundScene.Get().m_frontendEnvironmentCameraParent.transform);
					}
				}
			}
		}
		return this.LayerManager.RegisterUIScene(scene);
	}

	public Canvas GetBatchCanvas(IUIScene theScene, CanvasBatchType type)
	{
		return this.LayerManager.GetBatchCanvas(theScene, type);
	}

	public Canvas GetDefaultCanvas(IUIScene theScene)
	{
		return this.LayerManager.GetDefaultCanvas(theScene);
	}

	public Canvas GetDefaultCanvas(SceneType theScene)
	{
		return this.LayerManager.GetDefaultCanvas(theScene);
	}

	public int GetNameplateCanvasLayer()
	{
		return this.LayerManager.GetNameplateCanvasLayer();
	}

	public List<UISceneDisplayInfo> SetGameState(UIManager.ClientState newState)
	{
		if (this.CamType != RenderMode.WorldSpace)
		{
			if (newState == UIManager.ClientState.InFrontEnd)
			{
				if (this.ActiveCamera == null)
				{
					this.InstantiateCamera();
					goto IL_AB;
				}
			}
			if (newState == UIManager.ClientState.InGame)
			{
				if (this.ActiveCamera != null)
				{
					if (!this.CameraUsedInGame)
					{
						UnityEngine.Object.Destroy(this.ActiveCamera.gameObject);
					}
				}
				if (UICharacterSelectWorldObjects.Get() != null)
				{
					UICharacterSelectWorldObjects.Get().UnloadAllCharacters();
				}
			}
			IL_AB:;
		}
		else
		{
			if (newState == UIManager.ClientState.InFrontEnd)
			{
				if (this.ActiveCameraIsForInGame)
				{
					if (this.ActiveCamera != null)
					{
						UnityEngine.Object.Destroy(this.ActiveCamera.gameObject);
						this.ActiveCamera = null;
					}
				}
			}
			else if (newState == UIManager.ClientState.InGame)
			{
				if (!this.ActiveCameraIsForInGame)
				{
					if (this.ActiveCamera != null)
					{
						UnityEngine.Object.Destroy(this.ActiveCamera.gameObject);
						this.ActiveCamera = null;
					}
				}
			}
			if (this.ActiveCamera == null)
			{
				this.InstantiateCamera();
			}
		}
		return this.LayerManager.SetGameState(newState);
	}
}
