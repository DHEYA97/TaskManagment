﻿namespace TaskManagment.Core.Settinges
{
    public class MailSetting
    {
        public static string SectionName = "MailSettings";
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
