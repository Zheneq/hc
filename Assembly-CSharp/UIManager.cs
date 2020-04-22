using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public enum ClientState
	{
		InFrontEnd,
		InGame
	}

	public UICameraLayerInfo[] LayerInfos;

	[HideInInspector]
	public ClientState CurrentState;

	public int testValue = 100;

	private static UIManager s_instance;

	private bool init;

	private List<RuntimeSceneInfo> RunTimeScenes = new List<RuntimeSceneInfo>();

	private List<UIScene.CloseObjectInfo> MouseObjectClickListeners = new List<UIScene.CloseObjectInfo>();

	[CompilerGenerated]
	private static UIAnimationEventManager.AnimationDoneCallbackWithGameObjectParam _003C_003Ef__mg_0024cache0;

	[CompilerGenerated]
	private static UIAnimationEventManager.AnimationDoneCallbackWithGameObjectParam _003C_003Ef__mg_0024cache1;

	public bool DoneInitialLoading
	{
		get;
		private set;
	}

	public void Awake()
	{
		Debug.Log("UIManager Awake");
		s_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Initialize();
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		Log.Info("UIManager OnDestroy");
	}

	public static UIManager Get()
	{
		return s_instance;
	}

	public Camera GetCamera(CameraLayerName layerName)
	{
		for (int i = 0; i < LayerInfos.Length; i++)
		{
			if (LayerInfos[i].LayerType == layerName)
			{
				return LayerInfos[i].ActiveCamera;
			}
		}
		while (true)
		{
			return null;
		}
	}

	public Camera GetEnvirontmentCamera()
	{
		for (int i = 0; i < LayerInfos.Length; i++)
		{
			if (LayerInfos[i].LayerType == CameraLayerName.EnvironmentLayer)
			{
				return LayerInfos[i].ActiveCamera;
			}
		}
		while (true)
		{
			return null;
		}
	}

	public Canvas GetDefaultCanvas(IUIScene theScene)
	{
		Canvas canvas = null;
		int num = 0;
		while (true)
		{
			if (num < LayerInfos.Length)
			{
				canvas = LayerInfos[num].GetDefaultCanvas(theScene);
				if (canvas != null)
				{
					break;
				}
				num++;
				continue;
			}
			break;
		}
		return canvas;
	}

	public Canvas GetDefaultCanvas(SceneType theScene)
	{
		Canvas canvas = null;
		for (int i = 0; i < LayerInfos.Length; i++)
		{
			canvas = LayerInfos[i].GetDefaultCanvas(theScene);
			if (canvas != null)
			{
				break;
			}
		}
		return canvas;
	}

	public Canvas GetBatchCanvas(IUIScene theScene, CanvasBatchType type)
	{
		Canvas canvas = null;
		int num = 0;
		while (true)
		{
			if (num < LayerInfos.Length)
			{
				canvas = LayerInfos[num].GetBatchCanvas(theScene, type);
				if (canvas != null)
				{
					break;
				}
				num++;
				continue;
			}
			break;
		}
		return canvas;
	}

	public int GetNameplateCanvasLayer()
	{
		int num = -1;
		int num2 = 0;
		while (true)
		{
			if (num2 < LayerInfos.Length)
			{
				num = LayerInfos[num2].GetNameplateCanvasLayer();
				if (num != -1)
				{
					break;
				}
				num2++;
				continue;
			}
			break;
		}
		return num;
	}

	public static void ChangeLayersRecursively(Transform trans, string name)
	{
		trans.gameObject.layer = LayerMask.NameToLayer(name);
		IEnumerator enumerator = trans.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (transform != trans.gameObject)
				{
					ChangeLayersRecursively(transform, name);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public static void ReparentTransform(Transform child, Transform newParent)
	{
		ReparentTransform(child, newParent, Vector3.one);
	}

	public static void ReparentTransform(Transform child, Transform newParent, Vector3 newScale)
	{
		if (child == null)
		{
			return;
		}
		while (true)
		{
			if (newParent == null)
			{
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
			child.SetParent(newParent);
			child.localEulerAngles = Vector3.zero;
			child.localPosition = Vector3.zero;
			child.localScale = newScale;
			return;
		}
	}

	public static void SetGameObjectActive(Component component, bool doActive, DisableGameObjectWithAnimOutInfo overrideAnimInfo = null)
	{
		if (!(component == null))
		{
			SetGameObjectActive(component.gameObject, doActive, overrideAnimInfo);
		}
	}

	public static void AnimationDoneCallback(GameObject gameObject)
	{
		if (!(gameObject != null))
		{
			return;
		}
		while (true)
		{
			gameObject.SetActive(true);
			return;
		}
	}

	public static void SetGameObjectActive(GameObject gObject, bool doActive, DisableGameObjectWithAnimOutInfo overrideAnimInfo = null)
	{
		if (gObject == null)
		{
			return;
		}
		if (DisableGameObjectWithAnimOutInfo.s_attachedObjectInstanceIds != null)
		{
			if (DisableGameObjectWithAnimOutInfo.s_attachedObjectInstanceIds.Contains(gObject.GetInstanceID()))
			{
				DisableGameObjectWithAnimOutInfo disableGameObjectWithAnimOutInfo = overrideAnimInfo;
				if (disableGameObjectWithAnimOutInfo == null)
				{
					disableGameObjectWithAnimOutInfo = gObject.GetComponent<DisableGameObjectWithAnimOutInfo>();
				}
				if (!(disableGameObjectWithAnimOutInfo == null))
				{
					if (!(disableGameObjectWithAnimOutInfo.m_animator == null))
					{
						if (doActive)
						{
							bool flag = false;
							if (disableGameObjectWithAnimOutInfo.m_EnableGameObjectInfo != null)
							{
								if (disableGameObjectWithAnimOutInfo.m_EnableGameObjectInfo.Length > 0)
								{
									for (int i = 0; i < disableGameObjectWithAnimOutInfo.m_EnableGameObjectInfo.Length; i++)
									{
										DisableGameObjectWithAnimOutInfo.SetGameObjectEnableInfo setGameObjectEnableInfo = disableGameObjectWithAnimOutInfo.m_EnableGameObjectInfo[i];
										if (setGameObjectEnableInfo.m_AnimationNameToPlay.IsNullOrEmpty())
										{
											continue;
										}
										flag = true;
										UIAnimationEventManager uIAnimationEventManager = UIAnimationEventManager.Get();
										Animator animator = disableGameObjectWithAnimOutInfo.m_animator;
										string animationNameToPlay = setGameObjectEnableInfo.m_AnimationNameToPlay;
										string animationNameForDoneCallback = setGameObjectEnableInfo.m_AnimationNameForDoneCallback;
										int animLayer = setGameObjectEnableInfo.m_AnimLayer;
										float animStartTimeNormalized = setGameObjectEnableInfo.m_AnimStartTimeNormalized;
										if (_003C_003Ef__mg_0024cache0 == null)
										{
											_003C_003Ef__mg_0024cache0 = AnimationDoneCallback;
										}
										uIAnimationEventManager.PlayAnimation(animator, animationNameToPlay, null, animationNameForDoneCallback, animLayer, animStartTimeNormalized, true, true, _003C_003Ef__mg_0024cache0, gObject);
									}
								}
							}
							if (flag)
							{
								return;
							}
							while (true)
							{
								gObject.SetActive(true);
								return;
							}
						}
						bool flag2 = false;
						if (gObject.activeInHierarchy)
						{
							if (disableGameObjectWithAnimOutInfo.m_DisableGameObjectInfo != null && disableGameObjectWithAnimOutInfo.m_DisableGameObjectInfo.Length > 0)
							{
								for (int j = 0; j < disableGameObjectWithAnimOutInfo.m_DisableGameObjectInfo.Length; j++)
								{
									DisableGameObjectWithAnimOutInfo.SetGameObjectEnableInfo setGameObjectEnableInfo2 = disableGameObjectWithAnimOutInfo.m_DisableGameObjectInfo[j];
									if (!setGameObjectEnableInfo2.m_AnimationNameToPlay.IsNullOrEmpty())
									{
										flag2 = true;
										UIAnimationEventManager.Get().PlayAnimation(disableGameObjectWithAnimOutInfo.m_animator, setGameObjectEnableInfo2.m_AnimationNameToPlay, null, setGameObjectEnableInfo2.m_AnimationNameForDoneCallback, setGameObjectEnableInfo2.m_AnimLayer, setGameObjectEnableInfo2.m_AnimStartTimeNormalized, true, true, AnimationDoneCallback, gObject);
									}
								}
							}
						}
						if (!flag2)
						{
							gObject.SetActive(false);
						}
						return;
					}
				}
				gObject.SetActive(doActive);
				return;
			}
		}
		if (gObject.activeSelf != doActive)
		{
			gObject.SetActive(doActive);
		}
	}

	public void Initialize()
	{
		if (init)
		{
			return;
		}
		while (true)
		{
			init = true;
			base.gameObject.AddComponent<UIAnimationEventManager>();
			List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
			for (int i = 0; i < LayerInfos.Length; i++)
			{
				LayerInfos[i].CameraLayerContainer = new GameObject();
				LayerInfos[i].CameraLayerContainer.name = "(Camera)" + LayerInfos[i].LayerName;
				ReparentTransform(LayerInfos[i].CameraLayerContainer.transform, base.gameObject.transform);
				LayerInfos[i].Init();
				KeyValuePair<int, int> item = new KeyValuePair<int, int>(i, LayerInfos[i].Priority);
				list.Add(item);
			}
			while (true)
			{
				if (list.Count <= 1)
				{
					return;
				}
				while (true)
				{
					if (_003C_003Ef__am_0024cache0 == null)
					{
						_003C_003Ef__am_0024cache0 = delegate(KeyValuePair<int, int> keyA, KeyValuePair<int, int> keyB)
						{
							if (keyA.Value > keyB.Value)
							{
								return 1;
							}
							if (keyA.Value < keyB.Value)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										return -1;
									}
								}
							}
							return 0;
						};
					}
					list.Sort(_003C_003Ef__am_0024cache0);
					for (int j = 0; j < list.Count; j++)
					{
						LayerInfos[list[j].Key].CameraLayerContainer.transform.SetAsLastSibling();
					}
					return;
				}
			}
		}
	}

	public RuntimeSceneInfo RegisterUIScene(UIScene scene)
	{
		if ((object)scene == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogError("UIScene did not implement ui scene interface");
					return null;
				}
			}
		}
		for (int i = 0; i < LayerInfos.Length; i++)
		{
			RuntimeSceneInfo runtimeSceneInfo = LayerInfos[i].RegisterUIScene(scene);
			if (runtimeSceneInfo == null)
			{
				continue;
			}
			while (true)
			{
				RunTimeScenes.Add(runtimeSceneInfo);
				((IUIScene)scene).NotifyGameStateChange(new SceneStateParameters
				{
					NewClientGameState = CurrentState
				});
				if (scene.GetMouseClickObjects() != null)
				{
					MouseObjectClickListeners.AddRange(scene.GetMouseClickObjects());
				}
				return runtimeSceneInfo;
			}
		}
		Debug.LogError("Failed to register scene: " + ((IUIScene)scene).GetSceneType());
		return null;
	}

	public bool UpdateSceneState(SceneType aScene, SceneStateParameters stateParameters)
	{
		return true;
	}

	public bool SetSceneVisible(SceneType aScene, bool visible, SceneVisibilityParameters parameters)
	{
		return SetSceneVisible(new SceneType[1]
		{
			aScene
		}, visible, parameters);
	}

	public bool SetSceneVisible(IEnumerable<SceneType> aScenes, bool visible, SceneVisibilityParameters parameters)
	{
		int num = 0;
		List<SceneType> list = new List<SceneType>(aScenes);
		for (int i = 0; i < LayerInfos.Length; i++)
		{
			num += LayerInfos[i].SetSceneVisible(aScenes, visible, parameters);
		}
		while (true)
		{
			if (list.Count != num)
			{
			}
			return num == list.Count;
		}
	}

	public IEnumerator LoadSceneAsync(List<UISceneDisplayInfo> scenesToLoad)
	{
		scenesToLoad.Sort(delegate(UISceneDisplayInfo infoA, UISceneDisplayInfo infoB)
		{
			if (infoA != null)
			{
				if (infoB != null)
				{
					if (infoA.SceneLoadPriority == infoB.SceneLoadPriority)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return 0;
							}
						}
					}
					if (infoA.SceneLoadPriority < infoB.SceneLoadPriority)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								return -1;
							}
						}
					}
					return 1;
				}
			}
			Log.Error("Attempting to load empty scene");
			return 0;
		});
		for (int i = 0; i < scenesToLoad.Count; i++)
		{
			if (!DoesSceneExist(scenesToLoad[i].m_SceneType))
			{
				if (!scenesToLoad[i].UnitySceneLoadName.IsNullOrEmpty())
				{
					yield return AssetBundleManager.Get().LoadSceneAsync(scenesToLoad[i].UnitySceneLoadName, "uiscenes", LoadSceneMode.Additive);
				}
			}
		}
		while (true)
		{
			DoneInitialLoading = true;
			yield break;
		}
	}

	public void HandleNewSceneStateParameter(SceneStateParameters newParameters)
	{
		for (int i = 0; i < RunTimeScenes.Count; i++)
		{
			if (RunTimeScenes[i].RuntimeScene.DoesHandleParameter(newParameters))
			{
				RunTimeScenes[i].RuntimeScene.HandleNewSceneStateParameter(newParameters);
			}
		}
	}

	private bool DoesSceneExist(SceneType sceneType)
	{
		for (int i = 0; i < RunTimeScenes.Count; i++)
		{
			if (RunTimeScenes[i].DisplayInfo.m_SceneType != sceneType)
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

	public void SetGameState(ClientState newState)
	{
		CurrentState = newState;
		for (int i = 0; i < RunTimeScenes.Count; i++)
		{
			if (newState == ClientState.InFrontEnd)
			{
				if (!RunTimeScenes[i].DisplayInfo.m_InFrontEnd)
				{
					goto IL_0080;
				}
			}
			if (newState == ClientState.InGame)
			{
				if (!RunTimeScenes[i].DisplayInfo.m_InGame)
				{
					goto IL_0080;
				}
			}
			RunTimeScenes[i].RuntimeScene.NotifyGameStateChange(new SceneStateParameters
			{
				NewClientGameState = newState
			});
			continue;
			IL_0080:
			for (int j = 0; j < RunTimeScenes[i].RuntimeSceneContainer.transform.childCount; j++)
			{
				UnityEngine.Object.Destroy(RunTimeScenes[i].RuntimeSceneContainer.transform.GetChild(j).gameObject);
			}
			if (RunTimeScenes[i].RuntimeStaticSceneContainer != null)
			{
				for (int k = 0; k < RunTimeScenes[i].RuntimeStaticSceneContainer.transform.childCount; k++)
				{
					UnityEngine.Object.Destroy(RunTimeScenes[i].RuntimeStaticSceneContainer.transform.GetChild(k).gameObject);
				}
			}
			if (RunTimeScenes[i].RuntimeSemiStaticSceneContainer != null)
			{
				for (int l = 0; l < RunTimeScenes[i].RuntimeSemiStaticSceneContainer.transform.childCount; l++)
				{
					UnityEngine.Object.Destroy(RunTimeScenes[i].RuntimeSemiStaticSceneContainer.transform.GetChild(l).gameObject);
				}
			}
			if (RunTimeScenes[i].RuntimeCameraMovementSceneContainer != null)
			{
				for (int m = 0; m < RunTimeScenes[i].RuntimeCameraMovementSceneContainer.transform.childCount; m++)
				{
					UnityEngine.Object.Destroy(RunTimeScenes[i].RuntimeCameraMovementSceneContainer.transform.GetChild(m).gameObject);
				}
			}
			if (RunTimeScenes[i].RuntimePerFrameSceneContainer != null)
			{
				for (int n = 0; n < RunTimeScenes[i].RuntimePerFrameSceneContainer.transform.childCount; n++)
				{
					UnityEngine.Object.Destroy(RunTimeScenes[i].RuntimePerFrameSceneContainer.transform.GetChild(n).gameObject);
				}
			}
			RunTimeScenes.RemoveAt(i);
			i--;
		}
		while (true)
		{
			List<UISceneDisplayInfo> list = new List<UISceneDisplayInfo>();
			for (int num = 0; num < LayerInfos.Length; num++)
			{
				list.AddRange(LayerInfos[num].SetGameState(newState));
			}
			while (true)
			{
				DoneInitialLoading = false;
				StartCoroutine("LoadSceneAsync", list);
				return;
			}
		}
	}

	private void Update()
	{
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < MouseObjectClickListeners.Count; i++)
			{
				bool flag = true;
				UIScene.CloseObjectInfo closeObjectInfo = MouseObjectClickListeners[i];
				if (closeObjectInfo.m_SceneReference != null)
				{
					if (!(EventSystem.current != null))
					{
						continue;
					}
					if (!EventSystem.current.IsPointerOverGameObject(-1))
					{
						continue;
					}
					StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
					if (component != null)
					{
						if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
						{
							Transform transform = component.GetLastPointerEventDataPublic(-1).pointerEnter.transform;
							if (transform != null)
							{
								int num = 0;
								while (true)
								{
									if (num < closeObjectInfo.m_GameObjectsToIgnoreCloseCall.Length)
									{
										if (closeObjectInfo.m_GameObjectsToIgnoreCloseCall[num] == transform.gameObject)
										{
											flag = false;
											break;
										}
										num++;
										continue;
									}
									break;
								}
							}
							if (closeObjectInfo.m_checkParentObjectsOfClickedObject)
							{
								if (flag)
								{
									while (transform != null)
									{
										for (int j = 0; j < closeObjectInfo.m_GameObjectsToIgnoreCloseCall.Length; j++)
										{
											if (closeObjectInfo.m_GameObjectsToIgnoreCloseCall[j] == transform.gameObject)
											{
												flag = false;
												break;
											}
										}
										transform = transform.parent;
									}
								}
							}
						}
					}
					if (!flag)
					{
						continue;
					}
					if (closeObjectInfo.m_GameObjectsToDisableOnClick != null)
					{
						for (int k = 0; k < closeObjectInfo.m_GameObjectsToDisableOnClick.Length; k++)
						{
							SetGameObjectActive(closeObjectInfo.m_GameObjectsToDisableOnClick[k], false);
						}
					}
					if (closeObjectInfo.m_AnimatorToClose != null)
					{
						UIAnimationEventManager.Get().PlayAnimation(closeObjectInfo.m_AnimatorToClose, closeObjectInfo.m_animationToPlay, null, string.Empty);
					}
				}
				else
				{
					MouseObjectClickListeners.RemoveAt(i);
					i--;
				}
			}
			return;
		}
	}
}
