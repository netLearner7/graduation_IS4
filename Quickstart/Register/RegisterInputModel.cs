using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace is4.Quickstart.Register
{
    public class RegisterInputModel
    {
        [Required(ErrorMessage = "用户名是必填的")]
        [StringLength(maximumLength:9,MinimumLength =3,ErrorMessage ="用户名长度在3-9位之间")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "邮箱是必填的")]
        [EmailAddress(ErrorMessage ="邮箱格式错误")]
        public string Email { get; set; }
        [Required(ErrorMessage = "密码是必填的")]
        [StringLength(6,ErrorMessage ="密码最小长度为6")]
        public string PassWord { get; set; }

        [Phone(ErrorMessage ="电话号码格式错误")]
        public string PhoneNumber { get; set; }
        public string returnURL { get; set; }
    }
}
