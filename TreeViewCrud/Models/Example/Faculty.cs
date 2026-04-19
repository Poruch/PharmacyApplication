using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace TreeViewCrud.Models;
public class Faculty : INotifyPropertyChanged
{
    private int _id;
    private string _name;
    private ObservableCollection<Course> _courses;
    public int Id
    {
        get => _id;
        set { _id = value; OnPropertyChanged(); }
    }

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }
    public ObservableCollection<Course> Courses
    {
        get => _courses;
        set { _courses = value; OnPropertyChanged(); }
    }


    public Faculty()
    {
        Courses = new ObservableCollection<Course>();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}