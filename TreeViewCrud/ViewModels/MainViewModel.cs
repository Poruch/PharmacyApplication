using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TreeViewCrud.Models;
using TreeViewCrud.Services;
using TreeViewCrud.Views;

namespace TreeViewCrud.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly DataService _dataService;
    private object _selectedItem;

    public ObservableCollection<Category> Categories => _dataService.Categories;

    public object? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedItemInfo));
            UpdateCommands();
        }
    }

    public string SelectedItemInfo
    {
        get
        {
            if (_selectedItem == null)
                return "Ничего не выбрано";

            if (_selectedItem is Category cat)
                return $"Категория: {cat.Name}\nТоваров: {cat.Items?.Count ?? 0}";

            if (_selectedItem is Item item)
                return $"Товар: {item.Name}\nДозировка: {item.Dosage}\nФорма: {item.Form}\nРецепт: {(item.PrescriptionRequired ? "Да" : "Нет")}";

            if (_selectedItem is Batch batch)
                return $"Партия: {batch.SerialNumber}\nЦена: {batch.SellingPrice:C}\nОстаток: {batch.Quantity}\nГоден до: {batch.ExpiryDate:d}";

            return "Неизвестный тип";
        }
    }
    public void MoveItemToCategory(Item item, Category targetCategory)
    {
        item.CategoryId = targetCategory.CategoryId;
        _dataService.UpdateItem(item);
        var oldCategory = Categories.FirstOrDefault(c => c.Items.Contains(item));
        oldCategory?.Items.Remove(item);
        targetCategory.Items.Add(item);
    }

    public void MoveBatchToItem(Batch batch, Item targetItem)
    {
        batch.ItemId = targetItem.ItemId;
        _dataService.UpdateBatch(batch);
        var oldItem = _dataService.Items.FirstOrDefault(i => i.Batches.Contains(batch));
        oldItem?.Batches.Remove(batch);
        targetItem.Batches.Add(batch);
    }


    // Команды
    public RelayCommand AddCategoryCommand { get; }
    public RelayCommand AddItemCommand { get; }
    public RelayCommand AddBatchCommand { get; }
    public RelayCommand EditCommand { get; }
    public RelayCommand DeleteCommand { get; }
    public RelayCommand ExportToJson { get; }
    public RelayCommand ExportToXml { get; }
    public MainViewModel()
    {
        try
        {
            _dataService = new DataService();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка инициализации: {ex.Message}\n{ex.InnerException?.Message}");
            throw;
        }

        AddCategoryCommand = new RelayCommand(AddCategory, _ => true);
        AddItemCommand = new RelayCommand(AddItem, CanAddItem);
        AddBatchCommand = new RelayCommand(AddBatch, CanAddBatch);
        EditCommand = new RelayCommand(Edit, CanEditOrDelete);
        DeleteCommand = new RelayCommand(Delete, CanEditOrDelete);
        ExportToJson = new RelayCommand(ExportToJSON);
        ExportToXml = new RelayCommand(ExportToXML);
    }
    private void ExportToJSON(object parameter)
    {
        var dialog = new SaveFileDialog { Filter = "JSON файлы|*.json", DefaultExt = "json" };
        if (dialog.ShowDialog() == true)
        {
            ExportService.ExportToJson(dialog.FileName, Categories);
            MessageBox.Show("Экспорт завершён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
    private void ExportToXML(object parameter)
    {
        var dialog = new SaveFileDialog { Filter = "XML files (*.xml)|*.xml", DefaultExt = "xml" };
        if (dialog.ShowDialog() == true)
        {
            ExportService.ExportToXml(dialog.FileName, Categories);
            MessageBox.Show("Экспорт в XML выполнен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
    private bool CanAddItem(object parameter) => SelectedItem is Category;
    private bool CanAddBatch(object parameter) => SelectedItem is Item;
    private bool CanEditOrDelete(object parameter) => SelectedItem is Item || SelectedItem is Batch;

    private void AddCategory(object parameter)
    {
        var window = new CategoryWindow { Owner = Application.Current.MainWindow };
        if (window.ShowDialog() == true)
        {
            var newCategory = new Category
            {
                Name = window.CategoryName,
                Description = window.CategoryDescription
            };
            _dataService.AddCategory(newCategory);
            OnPropertyChanged(nameof(Categories));
        }
    }

    private void AddItem(object parameter)
    {
        var category = SelectedItem as Category;
        var window = new ItemWindow(category.CategoryId) { Owner = Application.Current.MainWindow };
        if (window.ShowDialog() == true)
        {
            var newItem = new Item
            {
                Name = window.ItemName,
                MnfName = window.ItemMnfName,
                Dosage = window.ItemDosage,
                Form = window.ItemForm,
                ManufacturerId = window.ItemManufacturerId,
                UnitId = window.ItemUnitId,
                PrescriptionRequired = window.ItemPrescriptionRequired,
                CategoryId = window.CategoryId

            };
            _dataService.AddItem(newItem);
            OnPropertyChanged(nameof(Categories));
        }
    }


    private void AddBatch(object parameter)
    {
        var item = SelectedItem as Item;
        var window = new BatchWindow(item.ItemId) { Owner = Application.Current.MainWindow };
        if (window.ShowDialog() == true)
        {
            var newBatch = new Batch
            {
                SerialNumber = window.BatchSerialNumber,
                ProductionDate = window.BatchProductionDate.Value,
                ExpiryDate = window.BatchExpiryDate,
                PurchasePrice = window.BatchPurchasePrice,
                SellingPrice = window.BatchSellingPrice,
                Quantity = window.BatchQuantity,
                ItemId = window.ItemId,
                SupplierId = window.BatchSupplierId,
                LocationId = window.BatchLocationId
            };
            _dataService.AddBatch(newBatch);
            OnPropertyChanged(nameof(Categories));
        }
    }

    private void Edit(object parameter)
    {
        if (SelectedItem is Item item)
        {
            var window = new ItemWindow(item.Name, item.MnfName, item.Dosage, item.Form, item.PrescriptionRequired)
            {
                Owner = Application.Current.MainWindow
            };
            if (window.ShowDialog() == true)
            {
                item.Name = window.ItemName;
                item.MnfName = window.ItemMnfName;
                item.Dosage = window.ItemDosage;
                item.Form = window.ItemForm;
                item.PrescriptionRequired = window.ItemPrescriptionRequired;
                _dataService.UpdateItem(item);
                OnPropertyChanged(nameof(SelectedItemInfo));
            }
        }
        if (SelectedItem is Batch batch)
        {
            var window = new BatchWindow(batch.SerialNumber, batch.ProductionDate, batch.ExpiryDate,
                                         batch.PurchasePrice, batch.SellingPrice, batch.Quantity)
            {
                Owner = Application.Current.MainWindow
            };
            if (window.ShowDialog() == true)
            {
                batch.SerialNumber = window.BatchSerialNumber;
                batch.ProductionDate = window.BatchProductionDate.Value;
                batch.ExpiryDate = window.BatchExpiryDate;
                batch.PurchasePrice = window.BatchPurchasePrice;
                batch.SellingPrice = window.BatchSellingPrice;
                batch.Quantity = window.BatchQuantity;
                _dataService.UpdateBatch(batch);
                OnPropertyChanged(nameof(SelectedItemInfo));
            }
        }
    }

    private void Delete(object parameter)
    {
        if (SelectedItem is Item item)
        {
            if (MessageBox.Show($"Удалить товар '{item.Name}'?", "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _dataService.DeleteItem(item);
                // удаляем из коллекции категории
                var category = Categories.FirstOrDefault(c => c.CategoryId == item.CategoryId);
                category?.Items?.Remove(item);
                if (SelectedItem == item) SelectedItem = null;
                OnPropertyChanged(nameof(Categories));
            }
        }
        else if (SelectedItem is Batch batch)
        {
            if (MessageBox.Show($"Удалить партию '{batch.SerialNumber}'?", "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _dataService.DeleteBatch(batch);
                var itemb = Categories.SelectMany(c => c.Items).FirstOrDefault(i => i.ItemId == batch.ItemId);
                itemb?.Batches?.Remove(batch);
                if (SelectedItem == batch) SelectedItem = null;
                OnPropertyChanged(nameof(Categories));
            }
        }
    }

    private void UpdateCommands()
    {
        AddItemCommand.RaiseCanExecuteChanged();
        AddBatchCommand.RaiseCanExecuteChanged();
        EditCommand.RaiseCanExecuteChanged();
        DeleteCommand.RaiseCanExecuteChanged();
    }
}