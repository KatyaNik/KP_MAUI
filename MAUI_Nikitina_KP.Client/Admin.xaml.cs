namespace MAUI_Nikitina_KP.Client;
using MAUI_Nikitina_KP.Shared.Models;
using MAUI_Nikitina_KP.Shared.Services;
using Microsoft.Data.Sqlite;
using System.Text; // Добавьте эту строку

using System.Collections.Generic;

public partial class Admin : ContentPage
{
    private string name;
    private string surname;
    private string secondname;
    private string post;
    private int age;
    private string education;
    LocalDatabase localDatabase = new LocalDatabase();
    public Admin()
	{
		InitializeComponent();
        FilterAlfPicker.ItemsSource = new string[] { "От А до Я", "От Я до А" };
        FilterStatPicker.ItemsSource = new string[] { "Имя", "Фамилия", "Отчество" };
    }
    public void OnGoOutButtonClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new MainPage();
    }
    public void OnAllEmployersButtonClicked(object sender, EventArgs e)
	{
        List<Employer> employers = localDatabase.GetEmployers(); // Убедитесь, что метод возвращает список работников

        StringBuilder stringBuilder = new StringBuilder();

       

        foreach (var employer in employers)
        {

            stringBuilder.AppendLine( $"Имя:           {employer.Name}\n" +
                                      $"Фамилия:       {employer.Surname}\n" +
                                      $"Отчество:      {employer.SecondName}\n" +
                                      $"Должность:     {employer.Post}\n" +
                                      $"Дата рождения: {employer.Age}\n" +
                                      $"Образование:   {employer.Education}\n"+
                                      $"История:       {employer.History}\n" +
                                      "\n");
        }
        InformationEditor.Text = stringBuilder.ToString();
    }
    public void OnFindEmployersButtonClicked(object sender, EventArgs e)
    {
        string filterAlf = FilterAlfPicker.SelectedItem.ToString();
        string filterStat = FilterStatPicker.SelectedItem.ToString();
        //DisplayAlert("Ошибка", filterStat, "ОК");
        if (filterStat == "Имя")
            filterStat = "Name";
        if (filterStat == "Фамилия")
            filterStat = "Surname";
        if (filterStat == "Отчество")
            filterStat = "SecondName";
        List<Employer> employers = localDatabase.FindFilterEmployers(filterAlf, filterStat);

        InformationEditor.Text = ""; // Очищаем редактор перед новым выводом

        foreach (var employer in employers)
        {
            InformationEditor.Text += $"{employer.Surname} {employer.Name} {employer.SecondName}, должность: {employer.Post}\n\r"; // Форматируем строку для вывода
        }
    }
    public void OnAddEmployerButtonClicked(object sender, EventArgs e)
    {
        //localDatabase.MessageText();
        Application.Current.MainPage = new Registration();
    }
    public void OnMessageButtonClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new MessagePage();
    }
    public void OnEditEmloyerButtonClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new Redactor();
    }
}