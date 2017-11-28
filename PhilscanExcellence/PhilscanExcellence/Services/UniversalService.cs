using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using PhilscanExcellence.Models;

namespace PhilscanExcellence.Services
{
    public class UniversalService
    {
        public static string GetRequestor(Guid? _ID, out string message)
        {
            try
            {
                message = "";

                using (var db = new PhilscanExcellenceEntities())
                {
                    var user = db.UserAccount.FirstOrDefault(r => r.ID == _ID);

                    return user.FirstName + " " + user.LastName;
                }
            }
            catch (Exception error)
            {
                message = error.Message;

                return null;
            }
        }

        public static UserModel CurrentUser
        {
            get
            {
                UserModel user = null;

                HttpCookie authCookie_philscan = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

                if(authCookie_philscan != null)
                {
                    FormsAuthenticationTicket authTicket_philscan = FormsAuthentication.Decrypt(authCookie_philscan.Value);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    PrincipalSerializeModel serializeModel = serializer.Deserialize<PrincipalSerializeModel>(authTicket_philscan.UserData);

                    Principal newUser = new Principal(authTicket_philscan.Name);

                    newUser.Username = serializeModel.Username;

                    newUser.SessionID = serializeModel.SessionID;

                    HttpContext.Current.User = newUser;

                    using (var db = new PhilscanExcellenceEntities())
                    {
                        var query = from u in db.UserAccount
                                    where u.Username == newUser.Username
                                    select new UserModel
                                    {
                                        ID = u.ID,
                                        FirstName = u.FirstName,
                                        MiddleInitial = u.MiddleInitial,
                                        LastName = u.LastName,
                                        Department = u.Department,
                                        Type = u.Type,
                                        Status = u.Status,
                                    };

                        user = query.FirstOrDefault();
                    }
                }

                return user;
            }
            set { }
        }
    }
}