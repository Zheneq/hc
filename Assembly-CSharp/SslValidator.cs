using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public static class SslValidator
{
	private static SslPolicyErrors m_acceptedSslPolicyErrors;

	[CompilerGenerated]
	private static RemoteCertificateValidationCallback _003C_003Ef__mg_0024cache0;

	public static SslPolicyErrors AcceptableSslPolicyErrors
	{
		get
		{
			return m_acceptedSslPolicyErrors;
		}
		set
		{
			m_acceptedSslPolicyErrors = value;
			ServicePointManager.ServerCertificateValidationCallback = SslCallback;
		}
	}

	static SslValidator()
	{
		m_acceptedSslPolicyErrors = SslPolicyErrors.None;
	}

	private static bool SslCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		if ((sslPolicyErrors & m_acceptedSslPolicyErrors) == sslPolicyErrors)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		Log.Error("Certificate {0} has errors: {1}", certificate.Subject, sslPolicyErrors.ToString());
		return false;
	}
}
