using Microsoft.EntityFrameworkCore;
using PropertyCrawler.Data.Entity;
using PropertyCrawler.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCrawler.Data.Repositories
{
    public interface IProcessRepository : IRepository<Process>
    {
        ProcessModels GeneralProcessInfo();

        Task<List<ProcessVM>> GetProcessesByStatus(string param);
    }


    public class ProcessRepository : Repository<Process>, IProcessRepository
    {
        AppContext _context;
        public ProcessRepository(AppContext context) : base(context)
        {
            _context = context;
        }


        public ProcessModels GeneralProcessInfo()
        {
            return new ProcessModels()
            {
                ProcessCount = _context.Processes.Count(),
                ProcessFailedCount = _context.Processes.Where(x => x.Status == ProcessStatus.Failed).Count(),
                ProcessSuccessfullCount = _context.Processes.Where(x => x.Status == ProcessStatus.Success).Count()
            };

        }


        public async Task<List<ProcessVM>> GetProcessesByStatus(string param)
        {
            if (param == "success")
            {
                return await _context.Processes.Where(x => x.Status == ProcessStatus.Success).Select(a => new ProcessVM
                {
                    Id = a.Id,
                    Active = a.Active,
                    DateAdded = a.DateAdded,
                    DateModified = a.DateModified,
                    Status = a.Status,
                    Type = a.Type,
                    PostalCode = a.ProcessPostalCodes.Select(x => x.PostalCode.Code).ToList()
                }).ToListAsync();
            }
            else if (param == "failed")
            {
                return await _context.Processes.Where(x => x.Status == ProcessStatus.Failed).Select(a => new ProcessVM
                {
                    Id = a.Id,
                    Active = a.Active,
                    DateAdded = a.DateAdded,
                    DateModified = a.DateModified,
                    Status = a.Status,
                    Type = a.Type,
                    PostalCode = a.ProcessPostalCodes.Select(x => x.PostalCode.Code).ToList()

                }).ToListAsync();
            }
            else
                return await _context.Processes.Select(a => new ProcessVM
                {
                    Id = a.Id,
                    Active = a.Active,
                    DateAdded = a.DateAdded,
                    DateModified = a.DateModified,
                    Status = a.Status,
                    Type = a.Type,
                    PostalCode = a.ProcessPostalCodes.Select(x => x.PostalCode.Code).ToList()
                }).ToListAsync();
        }


    }
}
