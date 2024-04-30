using EduNexBL.Base;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.Repository
{
    public class AttachmentRepo : Repository<AttachmentFile>
    {
        public EduNexContext _context { get; set; }
        public AttachmentRepo(EduNexContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

    }
}
