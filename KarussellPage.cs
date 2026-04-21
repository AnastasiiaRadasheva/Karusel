using System.Collections.ObjectModel;
using _8osa.Services;

namespace _8osa;

public class ProgrammingLanguage
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string DescriptionEn { get; set; }
    public string DescriptionEt { get; set; }
    public string HelloWorld { get; set; }
    public string CreatedYear { get; set; }
    public string DetailInfoEn { get; set; }
    public string DetailInfoEt { get; set; }

    public string Description =>
        LanguageService.CurrentLanguage == "et" ? DescriptionEt : DescriptionEn;

    public string DetailInfo =>
        LanguageService.CurrentLanguage == "et" ? DetailInfoEt : DetailInfoEn;
}

public class KarussellPage : ContentPage
{
    private CarouselView _carouselView;
    private ObservableCollection<ProgrammingLanguage> _items;
    private int _position = 0;
    private IDispatcherTimer _timer;
    private Label _hintLabel;
    private Label _pageTitle;
    private Button _btnEn, _btnEt;

    private readonly List<ProgrammingLanguage> _allItems = new()
    {
        new ProgrammingLanguage
        {
            Name = "C#",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/bd/Logo_C_sharp.svg/120px-Logo_C_sharp.svg.png",
            DescriptionEn = "Powerful object-oriented language by Microsoft",
            DescriptionEt = "Microsofti võimas objektorienteeritud keel",
            HelloWorld = "Console.WriteLine(\"Hello, World!\");",
            CreatedYear = "2000",
            DetailInfoEn = "Used for: .NET apps, games (Unity), web (ASP.NET), mobile (MAUI).\nCreator: Anders Hejlsberg.",
            DetailInfoEt = "Kasutatakse: .NET rakendused, mängud (Unity), veebi (ASP.NET), mobiil (MAUI).\nLooja: Anders Hejlsberg."
        },
        new ProgrammingLanguage
        {
            Name = "Python",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c3/Python-logo-notext.svg/120px-Python-logo-notext.svg.png",
            DescriptionEn = "Great for data science and automation",
            DescriptionEt = "Suurepärane andmeteaduseks ja automatiseerimiseks",
            HelloWorld = "print(\"Hello, World!\")",
            CreatedYear = "1991",
            DetailInfoEn = "Used for: AI/ML, data analysis, scripting, web (Django/Flask).\nCreator: Guido van Rossum.",
            DetailInfoEt = "Kasutatakse: AI/ML, andmeanalüüs, skriptimine, veebi (Django/Flask).\nLooja: Guido van Rossum."
        },
        new ProgrammingLanguage
        {
            Name = "JavaScript",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/99/Unofficial_JavaScript_logo_2.svg/120px-Unofficial_JavaScript_logo_2.svg.png",
            DescriptionEn = "The language of the web",
            DescriptionEt = "Veebiarenduse põhikeel",
            HelloWorld = "console.log(\"Hello, World!\");",
            CreatedYear = "1995",
            DetailInfoEn = "Used for: web frontend, backend (Node.js), mobile (React Native).\nCreator: Brendan Eich.",
            DetailInfoEt = "Kasutatakse: veebifrontend, backend (Node.js), mobiil (React Native).\nLooja: Brendan Eich."
        },
        new ProgrammingLanguage
        {
            Name = "Java",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/3/30/Java_programming_language_logo.svg/120px-Java_programming_language_logo.svg.png",
            DescriptionEn = "Write once, run anywhere",
            DescriptionEt = "Kirjuta kord, käivita igal pool",
            HelloWorld = "System.out.println(\"Hello, World!\");",
            CreatedYear = "1995",
            DetailInfoEn = "Used for: enterprise apps, Android, backend services.\nCreator: James Gosling.",
            DetailInfoEt = "Kasutatakse: ettevõtte rakendused, Android, backend teenused.\nLooja: James Gosling."
        },
        new ProgrammingLanguage
        {
            Name = "C++",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/18/ISO_C%2B%2B_Logo.svg/120px-ISO_C%2B%2B_Logo.svg.png",
            DescriptionEn = "High-performance system programming language",
            DescriptionEt = "Suure jõudlusega süsteemiprogrammeerimise keel",
            HelloWorld = "std::cout << \"Hello, World!\";",
            CreatedYear = "1985",
            DetailInfoEn = "Used for: games, OS, embedded systems, browsers.\nCreator: Bjarne Stroustrup.",
            DetailInfoEt = "Kasutatakse: mängud, operatsioonisüsteemid, sisseehitatud süsteemid.\nLooja: Bjarne Stroustrup."
        }
    };

    public KarussellPage()
    {
        BackgroundColor = Color.FromArgb("#0F0F1A");
        _items = new ObservableCollection<ProgrammingLanguage>(_allItems);
        BuildUI();
        StartAutoScroll();
        LanguageService.LanguageChanged += OnLanguageChanged;
    }

    private void BuildUI()
    {
        _pageTitle = new Label
        {
            Text = GetText("title"),
            TextColor = Colors.White,
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 0)
        };

        _btnEn = CreateLangButton("EN", "en");
        _btnEt = CreateLangButton("ET", "et");
        UpdateLangButtonStyles();

