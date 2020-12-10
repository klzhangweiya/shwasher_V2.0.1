namespace ShwasherSys.Models
{
    public class CurrentUserViewModel
    {
        public long UserId { get; set; }
        public string RealName { get; set; }
        public int UserType { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
}