﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Itm.Database.Core.Services;
using Itm.Database.Core.Services.ResponseTypes;
using Itm.Models;

namespace Itm.Database.Services
{
    internal interface IRoleService : IService
    {
        Task<IListResponse<RoleModel>> GetRolesAsync(int pageSize = 0, int pageNumber = 0);
        Task<ISingleResponse<RoleModel>> GetRoleByIDAsync(int roleID);
        Task<ISingleResponse<RoleModel>> UpdateRoleAsync(RoleModel updates);
        Task<ISingleResponse<RoleModel>> AddRoleAsync(RoleModel details);
        Task<ISingleResponse<RoleModel>> AddRoleResourcesAsync(RoleModel role, ICollection<ResourceModel> resources);
        Task<ISingleResponse<RoleModel>> AddRoleResourceAsync(int roleID, int resourceID);
        Task<ISingleResponse<RoleModel>> RemoveRoleAsync(int roleID);
    }
}
