public interface ISensor
{
    float TickRate { get; }
    float NextTickTime { get; set; }
    void Tick(AIBlackBoard blackBoard);
    void DrawGizmos(AIBlackBoard blackBoard);
}
