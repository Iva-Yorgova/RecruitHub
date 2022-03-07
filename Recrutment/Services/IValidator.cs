namespace Recrutment.Services
{
    using System.Collections.Generic;
    using Recrutment.ViewModels.Users;

    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserFormModel model);

    }
}
