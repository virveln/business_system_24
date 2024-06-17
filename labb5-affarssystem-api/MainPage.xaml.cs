using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition.Interactions;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;
using Windows.UI.Notifications;
using Windows.ApplicationModel.Chat;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

//Jessica Hvirfvel, grundversion

namespace labb5_affarssystem_api
{
    public sealed partial class MainPage : Page
    {
        private string newProductSelectedType;

        private FileManager fileManager;
        private SalesManager salesManager;
        private StockManager stockManager;
        private ShoppingCart shoppingCart;

        public MainPage()
        {
            this.InitializeComponent();

            salesManager = new SalesManager(this);
            stockManager = new StockManager(this);
            shoppingCart = ShoppingCart.GetInstance(this);
            fileManager = new FileManager();

            Initiate_Product_Lists();

            addNewProductCategory.SelectedIndex = 0;
        }

        //To make sure program read through file before updating view of product list
        public async void Initiate_Product_Lists()
        {
            await fileManager.OpenFile();

            Update_Product_List();
            Update_Sales_Product_List();
        }

        //Error message when error occurs
        public async void Error_Msg(string field, string msg)
        {
            var msgDialog = new MessageDialog(msg);
            await msgDialog.ShowAsync();
            Focus(field);
        }

        //Pop up message when an action is performed
        public void Popup_Msg(string msgText)
        {
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            var toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(msgText));
            var toast = new ToastNotification(toastXml);
            toastNotifier.Show(toast);
        }

        //Focuses on the field wehere the wrong input was made
        public void Focus(string field)
        {
            if (field == "productId")
                newProductId.Focus(FocusState.Programmatic);
            else if (field == "productName")
                newProductName.Focus(FocusState.Programmatic);
            else if (field == "productPrice")
                newProductPrice.Focus(FocusState.Programmatic);
            else if (field == "productDuration")
                newProductDuration.Focus(FocusState.Programmatic);
        }

        //***STOCK TAB***

        //Update product list in stock tab
        public void Update_Product_List()
        {
            // Update list views with products
            stockBookListView.ItemsSource = Product.ProductList.OfType<Book>().ToList();
            stockMovieListView.ItemsSource = Product.ProductList.OfType<Movie>().ToList();
            stockVideogameListView.ItemsSource = Product.ProductList.OfType<Videogame>().ToList();
            fileManager.SaveFile();
        }

        private void Add_Quantity_Product_Click(object sender, RoutedEventArgs e)
        {
            stockManager.Add_Quantity_Product((sender as FrameworkElement).DataContext as Product);
        }

        private void Delete_Product_Click(object sender, RoutedEventArgs e)
        {
            stockManager.Delete_Product((sender as FrameworkElement).DataContext as Product);
        }

        //Change visibility for input fields for combo box for adding new product
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = addNewProductCategory.SelectedItem as ComboBoxItem;
            newProductSelectedType = selectedItem.Name;
            if (newProductSelectedType == "cbBook")
            {
                addNewBook.Visibility = Visibility.Visible;
                addNewVideogame.Visibility = Visibility.Collapsed;
                addNewMovie.Visibility = Visibility.Collapsed;
            }
            else if (newProductSelectedType == "cbMovie")
            {
                addNewMovie.Visibility = Visibility.Visible;
                addNewBook.Visibility = Visibility.Collapsed;
                addNewVideogame.Visibility = Visibility.Collapsed;
            }
            else if (newProductSelectedType == "cbVideogame")
            {
                addNewVideogame.Visibility = Visibility.Visible;
                addNewBook.Visibility = Visibility.Collapsed;
                addNewMovie.Visibility = Visibility.Collapsed;
            }
        }

        private void Submit_New_Product_Click(object sender, RoutedEventArgs e)
        {
            var productId = newProductId.Text;
            var productName = newProductName.Text;
            var productPrice = newProductPrice.Text;
            var productAuthor = newProductAuthor.Text;
            var productGenre = newProductGenre.Text;
            var productBookFormat = newProductBookFormat.Text;
            var productLanguage = newProductLanguage.Text;
            var productMovieFormat = newProductMovieFormat.Text;
            var productDuration = newProductDuration.Text;
            var productPlatform = newProductPlatform.Text;

            //Call method to add new product if input is correct
            stockManager.Submit_New_Product(newProductSelectedType, productId, productName, productPrice, productAuthor, productGenre, productBookFormat, productLanguage, productMovieFormat, productDuration, productPlatform);

            //Empty input field
            if (stockManager.CorrectInput)
            {
                newProductId.Text = "";
                newProductName.Text = "";
                newProductPrice.Text = "";
                newProductAuthor.Text = "";
                newProductGenre.Text = "";
                newProductBookFormat.Text = "";
                newProductLanguage.Text = "";
                newProductMovieFormat.Text = "";
                newProductDuration.Text = "";
                newProductPlatform.Text = "";
                newProductMovieFormat.Text = "";
                stockManager.CorrectInput = false;
            }
        }

        //***SALES TAB***

        //Update product list in sales tab
        public void Update_Sales_Product_List()
        {
            //Filter products with quantity over 0
            var productsQuantOverZero = Product.ProductList.Where(p => p.Quantity > 0).ToList();

            // Update list views with products
            salesBookListView.ItemsSource = productsQuantOverZero.OfType<Book>().ToList();
            salesMovieListView.ItemsSource = productsQuantOverZero.OfType<Movie>().ToList();
            salesVideogameListView.ItemsSource = productsQuantOverZero.OfType<Videogame>().ToList();
        }

        //Update shopping cart
        public void Update_Shoppingcart()
        {
            salesShoppingcartListView.ItemsSource = shoppingCart.ShoppingCartList.ToList();
            totalPrice.Text = shoppingCart.PriceTotal.ToString() + " :-";
        }

        private void Add_To_Cart_Click(object sender, RoutedEventArgs e)
        {
            salesManager.Add_To_Cart((sender as FrameworkElement).DataContext as Product);
        }

        private void Buy_Shoppingcart_Click(object sender, RoutedEventArgs e)
        {
            shoppingCart.Buy_Shoppingcart();
            Update_Sales_Product_List();
        }

        //Sync local stock with API stock
        private async void Sync_Stock_From_API_Click(object sender, RoutedEventArgs e)
        {  
            await stockManager.Sync_Stock_From_API();                
            Update_Product_List();
            Update_Sales_Product_List();
        }
    }
}
