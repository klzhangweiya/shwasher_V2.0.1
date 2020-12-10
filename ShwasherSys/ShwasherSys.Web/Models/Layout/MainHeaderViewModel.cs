namespace ShwasherSys.Models.Layout
{
    public class MainHeaderViewModel
    {
        public CurrentUserViewModel UserInfos { get; set; }

        public bool IsMultiTenancyEnabled { get; set; } = false;

        public string GetShownLoginName()
        {
            var userName = "<span id=\"HeaderCurrentUserName\">" + UserInfos.UserName + "</span>";
            return userName;
        }
    }

}