using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Crossroads.Web.ViewModels.ValidationAttributes
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private const string DateFormat = "dd/MM/yyyy";
        private const string DefaultErrorMessage = "'{0}' трябва да е между {1:d} и {2:d}.";

        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }

        public DateRangeAttribute(string minDate, string maxDate)
            : base(DefaultErrorMessage)
        {
            MinDate = ParseDate(minDate);
            MaxDate = ParseDate(maxDate);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!(value is DateTime))
            {
                return false;
            }
            DateTime dateValue = (DateTime)value;
            return MinDate <= dateValue && dateValue <= MaxDate;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MinDate, MaxDate);
        }

        private static DateTime ParseDate(string dateValue)
        {
            return DateTime.ParseExact(dateValue, DateFormat, CultureInfo.InvariantCulture);
        }
    }
}