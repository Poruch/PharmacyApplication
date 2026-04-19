using TreeViewCrud.Models;
using TreeViewCrud.Services;

public class CourseViewModel : ViewModelBase
{
    private readonly Course _course;

    public int Id
    {
        get => _course.Id;
        set { _course.Id = value; OnPropertyChanged(); }
    }

    public string Name
    {
        get => _course.Name;
        set { _course.Name = value; OnPropertyChanged(); }
    }

    public string Teacher
    {
        get => _course.Teacher;
        set { _course.Teacher = value; OnPropertyChanged(); }
    }

    public int Credits
    {
        get => _course.Credits;
        set { _course.Credits = value; OnPropertyChanged(); }
    }

    public int FacultyId
    {
        get => _course.FacultyId;
        set { _course.FacultyId = value; OnPropertyChanged(); }
    }

    public string FacultyName => _course.Faculty?.Name ?? string.Empty;

    public bool IsValid
    {
        get
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Teacher) &&
                   Credits > 0 &&
                   Credits <= 10;
        }
    }

    public CourseViewModel()
    {
        _course = new Course();
    }

    public CourseViewModel(Course course)
    {
        _course = course;
    }

    public Course GetModel() => _course;
}