        var langRow = new HorizontalStackLayout
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Center,
            Children = { _btnEn, _btnEt }
        };

        var indicatorView = new IndicatorView
        {
            IndicatorColor = Color.FromArgb("#444466"),
            SelectedIndicatorColor = Color.FromArgb("#A78BFA"),
            HorizontalOptions = LayoutOptions.Center,
            IndicatorSize = 10,
            Margin = new Thickness(0, 8)
        };

        _carouselView = new CarouselView
        {
            ItemsSource = _items,
            HeightRequest = 400,
            PeekAreaInsets = new Thickness(30, 0, 30, 0),
            IndicatorView = indicatorView,
            ItemTemplate = new DataTemplate(CreateCard)
        };

        _hintLabel = new Label
        {
            Text = GetText("hint"),
            TextColor = Color.FromArgb("#8888AA"),
            FontSize = 13,
            HorizontalOptions = LayoutOptions.Center
        };

        var glowLine = new BoxView
        {
            HeightRequest = 2,
            Background = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(Colors.Transparent, 0),
                    new GradientStop(Color.FromArgb("#A78BFA"), 0.5f),
                    new GradientStop(Colors.Transparent, 1)
                }
            },
            Margin = new Thickness(40, 6, 40, 6)
        };

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = new Thickness(0, 20, 0, 40),
                Spacing = 8,
                Children = { _pageTitle, glowLine, langRow, _carouselView, indicatorView, _hintLabel }
            }
        };
    }

    private View CreateCard()
    {
        var frame = new Frame
        {
            CornerRadius = 20,
            HasShadow = true,
            Padding = 0,
            Margin = new Thickness(8),
            BackgroundColor = Color.FromArgb("#1A1A2E"),
            BorderColor = Color.FromArgb("#2A2A4A")
        };

        var grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(200) },
                new RowDefinition { Height = GridLength.Auto }
            }
        };

        var image = new Image { Aspect = Aspect.AspectFit, Margin = new Thickness(30, 20) };
        image.SetBinding(Image.SourceProperty, "ImageUrl");
        Grid.SetRow(image, 0);

        var gradientOverlay = new BoxView
        {
            Background = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(Colors.Transparent, 0.5f),
                    new GradientStop(Color.FromArgb("#1A1A2E"), 1f)
                }
            }
        };
        Grid.SetRow(gradientOverlay, 0);

        var infoStack = new VerticalStackLayout
        {
            Padding = new Thickness(20, 10, 20, 20),
            Spacing = 6
        };

        var nameLabel = new Label { TextColor = Colors.White, FontSize = 22, FontAttributes = FontAttributes.Bold };
        nameLabel.SetBinding(Label.TextProperty, "Name");

        var descLabel = new Label { TextColor = Color.FromArgb("#BBBBCC"), FontSize = 13 };
        descLabel.SetBinding(Label.TextProperty, "Description");

        var yearLabel = new Label { TextColor = Color.FromArgb("#A78BFA"), FontSize = 12 };
        yearLabel.SetBinding(Label.TextProperty, new Binding("CreatedYear", stringFormat: "📅 {0}"));

        infoStack.Children.Add(nameLabel);
        infoStack.Children.Add(descLabel);
        infoStack.Children.Add(yearLabel);
        Grid.SetRow(infoStack, 1);

        grid.Children.Add(image);
        grid.Children.Add(gradientOverlay);
        grid.Children.Add(infoStack);
        frame.Content = grid;

        var tap = new TapGestureRecognizer();
        tap.Tapped += async (s, e) =>
        {
            var lang = _items[_carouselView.Position];
            await DisplayAlert(
                lang.Name,
                $"Hello World:\n{lang.HelloWorld}\n\n📅 {(LanguageService.CurrentLanguage == "et" ? "Loodud:" : "Created:")} {lang.CreatedYear}\n\n{lang.DetailInfo}",
                "OK"
            );
        };
        frame.GestureRecognizers.Add(tap);

        return frame;
    }

    private void StartAutoScroll()
    {
        _timer = Application.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(4);
        _timer.Tick += (s, e) =>
        {
            if (_items == null || _items.Count == 0) return;
            _position = (_position + 1) % _items.Count;
            _carouselView.Position = _position;
        };
        _timer.Start();
    }

    private Button CreateLangButton(string text, string lang)
    {
        var btn = new Button
        {
            Text = text,
            FontSize = 14,
            CornerRadius = 20,
            Padding = new Thickness(16, 8),
            HeightRequest = 40
        };
        btn.Clicked += (s, e) => LanguageService.ChangeLanguage(lang);
        return btn;
    }

    private void UpdateLangButtonStyles()
    {
        bool isEt = LanguageService.CurrentLanguage == "et";
        _btnEn.BackgroundColor = isEt ? Color.FromArgb("#2A2A4A") : Color.FromArgb("#A78BFA");
        _btnEt.BackgroundColor = isEt ? Color.FromArgb("#A78BFA") : Color.FromArgb("#2A2A4A");
        _btnEn.TextColor = isEt ? Color.FromArgb("#8888AA") : Colors.White;
        _btnEt.TextColor = isEt ? Colors.White : Color.FromArgb("#8888AA");
    }

    private void OnLanguageChanged()
    {
        _pageTitle.Text = GetText("title");
        _hintLabel.Text = GetText("hint");
        UpdateLangButtonStyles();
        var temp = _items.ToList();
        _items.Clear();
        foreach (var item in temp) _items.Add(item);
    }

    private string GetText(string key) => key switch
    {
        "title" => LanguageService.CurrentLanguage == "et" ? "Programmeerimiskeeled" : "Programming Languages",
        "hint" => LanguageService.CurrentLanguage == "et" ? "Vajuta kaardile lisainfo saamiseks" : "Tap a card for more info",
        _ => key
    };

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Stop();
        LanguageService.LanguageChanged -= OnLanguageChanged;
    }
}