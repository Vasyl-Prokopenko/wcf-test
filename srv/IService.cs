namespace srv;

[ServiceContract]
public interface IService
{
	[OperationContract]
	string GetData(int value);
}