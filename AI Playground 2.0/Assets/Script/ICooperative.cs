public interface ICooperative
{
    RequestSystem HerdRequestSystem { get; set; }
    RequestHandler RequestHandler { get; set; }

    HerdAIBlackBoard HerdAIBlackBoard { get; set; }
}
