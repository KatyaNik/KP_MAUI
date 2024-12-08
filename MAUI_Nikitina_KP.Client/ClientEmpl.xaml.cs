namespace MAUI_Nikitina_KP.Client;
using MAUI_Nikitina_KP.Shared.Services;
using MAUI_Nikitina_KP.Shared.Models;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls.Compatibility;
using System.Text.RegularExpressions;

public partial class ClientEmpl : ContentPage
{
    LocalDatabase localDatabase = new LocalDatabase();
    Employer employer;
    string[] Person;
    int Id;
    public ClientEmpl(int id)
	{
        Id = id;//IdUserFrom
		InitializeComponent();
        LoadUsers();
    }
	public void OnGoOutButtonClicked(object sender, EventArgs e)
	{
        Application.Current.MainPage = new MainPage();
    }
    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
       
        try
        {
            if (typePicker.SelectedIndex != -1)
            {
                var selectedItem = typePicker.Items[typePicker.SelectedIndex];
                // ��������� ������ ������
                string firstCharacter = selectedItem.Substring(0, 1); // ���� ��������� ������ 1 ������ ������� � �������� �������
                InformationEditor.Text = $"{selectedItem}";
                string selectedWord = typePicker.Items[typePicker.SelectedIndex];
                // ��������� ������ �� ��������� ����� �� ��������
                string[] words = selectedWord.Split(' ');
                // ����� ������ �����
                string firstWord = words[0];
                // ������� ������ ������� ���������� � ������
                int index = -1;
                string wordname="";
                for (int i = 0; i < typePicker.Items.Count; i++)
                {
                    string currentItem = typePicker.Items[i];
                    string[] currentItemWords = currentItem.Split(' ');
                    if (currentItemWords.Length > 0 && currentItemWords[0].Equals(firstWord, StringComparison.OrdinalIgnoreCase))
                    {
                        index = i;
                        wordname= currentItemWords[0];
                        break;
                    }
                }
                for (int i = 0; i < ToNamePicker.Items.Count; i++)
                {
                    if (ToNamePicker.Items[i].StartsWith(wordname, StringComparison.OrdinalIgnoreCase))
                    {
                        index = i;
                        break;
                    }
                }

                if (index != -1)
                {
                    // ������������� ��������� ������ � Picker
                    ToNamePicker.SelectedIndex = index;
                }
                else
                {
                    DisplayAlert("������", "������ ��� ��������������: ", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("������", "������ ��� ��������������: " + ex.Message, "OK");

        }
    }
    public void OnSendLettersButtonClicked(object sender, EventArgs e)
    {
        try
        {
            if (InformationEditor.Text != null)
            {
                string nameFrom = typePicker.Items[typePicker.SelectedIndex];
                // ��������� ������ �� ������ ����
                // ��������� ������ �� ������ ����
                string[] words = nameFrom.Split(' ');

                // ���������, ��� ������ �� ������
                if (words.Length > 0)
                {
                    // �������� ��������� �����
                    string lastWord = words[words.Length - 1];
                    localDatabase.SendAnswerMessageText(lastWord, TextEditor.Text);
                }
            }
            else
            {
                string nameTo = ToNamePicker.Items[ToNamePicker.SelectedIndex];
                char lastSymbol = nameTo[nameTo.Length - 1];
                
                string NameTo = localDatabase.FindNameEmployerById(Convert.ToString(lastSymbol));
                string NameFrom = localDatabase.FindNameEmployerById(Convert.ToString(Id));
                localDatabase.SendMessageText(NameFrom, NameTo, TextEditor.Text);
            }
            
            DisplayAlert("�����", "��������� ����������", "OK");
        }
        catch (Exception ex)
        {
            DisplayAlert("������", "������ ��� ��������������: " + ex.Message, "OK");
        }
    }
    public void OnClearListButtonClicked(object sender, EventArgs e)
    {
        TextEditor.Text = "";
    }
    private void LoadUsers()
    {
        employer = localDatabase.FindEmployerID(Id);
        NameEntry.Text = employer.Name;
        SurnameEntry.Text = employer.Surname;
        SecondnameEntry.Text = employer.SecondName;
        DateEntry.Text = employer.Age;
        EducationEntry.Text = employer.Education;
        PostEntry.Text = employer.Post;
        try
        {
            
            Person = localDatabase.GetUsersFromSQLiteWithoutOneUser(Id);
            //InformationEditor.Text = Person[0];

            for (int i = 0; i < Person.Length; i++)
            {
                ToNamePicker.Items.Add(Person[i]);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("������", "������ ��� ��������������: " + ex.Message, "OK");

        }
        try
        {
            string Name = localDatabase.FindNameEmployerById(Convert.ToString(Id));
            string[] messages = localDatabase.FindNewMessage(Convert.ToString(Name));

            for (int i = 0; i < messages.Length; i++)
            {
                typePicker.Items.Add(messages[i]);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("������", "������ ��� ��������������: " + ex.Message, "OK");
        }
    }
}