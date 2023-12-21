using Tesseract;

namespace WFInfo.Services.Tesseract
{
    public interface ITesseractService
    {
        /// <summary>
        /// Engines for parallel processing the reward screen and snapit
        /// </summary>
        TesseractEngine[] Engines { get; }

        void ReloadEngines();
    }
}
