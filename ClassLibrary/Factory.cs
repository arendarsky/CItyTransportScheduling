using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Interfaces;

namespace ClassLibrary
{
    public class Factory
    {
        static Factory _instance;

        public static Factory Instance => _instance ?? (_instance = new Factory());

        private Factory() { }

        IStorage _storage;

        public IStorage GetDatabaseStorage()
        {
            return _storage ?? (_storage = new DatabaseStorage());
        }
        public IStorage GetFileStorage(bool ForDb)
        {
            if (ForDb)
                return _storage ?? (_storage = new FileStorage {ForDb = true});
            else
                return _storage ?? (_storage = new FileStorage {ForDb = false });
        }
    }
}
