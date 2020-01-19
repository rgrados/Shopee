namespace Shopee.Web.Mappers
{
    using Data.Entities;
    using Models;

    public static class UserMapper
    {
        public static User ToUser(this RegisterNewUserViewModel registerUser)
        {
            return new User
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Email = registerUser.Username,
                UserName = registerUser.Username
            };
        }

        public static LoginViewModel ToLoginViewModel(this RegisterNewUserViewModel registerUser)
        {
            return new LoginViewModel
            {
                Password = registerUser.Password,
                RememberMe = false,
                Username = registerUser.Username
            };
        }
    }
}
