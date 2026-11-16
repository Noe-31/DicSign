using Microsoft.Maui.Controls.Shapes;
using DicSign.Models;
using DicSign.Services;

namespace DicSign.Pages;

public sealed class DictionaryPage : ContentPage
{
    private readonly Entry _search = new()
    {
        Placeholder = "Buscar...",
        PlaceholderColor = Ui.Muted,
        TextColor = Ui.Ink,
        BackgroundColor = Colors.Transparent,
        FontSize = 14,
        ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
        VerticalOptions = LayoutOptions.Center
    };

    private readonly VerticalStackLayout _cardsContainer = new() { Spacing = 14 };
    private string _category = "Todas";

    public DictionaryPage()
    {
        Ui.ConfigurePage(this, "Categorías");

        var searchRow = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(26), new ColumnDefinition(GridLength.Star) },
            ColumnSpacing = 8,
            VerticalOptions = LayoutOptions.Center
        };
        searchRow.Add(Ui.Centered("🔍", 15, Ui.Muted), 0, 0);
        searchRow.Add(_search, 1, 0);

        var searchPill = new Border
        {
            Content = searchRow,
            BackgroundColor = Colors.White,
            Stroke = Colors.Transparent,
            HeightRequest = 44,
            Padding = new Thickness(14, 0),
            StrokeShape = new RoundRectangle { CornerRadius = 22 }
        };

        var header = Ui.CurvedHeader(new VerticalStackLayout
        {
            Spacing = 14,
            Children =
            {
                Ui.Centered("Categorías", 22, Colors.White, true),
                searchPill
            }
        }, 32);

        var chips = new HorizontalStackLayout
        {
            Spacing = 8,
            HorizontalOptions = LayoutOptions.Center
        };
        BuildChips(chips);

        _search.TextChanged += (_, _) => RefreshCards();

        var body = new VerticalStackLayout
        {
            Padding = new Thickness(20, 14, 20, 26),
            Spacing = 16,
            Children =
            {
                new ScrollView
                {
                    Orientation = ScrollOrientation.Horizontal,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                    Content = chips
                },
                _cardsContainer
            }
        };

        var pageLayout = new VerticalStackLayout
        {
            Spacing = 0,
            Children = { header, body }
        };

        Content = new ScrollView { Content = pageLayout };
        RefreshCards();
    }

    private void BuildChips(HorizontalStackLayout chips)
    {
        chips.Children.Clear();
        chips.Add(Ui.Chip("Todas", _category == "Todas", (_, _) => SetCategory("Todas", chips)));
        chips.Add(Ui.Chip("Saludos", _category == "Saludos", (_, _) => SetCategory("Saludos", chips)));
        chips.Add(Ui.Chip("Abecedario", _category == "Abecedario", (_, _) => SetCategory("Abecedario", chips)));
    }

    private void SetCategory(string category, HorizontalStackLayout chips)
    {
        _category = category;
        BuildChips(chips);
        RefreshCards();
    }

    private void RefreshCards()
    {
        _cardsContainer.Children.Clear();
        IEnumerable<DictionaryEntry> query = DictionaryCatalog.Entries;

        if (_category != "Todas")
            query = query.Where(x => x.Category == _category);

        if (!string.IsNullOrWhiteSpace(_search.Text))
            query = query.Where(x => x.Name.Contains(_search.Text.Trim(), StringComparison.OrdinalIgnoreCase));

        var entries = query.ToList();
        if (entries.Count == 0)
        {
            _cardsContainer.Add(Ui.Centered("No se encontraron señas con ese término.", 14, Ui.Muted));
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
            AddEntryCard(grid, entries[i], 0, row);
            if (i + 1 < entries.Count)
                AddEntryCard(grid, entries[i + 1], 1, row);
            row++;
        }
        _cardsContainer.Add(grid);
    }

    private static void AddEntryCard(Grid grid, DictionaryEntry entry, int column, int row)
    {
        string imageFile = Ui.GetSignImageSource(entry.IsLetter ? entry.Name : entry.Id, entry.IsLetter);

        var avatarCircle = new Border
        {
            BackgroundColor = Ui.YellowLight,
            Stroke = Colors.Transparent,
            WidthRequest = 92,
            HeightRequest = 92,
            Padding = 6,
            HorizontalOptions = LayoutOptions.Center,
            StrokeShape = new RoundRectangle { CornerRadius = 46 },
            Content = Ui.SignImage(imageFile, 78)
        };

        var cardContent = new VerticalStackLayout
        {
            Spacing = 10,
            Padding = new Thickness(4, 6),
            Children =
        {
            avatarCircle,
            Ui.Centered(entry.Name, 15, Ui.Ink, true),
            Ui.Centered(entry.Category, 11, Ui.Muted)
        }
        };

        var card = Ui.Card(cardContent, new Thickness(12), 20);
        var tap = new TapGestureRecognizer();
        tap.Tapped += async (_, _) => await Shell.Current.Navigation.PushAsync(new EntryDetailPage(entry));
        card.GestureRecognizers.Add(tap);

        grid.Add(card, column, row);
    }

}
