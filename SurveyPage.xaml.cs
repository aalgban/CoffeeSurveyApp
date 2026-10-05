using System;
using Microsoft.Maui.Controls;

namespace CoffeeSurveyApp
{
    public partial class SurveyPage : ContentPage
    {
        // Начальные значения, к которым возвращает кнопка «Очистить»
        private readonly DateTime _defaultBirthDate = new DateTime(2000, 1, 1);
        private const double DefaultCupsValue = 3;

        public SurveyPage()
        {
            InitializeComponent();
        }

        // Кнопка «Показать результат» доступна только после заполнения
        // обязательных полей (имя и e-mail) — изменение IsEnabled
        // в зависимости от заполнения анкеты.
        private void OnRequiredFieldChanged(object? sender, TextChangedEventArgs e)
        {
            bool nameFilled = !string.IsNullOrWhiteSpace(_NameEntry_.Text);
            bool emailFilled = !string.IsNullOrWhiteSpace(_EmailEntry_.Text);

            _ShowResultButton_.IsEnabled = nameFilled && emailFilled;
        }

        private async void ShowResultButton_Clicked(object? sender, EventArgs e)
        {
            // Повторная проверка двух обязательных полей
            if (string.IsNullOrWhiteSpace(_NameEntry_.Text) ||
                string.IsNullOrWhiteSpace(_EmailEntry_.Text))
            {
                await DisplayAlertAsync("Не все данные заполнены",
                    "Пожалуйста, заполните имя и электронную почту перед показом результата.",
                    "ОК");
                return;
            }

            string strength = "средняя";
            if (_LightRadio_.IsChecked) strength = "слабая";
            else if (_StrongRadio_.IsChecked) strength = "крепкая";

            string coffeeType = _CoffeeTypePicker_.SelectedItem?.ToString() ?? "не выбрано";

            string extras = "без добавок";
            if (_MilkCheck_.IsChecked && _SugarCheck_.IsChecked)
                extras = "молоко и сахар";
            else if (_MilkCheck_.IsChecked)
                extras = "только молоко";
            else if (_SugarCheck_.IsChecked)
                extras = "только сахар";

            string newsletter = _NewsletterSwitch_.IsToggled ? "да" : "нет";

            string comments = string.IsNullOrWhiteSpace(_CommentsEditor_.Text)
                ? "нет"
                : _CommentsEditor_.Text;

            string summary =
                $"Имя: {_NameEntry_.Text}\n" +
                $"Электронная почта: {_EmailEntry_.Text}\n" +
                $"Комментарии: {comments}\n" +
                $"Любимый вид кофе: {coffeeType}\n" +
                $"Крепость: {strength}\n" +
                $"Добавки: {extras}\n" +
                $"Подписка на рассылку: {newsletter}\n" +
                $"Дата рождения: {_BirthDatePicker_.Date:dd.MM.yyyy}\n" +
                $"Чашек в день: {_CupsSlider_.Value:F0}";

            _ResultLabel_.Text = summary;
            _ResultLabel_.IsVisible = true;
        }

        private void ClearButton_Clicked(object? sender, EventArgs e)
        {
            _NameEntry_.Text = string.Empty;
            _EmailEntry_.Text = string.Empty;
            _CommentsEditor_.Text = string.Empty;

            _CoffeeTypePicker_.SelectedItem = null;

            _MediumRadio_.IsChecked = true;

            _MilkCheck_.IsChecked = false;
            _SugarCheck_.IsChecked = false;

            _NewsletterSwitch_.IsToggled = false;

            _BirthDatePicker_.Date = _defaultBirthDate;

            _CupsSlider_.Value = DefaultCupsValue;

            _ResultLabel_.Text = string.Empty;
            _ResultLabel_.IsVisible = false;

            _ShowResultButton_.IsEnabled = false;
        }
    }
}
