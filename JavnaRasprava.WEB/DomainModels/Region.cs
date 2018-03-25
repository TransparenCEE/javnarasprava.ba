using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class Region
	{
		/// <summary>
		/// Gets or sets unique identifier for region
		/// </summary>
		public int RegionID { get; set; }

		/// <summary>
		/// Gets or sets municipality name of the location
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets name of entity.
		/// </summary>
		public string Entity { get; set; }

		/// <summary>
		/// Gets or sets coordination latitude 
		/// </summary>
		public float Latitude { get; set; }

		/// <summary>
		/// Gets or sets coordination longitude
		/// </summary>
		public float Longitude { get; set; }

		/// <summary>
		/// Gets or set collection of institutions in location
		/// </summary>
		public virtual ICollection<Location> Locations { get; set; }
	}
}