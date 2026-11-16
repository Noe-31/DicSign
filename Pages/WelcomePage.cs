using Microsoft.Maui.Controls.Shapes;
using DicSign.Services;

namespace DicSign.Pages;

public sealed class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        Ui.ConfigurePage(this);
        BackgroundColor = Colors.White;

        var root = new Grid();

        // Onda azul superior izquierda
        var topBlueWave = new Border
        {
            BackgroundColor = Ui.Blue,
            Stroke = Colors.Transparent,
            WidthRequest = 220,
            HeightRequest = 140,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            Margin = new Thickness(-50, -50, 0, 0),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(0, 0, 140, 0) }
        };

        // Onda amarilla inferior izquierda
        var bottomYellowWave = new Border
        {
            BackgroundColor = Ui.Yellow,
            Stroke = Colors.Transparent,
            WidthRequest = 240,
            HeightRequest = 130,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.End,
            Margin = new Thickness(-40, 0, 0, -55),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(0, 140, 0, 0) }
        };

        // Onda azul inferior derecha
        var bottomBlueWave = new Border
        {
            BackgroundColor = Ui.Blue,
            Stroke = Colors.Transparent,
            WidthRequest = 180,
            HeightRequest = 110,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.End,
            Margin = new Thickness(0, 0, -40, -45),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(120, 0, 0, 0) }
        };

        // Emblema superior con mano LSM y etiqueta DicSign
        var emblemBadge = new Border
        {
            BackgroundColor = Ui.Blue,
            Stroke = Colors.Transparent,
            WidthRequest = 116,
            HeightRequest = 116,
            HorizontalOptions = LayoutOptions.Center,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(58, 58, 24, 24) },
            Content = new VerticalStackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 2,
                Children =
                {
                    Ui.Centered("🤟", 48, Colors.White),
                    new Border
                    {
                        BackgroundColor = Ui.BlueDark,
                        Stroke = Colors.White,
                        StrokeThickness = 1.5,
                        Padding = new Thickness(10, 2),
                        StrokeShape = new RoundRectangle { CornerRadius = 10 },
                        Content = Ui.Centered("DicSign", 13, Colors.White, true)
                    }
                }
            }
        };

        // Título principal bicolor DicSign
        var titleWordmark = new Label
        {
            FontSize = 44,
            HorizontalTextAlignment = TextAlignment.Center,
            CharacterSpacing = -0.8,
            FormattedText = new FormattedString
            {
                Spans =
                {
                    new Span { Text = "Dic", TextColor = Ui.Ink, FontAttributes = FontAttributes.Bold },
                    new Span { Text = "Sign", TextColor = Ui.Yellow, FontAttributes = FontAttributes.Bold }
                }
            }
        };

        var slogan = Ui.Centered("Aprende. Comunica. Incluye.", 15, Ui.Ink, true);

        var startButton = Ui.PrimaryButton("Comenzar", async (_, _) => await Navigation.PushAsync(new LoginPage()));
        startButton.Margin = new Thickness(18, 24, 18, 0);

        var centerStack = new VerticalStackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            Padding = new Thickness(28, 0),
            Spacing = 12,
            Children =
            {
                emblemBadge,
                new BoxView { HeightRequest = 6, Opacity = 0 },
                titleWordmark,
                slogan,
                startButton
            }
        };

        root.Add(topBlueWave);
        root.Add(bottomYellowWave);
        root.Add(bottomBlueWave);
        root.Add(centerStack);

        Content = root;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var user = await AppDataService.Instance.GetCurrentUserAsync();
        if (user is not null)
            Application.Current!.MainPage = new AppShell();
    }
}
