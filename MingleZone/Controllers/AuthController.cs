using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Security.Cryptography;
using MingleZone.Models;
using System.Drawing.Printing;

namespace MingleZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private String CreateToken(int id)
        {
            var payload = new Dictionary<string, object>
{
    { "id", id },
};
            RSA prkey = LoadPrivateKeyFromPemFile();
            RSA pbkey = LoadPublicKeyFromPemFile();
            IJwtAlgorithm algorithm = new RS256Algorithm(pbkey,prkey);
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            const string key = null; // not needed if algorithm is asymmetric

            var token = encoder.Encode(payload, key);
            return token;
            
        }
        static RSA LoadPrivateKeyFromPemFile()
        {
 
            string privateKey = System.IO.File.ReadAllText(@"C:\Users\mprix\source\repos\MingleZone\MingleZone\keys\private_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKey);
            return rsa;           
        }        
        static RSA LoadPublicKeyFromPemFile()
        {
            string publicKey = System.IO.File.ReadAllText(@"C:\Users\mprix\source\repos\MingleZone\MingleZone\keys\public_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(publicKey);
            return rsa;           
        }
    }
    
}
