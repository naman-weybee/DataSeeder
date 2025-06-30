using Bogus;
using Bogus.DataSets;
using DataSeeder.Entities;
using DataSeeder.Enum;
using System.Data;

namespace DataSeeder
{
    public static class Generators
    {
        public static Faker Faker { get; set; } = new();
        public static Random random { get; set; } = new();

        public static List<Category> GenerateCategories(int dataCount)
        {
            var categoryIds = new List<Guid>();
            var categories = new List<Category>();

            for (int i = 0; i < dataCount; i++)
            {
                var id = Guid.NewGuid();
                Guid? parentId = null;
                categoryIds.Add(id);

                if (i > 0 && Faker.Random.Bool(0.2f))
                {
                    var parentIndex = Faker.Random.Int(0, categories.Count - 2);
                    parentId = categoryIds[parentIndex];
                }

                categories.Add(new Category
                {
                    Id = id,
                    ParentCategoryId = parentId,
                    Name = Faker.Commerce.Categories(1)[0] + $"_{i}",
                    Description = Faker.Commerce.ProductDescription()
                });
            }

            return categories;
        }

        public static List<Product> GenerateProducts(int dataCount, List<Category> categories)
        {
            var items = new List<Product>();
            var activeCategories = categories?.Where(c => !c.IsDeleted)?.Select(c => c.Id)?.ToList()!;

            for (int i = 0; i < dataCount; i++)
            {
                items.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    CategoryId = activeCategories[random.Next(activeCategories.Count)],
                    Name = Faker.Commerce.ProductName(),
                    Description = Faker.Commerce.ProductDescription(),
                    Price = Faker.Random.Double(1000, 500000),
                    Currency = "USD",
                    Stock = Faker.Random.Int(0, 1000),
                    SKU = Faker.Random.AlphaNumeric(8).ToUpper(),
                    Brand = Faker.Company.CompanyName()
                });
            }

