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

                //Item Types

                if (!context.ItemTypes.Any())
                {
                    context.ItemTypes.AddRange(
                        new ItemType { Name = "Plants" },
                        new ItemType { Name = "Pottery" },
                        new ItemType { Name = "Materials" }
                    );
					context.SaveChanges();
				}
                //Inventory 
                if (!context.Inventory.Any())
                {
                    context.Inventory.AddRange(
                        //Pottery
                        new Inventory
                        {
                            Name = "granite Fountain",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Pottery").ID,
                            Description = "granite Fountain",
                            Size = "48",
                            CostPrice = 750
                        },
                        new Inventory
                        {
                            Name = "Concret Ern",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Pottery").ID,
                            Description = "Concret Ern",
                            Size = "73",
                            CostPrice = 195

                        },
                        new Inventory
                        {
                            Name = "granite Pots",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Pottery").ID,
                            Description = "granite Pots",
                            Size = "50",
                            CostPrice = 195
                        },

                        new Inventory
                        {
                            Name = "Wooden pots",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Pottery").ID,
                            Description = "Wooden pots",
                            Size = "48",
                            CostPrice = 100

                        },

                        new Inventory
                        {
                            Name = "caryota mitis",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Plants").ID,
                            Description = "caryota mitis",
                            Size = "7",
                            CostPrice = 233
                        },

                        new Inventory
                        {
                            Name = "marginata",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Plants").ID,
                            Description = "marginata",
                            Size = "15",
                            CostPrice = 75
                        },

                        new Inventory
                        {
                            Name = "laccospadix australasica palm",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Plants").ID,
                            Description = "laccospadix australasica palm",
                            Size = "7",
                            CostPrice = 749

                        },

                        new Inventory
                        {
                            Name = "Blue Fescue",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Plants").ID,
                            Description = "Blue Fescue",
                            Size = "2",
                            CostPrice = 860

                        },

                        new Inventory
                        {
                            Name = "Top Soil",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Materials").ID,
                            Description = "Top Soil",
                            Size = "15",
                            CostPrice = 15.97M
                        },

                        new Inventory
                        {
                            Name = "Mulch",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Materials").ID,
                            Description = "Mulch",
                            Size = "10",

                            CostPrice = 20
                        },

                        new Inventory
                        {
                            Name = "decorative cedar bark (CBRK5)",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Materials").ID,
                            Description = "decorative cedar bark (CBRK5)",
                            Size = "5",
                            CostPrice = 15.95M
                        },

                        new Inventory
                        {
                            Name = "Screenings",
                            TypeID = context.ItemTypes.FirstOrDefault(c => c.Name == "Materials").ID,
                            Description = "Screenings",
                            Size = "5",
                            CostPrice = 20
                        }

                    );
					context.SaveChanges();
				}

                //Material Requriments
                if (!context.MaterialRequirments.Any())
                {
                    var materialRequirments = new List<MaterialRequirments>()
                    {
                      new MaterialRequirments
                      {
                          InventoryID = context.Inventory.FirstOrDefault(c => c.Name=="Screenings").ID,
                          //Quanity 
                          
                      }



                    };
                }

                //Provinces 
                if (!context.Provinces.Any())
                if (!context.Provinces.Any())
                {
                    var provinces = new List<Province>
                    {
                        new Province { ID = "ON", Name = "Ontario"},
                        new Province { ID = "PE", Name = "Prince Edward Island"},
                        new Province { ID = "NB", Name = "New Brunswick"},
                        new Province { ID = "BC", Name = "British Columbia"},
                        new Province { ID = "NL", Name = "Newfoundland and Labrador"},
                        new Province { ID = "SK", Name = "Saskatchewan"},
                        new Province { ID = "NS", Name = "Nova Scotia"},
                        new Province { ID = "MB", Name = "Manitoba"},
                        new Province { ID = "QC", Name = "Quebec"},
                        new Province { ID = "YT", Name = "Yukon"},
                        new Province { ID = "NU", Name = "Nunavut"},
                        new Province { ID = "NT", Name = "Northwest Territories"},
                        new Province { ID = "AB", Name = "Alberta"}
                    };
                    context.Provinces.AddRange(provinces);
                    context.SaveChanges();
                }
                //cities
                if (!context.Cities.Any())
                {
                    // Cities in Ontario
                    string[] ONCities = new string[] { "Toronto", "Mississauga", "Ottawa", "Hamilton", "London", "Kitchener", "Brampton", "Markham", "Vaughan", "Windsor", "Welland", "Niagara Falls" };

                    // Cities in Quebec
                    string[] QCCities = new string[] { "Montreal", "Quebec City", "Laval", "Gatineau", "Longueuil", "Sherbrooke", "Saguenay", "Levis", "Trois-Rivieres", "Terrebonne" };

                    // Cities in British Columbia
                    string[] BCCities = new string[] { "Vancouver", "Surrey", "Burnaby", "Richmond", "Abbotsford", "Kelowna", "Victoria", "Langley", "Coquitlam", "Saanich" };

                    // Cities in Alberta
                    string[] ABCities = new string[] { "Calgary", "Edmonton", "Red Deer", "Lethbridge", "Medicine Hat", "Fort McMurray", "Grande Prairie", "Airdrie", "Spruce Grove", "Leduc" };

                    // Cities in Manitoba
                    string[] MMBCities = new string[] { "Winnipeg", "Brandon", "Steinbach", "Thompson", "Portage la Prairie", "Selkirk", "Winkler", "Dauphin", "Morden", "The Pas" };

                    // Cities in Nova Scotia
                    string[] NSCities = new string[] { "Halifax", "Sydney", "Truro", "New Glasgow", "Yarmouth", "Kentville", "Amherst", "Bridgewater", "Dartmouth", "Wolfville" };

                    // Cities in Prince Edward Island
                    string[] PECities = new string[] { "Charlottetown", "Summerside", "Stratford", "Cornwall", "Montague", "Kensington", "Souris", "Alberton", "Tignish", "O'Leary" };

                    // Cities in Newfoundland and Labrador
                    string[] NLcities = new string[] { "St. John's", "Mount Pearl", "Corner Brook", "Grand Falls-Windsor", "Gander", "Labrador City", "Stephenville", "Marystown" };

                    // Define a dictionary to map province codes to city arrays
                    Dictionary<string, string[]> provinceCities = new Dictionary<string, string[]>()
                    {
                        { "ON", ONCities },
                        { "QC", QCCities },
                        { "BC", BCCities },
                        { "AB", ABCities },
                        { "MB", MMBCities },
                        { "NS", NSCities },
                        { "PE", PECities },
                        { "NL", NLcities }
                    };

                    // Loop through the dictionary and add cities to the context
                    foreach (var pc in provinceCities)
                    {
                        string provinceID = pc.Key;
                        string[] cities = pc.Value;

                        foreach (string cityName in cities)
                        {
                            City city = new City()
                            {
                                Name = cityName,
                                ProvinceID = provinceID
                            };
                            context.Cities.Add(city);
                        }
                    }

                    // Save changes to the database
                    context.SaveChanges();

                }

                //For Regular workeres
                string[] regularPositions = new string[] { "Designer", "Sales Associate", "Laborer", "Gardener", "Driver", "Botanist", "Equipment Operator"};
                //For Keri, Stan and other managers
                string[] importantPositions = new string[] {"Group Manager","Co-Owner","General Manager","Production Group Manager", "Administrative Assistant"};

                string[] combinedPosiitions = regularPositions.Concat(importantPositions).ToArray();

                if (!context.StaffPositions.Any())
                {
                    foreach (string s in combinedPosiitions)
                    {
                        StaffPosition r = new StaffPosition
                        {
                            PositionName = s
                        };
                        context.StaffPositions.Add(r);
                    }
                    context.SaveChanges();
                }

                //Seed data for Staff
                string[] firstNames = new string[] { "Lyric", "Antoinette", "Kendal", "Vivian", "Ruth", "Jamison", "Emilia", "Natalee", "Yadiel", "Jakayla", "Lukas", "Moses", "Kyler", "Karla", "Chanel", "Tyler", "Camilla", "Quintin", "Braden", "Clarence" };
                string[] lastNames = new string[] { "Watts", "Randall", "Arias", "Weber", "Stone", "Carlson", "Robles", "Frederick", "Parker", "Morris", "Soto", "Bruce", "Orozco", "Boyer", "Burns", "Cobb", "Blankenship", "Houston", "Estes", "Atkins", "Miranda", "Zuniga", "Ward", "Mayo", "Costa", "Reeves", "Anthony", "Cook", "Krueger", "Crane", "Watts", "Little", "Henderson", "Bishop" };
                int firstNamesCount = firstNames.Length;
                int lastNamesCount = lastNames.Length;
                int count = 0;
                if (!context.Staffs.Any())
                {
                    while (count < lastNamesCount)
                    {
                        Staff r = new Staff
                        {
                            FullName = $"{firstNames[random.Next(0, firstNamesCount)]} {lastNames[random.Next(0, lastNamesCount)]}",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            StaffPositionID = random.Next(1, regularPositions.Length + 1)
                        };
                        context.Staffs.Add(r);
                        count++;
                    }

                    //Staff that has been listed in PDF
                    context.Staffs.AddRange(
                        new Staff
                        {
                            FullName = "Keri Yamaguchi",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            StaffPositionID = context.StaffPositions.FirstOrDefault(s => s.PositionName == "Co-Owner").ID
                        },
                        new Staff
                        {
                            FullName = "Stan Fenton",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            StaffPositionID = context.StaffPositions.FirstOrDefault(s => s.PositionName == "Co-Owner").ID
                        },
                        new Staff
                        {
                            FullName = "Connie Nguyen",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            StaffPositionID = context.StaffPositions.FirstOrDefault(s => s.PositionName == "Administrative Assistant").ID
                        },
                        new Staff
                        {
                            FullName = "Cheryl Poy",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            StaffPositionID = context.StaffPositions.FirstOrDefault(s => s.PositionName == "Group Manager").ID
                        },
                        new Staff
                        {
                            FullName = "Bob Reinhardt",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            StaffPositionID = context.StaffPositions.FirstOrDefault(s => s.PositionName == "Sales Associate").ID
                        },
                        new Staff
                        {
                            FullName = "Tamara Bakken",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            StaffPositionID = context.StaffPositions.FirstOrDefault(s => s.PositionName == "Designer").ID
                        },
                        new Staff
                        {
                            FullName = "Sue Kaufman",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            StaffPositionID = context.StaffPositions.FirstOrDefault(s => s.PositionName == "Production Group Manager").ID
                        },
                        new Staff
                        {
                            FullName = "Monica Goce",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            StaffPositionID = context.StaffPositions.FirstOrDefault(s => s.PositionName == "Laborer").ID
                        },
                        new Staff
                        {
                            FullName = "Bert Swenson",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            StaffPositionID = context.StaffPositions.FirstOrDefault(s => s.PositionName == "Laborer").ID
                        },
                        new Staff
                        {
                            FullName = "Jerry",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            StaffPositionID = context.StaffPositions.FirstOrDefault(s => s.PositionName == "Equipment Operator").ID
                        }
                        );
                    context.SaveChanges();

                    
                }

                //string[] Names = {"Gagan Shrestha", "Josh Rydzpol Adlhaie Binalay", "Josh Martin", "Femarie Vien Briones", "Olubusiyi Olorungbemi Famobiwo" };
                if (!context.Clients.Any())
                {
                    context.Clients.AddRange(
                        new Client
                        {
                            Name = "Toronto Hotel",
                            ContactPerson = "Gagan Shrestha",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            Street = "123 Lincon street",
                            CityID = context.Cities.FirstOrDefault(c => c.Name == "Toronto").ID,
                            PostalCode = "L2O 1B4"

                        },
                        new Client
                        {
                            Name = "Outlet Mall",
                            ContactPerson = "Josh Rydzpol Adlhaie Binalay",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            Street = "124 Great Ave.",
                            CityID = context.Cities.FirstOrDefault(c => c.Name == "Niagara Falls").ID,
                            PostalCode = "L2O 1B4"
                        },
                        new Client
                        {
                            Name = "Welland Hospital",
                            ContactPerson = "Josh Martin",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            Street = "924 Heavenly Street",
                            CityID = context.Cities.FirstOrDefault(c => c.Name == "Welland").ID,
                            PostalCode = "E2O 1C4"
                        },
                        new Client
                        {
                            Name = "Queen's Mall",
                            ContactPerson = "Femarie Vien Briones",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            Street = "24 Fairy Street",
                            CityID = context.Cities.FirstOrDefault(c => c.Name == "Hamilton").ID,
                            PostalCode = "H2O 2C4"
                        },
                        new Client
                        {
                            Name = "Welland Hotel",
                            ContactPerson = "Olubusiyi Olorungbemi Famobiwo",
                            Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                            Street = "24 Victoria Ave.",
                            CityID = context.Cities.FirstOrDefault(c => c.Name == "Welland").ID,
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
                        ClientId = context.Clients.FirstOrDefault(c => c.Name == "Toronto Hotel").ID

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
                        ProjectSite = "Second Floor center lobby",
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
                        ProjectSite = "Clubhouse, Resort",
                        Est_BeginDate = new DateTime(2023, 11, 05),
                        Est_CompleteDate = new DateTime(2023, 11, 10),
                        BidAmount = random.Next(10000, 90000),
                        ClientId = context.Clients.FirstOrDefault(c => c.Name == "Queen's Mall").ID
                    },

                    new Project
                    {
                        BidDate = new DateTime(2023, 10, 30),
                        ProjectSite = "Mall Entrance",
                        Est_BeginDate = new DateTime(2023, 11, 07),
                        Est_CompleteDate = new DateTime(2023, 11, 11),
                        BidAmount = random.Next(10000, 90000),
                        ClientId = context.Clients.FirstOrDefault(c => c.Name == "Welland Hotel").ID
                    }
                    );
                    context.SaveChanges();
                }

                int[] projectIDs = context.Projects.Select(d => d.Id).ToArray();
                int projectIDCount = projectIDs.Length;

                if (!context.DesignBids.Any())
                {
                    context.DesignBids.AddRange(
                        new DesignBid
                        {
                            ProjectID = context.Projects.FirstOrDefault(p => p.ProjectSite == "Main Entrance Garden").Id
                        },
                        new DesignBid
                        {
                            ProjectID = context.Projects.FirstOrDefault(p => p.ProjectSite == "Patients recovery Garden").Id
                        },
                        new DesignBid
                        {
                            ProjectID = context.Projects.FirstOrDefault(p => p.ProjectSite == "First Floor center lobby").Id
                        },
                        new DesignBid
                        {
                            ProjectID = context.Projects.FirstOrDefault(p => p.ProjectSite == "Second Floor center lobby").Id
                        },
                        new DesignBid
                        {
                            ProjectID = context.Projects.FirstOrDefault(p => p.ProjectSite == "Main lobby").Id
                        },
                        new DesignBid
                        {
                            ProjectID = context.Projects.FirstOrDefault(p => p.ProjectSite == "Front desk lobby").Id
                        },
                        new DesignBid
                        {
                            ProjectID = context.Projects.FirstOrDefault(p => p.ProjectSite == "Center Area for Christmas Decoration").Id
                        },
                        new DesignBid
                        {
                            ProjectID = context.Projects.FirstOrDefault(p => p.ProjectSite == "Clubhouse, Resort").Id
                        },
                        new DesignBid
                        {
                            ProjectID = context.Projects.FirstOrDefault(p => p.ProjectSite == "Mall Entrance").Id
                        },
                        new DesignBid
                        {
                            ProjectID = context.Projects.FirstOrDefault(p => p.ProjectSite == "Entrance Garden for Christmas Decoration").Id
                        }
                        );

                    context.SaveChanges();
                }

                if (!context.DesignBidStaff.Any())
                {
                    context.DesignBidStaff.AddRange(
                        new DesignBidStaff
                        {
                            DesignBidID = context.DesignBids.FirstOrDefault(d => d.ID == 1).ID,
                            StaffID = context.Staffs.FirstOrDefault(d => d.FullName == "Tamara Bakken").ID
                        },
                        new DesignBidStaff
                        {
                            DesignBidID = context.DesignBids.FirstOrDefault(d => d.ID == 2).ID,
                            StaffID = context.Staffs.FirstOrDefault(d => d.FullName == "Bert Swenson").ID
                        },
                        new DesignBidStaff
                        {
                            DesignBidID = context.DesignBids.FirstOrDefault(d => d.ID == 3).ID,
                            StaffID = context.Staffs.FirstOrDefault(d => d.FullName == "Monica Goce").ID
                        },
                        new DesignBidStaff
                        {
                            DesignBidID = context.DesignBids.FirstOrDefault(d => d.ID == 4).ID,
                            StaffID = context.Staffs.FirstOrDefault(d => d.FullName == "Bert Swenson").ID
                        },
                        new DesignBidStaff
                        {
                            DesignBidID = context.DesignBids.FirstOrDefault(d => d.ID == 5).ID,
                            StaffID = context.Staffs.FirstOrDefault(d => d.FullName == "Tamara Bakken").ID
                        },
                        new DesignBidStaff
                        {
                            DesignBidID = context.DesignBids.FirstOrDefault(d => d.ID == 6).ID,
                            StaffID = context.Staffs.FirstOrDefault(d => d.FullName == "Tamara Bakken").ID
                        },
                        new DesignBidStaff
                        {
                            DesignBidID = context.DesignBids.FirstOrDefault(d => d.ID == 7).ID,
                            StaffID = context.Staffs.FirstOrDefault(d => d.FullName == "Tamara Bakken").ID
                        },
                        new DesignBidStaff
                        {
                            DesignBidID = context.DesignBids.FirstOrDefault(d => d.ID == 8).ID,
                            StaffID = context.Staffs.FirstOrDefault(d => d.FullName == "Tamara Bakken").ID
                        },
                        new DesignBidStaff
                        {
                            DesignBidID = context.DesignBids.FirstOrDefault(d => d.ID == 9).ID,
                            StaffID = context.Staffs.FirstOrDefault(d => d.FullName == "Tamara Bakken").ID
                        },
                        new DesignBidStaff
                        {
                            DesignBidID = context.DesignBids.FirstOrDefault(d => d.ID == 10).ID,
                            StaffID = context.Staffs.FirstOrDefault(d => d.FullName == "Tamara Bakken").ID
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
