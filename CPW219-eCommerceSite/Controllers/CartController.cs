using CPW219_eCommerceSite.Data;
using CPW219_eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace CPW219_eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        private readonly VideoGameContext _context;

        public CartController(VideoGameContext context)
        {
            _context = context;
        }

        public IActionResult Add(int id)
        {
            Game? gameToAdd = _context.Games.Where(g => g.GameID == id).SingleOrDefault();

            if(gameToAdd == null)
            {
                TempData["Message"] = "Sorry that game no longer exists";
                return RedirectToAction("Index", "Games");
            }

            TempData["Message"] = "Item added to cart";
            return RedirectToAction("Index", "Games");
        }
    }
}
