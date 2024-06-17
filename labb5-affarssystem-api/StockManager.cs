using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using System.Diagnostics;
using System.ServiceModel.Channels;
using Windows.UI.Notifications;

namespace labb5_affarssystem_api
{
    internal class StockManager
    {
        public bool CorrectInput { get; set; }
        private MainPage mainPage;

        public StockManager(MainPage mainPage)
        {
            CorrectInput = false;
            this.mainPage = mainPage;
        }

        //Increase quantity of clicked product if input is a positive int
        public async void Add_Quantity_Product(Product clickedProduct)
        {
            //Dialog for user input of quantity
            var inputDialog = new ContentDialog
            {
                Title = $"Hur många '{clickedProduct.Name}' vill du beställa?",
                PrimaryButtonText = "Beställ antal",
                SecondaryButtonText = "Avbryt"
            };

            var inputQuantity = new TextBox();
            inputDialog.Content = inputQuantity;

            ContentDialogResult result = await inputDialog.ShowAsync();

            //When clicked on enter button, checking id correct input
            if (result == ContentDialogResult.Primary)
            {
                if (int.TryParse(inputQuantity.Text, out var quantity) && quantity > 0)
                {
                    clickedProduct.Quantity += int.Parse(inputQuantity.Text);
                    mainPage.Update_Product_List();
                    mainPage.Update_Sales_Product_List();
                    mainPage.Popup_Msg("Beställde " + quantity + "st av produkten '" + clickedProduct.Name + "'.");
                }
                else
                {
                    //If wrong input, error message in same style as dialog
                    var errorDialog = new ContentDialog
                    {
                        Title = "Felaktig input",
                        Content = "Ange ett positivt nummer som antal.",
                        CloseButtonText = "OK"
                    };
                    await errorDialog.ShowAsync();
                    Add_Quantity_Product(clickedProduct);
                }
            }
        }

        //Delete product if quantity is 0 when desired, otherwise ask if user still want to delete product
        public async void Delete_Product(Product clickedProduct)
        {
            if (clickedProduct.Quantity != 0)
            {
                var msgDialog = new MessageDialog("Är du säker på att du vill ta bort produkten ur systemet?");
                msgDialog.Commands.Add(new UICommand("Ja"));
                msgDialog.Commands.Add(new UICommand("Avbryt"));

                var result = await msgDialog.ShowAsync();

                if (result.Label == "Ja")
                {
                    Product.ProductList.Remove(clickedProduct);
                }
            }
            else
            {
                Product.ProductList.Remove(clickedProduct);
                mainPage.Popup_Msg("Produkten '" + clickedProduct.Name + "' borttagen från lagersystemet.");
            }
            mainPage.Update_Product_List();
            mainPage.Update_Sales_Product_List();
        }

        //Add new product in system with correct inputs
        public void Submit_New_Product(string selectedOption, string productId, string productName, string productPrice, string productAuthor, string productGenre, string productBookFormat, string productLanguage, string productMovieFormat, string productDuration, string productPlatform)
        {
            try
            {
                //Check if id is correctly filled, otherwise throw exception
                if (!string.IsNullOrEmpty(productId))
                {
                    if (int.TryParse(productId, out int checkUniqueId))
                    {
                        if (checkUniqueId <= 0)
                            throw new ArgumentOutOfRangeException(nameof(productId));
                        foreach (var item in Product.ProductList)
                        {
                            if (item.Id == checkUniqueId)
                                throw new InvalidOperationException(nameof(productId));
                        }
                    }
                    else
                        throw new FormatException(nameof(productId));
                }
                else
                    throw new ArgumentNullException(nameof(productId));

                //Check if name is correctly filled, otherwise throw exception
                if (string.IsNullOrEmpty(productName))
                    throw new ArgumentNullException(nameof(productName));

                //Check if price is correctly filled, otherwise throw exception
                if (!string.IsNullOrEmpty(productPrice))
                {
                    if (decimal.TryParse(productPrice, out decimal price))
                    {
                        if (price <= 0)
                            throw new ArgumentOutOfRangeException(nameof(productPrice));
                    }
                    else
                        throw new FormatException(nameof(productPrice));
                }
                else
                    throw new ArgumentNullException(nameof(productPrice));

                //Check if duration is correctly filled, otherwise throw exception
                if (selectedOption == "cbMovie")
                {
                    if (!string.IsNullOrEmpty(productDuration))
                    {

                        if (int.TryParse(productDuration, out int duration))
                        {
                            if (duration <= 0)
                                throw new ArgumentOutOfRangeException(nameof(productDuration));
                        }
                        else
                        {
                            throw new FormatException(nameof(productDuration));
                        }
                    }
                }

                //If correct input, add new product
                dynamic newProduct = "";
                if (selectedOption == "cbBook")
                {
                    newProduct = new Book(typeof(Book).Name, 0, int.Parse(productId), productName, decimal.Parse(productPrice), productAuthor, productGenre, productBookFormat, productLanguage);
                }
                else if (selectedOption == "cbMovie")
                {
                    if (!string.IsNullOrEmpty(productDuration))
                        newProduct = new Movie(typeof(Movie).Name, 0, int.Parse(productId), productName, decimal.Parse(productPrice), productMovieFormat, int.Parse(productDuration));
                    else
                        newProduct = new Movie(typeof(Movie).Name, 0, int.Parse(productId), productName, decimal.Parse(productPrice), productMovieFormat, 0);
                }
                else if (selectedOption == "cbVideogame")
                {
                    newProduct = new Videogame(typeof(Videogame).Name, 0, int.Parse(productId), productName, decimal.Parse(productPrice), productPlatform);
                }
                Product.ProductList.Add(newProduct);
                mainPage.Update_Product_List();
                CorrectInput = true;
                mainPage.Popup_Msg("Produkten '" + productName + "' tillaggd i lagersystemet.");
            }
            catch (FormatException ex)
            {
                mainPage.Error_Msg(ex.Message, "Du måste fylla i siffror i fältet.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                mainPage.Error_Msg(ex.ParamName, "Du måste fylla i positiva siffror i fältet.");
            }
            catch (ArgumentNullException ex)
            {
                mainPage.Error_Msg(ex.ParamName, "Varunummer, namn och pris är obligatoriska fält.");
            }
            catch (InvalidOperationException ex)
            {
                mainPage.Error_Msg(ex.Message, "Du måste fylla i ett varunummer som inte redan finns i systemet.");
            }
            catch (Exception ex)
            {
                mainPage.Error_Msg("", "Ett fel inträffade, försök igen om en stund.");
            }
        }
      
        //Sync local stock with API stock
        public async Task Sync_Stock_From_API()
        {
            //Get a list of products from API
            List<Product> productsFromApi = await ApiManager.Get_Api_Products();
            
            if (productsFromApi != null)
            {
                //Update stock status for each product
                foreach (var apiProduct in productsFromApi)
                {
                    //Find the local product in product list based on ID
                    var localProduct = Product.ProductList.FirstOrDefault(p => p.Id == apiProduct.Id);

                    //Update product price and quantity if it already exists in local stock
                    if (localProduct != null)
                    {
                        localProduct.Quantity = apiProduct.Quantity;
                        localProduct.Price = apiProduct.Price;
                    }
                    //If no current product is found, the product is added to the local stock
                    else
                    {
                        Product.ProductList.Add(apiProduct);
                    }
                }       
            }
            else
            {
                Debug.WriteLine("No products returned from API.");
            }
        }
    }
}
