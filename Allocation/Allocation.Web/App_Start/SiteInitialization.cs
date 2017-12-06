namespace Allocation
{
    using Administration;
    using Common.Services;
    using Serenity;
    using Serenity.Abstractions;
    using Serenity.Data;
    using Serenity.Web;
    using System;
    using System.Configuration;

    public static partial class SiteInitialization
    {
        public static void ApplicationStart()
        {
            try
            {
                SqlSettings.AutoQuotedIdentifiers = true;
                Serenity.Web.CommonInitialization.Run();

                var registrar = Dependency.Resolve<IDependencyRegistrar>();
                registrar.RegisterInstance<IAuthorizationService>(new Administration.AuthorizationService());
                registrar.RegisterInstance<IAuthenticationService>(new Administration.AuthenticationService());
                registrar.RegisterInstance<IPermissionService>(new LogicOperatorPermissionService(new Administration.PermissionService()));
                registrar.RegisterInstance<IUserRetrieveService>(new Administration.UserRetrieveService());
                registrar.RegisterInstance<ISMSService>(new Administration.FakeSMSService());

                if (!ConfigurationManager.AppSettings["LDAP"].IsTrimmedEmpty())
                    registrar.RegisterInstance<IDirectoryService>(new LdapDirectoryService());

                if (!ConfigurationManager.AppSettings["ActiveDirectory"].IsTrimmedEmpty())
                    registrar.RegisterInstance<IDirectoryService>(new ActiveDirectoryService());

                InitializeExceptionLog();
                BackgroundTaskManager.Register(new MailingBackgroundTask());
                BackgroundTaskManager.Initialize();
            }
            catch (Exception ex)
            {
                ex.Log();
                throw;
            }

            foreach (var databaseKey in databaseKeys)
            {
                EnsureDatabase(databaseKey);
                RunMigrations(databaseKey);
            }
        }

        public static void ApplicationEnd()
        {
        }
    }
}