using System;
using System.Collections.Generic;
using System.IO;

namespace CSProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
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
            
            }
            return staffList;
        }
    }
}

