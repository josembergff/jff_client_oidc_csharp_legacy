# Client OIDC C#
Simple client for request OIDC token and requests. Having the .net framework 4.7.

## Install Package Manager

```bash
PM> Install-Package jff_client_oidc_csharp_legacy
```

## Install .NET CLI

```bash
> dotnet add package jff_client_oidc_csharp_legacy
```

## Install Paket CLI

```bash
> paket add jff_client_oidc_csharp_legacy
```

## Example Usage

```bash
using jff_client_oidc_csharp_legacy;

namespace ExampleConnectOIDC
{
    public class ConnectOIDC
    {
        private readonly ClientCredentials client;
        public ConnectOIDC(){}
            client = new ClientCredentials("{urlAuthority}", "{clientId}", "{clientSecret}", new string[] { "openid" });
        }

        public string GetToken(){
            return client.GetToken();
        }

        public dynamic GetApiRest(string url){
            return client.Get<dynamic>(url);
        }

        public dynamic PostApiRest(string url, dynamic objSend){
            return client.Post<dynamic, dynamic>(url, objSend);
        }

        public dynamic PutApiRest(string url, dynamic objSend){
            return client.Put<dynamic, dynamic>(url, objSend);
        }

        public dynamic DeleteApiRest(string url){
            return client.Delete<dynamic>(url);
        }
    }
 }
```
