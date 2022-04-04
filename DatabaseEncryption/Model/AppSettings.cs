namespace DatabaseEncryption
{
    public class AppSettings
    {
        public string DbPath { get; set; } = String.Empty;

        public string Password  { get; set; } = String.Empty;
        public int CipherPageSize { get; set; } = 1024;
        public int KdfIter { get; set; } = 1600;
        public string CipherHmacAlgorithm { get; set; } = String.Empty;
        public string CipherKdfAlgorithm { get; set; } = String.Empty;

    }
}
