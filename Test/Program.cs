using jff_client_oidc_csharp_legacy;

Console.WriteLine("Hello, World!");

testeClientLegacy();

void testeClientLegacy()
{
    var client = new ClientCredentialsLegacy("https://localhost:62862", "api", "secret", new string[] { "openid" });

    var token = client.GetToken();

    if (token.Success)
    {
        var resultAPI = client.Get<dynamic>("https://localhost:3000", "/products");

        Console.WriteLine(resultAPI.Result.ToString());
    }
    else
    {
        Console.WriteLine(token.Error);
        Console.WriteLine(string.Join(";", token.ListErrors));
    }
}