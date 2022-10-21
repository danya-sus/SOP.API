using SOP.Models.Entities;
using SOP.ModelsDto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP.Data.Repositories
{
    public interface IOwnerRepository
    {
        public int CountOwners();
        public IEnumerable<Owner> ListOwners(int index, int count);
        public Owner FindOwner(string id);
        public Owner CreateOwner(OwnerDto ownerDto);
        public Owner UpdateOwner(OwnerDto ownerDto);
        public void DeleteOwner(string id);
    }
}
