namespace Application.Common.Models.SeleniumLog
{
    public class SeleniumLogDto
    {
        public string Message { get; set; }
        public DateTimeOffset SentOn { get; set; }

        public SeleniumLogDto(string message)
        {
            Message = message;

            SentOn = DateTimeOffset.Now;
        }
    }
}
