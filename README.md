# BeymenCodeCase
Veritabanı olarak mongodb kullanıldı. Redis pub/sub yapısından faydalanıldı.
docker-compose.yml file içerisinde gönderilen örnek verilerin seed olarak eklenmesi sağlandı.

docker-compose up ile çalıştırılabilir. Bu şekilde ayağa kaldırıldıktan sonra;

-http://localhost/ -> back office configurasyon yönetim paneli. Burada listeleme,filtreleme, ekleme, düzenleme, silme yapılabilir.
 Ekleme ve güncelleme sonrası Redis pub/sub yapısı ile COnfigurationReader a bilgi verilip, configurasyonun güncel hali dbden çekilir.

-İki tane test servisi yazıldı. 
-Service-A -> http://localhost:8080/configurations/get-service-a
-Service-B -> http://localhost:8081/configurations/get-service-b

ConfigurationReader sınıfı içerisinde cacheleme yapıldı. Eğer veri cachelenmiş ise dbye bakmadan getiriyor. Eğer mongodbye erişim yoksa cachete en son çekilen veri geri
dönülür.

Ayrıca constructora verilen parametre ile belli bir sürede db kontrol ediliyor güncellemeler için. Burada lock objecti de kullanıldı.
