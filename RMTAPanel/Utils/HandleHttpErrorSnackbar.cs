using System.Net;
using System.Net.Http.Json;
using MudBlazor;
using RMTAPanel.Errors;
using Shared.General;

namespace RMTAPanel.Utils;

public class HandleHttpErrorSnackbar(ISnackbar snackbar)
{
    public async Task<bool> IsNotOk(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage.IsSuccessStatusCode) return false;

        try
        {
            var errorContent = await httpResponseMessage.Content.ReadFromJsonAsync<ErrorResult>();
            if (string.IsNullOrEmpty(errorContent?.Message)) httpResponseMessage.EnsureSuccessStatusCode();
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