using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace URLTester
{
    public static class URLHelper
    {
        const string urlFormatter = "https://www.stellencha.os/stellen/{0}/{1}/{2}/";
        const string titleSuffix = "stellencha.os";

        public static string GetFriendlyURL2(Portal portal, JobAd jobAd)
        {
            return portal.URLFormat
                .Replace("{ID}", jobAd.Id.ToString())
                .Replace("{COMPANY}", FriendlyFormat(jobAd.Company))
                .Replace("{REGION_ENC}", string.Join("-", jobAd.Regions.Select(r => FriendlyFormat(ReplaceUmlaut(r)))))
                .Replace("{REGION}", string.Join("-", jobAd.Regions.Select(r => FriendlyFormat(r))))
                .Replace("{TITLE_ENC}", FriendlyFormat(ReplaceUmlaut(jobAd.Title)))
                .Replace("{TITLE}", FriendlyFormat(jobAd.Title));
        }

        // TODO: No time left
        public static string GetFriendlyTitle2(Portal portal, JobAd jobAd) => string.Join(" | ",
            jobAd.Title,
            jobAd.Company,
            string.Join(", ", jobAd.Regions),
            titleSuffix);


        public static string GetFriendlyURL1(JobAd jobAd) => string.Format(urlFormatter,
                FriendlyFormat(jobAd.Company),
                jobAd.Id,
                FriendlyFormat(jobAd.Title));

        public static string GetFriendlyTitle1(JobAd jobAd) => string.Join(" | ",
                jobAd.Title,
                jobAd.Company,
                string.Join(", ", jobAd.Regions),
                titleSuffix);

        private static string ReplaceUmlaut(string text)
        {
            return (text ?? "").Replace("ö", "oe")
                .Replace("ä", "ae")
                .Replace("ü", "ue");
        }
        private static string FriendlyFormat(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            char[] Removals = { '-', ' ' };

            string url = Regex.Replace(text.ToLower(), @"^\W+|\W+$", "");
            url = Regex.Replace(url, @"_", "-");
            url = Regex.Replace(url, @"\W+", "-");

            while (url.IndexOf("--") >= 0)
            {
                url = url.Replace("--", "-");
            }

            return url.Trim(Removals);
        }

        // 12345
        // https://www.stellencha.os/stellen/software-more-inc/12345/junior-software-developer-m-w-d-100-remote/
        // Junior Software Developer(m/w/d) - 100% remote | Software & More Inc. | München | stellencha.os

        // 23456
        // https://www.stellencha.os/stellen/professional-software-gmbh-co-kg/23456/software-developer-m-w-d-100-home-office-möglich/
        // Software Developer (m/w/d) - 100% Home-Office möglich! | Professional Software GmbH & Co.KG | München, Bamberg, Augsburg | stellencha.os

        // 34567 
        // https://www.stellencha.os/stellen/daten-lösungen-ag/34567/senior-software-developer-m-w-d-50-aussendienst/
        // Senior Software Developer(m/w/d) - 50% Außendienst | Daten & Lösungen AG | Berlin, Hamburg, Frankfurt, Köln, Bayern | stellencha.os
        public static List<JobAd> JobAds => new List<JobAd>()
        {
            new JobAd(12345,"Junior Software Developer (m/w/d) - 100% remote",  "Software & More Inc.", "München"),
            new JobAd(23456,"Software Developer (m/w/d) - 100% Home-Office möglich!",  "Professional Software GmbH & Co.KG", "München,Bamberg,Augsburg"),
            new JobAd(12345,"Senior Software Developer (m/w/d) - 50% Außendienst",  "Daten & Lösungen AG", "Berlin,Hamburg,Frankfurt,Köln,Bayern"),
        };

        public static List<Portal> Portals => new List<Portal>()
        {
            new Portal("stellencha.os", "https://www.stellencha.os/stellen/{COMPANY}/{ID}/{TITLE}/", ""),
            new Portal("jobs-mit.biz", "https://www.jobs-mit.biz/jobs/{REGION_ENC}/{ID}/{TITLE_ENC}/", ""),
            new Portal("job.dealer", "https://www.job.dealer/job/{ID}-{TITLE}-bei-{COMPANY}-in-{REGION_ENC}", ""),
        };
    }

    public class Portal
    {
        public string Name { get; set; }
        public string URLFormat { get; set; }
        public string TitleFormat { get; set; }

        public Portal(string name, string urlFormat, string titleFormat)
        {
            this.Name = name;
            this.URLFormat = urlFormat;
            this.TitleFormat = titleFormat;
        }
    }

    public class JobAd
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string[] Regions { get; set; }

        public JobAd(int id, string title, string company, string regions)
        {
            this.Id = id;
            this.Title = title;
            this.Company = company;
            this.Regions = (regions ?? "").Split(',');
        }
    }
}
