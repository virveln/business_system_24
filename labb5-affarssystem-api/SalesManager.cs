using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System.Xml.Linq;
using Windows.UI.Notifications;

namespace labb5_affarssystem_api
{
    internal class SalesManager
    {
        private ShoppingCart shoppingCart;
        private MainPage mainPage;

        public SalesManager(MainPage mainPage)
        {
            shoppingCart = ShoppingCart.GetInstance(mainPage);
            this.mainPage = mainPage;
        }

        //Add product to shopping cart from sales product list
        public void Add_To_Cart(Product clickedProduct)
        {
            //check if product already exists in shopping cart, if so quantity and price is add up
            var existingProductInCart = shoppingCart.ShoppingCartList.FirstOrDefault(p => p.Id == clickedProduct.Id);
            if (existingProductInCart != null)
            {
                //Find the matching product in product list based on ID, to check quantity of product
                var stockProduct = Product.ProductList.FirstOrDefault(p => p.Id == existingProductInCart.Id);
                if ((existingProductInCart.Quantity + 1) > stockProduct.Quantity)
                {
                    mainPage.Error_Msg("", "Tyvärr, varans lagerstatus är 0 och kan inte läggas i varukorgen mer.");
                }
                else
                {
                    existingProductInCart.Quantity++;
                    existingProductInCart.Price += clickedProduct.Price;
                    shoppingCart.PriceTotal += clickedProduct.Price;
                    mainPage.Popup_Msg("Produkten '" + clickedProduct.Name + "' lades i varukorgen.");
                }
            }
            //if product not already extists in shopping cart, add it to list based on type and increase total price
            else
            {
                dynamic newProduct = "";
                if (clickedProduct.GetType() == typeof(Book))
                {
                    var book = clickedProduct as Book;
                    newProduct = new Book(book.Category, 1, book.Id, book.Name, book.Price, book.Author, book.Genre, book.Genre, book.Language);
                    shoppingCart.PriceTotal += book.Price;
                }
                else if (clickedProduct.GetType() == typeof(Movie))
                {
                    var movie = clickedProduct as Movie;
                    if (movie.Duration.HasValue)
                        newProduct = new Movie(movie.Category, 1, movie.Id, movie.Name, movie.Price, movie.Format, (int)movie.Duration);
                    else
                        newProduct = new Movie(movie.Category, 1, movie.Id, movie.Name, movie.Price, movie.Format, 0);

                    shoppingCart.PriceTotal += movie.Price;
                }
                else if (clickedProduct.GetType() == typeof(Videogame))
                {
                    var videogame = clickedProduct as Videogame;
                    newProduct = new Videogame(videogame.Category, 1, videogame.Id, videogame.Name, videogame.Price, videogame.Platform);
                    shoppingCart.PriceTotal += videogame.Price;
                }
                shoppingCart.ShoppingCartList.Add(newProduct);
                mainPage.Popup_Msg("Produkten '" + newProduct.Name + "' tillagd i varukorgen.");
            }
            mainPage.Update_Shoppingcart();
        }
    }
}
