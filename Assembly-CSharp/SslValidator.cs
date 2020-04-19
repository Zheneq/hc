using System;
using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public static class SslValidator
{
	private static SslPolicyErrors m_acceptedSslPolicyErrors = SslPolicyErrors.None;

	[CompilerGenerated]
	private static RemoteCertificateValidationCallback <>f__mg$cache0;

	public static SslPolicyErrors AcceptableSslPolicyErrors
	{
		get
		{
			return SslValidator.m_acceptedSslPolicyErrors;
		}
		set
		{
			SslValidator.m_acceptedSslPolicyErrors = value;
			if (SslValidator.<>f__mg$cache0 == null)
			{
				SslValidator.<>f__mg$cache0 = new RemoteCertificateValidationCallback(SslValidator.SslCallback);
			}
			ServicePointManager.ServerCertificateValidationCallback = SslValidator.<>f__mg$cache0;
		}
	}

	private static bool SslCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		if ((sslPolicyErrors & SslValidator.m_acceptedSslPolicyErrors) == sslPolicyErrors)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SslValidator.SslCallback(object, X509Certificate, X509Chain, SslPolicyErrors)).MethodHandle;
			}
			return true;
		}
		Log.Error("Certificate {0} has errors: {1}", new object[]
		{
			certificate.Subject,
			sslPolicyErrors.ToString()
		});
		return false;
	}
}
