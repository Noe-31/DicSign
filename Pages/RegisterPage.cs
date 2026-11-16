using Microsoft.Maui.Controls.Shapes;
using DicSign.Services;

namespace DicSign.Pages;

public sealed class RegisterPage : ContentPage
{
    private readonly Entry _username = new() { Placeholder = "Crea un usuario", TextColor = Ui.Ink, PlaceholderColor = Ui.Muted, BackgroundColor = Colors.Transparent, VerticalOptions = LayoutOptions.Center };
    private readonly Entry _password = new() { Placeholder = "Contraseña (mín. 6 caracteres)", IsPassword = true, TextColor = Ui.Ink, PlaceholderColor = Ui.Muted, BackgroundColor = Colors.Transparent, VerticalOptions = LayoutOptions.Center };
    private readonly Entry _confirm = new() { Placeholder = "Confirmar contraseña", IsPassword = true, TextColor = Ui.Ink, PlaceholderColor = Ui.Muted, BackgroundColor = Colors.Transparent, VerticalOptions = LayoutOptions.Center };

    public RegisterPage()
    {
        Ui.ConfigurePage(this);
        BackgroundColor = Colors.White;

        var root = new Grid();

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

        var form = new VerticalStackLayout
        {
            Padding = new Thickness(30, 88, 30, 40),
            Spacing = 14,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                Ui.Centered("Crear cuenta", 28, Ui.Ink, true),
                Ui.Centered("Guarda tus señas favoritas y tu avance", 14, Ui.Muted),
                new BoxView { HeightRequest = 8, Opacity = 0 },
                Ui.InputBorder(_username),
                Ui.InputBorder(_password),
                Ui.InputBorder(_confirm),
                new BoxView { HeightRequest = 6, Opacity = 0 },
                Ui.PrimaryButton("Registrarse", OnRegisterClicked),
                Ui.OutlineButton("Volver al inicio de sesión", async (_, _) => await Navigation.PopAsync())
            }
        };

        root.Add(topWave);
        root.Add(bottomYellow);
        root.Add(new ScrollView { Content = form });

        Content = root;
    }

    private async void OnRegisterClicked(object? sender, EventArgs e)
    {
        if (_password.Text != _confirm.Text)
        {
            await DisplayAlert("Contraseñas diferentes", "Verifica que ambas contraseñas sean iguales.", "Aceptar");
            return;
        }

        try
        {
            bool created = await AppDataService.Instance.RegisterAsync(_username.Text ?? "", _password.Text ?? "");
            if (!created)
            {
                await DisplayAlert("Usuario ocupado", "Ese nombre de usuario ya está registrado.", "Aceptar");
                return;
            }
            Application.Current!.MainPage = new AppShell();
        }
        catch (ArgumentException ex)
        {
            await DisplayAlert("Revisa tus datos", ex.Message, "Aceptar");
        }
    }
}
