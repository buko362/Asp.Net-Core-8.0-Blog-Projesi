using ClosedXML.Excel;
using CoreDemo.Areas.Admin.Models;
using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExcelBlogController : Controller
    {
        
        public IActionResult ExportStaticExcelBlogList()
        {
            using (var workbook=new XLWorkbook()) //excelde en üstte yazan excel adı
            {
                var worksheet = workbook.Worksheets.Add("Blog Listesi"); //excelde sayfa1in ismini belirledik
                worksheet.Cell(1, 1).Value = "Blog ID";//birinci satır birinci sütun blog ID olsun 
                worksheet.Cell(1, 2).Value = "Blog Adı";

                int BlogRowCount = 2;//başlangıç değeri 2 çünkü 1. satıra başlık yazılacak
                foreach(var item in GetBlogList())
                {
                    worksheet.Cell(BlogRowCount, 1).Value = item.ID;//blogrowcounttaki göndermiş olduğu değer satır değeri. satırın birinci sütununa idyi yazsın
                    worksheet.Cell(BlogRowCount, 2).Value = item.BlogName;//ikinci sütuna blogname verileri gelsin
                    BlogRowCount++; //değeri bir arttır
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream); //streamden gelen değerle farklı kaydet
                    var content= stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Calisma1.xlsx"); //en sondaki dosya ismi
                }

            }
        }
        public List<BlogModel> GetBlogList() //list türünde blogmodeli kullanan getbloglist isimli metod tanımladık
        {
            List<BlogModel> bm=new List<BlogModel>
            {
                new BlogModel{ID=1,BlogName="C# Programlamaya Giriş"},
                new BlogModel{ID=2,BlogName="Tesla Firmasının Araçları"},
                new BlogModel{ID=3,BlogName="2024 Olimpiyatları"}
            };//burada verileri girdik
            return bm;
        } 
        public IActionResult BlogListExcel() //yukarıdaki kodları tetikleyecek verileri de hemen yukarıdan alacak
        {
            return View();
        }        

        public IActionResult ExportDynamicExcelBlogList()
        {
            using (var workbook = new XLWorkbook()) //excelde en üstte yazan excel adı
            {
                var worksheet = workbook.Worksheets.Add("Blog Listesi"); //excelde sayfa1in ismini belirledik
                worksheet.Cell(1, 1).Value = "Blog ID";//birinci satır birinci sütun blog ID olsun 
                worksheet.Cell(1, 2).Value = "Blog Adı";

                int BlogRowCount = 2;//başlangıç değeri 2 çünkü 1. satıra başlık yazılacak
                foreach (var item in BlogTitleList())
                {
                    worksheet.Cell(BlogRowCount, 1).Value = item.ID;//blogrowcounttaki göndermiş olduğu değer satır değeri. satırın birinci sütununa idyi yazsın
                    worksheet.Cell(BlogRowCount, 2).Value = item.BlogName;//ikinci sütuna blogname verileri gelsin
                    BlogRowCount++; //değeri bir arttır
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream); //streamden gelen değerle farklı kaydet
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Calisma2.xlsx"); //en sondaki dosya ismi
                }

            }
        }
        public List<BlogModel2> BlogTitleList()
        {
            List<BlogModel2> bm2 = new List<BlogModel2>();
            using (var c = new Context())
            {
                bm2 = c.Blogs.Select(x => new BlogModel2
                {
                    ID=x.BlogID,
                    BlogName=x.BlogTitle
                }).ToList();
            }
            return bm2;
        }
        public IActionResult BlogTitleListExcel()
        {
            return View();  
        }
    }
}
