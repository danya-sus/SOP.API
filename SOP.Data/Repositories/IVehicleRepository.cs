using SOP.Models.Entities;
using SOP.ModelsDto.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOP.Data.Repositories
{
    public interface IVehicleRepository
    {
		public int CountVehicles();
		public IEnumerable<Vehicle> ListVehicles(int index, int count);
		public Vehicle FindVehicle(string registration);
		public Vehicle CreateVehicle(VehicleDto vehicleDto);
		public Vehicle UpdateVehicle(VehicleDto vehicleDto);
		public void DeleteVehicle(string registration);
	}
}
