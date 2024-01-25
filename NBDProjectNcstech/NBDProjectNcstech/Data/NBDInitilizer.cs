using Microsoft.EntityFrameworkCore;
using NBDProjectNcstech.Models;
using System.Diagnostics;

namespace NBDProjectNcstech.Data
{
	public static class NBDInitilizer
	{
		public static void Seed(IApplicationBuilder applicationBuilder)
		{
			NBDContext context = applicationBuilder.ApplicationServices.CreateScope()
				.ServiceProvider.GetRequiredService<NBDContext>();
			try
			{
                //Delete and recreate the Database with every restart
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                context.Database.Migrate();

                //To randomly generate data
                Random random = new Random();
				DateTime ranDate = DateTime.Now;
				//Provinces, Cities 
				//if (!context.Provinces.Any())
				//{
				//	var provinces = new List<Province>
				//	{
				//		new Province { ID = "ON", Name = "Ontario"},
				//		new Province { ID = "PE", Name = "Prince Edward Island"},
				//		new Province { ID = "NB", Name = "New Brunswick"},
				//		new Province { ID = "BC", Name = "British Columbia"},
				//		new Province { ID = "NL", Name = "Newfoundland and Labrador"},
				//		new Province { ID = "SK", Name = "Saskatchewan"},
				//		new Province { ID = "NS", Name = "Nova Scotia"},
				//		new Province { ID = "MB", Name = "Manitoba"},
				//		new Province { ID = "QC", Name = "Quebec"},
				//		new Province { ID = "YT", Name = "Yukon"},
				//		new Province { ID = "NU", Name = "Nunavut"},
				//		new Province { ID = "NT", Name = "Northwest Territories"},
				//		new Province { ID = "AB", Name = "Alberta"}
				//	};
				//	context.Provinces.AddRange(provinces);
				//	context.SaveChanges();
				//}


				//string[] Names = {"Gagan Shrestha", "Josh Rydzpol Adlhaie Binalay", "Josh Martin", "Femarie Vien Briones", "Olubusiyi Olorungbemi Famobiwo" };
				if (!context.Clients.Any())
				{
					context.Clients.AddRange(
						new Client
						{
							Name = "Toronto Hotel",
							ContactPerson= "Gagan Shrestha",
							Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
							Street = "123 Lincon street",
							City = "Toronto",
							Province = "Ontario",
							PostalCode = "L2O 1B4"

						},
						new Client
						{
							Name = "Outlet Mall",
							ContactPerson = "Josh Rydzpol Adlhaie Binalay",
							Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
							Street = "124 Great Ave.",
							City = "Niagara Falls",
							Province = "Ontario",
							PostalCode = "L2O 1B4"
						},
						new Client
						{
							Name = "Welland Hospital",
							ContactPerson = "Josh Martin",
							Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
							Street = "924 Heavenly Street",
							City = "Welland",
							Province = "Ontario",
							PostalCode = "E2O 1C4"
						},
						new Client
						{
							Name = "Queen's Mall",
							ContactPerson = "Femarie Vien Briones",
							Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
							Street = "24 Fairy Street",
							City = "Hamilton",
							Province = "Ontario",
							PostalCode = "H2O 2C4"
						},
						new Client
						{
							Name = "Welland Hotel",
							ContactPerson = "Olubusiyi Olorungbemi Famobiwo",
							Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
							Street = "24 Victoria Ave.",
							City = "Welland",
							Province = "Ontario",
							PostalCode = "O2B 2C4"
						});
					context.SaveChanges();
				}
				if (!context.Projects.Any())
				{
					DateTime ranBidDate = ranDate.AddDays(-random.Next(10, 60));
					context.Projects.AddRange(
					new Project
					{
						BidDate = new DateTime(2023, 08, 24),
						ProjectSite = "Main Entrance Garden",
						Est_BeginDate = new DateTime(2023, 09, 4),
						Est_CompleteDate = new DateTime(2023, 09, 28),
						BidAmount = random.Next(10000, 90000),
						ClientId = context.Clients.FirstOrDefault(c=>c.Name== "Toronto Hotel").ID

					},
					new Project
					{
						BidDate = new DateTime(2023, 06, 04),
						ProjectSite = "Patients recovery Garden",
						Est_BeginDate = new DateTime(2023, 06, 11),
						Est_CompleteDate = new DateTime(2023, 07, 23),
						BidAmount = random.Next(10000, 90000),
						ClientId = context.Clients.FirstOrDefault(c => c.Name == "Welland Hospital").ID
					},
					new Project
					{
						BidDate = new DateTime(2023, 09, 10),
						ProjectSite = "First Floor center lobby",
						Est_BeginDate = new DateTime(2023, 09, 20),
						Est_CompleteDate = new DateTime(2023, 11, 02),
						BidAmount = random.Next(10000, 90000),
						ClientId = context.Clients.FirstOrDefault(c => c.Name == "Outlet Mall").ID
					},
					new Project
					{
						BidDate = new DateTime(2023, 07, 24),
						ProjectSite = "Center area infornt of Gucci store",
						Est_BeginDate = new DateTime(2023, 08, 6),
						Est_CompleteDate = new DateTime(2023, 09, 15),
						BidAmount = random.Next(10000, 90000),
						ClientId = context.Clients.FirstOrDefault(c => c.Name == "Queen's Mall").ID
					},
					new Project
					{
						BidDate = new DateTime(2023, 09, 10),
						ProjectSite = "First Floor center lobby",
						Est_BeginDate = new DateTime(2023, 09, 20),
						Est_CompleteDate = new DateTime(2023, 11, 02),
						BidAmount = random.Next(10000, 90000),
						ClientId = context.Clients.FirstOrDefault(c => c.Name == "Welland Hotel").ID
					},
					new Project
					{
						BidDate = new DateTime(2023, 08, 24),
						ProjectSite = "Main lobby",
						Est_BeginDate = new DateTime(2023, 09, 4),
						Est_CompleteDate = new DateTime(2023, 09, 15),
						BidAmount = random.Next(10000, 90000),
						ClientId = context.Clients.FirstOrDefault(c => c.Name == "Toronto Hotel").ID
					},
					new Project
					{
						BidDate = new DateTime(2023, 06, 04),
						ProjectSite = "Front desk lobby",
						Est_BeginDate = new DateTime(2023, 06, 10),
						Est_CompleteDate = new DateTime(2023, 07, 02),
						BidAmount = random.Next(10000, 90000),
						ClientId = context.Clients.FirstOrDefault(c => c.Name == "Welland Hospital").ID
					},
					new Project
					{
						BidDate = new DateTime(2023, 11, 10),
						ProjectSite = "Center Area for Christmas Decoration",
						Est_BeginDate = new DateTime(2023, 11, 15),
						Est_CompleteDate = new DateTime(2023, 11, 18),
						BidAmount = random.Next(10000, 90000),
						ClientId = context.Clients.FirstOrDefault(c => c.Name == "Outlet Mall").ID
					},
					new Project
					{
						BidDate = new DateTime(2023, 10, 28),
						ProjectSite = "Center Area for Christmas Decoration",
						Est_BeginDate = new DateTime(2023, 11, 05),
						Est_CompleteDate = new DateTime(2023, 11, 10),
						BidAmount = random.Next(10000, 90000),
						ClientId = context.Clients.FirstOrDefault(c => c.Name == "Queen's Mall").ID
					},
					
					new Project
					{
						BidDate = new DateTime(2023, 10, 30),
						ProjectSite = "Entrance Garden for Christmas Decoration",
						Est_BeginDate = new DateTime(2023, 11, 07),
						Est_CompleteDate = new DateTime(2023, 11, 11),
						BidAmount = random.Next(10000, 90000),
						ClientId = context.Clients.FirstOrDefault(c => c.Name == "Welland Hotel").ID
					}
					);
					context.SaveChanges();
				}


			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.GetBaseException().Message);
			}
		}

	}
}
