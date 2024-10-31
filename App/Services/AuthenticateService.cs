using App.Providers;
using Blazored.LocalStorage;

namespace App.Services;

public class AuthenticateService(ILocalStorageService LocalStorage, CustomAuthStateProvider AuthProvider)
{
    public async Task SetSate(string? token)
    {
        if (string.IsNullOrEmpty(token)) throw new Exception("empty_token");
        await LocalStorage!.SetItemAsync("jwt_token", token);
        await AuthProvider!.NotifyStateChanged();
    }

    public async Task ClearSate()
    {
        await LocalStorage!.RemoveItemAsync("jwt_token");
        await AuthProvider!.NotifyStateChanged();
    }
}