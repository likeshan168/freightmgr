﻿using System.Net;
using System.Net.Http.Formatting;
using Allocation.Modules.Common;

namespace Allocation.Membership.Pages
{
    using Administration.Repositories;
    using Common;
    using Serenity;
    using Serenity.Abstractions;
    using Serenity.Services;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Net.Http;

    [RoutePrefix("Account"), Route("{action=index}")]
    public partial class AccountController : Controller
    {
        public static bool UseAdminLTELoginBox = false;

        [HttpGet]
        public ActionResult Login(int? denied, string activated, string returnUrl)
        {
            if (denied == 1)
                return View(MVC.Views.Errors.AccessDenied,
                    (object) (FormsAuthentication.LoginUrl + "?returnUrl=" + Uri.EscapeDataString(returnUrl)));
            ViewData["Activated"] = activated;
            ViewData["HideLeftNavigation"] = true;

            if (UseAdminLTELoginBox)
                return View(MVC.Views.Membership.Account.AccountLogin_AdminLTE);
            else
                return View(MVC.Views.Membership.Account.AccountLogin);
        }

        [HttpGet]
        public ActionResult AccessDenied(string returnURL)
        {
            ViewData["HideLeftNavigation"] = !Authorization.IsLoggedIn;

            return View(MVC.Views.Errors.AccessDenied, (object) returnURL);
        }

        [HttpPost, JsonFilter]
        public Result<ServiceResponse> Login(LoginRequest request)
        {
            return this.ExecuteMethod(() =>
            {
                request.CheckNotNull();

                if (string.IsNullOrEmpty(request.Username))
                    throw new ArgumentNullException("username");

                var username = request.Username;

                if (Dependency.Resolve<IAuthenticationService>().Validate(ref username, request.Password))
                {
                    CheckTwoFactorAuthentication(username, request);

                    WebSecurityHelper.SetAuthenticationTicket(username, false);
                    return new ServiceResponse();
                }

                throw new ValidationError("AuthenticationError", Texts.Validation.AuthenticationError);
            });
        }

        [HttpPost, JsonFilter]
        public HttpResponseMessage LoginForApp(LoginRequest request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            LoginResponse response = new LoginResponse();
            var formatter = new JsonMediaTypeFormatter();
            formatter.SerializerSettings =
                new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                };
            if (request == null)
            {
                response.IsSuccess = false;
                response.Result = "请求的数据不能为空";
                responseMessage.StatusCode = HttpStatusCode.BadRequest;
               
                responseMessage.Content = new ObjectContent<LoginResponse>(response,formatter);

                return responseMessage;
            }
            if (String.IsNullOrWhiteSpace(request.Username))
            {
                responseMessage.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Result = "用户名不能为空";
                responseMessage.Content = new ObjectContent<LoginResponse>(response, formatter,"application/json");
                return responseMessage;
            }
           

            var username = request.Username;

            if (Dependency.Resolve<IAuthenticationService>().Validate(ref username, request.Password))
            {
                CheckTwoFactorAuthentication(username, request);

                WebSecurityHelper.SetAuthenticationTicket(username, false);
                response.IsSuccess = true;
                response.Result = "登录成功";
                responseMessage.StatusCode = HttpStatusCode.OK;
                responseMessage.Content = new ObjectContent<LoginResponse>(response, formatter);
                return responseMessage;
            }
            
            response.IsSuccess = false;
            response.Result = "用户名或密码不正确";
            responseMessage.StatusCode = HttpStatusCode.BadRequest;
            responseMessage.Content = new ObjectContent<LoginResponse>(response, formatter);
            return responseMessage;
        }

        [Serializable]
        private class TwoFactorData
        {
            public string Username { get; set; }
            public int RetryCount { get; set; }
            public int TwoFactorCode { get; set; }
        }

