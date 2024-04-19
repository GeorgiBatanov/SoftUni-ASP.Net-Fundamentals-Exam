using Microsoft.VisualBasic;

namespace SeminarHub.Data
{
    public static class DataRequirments
    {
        //Seminar Requirements
        public const int TopicMinLength = 3;
        public const int TopicMaxLength = 100;

        public const int LecturerNameMinLength = 5;
        public const int LecturerNameMaxLength = 60;

        public const int DetailsMinLength = 10;
        public const int DetailsMaxLength = 500;

        public const string DateAndTimeFormat = "dd/MM/yyyy HH:mm";

        public const int DurationMin = 30;
        public const int DurationMax = 180;

        //Category Requirements
        public const int CetegoryNameMinLength = 3;
        public const int CetegoryNameMaxLength = 50;

        //Error messages
        public const string RequireErrorMessage = "The field {0} is required";
        public const string StringLengthErrorMessage = "The field {0} must be between {2} and {1} characters long";

    }
}
