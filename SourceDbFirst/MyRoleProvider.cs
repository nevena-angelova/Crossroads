using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Profonika.SMS.SMSServiceReference;

public class MyRoleProvider : RoleProvider
{

    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
        throw new NotImplementedException();
    }

    public override string ApplicationName
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public override void CreateRole(string roleName)
    {
        throw new NotImplementedException();
    }

    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
        throw new NotImplementedException();
    }

    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
        throw new NotImplementedException();
    }

    public override string[] GetAllRoles()
    {
        List<string> roles = new List<string>();
        roles.Add("Users");
        roles.Add("Administrators");
        roles.Add("God Like");

        return roles.ToArray();
    }

    public override string[] GetRolesForUser(string username)
    {
        SMSServiceClient service = new SMSServiceClient();

        User usr = service.GetUserByUserName(username);

        List<string> roles = new List<string>();

        var Roles = usr.SecUsersGroups_User;
        foreach (SecUsersGroup role in Roles)
        {
            roles.Add(role.Group.Name);
        }

        return roles.ToArray();
    }

    public override bool IsUserInRole(string username, string roleName)
    {
        SMSServiceClient service = new SMSServiceClient();

        User usr = service.GetUserByUserName(username);

        string Role = "";
        var Roles = usr.SecUsersGroups_User;
        foreach (SecUsersGroup role in Roles)
        {
            Role += role.Group.Name + ";";
        }

        if (Role.Contains("God Like"))
            return true;

        if (Role.Contains("Administrators") && (roleName == "Users" || roleName == "Administrators"))
            return true;

        if (Role.Contains("Users") && roleName == "Users")
            return true;

        return false;
    }

    public override string[] GetUsersInRole(string roleName)
    {
        throw new NotImplementedException();
    }

    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
        throw new NotImplementedException();
    }

    public override bool RoleExists(string roleName)
    {
        throw new NotImplementedException();
    }
}