using GraphQL.Types;
using SOP.Models.Entities;

namespace SOP.API.GraphQL.GraphTypes
{
    public sealed class OwnerGraphType : ObjectGraphType<Owner>
    {
        public OwnerGraphType()
        {
            Name = "owner";
            Field(c => c.Email);
            Field(c => c.Name);
            Field(c => c.Surname);
            Field(c => c.Birthday);
            Field(c => c.Vehicle, type: typeof(VehicleGraphType));
        }
    }
}
