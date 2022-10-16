using GraphQL;
using GraphQL.Types;
using SOP.API.GraphQL.GraphTypes;
using SOP.Data.Repositories;
using SOP.Models.Entities;

namespace SOP.API.GraphQL.Queries
{
    public class OwnerQuery : ObjectGraphType
    {
        private readonly IOwnerRepository _repository;

        public OwnerQuery(IOwnerRepository repository)
        {
            _repository = repository;

            Field<ListGraphType<OwnerGraphType>>("Owners", "Query to retrieve all Owners",
                resolve: GetAllOwners);

            Field<OwnerGraphType>("Owner", "Query to retrieve specific Vehicle",
                new QueryArguments(MakeNonNullStringArgument("email", "The email of the Owner")),
                resolve: GetOwnerById);
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description)
        {
            return new QueryArgument<NonNullGraphType<StringGraphType>>
            {
                Name = name,
                Description = description
            };
        }

        private IEnumerable<Owner> GetAllOwners(IResolveFieldContext<object> context) => _repository.ListOwners(0, 25);

        private Owner GetOwnerById(IResolveFieldContext<object> context)
        {
            var email = context.GetArgument<string>("email");
            return _repository.FindOwner(email);
        }
    }
}
