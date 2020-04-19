using System;
using UnityEngine;

public class ReactorCoreMapRespawnMonitor : MonoBehaviour
{
	public ReactorCoreMapRespawnMonitor.PortraitAssetType m_portraitAssetType;

	public ReactorCoreMapRespawnMonitor.RespawnMonitorSize m_respawnMonitorSize;

	private FadeObjectGroup m_fadeGroupInParent;

	private Renderer m_backgroundRenderer;

	private GameObject m_portraitInstance;

	private Renderer[] m_portraitRenderers;

	private GameObject m_vfxInstanceSearching;

	private GameObject m_vfxInstanceResurrecting;

	private bool m_desiredVisible;

	private void Start()
	{
		if (ReactorCoreMapMonitorCoordinator.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ReactorCoreMapRespawnMonitor.Start()).MethodHandle;
			}
			ReactorCoreMapMonitorCoordinator reactorCoreMapMonitorCoordinator = ReactorCoreMapMonitorCoordinator.Get();
			GameObject gameObject = null;
			if (this.m_portraitAssetType == ReactorCoreMapRespawnMonitor.PortraitAssetType.Vfx)
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
				gameObject = reactorCoreMapMonitorCoordinator.m_portraitPrefabForVfxMonitor;
			}
			else if (this.m_portraitAssetType == ReactorCoreMapRespawnMonitor.PortraitAssetType.PlainTexture)
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
				gameObject = reactorCoreMapMonitorCoordinator.m_portraitPrefabForTextureMonitor;
			}
			if (gameObject != null)
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
				this.m_portraitInstance = UnityEngine.Object.Instantiate<GameObject>(gameObject, Vector3.zero, Quaternion.identity);
				this.m_portraitInstance.transform.parent = base.transform;
				this.m_portraitInstance.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
				if (this.m_respawnMonitorSize == ReactorCoreMapRespawnMonitor.RespawnMonitorSize.Small)
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
					this.m_portraitInstance.transform.localPosition = reactorCoreMapMonitorCoordinator.m_smallMonitorPos;
					this.m_portraitInstance.transform.localScale = reactorCoreMapMonitorCoordinator.m_smallMonitorScale * Vector3.one;
				}
				else if (this.m_respawnMonitorSize == ReactorCoreMapRespawnMonitor.RespawnMonitorSize.Large)
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
					this.m_portraitInstance.transform.localPosition = reactorCoreMapMonitorCoordinator.m_largeMonitorPos;
					this.m_portraitInstance.transform.localScale = reactorCoreMapMonitorCoordinator.m_largeMonitorScale * Vector3.one;
				}
				this.m_portraitRenderers = this.m_portraitInstance.GetComponentsInChildren<Renderer>();
				this.m_portraitInstance.SetActive(false);
				reactorCoreMapMonitorCoordinator.AddPortraitController(this);
			}
			else
			{
				Debug.LogWarning("Did not find prefab to spawn for ReactorCore respawn portraits on " + base.name);
			}
			if (this.m_portraitAssetType == ReactorCoreMapRespawnMonitor.PortraitAssetType.PlainTexture)
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
				this.m_backgroundRenderer = base.GetComponent<Renderer>();
				if (this.m_backgroundRenderer != null)
				{
					this.m_backgroundRenderer.material = reactorCoreMapMonitorCoordinator.m_searchingMaterial;
				}
				this.m_fadeGroupInParent = base.GetComponentInParent<FadeObjectGroup>();
			}
			else if (this.m_portraitAssetType == ReactorCoreMapRespawnMonitor.PortraitAssetType.Vfx)
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
				GameObject gameObject2 = null;
				GameObject gameObject3 = null;
				if (this.m_respawnMonitorSize == ReactorCoreMapRespawnMonitor.RespawnMonitorSize.Small)
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
					gameObject2 = reactorCoreMapMonitorCoordinator.m_vfxPrefabSmallSearching;
					gameObject3 = reactorCoreMapMonitorCoordinator.m_vfxPrefabSmallResurrecting;
				}
				else if (this.m_respawnMonitorSize == ReactorCoreMapRespawnMonitor.RespawnMonitorSize.Large)
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
					gameObject2 = reactorCoreMapMonitorCoordinator.m_vfxPrefabLargeSearching;
					gameObject3 = reactorCoreMapMonitorCoordinator.m_vfxPrefabLargeResurrecting;
				}
				if (gameObject2 != null)
				{
					this.m_vfxInstanceSearching = this.SpawnVfxInstance(gameObject2);
				}
				if (gameObject3 != null)
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
					this.m_vfxInstanceResurrecting = this.SpawnVfxInstance(gameObject3);
					this.m_vfxInstanceResurrecting.SetActive(false);
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (ReactorCoreMapMonitorCoordinator.Get() != null)
		{
			ReactorCoreMapMonitorCoordinator.Get().RemovePortraitController(this);
		}
	}

	private GameObject SpawnVfxInstance(GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		return gameObject;
	}

	public void SetRespawnPortrait(Sprite spriteToDisplay)
	{
		ReactorCoreMapMonitorCoordinator reactorCoreMapMonitorCoordinator = ReactorCoreMapMonitorCoordinator.Get();
		if (spriteToDisplay == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ReactorCoreMapRespawnMonitor.SetRespawnPortrait(Sprite)).MethodHandle;
			}
			if (reactorCoreMapMonitorCoordinator != null)
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
				spriteToDisplay = reactorCoreMapMonitorCoordinator.m_fallbackPortraitSprite;
			}
		}
		if (this.m_portraitRenderers != null)
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
			if (spriteToDisplay != null)
			{
				this.SetPortraitRendererTextures(spriteToDisplay.texture);
				this.m_portraitInstance.SetActive(true);
				this.m_desiredVisible = true;
				if (this.m_portraitAssetType == ReactorCoreMapRespawnMonitor.PortraitAssetType.PlainTexture && this.m_backgroundRenderer != null)
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
					if (reactorCoreMapMonitorCoordinator != null)
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
						this.m_backgroundRenderer.material = reactorCoreMapMonitorCoordinator.m_resurrectingMaterial;
					}
				}
			}
		}
		if (this.m_portraitAssetType == ReactorCoreMapRespawnMonitor.PortraitAssetType.Vfx)
		{
			this.SetVfxAsResurrecting(true);
		}
	}

	public void HidePortrait()
	{
		if (this.m_portraitRenderers != null)
		{
			this.SetPortraitRendererTextures(null);
			this.m_portraitInstance.SetActive(false);
			this.m_desiredVisible = false;
			ReactorCoreMapMonitorCoordinator reactorCoreMapMonitorCoordinator = ReactorCoreMapMonitorCoordinator.Get();
			if (this.m_portraitAssetType == ReactorCoreMapRespawnMonitor.PortraitAssetType.PlainTexture)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ReactorCoreMapRespawnMonitor.HidePortrait()).MethodHandle;
				}
				if (this.m_backgroundRenderer != null && reactorCoreMapMonitorCoordinator != null)
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
					this.m_backgroundRenderer.material = reactorCoreMapMonitorCoordinator.m_searchingMaterial;
				}
			}
			if (this.m_portraitAssetType == ReactorCoreMapRespawnMonitor.PortraitAssetType.Vfx)
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
				this.SetVfxAsResurrecting(false);
			}
		}
	}

	private void SetPortraitRendererTextures(Texture texture)
	{
		if (this.m_portraitRenderers != null)
		{
			for (int i = 0; i < this.m_portraitRenderers.Length; i++)
			{
				Renderer renderer = this.m_portraitRenderers[i];
				if (renderer != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ReactorCoreMapRespawnMonitor.SetPortraitRendererTextures(Texture)).MethodHandle;
					}
					if (renderer.material != null)
					{
						renderer.material.mainTexture = texture;
					}
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
		}
	}

	private void SetVfxAsResurrecting(bool resurrecting)
	{
		if (this.m_vfxInstanceResurrecting != null)
		{
			this.m_vfxInstanceResurrecting.SetActive(resurrecting);
		}
		if (this.m_vfxInstanceSearching != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ReactorCoreMapRespawnMonitor.SetVfxAsResurrecting(bool)).MethodHandle;
			}
			this.m_vfxInstanceSearching.SetActive(!resurrecting);
		}
	}

	private void Update()
	{
		if (this.m_portraitInstance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ReactorCoreMapRespawnMonitor.Update()).MethodHandle;
			}
			if (this.m_fadeGroupInParent != null)
			{
				float currentAlpha = this.m_fadeGroupInParent.GetCurrentAlpha();
				bool flag;
				if (this.m_desiredVisible)
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
					flag = (currentAlpha > 0.98f);
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				if (flag2 != this.m_portraitInstance.activeSelf)
				{
					this.m_portraitInstance.SetActive(flag2);
				}
			}
		}
	}

	public enum PortraitAssetType
	{
		Vfx,
		PlainTexture
	}

	public enum RespawnMonitorSize
	{
		Small,
		Large
	}
}
