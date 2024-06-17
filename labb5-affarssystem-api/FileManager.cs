using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.IO;
using System.Diagnostics;
using ColorCode.Compilation.Languages;
using Windows.System;
using System.Reflection;


namespace labb5_affarssystem_api
{
    public class FileManager
    {
        private StorageFile file;

        public FileManager() { }

        //Open file as Task to make sure file is read through before updating view
        public async Task OpenFile()
        {
            try
            {
                FileOpenPicker fop = new FileOpenPicker();
                fop.ViewMode = PickerViewMode.Thumbnail;
                fop.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                fop.FileTypeFilter.Add(".csv");
                file = await fop.PickSingleFileAsync();
                if (file != null)
                {
                    var fileContent = await FileIO.ReadLinesAsync(file);

                    foreach (var line in fileContent)
                    {
                        //line = line.Trim();
                        string[] attributes = line.Split(';');

                        if (attributes.Length >= 4)
                        {
                            //Create object of product depending on type, and add it to product list
                            if (attributes[0] == typeof(Book).Name)
                                Product.ProductList.Add(new Book(attributes[0], int.Parse(attributes[1]), int.Parse(attributes[2]), attributes[3], decimal.Parse(attributes[4]), attributes[5], attributes[6], attributes[7], attributes[8]));
                            else if (attributes[0] == typeof(Movie).Name)
                            {
                                if (!string.IsNullOrEmpty(attributes[6]))
                                    Product.ProductList.Add(new Movie(attributes[0], int.Parse(attributes[1]), int.Parse(attributes[2]), attributes[3], decimal.Parse(attributes[4]), attributes[5], int.Parse(attributes[6])));
                                else
                                    Product.ProductList.Add(new Movie(attributes[0], int.Parse(attributes[1]), int.Parse(attributes[2]), attributes[3], decimal.Parse(attributes[4]), attributes[5], 0));
                            }
                            else if (attributes[0] == typeof(Videogame).Name)
                                Product.ProductList.Add(new Videogame(attributes[0], int.Parse(attributes[1]), int.Parse(attributes[2]), attributes[3], decimal.Parse(attributes[4]), attributes[5]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        //Save csv file
        public async void SaveFile()
        {
            Debug.WriteLine("i savefile");
            try
            {
                Debug.WriteLine(file.Name);
                if (file != null)
                {
                    StringBuilder fileContent = new StringBuilder();
                    foreach (Product product in Product.ProductList)
                    {
                        fileContent.AppendLine(product.GetCsvLine());
                    }
                    await FileIO.WriteTextAsync(file, fileContent.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
