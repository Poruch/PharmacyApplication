using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TreeViewCrud.Models;
public class Course : INotifyPropertyChanged
{
    private int _id;
    private string _name;
    private string _teacher;
    private int _credits;
    private int _facultyId;

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

    public string Teacher
    {
        get => _teacher;
        set { _teacher = value; OnPropertyChanged(); }
    }

    public int Credits
    {
        get => _credits;
        set { _credits = value; OnPropertyChanged(); }
    }

    public int FacultyId
    {
        get => _facultyId;
        set { _facultyId = value; OnPropertyChanged(); }
    }

    public Faculty Faculty { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
