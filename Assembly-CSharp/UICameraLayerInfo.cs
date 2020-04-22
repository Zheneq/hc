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
		return LayerManager.SetSceneVisible(aScenes, visible, parameters);
	}

	public void Init()
	{
		if (init)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			init = true;
			ActiveCameraIsForInGame = false;
			InstantiateCamera();
			LayerManager.Init(this);
			for (int i = 0; i < LayerManager.CanvasLayers.Length; i++)
			{
				for (int j = 0; j < LayerManager.CanvasLayers[i].SceneDisplayInfos.Length; j++)
				{
					CameraUsedInGame = (CameraUsedInGame || LayerManager.CanvasLayers[i].SceneDisplayInfos[j].m_InGame);
					CameraUsedInFrontEnd = (CameraUsedInFrontEnd || LayerManager.CanvasLayers[i].SceneDisplayInfos[j].m_InFrontEnd);
				}
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void InstantiateCamera()
	{
		if (CamType != RenderMode.WorldSpace)
		{
			if (RenderCameraPrefab != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (ActiveCamera == null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									ActiveCamera = UnityEngine.Object.Instantiate(RenderCameraPrefab);
									UIManager.ReparentTransform(ActiveCamera.transform, CameraLayerContainer.transform);
									ActiveCamera.depth = Priority;
									return;
								}
							}
						}
						return;
					}
				}
			}
			Debug.LogError("No camera prefab!");
			return;
		}
		Camera camera = null;
		if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
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
			camera = RenderInGameCameraPrefab;
			ActiveCameraIsForInGame = true;
		}
		else
		{
			camera = RenderCameraPrefab;
			ActiveCameraIsForInGame = false;
		}
		if (camera != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (ActiveCamera == null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								ActiveCamera = UnityEngine.Object.Instantiate(camera);
								UIManager.ReparentTransform(ActiveCamera.transform, CameraLayerContainer.transform);
								ActiveCamera.depth = Priority;
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (UIManager.Get().CurrentState != 0)
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
			Debug.LogError("No camera prefab!");
			return;
		}
	}

	public RuntimeSceneInfo RegisterUIScene(IUIScene scene)
	{
		if (CamType == RenderMode.WorldSpace)
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
			if (scene.GetSceneType() == SceneType.CharacterSelectBackground)
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
				if (LayerType == CameraLayerName.EnvironmentLayer)
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
					if (ActiveCamera != null)
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
						UIManager.ReparentTransform(ActiveCamera.transform, FrontEndCharacterSelectBackgroundScene.Get().m_frontendEnvironmentCameraParent.transform);
					}
				}
			}
		}
		return LayerManager.RegisterUIScene(scene);
	}

	public Canvas GetBatchCanvas(IUIScene theScene, CanvasBatchType type)
	{
		return LayerManager.GetBatchCanvas(theScene, type);
	}

	public Canvas GetDefaultCanvas(IUIScene theScene)
	{
		return LayerManager.GetDefaultCanvas(theScene);
	}

	public Canvas GetDefaultCanvas(SceneType theScene)
	{
		return LayerManager.GetDefaultCanvas(theScene);
	}

	public int GetNameplateCanvasLayer()
	{
		return LayerManager.GetNameplateCanvasLayer();
	}

	public List<UISceneDisplayInfo> SetGameState(UIManager.ClientState newState)
	{
		if (CamType != RenderMode.WorldSpace)
		{
			if (newState == UIManager.ClientState.InFrontEnd)
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
				if (ActiveCamera == null)
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
					InstantiateCamera();
					goto IL_016b;
				}
			}
			if (newState == UIManager.ClientState.InGame)
			{
				if (ActiveCamera != null)
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
					if (!CameraUsedInGame)
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
						UnityEngine.Object.Destroy(ActiveCamera.gameObject);
					}
				}
				if (UICharacterSelectWorldObjects.Get() != null)
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
					UICharacterSelectWorldObjects.Get().UnloadAllCharacters();
				}
			}
		}
		else
		{
			if (newState == UIManager.ClientState.InFrontEnd)
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
				if (ActiveCameraIsForInGame)
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
					if (ActiveCamera != null)
					{
						UnityEngine.Object.Destroy(ActiveCamera.gameObject);
						ActiveCamera = null;
					}
				}
			}
			else if (newState == UIManager.ClientState.InGame)
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
				if (!ActiveCameraIsForInGame)
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
					if (ActiveCamera != null)
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
						UnityEngine.Object.Destroy(ActiveCamera.gameObject);
						ActiveCamera = null;
					}
				}
			}
			if (ActiveCamera == null)
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
				InstantiateCamera();
			}
		}
		goto IL_016b;
		IL_016b:
		return LayerManager.SetGameState(newState);
	}
}
