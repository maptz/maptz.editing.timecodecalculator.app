using Maptz.Editing.TimeCodeCalculator.Engine;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Maptz.Editing.TimeCodeCalculator.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<ICalculator, Calculator>();
            services.AddTransient<ICommandRepository, CommandRepository>();

            var serviceProvider = services.BuildServiceProvider();

            var calculator = serviceProvider.GetRequiredService<ICalculator>();
            Console.WriteLine("Calculator initialized");
            string readLine = Console.ReadLine();
            while(readLine != "exit")
            {
                calculator.ParseInput(readLine);
                var buffer = calculator.State.Buffer;
                var output = buffer.Length == 0 ? string.Empty : buffer[buffer.Length - 1];
                
                Console.WriteLine(output);
                readLine = Console.ReadLine();
            }

        }
    }
}
