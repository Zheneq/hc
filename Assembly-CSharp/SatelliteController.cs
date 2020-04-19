using System;
using UnityEngine;
using UnityEngine.Networking;

public class SatelliteController : NetworkBehaviour
{
	public GameObject[] m_satelliteModelDataPrefabs;

	private PrefabResourceLink[] m_overrideSatelliteResourceLinks;

	protected PersistentSatellite[] m_satelliteInstances;

	private bool m_createdSatellites;

	private bool m_delayedOnAssignedToBoardSquare;

	private SerializeHelper m_serializeHelper = new SerializeHelper();

	private void SetupPrefabsFromOverrides()
	{
		for (int i = 0; i < this.m_satelliteModelDataPrefabs.Length; i++)
		{
			if (i < this.m_overrideSatelliteResourceLinks.Length)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SatelliteController.SetupPrefabsFromOverrides()).MethodHandle;
				}
				if (this.m_overrideSatelliteResourceLinks[i] != null)
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
					this.m_satelliteModelDataPrefabs[i] = this.m_overrideSatelliteResourceLinks[i].GetPrefab(false);
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
	}

	private void CreateSatellites()
	{
		if (!this.m_createdSatellites)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SatelliteController.CreateSatellites()).MethodHandle;
			}
			this.m_satelliteInstances = new PersistentSatellite[this.m_satelliteModelDataPrefabs.Length];
			for (int i = 0; i < this.m_satelliteModelDataPrefabs.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_satelliteModelDataPrefabs[i]);
				if (gameObject)
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
					this.m_satelliteInstances[i] = gameObject.GetComponent<PersistentSatellite>();
					if (this.m_satelliteInstances[i] != null)
					{
						int layer = LayerMask.NameToLayer("Actor");
						foreach (Transform transform in this.m_satelliteInstances[i].gameObject.GetComponentsInChildren<Transform>(true))
						{
							transform.gameObject.layer = layer;
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
				if (this.m_satelliteInstances[i] != null)
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
					this.m_satelliteInstances[i].Setup(this);
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
			this.m_createdSatellites = true;
		}
	}

	public PersistentSatellite GetSatellite(int index)
	{
		PersistentSatellite result = null;
		if (this.m_satelliteInstances != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SatelliteController.GetSatellite(int)).MethodHandle;
			}
			if (index < this.m_satelliteInstances.Length)
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
				result = this.m_satelliteInstances[index];
			}
		}
		return result;
	}

	public void OnAssignedToInitialBoardSquare()
	{
		if (this.m_createdSatellites)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SatelliteController.OnAssignedToInitialBoardSquare()).MethodHandle;
			}
			for (int i = 0; i < this.m_satelliteInstances.Length; i++)
			{
				if (this.m_satelliteInstances[i] != null)
				{
					this.m_satelliteInstances[i].OnAssignedToInitialBoardSquare();
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
		else
		{
			this.m_delayedOnAssignedToBoardSquare = true;
		}
	}

	private void OnRespawn()
	{
		if (this.m_satelliteInstances != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SatelliteController.OnRespawn()).MethodHandle;
			}
			for (int i = 0; i < this.m_satelliteInstances.Length; i++)
			{
				if (this.m_satelliteInstances[i] != null)
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
					this.m_satelliteInstances[i].OnRespawn();
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

	public void OnActorDeath()
	{
		if (this.m_satelliteInstances != null)
		{
			for (int i = 0; i < this.m_satelliteInstances.Length; i++)
			{
				if (this.m_satelliteInstances[i] != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(SatelliteController.OnActorDeath()).MethodHandle;
					}
					this.m_satelliteInstances[i].OnActorDeath();
				}
			}
		}
	}

	private void Update()
	{
		if (this.m_delayedOnAssignedToBoardSquare && this.m_createdSatellites)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SatelliteController.Update()).MethodHandle;
			}
			this.m_delayedOnAssignedToBoardSquare = false;
			this.OnAssignedToInitialBoardSquare();
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		return this.OnSerializeHelper(new NetworkWriterAdapter(writer), initialState);
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SatelliteController.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			num = reader.ReadPackedUInt32();
		}
		if (num != 0U)
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
			this.OnSerializeHelper(new NetworkReaderAdapter(reader), initialState);
		}
	}

	private bool OnSerializeHelper(IBitStream stream, bool initialState)
	{
		if (!initialState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SatelliteController.OnSerializeHelper(IBitStream, bool)).MethodHandle;
			}
			if (this.m_serializeHelper.ShouldReturnImmediately(ref stream))
			{
				return false;
			}
		}
		int num = 0;
		if (stream.isWriting)
		{
			num = this.m_overrideSatelliteResourceLinks.Length;
			stream.Serialize(ref num);
			for (int i = 0; i < num; i++)
			{
				PrefabResourceLink.Stream(stream, ref this.m_overrideSatelliteResourceLinks[i]);
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
		if (stream.isReading)
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
			stream.Serialize(ref num);
			this.m_overrideSatelliteResourceLinks = new PrefabResourceLink[num];
			for (int j = 0; j < num; j++)
			{
				PrefabResourceLink.Stream(stream, ref this.m_overrideSatelliteResourceLinks[j]);
			}
			this.SetupPrefabsFromOverrides();
			this.CreateSatellites();
		}
		return this.m_serializeHelper.End(initialState, base.syncVarDirtyBits);
	}

	private void UNetVersion()
	{
	}
}
