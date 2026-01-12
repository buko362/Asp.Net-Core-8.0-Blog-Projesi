using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class BlogValidator:AbstractValidator<Blog>
    {
        public BlogValidator() //ctor
        {
            RuleFor(x=>x.BlogTitle).NotEmpty().WithMessage("Blog başlığı boş bırakılamaz.");
            RuleFor(x=>x.BlogContent).NotEmpty().WithMessage("Blog içeriği boş bırakılamaz.");
            RuleFor(x=>x.BlogContent).MinimumLength(100).WithMessage("Blog içeriği en az 100 karakter içerelidir.");
            RuleFor(x=>x.BlogImage).NotEmpty().WithMessage("Blog görseli boş bırakılamaz.");
            RuleFor(x => x.BlogTitle).MaximumLength(150).WithMessage("En fazla 150 karakter girebilirsiniz");
            RuleFor(x => x.BlogTitle).MinimumLength(5).WithMessage("En az 5 karakter girebilirsiniz");
        }
    }
}
