using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        //Farklı caching managerlar kullanılabilir. O yüzden interface olarak oluşturulur
        //Örn: Redis kullanılmak istenilebilir. Redis kodları (Redis folder içerisinde) ICacheManager implementasyonu ile yazılır,
        //...ve başka yerin değiştrilmesine gerek kalmaz
        T Get<T>(string key); //getir////Generic method. Verilen key'e karşılık gelen data'ları çağırmak için.////T liste, nesne vb. olarabilir
        object Get(string key); //getir//Generic method kullanılmak istenmezse//Bu da kullanılabilir ancak tip dönüşümü gerekir
        void Add(string key, object value, int duration); //duration: bellekte ne kadar duracak. //Cache ye ekle//value: gelecek (db'de saklanan) data
        bool IsAdd(string key); //Cache de varmı?
        void Remove(string key); // Cacheden uçurma
        void RemoveByPattern(string pattern); //Belli alanları uçur
    }
}
