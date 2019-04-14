using System.Windows;

namespace Pharmacy.Utillities
{
    public static class MessageBoxes
    {
        // Members
        // ----------------
        public static readonly string AddErrorMessage = "Грешка при добавяне на запис.";
        public static readonly string PreviewErrorMessage = "Грешка при преглед на запис.";
        public static readonly string EditErrorMessage = "Грешка при редакция на запис.";
        public static readonly string DeleteErrorMessage = "Грешка при изтриване на запис.";
        public static readonly string DeleteMessage = "Сигурни ли сте, че искате да изтриете ибрания запис?";
        public static readonly string LoadDataErrorMessage = "Грешка при зареждане на данни.";
        public static readonly string NoRecordsFoundMessage = "Няма намерени резултати";

        // Methods
        // ----------------
        public static MessageBoxResult ShowError(string text)
        {
            return MessageBox.Show(text, App.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult ShowWarning(string text)
        {
            return MessageBox.Show(text, App.AppName, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static MessageBoxResult ShowInfo(string text)
        {
            return MessageBox.Show(text, App.AppName, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult MessageBoxShowDeleteMessage()
        {
            return MessageBox.Show(DeleteMessage,
                                   App.AppName,
                                   MessageBoxButton.YesNo,
                                   MessageBoxImage.Question);
        }
    }
}
