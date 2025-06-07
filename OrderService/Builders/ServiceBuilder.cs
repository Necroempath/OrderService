using OrderService.Models.Services;

namespace OrderService.Builders;

public class ServiceBuilder : IBuilder
{
    public enum BuildingReport
    {
        MissingPrice,
        InvalidPrice,
        ValidData
    }

    private bool _isValid;
    private string _description = "";
    private float? _price;
    
    public IReadOnlyList<string> Prompts => 
    [
        "Enter service description: ",
        "Enter service price: "
    ];

    public void SubmitInputs(IReadOnlyList<string> inputs)
    {
        _isValid = false;
        
        _description = inputs[0];

        if (float.TryParse(inputs[1], out var price))
        {
            _price = price;
        }
        else
        {
            _price = null;
        }
    }
    
    public BuildingReport Validate(out string message)
    {
        switch (_price)
        {
            case null:
                message = "'Service price' data is missing";
                return BuildingReport.MissingPrice;
            case < 0:
                message = "Price value cannot be negative number";
                return BuildingReport.InvalidPrice;
            default:
                message = "Service data is valid";
                break;
        }
        
        return BuildingReport.ValidData;
    }

    public Service? Build() => _isValid ? new Service { Description = _description, Price = (float)_price! } : null;
}