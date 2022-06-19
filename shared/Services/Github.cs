using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace shared.Services;

public class Github
{
    public Github()
    {
        
    }
    
    public async Task<byte[]> GetReference(string source, string reference)
    {
        var token = await GetToken();
        Console.WriteLine(token);
        
        return null;
    }
    
    /// <summary>
    /// ref https://vmsdurano.com/-net-core-3-1-signing-jwt-with-rsa/
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetToken()
    {
        var key = await ParsePEM(Path.GetFullPath("github.pem"));
        
        using RSA rsa = RSA.Create();
        
        rsa.ImportRSAPrivateKey(key, out _);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
        };
        
        var now = DateTime.Now;
        var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

        var jwt = new JwtSecurityToken(
            issuer: "207885",
            claims: new Claim[] {
                new Claim(JwtRegisteredClaimNames.Iat, (unixTimeSeconds - 60).ToString(), ClaimValueTypes.Integer64),
            },
            expires: now.AddMinutes(10),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private async Task<byte[]> ParsePEM(string getFullPath)
    {
        var contents = File.ReadAllText(getFullPath);
        var base64Parts = contents.Split('\n')
            .ToList()
            .Select(x => x.Trim())
            .Where(x => !x.StartsWith("-"))
            .ToList();

        var base64 = string.Join("", base64Parts)
            .Trim();
        
        return Convert.FromBase64String(base64);
    }
}