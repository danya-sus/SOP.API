using GraphQL.Types;
using SOP.API.GraphQL.Mutations;
using SOP.API.GraphQL.Queries;
using SOP.Data.Repositories;

namespace SOP.API.GraphQL.Schemas
{
    public class OwnerSchema : Schema
    {
        public OwnerSchema(IOwnerRepository repository)
        {
            Query = new OwnerQuery(repository);
            Mutation = new OwnerMutation(repository);
        }
    }
}
