using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using WebSocketSharp;

public class DiscordClientInterface : MonoBehaviour
{
	public Action<bool> OnConnected;
	public Action OnDisconnected;
	public Action<string> OnAuthorized;
	public Action<DiscordUserInfo> OnAuthenticated;
	public Action OnJoined;
	public Action OnLeft;
	public Action<DiscordUserInfo> OnUserJoined;
	public Action<DiscordUserInfo> OnUserLeft;
	public Action<DiscordUserInfo> OnUserSpeakingChanged;

	private static DiscordClientInterface s_instance;
	public static int s_RpcPortOverride;
	public static bool s_debugOutput;

	private Action<int, int, string> m_pushToTalkScanCallback;
	private List<DiscordUserInfo> m_discordChannelUsers;
	private static bool s_sdkInitialized;
	private WebSocketSharp.WebSocket m_webSocket;
	private string m_rpcUrl;
	private int m_rpcPortOffset;
	private bool m_retryToConnect;
	private DiscordAuthInfo m_authInfo;
	private DiscordUserInfo m_userInfo;
	private DiscordChannelInfo m_channelInfo;
	private JsonSerializerSettings m_jsonDateFormatSettings;
	private Scheduler m_rpcScheduler;

	private static readonly int RPC_PORT_BEGIN = 0x193F;
	private static readonly int RPC_PORT_END = 0x1949;
	private static readonly int RPC_COMMAND_TIMEOUT_SEC = 5;

	public DiscordClientInterface()
	{
		OnConnected = delegate {};
		OnDisconnected = delegate {};
		OnAuthorized = delegate {};
		OnAuthenticated = delegate {};
		OnJoined = delegate {};
		OnLeft = delegate {};
		OnUserJoined = delegate {};
		OnUserLeft = delegate {};
		OnUserSpeakingChanged = delegate {};
		OnError = delegate {};
		m_discordChannelUsers = new List<DiscordUserInfo>();
		
	}

	public SynchronizationContext SynchronizationContext { get; set; }

	public static DiscordClientInterface Get()
	{
		return s_instance;
	}

	public event Action<ErrorEventArgs> OnError;

	public static bool IsEnabled
	{
		get { return false; }
	}

	public static bool IsSdkEnabled
	{
		get { return false; }
	}

	public static bool IsInstalled
	{
		get
		{
			try
			{
				string name = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
				RegistryKey key = Registry.CurrentUser.OpenSubKey(name);
				if (key != null)
				{
					foreach (RegistryKey registryKey in from keyName in key.GetSubKeyNames() select key.OpenSubKey(keyName))
					{
						if (registryKey.GetValue("DisplayName") is string text && text.Contains("Discord"))
						{
							return true;
						}
					}
					key.Close();
				}
			}
			catch (Exception ex)
			{
				Log.Error(new StringBuilder().Append("Failed to check Discord installation in registry. ").Append(ex).ToString());
			}
			return false;
		}
	}

	public static bool CanJoinTeamChat
	{
		get
		{
			GameManager gameManager = GameManager.Get();
			return gameManager != null
			       && gameManager.GameInfo != null
			       && gameManager.GameInfo.GameStatus != GameStatus.Stopped
			       && gameManager.GameInfo.GameConfig != null
			       && (gameManager.GameInfo.GameConfig.GameType.IsQueueable() || gameManager.GameInfo.IsCustomGame)
			       && gameManager.PlayerInfo != null;
		}
	}

	public static bool CanJoinGroupChat
	{
		get
		{
			LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
			return groupInfo != null && groupInfo.InAGroup;
		}
	}

	public bool IsConnected
	{
		get { return m_webSocket != null && m_webSocket.IsAlive; }
	}

	public DiscordChannelInfo ChannelInfo
	{
		get { return m_channelInfo; }
	}

	public DiscordUserInfo UserInfo
	{
		get { return m_userInfo; }
	}

	public List<DiscordUserInfo> ChannelUsers
	{
		get { return m_discordChannelUsers; }
	}

	private int RpcPort
	{
		get { return s_RpcPortOverride != 0 ? s_RpcPortOverride : RPC_PORT_BEGIN + m_rpcPortOffset; }
	}

	private string RpcOrigin
	{
		get { return m_authInfo != null ? m_authInfo.RpcOrigin : string.Empty; }
	}

