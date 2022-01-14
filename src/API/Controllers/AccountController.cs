using System.Security.Claims;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using AutoMapper;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager,
         SignInManager<AppUser> signInManager,
         ITokenService tokenService,
         IMapper mapper)
     {
        _tokenService = tokenService;
        _mapper = mapper;
        _signInManager = signInManager;   
        _userManager = userManager;
     }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {

        var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            Username = user.DisplayName
        };
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery]string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetUserAddress()
    {

        var user = await _userManager.FindByEmailWithAddressAsync(HttpContext.User);

        return _mapper.Map<Address,AddressDto>(user.Address);
    }

    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
    {
        var user = await _userManager.FindByEmailWithAddressAsync(HttpContext.User);
        
        user.Address = _mapper.Map<AddressDto,Address>(address);

        var result = await _userManager.UpdateAsync(user);

        if(result.Succeeded) return Ok(_mapper.Map<Address,AddressDto>(user.Address));

        return BadRequest("Problem updateing the user");

    }

     [HttpPost("login")]
     public async Task<ActionResult<UserDto>> Login(LoginDto req)
     {
        var user = await _userManager.FindByEmailAsync(req.Email);

        if (user == null) return Unauthorized(new ApiResponse(401));

        var result = await _signInManager.CheckPasswordSignInAsync(user,req.Password,false);

        if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user), 
            Username = user.DisplayName
        };
     }


    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto req)
    {
        var user = new AppUser
        {
            DisplayName = req.UserName,
            Email = req.Email,
            UserName = req.UserName,
        };

        var result = await _userManager.CreateAsync(user,req.Password);

        if (!result.Succeeded) return BadRequest(new ApiResponse(400));
        
        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            Username = user.DisplayName
        };
    }
}
}