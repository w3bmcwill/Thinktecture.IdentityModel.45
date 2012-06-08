using System;
using System.Diagnostics;
using Resources;
using Thinktecture.IdentityModel.Extensions;

namespace Thinktecture.Samples
{
    public static class Helper
    {
        public static void Timer(Action a)
        {
            var sw = new Stopwatch();

            sw.Start();
            a();

            string.Format("\n\nElapsed Time: {0}\n", sw.ElapsedMilliseconds).ConsoleRed();
        }

        public static void ShowConsole(this Identity id)
        {
            "Client Identity".ConsoleYellow();

            if (!id.IsAuthenticated)
            {
                " anonymous".ConsoleGreen();
                return;
            }

            string.Format(" Name: {0}", id.Name).ConsoleGreen();
            string.Format(" Authentication type: {0}", id.AuthenticationType).ConsoleGreen();
            string.Format(" CLR type: {0}", id.ClrType).ConsoleGreen();

            "\nClaims\n".ConsoleYellow();

            id.Claims.ForEach(c =>
                {
                    Console.WriteLine(" " + c.ClaimType);
                    string.Format("  {0}", c.Value).ConsoleGreen();
                    Console.WriteLine("  {0} ({1})\n", c.Issuer, c.OriginalIssuer);
                });

        }
    }
}
