// AboutPage.cs
using Microsoft.Maui.ApplicationModel;
using DicSign.Services;

namespace DicSign.Pages;

public sealed class AboutPage : ContentPage
{
    public AboutPage()
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
        headerGrid.Add(Ui.Centered("Acerca de", 22, Colors.White, true), 1, 0);

        var content = Ui.PageContent(14);
        content.Add(Ui.Centered("🤟", 56, Ui.Blue));
        content.Add(Ui.Centered("DicSign", 28, Ui.Ink, true));
        content.Add(Ui.Centered("Aprende. Comunica. Incluye.", 14, Ui.Muted, true));
        content.Add(Ui.Card(new VerticalStackLayout
        {
            Spacing = 8,
            Children =
            {
                new Label { Text = "¿Qué es DicSign?", FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = Ui.Ink },
                Ui.Body("Aplicación educativa interactiva para aprender vocabulario y abecedario en Lengua de Señas Mexicana (LSM), guardar favoritos y practicar con ejercicios visuales.", 14)
            }
        }));
        content.Add(Ui.Card(new VerticalStackLayout
        {
            Spacing = 8,
            Children =
            {
                new Label { Text = "Fuente educativa", FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = Ui.Ink },
                Ui.Body("Biblioteca Universitaria UABCS (2025). LSM Biblioteca: Alfabeto de Lengua de Señas Mexicana. Licencia CC BY-SA 4.0.", 12),
                Ui.OutlineButton("Visitar recurso original", async (_, _) => await Launcher.Default.OpenAsync("https://uabcs.mx"))
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
}
