using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class PKFxPackDependent : MonoBehaviour
{
	private IEnumerator _WaitForPack()
	{
		do
		{
			yield return null;
		}
		while (!PKFxManager.m_PackCopied);
		yield break;
	}

	private IEnumerator _WaitForPackLoaded()
	{
		do
		{
			yield return null;
		}
		while (!PKFxManager.m_PackLoaded);
		yield break;
	}

	public Coroutine WaitForPack(bool isLoaded)
	{
		if (isLoaded)
		{
			return base.StartCoroutine("_WaitForPack");
		}
		return base.StartCoroutine("_WaitForPackLoaded");
	}

	private bool checkHash(KeyValuePair<string, string> entry, List<KeyValuePair<string, string>> deviceContent)
	{
		using (List<KeyValuePair<string, string>>.Enumerator enumerator = deviceContent.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> keyValuePair = enumerator.Current;
				if (keyValuePair.Key == entry.Key)
				{
					return keyValuePair.Value == entry.Value;
				}
			}
		}
		return false;
	}

	public virtual void BaseInitialize()
	{
		if (!PKFxManager.m_PackLoaded)
		{
			PKFxManager.Startup();
			base.StartCoroutine("CopyPackAsyncOnAndroid");
			this.WaitForPack(false);
			if (Application.platform != RuntimePlatform.Android)
			{
				if (!PKFxManager.TryLoadPackRelative())
				{
					PKFxManager.LoadPack(PKFxManager.m_PackPath + "/PackFx");
				}
			}
			PKFxManager.m_PackLoaded = true;
		}
	}

	private void Start()
	{
		this.BaseInitialize();
	}

	private IEnumerator CopyPackAsyncOnAndroid()
	{
		for (;;)
		{
			bool flag = false;
			uint num;
			string toPath;
			byte[] indexOf;
			switch (num)
			{
			case 0U:
				if (Application.platform == RuntimePlatform.Android)
				{
					string fromPath = Application.streamingAssetsPath + "/";
					toPath = Application.persistentDataPath + "/";
					List<KeyValuePair<string, string>> archiveContent = new List<KeyValuePair<string, string>>();
					WWW www = new WWW(fromPath + "Index.txt");
					yield return www;
					continue;
				}
				goto IL_5AA;
			case 1U:
			{
				WWW www;
				indexOf = www.bytes;
				string[] lines = Encoding.ASCII.GetString(indexOf).Split(new char[]
				{
					'\n'
				});
				List<KeyValuePair<string, string>> archiveContent;
				foreach (string text in lines)
				{
					if (text.Length > 1)
					{
						string key = text.Substring(0, text.LastIndexOf(':'));
						string value = text.Substring(text.LastIndexOf(':'));
						archiveContent.Add(new KeyValuePair<string, string>(key, value));
					}
				}
				if (File.Exists(toPath + "Index.txt"))
				{
					List<KeyValuePair<string, string>> deviceContent = new List<KeyValuePair<string, string>>();
					StreamReader sr = new StreamReader(toPath + "Index.txt");
					string line;
					while ((line = sr.ReadLine()) != null)
					{
						string key2 = line.Substring(0, line.LastIndexOf(':'));
						string value2 = line.Substring(line.LastIndexOf(':'));
						deviceContent.Add(new KeyValuePair<string, string>(key2, value2));
					}
					sr.Close();
					List<KeyValuePair<string, string>>.Enumerator enumerator = archiveContent.GetEnumerator();
					goto Block_8;
				}
				Debug.Log("[PKFX] No pack index found, copy everything!");
				List<KeyValuePair<string, string>>.Enumerator enumerator2 = archiveContent.GetEnumerator();
				goto Block_9;
			}
			case 2U:
				goto IL_27C;
			case 3U:
				goto IL_431;
			case 4U:
				goto IL_5C8;
			}
			break;
			IL_5AA:
			PKFxManager.m_PackCopied = true;
			yield return null;
			continue;
			IL_570:
			File.WriteAllBytes(toPath + "Index.txt", indexOf);
			PKFxManager.LoadPack(Application.persistentDataPath + "/PackFx/");
			PKFxManager.m_PackLoaded = true;
			goto IL_5AA;
			Block_8:
			try
			{
				IL_27C:
				List<KeyValuePair<string, string>>.Enumerator enumerator;
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> entry = enumerator.Current;
					List<KeyValuePair<string, string>> deviceContent;
					if (!this.checkHash(entry, deviceContent))
					{
						string fromPath;
						Debug.Log("[PKFX] " + fromPath + entry.Key + " : file doesn't exist or hash differs");
						WWW www = new WWW(fromPath + entry.Key);
						yield return www;
						flag = true;
						string dir = new string((toPath + entry.Key).ToCharArray(), 0, toPath.Length + entry.Key.LastIndexOf('/'));
						if (!Directory.Exists(dir))
						{
							Directory.CreateDirectory(dir);
						}
						File.WriteAllBytes(toPath + entry.Key, www.bytes);
					}
				}
			}
			finally
			{
				if (flag)
				{
				}
				else
				{
					List<KeyValuePair<string, string>>.Enumerator enumerator;
					((IDisposable)enumerator).Dispose();
				}
			}
			goto IL_570;
			Block_9:
			try
			{
				IL_431:
				List<KeyValuePair<string, string>>.Enumerator enumerator2;
				while (enumerator2.MoveNext())
				{
					KeyValuePair<string, string> entry2 = enumerator2.Current;
					string fromPath;
					WWW www = new WWW(fromPath + entry2.Key);
					yield return www;
					flag = true;
					string dir2 = new string((toPath + entry2.Key).ToCharArray(), 0, toPath.Length + entry2.Key.LastIndexOf('/'));
					if (!Directory.Exists(dir2))
					{
						Directory.CreateDirectory(dir2);
					}
					File.WriteAllBytes(toPath + entry2.Key, www.bytes);
				}
			}
			finally
			{
				if (flag)
				{
				}
				else
				{
					List<KeyValuePair<string, string>>.Enumerator enumerator2;
					((IDisposable)enumerator2).Dispose();
				}
			}
			goto IL_570;
		}
		yield break;
		IL_5C8:
		yield break;
	}
}
