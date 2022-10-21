using GraphQL;
using GraphQL.Types;
using SOP.API.GraphQL.GraphTypes;
using SOP.Data.Repositories;
using SOP.ModelsDto.Dto;

namespace SOP.API.GraphQL.Mutations;

public class VehicleMutation : ObjectGraphType
{
    private readonly IVehicleRepository _repository;

    public VehicleMutation(IVehicleRepository repository)
    {
        this._repository = repository;

        Field<VehicleGraphType>(
            "createVehicle",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "registration" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "color" },
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "year" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "modelCode" }
            ),
            resolve: context =>
            {
                var registration = context.GetArgument<string>("registration");
                var color = context.GetArgument<string>("color");
                var year = context.GetArgument<int>("year");
                var modelCode = context.GetArgument<string>("modelCode");

                var vehicle = new VehicleDto
                {
                    Registration = registration,
                    Color = color,
                    Year = year,
                    ModelCode = modelCode
                };

                var result = _repository.CreateVehicle(vehicle);
                return result;
            }
        );
    }
}