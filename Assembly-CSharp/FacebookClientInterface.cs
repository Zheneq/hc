using LobbyGameClientMessages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookClientInterface : MonoBehaviour
{
	private static FacebookClientInterface s_instance;

	private string m_message;

	private Texture2D m_texture;

	public static FacebookClientInterface Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void Start()
	{
	}

	public void Connect(OAuthInfo authInfo, string language, string message)
	{
		Application.OpenURL($"https://graph.facebook.com/oauth/authorize?client_id={authInfo.ClientId}&scope={authInfo.Scope}&redirect_uri={authInfo.RedirectUri}&state={authInfo.UserToken}&language={language}");
		m_message = message;
	}

	public void TakeScreenshot(Action<Texture2D> callback)
	{
		StartCoroutine(StartTakingScreenshot(callback));
	}

	public IEnumerator StartTakingScreenshot(Action<Texture2D> callback)
	{
		yield return new WaitForEndOfFrame();
		/*Error: Unable to find new state assignment for yield return*/;
	}

	public void UploadScreenshot(string accessToken)
	{
		StartCoroutine(StartUploadingScreenshot(accessToken));
	}

	public IEnumerator StartUploadingScreenshot(string accessToken)
	{
		WWWForm form = new WWWForm();
		form.AddBinaryData("photo", m_texture.EncodeToJPG(50), "screenshot.jpg");
		form.AddField("message", m_message);
		Dictionary<string, string> headers = form.headers;
		byte[] rawData = form.data;
		string url = "https://graph.facebook.com/v2.4/me/photos?access_token=" + accessToken;
		yield return new WWW(url, rawData, headers);
		/*Error: Unable to find new state assignment for yield return*/;
	}

	public void Reset()
	{
		m_message = null;
		if (!(m_texture != null))
		{
			return;
		}
		while (true)
		{
			UnityEngine.Object.DestroyImmediate(m_texture);
			m_texture = null;
			return;
		}
	}
}
