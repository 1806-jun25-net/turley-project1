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

        private static User CurrentUser = new User(-1,"");
        //Set to null to denote no location has been selected yet
        private static Location CurrentLocation = null;

        public static void StartScreen()
        {
            //System.Console.WriteLine(Logo);
            System.Console.WriteLine(WelcomeMessage);
            LoginScreen();
        }

        private static readonly string LoginMessage = "Please enter Username to continue...";

        static void LoginScreen()
        {
            string userInput;
            System.Console.WriteLine(LoginMessage);
            userInput = System.Console.ReadLine();
            System.Console.WriteLine("Username not found, create new?");
            System.Console.ReadLine();
            System.Console.WriteLine("Welcome, " + userInput);
            CurrentUser = new User(0,userInput);
            WhatNextScreen();
            return;
        }


        private static readonly string WhatNextMessage = "What would you like to do?";
        private static readonly string BadInputMessage = "Hm? Please try again";
        private static readonly string[] WhatNextCommands = {
            "Start an Order",
            "Exit"
        };
        static void WhatNextScreen()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine(WhatNextMessage);
                for (int i = 0; i < WhatNextCommands.Length-1; i++)
                    System.Console.WriteLine((i+1) + " : " + WhatNextCommands[i]);
                System.Console.WriteLine("ESC : Exit");
                char input = System.Console.ReadKey().KeyChar;
                switch (input)
                {
                    case '1':
                        SelectLocationScreen();
                        OrderScreen(new Order(CurrentUser, CurrentLocation ));
                        continue;
                    case (char)27:
                        ExitScreen();
                        return;
                    default:
                        continue;
                }
            }
        }

        static readonly string DefaultLocationMesage = "Use saved location?";
       static readonly string ChooseLocationMessage = "Please select location:";
         static void SelectLocationScreen()
        {
            
            while (true)
            {
                //TODO default location, if current = null. set current to null when user switch
                //note assumes small number of location options <10
                //If more, consider scrolling/subset chosen
                //possible serach new location
                System.Console.Clear();
                System.Console.WriteLine(ChooseLocationMessage);
                int[] ids = new int[Location.Locations().Count];
                int n = 0;
                foreach (int i in Location.Locations().Keys)
                {
                    System.Console.WriteLine((n + 1) + " : Store #" + i);
                    ids[n] = i;
                    n++;
                }
                System.Console.WriteLine("ESC : Cancel");
                char input = System.Console.ReadKey().KeyChar;
                System.Console.WriteLine();
                if (input > '0' && input <= '0' + n)
                {
                    CurrentLocation = Location.GetLocation(ids[input - '1']);
                    return;
                }
                else if (input == 27)
                    return;
            }
        }

         static readonly string[] OrderScreenCommands = {
            "Add a Pizza",
            "Remove a Pizza",
            "Change Location",
            "Complete Order"
        };

        static void OrderScreen(Order order)
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine(order.ToString());
                System.Console.WriteLine(WhatNextMessage);
                for (int i = 0; i < OrderScreenCommands.Length; i++)
                    System.Console.WriteLine((i + 1) + " : " + OrderScreenCommands[i]);
                System.Console.WriteLine("ESC : Cancel Order");
                char input = System.Console.ReadKey().KeyChar;
                System.Console.WriteLine();
                Pizza pizza = null;
                int pizzaID = -1;
                switch (input)
                {
                    case '1':
                        if(order.CanAddPizza())
                            pizza = CreatePizzaScreen();
                        if (pizza != null)
                            order.AddPizza(pizza);
                        System.Console.Clear();
                        continue;
                    case '3':
                        SelectLocationScreen();
                        order.Store = CurrentLocation;
                        System.Console.Clear();
                        continue;

                    case '2':
                        if (order.NumPizza == 0)
                        {
                            System.Console.WriteLine(NoPizzasMessage);
                        }
                        else
                        {
                            pizzaID = SelectPizzaScreen(order);
                            if (pizzaID != -1)
                            {
                                order.RemovePizza(pizzaID);
                            }
                        }
                        System.Console.Clear();
                        continue;
                    case '4':
                        //CompleteOrder
                        if (order.NumPizza == 0)
                        {
                            System.Console.WriteLine(NoPizzasMessage);
                        }
                        System.Console.Clear();
                        continue;
                    case (char)27:
                        return;
                    default:
                        continue;
                }
            }
        }

        
        static readonly string NoPizzasMessage = "No pizzas.";

        static readonly string SelectPizzaMessage = "Which Pizza?";
        //returns -1 for cancel, else the number pizza selected
        static int SelectPizzaScreen(Order order)
        {
            //note: written assuming max pizza = 12. could be made modular
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine(SelectPizzaMessage);
                for(int i = 0; i<order.NumPizza;i++)
                {
                    if (i < 9)
                        System.Console.WriteLine((i + 1) + " : " + order.Pizzas[i].ToString());
                    else if (i == 9)
                        System.Console.WriteLine("0 : " + order.Pizzas[i].ToString());
                    else
                    {
                        char c = (char)('a' + (i - 10));
                        System.Console.WriteLine(c+ " : " + order.Pizzas[i].ToString());
                    }
                }
                System.Console.WriteLine("ESC : Cancel");
                char input = System.Console.ReadKey().KeyChar;
                System.Console.WriteLine();
                if (input > '0' && input <= '9' && (input - '0') <= order.NumPizza)
                {
                    return input - '1';
                }
                else if (input == '0' && order.NumPizza >= 10)
                {
                    return 10;
                }
                else if (input == 'a' && order.NumPizza >= 11)
                {
                    return 11;
                }
                else if (input == 'b' && order.NumPizza == 12)
                {
                    return 12;
                }
                else if (input == 27)
                    return -1;
            }
        }
        
        static Pizza CreatePizzaScreen()
        {
            Pizza pizza = new Pizza();
            //Select size
            while(true)
            {
                System.Console.Clear();
                System.Console.WriteLine("What Size?");
                for (int i = 0; i < Pizza.Sizes.Length; i++)
                    System.Console.WriteLine((i+1)+" : "+Pizza.Sizes[i]);
                System.Console.WriteLine("ESC : Cancel Pizza");
                char input = System.Console.ReadKey().KeyChar;
                if (input == 27)
                    return null;
                else if (input >= '1' && input <= '1'+ Pizza.Sizes.Length)
                {
                    pizza.Size = (Pizza.SizeType)(input - '1' + Pizza.SizeType.Small);
                    break;
                }
            }
            //Select crust
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("What Crust?");
                for (int i = 0; i < Pizza.CrustTypes.Length; i++)
                    System.Console.WriteLine((i + 1) + " : " + Pizza.CrustTypes[i]);
                System.Console.WriteLine("ESC : Cancel Pizza");
                char input = System.Console.ReadKey().KeyChar;
                if (input == 27)
                    return null;
                else if (input >= '1' && input <= '1' + Pizza.CrustTypes.Length)
                {
                    pizza.Crust = (Pizza.CrustType)(input - '1');
                    break;
                }
            }
            //Select sauce
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("Sauce?");
                for (int i = 0; i < Pizza.Amounts.Length; i++)
                    System.Console.WriteLine((i + 1) + " : " + Pizza.Amounts[i]);
                System.Console.WriteLine("ESC : Cancel Pizza");
                char input = System.Console.ReadKey().KeyChar;
                if (input == 27)
                    return null;
                else if (input >= '1' && input <= '1'+ Pizza.Amounts.Length)
                {
                    pizza.Toppings[(int)Pizza.ToppingType.Sauce] = (Pizza.Amount)(input - '1');
                    break;
                }
            }
            //select cheese
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("Cheese?");
                for (int i = 0; i < Pizza.Amounts.Length; i++)
                    System.Console.WriteLine((i + 1) + " : " + Pizza.Amounts[i]);
                System.Console.WriteLine("ESC : Cancel Pizza");
                char input = System.Console.ReadKey().KeyChar;
                if (input == 27)
                    return null;
                else if (input >= '1' && input <= '1'+ Pizza.Amounts.Length)
                {
                    pizza.Toppings[(int)Pizza.ToppingType.Cheese] = (Pizza.Amount)(input - '1');
                    break;
                }
            }
            //select toppings
            bool complete = ToppingSelectScreen(pizza);
            if (!complete)
                pizza = null;
            return pizza;
    }
        static bool ToppingSelectScreen(Pizza pizza)
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("Toppings? (Select to change amount)");
                //toppings start at 2 (pepperoni)
                for (int i = 2; i < Pizza.ToppingTypes.Length; i++)
                {
                    string printMe = "";
                    if (i < 11)
                        printMe = (i-1) + " : " + Pizza.ToppingTypes[i] +" : "  + pizza.Toppings[i].ToString();
                    else if (i == 11)
                        printMe = "0 : " + Pizza.ToppingTypes[i] + " : " + pizza.Toppings[i].ToString();
                    else
                    {
                        char select = (char)('a' + (i - 12)); //12 starts with 'a'
                        printMe = select + " : " + Pizza.ToppingTypes[i] + " : " + pizza.Toppings[i].ToString();
                    }
                    System.Console.WriteLine(printMe);
                }
                System.Console.WriteLine("Enter : Complete Pizza");
                System.Console.WriteLine("ESC : Cancel Pizza");
                char input = System.Console.ReadKey().KeyChar;
                int topping = -1;
                if (input == 27)
                    return false;
                else if (input == 13)
                    return true;
                //topping 2-10
                else if (input >= '1' && input <= '9')
                {
                    topping= (input - '1' + 2);
                }
                //topping 11
                else if (input == '0')
                {
                    topping = 11;
                }
                //toppings 12+. MaxTopping = Length -1
                else if (input >= 'a' && input <= ('a' + (Pizza.ToppingTypes.Length-1)-12))
                {
                    topping = (input - 'a' + 12);
                }
                if(topping > -1)
                {
                    int amount = HowMuchScreen(topping);
                    pizza.Toppings[topping] = (Pizza.Amount)amount;
                }
            }
        }
        
        static int HowMuchScreen(int topping)
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine(Pizza.ToppingTypes[topping]+ "?");
                for (int i =0; i < Pizza.Amounts.Length; i++)
                {
                    System.Console.WriteLine((i + 1) + " : " + Pizza.Amounts[i]);
                }

                System.Console.WriteLine("ESC : Cancel Topping");
                char input = System.Console.ReadKey().KeyChar;
                if (input >= '1' && input <= '1'+Pizza.Amounts.Length)
                {
                    return (int)(input - '1');
                }
                else if (input == 27)
                {
                    return 0;
                }
            }

        }

        static readonly string ExitMessage = "Goodbye for now!";
        static void ExitScreen()
        {
            System.Console.Clear();
            System.Console.WriteLine(ExitMessage);
            System.Console.ReadLine();
            return;
        }
    }
}
