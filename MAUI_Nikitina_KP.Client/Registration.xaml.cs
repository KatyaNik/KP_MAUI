namespace MAUI_Nikitina_KP.Client;
using MAUI_Nikitina_KP.Shared.Models;
using MAUI_Nikitina_KP.Shared.Services;
using Microsoft.Data.Sqlite;
using System.Text; // Добавьте эту строку

using System.Collections.Generic;

public partial class Registration : ContentPage
{
    private string name;
    private string surname;
    private string secondname;
    private string post;
    private string age;
    private string education;
    private string history;
    private string login;
    private string password;

    LocalDatabase localDatabase = new LocalDatabase();
    public Registration()
	{
		InitializeComponent();
	}
	public void OnGoOutButtonClicked(object sender, EventArgs e)//Выйти с экрана регистрации
	{
        Application.Current.MainPage = new Admin();
    }
    public void OnRegistrationButtonClicked(object sender, EventArgs e)//Добавление нового пользователя
    {
        //Логика добавление нового пользователя
        name = NameEntry.Text;
        surname = SurnameEntry.Text;
        secondname = SecondnameEntry.Text;
        post = PostEntry.Text;
        education = EducationEntry.Text;
        history = HistoryEntry.Text;
        age = DateEntry.Text;
        login = LoginEntry.Text;
        password = PasswordEntry.Text;
        localDatabase.AddEmployer(name, surname, secondname, age, post, education, history, password, login);


        Application.Current.MainPage = new MainPage();
    }
}