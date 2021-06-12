public abstract class BaseBlackBoard : IBlackBoard
{
    public bool IsInitialized { get; protected set; }
    public IFactory Factory { get; set; }   
}
