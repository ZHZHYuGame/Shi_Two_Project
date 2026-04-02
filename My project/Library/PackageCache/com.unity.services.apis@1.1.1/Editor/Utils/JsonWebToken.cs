using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace Unity.Services.Apis.Sample
{
    struct JsonWebToken
    {
        static readonly char[] k_JwtSeparator = { '.' };

        public static string DecodePayload(string token)
        {
            var parts = token.Split(k_JwtSeparator);

            if (parts.Length != 3)
            {
                throw new Exception("JWT has an invalid number of sections");
            }

            var payload = parts[1];
            var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            return Format(payloadJson);
        }

        static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding

            var mod4 = input.Length % 4;
            if (mod4 > 0)
            {
                output += new string('=', 4 - mod4);
            }

            return Convert.FromBase64String(output);
        }

        static string Format(string payload)
        {
            try
            {
                return JToken.Parse(payload).ToString(Formatting.Indented);
            }
            catch
            {
                return null;
            }
        }
    }
}
