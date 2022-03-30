using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var performanceInfo = "";
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var thisAmount = CalculateAmount(play, perf);
                volumeCredits = CalculateVolumeCredits(volumeCredits, perf, play);

                // print line for this order
                performanceInfo += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
                totalAmount += thisAmount;
            }
            return $@"Statement for {invoice.Customer}
{performanceInfo}Amount owed is {Convert.ToDecimal(totalAmount / 100, cultureInfo):C}
You earned {volumeCredits} credits
";
            
        }

        private int CalculateVolumeCredits(int volumeCredits, Performance perf, Play play)
        {
            // add volume credits
            volumeCredits += Math.Max(perf.Audience - 30, 0);
            // add extra credit for every ten comedy attendees
            if ("comedy" == play.Type) volumeCredits += (int) Math.Floor((decimal) perf.Audience / 5);
            return volumeCredits;
        }

        private int CalculateAmount(Play play, Performance perf)
        {
            var thisAmount = 0;
            switch (play.Type)
            {
                case "tragedy":
                    thisAmount = CalculateTragedyAmount(perf);
                    break;
                case "comedy":
                    thisAmount = CalculateComedyAmount(perf);
                    break;
                default:
                    throw new Exception("unknown type: " + play.Type);
            }

            return thisAmount;
        }

        private int CalculateComedyAmount(Performance perf)
        {
            int thisAmount;
            thisAmount = 30000;
            if (perf.Audience > 20)
            {
                thisAmount += 10000 + 500 * (perf.Audience - 20);
            }

            thisAmount += 300 * perf.Audience;
            return thisAmount;
        }

        private int CalculateTragedyAmount(Performance perf)
        {
            int thisAmount;
            thisAmount = 40000;
            if (perf.Audience > 30)
            {
                thisAmount += 1000 * (perf.Audience - 30);
            }

            return thisAmount;
        }
    }
}
