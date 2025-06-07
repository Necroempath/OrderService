using OrderService.Models.Orders;
using OrderService.Models.Services;

namespace OrderService.Builders;

public class OrderBuilder : IBuilder
{
    public enum BuildingReport
    {
        NoServices,
        MissingName,
        ValidData
    }
    
    private bool _isValid;

    private string? _name;

    private readonly List<Service> _services = new();


    public IReadOnlyList<string> Prompts => ["Enter order name: "];
    
    public void AddService(Service service) => _services.Add(service);

    public void SubmitInputs(IReadOnlyList<string> inputs)
    {
        _isValid = false;
        _name = inputs[0];
    }
    
    public BuildingReport Validate(out string message)
    {
        if (_services.Count == 0)
        {
            message = "There are no services registered";
            return BuildingReport.NoServices;
        }

        if (string.IsNullOrEmpty(_name))
        {
            message = "Order name is empty";
            return BuildingReport.MissingName;
        }

        _isValid = true;
        message = "Order data is valid";
        return BuildingReport.ValidData;
    }
    
    public Order? Build() => _isValid ? new Order { Name = _name!, Services = _services } : null;
}