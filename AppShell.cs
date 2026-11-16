using DicSign.Pages;
using DicSign.Services;

namespace DicSign;

public sealed class AppShell : Shell
{
    public AppShell()
    {
        Shell.SetNavBarIsVisible(this, false);
        Shell.SetTabBarBackgroundColor(this, Ui.CardBg);
        Shell.SetTabBarForegroundColor(this, Ui.IsDarkMode ? Color.FromArgb("#60A5FA") : Ui.Blue);
        Shell.SetTabBarUnselectedColor(this, Ui.Muted);
        Shell.SetTabBarTitleColor(this, Ui.IsDarkMode ? Color.FromArgb("#60A5FA") : Ui.Blue);


        var tabs = new TabBar();
        tabs.Items.Add(CreateTab("Categorías", "dictionary", typeof(DictionaryPage)));
        tabs.Items.Add(CreateTab("Practica", "practice", typeof(PracticePage)));
        tabs.Items.Add(CreateTab("Favoritos", "favorites", typeof(FavoritesPage)));
        tabs.Items.Add(CreateTab("Perfil", "profile", typeof(ProfilePage)));
        Items.Add(tabs);
    }

    private static Tab CreateTab(string title, string route, Type pageType)
    {
        var tab = new Tab { Title = title };
        tab.Items.Add(new ShellContent
        {
            Title = title,
            Route = route,
            ContentTemplate = new DataTemplate(pageType)
        });
        return tab;
    }
}
