namespace LY.Model.Share
{
    public class ResponseModel<T>
    {
        public string Status { get; set; }

        public string Message { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { set; get; }

        public int DataCount { get; set; }

        public T Data { get; set; }
    }
}
