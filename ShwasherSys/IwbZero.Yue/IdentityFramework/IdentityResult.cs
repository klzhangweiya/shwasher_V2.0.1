using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace IwbZero.IdentityFramework
{
    public class IwbIdentityResult : IdentityResult
    {
        public IwbIdentityResult()
        {

        }

        public IwbIdentityResult(IEnumerable<string> errors)
            : base(errors)
        {

        }

        public IwbIdentityResult(params string[] errors)
            : base(errors)
        {

        }

        public new static IwbIdentityResult Failed(params string[] errors)
        {
            return new IwbIdentityResult(errors);
        }
    }
}
