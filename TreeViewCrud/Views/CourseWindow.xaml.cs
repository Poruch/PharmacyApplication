using System.Windows;

namespace TreeViewCrud.Views;
/// <summary>
/// Interaction logic for CourseWindow.xaml
/// </summary>
public partial class CourseWindow : Window
{
    // Свойства для передачи данных (без привязки к ViewModel)
    public string CourseName
    {
        get { return (string)GetValue(CourseNameProperty); }
        set { SetValue(CourseNameProperty, value); }
    }

    public static readonly DependencyProperty CourseNameProperty =
        DependencyProperty.Register("CourseName", typeof(string), typeof(CourseWindow),
            new PropertyMetadata("", OnPropertyChanged));

    public string CourseTeacher
    {
        get { return (string)GetValue(CourseTeacherProperty); }
        set { SetValue(CourseTeacherProperty, value); }
    }

    public static readonly DependencyProperty CourseTeacherProperty =
        DependencyProperty.Register("CourseTeacher", typeof(string), typeof(CourseWindow),
            new PropertyMetadata("", OnPropertyChanged));

    public int CourseCredits
    {
        get { return (int)GetValue(CourseCreditsProperty); }
        set { SetValue(CourseCreditsProperty, value); }
    }

    public static readonly DependencyProperty CourseCreditsProperty =
        DependencyProperty.Register("CourseCredits", typeof(int), typeof(CourseWindow),
            new PropertyMetadata(1, OnPropertyChanged));

    public bool CanSave
    {
        get { return (bool)GetValue(CanSaveProperty); }
        set { SetValue(CanSaveProperty, value); }
    }

    public static readonly DependencyProperty CanSaveProperty =
        DependencyProperty.Register("CanSave", typeof(bool), typeof(CourseWindow),
            new PropertyMetadata(false));

    public CourseWindow()
    {
        InitializeComponent();
        ValidateFields();
    }

    public CourseWindow(int facultyId) : this()
    {
        Title = "Добавление курса";
        CourseCredits = 3;
    }

    public CourseWindow(string name, string teacher, int credits) : this()
    {
        Title = "Редактирование курса";
        CourseName = name;
        CourseTeacher = teacher;
        CourseCredits = credits;
    }

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var window = d as CourseWindow;
        window?.ValidateFields();
    }

    private void ValidateFields()
    {
        var errorMessage = string.Empty;
        var isValid = true;

        if (string.IsNullOrWhiteSpace(CourseName))
        {
            errorMessage += "• Название курса не может быть пустым\n";
            isValid = false;
        }

        // здесь будут другие проверки

        if (!isValid)
        {
            txtError.Text = errorMessage.Trim();
            txtError.Visibility = Visibility.Visible;
        }
        else
        {
            txtError.Visibility = Visibility.Collapsed;
        }

        CanSave = isValid;
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
