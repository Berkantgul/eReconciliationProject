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
    }
}
