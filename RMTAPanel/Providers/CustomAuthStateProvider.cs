using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace RMTAPanel.Providers;

public class CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsStringAsync("jwt_token");
        var identity = new ClaimsIdentity();

        httpClient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(token))
        {
            identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            httpClient.DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        }

        var state = new AuthenticationState(new ClaimsPrincipal(identity));
        NotifyAuthenticationStateChanged(Task.FromResult(state));
        return state;
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split(".")[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuesPair = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        return keyValuesPair != null
            ? keyValuesPair.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()))
            : new List<Claim>();
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }
}