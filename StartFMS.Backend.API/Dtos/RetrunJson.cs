namespace StartFMS.Backend.API.Dtos
{
    public class RetrunJson
    {
        public dynamic Data { get; set; }
        public int HttpCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
