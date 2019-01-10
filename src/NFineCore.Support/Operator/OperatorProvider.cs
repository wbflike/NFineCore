
namespace NFineCore.Support
{
    public class OperatorProvider
    {
        public static OperatorProvider Provider
        {
            get { return new OperatorProvider(); }
        }
        private string LoginUserKey = "nfinecore_login_user";

        public OperatorModel GetCurrent()
        {
            string cookieString = string.Empty;
            StaticHttpContext.Current.Request.Cookies.TryGetValue(LoginUserKey, out cookieString);
            if (!string.IsNullOrEmpty(cookieString))
            {
                OperatorModel operatorModel = new OperatorModel();
                operatorModel = DESEncrypt.Decrypt(cookieString).ToObject<OperatorModel>();
                return operatorModel;
            }
            else
            {
                return null;
            }
        }
        public void AddCurrent(OperatorModel operatorModel)
        {
            StaticHttpContext.Current.Response.Cookies.Append(LoginUserKey, DESEncrypt.Encrypt(operatorModel.ToJson()));
        }
        public void RemoveCurrent()
        {
            StaticHttpContext.Current.Response.Cookies.Delete(LoginUserKey);
        }
    }
}
