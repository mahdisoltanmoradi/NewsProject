using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs.User
{

    #region Login

    public class LoginViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0}را وارد کنید ")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1}کراکتر باشد.")]
        public string Email { get; set; }

        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "لطفا {0}را وارد کنید ")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1}کراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
    #endregion

    #region Register

    public class RegisterViewModel
    {
        //[Display(Name = "نام کاربری")]
        //[Required(ErrorMessage = "لطفا {0}را وارد کنید ")]
        //[MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1}کراکتر باشد.")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0}را وارد کنید ")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1}کراکتر باشد.")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0}را وارد کنید ")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1}کراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور ")]
        [Required(ErrorMessage = "لطفا {0}را وارد کنید ")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1}کراکتر باشد.")]
        [Compare("Password")]
        public string RePassword { get; set; }
    }
    #endregion

    #region ForgotPassword

    public class ForgotPasswordViewModel
    {

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

    }


    public class ResetForgotPasswordViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0}را وارد کنید ")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1}کراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور ")]
        [Required(ErrorMessage = "لطفا {0}را وارد کنید ")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1}کراکتر باشد.")]
        [Compare("Password")]
        public string RePassword { get; set; }
    }

    #endregion


}
