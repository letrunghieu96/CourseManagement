﻿using CourseManagement.Domain;
using CourseManagement.Models;
using CourseManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Controllers
{
    public class HomeController : ControllerBase<HomeController>
    {
        public HomeController(IConfiguration config, IDomainFacade domainFacade)
            : base(config, domainFacade)
        {
        }

        [HttpGet("/Home")]
        public IActionResult Index()
        {
            var model = this.Service.CreateModel();
            return View(model);
        }


        #region +Properties
        /// <summary>Index work service</summary>
        private HomeService Service => new HomeService(_config, _domainFacade);
        #endregion
    }
}