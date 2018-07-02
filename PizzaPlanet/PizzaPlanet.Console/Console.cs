using PizzaPlanet.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Application
{
    internal static class Console
    {
        //private static readonly string Logo = "                     / (############*                                           \r\n           ###/ /#/              ,#########                                    \r\n         (# ####/###*    #/          #########.                                \r\n        /##(# #### ,# *,.            .###/*####                                \r\n        #*(#  ## (#*#.       ###.   .###    *#                                 \r\n     *.  #/#(#*##(.##      ####( .#######/                                     \r\n   ####/./#,#  #/##,      .#####(,     ####  ##########.######### .########.   \r\n              ..           ###        ####     #####/    /#####  ####  ####    \r\n    .* (###########*        ##        (###    #####(    .#####   *###  ####     \r\n                (########* #         ####  #####(    (#####     ####(####*     \r\n                   #########.                                           ..     \r\n              .     #########                                  .,,,            \r\n          ,###    .########(                                 .####.      *,    \r\n         ####(.#######( ,(                                   ####        ..    \r\n        ,####/.     .#### ,####.  .   (########  .########  ####.              \r\n        .###        #### .###/ /###  (###. #### ####   ### (###(               \r\n         ##        ####  ####  ###   #### ,###  ###/,##(   ####                \r\n         (         ###########(#########  ############(/((#####/**.           ";
        //BufferWidth = 120 default
        private static readonly string WelcomeMessage = "Welcome to Pizza Planet!";

        public static readonly char esc = (char)27;
        
        private static User CurrentUser = new User("");

        public static void StartScreen()
        {
            //System.Console.WriteLine(Logo);
            System.Console.WriteLine(WelcomeMessage);
            LoginScreen();
        }

        private static readonly string LoginMessage = "Please enter Username to continue...";

        public static void LoginScreen()
        {
            string userInput;
            System.Console.WriteLine(LoginMessage);
            userInput = System.Console.ReadLine();
            System.Console.WriteLine("Username not found, create new?");
            System.Console.ReadLine();
            System.Console.WriteLine("Welcome, " + userInput);
            CurrentUser = new User(userInput);
            WhatNextScreen();
            return;
        }


        private static readonly string WhatNextMessage = "What would you like to do?";
        private static readonly string WrongInputMessage = "Hm? Please try again";
        private static readonly string[] WhatNextCommands = {
            "Start an Order",
            "Exit"
        };
        public static void WhatNextScreen()
        {
            while (true)
            {
                System.Console.WriteLine(WhatNextMessage);
                for (int i = 0; i < WhatNextCommands.Length-1; i++)
                    System.Console.WriteLine((i+1) + " : " + WhatNextCommands[i]);
                System.Console.WriteLine("ESC : Exit");
                char input = System.Console.ReadKey().KeyChar;
                System.Console.WriteLine();
                switch (input)
                {
                    case '1':
                        SelectLocationScreen();
                        continue;
                    case (char)27:
                        ExitScreen();
                        return;
                    default:
                        System.Console.WriteLine(WrongInputMessage);
                        continue;
                }
            }
        }

        public static readonly string DefaultLocationMesage = "Use saved location?";
        public static readonly string ChooseLocationMessage = "Please select location:";
        public static void SelectLocationScreen()
        {
            while (true)
            {
                //TODO default location
                //note assumes small number of location options <10
                //If more, consider scrolling/subset chosen
                //possible serach new location
                System.Console.WriteLine(ChooseLocationMessage);
                int [] ids = new int[Location.Locations().Count];
                int n = 0;
                foreach (int i in Location.Locations().Keys)
                {
                    System.Console.WriteLine((n+1)+" : Store #" + i);
                    ids[n] = i;
                    n++;
                }
                System.Console.WriteLine("ESC : Return");
                char input = System.Console.ReadKey().KeyChar;
                System.Console.WriteLine();
                if (input > '0' && input <= '0' + n)
                {
                    //StartOrderScreen(ids[n]);
                    return;
                }
                else if (input == 27)
                    return;
                else
                    System.Console.WriteLine(WrongInputMessage);
            }
        }

        public static readonly string[] OrderScreenCommands = {
            "View Details",
            "Add a Pizza",
            "Remove a Pizza",
            "Complete Order",
            "Cancel Order"
        };
        public static void StartOrderScreen(int storeId)
        {
            while (true)
            {
                System.Console.WriteLine(WhatNextMessage);
                for (int i = 0; i < OrderScreenCommands.Length - 1; i++)
                    System.Console.WriteLine((i + 1) + " : " + WhatNextCommands[i]);
                System.Console.WriteLine("ESC : Cancel Order");
                char input = System.Console.ReadKey().KeyChar;
                System.Console.WriteLine();
                switch (input)
                {
                    case '1':
                        //DisplayOrderDetailsScreen();
                        continue;
                    case '2':
                        //CreatePizzaScreen
                        continue;
                    case '3':
                        //RemovePizzaScreen
                        continue;
                    case '4':
                        //CompleteOrder
                        continue;
                    case (char)27:
                        return;
                    default:
                        System.Console.WriteLine(WrongInputMessage);
                        continue;
                }
            }
        }

        public static readonly string ExitMessage = "Goodbye for now!";
        public static void ExitScreen()
        {
            System.Console.WriteLine(ExitMessage);
            System.Console.ReadLine();
            return;
        }
    }
}
