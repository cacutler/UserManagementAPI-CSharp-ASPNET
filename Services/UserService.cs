using UserManagementApi.Models;
using UserManagementApi.Dtos;
namespace UserManagementApi.Services {
    public class UserService {
        private readonly List<User> _users = new() {
            new User {Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Password = "password123"},
            new User {Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Password = "password456"}
        };
        public IEnumerable<User> GetAllUsers() => _users.ToList();
        public User? GetUserById(int id) {
            return _users.FirstOrDefault(u => u.Id == id);
        }
        public User AddUser(CreateUserDto dto) {
            if (_users.Any(u => u.Email == dto.Email)) {
                throw new InvalidOperationException("Email already in use.");
            }
            var nextId = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            var user = new User {
                Id = nextId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password
            };
            _users.Add(user);
            return user;
        }
        public bool UpdateUser(int id, UpdateUserDto dto) {
            var existingUser = GetUserById(id);
            if (existingUser is null) {
                return false;
            }
            existingUser.FirstName = dto.FirstName;
            existingUser.LastName = dto.LastName;
            existingUser.Email = dto.Email;
            existingUser.Password = dto.Password;
            return true;
        }
        public bool DeleteUser(int id) {
            var user = GetUserById(id);
            if (user is null) {
                return false;
            }
            _users.Remove(user);
            return true;
        }
    }
}