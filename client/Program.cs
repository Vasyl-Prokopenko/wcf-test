using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using ServiceReference;

namespace client;

public static class Program
{
	private static Binding Binding { get; } = new NetTcpBinding
	{
		MaxReceivedMessageSize = ServiceConfig.MaxNumberOfBytes,
		MaxBufferSize = ServiceConfig.MaxNumberOfBytes,
		TransferMode = TransferMode.Buffered,
		MaxBufferPoolSize = 20_000_000,
		Security = new NetTcpSecurity
		{
			Mode = SecurityMode.None,
			Transport = new TcpTransportSecurity
			{
				ClientCredentialType = TcpClientCredentialType.None
			},
			Message = new MessageSecurityOverTcp
			{
				ClientCredentialType = MessageCredentialType.None
			}
		},
		ReaderQuotas = XmlDictionaryReaderQuotas.Max,
		ReceiveTimeout = TimeSpan.FromMinutes(10),
		SendTimeout = TimeSpan.FromMinutes(10)
	};

	public static async Task Main()
	{
		using var cancel = new CancellationTokenSource(TimeSpan.FromMinutes(60));
		var delay = TimeSpan.FromSeconds(ServiceConfig.DelaySec);
		while (!cancel.Token.IsCancellationRequested)
		{
			var stopwatch = Stopwatch.StartNew();
			try
			{
				await using var client = new ServiceClient(
					Binding,
					new EndpointAddress(ServiceConfig.Uri));
				Console.WriteLine(
					"GetData request to '{0}' for {1}KB started...",
					ServiceConfig.Uri,
					ServiceConfig.SizeInBytes / 1024);
				var result = await client.GetDataAsync(ServiceConfig.SizeInBytes);
				stopwatch.Stop();
				Console.WriteLine(
					"Response (size: {0}KB) received in {1}ms",
					result.Length / 1024,
					stopwatch.ElapsedMilliseconds);
			}
			catch (Exception e)
			{
				stopwatch.Stop();
				Console.WriteLine("Error after {0}ms:\n{1}", stopwatch.ElapsedMilliseconds, e);
			}
			
			await Task.Delay(delay, cancel.Token);
		}
	}
}