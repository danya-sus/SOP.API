using SOP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOP.Data.Repositories
{
    public interface IModelRepository
    {
		public IEnumerable<Model> ListModels();

		public Model FindModel(string modelCode);
	}
}
