using NBDProjectNcstech.Models;

namespace NBDProjectNcstech.Utilities
{
    public class PaginatedHomeLists
    {
        public PaginatedList<Client> PagedClients { get; set; }
        public PaginatedList<Project> PagedProjects { get; set; }
    }
}