            return items;
        }

        public static List<Country> GenerateCountries(int dataCount)
        {
            var countries = new List<Country>();

            for (int i = 0; i < dataCount; i++)
            {
                countries.Add(new Country
                {
                    Id = Guid.NewGuid(),
                    Name = Faker.Address.Country(),
                });
            }

            return countries;
        }

        public static List<State> GenerateStates(int dataCount)
        {
            var states = new List<State>();

            for (int i = 0; i < dataCount; i++)
            {
                states.Add(new State
                {
                    Id = Guid.NewGuid(),
                    Name = Faker.Address.State(),
                });
            }

            return states;
        }

        public static List<City> GenerateCities(int dataCount)
        {
            var cities = new List<City>();

            for (int i = 0; i < dataCount; i++)
            {
                cities.Add(new City
                {
                    Id = Guid.NewGuid(),
                    Name = Faker.Address.City(),
                });
            }

            return cities;
        }

        public static List<Role> GenerateRoles(int dataCount)
        {
            var roles = new List<Role>();

            for (int i = 0; i < dataCount; i++)
            {
                roles.Add(new Role
                {
                    Id = Guid.NewGuid(),
                    Name = Faker.Name.JobTitle(),
                });
            }

            return roles;
        }

        public static List<Role> GenerateRoles()
        {
            return
            [
                new() { Id = Guid.NewGuid(), Name = "Admin" },
                new() { Id = Guid.NewGuid(), Name = "Moderator" },
                new() { Id = Guid.NewGuid(), Name = "Support" },
                new() { Id = Guid.NewGuid(), Name = "Viewer" },
                new() { Id = Guid.NewGuid(), Name = "Guest" }
            ];
        }

        public static List<RoleEntity> GenerateRoleEntities()
        {
            return
            [
                new() { Id = eRoleEntity.Unknown, Name = "Unknown" },
                new() { Id = eRoleEntity.Full, Name = "Full" },
                new() { Id = eRoleEntity.Country, Name = "Country" },
                new() { Id = eRoleEntity.State, Name = "State" },
                new() { Id = eRoleEntity.City, Name = "City" },
                new() { Id = eRoleEntity.Role, Name = "Role" },
                new() { Id = eRoleEntity.RolePermission, Name = "RolePermission" },
                new() { Id = eRoleEntity.RoleEntity, Name = "RoleEntity" },
                new() { Id = eRoleEntity.Gender, Name = "Gender" },
                new() { Id = eRoleEntity.Address, Name = "Addresses" },
                new() { Id = eRoleEntity.Category, Name = "Category" },
                new() { Id = eRoleEntity.Product, Name = "Product" },
                new() { Id = eRoleEntity.User, Name = "User" },
                new() { Id = eRoleEntity.CartItem, Name = "CartItem" },
                new() { Id = eRoleEntity.Order, Name = "Order" },
                new() { Id = eRoleEntity.OrderStatus, Name = "OrderStatus" },
                new() { Id = eRoleEntity.OrderItem, Name = "OrderItem" },
                new() { Id = eRoleEntity.RefreshToken, Name = "RefreshToken" },
                new() { Id = eRoleEntity.OTP, Name = "OTP" }
            ];
        }

        public static List<RolePermission> GenerateRolePermissions(int dataCount, List<Role> roles)
        {
            var items = new List<RolePermission>();

            var adminRole = roles.FirstOrDefault(r => r.Name == "Admin" && !r.IsDeleted);
            if (adminRole != null)
            {
                items.Add(new RolePermission
                {
                    RoleId = adminRole.Id,
                    RoleEntityId = eRoleEntity.Full,
                    HasViewPermission = true,
                    HasCreateOrUpdatePermission = true,
                    HasDeletePermission = true,
                    HasFullPermission = true
                });
            }

            var roleIds = roles?.Where(c => !c.IsDeleted && c.Id != adminRole!.Id)?.Select(c => c.Id)?.ToList()!;

            for (int i = 0; i < dataCount; i++)
            {
                items.Add(new RolePermission
                {
                    RoleId = roleIds[random.Next(roleIds.Count)],
                    RoleEntityId = Faker.PickRandom<eRoleEntity>(),
                    HasViewPermission = Faker.Random.Bool(0.8f),
                    HasCreateOrUpdatePermission = Faker.Random.Bool(0.5f),
                    HasDeletePermission = Faker.Random.Bool(0.3f),
                    HasFullPermission = false
                });

                var last = items[^1];
                last.HasFullPermission = last.HasViewPermission && last.HasCreateOrUpdatePermission && last.HasDeletePermission;
            }

            return items;
        }

        public static List<Gender> GenerateGenders()
        {
            return
            [
                new() { Id = Guid.NewGuid(), Name = "Male" },
                new() { Id = Guid.NewGuid(), Name = "Female" },
                new() { Id = Guid.NewGuid(), Name = "Other" }
            ];
        }

        public static List<User> GenerateUsers(int dataCount, List<Role> roles, List<Gender> genders)
        {
            var users = new List<User>();
            var roleIds = roles?.Where(c => !c.IsDeleted)?.Select(c => c.Id)?.ToList()!;
            var genderIds = genders?.Select(c => c.Id)?.ToList()!;

            for (int i = 0; i < dataCount; i++)
            {
                var isEmailVerified = Faker.Random.Bool(0.9f);
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = Faker.Name.FirstName(),
                    LastName = Faker.Name.LastName(),
                    Password = Faker.Internet.Password(8, true),
                    Email = Faker.Internet.Email(),
                    PhoneNumber = Faker.Phone.PhoneNumber(),
                    RoleId = roleIds[random.Next(roleIds.Count)],
                    DateOfBirth = Faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                    GenderId = genderIds[random.Next(genderIds.Count)],
                    IsActive = Faker.Random.Bool(0.9f),
                    IsEmailVerified = isEmailVerified,
                    EmailVerificationToken = isEmailVerified ? null : Faker.Random.AlphaNumeric(20),
                    IsPhoneNumberVerified = Faker.Random.Bool(0.8f),
                    IsSubscribedToNotifications = Faker.Random.Bool(0.5f)
                });
            }

            return users;
        }

        public static List<Addresses> GenerateAddresses(int AddressesCount, List<User> users, List<Country> countries, List<Country> states, List<Country> cities)
        {
            var Addresses = new List<Addresses>();
            var activeUsers = users?.Where(c => !c.IsDeleted)?.Select(c => c.Id)?.ToList()!;
            var activeCountries = countries?.Where(c => !c.IsDeleted)?.Select(c => c.Id)?.ToList()!;
            var activeStates = states?.Where(c => !c.IsDeleted)?.Select(c => c.Id)?.ToList()!;
            var activeCities = cities?.Where(c => !c.IsDeleted)?.Select(c => c.Id)?.ToList()!;

            for (int i = 0; i < AddressesCount; i++)
            {
                Addresses.Add(new Addresses()
                {
                    Id = Guid.NewGuid(),
                    FirstName = Faker.Name.FirstName(),
                    LastName = Faker.Name.LastName(),
                    UserId = activeUsers[random.Next(activeUsers.Count)],
                    CountryId = activeCountries[random.Next(activeCountries.Count)],
                    StateId = activeStates[random.Next(activeStates.Count)],
                    CityId = activeCities[random.Next(activeCities.Count)],
                    PostalCode = Faker.Address.ZipCode(),
                    AdderessType = Faker.PickRandom<eAddressType>(),
                    AddressLine = Faker.Address.StreetAddress(),
                    PhoneNumber = Faker.Phone.PhoneNumber()
                });
            }

            return Addresses;
        }
    }
}