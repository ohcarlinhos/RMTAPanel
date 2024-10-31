using System.Net;
using System.Net.Http.Json;
using App.Errors;
using MudBlazor;
using Shared.General;

namespace App.Utils;

public class HttpErrorSnackbarHandle(ISnackbar snackbar)
{
    public async Task<bool> IsNotOk(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage.IsSuccessStatusCode) return false;

        try
        {
            var errorContent = await httpResponseMessage.Content.ReadFromJsonAsync<ErrorResult>();
            if (errorContent == null || string.IsNullOrEmpty(errorContent.Message))
                httpResponseMessage.EnsureSuccessStatusCode();
            
            snackbar.Add(errorContent?.Message, Severity.Error);
            return true;
        }
        catch
        {
            httpResponseMessage.EnsureSuccessStatusCode();
        }

        return true;
    }

    public void GeneralError(HttpRequestException e)
    {
        if (e.StatusCode == HttpStatusCode.Forbidden)
        {
            snackbar.Add(ApiErrors.Forbidden, Severity.Error);
            return;
        }

        snackbar.Add(ApiErrors.GenericErrorServe, Severity.Error);
    }

    public void GeneralError()
    {
        snackbar.Add(ApiErrors.GenericErrorServe, Severity.Error);
    }
}