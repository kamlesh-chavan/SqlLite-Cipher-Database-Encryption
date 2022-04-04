namespace DatabaseEncryption
{
    public interface ISqliteDbHelper
    {
        bool CreateDbWithData();
        bool ReadData();
    }
}
