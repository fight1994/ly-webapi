namespace LY.Model.Share
{
    public class PageModel<T>
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 10;

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> Data { get; set; }
    }
}
