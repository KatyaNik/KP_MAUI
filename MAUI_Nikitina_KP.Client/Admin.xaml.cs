namespace MAUI_Nikitina_KP.Client;
using MAUI_Nikitina_KP.Shared.Models;
using MAUI_Nikitina_KP.Shared.Services;
using Microsoft.Data.Sqlite;
using System.Text; // �������� ��� ������

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
        FilterAlfPicker.ItemsSource = new string[] { "�� � �� �", "�� � �� �" };
        FilterStatPicker.ItemsSource = new string[] { "���", "�������", "��������" };
    }
    public void OnGoOutButtonClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new MainPage();
    }
    public void OnAllEmployersButtonClicked(object sender, EventArgs e)
	{
        List<Employer> employers = localDatabase.GetEmployers(); // ���������, ��� ����� ���������� ������ ����������

        StringBuilder stringBuilder = new StringBuilder();

       

        foreach (var employer in employers)
        {

            stringBuilder.AppendLine( $"���:           {employer.Name}\n" +
                                      $"�������:       {employer.Surname}\n" +
                                      $"��������:      {employer.SecondName}\n" +
                                      $"���������:     {employer.Post}\n" +
                                      $"���� ��������: {employer.Age}\n" +
                                      $"�����������:   {employer.Education}\n"+
                                      $"�������:       {employer.History}\n" +
                                      "\n");
        }
        InformationEditor.Text = stringBuilder.ToString();
    }
    public void OnFindEmployersButtonClicked(object sender, EventArgs e)
    {
        string filterAlf = FilterAlfPicker.SelectedItem.ToString();
        string filterStat = FilterStatPicker.SelectedItem.ToString();
        //DisplayAlert("������", filterStat, "��");
        if (filterStat == "���")
            filterStat = "Name";
        if (filterStat == "�������")
            filterStat = "Surname";
        if (filterStat == "��������")
            filterStat = "SecondName";
        List<Employer> employers = localDatabase.FindFilterEmployers(filterAlf, filterStat);

        InformationEditor.Text = ""; // ������� �������� ����� ����� �������

        foreach (var employer in employers)
        {
            InformationEditor.Text += $"{employer.Surname} {employer.Name} {employer.SecondName}, ���������: {employer.Post}\n\r"; // ����������� ������ ��� ������
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