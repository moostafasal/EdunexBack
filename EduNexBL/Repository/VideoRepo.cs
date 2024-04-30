using EduNexBL.Base;
using EduNexBL.IRepository;
using EduNexDB.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.Repository
{
    public class VideoRepo : Repository<EduNexDB.Entites.Video>, IVideo
    {

        public EduNexContext _context { get; set; }
        public VideoRepo(EduNexContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

    }
}
