// ROGUES
// SERVER
//using System;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class SatelliteController : NetworkBehaviour
{
	public GameObject[] m_satelliteModelDataPrefabs;
	// reactor
	private PrefabResourceLink[] m_overrideSatelliteResourceLinks;
	// rogues
	//private SatelliteController.SyncListPrefabResourceLink m_overrideSatelliteResourceLinks;
	protected PersistentSatellite[] m_satelliteInstances;
	private bool m_createdSatellites;
	private bool m_delayedOnAssignedToBoardSquare;
	// removed in rogues
	private SerializeHelper m_serializeHelper = new SerializeHelper();

	private void SetupPrefabsFromOverrides()
	{
		for (int i = 0; i < m_satelliteModelDataPrefabs.Length; i++)
		{
			if (i < m_overrideSatelliteResourceLinks.Length)
			{
				// reactor
				if (m_overrideSatelliteResourceLinks[i] != null)
				{
					m_satelliteModelDataPrefabs[i] = m_overrideSatelliteResourceLinks[i].GetPrefab();
				}
				// rogues
				//PrefabResourceLink prefabResourceLink = new PrefabResourceLink();
				//prefabResourceLink.SetValues(
				//	m_overrideSatelliteResourceLinks[i].m_resourcePath,
				//	m_overrideSatelliteResourceLinks[i].m_GUID,
				//	m_overrideSatelliteResourceLinks[i].m_debugPrefabPath);
				//m_satelliteModelDataPrefabs[i] = prefabResourceLink.GetPrefab();
			}
		}
	}

	private void CreateSatellites()
	{
		if (m_createdSatellites)
		{
			return;
		}
		m_satelliteInstances = new PersistentSatellite[m_satelliteModelDataPrefabs.Length];
		for (int i = 0; i < m_satelliteModelDataPrefabs.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(m_satelliteModelDataPrefabs[i]);
			if (gameObject != null)
			{
				m_satelliteInstances[i] = gameObject.GetComponent<PersistentSatellite>();
				if (m_satelliteInstances[i] != null)
				{
					int layer = LayerMask.NameToLayer("Actor");
					Transform[] componentsInChildren = m_satelliteInstances[i].gameObject.GetComponentsInChildren<Transform>(true);
					foreach (Transform transform in componentsInChildren)
					{
						transform.gameObject.layer = layer;
					}
				}
			}
			if (m_satelliteInstances[i] != null)
			{
				m_satelliteInstances[i].Setup(this);
			}
		}
		m_createdSatellites = true;
	}

	public PersistentSatellite GetSatellite(int index)
	{
		if (m_satelliteInstances != null && index < m_satelliteInstances.Length)
		{
			return m_satelliteInstances[index];
		}
		return null;
	}

	public void OnAssignedToInitialBoardSquare()
	{
		if (m_createdSatellites)
		{
			foreach (PersistentSatellite satellite in m_satelliteInstances)
			{
				if (satellite != null)
				{
					satellite.OnAssignedToInitialBoardSquare();
				}
			}
		}
		else
		{
			m_delayedOnAssignedToBoardSquare = true;
		}
	}

	private void OnRespawn()
	{
		if (m_satelliteInstances != null)
		{
			foreach (PersistentSatellite satellite in m_satelliteInstances)
			{
				if (satellite != null)
				{
					satellite.OnRespawn();
				}
			}
		}
	}

	public void OnActorDeath()
	{
		if (m_satelliteInstances != null)
		{
			foreach (PersistentSatellite satellite in m_satelliteInstances)
			{
				if (satellite != null)
				{
					satellite.OnActorDeath();
				}
			}
		}
	}

	private void Update()
	{
		if (m_delayedOnAssignedToBoardSquare && m_createdSatellites)
		{
			m_delayedOnAssignedToBoardSquare = false;
			OnAssignedToInitialBoardSquare();
		}
	}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		return OnSerializeHelper(new NetworkWriterAdapter(writer), initialState);
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        uint flags = uint.MaxValue;
        if (!initialState)
        {
            flags = reader.ReadPackedUInt32();
        }
        if (flags != 0)
        {
            OnSerializeHelper(new NetworkReaderAdapter(reader), initialState);
        }
    }

    // removed in rogues
    private bool OnSerializeHelper(IBitStream stream, bool initialState)
	{
		if (!initialState && m_serializeHelper.ShouldReturnImmediately(ref stream))
		{
			return false;
		}
		int num = 0;
		if (stream.isWriting)
		{
			num = m_overrideSatelliteResourceLinks.Length;
			stream.Serialize(ref num);
			for (int i = 0; i < num; i++)
			{
				PrefabResourceLink.Stream(stream, ref m_overrideSatelliteResourceLinks[i]);
			}
		}
		if (stream.isReading)
		{
			stream.Serialize(ref num);
			m_overrideSatelliteResourceLinks = new PrefabResourceLink[num];
			for (int j = 0; j < num; j++)
			{
				PrefabResourceLink.Stream(stream, ref m_overrideSatelliteResourceLinks[j]);
			}
			SetupPrefabsFromOverrides();
			CreateSatellites();
		}
		return m_serializeHelper.End(initialState, base.syncVarDirtyBits);
	}

	// added in rogues
