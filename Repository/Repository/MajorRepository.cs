﻿using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class MajorRepository : Repository<Major>, IMajorRepository
    {
        public MajorRepository(DbContext context) : base(context)
        {
        }
    }
}
