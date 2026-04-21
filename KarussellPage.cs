using _8osa.Resources.Localization;
using _8osa.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace _8osa;

public class KarussellPage : ContentPage
{
    public class CarouselItem
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string HelloWorld { get; set; }
        public string CreatedYear { get; set; }
        public Func<string> GetDesc { get; set; }
        public Func<string> GetDetail { get; set; }
    }

    private CarouselView _carousel;
    private ObservableCollection<CarouselItem> _items;
    private int _position = 0;
    private IDispatcherTimer _timer;
    private Label _titleLabel;
    private Label _hintLabel;
    private Button _btnEn;
    private Button _btnEt;

    private readonly List<CarouselItem> _data = new()
    {
        new CarouselItem
        {
            Name = "C#",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/bd/Logo_C_sharp.svg/120px-Logo_C_sharp.svg.png",
            HelloWorld = "Console.WriteLine(\"Hello, World!\");",
            CreatedYear = "2000",
            GetDesc   = () => AppResources.Csharp_Desc,
            GetDetail = () => AppResources.Csharp_Detail
        },
        new CarouselItem
        {
            Name = "Python",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c3/Python-logo-notext.svg/120px-Python-logo-notext.svg.png",
            HelloWorld = "print(\"Hello, World!\")",
            CreatedYear = "1991",
            GetDesc   = () => AppResources.Python_Desc,
            GetDetail = () => AppResources.Python_Detail
        },
        new CarouselItem
        {
            Name = "JavaScript",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/99/Unofficial_JavaScript_logo_2.svg/120px-Unofficial_JavaScript_logo_2.svg.png",
            HelloWorld = "console.log(\"Hello, World!\");",
            CreatedYear = "1995",
            GetDesc   = () => AppResources.JS_Desc,
            GetDetail = () => AppResources.JS_Detail
        },
        new CarouselItem
        {
            Name = "Java",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/3/30/Java_programming_language_logo.svg/120px-Java_programming_language_logo.svg.png",
            HelloWorld = "System.out.println(\"Hello, World!\");",
            CreatedYear = "1995",
            GetDesc   = () => AppResources.Java_Desc,
            GetDetail = () => AppResources.Java_Detail
        },
        new CarouselItem
        {
            Name = "C++",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/18/ISO_C%2B%2B_Logo.svg/120px-ISO_C%2B%2B_Logo.svg.png",
            HelloWorld = "std::cout << \"Hello, World!\";",
            CreatedYear = "1985",
            GetDesc   = () => AppResources.Cpp_Desc,
            GetDetail = () => AppResources.Cpp_Detail
        }
    };

    public KarussellPage()
    {
        BackgroundColor = Color.FromArgb("#0F0F1A");
        _items = new ObservableCollection<CarouselItem>(_data);

        _titleLabel = new Label
        {
            Text = AppResources.AppTitle,
            TextColor = Colors.White,
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 16, 0, 0)
        };

        _btnEn = new Button
        {
            Text = "🇬🇧 EN",
            CornerRadius = 20,
            Padding = new Thickness(20, 8),
            BackgroundColor = Color.FromArgb("#A78BFA"),
            TextColor = Colors.White,
            FontSize = 14
        };
        _btnEt = new Button
        {
            Text = "🇪🇪 ET",
            CornerRadius = 20,
            Padding = new Thickness(20, 8),
            BackgroundColor = Color.FromArgb("#2A2A4A"),
            TextColor = Color.FromArgb("#8888AA"),
            FontSize = 14
        };

        _btnEn.Clicked += (s, e) => LanguageService.ChangeLanguage("en");
        _btnEt.Clicked += (s, e) => LanguageService.ChangeLanguage("et");

        LanguageService.LanguageChanged += OnLanguageChanged;

        var langRow = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 10,
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

        _carousel = new CarouselView
        {
            ItemsSource = _items,
            HeightRequest = 380,
            PeekAreaInsets = new Thickness(30, 0, 30, 0),
            IndicatorView = indicatorView,
            ItemTemplate = new DataTemplate(() =>
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
                        new RowDefinition { Height = new GridLength(180) },
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };

                var image = new Image { Aspect = Aspect.AspectFit, Margin = new Thickness(40, 20) };
                image.SetBinding(Image.SourceProperty, "ImageUrl");
                Grid.SetRow(image, 0);

                var fadeBox = new BoxView
                {
                    Background = new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0),
                        EndPoint = new Point(0, 1),
                        GradientStops = new GradientStopCollection
                        {
                            new GradientStop(Colors.Transparent, 0.4f),
                            new GradientStop(Color.FromArgb("#1A1A2E"), 1f)
                        }
                    }
                };
                Grid.SetRow(fadeBox, 0);

                var nameLabel = new Label { TextColor = Colors.White, FontSize = 22, FontAttributes = FontAttributes.Bold };
                nameLabel.SetBinding(Label.TextProperty, "Name");

                var descLabel = new Label { TextColor = Color.FromArgb("#BBBBCC"), FontSize = 13 };
                var yearLabel = new Label { TextColor = Color.FromArgb("#A78BFA"), FontSize = 12 };
                yearLabel.SetBinding(Label.TextProperty, new Binding("CreatedYear", stringFormat: "📅 {0}"));

                var infoStack = new VerticalStackLayout
                {
                    Padding = new Thickness(20, 10, 20, 20),
                    Spacing = 6,
                    Children = { nameLabel, descLabel, yearLabel }
                };
                Grid.SetRow(infoStack, 1);

                grid.Children.Add(image);
                grid.Children.Add(fadeBox);
                grid.Children.Add(infoStack);
                frame.Content = grid;

                // Täida kirjeldus kui kaart ilmub
                frame.BindingContextChanged += (s, e) =>
                {
                    if (frame.BindingContext is CarouselItem item)
                        descLabel.Text = item.GetDesc();
                };

                // Klikk -> DisplayAlert
                var tap = new TapGestureRecognizer();
                tap.Tapped += async (s, e) =>
                {
                    if (_carousel.CurrentItem is CarouselItem item)
                    {
                        await DisplayAlert(
                            item.Name,
                            $"{AppResources.HelloWorldLabel}\n{item.HelloWorld}\n\n{AppResources.Created} {item.CreatedYear}\n\n{item.GetDetail()}",
                            AppResources.OK
                        );
                    }
                };
                frame.GestureRecognizers.Add(tap);

                return frame;
            })
        };

        _hintLabel = new Label
        {
            Text = AppResources.TapHint,
            TextColor = Color.FromArgb("#8888AA"),
            FontSize = 13,
            HorizontalOptions = LayoutOptions.Center
        };

        var glowLine = new BoxView
        {
            HeightRequest = 2,
            Margin = new Thickness(40, 4),
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
            }
        };

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Padding = new Thickness(0, 0, 0, 40),
                Children = { _titleLabel, glowLine, langRow, _carousel, indicatorView, _hintLabel }
            }
        };

        _timer = Application.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(4);
        _timer.Tick += (s, e) =>
        {
            _position = (_position + 1) % _items.Count;
            _carousel.Position = _position;
        };
        _timer.Start();
    }

    private void OnLanguageChanged()
    {
        // Uuenda staatilised tekstid resx kaudu
        _titleLabel.Text = AppResources.AppTitle;
        _hintLabel.Text = AppResources.TapHint;

        string lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        _btnEn.BackgroundColor = lang == "en" ? Color.FromArgb("#A78BFA") : Color.FromArgb("#2A2A4A");
        _btnEt.BackgroundColor = lang == "et" ? Color.FromArgb("#A78BFA") : Color.FromArgb("#2A2A4A");
        _btnEn.TextColor = lang == "en" ? Colors.White : Color.FromArgb("#8888AA");
        _btnEt.TextColor = lang == "et" ? Colors.White : Color.FromArgb("#8888AA");

        // Uuenda kaardid
        var temp = _items.ToList();
        _items.Clear();
        foreach (var item in temp)
            _items.Add(item);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Stop();
        LanguageService.LanguageChanged -= OnLanguageChanged;
    }
}