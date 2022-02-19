using LY.Common.AttributeExtend;
using LY.Model.DAO;
using LY.Repository.BASE;
using LY.Repository.Interface;

namespace LY.Repository
{
    [Export(typeof(IUserRepository))]
    public class UserRepository : BaseRepository<UserInfoDAO>, IUserRepository
    {
        public UserRepository()
        {

        }
    }
}
