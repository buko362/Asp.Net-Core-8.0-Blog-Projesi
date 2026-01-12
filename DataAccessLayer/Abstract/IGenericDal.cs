using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    //burada yazığım şeyler diğerlerini de etkileyeceği için tekrar tekrar tanımlamak zorunda kalmayacağız diğer entitylerde
    public interface IGenericDal<T> where T:class  //T bir classa ait bütün nitelikleri kullanacak   //T entitynin karşılığı , dışarıdan gönderdiğimiz bir entity
    {
        void Insert(T t);//ekleme, T türünde t isminde bir entityi çağırdık 
        void Delete(T t);
        void Update(T t);
        List<T> GetListAll();//listeleme
        T GetByID(int id);//idye göre çağır
        List<T> GetListAll(Expression<Func<T, bool>> filter);// şartlı listeleme işlemlerinded kullanılan bir yapı
                               //gönderilen T değeri, çıkış değeri>>parametre ismi  
    }
}
