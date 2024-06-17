using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb5_affarssystem_api
{
    internal class ShoppingCart
    {
        public List<Product> ShoppingCartList { get; set; } = new List<Product>();
        public decimal PriceTotal { get; set; }

        private MainPage mainPage;
        private static ShoppingCart instance;

        private ShoppingCart(MainPage mainP)
        {
            mainPage = mainP;
        }

        //Make class a Singleton
        public static ShoppingCart GetInstance(MainPage mainPage)
        {
            if (instance == null)
            {
                instance = new ShoppingCart(mainPage);
            }
            return instance;
        }

        //Buy products in shopping cart and decrease quantity for the bought products in product list
        public void Buy_Shoppingcart()
        {
            foreach (var shoppingCartItem in ShoppingCartList)
            {
                //Find the matching product in product list based on ID
                var matchingProduct = Product.ProductList.FirstOrDefault(p => p.Id == shoppingCartItem.Id);

                if (matchingProduct != null)
                {
                    //Decrease quantity on product list with the quantity on shopping list
                    Product.ProductList.FirstOrDefault(p => p.Id == shoppingCartItem.Id).Quantity -= shoppingCartItem.Quantity;

                    //If quantity reach under 0 set quantity to 0
                    if (matchingProduct.Quantity < 0)
                    {
                        matchingProduct.Quantity = 0;
                    }
                }
            }
            //Update view
            ShoppingCartList.Clear();
            PriceTotal = 0;
            mainPage.Update_Product_List();
            mainPage.Update_Shoppingcart();
            mainPage.Popup_Msg("Köp av produkter i varukorg genomförd.");

        }
    }
}
