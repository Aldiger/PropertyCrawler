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

        Task<Process> GetProcessById(int id);

        Task<ProcessVM> GetProcessVmById(int id);
    }


    public class ProcessRepository : Repository<Process>, IProcessRepository
    {
        AppContext _context;
        public ProcessRepository(AppContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ProcessVM> GetProcessVmById(int id)
        {
            return await _context.Processes.Include(x => x.ProcessPostalCodes).Where(a => a.Id == id).Select(z => new ProcessVM
            {
                PostalCode = z.ProcessPostalCodes.Select(a => a.PostalCode.Code).ToList(),
                DateAdded = z.DateAdded,
                DateModified = z.DateModified,
                PropertyType = z.PropertyType,
                Status = z.Status,
                Type = z.Type

            }).FirstOrDefaultAsync();
        }

        public async Task<Process> GetProcessById(int id)
        {
            return await _context.Processes.Include(x => x.ProcessPostalCodes).Where(a => a.Id == id).FirstOrDefaultAsync();
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
