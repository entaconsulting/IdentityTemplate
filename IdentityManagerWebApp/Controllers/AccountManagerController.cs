using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using IdentityManagerWebApp.Dtos;
using IdentityManagerWebApp.Infrastructure;
using IdentityManagerWebApp.Models;
using IdentityManagerWebApp.ViewModels.AccountManager;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGrease.Css.Extensions;

namespace IdentityManagerWebApp.Controllers
{

    public class AccountManagerController : Controller
    {
        private static readonly Random Random = new Random();

        private readonly Dictionary<string, string> _hardcodedMessages = new Dictionary<string, string>() {
            { "DuplicateUserName", HardcodedStrings.DuplicateUserName }
        };

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountManagerController()
        {
        }

        public AccountManagerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /AccountManager
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /AccountManager
        [HttpGet]
        public async Task<ActionResult> AssignRoles(string id, ManageMessageId? message)
        {
            ViewBag.StatusMessage =
               message == ManageMessageId.ActualizacionExitosa ? "Actualización Exitosa"
               : message == ManageMessageId.Error ? "Upss, Houston tenemos un problema :("
               : "";

            UserDetailsDto user = await UserManager.GetDetailsByIdAsync(id);
            AssignRolesViewModel vm = Mapper.Map<AssignRolesViewModel>(user);
            vm.RolesList = Roles.GetRoles();

            return View(vm);

        }

        [HttpGet]
        public async Task<ActionResult> AssignClaims(string id, ManageMessageId? message)
        {
            ViewBag.StatusMessage =
               message == ManageMessageId.ActualizacionExitosa ? "Actualización Exitosa"
               : message == ManageMessageId.Error ? "Upss, Houston tenemos un problema :("
               : "";

            UserDetailsDto user = await UserManager.GetDetailsByIdAsync(id);
            AssignClaimsViewModel vm = Mapper.Map<AssignClaimsViewModel>(user);
            vm.ClaimsList = Claims.GetClaimss();

            return View(vm);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Claims_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ClaimListViewModel> claims, string id)
        {
            if (claims != null && ModelState.IsValid)
            {
                var claimsUser = UserManager.GetClaims(id);

                foreach (var claim in claims)
                {
                    var claimValues = (string.IsNullOrEmpty(claim.ClaimValues)) ? new List<string>().ToArray() : claim.ClaimValues.Split(new[] { ConcatClaim }, StringSplitOptions.None);

                    var claimUserType = claimsUser.Where(a => a.Type == claim.ClaimType).ToList();

                    var claimsToAdd = claimValues.Except(claimUserType.Select(a => a.Value)).ToList();
                    var claimsToRemove = claimUserType.Select(a => a.Value).Except(claimValues).ToList();

                    await UserManager.AddClaimsAsync(id, claim.ClaimType, claimsToAdd);
                    await UserManager.RemoveClaimsAsync(id, claim.ClaimType, claimsToRemove);

                }

            }

            return Json(claims.ToDataSourceResult(request, ModelState));
        }

        private static string ConcatClaim
        {
            get { return "##"; }
        }

        public async Task<ActionResult> Claims_Read([DataSourceRequest] DataSourceRequest request, string id)
        {

            UserDetailsDto user = await UserManager.GetDetailsByIdAsync(id);

            var claims = Claims.GetClaimss()
                .Select(
                    a =>
                        new ClaimListViewModel()
                        {
                            ClaimType = a,
                            ClaimValues = string.Join(ConcatClaim, user.Claims.Where(b => b.Type == a).Select(c => c.Value))
                        });

            return Json(claims.ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<ActionResult> AssignClaims(AssignClaimsViewModel vm)
        {
            try
            {
                await UserManager.AddClaimAsync(vm.Id, new Claim(vm.SelectedClaim, vm.SelectedClaimValue));

                return RedirectToAction("AssignClaims", new { id = vm.Id, message = ManageMessageId.ActualizacionExitosa });
            }
            catch (Exception e)
            {
                return RedirectToAction("AssignClaims", new { id = vm.Id, message = ManageMessageId.Error });
            }

        }

        [HttpPost]
        public async Task<ActionResult> AssignRoles(AssignRolesViewModel vm)
        {
            try
            {
                if (vm.Roles == null)
                    vm.Roles = new List<string>();

                UserDetailsDto user = await UserManager.GetDetailsByIdAsync(vm.Id);

                var rolesToDelete = user.Roles.Except(vm.Roles);

                var rolesToAdd = vm.Roles.Except(user.Roles);

                await UserManager.RemoveFromRolesAsync(vm.Id, rolesToDelete.ToArray());

                await UserManager.AddToRolesAsync(vm.Id, rolesToAdd.ToArray());

                return RedirectToAction("AssignRoles", new { id = vm.Id, message = ManageMessageId.ActualizacionExitosa });
            }
            catch (Exception e)
            {
                return RedirectToAction("AssignRoles", new { id = vm.Id, message = ManageMessageId.Error });
            }

        }

        public enum ManageMessageId
        {
            ActualizacionExitosa,
            Error
        }

        //
        // POST: /AccountManager/List
        [HttpPost]
        public async Task<ActionResult> List([DataSourceRequest]DataSourceRequest request)
        {
            int page = request?.Page ?? 1;
            int pageSize = request?.PageSize ?? 20;

            DataSourceResult result = new DataSourceResult();
            result.Data = Mapper.Map<IEnumerable<UserListViewModel>>(await UserManager.GetPagedAsync(page, pageSize));
            result.Total = await UserManager.GetCountAsync();

            return Json(result);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            UserDetailsDto user = await UserManager.GetDetailsByIdAsync(id);
            DetailsViewModel vm = Mapper.Map<DetailsViewModel>(user);

            vm.ClaimsList = Claims.GetClaimss()
                .Select(
                    a =>
                        new ClaimListViewModel()
                        {
                            ClaimType = a,
                            ClaimValues = string.Join(ConcatClaim, user.Claims.Where(b => b.Type == a).Select(c => c.Value))
                        }).ToList();

            return View(vm);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id, ManageMessageId? message)
        {
            ViewBag.StatusMessage =
               message == ManageMessageId.ActualizacionExitosa ? "Actualización Exitosa"
               : message == ManageMessageId.Error ? "Upss, Houston tenemos un problema :("
               : "";
            
            UserDetailsDto user = await UserManager.GetDetailsByIdAsync(id);
            EditViewModel vm = Mapper.Map<EditViewModel>(user);

            return View(vm);
        }

        //
        // POST: /AccountManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(model.Id);
             
                user.Email = model.Email;
                user.UserName = model.UserName;

                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Edit", new { model.Id, message = ManageMessageId.ActualizacionExitosa });
                
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult CreateSuccess()
        {
            return View(new CreateSuccessViewModel()
            {
                CreatedUserName = TempData["CreatedUserName"].ToString(),
                Password = TempData["Password"].ToString()
            });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                var errorString = string.Empty;
                _hardcodedMessages.TryGetValue(error, out errorString);


                ModelState.AddModelError(string.Empty, string.IsNullOrEmpty(errorString) ? error : errorString);
            }
        }

        private string GenerateRandomPassword()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8) + "+" + Random.Next(0, 9).ToString() + RandomChar(false) + RandomChar(true);
        }

        private char RandomChar(bool uppercase)
        {
            int num = Random.Next(0, 26);
            char let = (char)((uppercase ? 'A' : 'a') + num);
            return let;
        }
    }
}