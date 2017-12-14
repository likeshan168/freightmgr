using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Allocation.Administration.Entities;
using Allocation.Administration.Repositories;
using Allocation.Allot.Entities;
using Allocation.Allot.Repositories;
using Allocation.Common;
using Allocation.Membership;
using Allocation.Modules.Common;
using Serenity;
using Serenity.Abstractions;
using Serenity.Data;
using Serenity.Services;
using Authorization = Serenity.Authorization;

namespace Allocation.Controllers
{
    public class LoginController : BaseApiController
    {
        private DeclarationDataRepository declarationDataRepository = new DeclarationDataRepository();
        public HttpResponseMessage Post(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            if (request == null)
            {
                response.IsSuccess = false;
                response.Result = "请求的数据不能为空";

                return BuildErrorResult(HttpStatusCode.BadRequest, response);
            }
            if (String.IsNullOrWhiteSpace(request.Username))
            {
                response.IsSuccess = false;
                response.Result = "用户名不能为空";
                return BuildErrorResult(HttpStatusCode.BadRequest, response);
            }


            var username = request.Username;


            if (Dependency.Resolve<IAuthenticationService>().Validate(ref username, request.Password))
            {
                CheckTwoFactorAuthentication(username, request);

                WebSecurityHelper.SetAuthenticationTicket(username, false);
                response.IsSuccess = true;
                response.Result = "登录成功";
                //var user = (UserDefinition) Authorization.UserDefinition;
                if (Dependency.Resolve<IUserRetrieveService>().ByUsername(username) is UserDefinition user)
                {
                    response.TenantId = user.TenantId;
                    string sql = $"select * from [dbo].[UserRoles] where UserId = {user.UserId}";
                    int roleId = 0;
                    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
                    {
                        var userRole = conn.Query<UserRoleRow>(sql).FirstOrDefault();
                        if (userRole != null && userRole.RoleId != null) roleId = userRole.RoleId.Value;
                    }
                   
                    //var userRole = new UserRoleRepository().List(SqlConnections.NewFor<UserRoleRow>(), new Administration.UserRoleListRequest{UserID = user.UserId}).Entities.First();
                    if (username == "admin" || roleId == 1)
                    {
                        response.IsAdmin = true;
                    }
                }


                return BuildSuccessResult(HttpStatusCode.OK, response);
            }

            response.IsSuccess = false;
            response.Result = "用户名或密码不正确";
            return BuildErrorResult(HttpStatusCode.BadRequest, response);
        }

        public HttpResponseMessage Get()
        {
            var response = new GetListResponse<DeclarationDataRow>();
            try
            {
                var list = declarationDataRepository.GetList(new ListRequest());
                response.IsSuccess = true;
                response.Message = "数据获取成功";
                response.Entities = list;
                return BuildSuccessResult(HttpStatusCode.OK, response);
            }
            catch (ValidationError err)
            {
                err.Log();
                response.IsSuccess = false;
                response.Message = "登录已过期，请重新登录";
                return BuildSuccessResult(HttpStatusCode.Forbidden, response);
            }
            catch (Exception ex)
            {
                ex.Log();
                response.IsSuccess = false;
                response.Message = "服务器发生异常";
                return BuildSuccessResult(HttpStatusCode.InternalServerError, response);
            }
        }

        public HttpResponseMessage Put(IList<DeclarationDataRow> declarationDataRows)
        {
            return BuildSuccessResult(HttpStatusCode.OK, declarationDataRepository.Update(declarationDataRows));
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
                    throw new Serenity.Services.ValidationError("Can't validate credentials. Please try login again!");

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
        [Serializable]
        private class TwoFactorData
        {
            public string Username { get; set; }
            public int RetryCount { get; set; }
            public int TwoFactorCode { get; set; }
        }
    }
}