	private string ClientId
	{
		get { return m_authInfo == null ? string.Empty : m_authInfo.ClientId; }
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void Start()
	{
		m_jsonDateFormatSettings = new JsonSerializerSettings
		{
			DateFormatHandling = DateFormatHandling.IsoDateFormat,
			DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
		};
		m_rpcScheduler = new Scheduler();
		m_rpcPortOffset = 0;
		s_RpcPortOverride = 0;
		SynchronizationContext = SynchronizationContext.Current;
	}

	private void Update()
	{
	}

	private static void Debug(string msg, params object[] parameters)
	{
		if (s_debugOutput)
		{
			string str = string.Format(msg, parameters);
			Log.Info(new StringBuilder().Append("Discord | ").Append(str).ToString());
			if (SynchronizationContext.Current != null && TextConsole.Get() != null)
			{
				TextConsole.Get().Write(new StringBuilder().Append("Discord | ").Append(str).ToString(), ConsoleMessageType.DiscordLog);
			}
		}
	}

	private static void Debug(RpcResponse resp)
	{
		if (s_debugOutput)
		{
			Log.Info(new StringBuilder().Append("Discord | ").Append(JsonConvert.SerializeObject(resp, Formatting.Indented)).ToString());
		}
	}

	private void Authorize()
	{
		m_webSocket.Send(JsonConvert.SerializeObject(new RpcRequest
		{
			nonce = Guid.NewGuid().ToString(),
			args = new Dictionary<object, object>
			{
				["client_id"] = m_authInfo.ClientId,
				["scopes"] = new List<string>
				{
					"rpc",
					"rpc.api",
					"guilds.join"
				},
				["rpc_token"] = m_authInfo.RpcToken
			},
			cmd = "AUTHORIZE"
		}));
	}

	public void Authenticate(DiscordUserInfo userInfo)
	{
		m_userInfo = userInfo;
		m_webSocket.Send(JsonConvert.SerializeObject(new RpcRequest
		{
			nonce = Guid.NewGuid().ToString(),
			args = new Dictionary<object, object>
			{
				["access_token"] = m_userInfo.AccessToken
			},
			cmd = "AUTHENTICATE"
		}));
	}

	private void Invoke(string jsonMessage)
	{
		RpcWebSocketEnvelope rpcWebSocketEnvelope = default(RpcWebSocketEnvelope);
		rpcWebSocketEnvelope.JsonMessage = jsonMessage;
		Action<string> messageDelegate = Dispatch;
		Action<string> invokerDelegate = delegate(string msg)
		{
			messageDelegate(msg);
		};
		rpcWebSocketEnvelope.InvokerDelegate = invokerDelegate;
		rpcWebSocketEnvelope.MethodInfo = messageDelegate.Method;
		if (SynchronizationContext != null)
		{
			SynchronizationContext.Post(InvokeAsync, rpcWebSocketEnvelope, rpcWebSocketEnvelope.MethodInfo);
		}
		else
		{
			InvokeAsync(rpcWebSocketEnvelope);
		}
	}

	private void InvokeAsync(object _envelope)
	{
		RpcWebSocketEnvelope rpcWebSocketEnvelope = (RpcWebSocketEnvelope)_envelope;
		rpcWebSocketEnvelope.InvokerDelegate(rpcWebSocketEnvelope.JsonMessage);
	}

	private void Dispatch(string jsonMessage)
	{
		try
		{
			RpcResponse rpcResponse = JsonConvert.DeserializeObject<RpcResponse>(jsonMessage, m_jsonDateFormatSettings);
			JObject resp = rpcResponse.data != null ? JObject.Parse(rpcResponse.data.ToString()) : null;
			Debug(rpcResponse);
			switch (rpcResponse.cmd)
			{
				case "CONNECTION_OPEN":
				{
					Debug("Connected to 127.0.0.1:{0}", RpcPort);
					OnConnected(m_authInfo == null);
					m_retryToConnect = false;
					m_rpcPortOffset = 0;
					break;
				}
				case "CONNECTION_ERROR":
					Debug("Connection error to 127.0.0.1:{0}", RpcPort);
					break;
				case "CONNECTION_CLOSE" when m_retryToConnect:
				{
					m_rpcPortOffset++;
					if (RPC_PORT_BEGIN + m_rpcPortOffset > RPC_PORT_END)
					{
						Debug("Failed to connect to discord");
						Disconnect();
						OnDisconnected();
					}
					else
					{
						m_webSocket.Close();
						m_webSocket = null;
						TryConnect();
					}

					break;
				}
				case "CONNECTION_CLOSE":
				{
					Debug("Disconnected from discord");
					if (m_authInfo != null)
					{
						OnLeft();
					}
					Disconnect();
					OnDisconnected();
					break;
				}
				case "DISPATCH":
					switch (rpcResponse.evt)
					{
						case "READY" when m_authInfo != null && m_userInfo == null:
							Authorize();
							break;
						case "CAPTURE_SHORTCUT_CHANGE" when resp != null:
						{
							JToken jtoken = resp.SelectToken("shortcut").ElementAt(0);
							int keyType = Convert.ToInt32(jtoken.SelectToken("type").ToString());
							int keyCode = Convert.ToInt32(jtoken.SelectToken("code").ToString());
							string text = jtoken.SelectToken("name").ToString();
							if (ClientGameManager.Get() != null)
							{
								ClientGameManager.Get().SetPushToTalkKey(keyType, keyCode, text);
							}
							m_pushToTalkScanCallback?.Invoke(keyType, keyCode, text);
							ScanPushToTalkKey(false, null);
							RefreshSettings();
							break;
						}
						case "VOICE_STATE_CREATE" when resp != null:
						{
							bool found = false;
							ulong userId = ulong.Parse(resp.SelectToken("user.id").ToString());
							foreach (DiscordUserInfo user in m_discordChannelUsers)
							{
								if (user.UserId == userId)
								{
									found = true;
									break;
								}
							}
							if (!found)
							{
								DiscordUserInfo discordUserInfo = new DiscordUserInfo
								{
									UserId = userId,
									UserName = resp.SelectToken("user.username").ToString(),
									Discriminator = resp.SelectToken("user.discriminator").ToString()
								};
								m_discordChannelUsers.Add(discordUserInfo);
								OnUserJoined(discordUserInfo);
							}
							break;
						}
						case "VOICE_STATE_DELETE" when resp != null:
						{
							ulong userId = ulong.Parse(resp.SelectToken("user.id").ToString());
							for (int i = 0; i < m_discordChannelUsers.Count; i++)
							{
								if (m_discordChannelUsers[i].UserId == userId)
								{
									DiscordUserInfo user = m_discordChannelUsers[i];
									m_discordChannelUsers.RemoveAt(i);
									OnUserLeft(user);
									break;
								}
							}
							break;
						}
						default:
						{
							if ((rpcResponse.evt == "SPEAKING_START" || rpcResponse.evt == "SPEAKING_STOP") && resp != null)
							{
								ulong userId = ulong.Parse(resp.SelectToken("user_id").ToString());
								foreach (DiscordUserInfo user in m_discordChannelUsers)
								{
									if (user.UserId == userId)
									{
										user.IsSpeaking = rpcResponse.evt == "SPEAKING_START";
										OnUserSpeakingChanged(user);
										break;
									}
								}
							}
							break;
						}
					}
					break;
				case "AUTHORIZE":
				{
					OnAuthorized(resp.SelectToken("code").ToString());
					break;
				}
				case "AUTHENTICATE" when rpcResponse.evt.IsNullOrEmpty():
				{
					m_userInfo.UserId = ulong.Parse(resp.SelectToken("user.id").ToString());
					OnAuthenticated(m_userInfo);
					break;
				}
				case "AUTHENTICATE":
					Debug("Failed to authenticate RPC connection {0}", resp);
					break;
				case "TRY_SELECT_VOICE_CHANNEL":
				{
					int code = int.Parse(resp.SelectToken("code").ToString());
					bool force = code == 0x138B;
					TrySelectVoiceChannel(force);
					break;
				}
				case "SELECT_VOICE_CHANNEL" when rpcResponse.evt.IsNullOrEmpty():
					Debug("Connected to voice channel {0}", m_channelInfo.VoiceChannelId);
					OnJoined();
					break;
				case "SELECT_VOICE_CHANNEL":
					Debug("Failed to connect to voice channel {0} {1}. Retrying", m_channelInfo.VoiceChannelId, resp);
					RetrySelectVoiceChannel(resp);
					break;
			}
		}
		catch (Exception ex)
		{
			Log.Error("DISCORD | exception {0}", ex);
		}
	}

	private void HandleOnOpen(object sender, EventArgs e)
	{
		Invoke(JsonConvert.SerializeObject(new RpcResponse
		{
			cmd = "CONNECTION_OPEN"
		}));
	}

	private void HandleOnMessage(object sender, MessageEventArgs e)
	{
		Invoke(e.Data);
	}

	private void HandleOnError(object sender, ErrorEventArgs e)
	{
		Invoke(JsonConvert.SerializeObject(new RpcResponse
		{
			cmd = "CONNECTION_ERROR",
			evt = e.Exception.Message
		}));
	}

	private void HandleOnClose(object sender, CloseEventArgs e)
	{
		Invoke(JsonConvert.SerializeObject(new RpcResponse
		{
			cmd = "CONNECTION_CLOSE",
			evt = e.Reason
		}));
	}

	public void Connect(DiscordAuthInfo authInfo, int portOffset = 0)
	{
		m_authInfo = authInfo;
		m_rpcPortOffset = portOffset;
		if (IsSdkEnabled && !s_sdkInitialized)
		{
			InitializeSdk();
		}
		else
		{
			TryConnect();
		}
	}

	private void InitializeSdk()
	{
		string name = "Hydrogen.DiscordSdk";
		Mutex mutex = new Mutex(true, name, out bool flag);
		if (!flag || mutex == null)
		{
			Debug("failed to initialize Sdk. Discord does not support launching multiple Sdk processes on the same computer at one time.");
			return;
		}
		string clientId = m_authInfo.ClientId;
		string resourcePath = new StringBuilder().Append(Application.dataPath).Append("/../").ToString();
		GameBridge.SetReadyCallback(HandleSdkReadyCallback, UIntPtr.Zero);
		GameBridge.SetUpdatingCallback(HandleSdkUpdatingCallback, UIntPtr.Zero);
		GameBridge.SetErrorCallback(HandleSdkErrorCallback, UIntPtr.Zero);
		GameBridge.CaptureOutput(delegate(uint type, string message, UIntPtr context) { Debug("[OUTPUT] {0}", message); }, UIntPtr.Zero);
		GameBridge.Initialize(clientId, resourcePath);
		s_sdkInitialized = true;
		Debug("initialized Sdk");
	}

	public static void Shutdown()
	{
		if (s_sdkInitialized)
		{
			GameBridge.Shutdown();
			s_sdkInitialized = false;
			Debug("shut down Sdk");
		}
	}

	private void HandleSdkReadyCallback(ushort port, UIntPtr context)
	{
		Debug("SdkReadyCallback {0}", port);
		s_RpcPortOverride = port;
		TryConnect();
	}

	private void HandleSdkUpdatingCallback(uint progress, UIntPtr context)
	{
		Debug("SdkUpdatingCallback {0}", progress);
	}

	private void HandleSdkErrorCallback(uint code, [MarshalAs(UnmanagedType.LPStr)] string message, UIntPtr context)
	{
		Debug("SdkErrorCallback {0} {1}", code, message);
	}

	private void TryConnect()
	{
		if (m_webSocket != null && m_webSocket.IsAlive)
		{
			Debug("Already connected to discord");
			return;
		}

		m_rpcUrl = new StringBuilder().Append("ws://127.0.0.1:").Append(RpcPort).Append("/?v=1&client_id=").Append(ClientId).Append("&encoding=json").ToString();
		Debug("Connecting to 127.0.0.1:{0}", RpcPort);
		m_webSocket = new WebSocketSharp.WebSocket(m_rpcUrl);
		m_webSocket.OnOpen += HandleOnOpen;
		m_webSocket.OnMessage += HandleOnMessage;
		m_webSocket.OnError += HandleOnError;
		m_webSocket.OnClose += HandleOnClose;
		if (s_debugOutput)
		{
			m_webSocket.Logger.Level = LogLevel.Trace;
		}
		m_webSocket.Origin = RpcOrigin;
		m_webSocket.WaitTime = TimeSpan.FromMilliseconds(100.0);
		m_webSocket.ConnectAsync();
		m_retryToConnect = true;
	}

	public void Disconnect()
	{
		if (m_webSocket != null)
		{
			m_webSocket.Close();
			m_webSocket = null;
		}
		m_retryToConnect = false;
		m_rpcPortOffset = 0;
		m_authInfo = null;
		m_userInfo = null;
		m_channelInfo = null;
		m_discordChannelUsers.Clear();
	}

	public void SelectVoiceChannel(DiscordChannelInfo channelInfo)
	{
		m_channelInfo = channelInfo;
		TrySelectVoiceChannel();
	}

	private void TrySelectVoiceChannel(bool force = false)
	{
		if (m_webSocket == null || !m_webSocket.IsAlive || m_channelInfo == null)
		{
			return;
		}
		RpcRequest rpcRequest = new RpcRequest
		{
			nonce = Guid.NewGuid().ToString(),
			args = new Dictionary<object, object>
			{
				["channel_id"] = m_channelInfo.VoiceChannelId.ToString(),
				["timeout"] = RPC_COMMAND_TIMEOUT_SEC
			}
		};
		if (force)
		{
			rpcRequest.args["force"] = force;
		}
		rpcRequest.cmd = "SELECT_VOICE_CHANNEL";
		m_webSocket.Send(JsonConvert.SerializeObject(rpcRequest));
	}

	private void RetrySelectVoiceChannel(object responseData = null)
	{
		m_rpcScheduler.AddTask(
			delegate
			{
				RpcResponse value = new RpcResponse
				{
					cmd = "TRY_SELECT_VOICE_CHANNEL",
					data = responseData
				};
				string jsonMessage = JsonConvert.SerializeObject(value);
				Invoke(jsonMessage);
			},
			(int)TimeSpan.FromSeconds(2.0).TotalMilliseconds);
	}

	public void RefreshSettings()
	{
		if (m_webSocket == null || !m_webSocket.IsAlive || m_channelInfo == null)
		{
			return;
		}
		Options_UI options_UI = Options_UI.Get();
		if (options_UI != null)
		{
			return;
		}
		RpcRequest rpcRequest = new RpcRequest
		{
			nonce = Guid.NewGuid().ToString(),
			args = new Dictionary<object, object>
			{
				["input"] = new
				{
					volume = options_UI.GetVoiceVolume()
				},
				["output"] = new
				{
					volume = options_UI.GetMicVolume()
				},
				["mute"] = options_UI.GetVoiceMute()
			},
			cmd = "SET_VOICE_SETTINGS"
		};
		Dictionary<object, object> mode = new Dictionary<object, object>();
		rpcRequest.args["mode"] = mode;
		mode["type"] = options_UI.GetVoicePushToTalk() ? "PUSH_TO_TALK" : "VOICE_ACTIVITY";
		if (ClientGameManager.Get() != null && ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			AccountComponent accountComponent =
				ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
			mode["shortcut"] = new object[]
			{
				new
				{
					type = accountComponent.PushToTalkKeyType,
					code = accountComponent.PushToTalkKeyCode,
					name = accountComponent.PushToTalkKeyName
				}
			};
		}

		m_webSocket.Send(JsonConvert.SerializeObject(rpcRequest));
	}

	public bool ScanPushToTalkKey(bool start, Action<int, int, string> callback)
	{
		if (m_webSocket == null || !m_webSocket.IsAlive || m_channelInfo == null)
		{
			return false;
		}
		m_pushToTalkScanCallback = callback;
		m_webSocket.Send(JsonConvert.SerializeObject(new RpcRequest
		{
			nonce = Guid.NewGuid().ToString(),
			args = new Dictionary<object, object>
			{
				["action"] = start ? "START" : "STOP"
			},
			cmd = "CAPTURE_SHORTCUT"
		}));
		return true;
	}

	internal struct RpcWebSocketEnvelope
	{
		public string JsonMessage;
		public MethodInfo MethodInfo;
		public Action<string> InvokerDelegate;
	}

	internal class RpcRequest
	{
		public string nonce { get; set; }
		public Dictionary<object, object> args { get; set; }
		public string cmd { get; set; }
	}

	internal class RpcResponse
	{
		public string cmd { get; set; }
		public object data { get; set; }
		public string evt { get; set; }
		public string nonce { get; set; }
	}
}
