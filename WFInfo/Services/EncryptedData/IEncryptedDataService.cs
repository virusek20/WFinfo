namespace WFInfo.Services.EncryptedData
{
    public interface IEncryptedDataService
    {
        /// <summary>
        /// Loads and decrypts stored WFMarket JWT or return <see langword="null"/> when no token is saved
        /// </summary>
        /// <returns>JWT or <see langword="null"/></returns>
        string LoadStoredJWT();

        /// <summary>
        /// Encrypts and persists WFMarket JWT
        /// </summary>
        void PersistJWT(string jwt);
    }
}
