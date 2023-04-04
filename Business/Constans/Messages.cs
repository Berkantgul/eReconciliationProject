using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contans
{
    public class Messages
    {
        // Company
        public static string AddedCompany = "Şirket kaydı başarıyla tamamlandı"; 
        public static string CompanyAlreadyExists = "Böyle bir şirket mevcut.";
        public static string UpdateCompany = "irket kaydı başarıyla güncellendi.";

        // User
        public static string UserNotFound = "Kullanıcı kaydı bulunamadı";
        public static string PasswordError = "Kullanıcı şifre yanlış";
        public static string SuccessfulLogin = "Giriş başarılı";
        public static string UserRegistered = "Kullanıcı eklendi.";
        public static string ExistsAlreadyUser = "Böyle bir kişi zaten mevcut.";
        public static string ConfirmValue = "Mail onaylama işlemi başarılı.";
        public static string ChangedPassword = "Şifre başarılı bir şekilde güncellendi.";
        public static string UpdatedUser = "Kullanıcı kaydı başarılı bir şekilde güncellendi.";
        public static string ChangeStatus = "Kullanıcı durum güncellendi.";
        public static string DeleteUserCompany = "Kullanıcının şirket ile ilişiği kesilmiştir.";
        public static string AddedUserCompany = "Kullanıcının şirket ile bağlantısı oluşturulmuştur.";
        public static string UserDelete = "Kullanıcı silme işlemi başarılı.";

        // Mail 
        public static string MailParameterUpdated = "Mail parametrelerini güncellendi.";
        public static string MailSendSuccess = "Başarılı bir şekilde mail gönderildi.";
        public static string MailConfirmSendSuccessfull = "Onay maili tekrar gönderildi.";
        public static string MailAlreadyConfirm = "Hesabınız zaten onaylı.";
        public static string MailConfirmationHasNotExpirated = "Hali hazırda onay maili mevcut 5 dakika sonra tekrar deneyin";
        
        //Mail Template
        public static string MailTemplateAdd = "Mail şablonu başarılı bir şekilde eklendi.";
        public static string RemovedMailTemplate = "Mail şablonu başarılı bir şekilde silindi.";
        public static string MailTemplateUpdated = "Mail şablonu başarılı bir şekilde güncellendi.";

        // Currency Account
        public static string AddedCurrencyAccount = "Cari hesap başarılı bir şekilde eklendi.";
        public static string DeletedCurrencyAccount = "Cari hesap başarılı bir şekilde kaldırıldı.";
        public static string UpdateCurrencyAccount = "Cari hesap başarılı bir şekilde güncellendi.";
        public static string AccountHaveReconciliaition = "Mutabakat işlemi olan cari kaydı silemezsiniz. İstersiniz cari kaydı pasife çekebilirsiniz.";
    
        // Account Reconciliaiton
        public static string AddedAccountRecanciliation = "Cari Mutabakat başarılı bir şekilde oluşturuldu.";
        public static string DeletedAccountRecanciliation = "Cari Mutabakat başarılı bir şekilde kaldırıldı.";
        public static string UpdatedAccountRecanciliation = "Cari Mutabakat başarılı bir şekilde güncellendi.";
        public static string FromExcelAddToAccountReconciliaiton = "Excel aracılığıyla Cari Mutabakat başarılı bir şekilde güncellendi.";
    
        // Account Reconciliation Detail
        public static string AddedAccountReconciliaitonDetail = "Cari Mutabakat detay başarılı bir şekilde oluşturuldu.";
        public static string DeletedAccountReconciliaitonDetail = "Cari Mutabakat detay başarılı bir şekilde kaldırıldı.";
        public static string UpdateAccountReconciliaitonDetail = "Cari Mutabakat detay başarılı bir şekilde güncellendi.";
        public static string FromExcelAddToAccountReconciliationDetail = "Excel aracılığıyla Cari Mutabakat detay başarılı bir şekilde güncellendi.";

        // BaBs Reconciliation 
        public static string AddedBaBsRecanciliation = "BaBs Mutabakat başarılı bir şekilde oluşturuldu.";
        public static string DeletedBaBsRecanciliation = "BaBs Mutabakat başarılı bir şekilde kaldırıldı.";
        public static string UpdatedBaBsRecanciliation = "BaBs Mutabakat başarılı bir şekilde güncellendi.";
        public static string FromExcelAddToBaBsReconciliaiton = "Excel aracılığıyla BaBs Mutabakat başarılı bir şekilde güncellendi.";

        // BaBs reconciliation Detail
        public static string AddedBaBsReconciliatonDetail = "BaBs Mutabakat detayı başarılı bir şekilde oluşturuldu.";
        public static string FromExcelAddToBaBsReconciliationDetail = "Excel aracılığıyla BaBs Mutabakat detayı başarılı bir şekilde oluşturuldu.";
        public static string DeletedBaBsRecanciliationDetail = "BaBs Mutabakat detayı başarılı bir şekilde kaldırıldı.";
        public static string UpdatedBaBsRecanciliationDetail = "BaBs Mutabakat detayı başarılı bir şekilde güncellendi.";

        // Operation Claim
        public static string AddedOperationClaim = "Yetki ekleme işlemi başarılı.";
        public static string DeletedOperationClaim = "Yetki kaldırma işlemi başarılı.";
        public static string UpdateOperationClaim = "Yetki güncelleme işlemi başarılı.";

        // TermsAndCondition
        public static string AddedTermsAndCondition = "Sözleşme güncelleme işlemi başarılı.";
    
        // UserOperationClaim
        
        public static string AddedUserOperationClaim = "Kullanıcıya yetki tanımlaması yapıldı.";
        public static string DeletedUserOperationClaim = "Kullanıcı yetkisi kaldırıldı.";
        public static string UpdatedUserOperationClaim = "Kullanıcı yetkisi güncellendi.";
    
    }
}
