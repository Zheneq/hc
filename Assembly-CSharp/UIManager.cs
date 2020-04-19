using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public UICameraLayerInfo[] LayerInfos;

	[HideInInspector]
	public UIManager.ClientState CurrentState;

	public int testValue = 0x64;

	private static UIManager s_instance;

	private bool init;

	private List<RuntimeSceneInfo> RunTimeScenes = new List<RuntimeSceneInfo>();

	private List<UIScene.CloseObjectInfo> MouseObjectClickListeners = new List<UIScene.CloseObjectInfo>();

	[CompilerGenerated]
	private static UIAnimationEventManager.AnimationDoneCallbackWithGameObjectParam <>f__mg$cache0;

	[CompilerGenerated]
	private static UIAnimationEventManager.AnimationDoneCallbackWithGameObjectParam <>f__mg$cache1;

	public bool DoneInitialLoading { get; private set; }

	public void Awake()
	{
		Debug.Log("UIManager Awake");
		UIManager.s_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.Initialize();
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		Log.Info("UIManager OnDestroy", new object[0]);
	}

	public static UIManager Get()
	{
		return UIManager.s_instance;
	}

	public Camera GetCamera(CameraLayerName layerName)
	{
		for (int i = 0; i < this.LayerInfos.Length; i++)
		{
			if (this.LayerInfos[i].LayerType == layerName)
			{
				return this.LayerInfos[i].ActiveCamera;
			}
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.GetCamera(CameraLayerName)).MethodHandle;
		}
		return null;
	}

	public Camera GetEnvirontmentCamera()
	{
		for (int i = 0; i < this.LayerInfos.Length; i++)
		{
			if (this.LayerInfos[i].LayerType == CameraLayerName.EnvironmentLayer)
			{
				return this.LayerInfos[i].ActiveCamera;
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.GetEnvirontmentCamera()).MethodHandle;
		}
		return null;
	}

	public Canvas GetDefaultCanvas(IUIScene theScene)
	{
		Canvas canvas = null;
		for (int i = 0; i < this.LayerInfos.Length; i++)
		{
			canvas = this.LayerInfos[i].GetDefaultCanvas(theScene);
			if (canvas != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.GetDefaultCanvas(IUIScene)).MethodHandle;
				}
				return canvas;
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			return canvas;
		}
	}

	public Canvas GetDefaultCanvas(SceneType theScene)
	{
		Canvas canvas = null;
		for (int i = 0; i < this.LayerInfos.Length; i++)
		{
			canvas = this.LayerInfos[i].GetDefaultCanvas(theScene);
			if (canvas != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.GetDefaultCanvas(SceneType)).MethodHandle;
				}
				break;
			}
		}
		return canvas;
	}

	public Canvas GetBatchCanvas(IUIScene theScene, CanvasBatchType type)
	{
		Canvas canvas = null;
		for (int i = 0; i < this.LayerInfos.Length; i++)
		{
			canvas = this.LayerInfos[i].GetBatchCanvas(theScene, type);
			if (canvas != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.GetBatchCanvas(IUIScene, CanvasBatchType)).MethodHandle;
				}
				return canvas;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			return canvas;
		}
	}

	public int GetNameplateCanvasLayer()
	{
		int num = -1;
		for (int i = 0; i < this.LayerInfos.Length; i++)
		{
			num = this.LayerInfos[i].GetNameplateCanvasLayer();
			if (num != -1)
			{
				return num;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.GetNameplateCanvasLayer()).MethodHandle;
			return num;
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
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform != trans.gameObject)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.ChangeLayersRecursively(Transform, string)).MethodHandle;
					}
					UIManager.ChangeLayersRecursively(transform, name);
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
		UIManager.ReparentTransform(child, newParent, Vector3.one);
	}

	public static void ReparentTransform(Transform child, Transform newParent, Vector3 newScale)
	{
		if (!(child == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.ReparentTransform(Transform, Transform, Vector3)).MethodHandle;
			}
			if (!(newParent == null))
			{
				child.SetParent(newParent);
				child.localEulerAngles = Vector3.zero;
				child.localPosition = Vector3.zero;
				child.localScale = newScale;
				return;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public static void SetGameObjectActive(Component component, bool doActive, DisableGameObjectWithAnimOutInfo overrideAnimInfo = null)
	{
		if (component == null)
		{
			return;
		}
		UIManager.SetGameObjectActive(component.gameObject, doActive, overrideAnimInfo);
	}

	public static void AnimationDoneCallback(GameObject gameObject)
	{
		if (gameObject != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.AnimationDoneCallback(GameObject)).MethodHandle;
			}
			gameObject.SetActive(true);
		}
	}

	public static void SetGameObjectActive(GameObject gObject, bool doActive, DisableGameObjectWithAnimOutInfo overrideAnimInfo = null)
	{
		if (!(gObject == null))
		{
			if (DisableGameObjectWithAnimOutInfo.s_attachedObjectInstanceIds != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.SetGameObjectActive(GameObject, bool, DisableGameObjectWithAnimOutInfo)).MethodHandle;
				}
				if (DisableGameObjectWithAnimOutInfo.s_attachedObjectInstanceIds.Contains(gObject.GetInstanceID()))
				{
					DisableGameObjectWithAnimOutInfo disableGameObjectWithAnimOutInfo = overrideAnimInfo;
					if (disableGameObjectWithAnimOutInfo == null)
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
						disableGameObjectWithAnimOutInfo = gObject.GetComponent<DisableGameObjectWithAnimOutInfo>();
					}
					if (!(disableGameObjectWithAnimOutInfo == null))
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
						if (disableGameObjectWithAnimOutInfo.m_animator == null)
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
						}
						else
						{
							if (doActive)
							{
								bool flag = false;
								if (disableGameObjectWithAnimOutInfo.m_EnableGameObjectInfo != null)
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
									if (disableGameObjectWithAnimOutInfo.m_EnableGameObjectInfo.Length > 0)
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
										for (int i = 0; i < disableGameObjectWithAnimOutInfo.m_EnableGameObjectInfo.Length; i++)
										{
											DisableGameObjectWithAnimOutInfo.SetGameObjectEnableInfo setGameObjectEnableInfo = disableGameObjectWithAnimOutInfo.m_EnableGameObjectInfo[i];
											if (!setGameObjectEnableInfo.m_AnimationNameToPlay.IsNullOrEmpty())
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
												UIAnimationEventManager uianimationEventManager = UIAnimationEventManager.Get();
												Animator animator = disableGameObjectWithAnimOutInfo.m_animator;
												string animationNameToPlay = setGameObjectEnableInfo.m_AnimationNameToPlay;
												UIAnimationEventManager.AnimationDoneCallback callbackOnDone = null;
												string animationNameForDoneCallback = setGameObjectEnableInfo.m_AnimationNameForDoneCallback;
												int animLayer = setGameObjectEnableInfo.m_AnimLayer;
												float animStartTimeNormalized = setGameObjectEnableInfo.m_AnimStartTimeNormalized;
												bool setAnimatorGameObjectActive = true;
												bool checkCurrentState = true;
												if (UIManager.<>f__mg$cache0 == null)
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
													UIManager.<>f__mg$cache0 = new UIAnimationEventManager.AnimationDoneCallbackWithGameObjectParam(UIManager.AnimationDoneCallback);
												}
												uianimationEventManager.PlayAnimation(animator, animationNameToPlay, callbackOnDone, animationNameForDoneCallback, animLayer, animStartTimeNormalized, setAnimatorGameObjectActive, checkCurrentState, UIManager.<>f__mg$cache0, gObject);
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
									}
								}
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
									gObject.SetActive(true);
								}
								return;
							}
							bool flag2 = false;
							if (gObject.activeInHierarchy)
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
								if (disableGameObjectWithAnimOutInfo.m_DisableGameObjectInfo != null && disableGameObjectWithAnimOutInfo.m_DisableGameObjectInfo.Length > 0)
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
									for (int j = 0; j < disableGameObjectWithAnimOutInfo.m_DisableGameObjectInfo.Length; j++)
									{
										DisableGameObjectWithAnimOutInfo.SetGameObjectEnableInfo setGameObjectEnableInfo2 = disableGameObjectWithAnimOutInfo.m_DisableGameObjectInfo[j];
										if (!setGameObjectEnableInfo2.m_AnimationNameToPlay.IsNullOrEmpty())
										{
											flag2 = true;
											UIAnimationEventManager uianimationEventManager2 = UIAnimationEventManager.Get();
											Animator animator2 = disableGameObjectWithAnimOutInfo.m_animator;
											string animationNameToPlay2 = setGameObjectEnableInfo2.m_AnimationNameToPlay;
											UIAnimationEventManager.AnimationDoneCallback callbackOnDone2 = null;
											string animationNameForDoneCallback2 = setGameObjectEnableInfo2.m_AnimationNameForDoneCallback;
											int animLayer2 = setGameObjectEnableInfo2.m_AnimLayer;
											float animStartTimeNormalized2 = setGameObjectEnableInfo2.m_AnimStartTimeNormalized;
											bool setAnimatorGameObjectActive2 = true;
											bool checkCurrentState2 = true;
											if (UIManager.<>f__mg$cache1 == null)
											{
												UIManager.<>f__mg$cache1 = new UIAnimationEventManager.AnimationDoneCallbackWithGameObjectParam(UIManager.AnimationDoneCallback);
											}
											uianimationEventManager2.PlayAnimation(animator2, animationNameToPlay2, callbackOnDone2, animationNameForDoneCallback2, animLayer2, animStartTimeNormalized2, setAnimatorGameObjectActive2, checkCurrentState2, UIManager.<>f__mg$cache1, gObject);
										}
									}
								}
							}
							if (!flag2)
							{
								gObject.SetActive(false);
								return;
							}
							return;
						}
					}
					gObject.SetActive(doActive);
					return;
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
			}
			if (gObject.activeSelf != doActive)
			{
				gObject.SetActive(doActive);
			}
		}
	}

	public void Initialize()
	{
		if (!this.init)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.Initialize()).MethodHandle;
			}
			this.init = true;
			base.gameObject.AddComponent<UIAnimationEventManager>();
			List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
			for (int i = 0; i < this.LayerInfos.Length; i++)
			{
				this.LayerInfos[i].CameraLayerContainer = new GameObject();
				this.LayerInfos[i].CameraLayerContainer.name = "(Camera)" + this.LayerInfos[i].LayerName;
				UIManager.ReparentTransform(this.LayerInfos[i].CameraLayerContainer.transform, base.gameObject.transform);
				this.LayerInfos[i].Init();
				KeyValuePair<int, int> item = new KeyValuePair<int, int>(i, this.LayerInfos[i].Priority);
				list.Add(item);
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (list.Count > 1)
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
				List<KeyValuePair<int, int>> list2 = list;
				if (UIManager.<>f__am$cache0 == null)
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
					UIManager.<>f__am$cache0 = delegate(KeyValuePair<int, int> keyA, KeyValuePair<int, int> keyB)
					{
						if (keyA.Value > keyB.Value)
						{
							return 1;
						}
						if (keyA.Value < keyB.Value)
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
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIManager.<Initialize>m__0(KeyValuePair<int, int>, KeyValuePair<int, int>)).MethodHandle;
							}
							return -1;
						}
						return 0;
					};
				}
				list2.Sort(UIManager.<>f__am$cache0);
				for (int j = 0; j < list.Count; j++)
				{
					this.LayerInfos[list[j].Key].CameraLayerContainer.transform.SetAsLastSibling();
				}
			}
		}
	}

	public RuntimeSceneInfo RegisterUIScene(UIScene scene)
	{
		if (scene == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.RegisterUIScene(UIScene)).MethodHandle;
			}
			Debug.LogError("UIScene did not implement ui scene interface");
			return null;
		}
		for (int i = 0; i < this.LayerInfos.Length; i++)
		{
			RuntimeSceneInfo runtimeSceneInfo = this.LayerInfos[i].RegisterUIScene(scene);
			if (runtimeSceneInfo != null)
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
				this.RunTimeScenes.Add(runtimeSceneInfo);
				((IUIScene)scene).NotifyGameStateChange(new SceneStateParameters
				{
					NewClientGameState = new UIManager.ClientState?(this.CurrentState)
				});
				if (scene.GetMouseClickObjects() != null)
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
					this.MouseObjectClickListeners.AddRange(scene.GetMouseClickObjects());
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
		return this.SetSceneVisible(new SceneType[]
		{
			aScene
		}, visible, parameters);
	}

	public bool SetSceneVisible(IEnumerable<SceneType> aScenes, bool visible, SceneVisibilityParameters parameters)
	{
		int num = 0;
		List<SceneType> list = new List<SceneType>(aScenes);
		for (int i = 0; i < this.LayerInfos.Length; i++)
		{
			num += this.LayerInfos[i].SetSceneVisible(aScenes, visible, parameters);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.SetSceneVisible(IEnumerable<SceneType>, bool, SceneVisibilityParameters)).MethodHandle;
		}
		if (list.Count != num)
		{
		}
		return num == list.Count;
	}

	public IEnumerator LoadSceneAsync(List<UISceneDisplayInfo> scenesToLoad)
	{
		scenesToLoad.Sort(delegate(UISceneDisplayInfo infoA, UISceneDisplayInfo infoB)
		{
			if (infoA != null)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIManager.<LoadSceneAsync>c__Iterator0.<>m__0(UISceneDisplayInfo, UISceneDisplayInfo)).MethodHandle;
				}
				if (infoB != null)
				{
					if (infoA.SceneLoadPriority == infoB.SceneLoadPriority)
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
						return 0;
					}
					if (infoA.SceneLoadPriority < infoB.SceneLoadPriority)
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
						return -1;
					}
					return 1;
				}
			}
			Log.Error("Attempting to load empty scene", new object[0]);
			return 0;
		});
		for (int i = 0; i < scenesToLoad.Count; i++)
		{
			if (!this.DoesSceneExist(scenesToLoad[i].m_SceneType))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.<LoadSceneAsync>c__Iterator0.MoveNext()).MethodHandle;
				}
				if (!scenesToLoad[i].UnitySceneLoadName.IsNullOrEmpty())
				{
					yield return AssetBundleManager.Get().LoadSceneAsync(scenesToLoad[i].UnitySceneLoadName, "uiscenes", LoadSceneMode.Additive);
				}
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		this.DoneInitialLoading = true;
		yield break;
	}

	public void HandleNewSceneStateParameter(SceneStateParameters newParameters)
	{
		for (int i = 0; i < this.RunTimeScenes.Count; i++)
		{
			if (this.RunTimeScenes[i].RuntimeScene.DoesHandleParameter(newParameters))
			{
				this.RunTimeScenes[i].RuntimeScene.HandleNewSceneStateParameter(newParameters);
			}
		}
	}

	private bool DoesSceneExist(SceneType sceneType)
	{
		for (int i = 0; i < this.RunTimeScenes.Count; i++)
		{
			if (this.RunTimeScenes[i].DisplayInfo.m_SceneType == sceneType)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.DoesSceneExist(SceneType)).MethodHandle;
				}
				return true;
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
		return false;
	}

	public void SetGameState(UIManager.ClientState newState)
	{
		this.CurrentState = newState;
		int i = 0;
		while (i < this.RunTimeScenes.Count)
		{
			if (newState != UIManager.ClientState.InFrontEnd)
			{
				goto IL_48;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.SetGameState(UIManager.ClientState)).MethodHandle;
			}
			if (!this.RunTimeScenes[i].DisplayInfo.m_InFrontEnd)
			{
				goto IL_80;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				goto IL_48;
			}
			IL_30C:
			i++;
			continue;
			IL_80:
			for (int j = 0; j < this.RunTimeScenes[i].RuntimeSceneContainer.transform.childCount; j++)
			{
				UnityEngine.Object.Destroy(this.RunTimeScenes[i].RuntimeSceneContainer.transform.GetChild(j).gameObject);
			}
			if (this.RunTimeScenes[i].RuntimeStaticSceneContainer != null)
			{
				for (int k = 0; k < this.RunTimeScenes[i].RuntimeStaticSceneContainer.transform.childCount; k++)
				{
					UnityEngine.Object.Destroy(this.RunTimeScenes[i].RuntimeStaticSceneContainer.transform.GetChild(k).gameObject);
				}
			}
			if (this.RunTimeScenes[i].RuntimeSemiStaticSceneContainer != null)
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
				for (int l = 0; l < this.RunTimeScenes[i].RuntimeSemiStaticSceneContainer.transform.childCount; l++)
				{
					UnityEngine.Object.Destroy(this.RunTimeScenes[i].RuntimeSemiStaticSceneContainer.transform.GetChild(l).gameObject);
				}
			}
			if (this.RunTimeScenes[i].RuntimeCameraMovementSceneContainer != null)
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
				for (int m = 0; m < this.RunTimeScenes[i].RuntimeCameraMovementSceneContainer.transform.childCount; m++)
				{
					UnityEngine.Object.Destroy(this.RunTimeScenes[i].RuntimeCameraMovementSceneContainer.transform.GetChild(m).gameObject);
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
			}
			if (this.RunTimeScenes[i].RuntimePerFrameSceneContainer != null)
			{
				for (int n = 0; n < this.RunTimeScenes[i].RuntimePerFrameSceneContainer.transform.childCount; n++)
				{
					UnityEngine.Object.Destroy(this.RunTimeScenes[i].RuntimePerFrameSceneContainer.transform.GetChild(n).gameObject);
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.RunTimeScenes.RemoveAt(i);
			i--;
			goto IL_30C;
			IL_48:
			if (newState == UIManager.ClientState.InGame)
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
				if (!this.RunTimeScenes[i].DisplayInfo.m_InGame)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						goto IL_80;
					}
				}
			}
			this.RunTimeScenes[i].RuntimeScene.NotifyGameStateChange(new SceneStateParameters
			{
				NewClientGameState = new UIManager.ClientState?(newState)
			});
			goto IL_30C;
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
		List<UISceneDisplayInfo> list = new List<UISceneDisplayInfo>();
		for (int num = 0; num < this.LayerInfos.Length; num++)
		{
			list.AddRange(this.LayerInfos[num].SetGameState(newState));
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
		this.DoneInitialLoading = false;
		base.StartCoroutine("LoadSceneAsync", list);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIManager.Update()).MethodHandle;
			}
			for (int i = 0; i < this.MouseObjectClickListeners.Count; i++)
			{
				bool flag = true;
				UIScene.CloseObjectInfo closeObjectInfo = this.MouseObjectClickListeners[i];
				if (closeObjectInfo.m_SceneReference != null)
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
					if (EventSystem.current != null)
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
						if (EventSystem.current.IsPointerOverGameObject(-1))
						{
							StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
							if (component != null)
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
								if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
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
									Transform transform = component.GetLastPointerEventDataPublic(-1).pointerEnter.transform;
									if (transform != null)
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
										for (int j = 0; j < closeObjectInfo.m_GameObjectsToIgnoreCloseCall.Length; j++)
										{
											if (closeObjectInfo.m_GameObjectsToIgnoreCloseCall[j] == transform.gameObject)
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
												flag = false;
												goto IL_147;
											}
										}
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									IL_147:
									if (closeObjectInfo.m_checkParentObjectsOfClickedObject)
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
										if (flag)
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
											while (transform != null)
											{
												for (int k = 0; k < closeObjectInfo.m_GameObjectsToIgnoreCloseCall.Length; k++)
												{
													if (closeObjectInfo.m_GameObjectsToIgnoreCloseCall[k] == transform.gameObject)
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
														flag = false;
														break;
													}
												}
												transform = transform.parent;
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
									}
								}
							}
							if (flag)
							{
								if (closeObjectInfo.m_GameObjectsToDisableOnClick != null)
								{
									for (int l = 0; l < closeObjectInfo.m_GameObjectsToDisableOnClick.Length; l++)
									{
										UIManager.SetGameObjectActive(closeObjectInfo.m_GameObjectsToDisableOnClick[l], false, null);
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
								if (closeObjectInfo.m_AnimatorToClose != null)
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
									UIAnimationEventManager.Get().PlayAnimation(closeObjectInfo.m_AnimatorToClose, closeObjectInfo.m_animationToPlay, null, string.Empty, 0, 0f, true, false, null, null);
								}
							}
						}
					}
				}
				else
				{
					this.MouseObjectClickListeners.RemoveAt(i);
					i--;
				}
			}
		}
	}

	public enum ClientState
	{
		InFrontEnd,
		InGame
	}
}
