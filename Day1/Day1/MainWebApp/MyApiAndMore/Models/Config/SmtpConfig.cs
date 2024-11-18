namespace MyApiAndMore.Models.Config;


public class SmtpConfig
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int TimeOut { get; set; }
    public string[] SupportedLanguages { get; set; }
}






//public class Rootobject
//{
//    public SmtpConfig smptExternal { get; set; }
//    public SmtpConfig smptInternal { get; set; }
//}

//public class Smptexternal
//{
//    public string HostName { get; set; }
//    public string UserName { get; set; }
//    public string Password { get; set; }
//    public int TimeOut { get; set; }
//    public string[] SupportedLanguages { get; set; }
//}

//public class Smptinternal
//{
//    public string HostName { get; set; }
//    public string UserName { get; set; }
//    public string Password { get; set; }
//    public int TimeOut { get; set; }
//    public string[] SupportedLanguages { get; set; }
//}
