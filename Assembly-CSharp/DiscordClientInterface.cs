using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using WebSocketSharp;

public class DiscordClientInterface : MonoBehaviour
{
	internal struct RpcWebSocketEnvelope
	{
		public string JsonMessage;

		public MethodInfo MethodInfo;

		public Action<string> InvokerDelegate;
	}

	internal class RpcRequest
	{
		public string nonce
		{
			get;
			set;
		}

		public Dictionary<object, object> args
		{
			get;
			set;
		}

		public string cmd
		{
			get;
			set;
		}
	}

	internal class RpcResponse
	{
		public string cmd
		{
			get;
			set;
		}

		public object data
		{
			get;
			set;
		}

		public string evt
		{
			get;
			set;
		}

		public string nonce
		{
			get;
			set;
		}
	}

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

	private static readonly int RPC_PORT_BEGIN = 6463;

	private static readonly int RPC_PORT_END = 6473;

	private static readonly int RPC_COMMAND_TIMEOUT_SEC = 5;

	public SynchronizationContext SynchronizationContext
	{
		get;
		set;
	}

	public static bool IsEnabled => false;

	public static bool IsSdkEnabled => false;

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
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
						{
							IEnumerator<RegistryKey> enumerator = (from keyName in key.GetSubKeyNames()
								select key.OpenSubKey(keyName)).GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									RegistryKey current = enumerator.Current;
									string text = current.GetValue("DisplayName") as string;
									if (text != null && text.Contains("Discord"))
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												break;
											default:
												return true;
											}
										}
									}
								}
							}
							finally
							{
								if (enumerator != null)
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											break;
										default:
											enumerator.Dispose();
											goto end_IL_00b1;
										}
									}
								}
								end_IL_00b1:;
							}
							key.Close();
							goto end_IL_0000;
						}
						}
					}
				}
				end_IL_0000:;
			}
			catch (Exception ex)
			{
				Log.Error("Failed to check Discord installation in registry. {0}", ex);
			}
			return false;
		}
	}

	public static bool CanJoinTeamChat
	{
		get
		{
			GameManager gameManager = GameManager.Get();
			int result;
			if (gameManager != null && gameManager.GameInfo != null)
			{
				if (gameManager.GameInfo.GameStatus != GameStatus.Stopped && gameManager.GameInfo.GameConfig != null)
				{
					if (gameManager.GameInfo.GameConfig.GameType.IsQueueable() || gameManager.GameInfo.IsCustomGame)
					{
						result = ((gameManager.PlayerInfo != null) ? 1 : 0);
						goto IL_0099;
					}
				}
			}
			result = 0;
			goto IL_0099;
			IL_0099:
			return (byte)result != 0;
		}
	}

	public static bool CanJoinGroupChat
	{
		get
		{
			LobbyPlayerGroupInfo groupInfo = ClientGameManager.Get().GroupInfo;
			int result;
			if (groupInfo != null)
			{
				result = (groupInfo.InAGroup ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	public bool IsConnected
	{
		get
		{
			int result;
			if (m_webSocket != null)
			{
				result = (m_webSocket.IsAlive ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	public DiscordChannelInfo ChannelInfo => m_channelInfo;

	public DiscordUserInfo UserInfo => m_userInfo;

	public List<DiscordUserInfo> ChannelUsers => m_discordChannelUsers;

	private int RpcPort
	{
		get
		{
			int result;
			if (s_RpcPortOverride != 0)
			{
				result = s_RpcPortOverride;
			}
			else
			{
				result = RPC_PORT_BEGIN + m_rpcPortOffset;
			}
			return result;
		}
	}

	private string RpcOrigin
	{
		get
		{
			string result;
			if (m_authInfo != null)
			{
				result = m_authInfo.RpcOrigin;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
	}

	private string ClientId => (m_authInfo == null) ? string.Empty : m_authInfo.ClientId;

	public event Action<ErrorEventArgs> OnError
	{
		add
		{
			Action<ErrorEventArgs> action = this.OnError;
			Action<ErrorEventArgs> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnError, (Action<ErrorEventArgs>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<ErrorEventArgs> action = this.OnError;
			Action<ErrorEventArgs> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnError, (Action<ErrorEventArgs>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	public DiscordClientInterface()
	{
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = delegate
			{
			};
		}
		OnConnected = _003C_003Ef__am_0024cache1;
		if (_003C_003Ef__am_0024cache2 == null)
		{
			_003C_003Ef__am_0024cache2 = delegate
			{
			};
		}
		OnDisconnected = _003C_003Ef__am_0024cache2;
		if (_003C_003Ef__am_0024cache3 == null)
		{
			_003C_003Ef__am_0024cache3 = delegate
			{
			};
		}
		OnAuthorized = _003C_003Ef__am_0024cache3;
		if (_003C_003Ef__am_0024cache4 == null)
		{
			_003C_003Ef__am_0024cache4 = delegate
			{
			};
		}
		OnAuthenticated = _003C_003Ef__am_0024cache4;
		if (_003C_003Ef__am_0024cache5 == null)
		{
			_003C_003Ef__am_0024cache5 = delegate
			{
			};
		}
		OnJoined = _003C_003Ef__am_0024cache5;
		OnLeft = delegate
		{
		};
		if (_003C_003Ef__am_0024cache7 == null)
		{
			_003C_003Ef__am_0024cache7 = delegate
			{
			};
		}
		OnUserJoined = _003C_003Ef__am_0024cache7;
		if (_003C_003Ef__am_0024cache8 == null)
		{
			_003C_003Ef__am_0024cache8 = delegate
			{
			};
		}
		OnUserLeft = _003C_003Ef__am_0024cache8;
		if (_003C_003Ef__am_0024cache9 == null)
		{
			_003C_003Ef__am_0024cache9 = delegate
			{
			};
		}
		OnUserSpeakingChanged = _003C_003Ef__am_0024cache9;
		if (_003C_003Ef__am_0024cacheA == null)
		{
			_003C_003Ef__am_0024cacheA = delegate
			{
			};
		}
		this.OnError = _003C_003Ef__am_0024cacheA;
		m_discordChannelUsers = new List<DiscordUserInfo>();
		
	}

	public static DiscordClientInterface Get()
	{
		return s_instance;
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

	private static void _001D(string _001D, params object[] _000E)
	{
		if (!s_debugOutput)
		{
			return;
		}
		while (true)
		{
			string str = string.Format(_001D, _000E);
			Log.Info("Discord | " + str);
			if (SynchronizationContext.Current == null)
			{
				return;
			}
			while (true)
			{
				if (TextConsole.Get() != null)
				{
					while (true)
					{
						TextConsole.Get().Write("Discord | " + str, ConsoleMessageType._001D);
						return;
					}
				}
				return;
			}
		}
	}

	private static void _001D(RpcResponse _001D)
	{
		if (!s_debugOutput)
		{
			return;
		}
		while (true)
		{
			string str = JsonConvert.SerializeObject(_001D, Formatting.Indented);
			Log.Info("Discord | " + str);
			return;
		}
	}

	private void Authorize()
	{
		RpcRequest rpcRequest = new RpcRequest();
		rpcRequest.nonce = Guid.NewGuid().ToString();
		rpcRequest.args = new Dictionary<object, object>();
		rpcRequest.args["client_id"] = m_authInfo.ClientId;
		List<string> list = new List<string>();
		list.Add("rpc");
		list.Add("rpc.api");
		list.Add("guilds.join");
		rpcRequest.args["scopes"] = list;
		rpcRequest.args["rpc_token"] = m_authInfo.RpcToken;
		rpcRequest.cmd = "AUTHORIZE";
		string data = JsonConvert.SerializeObject(rpcRequest);
		m_webSocket.Send(data);
	}

	public void Authenticate(DiscordUserInfo userInfo)
	{
		m_userInfo = userInfo;
		RpcRequest rpcRequest = new RpcRequest();
		rpcRequest.nonce = Guid.NewGuid().ToString();
		rpcRequest.args = new Dictionary<object, object>();
		rpcRequest.args["access_token"] = m_userInfo.AccessToken;
		rpcRequest.cmd = "AUTHENTICATE";
		string data = JsonConvert.SerializeObject(rpcRequest);
		m_webSocket.Send(data);
	}

	private void Invoke(string jsonMessage)
	{
		RpcWebSocketEnvelope rpcWebSocketEnvelope = default(RpcWebSocketEnvelope);
		rpcWebSocketEnvelope.JsonMessage = jsonMessage;
		Action<string> messageDelegate = Dispatch;
		Action<string> action = rpcWebSocketEnvelope.InvokerDelegate = delegate(string msg)
		{
			messageDelegate(msg);
		};
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
			object obj;
			if (rpcResponse.data != null)
			{
				obj = JObject.Parse(rpcResponse.data.ToString());
			}
			else
			{
				obj = null;
			}
			JObject jObject = (JObject)obj;
			_001D(rpcResponse);
			if (rpcResponse.cmd == "CONNECTION_OPEN")
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						_001D("Connected to 127.0.0.1:{0}", RpcPort);
						bool obj2 = m_authInfo == null;
						OnConnected(obj2);
						m_retryToConnect = false;
						m_rpcPortOffset = 0;
						return;
					}
					}
				}
			}
			if (rpcResponse.cmd == "CONNECTION_ERROR")
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						_001D("Connection error to 127.0.0.1:{0}", RpcPort);
						return;
					}
				}
			}
			if (rpcResponse.cmd == "CONNECTION_CLOSE")
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (m_retryToConnect)
						{
							m_rpcPortOffset++;
							if (RPC_PORT_BEGIN + m_rpcPortOffset > RPC_PORT_END)
							{
								_001D("Failed to connect to discord");
								Disconnect();
								OnDisconnected();
							}
							else
							{
								m_webSocket.Close();
								m_webSocket = null;
								TryConnect();
							}
						}
						else
						{
							_001D("Disconnected from discord");
							if (m_authInfo != null)
							{
								OnLeft();
							}
							Disconnect();
							OnDisconnected();
						}
						return;
					}
				}
			}
			if (rpcResponse.cmd == "DISPATCH")
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (rpcResponse.evt == "READY")
						{
							if (m_authInfo != null)
							{
								if (m_userInfo == null)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											Authorize();
											return;
										}
									}
								}
							}
						}
						if (rpcResponse.evt == "CAPTURE_SHORTCUT_CHANGE")
						{
							if (jObject != null)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
									{
										JToken jToken = jObject.SelectToken("shortcut").ElementAt(0);
										int num = Convert.ToInt32(jToken.SelectToken("type").ToString());
										int num2 = Convert.ToInt32(jToken.SelectToken("code").ToString());
										string text = jToken.SelectToken("name").ToString();
										if (ClientGameManager.Get() != null)
										{
											ClientGameManager.Get().SetPushToTalkKey(num, num2, text);
										}
										if (m_pushToTalkScanCallback != null)
										{
											m_pushToTalkScanCallback(num, num2, text);
										}
										ScanPushToTalkKey(false, null);
										RefreshSettings();
										return;
									}
									}
								}
							}
						}
						if (rpcResponse.evt == "VOICE_STATE_CREATE")
						{
							if (jObject != null)
							{
								bool flag = false;
								ulong num3 = ulong.Parse(jObject.SelectToken("user.id").ToString());
								int num4 = 0;
								while (true)
								{
									if (num4 >= m_discordChannelUsers.Count)
									{
										break;
									}
									if (m_discordChannelUsers[num4].UserId == num3)
									{
										flag = true;
										break;
									}
									num4++;
								}
								if (!flag)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
										{
											DiscordUserInfo discordUserInfo = new DiscordUserInfo();
											discordUserInfo.UserId = num3;
											discordUserInfo.UserName = jObject.SelectToken("user.username").ToString();
											discordUserInfo.Discriminator = jObject.SelectToken("user.discriminator").ToString();
											m_discordChannelUsers.Add(discordUserInfo);
											OnUserJoined(discordUserInfo);
											return;
										}
										}
									}
								}
								return;
							}
						}
						if (rpcResponse.evt == "VOICE_STATE_DELETE")
						{
							if (jObject != null)
							{
								ulong num5 = ulong.Parse(jObject.SelectToken("user.id").ToString());
								for (int i = 0; i < m_discordChannelUsers.Count; i++)
								{
									if (m_discordChannelUsers[i].UserId == num5)
									{
										while (true)
										{
											switch (3)
											{
											case 0:
												break;
											default:
											{
												DiscordUserInfo obj3 = m_discordChannelUsers[i];
												m_discordChannelUsers.RemoveAt(i);
												OnUserLeft(obj3);
												return;
											}
											}
										}
									}
								}
								while (true)
								{
									switch (3)
									{
									default:
										return;
									case 0:
										break;
									}
								}
							}
						}
						if (!(rpcResponse.evt == "SPEAKING_START"))
						{
							if (!(rpcResponse.evt == "SPEAKING_STOP"))
							{
								return;
							}
						}
						if (jObject != null)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
								{
									ulong num6 = ulong.Parse(jObject.SelectToken("user_id").ToString());
									int num7 = 0;
									while (true)
									{
										if (num7 >= m_discordChannelUsers.Count)
										{
											return;
										}
										if (m_discordChannelUsers[num7].UserId == num6)
										{
											break;
										}
										num7++;
									}
									while (true)
									{
										switch (5)
										{
										case 0:
											break;
										default:
											m_discordChannelUsers[num7].IsSpeaking = (rpcResponse.evt == "SPEAKING_START");
											OnUserSpeakingChanged(m_discordChannelUsers[num7]);
											return;
										}
									}
								}
								}
							}
						}
						return;
					}
				}
			}
			if (rpcResponse.cmd == "AUTHORIZE")
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						string obj4 = jObject.SelectToken("code").ToString();
						OnAuthorized(obj4);
						return;
					}
					}
				}
			}
			if (rpcResponse.cmd == "AUTHENTICATE")
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (rpcResponse.evt.IsNullOrEmpty())
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
								{
									JToken jToken2 = jObject.SelectToken("user.id");
									m_userInfo.UserId = ulong.Parse(jToken2.ToString());
									OnAuthenticated(m_userInfo);
									return;
								}
								}
							}
						}
						_001D("Failed to authenticate RPC connection {0}", jObject);
						return;
					}
				}
			}
			if (rpcResponse.cmd == "TRY_SELECT_VOICE_CHANNEL")
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						JToken jToken3 = jObject.SelectToken("code");
						int num8 = int.Parse(jToken3.ToString());
						bool force = num8 == 5003;
						TrySelectVoiceChannel(force);
						return;
					}
					}
				}
			}
			if (rpcResponse.cmd == "SELECT_VOICE_CHANNEL")
			{
				if (rpcResponse.evt.IsNullOrEmpty())
				{
					_001D("Connected to voice channel {0}", m_channelInfo.VoiceChannelId);
					OnJoined();
				}
				else
				{
					_001D("Failed to connect to voice channel {0} {1}. Retrying", m_channelInfo.VoiceChannelId, jObject);
					RetrySelectVoiceChannel(jObject);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("DISCORD | exception {0}", ex);
		}
	}

	private void HandleOnOpen(object sender, EventArgs e)
	{
		RpcResponse rpcResponse = new RpcResponse();
		rpcResponse.cmd = "CONNECTION_OPEN";
		RpcResponse value = rpcResponse;
		string jsonMessage = JsonConvert.SerializeObject(value);
		Invoke(jsonMessage);
	}

	private void HandleOnMessage(object sender, MessageEventArgs e)
	{
		string data = e.Data;
		Invoke(data);
	}

	private void HandleOnError(object sender, ErrorEventArgs e)
	{
		RpcResponse rpcResponse = new RpcResponse();
		rpcResponse.cmd = "CONNECTION_ERROR";
		rpcResponse.evt = e.Exception.Message;
		RpcResponse value = rpcResponse;
		string jsonMessage = JsonConvert.SerializeObject(value);
		Invoke(jsonMessage);
	}

	private void HandleOnClose(object sender, CloseEventArgs e)
	{
		RpcResponse rpcResponse = new RpcResponse();
		rpcResponse.cmd = "CONNECTION_CLOSE";
		rpcResponse.evt = e.Reason;
		RpcResponse value = rpcResponse;
		string jsonMessage = JsonConvert.SerializeObject(value);
		Invoke(jsonMessage);
	}

	public void Connect(DiscordAuthInfo authInfo, int portOffset = 0)
	{
		m_authInfo = authInfo;
		m_rpcPortOffset = portOffset;
		if (IsSdkEnabled)
		{
			if (!s_sdkInitialized)
			{
				InitializeSdk();
				return;
			}
		}
		TryConnect();
	}

	private void InitializeSdk()
	{
		string name = "Hydrogen.DiscordSdk";
		bool createdNew;
		Mutex mutex = new Mutex(true, name, out createdNew);
		if (createdNew)
		{
			if (mutex != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						string clientId = m_authInfo.ClientId;
						string resourcePath = Application.dataPath + "/../";
						GameBridge.SetReadyCallback(HandleSdkReadyCallback, UIntPtr.Zero);
						GameBridge.SetUpdatingCallback(HandleSdkUpdatingCallback, UIntPtr.Zero);
						GameBridge.SetErrorCallback(HandleSdkErrorCallback, UIntPtr.Zero);
						if (_003C_003Ef__am_0024cache0 == null)
						{
							_003C_003Ef__am_0024cache0 = delegate(uint type, string message, UIntPtr context)
							{
								_001D("[OUTPUT] {0}", message);
							};
						}
						GameBridge.CaptureOutput(_003C_003Ef__am_0024cache0, UIntPtr.Zero);
						GameBridge.Initialize(clientId, resourcePath);
						s_sdkInitialized = true;
						_001D("initialized Sdk");
						return;
					}
					}
				}
			}
		}
		_001D("failed to initialize Sdk. Discord does not support launching multiple Sdk processes on the same computer at one time.");
	}

	public static void Shutdown()
	{
		if (s_sdkInitialized)
		{
			GameBridge.Shutdown();
			s_sdkInitialized = false;
			_001D("shut down Sdk");
		}
	}

	private void HandleSdkReadyCallback(ushort port, UIntPtr context)
	{
		_001D("SdkReadyCallback {0}", port);
		s_RpcPortOverride = port;
		TryConnect();
	}

	private void HandleSdkUpdatingCallback(uint progress, UIntPtr context)
	{
		_001D("SdkUpdatingCallback {0}", progress);
	}

	private void HandleSdkErrorCallback(uint code, [MarshalAs(UnmanagedType.LPStr)] string message, UIntPtr context)
	{
		_001D("SdkErrorCallback {0} {1}", code, message);
	}

	private void TryConnect()
	{
		if (m_webSocket != null)
		{
			if (m_webSocket.IsAlive)
			{
				_001D("Already connected to discord");
				return;
			}
		}
		m_rpcUrl = $"ws://127.0.0.1:{RpcPort}/?v=1&client_id={ClientId}&encoding=json";
		_001D("Connecting to 127.0.0.1:{0}", RpcPort);
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
		if (m_webSocket == null || !m_webSocket.IsAlive)
		{
			return;
		}
		while (true)
		{
			if (m_channelInfo != null)
			{
				RpcRequest rpcRequest = new RpcRequest();
				rpcRequest.nonce = Guid.NewGuid().ToString();
				rpcRequest.args = new Dictionary<object, object>();
				rpcRequest.args["channel_id"] = m_channelInfo.VoiceChannelId.ToString();
				rpcRequest.args["timeout"] = RPC_COMMAND_TIMEOUT_SEC;
				if (force)
				{
					rpcRequest.args["force"] = force;
				}
				rpcRequest.cmd = "SELECT_VOICE_CHANNEL";
				string data = JsonConvert.SerializeObject(rpcRequest);
				m_webSocket.Send(data);
			}
			return;
		}
	}

	private void RetrySelectVoiceChannel(object responseData = null)
	{
		Action action = delegate
		{
			RpcResponse value = new RpcResponse
			{
				cmd = "TRY_SELECT_VOICE_CHANNEL",
				data = responseData
			};
			string jsonMessage = JsonConvert.SerializeObject(value);
			Invoke(jsonMessage);
		};
		m_rpcScheduler.AddTask(action, (int)TimeSpan.FromSeconds(2.0).TotalMilliseconds);
	}

	public void RefreshSettings()
	{
		if (m_webSocket == null)
		{
			return;
		}
		while (true)
		{
			if (!m_webSocket.IsAlive)
			{
				return;
			}
			while (true)
			{
				if (m_channelInfo == null)
				{
					while (true)
					{
						switch (4)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				Options_UI options_UI = Options_UI.Get();
				if (options_UI != null)
				{
					return;
				}
				RpcRequest rpcRequest = new RpcRequest();
				rpcRequest.nonce = Guid.NewGuid().ToString();
				rpcRequest.args = new Dictionary<object, object>();
				rpcRequest.args["input"] = new
				{
					volume = options_UI.GetVoiceVolume()
				};
				rpcRequest.args["output"] = new
				{
					volume = options_UI.GetMicVolume()
				};
				rpcRequest.args["mute"] = options_UI.GetVoiceMute();
				rpcRequest.cmd = "SET_VOICE_SETTINGS";
				Dictionary<object, object> dictionary = new Dictionary<object, object>();
				rpcRequest.args["mode"] = dictionary;
				object value;
				if (options_UI.GetVoicePushToTalk())
				{
					value = "PUSH_TO_TALK";
				}
				else
				{
					value = "VOICE_ACTIVITY";
				}
				dictionary["type"] = value;
				if (ClientGameManager.Get() != null)
				{
					if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
					{
						AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
						dictionary["shortcut"] = new object[1]
						{
							new
							{
								type = accountComponent.PushToTalkKeyType,
								code = accountComponent.PushToTalkKeyCode,
								name = accountComponent.PushToTalkKeyName
							}
						};
					}
				}
				string data = JsonConvert.SerializeObject(rpcRequest);
				m_webSocket.Send(data);
				return;
			}
		}
	}

	public bool ScanPushToTalkKey(bool start, Action<int, int, string> callback)
	{
		if (m_webSocket != null)
		{
			if (m_webSocket.IsAlive)
			{
				if (m_channelInfo != null)
				{
					m_pushToTalkScanCallback = callback;
					RpcRequest rpcRequest = new RpcRequest();
					rpcRequest.nonce = Guid.NewGuid().ToString();
					rpcRequest.args = new Dictionary<object, object>();
					Dictionary<object, object> args = rpcRequest.args;
					object value;
					if (start)
					{
						value = "START";
					}
					else
					{
						value = "STOP";
					}
					args["action"] = value;
					rpcRequest.cmd = "CAPTURE_SHORTCUT";
					string data = JsonConvert.SerializeObject(rpcRequest);
					m_webSocket.Send(data);
					return true;
				}
			}
		}
		return false;
	}
}