        private void CheckTwoFactorAuthentication(string username, LoginRequest request)
        {
            bool isTwoFactorReq = !string.IsNullOrEmpty(request.TwoFactorGuid) || request.TwoFactorCode != null;

            if (isTwoFactorReq)
            {
                Check.NotNullOrEmpty(request.TwoFactorGuid, "twoFactorGuid");
                Check.NotNull(request.TwoFactorCode, "twoFactorCode");

                var key = "TwoFactorAuth:" + request.TwoFactorGuid;
                var data = DistributedCache.Get<TwoFactorData>(key);
                if (data == null || data.Username == null || data.Username != username)
                    throw new ValidationError("Can't validate credentials. Please try login again!");

                data.RetryCount++;
                if (data.RetryCount > 3)
                {
                    DistributedCache.Set<TwoFactorData>(key, null);
                    throw new ValidationError("Can't validate credentials. Please try login again!");
                }
                else
                {
                    DistributedCache.Set(key, data);
                }

                if (data.TwoFactorCode != request.TwoFactorCode)
                    throw new ValidationError("Validation code is invalid. Please check and try again.");

                // login success. clear to not let same two factor guid/two factor code two be reused later
                DistributedCache.Set<TwoFactorData>(key, null);

                return;
            }

            var user = Dependency.Resolve<IUserRetrieveService>().ByUsername(username) as UserDefinition;
            if (user != null &&
                ((user.TwoFactorAuth == Administration.TwoFactorAuthType.SMS &&
                  user.MobilePhoneVerified &&
                  UserRepository.IsValidPhone(user.MobilePhoneNumber)) ||
                 (user.TwoFactorAuth == Administration.TwoFactorAuthType.Email)))
            {
                var env = Config.Get<EnvironmentSettings>();

                // you may disable two factor auth when <compilation debug="true" /> in web.config, by uncommenting lines below
                // if (HttpContext.IsDebuggingEnabled)
                //    return;

                var md5 = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(request.Password ?? ""));
                var data = new TwoFactorData
                {
                    RetryCount = 0,
                    Username = username,
                    TwoFactorCode = new Random().Next(9000) + 1000
                };

                // this is to prevent users from sending too many SMS in a certain time interval
                var throttler = new Throttler("TwoFactorAuthThrottle:" + username, TimeSpan.FromMinutes(5), 10);
                if (!throttler.Check())
                    throw new ValidationError(
                        "Can't proceed with two factor authentication. You are over your validation limit!");

                var twoFactorGuid = Guid.NewGuid().ToString("N");

                string authenticationMessage;
                if (user.TwoFactorAuth == Administration.TwoFactorAuthType.SMS)
                {
                    var mobile = user.MobilePhoneNumber.Trim();
                    Dependency.Resolve<Administration.ISMSService>().Send(
                        phoneNumber: mobile,
                        text: "Please use code " + data.TwoFactorCode + " for Allocation login.",
                        reason: "Sent by Allocation system for two factor authenication by SMS (" + user.Username +
                                ")");

                    // mask mobile number
                    mobile = mobile.Substring(0, 2) + new string('*', mobile.Length - 4) +
                             mobile.Substring(mobile.Length - 2, 2);
                    authenticationMessage = "Please enter code sent to your mobile phone with number " + mobile +
                                            " in <span class='counter'>{0}</span> seconds." +
                                            ((Dependency.Resolve<Administration.ISMSService>() is Administration
                                                .FakeSMSService)
                                                ? " (You can find a text file under App_Data/SMS directory, as you haven't configured SMS service yet)"
                                                : "");
                }
                else
                {
                    EmailHelper.Send(
                        address: user.Email,
                        subject: "Your two-factor authentication code for Allocation login",
                        body: "Please use code " + data.TwoFactorCode + " for Allocation login.");
                    authenticationMessage = "Please enter code sent to your e-mail adress in {0} seconds." +
                                            " (If you didn't configure an SMTP server, you can find e-mails under App_Data/Mail directory";
                }

                DistributedCache.Set("TwoFactorAuth:" + twoFactorGuid, data, TimeSpan.FromMinutes(2));
                throw new ValidationError("TwoFactorAuthenticationRequired",
                    authenticationMessage + "|" + twoFactorGuid, "Two factor authentication is required!");
            }
        }

        private ActionResult Error(string message)
        {
            return View(MVC.Views.Errors.ValidationError,
                new ValidationError(Texts.Validation.InvalidResetToken));
        }

        public ActionResult Signout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return new RedirectResult("~/");
        }
    }
}