#if SERVER
	public SatelliteController()
	{
		// rogues
		//this.m_overrideSatelliteResourceLinks = new SatelliteController.SyncListPrefabResourceLink();
		//base.InitSyncObject(this.m_overrideSatelliteResourceLinks);
	}
#endif

	// rogues
	//private void MirrorProcessed()
	//{
	//}

	// added in rogues
#if SERVER
	private void Start()
	{
		// rogues
		//this.m_overrideSatelliteResourceLinks.Callback += new SyncList<SatelliteController.PrefabResourceLinkOverride>.SyncListChanged(this.OnOverrideSatelliteResourceLinksUpdated);
	}
#endif

	// added in rogues
	//private void OnOverrideSatelliteResourceLinksUpdated(SyncList<PrefabResourceLinkOverride>.Operation op, int index, PrefabResourceLinkOverride item)
	//{
	//	SetupPrefabsFromOverrides();
	//	CreateSatellites();
	//}

	// TODO LOW looks weird
#if SERVER
	// custom
	private bool TrySetOverridePrefabs(PrefabResourceLink[] sourceList)
	{
		if (sourceList != null && sourceList.Length != 0)
		{
			m_overrideSatelliteResourceLinks = new PrefabResourceLink[sourceList.Length];
			for (int i = 0; i < sourceList.Length; i++)
			{
				m_overrideSatelliteResourceLinks[i] = sourceList[i];
			}
			return true;
		}
		return false;
	}

	// rogues
	//private bool TrySetOverridePrefabs(PrefabResourceLink[] sourceList)
	//{
	//	if (sourceList != null && sourceList.Length != 0)
	//	{
	//		this.m_overrideSatelliteResourceLinks = new SatelliteController.SyncListPrefabResourceLink();
	//		for (int i = 0; i < sourceList.Length; i++)
	//		{
	//			SatelliteController.PrefabResourceLinkOverride prefabResourceLinkOverride = default(SatelliteController.PrefabResourceLinkOverride);
	//			prefabResourceLinkOverride.m_resourcePath = sourceList[i].ResourcePath;
	//			prefabResourceLinkOverride.m_GUID = sourceList[i].GUID;
	//			prefabResourceLinkOverride.m_debugPrefabPath = sourceList[i].m_debugPrefabPath;
	//			this.m_overrideSatelliteResourceLinks.Add(prefabResourceLinkOverride);
	//		}
	//		return true;
	//	}
	//	return false;
	//}
#endif

	// added in rogues
#if SERVER
	public void OverridePrefabs(CharacterResourceLink resourceLink, CharacterVisualInfo visualInfo)
	{
		bool flag = false;
		if (visualInfo.skinIndex < resourceLink.m_skins.Count)
		{
			CharacterSkin characterSkin = resourceLink.m_skins[visualInfo.skinIndex];
			if (visualInfo.patternIndex < characterSkin.m_patterns.Count)
			{
				CharacterPattern characterPattern = characterSkin.m_patterns[visualInfo.patternIndex];
				if (visualInfo.colorIndex < characterPattern.m_colors.Count)
				{
					CharacterColor characterColor = characterPattern.m_colors[visualInfo.colorIndex];
					flag = this.TrySetOverridePrefabs(characterColor.m_satellitePrefabs);
				}
				if (!flag)
				{
					flag = this.TrySetOverridePrefabs(characterPattern.m_satellitePrefabs);
				}
			}
			if (!flag)
			{
				flag = this.TrySetOverridePrefabs(characterSkin.m_satellitePrefabs);
			}
		}
		if (this.m_overrideSatelliteResourceLinks == null)
		{
			// custom
			this.m_overrideSatelliteResourceLinks = new PrefabResourceLink[0];
			// rogues
			//this.m_overrideSatelliteResourceLinks = new SatelliteController.SyncListPrefabResourceLink();
		}
		this.SetupPrefabsFromOverrides();
		this.CreateSatellites();
	}
#endif


	// rogues
	//public struct PrefabResourceLinkOverride
	//{
	//	public string m_resourcePath;

	//	public string m_GUID;

	//	public string m_debugPrefabPath;
	//}

	// rogues
	//public class SyncListPrefabResourceLink : SyncList<SatelliteController.PrefabResourceLinkOverride>
	//{
	//	public override void SerializeItem(NetworkWriter writer, SatelliteController.PrefabResourceLinkOverride item)
	//	{
	//		GeneratedNetworkCode._WritePrefabResourceLinkOverride_SatelliteController(writer, item);
	//	}

	//	public override SatelliteController.PrefabResourceLinkOverride DeserializeItem(NetworkReader reader)
	//	{
	//		return GeneratedNetworkCode._ReadPrefabResourceLinkOverride_SatelliteController(reader);
	//	}
	//}
}
