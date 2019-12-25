using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace firebase_adapter.Controllers{
    public class FirebaseAuthPayload
    {

        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("returnSecureToken")]
        public bool ReturnSecureToken { get; set; }
    }
}