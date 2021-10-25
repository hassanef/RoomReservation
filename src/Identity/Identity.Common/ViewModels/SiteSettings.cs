using System;

namespace Identity.Common.ViewModels
{
    public class SiteSettings
    {
        public AdminUserSeed AdminUserSeed { get; set; }
        public Logging Logging { get; set; }
        public bool EnableEmailConfirmation { get; set; }
        public TimeSpan EmailConfirmationTokenProviderLifespan { get; set; }
        public int NotAllowedPreviouslyUsedPasswords { get; set; }
        public int ChangePasswordReminderDays { get; set; }
        public ActiveDatabase ActiveDatabase { get; set; }
        public string UsersAvatarsFolder { get; set; }
        public string UserDefaultPhoto { get; set; }
        public string ContentSecurityPolicyErrorLogUri { get; set; }
        public string[] EmailsBanList { get; set; }
        public string[] PasswordsBanList { get; set; }
    }
}