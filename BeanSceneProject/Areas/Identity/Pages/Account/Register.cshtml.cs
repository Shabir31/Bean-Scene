// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using BeanSceneProject.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace BeanSceneProject.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,

            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }


        [BindProperty]
        public InputModel Input { get; set; }



        public class InputModel
        {
            
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public string PhoneNumber { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string OptionalCode { get; set; }
        }

        public void OnGetAsync()
        {

        }

        //Below used to assign roles once a certain code has been entered

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new Member() { IdentityUser = new IdentityUser { Email = Input.Email, PhoneNumber = Input.PhoneNumber, UserName = Input.Email }, FirstName = Input.FirstName, LastName = Input.LastName,  };


                var result = await _userManager.CreateAsync(user.IdentityUser, Input.Password);

                var roleCode = _context.RoleCodes.First();

                if (result.Succeeded)
                {
                    if (Input.OptionalCode == roleCode.ManagerCode)
                    {
                        await _userManager.AddToRoleAsync(user.IdentityUser, "Manager");
                    }
                    else if (Input.OptionalCode == roleCode.StaffCode)
                    {
                        await _userManager.AddToRoleAsync(user.IdentityUser, "Staff");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user.IdentityUser, "Member");
                        var newMember = _context.Members.Add(user);
                        _context.SaveChanges();
                    }
                    await _signInManager.SignInAsync(user.IdentityUser, isPersistent: false);
                    return RedirectToAction("RedirectUser", "Home", new { area = "" });

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }



        

       
    }
}
