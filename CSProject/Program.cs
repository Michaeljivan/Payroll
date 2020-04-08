using System;
using System.Collections.Generic;
using System.IO;

namespace CSProject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Staff> staffList = new List<Staff>();
            FileReader fr = new FileReader();
            int month = 0;
            int year = 0;

            while(year == 0)
            {
                Console.Write("\nPlease enter the year: ");
                
                try
                {
                    year = Convert.ToInt32(Console.ReadLine());
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message + " Please try again.");
                }
            }

            while(month == 0)
            {
                Console.Write("\nPlease enter the month: ");
                
                try
                {
                    month = Convert.ToInt32(Console.ReadLine());
                    
                    if(month < 1 || month > 12)
                    {
                        Console.WriteLine("Month must be from 1 to 12. Please try again.");
                        month = 0;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message + " Please try again.");
                }
            }

            staffList = fr.ReadFile();
            //continue here page 150.
            for(int i = 0; i < staffList.Count; i++)
            {
                try
                {
                    Console.WriteLine("\nEnter hours worked for {0}", staffList[i].NameOfStaff);
                    staffList[i].HoursWorked = Convert.ToInt32(Console.ReadLine());
                    staffList[i].CalculatePay();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    i--;
                }
            }

            PaySlip payslip = new PaySlip(month, year);
            payslip.GeneratePaySlip(staffList);
            payslip.GenerateSummary(staffList);

            Console.Read();

        }
    }

    class Staff
    {
        private float hourlyRate;
        private int hWorked;

        public float TotalPay { get; protected set; }

        public float BasicPay { get; private set; }

        public string NameOfStaff { get; private set; }

        public int HoursWorked
        {
            get
            {
                return hWorked;
            }

            set 
            {
                if (value > 0)
                    hWorked = value;
                else
                    hWorked = 0;
            }
        }

        public Staff(string name, float rate)
        {
            NameOfStaff = name;
            hourlyRate = rate;
        }

        public virtual void CalculatePay()
        {
            BasicPay = hWorked * hourlyRate;
            TotalPay = BasicPay;
        }

        public override string ToString()
        {
            return "Total Pay:" + TotalPay + "/n"
                +"";
        }
    }

    class Manager : Staff
    {
        private const float managerHourlyRate = 50;

        public int Allowance { get; private set; }

        public Manager (string name) : base (name, managerHourlyRate)
        { 
            
        }

        public override void CalculatePay()
        {
            base.CalculatePay();
            Allowance = 1000;

            if (HoursWorked > 160)
            {
                TotalPay += Allowance;
            }
        }

        public override string ToString()
        {
            return "Manager Total Pay: " + TotalPay;
        }
    }

    class Admin : Staff 
    {
        private const float overtimeRate = 15.5f;
        private const float adminHourlyRate = 30f;

        public float Overtime { get; private set; }

        public Admin(string name) : base(name, adminHourlyRate)
        {        
        }

        public override void CalculatePay()
        {
            base.CalculatePay();

            if (HoursWorked > 160)
                TotalPay += (overtimeRate * (HoursWorked - 160));
        }

        public override string ToString()
        {
            return "Admin Total Pay: " + TotalPay;
        }

    }

    class FileReader 
    {
        public List<Staff> ReadFile()
        {
            List<Staff> staffList = new List<Staff>();
            string[] result = new string[2];
            string path = "staff.txt";
            string[] separator = { ", " };

            if (File.Exists(path))
            { 
                using (StreamReader sr = new StreamReader(path))
                    {
                        while(!sr.EndOfStream)
                        {
                            result = sr.ReadLine().Split(separator,StringSplitOptions.RemoveEmptyEntries);
                            
                            if(result[1] == "Manager")
                                staffList.Add(new Manager(result[0]));
                            else if(result[1] == "Admin")
                                staffList.Add(new Admin(result[0]));
                        }
                        sr.Close();
                    }
            }
            else
            {
                Console.WriteLine("Error: File does not exist.");
            }
            return staffList;
        }
    }

    class PaySlip
    {
        private int month;
        private int year;

        enum MonthsOfYear {JAN = 1, FEB = 2, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DEC };

        public PaySlip(int payMonth, int payYear)
        {
            month = payMonth;
            year = payYear;
        }

        public void GeneratePaySlip(List<Staff> staffList)
        {
            string path;

            foreach(Staff s in staffList)
            {
                path = s.NameOfStaff + ".txt";

                using(StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("PAYSLIP FOR {0} {1}", month, year);
                    sw.WriteLine("=================================");
                    sw.WriteLine("Name of Staff: {0}", s.NameOfStaff);
                    sw.WriteLine("Hours Worked: {0}", s.HoursWorked);
                    sw.WriteLine("");
                    sw.WriteLine("Basic Pay: {0:C}", s.BasicPay);

                    if(s.GetType() == typeof(Manager))
                    {
                        sw.WriteLine("Allowance: {0:C}", ((Manager)s).Allowance);
                    }
                    else if(s.GetType() == typeof(Admin))
                    {
                        sw.WriteLine("Overtime: {0:C}", ((Admin)s).Overtime);
                    }
                    
                    sw.WriteLine("");
                    sw.WriteLine("=================================");
                    sw.WriteLine("TOTAL PAY: {0:C}", s.TotalPay);
                    sw.WriteLine("=================================");
                }

            }
        }

        public void GenerateSummary(List<Staff> staffList)
        {
            var result = from s in staffList
                         where s.HoursWorked < 10
                         orderby s.NameOfStaff ascending
                         select new {s.NameOfStaff, s.HoursWorked};

            string path = "summary.txt";

            using(StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("Staff with less than 10  working hours");
                sw.WriteLine("");

                foreach( var s in result)
                {
                    sw.WriteLine("Name of Staff: {0}, Hours Worked: {1}");
                }
                sw.Close();
            }
        }

        public override string ToString()
        {
            return "month = " + month + "year = "+ year;
        }

    }
}

