using GraphQL;
using GraphQL.Types;
using SOP.API.GraphQL.GraphTypes;
using SOP.Data.Repositories;
using SOP.ModelsDto.Dto;

namespace SOP.API.GraphQL.Mutations
{
    public class OwnerMutation : ObjectGraphType
    {
        private readonly IOwnerRepository _repository;

        public OwnerMutation(IOwnerRepository repository)
        {
            this._repository = repository;

            Field<OwnerGraphType>(
                "createOwner",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "surname" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "birthday" },
                    new QueryArgument<StringGraphType> { Name = "vehicleRegistration"}
                ),
                resolve: context =>
                {
                    var email = context.GetArgument<string>("email");
                    var name = context.GetArgument<string>("name");
                    var surname = context.GetArgument<string>("surname");
                    var birthday = context.GetArgument<string>("birthday");
                    var vehicleRegistration = context.GetArgument<string>("vehicleRegistration");

                    var owner = new OwnerDto
                    {
                        Email = email,
                        Name = name,
                        Surname = surname,
                        Birthday = birthday,
                        VehicleRegistration = vehicleRegistration
                    };

                    var result = _repository.CreateOwner(owner);
                    return result;
                }
            );

            Field<OwnerGraphType>(
                "updateOwner",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "surname" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "birthday" },
                    new QueryArgument<StringGraphType> { Name = "vehicleRegistration" }
                ),
                resolve: context =>
                {
                    var email = context.GetArgument<string>("email");
                    var name = context.GetArgument<string>("name");
                    var surname = context.GetArgument<string>("surname");
                    var birthday = context.GetArgument<string>("birthday");
                    var vehicleRegistration = context.GetArgument<string>("vehicleRegistration");

                    var owner = new OwnerDto
                    {
                        Email = email,
                        Name = name,
                        Surname = surname,
                        Birthday = birthday,
                        VehicleRegistration = vehicleRegistration
                    };

                    var result = _repository.UpdateOwner(owner);
                    return result;
                }
            );

            Field<StringGraphType>(
                "deleteOwner",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" }
                ),
                resolve: context =>
                {
                    var email = context.GetArgument<string>("email");

                    _repository.DeleteOwner(email);
                    return email;
                }
            );
        }
    }
}
