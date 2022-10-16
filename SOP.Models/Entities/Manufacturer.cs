
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SOP.Models.Entities
{
	public partial class Manufacturer
	{
		public Manufacturer()
		{
			Models = new HashSet<Model>();
		}

        [Key]
		public string Code { get; set; }
		public string Name { get; set; }

		[JsonIgnore]
		public virtual ICollection<Model> Models { get; set; }
	}
}
