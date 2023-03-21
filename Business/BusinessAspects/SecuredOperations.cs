using Business.Contans;
using Castle.DynamicProxy;
using Core.Extensions;
using Core.Utilities.Interceptions;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessAspects
{
    public class SecuredOperations : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;

        public SecuredOperations(string roles)
        {
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }

        protected override void OnBefore(IInvocation invocation)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            if (token != "")
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var decodeToken = jwtSecurityToken.Claims;
                foreach (var claim in decodeToken)
                {
                    foreach (var role in _roles)
                    {
                        if (claim.ToString().Contains(role))
                        {
                            return;
                        }
                    }
                }
            }
            throw new Exception("İşlem için yetkiniz bulunmuyor");
        }
    }
}
