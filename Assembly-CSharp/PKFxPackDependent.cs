using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKFxPackDependent : MonoBehaviour
{
	private IEnumerator _WaitForPack()
	{
		yield return null;
		/*Error: Unable to find new state assignment for yield return*/;
	}

	private IEnumerator _WaitForPackLoaded()
	{
		yield return null;
		/*Error: Unable to find new state assignment for yield return*/;
	}

	public Coroutine WaitForPack(bool isLoaded)
	{
		if (isLoaded)
		{
			return StartCoroutine("_WaitForPack");
		}
		return StartCoroutine("_WaitForPackLoaded");
	}

	private bool checkHash(KeyValuePair<string, string> entry, List<KeyValuePair<string, string>> deviceContent)
	{
		using (List<KeyValuePair<string, string>>.Enumerator enumerator = deviceContent.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current = enumerator.Current;
				if (current.Key == entry.Key)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							return current.Value == entry.Value;
						}
					}
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public virtual void BaseInitialize()
	{
		if (PKFxManager.m_PackLoaded)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			PKFxManager.Startup();
			StartCoroutine("CopyPackAsyncOnAndroid");
			WaitForPack(false);
			if (Application.platform != RuntimePlatform.Android)
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
				if (!PKFxManager.TryLoadPackRelative())
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
					PKFxManager.LoadPack(PKFxManager.m_PackPath + "/PackFx");
				}
			}
			PKFxManager.m_PackLoaded = true;
			return;
		}
	}

	private void Start()
	{
		BaseInitialize();
	}

	private IEnumerator CopyPackAsyncOnAndroid()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					string fromPath = Application.streamingAssetsPath + "/";
					string toPath = Application.persistentDataPath + "/";
					List<KeyValuePair<string, string>> archiveContent = new List<KeyValuePair<string, string>>();
					yield return new WWW(fromPath + "Index.txt");
					/*Error: Unable to find new state assignment for yield return*/;
				}
				}
			}
		}
		PKFxManager.m_PackCopied = true;
		yield return null;
	}
}
