namespace MAUI_Nikitina_KP.Client;
using MAUI_Nikitina_KP.Shared.Services;
using MAUI_Nikitina_KP.Shared.Models;
using System;

public partial class MessagePage : ContentPage
{
    LocalDatabase localDatabase = new LocalDatabase();
    
    string[] Person;
    public MessagePage()
	{
		InitializeComponent();
        LoadUsers();
    }
    public void OnClearListButtonClicked(object sender, EventArgs e)
    {
        TextEditor.Text = "";
    }
    public void LoadUsers()
    {
        try
        {
            string[] messages = localDatabase.FindNewMessage("���������");

            for (int i = 0; i < messages.Length; i++)
            {
                typePicker.Items.Add(messages[i]);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("������", "������ ��� ��������������: " + ex.Message, "OK");
        }
        try
        {
            Person = localDatabase.GetUsersFromSQLiteWithoutOneUser(3);
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

    }
    public void OnGoOutButtonClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new Admin();
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
                string wordname = "";
                for (int i = 0; i < typePicker.Items.Count; i++)
                {
                    string currentItem = typePicker.Items[i];
                    string[] currentItemWords = currentItem.Split(' ');
                    if (currentItemWords.Length > 0 && currentItemWords[0].Equals(firstWord, StringComparison.OrdinalIgnoreCase))
                    {
                        index = i;
                        wordname = currentItemWords[0];
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
}