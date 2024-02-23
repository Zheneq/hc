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
					PKFxManager.LoadPack(new StringBuilder().Append(PKFxManager.m_PackPath).Append("/PackFx").ToString());
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
		// TODO DECOMP looked completely broken (looks like a hack for Android anyway)
		PKFxManager.m_PackCopied = true;
		yield break;
	}
}
