using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.Entity;

namespace ClassLibrary
{
    internal abstract class BaseRepository<T>: IRepository<T>
    {
        protected List<T> _items;
        public IEnumerable<T> Items => _items;
        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
        }

        public abstract void Save();
    }
    internal class FileRepository<T>: BaseRepository<T>
    {
        string _fileName;

        public FileRepository(string fileName)
        {
            _fileName = fileName;
            Restore();
        }

        private void Restore()
        {
            using (var sr = new StreamReader(_fileName))
            {
                using (var jsonReader = new JsonTextReader(sr))
                {
                    var serializer = new JsonSerializer();
                    _items = serializer.Deserialize<List<T>>(jsonReader);
                }
            }
        }

        public override void Save()
        {
            using (var sw = new StreamWriter(_fileName))
            {
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.Formatting = Formatting.Indented;

                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, _items);
                }
            }
        }
    }
    internal class DatabaseRepository<T>: BaseRepository<T>
    {
        List<T> _dbItems;
        public DatabaseRepository(List<T> dbItems)
        {
            _dbItems = dbItems;
            Restore();
        }
        private void Restore()
        {
            _items = _dbItems;
        }
        public override void Save()
        {

        }
    }
}
