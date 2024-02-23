using System.Text;
using UnityEngine;

public class ReactorCoreMapRespawnMonitor : MonoBehaviour
{
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

	public PortraitAssetType m_portraitAssetType;

	public RespawnMonitorSize m_respawnMonitorSize;

	private FadeObjectGroup m_fadeGroupInParent;

	private Renderer m_backgroundRenderer;

	private GameObject m_portraitInstance;

	private Renderer[] m_portraitRenderers;

	private GameObject m_vfxInstanceSearching;

	private GameObject m_vfxInstanceResurrecting;

	private bool m_desiredVisible;

	private void Start()
	{
		if (!(ReactorCoreMapMonitorCoordinator.Get() != null))
		{
			return;
		}
		while (true)
		{
			ReactorCoreMapMonitorCoordinator reactorCoreMapMonitorCoordinator = ReactorCoreMapMonitorCoordinator.Get();
			GameObject gameObject = null;
			if (m_portraitAssetType == PortraitAssetType.Vfx)
			{
				gameObject = reactorCoreMapMonitorCoordinator.m_portraitPrefabForVfxMonitor;
			}
			else if (m_portraitAssetType == PortraitAssetType.PlainTexture)
			{
				gameObject = reactorCoreMapMonitorCoordinator.m_portraitPrefabForTextureMonitor;
			}
			if (gameObject != null)
			{
				m_portraitInstance = Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
				m_portraitInstance.transform.parent = base.transform;
				m_portraitInstance.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
				if (m_respawnMonitorSize == RespawnMonitorSize.Small)
				{
					m_portraitInstance.transform.localPosition = reactorCoreMapMonitorCoordinator.m_smallMonitorPos;
					m_portraitInstance.transform.localScale = reactorCoreMapMonitorCoordinator.m_smallMonitorScale * Vector3.one;
				}
				else if (m_respawnMonitorSize == RespawnMonitorSize.Large)
				{
					m_portraitInstance.transform.localPosition = reactorCoreMapMonitorCoordinator.m_largeMonitorPos;
					m_portraitInstance.transform.localScale = reactorCoreMapMonitorCoordinator.m_largeMonitorScale * Vector3.one;
				}
				m_portraitRenderers = m_portraitInstance.GetComponentsInChildren<Renderer>();
				m_portraitInstance.SetActive(false);
				reactorCoreMapMonitorCoordinator.AddPortraitController(this);
			}
			else
			{
				Debug.LogWarning(new StringBuilder().Append("Did not find prefab to spawn for ReactorCore respawn portraits on ").Append(base.name).ToString());
			}
			if (m_portraitAssetType == PortraitAssetType.PlainTexture)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_backgroundRenderer = GetComponent<Renderer>();
						if (m_backgroundRenderer != null)
						{
							m_backgroundRenderer.material = reactorCoreMapMonitorCoordinator.m_searchingMaterial;
						}
						m_fadeGroupInParent = GetComponentInParent<FadeObjectGroup>();
						return;
					}
				}
			}
			if (m_portraitAssetType != 0)
			{
				return;
			}
			while (true)
			{
				GameObject gameObject2 = null;
				GameObject gameObject3 = null;
				if (m_respawnMonitorSize == RespawnMonitorSize.Small)
				{
					gameObject2 = reactorCoreMapMonitorCoordinator.m_vfxPrefabSmallSearching;
					gameObject3 = reactorCoreMapMonitorCoordinator.m_vfxPrefabSmallResurrecting;
				}
				else if (m_respawnMonitorSize == RespawnMonitorSize.Large)
				{
					gameObject2 = reactorCoreMapMonitorCoordinator.m_vfxPrefabLargeSearching;
					gameObject3 = reactorCoreMapMonitorCoordinator.m_vfxPrefabLargeResurrecting;
				}
				if (gameObject2 != null)
				{
					m_vfxInstanceSearching = SpawnVfxInstance(gameObject2);
				}
				if (gameObject3 != null)
				{
					while (true)
					{
						m_vfxInstanceResurrecting = SpawnVfxInstance(gameObject3);
						m_vfxInstanceResurrecting.SetActive(false);
						return;
					}
				}
				return;
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
		GameObject gameObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
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
			if (reactorCoreMapMonitorCoordinator != null)
			{
				spriteToDisplay = reactorCoreMapMonitorCoordinator.m_fallbackPortraitSprite;
			}
		}
		if (m_portraitRenderers != null)
		{
			if (spriteToDisplay != null)
			{
				SetPortraitRendererTextures(spriteToDisplay.texture);
				m_portraitInstance.SetActive(true);
				m_desiredVisible = true;
				if (m_portraitAssetType == PortraitAssetType.PlainTexture && m_backgroundRenderer != null)
				{
					if (reactorCoreMapMonitorCoordinator != null)
					{
						m_backgroundRenderer.material = reactorCoreMapMonitorCoordinator.m_resurrectingMaterial;
					}
				}
			}
		}
		if (m_portraitAssetType == PortraitAssetType.Vfx)
		{
			SetVfxAsResurrecting(true);
		}
	}

	public void HidePortrait()
	{
		if (m_portraitRenderers == null)
		{
			return;
		}
		SetPortraitRendererTextures(null);
		m_portraitInstance.SetActive(false);
		m_desiredVisible = false;
		ReactorCoreMapMonitorCoordinator reactorCoreMapMonitorCoordinator = ReactorCoreMapMonitorCoordinator.Get();
		if (m_portraitAssetType == PortraitAssetType.PlainTexture)
		{
			if (m_backgroundRenderer != null && reactorCoreMapMonitorCoordinator != null)
			{
				m_backgroundRenderer.material = reactorCoreMapMonitorCoordinator.m_searchingMaterial;
			}
		}
		if (m_portraitAssetType != 0)
		{
			return;
		}
		while (true)
		{
			SetVfxAsResurrecting(false);
			return;
		}
	}

	private void SetPortraitRendererTextures(Texture texture)
	{
		if (m_portraitRenderers == null)
		{
			return;
		}
		for (int i = 0; i < m_portraitRenderers.Length; i++)
		{
			Renderer renderer = m_portraitRenderers[i];
			if (renderer != null)
			{
				if (renderer.material != null)
				{
					renderer.material.mainTexture = texture;
				}
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

	private void SetVfxAsResurrecting(bool resurrecting)
	{
		if (m_vfxInstanceResurrecting != null)
		{
			m_vfxInstanceResurrecting.SetActive(resurrecting);
		}
		if (!(m_vfxInstanceSearching != null))
		{
			return;
		}
		while (true)
		{
			m_vfxInstanceSearching.SetActive(!resurrecting);
			return;
		}
	}

	private void Update()
	{
		if (!(m_portraitInstance != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_fadeGroupInParent != null))
			{
				return;
			}
			float currentAlpha = m_fadeGroupInParent.GetCurrentAlpha();
			int num;
			if (m_desiredVisible)
			{
				num = ((currentAlpha > 0.98f) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
			if (flag != m_portraitInstance.activeSelf)
			{
				m_portraitInstance.SetActive(flag);
			}
			return;
		}
	}
}
