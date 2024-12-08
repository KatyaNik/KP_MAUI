namespace MAUI_Nikitina_KP.Client;
using MAUI_Nikitina_KP.Shared.Services;
using MAUI_Nikitina_KP.Shared.Models;
public partial class Redactor : ContentPage
{
    LocalDatabase localDatabase = new LocalDatabase();
    Employer employer;
    string[] Person;
    int selectedIndex;
    string[] Person_ID;
    int id;
    public Redactor()
	{
		InitializeComponent();
        LoadUsers();

    }
    private void LoadUsers()
    {
        try
        {
            Person = localDatabase.GetUsersFromSQLite();
            Person_ID = localDatabase.GetUsersIDFromSQLite();
            //InformationEditor.Text = Person[0];

            for (int i = 0; i < Person.Length; i++)
            {
                typePicker.Items.Add(Person[i]);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Ошибка", "Ошибка при аутентификации: " + ex.Message, "OK");

        }
    }
    public void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        try
        {
            localDatabase.DeleteEmployer(id);
            
        }
        catch (Exception ex)
        {
            DisplayAlert("Ошибка", "Ошибка при аутентификации: " + ex.Message, "OK");

        }
    }
    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (typePicker.SelectedIndex >= 0 && typePicker.SelectedIndex < Person.Length)
        {
            selectedIndex = typePicker.SelectedIndex;
            id = Convert.ToInt32(Person_ID[selectedIndex]);
            //DisplayAlert("Выбранная запись", $"Индекс записи: {selectedIndex}", "OK");

            employer = localDatabase.FindEmployer_Information(id);

            if (employer != null)
            {
                NameEntry.Text = employer.Name;
                SurnameEntry.Text = employer.Surname;
                SecondnameEntry.Text = employer.SecondName;
                PostEntry.Text = employer.Post;
                EducationEntry.Text = employer.Education;
                HistoryEntry.Text = employer.History;
                DateEntry.Text = employer.Age;
            }
            else
            {
                DisplayAlert("Ошибка", "Сотрудник с указанным ID не найден.", "ОК");
            }
        }
    }
    public void OnRedactorButtonClicked(object sender, EventArgs e)
    {
        try
        {
            localDatabase.RefactorEmployer(id, NameEntry.Text, SurnameEntry.Text, SecondnameEntry.Text, EducationEntry.Text, PostEntry.Text, HistoryEntry.Text, DateEntry.Text);
            DisplayAlert("Выбранная запись", "Все хорошо", "OK");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ошибка", "Ошибка при аутентификации: " + ex.Message, "OK");

        }

    }
    public void OnGoOutButtonClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new Admin();
    }
}