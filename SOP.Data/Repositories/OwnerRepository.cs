using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SOP.Models.Entities;
using SOP.ModelsDto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOP.Data.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly SOPContext _context;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ILogger<OwnerRepository> _logger;

        public OwnerRepository(SOPContext context, IVehicleRepository vehicleRepository, ILogger<OwnerRepository> logger)
        {
            _context = context;
            _vehicleRepository = vehicleRepository;
            _logger = logger;
        }

        public int CountOwners()
        {
            return _context.Owners.Count();
        }

        public IEnumerable<Owner> ListOwners(int index, int count)
        {
            return _context.Owners.Skip(index).Take(count).Include(v => v.Vehicle).ThenInclude(m => m.VehicleModel).ThenInclude(m => m.Manufacturer).ToList();
        }

        public Owner FindOwner(string id)
        {
            return _context.Owners.Include(v => v.Vehicle).ThenInclude(m => m.VehicleModel).ThenInclude(m => m.Manufacturer).FirstOrDefault(o => o.Email == id);
        }

        public void CreateOwner(OwnerDto ownerDto)
        {
            var vehicle = _vehicleRepository.FindVehicle(ownerDto.VehicleRegistration);
            var owner = new Owner
            {
                Email = ownerDto.Email,
                Name = ownerDto.Name,
                Surname = ownerDto.Surname,
                Birthday = DateOnly.Parse(ownerDto.Birthday),
                Vehicle = vehicle
            };

            _context.Owners.Add(owner);
            _context.SaveChanges();
        }

        public void UpdateOwner(OwnerDto ownerDto)
        {
            var str = $"UPDATE owners " +
                      $"SET name = \'{ownerDto.Name}\', " +
                      $"surname = \'{ownerDto.Surname}\', " +
                      $"birthday = \'{ownerDto.Birthday}\'," +
                      $"vehicle_registration = \'{ownerDto.VehicleRegistration}\' " +
                      $"WHERE email = \'{ownerDto.Email}\'";

            var result = _context.Database.ExecuteSqlRaw(str);
            if (result > 0) return;

            CreateOwner(ownerDto);
        }

        public void DeleteOwner(string id)
        {
            _context.Owners.Remove(FindOwner(id));
            _context.SaveChanges();
        }
    }
}
