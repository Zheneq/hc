using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LobbyGameClientMessages;
using UnityEngine;

public class FacebookClientInterface : MonoBehaviour
{
	private static FacebookClientInterface s_instance;

	private string m_message;

	private Texture2D m_texture;

	public static FacebookClientInterface Get()
	{
		return FacebookClientInterface.s_instance;
	}

	private void Awake()
	{
		FacebookClientInterface.s_instance = this;
	}

	private void Start()
	{
	}

	public void Connect(OAuthInfo authInfo, string language, string message)
	{
		Application.OpenURL(string.Format("https://graph.facebook.com/oauth/authorize?client_id={0}&scope={1}&redirect_uri={2}&state={3}&language={4}", new object[]
		{
			authInfo.ClientId,
			authInfo.Scope,
			authInfo.RedirectUri,
			authInfo.UserToken,
			language
		}));
		this.m_message = message;
	}

	public void TakeScreenshot(Action<Texture2D> callback)
	{
		base.StartCoroutine(this.StartTakingScreenshot(callback));
	}

	public IEnumerator StartTakingScreenshot(Action<Texture2D> callback)
	{
		yield return new WaitForEndOfFrame();
		this.m_texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		this.m_texture.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0);
		this.m_texture.Apply();
		callback(this.m_texture);
		yield break;
	}

	public void UploadScreenshot(string accessToken)
	{
		base.StartCoroutine(this.StartUploadingScreenshot(accessToken));
	}

	public IEnumerator StartUploadingScreenshot(string accessToken)
	{
		WWWForm form = new WWWForm();
		form.AddBinaryData("photo", this.m_texture.EncodeToJPG(0x32), "screenshot.jpg");
		form.AddField("message", this.m_message);
		Dictionary<string, string> headers = form.headers;
		byte[] rawData = form.data;
		string url = new StringBuilder().Append("https://graph.facebook.com/v2.4/me/photos?access_token=").Append(accessToken).ToString();
		WWW www = new WWW(url, rawData, headers);
		yield return www;
		if (!www.error.IsNullOrEmpty())
		{
			Log.Warning("There was an error: {0} {1}", new object[]
			{
				www.error,
				www.text
			});
		}
		this.Reset();
		yield break;
	}

	public void Reset()
	{
		this.m_message = null;
		if (this.m_texture != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m_texture);
			this.m_texture = null;
		}
	}
}
