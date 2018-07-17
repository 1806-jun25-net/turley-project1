using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaPlanet.DBData;
using PizzaPlanet.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using PZ = PizzaPlanet.Library.Pizza;

namespace PizzaPlanet.Application
{
    internal static class Console

    {/*
        private static readonly string Logo = "                     / (############*                                           \r\n           ###/ /#/              ,#########                                    \r\n         (# ####/###*    #/          #########.                                \r\n        /##(# #### ,# *,.            .###/*####                                \r\n        #*(#  ## (#*#.       ###.   .###    *#                                 \r\n     *.  #/#(#*##(.##      ####( .#######/                                     \r\n   ####/./#,#  #/##,      .#####(,     ####  ##########.######### .########.   \r\n              ..           ###        ####     #####/    /#####  ####  ####    \r\n    .* (###########*        ##        (###    #####(    .#####   *###  ####     \r\n                (########* #         ####  #####(    (#####     ####(####*     \r\n                   #########.                                           ..     \r\n              .     #########                                  .,,,            \r\n          ,###    .########(                                 .####.      *,    \r\n         ####(.#######( ,(                                   ####        ..    \r\n        ,####/.     .#### ,####.  .   (########  .########  ####.              \r\n        .###        #### .###/ /###  (###. #### ####   ### (###(               \r\n         ##        ####  ####  ###   #### ,###  ###/,##(   ####                \r\n         (         ###########(#########  ############(/((#####/**.           ";
        //BufferWidth = 120 default
        private static readonly string WelcomeMessage = "Welcome to Pizza Planet!";
        static readonly string PressEnterMessage = "(Press ENTER to continue)";

        private static User CurrentUser = null;
        //Set to null to denote no location has been selected yet
        private static Library.Store CurrentLocation = null;
        private static string TopMessage = "";

        public static void StartScreen()
        {
            //System.Console.WriteLine(Logo);
            //System.Console.WriteLine(PressEnterMessage);
            //System.Console.ReadLine();
            TopMessage = WelcomeMessage;
            LoginScreen();
        }
        
        static void ClearScreen()
        {
            System.Console.Clear();
            System.Console.WriteLine(TopMessage);
            System.Console.WriteLine();
            TopMessage = "";
            if(CurrentUser!= null)
                TopMessage = "<"+CurrentUser.Name+">";
        }

        static readonly string NoUserMessage = "Username not found...";
        static readonly string LoginMessage = "Please ENTER Username to continue:\r\n(Enter nothing to EXIT)";


        static void LoginScreen()
        {
            while (CurrentUser == null)
            {
                CurrentUser = null;
                ClearScreen();
                string name;
                System.Console.WriteLine(LoginMessage);
                name = System.Console.ReadLine();
                if (name == "" || name == "nothing") //exit
                    break;
                else if (name.Length < 4) //input too short
                {
                    TopMessage = "Username too short, must be at least 4 characters";
                    continue;
                }
                CurrentUser = User.TryUser(name);
                if (CurrentUser != null)   //found user
                {
                    TopMessage = "Welcome back, " + CurrentUser.Name;
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
                                TopMessage = "Welcome, " + CurrentUser.Name;
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
        private static readonly string TooSoonMessage = "You ordered from there recently, try again in: ";
        //private static readonly string BadInputMessage = "Hm? Please try again";
        private static readonly string[] WhatNextCommands = {
            "Start an Order",
            "View your Order History",
            "View Order History for a Store",
            "Logout"
        };
        static void WhatNextScreen()
        {
            while (true)
            {
                CurrentLocation = null;
                ClearScreen();
                System.Console.WriteLine(WhatNextMessage);
                for (int i = 0; i < WhatNextCommands.Length; i++)
                    System.Console.WriteLine((i+1) + " : " + WhatNextCommands[i]);
                System.Console.WriteLine("ESC : Exit");
                char input = System.Console.ReadKey().KeyChar;
                switch (input)
                {
                    case '1'://Start order
                        bool useLast = false;
                        Order order;
                        if (CurrentUser.LastOrder() != null)
                            useLast = SuggestOrderScreen(CurrentUser.LastOrder());
                        if (useLast == false)//old way, no suggestion
                        {
                            SelectLocationScreen();
                            if (CurrentLocation == null)
                                continue;
                            try
                            {
                                order = new Order(CurrentUser, CurrentLocation);
                            }
                            catch (PizzaTooSoonException)
                            {
                                int minutes = Math.Max(1, 120 - (int)Math.Truncate((DateTime.Now - CurrentUser.LastOrder().Time).TotalMinutes));
                                TopMessage = TooSoonMessage + minutes + " minutes";
                                continue;
                            }
                        }
                        else //using the suggestion
                        {
                            try
                            {
                                order = new Order(CurrentUser.LastOrder());
                            }
                            catch (PizzaTooSoonException)
                            {
                                int minutes = Math.Max(1, 120 - (int)Math.Truncate((DateTime.Now - CurrentUser.LastOrder().Time).TotalMinutes));
                                TopMessage = TooSoonMessage + minutes + " minutes";
                                continue;
                            }
                        }
                        OrderScreen(order);
                        continue;
                    case '2'://OrderHistory for you
                        OrderHistoryScreen(CurrentUser.Orders());
                        continue;
                    case '3'://ORderhistory for a store
                        SelectLocationScreen();
                        if (CurrentLocation == null)
                            continue;
                        OrderHistoryScreen(CurrentLocation.Orders());
                        continue;
                    case '4'://logout
                        {
                            TopMessage = WelcomeMessage;
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

        private static readonly string PreviousOrderMessage = "Would you like to order this again?";
        private static bool SuggestOrderScreen(Order o)
        {
            while (true)
            {
                TopMessage = PreviousOrderMessage;
                ClearScreen();
                System.Console.WriteLine(o.FullDetails());
                System.Console.WriteLine();
                System.Console.WriteLine("ENTER : YES");
                System.Console.WriteLine("ESC : NO");
                char input = System.Console.ReadKey().KeyChar;
                if (input == (char)27)
                    return false;
                else if (input == (char)13)
                    return true;
            }
        }

        private static readonly string NoOrdersMessage = "No orders found...";
        private static void OrderHistoryScreen(IEnumerable<Order> orders)
        {
            if (orders == null)
            {
                TopMessage = NoOrdersMessage;
                return;
            }
            else if (!orders.Any())
            {
                TopMessage = NoOrdersMessage;
                return;
            }
            else
            {
                int maxLines = 10;// max we will display in 1 page
                Order[] arr = orders.ToArray();
                int last = arr.Length-1;
                int start = 0; //starting [] to display
                int finish = Math.Min(maxLines - 1, last); //final [] to display
                string currentSort = "most recent";
                while (true)
                {
                    TopMessage = "Sorted by " + currentSort;
                    ClearScreen();
                    bool less = start > 0;//less to display (previous)
                    bool more = finish < last;//more to display
                    for(int i = start; i < finish; i++)
                        System.Console.WriteLine((char)('1' + i - start) + " : " + arr[i].Details());
                    if(finish == start + 9)
                        System.Console.WriteLine('0' + " : " + arr[finish].Details());
                    else
                        System.Console.WriteLine((finish-start+1) + " : " + arr[finish].Details());
                    if (less)
                        System.Console.WriteLine('[' + " : View Previous Entries");
                    if (more)
                        System.Console.WriteLine(']' + " : View Next Entries");
                    System.Console.WriteLine("c : Change Order Sorting");
                    System.Console.WriteLine("ESC : Return");
                    char input = System.Console.ReadKey().KeyChar;
                    if (input >= '1' && input <= '9' && (input - '1') <= (finish - start))
                        ShowOrderDetailsScreen(arr[input - '1' + start]);
                    else if (input == '0' && finish == start + 9)
                        ShowOrderDetailsScreen(arr[finish]);
                    else if (input == (char)27)
                        return;
                    else if (more && input == ']')
                    {
                        start = start + maxLines;
                        finish = Math.Min(start + maxLines-1, last);
                    }
                    else if (less && input == '[')
                    {
                        start = start - maxLines;
                        finish = start+maxLines-1;
                    }
                    else if (input == 'c')
                    {
                        string nextSort = ChangeSortingScreen(currentSort);
                        if (nextSort == currentSort)
                            continue;
                        else
                        {
                            switch (nextSort)
                            {
                                case "most recent":
                                    arr = orders.OrderBy(o => (DateTime.Now - o.Time)).ToArray();
                                    break;
                                case "oldest":
                                    arr = orders.OrderBy(o => o.Time).ToArray();
                                    break;
                                case "most expensive":
                                    arr = orders.OrderBy(o => (Order.MaxPrice - o.Price())).ToArray();
                                    break;
                                case "least expensive":
                                    arr = orders.OrderBy(o => o.Price()).ToArray();
                                    break;
                            }
                            start = 0;
                            finish = Math.Min(maxLines - 1, last);
                            currentSort = nextSort;
                        }
                    }
                }
            }

        }

        private static readonly string[] SortingOptions = { "most recent", "oldest", "most expensive", "least expensive" };
        static string ChangeSortingScreen(string current)
        {
            while(true)
            {
                TopMessage = "Sorted by " + current;
                ClearScreen();
                System.Console.WriteLine("Select from the following:");
                for (int i = 0; i < SortingOptions.Length; i++)
                    System.Console.WriteLine((i + 1) + " : " + SortingOptions[i]);
                System.Console.WriteLine("ESC : Cancel");
                char input = System.Console.ReadKey().KeyChar;
                if (input - '1' < SortingOptions.Length)
                    return SortingOptions[input - '1'];
                else if (input == 27)
                    return current;
            }
        }

        static void ShowOrderDetailsScreen(Order o)
        {
            while (true)
            {
                ClearScreen();
                System.Console.WriteLine(o.FullDetails());
                System.Console.WriteLine("ESC : Return");
                char input = System.Console.ReadKey().KeyChar;
                if (input == (char)27)
                    return;
            }

        }
    

       //static readonly string DefaultLocationMesage = "Use saved location?";
       static readonly string ChooseLocationMessage = "Please select location:";
         static void SelectLocationScreen()
        {
            
            while (true)
            {
                //note assumes small number of location options <10
                //If more, consider scrolling/subset chosen
                //possible serach new location
                ClearScreen();
                System.Console.WriteLine(ChooseLocationMessage);
                List<int> ids = new List<int>();
                foreach (Library.Store loc in Library.Store.Stores())
                    ids.Add(loc.Id);
                ids.Sort();
                for (int i = 0; i < ids.Count; i++)
                {
                    System.Console.WriteLine((i + 1) + " : Store #" + ids[i]);
                }
                System.Console.WriteLine("ESC : Cancel");
                char input = System.Console.ReadKey().KeyChar;
                if (input > '0' && input <= '0' + ids.Count)
                {
                    CurrentLocation = Library.Store.GetStore(ids[input - '1']);
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
            "Change Location"
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
                System.Console.WriteLine("ENTER : Complete Order");
                System.Console.WriteLine("ESC : Cancel Order");
                char input = System.Console.ReadKey().KeyChar;
                System.Console.WriteLine();
                PZ pizza = null;
                int pizzaID = -1;
                switch (input)
                {
                    case '1':
                        if (order.CanAddPizza())
                        {
                            pizza = CreatePizzaScreen();
                            if (pizza != null)
                                order.AddPizza(pizza);
                        }
                        else
                        {
                            TopMessage = "Cannot add pizza: Order is full";
                        }
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
                    case (char)13://Complete order
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
                                System.Console.WriteLine(PressEnterMessage);
                                PizzaRepository.Repo().PlaceOrder(order);
                                System.Console.ReadLine();
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
        
        static PZ CreatePizzaScreen()
        {
            PZ pizza = new PZ();
            //Select size
            while(true)
            {
                ClearScreen();
                System.Console.WriteLine("What Size?");
                for (int i = 0; i < PZ.Sizes.Length; i++)
                    System.Console.WriteLine((i+1)+" : "+PZ.Sizes[i]);
                System.Console.WriteLine("ESC : Cancel Pizza");
                char input = System.Console.ReadKey().KeyChar;
                if (input == 27)
                    return null;
                else if (input >= '1' && input <= '1'+ PZ.Sizes.Length)
                {
                    pizza.Size = (PZ.SizeType)(input - '1' + PZ.SizeType.Small);
                    break;
                }
            }
            //Select crust
            while (true)
            {
                ClearScreen();
                System.Console.WriteLine("What Crust?");
                for (int i = 0; i < PZ.CrustTypes.Length; i++)
                    System.Console.WriteLine((i + 1) + " : " + PZ.CrustTypes[i]);
                System.Console.WriteLine("ESC : Cancel Pizza");
                char input = System.Console.ReadKey().KeyChar;
                if (input == 27)
                    return null;
                else if (input >= '1' && input <= '1' + PZ.CrustTypes.Length)
                {
                    pizza.Crust = (PZ.CrustType)(input - '1');
                    break;
                }
            }
            //Select sauce
            while (true)
            {
                ClearScreen();
                System.Console.WriteLine("Sauce?");
                for (int i = 0; i < PZ.Amounts.Length; i++)
                    System.Console.WriteLine((i + 1) + " : " + PZ.Amounts[i]);
                System.Console.WriteLine("ESC : Cancel Pizza");
                char input = System.Console.ReadKey().KeyChar;
                if (input == 27)
                    return null;
                else if (input >= '1' && input <= '1'+ PZ.Amounts.Length)
                {
                    pizza.Toppings[(int)PZ.ToppingType.Sauce] = (PZ.Amount)(input - '1');
                    break;
                }
            }
            //select cheese
            while (true)
            {
                ClearScreen();
                System.Console.WriteLine("Cheese?");
                for (int i = 0; i < PZ.Amounts.Length; i++)
                    System.Console.WriteLine((i + 1) + " : " + PZ.Amounts[i]);
                System.Console.WriteLine("ESC : Cancel Pizza");
                char input = System.Console.ReadKey().KeyChar;
                if (input == 27)
                    return null;
                else if (input >= '1' && input <= '1'+ PZ.Amounts.Length)
                {
                    pizza.Toppings[(int)PZ.ToppingType.Cheese] = (PZ.Amount)(input - '1');
                    break;
                }
            }
            //select toppings
            bool complete = ToppingSelectScreen(pizza);
            if (!complete)
                pizza = null;
            return pizza;
    }
        static bool ToppingSelectScreen(PZ pizza)
        {
            while (true)
            {
                ClearScreen();
                System.Console.WriteLine("Toppings? (Select to change amount)");
                //toppings start at 2 (pepperoni)
                for (int i = 2; i < PZ.ToppingTypes.Length; i++)
                {
                    string printMe = "";
                    if (i < 11)
                        printMe = (i-1) + " : " + PZ.ToppingTypes[i] +" : "  + pizza.Toppings[i].ToString();
                    else if (i == 11)
                        printMe = "0 : " + PZ.ToppingTypes[i] + " : " + pizza.Toppings[i].ToString();
                    else
                    {
                        char select = (char)('a' + (i - 12)); //12 starts with 'a'
                        printMe = select + " : " + PZ.ToppingTypes[i] + " : " + pizza.Toppings[i].ToString();
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
                    topping = -11;
                }
                //toppings 12+. MaxTopping = Length -1
                else if (input >= 'a' && input <= ('a' + (PZ.ToppingTypes.Length-1)-12))
                {
                    topping = (input - 'a' + 12);
                }
                if(topping > -1)
                {
                    int amount = HowMuchScreen(topping);
                    pizza.Toppings[topping] = (PZ.Amount)amount;
                }
            }
        }
        
        static int HowMuchScreen(int topping)
        {
            while (true)
            {
                ClearScreen();
                System.Console.WriteLine(PZ.ToppingTypes[topping]+ "?");
                for (int i =0; i < PZ.Amounts.Length; i++)
                {
                    System.Console.WriteLine((i + 1) + " : " + PZ.Amounts[i]);
                }

                System.Console.WriteLine("ESC : Cancel Topping");
                char input = System.Console.ReadKey().KeyChar;
                if (input >= '1' && input <= '1'+ Library.Pizza.Amounts.Length)
                {
                    return (int)(input - '1');
                }
                else if (input == 27)
                {
                    return 0;
                }
            }

        }

        static readonly string ExitMessage = "Goodbye for now!\r\n\r\n(Press ENTER to close)";
        static void ExitScreen()
        {
            CurrentUser = null;
            TopMessage = ExitMessage;
            ClearScreen();
            System.Console.ReadLine();
            return;
        }
        */
    }
}
