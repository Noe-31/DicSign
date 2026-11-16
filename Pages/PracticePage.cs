using Microsoft.Maui.Controls.Shapes;
using DicSign.Models;
using DicSign.Services;

namespace DicSign.Pages;

public sealed class PracticePage : ContentPage
{
    private const int RoundSize = 5;
    private readonly List<DictionaryEntry> _pool = DictionaryCatalog.Entries.Where(x => !x.IsLetter).ToList();
    private List<DictionaryEntry> _questions = new();
    private int _currentIndex;
    private int _score;
    private DictionaryEntry? _currentAnswer;

    private readonly ProgressBar _progressBar = new()
    {
        Progress = 0.2,
        ProgressColor = Ui.Yellow,
        BackgroundColor = Color.FromArgb("#E2E8F0"),
        HeightRequest = 8
    };

    private readonly Label _counterLabel = new()
    {
        Text = "1/5",
        FontSize = 13,
        FontAttributes = FontAttributes.Bold,
        TextColor = Ui.Ink,
        VerticalTextAlignment = TextAlignment.Center
    };

    private readonly Image _signDisplay = new()
    {
        Aspect = Aspect.AspectFit,
        HorizontalOptions = LayoutOptions.Fill,
        VerticalOptions = LayoutOptions.Fill
    };


    private readonly Button[] _optionButtons = new Button[4];

    public PracticePage()
    {
        Ui.ConfigurePage(this, "Practica");
        BackgroundColor = Ui.IsDarkMode ? Color.FromArgb("#1E293B") : Color.FromArgb("#F1F5F9");

        var backLabel = Ui.Centered("‹", 28, Colors.White);
        var backTap = new TapGestureRecognizer();
        backTap.Tapped += async (_, _) => await Shell.Current.GoToAsync("//dictionary");
        backLabel.GestureRecognizers.Add(backTap);

        var headerGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(36),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(36)
            }
        };
        headerGrid.Add(backLabel, 0, 0);
        headerGrid.Add(Ui.Centered("Practica", 22, Colors.White, true), 1, 0);

        var header = Ui.CurvedHeader(headerGrid, 30);

        // Barra de progreso amarilla + 1/5
        var progressRow = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            },
            ColumnSpacing = 12,
            Margin = new Thickness(28, 16, 28, 4)
        };
        progressRow.Add(_progressBar, 0, 0);
        progressRow.Add(_counterLabel, 1, 0);

        var questionLabel = Ui.Centered("¿Qué seña es esta?", 20, Ui.Ink, true);
        questionLabel.Margin = new Thickness(0, 6, 0, 8);

        // Tarjeta central con ilustración de la mano
        var signCard = new Border
        {
            BackgroundColor = Color.FromArgb("#F1F5F9"),
            Stroke = Ui.Line,
            StrokeThickness = 1,
            HeightRequest = 210,
            Margin = new Thickness(32, 0),
            StrokeShape = new RoundRectangle { CornerRadius = 24 },
            Content = _signDisplay
        };

        // Cuadrícula 2x2 de opciones
        var optionsGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            },
            RowDefinitions =
            {
                new RowDefinition(52),
                new RowDefinition(52)
            },
            ColumnSpacing = 12,
            RowSpacing = 12,
            Margin = new Thickness(28, 16, 28, 24)
        };

        for (int i = 0; i < 4; i++)
        {
            int index = i;
            var btn = new Button
            {
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Ui.Ink,
                BackgroundColor = Colors.White,
                BorderColor = Color.FromArgb("#CBD5E1"),
                BorderWidth = 1.2,
                CornerRadius = 24
            };
            btn.Clicked += async (_, _) => await OnOptionSelectedAsync(index);
            _optionButtons[i] = btn;
            optionsGrid.Add(btn, i % 2, i / 2);
        }

        var pageContent = new VerticalStackLayout
        {
            Spacing = 10,
            Children =
            {
                header,
                progressRow,
                questionLabel,
                signCard,
                optionsGrid
            }
        };

        Content = new ScrollView { Content = pageContent };
        StartNewRound();
    }

    private void StartNewRound()
    {
        var rng = Random.Shared;
        _questions = _pool.OrderBy(_ => rng.Next()).Take(RoundSize).ToList();
        _currentIndex = 0;
        _score = 0;
        LoadQuestion();
    }

    private void LoadQuestion()
    {
        _currentAnswer = _questions[_currentIndex];
        _counterLabel.Text = $"{_currentIndex + 1}/{RoundSize}";
        _progressBar.Progress = (double)(_currentIndex + 1) / RoundSize;
        _signDisplay.Source = Ui.GetSignImageSource(_currentAnswer.Id, false);

        var rng = Random.Shared;
        var distractors = _pool
            .Where(x => x.Id != _currentAnswer.Id)
            .OrderBy(_ => rng.Next())
            .Take(3)
            .ToList();

        var choices = distractors.Append(_currentAnswer).OrderBy(_ => rng.Next()).ToList();

        for (int i = 0; i < 4; i++)
        {
            _optionButtons[i].Text = choices[i].Name;
            _optionButtons[i].BackgroundColor = Colors.White;
            _optionButtons[i].TextColor = Ui.Ink;
            _optionButtons[i].BorderColor = Color.FromArgb("#CBD5E1");
        }
    }

    private async Task OnOptionSelectedAsync(int buttonIndex)
    {
        if (_currentAnswer is null)
            return;

        var selectedButton = _optionButtons[buttonIndex];
        bool isCorrect = selectedButton.Text == _currentAnswer.Name;

        selectedButton.BackgroundColor = isCorrect ? Color.FromArgb("#DCFCE7") : Color.FromArgb("#FEE2E2");
        selectedButton.BorderColor = isCorrect ? Color.FromArgb("#16A34A") : Color.FromArgb("#DC2626");
        selectedButton.TextColor = isCorrect ? Color.FromArgb("#15803D") : Color.FromArgb("#B91C1C");

        if (isCorrect)
            _score++;

        await AppDataService.Instance.RecordPracticeAsync(isCorrect);
        await Task.Delay(550);

        _currentIndex++;
        if (_currentIndex < RoundSize)
        {
            LoadQuestion();
        }
        else
        {
            await DisplayAlert(
                "¡Ronda completada!",
                $"Obtuviste {_score} de {RoundSize} respuestas correctas.",
                "Practicar de nuevo");
            StartNewRound();
        }
    }
}
