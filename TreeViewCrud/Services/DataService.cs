using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using TreeViewCrud.Models;

namespace TreeViewCrud.Services
{
    public class DataService
    {
        private string _connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"F:\\Вшэ\\2 курс\\КПО\\TreeViewCrud\\TreeViewCrud\\PharmacyDB.mdf\";Integrated Security=True";
        private readonly EntityRepository _repo;

        // Локальные коллекции, которые будут привязаны к TreeView
        public ObservableCollection<Category> Categories { get; private set; }
        public ObservableCollection<Item> Items { get; private set; }
        public ObservableCollection<Batch> Batches { get; private set; }

        public DataService()
        {
            _repo = new EntityRepository(_connectionString);
            // Загружаем все данные из БД при инициализации
            LoadAllData();
        }

        private void LoadAllData()
        {
            Categories = new ObservableCollection<Category>(_repo.GetAll<Category>());
            Items = new ObservableCollection<Item>(_repo.GetAll<Item>());
            Batches = new ObservableCollection<Batch>(_repo.GetAll<Batch>());
        }

        // ========== CATEGORY ==========
        public void AddCategory(Category category)
        {
            int newId = _repo.Add(category); // category.Id обновится внутри репозитория
            Categories.Add(category);        // добавляем в локальную коллекцию
        }

        public void UpdateCategory(Category category)
        {
            _repo.Update(category);
            // Заменяем элемент в коллекции (чтобы вызвать обновление в UI)
            var index = Categories.IndexOf(Categories.FirstOrDefault(c => c.Id == category.Id));
            if (index != -1)
                Categories[index] = category;
        }

        public void DeleteCategory(Category category)
        {
            _repo.Delete(category);
            Categories.Remove(category);
        }

        // ========== ITEM ==========
        public void AddItem(Item item)
        {
            int newId = _repo.Add(item);
            Items.Add(item);
        }

        public void UpdateItem(Item item)
        {
            _repo.Update(item);
            var index = Items.IndexOf(Items.FirstOrDefault(i => i.Id == item.Id));
            if (index != -1)
                Items[index] = item;
        }

        public void DeleteItem(Item item)
        {
            _repo.Delete(item);
            Items.Remove(item);
        }

        // ========== BATCH ==========
        public void AddBatch(Batch batch)
        {
            int newId = _repo.Add(batch);
            Batches.Add(batch);
        }

        public void UpdateBatch(Batch batch)
        {
            _repo.Update(batch);
            var index = Batches.IndexOf(Batches.FirstOrDefault(b => b.Id == batch.Id));
            if (index != -1)
                Batches[index] = batch;
        }

        public void DeleteBatch(Batch batch)
        {
            _repo.Delete(batch);
            Batches.Remove(batch);
        }

        // Если нужно получить элементы по родительскому ID (например, Items категории)
        public IEnumerable<Item> GetItemsByCategory(int categoryId)
        {
            return Items.Where(i => i.CategoryId == categoryId);
        }

        public IEnumerable<Batch> GetBatchesByItem(int itemId)
        {
            return Batches.Where(b => b.ItemId == itemId);
        }
    }
}