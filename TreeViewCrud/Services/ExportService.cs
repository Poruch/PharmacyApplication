
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using TreeViewCrud.Models;
using System.Text.Json.Serialization;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace TreeViewCrud.Services
{
    internal class ExportService
    {
        public static void ExportToJson(string filePath, IEnumerable<Category> categories)
        {
            var exportData = categories.Select(c => new
            {
                c.CategoryId,
                c.Name,
                c.Description,
                Items = c.Items.Select(i => new
                {
                    i.ItemId,
                    i.Name,
                    i.MnfName,
                    i.Dosage,
                    i.Form,
                    i.PrescriptionRequired,
                    Batches = i.Batches.Select(b => new
                    {
                        b.BatchId,
                        b.SerialNumber,
                        b.ProductionDate,
                        b.ExpiryDate,
                        b.PurchasePrice,
                        b.SellingPrice,
                        b.Quantity
                    })
                })
            });

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(exportData, options);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }
        public static void ExportToXml(string filePath, IEnumerable<Category> categories)
        {
            var categoriesList = categories.ToList();
            var overrides = new XmlAttributeOverrides();
            var itemAttrs = new XmlAttributes { XmlIgnore = true };
            overrides.Add(typeof(Item), "Category", itemAttrs);
            var batchAttrs = new XmlAttributes { XmlIgnore = true };
            overrides.Add(typeof(Batch), "Item", batchAttrs);
            var serializer = new XmlSerializer(typeof(List<Category>), overrides);

            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
            serializer.Serialize(writer, categoriesList);
        }
    }
}
