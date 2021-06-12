public interface ICondition
{
    string Name { get; } // For indication
    bool IsValid(IBlackBoard blackBoard); // Function for checking if conditions are met
}