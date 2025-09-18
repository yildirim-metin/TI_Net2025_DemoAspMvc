using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using TI_Net2025_DemoAspMvc.Extensions;
using TI_Net2025_DemoAspMvc.Mappers;
using TI_Net2025_DemoAspMvc.Models.Dtos.User;
using TI_Net2025_DemoAspMvc.Models.Entities;
using TI_Net2025_DemoAspMvc.Repositories;

namespace TI_Net2025_DemoAspMvc.Controllers;

public class UserController : Controller
{
    private readonly UserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public ActionResult Register()
    {
        return View(new RegisterFormDto());
    }

    [HttpPost]
    public IActionResult Register([FromForm] RegisterFormDto registerForm)
    {
        bool isValid = true;
        if (!ModelState.IsValid)
        {
            isValid = false;
        }

        if (_userRepository.ExistByEmail(registerForm.Email))
        {
            ModelState.AddModelError<RegisterFormDto>(f => f.Email, "Email already exist");
            isValid = false;
        }

        if (_userRepository.ExistByUsername(registerForm.Username))
        {
            ModelState.AddModelError<RegisterFormDto>(f => f.Username, "Username already exist");
            isValid = false;
        }

        if (!isValid)
        {
            registerForm.Password = "";
            return View(registerForm);
        }

        User user = registerForm.ToUser();
        user.Password = Argon2.Hash(registerForm.Password);
        user.Role = Models.UserRole.Admin;

        _userRepository.Add(user);

        return RedirectToAction("Index", "/User/Login");
    }

    public IActionResult Login()
    {
        return View(new LoginFormDto());
    }

    [HttpPost]
    public IActionResult Login([FromForm] LoginFormDto loginForm)
    {
        if (!ModelState.IsValid)
        {
            loginForm.Password = "";
            return View(loginForm);
        }

        User? user = _userRepository.GetUserByUsernameOrEmail(loginForm.Login);
        if (user == null)
        {
            ModelState.AddModelError<LoginFormDto>(m => m.Login, "User not found");
            loginForm.Password = "";
            return View(loginForm);
        }

        if (!Argon2.Verify(user.Password, loginForm.Password))
        {
            ModelState.AddModelError<LoginFormDto>(m => m.Password, "Wrong password");
            loginForm.Password = "";
            return View(loginForm);
        }

        ClaimsPrincipal claims = new(
            new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            ], CookieAuthenticationDefaults.AuthenticationScheme));

        HttpContext.SignInAsync(claims);

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpPost]
    public IActionResult Logout()
    {
        Console.WriteLine($"Id: {User.GetId()}");
        Console.WriteLine($"Role: {User.GetRole()}");
        HttpContext.SignOutAsync();
        return RedirectToAction("Login", "User");
    }
}
