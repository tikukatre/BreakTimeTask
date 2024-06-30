using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
class BusStation{

    static List<BreakTime> breakTimes = new List<BreakTime>();
    


/*
Method AddBreakTime parses a break times from an input string and adds it to breakTimes list 
and uses FindBusiestPeriod method to update the busiest period based on new times
*/
    static void AddBreakTime(string input){
        if (input.Length != 10)
        {
            Console.WriteLine($"Invalid input format: {input}. Please use 'HH:mmHH:mm'.");
            return;
        }

        string startTimeString = input.Substring(0, 5);
        string endTimeString = input.Substring(5, 5);
        
        if (DateTime.TryParseExact(startTimeString, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime start) &&
            DateTime.TryParseExact(endTimeString, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime end)) {
            breakTimes.Add(new BreakTime { Start = start, End = end });
        } else {
            Console.WriteLine("Invalid time format. Please use 'HH:mm'.");
        }

    }

    /*
    Method FindBusiestPeriod finds the busiest break period based on the added break times in the breakTimes list
    */
    static void FindBusiestPeriod()
    {
        DateTime busiestStart = DateTime.MinValue;//Start time of the busiest period
        DateTime busiestEnd = DateTime.MinValue;//End time of the busiest period

        int maxDriversOnBreak = 0; //Amount of drivers that are on the break during the busiest period

        if (!breakTimes.Any()) {
            Console.WriteLine("No break times available.");
            return;
        }
        
        var times = new List<Tuple<DateTime, bool>>();
        DateTime lowestEndTime = DateTime.MaxValue;
        foreach (var breakTime in breakTimes)
        {
            //Adding all the times to the times list and marking whether the time is start or end of the break
            times.Add(new Tuple<DateTime, bool>(breakTime.Start, true));//Start of a break
            times.Add(new Tuple<DateTime, bool>(breakTime.End, false));//End of a break
            if (breakTime.End< lowestEndTime){
                lowestEndTime=breakTime.End;
            }
        }

        times.Sort((a, b) => a.Item1.CompareTo(b.Item1));//Sorting the times in the list

        int currentlyOnBreak = 0;

        foreach (var time in times)
        {
            if (time.Item2) // Checking if it is break start time
            {
                currentlyOnBreak++; //Increasing the amount of drivers that are currently on a break
                if (currentlyOnBreak > maxDriversOnBreak) 
                /* If there are currently more divers on the break than on the amount of drivers on the busiest period
                we replace the driver count to the new one and replace the busiest start time
                */
                {
                    maxDriversOnBreak = currentlyOnBreak;
                    busiestStart = time.Item1;
                }
            }
            else 
            {
                if (currentlyOnBreak == maxDriversOnBreak) 
                // If the currently on breaak count is the same as the drivers on the busiest period we have found the end of the busiest period
                {
                    if (busiestStart<lowestEndTime && busiestEnd<lowestEndTime){
                        busiestEnd = time.Item1;
                    }
                    if (busiestStart>lowestEndTime){
                        Console.WriteLine(" buestestStart lower "+ busiestStart+ " end " + busiestEnd);
                        lowestEndTime=busiestEnd;
                        busiestEnd = time.Item1;
                    }
                }
                currentlyOnBreak--;//Decreasing the amount of drivers on break since one driver's break ended
            }
        }

        Console.WriteLine($"The busiest period is {busiestStart:HH:mm}-{busiestEnd:HH:mm} with {maxDriversOnBreak} drivers on break.");
    }


    static void Main(string[] args)
    {
        if (args.Length == 2 && args[0] == "filename") {
            // Checking if there is filename argument and then reading it
            string filePath = args[1];
            if (File.Exists(filePath)) {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines) {
                    AddBreakTime(line.Trim());//Adding the times 
                }
            FindBusiestPeriod(); //Finding the busiest period based on the provided file
            } else {
                Console.WriteLine("File not found.");
                return;
            }
        } 

        Console.WriteLine("Enter break times in the format 'HH:mmHH:mm' (example '13:1514:00'). Type 'exit' to terminate the program.");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            
            if (input.ToLower() == "exit")
                break;

            AddBreakTime(input);//Adding the new time
            FindBusiestPeriod();//Finding the new busiest period
        }
    }
}