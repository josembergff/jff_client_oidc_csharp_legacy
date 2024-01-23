using jff_client_oidc_csharp_legacy.Models;
using jff_client_oidc_csharp_legacy.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace jff_client_oidc_csharp_legacy
{
    public class ClientCredentialsLegacy
    {
        private readonly string urlAuthority;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly IEnumerable<string> scopes;
        private string accessToken;
        private DateTime expireDate;

        public ClientCredentialsLegacy(string urlAuthority, string clientId, string clientSecret, IEnumerable<string> scopes)
        {
            this.urlAuthority = urlAuthority;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.scopes = scopes;
            expireDate = DateTime.MinValue;
            accessToken = string.Empty;
        }

        public DefaultResponseModel<string> GetToken()
        {
            var objReturn = new DefaultResponseModel<string>();
            if (!string.IsNullOrEmpty(urlAuthority))
            {
                if (expireDate <= DateTime.Now)
                {
                    try
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.BaseAddress = urlAuthority + "/";
                            var json = webClient.DownloadString(".well-known/openid-configuration");
                            var objToken = JsonConvert.JsonDeserializer<DefaultConfigTokenModel>(json);
                            var resultToken = getTokenValue(objToken.token_endpoint);
                            objReturn.Extract(resultToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        objReturn.ListErrors.Add($"An error has occurred in request initial configurations to '{urlAuthority}'.");
                        accessToken = string.Empty;
                        objReturn.Extract(ex);
                    }
                }
            }
            else
            {
                objReturn.ListErrors.Add("The parameter 'urlAuthority' is required in the create new instance class.");
            }

            return objReturn;
        }

        public DefaultResponseModel<ReturnEntity> Get<ReturnEntity>(string urlApi, string urlPath)
        {
            var objReturn = new DefaultResponseModel<ReturnEntity>();
            var resultToken = GetToken();
            objReturn.Extract(resultToken);

            if (objReturn.Success)
            {
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.BaseAddress = urlApi + "/";
                        var json = webClient.DownloadString(urlPath);
                        var objResult = JsonConvert.JsonDeserializer<ReturnEntity>(json);
                        objReturn.Result = objResult;
                        objReturn.Extract(resultToken);
                    }
                }
                catch (Exception ex)
                {

                    objReturn.ListErrors.Add($"An error has occurred in GET request to '{urlApi}'.");
                    objReturn.Extract(ex);
                }
            }

            return objReturn;
        }

        public DefaultResponseModel<ReturnEntity> Post<SendEntity, ReturnEntity>(string urlApi, string pathUrl, SendEntity obj)
        {
            var objReturn = new DefaultResponseModel<ReturnEntity>();
            var resultToken = GetToken();
            objReturn.Extract(resultToken);

            try
            {
                objReturn.Result = RequestRest.POST<ReturnEntity, SendEntity>(urlApi, pathUrl, obj, accessToken, false);
            }
            catch (Exception ex) {
                objReturn.ListErrors.Add($"An error has occurred in POST request to '{urlApi}/{pathUrl}'."); 
                objReturn.Extract(ex); 
            }

            return objReturn;
        }

        public DefaultResponseModel<ReturnEntity> Put<SendEntity, ReturnEntity>(string urlApi, string pathUrl, SendEntity obj)
        {
            var objReturn = new DefaultResponseModel<ReturnEntity>();
            var resultToken = GetToken();
            objReturn.Extract(resultToken);

            try
            {
                objReturn.Result = RequestRest.PUT<ReturnEntity, SendEntity>(urlApi, pathUrl, obj, accessToken, false);
            }
            catch (Exception ex)
            {
                objReturn.ListErrors.Add($"An error has occurred in PUT request to '{urlApi}/{pathUrl}'.");
                objReturn.Extract(ex); 
            }

            return objReturn;
        }

        public DefaultResponseModel<ReturnEntity> Delete<ReturnEntity>(string urlApi, string pathUrl)
        {
            var objReturn = new DefaultResponseModel<ReturnEntity>();
            var resultToken = GetToken();
            objReturn.Extract(resultToken);
            try
            {
                objReturn.Result = RequestRest.PUT<ReturnEntity, object>(urlApi, pathUrl, null, accessToken, false);
            }
            catch (Exception ex)
            {
                objReturn.ListErrors.Add($"An error has occurred in DELETE request to '{urlApi}/{pathUrl}'."); 
                objReturn.Extract(ex); 
            }

            return default;
        }

        private DefaultResponseModel<string> getTokenValue(string urlToken)
        {
            var objReturn = new DefaultResponseModel<string>();
            if (!string.IsNullOrEmpty(urlToken))
            {
                try
                {
                    var baseUrl = urlAuthority + "/";
                    var pathUrl = urlToken.Replace(baseUrl, "");
                    var scope = "";
                    if (scopes?.Any() == true)
                    {
                        var uniqueScope = string.Join("", scopes);
                        scope = uniqueScope;
                    }
                    var objSend = new SendRequestTokenModel() { client_id = clientId, client_secret = clientSecret, grant_type = "client_credentials", scope = scope };
                    var objToken = RequestRest.POST<DefaultResponseTokenModel, SendRequestTokenModel>(urlAuthority, pathUrl, objSend, sendWWWForm: true);

                    if (objToken.expires_in > 0)
                    {
                        expireDate = DateTime.Now.AddSeconds(objToken.expires_in);
                    }

                    accessToken = objToken.access_token ?? string.Empty;

                    objReturn.Result = accessToken;
                }
                catch (Exception ex)
                {
                    objReturn.ListErrors.Add($"An error has occurred in request to '{urlToken}'.");
                    accessToken = string.Empty;
                    objReturn.Extract(ex);
                }
            }
            else
            {
                objReturn.ListErrors.Add("The parameter 'urlToken' is required.");
                accessToken = string.Empty;
            }

            return objReturn;
        }
    }


}