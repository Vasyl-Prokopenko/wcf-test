public static class ServiceConfig
{
	public const int MaxNumberOfBytes = 128 * 1024 * 1024;

	public static Uri Uri { get; } = new(Environment.GetEnvironmentVariable("SVC_URL") ?? "net.tcp://localhost:5000/Service.svc");

	public static int SizeInBytes { get; } = int.TryParse(Environment.GetEnvironmentVariable("MSG_SIZE_BYTE"), out var size)
		? size
		: 2 * 1024 * 1024;

	public static int DelaySec { get; } =
		int.TryParse(Environment.GetEnvironmentVariable("DELAY_SEC"), out var del) ? del : 10;
}