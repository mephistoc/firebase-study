using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace firebase_adapter.Models{
    public class FirebaseAuthResult
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }
        [JsonProperty("localId")]
        public string LocalId { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("idToken")]
        public string IdToken { get; set; }
        [JsonProperty("registered")]
        public bool Registered { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonProperty("expiresIn")]
        public string ExpiresIn { get; set; }
    }
}