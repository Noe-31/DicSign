// ProgressPage.cs
using DicSign.Services;

namespace DicSign.Pages;

public sealed class ProgressPage : ContentPage
{
    private readonly Label _studied = new() { FontSize = 36, FontAttributes = FontAttributes.Bold, TextColor = Ui.Blue, HorizontalTextAlignment = TextAlignment.Center };
    private readonly Label _learned = new() { FontSize = 36, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#16A34A"), HorizontalTextAlignment = TextAlignment.Center };
    private readonly Label _favorites = new() { FontSize = 36, FontAttributes = FontAttributes.Bold, TextColor = Ui.Yellow, HorizontalTextAlignment = TextAlignment.Center };

    public ProgressPage()
    {
        Ui.ConfigurePage(this);

        var headerGrid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(36), new ColumnDefinition(GridLength.Star), new ColumnDefinition(36) }
        };
        var back = Ui.Centered("‹", 28, Colors.White);
        var tap = new TapGestureRecognizer();
        tap.Tapped += async (_, _) => await Shell.Current.Navigation.PopAsync();
        back.GestureRecognizers.Add(tap);

        headerGrid.Add(back, 0, 0);
        headerGrid.Add(Ui.Centered("Mi progreso", 22, Colors.White, true), 1, 0);

        var content = Ui.PageContent(14);
        content.Add(CreateStat("Señas practicadas", _studied));
        content.Add(CreateStat("Respuestas correctas", _learned));
        content.Add(CreateStat("Señas favoritas", _favorites));

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = { Ui.CurvedHeader(headerGrid, 30), content }
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var user = await AppDataService.Instance.GetCurrentUserAsync();
        _studied.Text = (user?.StudiedCount ?? 0).ToString();
        _learned.Text = (user?.LearnedCount ?? 0).ToString();
        _favorites.Text = (user?.FavoriteIds.Count ?? 0).ToString();
    }

    private static Border CreateStat(string title, Label value) => Ui.Card(new VerticalStackLayout
    {
        Spacing = 4,
        Children =
        {
            Ui.Centered(title, 13, Ui.Muted),
            value
        }
    });
}
