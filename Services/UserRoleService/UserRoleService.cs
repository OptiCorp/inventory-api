// using Inventory.Models;
// using Microsoft.EntityFrameworkCore;
//
// namespace Inventory.Services
// {
//     public class UserRoleService : IUserRoleService
//     {
//         private readonly InventoryDbContext _context;
//
//         public UserRoleService(InventoryDbContext context)
//         {
//             _context = context;
//         }
//
//
//         public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
//         {
//             return await _context.UserRoles.ToListAsync();
//         }
//
//         public async Task<UserRole> GetUserRoleByIdAsync(string id)
//         {
//             return await _context.UserRoles.FirstOrDefaultAsync(userRole => userRole.Id == id);
//
//         }
//
//         public async Task<string> CreateUserRoleAsync(UserRole userRole)
//         {
//             _context.UserRoles.Add(userRole);
//             await _context.SaveChangesAsync();
//
//             return userRole.Id;
//         }
//
//         public async Task UpdateUserRoleAsync(UserRole updatedUserRole)
//         {
//             var userRole = await _context.UserRoles.FirstOrDefaultAsync(userRole => userRole.Id == updatedUserRole.Id);
//
//             if (userRole != null)
//             {
//                 if (updatedUserRole.Name != null)
//                 {
//                     userRole.Name = updatedUserRole.Name;
//                 }
//
//                 await _context.SaveChangesAsync();
//             }
//         }
//
//         public async Task DeleteUserRoleAsync(string id)
//         {
//
//             var userRole = await GetUserRoleByIdAsync(id);
//
//             if (userRole != null)
//             {
//                 _context.UserRoles.Remove(userRole);
//                 await _context.SaveChangesAsync();
//             }
//
//         }
//         
//         public async Task<bool> IsUserRoleNameTaken(string userRoleName)
//         {
//             var userRoles = await _context.UserRoles.ToListAsync();
//             return userRoles.Any(userRole => userRole.Name == userRoleName);
//         }
//
//         public async Task<bool> IsValidUserRole(string userRoleId)
//         {
//             var userRoles = await _context.UserRoles.ToListAsync();
//             return userRoles.Any(userRole => userRole.Id == userRoleId);
//         }
//         
//         public async Task<bool> IsUserRoleInUseAsync(UserRole userRole)
//         {
//             return await _context.User.AnyAsync(user => user.UserRole == userRole);
//         }
//     }
// }
