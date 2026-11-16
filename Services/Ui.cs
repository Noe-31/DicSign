using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Storage;

namespace DicSign.Services;

public static class Ui
{
    private const string ThemeKey = "dicsign_dark_mode";

    public static readonly Color Blue = Color.FromArgb("#1D64D8");
    public static readonly Color BlueDark = Color.FromArgb("#154CB0");
    public static readonly Color BlueLight = Color.FromArgb("#E8F0FE");
    public static readonly Color Yellow = Color.FromArgb("#F7C616");
    public static readonly Color YellowLight = Color.FromArgb("#FFF8DB");

    public static bool IsDarkMode => Preferences.Default.Get(ThemeKey, false);

    public static Color Ink => IsDarkMode ? Color.FromArgb("#F1F5F9") : Color.FromArgb("#1A2233");
    public static Color Muted => IsDarkMode ? Color.FromArgb("#94A3B8") : Color.FromArgb("#6B7280");
    public static Color Line => IsDarkMode ? Color.FromArgb("#334155") : Color.FromArgb("#E2E8F0");
    public static Color Background => IsDarkMode ? Color.FromArgb("#0F172A") : Color.FromArgb("#F8FAFC");
    public static Color CardBg => IsDarkMode ? Color.FromArgb("#1E293B") : Colors.White;

    public static void ApplyTheme(bool isDark)
    {
        Preferences.Default.Set(ThemeKey, isDark);
        if (Application.Current is not null)
        {
            Application.Current.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;
            Application.Current.MainPage = new AppShell();
        }
    }

    public static void ConfigurePage(ContentPage page, string title = "")
    {
        page.Title = title;
        page.BackgroundColor = Background;
        NavigationPage.SetHasNavigationBar(page, false);
        Shell.SetNavBarIsVisible(page, false);
    }

    public static Label Heading(string text, double size = 26) => new()
    {
        Text = text,
        FontSize = size,
        FontAttributes = FontAttributes.Bold,
        TextColor = Ink,
        HorizontalOptions = LayoutOptions.Start
    };

    public static Label Body(string text, double size = 14) => new()
    {
        Text = text,
        FontSize = size,
        TextColor = Muted,
        LineBreakMode = LineBreakMode.WordWrap
    };

    public static Label Centered(string text, double size, Color color, bool bold = false) => new()
    {
        Text = text,
        FontSize = size,
        TextColor = color,
        FontAttributes = bold ? FontAttributes.Bold : FontAttributes.None,
        HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center
    };

    public static Button PrimaryButton(string text, EventHandler clicked)
    {
        var button = new Button
        {
            Text = text,
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
            BackgroundColor = Blue,
            CornerRadius = 24,
            HeightRequest = 50,
            Padding = new Thickness(24, 0),
            HorizontalOptions = LayoutOptions.Fill
        };
        button.Clicked += clicked;
        return button;
    }

    public static Button OutlineButton(string text, EventHandler clicked)
    {
        var button = new Button
        {
            Text = text,
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            TextColor = IsDarkMode ? Color.FromArgb("#93C5FD") : Blue,
            BackgroundColor = CardBg,
            BorderColor = Line,
            BorderWidth = 1.2,
            CornerRadius = 24,
            HeightRequest = 48,
            Padding = new Thickness(24, 0),
            HorizontalOptions = LayoutOptions.Fill
        };
        button.Clicked += clicked;
        return button;
    }

    public static Border Card(View content, Thickness? padding = null, double radius = 20)
    {
        return new Border
        {
            Content = content,
            BackgroundColor = CardBg,
            Stroke = Line,
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(radius) },
            Padding = padding ?? new Thickness(16),
            Shadow = new Shadow
            {
                Brush = Brush.Black,
                Offset = new Point(0, 3),
                Radius = 8,
                Opacity = IsDarkMode ? 0.25f : 0.05f
            }
        };
    }

    public static Border InputBorder(View content)
    {
        return new Border
        {
            Content = content,
            BackgroundColor = CardBg,
            Stroke = Line,
            StrokeThickness = 1.2,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(24) },
            Padding = new Thickness(16, 0),
            HeightRequest = 50
        };
    }

    public static VerticalStackLayout PageContent(double spacing = 16) => new()
    {
        Padding = new Thickness(22, 20, 22, 28),
        Spacing = spacing
    };

    public static Border Chip(string text, bool selected, EventHandler clicked)
    {
        var button = new Button
        {
            Text = text,
            FontSize = 13,
            FontAttributes = FontAttributes.Bold,
            TextColor = selected ? Colors.White : Muted,
            BackgroundColor = selected ? Blue : CardBg,
            CornerRadius = 18,
            BorderColor = selected ? Blue : Line,
            BorderWidth = 1,
            Padding = new Thickness(16, 0),
            HeightRequest = 36
        };
        button.Clicked += clicked;
        return new Border
        {
            Content = button,
            Stroke = Colors.Transparent,
            BackgroundColor = Colors.Transparent
        };
    }

    public static Border CurvedHeader(View innerContent, double bottomRadius = 36)
    {
        return new Border
        {
            Content = innerContent,
            BackgroundColor = Blue,
            Stroke = Colors.Transparent,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(0, 0, bottomRadius, bottomRadius)
            },
            Padding = new Thickness(20, 24, 20, 26)
        };
    }

    public static string GetSignImageSource(string idOrName, bool isLetter)
    {
        if (isLetter)
            return $"sign_{idOrName.Trim().ToLowerInvariant()}.png";

        return idOrName.ToLowerInvariant() switch
        {
            "hello" or "hola" => "sign_hola.png",
            "thanks" or "gracias" => "sign_gracias.png",
            "please" or "por favor" => "sign_porfavor.png",
            "goodbye" or "adiós" => "sign_adios.png",
            "goodmorning" or "buenos días" => "sign_buenosdias.png",
            "goodnight" or "buenas noches" => "sign_buenasnoches.png",
            "sorry" or "perdón" => "sign_perdon.png",
            "yes" or "sí" => "sign_si.png",
            "no" => "sign_no.png",
            _ => "sign_hola.png"
        };
    }

    public static Image SignImage(string fileName, double size) => new()
    {
        Source = ImageSource.FromFile(fileName),
        WidthRequest = size,
        HeightRequest = size,
        MinimumWidthRequest = size,
        MinimumHeightRequest = size,
        Aspect = Aspect.AspectFit,
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center
    };
}
