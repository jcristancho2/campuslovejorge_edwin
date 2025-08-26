using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace campuslovejorge_edwin.Src.Modules.MainMenu
{
    public class MainMenu
    {
        public void Show()
        {
            int option = -1;
            do
            {
                Console.Clear();
                Console.WriteLine("      Campus Love   ");
                Console.WriteLine("1. Registro");
                Console.WriteLine("2. login");
                Console.WriteLine("3. exit");
                Console.Write("Select an option: ");

                if (!int.TryParse(Console.ReadLine(), out option))
                {
                    Console.WriteLine("Invalid input, press any key...");
                    Console.ReadKey();
                    continue;
                }

                switch (option)
                {
                    case 1:
                    //reguistro
                        break;
                    case 2:
                    //login
                        break;
                    case 3:
                        Console.WriteLine("👋 Exiting... Bye!");
                        break;
                    default:
                        Console.WriteLine("❌ Invalid option. Try again.");
                        Console.ReadKey();
                        break;
                }
            } while (option != 0);
        }
    }
}

