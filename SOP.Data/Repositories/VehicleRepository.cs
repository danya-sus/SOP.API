using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SOP.Models.Entities;
using SOP.ModelsDto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly SOPContext _context;
        private readonly IModelRepository _modelRepository;
        private readonly ILogger<VehicleRepository> _logger;

        public VehicleRepository(SOPContext context, IModelRepository modelRepository, ILogger<VehicleRepository> logger)
        {
            _context = context;
            _modelRepository = modelRepository;
            _logger = logger;
        }

        public int CountVehicles()
        {
            return _context.Vehicles.Count();
        }

        public IEnumerable<Vehicle> ListVehicles(int index, int count)
        {
            return _context.Vehicles.Skip(index).Take(count).Include(m => m.VehicleModel).ThenInclude(m => m.Manufacturer).ToList();
        }

        public Vehicle FindVehicle(string registration)
        {
            return _context.Vehicles.Include(m => m.VehicleModel).ThenInclude(m => m.Manufacturer).FirstOrDefault(v => v.Registration == registration);
        }

        public Vehicle CreateVehicle(VehicleDto vehicleDto)
        {
            var model = _context.Models.FirstOrDefault(c => c.Code == vehicleDto.ModelCode);
            var vehicle = new Vehicle
            {
                Registration = vehicleDto.Registration,
                Color = vehicleDto.Color,
                Year = vehicleDto.Year,
                VehicleModel = model
            };

            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            return FindVehicle(vehicleDto.Registration);
        }

        public Vehicle UpdateVehicle(VehicleDto vehicleDto)
        {
            var str = $"UPDATE \"vehicles\" " +
                      $"SET \'model_code\' = \'{vehicleDto.ModelCode}\' " +
                      $"\'color\' = \'{vehicleDto.Color}\' " +
                      $"\'year\' = {vehicleDto.Year} " +
                      $"WHERE \'reqistration\' = \'{vehicleDto.Registration}\'";

            var result = _context.Database.ExecuteSqlRaw(str);
            if (result == 0) throw new DbUpdateException();

            return FindVehicle(vehicleDto.Registration);
        }

        public void DeleteVehicle(string registration)
        {
            _context.Vehicles.Remove(FindVehicle(registration));
            _context.SaveChanges();
        }
    }
}
