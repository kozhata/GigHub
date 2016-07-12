using GigHub.Controllers;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using GigHub.Controllers.MVC;

namespace GigHub.ViewModels
{
    public class GigFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        [DisplayName("Genre")]
        public int GenreId { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public string Heading { get; set; }

        public string Action
        {
            get
            {
                Expression<Func<GigsController, ActionResult>> update = (x => x.Edit(this));
                Expression<Func<GigsController, ActionResult>> create = (x => x.Create(this));

                var action = (Id != 0) ? update : create;
                var methodCallExpression = action.Body as MethodCallExpression;

                if (methodCallExpression == null)
                {
                    throw new Exception();
                }

                string actionName = methodCallExpression.Method.Name;

                return actionName;
            }
        }

        public DateTime GetDate()
        {
            return DateTime.ParseExact($"{Date} {Time}", "dd MMMM yyyy HH mm", CultureInfo.InvariantCulture);
        }
    }
}