﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020078.DomainModels
{
    public class Employee
    {
        /// <summary>
        /// Nhân viên
        /// </summary>
        public int EmployeeID { get; set; }
        public string FullName { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Photo { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsWorking { get; set; } = true;
    }
}
