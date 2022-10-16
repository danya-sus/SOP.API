using Microsoft.Extensions.Logging;
using SOP.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SOP.Data.Repositories
{
    public class ModelRepository : IModelRepository
    {
        private readonly SOPContext _context;
        private readonly ILogger<ModelRepository> _logger;

        public ModelRepository(SOPContext context, ILogger<ModelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Model> ListModels()
        {
            return _context.Models.ToList();
        }

        public Model FindModel(string modelCode)
        {
            return _context.Models.FirstOrDefault(m => m.Code == modelCode);
        }
    }
}
