using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFineCore.Support;
using NFineCore.EntityFramework.Models.SystemManage;
using NFineCore.Service.SystemManage;
using NFineCore.EntityFramework.Dtos.SystemManage;

namespace NFineCore.Web.Controllers
{
    public class AccountController : Controller
    {
        UserService userService = new UserService();
        LoginLogService loginLogService = new LoginLogService();

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            OperatorModel operatorModel = OperatorProvider.Provider.GetCurrent();
            LoginLogInputDto loginLogInputDto = new LoginLogInputDto();
            loginLogInputDto.UserId = operatorModel.Id;
            loginLogInputDto.UserName = operatorModel.UserName;
            loginLogInputDto.OperateType = "Logout";
            loginLogInputDto.OperateResult = true;
            loginLogInputDto.OperateTime = System.DateTime.Now;
            loginLogInputDto.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            loginLogInputDto.IpAddressLocation = NetHelper.GetLocation(loginLogInputDto.IpAddress);
            loginLogInputDto.Description = "安全退出。";
            loginLogService.SubmitForm(loginLogInputDto, null);

            OperatorProvider.Provider.RemoveCurrent();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAuthCode()
        {
            return File(GetVerifyCode(), @"image/Gif");
        }
        
        [HttpPost]
        [AllowAnonymous]
        public IActionResult CheckLogin(string username, string password, string verifycode)
        {
            LoginLogInputDto loginLogInputDto = new LoginLogInputDto();
            loginLogInputDto.UserName = username;
            loginLogInputDto.OperateType = "Login";
            loginLogInputDto.OperateTime = System.DateTime.Now;
            loginLogInputDto.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            loginLogInputDto.IpAddressLocation = NetHelper.GetLocation(loginLogInputDto.IpAddress);
            try
            {
                var SessionVerifyCode = HttpContext.Session.GetString("nfinecore_session_verifycode");
                var Md5VerifyCode = Md5.md5(verifycode.ToLower(), 16);
                if (SessionVerifyCode != Md5VerifyCode)
                {
                    throw new Exception("验证码错误，请重新输入。");
                }
                UserOutputDto userOutputDto = userService.CheckLogin(username, password);
                if (userOutputDto != null)
                {
                    loginLogInputDto.UserId = userOutputDto.Id;
                    loginLogInputDto.OperateResult = true;
                    loginLogInputDto.Description = "系统登录，登录成功。";
                    loginLogService.SubmitForm(loginLogInputDto, null);

                    OperatorModel operatorModel = new OperatorModel();
                    operatorModel.Id = userOutputDto.Id;
                    operatorModel.UserName = userOutputDto.UserName;
                    operatorModel.MobilePhone = userOutputDto.MobilePhone;
                    operatorModel.Email = userOutputDto.Email;
                    OperatorProvider.Provider.AddCurrent(operatorModel);
                }
                return Content(new AjaxResult { state = ResultType.success.ToString(), message = "登录成功。" }.ToJson());
            }
            catch (Exception ex)
            {
                loginLogInputDto.OperateResult = false;
                loginLogInputDto.Description = "系统登录，" + ex.Message;
                loginLogService.SubmitForm(loginLogInputDto, null);
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }

        public byte[] GetVerifyCode()
        {
            int codeW = 80;
            int codeH = 30;
            int fontSize = 16;
            string chkCode = string.Empty;
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表，用于验证码 
            string[] font = { "Times New Roman" };
            //验证码的字符集，去掉了一些容易混淆的字符 
            char[] character = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rnd = new Random();
            //生成验证码字符串 
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            //写入Session、验证码加密
            HttpContext.Session.SetString("nfinecore_session_verifycode", Md5.md5(chkCode.ToLower(), 16));
            //创建画布
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            //画噪线 
            for (int i = 0; i < 3; i++)
            {
                int x1 = rnd.Next(codeW);
                int y1 = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            //画验证码字符串 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, fontSize);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 18, (float)0);
            }
            //将验证码图片写入内存流，并将其以 "image/Png" 格式输出 
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                g.Dispose();
                bmp.Dispose();
            }
        }

    }
}