﻿
namespace jff_client_oidc_csharp_legacy.Models
{
    public class DefaultResponseTokenModel
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }
}
