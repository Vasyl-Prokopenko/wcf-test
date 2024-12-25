using System.Diagnostics;

namespace srv;

public class Service : IService
{
	private static readonly Random Random = new();

	public string GetData(int value)
	{
		Console.WriteLine("GetData request received for {0} bytes", value);
		var stopwatch = Stopwatch.StartNew();
		try
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, value).Select(s => s[Random.Next(s.Length)]).ToArray());
		}
		finally
		{
			stopwatch.Stop();
			Console.WriteLine("GetData({0}) took {1}ms", value, stopwatch.ElapsedMilliseconds);
		}
	}
}