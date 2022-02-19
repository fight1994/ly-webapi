using SqlSugar;

namespace LY.Model.DAO
{
    [SugarTable("UserInfo")]
    public class UserInfoDAO
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }

        [SugarColumn(ColumnName = "account")]
        public string UserName { get; set; }

        [SugarColumn(ColumnName = "password")]
        public string PassWord { get; set; }

        [SugarColumn(ColumnName = "email")]
        public string Email { get; set; }

        [SugarColumn(ColumnName = "address")]
        public string Address { get; set; }

        [SugarColumn(ColumnName = "last_login")]
        public DateTime? LastLoginDate { get; set; }

        [SugarColumn(ColumnName = "create_date")]
        public DateTime CreateDate { get; set; }

        [SugarColumn(ColumnName = "edit_date")]
        public DateTime? EditDate { get; set; }

        [SugarColumn(ColumnName = "remark")]
        public string Remark { get; set; }
    }
}
