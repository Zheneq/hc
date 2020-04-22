using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ClientStatusReport : WebSocketMessage
	{
		public enum ClientStatusReportType
		{
			_001D,
			_000E,
			_0012,
			_0015,
			_0016
		}

		public ClientStatusReportType Status;

		public string StatusDetails;

		public string DeviceIdentifier;

		public string UserMessage;

		public string FileDateTime;
	}
}
