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
            bool nameFilled = !string.IsNullOrWhiteSpace(NameEntry.Text);
            bool emailFilled = !string.IsNullOrWhiteSpace(EmailEntry.Text);

            ShowResultButton.IsEnabled = nameFilled && emailFilled;
        }

        private async void ShowResultButton_Clicked(object? sender, EventArgs e)
        {
            // Повторная проверка двух обязательных полей
            if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
                string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                await DisplayAlertAsync("Не все данные заполнены",
                    "Пожалуйста, заполните имя и электронную почту перед показом результата.",
                    "ОК");
                return;
            }

            string strength = "средняя";
            if (LightRadio.IsChecked) strength = "слабая";
            else if (StrongRadio.IsChecked) strength = "крепкая";

            string coffeeType = CoffeeTypePicker.SelectedItem?.ToString() ?? "не выбрано";

            string extras = "без добавок";
            if (MilkCheck.IsChecked && SugarCheck.IsChecked)
                extras = "молоко и сахар";
            else if (MilkCheck.IsChecked)
                extras = "только молоко";
            else if (SugarCheck.IsChecked)
                extras = "только сахар";

            string newsletter = NewsletterSwitch.IsToggled ? "да" : "нет";

            string comments = string.IsNullOrWhiteSpace(CommentsEditor.Text)
                ? "нет"
                : CommentsEditor.Text;

            string summary =
                $"Имя: {NameEntry.Text}\n" +
                $"Электронная почта: {EmailEntry.Text}\n" +
                $"Комментарии: {comments}\n" +
                $"Любимый вид кофе: {coffeeType}\n" +
                $"Крепость: {strength}\n" +
                $"Добавки: {extras}\n" +
                $"Подписка на рассылку: {newsletter}\n" +
                $"Дата рождения: {BirthDatePicker.Date:dd.MM.yyyy}\n" +
                $"Чашек в день: {CupsSlider.Value:F0}";

            ResultLabel.Text = summary;
            ResultLabel.IsVisible = true;
        }

        private void ClearButton_Clicked(object? sender, EventArgs e)
        {
            NameEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            CommentsEditor.Text = string.Empty;

            CoffeeTypePicker.SelectedItem = null;

            MediumRadio.IsChecked = true;

            MilkCheck.IsChecked = false;
            SugarCheck.IsChecked = false;

            NewsletterSwitch.IsToggled = false;

            BirthDatePicker.Date = _defaultBirthDate;

            CupsSlider.Value = DefaultCupsValue;

            ResultLabel.Text = string.Empty;
            ResultLabel.IsVisible = false;

            ShowResultButton.IsEnabled = false;
        }
    }
}
