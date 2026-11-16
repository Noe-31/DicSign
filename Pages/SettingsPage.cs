using DicSign.Services;

namespace DicSign.Pages;

public sealed class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        Ui.ConfigurePage(this);

        var headerGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(36),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(36)
            }
        };
        var back = Ui.Centered("‹", 28, Colors.White);
        var tap = new TapGestureRecognizer();
        tap.Tapped += async (_, _) => await Shell.Current.Navigation.PopAsync();
        back.GestureRecognizers.Add(tap);

        headerGrid.Add(back, 0, 0);
        headerGrid.Add(Ui.Centered("Configuración", 22, Colors.White, true), 1, 0);

        // Tarjeta de Modo Oscuro / Modo Claro
        var themeSwitch = new Switch
        {
            IsToggled = Ui.IsDarkMode,
            OnColor = Ui.Blue,
            ThumbColor = Colors.White,
            VerticalOptions = LayoutOptions.Center
        };

        var themeStatusLabel = Ui.Body(Ui.IsDarkMode ? "Modo oscuro activado" : "Modo claro activado", 13);

        themeSwitch.Toggled += (_, e) =>
        {
            themeStatusLabel.Text = e.Value ? "Modo oscuro activado" : "Modo claro activado";
            Ui.ApplyTheme(e.Value);
        };

        var themeRow = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            },
            ColumnSpacing = 12
        };

        themeRow.Add(new VerticalStackLayout
        {
            Spacing = 4,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                new Label
                {
                    Text = "Apariencia",
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Ui.Ink
                },
                themeStatusLabel
            }
        }, 0, 0);
        themeRow.Add(themeSwitch, 1, 0);

        var content = Ui.PageContent(16);
        content.Add(Ui.Card(themeRow));

        content.Add(Ui.Card(new VerticalStackLayout
        {
            Spacing = 10,
            Children =
            {
                new Label
                {
                    Text = "Restablecer progreso",
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Ui.Ink
                },
                Ui.Body("Pone en cero las señas practicadas y acertadas. Tus favoritos y tu cuenta se mantienen."),
                Ui.OutlineButton("Restablecer estadísticas", OnResetClicked)
            }
        }));

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = { Ui.CurvedHeader(headerGrid, 30), content }
            }
        };
    }

    private async void OnResetClicked(object? sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Restablecer progreso", "¿Seguro que quieres borrar tus estadísticas de práctica?", "Restablecer", "Cancelar");
        if (!confirm)
            return;

        await AppDataService.Instance.ResetProgressAsync();
        await DisplayAlert("Listo", "Tus estadísticas se restablecieron.", "Aceptar");
    }
}
