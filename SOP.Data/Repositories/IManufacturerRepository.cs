using SOP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOP.Data.Repositories
{
    public interface IManufacturerRepository
    {
		public int CountManufacturers();
		public IEnumerable<Manufacturer> ListManufacturers();

		public Manufacturer FindManufacturer(string modelCode);

		public void CreateManufacturer(Manufacturer manufacturer);
		public void UpdateManufacturer(Manufacturer manufacturer);
		public void DeleteManufacturer(Manufacturer manufacturer);
	}
}
