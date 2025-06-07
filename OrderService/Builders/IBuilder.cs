namespace OrderService.Builders;

public interface IBuilder
{
    public IReadOnlyList<string> Prompts { get; }

    public void SubmitInputs(IReadOnlyList<string> inputs);
}