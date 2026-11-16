namespace DicSign;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new Pages.WelcomePage())
        {
            BarBackgroundColor = Color.FromArgb("#FFFFFF"),
            BarTextColor = Color.FromArgb("#17233B")
        };
    }
}
