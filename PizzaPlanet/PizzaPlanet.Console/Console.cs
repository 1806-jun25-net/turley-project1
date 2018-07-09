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
        static readonly string PressAnyKeyMessage = "(Press any key to continue...)";

        private static User CurrentUser = null;
        //Set to null to denote no location has been selected yet
        private static Location CurrentLocation = null;
        private static string TopMessage = "";

        public static void StartScreen()
        {
            //System.Console.WriteLine(Logo);
            TopMessage = WelcomeMessage;
            LoginScreen();
        }

        static void ClearScreen()
        {
            System.Console.Clear();
            System.Console.WriteLine(TopMessage);
            System.Console.WriteLine();
            TopMessage = "";
        }

        static readonly string NoUserMessage = "Username not found...";
        static readonly string LoginMessage = "Please enter Username to continue:";


        static void LoginScreen()
        {
            while (CurrentUser == null)
            {
                ClearScreen();
                string name;
                System.Console.WriteLine(LoginMessage);
                name = System.Console.ReadLine();
                CurrentUser = User.TryUser(name);
                if (name.Length < 4) //input too short
                {
                    TopMessage = "Username too short, must be 4+ characters";
                    continue;
                }
                else if (CurrentUser != null)   //found user
                {
                    TopMessage = "Welcome, " + CurrentUser.Name;
                    WhatNextScreen();
                }
                else //no user by that name
                {
                    TopMessage = NoUserMessage;
                    bool what = true;
                    while(what)
                    { 
                        ClearScreen();
                        System.Console.WriteLine("What should we do?");
                        System.Console.WriteLine("1 : New User with name: "+name);
                        System.Console.WriteLine("2 : Try Again");
                        System.Console.WriteLine("ESC : Exit");
                        char input = System.Console.ReadKey().KeyChar;
                        switch (input)
                        {
                            case '1'://New User
                                CurrentUser = User.MakeUser(name);
                                WhatNextScreen();
                                what = false;
                                break;
                            case '2'://Try again
                                what = false;
                                break;
                            case (char)27:
                                ExitScreen();
                                return;
                            default:
                                continue;
                        }
                    }
                }
            }
            ExitScreen();
        }


        private static readonly string WhatNextMessage = "What would you like to do?";
        //private static readonly string BadInputMessage = "Hm? Please try again";
        private static readonly string[] WhatNextCommands = {
            "Start an Order",
            "Logout"
        };
        static void WhatNextScreen()
        {
            while (true)
            {
                ClearScreen();
                System.Console.WriteLine(WhatNextMessage);
                for (int i = 0; i < WhatNextCommands.Length; i++)
                    System.Console.WriteLine((i+1) + " : " + WhatNextCommands[i]);
                System.Console.WriteLine("ESC : Exit");
                char input = System.Console.ReadKey().KeyChar;
                switch (input)
                {
                    case '1'://Start order
                        SelectLocationScreen();
                        OrderScreen(new Order(CurrentUser, CurrentLocation ));
                        continue;
                    case '2'://logout
                        {
                            CurrentUser = null;
                            return;
                        }
                    case (char)27:
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
                ClearScreen();
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

        static readonly string OrderFailedMessage = "Location failed to place order";
        static readonly string OrderSuccessMessage = "Order Successfully Placed!";
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
                ClearScreen();
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
                        continue;
                    case '3': //Change Location
                        SelectLocationScreen();
                        order.Store = CurrentLocation;
                        continue;

                    case '2'://Remove Pizza
                        if (order.NumPizza == 0)
                        {
                            TopMessage = NoPizzasMessage;
                        }
                        else
                        {
                            pizzaID = SelectPizzaScreen(order);
                            if (pizzaID != -1)
                            {
                                order.RemovePizza(pizzaID);
                            }
                        }
                        continue;
                    case '4'://Complete order
                        if (order.NumPizza == 0)
                        {
                            TopMessage = NoPizzasMessage;
                        }
                        else
                        {
                            if (order.Store.PlaceOrder(order))
                            {
                                ClearScreen();
                                System.Console.WriteLine(order.ToString());
                                System.Console.WriteLine("\r\n" + OrderSuccessMessage);
                                System.Console.WriteLine(PressAnyKeyMessage);
                                System.Console.Read();
                                return;
                            }
                            else
                                TopMessage = OrderFailedMessage;
                        }
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
                ClearScreen();
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
                ClearScreen();
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
                ClearScreen();
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
                ClearScreen();
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
                ClearScreen();
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
                ClearScreen();
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
                ClearScreen();
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
            ClearScreen();
            System.Console.WriteLine(ExitMessage);
            System.Console.ReadLine();
            return;
        }
    }
}
