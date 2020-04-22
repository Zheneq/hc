using UnityEngine;

[ExecuteInEditMode]
public class ReplaceableVFXPrefab : MonoBehaviour, IGameEventListener
{
	public PrefabResourceLink m_prefabLink;

	private GameObject m_prefab;

	private bool m_prefabInstantiated;

	private void Awake()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplaceVFXPrefab);
			return;
		}
	}

	private void Start()
	{
		m_prefabInstantiated = (base.transform.childCount > 0);
		if (base.transform.childCount > 1)
		{
			Log.Error("VFX prefab bug: {0} has {1} children of the ReplaceableVFXPrefab object, but should have at most 1", base.gameObject.name, base.transform.childCount);
		}
		if (m_prefabInstantiated)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_prefabInstantiated = InstantiatePrefab();
			return;
		}
	}

	private bool InstantiatePrefab()
	{
		bool flag = false;
		if (m_prefab == null)
		{
			while (true)
			{
				switch (7)
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
			m_prefab = m_prefabLink.GetPrefab(true);
		}
		if (m_prefab != null)
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
			GameObject gameObject = Object.Instantiate(m_prefab);
			if (gameObject != null)
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
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				flag = true;
				if (!Application.isPlaying)
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
					gameObject.name = $"{gameObject.name}_DoNotEdit_AutoCreatedByParentReplaceableVFXPrefab";
				}
			}
		}
		if (!flag)
		{
			if (Application.isPlaying)
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
				object[] array = new object[1];
				object obj;
				if (m_prefabLink == null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					obj = "NULL";
				}
				else
				{
					obj = m_prefabLink.ResourcePath;
				}
				array[0] = obj;
				Log.Error("Failed to instantiate prefab link {0}", array);
			}
			else
			{
				object[] array2 = new object[1];
				object obj2;
				if (m_prefabLink == null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					obj2 = "NULL";
				}
				else
				{
					obj2 = m_prefabLink.ResourcePath;
				}
				array2[0] = obj2;
				Debug.LogErrorFormat("Failed to instantiate prefab link {0}", array2);
			}
		}
		return flag;
	}

	private void OnDestroy()
	{
		if (Application.isPlaying)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplaceVFXPrefab);
		}
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		PrefabResourceLink prefabLink = m_prefabLink;
		if (this != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (base.transform != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						GameEventManager.ReplaceVFXPrefab replaceVFXPrefab = (GameEventManager.ReplaceVFXPrefab)args;
						if (!m_prefabInstantiated && replaceVFXPrefab != null)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (replaceVFXPrefab.vfxRoot != null && replaceVFXPrefab.vfxRoot == base.transform.root)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										prefabLink = replaceVFXPrefab.characterResourceLink.ReplacePrefabResourceLink(prefabLink, replaceVFXPrefab.characterVisualInfo);
										if (prefabLink != null)
										{
											while (true)
											{
												switch (4)
												{
												case 0:
													continue;
												}
												break;
											}
											m_prefab = prefabLink.GetPrefab();
										}
										m_prefabInstantiated = InstantiatePrefab();
										return;
									}
								}
							}
						}
						if (replaceVFXPrefab == null)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
								{
									string text;
									if (base.transform.parent != null)
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
										text = base.transform.parent.name;
									}
									else if (m_prefabLink != null)
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
										text = m_prefabLink.ToString();
									}
									else
									{
										text = base.transform.name;
									}
									string str = text;
									Log.Error("ReplaceableVFXPRefab on " + str + " received event with null argument");
									return;
								}
								}
							}
						}
						return;
					}
					}
				}
			}
		}
		Log.Error("ReplaceableVFXPRefab reference or transform is null on event " + eventType);
	}
}
