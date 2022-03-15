using CPW219_eCommerceSite.Data;
using CPW219_eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CPW219_eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        private readonly VideoGameContext _context;
        private const string Cart = "ShoppingCart";

        public CartController(VideoGameContext context)
        {
            _context = context;
        }

        public IActionResult Add(int id)
        {
            Game? gameToAdd = _context.Games.Where(g => g.GameID == id).SingleOrDefault();

            if (gameToAdd == null)
            {
                TempData["Message"] = "Sorry that game no longer exists";
                return RedirectToAction("Index", "Games");
            }

            CartGameViewModel cartGame = new()
            {
                GameId = gameToAdd.GameID,
                Title = gameToAdd.Title,
                Price = gameToAdd.Price
            };

            List<CartGameViewModel> cartGames = GetExistingCartData();
            cartGames.Add(cartGame);
            WriteShoppingCartCookie(cartGames);

            TempData["Message"] = "Item added to cart";
            return RedirectToAction("Index", "Games");
        }

        private void WriteShoppingCartCookie(List<CartGameViewModel> cartGames)
        {
            string cookieData = JsonConvert.SerializeObject(cartGames);

            HttpContext.Response.Cookies.Append("ShoppingCart", cookieData, new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddYears(1)
            });
        }

        /// <summary>
        /// Return the current list of video games in the users shopping
        /// cart cookie. If there is no cookie, an empty list will be returned
        /// </summary>
        /// <returns></returns>
        private List<CartGameViewModel> GetExistingCartData()
        {
            string? cookie = HttpContext.Request.Cookies[Cart];

            if (string.IsNullOrEmpty(cookie))
            {
                return new List<CartGameViewModel>();
            }

            return JsonConvert.DeserializeObject<List<CartGameViewModel>>(cookie);
        }

        public IActionResult Summary()
        {
            // Read shopping cart data and convert to list of view model
            List<CartGameViewModel> cartGames = GetExistingCartData();

            return View(cartGames);
        }

        public IActionResult Remove(int id)
        {
            List<CartGameViewModel> cartGames = GetExistingCartData();

            CartGameViewModel? targetGame = cartGames.FirstOrDefault(g => g.GameId == id);

            cartGames.Remove(targetGame);
            
            WriteShoppingCartCookie(cartGames);

            return RedirectToAction("Summary");
        }
    }
}
