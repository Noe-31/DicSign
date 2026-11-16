using Microsoft.Maui.Controls.Shapes;
using DicSign.Services;

namespace DicSign.Pages;

public sealed class ProfilePage : ContentPage
{
    private readonly Label _usernameLabel = new()
    {
        Text = "Usuario",
        FontSize = 24,
        FontAttributes = FontAttributes.Bold,
        TextColor = Ui.Ink,
        HorizontalTextAlignment = TextAlignment.Center
    };

    public ProfilePage()
    {
        Ui.ConfigurePage(this, "Perfil");

        var headerBg = new Border
        {
            BackgroundColor = Ui.Blue,
            Stroke = Colors.Transparent,
            HeightRequest = 145,
            VerticalOptions = LayoutOptions.Start,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(0, 0, 90, 90) }
        };

        // Avatar circular superpuesto en el borde inferior de la cabecera azul
        var avatarCircle = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Ui.Blue,
            StrokeThickness = 3,
            WidthRequest = 112,
            HeightRequest = 112,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 78, 0, 0),
            StrokeShape = new Ellipse(),
            Content = new Border
            {
                BackgroundColor = Ui.BlueLight,
                Stroke = Colors.Transparent,
                Margin = 6,
                StrokeShape = new Ellipse(),
                Content = Ui.Centered("👤", 54, Ui.Blue)
            }
        };

        var topContainer = new Grid
        {
            Children = { headerBg, avatarCircle }
        };

        var menuList = new VerticalStackLayout
        {
            Padding = new Thickness(24, 10, 24, 30),
            Spacing = 14,
            Children =
            {
                _usernameLabel,
                new BoxView { HeightRequest = 6, Opacity = 0 },
                CreatePillOption("⭐", Ui.YellowLight, "Mis favoritos", async () => await Shell.Current.GoToAsync("//favorites")),
                CreatePillOption("📊", Ui.BlueLight, "Mi progreso", async () => await Shell.Current.Navigation.PushAsync(new ProgressPage())),
                CreatePillOption("⚙️", Color.FromArgb("#F1F5F9"), "Configuración", async () => await Shell.Current.Navigation.PushAsync(new SettingsPage())),
                CreatePillOption("ℹ️", Ui.BlueLight, "Acerca de", async () => await Shell.Current.Navigation.PushAsync(new AboutPage())),
                new BoxView { HeightRequest = 10, Opacity = 0 },
                Ui.OutlineButton("Cerrar sesión", OnLogoutClicked)
            }
        };

        var pageStack = new VerticalStackLayout
        {
            Spacing = 8,
            Children = { topContainer, menuList }
        };

        Content = new ScrollView { Content = pageStack };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var user = await AppDataService.Instance.GetCurrentUserAsync();
        _usernameLabel.Text = user?.Username ?? "Usuario";
    }

    private static Border CreatePillOption(string icon, Color badgeBg, string title, Func<Task> action)
    {
        var row = new Grid
        {
            Padding = new Thickness(12, 0, 18, 0),
            ColumnSpacing = 14,
            ColumnDefinitions =
            {
                new ColumnDefinition(40),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            },
            HeightRequest = 56
        };

        var iconBadge = new Border
        {
            BackgroundColor = badgeBg,
            Stroke = Colors.Transparent,
            WidthRequest = 38,
            HeightRequest = 38,
            VerticalOptions = LayoutOptions.Center,
            StrokeShape = new Ellipse(),
            Content = Ui.Centered(icon, 18, Ui.Blue)
        };

        var titleLabel = new Label
        {
            Text = title,
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            TextColor = Ui.Ink,
            VerticalTextAlignment = TextAlignment.Center
        };

        var arrowLabel = new Label
        {
            Text = "›",
            FontSize = 24,
            TextColor = Ui.Muted,
            VerticalTextAlignment = TextAlignment.Center
        };

        row.Add(iconBadge, 0, 0);
        row.Add(titleLabel, 1, 0);
        row.Add(arrowLabel, 2, 0);

        var pill = new Border
        {
            Content = row,
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#CBD5E1"),
            StrokeThickness = 1.2,
            StrokeShape = new RoundRectangle { CornerRadius = 28 }
        };

        var tap = new TapGestureRecognizer();
        tap.Tapped += async (_, _) => await action();
        pill.GestureRecognizers.Add(tap);

        return pill;
    }

    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Cerrar sesión", "¿Quieres salir de tu cuenta?", "Sí, salir", "Cancelar");
        if (!confirm)
            return;

        AppDataService.Instance.Logout();
        Application.Current!.MainPage = new NavigationPage(new WelcomePage());
    }
}
