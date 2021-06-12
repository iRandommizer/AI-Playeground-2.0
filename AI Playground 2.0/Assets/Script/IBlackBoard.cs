public interface IBlackBoard 
{
    bool IsInitialized { get; }

    IFactory Factory { get; set; }
}
