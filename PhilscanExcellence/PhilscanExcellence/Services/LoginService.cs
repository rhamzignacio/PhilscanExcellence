using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhilscanExcellence.Models;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace PhilscanExcellence.Services
{
    public class LoginService
    {
        public static void LogoutFromSession(out string message)
        {
            try
            {
                message = "";

                FormsAuthentication.SignOut();
            }
            catch(Exception error)
            {
                message = error.Message;
            }
        }

        public static UserModel ValidateLogin(string _username, string _password, out string message)
        {
            message = "";

            try
            {
                using (var db = new PhilscanExcellenceEntities())
                {
                    var user = db.UserAccount.FirstOrDefault(r => r.Username.ToLower() == _username.ToLower() && r.Password == _password);

                    if (user != null)
                    {
                        if (user.Status == 0)
                            message = "User is currently deactivated";
                        else
                        {
                            UserModel userModel = new UserModel
                            {
                                ID = user.ID,
                                Username = user.Username,
                                FirstName = user.FirstName,
                                MiddleInitial = user.MiddleInitial,
                                LastName = user.LastName,
                                Department = user.Department,
                                Type = user.Type,
                                Status = user.Status,
                            };

                            return userModel;
                        }
                    }
                    else
                        message = "Invalid username or password";

                    return null;
                }
            }
            catch(Exception error)
            {
                message = error.Message;

                return null;
            }
        }

        public static bool LoginToSession(UserModel _user)
        {
            try
            {
                HttpContext.Current.Session["session_status"] = "online";

                PrincipalSerializeModel serializeModel = new PrincipalSerializeModel();

                serializeModel.Username = _user.Username;
                
                serializeModel.Password = _user.Password;

                serializeModel.SessionID = HttpContext.Current.Session.SessionID;

                serializeModel.Status = _user.Status;

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                string userData = serializer.Serialize(serializeModel);

                FormsAuthenticationTicket authenticationPhilscan = new FormsAuthenticationTicket(1, _user.Username, DateTime.Now, DateTime.Now.AddMinutes(30), true, userData);

                string encryptedTicket = FormsAuthentication.Encrypt(authenticationPhilscan);

                HttpCookie authenticationCookie_Philscan = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                HttpResponse response = HttpContext.Current.Response;

                response.Cookies.Add(authenticationCookie_Philscan);

                return true;
            }
            catch { return false; }
        }
    }
}