using DicSign.Services;

namespace DicSign.Pages;

public sealed class LSMResourcePage : ContentPage
{
    public LSMResourcePage(string? letter = null)
    {
        Ui.ConfigurePage(this);
        string url = "https://biblio.uabcs.mx/recursosedu/lsm/";
        if (!string.IsNullOrWhiteSpace(letter))
            url += $"?letra={Uri.EscapeDataString(letter.ToUpperInvariant())}";

        var web = new WebView
        {
            Source = new UrlWebViewSource { Url = url },
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill
        };

        var top = new Grid
        {
            Padding = new Thickness(16, 12),
            ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) },
            Children =
            {
                new Button { Text = "‹", FontSize = 28, TextColor = Ui.Blue, BackgroundColor = Colors.Transparent, WidthRequest = 48 },
                new Label { Text = "Recurso educativo de LSM", FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = Ui.Ink, VerticalTextAlignment = TextAlignment.Center }
            }
        };
        var back = (Button)top.Children[0];
        back.Clicked += async (_, _) => await Shell.Current.Navigation.PopAsync();
        top.SetColumn(top.Children[1], 1);

        var footer = new Label
        {
            Text = "Fuente: Biblioteca Universitaria UABCS (2025). Recurso Educativo Abierto · CC BY-SA 4.0.",
            FontSize = 10,
            TextColor = Ui.Muted,
            Padding = new Thickness(12, 8),
            HorizontalTextAlignment = TextAlignment.Center
        };

        Content = new Grid
        {
            RowDefinitions = { new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Star), new RowDefinition(GridLength.Auto) },
            Children = { top, web, footer }
        };
        Grid.SetRow(web, 1);
        Grid.SetRow(footer, 2);
    }
}
