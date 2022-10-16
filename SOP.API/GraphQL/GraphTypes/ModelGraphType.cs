using GraphQL.Types;
using SOP.Models.Entities;

namespace SOP.API.GraphQL.GraphTypes
{
    public sealed class ModelGraphType : ObjectGraphType<Model>
    {
        public ModelGraphType()
        {
            Name = "model";
            Field(m => m.Name).Description("The name of this model, e.g. Golf, Beetle, 5 Series, Model X");
            Field(m => m.Manufacturer, type: typeof(ManufacturerGraphType)).Description("The make of this model of car");
        }
    }
}
