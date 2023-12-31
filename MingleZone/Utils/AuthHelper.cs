using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using JWT;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Dynamic;
using MingleZone.Models;

namespace MingleZone.Utils
{
    public class AuthHelper
    {
        public RSA LoadPrivateKeyFromPemFile()
        {

            string privateKey = System.IO.File.ReadAllText(@"C:\Users\mprix\source\repos\MingleZone\MingleZone\keys\private_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKey);
            return rsa;
        }
        public RSA LoadPublicKeyFromPemFile()
        {
            string publicKey = System.IO.File.ReadAllText(@"C:\Users\mprix\source\repos\MingleZone\MingleZone\keys\public_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(publicKey);
            return rsa;
        }

        public TokenValidationResult ValidateToken(string token)
        {

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new RS256Algorithm(LoadPublicKeyFromPemFile(), LoadPrivateKeyFromPemFile());
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(token);
                dynamic jsonObject = JsonConvert.DeserializeObject(json);
                int userId = jsonObject.id;
                return new TokenValidationResult { UserId = userId, IsTokenValid = true };
            }
            catch (TokenNotYetValidException)
            {
                return new TokenValidationResult { IsTokenValid = false, ErrorMessage = "Token is not valid yet" };
            }
            catch (TokenExpiredException)
            {
                return new TokenValidationResult { IsTokenValid = false, ErrorMessage = "Token has expired" };
            }
            catch (SignatureVerificationException)
            {
                return new TokenValidationResult { IsTokenValid = false, ErrorMessage = "Token has invalid signature" };
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return new TokenValidationResult { IsTokenValid = false, ErrorMessage = "Unexpected error: " + ex };
            }
        }

    }
}
