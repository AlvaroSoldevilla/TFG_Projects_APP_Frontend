using TFG_Projects_APP_Frontend.Entities.Models;

namespace TFG_Projects_APP_Frontend.Services.UsersService;

public class UserSession
{
    public AppUser User { get; set; } = new() { Id = 1, Username = "Admin", Email = "admin@test.com"};
    public string Token { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiYWRtaW5AdGVzdC5jb20iLCJleHBpcmVzIjoxNzQ5Mjk1MTczLjc3MTcyMzV9.WRrHKpL-28lKdo-aE2ctgYU4_RW0CTAK52G3Rk8RJK0";
}
