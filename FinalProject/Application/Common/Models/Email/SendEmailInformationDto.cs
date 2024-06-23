using Domain.Enums;
using System.ComponentModel;

namespace Application.Common.Models.Email
{
    public class SendEmailInformationDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Link { get; set; }
        public OrderStatus OrderStatus { get; set; }

    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
