using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using System.Xml.Linq;
using Windows.Media;

namespace labb5_affarssystem_api
{
    internal class ApiManager
    {
        private static HttpClient httpClient = new HttpClient();
        private static string apiUrl = "https://hex.cse.kau.se/~jonavest/csharp-api/";

        public static async Task<List<Product>> Get_Api_Products()
        {
            try
            {
                //Request data from API
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                //If fetch succeded
                if (response.IsSuccessStatusCode)
                {
                    //Get XML-answer and parse to XML-element
                    XElement xml = XElement.Parse(await response.Content.ReadAsStringAsync());

                    List<Product> apiProductList = new List<Product>();

                    //Loop through each product in XML-response and create a Product
                    foreach (XElement xmlProduct in xml.Element("products").Elements())
                    {
                        int quantity = int.Parse(xmlProduct.Element("stock")?.Value);
                        int id = int.Parse(xmlProduct.Element("id")?.Value);
                        string name = xmlProduct.Element("name")?.Value;
                        decimal price = decimal.Parse(xmlProduct.Element("price")?.Value);

                        //Creating new product depending on type - book, movie, videogame
                        dynamic newProduct = "";
                        if (xmlProduct.Name.LocalName == "book")
                        {
                            string author = xmlProduct.Element("author")?.Value;
                            string genre = xmlProduct.Element("genre")?.Value;
                            string bookFormat = xmlProduct.Element("format")?.Value;
                            string language = xmlProduct.Element("language")?.Value;
                            newProduct = (new Book(typeof(Book).Name, quantity, id, name, price, author, genre, bookFormat, language));
                        }
                        else if(xmlProduct.Name.LocalName == "movie")
                        {
                            string movieFormat = xmlProduct.Element("format")?.Value;
                            int duration = int.Parse(xmlProduct.Element("playtime")?.Value ?? "0");
                            newProduct = (new Movie(typeof(Movie).Name, quantity, id, name, price, movieFormat, duration));
                        }
                        else if(xmlProduct.Name.LocalName == "game")
                        {
                            string platform = xmlProduct.Element("platform")?.Value;
                            newProduct = (new Videogame(typeof(Videogame).Name, quantity, id, name, price, platform));
                        }      
                        //Add the API-product to list
                        apiProductList.Add(newProduct);
                    }
                    return apiProductList;
                }
                else
                {
                    throw new HttpRequestException($"Error: {response.StatusCode.ToString()} - {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }       
    }
}
