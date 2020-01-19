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

        public static User ToUpdateUser(this ChangeUserViewModel changeUser, User user)
        {
            user.FirstName = changeUser.FirstName;
            user.LastName = changeUser.LastName;
            return user;
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

        public static ChangeUserViewModel ToChangeUserViewModel(this User user)
        {
            ChangeUserViewModel model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
            }

            return model;
        }
    }
}
