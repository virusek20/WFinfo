using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;

namespace WFInfo.Services.EncryptedData
{
    public class EncryptedDataService : IEncryptedDataService
    {
        private readonly IDataProtector _jwtProtector;

        public EncryptedDataService(IDataProtectionProvider protectionProvider)
        {
            _jwtProtector = protectionProvider.CreateProtector("WFInfo.JWT.v1");
        }

        public string LoadStoredJWT()
        {
            try
            {
                var fileText = File.ReadAllText(Main.AppPath + @"\jwt_encrypted");
                return _jwtProtector.Unprotect(fileText);
            }
            catch (FileNotFoundException e)
            {
                Main.AddLog($"{e.Message} JWT not set");
            }
            catch (CryptographicException e)
            {
                Main.AddLog($"{e.Message} JWT decryption failed");
            }

            return null;
        }

        public void PersistJWT(string jwt)
        {
            var encryptedJWT = _jwtProtector?.Protect(jwt);
            File.WriteAllText(Main.AppPath + @"\jwt_encrypted", encryptedJWT);
        }
    }
}