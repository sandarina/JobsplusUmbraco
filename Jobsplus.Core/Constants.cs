using System;

namespace Jobsplus.Backoffice
{
    public class JobsplusConstants
    {
        public const string DefaultEmail = "info@salmaplus.cz";
        public const string EmailRobotEmail = "info@jobsplus.cz";
        public const string SendEmailErrorMsg = "Odeslání emailu selhalo! Prosím kotaktujte naši technickou podporu na emailu info@salmaplus.cz. Do emailu uveďte následující text:";

        public static string[] BrigadeWorkTypes = {
                                                      "Dohoda o provedení činnosti",
                                                      "Dohoda o provedení práce",
                                                      "Brigáda",
                                                      "Stáž",
                                                      "Praxe"
                                                  };
    }
}
