namespace FinalProject.Infrastructure.Services
{
    public interface IToasterService
    {
        void ShowSuccess(string message);

        void ShowError(string message);
    }
}