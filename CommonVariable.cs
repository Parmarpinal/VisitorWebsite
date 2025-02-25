namespace VisitorWebsite
{

    public class CommonVariable
    {
        private static IHttpContextAccessor _HttpContextAccessor;

        static CommonVariable()
        {
            _HttpContextAccessor = new HttpContextAccessor();
        }


        public static int? UserID()
        {

            if (_HttpContextAccessor.HttpContext.Session.GetString("UserID") == null)
            {
                return null;
            }

            return Convert.ToInt32(_HttpContextAccessor.HttpContext.Session.GetString("UserID"));
        }

        public static string UserName()
        {
            if (_HttpContextAccessor.HttpContext.Session.GetString("UserName") == null)
            {
                return null;
            }

            return _HttpContextAccessor.HttpContext.Session.GetString("UserName");
        }

        public static string ImagePath()
        {
            if (_HttpContextAccessor.HttpContext.Session.GetString("ImagePath") == null)
            {
                return null;
            }
            return _HttpContextAccessor.HttpContext.Session.GetString("ImagePath");
        }
        public static int? OrganizationID()
        {

            if (_HttpContextAccessor.HttpContext.Session.GetString("OrganizationID") == null)
            {
                return null;
            }

            return Convert.ToInt32(_HttpContextAccessor.HttpContext.Session.GetString("OrganizationID"));
        }
        public static int? DepartmentID()
        {

            if (_HttpContextAccessor.HttpContext.Session.GetString("DepartmentID") == null)
            {
                return null;
            }

            return Convert.ToInt32(_HttpContextAccessor.HttpContext.Session.GetString("DepartmentID"));
        }
    }
}
