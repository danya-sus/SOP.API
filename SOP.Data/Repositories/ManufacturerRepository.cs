using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SOP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOP.Data.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly SOPContext _context;
        private readonly ILogger<ManufacturerRepository> _logger;

        public ManufacturerRepository(SOPContext context, ILogger<ManufacturerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public int CountManufacturers()
        {
            return _context.Manufacturers.Count();
        }

        public IEnumerable<Manufacturer> ListManufacturers()
        {
            return _context.Manufacturers.ToList();
        }

        public Manufacturer FindManufacturer(string modelCode)
        {
            return _context.Manufacturers.FirstOrDefault(m => m.Code == modelCode);
        }

        public void CreateManufacturer(Manufacturer manufacturer)
        {
            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();
        }

        public void UpdateManufacturer(Manufacturer manufacturer)
        {
            var str = $"UPDATE \"manufacturers\" " +
                      $"SET \'manufacturer_code\' = {manufacturer.Code} " +
                      $"\'name\' = {manufacturer.Name}";

            try
            {
                var result = _context.Database.ExecuteSqlRaw(str);
                if (result < 0) new Exception();
            }
            catch (Exception)
            {
                throw new DbUpdateException();
            }
        }

        public void DeleteManufacturer(Manufacturer manufacturer)
        {
            _context.Remove(manufacturer);
            _context.SaveChanges();
        }
    }
}
