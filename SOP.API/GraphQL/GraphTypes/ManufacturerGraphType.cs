using GraphQL.Types;
using SOP.Models.Entities;

namespace SOP.API.GraphQL.GraphTypes
{
    public sealed class ManufacturerGraphType : ObjectGraphType<Manufacturer>
    {
        public ManufacturerGraphType()
        {
            Name = "manufacturer";
            Field(c => c.Name).Description("The name of the manufacturer, e.g. Tesla, Volkswagen, Ford");
        }
    }
}
