﻿using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace SV20T1020078.Web.Controllers
{
    public class TestController : Controller
    {
      public IActionResult Create()
        {
            var model = new Models.Person()
            {
                Name = "Trần Nguyên Phong",
                Birthday = new DateTime(1990, 10, 28),
                Salary = 500.25m,

            };
            return View(model);
        }
        public IActionResult Save(Models.Person model,string birthdayInput="")
        {
            //Chuyển chuỗi birthdayInput thành giá trị ngày , nếu hợp lệ thì mới dùng giá trị do người dùng nhập 
            DateTime? d = null;
            try
            {
                d = DateTime.ParseExact(birthdayInput, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }
            //Nếu mà có giá trị 
            if(d.HasValue)
            {
                model.Birthday = d.Value;
            }

            return Json(model);
        }
    }
}
