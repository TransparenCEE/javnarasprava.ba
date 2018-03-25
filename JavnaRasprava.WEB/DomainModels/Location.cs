
namespace JavnaRasprava.WEB.DomainModels
{
	/// <summary>
	/// Generic location 
	/// </summary>
	public class Location
	{
		/// <summary>
		/// Gets or sets unique identifier for location
		/// </summary>
		public int LocationID { get; set; }

		/// <summary>
		/// Gets or sets municipality name of the location
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets coordination latitude 
		/// </summary>
		public float Latitude { get; set; }

		/// <summary>
		/// Gets or sets coordination longitude
		/// </summary>
		public float Longitude { get; set; }

		/// <summary>
		/// Gets or sets region location belongs to
		/// </summary>
		public virtual Region Region { get; set; }
		public int RegionID { get; set; }
	}
}