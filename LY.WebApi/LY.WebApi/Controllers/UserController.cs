using LY.IServices;
using LY.Model.DTO;
using LY.Model.Share;
using LY.Model.ViewModel;
using LY.WebApi.Controllers.BASE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LY.WebApi.Controllers
{
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private IUserServices iuserServices;

        public UserController(IUserServices _iUserServices)
        {
            this.iuserServices = _iUserServices;
        }

        [HttpPost("Login"), AllowAnonymous]
        public ResponseModel<string> Login([FromForm] LoginDTO dto)
        {
            var status = string.Empty;
            var message = string.Empty;
            var token = string.Empty;

            token = iuserServices?.GetToken(dto);

            if (!string.IsNullOrWhiteSpace(token))
            {
                status = "Success";
            }
            else
            {
                status = "Failure";
                message = "登录失败";
            }

            return new ResponseModel<string>()
            {
                Status = status,
                Message = message,
                Data = token,
            };
        }

        [HttpGet("GetUserInfoByToken")]
        public ResponseModel<UserInfoViewModel> GetUserInfoByToken()
        {
            var status = string.Empty;
            var message = string.Empty;
            UserInfoViewModel userinfo = null;

            var authorization = this.Request.Headers["Authorization"];
            var token = authorization.ToString().Replace("Bearer ", "");
            userinfo = iuserServices?.GetUserByToken(token);

            if (userinfo != null)
            {
                status = "Success";
            }
            else
            {
                status = "Failure";
                message = "获取用户信息失败";
            }

            return new ResponseModel<UserInfoViewModel>()
            {
                Status = status,
                Message = message,
                Data = userinfo,
            };
        }
    }
}
