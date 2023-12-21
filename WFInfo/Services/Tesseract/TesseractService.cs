using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Tesseract;
using WFInfo.Settings;

namespace WFInfo.Services.Tesseract
{
    /// <summary>
    /// Holds all the TesseractEngine instances and is responsible for loadind/reloading them
    /// They are all configured in the same way
    /// </summary>
    public class TesseractService : ITesseractService
    {
        /// <summary>
        /// Engines for parallel processing the reward screen and snapit
        /// </summary>
        public TesseractEngine[] Engines { get; } = new TesseractEngine[4];

        private static string Locale => ApplicationSettings.GlobalReadonlySettings.Locale;
        private static string AppdataTessdataFolder => CustomEntrypoint.appdata_tessdata_folder;
        private static readonly string ApplicationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WFInfo";
        private static readonly string DataPath = ApplicationDirectory + @"\tessdata";

        public TesseractService()
        {
            ReloadEngines();
        }

        private TesseractEngine CreateEngine() => new TesseractEngine(DataPath, Locale)
        {
            DefaultPageSegMode = PageSegMode.SingleBlock
        };

        private void LoadEngines()
        {
            for (var i = 0; i < 4; i++)
            {
                Engines[i]?.Dispose();
                Engines[i] = CreateEngine();
            }
        }

        public void ReloadEngines()
        {
            GetLocaleTessdata();
            LoadEngines();
        }

        private void GetLocaleTessdata()
        {
            string traineddata_hotlink_prefix = "https://raw.githubusercontent.com/WFCD/WFinfo/libs/tessdata/";
            JObject traineddata_checksums = new JObject
            {
                {"en", "7af2ad02d11702c7092a5f8dd044d52f"},
                {"ko", "c776744205668b7e76b190cc648765da"}
            };

            // get trainned data
            string traineddata_hotlink = traineddata_hotlink_prefix + Locale + ".traineddata";
            string app_data_traineddata_path = AppdataTessdataFolder + @"\" + Locale + ".traineddata";

            WebClient webClient = CustomEntrypoint.createNewWebClient();

            if (!File.Exists(app_data_traineddata_path) || CustomEntrypoint.GetMD5hash(app_data_traineddata_path) != traineddata_checksums.GetValue(Locale).ToObject<string>())
            {
                try
                {
                    webClient.DownloadFile(traineddata_hotlink, app_data_traineddata_path);
                }
                catch (Exception) { }
            }
        }
    }
}