using jff_client_oidc_csharp_legacy;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            testeClientLegacy();

            Console.ReadLine();
        }

        private static void testeClientLegacy()
        {
            var client = new ClientCredentialsLegacy("https://localhost:62862", "api", "secret", new string[] { "openid" });

            var token = client.GetToken();

            if (token.Success)
            {
                var resultAPI = client.Get<dynamic>("https://localhost:62862", "/products");

                if (resultAPI.Success)
                {
                    Console.WriteLine(resultAPI.Result.ToString());
                }
                else
                {
                    Console.WriteLine(resultAPI.Error);
                    Console.WriteLine(string.Join(";", resultAPI.ListErrors));
                }
            }
            else
            {
                Console.WriteLine(token.Error);
                Console.WriteLine(string.Join(";", token.ListErrors));
            }
        }
    }
}
