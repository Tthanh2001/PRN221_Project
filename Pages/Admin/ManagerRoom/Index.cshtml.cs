using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Project.Utils;
using PRN221_Project.Models;

namespace PRN221_Project.Pages.Admin.ManagerRoom
{
    public class IndexModel : PageModel
    {
        private readonly CinphileDbContext _context;

        [BindProperty]
        public List<Room> rooms { get; set; }

        [BindProperty]
        public Room AddRoom { get; set; }

        public IndexModel(CinphileDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            rooms = _context.Rooms.ToList();
        }
        public IActionResult OnPost()
        {
            string roomName = Request.Form["RoomName"];
            int NumberOfRows = int.Parse(Request.Form["NumberOfRows"]);
            int NumberOfCols = int.Parse(Request.Form["NumberOfCols"]);
            Room room = new Room();
            Room Addroom = new Room();
            room = GetRoomByName(roomName);
            if (room != null)
            {
                ViewData["msg"] = "Không được đặt tên trùng Room";
                OnGet();
                return Page();

            }
            else
            {
                Addroom.RoomName = roomName;
                Addroom.NumberOfRows = NumberOfRows;
                Addroom.NumberOfCols = NumberOfCols;
                _context.Add(Addroom);
                _context.SaveChanges();
                OnGet();
                return Page();
            }
        }
        public IActionResult OnPostDelete(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
            OnGet();
            return Page();
        }
        public Room GetRoomByName(string name)
        {
            Room room = new Room();
            room = _context.Rooms.FirstOrDefault(x => x.RoomName.Equals(name));
            return room;
        }
    }
}
