using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IBlogDal: IGenericDal<Blog>
    {
        List<Blog> GetListWithCategory();//sadece bloglara özel
        List<Blog> GetListWithCategoryByWriter(int id);//yazara göre categoriyle birlikte getir,dışarıdan da bir id parametresi alacak 
    }
}
