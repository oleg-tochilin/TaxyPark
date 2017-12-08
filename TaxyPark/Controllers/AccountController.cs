using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaxyPark.Models;

namespace TaxyPark.Controllers
{
    public class AccountController : BaseController
    {
      string CreateMD5(string s)
      {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
          byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(s);
          byte[] hashBytes = md5.ComputeHash(inputBytes);
            
          StringBuilder sb = new StringBuilder();
          for (int i = 0; i < hashBytes.Length; i++)
          {
            sb.Append(hashBytes[i].ToString("X2"));
          }
          return sb.ToString();
        }
      }
      
      [AllowAnonymous]
      public ActionResult Login()
      {
        return View();
      }


      [AllowAnonymous]
      [HttpPost]
      public JsonResult EnterUser(UserLogin user)
      {
        if(user!=null && !String.IsNullOrEmpty( user.Login))
        {
          var u = db.Users.Where(x=>x.Login == user.Login).FirstOrDefault();

          if(u!=null)
          {
            if(u.Role==UserRole.New)
            {
              return Json(new { IsSuccess = false, Message = "Ваша регистрация еще не подтверждена администратором"});
            }
            else
            {
              if(u.Password==CreateMD5(user.Password))
              {
                FormsAuthentication.SetAuthCookie(user.Login,true);
                return Json(new { IsSuccess = true} );
              }
            }
          }
        }

        return Json(new { IsSuccess = false, Message = "Неверный логин или пароль"});
      }

      public ActionResult Logout()
      {
        FormsAuthentication.SignOut();
        Session.Abandon();

        

        return RedirectToAction("Index","Home");
      }


      [AllowAnonymous]
      public ActionResult Register(UserRegister login)
      {
        return View();
      }


      [AllowAnonymous]
      public JsonResult Check()
      {
        if(!db.Users.Any(x=>x.Role==UserRole.Admin))
        {
          return Json(new {IsNewAdmin = true,Message = "Поскольку это первый запуск приложения, будет создана учетная запись с ролью 'Администратор'"},JsonRequestBehavior.AllowGet);
        }

        return Json(new {IsNewAdmin = false, Message = String.Empty},JsonRequestBehavior.AllowGet);
      }

      [AllowAnonymous]
      [HttpPost]
      public JsonResult CreateUser(UserRegister user)
      {
        
        if(user!=null && !String.IsNullOrEmpty(user.Name) && !String.IsNullOrEmpty(user.Password))
        {
          if(user.Password==user.ConfirmPassword)
          {

            if(db.Users.Any(x=>x.Login==user.Login))
            {
              return Json(new {IsSuccess = false,Message = "Такой логин уже существует. Выберите другой."});
            }

            User newUser=new Models.User
            {
              Name = user.Name,
              Login = user.Login,
              Password = CreateMD5(user.Password),
              Role = UserRole.New
            };

            //первый пользователь будет админом
            if(!db.Users.Any(x=>x.Role==UserRole.Admin))
            {
              newUser.Role=UserRole.Admin;
            }
            
            db.Users.Add(newUser);
            db.SaveChanges();


            if(newUser.Role==UserRole.Admin)
            {
              FormsAuthentication.SetAuthCookie(user.Login,true);
            }

            return Json(new {RedirectUrl=newUser.Role==UserRole.Admin ? "/" : "/Account/Login",IsSuccess = true,Message = String.Empty});
          }
          
          return Json(new {IsSuccess = false,Message = "Пароли не совпадают"});
        }

        return Json(new {IsSuccess = false,Message = String.Empty});
      }
    }
}