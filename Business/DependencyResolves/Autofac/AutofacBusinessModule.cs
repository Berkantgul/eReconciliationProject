using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Entities;
using Core.Utilities.Interceptions;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolves.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CompanyManager>().As<ICompanyService>();
            builder.RegisterType<EfCompanyDal>().As<ICompanyDal>();

            builder.RegisterType<AccountRecanciliationManager>().As<IAccountRecanciliationService>();
            builder.RegisterType<EfAccountRecanciliationDal>().As<IAccountRecanciliationDal>();

            builder.RegisterType<AccountRecanciliationDetailManager>().As<IAccountRecanciliationDetailService>();
            builder.RegisterType<EfAccountRecanciliationDetailDal>().As<IAccountRecanciliationDetailDal>();

            builder.RegisterType<BaBsRecanciliationManager>().As<IBaBsRecanciliationService>();
            builder.RegisterType<EfBaBsRecanciliationDal>().As<IBaBsRecanciliationDal>();

            builder.RegisterType<BaBsRecanciliationDetailManager>().As<IBaBsRecanciliationDetailService>();
            builder.RegisterType<EfBaBsRecanciliationDetailDal>().As<IBaBsRecanciliationDetailDal>();

            builder.RegisterType<CurrencyManager>().As<ICurrencyService>();
            builder.RegisterType<EfCurrencyDal>().As<ICurrencyDal>();

            builder.RegisterType<CurrencyAccountManager>().As<ICurrencyAccountService>();
            builder.RegisterType<EfCurrencyAccountDal>().As<ICurrencyAccountDal>();

            builder.RegisterType<MailParameterManager>().As<IMailParameterService>();
            builder.RegisterType<EfMailParameterDal>().As<IMailParameterDal>();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JWTHelper>().As<ITokenHelper>();

            builder.RegisterType<MailManager>().As<IMailService>();
            builder.RegisterType<EfMailDal>().As<IMailDal>();

            builder.RegisterType<MailParameterManager>().As<IMailParameterService>();
            builder.RegisterType<EfMailParameterDal>().As<IMailParameterDal>();

            builder.RegisterType<MailTemplateManager>().As<IMailTemplateService>();
            builder.RegisterType<EfMailTemplateDal>().As<IMailTemplateDal>();

            builder.RegisterType<TermsAndConditionManager>().As<ITermsAndConditionService>();
            builder.RegisterType<EfTermsAndConditionDal>().As<ITermsAndConditionDal>();

            builder.RegisterType<ForgotPasswordManager>().As<IForgotPasswordService>();
            builder.RegisterType<EfForgotPasswordDal>().As<IForgotPasswordDal>();

            builder.RegisterType<UserOperationClaimManager>().As<IUserOperationClaimService>();
            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>();


            builder.RegisterType<OperationClaimManager>().As<IOperationClaimService>();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>();

            builder.RegisterType<UserReletionShipManager>().As<IUserReletionShipService>();
            builder.RegisterType<EfUserReletionShipDal>().As<IUserReletionShipDal>();

            builder.RegisterType<UserCompanyManager>().As<IUserCompanyService>();
            builder.RegisterType<EfUserCompanyDal>().As<IUserCompanyDal>();

            builder.RegisterType<UserThemeOptionManager>().As<IUserThemeOptionService>();
            builder.RegisterType<EfUserThemeOptionDal>().As<IUserThemeOptionDal>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
              .EnableInterfaceInterceptors(new ProxyGenerationOptions()
              {
                  Selector = new AspectInterceptorSelector()
              }).SingleInstance();

        }
    }
}
