using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;
using Debug = UnityEngine.Debug;

namespace ArtemisServer.BridgeServer
{
    // based on InsightClient
    public abstract class ArtemisClientBase : MonoBehaviour
    {
        public enum CallbackStatus
        {
            Ok,
            Error,
            Timeout
        }

        public struct CallbackData
        {
            public CallbackHandler callback;
            public float timeout;
        }

        public delegate void NetworkMessageDelegate(AllianceMessageBase netMsg);

        public delegate void CallbackHandler(CallbackStatus status, AllianceMessageBase netMsg);

        private WebSocketSharp.WebSocket ws;
        public string networkAddress = "localhost";
        protected Stopwatch m_overallConnectionTimer;
        protected Stopwatch m_reconnectDelayTimer;
        protected Dictionary<short, NetworkMessageDelegate> messageHandlers = new Dictionary<short, NetworkMessageDelegate>();

        protected int callbackIdIndex;
        protected Dictionary<int, CallbackData> callbacks = new Dictionary<int, CallbackData>();
        public const float callbackTimeout = 30f;

        public bool isConnected => ws != null && ws.ReadyState == WebSocketState.Open;

        protected abstract List<Type> GetMessageTypes();
        protected abstract void OnConnecting();
        protected abstract void OnConnected();
        protected abstract void OnDisconnected();

        protected virtual void OnError(ErrorEventArgs e)
        {
        }

        public void Connect()
        {
            if (isConnected)
            {
                return;
            }

            OnConnecting();

            // custom
            ws = new WebSocketSharp.WebSocket(networkAddress);
            ws.OnMessage += Ws_OnMessage;
            ws.OnError += Ws_OnError;
            ws.OnOpen += Ws_OnOpen;
            ws.OnClose += Ws_OnClose;
            ws.Connect();

            // custom
            m_reconnectDelayTimer.Start();
        }

        public void Disconnect()
        {
            // rogues
            m_overallConnectionTimer.Reset();
            // custom
            if (isConnected && ws != null)
            {
                ws.Close();
                ws = null;
            }
        }

        public void Reconnect()
        {
            m_reconnectDelayTimer.Reset();
            m_reconnectDelayTimer.Start();
            Disconnect();
            Connect();
        }

        public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
        {
            if (messageHandlers.ContainsKey(msgType))
            {
                Debug.Log("NetworkConnection.RegisterHandler replacing " + msgType);
            }

            messageHandlers[msgType] = handler;
        }

        public void UnRegisterHandler(short msgType, NetworkMessageDelegate handler)
        {
            messageHandlers.Remove(msgType);
        }

        public void RegisterMessageDelegate<TMessage>(NetworkMessageDelegate handler) where TMessage : AllianceMessageBase
        {
            short messageType = GetMessageType<TMessage>();
            if (messageType >= 0)
            {
                RegisterHandler(messageType, handler);
                return;
            }

            Log.Error("Message type {0} is not in the MonitorGameServerInsightMessages MessageTypes list and cant be listened for", typeof(TMessage).Name);
        }

        public short GetMessageType<TMessage>() where TMessage : AllianceMessageBase
        {
            short num = (short)GetMessageTypes().IndexOf(typeof(TMessage));
            if (num < 0)
            {
                Log.Error($"Message type {typeof(TMessage).Name} is not in the MonitorGameServerInsightMessages MessageTypes list and doesnt have a type");
            }

            return num;
        }

        public short GetMessageType(AllianceMessageBase msg)
        {
            short num = (short)GetMessageTypes().IndexOf(msg.GetType());
            if (num < 0)
            {
                Log.Error($"Message type {msg.GetType().Name} is not in the MonitorGameServerInsightMessages MessageTypes list and doesnt have a type");
            }

            return num;
        }

        public bool Send(AllianceMessageBase msg, CallbackHandler callback = null)
        {
            short messageType = GetMessageType(msg);
            if (messageType >= 0)
            {
                if (callback != null)
                {
                    Send(messageType, msg, callback);
                }
                else
                {
                    Send(messageType, msg);
                }
                return true;
            }
            return false;
        }

        private bool Send(short msgType, AllianceMessageBase msg, int originalCallbackId = 0)
        {
            if (!isConnected)
            {
                Debug.LogError("[InsightClient] - Client not connected! " + msg.GetType().Name);
                return false;
            }

            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(msgType);
            networkWriter.Write(originalCallbackId);
            msg.Serialize(networkWriter);
            Log.Debug($"Client Msg {msgType}-{msg.GetType().Name}-{originalCallbackId}: {networkWriter.ToArray().ToHexString()}");
            ws.Send(networkWriter.ToArray());
            return true;
        }

        public void Send(short msgType, AllianceMessageBase msg, CallbackHandler callback)
        {
            if (!isConnected)
            {
                Debug.LogError("[InsightClient] - Client not connected! " + msg.GetType().Name);
                return;
            }
            
            int callbackId = 0;
            if (callback != null)
            {
                callbackId = ++callbackIdIndex;
                callbacks.Add(callbackId, new CallbackData
                {
                    callback = callback,
                    timeout = Time.realtimeSinceStartup + 30f
                });
            }
            Send(msgType, msg, callbackId);
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            OnConnected();
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            OnDisconnected();
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            Log.Info("--- Websocket Error ---");
            Log.Info(e.Exception.Source);
            Log.Info(e.Message);
            Log.Info(e.Exception.StackTrace);
            OnError(e);
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            NetworkReader networkReader = new NetworkReader(e.RawData);
            short messageType = networkReader.ReadInt16();
            int callbackId = networkReader.ReadInt32();
            List<Type> messageTypes = GetMessageTypes();
            if (messageType >= messageTypes.Count)
            {
                Log.Error($"Unknown bridge message type {messageType}");
                return;
            }

            Type type = messageTypes[messageType];
            AllianceMessageBase msg = (AllianceMessageBase)Activator.CreateInstance(type);
            msg.Deserialize(networkReader);
            if (callbacks.ContainsKey(callbackId))
            {
                callbacks[callbackId].callback(CallbackStatus.Ok, msg);
                callbacks.Remove(callbackId);
            }
            else if (messageHandlers.TryGetValue(messageType, out NetworkMessageDelegate handler))
            {
                handler(msg);
            }
            else
            {
                Debug.LogError("Unknown message ID " + messageType);
            }
        }
    }
}