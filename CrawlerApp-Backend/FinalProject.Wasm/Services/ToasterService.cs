using Blazored.Toast.Services;
using Microsoft.JSInterop;
using Shipwreck.BlazorJqueryToast;
using FinalProject.Domain.Services;

namespace FinalProject.Wasm.Services
{
    public class ToasterService : IToasterService
    {
        private readonly IJSRuntime _jsRuntime;

        public ToasterService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public void ShowSuccess(string message)
        {
            _jsRuntime.ShowToastAsync(new ToastOptions
            {
                Text = message,
                Position = ToastPosition.TopCenter,
                Heading = "FinalProject"
            });
        }

        public void ShowError(string message)
        {
            _jsRuntime.ShowToastAsync(new ToastOptions
            {
                Text = message,
                Position = ToastPosition.TopCenter,
                Heading = "Error!"
            });
        }
    }
}