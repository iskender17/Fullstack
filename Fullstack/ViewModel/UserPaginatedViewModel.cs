using Fullstack.Models;

namespace Fullstack.ViewModel
{
    public class UserPaginatedViewModel
    {
        public IEnumerable<User> Users { get; set; }

        public int FirstRowNumber { get; set; }
        public int LastRowNumber { get; set; }
        public int TotalPageNumber { get; set; }
        public int ActivePageNumber { get; set; }
        public int TotalRowsNumber { get; set; }
        public bool PreviousPage { get; set; }
        public bool NextPage { get; set; }
        public bool FirstPage { get; set; }
        public bool LastPage { get; set; }

        
    }
}
