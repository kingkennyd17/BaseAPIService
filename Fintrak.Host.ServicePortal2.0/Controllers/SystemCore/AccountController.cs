using Fintrak.Model.SystemCore.Common;
using Fintrak.Model.SystemCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Fintrak.Service.SystemCore.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Fintrak.Host.ServicePortal2._0;
using Fintrak.Model.SystemCore.DTO;
using Fintrak.Model.SystemCore.Enum;

namespace Fintrak.Host.ServicePortal2._0.Controllers.SystemCore
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserSetup> _userManager;
        private readonly SignInManager<UserSetup> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ISystemCoreManager _systemCoreManager;

        public AccountController(UserManager<UserSetup> userManager, SignInManager<UserSetup> signInManager, IConfiguration configuration, ISystemCoreManager systemCoreManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _systemCoreManager = systemCoreManager;
        }
        //NewTenant
        [Authorize(Roles = "AdminMaker")]
        [HttpPost("createuser")]
        public async Task<IActionResult> Register(RegisterUserDto registerDto)
        {
            var user = new UserSetup { UserName = registerDto.Username, Email = registerDto.Email, 
                                        Name = registerDto.Name, 
                                        StaffID = registerDto.StaffID };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assign the role to the user
            await _userManager.AddToRolesAsync(user, registerDto.Roles);

            return Ok();
        }

        [Authorize(Roles = "TenantAdmin")]
        [HttpPost("createnewtenant")]
        public async Task<IActionResult> NewTenant(NewTenant newTenant)
        {
            var user = new UserSetup
            {
                UserName = newTenant.Username,
                Email = newTenant.Email,
                Name = newTenant.Name,
                StaffID = newTenant.StaffID,
                TenantId = newTenant.TenantId,
                ApprovalStatus = ApprovalStatus.Approved
            };
            var result = await _userManager.CreateAsync(user, newTenant.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assign the role to the user
            await _userManager.AddToRolesAsync(user, new string[] {"AdminMaker", "AdminChecker"});

            return Ok();
        }

        [HttpGet]
        [Route("getallusers")]
        [Authorize(Roles = "AdminMaker, AdminChecker")]
        public async Task<ActionResult<IEnumerable<UserSetup>>> GetAllUsers()
        {
            var response = await _systemCoreManager.GetAllUserSetupByTenant();
            return Ok(response);
        }

        [HttpGet]
        [Route("getallroles")]
        [Authorize(Roles = "AdminMaker, AdminChecker")]
        public async Task<ActionResult<IEnumerable<Roles>>> GetAllRoles()
        {
            var roles = await _systemCoreManager.GetRoles();
            return Ok(roles);
        }

        [HttpGet]
        [Route("getuserdetails/{userId}")]
        [Authorize(Roles = "AdminMaker, AdminChecker")]
        public async Task<ActionResult<UserDetail>> GetUserDetails([FromRoute]string userId)
        {
            UserDetail userDetail = new UserDetail();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized();
            }
            userDetail.User = user;
            var currentRoles = await _userManager.GetRolesAsync(userDetail.User);
            userDetail.Roles = currentRoles.ToList();
            return Ok(userDetail);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.IsNotAllowed)
            {
                return Forbid("Your account is not approved yet.");
            }
            else if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var token = await GenerateJwtToken(user);

            // Update the user's current session ID
            user.CurrentSessionId = token;
            await _systemCoreManager.UserSetupUpdateTokenAsync(user);

            return Ok(new { token });
        }

        [Authorize(Roles = "AdminMaker")]
        [HttpPut("updateuser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDetail model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.User.Id.ToString());
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Update user details
            user.Email = model.User.Email;
            user.Name = model.User.Name;
            user.StaffID = model.User.StaffID;
            user.ApprovalStatus = ApprovalStatus.Pending;
            //user.PhoneNumber = model.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Remove user from all roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                return BadRequest(removeResult.Errors);
            }

            // Add user to the new roles
            var addResult = await _userManager.AddToRolesAsync(user, model.Roles);

            if (!addResult.Succeeded)
            {
                return BadRequest(addResult.Errors);
            }

            if (result.Succeeded && removeResult.Succeeded && addResult.Succeeded)
            {
                return Ok("User updated successfully");
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "AdminChecker")]
        [HttpPost("lockuser")]
        public async Task<IActionResult> LockUser(string userId, DateTime? lockoutEnd)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Lock the user
            var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd ?? DateTime.UtcNow.AddYears(100)); // Lock indefinitely if lockoutEnd is not provided

            if (result.Succeeded)
            {
                return Ok("User locked successfully");
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "AdminChecker")]
        [HttpPost("unlockuser")]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Unlock the user
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow);

            if (result.Succeeded)
            {
                return Ok("User unlocked successfully");
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "AdminChecker")]
        [HttpPost("approveuser")]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.ApprovalStatus = ApprovalStatus.Approved;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok("User approved successfully");
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "AdminChecker")]
        [HttpPost("rejectuser")]
        public async Task<IActionResult> RejectUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.ApprovalStatus = ApprovalStatus.Rejected;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok("User rejected successfully");
            }

            return BadRequest(result.Errors);
        }


        private async Task<string> GenerateJwtToken(UserSetup user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("TenantId", user.TenantId),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Name, user.Id.ToString())
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
