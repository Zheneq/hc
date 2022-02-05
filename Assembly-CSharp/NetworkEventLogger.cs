// ROGUES
// SERVER
using System;

// server-only, missing in reactor
#if SERVER
public class NetworkEventLogger : EventLogger
{
	public NetworkDataWriter NetworkDataWriter { get; private set; }

	public string RemoteAddress
	{
		get
		{
			return NetworkDataWriter.RemoteAddress;
		}
		set
		{
			NetworkDataWriter.RemoteAddress = value;
		}
	}

	public int RemotePort
	{
		get
		{
			return NetworkDataWriter.DefaultPort;
		}
		set
		{
			NetworkDataWriter.DefaultPort = value;
		}
	}

	public NetworkEventLogger()
	{
		NetworkDataWriter = new NetworkDataWriter();
		NetworkDataWriter.MaxQueuedMessages = 10000;
		NetworkDataWriter.DefaultPort = 2910;
		NetworkDataWriter.Name = "event log listener";
	}

	public override void Write(EventLogMessage message)
	{
		NetworkDataWriter.Write(string.Format("{0}\t{1}\r\n", message.MetadataString, message.DataString));
	}

	public override void Shutdown(TimeSpan timeout = default(TimeSpan))
	{
		NetworkDataWriter.Shutdown(timeout);
	}
}
#endif
