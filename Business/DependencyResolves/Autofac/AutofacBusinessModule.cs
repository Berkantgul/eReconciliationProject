using Autofac;
using Business.Abstract;
using Business.Concrete;
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
        }
    }
}
