using GraphQL.Types;
using SOP.API.GraphQL.Mutations;
using SOP.API.GraphQL.Queries;
using SOP.Data.Repositories;

namespace SOP.API.GraphQL.Schemas
{
    public class AutoSchema : Schema
    {
        public AutoSchema(IVehicleRepository repository)
        {
            Query = new VehicleQuery(repository);
            Mutation = new VehicleMutation(repository);
        }
    }
}
