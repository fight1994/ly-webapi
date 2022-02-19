using LY.IServices.BASE;
using LY.Model.DAO;
using LY.Model.DTO;
using LY.Model.ViewModel;

namespace LY.IServices
{
    public interface IUserServices : IBaseServices<UserInfoDAO>
    {
        UserInfoViewModel GetUserByToken(string token);

        string GetToken(LoginDTO dto);
    }
}
