using BeanScene.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeanScene.Migrations
{
    public partial class IdentityData : Migration
    {
        private MigrationBuilder? _migrationBuilder;

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Make sure the migration builder is available to other methods
            _migrationBuilder = migrationBuilder;

            //GUIDS
            // 
            // 
            // 
            // 
            // 
            // 

            string roleMemberId = "e711dec9-eebb-4a57-bf2f-fc2de00466b8";
            string roleStaffId = "58210fd5-c178-4e7c-a3a9-cfcbe3609679";
            string roleManagerId = "2803e2aa-eafa-49a3-b07e-8b04194277f8";

            // Add roles to the AspNetRoles table
            CreateRole(roleMemberId, "Member");
            CreateRole(roleStaffId, "Staff");
            CreateRole(roleManagerId, "Manager");

            // Add users, Assign user roles into the AspNetUsers, AspNetUserRoles table
            CreateUser("1461cbee-041d-4916-a53a-6164d337c7ae", "m@test.com", "m", "m@test.com", "0404 040 404", "Seed1", "Member", new string[] {roleMemberId});
            CreateUser("d69d5834-b69f-484b-b376-33d1445151b2", "s@test.com", "s", "s@test.com", "0404 040 405", "Seed2", "Staff", new string[] { roleStaffId });
            CreateUser("339ed212-a945-4637-8375-11a977cd8647", "mg@test.com", "mg", "mg@test.com", "0404 040 406", "Seed3", "Manager", new string[] { roleStaffId, roleManagerId });


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string roleMemberId = "e711dec9-eebb-4a57-bf2f-fc2de00466b8";
            string roleStaffId = "58210fd5-c178-4e7c-a3a9-cfcbe3609679";
            string roleManagerId = "2803e2aa-eafa-49a3-b07e-8b04194277f8";

            // Make sure the migration builder is available to other methods
            _migrationBuilder = migrationBuilder;

            // Delete roles to the AspNetRoles table
            DeleteRole(roleMemberId);
            DeleteRole(roleStaffId);
            DeleteRole(roleManagerId);

            // Delete users to the AspNetUsers table
            DeleteUser("1461cbee-041d-4916-a53a-6164d337c7ae");
            DeleteUser("d69d5834-b69f-484b-b376-33d1445151b2");
            DeleteUser("339ed212-a945-4637-8375-11a977cd8647");
        }


        /// <summary>
        /// Create an identity role
        /// </summary>

        private void CreateRole(string id, string name)
        {
            IdentityRole role = new IdentityRole();
            role.Id = id;
            role.Name = name;

            // Generate normalized name 
            role.NormalizedName = name.ToUpperInvariant();

            // Concurrency stamp is a random value that must change whenever a use is persisited to the store
            role.ConcurrencyStamp = Guid.NewGuid().ToString();

            // Build query
            string[] fields = { "Id", "Name", "NormalizedName", "ConcurrencyStamp" };
            object[] data = { role.Id, role.Name, role.NormalizedName, role.ConcurrencyStamp };

            // Insert record into the database
            _migrationBuilder.InsertData("AspNetRoles", fields, data);
        }

        private void DeleteRole(string id)
        {
            // Delete record into the database
            _migrationBuilder.DeleteData("AspNetRoles", "Id", id);
        }

        private void CreateUser(string id, string userName, string password, string email, string? phone, string firstName, string lastName, string[]? roleIds = null)
        {
            // New user object to hold all the data
            ApplicationUser user = new ApplicationUser();

            user.Id = id;
            user.UserName = userName;
            user.Email = email;
            user.PhoneNumber = phone;
            user.FirstName = firstName;
            user.LastName = lastName;

            // Generate nomailized name
            user.NormalizedUserName = userName.ToUpperInvariant();
            user.NormalizedEmail = email.ToUpperInvariant();

            // Concurrency stamp is a random value that must change whenever a user is persisted to the store
            user.ConcurrencyStamp = Guid.NewGuid().ToString();

            // Generate password hash from plainttext password
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

            // Security  stamp is a random value that must change whenever a user's credentials change (password changed, login removed)
            user.PasswordHash = passwordHasher.HashPassword(user, password);

            // Other data
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = false;
            user.TwoFactorEnabled = false;
            user.LockoutEnabled = true;
            user.AccessFailedCount = 0;

            
            // Build query
            string[] fields = { "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount", "FirstName", "LastName" };
            object[] data = { user.Id, user.UserName, user.NormalizedUserName, user.Email, user.NormalizedEmail, user.EmailConfirmed, user.PasswordHash, user.SecurityStamp, user.ConcurrencyStamp, user.PhoneNumber, user.PhoneNumberConfirmed, user.TwoFactorEnabled, user.LockoutEnd, user.LockoutEnabled, user.AccessFailedCount, user.FirstName, user.LastName };

            // Insert record into the database
            _migrationBuilder.InsertData("AspNetUsers", fields, data);

            // Assign role(s) to user
            if (roleIds != null)
            {
                foreach (string roleId in roleIds)
                {
                    AssignRoleToUser(user.Id, roleId);
                }
            }
        }

        private void DeleteUser(string id)
        {
            _migrationBuilder.DeleteData("AspNetUsers", "Id", id);
        }

        /// <summary>
        /// Assign a role to a user
        /// </summary>
        
        private void AssignRoleToUser(string userId, string roleId)
        {
            // Build query
            string[] fields = { "UserId", "RoleId"};
            object[] data = { userId, roleId };

            // Insert record into the database
            _migrationBuilder.InsertData("AspNetUserRoles", fields, data);
        }


    }
}
