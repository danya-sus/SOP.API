using GraphQL;
using GraphQL.Types;
using SOP.API.GraphQL.GraphTypes;
using SOP.Data.Repositories;
using SOP.Models.Entities;
using System.Collections.Generic;

namespace SOP.API.GraphQL.Queries
{
    public class VehicleQuery : ObjectGraphType
    {
        private readonly IVehicleRepository _repository;

        public VehicleQuery(IVehicleRepository repository)
        {
            _repository = repository;

            Field<ListGraphType<VehicleGraphType>>("Vehicles", "Query to retrieve all Vehicles", 
                resolve: GetAllVehicles);

            Field<VehicleGraphType>("Vehicle", "Query to retrieve specific Vehicle",
                new QueryArguments(MakeNonNullStringArgument("registration", "The registration (licence plate) of the Vehicle")),
                resolve: GetVehicleById);
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description)
        {
            return new QueryArgument<NonNullGraphType<StringGraphType>>
            {
                Name = name,
                Description = description
            };
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> context) => _repository.ListVehicles(0, 25);

        private Vehicle GetVehicleById(IResolveFieldContext<object> context)
        {
            var registration = context.GetArgument<string>("registration");
            return _repository.FindVehicle(registration);
        }
    }
}
