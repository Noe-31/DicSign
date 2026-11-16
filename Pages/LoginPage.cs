using Microsoft.Maui.Controls.Shapes;
using DicSign.Services;

namespace DicSign.Pages;

public sealed class LoginPage : ContentPage
{
    private readonly Entry _username = new()
    {
        Placeholder = "Usuario",
        FontSize = 15,
        TextColor = Ui.Ink,
        PlaceholderColor = Ui.Muted,
        BackgroundColor = Colors.Transparent,
        VerticalOptions = LayoutOptions.Center
    };

    private readonly Entry _password = new()
    {
        Placeholder = "Contraseña",
        FontSize = 15,
        TextColor = Ui.Ink,
        PlaceholderColor = Ui.Muted,
        IsPassword = true,
        BackgroundColor = Colors.Transparent,
        VerticalOptions = LayoutOptions.Center
    };

    public LoginPage()
    {
        Ui.ConfigurePage(this);
        BackgroundColor = Colors.White;

        var root = new Grid();

        // Onda azul superior izquierda
        var topWave = new Border
        {
            BackgroundColor = Ui.Blue,
            Stroke = Colors.Transparent,
            WidthRequest = 240,
            HeightRequest = 135,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            Margin = new Thickness(-50, -55, 0, 0),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(0, 0, 140, 0) }
        };

        // Onda amarilla inferior
        var bottomYellow = new Border
        {
            BackgroundColor = Ui.Yellow,
            Stroke = Colors.Transparent,
            HeightRequest = 48,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End,
            Margin = new Thickness(-20, 0, -20, -24),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(60, 60, 0, 0) }
        };

        var passwordToggle = new Button
        {
            Text = "👁",
            FontSize = 16,
            WidthRequest = 38,
            BackgroundColor = Colors.Transparent,
            TextColor = Ui.Muted,
            Padding = 0
        };
        passwordToggle.Clicked += (_, _) => _password.IsPassword = !_password.IsPassword;

        // Ilustración central de dos manos saludando (azul y amarilla)
        var handsIllustration = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = -6,
            Margin = new Thickness(0, 6, 0, 10),
            Children =
            {
                new Label { Text = "🖐️", FontSize = 54, Rotation = -14 },
                new Label { Text = "✋", FontSize = 54, Rotation = 14 }
            }
        };

        var userGrid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(28), new ColumnDefinition(GridLength.Star) },
            ColumnSpacing = 8,
            VerticalOptions = LayoutOptions.Center
        };
        userGrid.Add(Ui.Centered("👤", 16, Ui.Ink), 0, 0);
        userGrid.Add(_username, 1, 0);

        var passGrid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(28), new ColumnDefinition(GridLength.Star), new ColumnDefinition(38) },
            ColumnSpacing = 8,
            VerticalOptions = LayoutOptions.Center
        };
        passGrid.Add(Ui.Centered("🔒", 16, Ui.Ink), 0, 0);
        passGrid.Add(_password, 1, 0);
        passGrid.Add(passwordToggle, 2, 0);

        var form = new VerticalStackLayout
        {
            Padding = new Thickness(30, 88, 30, 40),
            Spacing = 14,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                Ui.Centered("Bienvenido", 30, Ui.Ink, true),
                Ui.Centered("Inicia sesión para continuar", 14, Ui.Muted),
                handsIllustration,
                Ui.InputBorder(userGrid),
                Ui.InputBorder(passGrid),
                new BoxView { HeightRequest = 4, Opacity = 0 },
                Ui.PrimaryButton("Iniciar sesión", OnLoginClicked),
                Ui.OutlineButton("Registrarse", async (_, _) => await Navigation.PushAsync(new RegisterPage()))
            }
        };

        root.Add(topWave);
        root.Add(bottomYellow);
        root.Add(new ScrollView { Content = form });

        Content = root;
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_username.Text) || string.IsNullOrEmpty(_password.Text))
        {
            await DisplayAlert("Datos incompletos", "Escribe tu usuario y contraseña.", "Aceptar");
            return;
        }

        bool valid = await AppDataService.Instance.LoginAsync(_username.Text, _password.Text);
        if (!valid)
        {
            await DisplayAlert("No se pudo iniciar sesión", "Revisa tu usuario y contraseña.", "Aceptar");
            return;
        }

        Application.Current!.MainPage = new AppShell();
    }
}
