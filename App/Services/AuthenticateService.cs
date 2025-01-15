using App.Providers;
using Blazored.LocalStorage;

namespace App.Services;

public class AuthenticateService(ILocalStorageService localStorage, CustomAuthStateProvider authProvider)
{
    public async Task Login(string? token)
    {
        if (string.IsNullOrEmpty(token)) throw new Exception("empty_token");
        await localStorage!.SetItemAsync("jwt_token", token);
        await authProvider!.NotifyStateChanged();
    }

    public async Task Logout()
    {
        await localStorage!.RemoveItemAsync("jwt_token");
        authProvider!.ClearSessionAndNotify();
    }
}