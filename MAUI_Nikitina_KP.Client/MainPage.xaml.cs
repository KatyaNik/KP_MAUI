namespace MAUI_Nikitina_KP.Client
{
    using MAUI_Nikitina_KP.Shared.Services;
    public partial class MainPage : ContentPage
    {
        LocalDatabase localDatabase = new LocalDatabase();
        public MainPage()
        {
            InitializeComponent();
        }
        public void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if(localDatabase.AuthenticateUser(LoginEntry.Text, PasswordEntry.Text) == 2)
            {
                Application.Current.MainPage = new Admin();
            }
            if (localDatabase.AuthenticateUser(LoginEntry.Text, PasswordEntry.Text) == 1)
            {
                int id = localDatabase.GetUserID(LoginEntry.Text, PasswordEntry.Text);
                Application.Current.MainPage = new ClientEmpl(id);
            }
            else
            {
                var result = DisplayAlert("Ошибка", "Логин или Пароль неверны!", "ОК", "Отмена");
            }
        }
    }
}
