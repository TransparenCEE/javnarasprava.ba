using JavnaRasprava.WEB.DomainModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext.Seed
{

	public class Locations
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		public static void Seed( DomainModels.ApplicationDbContext context )
		{
			context.Regions.AddOrUpdate( x => new { x.Name, x.Entity },
				 new Region
				 {
					 Name = "Banja Luka",
					 Entity = "RS",
					 Latitude = 44.769160F,
					 Longitude = 17.182620F,
					 Locations = new List<Location> 
					 { 					 
						 new Location{ Name= "Banja Luka",  Latitude=44.769162F, Longitude = 17.182617F },
						 new Location{ Name= "Kozarska Dubica",  Latitude=45.184094F, Longitude = 16.806765F },
						 new Location{ Name= "Gradiška",  Latitude=45.142276F, Longitude = 17.252827F },
						 new Location{ Name= "Kostajnica",  Latitude=45.218657F, Longitude = 16.545196F },
						 new Location{ Name= "Novi Grad (RS)",  Latitude=45.045419F, Longitude = 16.384521F },
						 new Location{ Name= "Čelinac",  Latitude=44.738991F, Longitude = 17.322521F },
						 new Location{ Name= "Istočni Drvar",  Latitude=44.403945F, Longitude = 16.629868F },
						 new Location{ Name= "Jezero",  Latitude=44.350767F, Longitude = 17.169399F },
						 new Location{ Name= "Kotor-Varoš",  Latitude=44.622365F, Longitude = 17.376380F },
						 new Location{ Name= "Krupa na Uni",  Latitude=44.901605F, Longitude = 16.324997F },
						 new Location{ Name= "Kupres (RS)",  Latitude=44.886688F, Longitude = 16.354523F },
						 new Location{ Name= "Laktaši",  Latitude=44.907320F, Longitude = 17.300205F },
						 new Location{ Name= "Mrkonjić Grad",  Latitude=44.419804F, Longitude = 17.083397F },
						 new Location{ Name= "Oštra Luka",  Latitude=44.860371F, Longitude = 16.785822F },
						 new Location{ Name= "Petrovac",  Latitude=43.652472F, Longitude = 16.935081F },
						 new Location{ Name= "Prijedor",  Latitude=44.985199F, Longitude = 16.703339F },
						 new Location{ Name= "Prnjavor",  Latitude=44.867428F, Longitude = 17.665844F },
						 new Location{ Name= "Ribnik",  Latitude=44.484443F, Longitude = 16.814661F },
						 new Location{ Name= "Kneževo",  Latitude=44.491087F, Longitude = 17.379341F },
						 new Location{ Name= "Srbac",  Latitude=45.096307F, Longitude = 17.519245F },
						 new Location{ Name= "Šipovo",  Latitude=44.284721F, Longitude = 17.088461F },
						 new Location{ Name= "Teslić",  Latitude=44.608068F, Longitude = 17.852440F },
					 }
				 },
				 new Region
				 {
					 Name = "Bijeljina",
					 Entity = "RS",
					 Latitude = 44.750270F,
					 Longitude = 19.216800F,
					 Locations = new List<Location> 
					 {
						new Location{ Name= "Bijeljina", Latitude=44.750269F, Longitude = 19.216805F },
						new Location{ Name= "Lopare",  Latitude=44.637910F, Longitude = 18.845072F },
						new Location{ Name= "Ugljevik", Latitude=44.692393F, Longitude = 18.994546F },

				 }
				 },
				 new Region
				 {
					 Name = "Bosansko-podrinjski ",
					 Entity = "FBiH",
					 Latitude = 43.663400F,
					 Longitude = 18.974250F,
					 Locations = new List<Location>
					 { 
						 new Location{ Name= "Foča-Ustikolina",  Latitude=43.585194F, Longitude = 18.783209F },
						 new Location{ Name= "Goražde",  Latitude=43.663401F, Longitude = 18.974247F },
						 new Location{ Name= "Pale-Prača",  Latitude=43.765546F, Longitude = 18.763661F },
				 	 }
				 },
				 new Region
				 {
					 Name = "Brčko",
					 Entity = "Brčko",
					 Latitude = 44.870350F,
					 Longitude = 18.809970F,
					 Locations = new List<Location> 
					 { 
						 new Location{ Name= "Brčko", Latitude=44.870348F, Longitude = 18.809967F },
					 }
				 },
				 new Region
				 {
					 Name = "Doboj",
					 Entity = "RS",
					 Latitude = 44.740940F,
					 Longitude = 18.092940F,
					 Locations = new List<Location> 
					 { 
						 new Location{ Name= "Brod",  Latitude=45.133436F, Longitude = 17.983332F },
						 new Location{ Name= "Šamac",  Latitude=45.046905F, Longitude = 18.481364F },
						 new Location{ Name= "Derventa",  Latitude=44.981314F, Longitude = 17.909603F },
						 new Location{ Name= "Doboj",  Latitude=44.740942F, Longitude = 18.092937F },
						 new Location{ Name= "Donji Žabar",  Latitude=44.945604F, Longitude = 18.644636F },
						 new Location{ Name= "Modriča",  Latitude=44.959211F, Longitude = 18.298244F },
						 new Location{ Name= "Pelagićevo",  Latitude=44.907168F, Longitude = 18.612256F },
						 new Location{ Name= "Petrovo",  Latitude=44.625847F, Longitude = 18.364506F },
						 new Location{ Name= "Vukosavlje",  Latitude=44.988143F, Longitude = 18.257260F },
					 }
				 },
				 new Region
				 {
					 Name = "Foča",
					 Entity = "RS",
					 Latitude = 43.504740F,
					 Longitude = 18.778210F,
					 Locations = new List<Location> 
					 { 
						 new Location{ Name= "Čajniče",  Latitude=43.558563F, Longitude = 19.071879F },
						 new Location{ Name= "Foča",  Latitude=43.504737F, Longitude = 18.778210F },
						 new Location{ Name= "Kalinovik",  Latitude=43.504394F, Longitude = 18.447161F },
						 new Location{ Name= "Rudo",  Latitude=43.614081F, Longitude = 19.370184F },
						 new Location{ Name= "Ustiprača",  Latitude=43.689342F, Longitude = 19.091610F },
						 new Location{ Name= "Višegrad",  Latitude=43.786711F, Longitude = 19.293280F },
					 }
				 },
				 new Region
				 {
					 Name = "Hercegovačko-neretvanski",
					 Entity = "FBiH",
					 Latitude = 43.335540F,
					 Longitude = 17.799400F,
					 Locations = new List<Location>
					 {
						 new Location{ Name= "Čapljina",  Latitude=43.114017F, Longitude = 17.715454F },
						 new Location{ Name= "Čitluk",  Latitude=43.205739F, Longitude = 17.707386F },
						 new Location{ Name= "Jablanica",  Latitude=43.663432F, Longitude = 17.760344F },
						 new Location{ Name= "Konjic",  Latitude=43.651851F, Longitude = 17.962132F },
						 new Location{ Name= "Mostar",  Latitude=43.335542F, Longitude = 17.799397F },
						 new Location{ Name= "Neum",  Latitude=42.925037F, Longitude = 17.618036F },
						 new Location{ Name= "Prozor - Rama",  Latitude=43.821214F, Longitude = 17.612114F },
						 new Location{ Name= "Ravno",  Latitude=42.812167F, Longitude = 18.027867F },
						 new Location{ Name= "Stolac",  Latitude=43.083386F, Longitude = 17.959492F },
					 }
				 },
				 new Region
				 {
					 Name = "Kanton br. 10",
					 Entity = "FBiH",
					 Latitude = 43.824930F,
					 Longitude = 17.007090F,
					 Locations = new List<Location> 
					 { 
						 new Location{ Name= "Bosansko Grahovo",  Latitude=44.161396F, Longitude = 16.377354F },
						 new Location{ Name= "Drvar",  Latitude=44.368042F, Longitude = 16.391816F },
						 new Location{ Name= "Glamoč",  Latitude=44.047715F, Longitude = 16.849852F },
						 new Location{ Name= "Kupres (FBIH)",  Latitude=43.994821F, Longitude = 17.283211F },
						 new Location{ Name= "Livno",  Latitude=43.824929F, Longitude = 17.007093F },
						 new Location{ Name= "Tomislavgrad",  Latitude=43.719133F, Longitude = 17.226391F },
					 }
				 },
				 new Region
				 {
					 Name = "Posavski ",
					 Entity = "FBiH",
					 Latitude = 44.982350F,
					 Longitude = 18.729670F,
					 Locations = new List<Location> 
					 { 
						 new Location{ Name= "Domaljevac-Šamac",  Latitude=45.058312F, Longitude = 18.584898F },
						 new Location{ Name= "Odžak",  Latitude=45.010417F, Longitude = 18.326483F },
						 new Location{ Name= "Orašje",  Latitude=44.982346F, Longitude = 18.729672F },
					 }
				 },
				 new Region
				 {
					 Name = "Sarajevo",
					 Entity = "FBiH",
					 Latitude = 43.858230F,
					 Longitude = 18.414140F,
					 Locations = new List<Location> 
					 { 
						 new Location{ Name= "Centar",  Latitude=43.858235F, Longitude = 18.414137F },
						 new Location{ Name= "Hadžići",  Latitude=43.824542F, Longitude = 18.220911F },
						 new Location{ Name= "Ilidža",  Latitude=43.828304F, Longitude = 18.300347F },
						 new Location{ Name= "Ilijaš",  Latitude=43.950192F, Longitude = 18.273268F },
						 new Location{ Name= "Novi Grad (Sarajevo)",  Latitude=43.848889F, Longitude = 18.371111F },
						 new Location{ Name= "Novo Sarajevo",  Latitude=43.868889F, Longitude = 18.408611F },		
						 new Location{ Name= "Stari Grad",  Latitude=43.866667F, Longitude = 18.433333F },
						 new Location{ Name= "Trnovo (FBIH)",  Latitude=43.666568F, Longitude = 18.446560F },
						 new Location{ Name= "Vogošća",  Latitude=43.887408F, Longitude = 18.384976F }, 
					 }
				 },
				 new Region
				 {
					 Name = "Sarajevsko-romanijska",
					 Entity = "RS",
					 Latitude = 44.082710F,
					 Longitude = 18.956050F,
					 Locations = new List<Location> 
					 { 			
						 new Location{ Name= "Han-Pijesak", Latitude=44.082714F, Longitude = 18.956051F },
						 new Location{ Name= "Istočna Ilidža", Latitude=43.812915F, Longitude = 18.363047F },
						 new Location{ Name= "Istočni Stari Grad", Latitude=43.880861F, Longitude = 18.497028F },
						 new Location{ Name= "Istočno Novo Sarajevo", Latitude=43.824620F, Longitude = 18.364763F },
						 new Location{ Name= "Pale", Latitude=43.814711F, Longitude = 18.570671F },		
						 new Location{ Name= "Rogatica", Latitude=43.800991F, Longitude = 19.001670F },
						 new Location{ Name= "Sokolac", Latitude=43.932671F, Longitude = 18.794947F },
						 new Location{ Name= "Trnovo (RS)", Latitude=43.666847F, Longitude = 18.446946F },
					 }
				 },
				 new Region
				 {
					 Name = "Srednjobosanski ",
					 Entity = "RS",
					 Latitude = 44.227390F,
					 Longitude = 17.661170F,
					 Locations = new List<Location> 
					 { 
						 new Location{ Name= "Bugojno",  Latitude=44.052187F, Longitude = 17.445517F },			
						 new Location{ Name= "Busovača",  Latitude=44.099421F, Longitude = 17.885056F },
						 new Location{ Name= "Dobretići",  Latitude=44.395186F, Longitude = 17.402859F },
						 new Location{ Name= "Donji Vakuf",  Latitude=44.144338F, Longitude = 17.400885F },
						 new Location{ Name= "Fojnica",  Latitude=43.963878F, Longitude = 17.893381F },
						 new Location{ Name= "Jajce",  Latitude=44.342727F, Longitude = 17.268105F },
						 new Location{ Name= "Kiseljak",  Latitude=43.940058F, Longitude = 18.079290F },
						 new Location{ Name= "Kreševo",  Latitude=43.878593F, Longitude = 18.058777F },
						 new Location{ Name= "Novi Travnik",  Latitude=44.172293F, Longitude = 17.657347F },
						 new Location{ Name= "Travnik",  Latitude=44.227396F, Longitude = 17.661166F },
						 new Location{ Name= "Vitez",  Latitude=44.151174F, Longitude = 17.788925F },
						 new Location{ Name= "Gornji Vakuf - Uskoplje",  Latitude=43.935484F, Longitude =  17.583361F },            
					 }
				 },
				 new Region
				 {
					 Name = "Trebinje",
					 Entity = "RS",
					 Latitude = 42.712080F,
					 Longitude = 18.342190F,
					 Locations = new List<Location> 
					 {  
						 new Location{ Name= "Berkovići", Latitude=43.095468F, Longitude = 18.169670F },
						 new Location{ Name= "Bileća", Latitude=42.867456F, Longitude = 18.415596F },
						 new Location{ Name= "Gacko", Latitude=43.165373F, Longitude = 18.537283F },
						 new Location{ Name= "Istočni Mostar", Latitude=43.416021F, Longitude = 18.072510F },
						 new Location{ Name= "Ljubinje", Latitude=42.951146F, Longitude = 18.088474F },
						 new Location{ Name= "Nevesinje", Latitude=43.259268F, Longitude = 18.116455F },
						 new Location{ Name= "Trebinje", Latitude=42.712083F, Longitude = 18.342190F },
					 }					
				 },
				 new Region
				 {
					 Name = "Tuzlanski",
					 Entity = "FBiH",
					 Latitude = 44.540000F,
					 Longitude = 18.670000F,
					 Locations = new List<Location> 
					 {
						 new Location{ Name= "Banovići",  Latitude=44.411743F, Longitude = 18.553441F }, 
						 new Location{ Name= "Čelić",  Latitude=44.716932F, Longitude = 18.820653F },
						 new Location{ Name= "Doboj Istok",  Latitude=44.731552F, Longitude = 18.077831F },
						 new Location{ Name= "Gračanica",  Latitude=44.698983F, Longitude = 18.305626F },
						 new Location{ Name= "Gradačac",  Latitude=44.874849F, Longitude = 18.429136F },
						 new Location{ Name= "Kalesija",  Latitude=44.439112F, Longitude = 18.912449F },
						 new Location{ Name= "Kladanj",  Latitude=44.226873F, Longitude = 18.691864F },
						 new Location{ Name= "Lukavac",  Latitude=44.544484F, Longitude = 18.476257F },
						 new Location{ Name= "Sapna",  Latitude=44.508136F, Longitude = 19.000082F },
						 new Location{ Name= "Srebrenik",  Latitude=44.728199F, Longitude = 18.562431F },
						 new Location{ Name= "Teočak",  Latitude=44.602263F, Longitude = 18.995190F },
						 new Location{ Name= "Tuzla",  Latitude=44.540000F, Longitude = 18.670000F },
						 new Location{ Name= "Živinice",  Latitude=44.451352F, Longitude = 18.640988F },
					}
				 },
				 new Region
				 {
					 Name = "Unsko-Sanski",
					 Entity = "FBiH",
					 Latitude = 44.819840F,
					 Longitude = 15.870780F,
					 Locations = new List<Location> 
					 { 
						 new Location{ Name= "Bihać", Latitude=44.819838F, Longitude = 15.870781F },
						 new Location{ Name= "Bosanska Krupa", Latitude=44.883120F, Longitude = 16.150589F },
						 new Location{ Name= "Bosanski Petrovac", Latitude=44.555677F, Longitude = 16.370487F },
						 new Location{ Name= "Bužim", Latitude=45.068974F, Longitude = 16.023645F },
						 new Location{ Name= "Cazin", Latitude=44.966863F, Longitude = 15.938931F },
						 new Location{ Name= "Ključ", Latitude=44.535124F, Longitude = 16.771917F },
						 new Location{ Name= "Sanski Most", Latitude=44.765749F, Longitude = 16.664243F },
						 new Location{ Name= "Velika Kladuša", Latitude=45.185788F, Longitude = 15.800228F },			 
					 }
				 },
				 new Region
				 {
					 Name = "Vlasenica",
					 Entity = "RS",
					 Latitude = 44.181220F,
					 Longitude = 18.944890F,
					 Locations = new List<Location> 
					 { 
						 new Location{ Name= "Bratunac",  Latitude=44.198636F, Longitude = 19.343619F },
						 new Location{ Name= "Milići",  Latitude=44.166937F, Longitude = 19.078960F },
						 new Location{ Name= "Osmaci",  Latitude=44.403250F, Longitude = 18.915882F },
						 new Location{ Name= "Srebrenica",  Latitude=44.104259F, Longitude = 19.297593F },
						 new Location{ Name= "Šekovići",  Latitude=44.296824F, Longitude = 18.854256F },
						 new Location{ Name= "Vlasenica",  Latitude=44.181219F, Longitude = 18.944893F },
						 new Location{ Name= "Zvornik",  Latitude=44.393070F, Longitude = 19.105997F },
					 }
				 },

				 new Region
				 {
					 Name = "Zapadnohercegovački",
					 Entity = "FBiH",
					 Latitude = 43.378100F,
					 Longitude = 17.580870F,
					 Locations = new List<Location>
					 {
						 new Location{ Name= "Grude", Latitude=43.377729F, Longitude = 17.396164F },				
						 new Location{ Name= "Ljubuški", Latitude=43.192631F, Longitude = 17.546668F },			   
						 new Location{ Name= "Posušje", Latitude=43.471733F, Longitude = 17.330332F },
						 new Location{ Name= "Široki Brijeg", Latitude=43.378103F, Longitude = 17.580872F },
					 }
				 },

				 new Region
				 {
					 Name = "Zeničko-dobojski",
					 Entity = "FBiH",
					 Latitude = 44.214140F,
					 Longitude = 17.916380F,
					 Locations = new List<Location>
					 { 
						 new Location{ Name= "Breza",  Latitude=44.014299F, Longitude = 18.280048F },
						 new Location{ Name= "Doboj Jug",  Latitude=44.728199F, Longitude = 18.086758F },
						 new Location{ Name= "Kakanj",  Latitude=44.123824F, Longitude = 18.100834F },
						 new Location{ Name= "Maglaj",  Latitude=44.549836F, Longitude = 18.096714F },
						 new Location{ Name= "Olovo",  Latitude=44.129462F, Longitude = 18.588953F },
						 new Location{ Name= "Tešanj",  Latitude=44.629085F, Longitude = 17.983932F },
						 new Location{ Name= "Usora",  Latitude=44.602049F, Longitude = 17.732577F },
						 new Location{ Name= "Vareš",  Latitude=44.159056F, Longitude = 18.325882F },
						 new Location{ Name= "Visoko",  Latitude=43.999730F, Longitude = 18.178253F },
						 new Location{ Name= "Zavidovići",  Latitude=44.441793F, Longitude = 18.147376F },
						 new Location{ Name= "Zenica",  Latitude=44.214141F, Longitude = 17.916384F },
						 new Location{ Name= "Žepče",  Latitude=44.415145F, Longitude = 18.110747F },
					 }
				 }
				 );
		}

		public static void GetAllLocations( out List<Region> regions, out List<Location> locations )
		{
			var banjaLuka = new Region { Name = "Banja Luka", Entity = "RS", Latitude = 44.769160F, Longitude = 17.182620F };
			var bijeljina = new Region { Name = "Bijeljina", Entity = "RS", Latitude = 44.750270F, Longitude = 19.216800F };
			var bpk = new Region { Name = "Bosansko-podrinjski ", Entity = "FBiH", Latitude = 43.663400F, Longitude = 18.974250F };
			var brčko = new Region { Name = "Brčko", Entity = "Brčko", Latitude = 44.870350F, Longitude = 18.809970F };
			var doboj = new Region { Name = "Doboj", Entity = "RS", Latitude = 44.740940F, Longitude = 18.092940F };
			var foča = new Region { Name = "Foča", Entity = "RS", Latitude = 43.504740F, Longitude = 18.778210F };
			var hnk = new Region { Name = "Hercegovačko-neretvanski", Entity = "FBiH", Latitude = 43.335540F, Longitude = 17.799400F };
			var k10 = new Region { Name = "Kanton br. 10", Entity = "FBiH", Latitude = 43.824930F, Longitude = 17.007090F };
			var posavski = new Region { Name = "Posavski ", Entity = "FBiH", Latitude = 44.982350F, Longitude = 18.729670F };
			var sarajevo = new Region { Name = "Sarajevo", Entity = "FBiH", Latitude = 43.858230F, Longitude = 18.414140F };
			var sr = new Region { Name = "Sarajevsko-romanijska", Entity = "RS", Latitude = 44.082710F, Longitude = 18.956050F };
			var srednjobosanski = new Region { Name = "Srednjobosanski ", Entity = "RS", Latitude = 44.227390F, Longitude = 17.661170F };
			var trebinje = new Region { Name = "Trebinje", Entity = "RS", Latitude = 42.712080F, Longitude = 18.342190F };
			var tuzlanski = new Region { Name = "Tuzlanski", Entity = "FBiH", Latitude = 44.540000F, Longitude = 18.670000F };
			var usk = new Region { Name = "Unsko-Sanski", Entity = "FBiH", Latitude = 44.819840F, Longitude = 15.870780F };
			var vlasenica = new Region { Name = "Vlasenica", Entity = "RS", Latitude = 44.181220F, Longitude = 18.944890F };
			var zapadnohercegovački = new Region { Name = "Zapadnohercegovački", Entity = "FBiH", Latitude = 43.378100F, Longitude = 17.580870F };
			var zedok = new Region { Name = "Zeničko-dobojski", Entity = "FBiH", Latitude = 44.214140F, Longitude = 17.916380F };


			regions = new List<Region>
			{
				banjaLuka, bijeljina, bpk, brčko, doboj, foča, hnk, k10, posavski, sarajevo, sr, srednjobosanski, trebinje,
				tuzlanski, usk, vlasenica, zapadnohercegovački, zedok
			};
			locations = new List<Location>
			{
				//new Location{ Name= "", RegionID="", Entity="", Latitute= 0F, Longitute = 0F }
				 new Location{ Name= "Banovići", RegionID=tuzlanski.RegionID, Latitude=44.411743F, Longitude = 18.553441F },
				 new Location{ Name= "Banja Luka", RegionID=banjaLuka.RegionID, Latitude=44.769162F, Longitude = 17.182617F },
				 new Location{ Name= "Berkovići", RegionID=trebinje.RegionID, Latitude=43.095468F, Longitude = 18.169670F },
				 new Location{ Name= "Bihać", RegionID=usk.RegionID, Latitude=44.819838F, Longitude = 15.870781F },
				 new Location{ Name= "Bijeljina", RegionID=bijeljina.RegionID, Latitude=44.750269F, Longitude = 19.216805F },
				 new Location{ Name= "Bileća", RegionID=trebinje.RegionID, Latitude=42.867456F, Longitude = 18.415596F },
				 new Location{ Name= "Kozarska Dubica", RegionID=banjaLuka.RegionID, Latitude=45.184094F, Longitude = 16.806765F },
				 new Location{ Name= "Gradiška", RegionID=banjaLuka.RegionID, Latitude=45.142276F, Longitude = 17.252827F },
				 new Location{ Name= "Kostajnica", RegionID=banjaLuka.RegionID, Latitude=45.218657F, Longitude = 16.545196F },
				 new Location{ Name= "Bosanska Krupa", RegionID=usk.RegionID, Latitude=44.883120F, Longitude = 16.150589F },
				 new Location{ Name= "Brod", RegionID=doboj.RegionID, Latitude=45.133436F, Longitude = 17.983332F },
				 new Location{ Name= "Novi Grad (RS)", RegionID=banjaLuka.RegionID, Latitude=45.045419F, Longitude = 16.384521F },
				 new Location{ Name= "Bosanski Petrovac", RegionID=usk.RegionID, Latitude=44.555677F, Longitude = 16.370487F },
				 new Location{ Name= "Šamac", RegionID=doboj.RegionID, Latitude=45.046905F, Longitude = 18.481364F },
				 new Location{ Name= "Bosansko Grahovo", RegionID=k10.RegionID, Latitude=44.161396F, Longitude = 16.377354F },
				 new Location{ Name= "Bratunac", RegionID=vlasenica.RegionID, Latitude=44.198636F, Longitude = 19.343619F },
				 new Location{ Name= "Brčko", RegionID=brčko.RegionID, Latitude=44.870348F, Longitude = 18.809967F },
				 new Location{ Name= "Breza", RegionID=zedok.RegionID, Latitude=44.014299F, Longitude = 18.280048F },
				 new Location{ Name= "Bugojno", RegionID=srednjobosanski.RegionID, Latitude=44.052187F, Longitude = 17.445517F },
				 new Location{ Name= "Busovača", RegionID=srednjobosanski.RegionID, Latitude=44.099421F, Longitude = 17.885056F },
				 new Location{ Name= "Bužim", RegionID=usk.RegionID, Latitude=45.068974F, Longitude = 16.023645F },
				 new Location{ Name= "Cazin", RegionID=usk.RegionID, Latitude=44.966863F, Longitude = 15.938931F },
				 new Location{ Name= "Centar", RegionID=sarajevo.RegionID, Latitude=43.858235F, Longitude = 18.414137F },
				 new Location{ Name= "Čajniče", RegionID=foča.RegionID, Latitude=43.558563F, Longitude = 19.071879F },
				 new Location{ Name= "Čapljina", RegionID=hnk.RegionID, Latitude=43.114017F, Longitude = 17.715454F },
				 new Location{ Name= "Čelić", RegionID=tuzlanski.RegionID, Latitude=44.716932F, Longitude = 18.820653F },
				 new Location{ Name= "Čelinac", RegionID=banjaLuka.RegionID, Latitude=44.738991F, Longitude = 17.322521F },
				 new Location{ Name= "Čitluk", RegionID=hnk.RegionID, Latitude=43.205739F, Longitude = 17.707386F },
				 new Location{ Name= "Derventa", RegionID=doboj.RegionID, Latitude=44.981314F, Longitude = 17.909603F },
				 new Location{ Name= "Drvar", RegionID=k10.RegionID, Latitude=44.368042F, Longitude = 16.391816F },
				 new Location{ Name= "Doboj", RegionID=doboj.RegionID, Latitude=44.740942F, Longitude = 18.092937F },
				 new Location{ Name= "Doboj Istok", RegionID=tuzlanski.RegionID, Latitude=44.731552F, Longitude = 18.077831F },
				 new Location{ Name= "Doboj Jug", RegionID=zedok.RegionID, Latitude=44.728199F, Longitude = 18.086758F },
				 new Location{ Name= "Dobretići", RegionID=srednjobosanski.RegionID, Latitude=44.395186F, Longitude = 17.402859F },
				 new Location{ Name= "Domaljevac-Šamac", RegionID=posavski.RegionID, Latitude=45.058312F, Longitude = 18.584898F },
				 new Location{ Name= "Donji Vakuf", RegionID=srednjobosanski.RegionID, Latitude=44.144338F, Longitude = 17.400885F },
				 new Location{ Name= "Donji Žabar", RegionID=doboj.RegionID, Latitude=44.945604F, Longitude = 18.644636F },
				 new Location{ Name= "Foča", RegionID=foča.RegionID, Latitude=43.504737F, Longitude = 18.778210F },
				 new Location{ Name= "Foča-Ustikolina", RegionID=bpk.RegionID, Latitude=43.585194F, Longitude = 18.783209F },
				 new Location{ Name= "Fojnica", RegionID=srednjobosanski.RegionID, Latitude=43.963878F, Longitude = 17.893381F },
				 new Location{ Name= "Gacko", RegionID=trebinje.RegionID, Latitude=43.165373F, Longitude = 18.537283F },
				 new Location{ Name= "Glamoč", RegionID=k10.RegionID, Latitude=44.047715F, Longitude = 16.849852F },
				 new Location{ Name= "Goražde", RegionID=bpk.RegionID, Latitude=43.663401F, Longitude = 18.974247F },
				 new Location{ Name= "Gornji Vakuf - Uskoplje", RegionID=srednjobosanski.RegionID, Latitude=43.935484F, Longitude = 17.583361F },
				 new Location{ Name= "Gračanica", RegionID=tuzlanski.RegionID, Latitude=44.698983F, Longitude = 18.305626F },
				 new Location{ Name= "Gradačac", RegionID=tuzlanski.RegionID, Latitude=44.874849F, Longitude = 18.429136F },
				 new Location{ Name= "Grude", Latitude=43.377729F, Longitude = 17.396164F },
				 new Location{ Name= "Hadžići", RegionID=sarajevo.RegionID, Latitude=43.824542F, Longitude = 18.220911F },
				 new Location{ Name= "Han-Pijesak", RegionID=sr.RegionID, Latitude=44.082714F, Longitude = 18.956051F },
				 new Location{ Name= "Ilidža", RegionID=sarajevo.RegionID, Latitude=43.828304F, Longitude = 18.300347F },
				 new Location{ Name= "Ilijaš", RegionID=sarajevo.RegionID, Latitude=43.950192F, Longitude = 18.273268F },
				 new Location{ Name= "Istočna Ilidža", RegionID=sr.RegionID, Latitude=43.812915F, Longitude = 18.363047F },
				 new Location{ Name= "Istočni Drvar", RegionID=banjaLuka.RegionID, Latitude=44.403945F, Longitude = 16.629868F },
				 new Location{ Name= "Istočni Mostar", RegionID=trebinje.RegionID, Latitude=43.416021F, Longitude = 18.072510F },
				 new Location{ Name= "Istočni Stari Grad", RegionID=sr.RegionID, Latitude=43.880861F, Longitude = 18.497028F },
				 new Location{ Name= "Istočno Novo Sarajevo", RegionID=sr.RegionID, Latitude=43.824620F, Longitude = 18.364763F },
				 new Location{ Name= "Jablanica", RegionID=hnk.RegionID, Latitude=43.663432F, Longitude = 17.760344F },
				 new Location{ Name= "Jajce", RegionID=srednjobosanski.RegionID, Latitude=44.342727F, Longitude = 17.268105F },
				 new Location{ Name= "Jezero", RegionID=banjaLuka.RegionID, Latitude=44.350767F, Longitude = 17.169399F },
				 new Location{ Name= "Kakanj", RegionID=zedok.RegionID, Latitude=44.123824F, Longitude = 18.100834F },
				 new Location{ Name= "Kalesija", RegionID=tuzlanski.RegionID, Latitude=44.439112F, Longitude = 18.912449F },
				 new Location{ Name= "Kalinovik", RegionID=foča.RegionID, Latitude=43.504394F, Longitude = 18.447161F },
				 new Location{ Name= "Kiseljak", RegionID=srednjobosanski.RegionID, Latitude=43.940058F, Longitude = 18.079290F },
				 new Location{ Name= "Kladanj", RegionID=tuzlanski.RegionID, Latitude=44.226873F, Longitude = 18.691864F },
				 new Location{ Name= "Ključ", RegionID=usk.RegionID, Latitude=44.535124F, Longitude = 16.771917F },
				 new Location{ Name= "Konjic", RegionID=hnk.RegionID, Latitude=43.651851F, Longitude = 17.962132F },
				 new Location{ Name= "Kotor-Varoš", RegionID=banjaLuka.RegionID, Latitude=44.622365F, Longitude = 17.376380F },
				 new Location{ Name= "Kreševo", RegionID=srednjobosanski.RegionID, Latitude=43.878593F, Longitude = 18.058777F },
				 new Location{ Name= "Krupa na Uni", RegionID=banjaLuka.RegionID, Latitude=44.901605F, Longitude = 16.324997F },
				 new Location{ Name= "Kupres (FBIH)", RegionID=k10.RegionID, Latitude=43.994821F, Longitude = 17.283211F },
				 new Location{ Name= "Kupres (RS)", RegionID=banjaLuka.RegionID, Latitude=44.886688F, Longitude = 16.354523F },
				 new Location{ Name= "Laktaši", RegionID=banjaLuka.RegionID, Latitude=44.907320F, Longitude = 17.300205F },
				 new Location{ Name= "Livno", RegionID=k10.RegionID, Latitude=43.824929F, Longitude = 17.007093F },
				 new Location{ Name= "Lopare", RegionID=bijeljina.RegionID, Latitude=44.637910F, Longitude = 18.845072F },
				 new Location{ Name= "Lukavac", RegionID=tuzlanski.RegionID, Latitude=44.544484F, Longitude = 18.476257F },
				 new Location{ Name= "Ljubinje", RegionID=trebinje.RegionID, Latitude=42.951146F, Longitude = 18.088474F },
				 new Location{ Name= "Ljubuški", Latitude=43.192631F, Longitude = 17.546668F },
				 new Location{ Name= "Maglaj", RegionID=zedok.RegionID, Latitude=44.549836F, Longitude = 18.096714F },
				 new Location{ Name= "Milići", RegionID=vlasenica.RegionID, Latitude=44.166937F, Longitude = 19.078960F },
				 new Location{ Name= "Modriča", RegionID=doboj.RegionID, Latitude=44.959211F, Longitude = 18.298244F },
				 new Location{ Name= "Mostar", RegionID=hnk.RegionID, Latitude=43.335542F, Longitude = 17.799397F },
				 new Location{ Name= "Mrkonjić Grad", RegionID=banjaLuka.RegionID, Latitude=44.419804F, Longitude = 17.083397F },
				 new Location{ Name= "Neum", RegionID=hnk.RegionID, Latitude=42.925037F, Longitude = 17.618036F },
				 new Location{ Name= "Nevesinje", RegionID=trebinje.RegionID, Latitude=43.259268F, Longitude = 18.116455F },
				 new Location{ Name= "Novi Grad (Sarajevo)", RegionID=sarajevo.RegionID, Latitude=43.848889F, Longitude = 18.371111F },
				 new Location{ Name= "Novi Travnik", RegionID=srednjobosanski.RegionID, Latitude=44.172293F, Longitude = 17.657347F },
				 new Location{ Name= "Novo Sarajevo", RegionID=sarajevo.RegionID, Latitude=43.868889F, Longitude = 18.408611F },
				 new Location{ Name= "Odžak", RegionID=posavski.RegionID, Latitude=45.010417F, Longitude = 18.326483F },
				 new Location{ Name= "Olovo", RegionID=zedok.RegionID, Latitude=44.129462F, Longitude = 18.588953F },
				 new Location{ Name= "Orašje", RegionID=posavski.RegionID, Latitude=44.982346F, Longitude = 18.729672F },
				 new Location{ Name= "Osmaci", RegionID=vlasenica.RegionID, Latitude=44.403250F, Longitude = 18.915882F },
				 new Location{ Name= "Oštra Luka", RegionID=banjaLuka.RegionID, Latitude=44.860371F, Longitude = 16.785822F },
				 new Location{ Name= "Pale", RegionID=sr.RegionID, Latitude=43.814711F, Longitude = 18.570671F },
				 new Location{ Name= "Pale-Prača", RegionID=bpk.RegionID, Latitude=43.765546F, Longitude = 18.763661F },
				 new Location{ Name= "Pelagićevo", RegionID=doboj.RegionID, Latitude=44.907168F, Longitude = 18.612256F },
				 new Location{ Name= "Petrovac", RegionID=banjaLuka.RegionID, Latitude=43.652472F, Longitude = 16.935081F },
				 new Location{ Name= "Petrovo", RegionID=doboj.RegionID, Latitude=44.625847F, Longitude = 18.364506F },
				 new Location{ Name= "Posušje", Latitude=43.471733F, Longitude = 17.330332F },
				 new Location{ Name= "Prijedor", RegionID=banjaLuka.RegionID, Latitude=44.985199F, Longitude = 16.703339F },
				 new Location{ Name= "Prnjavor", RegionID=banjaLuka.RegionID, Latitude=44.867428F, Longitude = 17.665844F },
				 new Location{ Name= "Prozor - Rama", RegionID=hnk.RegionID, Latitude=43.821214F, Longitude = 17.612114F },
				 new Location{ Name= "Ravno", RegionID=hnk.RegionID, Latitude=42.812167F, Longitude = 18.027867F },
				 new Location{ Name= "Ribnik", RegionID=banjaLuka.RegionID, Latitude=44.484443F, Longitude = 16.814661F },
				 new Location{ Name= "Rogatica", RegionID=sr.RegionID, Latitude=43.800991F, Longitude = 19.001670F },
				 new Location{ Name= "Rudo", RegionID=foča.RegionID, Latitude=43.614081F, Longitude = 19.370184F },
				 new Location{ Name= "Sanski Most", RegionID=usk.RegionID, Latitude=44.765749F, Longitude = 16.664243F },
				 new Location{ Name= "Sapna", RegionID=tuzlanski.RegionID, Latitude=44.508136F, Longitude = 19.000082F },
				 new Location{ Name= "Kneževo", RegionID=banjaLuka.RegionID, Latitude=44.491087F, Longitude = 17.379341F },
				 new Location{ Name= "Sokolac", RegionID=sr.RegionID, Latitude=43.932671F, Longitude = 18.794947F },
				 new Location{ Name= "Srbac", RegionID=banjaLuka.RegionID, Latitude=45.096307F, Longitude = 17.519245F },
				 new Location{ Name= "Srebrenica", RegionID=vlasenica.RegionID, Latitude=44.104259F, Longitude = 19.297593F },
				 new Location{ Name= "Srebrenik", RegionID=tuzlanski.RegionID, Latitude=44.728199F, Longitude = 18.562431F },
				 new Location{ Name= "Stari Grad", RegionID=sarajevo.RegionID, Latitude=43.866667F, Longitude = 18.433333F },
				 new Location{ Name= "Stolac", RegionID=hnk.RegionID, Latitude=43.083386F, Longitude = 17.959492F },
				 new Location{ Name= "Šekovići", RegionID=vlasenica.RegionID, Latitude=44.296824F, Longitude = 18.854256F },
				 new Location{ Name= "Šipovo", RegionID=banjaLuka.RegionID, Latitude=44.284721F, Longitude = 17.088461F },
				 new Location{ Name= "Široki Brijeg", Latitude=43.378103F, Longitude = 17.580872F },
				 new Location{ Name= "Teočak", RegionID=tuzlanski.RegionID, Latitude=44.602263F, Longitude = 18.995190F },
				 new Location{ Name= "Teslić", RegionID=banjaLuka.RegionID, Latitude=44.608068F, Longitude = 17.852440F },
				 new Location{ Name= "Tešanj", RegionID=zedok.RegionID, Latitude=44.629085F, Longitude = 17.983932F },
				 new Location{ Name= "Tomislavgrad", RegionID=k10.RegionID, Latitude=43.719133F, Longitude = 17.226391F },
				 new Location{ Name= "Travnik", RegionID=srednjobosanski.RegionID, Latitude=44.227396F, Longitude = 17.661166F },
				 new Location{ Name= "Trebinje", RegionID=trebinje.RegionID, Latitude=42.712083F, Longitude = 18.342190F },
				 new Location{ Name= "Trnovo (RS)", RegionID=sr.RegionID, Latitude=43.666847F, Longitude = 18.446946F },
				 new Location{ Name= "Trnovo (FBIH)", RegionID=sarajevo.RegionID, Latitude=43.666568F, Longitude = 18.446560F },
				 new Location{ Name= "Tuzla", RegionID=tuzlanski.RegionID, Latitude=44.540000F, Longitude = 18.670000F },
				 new Location{ Name= "Ugljevik", RegionID=bijeljina.RegionID, Latitude=44.692393F, Longitude = 18.994546F },
				 new Location{ Name= "Usora", RegionID=zedok.RegionID, Latitude=44.602049F, Longitude = 17.732577F },
				 new Location{ Name= "Ustiprača", RegionID=foča.RegionID, Latitude=43.689342F, Longitude = 19.091610F },
				 new Location{ Name= "Vareš", RegionID=zedok.RegionID, Latitude=44.159056F, Longitude = 18.325882F },
				 new Location{ Name= "Velika Kladuša", RegionID=usk.RegionID, Latitude=45.185788F, Longitude = 15.800228F },
				 new Location{ Name= "Visoko", RegionID=zedok.RegionID, Latitude=43.999730F, Longitude = 18.178253F },
				 new Location{ Name= "Višegrad", RegionID=foča.RegionID, Latitude=43.786711F, Longitude = 19.293280F },
				 new Location{ Name= "Vitez", RegionID=srednjobosanski.RegionID, Latitude=44.151174F, Longitude = 17.788925F },
				 new Location{ Name= "Vlasenica", RegionID=vlasenica.RegionID, Latitude=44.181219F, Longitude = 18.944893F },
				 new Location{ Name= "Vogošća", RegionID=sarajevo.RegionID, Latitude=43.887408F, Longitude = 18.384976F },
				 new Location{ Name= "Vukosavlje", RegionID=doboj.RegionID, Latitude=44.988143F, Longitude = 18.257260F },
				 new Location{ Name= "Zavidovići", RegionID=zedok.RegionID, Latitude=44.441793F, Longitude = 18.147376F },
				 new Location{ Name= "Zenica", RegionID=zedok.RegionID, Latitude=44.214141F, Longitude = 17.916384F },
				 new Location{ Name= "Zvornik", RegionID=vlasenica.RegionID, Latitude=44.393070F, Longitude = 19.105997F },
				 new Location{ Name= "Žepče", RegionID=zedok.RegionID, Latitude=44.415145F, Longitude = 18.110747F },
				 new Location{ Name= "Živinice", RegionID=tuzlanski.RegionID, Latitude=44.451352F, Longitude = 18.640988F },
			};
		}

		#endregion
	}
}