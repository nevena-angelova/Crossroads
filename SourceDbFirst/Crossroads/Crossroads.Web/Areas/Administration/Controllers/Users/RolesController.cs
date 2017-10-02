using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crossroads.Data;
using Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Roles;
using Crossroads.Web.Controllers;
using Crossroads.Web.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Crossroads.Web.Areas.Administration.Controllers.Users
{
    [Authorize(Roles = UserRoles.AdminRole)]
    public class RolesController : BaseController
    {
        public RolesController(ICrossroadsData data)
            : base(data)
        {
        }

        public ActionResult ListRoles(bool? ordered)
        {
            IList<RoleOutputViewModel> roles = null;

            if (ordered == true)
            {
                roles = this.Data.Roles.All()
                    .OrderBy(i => i.Name)
                    .Project()
                    .To<RoleOutputViewModel>()
                    .ToList();
            }
            else
            {
                roles = this.Data.Roles.All()
                    .Project()
                    .To<RoleOutputViewModel>()
                    .ToList();
            }

            return this.View(Views.ListRoles, roles);
        }

        public ActionResult AddRole()
        {
            return this.PartialView(Partials.AddRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(RoleInputViewModel role)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (role != null && ModelState.IsValid)
            {
                Role dbRole = Mapper.Map<Role>(role);
                this.Data.Roles.Add(dbRole);
                this.Data.SaveChanges();

                RoleOutputViewModel newRole = Mapper.Map<RoleOutputViewModel>(dbRole);

                return this.PartialView(Partials.Role, newRole);
            }

            return this.PartialView(Partials.AddRole);
        }

        public ActionResult EditRole(int id, bool? refuse)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Role dbRole = this.Data.Roles.GetById(id);
            if (dbRole == null)
            {
                return HttpNotFound("Role not found!");
            }

            RoleInputViewModel role = Mapper.Map<RoleInputViewModel>(dbRole);
            if (refuse == true)
            {
                return this.Content(role.Name);
            }

            return this.PartialView(Partials.EditRole, role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(RoleInputViewModel role)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (role != null && ModelState.IsValid)
            {
                Role dbRole = this.Data.Roles.GetById(role.Id);
                dbRole.Name = role.Name;

                this.Data.SaveChanges();

                return this.Content(role.Name);
            }

            return this.PartialView(Partials.EditRole, role);
        }

        public ActionResult DeleteRole(int id)
        {
            Role role = this.Data.Roles.GetById(id);
            if (role == null)
            {
                return HttpNotFound("Role not found!");
            }

            this.Data.Roles.Delete(role);

            this.Data.SaveChanges();

            return this.Content(String.Empty);
        }
    }
}