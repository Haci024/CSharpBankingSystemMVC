using DTOLayer.DTOS.AppUserDTOS;
using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.ValidationRules.AppUserValidation
{
    public class AppUserRegisterValidator:AbstractValidator<AppUserRegisterDto>
    {
        public AppUserRegisterValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad boş ola bilməz!");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyad boş ola bilməz!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifrə boş ola bilməz!"); 
            RuleFor(x => x.Username).NotEmpty().WithMessage("İstifadəçi adı boş ola bilməz!");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email adresi boş ola bilməz!");
			RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Şifrə təkrarı boş ola bilməz!");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Uyğun email ünvanı daxil edin!");
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("Şifrələr eyni deyil!");
            RuleFor(x => x.Name).MinimumLength(2).WithMessage("Ad minimum 2 simvoldan ibarət olmalıdır");
            RuleFor(x => x.Name).MaximumLength(30).WithMessage("Ad maksimum 30 simvoldan ibarət olmalıdır");
        }

    }
}
