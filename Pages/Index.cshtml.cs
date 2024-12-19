using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace FInal_Exam.Pages
{
    public class Index : PageModel
    {
        private readonly ILogger<Index> _logger;

        public Index(ILogger<Index> logger)
        {
            _logger = logger;
        }

        public List<Room> Rooms { get; set; }

        [BindProperty]
        public SearchParameters? SearchParams { get; set; }

        public void OnGet(string? keyword = "", string? searchBy = "", string? sortBy = null, string? sortAsc = "true", int pageSize = 5, int pageIndex = 1)
        {
            if (SearchParams == null)
            {
                SearchParams = new SearchParameters()
                {
                    SortBy = sortBy,
                    SortAsc = sortAsc == "true",
                    SearchBy = searchBy,
                    Keyword = keyword,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            }

            List<Room> rooms = new List<Room>()
            {
                new Room() { RoomNumber = "101", Type = "Single", Capacity = 1, IsAvailable = true },
                new Room() { RoomNumber = "102", Type = "Double", Capacity = 2, IsAvailable = false },
                new Room() { RoomNumber = "103", Type = "Suite", Capacity = 3, IsAvailable = true },
                new Room() { RoomNumber = "104", Type = "Single", Capacity = 1, IsAvailable = false },
                new Room() { RoomNumber = "105", Type = "Double", Capacity = 2, IsAvailable = true },
                new Room() { RoomNumber = "106", Type = "Suite", Capacity = 3, IsAvailable = true }
            };

            // Search Filter
            if (!string.IsNullOrEmpty(SearchParams.SearchBy) && !string.IsNullOrEmpty(SearchParams.Keyword))
            {
                if (SearchParams.SearchBy.ToLower() == "roomnumber")
                {
                    rooms = rooms.Where(r => r.RoomNumber.ToLower().Contains(SearchParams.Keyword.ToLower())).ToList();
                }
                else if (SearchParams.SearchBy.ToLower() == "type")
                {
                    rooms = rooms.Where(r => r.Type.ToLower().Contains(SearchParams.Keyword.ToLower())).ToList();
                }
                else if (SearchParams.SearchBy.ToLower() == "capacity")
                {
                    rooms = rooms.Where(r => r.Capacity.ToString().Contains(SearchParams.Keyword)).ToList();
                }
                else if (SearchParams.SearchBy.ToLower() == "isavailable")
                {
                    bool isAvailable = SearchParams.Keyword.ToLower() == "true";
                    rooms = rooms.Where(r => r.IsAvailable == isAvailable).ToList();
                }
            }

            // Sorting
            if (SearchParams.SortBy != null && SearchParams.SortAsc != null)
            {
                if (SearchParams.SortBy.ToLower() == "roomnumber")
                {
                    rooms = SearchParams.SortAsc == true ? rooms.OrderBy(r => r.RoomNumber).ToList() : rooms.OrderByDescending(r => r.RoomNumber).ToList();
                }
                else if (SearchParams.SortBy.ToLower() == "type")
                {
                    rooms = SearchParams.SortAsc == true ? rooms.OrderBy(r => r.Type).ToList() : rooms.OrderByDescending(r => r.Type).ToList();
                }
                else if (SearchParams.SortBy.ToLower() == "capacity")
                {
                    rooms = SearchParams.SortAsc == true ? rooms.OrderBy(r => r.Capacity).ToList() : rooms.OrderByDescending(r => r.Capacity).ToList();
                }
                else if (SearchParams.SortBy.ToLower() == "isavailable")
                {
                    rooms = SearchParams.SortAsc == true ? rooms.OrderBy(r => r.IsAvailable).ToList() : rooms.OrderByDescending(r => r.IsAvailable).ToList();
                }
            }

            // Pagination
            int totalRooms = rooms.Count;
            int skip = (pageIndex - 1) * pageSize;
            Rooms = rooms.Skip(skip).Take(pageSize).ToList();

            SearchParams.PageCount = (int)Math.Ceiling((double)totalRooms / pageSize);
            SearchParams.SearchCount = totalRooms;
        }

        public class Room
        {
            public string RoomNumber { get; set; }
            public string Type { get; set; }
            public int Capacity { get; set; }
            public bool IsAvailable { get; set; }
        }

        public class SearchParameters
        {
            public string? SearchBy { get; set; }
            public string? Keyword { get; set; }
            public string? SortBy { get; set; }
            public bool? SortAsc { get; set; }
            public int PageSize { get; set; } = 5;
            public int PageIndex { get; set; } = 1;
            public int? PageCount { get; set; }
            public int? SearchCount { get; set; }
        }
    }
}
