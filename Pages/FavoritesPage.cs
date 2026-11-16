using Microsoft.Maui.Controls.Shapes;
using DicSign.Models;
using DicSign.Services;

namespace DicSign.Pages;

public sealed class FavoritesPage : ContentPage
{
    private readonly VerticalStackLayout _list = new() { Spacing = 14, Padding = new Thickness(20, 16, 20, 26) };

    public FavoritesPage()
    {
        Ui.ConfigurePage(this, "Favoritos");

        var header = Ui.CurvedHeader(new VerticalStackLayout
        {
            Spacing = 6,
            Children =
            {
                Ui.Centered("Mis favoritos", 22, Colors.White, true),
                Ui.Centered("Señas guardadas para repasar", 13, Color.FromArgb("#DBEAFE"))
            }
        }, 32);

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Spacing = 0,
                Children = { header, _list }
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadFavoritesAsync();
    }

    private async Task LoadFavoritesAsync()
    {
        _list.Children.Clear();
        List<DictionaryEntry> entries = await AppDataService.Instance.GetFavoriteEntriesAsync();
        if (entries.Count == 0)
        {
            _list.Add(Ui.Card(new VerticalStackLayout
            {
                Spacing = 8,
                Padding = new Thickness(8, 16),
                Children =
                {
                    Ui.Centered("♡", 44, Ui.Blue),
                    Ui.Centered("Todavía no tienes favoritos", 16, Ui.Ink, true),
                    Ui.Centered("Abre cualquier seña y toca el corazón para guardarla aquí.", 13, Ui.Muted)
                }
            }));
            return;
        }

        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
            ColumnSpacing = 14,
            RowSpacing = 14
        };

        int row = 0;
        for (int i = 0; i < entries.Count; i += 2)
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            AddFavoriteCard(grid, entries[i], 0, row);
            if (i + 1 < entries.Count)
                AddFavoriteCard(grid, entries[i + 1], 1, row);
            row++;
        }
        _list.Add(grid);
    }

    private static void AddFavoriteCard(Grid grid, DictionaryEntry entry, int column, int row)
    {
        string imageFile = Ui.GetSignImageSource(entry.IsLetter ? entry.Name : entry.Id, entry.IsLetter);

        var stack = new VerticalStackLayout
        {
            Spacing = 10,
            Padding = new Thickness(4, 6),
            Children =
        {
            new Border
            {
                Content = Ui.SignImage(imageFile, 78),
                BackgroundColor = Ui.YellowLight,
                Stroke = Colors.Transparent,
                WidthRequest = 92,
                HeightRequest = 92,
                Padding = 6,
                HorizontalOptions = LayoutOptions.Center,
                StrokeShape = new RoundRectangle { CornerRadius = 46 }
            },
            Ui.Centered(entry.Name, 15, Ui.Ink, true),
            Ui.Centered(entry.Category, 11, Ui.Muted)
        }
        };

        var card = Ui.Card(stack, new Thickness(12), 20);
        var tap = new TapGestureRecognizer();
        tap.Tapped += async (_, _) => await Shell.Current.Navigation.PushAsync(new EntryDetailPage(entry));
        card.GestureRecognizers.Add(tap);
        grid.Add(card, column, row);
    }

}
