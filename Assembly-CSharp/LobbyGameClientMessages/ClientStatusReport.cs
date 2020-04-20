using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ClientStatusReport : WebSocketMessage
	{
		public ClientStatusReport.ClientStatusReportType Status;

		public string StatusDetails;

		public string DeviceIdentifier;

		public string UserMessage;

		public string FileDateTime;

		public enum ClientStatusReportType
		{
			symbol_001D,
			symbol_000E,
			symbol_0012,
			symbol_0015,
			symbol_0016
		}
	}
}
