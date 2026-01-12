##--ASP.NET Core 8.0 Blog Projesi--##

Bu proje, değerli hocam Murat Yücedağ'ın "Asp.Net Core 5.0 Proje Kampı" aracılığı ile ASP.NET Core 8.0 kullanılarak geliştirilmiş, kullanıcı, yazar ve admin rollerine sahip kapsamlı bir blog platformudur.
Kullanıcıların blogları görüntüleyebildiği, yorum yapabildiği ve içerik üreticileriyle etkileşime geçebildiği; yazarların ise kendi bloglarını ve profillerini yönetebildiği, adminlerin tüm sistemi kontrol edebildiği bir yapı sunmaktadır.

Projede blogların puanlanması, yorumlanması, kullanıcılar arası mesajlaşma, rol bazlı yetkilendirme ve dinamik yönetim panelleri gibi gerçek hayat senaryolarında karşılaşılan pek çok özellik uygulanmıştır.

--!--ÖNEMLİ--!--

BU REPO TOPLAM 3 PROJEDEN OLUŞUYOR. "BlogApiDemo" ve "jwt_core_proje_kampi" EĞİTİM İÇERİĞİNDE OLDUĞU İÇİN EKLENMİŞTİR. 

##--Projede Kullanılan Yapılar ve Özellikler--##

ASP.NET Core 8.0 (MVC) kullanılarak geliştirilmiştir.
Katmanlı Mimari (N-Tier Architecture) uygulanmıştır.
* Entity Layer
* Data Access Layer
* Business Layer
* UI Layer

SOLID prensipleri dikkate alınarak geliştirme yapılmıştır.

##--Kimlik Doğrulama ve Yetkilendirme--##
ASP.NET Core Identity kullanılarak kullanıcı, yazar ve admin işlemleri yönetilmiştir.
Authentication & Authorization mekanizmaları uygulanmıştır.
Gerekli alanlarda [Authorize] attribute’ları kullanılmıştır.
Kullanıcı şifreleri hashleme ve saltlama algoritmaları ile güvenli şekilde saklanmaktadır.
Rol bazlı yetkilendirme ile:
* User
* Writer
* Admin
ayrımı sağlanmıştır.

##--Blog ve İçerik Yönetimi--##
* Blog ekleme, güncelleme, silme işlemleri
* Bloglara puan verme ve yorum yapma
* Yazarın kendi bloglarını yönetebildiği özel panel
* Yazarın diğer yazılarını listeleyen dinamik bileşenler
* Blog–Yorum ilişkilerinde veri bütünlüğü ve cascade delete yönetimi

##--Mesajlaşma ve Bildirim Sistemi--##
* Kullanıcılar, yazarlar ve adminler arasında çalışan dahili mesajlaşma sistemi
* Inbox / Sendbox yapısı
* Mesaj detay ve yanıtla (reply) özelliği
* Bildirim mantığı ile yeni mesajların görüntülenmesi

##--Dinamik Yapılar--##
ViewComponent kullanılarak:
* Son bloglar
* Yazarın diğer yazıları
* Bildirim alanları
gibi tekrar eden alanlar dinamik hâle getirilmiştir.

AJAX kullanılarak:
Admin panelinde dinamik işlemler
Sayfa yenilenmeden veri güncellemeleri
sağlanmıştır.

##--Veritabanı ve Performans--##
Entity Framework Core (Code First) yaklaşımı kullanılmıştır.
Migration yapısı projeye dâhildir.
LINQ sorguları ile veri işlemleri gerçekleştirilmiştir.
Async / Await ve senkron işlemler performans dikkate alınarak birlikte kullanılmıştır.

##--Güvenlik ve API Yapıları--##
JSON Web Token (JWT) kullanılarak token tabanlı kimlik doğrulama senaryoları uygulanmıştır.
Yetkisiz erişimler için 403 Access Denied sayfaları oluşturulmuştur.
Kullanıcı-yazar Admin paneline girse bile yetkisi olmadığı sayfaları göremez.

##---PROJE GÖRÜNÜMÜ---##

#-Kullanıcı Arayüzü-#
<p align="center">
  <img src="images/login.png" width="600">
  <img src="images/blog1.png" width="600">
  <img src="images/blogreadall.png" width="600">
  <img src="images/comment.png" width="600">
  <img src="images/details.png" width="600">
  <img src="images/about.png" width="600">
  <img src="images/contact.png" width="600">
</p>

#-Yazar Paneli-#
<p align="center">
  <img src="images/writerdashboard.png" width="600">
  <img src="images/writerdashboard2.png" width="600">
  <img src="images/writereditprofile.png" width="600">
  <img src="images/bloglistbywriter.png" width="600">
  <img src="images/editblog.png" width="600">
  <img src="images/blogadd.png" width="600">
</p>

#-Yazar Paneli Mesajlaşma Sistemi-#
<p align="center">
  <img src="images/inbox.png" width="600">
  <img src="images/messagedetails.png" width="600">
  <img src="images/sendbox.png" width="600">
  <img src="images/sendmessage.png" width="600">
  <img src="images/allnotification.png" width="600">
</p>

#-Admin Paneli-#
<p align="center">
  <img src="images/admindashboard.png" width="600">
  <img src="images/adminprofile.png" width="600">
  <img src="images/adminchart.png" width="600">
  <img src="images/admininbox.png" width="600">
  <img src="images/adminsendbox.png" width="600">
  <img src="images/adminmaildetails.png" width="600">
  <img src="images/admincomposemessage.png" width="600">
  <img src="images/admincomposemessagereply.png" width="600">
</p>

#-Admin Paneli ~ İçerik & Yetkilendirme-#
<p align="center">
  <img src="images/admincategory.png" width="600">
  <img src="images/adminaddcategory.png" width="600">
  <img src="images/adminblogindex.png" width="600">
  <img src="images/admincomment.png" width="600">
  <img src="images/adminwriterprofilelist.png" width="600">
  <img src="images/adminbloglistbywriter.png" width="600">
  <img src="images/adminajax.png" width="600">
  <img src="images/adminrole.png" width="600">
  <img src="images/adminuserrolelist.png" width="600">
  <img src="images/admin403.png" width="600">
</p>
