using System;

namespace URLTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1
            foreach (var jobAd in URLHelper.JobAds)
            {
                Console.WriteLine(URLHelper.GetFriendlyURL1(jobAd));
                Console.WriteLine(URLHelper.GetFriendlyTitle1(jobAd));
                Console.WriteLine("----------");
            }


            // 2
            foreach (var portal in URLHelper.Portals)
            {
                foreach (var jobAd in URLHelper.JobAds)
                {

                    Console.WriteLine(URLHelper.GetFriendlyURL2(portal, jobAd));
                    Console.WriteLine(URLHelper.GetFriendlyTitle2(portal, jobAd));
                    Console.WriteLine();
                }

                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
