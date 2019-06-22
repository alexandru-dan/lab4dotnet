using Lab1.Models;
using Lab1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Validators
{
    public interface IUserRoleValidator
    {
        ErrorsCollection Validate(UserToRolePostModel userUserRolePostModel, DataDbContext context);
    }


    public class UserRoleValidator : IUserRoleValidator
    {
        public ErrorsCollection Validate(UserToRolePostModel userUserRolePostModel, DataDbContext context)
        {
            ErrorsCollection errorsCollection = new ErrorsCollection { Entity = nameof(UserToRolePostModel) };

            List<string> userRoles = context.UserRoles
                .Select(userRole => userRole.Name)
                .ToList();

            if (!userRoles.Contains(userUserRolePostModel.UserRoleName))
            {
                errorsCollection.ErrorMessages.Add($"The userRole {userUserRolePostModel.UserRoleName} does not exists in Db!");
            }

            if (errorsCollection.ErrorMessages.Count > 0)
            {
                return errorsCollection;
            }
            return null;
        }
    }
}
