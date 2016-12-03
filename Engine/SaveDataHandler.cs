using System;
using System.IO;
using System.IO.IsolatedStorage;
namespace Engine
{
    public static class SaveDataHandler
    {
        public static bool SaveData(string data, string filename = "savedata.json")
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            try
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(filename, FileMode.Create, isoStore))
                {
                    using (StreamWriter writer = new StreamWriter(isoStream))
                    {
                        writer.WriteLine(data);
                        Console.WriteLine("You have written to the file.");
                    }
                }
                return true;
            }
            catch
            {
                Console.WriteLine("Something went wrong writing to the file");
                return false;
            }
        }

        public static string LoadData(string filename = "savedata.json")
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            
            if (isoStore.FileExists(filename))
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(filename, FileMode.Open, isoStore))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        Console.WriteLine("Reading contents:");
                        return reader.ReadToEnd();
                    }
                }
            }
            return null;
        }
    }
}
