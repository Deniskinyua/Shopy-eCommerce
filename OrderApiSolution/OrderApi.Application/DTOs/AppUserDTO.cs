using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs;

public record AppUserDTO(
     int UserId,
    [Required] string UserName,
     [Required] string Phone,
     [Required] string Address,
     [Required] string Email,
     [Required] string Password,
     [Required] string Role
    );