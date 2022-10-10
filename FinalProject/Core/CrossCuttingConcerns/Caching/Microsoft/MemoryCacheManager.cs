using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        IMemoryCache _memoryCache;  //using Microsoft.Extensions.Caching.Memory;

        public MemoryCacheManager() //_memoryCache'yi injecitondan alıyoruz. 
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
            //IMemoryCache Microsoft'un servisi;
            //Core içerisinde olduğu için WebAPI-Business-DAL sıralamasına giremez
            //O yüzden constructor üzerinde değilde;
            //CoreModule üzerinden instance'lanır ve WebAPI içerisinde AddDependecyResolvers();
            //olarak çalıştırılır.
        }
        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
            //duration'da belirtilen dakika zaman aralığında ("TimeSpan")
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _); //Birşey döndürmesini istemiyorsan C#'da out _ yapısını kullanırız.
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            //Git belleğe bak bellekte MemoryCache türünde olan EntriesCollection(Microsoft Cache datalarını byöle tutuyor)'u bul.
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic; //Definitionu _memoryCache olanları bul.
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection) //Her bir cache elemanını gez.
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();
             
            foreach (var key in keysToRemove)  //uygun olanları uçur.
            {
                _memoryCache.Remove(key);
            }
        }
    }
}
