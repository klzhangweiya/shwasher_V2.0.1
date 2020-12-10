﻿using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Logging;
using Abp.Text;
using Abp.UI;
using Microsoft.AspNet.Identity;

namespace IwbZero.IdentityFramework
{
    public static class IwbIdentityResultExtensions
    {
        private static readonly Dictionary<string, string> IdentityLocalizations
            = new Dictionary<string, string>
              {
                  {"User already in role.", "Identity.UserAlreadyInRole"},
                  {"User is not in role.", "Identity.UserNotInRole"},
                  {"Role {0} does not exist.", "Identity.RoleNotFound"},
                  {"Incorrect password.", "Identity.PasswordMismatch"},
                  {"User name {0} is invalid, can only contain letters or digits.", "Identity.InvalidUserName"},
                  {"Passwords must be at least {0} characters.", "Identity.PasswordTooShort"},
                  {"{0} cannot be null or empty.", "Identity.PropertyTooShort"},
                  {"Name {0} is already taken.", "Identity.DuplicateUserName"},
                  {"User already has a password set.", "Identity.UserAlreadyHasPassword"},
                  {"Passwords must have at least one non letter or digit character.", "Identity.PasswordRequireNonLetterOrDigit"},
                  {"UserId not found.", "Identity.UserIdNotFound"},
                  {"Invalid token.", "Identity.InvalidToken"},
                  {"Email '{0}' is invalid.", "Identity.InvalidEmail"},
                  {"User {0} does not exist.", "Identity.UserNameNotFound"},
                  {"Lockout is not enabled for this user.", "Identity.LockoutNotEnabled"},
                  {"Passwords must have at least one uppercase ('A'-'Z').", "Identity.PasswordRequireUpper"},
                  {"Passwords must have at least one digit ('0'-'9').", "Identity.PasswordRequireDigit"},
                  {"Passwords must have at least one lowercase ('a'-'z').", "Identity.PasswordRequireLower"},
                  {"Email '{0}' is already taken.", "Identity.DuplicateEmail"},
                  {"A user with that external login already exists.", "Identity.ExternalLoginExists"},
                  {"An unknown failure has occured.", "Identity.DefaultError"}
              };

        /// <summary>
        /// Checks errors of given <see cref="IdentityResult"/> and throws <see cref="UserFriendlyException"/> if it's not succeeded.
        /// </summary>
        /// <param name="identityResult">Identity result to check</param>
        /// <param name="severity">Exception severity</param>
        public static void CheckErrors(this IdentityResult identityResult,LogSeverity severity = LogSeverity.Warn)
        {
            if (identityResult.Succeeded)
            {
                return;
            }

            throw new UserFriendlyException(identityResult.Errors.JoinAsString(", "), severity);
        }

        /// <summary>
        /// Checks errors of given <see cref="IdentityResult"/> and throws <see cref="UserFriendlyException"/> if it's not succeeded.
        /// </summary>
        /// <param name="identityResult">Identity result to check</param>
        /// <param name="localizationManager">Localization manager to localize error messages</param>
        /// <param name="localizationSourceName"></param>
        /// <param name="severity">Exception severity</param>
        public static void CheckErrors(this IdentityResult identityResult, ILocalizationManager localizationManager, string localizationSourceName = null,LogSeverity severity = LogSeverity.Warn)
        {
            if (identityResult.Succeeded)
            {
                return;
            }
            
            throw new UserFriendlyException(identityResult.LocalizeErrors(localizationManager, localizationSourceName), severity);
        }

        public static string LocalizeErrors(this IdentityResult identityResult, ILocalizationManager localizationManager, string localizationSourceName = null)
        {

            if (identityResult.Succeeded)
            {
                throw new ArgumentException("identityResult.Succeeded should be false in order to localize errors.");
            }

            if (identityResult.Errors == null)
            {
                throw new ArgumentException("identityResult.Errors should not be null.");
            }

            if (identityResult is IwbIdentityResult)
            {
                return identityResult.Errors.JoinAsString(" ");
            }

            return identityResult.Errors.Select(err => LocalizeErrorMessage(err, localizationManager, localizationSourceName)).JoinAsString(" ");
        }

        private static string LocalizeErrorMessage(string identityErrorMessage, ILocalizationManager localizationManager, string localizationSourceName = null)
        {
            localizationSourceName=localizationSourceName?? IwbZeroConsts.IwbZeroLocalizationSourceName;
            var localizationSource = localizationManager.GetSource(localizationSourceName);

            foreach (var identityLocalization in IdentityLocalizations)
            {
                string[] values;
                if (FormattedStringValueExtracter.IsMatch(identityErrorMessage, identityLocalization.Key, out values))
                {
                    return localizationSource.GetString(identityLocalization.Value, values.Cast<object>().ToArray());
                }
            }

            return localizationSource.GetString("Identity.DefaultError");
        }
    }
}