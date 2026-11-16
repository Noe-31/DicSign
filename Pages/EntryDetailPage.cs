using Microsoft.Maui.Controls.Shapes;
using DicSign.Models;
using DicSign.Services;

namespace DicSign.Pages;

public sealed class EntryDetailPage : ContentPage
{
    private readonly DictionaryEntry _entry;
    private readonly Button _favoriteButton = new()
    {
        Text = "♡",
        FontSize = 26,
        BackgroundColor = Colors.Transparent,
        TextColor = Ui.Ink,
        WidthRequest = 48,
        HeightRequest = 48,
        Padding = 0
    };

    public EntryDetailPage(DictionaryEntry entry)
    {
        _entry = entry;
        Ui.ConfigurePage(this);
        BackgroundColor = Ui.IsDarkMode ? Color.FromArgb("#1E293B") : Color.FromArgb("#F1F5F9");


        var backButton = new Button
        {
            Text = "‹",
            FontSize = 32,
            TextColor = Ui.Ink,
            BackgroundColor = Colors.Transparent,
            WidthRequest = 48,
            HeightRequest = 48,
            Padding = 0
        };
        backButton.Clicked += async (_, _) => await Shell.Current.Navigation.PopAsync();
        _favoriteButton.Clicked += OnFavoriteClicked;

        var topBar = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            },
            Padding = new Thickness(16, 12, 16, 4)
        };
        topBar.Add(backButton, 0, 0);
        topBar.Add(_favoriteButton, 2, 0);

        string imageFile = Ui.GetSignImageSource(entry.IsLetter ? entry.Name : entry.Id, entry.IsLetter);

        var visualBox = new Border
        {
            BackgroundColor = Color.FromArgb("#F1F5F9"),
            Stroke = Ui.Line,
            StrokeThickness = 1,
            HeightRequest = 300,
            Padding = 16,
            Margin = new Thickness(24, 8, 24, 0),
            StrokeShape = new RoundRectangle { CornerRadius = 24 },
            Content = new Image
            {
                Source = ImageSource.FromFile(imageFile),
                HeightRequest = 250,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }
        };

        var titleLabel = Ui.Centered(entry.Name, 30, Ui.Ink, true);
        titleLabel.Margin = new Thickness(24, 18, 24, 0);

        var descLabel = Ui.Centered(entry.Description, 14, Ui.Muted);
        descLabel.Margin = new Thickness(32, 4, 32, 0);

        var nextButton = new Button
        {
            Text = "➔",
            FontSize = 26,
            FontAttributes = FontAttributes.Bold,
            TextColor = Ui.Blue,
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.End,
            Margin = new Thickness(0, 18, 24, 24)
        };
        nextButton.Clicked += OnNextSignClicked;

        var layout = new VerticalStackLayout
        {
            Spacing = 8,
            Children =
            {
                topBar,
                visualBox,
                titleLabel,
                descLabel,
                nextButton
            }
        };

        Content = new ScrollView { Content = layout };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        bool favorite = await AppDataService.Instance.IsFavoriteAsync(_entry.Id);
        UpdateFavoriteIcon(favorite);
    }

    private async void OnFavoriteClicked(object? sender, EventArgs e)
    {
        bool favorite = await AppDataService.Instance.ToggleFavoriteAsync(_entry.Id);
        UpdateFavoriteIcon(favorite);
    }

    private void UpdateFavoriteIcon(bool favorite)
    {
        _favoriteButton.Text = favorite ? "♥" : "♡";
        _favoriteButton.TextColor = favorite ? Color.FromArgb("#E11D48") : Ui.Ink;
    }

    private async void OnNextSignClicked(object? sender, EventArgs e)
    {
        var all = DictionaryCatalog.Entries.ToList();
        int idx = all.FindIndex(x => x.Id == _entry.Id);
        var next = all[(idx + 1) % all.Count];
        await Shell.Current.Navigation.PushAsync(new EntryDetailPage(next));
    }
}
