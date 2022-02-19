using LY.Common.AttributeExtend;
using LY.Common.Helper;
using LY.IServices;
using LY.Model.DAO;
using LY.Model.DTO;
using LY.Model.Share;
using LY.Model.ViewModel;
using LY.Repository.Interface;
using LY.Services.BASE;
using Microsoft.Extensions.DependencyInjection;

namespace LY.Services
{
    [Export(typeof(IUserServices), Lifetime = ServiceLifetime.Scoped)]
    public class UserServices : BaseServices<UserInfoDAO>, IUserServices
    {
        IBaseRepository<UserInfoDAO> userInfoDAL;

        public UserServices(IBaseRepository<UserInfoDAO> _userInfoDAL)
        {
            this.userInfoDAL = _userInfoDAL;
        }

        public string GetToken(LoginDTO dto)
        {
#if DEBUG
            if (dto?.Account == "ly" && dto?.Password == "shuaige")
            {
                return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJMWSIsIlVzZXJJZCI6Imx5IiwiVXNlck5hbWUiOiJzaHVhaWdlIiwiZXhwIjoxNjQ1MTE4NDQ3fQ.cBed_3_sZc2jhpH5KjI30lCVD6Mpt_zAPd3COlQO2J8";
            }
#endif
            string result = string.Empty;
            var userInfo = GetUserByAccountPwd(dto.Account, dto.Password);
            if (userInfo != null)
            {
                var appsettingsHelper = new AppsettingsHelper();
                var jwtConfig = appsettingsHelper.Get<JwtConfigModel>("JWT");
                result = JWTHelper.CreateToken(jwtConfig, new JwtUserInfo() { UserId = userInfo.UserId, UserName = userInfo.UserName });
            }

            return result;
        }

        public UserInfoViewModel GetUserByToken(string token)
        {
            var jwtUserInfo = JWTHelper.TokenToModel(token);
            var userInfo = GetUserById(jwtUserInfo?.UserId);
            return userInfo;
        }

        private UserInfoViewModel GetUserByAccountPwd(string account, string pwd)
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(pwd)) return null;

            UserInfoViewModel result = null;
            try
            {
                var userDao = userInfoDAL.Query(o => o.UserName == account && o.PassWord == pwd).Result?.FirstOrDefault();
                result = new UserInfoViewModel()
                {
                    UserId = userDao?.Id,
                    UserName = userDao?.UserName,
                    Address = userDao?.Address,
                    Email = userDao?.Email
                };
            }
            catch (Exception)
            {

            }

            return result;
        }

        private UserInfoViewModel GetUserById(string userId)
        {
#if DEBUG
            if (userId == "ly")
            {
                return new UserInfoViewModel() { UserId = "ly", UserName = "shuaige" };
            }
#endif
            if (string.IsNullOrWhiteSpace(userId)) return null;

            UserInfoViewModel result = null;
            try
            {
                var userDao = userInfoDAL.Query(o => o.Id == userId).Result?.FirstOrDefault();
                result = new UserInfoViewModel()
                {
                    UserId = userDao?.Id,
                    UserName = userDao?.UserName,
                    Address = userDao?.Address,
                    Email = userDao?.Email
                };
            }
            catch (Exception)
            {

            }

            return result;
        }
    }
}
