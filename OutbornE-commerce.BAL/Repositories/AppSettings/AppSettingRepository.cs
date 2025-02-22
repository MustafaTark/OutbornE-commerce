﻿using OutbornE_commerce.BAL.Repositories.BaseRepositories;
using OutbornE_commerce.DAL.Data;
using OutbornE_commerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Repositories.AppSettings
{
    public class AppSettingRepository : BaseRepository<AppSetting> , IAppSettingRepository
    {
        public AppSettingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
